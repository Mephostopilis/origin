using Entitas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Bacon.Game.Components;

namespace Bacon.Game.Systems {
    class IndexSystem : Entitas.ISystem {
        private Entitas.Context<Entitas.Entity> _context;

        public IndexSystem(Entitas.Context<Entitas.Entity> context) {
            _context = context;
            _context.OnEntityCreated += ContextEntityChanged;
        }

        void ContextEntityChanged(IContext context, IEntity entity) {
            var e = entity as Entitas.Entity;
            
        }
    }
}
