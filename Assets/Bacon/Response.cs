using Bacon;
using Sproto;
using System;
using System.Text;
using UnityEngine;

namespace Maria.Network {
    class Response {
        private Context _ctx;
        private ClientSocket _cs;
        public Response(Context ctx, ClientSocket cs) {
            _ctx = ctx;
            _cs = cs;

            _cs.RegisterResponse(C2sProtocol.handshake.Tag, handshake);
            _cs.RegisterResponse(C2sProtocol.join.Tag, join);
            _cs.RegisterResponse(C2sProtocol.born.Tag, born);
            _cs.RegisterResponse(C2sProtocol.opcode.Tag, opcode);
        }

        public void handshake(uint session, SprotoTypeBase responseObj) {
            InitController controller = _ctx.GetController<InitController>("init");
            controller.Handshake(responseObj);
        }

        // 进入房间这个协议, authudp
        public void join(uint session, SprotoTypeBase responseObj) {
            C2sSprotoType.join.response o = responseObj as C2sSprotoType.join.response;
            _cs.AuthUdpCb(o.session, o.host, (int)o.port);
            GameController controller = _ctx.Top() as GameController;
            controller.Join(responseObj);
        }

        public void born(uint session, SprotoTypeBase responseObj) {
            GameController ctr = _ctx.Top() as GameController;
            ctr.Born(responseObj);
        }

        public void opcode(uint session, SprotoTypeBase responseObj) {
            GameController ctr = _ctx.Top() as GameController;
            ctr.OpCode(responseObj);
        }
    }
}
