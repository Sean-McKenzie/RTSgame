using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities;

namespace Game
{
    class Arrows : IProjectiles
    {
        public Vector Pos { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public int HP { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public int Width => throw new NotImplementedException();

        public int Height => throw new NotImplementedException();

        public Image Img => throw new NotImplementedException();

        public Vector Goal { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public Vector Vel { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public Vector Acc { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public int Speed { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public int StartingHP { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string TeamName { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public double Angle { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public ILocType LocType { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public ILocState LocState { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public void Move(double timeStep)
        {
            throw new NotImplementedException();
        }
    }
}
