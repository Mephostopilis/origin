using Maria;
using Bacon.Service;
using Bacon.Helper;

namespace Bacon.Login
{
    class LoginController : Controller {

        private LoginActor _loginActor;
        private int _reconnected = 0;

        public LoginController(Context ctx) : base(ctx) {
            _name = "login";
            _loginActor = new LoginActor(_ctx, this);
        }

        public override void OnEnter() {
            base.OnEnter();

            InitService service = _ctx.QueryService<InitService>(InitService.Name);
            if (service != null) {
                SMActor actor = service.SMActor;
                actor.LoadScene("login");
            }
        }

        public override void OnExit() {
            base.OnExit();
        }

        public void LoginAuth(string server, string username, string password) {
            if (((AppConfig)_ctx.Config).VTYPE == AppConfig.VERSION_TYPE.DEV) {
                _ctx.Push(typeof(MainController));
            } else {
                _ctx.LoginAuth(server, username, password);
                _reconnected = 0;
            }
        }

        public override void OnLoginAuthed(int code, byte[] secret, string dummy) {
            if (code == 200) {
                int uid = _ctx.U.Uid;
                int subid = _ctx.U.Subid;

                AppContext ctx = _ctx as AppContext;
                //EntityMgr mgr = ctx.GetEntityMgr();
                //UEntity e = new UEntity(_ctx, (uint)uid);
                //mgr.AddEntity(e);
                //mgr.MyEntity = e;

                _loginActor.ShowTips("提示：成功登陆");
            } else if (code == 401) {
                _loginActor.EnableCommitOk();
                _loginActor.ShowTips("错误吗：401 提示：没有授权成功");
            } else if (code == 403) {
                _loginActor.EnableCommitOk();
                _loginActor.ShowTips("错误吗：403 提示：登陆不陈宫");
            }
        }

        public override void OnLoginConnected(bool connected) {
            if (!connected) { // 没有链接成功
                if (_reconnected > 7) {
                    _loginActor.ShowTips("提示:服务器正在维护中");
                } else {
                    ++_reconnected;
                    _loginActor.ShowTips(string.Format("重新第{0}次练级服务器", _reconnected));
                }
            }
        }

        public override void OnLoginDisconnected() {

        }

        public override void OnGateAuthed(int code) {
            base.OnGateAuthed(code);
            if (code == 200) {
                _ctx.Push(typeof(MainController));
            } else {
                _loginActor.EnableCommitOk();
            }
        }

        public override void OnGateConnected(bool connected) {
            if (!connected) {
                _loginActor.ShowTips("提示:服务器正在维护中");
            }
        }

        public override void OnGateDisconnected() {
            base.OnGateDisconnected();
        }
    }
}
