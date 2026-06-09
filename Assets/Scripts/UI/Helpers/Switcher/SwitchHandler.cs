using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Helpers.Switcher
{
    public class SwitchHandler : MonoBehaviour
    {
        [Serializable]
        private class SwitchPair
        {
            public Button button;
            public GameObject gameObj;
        }
    
        [SerializeField]
        private List<SwitchPair> switches;
        [SerializeField]
        private GameObject initial;
        private SwitchPair _current;

        private void Start()
        {
            foreach (var pair in switches)
            {
                pair.button.onClick.AddListener(() => { Switch(pair); });
                if (pair.gameObj && pair.gameObj == initial)
                {
                    pair.gameObj.SetActive(true);
                    _current = pair;
                }
                else
                    pair.gameObj.SetActive(false);
            }
        }

        private void OnDestroy()
        {
            foreach (var pair in switches)
            {
                pair.button.onClick.RemoveAllListeners();
            }
        }

        private void Switch(SwitchPair current)
        {
            if(current.gameObj==_current.gameObj) return;
            current.gameObj.SetActive(true);
            if(_current.gameObj)
                _current.gameObj.SetActive(false);
            _current = current;
        }
    }
}
