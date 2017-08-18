using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Maria {

    [XLua.LuaCallCSharp]
    public class EventCmd : Event {

        /// <summary>
        /// ui 到框架的事件传递
        /// </summary>
        private Context _ctx = null;
        private uint _cmd = 0;
        private GameObject _orgin = null;
        private Message _msg = null;

        public EventCmd(Context ctx, uint cmd)
            : this(ctx, cmd, null, null) {
        }

        public EventCmd(Context ctx, uint cmd, Message msg)
            : this(ctx, cmd, null, msg) {
        }

        public EventCmd(Context ctx, uint cmd, GameObject orgin)
            : this(ctx, cmd, orgin, null) {
        }

        public EventCmd(Context ctx, uint cmd, GameObject orgin, Message msg)
            : base(ctx) {
            _type = Type.CMD;
            _cmd = cmd;
            _orgin = orgin;
            _msg = msg;
        }

        public uint Cmd { get { return _cmd; } }
        public GameObject Orgin { get { return _orgin; } }
        public Message Msg { get { return _msg; } }
    }
}
