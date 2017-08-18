using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Maria
{
    public class World
    {
        private List<Entity> _container = new List<Entity>();
        private Dictionary<int, Entity> _dic = new Dictionary<int, Entity>();
        private Context _ctx;

        public World(Context ctx)
        {
            _ctx = ctx;
        }

        public Entity GetEntity(int idx)
        {
            return _dic[idx];
        }

        public void AddEntity(Entity e)
        {
            _container.Add(e);
            _dic.Add(e.Idx, e);
        }

        public void RemoveEntity(Entity e)
        {
            _dic.Remove(e.Idx);
            _container.Remove(e);
        }

        public virtual void OnFrameUpdate(float delata)
        {
            foreach (var item in _container)
            {
                item.OnFrameUpdate(delata);
            }
        }
    }
}
