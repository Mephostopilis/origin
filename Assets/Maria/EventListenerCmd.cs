using System;
using System.Collections.Generic;
using System.Text;

namespace Maria {
    public class EventListenerCmd : EventListener {
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
