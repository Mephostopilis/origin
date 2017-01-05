using System;
using System.Collections.Generic;
using System.Text;

namespace Maria
{
    public class User
    {
        public string Server { get; set; }
        public string Username { set; get; }
        public string Password { set; get; }
        public int Uid { get; set; }
        public int Subid { set; get; }
        public byte[] Secret { set; get; }
    }
}
