using System.Linq;
using Configs;
using Gameplay.Estates.Generic;
using Gameplay.Services;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI.EstateViews.Default
{
    public class PurchaseView : MonoBehaviour
    {
        [SerializeField] private TMP_Text nameText;
        [SerializeField] private TMP_Text countText;
        [SerializeField] private Image image;
        [SerializeField] private TMP_Text priceText;
        [SerializeField] private TMP_Text incomeText;
        [SerializeField] private TMP_InputField nameInput;
        [SerializeField] private Button buyButton;

        [Inject]private MoneyService _moneyService;
        [Inject]private EstateManager _estateManager;

        private EstateConfig _config;

        public void Initialize(Sprite icon, EstateConfig config)
        {
            nameText.text = config.name;
            image.sprite = icon;
            priceText.text = $"{config.Price}$";
            incomeText.text = $"{config.Income}$";
            _config = config;
        }

        private void Start()
        {
            buyButton.onClick.AddListener(Buy);
            _moneyService.OnMoneyChanged += CheckButton;
            _estateManager.OnEstatesChanged += CheckButton;
            CheckButton();
        }

        private void OnDestroy()
        {
            if (_moneyService != null)
                _moneyService.OnMoneyChanged -= CheckButton;
            if (_estateManager != null)
                _estateManager.OnEstatesChanged -= CheckButton;
        }

        private void Buy()
        {
            var text = nameInput.text;
            if (text.Length == 0)
                text = "Unnamed";
            if (!_estateManager.TryAddEstate(text, _config)) return;
            nameInput.text = "";
        }

        private void CheckButton()
        {
            if (!_config) return;
            var num = _estateManager.Estates.Count(estate => estate.Type == _config.Type);
            countText.text =
                $"{num}/{_config.MaxCount}";
            if (_moneyService == null) return;
            buyButton.interactable = 
                _moneyService.CanSpend(_config.Income) && num < _config.MaxCount;
        }
    }
}