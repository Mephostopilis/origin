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
    public class MariaCommandWrap
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			Utils.BeginObjectRegister(typeof(Maria.Command), L, translator, 0, 0, 3, 1);
			
			
			
			Utils.RegisterFunc(L, Utils.GETTER_IDX, "Cmd", _g_get_Cmd);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "Orgin", _g_get_Orgin);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "Msg", _g_get_Msg);
            
			Utils.RegisterFunc(L, Utils.SETTER_IDX, "Msg", _s_set_Msg);
            
			Utils.EndObjectRegister(typeof(Maria.Command), L, translator, null, null,
			    null, null, null);

		    Utils.BeginClassRegister(typeof(Maria.Command), L, __CreateInstance, 1, 0, 0);
			
			
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "UnderlyingSystemType", typeof(Maria.Command));
			
			
			Utils.EndClassRegister(typeof(Maria.Command), L, translator);
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CreateInstance(RealStatePtr L)
        {
            
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			try {
				if(LuaAPI.lua_gettop(L) == 2 && LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2))
				{
					uint cmd = LuaAPI.xlua_touint(L, 2);
					
					Maria.Command __cl_gen_ret = new Maria.Command(cmd);
					translator.Push(L, __cl_gen_ret);
                    
					return 1;
				}
				if(LuaAPI.lua_gettop(L) == 3 && LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2) && translator.Assignable<UnityEngine.GameObject>(L, 3))
				{
					uint cmd = LuaAPI.xlua_touint(L, 2);
					UnityEngine.GameObject orgin = (UnityEngine.GameObject)translator.GetObject(L, 3, typeof(UnityEngine.GameObject));
					
					Maria.Command __cl_gen_ret = new Maria.Command(cmd, orgin);
					translator.Push(L, __cl_gen_ret);
                    
					return 1;
				}
				if(LuaAPI.lua_gettop(L) == 4 && LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2) && translator.Assignable<UnityEngine.GameObject>(L, 3) && translator.Assignable<Maria.Message>(L, 4))
				{
					uint cmd = LuaAPI.xlua_touint(L, 2);
					UnityEngine.GameObject orgin = (UnityEngine.GameObject)translator.GetObject(L, 3, typeof(UnityEngine.GameObject));
					Maria.Message msg = (Maria.Message)translator.GetObject(L, 4, typeof(Maria.Message));
					
					Maria.Command __cl_gen_ret = new Maria.Command(cmd, orgin, msg);
					translator.Push(L, __cl_gen_ret);
                    
					return 1;
				}
				
			}
			catch(System.Exception __gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to Maria.Command constructor!");
            
        }
        
		
        
		
        
        
        
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_Cmd(RealStatePtr L)
        {
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            try {
			
                Maria.Command __cl_gen_to_be_invoked = (Maria.Command)translator.FastGetCSObj(L, 1);
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
			
                Maria.Command __cl_gen_to_be_invoked = (Maria.Command)translator.FastGetCSObj(L, 1);
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
			
                Maria.Command __cl_gen_to_be_invoked = (Maria.Command)translator.FastGetCSObj(L, 1);
                translator.Push(L, __cl_gen_to_be_invoked.Msg);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_Msg(RealStatePtr L)
        {
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            try {
			
                Maria.Command __cl_gen_to_be_invoked = (Maria.Command)translator.FastGetCSObj(L, 1);
                __cl_gen_to_be_invoked.Msg = (Maria.Message)translator.GetObject(L, 2, typeof(Maria.Message));
            
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 0;
        }
        
		
		
		
		
    }
}
