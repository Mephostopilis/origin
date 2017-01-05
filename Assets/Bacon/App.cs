using Maria;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bacon {
    class App : Application {
        private AppConfig _config = null;
        private global::App _app = null;
        public App(global::App app) {
            _app = app;

            _config = new AppConfig();
            _ctx = new AppContext(this, _config, _tiSync);
            _dispatcher = _ctx.EventDispatcher;
        }

        public global::App GApp {
            get { return _app; }
        }
    }
}
