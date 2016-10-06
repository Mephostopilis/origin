using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Maria
{
    public class Config
    {
        protected string _loginIp;
        protected int _loginPort;
        protected string _gateIp;
        protected int _gatePort;

        public Config()
        {
            _loginIp = "192.168.199.239";
            _loginPort = 3002;
            _gateIp = "192.168.199.239";
            _gatePort = 3301;
        }

        public string LoginIp { get { return _loginIp; } }
        public int LoginPort { get { return _loginPort; } }
        public string GateIp { get { return _gateIp; } }
        public int GatePort { get { return _gatePort; } }
    }
}
