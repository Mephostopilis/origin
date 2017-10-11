using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bacon.Game.Systems {
    class MyPlayerSystem : Entitas.ISystem {
        private GameContext _context;
        private long _index;

        public MyPlayerSystem(GameContext context) {
            _context = context;
        }

        public void Join(long index) {
            _index = index;
        }
    }
}
