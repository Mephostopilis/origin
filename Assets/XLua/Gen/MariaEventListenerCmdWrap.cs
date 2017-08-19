#if USE_UNI_LUA
using LuaAPI = UniLua.Lua;
using RealStatePtr = UniLua.ILuaState;
using LuaCSFunction = UniLua.CSharpFunctionDelegate;
#else
using LuaAPI = XLua.LuaDLL.Lua;
using RealStatePtr = System.IntPtr;
using LuaCSFunction = XLua.LuaDLL.lua_CSFunction;
#endif

using XLua;
using System.Collections.Generic;


namespace XLua.CSObjectWrap
{
    using Utils = XLua.Utils;
    public class MariaEventListenerCmdWrap
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			Utils.BeginObjectRegister(typeof(Maria.EventListenerCmd), L, translator, 0, 0, 2, 0);
			
			
			
			Utils.RegisterFunc(L, Utils.GETTER_IDX, "Cmd", _g_get_Cmd);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "Handler", _g_get_Handler);
            
			
			Utils.EndObjectRegister(typeof(Maria.EventListenerCmd), L, translator, null, null,
			    null, null, null);

		    Utils.BeginClassRegister(typeof(Maria.EventListenerCmd), L, __CreateInstance, 1, 0, 0);
			
			
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "UnderlyingSystemType", typeof(Maria.EventListenerCmd));
			
			
			Utils.EndClassRegister(typeof(Maria.EventListenerCmd), L, translator);
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CreateInstance(RealStatePtr L)
        {
            
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			try {
				if(LuaAPI.lua_gettop(L) == 3 && LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2) && translator.Assignable<Maria.EventListenerCmd.OnEventCmdHandler>(L, 3))
				{
					uint cmd = LuaAPI.xlua_touint(L, 2);
					Maria.EventListenerCmd.OnEventCmdHandler callback = translator.GetDelegate<Maria.EventListenerCmd.OnEventCmdHandler>(L, 3);
					
					Maria.EventListenerCmd __cl_gen_ret = new Maria.EventListenerCmd(cmd, callback);
					translator.Push(L, __cl_gen_ret);
                    
					return 1;
				}
				
			}
			catch(System.Exception __gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to Maria.EventListenerCmd constructor!");
            
        }
        
		
        
		
        
        
        
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_Cmd(RealStatePtr L)
        {
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            try {
			
                Maria.EventListenerCmd __cl_gen_to_be_invoked = (Maria.EventListenerCmd)translator.FastGetCSObj(L, 1);
                LuaAPI.xlua_pushuint(L, __cl_gen_to_be_invoked.Cmd);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_Handler(RealStatePtr L)
        {
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            try {
			
                Maria.EventListenerCmd __cl_gen_to_be_invoked = (Maria.EventListenerCmd)translator.FastGetCSObj(L, 1);
                translator.Push(L, __cl_gen_to_be_invoked.Handler);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        
        
		
		
		
		
    }
}
