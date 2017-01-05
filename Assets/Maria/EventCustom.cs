using System;
using System.Collections.Generic;
using System.Text;

namespace Maria {
    public class EventCustom : Event {
        public static string OnDisconnected = "OnDisconnected";
        public static string OnAuthed = "OnAuthed";

        private string _name = string.Empty;
        private object _ud = null;

        public EventCustom(string name)
            : this(name, null) {
        }

        public EventCustom(string name, object ud) {
            _name = name;
            _ud = ud;
        }

        public object Ud { get { return _ud; } }
        public string Name { get { return _name; } }

    }
}
