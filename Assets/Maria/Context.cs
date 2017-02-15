using UnityEngine;
using System.Collections.Generic;
using System.Threading;
using Maria.Network;
using System;
using Sproto;
using System.Text;

namespace Maria {
    public class Context : DisposeObject, INetwork {
        private class Timer {
            public string Name { get; set; }
            public float CD { get; set; }
            public CountdownCb CB { get; set; }
        }

        public delegate void CountdownCb();

        protected Application _application;
        protected Config _config = null;
        protected TimeSync _ts = null;
        protected SharpC _sharpc = null;

        protected EventDispatcher _dispatcher = null;
        protected Dictionary<string, Controller> _hash = new Dictionary<string, Controller>();
        protected Stack<Controller> _stack = new Stack<Controller>();

        private Dictionary<string, Timer> _timer = new Dictionary<string, Timer>();
        private Dictionary<string, Service> _services = new Dictionary<string, Service>();

        protected ClientLogin _login = null;
        protected ClientSocket _client = null;
        protected User _user = new User();

        protected bool _authtcp = false;
        protected bool _authudp = false;

        public Context(Application application, Config config, TimeSync ts) {
            _application = application;
            _config = config;
            _ts = ts;
            _sharpc = new SharpC();

            _dispatcher = new EventDispatcher(this);

            _login = new ClientLogin(this);
            _login.OnLogined = LoginAuthCb;
            _login.OnDisconnected = LoginDisconnect;

            _client = new ClientSocket(this, _config.s2c, _config.c2s);
            _client.OnAuthed = OnGateAuthed;
            _client.OnDisconnected = OnGateDisconnected;
            _client.OnSyncUdp = OnUdpSync;
            _client.OnRecvUdp = OnUdpRecv;
        }

        protected override void Dispose(bool disposing) {
            if (_disposed) {
                return;
            }
            if (disposing) {
                // 清理托管资源，调用自己管理的对象的Dispose方法
                _client.Dispose();
            }
            // 清理非托管资源

            _disposed = true;
        }

        // Update is called once per frame
        public virtual void Update(float delta) {
            _login.Update();
            _client.Update();

            foreach (var item in _timer) {
                Timer tm = item.Value as Timer;
                if (tm != null) {
                    if (tm.CD > 0) {
                        tm.CD -= delta;
                        if (tm.CD < 0) {
                            tm.CB();
                            //_timer.Remove()
                            //_timer.Remove(tm.Name);
                        }
                    } else {
                        //_timer.Remove(tm.Name);
                    }
                }
            }

            foreach (var item in _services) {
                item.Value.Update(delta);
            }

            if (_stack.Count > 0) {
                Controller controller = Top();
                if (controller != null) {
                    controller.Update(delta);
                }
            }
        }

        public EventDispatcher EventDispatcher { get { return _dispatcher; } set { _dispatcher = value; } }

        public Config Config { get { return _config; } set { _config = value; } }

        public TimeSync TiSync { get { return _ts; } set { _ts = value; } }

        public SharpC SharpC { get { return _sharpc; } }

        public User U { get { return _user; } }

        public T GetController<T>(string name) where T : Controller {
            try {
                if (_hash.ContainsKey(name)) {
                    Controller controller = _hash[name];
                    return controller as T;
                } else {
                    Debug.LogError(string.Format("{0} no't exitstence", name));
                    return null;
                }
            } catch (KeyNotFoundException ex) {
                Debug.LogError(ex.Message);
                return null;
            }
        }

        // login
        public void LoginAuth(string s, string u, string pwd) {
            _authtcp = false;

            _user.Server = s;
            _user.Username = u;
            _user.Password = pwd;
            string ip = Config.LoginIp;
            int port = Config.LoginPort;
            _login.Auth(ip, port, s, u, pwd);
        }

        public void LoginAuthCb(int code, byte[] secret, string dummy) {
            if (code == 200) {
                int _1 = dummy.IndexOf('#');
                int _2 = dummy.IndexOf('@', _1);
                int _3 = dummy.IndexOf(':', _2);

                int uid = Int32.Parse(dummy.Substring(0, _1));
                int sid = Int32.Parse(dummy.Substring(_1 + 1, _2 - _1 - 1));
                string gip = dummy.Substring(_2 + 1, _3 - _2 - 1);
                int gpt = Int32.Parse(dummy.Substring(_3 + 1));

                Debug.Log(string.Format("uid: {0}, sid: {1}", uid, sid));
                Debug.Log("login");

                _user.Secret = secret;
                _user.Uid = uid;
                _user.Subid = sid;

                //_config.GateIp = gip;
                //_config.GatePort = gpt;
                GateAuth();
            } else {
            }
        }

        public virtual void LoginDisconnect() {
        }

        // TCP
        public void SendReq<T>(int tag, SprotoTypeBase obj) {
            _client.SendReq<T>(tag, obj);
        }

        public void GateAuth() {
            _client.Auth(Config.GateIp, Config.GatePort, _user);
        }

        // UDP
        public void UdpAuth(long session, string ip, int port) {
            _client.UdpAuth(session, ip, port);
        }


        public void SendUdp(byte[] data) {
            if (_authudp) {
                _client.SendUdp(data);
            }
        }

        public Controller Top() {
            if (_stack.Count > 0) {
                return _stack.Peek();
            }
            return null;
        }

        public void Push(string name) {
            var ctr = _hash[name];
            Debug.Assert(ctr != null);
            _stack.Push(ctr);
            ctr.Enter();
        }

        public void Pop() {
            _stack.Pop();
        }

        public void Countdown(string name, float cd, CountdownCb cb) {
            Timer tm = null;
            if (_timer.ContainsKey(name)) {
                tm = _timer[name];
                tm.CD = cd;
                tm.CB = cb;
            } else {
                tm = new Timer();
                tm.Name = name;
                tm.CD = cd;
                tm.CB = cb;
                _timer[name] = tm;
            }
        }

        public void RegService(string name, Service s) {
            if (_services.ContainsKey(name)) {
                _services[name] = s;
            } else {
                _services[name] = s;
            }
        }

        public void UnrService(string name, Service s) {
            if (_services.ContainsKey(name)) {
                _services.Remove(name);
            }
        }

        public Service QueryService(string name) {
            if (_services.ContainsKey(name)) {
                return _services[name];
            }
            return null;
        }

        public void EnqueueRenderQueue(Actor.RenderHandler handler) {
            _application.EnqueueRenderQueue(handler);
        }

        public void FireCustomEvent(string eventName, object ud) {
            _dispatcher.FireCustomEvent(eventName, ud);
        }

        public void OnGateAuthed(int code) {
            if (code == 200) {
                _authtcp = true;
                string dummy = string.Empty;
                //
                EventDispatcher.FireCustomEvent(EventCustom.OnAuthed, null); 
                //
                Controller controller = Top();
                controller.OnGateAuthed(code);
            } else if (code == 403) {
                //LoginAuth(_user.Server, _user.Username, _user.Password);
            }
        }

        public void OnGateDisconnected() {
            EventDispatcher.FireCustomEvent(EventCustom.OnDisconnected, null);
            var controller = Top();
            controller.OnGateDisconnected();
        }

        public void OnUdpSync() {
            var controller = Top();
            controller.OnUdpSync();
        }

        public void OnUdpRecv(PackageSocketUdp.R r) {
            var controller = Top();
            controller.OnUdpRecv(r);
        }
    }
}

