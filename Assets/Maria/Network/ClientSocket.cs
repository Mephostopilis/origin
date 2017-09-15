using UnityEngine;
using System.Collections.Generic;
using System;
using System.Text;
using Sproto;
using System.Net.Sockets;
using Maria.Encrypt;

namespace Maria.Network {

    [XLua.LuaCallCSharp]
    public class ClientSocket : DisposeObject {

        public delegate void AuthedCb(int ok);
        public delegate void ConnectedCb(bool connected);
        public delegate void DisconnectedCb();

        public delegate void RspCb(uint session, SprotoTypeBase responseObj, object ud);
        public delegate SprotoTypeBase ReqCb(uint session, SprotoTypeBase requestObj);

        private class ReqPg {
            public long Session { get; set; }
            public string Protocol { get; set; }
            public byte[] Buffer { get; set; }
            public int Version { get; set; }  // nonsence
            public int Index { get; set; }   // 
        }

        private class RspPg {
            public long Session { get; set; }
            public int Tag { get; set; }
            public int Version { get; set; }
            public int Index { get; set; }
            public object Ud { get; set; }
        }

        private Context _ctx = null;
        private bool _tcpflag = false;
        private PackageSocket _tcp = null;
        private User _user = new User();

        private string _ip = String.Empty;
        private int _port = 0;
        private int _step = 0;
        private bool _handshake = false;

        private int _index = 0;
        private int _version = 0;
        private long _session = 0;
        private const long _sessionmax = (1 << 16 - 1);
        private SprotoRpc _host = null;
        private SprotoRpc.RpcRequest _sendRequest = null;

        private Dictionary<int, ReqCb> _req = new Dictionary<int, ReqCb>();
        private Dictionary<int, RspCb> _rsp = new Dictionary<int, RspCb>();

        private Dictionary<string, ReqPg> _reqPg = new Dictionary<string, ReqPg>();
        private Dictionary<string, RspPg> _rspPg = new Dictionary<string, RspPg>();
        private Lua.ClientSock _clientSockScript = null;
        private bool _clientSockScriptEnable = false;

        // udp
        private PackageSocketUdp _udp = null;
        private long _udpsession = 0;
        private string _udpip = null;
        private int _udpport = 0;
        private bool _udpflag = false;

        public ClientSocket(Context ctx, ProtocolBase s2c, ProtocolBase c2s) {
            _ctx = ctx;
            _host = new SprotoRpc(s2c);
            _sendRequest = _host.Attach(c2s);
        }

        protected override void Dispose(bool disposing) {
            if (_disposed) {
                return;
            }
            if (disposing) {
                // 清理托管资源，调用自己管理的对象的Dispose方法
                if (_udp != null) {
                    _udp.Dispose();
                }
            }
            // 清理非托管资源

            _disposed = true;
        }

        public AuthedCb OnAuthed { get; set; }
        public ConnectedCb OnConnected { get; set; }
        public DisconnectedCb OnDisconnected { get; set; }
        public PackageSocketUdp.RecvCB OnRecvUdp { get; set; }
        public PackageSocketUdp.SyncCB OnSyncUdp { get; set; }
        public Lua.ClientSock ClintSockscript {
            get { return _clientSockScript; }
            set {
                _clientSockScript = value;
                _clientSockScriptEnable = _clientSockScript.enable();
            }
        }

        // Update is called once per frame
        public void Update() {
            if (_tcp != null) {
                _tcp.Update();
            }
            if (_udp != null) {
                _udp.Update();
            }
        }

        public void RegisterResponse(int tag, RspCb cb) {
            if (_rsp.ContainsKey(tag)) {
                throw new Exception(string.Format("tag {0} has register", tag));
            } else {
                _rsp[tag] = cb;
            }
        }

        public void RegisterRequest(int tag, ReqCb cb) {
            if (_req.ContainsKey(tag)) {
                throw new Exception(string.Format("tag {0} has register", tag));
            } else {
                _req[tag] = cb;
            }
        }

        public long genSession() {
            ++_session;
            _session = (_session >= _sessionmax) ? 1 : _session;
            return _session;
        }

        public void SendReq<T>(int tag, SprotoTypeBase obj, long session, object ud) {
            //UnityEngine.Debug.Assert(_tcpflag == true);
            if (_tcpflag) {

                if (session == 0) {
                    byte[] d = _sendRequest.Invoke<T>(obj, null);
                    UnityEngine.Debug.Assert(d != null);
                    _tcp.Send(d, 0, d.Length);
                } else {
                    byte[] d = _sendRequest.Invoke<T>(obj, session);
                    UnityEngine.Debug.Assert(d != null);

                    RspPg pg = new RspPg();
                    pg.Tag = tag;
                    pg.Session = session;
                    pg.Index = _index;
                    pg.Version = _version;
                    pg.Ud = ud;

                    string key = idToHex(session);
                    _rspPg[key] = pg;
                    _tcp.Send(d, 0, d.Length);
                }
            }
        }

        public void SendReq<T>(int tag, SprotoTypeBase obj, long session) {
            SendReq<T>(tag, obj, session, null);
        }

        public void SendReq<T>(int tag, SprotoTypeBase obj, object ud) {
            SendReq<T>(tag, obj, genSession(), ud);
        }

        public void SendReq<T>(int tag, SprotoTypeBase obj) {
            SendReq<T>(tag, obj, genSession(), null);
        }

        public void Send(string pack) {
            if (_tcpflag) {
                byte[] d = ASCIIEncoding.ASCII.GetBytes(pack);
                _tcp.Send(d, 0, d.Length);
            }
        }

        public void Auth(string ipstr, int pt, User u) {
            _step = 0;
            _index++;   // index increment.
            _ip = ipstr;
            _port = pt;
            _user = u;
            _handshake = true;

            // TODO:
            // 这里可能需要修改下
            _tcp = new PackageSocket();
            _tcp.OnConnect = OnConnect;
            _tcp.OnRecvive = OnRecvive;
            _tcp.OnDisconnect = OnDisconnect;
            _tcp.SetEnabledPing(false);
            _tcp.SetPackageSocketType(PackageSocketType.Header);
            _tcp.Connect(_ip, _port);
        }

        public void Reset() {
            _user = null;
            _step = 0;
            _handshake = false;
        }

        private byte[] WriteToken() {
            string u = Encoding.ASCII.GetString(Crypt.base64encode(Encoding.ASCII.GetBytes(string.Format("{0}", _user.Uid))));
            string s = Encoding.ASCII.GetString(Crypt.base64encode(Encoding.ASCII.GetBytes(_user.Server)));
            string sid = Encoding.ASCII.GetString(Crypt.base64encode(Encoding.ASCII.GetBytes(string.Format("{0}", _user.Subid))));
            string token = string.Format("{0}@{1}#{2}:{3}", u, s, sid, _index);
            UnityEngine.Debug.Log(token);
            return Encoding.ASCII.GetBytes(token);
        }

        private void DoAuth() {
            byte[] token = WriteToken();
            byte[] hmac = Crypt.base64encode(Crypt.hmac64(Crypt.hashkey(token), _user.Secret));
            byte[] tmp = new byte[token.Length + hmac.Length + 1];
            Array.Copy(token, 0, tmp, 0, token.Length);
            tmp[token.Length] = Encoding.ASCII.GetBytes(":")[0];
            Array.Copy(hmac, 0, tmp, token.Length + 1, hmac.Length);
            _tcp.Send(tmp, 0, tmp.Length);
            _step++;
        }

        private void OnConnect(bool connected) {
            if (connected) {
                if (_handshake)
                    DoAuth();
            } else {
                OnConnected(connected);
            }
        }

        private void OnRecvive(byte[] data, int start, int length) {
            if (length <= 0)
                return;
            if (_handshake) {
                byte[] buffer = new byte[length];
                Array.Copy(data, start, buffer, 0, length);
                if (_step == 1) {
                    string str = Encoding.ASCII.GetString(buffer);
                    int code = Int32.Parse(str.Substring(0, 3));
                    string msg = str.Substring(4);
                    if (code == 200) {
                        _tcpflag = true;
                        _step = 0;
                        _handshake = false;
                        //_tcp.SetEnabledPing(true);
                        //_tcp.SendPing();
                        UnityEngine.Debug.Log(string.Format("{0},{1}", code, msg));
                        if (OnAuthed != null) {
                            OnAuthed(code);
                        }
                    } else if (code == 403) {
                        _step = 0;
                        _handshake = false;
                        if (OnAuthed != null) {
                            OnAuthed(code);
                        }
                    } else {
                        UnityEngine.Debug.Assert(false);
                        _step = 0;
                        DoAuth();
                        UnityEngine.Debug.LogError(string.Format("error code : {0}, {1}", code, msg));
                        if (OnAuthed != null) {
                            OnAuthed(code);
                        }
                    }
                }
            } else {
                byte[] buffer = new byte[length];
                Array.Copy(data, start, buffer, 0, length);
                if (_clientSockScriptEnable) {
                    if (_clientSockScript.recv(Encoding.ASCII.GetString(buffer))) {
                        return;
                    }
                }
                SprotoRpc.RpcInfo sinfo = _host.Dispatch(buffer);
                if (sinfo.type == SprotoRpc.RpcType.REQUEST) {
                    int tag = (int)sinfo.tag;
                    try {
                        var cb = _req[tag];
                        if (sinfo.session != null && sinfo.session > 0) {
                            SprotoTypeBase rsp = cb((uint)sinfo.session, sinfo.requestObj);
                            byte[] d = sinfo.Response(rsp);
                            _tcp.Send(d, 0, d.Length);
                        } else {
                            cb(0, sinfo.requestObj);
                        }
                    } catch (Exception ex) {
                        UnityEngine.Debug.LogException(ex);
                    }
                } else if (sinfo.type == SprotoRpc.RpcType.RESPONSE) {
                    UnityEngine.Debug.Assert(sinfo.session != null);
                    long session = (long)sinfo.session;
                    string key = idToHex(session);
                    try {
                        RspPg pg = _rspPg[key];
                        var cb = _rsp[pg.Tag];
                        cb((uint)session, sinfo.responseObj, pg.Ud);
                        _rspPg.Remove(key);
                    } catch (Exception ex) {
                        UnityEngine.Debug.LogException(ex);
                    }
                }
            }
        }

        private void OnDisconnect(SocketError socketError, PackageSocketError packageSocketError) {
            _tcpflag = false;
            _tcp = null;
            if (OnDisconnected != null) {
                OnDisconnected();
            }
        }

        private string idToHex(long id) {
            byte[] tmp = new byte[9];
            byte[] hex = new byte[16] { 48, 49, 50, 51, 52, 53, 54, 55, 56, 57, 65, 66, 67, 68, 69, 70 };
            tmp[0] = 58;
            for (int i = 0; i < 8; i++) {
                tmp[i + 1] = hex[(id >> ((7 - i) * 4)) & 0xf];
            }
            return Encoding.ASCII.GetString(tmp);
        }


        // UDP
        public void UdpAuth(long session, string ip, int port) {
            UnityEngine.Debug.Assert(_udpflag == false);
            UnityEngine.Debug.Assert(_udp == null);
            _udpsession = session;
            _udpip = ip;
            _udpport = port;

            TimeSync ts = _ctx.TiSync;
            _udp = new PackageSocketUdp(_ctx, _user.Secret, (uint)session);
            _udp.OnRecv = UdpRecv;
            _udp.OnSync = UdpSync;
            UnityEngine.Debug.Assert(_udp != null);
            _udp.Connect(ip, port);
            _udp.Sync();
        }

        private void UdpSync() {
            _udpflag = true;
            if (OnSyncUdp != null) {
                OnSyncUdp();
            }
        }

        private void UdpRecv(PackageSocketUdp.R r) {
            if (OnRecvUdp != null) {
                OnRecvUdp(r);
            }
        }

        public void SendUdp(byte[] data) {
            if (_udpflag) {
                _udp.Send(data);
            }
        }
    }
}