using UnityEngine;

namespace Configs
{
    [CreateAssetMenu(fileName = "InvestmentListConfig", menuName = "Configs/InvestmentListConfig")]
    public class InvestmentListConfig: ScriptableObject
    {
        public InvestmentConfig[] InvestmentConfigs;
        public float UpdateTime;
    }
}