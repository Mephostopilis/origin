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
        public delegate void CB(bool ok, object ud, byte[] subid, byte[] secret);

        public delegate void RespCb(uint session, SprotoTypeBase responseObj, object ud);
        public delegate void ReqCb(uint session, SprotoTypeBase requestObj, SprotoRpc.ResponseFunction Response, object ud);

        private class ReqAction
        {
            public object Ud { get; set; }
            public ReqCb Action { get; set; }
        }

        private class RespAction
        {
            public object Ud { get; set; }
            public RespCb Action { get; set; }
        }

        private class ReqPg
        {
            public uint Session { get; set; }
            public string Protocol { get; set; }
            public byte[] Buffer { get; set; }
            public int Version { get; set; }  // nonsence
            public uint Index { get; set; }   // 
        }

        private class RespPg
        {
            public uint Session { get; set; }
            public string Protocol { get; set; }
            public byte[] Buffer { get; set; }
            public int Version { get; set; }
            public uint Index { get; set; }
        }

        private Context _ctx;
        private PackageSocket sock = new PackageSocket();
        private string ip = String.Empty;
        private int port = 0;
        private User user = null;
        private int step = 0;
        private bool handshake = false;
        private object ud = null;
        private CB callback = null;

        private uint index = 0;
        private uint session = 0;
        private SprotoRpc host = null;
        private SprotoRpc.RpcRequest send_request = null;


        private const int c2s_req_tag = 1 << 0;
        private const int c2s_resp_tag = 1 << 1;
        private const int s2c_req_tag = 1 << 2;
        private const int s2c_resp_tag = 1 << 3;

        private Dictionary<string, RespAction> response = new Dictionary<string, RespAction>();
        private Dictionary<string, ReqAction> request = new Dictionary<string, ReqAction>();
        private Dictionary<string, ReqPg> request_pg = new Dictionary<string, ReqPg>();      /*protocal -> pg */
        private Dictionary<string, RespPg> response_pg = new Dictionary<string, RespPg>();   /*protocal -> pg */

        public ClientSocket(Context ctx)
        {
            _ctx = ctx;

            host = new SprotoRpc(S2cProtocol.Instance);
            send_request = host.Attach(C2sProtocol.Instance);
            sock.OnConnect = OnConnect;
            sock.OnRecvive = OnRecvive;
            sock.OnDisconnect = OnDisconnect;
            sock.SetEnabledPing(true);
            sock.SetPackageSocketType(PackageSocketType.Header);
            RegisterProtocol();
        }

        // Use this for initialization
        public void Start()
        {
            
        }

        // Update is called once per frame
        public void Update()
        {
            sock.Update();
        }

        private void DoAuth()
        {
            byte[] token = WriteToken();
            byte[] hmac = Crypt.base64encode(Crypt.hmac64(Crypt.hashkey(token), user.Secret));
            byte[] tmp = new byte[token.Length + hmac.Length + 1];
            Array.Copy(token, 0, tmp, 0, token.Length);
            tmp[token.Length] = Encoding.ASCII.GetBytes(":")[0];
            Array.Copy(hmac, 0, tmp, token.Length + 1, hmac.Length);
            sock.Send(tmp, 0, tmp.Length);
            step++;
        }

        public void OnConnect(bool connected)
        {
            if (connected)
            {
                if (handshake)
                    DoAuth();
            }
            else
            {
                Auth(ip, port, user, ud, null);
            }
        }

        void OnRecvive(byte[] data, int start, int length)
        {
            if (length <= 0)
                return;
            if (handshake)
            {
                byte[] buffer = new byte[length];
                Array.Copy(data, start, buffer, 0, length);
                if (step == 1)
                {
                    step = 0;
                    handshake = false;
                    string str = Encoding.ASCII.GetString(buffer);
                    int code = Int32.Parse(str.Substring(0, 3));
                    string msg = str.Substring(4);
                    if (code == 200)
                    {
                        Debug.Log(string.Format("{0},{1}", code, msg));
                        callback(true, ud, user.Subid, user.Secret);
                    }
                    else
                    {
                        Debug.LogError(string.Format("error code : {0}, {1}", code, msg));
                        callback(false, ud, user.Subid, user.Secret);
                    }
                }
            }
            else
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
                    SprotoRpc.RpcInfo sinfo = host.Dispatch(buffer);
                    Debug.Assert(sinfo.type == SprotoRpc.RpcType.RESPONSE);
                    Debug.Assert(sinfo.session != null);
                    Debug.Assert(sinfo.session == session);
                    string key = id_to_hex(session);
                    RespPg pg = response_pg[key];
                    var rpc = response[pg.Protocol];
                    rpc.Action(session, sinfo.responseObj, rpc.Ud);
                }
                else if (tag == s2c_req_tag)
                {
                    SprotoRpc.RpcInfo sinfo = host.Dispatch(buffer);
                    Debug.Assert(sinfo.type == SprotoRpc.RpcType.REQUEST);
                    Debug.Assert(sinfo.session != null);
                    Debug.Assert(sinfo.session == session);
                    Debug.Assert(sinfo.tag != null);

                    /**********************************/
                    string key = id_to_hex(session);
                    ReqPg pg = new ReqPg();
                    pg.Session = session;
                    pg.Protocol = sinfo.ToString();
                    pg.Index = index;
                    pg.Buffer = buffer;
                    pg.Version = 0;
                    request_pg[key] = pg;

                    /************************************/
                    var rpc = request[pg.Protocol];
                    rpc.Action(session, sinfo.requestObj, sinfo.Response, rpc.Ud);
                }
            }
        }

        void OnDisconnect(SocketError socketError, PackageSocketError packageSocketError)
        {
        }

        private byte[] WriteToken()
        {
            string u = Encoding.ASCII.GetString(Crypt.base64encode(user.Uid));
            string s = Encoding.ASCII.GetString(Crypt.base64encode(Encoding.ASCII.GetBytes(user.Server)));
            string sid = Encoding.ASCII.GetString(Crypt.base64encode(user.Subid));
            string token = string.Format("{0}@{1}#{2}:{3}", u, s, sid, index);
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

        private void Wirte(byte[] buffer, uint session, byte tag)
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
            sock.Send(tmp, 0, l);
        }

        private void Send(object ud, uint id, byte[] buffer, string protocol)
        {
            RespPg pg = new RespPg();
            pg.Session = id;
            pg.Buffer = buffer;
            pg.Index = index;
            pg.Protocol = protocol;
            pg.Version = 0;
            string key = id_to_hex(id);
            response_pg[key] = pg;
            Wirte(buffer, id, c2s_req_tag);
        }

        private uint genSession()
        {
            ++session;
            if (session == 0)
                session++;
            return session;
        }

        private string id_to_hex(uint id)
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

        public void Auth(string ipstr, int pt, User u, object d, CB cb)
        {
            index++;   // index increment.
            ip = ipstr;
            port = pt;
            user = u;
            ud = u;
            callback = cb;
            handshake = true;
            sock.Connect(ip, port);
        }

        public void Reset()
        {
            user = null;
            step = 0;
            ud = null;
            callback = null;
            handshake = false;
        }

        private void RegisterProtocol()
        {
            /*********************************/
            response["role_info"] = new RespAction { Action = role_info, Ud = null };

            /*********************************/
            request["finish_achi"] = new ReqAction { Action = finish_achi, Ud = null };
        }

        public void send_role_info(Dictionary<string, object> args)
        {
            C2sSprotoType.role_info.request requestObj = new C2sSprotoType.role_info.request();
            requestObj.role_id = (Int32)args["role_id"];
            uint id = genSession();
            byte[] req = send_request.Invoke<C2sProtocol.role_info>(requestObj, id);
            Debug.Assert(req != null);
            Send(null, id, req, "role_info");
        }

        public void role_info(uint session, SprotoTypeBase o, object ud)
        {
            C2sSprotoType.role_info.response responseObj = (C2sSprotoType.role_info.response)o;
            // TODO:
        }

        public void finish_achi(uint session, SprotoTypeBase requestObj, SprotoRpc.ResponseFunction Response, object ud)
        {
            // TODO:

            Debug.Assert(Response != null);
            S2cSprotoType.finish_achi.response responseObj = new finish_achi.response();
            byte[] resp = Response(responseObj);
            Wirte(resp, session, s2c_resp_tag);
        }

        /// <summary>
        /// 注册推送接受
        /// </summary>
        /// <param name="req"></param>
        /// <param name="str"></param>
        public void RegisterResponse(ReqCb req, String str, object ud)
        {
            request[str] = new ReqAction { Action = req, Ud = ud };
        }

        /// <summary>
        /// 注册请求
        /// </summary>
        /// <param name="req"></param>
        /// <param name="str"></param>
        public void RegisterRequest(RespCb resp, String str)
        {
            if (response.ContainsKey(str))
            {
                if (request.ContainsKey(str))
                {
                    response[str].Ud = resp;
                }
                else
                {
                    response.Add(str, new RespAction { Action = resp, Ud = ud });
                }
            }
        }

        /// <summary>
        /// 发送请求
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="resp"></param>
        /// <param name="requestObj"></param>
        /// <param name="str"></param>
        /// <param name="ud"></param>
        public void Resquest<T>(RespCb resp, SprotoTypeBase requestObj, String str, object ud)
        {
            //request[str].Ud = new RespAction { Action = resp,Ud = ud};
            RegisterRequest(resp, str);
            SendReq<T>(str, requestObj);
        }
        /// <summary>
        /// 发送推送后的结果
        /// </summary>
        /// <param name="session"></param>
        /// <param name="requestObj"></param>
        /// <param name="response"></param>
        /// <param name="ud"></param>
        public void Response(uint session, SprotoTypeBase requestObj, SprotoRpc.ResponseFunction response, object ud)
        {
            Debug.Assert(response != null);
            SendResp(requestObj, response);
        }
        public void SendReq<T>(String callback, SprotoTypeBase obj)
        {
            uint id = genSession();
            byte[] req = send_request.Invoke<T>(obj, id);
            Debug.Assert(req != null);
            Send(null, id, req, callback);
        }
        public void SendResp(SprotoTypeBase requestObj, SprotoRpc.ResponseFunction Response)
        {
            byte[] resp = Response(requestObj);
            Wirte(resp, session, s2c_resp_tag);
        }
    }
}