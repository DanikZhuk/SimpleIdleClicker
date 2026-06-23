using UnityEngine;

namespace Configs
{
    [CreateAssetMenu(fileName = "BusinessListConfig", menuName = "Configs/BusinessListConfig")]
    public class BusinessListConfig : ScriptableObject
    {
        public BusinessConfig[] Businesses;
        public float IncomeHourInSeconds = 10f;
    }
}