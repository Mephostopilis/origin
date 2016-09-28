using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Maria
{
    public class User
    {
        public string Server { get; set; }
        public string Username { set; get; }
        public string Password { set; get; }
        public byte[] Uid { get; set; }
        public byte[] Secret { set; get; }
        public byte[] Subid { set; get; }
    }
}
