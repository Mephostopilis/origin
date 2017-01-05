using UnityEngine;
using System.Collections.Generic;
using System;
using System.Text;
using Sproto;
using System.Net.Sockets;
using Maria.Encrypt;

namespace Maria.Network {
    public class ClientSocket {

        public delegate void AuthedCb(int ok);
        public delegate void DisconnectedCb();
        
        public delegate void RspCb(uint session, SprotoTypeBase responseObj);
        public delegate SprotoTypeBase ReqCb(uint session, SprotoTypeBase requestObj);

        private class ReqPg {
            public uint Session { get; set; }
            public string Protocol { get; set; }
            public byte[] Buffer { get; set; }
            public int Version { get; set; }  // nonsence
            public int Index { get; set; }   // 
        }

        private class RspPg {
            public uint Session { get; set; }
            public int Tag { get; set; }
            public int Version { get; set; }
            public int Index { get; set; }
            public byte[] Buffer { get; set; }
        }

        private Context _ctx = null;
        private bool _tcpflag = false;
        private PackageSocket _tcp = null;
        private User _user = new User();

        private string       _ip = String.Empty;
        private int          _port = 0;
        private int          _step = 0;
        private bool         _handshake = false;
        
        private int _index = 0;
        private int _version = 0;
        private uint _session = 0;
        private SprotoRpc _host = null;
        private SprotoRpc.RpcRequest _sendRequest = null;

        private Dictionary<int, ReqCb> _req = new Dictionary<int, ReqCb>();
        private Dictionary<int, RspCb> _rsp = new Dictionary<int, RspCb>();

        private Dictionary<string, ReqPg> _reqPg = new Dictionary<string, ReqPg>();
        private Dictionary<string, RspPg> _rspPg = new Dictionary<string, RspPg>();

        // udp
        private PackageSocketUdp _udp = null;
        private long             _udpsession = 0;
        private string           _udpip = null;
        private int              _udpport = 0;
        private bool             _udpflag = false;
        private AuthedCb         _udpAuthed = null;

        public ClientSocket(Context ctx, ProtocolBase s2c, ProtocolBase c2s) {
            _ctx = ctx;
            _host = new SprotoRpc(s2c);
            _sendRequest = _host.Attach(c2s);
        }

        public AuthedCb OnAuthed { get; set; }
        public DisconnectedCb OnDisconnected { get; set; }
        public PackageSocketUdp.RecvCB OnRecvUdp { get; set; }
        public PackageSocketUdp.SyncCB OnSyncUdp { get; set; }

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
            _rsp[tag] = cb;
        }

        public void RegisterRequest(int tag, ReqCb cb) {
            _req[tag] = cb;
        }

        public void SendReq<T>(int tag, SprotoTypeBase obj) {
            Debug.Assert(_tcpflag == true);
            uint id = genSession();
            byte[] d = _sendRequest.Invoke<T>(obj, id);
            Debug.Assert(d != null);

            RspPg pg = new RspPg();
            pg.Session = id;
            pg.Buffer = d;
            pg.Index = _index;
            pg.Tag = tag;
            pg.Version = _version;
            string key = idToHex(id);
            _rspPg[key] = pg;

            _tcp.Send(d, 0, d.Length);
        }

        private byte[] WriteToken() {
            string u = Encoding.ASCII.GetString(Crypt.base64encode(Encoding.ASCII.GetBytes(string.Format("{0}", _user.Uid))));
            string s = Encoding.ASCII.GetString(Crypt.base64encode(Encoding.ASCII.GetBytes(_user.Server)));
            string sid = Encoding.ASCII.GetString(Crypt.base64encode(Encoding.ASCII.GetBytes(string.Format("{0}", _user.Subid))));
            string token = string.Format("{0}@{1}#{2}:{3}", u, s, sid, _index);
            Debug.Log(token);
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
                Auth(_ip, _port, _user);
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
                        Debug.Log(string.Format("{0},{1}", code, msg));
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
                        Debug.Assert(false);
                        _step = 0;
                        DoAuth();
                        Debug.LogError(string.Format("error code : {0}, {1}", code, msg));
                        if (OnAuthed != null) {
                            OnAuthed(code);
                        }
                    }
                }
            } else {
                byte[] buffer = new byte[length];
                Array.Copy(data, start, buffer, 0, length);
                SprotoRpc.RpcInfo sinfo = _host.Dispatch(buffer);
                if (sinfo.type == SprotoRpc.RpcType.REQUEST) {
                    int tag = (int)sinfo.tag;
                    try {
                        var cb = _req[tag];
                        SprotoTypeBase rsp = cb(0, sinfo.requestObj);
                        byte[] d = sinfo.Response(rsp);
                        _tcp.Send(d, 0, d.Length);
                    } catch (KeyNotFoundException ex) {
                        Debug.LogError(ex.Message);
                    }
                } else if (sinfo.type == SprotoRpc.RpcType.RESPONSE) {
                    Debug.Assert(sinfo.session != null);
                    uint session = (uint)sinfo.session;
                    string key = idToHex(session);
                    RspPg pg = _rspPg[key];
                    try {
                        var cb = _rsp[pg.Tag];
                        cb(session, sinfo.responseObj);
                    } catch (KeyNotFoundException ex) {
                        Debug.LogError(ex.Message);
                        //throw;
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

        private uint genSession() {
            ++_session;
            if (_session == 0)
                _session++;
            return _session;
        }

        private string idToHex(uint id) {
            byte[] tmp = new byte[9];
            byte[] hex = new byte[16] { 48, 49, 50, 51, 52, 53, 54, 55, 56, 57, 65, 66, 67, 68, 69, 70 };
            tmp[0] = 58;
            for (int i = 1; i < 8; i++) {
                tmp[i + 1] = hex[(id >> ((7 - i) * 4)) & 0xf];
            }
            return Encoding.ASCII.GetString(tmp);
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

        // UDP
        public void UdpAuth(long session, string ip, int port, AuthedCb authed) {
            Debug.Assert(_udpflag == false);
            Debug.Assert(_udp == null);
            _udpsession = session;
            _udpip = ip;
            _udpport = port;
            _udpAuthed = authed;

            TimeSync ts = _ctx.TiSync;
            _udp = new PackageSocketUdp(_user.Secret, (uint)session, ts);
            _udp.OnRecv = UdpRecv;
            _udp.OnSync = UdpSync;
            Debug.Assert(_udp != null);
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