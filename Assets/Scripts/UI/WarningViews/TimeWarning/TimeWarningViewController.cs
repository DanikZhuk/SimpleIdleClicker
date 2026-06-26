using Cysharp.Threading.Tasks;
using Gameplay.Services;
using UI.Helpers.SystemMessages;
using UnityEngine;
using Zenject;

namespace UI.WarningViews.TimeWarning
{
    public class TimeWarningViewController : MonoBehaviour
    {
        [SerializeField] private TimeWarningView timeWarningViewPrefab;
        [SerializeField] private RectTransform warningViewContainer;
        [Inject] TimeService _timeService;
        [Inject] SystemMessageManager _systemMessageManager;

        private TimeWarningView _timeWarningView;

        private void Awake()
        {
            _timeService.OnTimeWarning += CreateTimeWarning;
        }

        private void CreateTimeWarning()
        {
            _timeWarningView = Instantiate(timeWarningViewPrefab, warningViewContainer);
            _timeWarningView.OnRecoverButtonPressed += OnRecoverButtonPressed;
        }

        private void OnRecoverButtonPressed()
        {
            OnRecoverButtonPressedAsync().Forget();
        }

        private async UniTask OnRecoverButtonPressedAsync()
        {
            _timeWarningView.SetRecoverButtonInteractable(false);
            if (await _timeService.OnRecover())
            {
                _timeWarningView.OnRecoverButtonPressed -= OnRecoverButtonPressed;
                Destroy(_timeWarningView.gameObject);
                return;
            }
            _timeWarningView.SetRecoverButtonInteractable(true);
        }
    }
}