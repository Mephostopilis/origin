using UnityEngine;
using System.Collections.Generic;
using XLua;
using System;
using Bacon.GL.Common;

namespace Maria.Util {
    public class App : MonoBehaviour {

        [CSharpCallLua]
        public static List<Type> CSCallLuaModule {
            get {
                return new List<Type>() {
                    typeof(Action<Maria.Context>)
                };
            }
        }

        public class Notification {

            public Notification(string name, UnityEngine.Component sender) {
                this.name = name;
                this.sender = sender;
                this.data = null;
            }
            public Notification(string name, UnityEngine.Component sender, object data) {
                this.name = name;
                this.sender = sender;
                this.data = data;
            }

            public string name {
                get; set;
            }                                              //存储委托存在字典中的名字  
            public UnityEngine.Component sender { get; set; }          //注册的用户组件  
            public object data { get; set; }               //存放的信息  
        }

        public static App current = null;

        public StartBehaviour _start = null;
        private Bacon.App _app = null;
        private Dictionary<string, Action<Notification>> _dic = null;

        void Awake() {
            if (current == null) {
                current = this;
            }
        }

        // Use this for initialization
        void Start() {
            DontDestroyOnLoad(this);
            _app = new Bacon.App(this);
            _dic = new Dictionary<string, Action<Notification>>();

            if (_start != null) {
                _start.SetupStartRoot();
            } else {
                throw new System.Exception("not imple");
            }
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
            PostNotification(code, this, msg);
        }

        //添加观察者的方法  
        public void AddObserver(string name, Action<Notification> action) {
            if (_dic.ContainsKey(name)) {
                _dic[name] += action;                       //若字典存在该key则将对应的委托添加传来的委托  
            } else {
                _dic.Add(name, action);                     //不存在则添加该key和传来的委托  
            }
        }

        //移除观察者的方法  
        public void RemoveObserver(string name, Action<Notification> action) {
            if (_dic.ContainsKey(name)) {
                _dic[name] -= action;
                if (_dic[name] == null) {
                    _dic.Remove(name);
                }
            }
        }

        //传递消息的方法  
        public void PostNotification(string name, UnityEngine.Component sender) {
            PostNotification(name, sender, null);
        }

        public void PostNotification(string name, UnityEngine.Component sender, object data) {
            PostNotification(new Notification(name, sender, null));
        }

        public void PostNotification(Notification notification) {
            if (_dic.ContainsKey(notification.name)) {
                _dic[notification.name](notification);
            }
        }
    }
}