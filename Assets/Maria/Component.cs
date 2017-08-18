using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Maria {
    public class Component {
        protected Entity _entity;
        public Component(Entity entity) {
            _entity = entity;
        }

        public T GetComponent<T>() where T : Component {
            return _entity.GetComponent<T>();
        }

        public void AddComponent<T>() where T : Component {
            _entity.AddComponent<T>();
        }

        public void RemoveComponent<T>() where T : Component {
            _entity.RemoveComponent<T>();
        }

    }
}
