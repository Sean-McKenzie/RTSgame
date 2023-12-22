using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Utilities;
using System.Drawing;

namespace Game
{
    public class Game
    {
        
        int UserX, UserY;
        public List<IMOB> SelectedUnits = new List<IMOB>(); // To identify the unit/units selected
        public List<ILocatables> sels = new List<ILocatables>();
        protected Timer tmr;
        public Dictionary<IMOB, List<ILocatables>> atlas = new Dictionary<IMOB, List<ILocatables>>(); // All MAPs (for each mobs)
        public List<Player> players = new List<Player>();
        public List<ILocatables> environment = new List<ILocatables>(); // Every object
        protected DecAnis dta;  //Decision type / animation relationship
        public event UpdatedHandler Update; //event to notify others of game changes during play
       // Spawn += SpawnComplete; this is for after the menu exists will be a villager select

        public void Start()// to allow game to start (call this function once)
        {
            if(tmr.Enabled == false)
            {
                tmr.Start();
            }
        }

        public void Stop()// allow game to pause (if applicable)
        {
            if (tmr.Enabled == true)
            {
                tmr.Stop();
            }
        }
        
        void SpawnComplete(Soldier s, int count, Building b)
        {
            if (s.TroopType == SoldierType.Cavalry)
            {
                for (int i = 0; i < count; i++)
                {
                    
                    Vector offset = new Vector(2.0, 2.0);
                    Vector gatheringPoint = b.Pos - offset;
                    s = new Soldier(MobType.Swordsman, gatheringPoint, b.TeamName, "s" + i.ToString() , dta);
                    environment.Add(s);
                }
            }
        }
      
        protected List<ILocatables> MergeAtlases(Dictionary<IMOB, List<ILocatables>> atlas)
        {
            List<ILocatables> superAtlas = new List<ILocatables>();
            foreach (KeyValuePair<IMOB, List<ILocatables>> kvp in atlas)
            {
                foreach (ILocatables loc in kvp.Value)
                {
                    if (!superAtlas.Contains(loc))
                    {
                        superAtlas.Add(loc);
                    }
                }
                superAtlas.Add(kvp.Key);// don't forget to add MYSELF to the superAtlas so Pic can paint me
            }
            return superAtlas;
        }


        public Game(List<string> teamNames)
        {
            foreach (string team in teamNames)
            {
                Player p = new Player();
                p.PTeamName = team;
                players.Add(p);
            }
            Dictionary<Pair, Animation> dictMobs = new Dictionary<Pair, Animation>();
            // create a "ghost" soldier for purposes of creating a DTA...
            Soldier sGhost = new Soldier(MobType.Archer, new Vector(100, 100), players[0].PTeamName, "joe", dta);
          
            Decision d = Decision.None;
            Pair pr = new Pair(sGhost.Type, d);
            Animation aniGuy = new Animation(Properties.Resources.kindaSoldier2, 48, 48, 0, 0, 2, 4, 1.0, 0.3, true);
            dictMobs.Add(pr, aniGuy);// don't forget to add this animation to the dta's dictionary of animations
            d = Decision.Move;
            pr = new Pair(sGhost.Type, d);
            Animation aniGuyMove = new Animation(Properties.Resources.kindaSoldier2, 48, 48, 0, 0, 2, 4, 1.0, 0.3, true);
            dictMobs.Add(pr, aniGuyMove);
            Dictionary<Pair, Animation> dictLocs = new Dictionary<Pair, Animation>();

            dta = new DecAnis(dictMobs, dictLocs);

            for (int i = 0; i < 10; i++)
            {
                // now create a "real" soldier, notifying him of the "live" dta and then have him report ...
                Soldier s = new Soldier(MobType.Archer, new Vector(100 + i*60, 100), players[0].PTeamName, "joe", dta);
                Utilities.Action actStart = new Utilities.Action();
                actStart.D = Decision.None;
                actStart.goal = s.Pos;
                s.Mode = actStart;
                
                dta.AddMOB(s);// inform the DTA that a new Soldier has spawned
                players[0].Thinkers.Add(s);// add this soldier to the 1st Player
                environment.Add(s);
            }
            tmr = new Timer();
            tmr.Interval = 17;
            tmr.Tick += Tmr_Tick;
            tmr.Start();
        }


        private void Tmr_Tick(object sender, EventArgs e)
        {
            Play(0.017); // Play out timer tick of game
            if (Update != null)
            {//send out message of players and current atlas for Pic to paint
                List<ILocatables> superAtlas = MergeAtlases(atlas);
                Update(superAtlas, players);
            }
        }


        public void Play(double timestep)
        {
            foreach (Player p in players)
            {
                // differentiate between human player and computer

                foreach (IThinking thinker in p.Thinkers)
                {   // first find what I CURRENTLY SEE (no map needed for this)...
                    List<ILocatables> whatICurrentlySee = FindWhatICanSee(thinker);
                    if (thinker is IMOB)
                    {
                        #region DetermineMap
                        IMOB me = (IMOB)thinker;
                        if (atlas.ContainsKey(me))
                        {
                            List<ILocatables> myMap = atlas[me];

                            foreach (ILocatables loc in whatICurrentlySee)
                            {
                                if (!(loc is IMove))
                                {
                                    if (!myMap.Contains(loc))
                                    {
                                        myMap.Add(loc);
                                    }
                                }
                            }
                            atlas[me] = myMap;
                        }
                        else
                        {
                            List<ILocatables> newMap = new List<ILocatables>();
                            atlas.Add(me, newMap);

                        }
                        #endregion
                    }

                    // Now allow the Thinker to React based on things he can see...
                    if (thinker.Orders.Count == 0)
                    {
                        Utilities.Action act;
                        if (thinker is IMOB)
                        {// handle decision in case of Mob...
                            IMOB me = (IMOB)thinker;
                            act = me.Decide(whatICurrentlySee, atlas[me]);
                        }
                        else
                        {// handle decision in case of Trap or Fort, etc...
                            IMOB me = (IMOB)thinker;
                            List<ILocatables> emptyList = new List<ILocatables>();
                            act = me.Decide(whatICurrentlySee, emptyList);
                        }
                    }
                    else
                    {
                        thinker.Mode = thinker.Orders[0]; //the Order packages the Mode and the goal together
                    }
//                    ActOnThisDecision(thinker, act);
                    // now allow him to Move (if possible)...
                    if(thinker is IMove)
                    {
                        IMove mover = (IMove)thinker;
                        mover.Move(timestep);
                    }
                }

                // handle projectiles currently in the air...
                foreach (IMove mover in p.Moves)
                {
                    
                    mover.Move(timestep);
                }
            }
        }



        private static void ActOnThisDecision(IThinking me, Utilities.Action act)
        {
            me.Mode = act; // set the MOB's Mode
            switch (act.D)
            {
                case Decision.Attack:
                    //...
                    break;
                case Decision.Defend:
                    //...
                    break;
            }
        }

        #region
        protected List<ILocatables> FindWhatICanSee(IThinking me) // Creates the character's map
        {
            List<ILocatables> stuffICanSee = new List<ILocatables>();
            foreach(Player p in players)
            {
                foreach (IMOB other in p.Thinkers)
                {
                    if ((other is ILocatables) && (other is IThinking))
                    {// if (other is IMob)
                        if (other != me)
                        {
                            ILocatables otherLoc = (ILocatables)other;
                            Vector pointing = other.Pos - otherLoc.Pos;
                            double dist = pointing.Magnitude;
                            IThinking thinker = (IThinking)other;
                            if (dist < thinker.SightRange)
                            {
                                stuffICanSee.Add(other);
                            }
                        }
                    }
                }
                if (me is IMOB)// only Mobs can have an atlas...
                {
                    IMOB meAsMob = (IMOB)me;
                    foreach (Building bldg in p.Buildings)
                    {
                        Vector pointing = bldg.Pos - meAsMob.Pos;
                        double dist = pointing.Magnitude;
                        if (dist < me.SightRange)
                        {
                            stuffICanSee.Add(bldg);
                            List<ILocatables> myMap = atlas[meAsMob];
                            if (!myMap.Contains(bldg))
                            {
                                myMap.Add(bldg);
                            }
                        }
                    }
                }
            }
            // Players stuff is done, so now check the Game's environment...
            if (me is IMOB)// only check Resources around us if we are Mob
            {
                IMOB meAsMob = (IMOB)me;
                foreach (ILocatables loc in environment)
                {
                    if (loc is IResource)
                    {
                        Vector pointing = loc.Pos - meAsMob.Pos;
                        double dist = pointing.Magnitude;
                        if (dist < me.SightRange)
                        {
                            stuffICanSee.Add(loc);
                            List<ILocatables> myMap = atlas[meAsMob];
                            if (!myMap.Contains(loc))
                            {
                                myMap.Add(loc);
                            }
                        }
                    }
                }
            }
            return stuffICanSee;
        }
        #endregion


        public void SetUserInput(UserInput ui) //received from form, handle user input here 
        {
            double mX = ui.Position.X;
            double mY = ui.Position.Y;
            UserAction userAct = ui.UA;
           // MouseButtons mouse = (MouseButtons)sender;
            
            if (userAct == UserAction.Sel)
            {
                sels.Clear();
                SelectedUnits.Clear();
                foreach (ILocatables loc in environment)
                {
                    double xCharBox = loc.Pos.X - loc.Width / 2;
                    double yCharBox = loc.Pos.Y - loc.Height / 2;
                    Point charPoint = new Point((int)mX, (int)mY);
                    Rectangle charBox = new Rectangle((int)xCharBox, (int)yCharBox, loc.Width, loc.Height);
                    if (charBox.Contains(charPoint))
                    {
                        if (loc is IMOB)
                        {
                            IMOB mo = (IMOB)loc;
                            mo.SelectedUnit = true;
                            SelectedUnits.Add(mo);
                            break;  // quit searching in case of single selection
                        }
                        else if (!(loc is IMove))
                        {
                            sels.Add(loc);
                            break;// quit searching in case of single selection
                        }
                    }
                }
            }
            else if (userAct == UserAction.SelMult)
            {
                sels.Clear();
                SelectedUnits.Clear();
                foreach (ILocatables loc in environment)
                {
                    Rectangle charBox = ui.Rectangle;
                    if (charBox.Contains((int)loc.Pos.X, (int)loc.Pos.Y))
                    {
                        if (loc is IMOB)
                        {
                            IMOB mo = (IMOB)loc;
                            mo.SelectedUnit = true;
                            SelectedUnits.Add(mo);
                            // How to notify form of troops, find a way to signify this is the selected group
                        }
                        else if(!(loc is IMove))
                        {
                            sels.Add(loc);
                        }
                    }
                }
                // figure out position of multiple units in area and compare the rectangle created
            }
            else if (userAct == UserAction.Do)
            {
                // 1st determine WHERE the do action takes place so we can see if it's to ATTACK there or MOVE there
                int count = SelectedUnits.Count + sels.Count;
                if (count > 0)
                {// only proceed if at least 1 item has been selected...
                    Utilities.Action act = new Utilities.Action();// Action is a placeholder for a future decision based upon WHAT what rt-clicked on...
                    Point rtClicked = new Point((int)ui.Position.X, (int)ui.Position.Y);
                    bool rtClickedOnSomething = false;
                    foreach(ILocatables loc in environment)
                    {
                        Rectangle charBox = new Rectangle((int)(loc.Pos.X - loc.Width / 2), (int)(loc.Pos.Y - loc.Height / 2), loc.Width, loc.Height);
                        if (charBox.Contains(rtClicked))
                        {
                            rtClickedOnSomething = true;// make note that we rt-clicked ON something
                            string selTeam = SelectedUnits[0].TeamName;// friend or foe
                            if(loc is IMOB)
                            {
                                if(selTeam == loc.TeamName)// friendly MOB
                                {// tell all soldiers to DEFEND, tell villages to MOVE...
                                    act.D = Decision.Defend;
                                    act.goal = new Vector(rtClicked.X, rtClicked.Y);
                                    Utilities.Action act2 = new Utilities.Action();
                                    act2.D = Decision.Move;
                                    act2.goal = new Vector(rtClicked.X, rtClicked.Y);
                                    foreach(IMOB m in SelectedUnits)
                                    {
                                        m.Orders.Clear();
                                        if (m is Soldier)
                                        {
                                            
                                           
                                            m.Orders.Add(act);  // tells soldier to remember this action (DEFEND this POS)
                                           
                                        }
                                        else
                                        {
                                            m.Orders.Add(act2);// tells villages to remember this action (MOVE to a POS)

                                        }
                                    }
                                }
                                else    // enemy MOB
                                {
                                    act.D = Decision.Attack;
                                    act.goal = new Vector(rtClicked.X, rtClicked.Y);
                                    foreach(IMOB m in SelectedUnits)
                                    {
                                        m.Orders.Clear();
                                        m.Orders.Add(act);  // soldiers AND villagers are told to ATTACK at a POS
                                    }
                                }
                            }
                            else if(loc is Building)
                            {

                            }
                            else if (loc is IResource)
                            {

                            }
                        }
                    }

                    if(rtClickedOnSomething == false)
                    {// if we make it here, nothing was in the charbox, so set action to Move to the position...
                        Utilities.Action actMove = new Utilities.Action();
                        actMove.D = Decision.Move;
                        actMove.goal = new Vector(rtClicked.X, rtClicked.Y);
                        foreach(IMOB m in SelectedUnits)
                        {
                            m.Orders.Clear();
                            m.Orders.Add(actMove);
                        }
                    }

                    // Maybe not needed...
                    //foreach (IMOB m in SelectedUnits)
                    //{
                    //    foreach (Player p in players)
                    //    {
                    //        foreach (IMOB mob in p.Thinkers)
                    //        {
                    //            if (mob.Pos.X == mX && mob.Pos.Y == mY)
                    //            {
                    //                if (mob.TeamName != m.TeamName)
                    //                {
                    //                    // Move to attack range and attack
                    //                }
                    //            }
                    //        }
                    //        foreach (ILocatables loc in environment)
                    //        {
                    //            if (loc.Pos.X == mX && loc.Pos.Y == mY)
                    //            {
                    //                if (loc is ITrap)
                    //                {

                    //                    if (m is Villagers)
                    //                    {
                    //                        if (m.TeamName == loc.TeamName)
                    //                        {
                    //                            if (loc.HP < loc.StartingHP * .9)
                    //                            {
                    //                                m.Mode = Decision.Repair;
                    //                            }
                    //                        }
                    //                    }

                    //                }
                    //                else if (loc is Building)
                    //                {
                    //                    if (m is Villagers)
                    //                    {
                    //                        if (m.TeamName == loc.TeamName)
                    //                        {
                    //                            if (loc.HP < loc.StartingHP * .9)
                    //                            {
                    //                                m.Mode = Decision.Repair;
                    //                            }
                    //                        }
                    //                    }
                    //                }
                    //                else if (loc is IResource)
                    //                {
                    //                    if (m is Villagers)
                    //                    {
                    //                        m.Mode = Decision.Gather;
                    //                    }
                    //                }
                    //            }
                    //            else
                    //            {
                    //                //nothing exists here, move to location
                    //                // m.Move();
                    //            }
                    //        }
                    //    }
                    //}
                }
            }
            
        }
    }
}
