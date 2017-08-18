using UnityEngine;
using System.Collections.Generic;
using System.Threading;
using Maria.Network;
using System;
using Sproto;
using System.Text;
using XLua;

namespace Maria {

    [CSharpCallLua]
    [LuaCallCSharp]
    public class Context : DisposeObject, INetwork {

        protected Application _application = null;
        protected Config _config = null;
        protected TimeSync _ts = null;
        protected SharpC _sharpc = null;
        protected Debug _logger = null;

        protected EventDispatcher _dispatcher = null;
        
        protected List<Timer> _timer = new List<Timer>();
        protected Stack<Controller> _stack = new Stack<Controller>();
        protected Dictionary<string, Service> _services = new Dictionary<string, Service>();

        protected ClientLogin  _login = null;
        protected ClientSocket _client = null;
        protected User _user = new User();
        protected bool _logined = false;
        protected bool _authtcp = false;
        protected bool _authudp = false;
        
        protected Lua.Env _envScript = null;
        protected System.Random _rand = new System.Random();
        protected ModelMgr _modelmgr = null;
        protected DataSetMgr _datasetmgr = null;


        public Context(Application application, Config config, TimeSync ts) {
            _application = application;
            _config = config;
            _ts = ts;
            _sharpc = new SharpC();
            _dispatcher = new EventDispatcher(this);

            _login = new ClientLogin(this);
            _login.OnLogined = OnLoginAuthed;
            _login.OnConnected = OnLoginConnected;
            _login.OnDisconnected = OnLoginDisconnected;

            _client = new ClientSocket(this, _config.s2c, _config.c2s);
            _client.OnAuthed = OnGateAuthed;
            _client.OnConnected = OnGateConnected;
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
            //_env.update();

            //int now = _ts.LocalTime();
            for (int i = _timer.Count -1; i >= 0; i--) {
                Timer tm = _timer[i];
                if (tm.CD <= 0) {
                    if (tm.Enable && tm.CB != null) {
                        tm.CB();
                    }
                    _timer.RemoveAt(i);
                } else {
                    float cd = tm.CD;
                    cd -= delta;
                    if (cd <= 0) {
                        _timer.RemoveAt(i);
                    } else {
                        if (tm.Enable && tm.DCB != null) {
                            tm.DCB((int)delta, (int)tm.CD);
                        }
                    }
                }
            }

            foreach (var item in _services) {
                item.Value.Update(delta);
            }

            if (_stack.Count > 0) {
                Controller controller = Peek();
                if (controller != null) {
                    controller.Update(delta);
                }
            }
        }

        public Config Config { get { return _config; } }
        public TimeSync TiSync { get { return _ts; } }
        public SharpC SharpC { get { return _sharpc; } }
        public EventDispatcher EventDispatcher { get { return _dispatcher; } }

        public ClientSocket Client { get { return _client; } }
        public User U { get { return _user; } }
        public bool AuthTcp { get { return _authtcp; } }
        public bool AuthUdp { get { return _authudp; } }
        public bool Logined { get { return _logined; } }
        
        public Lua.Env EnvScript { get { return _envScript; } set { _envScript = value; } }
        public ModelMgr ModelMgr { get { return _modelmgr; } }
        public DataSetMgr DataSetMgr { get { return _datasetmgr; } }

        public int Range(int min, int max) {
            return _rand.Next(min, max);
        }

        // login
        public void LoginAuth(string s, string u, string pwd) {

            _logined = false;
            _authtcp = false;
            
            _user.Server = s;
            _user.Username = u;
            _user.Password = pwd;
            string ip = Config.LoginIp;
            int port = Config.LoginPort;
            _login.Auth(ip, port, s, u, pwd);
        }

        public void OnLoginAuthed(int code, byte[] secret, string dummy) {
            if (code == 200) {
                int _1 = dummy.IndexOf('#');
                int _2 = dummy.IndexOf('@', _1);
                int _3 = dummy.IndexOf(':', _2);

                int uid = Int32.Parse(dummy.Substring(0, _1));
                int sid = Int32.Parse(dummy.Substring(_1 + 1, _2 - _1 - 1));
                string gip = dummy.Substring(_2 + 1, _3 - _2 - 1);
                int gpt = Int32.Parse(dummy.Substring(_3 + 1));

                UnityEngine.Debug.Log(string.Format("uid: {0}, sid: {1}", uid, sid));
                UnityEngine.Debug.Log("login");

                _user.Secret = secret;
                _user.Uid = uid;
                _user.Subid = sid;

                _logined = true;
                //_config.GateIp = gip;
                //_config.GatePort = gpt;

                GateAuth();
            } else {

            }
            if (_stack.Count > 0) {
                _stack.Peek().OnLoginAuthed(code, secret, dummy);
            }
        }

        public void OnLoginConnected(bool connected) {
            if (!connected) {
                if (_stack.Count > 0) {
                    _stack.Peek().OnLoginConnected(connected);
                }
            }
        }

        public void OnLoginDisconnected() {
            if (_stack.Count > 0) {
                _stack.Peek().OnLoginDisconnected();
            }
        }

        // Gate
        public void SendReq<T>(int tag, SprotoTypeBase obj) {
            _client.SendReq<T>(tag, obj);
        }

        public void GateAuth() {
            _client.Auth(Config.GateIp, Config.GatePort, _user);
        }

        public void OnGateAuthed(int code) {
            if (code == 200) {
                _authtcp = true;
                
                string dummy = string.Empty;
                //
                EventDispatcher.FireCustomEvent(EventCustom.OnGateAuthed, null);

                //
                if (_stack.Count > 0) {
                    Controller controller = Peek();
                    controller.OnGateAuthed(code);
                }
            } else if (code == 403) {
                //LoginAuth(_user.Server, _user.Username, _user.Password);
            }
        }

        public void OnGateConnected(bool connected) {
            if (!connected) {
                if (_stack.Count > 0) {
                    Controller controller = Peek();
                    controller.OnGateConnected(connected);
                }
            }
        }

        public void OnGateDisconnected() {
            UnityEngine.Debug.Assert(_authtcp);
            EventDispatcher.FireCustomEvent(EventCustom.OnGateDisconnected, null);
            if (_stack.Count > 0) {
                var controller = Peek();
                controller.OnGateDisconnected();
            }
        }

        public Controller Peek() {
            if (_stack.Count > 0) {
                return _stack.Peek();
            }
            return null;
        }

        public T Peek<T>() where T : Controller {
            if (_stack.Count > 0) {
                return _stack.Peek() as T;
            }
            return null;
        }

        public Controller Push(Type type) {
            Controller controller = (Controller)Activator.CreateInstance(type, this);
            if (_stack.Count > 0) {
                _stack.Peek().OnExit();
            }
            _stack.Push(controller);
            controller.OnEnter();
            return controller;
        }

        public T Push<T>() where T : Controller {
            T controller = Activator.CreateInstance(typeof(T), this) as T;
            if (_stack.Count > 0) {
                _stack.Peek().OnExit();
            }
            _stack.Push(controller);
            controller.OnEnter();
            return controller;
        }

        public Controller Pop() {
            Controller controller = null;
            if (_stack.Count > 0) {
                controller = _stack.Peek();
                controller.OnExit();
                _stack.Pop();
            }
            if (_stack.Count > 0) {
                _stack.Peek().OnEnter();
            }
            return controller;
        }

        public T Pop<T>() where T : Controller {
            T controller = null;
            if (_stack.Count > 0) {
                controller = _stack.Peek() as T;
                controller.OnExit();
                _stack.Pop();
            }
            if (_stack.Count > 0) {
                _stack.Peek().OnEnter();
            }
            return controller;
        }

        public Timer Countdown(string name, int cd, Timer.CountdownDeltaCb dcb, Timer.CountdownCb cb) {
            Timer tm = new Timer();
            tm.Name = name;
            tm.Enable = true;
            tm.CD = cd;
            tm.DCB = dcb;
            tm.CB = cb;
            _timer.Add(tm);
            return tm;
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

        public T QueryService<T>(string name) where T : Service {
            if (_services.ContainsKey(name)) {
                return _services[name] as T;
            }
            return null;
        }

        public Service QueryService(string name) {
            if (_services.ContainsKey(name)) {
                return _services[name];
            }
            return null;
        }

        public void Enqueue(Command cmd) {
            _application.Enqueue(cmd);
        }

        public void EnqueueRenderQueue(Actor.RenderHandler handler) {
            _application.EnqueueRenderQueue(handler);
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

        public void OnUdpSync() {
            var controller = Peek();
            if (controller != null) {
                controller.OnUdpSync();
            }
        }

        public void OnUdpRecv(PackageSocketUdp.R r) {
            var controller = Peek();
            if (controller != null) {
                controller.OnUdpRecv(r);
            }
        }
    }
}

