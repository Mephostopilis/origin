using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Maria {
    public class Service {

        protected Context _ctx = null;

        public Service(Context ctx) {
            _ctx = ctx;
        }

        public virtual void Update(float delta) {}
    }
}
