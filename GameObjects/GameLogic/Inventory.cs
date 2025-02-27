using GameObjects.Drawing;
using GameObjects.Resources;

namespace GameObjects.GameLogic
{
    public class Inventory
    {
        private List<GameResource> _resources;
        public Inventory()
        {
            _resources = new List<GameResource>();
        }
        public Inventory(params GameResource[] resources)
        {
            _resources = new List<GameResource>();
            foreach (var resource in resources)
                _resources.Add(resource);
        }
        public int GetItemsCount()
        {
            return _resources.Sum(o => o.Quantity);
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
        public static bool operator >=(Inventory a, Inventory b)
        {
            foreach (var resource in b._resources)
            {
                var type = resource.Type;
                if (a.Get(type) < b.Get(type))
                    return false;
            }
            return true;
        }
        public static bool operator <=(Inventory a, Inventory b)
        {
            return b >= a;
        }
        public static Inventory operator - (Inventory a, Inventory b)
        {
            Inventory newinv = new Inventory();
            foreach(var resource in a._resources)
            {
                newinv.Add(resource);
            }
            foreach (var resource in b._resources)
            {
                newinv.Subtract(resource);
            }
            return newinv;
        }
        public static Inventory operator +(Inventory a, Inventory b)
        {
            Inventory newinv = new Inventory();
            foreach (var resource in a._resources)
            {
                newinv.Add(resource);
            }
            foreach (var resource in b._resources)
            {
                newinv.Add(resource);
            }
            return newinv;
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
            var res = _resources.SingleOrDefault(r => r.Type == type);
            if (res == null)
                return 0;
            return res.Quantity;
        }
        public GameResource GetResource(Tile type)
        {
            var res = _resources.SingleOrDefault(r => r.Type == type);
            if (res == null)
                return ResourceFactory.CreateResource(type, 0);
            return res;
        }
        public GameResource GetMostPerspective()
        {
            var res = _resources.OrderBy(r => r.Quantity).FirstOrDefault();
            return res;
        }
    }
}
