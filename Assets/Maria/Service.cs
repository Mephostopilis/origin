using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Maria {
    public class Service {

        protected Context _ctx = null;
        private Hashtable _hasht = new Hashtable();

        public Service(Context ctx) {
            _ctx = ctx;
        }

        public virtual void Update(float delta) {}

        public object this[string key] {
            get {
                return _hasht[key];
            }
        }

    }
}
