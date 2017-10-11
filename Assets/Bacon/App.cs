using Maria;
using XLua;

namespace Bacon {

    class App : Application {

        private AppConfig _config = null;

        public App(XLua.LuaEnv luaEnv) : base(luaEnv) {
            _config = new AppConfig();
            _ctx = Maria.Lua.LuaPool.Instance.Create<AppContext>(this, _config, _tiSync);
            _dispatcher = _ctx.EventDispatcher;
        }

    }
}
