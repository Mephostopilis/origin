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
    public class MariaEventCmdWrap
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			Utils.BeginObjectRegister(typeof(Maria.EventCmd), L, translator, 0, 0, 3, 0);
			
			
			
			Utils.RegisterFunc(L, Utils.GETTER_IDX, "Cmd", _g_get_Cmd);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "Orgin", _g_get_Orgin);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "Msg", _g_get_Msg);
            
			
			Utils.EndObjectRegister(typeof(Maria.EventCmd), L, translator, null, null,
			    null, null, null);

		    Utils.BeginClassRegister(typeof(Maria.EventCmd), L, __CreateInstance, 1, 6, 6);
			
			
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "UnderlyingSystemType", typeof(Maria.EventCmd));
			Utils.RegisterFunc(L, Utils.CLS_GETTER_IDX, "EVENT_UPDATERES_BEGIN", _g_get_EVENT_UPDATERES_BEGIN);
            Utils.RegisterFunc(L, Utils.CLS_GETTER_IDX, "EVENT_UPDATERES_END", _g_get_EVENT_UPDATERES_END);
            Utils.RegisterFunc(L, Utils.CLS_GETTER_IDX, "EVENT_START_SETUP_ROOT", _g_get_EVENT_START_SETUP_ROOT);
            Utils.RegisterFunc(L, Utils.CLS_GETTER_IDX, "EVENT_NOTREACHABLE", _g_get_EVENT_NOTREACHABLE);
            Utils.RegisterFunc(L, Utils.CLS_GETTER_IDX, "EVENT_REACHABLEVIACARRIERDATANETWORK", _g_get_EVENT_REACHABLEVIACARRIERDATANETWORK);
            Utils.RegisterFunc(L, Utils.CLS_GETTER_IDX, "EVENT_REACHABLEVIALOCALAREANETWORK", _g_get_EVENT_REACHABLEVIALOCALAREANETWORK);
            
			Utils.RegisterFunc(L, Utils.CLS_SETTER_IDX, "EVENT_UPDATERES_BEGIN", _s_set_EVENT_UPDATERES_BEGIN);
            Utils.RegisterFunc(L, Utils.CLS_SETTER_IDX, "EVENT_UPDATERES_END", _s_set_EVENT_UPDATERES_END);
            Utils.RegisterFunc(L, Utils.CLS_SETTER_IDX, "EVENT_START_SETUP_ROOT", _s_set_EVENT_START_SETUP_ROOT);
            Utils.RegisterFunc(L, Utils.CLS_SETTER_IDX, "EVENT_NOTREACHABLE", _s_set_EVENT_NOTREACHABLE);
            Utils.RegisterFunc(L, Utils.CLS_SETTER_IDX, "EVENT_REACHABLEVIACARRIERDATANETWORK", _s_set_EVENT_REACHABLEVIACARRIERDATANETWORK);
            Utils.RegisterFunc(L, Utils.CLS_SETTER_IDX, "EVENT_REACHABLEVIALOCALAREANETWORK", _s_set_EVENT_REACHABLEVIALOCALAREANETWORK);
            
			Utils.EndClassRegister(typeof(Maria.EventCmd), L, translator);
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CreateInstance(RealStatePtr L)
        {
            
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			try {
				if(LuaAPI.lua_gettop(L) == 3 && translator.Assignable<Maria.Context>(L, 2) && LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3))
				{
					Maria.Context ctx = (Maria.Context)translator.GetObject(L, 2, typeof(Maria.Context));
					uint cmd = LuaAPI.xlua_touint(L, 3);
					
					Maria.EventCmd __cl_gen_ret = new Maria.EventCmd(ctx, cmd);
					translator.Push(L, __cl_gen_ret);
                    
					return 1;
				}
				if(LuaAPI.lua_gettop(L) == 4 && translator.Assignable<Maria.Context>(L, 2) && LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3) && translator.Assignable<Maria.Message>(L, 4))
				{
					Maria.Context ctx = (Maria.Context)translator.GetObject(L, 2, typeof(Maria.Context));
					uint cmd = LuaAPI.xlua_touint(L, 3);
					Maria.Message msg = (Maria.Message)translator.GetObject(L, 4, typeof(Maria.Message));
					
					Maria.EventCmd __cl_gen_ret = new Maria.EventCmd(ctx, cmd, msg);
					translator.Push(L, __cl_gen_ret);
                    
					return 1;
				}
				if(LuaAPI.lua_gettop(L) == 4 && translator.Assignable<Maria.Context>(L, 2) && LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3) && translator.Assignable<UnityEngine.GameObject>(L, 4))
				{
					Maria.Context ctx = (Maria.Context)translator.GetObject(L, 2, typeof(Maria.Context));
					uint cmd = LuaAPI.xlua_touint(L, 3);
					UnityEngine.GameObject orgin = (UnityEngine.GameObject)translator.GetObject(L, 4, typeof(UnityEngine.GameObject));
					
					Maria.EventCmd __cl_gen_ret = new Maria.EventCmd(ctx, cmd, orgin);
					translator.Push(L, __cl_gen_ret);
                    
					return 1;
				}
				if(LuaAPI.lua_gettop(L) == 5 && translator.Assignable<Maria.Context>(L, 2) && LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3) && translator.Assignable<UnityEngine.GameObject>(L, 4) && translator.Assignable<Maria.Message>(L, 5))
				{
					Maria.Context ctx = (Maria.Context)translator.GetObject(L, 2, typeof(Maria.Context));
					uint cmd = LuaAPI.xlua_touint(L, 3);
					UnityEngine.GameObject orgin = (UnityEngine.GameObject)translator.GetObject(L, 4, typeof(UnityEngine.GameObject));
					Maria.Message msg = (Maria.Message)translator.GetObject(L, 5, typeof(Maria.Message));
					
					Maria.EventCmd __cl_gen_ret = new Maria.EventCmd(ctx, cmd, orgin, msg);
					translator.Push(L, __cl_gen_ret);
                    
					return 1;
				}
				
			}
			catch(System.Exception __gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to Maria.EventCmd constructor!");
            
        }
        
		
        
		
        
        
        
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_Cmd(RealStatePtr L)
        {
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            try {
			
                Maria.EventCmd __cl_gen_to_be_invoked = (Maria.EventCmd)translator.FastGetCSObj(L, 1);
                LuaAPI.xlua_pushuint(L, __cl_gen_to_be_invoked.Cmd);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_Orgin(RealStatePtr L)
        {
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            try {
			
                Maria.EventCmd __cl_gen_to_be_invoked = (Maria.EventCmd)translator.FastGetCSObj(L, 1);
                translator.Push(L, __cl_gen_to_be_invoked.Orgin);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_Msg(RealStatePtr L)
        {
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            try {
			
                Maria.EventCmd __cl_gen_to_be_invoked = (Maria.EventCmd)translator.FastGetCSObj(L, 1);
                translator.Push(L, __cl_gen_to_be_invoked.Msg);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_EVENT_UPDATERES_BEGIN(RealStatePtr L)
        {
            
            try {
			    LuaAPI.xlua_pushuint(L, Maria.EventCmd.EVENT_UPDATERES_BEGIN);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_EVENT_UPDATERES_END(RealStatePtr L)
        {
            
            try {
			    LuaAPI.xlua_pushuint(L, Maria.EventCmd.EVENT_UPDATERES_END);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_EVENT_START_SETUP_ROOT(RealStatePtr L)
        {
            
            try {
			    LuaAPI.xlua_pushuint(L, Maria.EventCmd.EVENT_START_SETUP_ROOT);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_EVENT_NOTREACHABLE(RealStatePtr L)
        {
            
            try {
			    LuaAPI.xlua_pushuint(L, Maria.EventCmd.EVENT_NOTREACHABLE);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_EVENT_REACHABLEVIACARRIERDATANETWORK(RealStatePtr L)
        {
            
            try {
			    LuaAPI.xlua_pushuint(L, Maria.EventCmd.EVENT_REACHABLEVIACARRIERDATANETWORK);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_EVENT_REACHABLEVIALOCALAREANETWORK(RealStatePtr L)
        {
            
            try {
			    LuaAPI.xlua_pushuint(L, Maria.EventCmd.EVENT_REACHABLEVIALOCALAREANETWORK);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_EVENT_UPDATERES_BEGIN(RealStatePtr L)
        {
            
            try {
			    Maria.EventCmd.EVENT_UPDATERES_BEGIN = LuaAPI.xlua_touint(L, 1);
            
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_EVENT_UPDATERES_END(RealStatePtr L)
        {
            
            try {
			    Maria.EventCmd.EVENT_UPDATERES_END = LuaAPI.xlua_touint(L, 1);
            
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_EVENT_START_SETUP_ROOT(RealStatePtr L)
        {
            
            try {
			    Maria.EventCmd.EVENT_START_SETUP_ROOT = LuaAPI.xlua_touint(L, 1);
            
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_EVENT_NOTREACHABLE(RealStatePtr L)
        {
            
            try {
			    Maria.EventCmd.EVENT_NOTREACHABLE = LuaAPI.xlua_touint(L, 1);
            
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_EVENT_REACHABLEVIACARRIERDATANETWORK(RealStatePtr L)
        {
            
            try {
			    Maria.EventCmd.EVENT_REACHABLEVIACARRIERDATANETWORK = LuaAPI.xlua_touint(L, 1);
            
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_EVENT_REACHABLEVIALOCALAREANETWORK(RealStatePtr L)
        {
            
            try {
			    Maria.EventCmd.EVENT_REACHABLEVIALOCALAREANETWORK = LuaAPI.xlua_touint(L, 1);
            
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 0;
        }
        
		
		
		
		
    }
}
