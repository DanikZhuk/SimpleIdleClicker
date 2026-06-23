using System;
using System.Collections.Generic;
using Configs;

namespace Gameplay.Businesses.BusinessModels
{
    public class RepairShopBusinessModel : BusinessModel
    {
        public List<HouseModel> HouseOffers;
        public List<HouseModel> PurchasedHouses;

        public RepairShopBusinessModel(BusinessConfig businessConfig, string businessName) : base(businessConfig,
            businessName)
        {
            HouseOffers = new List<HouseModel>();
            PurchasedHouses = new List<HouseModel>();
        }

        public RepairShopBusinessModel()
        { }
    }

    public enum HouseCondition
    {
        NeedRepair,
        UnderRepair,
        FullyRepaired
    }

    public class HouseModel
    {
        private static long _nextId;
        public HouseCondition Condition { get; set; }
        public long Cost { get; set; }
        public long RepairCost { get; private set; }
        public float RemainingRepairTime { get; set; }

        public HouseModel(long cost, long repairCost)
        {
            Condition = HouseCondition.NeedRepair;
            Cost = cost;
            RepairCost = repairCost;
        }
    }
}