using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI.WarningViews.TimeWarning
{
    public class TimeWarningView: MonoBehaviour
    {
        [SerializeField] private Button recoverButton;
        
        public event Action OnRecoverButtonPressed;

        private void Start()
        {
            recoverButton.onClick.AddListener(RecoverButton_OnClick);
        }

        public void SetRecoverButtonInteractable(bool interactable)
        {
            recoverButton.interactable = interactable;
        }

        private void RecoverButton_OnClick()
        {
            OnRecoverButtonPressed?.Invoke();
        }
    }
}