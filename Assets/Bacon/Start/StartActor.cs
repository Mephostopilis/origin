using Bacon.Event;
using Bacon.GL.Start;
using Bacon.Login;
using Maria;

namespace Bacon.Start
{
    class StartActor : Actor {

        public StartActor(Context ctx, Controller controller) : base(ctx, controller) {
            EventListenerCmd listener1 = new EventListenerCmd(EventCmd.EVENT_UPDATERES_BEGIN, OnEventUpdateResBegin);
            _ctx.EventDispatcher.AddCmdEventListener(listener1);

            EventListenerCmd listener2 = new EventListenerCmd(EventCmd.EVENT_UPDATERES_END, OnEventUpdateResEnd);
            _ctx.EventDispatcher.AddCmdEventListener(listener2);
        }

        public void UpdateRes() {
            if (((AppConfig)_ctx.Config).UpdateRes) {
                _ctx.EnqueueRenderQueue(RenderUpdateRes);
            } else {
                _ctx.EnqueueRenderQueue(RenderTestRes);
            }
        }

        private void RenderUpdateRes() {
            _go.GetComponent<StartBehaviour>().UpdateRes();
        }

        private void RenderTestRes() {
            _go.GetComponent<StartBehaviour>().TestRes();
        }


        private void OnEventUpdateResBegin(EventCmd e) { }

        private void OnEventUpdateResEnd(EventCmd e) {
            _ctx.Push(typeof(LoginController));
        }

    }
}
