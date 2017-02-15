using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Maria;
using Sproto;

namespace Bacon {
    class MainController : Controller {
        public MainController(Context ctx) : base(ctx) {
            _name = "main";
            EventListenerCmd listener1 = new EventListenerCmd(MyEventCmd.EVENT_MUI_MATCH, OnSendMatch);
            _ctx.EventDispatcher.AddCmdEventListener(listener1);
        }

        public override void Update(float delta) {
            base.Update(delta);
        }

        public override void Enter() {
            base.Enter();
            InitService service = (InitService)_ctx.QueryService(InitService.Name);
            SMActor actor = service.SMActor;
            actor.LoadScene(_name);
        }

        public void OnSendMatch(EventCmd e) {
            if (((AppConfig)_ctx.Config).VERSION == AppConfig.VERSION_TYPE.TEST) {
                _ctx.Push("game");
            } else {
                C2sSprotoType.match.request request = new C2sSprotoType.match.request();
                request.mode = 1;
                _ctx.SendReq<C2sProtocol.match>(C2sProtocol.match.Tag, request);
            }
        }

        public void Match(SprotoTypeBase responseObj) {
            C2sSprotoType.match.response obj = responseObj as C2sSprotoType.match.response;
            UnityEngine.Debug.Assert(obj.errorcode == Errorcode.SUCCESS);
        }

        public SprotoTypeBase OnMatch(SprotoTypeBase requestObj) {
            S2cSprotoType.match.request obj = requestObj as S2cSprotoType.match.request;

            GameService service = _ctx.QueryService(GameService.Name) as GameService;
            service.RoomId = (int)obj.roomid;

            //_ctx.Push("game");

            C2sSprotoType.join.request request = new C2sSprotoType.join.request();
            request.roomid = obj.roomid;
            _ctx.SendReq<C2sProtocol.join>(C2sProtocol.join.Tag, request);

            S2cSprotoType.match.response responseObj = new S2cSprotoType.match.response();
            responseObj.errorcode = Errorcode.SUCCESS;
            return responseObj;
        }
    }
}
