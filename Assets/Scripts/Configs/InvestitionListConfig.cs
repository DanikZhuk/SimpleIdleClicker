using System.Collections.Generic;
using UnityEngine;

namespace Configs
{
    [CreateAssetMenu(fileName = "InvestitionListConfig", menuName = "Configs/InvestitionListConfig")]
    public class InvestitionListConfig: ScriptableObject
    {
        public InvestitionConfig[] InvestitionConfigs;
        public int HistorySize;
    }
}