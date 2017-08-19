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
    public class MariaResABLoaderWrap
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			Utils.BeginObjectRegister(typeof(Maria.Res.ABLoader), L, translator, 0, 3, 0, 0);
			
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "FetchVersion", _m_FetchVersion);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "LoadTextAsset", _m_LoadTextAsset);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "Unload", _m_Unload);
			
			
			
			
			Utils.EndObjectRegister(typeof(Maria.Res.ABLoader), L, translator, null, null,
			    null, null, null);

		    Utils.BeginClassRegister(typeof(Maria.Res.ABLoader), L, __CreateInstance, 1, 1, 1);
			
			
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "UnderlyingSystemType", typeof(Maria.Res.ABLoader));
			Utils.RegisterFunc(L, Utils.CLS_GETTER_IDX, "current", _g_get_current);
            
			Utils.RegisterFunc(L, Utils.CLS_SETTER_IDX, "current", _s_set_current);
            
			Utils.EndClassRegister(typeof(Maria.Res.ABLoader), L, translator);
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CreateInstance(RealStatePtr L)
        {
            
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			try {
				if(LuaAPI.lua_gettop(L) == 1)
				{
					
					Maria.Res.ABLoader __cl_gen_ret = new Maria.Res.ABLoader();
					translator.Push(L, __cl_gen_ret);
                    
					return 1;
				}
				
			}
			catch(System.Exception __gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to Maria.Res.ABLoader constructor!");
            
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_FetchVersion(RealStatePtr L)
        {
            
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            Maria.Res.ABLoader __cl_gen_to_be_invoked = (Maria.Res.ABLoader)translator.FastGetCSObj(L, 1);
            
            
            try {
                
                {
                    System.Action cb = translator.GetDelegate<System.Action>(L, 2);
                    
                    __cl_gen_to_be_invoked.FetchVersion( cb );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_LoadTextAsset(RealStatePtr L)
        {
            
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            Maria.Res.ABLoader __cl_gen_to_be_invoked = (Maria.Res.ABLoader)translator.FastGetCSObj(L, 1);
            
            
            try {
                
                {
                    string path = LuaAPI.lua_tostring(L, 2);
                    string name = LuaAPI.lua_tostring(L, 3);
                    
                        UnityEngine.TextAsset __cl_gen_ret = __cl_gen_to_be_invoked.LoadTextAsset( path, name );
                        translator.Push(L, __cl_gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Unload(RealStatePtr L)
        {
            
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            Maria.Res.ABLoader __cl_gen_to_be_invoked = (Maria.Res.ABLoader)translator.FastGetCSObj(L, 1);
            
            
            try {
                
                {
                    
                    __cl_gen_to_be_invoked.Unload(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_current(RealStatePtr L)
        {
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            try {
			    translator.Push(L, Maria.Res.ABLoader.current);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_current(RealStatePtr L)
        {
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            try {
			    Maria.Res.ABLoader.current = (Maria.Res.ABLoader)translator.GetObject(L, 1, typeof(Maria.Res.ABLoader));
            
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 0;
        }
        
		
		
		
		
    }
}
