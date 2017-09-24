using System;
using System.Collections.Generic;
using System.Text;

namespace Maria {
    public class Event {
        public enum Type {
            NONE,
            CMD,
            CUSTOM,
        }

        protected Context _ctx = null;
        protected Type _type = Type.NONE;

        public Event(Context ctx) {
            _ctx = ctx;
        }

        public Context Ctx { get { return _ctx; } }
        public Type T { get { return _type; } }
    }
}
