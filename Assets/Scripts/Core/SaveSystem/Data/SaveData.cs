using System;
using System.Collections.Generic;
using Gameplay.Businesses.BusinessModels;
using Gameplay.Investments;

namespace Core.SaveSystem.Data
{
    public class SaveData
    {
        public readonly List<BusinessModel> BusinessModels = new();
        public readonly List<InvestmentModel> Investments = new();
        public long Money;

        public DateTime RecordTime;
        public TimeSpan ServerTimeOffset;
        public bool TimeDataInitialized = false;
    }
}