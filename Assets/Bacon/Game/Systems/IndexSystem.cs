using Entitas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bacon.Game.Systems {
    class IndexSystem : Entitas.ISystem {
        private GameContext _context;
        private Dictionary<int, Entitas.IEntity> _entitas = new Dictionary<int, IEntity>();

        public IndexSystem(GameContext context) {
            _context = context;
            _context.OnEntityCreated += OnEntityCreated;
            _context.OnEntityDestroyed += OnEntityDestroyed;
            _context.OnEntityWillBeDestroyed += OnEntityWillBeDestroyed;
        }

        public GameEntity FindEntity(int index) {
            if (_entitas.ContainsKey(index)) {
                return _entitas[index] as GameEntity;
            }
            return null;
        }

        private void OnEntityWillBeDestroyed(IContext context, IEntity entity) {
            // 其他系统都在这里处理
        }

        private void OnEntityCreated(IContext context, IEntity entity) {
            var e = entity as GameEntity;
            int index = e.index.index;
            _entitas.Add(index, e);
        }

        private void OnEntityDestroyed(IContext context, IEntity entity) {
            var e = entity as GameEntity;
            int index = e.index.index;
            _entitas.Remove(index);
        }

    }
}
