using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities;

namespace Game
{
    public interface IMove : ILocatables
    {
        //Vector Goal { get; set; }
        Vector Vel { get; set; }
        Vector Acc { get; set; }
        int Speed { get; set; }
        void Move(double timeStep);
       
    }
}
