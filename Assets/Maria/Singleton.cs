using UnityEngine;
using System;
using System.Collections;
using System.Threading;

namespace Maria {

    //public class Singleton<T> where T : class, new() {
    public class Singleton<T> where T : new() {
        protected static T _instance;
        protected static object _lock = new object();

        public static T Instance {
            get {
                object obj;
                Monitor.Enter(obj = _lock);
                try {
                    if (_instance == null) {
                        _instance = default(T);
                        if (_instance == null) {
                            _instance = Activator.CreateInstance<T>();
                        }
                    }
                } finally {
                    Monitor.Exit(obj);
                }
                return _instance;
            }
        }

    }
}
