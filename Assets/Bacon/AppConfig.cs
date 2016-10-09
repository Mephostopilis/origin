using Maria;

namespace Bacon
{
    class AppConfig : Config
    {
        public AppConfig()
            : base()
        {
            _loginIp = "192.168.199.239";
            _loginPort = 3002;
            _gateIp = "192.168.199.239";
            _gatePort = 3301;
        }
    }
}
