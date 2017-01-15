using Maria;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bacon {
    class MyEventCmd : EventCmd {





        public static uint EVENT_SETUP_STARTROOT = 108;

        public static uint EVENT_ONBORN = 109;
        public static uint EVENT_ONJOIN = 110;
        public static uint EVENT_SETUP_UIROOT = 111;

        public static uint EVENT_MUI_MATCH = 112;

        // 游戏场景
        public static uint EVENT_SETUPG_TANK = 201;
        public static uint EVENT_SETUPG_SCENE = 202;
        public static uint EVENT_SETUPG_VIEW = 203;
        public static uint EVENT_SETUPG_MAP = 204;

        public static uint EVENT_PRESSUP = 211;
        public static uint EVENT_PRESSRIGHT = 212;
        public static uint EVENT_PRESSDOWN = 213;
        public static uint EVENT_PRESSLEFT = 214;

        public MyEventCmd(uint cmd) : base(cmd) {
        }
    }
}
