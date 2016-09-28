using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Maria.Network;

namespace Maria.Ball
{
    public class AppContext : Context
    {
        
        public AppContext()
            : base()
        {
            TimeSync = new TimeSync();
        }

        public TimeSync TimeSync { get; set; }
        
    }
}
