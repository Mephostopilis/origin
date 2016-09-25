using UnityEngine;
using System;
using System.Text;
using System.Net.Sockets;
using Maria.Encrypt;

namespace Maria.Network
{
    public class ClientLogin
    {
        public delegate void CB(bool ok, object ud, byte[] secret, string dummy);

        private Context _ctx;
        private PackageSocket sock = new PackageSocket();
        private string ip = String.Empty;
        private int port = 0;
        private string server = null;
        private string user = null;
        private string password = null;
        private byte[] challenge = null;
        private byte[] clientkey = null;
        private byte[] secret = null;
        private byte[] uid = null;
        private byte[] subid = null;
        private int step = 0;
        private bool handshake = false;
        private object ud = null;
        private CB callback = null;

        public ClientLogin(Context ctx)
        {
            _ctx = ctx;
            sock.OnConnect = OnConnect;
            sock.OnDisconnect = OnDisconnect;
            sock.OnRecvive = OnRecvive;
            sock.SetEnabledPing(false);
            sock.SetPackageSocketType(PackageSocketType.Line);
        }

        // Use this for initialization
        void Start()
        {
        }

        // Update is called once per frame
        public void Update()
        {
            sock.Update();
        }

        void OnConnect(bool connected)
        {
            if (handshake)
                step++;
        }

        void OnRecvive(byte[] data, int start, int length)
        {
            byte[] buffer = new byte[length];
            Array.Copy(data, start, buffer, 0, length);
            if (step == 1)
            {
                challenge = Crypt.base64decode(buffer);
                clientkey = Crypt.randomkey();
                var buf = Crypt.base64encode(Crypt.dhexchange(clientkey));
                sock.SendLine(buf, 0, buf.Length);
                step++;
            }
            else if (step == 2)
            {
                byte[] key = Crypt.base64decode(buffer);
                secret = Crypt.dhsecret(key, clientkey);
                Debug.Log("sceret is " + Encoding.ASCII.GetString(Crypt.hexencode(secret)));
                byte[] hmac = Crypt.hmac64(challenge, secret);
                var buf = Crypt.base64encode(hmac);
                sock.SendLine(buf, 0, buf.Length);
                WriteToke(server, user, password);
                step++;
            }
            else if (step == 3)
            {
                string str = Encoding.ASCII.GetString(buffer);
                int code = Int32.Parse(str.Substring(0, 3));
                string msg = str.Substring(4);
                if (code == 200)
                {
                    callback(true, ud, secret, msg);
                    handshake = false;
                    sock.Close();
                    Reset();
                }
                else
                {
                    Debug.LogError(string.Format("error code : {0}, {1}", code, msg));
                    if (code == 406)
                    {
                        callback(true, ud, secret, msg);
                    }
                    else
                    {
                        callback(false, ud, secret, msg);
                    }

                    handshake = false;
                    sock.Close();
                    Reset();
                }
            }
        }

        void OnDisconnect(SocketError socketError, PackageSocketError packageSocketError)
        {
            if (handshake)
            {
                callback(false, ud, secret, string.Empty);
                Reset();
            }
        }

        protected void WriteToke(string server, string user, string password)
        {
            string str = String.Format("{0}@{1}:{2}", Encoding.ASCII.GetString(Crypt.base64encode(Encoding.ASCII.GetBytes(user))),
                Encoding.ASCII.GetString(Crypt.base64encode(Encoding.ASCII.GetBytes(server))),
                Encoding.ASCII.GetString(Crypt.base64encode(Encoding.ASCII.GetBytes(password))));
            byte[] etoken = Crypt.desencode(secret, Encoding.ASCII.GetBytes(str));
            byte[] b = Crypt.base64encode(etoken);
            sock.SendLine(b, 0, b.Length);
        }

        private void Reset()
        {
            ip = null;
            port = 0;
            server = null;
            user = null;
            password = null;
            challenge = null;
            clientkey = null;
            secret = null;
            subid = null;
            step = 0;
            handshake = false;
            ud = null;
            callback = null;
        }

        public void Auth(string ipstr, int pt, string s, string u, string pwd, object d, CB cb)
        {
            ip = ipstr;
            port = pt;
            server = s;
            user = u;
            password = pwd;
            ud = d;
            callback = cb;
            handshake = true;
            sock.Connect(ip, port);
        }
    }

}