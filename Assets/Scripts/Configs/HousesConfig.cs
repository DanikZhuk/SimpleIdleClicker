using UnityEngine;

namespace Configs
{
    [CreateAssetMenu(fileName = "Houses", menuName = "Configs/Houses")]
    public class HousesConfig: ScriptableObject
    {
        public int OffersAmount;
        public int MaxPurchasedAmount;
        public int MinCost;
        public int MaxCost;
        public int MinRenovationCost;
        public int MaxRenovationCost;
        public float SellCoeff;
        public float MinRenovationTime;
        public float MaxRenovationTime;
    }
}