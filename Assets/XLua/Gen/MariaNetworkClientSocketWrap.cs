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
    public class MariaNetworkClientSocketWrap
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			Utils.BeginObjectRegister(typeof(Maria.Network.ClientSocket), L, translator, 0, 10, 6, 6);
			
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "Update", _m_Update);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "RegisterResponse", _m_RegisterResponse);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "RegisterRequest", _m_RegisterRequest);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "genSession", _m_genSession);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "Send", _m_Send);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "Auth", _m_Auth);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "Reset", _m_Reset);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "UdpAuth", _m_UdpAuth);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "SendUdp", _m_SendUdp);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "StartScript", _m_StartScript);
			
			
			Utils.RegisterFunc(L, Utils.GETTER_IDX, "OnAuthed", _g_get_OnAuthed);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "OnConnected", _g_get_OnConnected);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "OnDisconnected", _g_get_OnDisconnected);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "OnRecvUdp", _g_get_OnRecvUdp);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "OnSyncUdp", _g_get_OnSyncUdp);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "ClintSockscript", _g_get_ClintSockscript);
            
			Utils.RegisterFunc(L, Utils.SETTER_IDX, "OnAuthed", _s_set_OnAuthed);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "OnConnected", _s_set_OnConnected);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "OnDisconnected", _s_set_OnDisconnected);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "OnRecvUdp", _s_set_OnRecvUdp);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "OnSyncUdp", _s_set_OnSyncUdp);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "ClintSockscript", _s_set_ClintSockscript);
            
			Utils.EndObjectRegister(typeof(Maria.Network.ClientSocket), L, translator, null, null,
			    null, null, null);

		    Utils.BeginClassRegister(typeof(Maria.Network.ClientSocket), L, __CreateInstance, 1, 0, 0);
			
			
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "UnderlyingSystemType", typeof(Maria.Network.ClientSocket));
			
			
			Utils.EndClassRegister(typeof(Maria.Network.ClientSocket), L, translator);
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CreateInstance(RealStatePtr L)
        {
            
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			try {
				if(LuaAPI.lua_gettop(L) == 4 && translator.Assignable<Maria.Context>(L, 2) && translator.Assignable<Sproto.ProtocolBase>(L, 3) && translator.Assignable<Sproto.ProtocolBase>(L, 4))
				{
					Maria.Context ctx = (Maria.Context)translator.GetObject(L, 2, typeof(Maria.Context));
					Sproto.ProtocolBase s2c = (Sproto.ProtocolBase)translator.GetObject(L, 3, typeof(Sproto.ProtocolBase));
					Sproto.ProtocolBase c2s = (Sproto.ProtocolBase)translator.GetObject(L, 4, typeof(Sproto.ProtocolBase));
					
					Maria.Network.ClientSocket __cl_gen_ret = new Maria.Network.ClientSocket(ctx, s2c, c2s);
					translator.Push(L, __cl_gen_ret);
                    
					return 1;
				}
				
			}
			catch(System.Exception __gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to Maria.Network.ClientSocket constructor!");
            
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Update(RealStatePtr L)
        {
            
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            Maria.Network.ClientSocket __cl_gen_to_be_invoked = (Maria.Network.ClientSocket)translator.FastGetCSObj(L, 1);
            
            
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
        static int _m_RegisterResponse(RealStatePtr L)
        {
            
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            Maria.Network.ClientSocket __cl_gen_to_be_invoked = (Maria.Network.ClientSocket)translator.FastGetCSObj(L, 1);
            
            
            try {
                
                {
                    int tag = LuaAPI.xlua_tointeger(L, 2);
                    Maria.Network.ClientSocket.RspCb cb = translator.GetDelegate<Maria.Network.ClientSocket.RspCb>(L, 3);
                    
                    __cl_gen_to_be_invoked.RegisterResponse( tag, cb );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_RegisterRequest(RealStatePtr L)
        {
            
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            Maria.Network.ClientSocket __cl_gen_to_be_invoked = (Maria.Network.ClientSocket)translator.FastGetCSObj(L, 1);
            
            
            try {
                
                {
                    int tag = LuaAPI.xlua_tointeger(L, 2);
                    Maria.Network.ClientSocket.ReqCb cb = translator.GetDelegate<Maria.Network.ClientSocket.ReqCb>(L, 3);
                    
                    __cl_gen_to_be_invoked.RegisterRequest( tag, cb );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_genSession(RealStatePtr L)
        {
            
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            Maria.Network.ClientSocket __cl_gen_to_be_invoked = (Maria.Network.ClientSocket)translator.FastGetCSObj(L, 1);
            
            
            try {
                
                {
                    
                        long __cl_gen_ret = __cl_gen_to_be_invoked.genSession(  );
                        LuaAPI.lua_pushint64(L, __cl_gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Send(RealStatePtr L)
        {
            
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            Maria.Network.ClientSocket __cl_gen_to_be_invoked = (Maria.Network.ClientSocket)translator.FastGetCSObj(L, 1);
            
            
            try {
                
                {
                    string pack = LuaAPI.lua_tostring(L, 2);
                    
                    __cl_gen_to_be_invoked.Send( pack );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Auth(RealStatePtr L)
        {
            
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            Maria.Network.ClientSocket __cl_gen_to_be_invoked = (Maria.Network.ClientSocket)translator.FastGetCSObj(L, 1);
            
            
            try {
                
                {
                    string ipstr = LuaAPI.lua_tostring(L, 2);
                    int pt = LuaAPI.xlua_tointeger(L, 3);
                    Maria.User u = (Maria.User)translator.GetObject(L, 4, typeof(Maria.User));
                    
                    __cl_gen_to_be_invoked.Auth( ipstr, pt, u );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Reset(RealStatePtr L)
        {
            
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            Maria.Network.ClientSocket __cl_gen_to_be_invoked = (Maria.Network.ClientSocket)translator.FastGetCSObj(L, 1);
            
            
            try {
                
                {
                    
                    __cl_gen_to_be_invoked.Reset(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_UdpAuth(RealStatePtr L)
        {
            
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            Maria.Network.ClientSocket __cl_gen_to_be_invoked = (Maria.Network.ClientSocket)translator.FastGetCSObj(L, 1);
            
            
            try {
                
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
            
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            Maria.Network.ClientSocket __cl_gen_to_be_invoked = (Maria.Network.ClientSocket)translator.FastGetCSObj(L, 1);
            
            
            try {
                
                {
                    byte[] data = LuaAPI.lua_tobytes(L, 2);
                    
                    __cl_gen_to_be_invoked.SendUdp( data );
                    
                    
                    
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
            
            
            Maria.Network.ClientSocket __cl_gen_to_be_invoked = (Maria.Network.ClientSocket)translator.FastGetCSObj(L, 1);
            
            
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
        static int _g_get_OnAuthed(RealStatePtr L)
        {
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            try {
			
                Maria.Network.ClientSocket __cl_gen_to_be_invoked = (Maria.Network.ClientSocket)translator.FastGetCSObj(L, 1);
                translator.Push(L, __cl_gen_to_be_invoked.OnAuthed);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_OnConnected(RealStatePtr L)
        {
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            try {
			
                Maria.Network.ClientSocket __cl_gen_to_be_invoked = (Maria.Network.ClientSocket)translator.FastGetCSObj(L, 1);
                translator.Push(L, __cl_gen_to_be_invoked.OnConnected);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_OnDisconnected(RealStatePtr L)
        {
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            try {
			
                Maria.Network.ClientSocket __cl_gen_to_be_invoked = (Maria.Network.ClientSocket)translator.FastGetCSObj(L, 1);
                translator.Push(L, __cl_gen_to_be_invoked.OnDisconnected);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_OnRecvUdp(RealStatePtr L)
        {
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            try {
			
                Maria.Network.ClientSocket __cl_gen_to_be_invoked = (Maria.Network.ClientSocket)translator.FastGetCSObj(L, 1);
                translator.Push(L, __cl_gen_to_be_invoked.OnRecvUdp);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_OnSyncUdp(RealStatePtr L)
        {
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            try {
			
                Maria.Network.ClientSocket __cl_gen_to_be_invoked = (Maria.Network.ClientSocket)translator.FastGetCSObj(L, 1);
                translator.Push(L, __cl_gen_to_be_invoked.OnSyncUdp);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_ClintSockscript(RealStatePtr L)
        {
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            try {
			
                Maria.Network.ClientSocket __cl_gen_to_be_invoked = (Maria.Network.ClientSocket)translator.FastGetCSObj(L, 1);
                translator.Push(L, __cl_gen_to_be_invoked.ClintSockscript);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_OnAuthed(RealStatePtr L)
        {
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            try {
			
                Maria.Network.ClientSocket __cl_gen_to_be_invoked = (Maria.Network.ClientSocket)translator.FastGetCSObj(L, 1);
                __cl_gen_to_be_invoked.OnAuthed = translator.GetDelegate<Maria.Network.ClientSocket.AuthedCb>(L, 2);
            
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_OnConnected(RealStatePtr L)
        {
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            try {
			
                Maria.Network.ClientSocket __cl_gen_to_be_invoked = (Maria.Network.ClientSocket)translator.FastGetCSObj(L, 1);
                __cl_gen_to_be_invoked.OnConnected = translator.GetDelegate<Maria.Network.ClientSocket.ConnectedCb>(L, 2);
            
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_OnDisconnected(RealStatePtr L)
        {
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            try {
			
                Maria.Network.ClientSocket __cl_gen_to_be_invoked = (Maria.Network.ClientSocket)translator.FastGetCSObj(L, 1);
                __cl_gen_to_be_invoked.OnDisconnected = translator.GetDelegate<Maria.Network.ClientSocket.DisconnectedCb>(L, 2);
            
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_OnRecvUdp(RealStatePtr L)
        {
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            try {
			
                Maria.Network.ClientSocket __cl_gen_to_be_invoked = (Maria.Network.ClientSocket)translator.FastGetCSObj(L, 1);
                __cl_gen_to_be_invoked.OnRecvUdp = translator.GetDelegate<Maria.Network.PackageSocketUdp.RecvCB>(L, 2);
            
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_OnSyncUdp(RealStatePtr L)
        {
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            try {
			
                Maria.Network.ClientSocket __cl_gen_to_be_invoked = (Maria.Network.ClientSocket)translator.FastGetCSObj(L, 1);
                __cl_gen_to_be_invoked.OnSyncUdp = translator.GetDelegate<Maria.Network.PackageSocketUdp.SyncCB>(L, 2);
            
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_ClintSockscript(RealStatePtr L)
        {
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            try {
			
                Maria.Network.ClientSocket __cl_gen_to_be_invoked = (Maria.Network.ClientSocket)translator.FastGetCSObj(L, 1);
                __cl_gen_to_be_invoked.ClintSockscript = (Maria.Lua.ClientSock)translator.GetObject(L, 2, typeof(Maria.Lua.ClientSock));
            
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 0;
        }
        
		
		
		
		
    }
}
