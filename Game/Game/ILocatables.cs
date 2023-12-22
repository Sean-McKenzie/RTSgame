using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using Utilities;

namespace Game
{
    public interface ILocatables
    {
        ILocType LocType { get; set; }
        ILocState LocState { get; set; }
        double Angle { get; set; }
        string TeamName { get; set; }
        Vector Pos { get; set; }
        int StartingHP { get; set; }
        int HP { get; set; }
        int Width { get; }
        int Height { get; }
        Image Img { get; }
    }
}
