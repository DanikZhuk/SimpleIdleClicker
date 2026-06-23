using System;
using System.Collections.Generic;
using Configs;
using Gameplay.Businesses.Generic.Models;

namespace Gameplay.Businesses.BusinessModels
{
    public class RepairShopBusinessModel: BusinessModel
    {
        public List<HouseModel> HouseOffers;
        public List<HouseModel> PurchasedHouses;
        public DateTime TimeData;

        public RepairShopBusinessModel(BusinessConfig businessConfig, string businessName) : base(businessConfig, businessName)
        {
            HouseOffers = new List<HouseModel>();
            PurchasedHouses = new List<HouseModel>();
        }

        public RepairShopBusinessModel()
        {
        }
    }
    
    public enum HouseCondition
    {
        NeedRepair,
        UnderRepair,
        FullyRepaired
    }

    public class HouseModel
    {
        public string Id { get; private set; }
        public HouseCondition Condition { get; set; }
        public long Cost { get; set; }
        public long RepairCost { get; private set; }
        public float RemainingRepairTime { get; set; }

        private static long _nextId = 0;

        public HouseModel(long cost, long repairCost)
        {
            Id = _nextId++.ToString();
            Condition = HouseCondition.NeedRepair;
            Cost = cost;
            RepairCost = repairCost;
        }
    }
}