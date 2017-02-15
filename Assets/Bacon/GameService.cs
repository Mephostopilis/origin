using Maria;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using Sproto;

namespace Bacon  {
    class GameService : Service {

        public static readonly string Name = "game";

        private long _mysession = 0;
        private Dictionary<long, Player> _playes = new Dictionary<long, Player>();

        public GameService(Context ctx) : base(ctx) {
        }

        public void AddPlayer(long session, Player player) {
            _playes[session] = player;
        }

        public void AddMyPlayer(long session, Player player) {
            _mysession = session;
            _playes[session] = player;
        }

        public int RoomId { get; set; }

        public void Join(SprotoTypeBase responseObj) {
            C2sSprotoType.join.response obj = responseObj as C2sSprotoType.join.response;
            _ctx.UdpAuth(obj.session, obj.host, (int)obj.port);
            _mysession = obj.session;

            //_ctx.Push("game");
        }

    }
}
