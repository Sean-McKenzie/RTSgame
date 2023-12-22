using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities;
using System.Drawing;

namespace Game
{
    public struct Pair
    {
        MobType MobType;
        Decision MobState;
        ILocType LocType;
        ILocState LocState;

        public Pair(MobType mobType, Decision d)
        {
            MobState = d;
            MobType = mobType;
            LocType = ILocType.NotBuilding;// bogus because not used for this Pair type
            LocState = ILocState.Healthy;// bogus because not used for this Pair type
        }

        public Pair(ILocType locType, ILocState locState)
        {
            LocType = locType;
            LocState = locState;
            MobState = Decision.None;// bogus because not used for this Pair type
            MobType = MobType.Villager;// bogus because not used for this Pair type
        }
    }

    public class DecAnis
    {
        Dictionary<Pair, Animation> aniMobs;
        Dictionary<Pair, Animation> aniLocs;
        Dictionary<IMOB, double> aniMobIndices;
        Dictionary<ILocatables, double> aniLocIndices;
        
        public DecAnis(Dictionary<Pair, Animation> aniMobs, Dictionary<Pair, Animation> aniLocs)
        {
            this.aniMobs = aniMobs;
            this.aniMobIndices = new Dictionary<IMOB, double>();
            this.aniLocs = aniLocs;
            this.aniLocIndices = new Dictionary<ILocatables, double>();
        }

        public void AddMOB(IMOB m)
        {
            aniMobIndices.Add(m, 1);
        }

        public void AddLoc(ILocatables loc)
        {
            aniLocIndices.Add(loc, 1);
        }

        public Image GetFrame(IMOB m)
        {
            Pair pr = new Pair(m.Type, m.Mode.D);
            Animation ani = aniMobs[pr];
            double index = aniMobIndices[m];// retrieve the current frame index

            index = GetIndex(ani, index);

            aniMobIndices[m] = index;// update this Mob's frame index to the next frame
            return ani.GetFrame((int)index);
        }

        public Image GetFrame(ILocatables loc)
        {
            Pair pr = new Pair(loc.LocType, loc.LocState);
            Animation ani = aniLocs[pr];
            double index = aniLocIndices[loc];// retrieve the current frame index

            index = GetIndex(ani, index);

            aniLocIndices[loc] = index;// update this Mob's frame index to the next frame
            return ani.GetFrame((int)index);
        }

        protected double GetIndex(Animation ani, double index)
        {
            index += ani.AniSpeed;//advance to the next frame
            if (index >= ani.FrameCount)// check if we are past the end of animation
            {
                if (ani.Continuous)
                {
                    index = 0;// restart to the 1st frame in the animation
                }
                else
                {
                    //if not continuous, then keep animation on final frame
                    index = ani.FrameCount - 1;
                }
            }

            return index;
        }

        public int GetWidth(IMOB m)
        {
            Pair pr = new Pair(m.Type, m.Mode.D);
            Animation ani = aniMobs[pr];
            return ani.Width;
        }

        public int GetWidth(ILocatables loc)
        {
            Pair pr = new Pair(loc.LocType, loc.LocState);
            Animation ani = aniLocs[pr];
            return ani.Width;
        }

        public int GetHeight(IMOB m)
        {
            Pair pr = new Pair(m.Type, m.Mode.D);
            Animation ani = aniMobs[pr];
            return ani.Height;
        }

        public int GetHeight(ILocatables loc)
        {
            Pair pr = new Pair(loc.LocType, loc.LocState);
            Animation ani = aniLocs[pr];
            return ani.Height;
        }
    }
}
