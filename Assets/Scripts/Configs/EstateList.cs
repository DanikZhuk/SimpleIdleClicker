using System.Collections.Generic;
using UnityEngine;

namespace Configs
{
    [CreateAssetMenu(fileName = "EstateList", menuName = "Configs/EstateList")]
    public class EstateList:ScriptableObject
    {
        public List<EstateConfig> Estates;
    }
}