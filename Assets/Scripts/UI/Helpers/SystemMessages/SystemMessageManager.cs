using UnityEngine;

namespace UI.Helpers.SystemMessages
{
    public class SystemMessageManager: MonoBehaviour
    {
        [SerializeField] SystemMessageView systemMessageViewPrefab;
        [SerializeField] Transform rectTransform;

        public void Log(string message, Color color)
        {
            Instantiate(systemMessageViewPrefab, rectTransform).Initialize(message, color);
        }
        public void Log(string message)
        {
            Instantiate(systemMessageViewPrefab, rectTransform).Initialize(message, Color.white);
        }
    }
}