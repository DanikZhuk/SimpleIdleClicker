using Gameplay.Estates.Generic;
using UnityEngine;

namespace Configs
{
    [CreateAssetMenu(fileName = "EstateConfig", menuName = "Configs/EstateConfig")]
    public class EstateConfig: ScriptableObject
    {
        public EstateType Type;
        public string EstateName;
        public long Price;
        public long Income;
        public int MaxCount;
        public float SellPercentage = 0.3f;
    }
}