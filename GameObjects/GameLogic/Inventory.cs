using GameObjects.Drawing;
using GameObjects.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameObjects.GameLogic
{
    public class Inventory
    {
        private List<GameResource> _resources;
        public Inventory()
        {
            _resources = new List<GameResource>();
        }
        public void Add(GameResource resource)
        {
            if (resource != null)
            {
                var type = resource.GetType();
                var addingRes = _resources.Where(r => r.GetType() == type).SingleOrDefault();
                if (addingRes == null)
                {
                    _resources.Add(resource);
                }
                else
                {
                    _resources[_resources.IndexOf(addingRes)] = addingRes + resource;
                }
            }
        }
        public void Subtract(GameResource resource)
        {
            if (resource != null)
            {
                var type = resource.GetType();
                var addingRes = _resources.Where(r => r.GetType() == type).SingleOrDefault();
                if (addingRes == null)
                {
                    _resources.Add(resource);
                }
                else
                {
                    _resources[_resources.IndexOf(addingRes)] = addingRes - resource;
                }
            }
        }
        public int Get(Tile type)
        {
            var res = _resources.SingleOrDefault(r=>r.Type== type);
            if (res == null)
                return 0;
            return res.Quantity;
        }
        public GameResource GetResource(Tile type)
        {
            var res = _resources.SingleOrDefault(r => r.Type == type);
            if (res == null)
                return ResourceFactory.CreateResource(type,0);
            return res;
        }
        public GameResource GetMostPerspective()
        {
            var res = _resources.OrderBy(r=>r.Quantity).FirstOrDefault();
            return res;
        }
    }
}
