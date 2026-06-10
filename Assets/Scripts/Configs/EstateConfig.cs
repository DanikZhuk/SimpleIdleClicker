using Gameplay.Estates;
using Gameplay.Estates.Generic;
using UnityEngine;

namespace Configs
{
    [CreateAssetMenu(fileName = "EstateConfig", menuName = "Configs/EstateConfig")]
    public class EstateConfig: ScriptableObject
    {
        public EstateType Type;
        public string EstateName;
        public Sprite Icon;
        public float Price;
        public float Income;
        public int MaxCount;
        public GameObject Tab;
    }
}