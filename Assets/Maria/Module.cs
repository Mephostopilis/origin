using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Maria
{
    public class Module
    {
        protected User _user;

        public Module(User u)
        {
            _user = u;
        }

        public T GetModule<T>() where T : Module
        {
            return _user.GetModule<T>();
        }

        public void AddModule<T>() where T : Module
        {
            _user.AddModule<T>();
        }

        public void RemoveModule<T>() where T : Module
        {
            _user.RemoveModule<T>();
        }

        public virtual void OnGateConnected(bool connected) {}

        public virtual void OnGateAuthed(int code) {}

        public virtual void OnGateDisconnected() {}
    }
}
