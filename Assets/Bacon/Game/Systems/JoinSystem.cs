using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bacon.Game.Systems {
    class JoinSystem : Entitas.ISystem {
        private GameContext _context;

        public JoinSystem(GameContext context) {
            _context = context;
        }

        public void Join(int index) {
            var e = _context.CreateEntity();
            e.AddIndex(index);
        }

        public void Leave(IndexSystem indexsystem, int index) {
            var e = indexsystem.FindEntity(index);
            if (e != null) {
                e.Destroy();
            }
        }
    }
}
