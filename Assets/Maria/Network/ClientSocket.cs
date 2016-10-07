using UnityEngine;
using System.Collections.Generic;
using System;
using System.Text;
using Sproto;
using S2cSprotoType;
using System.Net.Sockets;
using Maria.Encrypt;

namespace Maria.Network
{
    public class ClientSocket
    {
        public delegate void CB(int ok);

        public delegate void RspCb(uint session, SprotoTypeBase responseObj);
        public delegate SprotoTypeBase ReqCb(uint session, SprotoTypeBase requestObj);

        private class ReqPg
        {
            public uint Session { get; set; }
            public string Protocol { get; set; }
            public byte[] Buffer { get; set; }
            public int Version { get; set; }  // nonsence
            public int Index { get; set; }   // 
        }

        private class RspPg
        {
            public uint Session { get; set; }
            public string Protocol { get; set; }
            public byte[] Buffer { get; set; }
            public int Version { get; set; }
            public int Index { get; set; }
        }

        private Context _ctx = null;
        private PackageSocket _tcp = null;
        private PackageSocketUdp _udp = null;
        private User _user = new User();

        private string _ip = String.Empty;
        private int _port = 0;
        private int _step = 0;
        private bool _handshake = false;
        private CB _callback = null;

        private int _index = 0;
        private int _version = 0;
        private uint _session = 0;
        private SprotoRpc _host = null;
        private SprotoRpc.RpcRequest _sendRequest = null;

        private const int c2s_req_tag = 1 << 0;
        private const int c2s_resp_tag = 1 << 1;
        private const int s2c_req_tag = 1 << 2;
        private const int s2c_resp_tag = 1 << 3;

        private Request _request = null;
        private Response _response = null;
        private Dictionary<string, ReqCb> _req = new Dictionary<string, ReqCb>();
        private Dictionary<string, RspCb> _rsp = new Dictionary<string, RspCb>();

        private Dictionary<string, ReqPg> _reqPg = new Dictionary<string, ReqPg>();
        private Dictionary<string, RspPg> _rspPg = new Dictionary<string, RspPg>();

        public ClientSocket(Context ctx)
        {
            _ctx = ctx;

            _tcp = new PackageSocket();
            _tcp.OnConnect = OnConnect;
            _tcp.OnRecvive = OnRecvive;
            _tcp.OnDisconnect = OnDisconnect;
            _tcp.SetEnabledPing(false);
            _tcp.SetPackageSocketType(PackageSocketType.Header);

            _host = new SprotoRpc(S2cProtocol.Instance);
            _sendRequest = _host.Attach(C2sProtocol.Instance);

            _request = new Request(_ctx);
            _response = new Response(_ctx);
            RegisterProtocol();
        }

        // Use this for initialization
        public void Start()
        {
        }

        // Update is called once per frame
        public void Update()
        {
            _tcp.Update();
            if (_udp != null)
            {
                _udp.Update();
            }

            if (true)
            {

            }

        }

        private void DoAuth()
        {
            byte[] token = WriteToken();
            byte[] hmac = Crypt.base64encode(Crypt.hmac64(Crypt.hashkey(token), _user.Secret));
            byte[] tmp = new byte[token.Length + hmac.Length + 1];
            Array.Copy(token, 0, tmp, 0, token.Length);
            tmp[token.Length] = Encoding.ASCII.GetBytes(":")[0];
            Array.Copy(hmac, 0, tmp, token.Length + 1, hmac.Length);
            _tcp.Send(tmp, 0, tmp.Length);
            _step++;
        }

        public void OnConnect(bool connected)
        {
            if (connected)
            {
                if (_handshake)
                    DoAuth();
            }
            else
            {
                Auth(_ip, _port, _user, null);
            }
        }

        void OnRecvive(byte[] data, int start, int length)
        {
            if (length <= 0)
                return;
            if (_handshake)
            {
                byte[] buffer = new byte[length];
                Array.Copy(data, start, buffer, 0, length);
                if (_step == 1)
                {
                    string str = Encoding.ASCII.GetString(buffer);
                    int code = Int32.Parse(str.Substring(0, 3));
                    string msg = str.Substring(4);
                    if (code == 200)
                    {
                        _step = 0;
                        _handshake = false;
                        _tcp.SetEnabledPing(true);
                        Debug.Log(string.Format("{0},{1}", code, msg));
                        if (_callback != null)
                        {
                            _callback(code);
                        }

                        //_ctx.AuthUdp();
                    }
                    else if (code == 403)
                    {
                        _step = 0;
                        _handshake = false;
                        if (_callback != null)
                        {
                            _callback(code);
                        }
                    }
                    else
                    {
                        Debug.Assert(false);
                        _step = 0;
                        DoAuth();
                        Debug.LogError(string.Format("error code : {0}, {1}", code, msg));
                        if (_callback != null)
                        {
                            _callback(code);
                        }
                    }
                }
            }
            else
            {
                if (false)
                {
                    byte tag = data[start + length - 1];
                    uint session = 0;
                    for (int i = 0; i < 4; i++)
                    {
                        session |= (uint)(data[start + length - (5 - i)] & 0xff) << (3 - i) * 8;
                    }

                    byte[] buffer = new byte[length - 5];
                    Array.Copy(data, start, buffer, 0, length - 5);
                    if (tag == c2s_resp_tag)
                    {
                        SprotoRpc.RpcInfo sinfo = _host.Dispatch(buffer);
                        Debug.Assert(sinfo.type == SprotoRpc.RpcType.RESPONSE);
                        Debug.Assert(sinfo.session != null);
                        Debug.Assert(sinfo.session == session);
                        string key = idToHex(session);
                        RspPg pg = _rspPg[key];
                        var cb = _rsp[pg.Protocol];
                        cb(session, sinfo.responseObj);
                    }
                    else if (tag == s2c_req_tag)
                    {
                        SprotoRpc.RpcInfo sinfo = _host.Dispatch(buffer);
                        Debug.Assert(sinfo.type == SprotoRpc.RpcType.REQUEST);
                        Debug.Assert(sinfo.session != null);
                        Debug.Assert(sinfo.session == session);
                        Debug.Assert(sinfo.tag != null);

                        // 新建一个请求包
                        var cb = _req[sinfo.ToString()];
                        SprotoTypeBase rsp = cb(session, sinfo.requestObj);
                        byte[] d = sinfo.Response(rsp);
                        Write(d, session, s2c_resp_tag);
                    }
                }
                else
                {
                    byte[] buffer = new byte[length];
                    Array.Copy(data, start, buffer, 0, length);
                    SprotoRpc.RpcInfo sinfo = _host.Dispatch(buffer);
                    if (sinfo.type == SprotoRpc.RpcType.REQUEST)
                    {
                        uint session = (uint)sinfo.session;
                        var cb = _req[sinfo.ToString()];
                        SprotoTypeBase rsp = cb(session, sinfo.requestObj);
                        byte[] d = sinfo.Response(rsp);
                        Write(d, session, s2c_resp_tag);
                    }
                    else if (sinfo.type == SprotoRpc.RpcType.RESPONSE)
                    {
                        Debug.Assert(sinfo.type == SprotoRpc.RpcType.RESPONSE);
                        Debug.Assert(sinfo.session != null);
                        //Debug.Assert(sinfo.session == session);
                        uint session = (uint)sinfo.session;
                        string key = idToHex((uint)sinfo.session);
                        RspPg pg = _rspPg[key];
                        var cb = _rsp[pg.Protocol];
                        cb(session, sinfo.responseObj);
                    }
                }
            }
        }

        void OnDisconnect(SocketError socketError, PackageSocketError packageSocketError)
        {
            _tcp = new PackageSocket();
            _tcp.OnConnect = OnConnect;
            _tcp.OnRecvive = OnRecvive;
            _tcp.OnDisconnect = OnDisconnect;
            _tcp.SetEnabledPing(false);
            _tcp.SetPackageSocketType(PackageSocketType.Header);

            var ctr = _ctx.GetCurController();
            ctr.OnDisconnect();
            //Auth(_ip, _port, _user, null);
        }

        private byte[] WriteToken()
        {
            string u = Encoding.ASCII.GetString(Crypt.base64encode(_user.Uid));
            string s = Encoding.ASCII.GetString(Crypt.base64encode(Encoding.ASCII.GetBytes(_user.Server)));
            string sid = Encoding.ASCII.GetString(Crypt.base64encode(_user.Subid));
            string token = string.Format("{0}@{1}#{2}:{3}", u, s, sid, _index);
            Debug.Log(token);
            return Encoding.ASCII.GetBytes(token);
        }

        private uint B2L(byte[] buffer, int start, int length)
        {
            uint r = 0;
            for (int i = 0; i < length; i++)
            {
                int idx = start + i;
                int b = buffer[idx];
            }
            return r;
        }

        private void Write(byte[] buffer, uint session, byte tag)
        {
            int l = buffer.Length + 5;
            byte[] tmp = new byte[l];
            Array.Copy(buffer, tmp, buffer.Length);
            byte[] s = new byte[4];
            for (int i = 0; i < 4; i++)
            {
                s[i] = (byte)(session >> (8 * (3 - i)) & 0xff);
            }
            Array.Copy(s, 0, tmp, buffer.Length, 4);
            tmp[l - 1] = tag;
            _tcp.Send(tmp, 0, l);
        }

        public void SendReq<T>(string name, SprotoTypeBase obj)
        {
            uint id = genSession();
            byte[] d = _sendRequest.Invoke<T>(obj, id);
            Debug.Assert(d != null);

            RspPg pg = new RspPg();
            pg.Session = id;
            pg.Buffer = d;
            pg.Index = _index;
            pg.Protocol = name;
            pg.Version = _version;
            string key = idToHex(id);
            _rspPg[key] = pg;

            _tcp.Send(d, 0, d.Length);
            //Write(d, id, c2s_req_tag);
        }

        private uint genSession()
        {
            ++_session;
            if (_session == 0)
                _session++;
            return _session;
        }

        private string idToHex(uint id)
        {
            byte[] tmp = new byte[9];
            byte[] hex = new byte[16] { 48, 49, 50, 51, 52, 53, 54, 55, 56, 57, 65, 66, 67, 68, 69, 70 };
            tmp[0] = 58;
            for (int i = 1; i < 8; i++)
            {
                tmp[i + 1] = hex[(id >> ((7 - i) * 4)) & 0xf];
            }
            return Encoding.ASCII.GetString(tmp);
        }

        public void Auth(string ipstr, int pt, User u, CB cb)
        {
            _step = 0;
            _index++;   // index increment.
            _ip = ipstr;
            _port = pt;
            _user = u;
            _callback = cb;
            _handshake = true;
            _tcp.Connect(_ip, _port);
        }

        public void Reset()
        {
            _user = null;
            _step = 0;
            _callback = null;
            _handshake = false;
        }

        private void RegisterProtocol()
        {
            RegisterResponse();
            RegisterRequest();
        }

        private void RegisterResponse()
        {
            //_rsp["role_info"] = _response.role_info;
            //_rsp["join"] = _response.join;
            //_rsp["handshake"] = _response.handshake;
        }

        private void RegisterRequest()
        {
            //_req["role_info"] = _request.role_info;
        }

        public void AuthUdp()
        {
            C2sSprotoType.join.request requestObj = new C2sSprotoType.join.request();
            requestObj.room = 1;
            SendReq<C2sProtocol.join>("join", requestObj);
        }

        public void ConnectUdp(long session, string ip, int port)
        {
            if (_udp == null)
            {
                TimeSync ts = null;
                _udp = new PackageSocketUdp(_user.Secret, session, ts);
                _udp.OnRecviveUdp = OnRecviveUdp;
                Debug.Assert(_udp != null);
                _udp.Connect(ip, port);
                _udp.Sync();
                _ctx.AuthUdpFlag = true;
            }
        }

        void OnRecviveUdp(PackageSocketUdp.R r)
        {
            Debug.Log(r.Eventtime);
            Debug.Log(r.Session);
            string str = Encoding.ASCII.GetString(r.Data);
            Debug.Log(str);
        }

        public void SendUdp(byte[] data)
        {
            _udp.Send(data);
        }
    }
}