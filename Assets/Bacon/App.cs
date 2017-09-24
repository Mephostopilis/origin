using Maria;
using XLua;

namespace Bacon {

    class App : Application {

        private AppConfig _config = null;
        
        public App() {
            _config = new AppConfig();
            _ctx = new AppContext(this, _config, _tiSync);
            _dispatcher = _ctx.EventDispatcher;
        }

        sealed public override void StartScript() {
            base.StartScript();
            _ctx.StartScript();
        }
    }
}
