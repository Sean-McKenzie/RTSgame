using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities;

namespace Game
{
    public interface IMOB:IMove, IThinking
    {
        MobType Type { get; set; }
        DecAnis dta { get; set; }
        string Name { get; set; }
        bool SelectedUnit { get; set; }
        int Atk { get; set; }
        int AC { get; set; }
        int HousingCost { get; set; }
        int Cost { get; set; }
        int HearingRange { get; set; }
        int AtkRange { get; set; }
    }
}
