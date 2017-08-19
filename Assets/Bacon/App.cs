using Maria;
using XLua;

namespace Bacon {

    class App : Application {

        private AppConfig _config = null;
        
        public App(Maria.Util.App app) : base(app) {
            _config = new AppConfig();
            _ctx = new AppContext(this, _config, _tiSync);
            _dispatcher = _ctx.EventDispatcher;

            _dispatcher.AddCmdEventListener(new EventListenerCmd(EventCmd.EVENT_START_SETUP_ROOT, (EventCmd e) => {
                StartController controller = _ctx.Push<StartController>();
                controller.SetupRoot(e);
            }));
        }

        public Maria.Util.App GApp {
            get { return _app; }
        }

        sealed public override void StartScript() {
            base.StartScript();
            _ctx.StartScript();
        }
    }
}
