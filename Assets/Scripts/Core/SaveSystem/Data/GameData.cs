using System.Collections.Generic;
using Gameplay.Businesses.Generic.Models;
using Gameplay.Investitions;

namespace Core.SaveSystem.Data
{
    public class GameData
    {
        public long Money;
        
        public readonly List<BusinessModel> BusinessModels = new();
        public readonly List<Investition> Investitions = new();
    }
}