using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bacon.Game.Systems {
    class MapSystem : Entitas.ISystem, Entitas.IInitializeSystem, Entitas.IExecuteSystem {

        private Entitas.Context<Entitas.Entity> _context;
        public MapSystem(Entitas.Context<Entitas.Entity> context) {
            _context = context;
        }

        public void Execute() {
            throw new NotImplementedException();
        }

        public void Initialize() {
            throw new NotImplementedException();
        }
    }
}
