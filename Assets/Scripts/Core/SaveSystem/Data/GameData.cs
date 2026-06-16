using System.Collections.Generic;
using Gameplay.Estates.Generic;
using Gameplay.Investitions;

namespace Core.SaveSystem.Data
{
    public class GameData
    {
        public long Money;
        
        public readonly List<Estate> Estates = new();
        public readonly List<Investition> Investitions = new();
    }
}