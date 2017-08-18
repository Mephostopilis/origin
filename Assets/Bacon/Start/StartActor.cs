using Bacon.Event;
using Bacon.GL.Common;
using Bacon.Login;
using Maria;

namespace Bacon {
    class StartActor : Actor {

        public StartActor(Context ctx, Controller controller) : base(ctx, controller) {
            EventListenerCmd listener1 = new EventListenerCmd(MyEventCmd.EVENT_SETUP_STARTROOT, SetupStartRoot);
            _ctx.EventDispatcher.AddCmdEventListener(listener1);

            EventListenerCmd listener2 = new EventListenerCmd(MyEventCmd.EVENT_UPdATERES, CountdownCb);
            _ctx.EventDispatcher.AddCmdEventListener(listener2);
        }

        private void SetupStartRoot(EventCmd e) {
            _go = e.Orgin;
            if (((AppConfig)_ctx.Config).UpdateRes) {
                _ctx.EnqueueRenderQueue(RenderUpdateRes);
            } else {
                _ctx.EnqueueRenderQueue(RenderTestRes);
            }
        }

        public void RenderUpdateRes() {
            _go.GetComponent<StartBehaviour>().UpdateRes();
        }

        public void RenderTestRes() {
            _go.GetComponent<StartBehaviour>().TestRes();
        }

        private void CountdownCb(EventCmd e) {
            _ctx.Push(typeof(LoginController));
        }

    }
}
