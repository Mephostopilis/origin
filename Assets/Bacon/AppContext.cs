using Maria;
using Maria.Network;

namespace Bacon
{
    public class AppContext : Context {
        
        private InitService _initService = null;
        private Request _request = null;
        private Response _response = null;

        public AppContext(Application application, Config config, TimeSync ts) : base(application, config, ts) {
            _hash["start"] = new StartController(this);
            _hash["login"] = new LoginController(this);
            _hash["main"] = new MainController(this);
            _hash["game"] = new GameController(this);

            RegService(InitService.Name, new InitService(this));
            RegService(GameService.Name, new GameService(this));

            _request = new Request(this, _client);
            _response = new Response(this, _client);

            Push("start");
        }

        public global::App GApp { get { return ((App)_application).GApp; } }

    }
}
