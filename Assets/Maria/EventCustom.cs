using System;
using System.Collections.Generic;
using System.Text;

namespace Maria {

    [XLua.LuaCallCSharp]
    public class EventCustom : Event {

        public static readonly string OnGateAuthed = "OnGateAuthed";
        public static readonly string OnGateDisconnected = "OnGateDisconnected";

        private Context _ctx = null;
        private string _name = string.Empty;
        private object _addition = null;
        private object _ud = null;

        public EventCustom(Context ctx, string name)
            : this(ctx, name, null, null) {
        }

        public EventCustom(Context ctx, string name, object addition)
            : this(ctx, name, addition, null) {
        }

        public EventCustom(Context ctx, string name, object addition, object ud)
            : base(ctx) {
            _type = Type.CUSTOM;
            _name = name;
            _addition = addition;
            _ud = ud;
        }

        
        public string Name { get { return _name; } }
        public object Addition { get { return _addition; } }
        public object Ud { get { return _ud; } }

    }
}
