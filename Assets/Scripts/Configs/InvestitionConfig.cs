using Gameplay.Investitions;
using UnityEngine;

namespace Configs
{
    [CreateAssetMenu(fileName = "InvestitionConfig", menuName = "Configs/InvestitionConfig")]
    public class InvestitionConfig: ScriptableObject
    {
        public InvestitionType Type;
        public string Name;
        public long MinCost;
        public long MaxCost;
        public long InitialCost;
        public float ResumptionTime;
        public int MaxAmount;
        public float MeanReversionStrength;
        public float MaxAllowedChangeCoeff;
        public float BaseStep;
        public float NoiseAmplitude;
        public float NoiseFrequency;
    }
}