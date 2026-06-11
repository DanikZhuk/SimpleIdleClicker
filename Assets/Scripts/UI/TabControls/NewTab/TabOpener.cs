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
            openButton.onClick.AddListener(OpenTab);
        }

        private void OpenTab()
        {
            Tab=Instantiate(tabPrefab, container);
        }
    }
}