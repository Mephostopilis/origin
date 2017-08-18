using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Maria {
    public class Service {

        protected Context _ctx = null;
        protected List<Actor> _actors = new List<Actor>();
        protected Hashtable _hasht = new Hashtable();

        public Service(Context ctx) {
            _ctx = ctx;
        }

        public virtual void Update(float delta) {
            foreach (var item in _actors) {
                item.Update(delta);
            }
        }

        public void Add(Actor item) {
            _actors.Add(item);
        }

        public bool Remove(Actor item) {
            return _actors.Remove(item);
        }

        public object this[string key] {
            get {
                return _hasht[key];
            }
        }

    }
}
