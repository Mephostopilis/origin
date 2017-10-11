using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Maria.Lua {
    class LuaPool : Singleton<LuaPool> {
        private Dictionary<Type, List<object>> _pool = new Dictionary<Type, List<object>>();
        private Dictionary<Type, object> _cache = new Dictionary<Type, object>();

        public T Create<T>(params object[] args) where T : class, ILua {
            Type t = typeof(T);
            List<object> pool = null;
            if (_pool.ContainsKey(t)) {
                pool = _pool[t];
            } else {
                pool = new List<object>();
            }
            if (pool.Count > 0) {
                T res = pool[0] as T;
                pool.RemoveAt(0);
                res.OnCreateLua();
                return res;
            } else {
                T res = Activator.CreateInstance(typeof(T), args) as T;
                res.OnCreateLua();
                return res;
            }
        }

        public object Create(Type t, params object[] args) {
            List<object> pool = null;
            if (_pool.ContainsKey(t)) {
                pool = _pool[t];
            } else {
                pool = new List<object>();
            }
            if (pool.Count > 0) {
                ILua res = pool[0] as ILua;
                pool.RemoveAt(0);
                res.OnCreateLua();
                return res;
            } else {
                ILua res = Activator.CreateInstance(t, args) as ILua;
                res.OnCreateLua();
                return res;
            }
        }

        public T Cache<T>(T value) {
            Type t = typeof(T);
            if (value == null) {
                if (_cache.ContainsKey(t)) {
                    return (T)_cache[t];
                }
                return default(T);
            } else {
                _cache[t] = value;
                return value;
            }
        }

    }
}
