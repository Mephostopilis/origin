using Maria;

namespace Maria.App
{
    class AppConfig : Config
    {
        public AppConfig()
        {
            _loginIp = "192.168.1.103";
            _loginPort = 3002;
            _gateIp = "192.168.1.103";
            _gatePort = 3301;
        }
    }
}
