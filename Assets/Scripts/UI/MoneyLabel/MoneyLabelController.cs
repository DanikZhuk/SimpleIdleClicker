using Gameplay.GameManager;
using TMPro;
using UnityEngine;

namespace UI.MoneyLabel
{
    public class MoneyLabelController : MonoBehaviour
    {
        [SerializeField] private TMP_Text moneyText;
        [SerializeField] private GameManager gameManager;

        private void Start()
        {
            gameManager.MoneyService.OnMoneyChanged += UpdateText;
            UpdateText();
        }

        private void OnDestroy()
        {
            gameManager.MoneyService.OnMoneyChanged -= UpdateText;
        }

        private void UpdateText()
        {
            moneyText.text = $"{gameManager.MoneyService.Money}$";
        }
    }
}