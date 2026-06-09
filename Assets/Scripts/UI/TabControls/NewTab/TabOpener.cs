using System;
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
            if (!openButton)
            {
                openButton = gameObject.GetComponent<Button>();
            }
            
            openButton.onClick.AddListener(OpenTab);
        }

        private void OnDestroy()
        {
            openButton.onClick.RemoveAllListeners();
        }

        private void OpenTab()
        {
            Instantiate(tabPrefab, container);
        }
    }
}