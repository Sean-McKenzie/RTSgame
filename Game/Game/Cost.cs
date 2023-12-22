using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities;

namespace Game
{
    public struct Cost
    {
        public Dictionary<ResourceType, int> ResourceCost;
        public Cost(bool flag)
        {
            ResourceCost = new Dictionary<ResourceType, int>();
        }
        public void Add(ResourceType rt, int amount)
        {
            if (ResourceCost.ContainsKey(rt))
            {
                ResourceCost[rt] += amount;
            }
            else
            {
                ResourceCost.Add(rt, amount);
            }
        }
    }
}
