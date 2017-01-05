﻿using Maria;

namespace Bacon
{
    class AppConfig : Config
    {
        public AppConfig()
            : base()
        {
            _loginIp = "192.168.1.132";
            _loginPort = 3002;
            _gateIp = "192.168.1.132";
            _gatePort = 3301;

            c2s = C2sProtocol.Instance;
            s2c = S2cProtocol.Instance;
        }
    }
}
