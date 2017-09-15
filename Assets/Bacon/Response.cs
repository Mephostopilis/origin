using Bacon;
using Bacon.Service;
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
            _cs.RegisterResponse(C2sProtocol.match.Tag, match);
            //_cs.RegisterResponse(C2sProtocol.join.Tag, join);
            ////_cs.RegisterResponse(C2sProtocol.born.Tag, born);
            ////_cs.RegisterResponse(C2sProtocol.opcode.Tag, opcode);

        }

        public void handshake(uint session, SprotoTypeBase responseObj, object ud) {
            InitService service = (InitService)_ctx.QueryService("init");
            if (service != null) {
                service.OnRspHandshake(responseObj);
            }
        }

        public void match(uint session, SprotoTypeBase responseObj, object ud) {
            MainController controller = _ctx.Peek<MainController>();
            controller.OnRspMatch(responseObj);
        }

        // 进入房间这个协议, authudp
        public void join(uint session, SprotoTypeBase responseObj, object ud) {
            C2sSprotoType.join.response o = responseObj as C2sSprotoType.join.response;
            //GameService service = (GameService)_ctx.QueryService(GameService.Name);
            //service.Join(responseObj);
        }

        public void born(uint session, SprotoTypeBase responseObj, object ud) {
            //GameController ctr = _ctx.Top() as GameController;
            //ctr.Born(responseObj);
        }

        public void opcode(uint session, SprotoTypeBase responseObj, object ud) {
            //GameController ctr = _ctx.Top() as GameController;
            //ctr.OpCode(responseObj);
        }

        
    }
}
