using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bacon.Game.Systems {
    class JoinSystem : Entitas.ISystem {
        private Entitas.Context<Entitas.Entity> _context;

        public JoinSystem(Entitas.Context<Entitas.Entity> context) {
            _context = context;
        }

        public void Join(int index) {
            
        }


    }
}
