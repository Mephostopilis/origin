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
    public class MariaEventCustomWrap
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			Utils.BeginObjectRegister(typeof(Maria.EventCustom), L, translator, 0, 0, 3, 0);
			
			
			
			Utils.RegisterFunc(L, Utils.GETTER_IDX, "Name", _g_get_Name);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "Addition", _g_get_Addition);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "Ud", _g_get_Ud);
            
			
			Utils.EndObjectRegister(typeof(Maria.EventCustom), L, translator, null, null,
			    null, null, null);

		    Utils.BeginClassRegister(typeof(Maria.EventCustom), L, __CreateInstance, 3, 0, 0);
			
			
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "OnGateAuthed", Maria.EventCustom.OnGateAuthed);
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "OnGateDisconnected", Maria.EventCustom.OnGateDisconnected);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "UnderlyingSystemType", typeof(Maria.EventCustom));
			
			
			Utils.EndClassRegister(typeof(Maria.EventCustom), L, translator);
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CreateInstance(RealStatePtr L)
        {
            
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			try {
				if(LuaAPI.lua_gettop(L) == 3 && translator.Assignable<Maria.Context>(L, 2) && (LuaAPI.lua_isnil(L, 3) || LuaAPI.lua_type(L, 3) == LuaTypes.LUA_TSTRING))
				{
					Maria.Context ctx = (Maria.Context)translator.GetObject(L, 2, typeof(Maria.Context));
					string name = LuaAPI.lua_tostring(L, 3);
					
					Maria.EventCustom __cl_gen_ret = new Maria.EventCustom(ctx, name);
					translator.Push(L, __cl_gen_ret);
                    
					return 1;
				}
				if(LuaAPI.lua_gettop(L) == 4 && translator.Assignable<Maria.Context>(L, 2) && (LuaAPI.lua_isnil(L, 3) || LuaAPI.lua_type(L, 3) == LuaTypes.LUA_TSTRING) && translator.Assignable<object>(L, 4))
				{
					Maria.Context ctx = (Maria.Context)translator.GetObject(L, 2, typeof(Maria.Context));
					string name = LuaAPI.lua_tostring(L, 3);
					object addition = translator.GetObject(L, 4, typeof(object));
					
					Maria.EventCustom __cl_gen_ret = new Maria.EventCustom(ctx, name, addition);
					translator.Push(L, __cl_gen_ret);
                    
					return 1;
				}
				if(LuaAPI.lua_gettop(L) == 5 && translator.Assignable<Maria.Context>(L, 2) && (LuaAPI.lua_isnil(L, 3) || LuaAPI.lua_type(L, 3) == LuaTypes.LUA_TSTRING) && translator.Assignable<object>(L, 4) && translator.Assignable<object>(L, 5))
				{
					Maria.Context ctx = (Maria.Context)translator.GetObject(L, 2, typeof(Maria.Context));
					string name = LuaAPI.lua_tostring(L, 3);
					object addition = translator.GetObject(L, 4, typeof(object));
					object ud = translator.GetObject(L, 5, typeof(object));
					
					Maria.EventCustom __cl_gen_ret = new Maria.EventCustom(ctx, name, addition, ud);
					translator.Push(L, __cl_gen_ret);
                    
					return 1;
				}
				
			}
			catch(System.Exception __gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to Maria.EventCustom constructor!");
            
        }
        
		
        
		
        
        
        
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_Name(RealStatePtr L)
        {
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            try {
			
                Maria.EventCustom __cl_gen_to_be_invoked = (Maria.EventCustom)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushstring(L, __cl_gen_to_be_invoked.Name);
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
			
                Maria.EventCustom __cl_gen_to_be_invoked = (Maria.EventCustom)translator.FastGetCSObj(L, 1);
                translator.PushAny(L, __cl_gen_to_be_invoked.Addition);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_Ud(RealStatePtr L)
        {
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            try {
			
                Maria.EventCustom __cl_gen_to_be_invoked = (Maria.EventCustom)translator.FastGetCSObj(L, 1);
                translator.PushAny(L, __cl_gen_to_be_invoked.Ud);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        
        
		
		
		
		
    }
}
