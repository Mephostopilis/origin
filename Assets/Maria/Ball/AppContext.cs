using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Maria.Network;

namespace Maria.Ball
{
    public class AppContext : Context
    {

        public AppContext(global::App app)
            : base(app)
        {
            TimeSync = new TimeSync();

            _hash["login"] = new LoginController(this);
            _hash["game"] = new GameController(this);
        }

        public TimeSync TimeSync { get; set; }

    }
}
