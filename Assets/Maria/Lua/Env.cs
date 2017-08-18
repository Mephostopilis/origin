using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Maria.Lua {
    [XLua.CSharpCallLua]
    public interface Env {
        //void ctor(Context ctx);
        ClientSock clientsock();
        void update();
        //int f1 { get; set; }
        //int f2 { get; set; }
        //int add(int a, int b);
    }
}
