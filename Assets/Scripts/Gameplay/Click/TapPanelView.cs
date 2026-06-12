using Gameplay.Services;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Gameplay.Click
{
    public class TapPanelView : MonoBehaviour
    {
        [SerializeField] private Button button;
        
        [Inject] private MoneyService _moneyService;

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