using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game
{
    public class Player
    {
        public string PTeamName;
        public List<IThinking> Thinkers = new List<IThinking>(); // every player owned MOB
        public List<Building> Buildings = new List<Building>(); //All player buildings
        public List<IMove> Moves = new List<IMove>();// all projectiles, etc owned by this player
    }
}
