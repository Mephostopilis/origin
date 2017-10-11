using System;
using System.Collections.Generic;
using UnityEngine;
using XLua;
using Maria.Util;
using Bacon.GL.Start;
using Maria.Res;

namespace Bacon.GL.Util {

    [RequireComponent(typeof(Maria.Util.NotificationCenter))]
    [RequireComponent(typeof(Maria.Util.SoundMgr))]
    [RequireComponent(typeof(Maria.Res.ABLoader))]
    public class App : MonoBehaviour {

        [XLua.CSharpCallLua]
        public delegate Bacon.Lua.ILuaPool Main();

        [CSharpCallLua]
        public static List<Type> CSCallLuaModule {
            get {
                return new List<Type>() {
                    typeof(Action<Maria.Context>)
                };
            }
        }

        public static App current = null;

        public StartBehaviour _start = null;

        private XLua.LuaEnv _luaenv = null;
        private Bacon.App _app = null;

        void Awake() {
            if (current == null) {
                current = this;
            }
        }

        // Use this for initialization
        void Start() {
            DontDestroyOnLoad(this);
            StartScript();

            _app = new Bacon.App(_luaenv);
            _start.SetupRoot();
        }

        // Update is called once per frame
        void Update() {
            try {
                if (_app != null) {
                    _app.Update();
                }
            } catch (System.Exception ex) {
                UnityEngine.Debug.LogException(ex);
            }
        }

        void OnApplicationFocus(bool isFocus) {
            if (_app != null) {
                _app.OnApplicationFocus(isFocus);
            }
        }

        void OnApplicationPause(bool isPause) {
            if (_app != null) {
#if UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN
#else
            _app.OnApplicationPause(isPause);
#endif
            }
        }

        void OnApplicationQuit() {
            if (_app != null) {
                _app.OnApplicationQuit();
            }
            //ABLoader.current.Unload();
        }

        public void Enqueue(Maria.Command cmd) {
            UnityEngine.Debug.Assert(cmd != null);
            _app.Enqueue(cmd);
        }

        // android call c#
        public void Pipe(string code, string msg) {
            NotificationCenter.current.PostNotification(code, this, msg);
        }


        // second step
        public virtual void StartScript() {
            _luaenv = new XLua.LuaEnv();
            _luaenv.AddBuildin("cjson", Maria.Lua.BuildInInit.LoadCJson);
            _luaenv.AddBuildin("lpeg", Maria.Lua.BuildInInit.LoadLpeg);
            _luaenv.AddBuildin("sproto.core", Maria.Lua.BuildInInit.LoadSprotoCore);
            _luaenv.AddBuildin("ball", Maria.Lua.BuildInInit.LoadBall);
            _luaenv.AddLoader((ref string filepath) => {
                UnityEngine.Debug.LogFormat("LUA custom loader {0}", filepath);

                string[] xpaths = filepath.Split(new char[] { '.' });
                string path = "xlua/src";
                int idx = 0;
                while (idx + 1 < xpaths.Length) {
                    path += "/";
                    path += xpaths[idx];
                    idx++;
                }

                TextAsset file = ABLoader.current.LoadAsset<TextAsset>(path, xpaths[idx] + ".lua");
                if (file != null) {
                    return file.bytes;
                } else {
                    file = ABLoader.current.LoadAsset<TextAsset>(path + "/lualib", xpaths[idx] + ".lua");
                    if (file != null) {
                        return file.bytes;
                    }
                    return null;
                }
            });
            _luaenv.DoString(@" require 'main' ");
            Main main = _luaenv.Global.Get<Main>("main");
            Bacon.Lua.ILuaPool pool = main();
            Maria.Lua.LuaPool.Instance.Cache<Bacon.Lua.ILuaPool>(pool);
        }
    }
}