using UnityEngine;

namespace Configs
{
    [CreateAssetMenu(fileName = "TimeConfig", menuName = "Configs/TimeConfig")]
    public class TimeConfig: ScriptableObject
    {
        public float incomeSeconds;
    }
}