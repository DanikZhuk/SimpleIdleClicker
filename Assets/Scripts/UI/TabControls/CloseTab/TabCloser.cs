using UnityEngine;
using UnityEngine.UI;

namespace UI.TabControls.CloseTab
{
    public class TabCloser: MonoBehaviour
    {
        [SerializeField]
        private GameObject tab;
        [SerializeField]
        private Button closeButton;

        private void Start()
        {
            if (!closeButton)
            {
                closeButton = gameObject.GetComponent<Button>();
            }
            
            closeButton.onClick.AddListener(CloseTab);
        }

        private void OnDestroy()
        {
            closeButton.onClick.RemoveAllListeners();
        }

        private void CloseTab()
        {
            Destroy(tab);
        }
    }
}