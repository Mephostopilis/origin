using UnityEngine;
using System.Collections.Generic;
using System.Threading;
using Maria.Network;
using System;
using Sproto;
using System.Text;

namespace Maria
{
    public class Context
    {
        private class Timer
        {
            public string Name { get; set; }
            public float CD { get; set; }
            public CountdownCb CB { get; set; }
        }

        public delegate void CountdownCb();

        protected Thread _worker = null;
        protected Queue<Message> _queue = new Queue<Message>();
        protected Dictionary<string, Controller> _hash = new Dictionary<string, Controller>();
        protected Stack<Controller> _stack = new Stack<Controller>();
        protected Controller _cur;
        protected ClientLogin _login = null;
        protected ClientSocket _client = null;
        protected Gate _gate = null;
        protected User _user = new User();
        private ClientSocket.CB _authcb;
        protected readonly global::App _app;
        private Dictionary<string, Timer> _timer = new Dictionary<string, Timer>();
        protected bool _auth = false;

        public Context(global::App app)
        {
            _app = app;
            var go = GameObject.Find("/Assets");
            Assets = go;

            _worker = new Thread(new ThreadStart(Worker));
            _worker.Start();

            _gate = new Gate(this);

            _login = new ClientLogin(this);
            _client = new ClientSocket(this);

            _hash["start"] = new StartController(this);
            _hash["login"] = new LoginController(this);

            _hash["start"].Run();

            AuthUdpFlag = false;
        }

        // Use this for initialization
        public void Start()
        {

        }

        // Update is called once per frame
        public virtual void Update(float delta)
        {

            _login.Update();
            _client.Update();

            foreach (var item in _timer)
            {
                Timer tm = item.Value as Timer;
                if (tm != null)
                {
                    if (tm.CD > 0)
                    {
                        Debug.Log(tm.CD);
                        tm.CD -= delta;
                        if (tm.CD < 0)
                        {
                            tm.CB();
                            //_timer.Remove()
                            //_timer.Remove(tm.Name);
                        }
                    }
                    else
                    {
                        //_timer.Remove(tm.Name);
                    }
                }
            }

            if (_cur != null)
            {
                _cur.Update(delta);
            }
        }

        public Config Config { get; set; }

        public global::App App { get { return _app; } }

        public GameObject Assets { get; set; }

        public bool AuthUdpFlag { get; set; }

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

        public Controller GetCurController()
        {
            return _cur;
        }

        public void SendReq<T>(String callback, SprotoTypeBase obj)
        {
            _client.SendReq<T>(callback, obj);
        }

        public void AuthLogin(string s, string u, string pwd, ClientSocket.CB cb)
        {
            _authcb = cb;
            
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

        public void AuthGate(ClientSocket.CB cb)
        {
            _authcb = cb;
            _client.Auth(Config.GateIp, Config.GatePort, _user, AuthGateCB);
        }

        public void AuthGateCB(int ok)
        {
            if (ok == 200)
            {
                _auth = true;
                string dummy = string.Empty;
                _authcb(ok);
            }
            else if (ok == 403)
            {
                AuthLogin(_user.Server, _user.Username, _user.Password, _authcb);
            }
        }

        public void AuthUdp()
        {
            if (!AuthUdpFlag)
            {
                _client.AuthUdp();
            }
        }

        public void SendUdp(byte[] data)
        {
            _client.SendUdp(data);
        }

        public void ConnectUdp(long session, string ip, int port)
        {
            _client.ConnectUdp(session, ip, port);
        }

        public void Push(string name)
        {
            var ctr = _hash[name];
            Debug.Assert(ctr != null);
            _stack.Push(ctr);
            _cur = ctr;
            _cur.Enter();
        }

        public void Pop()
        {

        }

        public void Countdown(string name, float cd, CountdownCb cb)
        {
            var tm = new Timer();
            tm.Name = name;
            tm.CD = cd;
            tm.CB = cb;
            _timer[name] = tm;
        }
    }
}

