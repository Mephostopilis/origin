using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Maria {

    [XLua.LuaCallCSharp]
    public class EventCmd : Event {

        // 1 ~ 1000  框架保留事件
        public static uint EVENT_UPDATERES_BEGIN = 1;
        public static uint EVENT_UPDATERES_END = 2;
        public static uint EVENT_START_SETUP_ROOT = 3;
        public static uint EVENT_NOTREACHABLE = 4;
        public static uint EVENT_REACHABLEVIACARRIERDATANETWORK = 5;
        public static uint EVENT_REACHABLEVIALOCALAREANETWORK = 6;

        /// <summary>
        /// ui 到框架的事件传递
        /// </summary>
        
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
