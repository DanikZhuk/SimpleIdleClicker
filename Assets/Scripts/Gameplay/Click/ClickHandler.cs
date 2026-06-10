using Gameplay.Services.MoneyService;
using Reflex.Attributes;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay.Click
{
    public class ClickHandler : MonoBehaviour
    {
        [SerializeField] private Button button;
        [Inject] private MoneyService _moneyService;

        private void Awake()
        {
            button.onClick.AddListener(OnClick);
        }

        private void OnDestroy()
        {
            button.onClick.RemoveAllListeners();
        }

        private void OnClick()
        {
            _moneyService.Earn();
        }
    }
}