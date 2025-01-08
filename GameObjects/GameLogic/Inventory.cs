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
            var type = resource.GetType();
            var addingRes = _resources.Where(r=>r.GetType() == type).SingleOrDefault();
            if (addingRes == null)
            {
                _resources.Add(resource);
            }
            else
            {
                _resources[_resources.IndexOf(addingRes)] = addingRes+resource;
            }
        }
        public int Get(string name)
        {
            var res = _resources.SingleOrDefault(r=>r.Name== name);
            if (res == null)
                return 0;
            return res.Quantity;
        }
        public GameResource GetMostPerspective()
        {
            var res = _resources.OrderBy(r=>r.Quantity).FirstOrDefault();
            return res;
        }
    }
}
