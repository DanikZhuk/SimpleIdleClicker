using UnityEngine;
using UnityEngine.UI;

namespace UI.TabControls.NewTab
{
    public class TabOpener: MonoBehaviour
    {
        [SerializeField]
        private GameObject tabPrefab;
        [SerializeField]
        private Transform container;
        [SerializeField]
        private Button openButton;

        private void Start()
        {
            openButton.onClick.AddListener(OpenTab);
        }

        private void OpenTab()
        {
            Instantiate(tabPrefab, container);
        }
    }
}