using System;
using System.Collections.Generic;
using Gameplay.Businesses.BusinessModels;
using Gameplay.Investments;

namespace Core.SaveSystem.Data
{
    public class SaveData
    {
        public long Money;
        
        public readonly List<BusinessModel> BusinessModels = new();
        public readonly List<InvestmentModel> Investments = new();

        public DateTime RecordTime;
    }
}