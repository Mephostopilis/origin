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
    public class MariaEventDispatcherWrap
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			Utils.BeginObjectRegister(typeof(Maria.EventDispatcher), L, translator, 0, 7, 0, 0);
			
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "AddCmdEventListener", _m_AddCmdEventListener);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "AddCustomEventListener", _m_AddCustomEventListener);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "DispatchCmdEvent", _m_DispatchCmdEvent);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "FireCustomEvent", _m_FireCustomEvent);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "SubCustomEventListener", _m_SubCustomEventListener);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "UnsubCustomEventListener", _m_UnsubCustomEventListener);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "PubCustomEvent", _m_PubCustomEvent);
			
			
			
			
			Utils.EndObjectRegister(typeof(Maria.EventDispatcher), L, translator, null, null,
			    null, null, null);

		    Utils.BeginClassRegister(typeof(Maria.EventDispatcher), L, __CreateInstance, 1, 0, 0);
			
			
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "UnderlyingSystemType", typeof(Maria.EventDispatcher));
			
			
			Utils.EndClassRegister(typeof(Maria.EventDispatcher), L, translator);
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CreateInstance(RealStatePtr L)
        {
            
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			try {
				if(LuaAPI.lua_gettop(L) == 2 && translator.Assignable<Maria.Context>(L, 2))
				{
					Maria.Context ctx = (Maria.Context)translator.GetObject(L, 2, typeof(Maria.Context));
					
					Maria.EventDispatcher __cl_gen_ret = new Maria.EventDispatcher(ctx);
					translator.Push(L, __cl_gen_ret);
                    
					return 1;
				}
				
			}
			catch(System.Exception __gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to Maria.EventDispatcher constructor!");
            
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_AddCmdEventListener(RealStatePtr L)
        {
            
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            Maria.EventDispatcher __cl_gen_to_be_invoked = (Maria.EventDispatcher)translator.FastGetCSObj(L, 1);
            
            
            try {
                
                {
                    Maria.EventListenerCmd listener = (Maria.EventListenerCmd)translator.GetObject(L, 2, typeof(Maria.EventListenerCmd));
                    
                    __cl_gen_to_be_invoked.AddCmdEventListener( listener );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_AddCustomEventListener(RealStatePtr L)
        {
            
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            Maria.EventDispatcher __cl_gen_to_be_invoked = (Maria.EventDispatcher)translator.FastGetCSObj(L, 1);
            
            
            try {
                
                {
                    string eventName = LuaAPI.lua_tostring(L, 2);
                    Maria.EventListenerCustom.OnEventCustomHandler callback = translator.GetDelegate<Maria.EventListenerCustom.OnEventCustomHandler>(L, 3);
                    object addition = translator.GetObject(L, 4, typeof(object));
                    
                        Maria.EventListenerCustom __cl_gen_ret = __cl_gen_to_be_invoked.AddCustomEventListener( eventName, callback, addition );
                        translator.Push(L, __cl_gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_DispatchCmdEvent(RealStatePtr L)
        {
            
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            Maria.EventDispatcher __cl_gen_to_be_invoked = (Maria.EventDispatcher)translator.FastGetCSObj(L, 1);
            
            
            try {
                
                {
                    Maria.Command cmd = (Maria.Command)translator.GetObject(L, 2, typeof(Maria.Command));
                    
                    __cl_gen_to_be_invoked.DispatchCmdEvent( cmd );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_FireCustomEvent(RealStatePtr L)
        {
            
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            Maria.EventDispatcher __cl_gen_to_be_invoked = (Maria.EventDispatcher)translator.FastGetCSObj(L, 1);
            
            
            try {
                
                {
                    string eventName = LuaAPI.lua_tostring(L, 2);
                    object ud = translator.GetObject(L, 3, typeof(object));
                    
                    __cl_gen_to_be_invoked.FireCustomEvent( eventName, ud );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SubCustomEventListener(RealStatePtr L)
        {
            
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            Maria.EventDispatcher __cl_gen_to_be_invoked = (Maria.EventDispatcher)translator.FastGetCSObj(L, 1);
            
            
            try {
                
                {
                    string eventName = LuaAPI.lua_tostring(L, 2);
                    Maria.EventListenerCustom.OnEventCustomHandler callback = translator.GetDelegate<Maria.EventListenerCustom.OnEventCustomHandler>(L, 3);
                    object addition = translator.GetObject(L, 4, typeof(object));
                    
                        Maria.EventListenerCustom __cl_gen_ret = __cl_gen_to_be_invoked.SubCustomEventListener( eventName, callback, addition );
                        translator.Push(L, __cl_gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_UnsubCustomEventListener(RealStatePtr L)
        {
            
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            Maria.EventDispatcher __cl_gen_to_be_invoked = (Maria.EventDispatcher)translator.FastGetCSObj(L, 1);
            
            
            try {
                
                {
                    Maria.EventListenerCustom listener = (Maria.EventListenerCustom)translator.GetObject(L, 2, typeof(Maria.EventListenerCustom));
                    
                        bool __cl_gen_ret = __cl_gen_to_be_invoked.UnsubCustomEventListener( listener );
                        LuaAPI.lua_pushboolean(L, __cl_gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_PubCustomEvent(RealStatePtr L)
        {
            
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            Maria.EventDispatcher __cl_gen_to_be_invoked = (Maria.EventDispatcher)translator.FastGetCSObj(L, 1);
            
            
            try {
                
                {
                    string eventName = LuaAPI.lua_tostring(L, 2);
                    object ud = translator.GetObject(L, 3, typeof(object));
                    
                    __cl_gen_to_be_invoked.PubCustomEvent( eventName, ud );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        
        
        
        
        
		
		
		
		
    }
}
