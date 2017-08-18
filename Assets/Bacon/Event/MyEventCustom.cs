using Maria;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bacon.Event {
    public class MyEventCustom : EventCustom {

        public static readonly string LOGOUT = "LOGOUT";

        public MyEventCustom(Context ctx, string name)
            : base(ctx, name) {
        }

        public MyEventCustom(Context ctx, string name, object ud) :
            base(ctx, name, ud) {
        }
    }
}
