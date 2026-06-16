using System;
using System.Collections.Generic;
using Gameplay.Estates.Generic;
using Gameplay.Investitions;
using UI.EstatePage.EstateViews.Renovation;

namespace Core.SaveSystem.Data
{
    public class GameData
    {
        public long Money;
        
        public readonly List<Estate> Estates = new();
        public readonly List<Investition> Investitions = new();
        
        //TODO: Move to the class inheriting Estate
        public readonly Dictionary<string, List<House>> Houses = new();
        public Dictionary<string, DateTime> TimeData = new();
        //TODO: Move to the class inheriting Estate
    }
}