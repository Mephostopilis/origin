
using System;
using System.Collections.Generic;
using UnityEngine;
using XLua;
using Maria.Util;
using Bacon.GL.Start;

namespace Bacon.Util {

    [RequireComponent(typeof(Maria.Util.NotificationCenter))]
    [RequireComponent(typeof(Maria.Util.SoundMgr))]
    [RequireComponent(typeof(Maria.Res.ABLoader))]
    public class App : MonoBehaviour {

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
        private Bacon.App _app = null;

        void Awake() {
            if (current == null) {
                current = this;
            }
        }

        // Use this for initialization
        void Start() {
            DontDestroyOnLoad(this);
            _app = new Bacon.App();
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
    }
}