using Gameplay.Estates;
using UnityEngine;

namespace Configs
{
    [CreateAssetMenu(fileName = "EstateConfig", menuName = "Configs/EstateConfig")]
    public class Estate: ScriptableObject
    {
        public EstateType Type;
        public string EstateName;
        public Sprite Icon;
        public float Price;
        public float Income;
        public int MaxCount;
    }
}