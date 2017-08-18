using System;
using System.Collections.Generic;
using System.Text;

namespace Maria {

    /// <summary>
    /// 自定义事件仅限于框架内部使用
    /// </summary>
    [XLua.CSharpCallLua]
    [XLua.LuaCallCSharp]
    public class EventListenerCustom : EventListener {

        [XLua.CSharpCallLua]
        [XLua.LuaCallCSharp]
        public delegate void OnEventCustomHandler(EventCustom e);

        private string _name;
        private OnEventCustomHandler _callback;
        private object _addition;

        public EventListenerCustom(string name, OnEventCustomHandler callback, object addition) {
            _name = name;
            _callback = callback;
            _addition = addition;
        }

        public string Name { get { return _name; } }
        public OnEventCustomHandler Handler { get { return _callback; } }
        public object Addition { get { return _addition; } }

    }
}
