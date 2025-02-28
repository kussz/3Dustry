using GameObjects.Drawing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace GameObjects.Resources
{
    public abstract class GameResource
    {
        protected GameResource(ResourceType type,int quantity)
        {
            Type=type;
            Quantity =quantity;
        }
        public int Quantity { get; set; }
        public ResourceType Type { get; set; }
        public static GameResource operator +(GameResource a, GameResource b)
        {
            if(a.GetType() != b.GetType())
                throw new InvalidOperationException("Cannot sum resources of different types.");
            var result = (GameResource)a.MemberwiseClone();
            result.Quantity += b.Quantity;
        return result;
        }
        public static GameResource operator -(GameResource a, GameResource b)
        {
            if (a.GetType() != b.GetType())
                throw new InvalidOperationException("Cannot sum resources of different types.");
            var result = (GameResource)a.MemberwiseClone();
            result.Quantity -= b.Quantity;
            return result;
        }
    }
}
