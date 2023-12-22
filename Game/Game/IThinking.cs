using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities;

namespace Game
{
    public interface IThinking
    {
        
        int SightRange { get; set; }
        Utilities.Action Mode { get; set; }
        List<Utilities.Action> Orders { get; set; }
        Utilities.Action Decide(List<ILocatables> whatICanSeeNow, List<ILocatables> atlas);

    }
}
