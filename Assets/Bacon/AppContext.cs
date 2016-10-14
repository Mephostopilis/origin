using Maria;
using Maria.Network;

namespace Bacon
{
    public class AppContext : Context
    {
        private InitController _init = null;

        public AppContext(global::App app)
            : base(app)
        {
            _hash["game"] = new GameController(this);
            _init = new InitController(this);
            _hash["init"] = _init;
            _config = new AppConfig();
        }

        public override void Update(float delta)
        {
            // delta in seconds
            base.Update(delta);
            _init.Update(delta);
        }

    }
}
