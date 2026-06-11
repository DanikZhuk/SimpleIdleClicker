using UnityEngine;

namespace Configs
{
    [CreateAssetMenu(fileName = "EconomyConfig", menuName = "Configs/EconomyConfig")]
    public class EconomyConfig : ScriptableObject
    {
        public float sellPercentage;
    }
}