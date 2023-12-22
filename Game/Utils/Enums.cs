using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Utilities
{
    public enum CostType
    {
        GoldCost, StoneCost, WoodCost, FoodCost
    }
    public enum ResourceType
    {
        GoldMine, StoneMine, BerryBush, Wood, Food
    }
    public enum Decision
    {
        Gather, Flee, Attack, Defend, Move, Build, Repair, None
    }
    public enum ILocType
    {
        NotBuilding, House, Barracks, Camp, TownHall, GreatHall, ArcheryRange, Stables, Resource
    }

    public enum ILocState
    {
        Healthy, Damaged, Destroyed
    }
    public struct Action
    {
        public Decision D;
        public Vector goal;
    }
    public enum UserAction
    {
        Do, Sel, SelMult, Play, Pause
    }

    public struct UserInput  //Hasn't served a purpose yet
    {
        public Vector Position;
        public Rectangle Rectangle;
        public UserAction UA;

        public UserInput(Vector Ps, Rectangle re, UserAction u)
        {
            Position = Ps;
            Rectangle = re;
            UA = u;
        }
    } // Probably useless

    public enum SoldierType
    {
        Swordsman, Archer, Cavalry
    }

    public enum MobType
    {
        NotMob, Swordsman, Archer, Cavalry, Villager
    }
}
