using UnityEngine;
using UnityEngine.Serialization;

namespace Configs
{
    [CreateAssetMenu(fileName = "BusinessListConfig", menuName = "Configs/BusinessListConfig")]
    public class BusinessListConfig:ScriptableObject
    {
        public BusinessConfig[] Businesses;
    }
}