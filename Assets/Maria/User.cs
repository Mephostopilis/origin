using System;
using System.Collections.Generic;
using System.Text;

namespace Maria {
    public class User {
        private Dictionary<string, Module> _modules = new Dictionary<string, Module>();

        public string Server { get; set; }    // 没有啥用
        public string Username { set; get; }  // 没有啥用
        public string Password { set; get; }  // 没有啥用
        public int Uid { get; set; }
        public int Subid { set; get; }
        public byte[] Secret { set; get; }

        public T GetModule<T>() where T : Module
        {
            Type t = typeof(T);
            return _modules[t.FullName] as T;
        }

        public void AddModule<T>() where T : Module
        {
            Module o = Activator.CreateInstance(typeof(T), this) as T;
            string name = o.GetType().FullName;
            _modules[name] = o;
        }

        public void RemoveModule<T>() where T : Module
        {
            Type t = typeof(T);
            string name = t.FullName;
            _modules.Remove(name);
        }

        public virtual void OnGateConnected(bool connected) {
            foreach (var m in _modules) {
                m.Value.OnGateConnected(connected);
            }
        }

        public virtual void OnGateAuthed(int code) {
            foreach (var m in _modules) {
                m.Value.OnGateAuthed(code);
            }
        }

        public virtual void OnGateDisconnected() {
            foreach (var m in _modules) {
                m.Value.OnGateDisconnected();
            }
        }
    }
}
