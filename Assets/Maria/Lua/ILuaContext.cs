using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Maria.Lua {
    [XLua.CSharpCallLua]
    public interface ILuaContext {
        
        void Update(float delta);
        Controller Peek();
        void Push(Controller controller);
        void Pop();
    }
}
