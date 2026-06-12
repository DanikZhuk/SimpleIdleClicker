using UnityEngine;

namespace Configs
{
    [CreateAssetMenu(fileName = "ClickConfig", menuName = "Configs/ClickConfig")]
    public class ClickConfig: ScriptableObject
    {
        public int TapAmount=10;
    }
}