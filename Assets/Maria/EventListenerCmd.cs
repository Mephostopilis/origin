using System;
using System.Collections.Generic;
using System.Text;

namespace Maria {

    /// <summary>
    /// 此事件仅限与ui传递到框架内部使用
    /// </summary>
    [XLua.CSharpCallLua]
    [XLua.LuaCallCSharp]
    public class EventListenerCmd : EventListener {

        [XLua.CSharpCallLua]
        [XLua.LuaCallCSharp]
        public delegate void OnEventCmdHandler(EventCmd e);

        private uint _cmd;
        private OnEventCmdHandler _callback;

        public EventListenerCmd(uint cmd, OnEventCmdHandler callback) {
            _cmd = cmd;
            _callback = callback;
        }

        public uint Cmd { get { return _cmd; } }

        public OnEventCmdHandler Handler { get { return _callback; } }
    }
}
