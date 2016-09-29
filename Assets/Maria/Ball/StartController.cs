using UnityEngine;

namespace Maria.Ball
{
    public class StartController : Controller
    {

        public StartController(Context ctx) : base(ctx)
        {
        }

        public override void Enter()
        {
            _ctx.Countdown("startcontroller", 2, CountdownCb);
        }

        public override void Exit()
        {
        }

        public void CountdownCb()
        {
            Debug.Log("hello word.");
            _ctx.Push("login");
        }
    }
}
