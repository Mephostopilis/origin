using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Maria {
    public class Command {
        
        private uint _cmd;
        private GameObject _orgin;
        private Message _msg = null;

        public Command(uint cmd) {
            _cmd = cmd;
        }

        public Command(uint cmd, GameObject orgin) {
            _cmd = cmd;
            _orgin = orgin;
        }

        public Command(uint cmd, GameObject orgin, Message msg) {
            _cmd = cmd;
            _orgin = orgin;
            _msg = msg;
        }

        public uint Cmd { get { return _cmd; } }

        public GameObject Orgin { get { return _orgin; } }

        public Message Msg { get { return _msg; } set { _msg = value; } }
        
    }
}
