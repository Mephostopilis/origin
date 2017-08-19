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
    public class MariaEventListenerCustomWrap
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			Utils.BeginObjectRegister(typeof(Maria.EventListenerCustom), L, translator, 0, 0, 3, 0);
			
			
			
			Utils.RegisterFunc(L, Utils.GETTER_IDX, "Name", _g_get_Name);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "Handler", _g_get_Handler);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "Addition", _g_get_Addition);
            
			
			Utils.EndObjectRegister(typeof(Maria.EventListenerCustom), L, translator, null, null,
			    null, null, null);

		    Utils.BeginClassRegister(typeof(Maria.EventListenerCustom), L, __CreateInstance, 1, 0, 0);
			
			
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "UnderlyingSystemType", typeof(Maria.EventListenerCustom));
			
			
			Utils.EndClassRegister(typeof(Maria.EventListenerCustom), L, translator);
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CreateInstance(RealStatePtr L)
        {
            
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			try {
				if(LuaAPI.lua_gettop(L) == 4 && (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING) && translator.Assignable<Maria.EventListenerCustom.OnEventCustomHandler>(L, 3) && translator.Assignable<object>(L, 4))
				{
					string name = LuaAPI.lua_tostring(L, 2);
					Maria.EventListenerCustom.OnEventCustomHandler callback = translator.GetDelegate<Maria.EventListenerCustom.OnEventCustomHandler>(L, 3);
					object addition = translator.GetObject(L, 4, typeof(object));
					
					Maria.EventListenerCustom __cl_gen_ret = new Maria.EventListenerCustom(name, callback, addition);
					translator.Push(L, __cl_gen_ret);
                    
					return 1;
				}
				
			}
			catch(System.Exception __gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to Maria.EventListenerCustom constructor!");
            
        }
        
		
        
		
        
        
        
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_Name(RealStatePtr L)
        {
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            try {
			
                Maria.EventListenerCustom __cl_gen_to_be_invoked = (Maria.EventListenerCustom)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushstring(L, __cl_gen_to_be_invoked.Name);
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
			
                Maria.EventListenerCustom __cl_gen_to_be_invoked = (Maria.EventListenerCustom)translator.FastGetCSObj(L, 1);
                translator.Push(L, __cl_gen_to_be_invoked.Handler);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_Addition(RealStatePtr L)
        {
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            try {
			
                Maria.EventListenerCustom __cl_gen_to_be_invoked = (Maria.EventListenerCustom)translator.FastGetCSObj(L, 1);
                translator.PushAny(L, __cl_gen_to_be_invoked.Addition);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        
        
		
		
		
		
    }
}
