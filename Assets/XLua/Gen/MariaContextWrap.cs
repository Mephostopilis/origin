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
    public class MariaContextWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(Maria.Context);
			Utils.BeginObjectRegister(type, L, translator, 0, 25, 9, 0);
			
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "Update", _m_Update);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "Range", _m_Range);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "LoginAuth", _m_LoginAuth);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "OnLoginConnected", _m_OnLoginConnected);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "OnLoginAuthed", _m_OnLoginAuthed);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "OnLoginDisconnected", _m_OnLoginDisconnected);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GateAuth", _m_GateAuth);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "OnGateConnected", _m_OnGateConnected);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "OnGateAuthed", _m_OnGateAuthed);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "OnGateDisconnected", _m_OnGateDisconnected);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "Peek", _m_Peek);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "Push", _m_Push);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "Pop", _m_Pop);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "Countdown", _m_Countdown);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "RegService", _m_RegService);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "UnrService", _m_UnrService);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "QueryService", _m_QueryService);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "Enqueue", _m_Enqueue);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "EnqueueRenderQueue", _m_EnqueueRenderQueue);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "UdpAuth", _m_UdpAuth);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "SendUdp", _m_SendUdp);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "OnUdpRecv", _m_OnUdpRecv);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "StartScript", _m_StartScript);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "OnCreateLua", _m_OnCreateLua);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "OnDestroyLua", _m_OnDestroyLua);
			
			
			Utils.RegisterFunc(L, Utils.GETTER_IDX, "Config", _g_get_Config);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "TiSync", _g_get_TiSync);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "SharpC", _g_get_SharpC);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "EventDispatcher", _g_get_EventDispatcher);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "Client", _g_get_Client);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "U", _g_get_U);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "AuthTcp", _g_get_AuthTcp);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "Logined", _g_get_Logined);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "LuaContext", _g_get_LuaContext);
            
			
			
			Utils.EndObjectRegister(type, L, translator, null, null,
			    null, null, null);

		    Utils.BeginClassRegister(type, L, __CreateInstance, 1, 0, 0);
			
			
            
			
			
			
			Utils.EndClassRegister(type, L, translator);
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CreateInstance(RealStatePtr L)
        {
            
			try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
				if(LuaAPI.lua_gettop(L) == 4 && translator.Assignable<Maria.Application>(L, 2) && translator.Assignable<Maria.Config>(L, 3) && translator.Assignable<Maria.Network.TimeSync>(L, 4))
				{
					Maria.Application application = (Maria.Application)translator.GetObject(L, 2, typeof(Maria.Application));
					Maria.Config config = (Maria.Config)translator.GetObject(L, 3, typeof(Maria.Config));
					Maria.Network.TimeSync ts = (Maria.Network.TimeSync)translator.GetObject(L, 4, typeof(Maria.Network.TimeSync));
					
					Maria.Context __cl_gen_ret = new Maria.Context(application, config, ts);
					translator.Push(L, __cl_gen_ret);
                    
					return 1;
				}
				
			}
			catch(System.Exception __gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to Maria.Context constructor!");
            
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Update(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Maria.Context __cl_gen_to_be_invoked = (Maria.Context)translator.FastGetCSObj(L, 1);
            
            
                
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
        static int _m_Range(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Maria.Context __cl_gen_to_be_invoked = (Maria.Context)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    int min = LuaAPI.xlua_tointeger(L, 2);
                    int max = LuaAPI.xlua_tointeger(L, 3);
                    
                        int __cl_gen_ret = __cl_gen_to_be_invoked.Range( min, max );
                        LuaAPI.xlua_pushinteger(L, __cl_gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_LoginAuth(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Maria.Context __cl_gen_to_be_invoked = (Maria.Context)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    string s = LuaAPI.lua_tostring(L, 2);
                    string u = LuaAPI.lua_tostring(L, 3);
                    string pwd = LuaAPI.lua_tostring(L, 4);
                    
                    __cl_gen_to_be_invoked.LoginAuth( s, u, pwd );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_OnLoginConnected(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Maria.Context __cl_gen_to_be_invoked = (Maria.Context)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    bool connected = LuaAPI.lua_toboolean(L, 2);
                    
                    __cl_gen_to_be_invoked.OnLoginConnected( connected );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_OnLoginAuthed(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Maria.Context __cl_gen_to_be_invoked = (Maria.Context)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    int code = LuaAPI.xlua_tointeger(L, 2);
                    byte[] secret = LuaAPI.lua_tobytes(L, 3);
                    string dummy = LuaAPI.lua_tostring(L, 4);
                    
                    __cl_gen_to_be_invoked.OnLoginAuthed( code, secret, dummy );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_OnLoginDisconnected(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Maria.Context __cl_gen_to_be_invoked = (Maria.Context)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    __cl_gen_to_be_invoked.OnLoginDisconnected(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GateAuth(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Maria.Context __cl_gen_to_be_invoked = (Maria.Context)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    __cl_gen_to_be_invoked.GateAuth(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_OnGateConnected(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Maria.Context __cl_gen_to_be_invoked = (Maria.Context)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    bool connected = LuaAPI.lua_toboolean(L, 2);
                    
                    __cl_gen_to_be_invoked.OnGateConnected( connected );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_OnGateAuthed(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Maria.Context __cl_gen_to_be_invoked = (Maria.Context)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    int code = LuaAPI.xlua_tointeger(L, 2);
                    
                    __cl_gen_to_be_invoked.OnGateAuthed( code );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_OnGateDisconnected(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Maria.Context __cl_gen_to_be_invoked = (Maria.Context)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    __cl_gen_to_be_invoked.OnGateDisconnected(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Peek(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Maria.Context __cl_gen_to_be_invoked = (Maria.Context)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                        Maria.Controller __cl_gen_ret = __cl_gen_to_be_invoked.Peek(  );
                        translator.Push(L, __cl_gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Push(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Maria.Context __cl_gen_to_be_invoked = (Maria.Context)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    System.Type type = (System.Type)translator.GetObject(L, 2, typeof(System.Type));
                    
                        Maria.Controller __cl_gen_ret = __cl_gen_to_be_invoked.Push( type );
                        translator.Push(L, __cl_gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Pop(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Maria.Context __cl_gen_to_be_invoked = (Maria.Context)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                        Maria.Controller __cl_gen_ret = __cl_gen_to_be_invoked.Pop(  );
                        translator.Push(L, __cl_gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Countdown(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Maria.Context __cl_gen_to_be_invoked = (Maria.Context)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    string name = LuaAPI.lua_tostring(L, 2);
                    int cd = LuaAPI.xlua_tointeger(L, 3);
                    Maria.Timer.CountdownDeltaCb dcb = translator.GetDelegate<Maria.Timer.CountdownDeltaCb>(L, 4);
                    Maria.Timer.CountdownCb cb = translator.GetDelegate<Maria.Timer.CountdownCb>(L, 5);
                    
                        Maria.Timer __cl_gen_ret = __cl_gen_to_be_invoked.Countdown( name, cd, dcb, cb );
                        translator.Push(L, __cl_gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_RegService(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Maria.Context __cl_gen_to_be_invoked = (Maria.Context)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    string name = LuaAPI.lua_tostring(L, 2);
                    Maria.Service s = (Maria.Service)translator.GetObject(L, 3, typeof(Maria.Service));
                    
                    __cl_gen_to_be_invoked.RegService( name, s );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_UnrService(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Maria.Context __cl_gen_to_be_invoked = (Maria.Context)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    string name = LuaAPI.lua_tostring(L, 2);
                    Maria.Service s = (Maria.Service)translator.GetObject(L, 3, typeof(Maria.Service));
                    
                    __cl_gen_to_be_invoked.UnrService( name, s );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_QueryService(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Maria.Context __cl_gen_to_be_invoked = (Maria.Context)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    string name = LuaAPI.lua_tostring(L, 2);
                    
                        Maria.Service __cl_gen_ret = __cl_gen_to_be_invoked.QueryService( name );
                        translator.Push(L, __cl_gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Enqueue(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Maria.Context __cl_gen_to_be_invoked = (Maria.Context)translator.FastGetCSObj(L, 1);
            
            
                
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
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Maria.Context __cl_gen_to_be_invoked = (Maria.Context)translator.FastGetCSObj(L, 1);
            
            
                
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
        static int _m_UdpAuth(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Maria.Context __cl_gen_to_be_invoked = (Maria.Context)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    long session = LuaAPI.lua_toint64(L, 2);
                    string ip = LuaAPI.lua_tostring(L, 3);
                    int port = LuaAPI.xlua_tointeger(L, 4);
                    
                    __cl_gen_to_be_invoked.UdpAuth( session, ip, port );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SendUdp(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Maria.Context __cl_gen_to_be_invoked = (Maria.Context)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    byte[] data = LuaAPI.lua_tobytes(L, 2);
                    int start = LuaAPI.xlua_tointeger(L, 3);
                    int len = LuaAPI.xlua_tointeger(L, 4);
                    
                    __cl_gen_to_be_invoked.SendUdp( data, start, len );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_OnUdpRecv(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Maria.Context __cl_gen_to_be_invoked = (Maria.Context)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    byte[] data = LuaAPI.lua_tobytes(L, 2);
                    int start = LuaAPI.xlua_tointeger(L, 3);
                    int len = LuaAPI.xlua_tointeger(L, 4);
                    
                    __cl_gen_to_be_invoked.OnUdpRecv( data, start, len );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_StartScript(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Maria.Context __cl_gen_to_be_invoked = (Maria.Context)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    __cl_gen_to_be_invoked.StartScript(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_OnCreateLua(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Maria.Context __cl_gen_to_be_invoked = (Maria.Context)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    __cl_gen_to_be_invoked.OnCreateLua(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_OnDestroyLua(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Maria.Context __cl_gen_to_be_invoked = (Maria.Context)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    __cl_gen_to_be_invoked.OnDestroyLua(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_Config(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Maria.Context __cl_gen_to_be_invoked = (Maria.Context)translator.FastGetCSObj(L, 1);
                translator.Push(L, __cl_gen_to_be_invoked.Config);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_TiSync(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Maria.Context __cl_gen_to_be_invoked = (Maria.Context)translator.FastGetCSObj(L, 1);
                translator.Push(L, __cl_gen_to_be_invoked.TiSync);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_SharpC(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Maria.Context __cl_gen_to_be_invoked = (Maria.Context)translator.FastGetCSObj(L, 1);
                translator.Push(L, __cl_gen_to_be_invoked.SharpC);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_EventDispatcher(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Maria.Context __cl_gen_to_be_invoked = (Maria.Context)translator.FastGetCSObj(L, 1);
                translator.Push(L, __cl_gen_to_be_invoked.EventDispatcher);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_Client(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Maria.Context __cl_gen_to_be_invoked = (Maria.Context)translator.FastGetCSObj(L, 1);
                translator.Push(L, __cl_gen_to_be_invoked.Client);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_U(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Maria.Context __cl_gen_to_be_invoked = (Maria.Context)translator.FastGetCSObj(L, 1);
                translator.Push(L, __cl_gen_to_be_invoked.U);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_AuthTcp(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Maria.Context __cl_gen_to_be_invoked = (Maria.Context)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, __cl_gen_to_be_invoked.AuthTcp);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_Logined(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Maria.Context __cl_gen_to_be_invoked = (Maria.Context)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, __cl_gen_to_be_invoked.Logined);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_LuaContext(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Maria.Context __cl_gen_to_be_invoked = (Maria.Context)translator.FastGetCSObj(L, 1);
                translator.Push(L, __cl_gen_to_be_invoked.LuaContext);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        
        
		
		
		
		
    }
}
