using Sproto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

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

        public void join(uint session, SprotoTypeBase responseObj)
        {
            if (responseObj != null)
            {
                C2sSprotoType.join.response o = responseObj as C2sSprotoType.join.response;

                string host = o.host;
                int port = (int)o.port;
                _ctx.ConnectUdp(o.session, host, port);
            }
        }

        public void handshake(uint session, SprotoTypeBase responseObj)
        {
            Debug.Log("handshake");
        }
    }
}
