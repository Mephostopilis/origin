using Sproto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Maria.Network
{
    class Response
    {
        private Context _ctx;
        public Response(Context ctx)
        {
            _ctx = ctx;
        }

        public void role_info(uint session, SprotoTypeBase responseObj)
        {
        }
    }
}
