using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bacon.Game.Systems {
    class MapSystem : Entitas.ISystem, Entitas.IInitializeSystem, Entitas.IExecuteSystem {

        private GameContext _context;
        public MapSystem(GameContext context) {
            _context = context;
        }

        public void Execute() {
            throw new NotImplementedException();
        }

        public void Initialize() {
            UnityEngine.Debug.Log("MapSystem Initialize");
        }
    }
}
