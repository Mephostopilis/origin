using System.Runtime.InteropServices;
using XLua;

namespace Maria.Lua {
   
    public partial class BuildInInit {

        const string LUADLL = "xlua";

        [DllImport(LUADLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern int luaopen_cjson(System.IntPtr L);

        [MonoPInvokeCallback(typeof(XLua.LuaDLL.lua_CSFunction))]
        public static int LoadCJson(System.IntPtr L) {
            return luaopen_cjson(L);
        }

        [DllImport(LUADLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern int luaopen_lpeg(System.IntPtr L);

        [MonoPInvokeCallback(typeof(XLua.LuaDLL.lua_CSFunction))]
        public static int LoadLpeg(System.IntPtr L) {
            return luaopen_lpeg(L);
        }

        [DllImport(LUADLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern int luaopen_sproto_core(System.IntPtr L);

        [MonoPInvokeCallback(typeof(XLua.LuaDLL.lua_CSFunction))]
        public static int LoadSprotoCore(System.IntPtr L) {
            return luaopen_sproto_core(L);
        }

    }
}