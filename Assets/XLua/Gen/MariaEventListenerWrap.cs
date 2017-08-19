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
    public class MariaEventListenerWrap
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			Utils.BeginObjectRegister(typeof(Maria.EventListener), L, translator, 0, 0, 1, 1);
			
			
			
			Utils.RegisterFunc(L, Utils.GETTER_IDX, "Enable", _g_get_Enable);
            
			Utils.RegisterFunc(L, Utils.SETTER_IDX, "Enable", _s_set_Enable);
            
			Utils.EndObjectRegister(typeof(Maria.EventListener), L, translator, null, null,
			    null, null, null);

		    Utils.BeginClassRegister(typeof(Maria.EventListener), L, __CreateInstance, 1, 0, 0);
			
			
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "UnderlyingSystemType", typeof(Maria.EventListener));
			
			
			Utils.EndClassRegister(typeof(Maria.EventListener), L, translator);
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CreateInstance(RealStatePtr L)
        {
            
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			try {
				if(LuaAPI.lua_gettop(L) == 1)
				{
					
					Maria.EventListener __cl_gen_ret = new Maria.EventListener();
					translator.Push(L, __cl_gen_ret);
                    
					return 1;
				}
				
			}
			catch(System.Exception __gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to Maria.EventListener constructor!");
            
        }
        
		
        
		
        
        
        
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_Enable(RealStatePtr L)
        {
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            try {
			
                Maria.EventListener __cl_gen_to_be_invoked = (Maria.EventListener)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, __cl_gen_to_be_invoked.Enable);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_Enable(RealStatePtr L)
        {
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            try {
			
                Maria.EventListener __cl_gen_to_be_invoked = (Maria.EventListener)translator.FastGetCSObj(L, 1);
                __cl_gen_to_be_invoked.Enable = LuaAPI.lua_toboolean(L, 2);
            
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 0;
        }
        
		
		
		
		
    }
}
