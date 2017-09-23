using Maria;
using XLua;

namespace Bacon {

    class App : Application {

        private AppConfig _config = null;
        
        public App() {
            _config = new AppConfig();
            _ctx = new AppContext(this, _config, _tiSync);
            _dispatcher = _ctx.EventDispatcher;

            _dispatcher.AddCmdEventListener(new EventListenerCmd(EventCmd.EVENT_START_SETUP_ROOT, (EventCmd e) => {
                StartController controller = _ctx.Push<StartController>();
                controller.SetupRoot(e);
            }));
        }

        sealed public override void StartScript() {
            base.StartScript();
            _ctx.StartScript();
        }
    }
}
