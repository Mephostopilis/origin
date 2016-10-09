using Maria;
using Maria.Network;

namespace Bacon
{
    public class AppContext : Context
    {
        public AppContext(global::App app)
            : base(app)
        {
            _hash["game"] = new GameController(this);
            _config = new AppConfig();
        }

        public override void Update(float delta)
        {
            // delta in seconds
            base.Update(delta);

        }

    }
}
