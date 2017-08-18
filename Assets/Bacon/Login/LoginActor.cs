using Bacon.Event;
using Bacon.GL.Login.UI;
using Maria;

namespace Bacon.Login {
    public class LoginActor : Actor {

        private bool _commit = false;
        private string _tips = string.Empty;

        public LoginActor(Context ctx, Controller controller) : base(ctx, controller) {
            EventListenerCmd listener1 = new EventListenerCmd(MyEventCmd.EVENT_LOGIN, Login);
            _ctx.EventDispatcher.AddCmdEventListener(listener1);

            EventListenerCmd listener2 = new EventListenerCmd(MyEventCmd.EVENT_SETUP_LOGINPANEL, SetupLoginPanel);
            _ctx.EventDispatcher.AddCmdEventListener(listener2);
        }

        public void SetupLoginPanel(EventCmd e) {
            _go = e.Orgin;
            EnableCommitOk();
        }

        public void Login(EventCmd e) {
            Message msg = e.Msg;
            string server = (string)msg["server"];
            string username = (string)msg["username"];
            string password = (string)msg["password"];
            LoginController controller = _controller as LoginController;
            controller.LoginAuth(server, username, password);
            _commit = true;
            EnableCommitOk();
        }

        public void EnableCommitOk() {
            _ctx.EnqueueRenderQueue(RenderEnableCommitOk);
        }

        private void RenderEnableCommitOk() {
            var com = _go.GetComponent<LoginPanel>();
            com.EnableCommitOk(_commit);
        }

        public void ShowTips(string tips) {
            _tips = tips;
            _ctx.EnqueueRenderQueue(RenderShowTips);
        }

        public void RenderShowTips() {
            var com = _go.GetComponent<LoginPanel>();
            com.ShowTips(_tips);
        }

    }
}
