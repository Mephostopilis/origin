using Maria;

namespace Bacon.Start {
    class StartController : Controller {
        private StartActor _startActor = null;

        public StartController(Context ctx) : base(ctx) {
            _startActor = new StartActor(ctx, this);

            EventListenerCmd listener2 = new EventListenerCmd(EventCmd.EVENT_START_SETUP_ROOT, SetupRoot);
            _ctx.EventDispatcher.AddCmdEventListener(listener2);
        }

        public override void OnEnter() {
            base.OnEnter();
            // 一般是加载场景在这里
        }

        public override void OnExit() {
            base.OnExit();
        }

        public void SetupRoot(EventCmd e) {
            _startActor.Go = e.Orgin;
            _startActor.UpdateRes();
        }

    }
}
