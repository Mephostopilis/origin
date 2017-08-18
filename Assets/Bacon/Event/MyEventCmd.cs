using Maria;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bacon.Event {
    class MyEventCmd : EventCmd {

        public static uint EVENT_SETUP_STARTROOT = 51;
        public static uint EVENT_UPdATERES = 52;
        public static uint EVENT_TSETRES = 53;

        public static uint EVENT_ONBORN = 109;
        public static uint EVENT_ONJOIN = 110;
        public static uint EVENT_SETUP_UIROOT = 111;

        // 主界面
        public static uint EVENT_SETUP_MUI = 120;
        public static uint EVENT_MUI_MATCH = 121;
        public static uint EVENT_MUI_CREATE = 122;
        public static uint EVENT_MUI_JOIN = 123;
        public static uint EVENT_MUI_MSG = 124;
        public static uint EVENT_MUI_VIEWMAIL = 125;
        public static uint EVENT_MUI_VIEWEDMAIL = 126;
        public static uint EVENT_MUI_MSGCLOSED = 127;
        public static uint EVENT_MUI_SHOWCREATE = 128;

        public static uint EVENT_MUI_EXITLOGIN = 129;


        // 游戏界面



        // 
        public static uint EVENT_PRESSDOWN = 201;
        public static uint EVENT_PRESSUP = 202;
        public static uint EVENT_PRESSLEFT = 203;
        public static uint EVENT_PRESSRIGHT = 204;



        public static uint EVENT_ANDROID_WX_LOGIN = 500;
        public static uint EVENT_ANDROID_WX_SHARE_APPINFO = 501;
        public static uint EVENT_ANDROID_WX_SHARE_ROOMID = 502;
        public static uint EVENT_SETUP_LOGINPANEL = 503;
        public static uint EVENT_LOGIN = 504;

        public MyEventCmd(Context ctx, uint cmd) : base(ctx, cmd) {
        }
    }
}
