using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities;
using System.Windows.Forms;

namespace Game
{
    public class Building : ILocatables
    {
        public event SpawnedHandler Spawn;
        protected Animation ani;
        public ILocType Type;
        public List<IMOB> inQueue;
        public Vector Pos { get; set; }
        protected Timer tmrTrain = new Timer();
        protected int cot;
        public DecAnis dta;// = new DecAnis();
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
                return ani.Width;
            }
        }
        public int Height
        {
            get
            {
                return ani.Height;
            }
        }
        public string Name { get; set; }
        public Image Img
        {
            get
            {
                return dta.GetFrame(this);
            }
        }

        public int StartingHP { get; set; }
        public string TeamName { get; set; }
        public double Angle { get ; set; }
        public ILocType LocType { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public ILocState LocState { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public void StartTrain(int count)
        {
            cot = count;
            tmrTrain.Start();
            tmrTrain.Tick += TmrTrain_Tick;
        }

        private void TmrTrain_Tick(object sender, EventArgs e)
        {
           if (Spawn != null)
           {
               Spawn(SoldierType.Cavalry, cot);
           }
        }

        public Building (DecAnis dta)
        {
            tmrTrain.Interval = 30000;
            this.dta = dta; // Store a reference to the game's dta object to look up animation frame


        }


        //public void TrainComplete()
        //{
        //    if(Spawn != null)
        //    {
        //        Spawn(SoldierType.Cavalry, cot);
        //    }
        //}













        public void Produce(Player p)
        {
            if (Type != ILocType.Camp && Type != ILocType.House)
            {
                if (Type == ILocType.Barracks)
                {
                    int numName = 0;
                    foreach (IMOB mob in p.Thinkers)
                    {
                        if (mob is Soldier)
                        {
                            numName = p.Thinkers.LastIndexOf(mob) + 1;
                        }
                        else
                        {
                            numName = 1;
                        }
                    }
                    string name = "s" + numName.ToString();

                    Vector pos = new Vector(0.0);
                    Vector gatheringPoint = this.Pos - pos;
                    Soldier s = new Soldier(MobType.Swordsman, gatheringPoint, this.TeamName, name, dta);
                    inQueue.Add(s);
                }
                else if(Type == ILocType.ArcheryRange)
                {
                    int numName = 0;
                    foreach (IMOB mob in p.Thinkers)
                    {
                        if (mob is Soldier)
                        {
                            numName = p.Thinkers.LastIndexOf(mob) + 1;
                        }
                        else
                        {
                            numName = 1;
                        }
                    }
                    string name = "s" + numName.ToString();

                    Vector pos = new Vector(0.0);
                    Vector gatheringPoint = this.Pos - pos;
                    Soldier s = new Soldier(MobType.Archer, gatheringPoint, this.TeamName, name, dta);
                    inQueue.Add(s);
                }
                else if (Type == ILocType.Stables)
                {
                    int numName = 0;
                    foreach (IMOB mob in p.Thinkers)
                    {
                        if (mob is Soldier)
                        {
                            numName = p.Thinkers.LastIndexOf(mob) + 1;
                        }
                        else
                        {
                            numName = 1;
                        }
                    }
                    string name = "s" + numName.ToString();

                    Vector pos = new Vector(0.0);
                    Vector gatheringPoint = this.Pos - pos;
                    Soldier s = new Soldier(MobType.Cavalry, gatheringPoint,this.TeamName , name, dta);
                    inQueue.Add(s);
                }
            }
        }
    }
   
}