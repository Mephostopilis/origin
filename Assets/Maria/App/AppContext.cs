using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Maria.App
{
    public class AppContext : Context
    {
        public AppContext()
            :base()
        {
            GameController gctl = new GameController(this);
            _hash["game"] = gctl;
            LoginController lctl = new LoginController(this);
            _hash["login"] = lctl;
        }
    }
}
