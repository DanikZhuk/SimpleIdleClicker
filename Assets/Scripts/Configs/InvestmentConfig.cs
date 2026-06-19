using Gameplay.Investments;
using UnityEngine;

namespace Configs
{
    [CreateAssetMenu(fileName = "InvestmentConfig", menuName = "Configs/InvestmentConfig")]
    public class InvestmentConfig: ScriptableObject
    {
        public InvestmentType Type;
        public string Name;
        public long MinCost;
        public long MaxCost;
        public long InitialCost;
        public float ResumptionTime;
        public int MaxAmount;
        public int HistorySize=200;
        public float MeanReversionStrength;
        public float MaxAllowedChangeCoeff;
        public float BaseStep;
        public float NoiseAmplitude;
        public float NoiseFrequency;
    }
}