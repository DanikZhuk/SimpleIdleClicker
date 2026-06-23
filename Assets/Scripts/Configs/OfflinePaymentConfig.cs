using UnityEngine;

namespace Configs
{
    [CreateAssetMenu(fileName = "OfflinePaymentConfig", menuName = "Configs/OfflinePaymentConfig")]
    public class OfflinePaymentConfig : ScriptableObject
    {
        public float IncomeIntervalSeconds = 5f;
    }
}