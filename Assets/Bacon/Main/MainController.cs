using Maria;
using Maria.Util;
using Sproto;
using UnityEngine;
using XLua;
using Bacon.Service;
using Bacon.Game;
using Bacon.Helper;
using Bacon.Event;
using Maria.Res;

namespace Bacon
{

    [Hotfix]
    [LuaCallCSharp]
    class MainController : Controller {
        
        private GameObject _uiroot = null;
        private GameObject _role = null;
        private GameObject _waiting = null;

        private long _curmsgid;
        private long _roomid;

        
        public MainController(Context ctx) : base(ctx) {
            _name = "main";

            //EventListenerCmd listener1 = new EventListenerCmd(MyEventCmd.EVENT_MUI_CREATE, OnSendCreate);
            //_ctx.EventDispatcher.AddCmdEventListener(listener1);

            //EventListenerCmd listener2 = new EventListenerCmd(MyEventCmd.EVENT_MUI_JOIN, OnSendJoin);
            //_ctx.EventDispatcher.AddCmdEventListener(listener2);

            //EventListenerCmd listener3 = new EventListenerCmd(MyEventCmd.EVENT_MUI_MSG, OnSendMsg);
            //_ctx.EventDispatcher.AddCmdEventListener(listener3);

            //EventListenerCmd listener4 = new EventListenerCmd(MyEventCmd.EVENT_MUI_VIEWMAIL, OnSendViewMail);
            //_ctx.EventDispatcher.AddCmdEventListener(listener4);

            //EventListenerCmd listener5 = new EventListenerCmd(MyEventCmd.EVENT_MUI_VIEWEDMAIL, OnViewedMail);
            //_ctx.EventDispatcher.AddCmdEventListener(listener5);

            //EventListenerCmd listener6 = new EventListenerCmd(MyEventCmd.EVENT_MUI_MSGCLOSED, OnMsgClosed);
            //_ctx.EventDispatcher.AddCmdEventListener(listener6);

            //EventListenerCmd listener7 = new EventListenerCmd(MyEventCmd.EVENT_SETUP_MUI, SetupUI);
            //_ctx.EventDispatcher.AddCmdEventListener(listener7);

            //EventListenerCmd listener8 = new EventListenerCmd(MyEventCmd.EVENT_MUI_SHOWCREATE, OnShowCreate);
            //_ctx.EventDispatcher.AddCmdEventListener(listener8);

            //EventListenerCmd listener9 = new EventListenerCmd(MyEventCmd.EVENT_MUI_EXITLOGIN, OnLogout);
            //_ctx.EventDispatcher.AddCmdEventListener(listener9);

        }

        public override void OnEnter() {
            base.OnEnter();
            InitService service = _ctx.QueryService<InitService>(InitService.Name);
            SMActor actor = service.SMActor;
            actor.LoadScene(_name);
        }

        public override void OnExit() {
            base.OnExit();
            _ctx.EnqueueRenderQueue(RenderExit);
        }

        public override void OnGateAuthed(int code) {
            base.OnGateAuthed(code);
            _ctx.EnqueueRenderQueue(RenderCancelWaiting);
        }

        public override void OnGateDisconnected() {
            base.OnGateDisconnected();
            UnityEngine.Debug.LogFormat("main controller gate diconnented, start connect ...");
            _ctx.EnqueueRenderQueue(RenderWaiting);
        }

        public override void Logout() {
            _ctx.Pop();
        }

        #region event
        public void SetupUI(EventCmd e) {
            _uiroot = e.Orgin;

            _ctx.EnqueueRenderQueue(RenderSetupUI);
        }

        private void OnShowCreate(EventCmd e) {
            _ctx.EnqueueRenderQueue(RenderShowCreate);
        }

        public void OnSendMatch(EventCmd e) {
            if (((AppConfig)_ctx.Config).VTYPE == AppConfig.VERSION_TYPE.TEST_WIN) {
                _ctx.Push(typeof(GameController));
            } else {
                C2sSprotoType.match.request request = new C2sSprotoType.match.request();
                request.mode = 1;
                _ctx.SendReq<C2sProtocol.match>(C2sProtocol.match.Tag, request);
            }
        }

        public void OnSendMsg(EventCmd e) {
            //List<long> mailids = new List<long>();
            //if (_service.SysInBox.Count > 0) {
            //    foreach (var item in _service.SysInBox) {
            //        mailids.Add(item.Id);
            //    }
            //}
            //C2sSprotoType.syncsysmail.request request = new C2sSprotoType.syncsysmail.request();
            //request.all = mailids;
            //_ctx.SendReq<C2sProtocol.syncsysmail>(C2sProtocol.syncsysmail.Tag, request);
        }

        public void OnSendViewMail(EventCmd e) {
            
            _ctx.EnqueueRenderQueue(RenderViewMail);
            //var mailwnd = com._MailWnd.GetComponent<MailWnd>();
            //if ((MsgItem.Type)e.Msg["type"] == MsgItem.Type.Sys) {
            //    Sysmail mail = _service.SysInBox.GetMail((long)e.Msg["id"]);
            //    mailwnd.ShowMailInfo()
            //}

            //_service.SysInBox
            //mailwnd.ShowMailInfo()
            //C2sSprotoType.syncsysmail.request request = new C2sSprotoType.syncsysmail.request();
            //request.all = mailids;
            //_ctx.SendReq<C2sProtocol.syncsysmail>(C2sProtocol.syncsysmail.Tag, request);
        }

        public void OnViewedMail(EventCmd e) {
            //MsgItem.Type type = e.Msg.GetField<MsgItem.Type>("type");
            //long id = e.Msg.GetField<long>("id");
            //_curmsgid = id;
            //_curtype = type;
            //Sysmail mail = _service.SysInBox.GetMail(id);
            //_service.SysInBox.Remove(mail);

            //C2sSprotoType.viewedsysmail.request request = new C2sSprotoType.viewedsysmail.request();
            //request.mailid = id;
            //_ctx.SendReq<C2sProtocol.viewedsysmail>(C2sProtocol.viewedsysmail.Tag, request);

            //_ctx.EnqueueRenderQueue(RenderSyncSysMail);
        }

        //public void OnSendJoin(EventCmd e) {
        //    int roomid = (int)e.Msg["roomid"];
        //    GameService service = _ctx.QueryService<GameService>(GameService.Name);

        //    C2sSprotoType.join.request request = new C2sSprotoType.join.request();
        //    request.roomid = roomid;
        //    _ctx.SendReq<C2sProtocol.join>(C2sProtocol.join.Tag, request);
        //}

        public void OnMsgClosed(EventCmd e) {
            _ctx.EnqueueRenderQueue(RenderFetchSysmail);
        }

        public void OnSendCreate(EventCmd e) {
           
        }

        public void OnSendRecords(EventCmd e) {

        }

        public void OnLogout(EventCmd e) {
        }
        #endregion

        #region request
        public SprotoTypeBase OnReqMatch(SprotoTypeBase requestObj) {
            S2cSprotoType.match.request obj = requestObj as S2cSprotoType.match.request;

            _ctx.U.GetModule<Bacon.Modules.BattleScene>().RoomId = obj.roomid;

            _ctx.Push<GameController>();

            S2cSprotoType.match.response responseObj = new S2cSprotoType.match.response();
            responseObj.errorcode = Errorcode.SUCCESS;
            return responseObj;
        }
        #endregion

        #region response
        //public void First(SprotoTypeBase responseObj) {
        //    EntityMgr mgr = ((AppContext)_ctx).GetEntityMgr();
        //    mgr.MyEntity.GetComponent<Bacon.Model.UComUser>().First(responseObj);
        //}

        //public void FetchSysmail(SprotoTypeBase responseObj) {
            
        //}

        //public void FetchRecords(SprotoTypeBase responseObj) {

        //    AppContext ctx = _ctx as AppContext;
        //    EntityMgr mgr = ctx.GetEntityMgr();
        //    Entity entity = mgr.MyEntity;
        //    entity.GetComponent<UComRecordMgr>().FetchRecords(responseObj);
        //}

        public void OnRspMatch(SprotoTypeBase responseObj) {
            C2sSprotoType.match.response obj = responseObj as C2sSprotoType.match.response;
            UnityEngine.Debug.Assert(obj.errorcode == Errorcode.SUCCESS);
        }

        public void SyncSysmail(SprotoTypeBase responseObj) {
            //C2sSprotoType.syncsysmail.response obj = responseObj as C2sSprotoType.syncsysmail.response;
            //_service.SysInBox.Clear();
            //if (obj.inbox.Count > 0) {
            //    for (int i = 0; i < obj.inbox.Count; i++) {
            //        var mail = _service.SysInBox.CreateMail();
            //        mail.Id = obj.inbox[i].id;
            //        mail.Title = obj.inbox[i].title;
            //        mail.DateTime = obj.inbox[i].datetime;
            //        mail.Content = obj.inbox[i].content;
            //        _service.SysInBox.Add(mail);
            //    }
            //}
            //// 显示邮件
            //_ctx.EnqueueRenderQueue(RenderSyncSysMail);
        }

        public void Records(SprotoTypeBase responseObj) {

        }

        public void Record(SprotoTypeBase responseObj) {

        }

        public void Logout(SprotoTypeBase responseObj) {
            _ctx.Pop();
        }

        //public void Adver(SprotoTypeBase responseObj) {
        //    AppContext ctx = _ctx as AppContext;
        //    ctx.GetBoardMgr().Adver(responseObj);
        //}

        //public void Board(SprotoTypeBase responseObj) {
        //    AppContext ctx = _ctx as AppContext;
        //    ctx.GetBoardMgr().Board(responseObj);
        //}
        #endregion

        #region render

        private void RenderExit() {
            SoundMgr.current.StopMusic();
        }

        public void RenderSetupUI() {
            ABLoader.current.LoadAssetAsync<AudioClip>("Sound/MusicEx", "MusicEx_Welcome", (AudioClip clip) => {
                SoundMgr.current.PlayMusic(clip);
            });
        }

        public void RenderFetchSysmail() {
            
        }

        public void RenderShowCreate() {
            AppContext ctx = _ctx as AppContext;
            //long num = ctx.GetEntityMgr().MyEntity.GetComponent<UComUser>().RCard;
            //_uiroot.GetComponent<UIRoot>().ShowCreate(num);
        }

        public void RenderSyncSysMail() {
           
        }

        public void RenderViewMail() {
            //if (_curtype == MsgItem.Type.Sys) {
            //    Sysmail mail = _service.SysInBox.GetMail(_curmsgid);
            //    var com = _uiroot.GetComponent<MUIRoot>();
            //    var mailwnd = com._MailWnd.GetComponent<MailWnd>();
            //    mailwnd._InfoPage.GetComponent<MsgItemInfo>().Show(_curtype, _curmsgid, mail.Title, mail.Content);
            //}
        }

        public void RenderViewedMail() { }

        public void RenderCancelWaiting() {
            if (_uiroot && _waiting) {
                _waiting.SetActive(false);
            }
        }

        public void RenderWaiting() {
            if (_uiroot) {
                //MUIRoot com = _uiroot.GetComponent<global::MUIRoot>();
                //ABLoader.current.LoadAssetAsync<GameObject>("Prefabs/Common", "Waiting", (GameObject go) => {
                //    _waiting = go;
                //    go.transform.SetParent(com._Extra.transform);
                //});
            }
        }

        public void RenderShowTips() {
            //_uiroot.GetComponent<MUIRoot>().ShowTips(_tipscontent);
        }

        //public void RenderAdver() {
        //    BoardMgr mgr = ((AppContext)_ctx).GetBoardMgr();

        //    UIRoot muiroot = _uiroot.GetComponent<UIRoot>();
        //    muiroot.SetAdver(mgr.AdverMsg);
        //}

        //public void RenderBoard() {
        //    BoardMgr mgr = ((AppContext)_ctx).GetBoardMgr();

        //    UIRoot muiroot = _uiroot.GetComponent<UIRoot>();
        //    muiroot.SetBoard(mgr.BoardMsg);
        //}

        public void RenderFirst() {

        }
        #endregion
    }
}
