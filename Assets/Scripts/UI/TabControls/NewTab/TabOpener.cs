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

        protected GameObject Tab;

        private void Start()
        {
            if (!openButton)
            {
                openButton = gameObject.GetComponent<Button>();
            }
            if (!container)
            {
                container = GameObject.FindGameObjectWithTag("TabContainer").transform;
            }
            
            openButton.onClick.AddListener(OpenTab);
        }

        private void OnDestroy()
        {
            if(openButton)
                openButton.onClick.RemoveAllListeners();
        }

        protected virtual void OpenTab()
        {
            Tab=Instantiate(tabPrefab, container);
        }
    }
}