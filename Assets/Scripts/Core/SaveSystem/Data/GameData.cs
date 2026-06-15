using System.Collections.Generic;
using Gameplay.Estates.Generic;
using Gameplay.Investitions;

namespace Core.SaveSystem.Data
{
    public class GameData
    {
        public List<Estate> Estates = new();
        public List<Investition> Investitions = new();
        public long Money;
    }
}