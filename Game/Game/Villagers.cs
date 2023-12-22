using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities;

namespace Game
{
    class Villagers : IMOB
    {
        protected int speed;
        public int Speed
        {
            get
            {
                return speed;
            }
            set
            {
                speed = value;
            }
        }

        protected int atk;
        public int Atk
        {
            get
            {
                return atk;
            }
            set
            {
                atk = value;
            }
        }//

        protected int ac;
        public int AC
        {
            get
            {
                return ac;
            }
            set
            {
                ac = value;
            }
        }//

        protected int housingCost;
        public int HousingCost
        {
            get
            {
                return housingCost;
            }
            set
            {
                housingCost = value;
            }
        }//
        protected int hp;
        public int HP
        {
            get
            {
                return hp;
            }
            set
            {
                if (value <= 0)
                {
                    hp = 0;
                }
            }
        }
        protected int sightRange;
        public int SightRange
        {
            get
            {
                return sightRange;
            }
            set
            {
                sightRange = value;
            }
        }
        protected string name;
        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
            }
        }

  
        public Vector Goal { get; set; }
        public Vector Vel { get; set; }
        public Vector Acc { get; set; }

        public int Cost { get; set; }

        public Utilities.Action Mode { get; set; }
        public Vector Pos { get; set; }


        public int Width => throw new NotImplementedException();

        public int Height => throw new NotImplementedException();

        public Image Img => throw new NotImplementedException();

        public bool SelectedUnit { get; set; }
        public int startinghp;
        public int StartingHP
        {
            get
            {
                return startinghp;
            }
            set
            {
                startinghp = value;
            }
        }

        public string TeamName { get ; set ; }
        public DecAnis dta { get; set; }
        public int HearingRange { get; set; }
        public int AtkRange { get; set; }
        public double Angle { get ; set; }
        public MobType Type { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public ILocType LocType { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public ILocState LocState { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        
        public List<Utilities.Action> Orders { get; set; }

        public Utilities.Action Decide(List<ILocatables> whatICanSeeNow, List<ILocatables> map)
        {
            throw new NotImplementedException();
        }

        public Villagers( Vector pos, string nm, DecAnis dta)
        {
            Orders = new List<Utilities.Action>();
            this.dta = dta; // Store a reference to the game's dta object to look up animation frame
            Pos = pos;
            startinghp = 10;
            hp = 10;
            speed = 1;
            atk = 1;
            ac = 1;
            sightRange = 5;
            housingCost = 1;
            name = nm;
        }

        public void Move(double timeStep)
        {
            throw new NotImplementedException();
        }
    }
}
