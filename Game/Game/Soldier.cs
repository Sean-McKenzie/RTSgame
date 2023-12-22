using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities;

namespace Game
{
    class Soldier : IMOB
    {
        
//      protected Animation ani;
        public SoldierType TroopType;
     
        public struct AssociatedCosts
        {
            public CostType cost;
            public int value;
        }
        public Dictionary<CostType, int> UpkeepCost;
        public void UpKeepCalc()
        {
            

        }
        protected Vector vel;
        public Vector Vel
        {
            get
            {
                return vel;
            }
            set
            {
                vel = value;
            }
        }

        protected Vector acc;
        public Vector Acc
        {
            get
            {
                return acc;
            }
            set
            {
               acc = value;
            }
        }

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

        protected int cost ;
        public int Cost
        {
            get
            {
                return cost;
            }
            set
            {
                cost= value;
            }
        }

        protected Vector pos;
        public Vector Pos
        {
            get
            {
                return pos;
            }
            set
            {
                pos = value;
            }
        }

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

        public int Width
        {
            get
            {
                return dta.GetWidth(this);
            }
        }
        
        public int Height
        {
            get
            {
                return dta.GetHeight(this);
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

        public Image Img
        {
            get
            {
                return dta.GetFrame(this);
            }
        }

        protected Utilities.Action mode;
        public Utilities.Action Mode { get; set; } // 

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

        protected Vector goal;
        public Vector Goal
        {
            get
            {
                return goal;
            }
            set
            {
                goal = value;
            }
        }

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
        public int HearingRange { get; set; }
        public int AtkRange { get; set; }
        public DecAnis dta { get; set; }
        public double Angle { get; set; }
        public MobType Type { get; set; }
        public ILocType LocType { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public ILocState LocState { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public List<Utilities.Action> Orders { get; set; }

        // public List<Utilities.Action> Orders { get; set; }

        public Soldier(MobType soldierType, Vector pos, string teamName ,string nm, DecAnis dta)
        {
            
            this.Type = soldierType;
            Orders = new List<Utilities.Action>();
            Acc = new Vector();
            Pos = pos;// store the pos given at my birth
            Goal = pos;
            Utilities.Action actStart = new Utilities.Action();
            actStart.D = Decision.None;
            actStart.goal = Pos;
            Mode = actStart;
            this.dta = dta; // Store a reference to the game's dta object to look up animation frame
            if (Type == MobType.Swordsman)
            {
                startinghp = 500;
                hp = 500;
                speed = 70;
                atk = 1;
                ac = 1;
                sightRange = 5;
                housingCost = 2;
                TeamName = teamName;
                name = nm;
            }
            else if (Type == MobType.Archer)
            {
                startinghp = 300;
                hp = 300;
                speed = 100;
                Atk = 2;
                AC = 1;
                SightRange = 7;
                HousingCost = 2;
                name = nm;
            }
            else if (Type == MobType.Cavalry)
            {
                startinghp = 500;
                hp = 500;
                speed = 150;
                Atk = 3;
                AC = 1;
                SightRange = 7;
                HousingCost = 2;
                name = nm;
            }
        }
        public Utilities.Action Decide(List<ILocatables> whatICanSeeNow, List<ILocatables> map)
        {
            
            
            // use my own decision for what to do...
            //define the decisions for the soldier multiple cases are required
            Decision d = Decision.None;
            Vector goal = new Vector(0, 0);
            Utilities.Action act = new Utilities.Action();
            act.D = d;
            act.goal = goal;
            mode = act;
            return mode;
           
        }

        public void Sense()
        {

        }


        public void Move(double timeStep)
        {
            
            if (Mode.D == Decision.Move)
            {
                Vector pointToGoal = Mode.goal - Pos;
                double distToGoal = pointToGoal.Magnitude;
                if (distToGoal > 10)// only calc if not already at my Goal...
                {
                    Vector point = Mode.goal - Pos;// face my goal
                    Vector unit = point.Unitized;
                    //                Acc = Acceleration * unit;// "pull" me in that direction
                    Vel = Speed * unit;// scale up velocity to whatever speed I can run at
                }
                else
                {
                    Acc = new Vector(0, 0);
                    Vel = new Vector(0, 0);// already at Goal, so stop moving!
                }

                Vel = Vel + Acc * timeStep;// calc my new Velocity
                Pos = Pos + Vel * timeStep;// calc my new Position in space
                Angle = pointToGoal.Angle;
            }
        }
        //protected Vector CalcAcc(Vector goal)
        //{
        //    Vector accNew;
        //    double lowBound = Speed * 0.3;
        //    double highBound = Speed * 0.7;
        //    double lowDist = 30;
        //    double highDist = 200;
        //    Vector pointToGoal = goal - Pos;
        //    double distToGoal = pointToGoal.Magnitude;
        //    double currSpd = Vel.Magnitude;
        //    if ((distToGoal < lowDist) && (currSpd < lowBound)
        //        || ((distToGoal >= lowDist && distToGoal < highDist) && (currSpd >= lowBound && currSpd < highBound))
        //        || (distToGoal >= highDist && currSpd >= highBound))
        //    {
        //        accNew = new Vector(0, 0);// no gas, no brake
        //    }
        //    else if ((distToGoal < lowDist && (currSpd >= lowBound && currSpd < highBound))
        //        || (distToGoal >= lowDist && distToGoal < highDist) && currSpd >= highBound)
        //    {
        //        pointToGoal = -1 * pointToGoal;// reverse direction
        //        Vector unit = pointToGoal.Unitized;
        //        accNew = 0.5 * Acceleration * unit;// only apply half braking
        //    }
        //    else if (((distToGoal >= lowDist && distToGoal < highDist) && currSpd < lowBound)
        //        || (distToGoal >= highDist && currSpd >= lowBound && currSpd < highBound))
        //    {
        //        Vector unit = pointToGoal.Unitized;
        //        accNew = 0.5 * Acceleration * unit;// only apply half gas
        //    }
        //    else if (distToGoal < lowDist && currSpd >= highBound)
        //    {
        //        pointToGoal = -1 * pointToGoal;// reverse direction
        //        Vector unit = pointToGoal.Unitized;
        //        accNew = 1.0 * Acceleration * unit;//  apply FULL braking
        //    }
        //    else if (distToGoal >= highDist && currSpd < lowBound)
        //    {
        //        Vector unit = pointToGoal.Unitized;
        //        accNew = 1.0 * Acceleration * unit;//  apply FULL gas
        //    }
        //    else
        //    {// should never get here
        //        throw new Exception("Fire Chris Leidig immediately!");
        //    }
        //    return accNew;
        //}

        //protected Vector CalcAcc2(Vector goal)
        //{
        //    Vector accDesired;
        //    Vector pointToGoal = goal - Pos;
        //    Vector unitToGoal = pointToGoal.Unitized;
        //    double distToGoal = pointToGoal.Magnitude;
        //    double speedDesired = Speed;// desire max speed by default
        //    if (distToGoal < 400)
        //    {// if close to goal, then desire less than max speed...
        //        speedDesired = (int)((distToGoal / 400.0) * speedDesired);
        //    }
        //    Vector velDesired = speedDesired * unitToGoal;
        //    accDesired = 1 * (velDesired - Vel);
        //    return accDesired;
        //}
    }
}
