using Maria;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bacon.Event {
    class MyEventCmd : EventCmd {

        // 1001 ~ 1050
        public static uint EVENT_APP_START = 1001;

        // 开始界面 1051 ~ 1100
        public static uint EVENT_SETUP_STARTROOT = 1051;
        public static uint EVENT_UPdATERES = 1052;
        public static uint EVENT_TSETRES = 1053;

        public static uint EVENT_ONBORN = 1109;
        public static uint EVENT_ONJOIN = 1110;
        public static uint EVENT_SETUP_UIROOT = 1111;

        // 主界面 1101 ~ 1200
        public static uint EVENT_SETUP_MUI = 1120;
        public static uint EVENT_MUI_MATCH = 1121;
        public static uint EVENT_MUI_CREATE = 1122;
        public static uint EVENT_MUI_JOIN = 1123;
        public static uint EVENT_MUI_MSG = 1124;
        public static uint EVENT_MUI_VIEWMAIL = 1125;
        public static uint EVENT_MUI_VIEWEDMAIL = 1126;
        public static uint EVENT_MUI_MSGCLOSED = 1127;
        public static uint EVENT_MUI_SHOWCREATE = 1128;

        public static uint EVENT_MUI_EXITLOGIN = 1129;


        // 游戏界面 1201 ~ 2000
        public static uint EVENT_PRESSDOWN = 1201;
        public static uint EVENT_PRESSUP = 1202;
        public static uint EVENT_PRESSLEFT = 1203;
        public static uint EVENT_PRESSRIGHT = 1204;

        public static uint EVENT_SETUP_MAP = 1205;

        public static uint EVENT_ANDROID_WX_LOGIN = 1500;
        public static uint EVENT_ANDROID_WX_SHARE_APPINFO = 1501;
        public static uint EVENT_ANDROID_WX_SHARE_ROOMID = 1502;
        public static uint EVENT_SETUP_LOGINPANEL = 1503;
        public static uint EVENT_LOGIN = 1504;



        public MyEventCmd(Context ctx, uint cmd) : base(ctx, cmd) {
        }
    }
}
