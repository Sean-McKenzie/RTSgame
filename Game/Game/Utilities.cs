﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities;

namespace Game
{
    public delegate void UpdatedHandler(List<ILocatables> superAtlas, List<Player> player);
    public delegate void SpawnedHandler(SoldierType soldierType , int count);
}
