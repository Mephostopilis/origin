using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Maria {
    public class LoginActor : Actor {
        public LoginActor(Context ctx, Controller controller) : base(ctx, controller) {
            EventListenerCmd listener1 = new EventListenerCmd(EventCmd.EVENT_LOGIN, Login);
            _ctx.EventDispatcher.AddCmdEventListener(listener1);

            EventListenerCmd listener2 = new EventListenerCmd(EventCmd.EVENT_SETUP_LOGINPANEL, SetupLoginPanel);
            _ctx.EventDispatcher.AddCmdEventListener(listener2);
        }

        public void SetupLoginPanel(EventCmd e) {
            _go = e.Orgin;
        }

        public void Login(EventCmd e) {
            string str = "login controller login.";
            Debug.Log(str);

            Message msg = e.Msg;
            string server = msg["server"].ToString();
            string username = msg["username"].ToString();
            string password = msg["password"].ToString();
            LoginController controller = _controller as LoginController;
            controller.LoginAuth(server, username, password);
        }

        public void EnableCommitOk() {
            _ctx.EnqueueRenderQueue(RenderEnableCommitOk);
        }

        private void RenderEnableCommitOk() {
            var com = _go.GetComponent<LoginPanelBehaviour>();
            com.EnableCommitOk();
        }
    }
}
