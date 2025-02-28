using GameObjects.Drawing;
using GameObjects.Resources;

namespace GameObjects.GameLogic
{
    public class Inventory
    {
        private List<GameResource> _resources;
        public int MaxItems { get; set; } = 0;
        public Inventory()
        {
            _resources = new List<GameResource>();
        }
        public Inventory(params GameResource[] resources)
        {
            _resources = [.. resources];
        }
        public int GetItemsCount()
        {
            return _resources.Sum(o => o.Quantity);
        }
        public bool Add(GameResource resource)
        {
            if (resource != null)
            {
                var addingRes = GetResource(resource.Type);
                int checking = resource.Quantity;
                if(addingRes!=null)
                    checking += addingRes.Quantity;
                if (MaxItems>0&&checking > MaxItems)
                {
                    return false;
                }

                if (addingRes == null)
                {
                    _resources.Add(resource);
                }
                else
                {
                    _resources[_resources.IndexOf(addingRes)] = addingRes + resource;
                }
                return true;
            }
            return false;
        }
        public static bool operator >=(Inventory a, Inventory b)
        {
            foreach (var resource in b._resources)
            {
                var type = resource.Type;
                if (a.GetCount(type) < b.GetCount(type))
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
        public GameResource GetFirst()
        {
            return _resources.FirstOrDefault();
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
                    if((addingRes-resource).Quantity>0)
                        _resources[_resources.IndexOf(addingRes)] = addingRes - resource;
                    else
                        _resources.Remove(addingRes);
                }
            }
        }
        public void Subtract(Inventory inv)
        {
            foreach (var resource in inv._resources)
            {
                Subtract(resource);
            }
        }
        public int GetCount(ResourceType type)
        {
            var res = _resources.SingleOrDefault(r => r.Type == type);
            if (res == null)
                return 0;
            return res.Quantity;
        }
        public GameResource GetResource(ResourceType type)
        {
            var res = _resources.SingleOrDefault(r => r.Type == type);
            if (res == null)
                return null;
            return res;
        }
        public GameResource GetMostPerspective()
        {
            var res = _resources.OrderBy(r => r.Quantity).FirstOrDefault();
            return res;
        }
    }
}
