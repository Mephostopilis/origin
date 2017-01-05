using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Maria {
    public class EventCmd : Event {

        public static uint EVENT_LOGIN = 1;
        public static uint EVENT_SETUP_LOGINPANEL = 2;

        private uint _cmd;
        private GameObject _orgin;
        private Message _msg;

        public EventCmd(uint cmd)
            : this(cmd, null, null) {
        }

        public EventCmd(uint cmd, Message msg)
            : this(cmd, null, msg) {
        }

        public EventCmd(uint cmd, GameObject orgin)
            : this(cmd, orgin, null) {
        }

        public EventCmd(uint cmd, GameObject orgin, Message msg) {
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
