using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Maria;

namespace Bacon.Modules {
    class BattleScene : Maria.Module {
        private long _roomid = 0L;

        public BattleScene(User u) : base(u) {
        }

        public long RoomId { get { return _roomid; } set { _roomid = value; } }
    }
}
