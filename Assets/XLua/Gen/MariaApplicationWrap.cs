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
    public class MariaApplicationWrap
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			Utils.BeginObjectRegister(typeof(Maria.Application), L, translator, 0, 8, 1, 0);
			
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "Enqueue", _m_Enqueue);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "EnqueueRenderQueue", _m_EnqueueRenderQueue);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "Update", _m_Update);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "StartUpdateRes", _m_StartUpdateRes);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "StartScript", _m_StartScript);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "OnApplicationFocus", _m_OnApplicationFocus);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "OnApplicationPause", _m_OnApplicationPause);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "OnApplicationQuit", _m_OnApplicationQuit);
			
			
			Utils.RegisterFunc(L, Utils.GETTER_IDX, "LuaEnv", _g_get_LuaEnv);
            
			
			Utils.EndObjectRegister(typeof(Maria.Application), L, translator, null, null,
			    null, null, null);

		    Utils.BeginClassRegister(typeof(Maria.Application), L, __CreateInstance, 1, 0, 0);
			
			
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "UnderlyingSystemType", typeof(Maria.Application));
			
			
			Utils.EndClassRegister(typeof(Maria.Application), L, translator);
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CreateInstance(RealStatePtr L)
        {
            
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			try {
				if(LuaAPI.lua_gettop(L) == 2 && translator.Assignable<Maria.Util.App>(L, 2))
				{
					Maria.Util.App app = (Maria.Util.App)translator.GetObject(L, 2, typeof(Maria.Util.App));
					
					Maria.Application __cl_gen_ret = new Maria.Application(app);
					translator.Push(L, __cl_gen_ret);
                    
					return 1;
				}
				
			}
			catch(System.Exception __gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to Maria.Application constructor!");
            
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Enqueue(RealStatePtr L)
        {
            
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            Maria.Application __cl_gen_to_be_invoked = (Maria.Application)translator.FastGetCSObj(L, 1);
            
            
            try {
                
                {
                    Maria.Command cmd = (Maria.Command)translator.GetObject(L, 2, typeof(Maria.Command));
                    
                    __cl_gen_to_be_invoked.Enqueue( cmd );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_EnqueueRenderQueue(RealStatePtr L)
        {
            
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            Maria.Application __cl_gen_to_be_invoked = (Maria.Application)translator.FastGetCSObj(L, 1);
            
            
            try {
                
                {
                    Maria.Actor.RenderHandler handler = translator.GetDelegate<Maria.Actor.RenderHandler>(L, 2);
                    
                    __cl_gen_to_be_invoked.EnqueueRenderQueue( handler );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Update(RealStatePtr L)
        {
            
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            Maria.Application __cl_gen_to_be_invoked = (Maria.Application)translator.FastGetCSObj(L, 1);
            
            
            try {
                
                {
                    
                    __cl_gen_to_be_invoked.Update(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_StartUpdateRes(RealStatePtr L)
        {
            
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            Maria.Application __cl_gen_to_be_invoked = (Maria.Application)translator.FastGetCSObj(L, 1);
            
            
            try {
                
                {
                    
                    __cl_gen_to_be_invoked.StartUpdateRes(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_StartScript(RealStatePtr L)
        {
            
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            Maria.Application __cl_gen_to_be_invoked = (Maria.Application)translator.FastGetCSObj(L, 1);
            
            
            try {
                
                {
                    
                    __cl_gen_to_be_invoked.StartScript(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_OnApplicationFocus(RealStatePtr L)
        {
            
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            Maria.Application __cl_gen_to_be_invoked = (Maria.Application)translator.FastGetCSObj(L, 1);
            
            
            try {
                
                {
                    bool hasFocus = LuaAPI.lua_toboolean(L, 2);
                    
                    __cl_gen_to_be_invoked.OnApplicationFocus( hasFocus );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_OnApplicationPause(RealStatePtr L)
        {
            
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            Maria.Application __cl_gen_to_be_invoked = (Maria.Application)translator.FastGetCSObj(L, 1);
            
            
            try {
                
                {
                    bool pauseStatus = LuaAPI.lua_toboolean(L, 2);
                    
                    __cl_gen_to_be_invoked.OnApplicationPause( pauseStatus );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_OnApplicationQuit(RealStatePtr L)
        {
            
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            Maria.Application __cl_gen_to_be_invoked = (Maria.Application)translator.FastGetCSObj(L, 1);
            
            
            try {
                
                {
                    
                    __cl_gen_to_be_invoked.OnApplicationQuit(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_LuaEnv(RealStatePtr L)
        {
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            try {
			
                Maria.Application __cl_gen_to_be_invoked = (Maria.Application)translator.FastGetCSObj(L, 1);
                translator.Push(L, __cl_gen_to_be_invoked.LuaEnv);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        
        
		
		
		
		
    }
}
