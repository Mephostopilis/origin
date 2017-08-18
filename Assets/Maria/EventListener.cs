using System;
using System.Collections.Generic;
using System.Text;

namespace Maria {

    [XLua.LuaCallCSharp]
    public class EventListener {

        protected bool _enable;

        public EventListener() {
            _enable = true;
        }

        public bool Enable { get { return _enable; } set { _enable = value; } }

    }
}
