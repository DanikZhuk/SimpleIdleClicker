using UnityEngine;

namespace Configs.BusinessConfigs
{
    [CreateAssetMenu(fileName = "RepairShopBusinessConfig", menuName = "Configs/RepairShopBusinessConfig")]
    public class RepairShopBusinessConfig : BusinessConfig
    {
        [Header("Houses properties")]
        public int HouseOffersAmount;
        public int MaxPurchasedHousesAmount;
        public long MinHouseCost;
        public long MaxHouseCost;
        public long MinRepairCost;
        public long MaxRepairCost;
        public float AfterRepairCostCoeff;
        public float MinRepairTime;
        public float MaxRepairTime;
    }
}