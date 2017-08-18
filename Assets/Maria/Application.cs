using Maria.Network;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using UnityEngine;
using XLua;
using System.Reflection;
using Maria.Util;

namespace Maria {

    [XLua.Hotfix]
    [LuaCallCSharp]
    public class Application : DisposeObject {

        protected enum CoType {
            NONE = 0,
            THREAD = 1,
            CO = 2,
        }

        protected Maria.Util.App _app;
        protected CommandQueue _queue = new CommandQueue();
        protected Queue<Actor.RenderHandler> _renderQueue = new Queue<Actor.RenderHandler>();
        protected Semaphore _semaphore = null;
        protected Thread _worker = null;
        protected TimeSync _tiSync = null;
        protected int _lastTi;
        protected Context _ctx = null;
        protected EventDispatcher _dispatcher = null;
        protected CoType _cotype = CoType.THREAD;
        protected XLua.LuaEnv _luaenv = null;

        public Application(Maria.Util.App app) {
            _app = app;
            _tiSync = new TimeSync();
            _tiSync.LocalTime();
            _lastTi = _tiSync.LocalTime();

#if UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN
            //_cotype = CoType.THREAD;
            _cotype = CoType.CO;
#elif UNITY_IOS || UNITY_ANDROID
            _cotype = CoType.CO;
#endif
            if (_cotype == CoType.THREAD) {
                _semaphore = new Semaphore(1, 1);
                _worker = new Thread(new ThreadStart(Worker));
                _worker.IsBackground = true;
                _worker.Start();
                UnityEngine.Debug.LogWarning("create thread success.");
            } else {
                UnityEngine.Debug.LogWarning("create co success.");
            }
            _luaenv = new XLua.LuaEnv();
            _luaenv.AddBuildin("cjson", XLua.LuaDLL.Lua.LoadCJson);
            _luaenv.AddBuildin("lpeg", XLua.LuaDLL.Lua.LoadLpeg);
            _luaenv.AddBuildin("sproto.core", XLua.LuaDLL.Lua.LoadSprotoCore);
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
                    file = ABLoader.current.LoadAsset<TextAsset>(path+"/lualib", xpaths[idx] + ".lua");
                    if (file != null) {
                        return file.bytes;
                    }
                    return null;
                }
            });
            _luaenv.DoString(@"
require 'main'
");
        }

        protected override void Dispose(bool disposing) {
            if (_disposed) {
                return;
            }
            if (disposing) {
                // 清理托管资源，调用自己管理的对象的Dispose方法
                _ctx.Dispose();
                //_worker.Abort();
                //_luaenv.Dispose();
            }
            // 清理非托管资源
            _disposed = true;
        }

        public XLua.LuaEnv Env { get { return _luaenv; } }

        private void Worker() {
            while (true) {
                _semaphore.WaitOne();
                try {
                    if (_dispatcher != null) {
                        while (_queue.Count > 0) {
                            Command command = null;
                            lock (_queue) {
                                command = _queue.Dequeue();
                            }
                            _dispatcher.DispatchCmdEvent(command);
                        }
                    }

                    int now = _tiSync.LocalTime();
                    int delta = now - _lastTi;
                    _lastTi = now;

                    if (_ctx != null) {
                        _ctx.Update(((float)delta) / 100.0f);
                    }
                } catch (Exception ex) {
                    UnityEngine.Debug.LogException(ex);
                }

                //_tiSync.Sleep(10);
                _semaphore.Release();
            }
        }

        IEnumerator Co(Command cmd) {
            try {
                _dispatcher.DispatchCmdEvent(cmd);
            } catch (Exception ex) {
                UnityEngine.Debug.LogException(ex);
            }
            yield break;
        }

        IEnumerator CoHandler(Actor.RenderHandler handler) {
            try {
                handler();
            } catch (Exception ex) {
                UnityEngine.Debug.LogException(ex);
            }
            yield break;
        }

        private void CoWorker() {
            for (int i = 0; i < 1; i++) {
                if (_dispatcher != null) {
                    while (_queue.Count > 0) {
                        Command command = _queue.Dequeue();
                        _app.StartCoroutine(Co(command));
                    }
                }

                int now = _tiSync.LocalTime();
                int delta = now - _lastTi;
                _lastTi = now;

                try {
                    if (_ctx != null) {
                        _ctx.Update(((float)delta) / 100.0f);
                    }
                } catch (Exception ex) {
                    UnityEngine.Debug.LogException(ex);
                }
            }
        }

        public void Enqueue(Command cmd) {
            if (_cotype == CoType.THREAD) {
                lock (_queue) {
                    _queue.Enqueue(cmd);
                }
            } else {
                _queue.Enqueue(cmd);
            }
        }

        public void EnqueueRenderQueue(Actor.RenderHandler handler) {
            if (_cotype == CoType.THREAD) {
                lock (_renderQueue) {
                    _renderQueue.Enqueue(handler);
                }
            } else if (_cotype == CoType.CO) {
                _renderQueue.Enqueue(handler);
            }
        }

        // Update is called once per frame
        public virtual void Update() {
            _luaenv.Tick();

            if (_cotype == CoType.CO) {
                CoWorker();
                while (_renderQueue.Count > 0) {
                    Actor.RenderHandler handler = _renderQueue.Dequeue();
                    _app.StartCoroutine(CoHandler(handler));
                    //handler();
                }
            } else {
                // 此段代码可以用协程
                while (_renderQueue.Count > 0) {
                    Actor.RenderHandler handler = null;
                    lock (_renderQueue) {
                        handler = _renderQueue.Dequeue();
                    }
                    handler();
                }
            }
        }

        public void OnApplicationFocus(bool hasFocus) {
            //if (isFocus) {
            //    if (_worker.IsAlive) {

            //    }
            //    _worker.A
            //} else {
            //    UnityEngine.Debug.Log("离开游戏 激活推送");  //  返回游戏的时候触发     执行顺序 1  
            //}
        }

        public void OnApplicationPause(bool pauseStatus) {
            if (pauseStatus) {
                UnityEngine.Debug.Log("游戏暂停 一切停止");  // 缩到桌面的时候触发  
                if (_cotype == CoType.THREAD) {
                    _semaphore.WaitOne();
                }
            } else {
                UnityEngine.Debug.Log("游戏开始  万物生机");  //回到游戏的时候触发 最晚  
                if (_cotype == CoType.THREAD) {
                    _semaphore.Release();
                }
            }
        }

        public void OnApplicationQuit() {
            if (_cotype == CoType.THREAD) {
                _worker.Abort();
            }
            Dispose(true);
        }
    }
}
