using System.Collections.Generic;
using Maria.Network;
using System;

/// <summary>
/// Controller 主要作用就是调度Actor,分发事件
/// </summary>
namespace Maria {
    public class Controller : INetwork {
        protected Context _ctx = null;
        protected bool _login = false;
        protected bool _authtcp = false;
        protected bool _authudp = false;
        protected List<Actor> _actors = new List<Actor>();
        protected string _name = string.Empty;

        public Controller(Context ctx) {
            UnityEngine.Debug.Assert(ctx != null);
            _ctx = ctx;
        }

        public Context Ctx { get { return _ctx; } }

        public string Name { get { return _name; } }

        public virtual void OnEnter() {
            foreach (var item in _actors) {
                item.OnEnter();
            }
        }

        public virtual void OnExit() {
            foreach (var item in _actors) {
                item.OnExit();
            }
        }

        // Update is called once per frame
        public virtual void Update(float delta) {
            foreach (var item in _actors) {
                item.Update(delta);
            }
        }

        public void Add(Actor item) {
            _actors.Add(item);
        }

        public bool Remove(Actor item) {
            return _actors.Remove(item);
        }

        public virtual void LoginAuth(string server, string username, string password) {}

        /// <summary>
        /// @param connected 开始不一定就连接成功
        /// </summary>
        /// <param name="connected"></param>
        public virtual void OnLoginConnected(bool connected) {}

        public virtual void OnLoginAuthed(int code, byte[] secret, string dummy) {}

        public virtual void OnLoginDisconnected() {}


        public virtual void GateAuth() {}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="connected"></param>
        public virtual void OnGateConnected(bool connected) {}

        public virtual void OnGateAuthed(int code) {
            if (code == 200) {
                _authtcp = true;
            }
        }

        public virtual void OnGateDisconnected() {
            _authtcp = false;
        }

        public virtual void Logout() {

        }

        public virtual void OnUdpRecv(byte[] data, int start, int size) {
            throw new NotImplementedException();
        }

    }
}
