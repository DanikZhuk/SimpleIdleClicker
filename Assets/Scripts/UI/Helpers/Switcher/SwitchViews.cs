using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Helpers.Switcher
{
    public class SwitchViews : MonoBehaviour
    {
        [SerializeField] private List<SwitchPair> switches;
        [SerializeField] private GameObject initial;
        private SwitchPair _current;

        private void Start()
        {
            foreach (var pair in switches)
            {
                pair.button.onClick.AddListener(() => { Switch(pair); });
                if (pair.panel && pair.panel == initial)
                {
                    pair.panel.SetActive(true);
                    _current = pair;
                }
                else
                {
                    pair.panel.SetActive(false);
                }
            }
        }

        private void OnDestroy()
        {
            foreach (var pair in switches) pair.button.onClick.RemoveAllListeners();
        }

        private void Switch(SwitchPair current)
        {
            if (current.panel == _current.panel) return;
            current.panel.SetActive(true);
            if (_current.panel)
                _current.panel.SetActive(false);
            _current = current;
        }

        [Serializable]
        private class SwitchPair
        {
            public Button button;
            public GameObject panel;
        }
    }
}