using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities;

namespace Game
{
    public interface IResource:ILocatables
    {
        ResourceType Type { get; }
        double Rate { get; set; }
    }
}
