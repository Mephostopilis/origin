using Maria.Lua;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bacon.Lua {

    [XLua.CSharpCallLua]
    public interface ILuaPool {
        ILuaContext CreateContext();
        ILuaClientSock CreateClientSock();
        ILuaGameController CreateGameController();
    }
}
