using Gameplay.Businesses.Enums;
using UnityEngine;

namespace Configs
{
    [CreateAssetMenu(fileName = "BusinessConfig", menuName = "Configs/BusinessConfig")]
    public class BusinessConfig: ScriptableObject
    {
        public BusinessType Type;
        public string BusinessName;
        public long Price;
        public long Income;
        public int MaxCount;
        public float SellPercentage = 0.3f;
    }
}