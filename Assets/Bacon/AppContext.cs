using Bacon.Service;
using Maria;
using Maria.Network;

namespace Bacon
{
    public class AppContext : Context {
        
        private InitService _initService = null;
        private Request _request = null;
        private Response _response = null;
        
        public AppContext(Application application, Config config, TimeSync ts) : base(application, config, ts) {

            _user.AddModule<Bacon.Modules.BattleScene>();

            _request = new Request(this, _client);
            _response = new Response(this, _client);

            RegService(InitService.Name, new InitService(this));
            //RegService(GameService.Name, new GameService(this));
            //RegService(PlayService.Name, new PlayService(this));

            Push(typeof(StartController));
        }

        public Maria.Util.App GApp { get { return ((App)_application).GApp; } }

    }
}
