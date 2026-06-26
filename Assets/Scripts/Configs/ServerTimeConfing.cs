using UnityEngine;

namespace Configs
{
    [CreateAssetMenu(fileName = "ServerTimeConfing", menuName = "Configs/ServerTimeConfing")]
    public class ServerTimeConfing: ScriptableObject
    {
        public int ServerTimeUpdateMilliseconds;
        public float MaxSecondsTolerance;
    }
}