using Maria;

namespace Bacon {
    class AppConfig : Config {
        public enum VERSION_TYPE {
            TEST_LINUX,
            TEST_WIN,
            DEV,
            PUBLIC,
        }

        public AppConfig() : base() {

            VTYPE = VERSION_TYPE.TEST_WIN;
            UpdateRes = true;
            if (VTYPE == VERSION_TYPE.PUBLIC) {
                _loginIp = "120.76.248.223";
                _loginPort = 3002;
                _gateIp = "120.76.248.223";
                _gatePort = 3301;

            } else if (VTYPE == VERSION_TYPE.TEST_LINUX) {
                _loginIp = "192.168.1.123";
                _loginPort = 3002;
                _gateIp = "192.168.1.123";
                _gatePort = 3301;
            } else if (VTYPE == VERSION_TYPE.TEST_WIN) {
                _loginIp = "127.0.0.1";
                _loginPort = 3002;
                _gateIp = "127.0.0.1";
                _gatePort = 3301;
            }

            c2s = C2sProtocol.Instance;
            s2c = S2cProtocol.Instance;
        }

        public VERSION_TYPE VTYPE { get; set; }
        public bool UpdateRes { get; set; }

    }
}
