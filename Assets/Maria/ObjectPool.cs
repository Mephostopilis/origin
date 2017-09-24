using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace Maria {
    class ObjectPool {
        public enum CSType {
            OBJECT,
            FUNCTION,
            INTPTR,
            INT32,
        }

        public class CSObject {
            public CSObject() {
            }

            public CSType Type { get; set; }
            public WeakReference obj { get; set; }

            //public object obj { get; set; }
            //public IntPtr ptr { get; set; }
            //public Int32  v32 { get; set; }

        }

        private CSObject[] _slots = new CSObject[512];
        private int _count = -1;

        public ObjectPool() { }

        public CSObject this[int i] {
            get {
                if (i >= 0 && i < _count) {
                    return _slots[i];
                }
                return null;
            }
        }

        public Object GetValue(int i) {
            if (i >= 0 && i < _count) {
                return _slots[i].obj;
            }
            return null;
        }

        public bool TryGetValue(int i, out Object obj) {
            if (i >= 0 && i < _count) {
                CSObject csobj = _slots[i];
                if (Object.ReferenceEquals(csobj.obj, null)) {
                    obj = null;
                    return false;
                }
                obj = csobj.obj;
                return true;
            }
            obj = null;
            return false;
        }

        public int Add(CSType t, Object obj) {
            _count++;
            _slots[_count].Type = t;
            _slots[_count].obj = new WeakReference(obj);
            return _count;
        }

        public void Remove(int i) {

        }
    }
}
