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
    public class MariaActorWrap
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			Utils.BeginObjectRegister(typeof(Maria.Actor), L, translator, 0, 3, 3, 3);
			
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "OnEnter", _m_OnEnter);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "OnExit", _m_OnExit);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "Update", _m_Update);
			
			
			Utils.RegisterFunc(L, Utils.GETTER_IDX, "Go", _g_get_Go);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "Controller", _g_get_Controller);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "Service", _g_get_Service);
            
			Utils.RegisterFunc(L, Utils.SETTER_IDX, "Go", _s_set_Go);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "Controller", _s_set_Controller);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "Service", _s_set_Service);
            
			Utils.EndObjectRegister(typeof(Maria.Actor), L, translator, null, null,
			    null, null, null);

		    Utils.BeginClassRegister(typeof(Maria.Actor), L, __CreateInstance, 1, 0, 0);
			
			
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "UnderlyingSystemType", typeof(Maria.Actor));
			
			
			Utils.EndClassRegister(typeof(Maria.Actor), L, translator);
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CreateInstance(RealStatePtr L)
        {
            
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			try {
				if(LuaAPI.lua_gettop(L) == 3 && translator.Assignable<Maria.Context>(L, 2) && translator.Assignable<Maria.Controller>(L, 3))
				{
					Maria.Context ctx = (Maria.Context)translator.GetObject(L, 2, typeof(Maria.Context));
					Maria.Controller controller = (Maria.Controller)translator.GetObject(L, 3, typeof(Maria.Controller));
					
					Maria.Actor __cl_gen_ret = new Maria.Actor(ctx, controller);
					translator.Push(L, __cl_gen_ret);
                    
					return 1;
				}
				if(LuaAPI.lua_gettop(L) == 4 && translator.Assignable<Maria.Context>(L, 2) && translator.Assignable<Maria.Controller>(L, 3) && translator.Assignable<UnityEngine.GameObject>(L, 4))
				{
					Maria.Context ctx = (Maria.Context)translator.GetObject(L, 2, typeof(Maria.Context));
					Maria.Controller controller = (Maria.Controller)translator.GetObject(L, 3, typeof(Maria.Controller));
					UnityEngine.GameObject go = (UnityEngine.GameObject)translator.GetObject(L, 4, typeof(UnityEngine.GameObject));
					
					Maria.Actor __cl_gen_ret = new Maria.Actor(ctx, controller, go);
					translator.Push(L, __cl_gen_ret);
                    
					return 1;
				}
				if(LuaAPI.lua_gettop(L) == 3 && translator.Assignable<Maria.Context>(L, 2) && translator.Assignable<Maria.Service>(L, 3))
				{
					Maria.Context ctx = (Maria.Context)translator.GetObject(L, 2, typeof(Maria.Context));
					Maria.Service service = (Maria.Service)translator.GetObject(L, 3, typeof(Maria.Service));
					
					Maria.Actor __cl_gen_ret = new Maria.Actor(ctx, service);
					translator.Push(L, __cl_gen_ret);
                    
					return 1;
				}
				if(LuaAPI.lua_gettop(L) == 4 && translator.Assignable<Maria.Context>(L, 2) && translator.Assignable<Maria.Service>(L, 3) && translator.Assignable<UnityEngine.GameObject>(L, 4))
				{
					Maria.Context ctx = (Maria.Context)translator.GetObject(L, 2, typeof(Maria.Context));
					Maria.Service service = (Maria.Service)translator.GetObject(L, 3, typeof(Maria.Service));
					UnityEngine.GameObject go = (UnityEngine.GameObject)translator.GetObject(L, 4, typeof(UnityEngine.GameObject));
					
					Maria.Actor __cl_gen_ret = new Maria.Actor(ctx, service, go);
					translator.Push(L, __cl_gen_ret);
                    
					return 1;
				}
				
			}
			catch(System.Exception __gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to Maria.Actor constructor!");
            
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_OnEnter(RealStatePtr L)
        {
            
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            Maria.Actor __cl_gen_to_be_invoked = (Maria.Actor)translator.FastGetCSObj(L, 1);
            
            
            try {
                
                {
                    
                    __cl_gen_to_be_invoked.OnEnter(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_OnExit(RealStatePtr L)
        {
            
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            Maria.Actor __cl_gen_to_be_invoked = (Maria.Actor)translator.FastGetCSObj(L, 1);
            
            
            try {
                
                {
                    
                    __cl_gen_to_be_invoked.OnExit(  );
                    
                    
                    
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
            
            
            Maria.Actor __cl_gen_to_be_invoked = (Maria.Actor)translator.FastGetCSObj(L, 1);
            
            
            try {
                
                {
                    float delta = (float)LuaAPI.lua_tonumber(L, 2);
                    
                    __cl_gen_to_be_invoked.Update( delta );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_Go(RealStatePtr L)
        {
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            try {
			
                Maria.Actor __cl_gen_to_be_invoked = (Maria.Actor)translator.FastGetCSObj(L, 1);
                translator.Push(L, __cl_gen_to_be_invoked.Go);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_Controller(RealStatePtr L)
        {
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            try {
			
                Maria.Actor __cl_gen_to_be_invoked = (Maria.Actor)translator.FastGetCSObj(L, 1);
                translator.Push(L, __cl_gen_to_be_invoked.Controller);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_Service(RealStatePtr L)
        {
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            try {
			
                Maria.Actor __cl_gen_to_be_invoked = (Maria.Actor)translator.FastGetCSObj(L, 1);
                translator.Push(L, __cl_gen_to_be_invoked.Service);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_Go(RealStatePtr L)
        {
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            try {
			
                Maria.Actor __cl_gen_to_be_invoked = (Maria.Actor)translator.FastGetCSObj(L, 1);
                __cl_gen_to_be_invoked.Go = (UnityEngine.GameObject)translator.GetObject(L, 2, typeof(UnityEngine.GameObject));
            
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_Controller(RealStatePtr L)
        {
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            try {
			
                Maria.Actor __cl_gen_to_be_invoked = (Maria.Actor)translator.FastGetCSObj(L, 1);
                __cl_gen_to_be_invoked.Controller = (Maria.Controller)translator.GetObject(L, 2, typeof(Maria.Controller));
            
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_Service(RealStatePtr L)
        {
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            try {
			
                Maria.Actor __cl_gen_to_be_invoked = (Maria.Actor)translator.FastGetCSObj(L, 1);
                __cl_gen_to_be_invoked.Service = (Maria.Service)translator.GetObject(L, 2, typeof(Maria.Service));
            
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 0;
        }
        
		
		
		
		
    }
}
