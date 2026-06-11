using Gameplay.Services.MoneyService;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Gameplay.Click
{
    public class TapPanelView : MonoBehaviour
    {
        [SerializeField] private Button button;
        private IMoneyService _moneyService;

        [Inject]
        private void Construct(IMoneyService moneyService)
        {
            _moneyService = moneyService;
        }

        private void Awake()
        {
            button.onClick.AddListener(Button_OnClick);
        }

        private void OnDestroy()
        {
            button.onClick.RemoveAllListeners();
        }

        private void Button_OnClick()
        {
            _moneyService.TapEarn();
        }
    }
}