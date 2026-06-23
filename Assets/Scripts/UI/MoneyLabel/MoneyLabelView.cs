using Gameplay.Services;
using TMPro;
using UnityEngine;
using Zenject;

namespace UI.MoneyLabel
{
    public class MoneyLabelView : MonoBehaviour
    {
        [SerializeField] private TMP_Text moneyText;

        [Inject] private MoneyService _moneyService;

        private void Start()
        {
            _moneyService.OnMoneyChanged += OnMoneyChanged;
            OnMoneyChanged();
        }

        private void OnDestroy()
        {
            _moneyService.OnMoneyChanged -= OnMoneyChanged;
        }

        private void OnMoneyChanged()
        {
            moneyText.text = $"{_moneyService.Money}$";
        }
    }
}