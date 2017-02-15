using Maria.Network;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using UnityEngine;

namespace Maria {
    public class Application : DisposeObject {

        protected CommandQueue _queue = new CommandQueue();
        protected Queue<Actor.RenderHandler> _renderQueue = new Queue<Actor.RenderHandler>();
        protected Semaphore _semaphore = null;
        protected Thread _worker = null;
        protected TimeSync _tiSync = null;
        protected int _lastTi;

        protected Context _ctx = null;
        protected EventDispatcher _dispatcher = null;

        public Application() {
            _tiSync = new TimeSync();
            _tiSync.LocalTime();
            _lastTi = _tiSync.LocalTime();

            _semaphore = new Semaphore(1, 1);
            _worker = new Thread(new ThreadStart(Worker));
            _worker.IsBackground = true;
            _worker.Start();

        }

        protected override void Dispose(bool disposing) {
            if (_disposed) {
                return;
            }
            if (disposing) {
                // 清理托管资源，调用自己管理的对象的Dispose方法
                _ctx.Dispose();
                //_worker.Abort();
            }
            // 清理非托管资源

            _disposed = true;
        }

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
                    Debug.LogException(ex);
                }

                //_tiSync.Sleep(10);
                _semaphore.Release();
            }
        }

        public void Enqueue(Command cmd) {
            lock (_queue) {
                _queue.Enqueue(cmd);
            }
        }

        public void EnqueueRenderQueue(Actor.RenderHandler handler) {
            lock (_renderQueue) {
                _renderQueue.Enqueue(handler);
            }
        }

        // Update is called once per frame
        public void Update() {
            while (_renderQueue.Count > 0) {
                Actor.RenderHandler handler = null;
                lock (_renderQueue) {
                    handler = _renderQueue.Dequeue();
                }
                handler();
            }
        }

        public void OnApplicationFocus(bool hasFocus) {
            //if (isFocus) {
            //    if (_worker.IsAlive) {

            //    }
            //    _worker.A
            //} else {
            //    Debug.Log("离开游戏 激活推送");  //  返回游戏的时候触发     执行顺序 1  
            //}
        }

        public void OnApplicationPause(bool pauseStatus) {
            if (pauseStatus) {
                //_worker.
                //Debug.Log("游戏暂停 一切停止");  // 缩到桌面的时候触发  
                _semaphore.WaitOne();
            } else {
                //Debug.Log("游戏开始  万物生机");  //回到游戏的时候触发 最晚  
                _semaphore.Release();
            }
        }

        public void OnApplicationQuit() {
            _worker.Abort();
            Dispose(true);
        }
    }
}
