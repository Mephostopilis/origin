using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using Maria.Network;
using System;

namespace Maria  {
    public class Controller : INetwork {
        protected Context _ctx = null;
        protected bool _authtcp = false;
        protected bool _authudp = false;
        protected List<Actor> _actors = new List<Actor>();
        protected string _name = string.Empty;

        public Controller(Context ctx) {
            Debug.Assert(ctx != null);
            _ctx = ctx;
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

        public virtual void Enter() {
        }

        public virtual void Exit() {
        }

        public virtual void OnGateAuthed(int code) {
            if (code == 200) {
                _authtcp = true;
            }
        }

        public virtual void OnGateDisconnected() {
            _authtcp = false;
        }

        public virtual void OnUdpSync() {
            _authudp = true;
        }

        public virtual void OnUdpRecv(PackageSocketUdp.R r) {
            throw new NotImplementedException();
        }
    }
}
