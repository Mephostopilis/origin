using Maria;
using Maria.Network;

namespace Bacon
{
    public class AppContext : Context {
        private InitController _init = null;

        public AppContext(global::App app, Application application) : base(app, application) {
            _init = new InitController(this);
            _hash["init"] = _init;
            _hash["game"] = new GameController(this);

            _config = new AppConfig();
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
