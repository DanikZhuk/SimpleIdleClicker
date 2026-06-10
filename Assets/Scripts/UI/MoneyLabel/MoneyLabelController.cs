using Gameplay.GameManager;
using Gameplay.Services.MoneyService;
using Reflex.Attributes;
using TMPro;
using UnityEngine;

namespace UI.MoneyLabel
{
    public class MoneyLabelController : MonoBehaviour
    {
        [SerializeField] private TMP_Text moneyText;

        [Inject] private IMoneyService _moneyService;
        
        private void Start()
        {
            _moneyService.OnMoneyChanged += UpdateText;
            UpdateText();
        }

        private void OnDestroy()
        {
            _moneyService.OnMoneyChanged -= UpdateText;
        }

        private void UpdateText()
        {
            moneyText.text = $"{_moneyService.Money}$";
        }
    }
}