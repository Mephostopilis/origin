﻿using Bacon;
using Sproto;
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
                _ctx.AuthUdpCb(o.session, host, port);
            }
        }

        public void handshake(uint session, SprotoTypeBase responseObj)
        {
            C2sSprotoType.handshake.response o = responseObj as C2sSprotoType.handshake.response;
            Debug.Log(string.Format("handshake {0}", o.errorcode));
        }

        public void born(uint session, SprotoTypeBase responseObj)
        {
            GameController ctr = _ctx.GetController<GameController>("game");
            //ctr.Born()
        }
    }
}
