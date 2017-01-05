using Maria;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Bacon {
    public class MyEventCustom : EventCustom {
        public MyEventCustom(string name)
            : base(name) {
        }

        public MyEventCustom(string name, object ud) :
            base(name, ud) {
        }
    }
}
