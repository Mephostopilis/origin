using UnityEngine;
using System;
using System.Text;
using System.Net.Sockets;
using Maria.Encrypt;

namespace Maria.Network {
    public class ClientLogin {
        public delegate void OnLoginedCB(int code, byte[] secret, string dummy);
        public delegate void OnDisconnectCB();

        private Context _ctx;
        private PackageSocket _sock = new PackageSocket();
        private string _ip = String.Empty;
        private int _port = 0;
        private string _server = null;
        private string _user = null;
        private string _password = null;
        private byte[] _challenge = null;
        private byte[] _clientkey = null;
        private byte[] _secret = null;
        private int _step = 0;

        public ClientLogin(Context ctx) {
            _ctx = ctx;
            _sock.OnConnect = OnConnect;
            _sock.OnDisconnect = OnDisconnect;
            _sock.OnRecvive = OnRecvive;
            _sock.SetEnabledPing(false);
            _sock.SetPackageSocketType(PackageSocketType.Line);
        }

        public OnLoginedCB OnLogined { get; set; }
        public OnDisconnectCB OnDisconnected { get; set; }

        public void Auth(string ipstr, int pt, string s, string u, string pwd) {
            _ip = ipstr;
            _port = pt;
            _server = s;
            _user = u;
            _password = pwd;
            _step = 0;
            _sock.Connect(_ip, _port);
        }

        // Update is called once per frame
        public void Update() {
            _sock.Update();
        }

        private void OnConnect(bool connected) {
            if (connected) {
                _step++;
            } else {
                _sock.Connect(_ip, _port);
            }
        }

        private void OnRecvive(byte[] data, int start, int length) {
            byte[] buffer = new byte[length];
            Array.Copy(data, start, buffer, 0, length);
            if (_step == 1) {
                _challenge = Crypt.base64decode(buffer);
                _clientkey = Crypt.randomkey();
                var buf = Crypt.base64encode(Crypt.dhexchange(_clientkey));
                _sock.SendLine(buf, 0, buf.Length);
                _step++;
            } else if (_step == 2) {
                byte[] key = Crypt.base64decode(buffer);
                _secret = Crypt.dhsecret(key, _clientkey);
                Debug.Log("sceret is " + Encoding.ASCII.GetString(Crypt.hexencode(_secret)));
                byte[] hmac = Crypt.hmac64(_challenge, _secret);
                var buf = Crypt.base64encode(hmac);
                _sock.SendLine(buf, 0, buf.Length);
                WriteToke(_server, _user, _password);
                _step++;
            } else if (_step == 3) {
                _sock.Close();

                string str = Encoding.ASCII.GetString(buffer);
                int code = Int32.Parse(str.Substring(0, 3));
                string msg = str.Substring(4);
                if (code == 200) {
                    byte[] buf = Encoding.ASCII.GetBytes(msg);
                    buf = Crypt.base64decode(buf);
                    string pg = Encoding.ASCII.GetString(buf);
                    if (OnLogined != null) {
                        OnLogined(code, _secret, pg);
                    }
                } else if (code == 403) {
                    if (OnLogined != null) {
                        OnLogined(code, _secret, msg);
                    }
                } else {
                    if (OnLogined != null) {
                        OnLogined(code, _secret, msg);
                    }
                }
                Reset();
            }
        }

        private void OnDisconnect(SocketError _socketError, PackageSocketError packageSocketError) {
            if (OnDisconnected != null) {
                OnDisconnected();
            }
        }

        private void WriteToke(string _server, string _user, string _password) {
            string str = String.Format("{0}@{1}:{2}", Encoding.ASCII.GetString(Crypt.base64encode(Encoding.ASCII.GetBytes(_user))),
                Encoding.ASCII.GetString(Crypt.base64encode(Encoding.ASCII.GetBytes(_server))),
                Encoding.ASCII.GetString(Crypt.base64encode(Encoding.ASCII.GetBytes(_password))));
            byte[] etoken = Crypt.desencode(_secret, Encoding.ASCII.GetBytes(str));
            byte[] b = Crypt.base64encode(etoken);
            _sock.SendLine(b, 0, b.Length);
        }

        private void Reset() {
            _ip = null;
            _port = 0;
            _server = null;
            _user = null;
            _password = null;
            _challenge = null;
            _clientkey = null;
            _secret = null;
            _step = 0;
        }

    }

}