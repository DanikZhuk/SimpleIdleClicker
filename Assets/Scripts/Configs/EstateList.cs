using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Configs
{
    [CreateAssetMenu(fileName = "EstateList", menuName = "Configs/EstateList")]
    public class EstateList:ScriptableObject
    {
        public List<EstateConfig> Estates;
    }
}