using Maria;
using Maria.Network;
using Sproto;

namespace Bacon {
    class Request {
        private Context _ctx;
        private ClientSocket _cs;
        public Request(Context ctx, ClientSocket cs) {
            _ctx = ctx;
            _cs = cs;

            _cs.RegisterRequest(S2cProtocol.handshake.Tag, handshake);
            _cs.RegisterRequest(S2cProtocol.join.Tag, join);
            _cs.RegisterRequest(S2cProtocol.born.Tag, born);
            _cs.RegisterRequest(S2cProtocol.leave.Tag, leave);
            _cs.RegisterRequest(S2cProtocol.die.Tag, die);
        }

        public SprotoTypeBase handshake(uint session, SprotoTypeBase requestObj) {
            S2cSprotoType.handshake.response responseObj = new S2cSprotoType.handshake.response();
            responseObj.errorcode = Errorcode.SUCCESS;
            return responseObj;
        }

        public SprotoTypeBase join(uint session, SprotoTypeBase requestObj) {
            GameController controller = _ctx.Top() as GameController;
            return controller.OnJoin(requestObj);
        }

        public SprotoTypeBase born(uint session, SprotoTypeBase requestObj) {
            GameController controller = _ctx.Top() as GameController;
            return controller.OnBorn(requestObj);
        }

        public SprotoTypeBase leave(uint session, SprotoTypeBase requestObj) {
            GameController controller = _ctx.Top() as GameController;
            return controller.OnLeave(requestObj);
        }

        public SprotoTypeBase die(uint session, SprotoTypeBase requestObj) {
            GameController controller = _ctx.Top() as GameController;
            return controller.OnDie(requestObj);
        }
    }
}
