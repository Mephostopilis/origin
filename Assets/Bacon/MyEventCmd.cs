using Maria;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bacon {
    class MyEventCmd : EventCmd {

        public static uint EVENT_PRESSUP    = 101;
        public static uint EVENT_PRESSRIGHT = 102;
        public static uint EVENT_PRESSDOWN  = 103;
        public static uint EVENT_PRESSLEFT  = 104;

        public static uint EVENT_SETUP_SCENE = 105;
        public static uint EVENT_SETUP_VIEW = 106;
        public static uint EVENT_SETUP_MAP = 107;

        public static uint EVENT_SETUP_STARTROOT = 108;

        public static uint EVENT_ONBORN = 109;
        public static uint EVENT_ONJOIN = 110;
        public static uint EVENT_SETUP_UIROOT = 111;

        public MyEventCmd(uint cmd) : base(cmd) {
        }
    }
}
