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
    public class BaconMainControllerWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(Bacon.MainController);
			Utils.BeginObjectRegister(type, L, translator, 0, 31, 0, 0);
			
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "OnEnter", _m_OnEnter);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "OnExit", _m_OnExit);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "OnGateAuthed", _m_OnGateAuthed);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "OnGateDisconnected", _m_OnGateDisconnected);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "Logout", _m_Logout);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "OnCreateLua", _m_OnCreateLua);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "OnDestroyLua", _m_OnDestroyLua);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "SetupUI", _m_SetupUI);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "OnSendMatch", _m_OnSendMatch);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "OnSendMsg", _m_OnSendMsg);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "OnSendViewMail", _m_OnSendViewMail);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "OnViewedMail", _m_OnViewedMail);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "OnMsgClosed", _m_OnMsgClosed);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "OnSendCreate", _m_OnSendCreate);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "OnSendRecords", _m_OnSendRecords);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "OnLogout", _m_OnLogout);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "OnReqMatch", _m_OnReqMatch);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "OnRspMatch", _m_OnRspMatch);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "SyncSysmail", _m_SyncSysmail);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "Records", _m_Records);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "Record", _m_Record);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "RenderSetupUI", _m_RenderSetupUI);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "RenderFetchSysmail", _m_RenderFetchSysmail);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "RenderShowCreate", _m_RenderShowCreate);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "RenderSyncSysMail", _m_RenderSyncSysMail);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "RenderViewMail", _m_RenderViewMail);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "RenderViewedMail", _m_RenderViewedMail);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "RenderCancelWaiting", _m_RenderCancelWaiting);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "RenderWaiting", _m_RenderWaiting);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "RenderShowTips", _m_RenderShowTips);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "RenderFirst", _m_RenderFirst);
			
			
			
			
			
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
				if(LuaAPI.lua_gettop(L) == 2 && translator.Assignable<Maria.Context>(L, 2))
				{
					Maria.Context ctx = (Maria.Context)translator.GetObject(L, 2, typeof(Maria.Context));
					
					Bacon.MainController __cl_gen_ret = new Bacon.MainController(ctx);
					translator.Push(L, __cl_gen_ret);
                    
					return 1;
				}
				
			}
			catch(System.Exception __gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to Bacon.MainController constructor!");
            
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_OnEnter(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Bacon.MainController __cl_gen_to_be_invoked = (Bacon.MainController)translator.FastGetCSObj(L, 1);
            
            
                
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
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Bacon.MainController __cl_gen_to_be_invoked = (Bacon.MainController)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    __cl_gen_to_be_invoked.OnExit(  );
                    
                    
                    
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
            
            
                Bacon.MainController __cl_gen_to_be_invoked = (Bacon.MainController)translator.FastGetCSObj(L, 1);
            
            
                
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
            
            
                Bacon.MainController __cl_gen_to_be_invoked = (Bacon.MainController)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    __cl_gen_to_be_invoked.OnGateDisconnected(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Logout(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Bacon.MainController __cl_gen_to_be_invoked = (Bacon.MainController)translator.FastGetCSObj(L, 1);
            
            
			    int __gen_param_count = LuaAPI.lua_gettop(L);
            
                if(__gen_param_count == 1) 
                {
                    
                    __cl_gen_to_be_invoked.Logout(  );
                    
                    
                    
                    return 0;
                }
                if(__gen_param_count == 2&& translator.Assignable<Sproto.SprotoTypeBase>(L, 2)) 
                {
                    Sproto.SprotoTypeBase responseObj = (Sproto.SprotoTypeBase)translator.GetObject(L, 2, typeof(Sproto.SprotoTypeBase));
                    
                    __cl_gen_to_be_invoked.Logout( responseObj );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to Bacon.MainController.Logout!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_OnCreateLua(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Bacon.MainController __cl_gen_to_be_invoked = (Bacon.MainController)translator.FastGetCSObj(L, 1);
            
            
                
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
            
            
                Bacon.MainController __cl_gen_to_be_invoked = (Bacon.MainController)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    __cl_gen_to_be_invoked.OnDestroyLua(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SetupUI(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Bacon.MainController __cl_gen_to_be_invoked = (Bacon.MainController)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    Maria.EventCmd e = (Maria.EventCmd)translator.GetObject(L, 2, typeof(Maria.EventCmd));
                    
                    __cl_gen_to_be_invoked.SetupUI( e );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_OnSendMatch(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Bacon.MainController __cl_gen_to_be_invoked = (Bacon.MainController)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    Maria.EventCmd e = (Maria.EventCmd)translator.GetObject(L, 2, typeof(Maria.EventCmd));
                    
                    __cl_gen_to_be_invoked.OnSendMatch( e );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_OnSendMsg(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Bacon.MainController __cl_gen_to_be_invoked = (Bacon.MainController)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    Maria.EventCmd e = (Maria.EventCmd)translator.GetObject(L, 2, typeof(Maria.EventCmd));
                    
                    __cl_gen_to_be_invoked.OnSendMsg( e );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_OnSendViewMail(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Bacon.MainController __cl_gen_to_be_invoked = (Bacon.MainController)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    Maria.EventCmd e = (Maria.EventCmd)translator.GetObject(L, 2, typeof(Maria.EventCmd));
                    
                    __cl_gen_to_be_invoked.OnSendViewMail( e );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_OnViewedMail(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Bacon.MainController __cl_gen_to_be_invoked = (Bacon.MainController)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    Maria.EventCmd e = (Maria.EventCmd)translator.GetObject(L, 2, typeof(Maria.EventCmd));
                    
                    __cl_gen_to_be_invoked.OnViewedMail( e );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_OnMsgClosed(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Bacon.MainController __cl_gen_to_be_invoked = (Bacon.MainController)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    Maria.EventCmd e = (Maria.EventCmd)translator.GetObject(L, 2, typeof(Maria.EventCmd));
                    
                    __cl_gen_to_be_invoked.OnMsgClosed( e );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_OnSendCreate(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Bacon.MainController __cl_gen_to_be_invoked = (Bacon.MainController)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    Maria.EventCmd e = (Maria.EventCmd)translator.GetObject(L, 2, typeof(Maria.EventCmd));
                    
                    __cl_gen_to_be_invoked.OnSendCreate( e );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_OnSendRecords(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Bacon.MainController __cl_gen_to_be_invoked = (Bacon.MainController)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    Maria.EventCmd e = (Maria.EventCmd)translator.GetObject(L, 2, typeof(Maria.EventCmd));
                    
                    __cl_gen_to_be_invoked.OnSendRecords( e );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_OnLogout(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Bacon.MainController __cl_gen_to_be_invoked = (Bacon.MainController)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    Maria.EventCmd e = (Maria.EventCmd)translator.GetObject(L, 2, typeof(Maria.EventCmd));
                    
                    __cl_gen_to_be_invoked.OnLogout( e );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_OnReqMatch(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Bacon.MainController __cl_gen_to_be_invoked = (Bacon.MainController)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    Sproto.SprotoTypeBase requestObj = (Sproto.SprotoTypeBase)translator.GetObject(L, 2, typeof(Sproto.SprotoTypeBase));
                    
                        Sproto.SprotoTypeBase __cl_gen_ret = __cl_gen_to_be_invoked.OnReqMatch( requestObj );
                        translator.Push(L, __cl_gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_OnRspMatch(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Bacon.MainController __cl_gen_to_be_invoked = (Bacon.MainController)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    Sproto.SprotoTypeBase responseObj = (Sproto.SprotoTypeBase)translator.GetObject(L, 2, typeof(Sproto.SprotoTypeBase));
                    
                    __cl_gen_to_be_invoked.OnRspMatch( responseObj );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SyncSysmail(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Bacon.MainController __cl_gen_to_be_invoked = (Bacon.MainController)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    Sproto.SprotoTypeBase responseObj = (Sproto.SprotoTypeBase)translator.GetObject(L, 2, typeof(Sproto.SprotoTypeBase));
                    
                    __cl_gen_to_be_invoked.SyncSysmail( responseObj );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Records(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Bacon.MainController __cl_gen_to_be_invoked = (Bacon.MainController)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    Sproto.SprotoTypeBase responseObj = (Sproto.SprotoTypeBase)translator.GetObject(L, 2, typeof(Sproto.SprotoTypeBase));
                    
                    __cl_gen_to_be_invoked.Records( responseObj );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Record(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Bacon.MainController __cl_gen_to_be_invoked = (Bacon.MainController)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    Sproto.SprotoTypeBase responseObj = (Sproto.SprotoTypeBase)translator.GetObject(L, 2, typeof(Sproto.SprotoTypeBase));
                    
                    __cl_gen_to_be_invoked.Record( responseObj );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_RenderSetupUI(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Bacon.MainController __cl_gen_to_be_invoked = (Bacon.MainController)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    __cl_gen_to_be_invoked.RenderSetupUI(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_RenderFetchSysmail(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Bacon.MainController __cl_gen_to_be_invoked = (Bacon.MainController)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    __cl_gen_to_be_invoked.RenderFetchSysmail(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_RenderShowCreate(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Bacon.MainController __cl_gen_to_be_invoked = (Bacon.MainController)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    __cl_gen_to_be_invoked.RenderShowCreate(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_RenderSyncSysMail(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Bacon.MainController __cl_gen_to_be_invoked = (Bacon.MainController)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    __cl_gen_to_be_invoked.RenderSyncSysMail(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_RenderViewMail(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Bacon.MainController __cl_gen_to_be_invoked = (Bacon.MainController)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    __cl_gen_to_be_invoked.RenderViewMail(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_RenderViewedMail(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Bacon.MainController __cl_gen_to_be_invoked = (Bacon.MainController)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    __cl_gen_to_be_invoked.RenderViewedMail(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_RenderCancelWaiting(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Bacon.MainController __cl_gen_to_be_invoked = (Bacon.MainController)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    __cl_gen_to_be_invoked.RenderCancelWaiting(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_RenderWaiting(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Bacon.MainController __cl_gen_to_be_invoked = (Bacon.MainController)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    __cl_gen_to_be_invoked.RenderWaiting(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_RenderShowTips(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Bacon.MainController __cl_gen_to_be_invoked = (Bacon.MainController)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    __cl_gen_to_be_invoked.RenderShowTips(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_RenderFirst(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Bacon.MainController __cl_gen_to_be_invoked = (Bacon.MainController)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    __cl_gen_to_be_invoked.RenderFirst(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        
        
        
        
        
		
		
		
		
    }
}
