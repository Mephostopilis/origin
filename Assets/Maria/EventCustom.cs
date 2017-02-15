using System;
using System.Collections.Generic;
using System.Text;

namespace Maria {
    public class EventCustom : Event {
        public static string OnDisconnected = "OnDisconnected";
        public static string OnAuthed = "OnAuthed";

        private string _name = string.Empty;
        private object _param = null;
        private object _ud = null;

        public EventCustom(string name)
            : this(name, null, null) {
        }

        public EventCustom(string name, object param)
            : this(name, param, null) {
        }

        public EventCustom(string name, object param, object ud) {
            _name = name;
            _param = param;
            _ud = ud;
        }

        public string Name { get { return _name; } }
        public object Param { get { return _param; } }
        public object Ud { get { return _ud; } }

    }
}
