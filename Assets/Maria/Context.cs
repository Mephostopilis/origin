using UnityEngine;
using System.Collections.Generic;
using System.Threading;
using Maria.Network;
using System;
using Sproto;
using Maria.Ball;
using System.Text;

namespace Maria
{
    public class Context
    {
        protected Thread _worker = null;
        protected Queue<Message> _queue = new Queue<Message>();
        protected Dictionary<string, Controller> _hash = new Dictionary<string, Controller>();
        protected ClientLogin _login = null;
        protected ClientSocket _client = null;
        protected Gate _gate = null;
        protected User _user = new User();
        private ClientLogin.CB _loginCb;
        protected  App _app;

        public Context(App app)
        {
            _app = app;

            _worker = new Thread(new ThreadStart(Worker));
            _worker.Start();

            _gate = new Gate(this);

            _login = new ClientLogin(this);
            _client = new ClientSocket(this);

            Config = new Config();
        }

        // Use this for initialization
        public void Start()
        {

        }

        // Update is called once per frame
        public void Update(float delta)
        {
            _login.Update();
            _client.Update();
        }

        public Config Config { get; set; }

        public void Enqueue(Message msg)
        {
            lock (_queue)
            {
                _queue.Enqueue(msg);
            }
        }

        private void Worker()
        {
            while (true)
            {
                _gate.Run();

                Message msg = null;
                lock (_queue)
                {
                    if (_queue.Count > 0)
                    {
                        msg = _queue.Dequeue();
                    }
                }
                if (msg != null)
                {
                    msg.execute();
                }
                else
                {
                    Thread.Sleep(1000);
                }
            }
        }

        public T GetController<T>(string name) where T : Controller
        {
            return (T)_hash[name];
        }

        public void SendReq<T>(String callback, SprotoTypeBase obj)
        {
            _client.SendReq<T>(callback, obj);
        }

        public void AuthLogin(string s, string u, string pwd, ClientLogin.CB cb)
        {
            _loginCb = cb;
            _user.Server = s;
            _user.Username = u;
            _user.Password = pwd;
            string ip = Config.LoginIp;
            int port = Config.LoginPort;
            _login.Auth(ip, port, s, u, pwd, AuthLoginCb);
        }

        public void AuthLoginCb(bool ok, byte[] secret, string dummy)
        {
            if (ok)
            {
                int _1 = dummy.IndexOf('#');
                int _2 = dummy.IndexOf('@', _1);
                int _3 = dummy.IndexOf(':', _2);

                byte[] uid = Encoding.ASCII.GetBytes(dummy.Substring(0, _1));
                byte[] sid = Encoding.ASCII.GetBytes(dummy.Substring(_1 + 1, _2 - _1 - 1));
                string gip = dummy.Substring(_2 + 1, _3 - _2 - 1);
                int gpt = Int32.Parse(dummy.Substring(_3 + 1));

                Debug.Log(string.Format("uid: {0}, sid: {1}", uid, sid));
                Debug.Log("login");

                _user.Secret = secret;
                _user.Uid = uid;
                _user.Subid = sid;

                _client.Auth(Config.GateIp, Config.GatePort, _user, AuthGateCB);
            }
            else
            {
            }
        }

        public void AuthGateCB(bool ok)
        {
            string dummy = string.Empty;
            _loginCb(ok, _user.Secret, dummy);
        }
    }
}

