using UnityEngine;

namespace Configs
{
    [CreateAssetMenu(fileName = "ClickConfig", menuName = "Configs/ClickConfig")]
    public class ClickConfig: ScriptableObject
    {
        public long TapAmount=10;
    }
}