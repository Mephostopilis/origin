using Maria;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bacon {
    class App : Application {
        private AppConfig _config = null;
        
        public App() {
            _config = new AppConfig();
            _ctx = new AppContext(this, _config);
            _dispatcher = _ctx.EventDispatcher;
        }
    }
}
