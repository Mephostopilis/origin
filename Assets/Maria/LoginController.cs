using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Maria
{
    class LoginController : Controller
    {
        public LoginController(Context ctx) : base(ctx)
        {
        }

        public override void Enter()
        {
            base.Enter();
            LoadScene("login");
        }

        public override void Exit()
        {
            base.Exit();
        }
    }
}
