using Gameplay.Services;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI.ClickView
{
    public class TapPanelView : MonoBehaviour
    {
        [SerializeField] private Button tapAreaButton;
        
        [Inject] private MoneyService _moneyService;

        private void Awake()
        {
            tapAreaButton.onClick.AddListener(TapAreaButton_OnClick);
        }

        private void TapAreaButton_OnClick()
        {
            _moneyService.TapEarn();
        }
    }
}