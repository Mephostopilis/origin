using Maria;
using Maria.Network;

namespace Bacon
{
    public class AppContext : Context {
        private InitController _init = null;
        private Request _request = null;
        private Response _response = null;

        public AppContext(Application application, Config config) : base(application, config) {
            _init = new InitController(this);
            _hash["init"] = _init;
            _hash["login"] = new LoginController(this);
            _hash["game"] = new GameController(this);

            _request = new Request(this, _client);
            _response = new Response(this, _client);
        }

        public override void Update(float delta) {
            // delta in seconds
            base.Update(delta);
            _init.Update(delta);
        }

        public SMActor SMActor {
            get {
                InitController controller = _hash["init"] as InitController;
                if (controller != null) {
                    return controller.SMActor;
                } else {
                    return null;
                }
            }
        }
    }
}
