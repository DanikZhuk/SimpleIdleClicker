using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Configs
{
    [CreateAssetMenu(fileName = "InvestitionListConfig", menuName = "Configs/InvestitionListConfig")]
    public class InvestitionListConfig: ScriptableObject
    {
        [FormerlySerializedAs("InvestitionConfigs")] public InvestmentConfig[] InvestmentConfigs;
        public float UpdateTime;
    }
}