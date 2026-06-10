using System.Linq;
using Configs;
using Gameplay.Estates.Generic;
using Gameplay.Services.MoneyService;
using Reflex.Attributes;
using TMPro;
using UI.TabControls.CloseTab;
using UnityEngine;
using UnityEngine.UI;

namespace UI.EstateTab
{
    public class PurchaseController : MonoBehaviour
    {
        [SerializeField] private TMP_Text nameText;
        [SerializeField] private TMP_Text countText;
        [SerializeField] private Image image;
        [SerializeField] private TMP_Text priceText;
        [SerializeField] private TMP_Text incomeText;
        [SerializeField] private TMP_InputField nameInput;
        [SerializeField] private Button buyButton;

        private EstateConfig _config;

        [Inject] IMoneyService _moneyService;
        [Inject] EstateManager _estateManager;

        public EstateConfig Config
        {
            get => _config;
            set
            {
                _config = value;
                UpdateInfo();
            }
        }

        private void UpdateInfo()
        {
            if (!_config)
                return;
            nameText.text = _config.name;
            image.sprite = _config.Icon;
            priceText.text = $"{_config.Price}$";
            incomeText.text = $"{_config.Income}$";
        }

        private void Start()
        {
            Initialize();
        }

        private void Initialize()
        {
            buyButton.onClick.AddListener(Buy);
            _moneyService.OnMoneyChanged += CheckButton;
            _estateManager.OnEstatesChanged += CheckButton;
            UpdateInfo();
            CheckButton();
        }

        private void OnDestroy()
        {
            buyButton.onClick.RemoveAllListeners();
            if (_moneyService != null)
                _moneyService.OnMoneyChanged -= CheckButton;
            if (_estateManager != null)
                _estateManager.OnEstatesChanged -= CheckButton;
        }

        private void Buy()
        {
            if (!_moneyService.CanSpend(_config.Price)) return;
            var text = nameInput.text;
            if (text.Length == 0)
                text = "Unnamed";
            if (!_estateManager.TryAddEstate(text, _config)) return;
            nameInput.text = "";
            _moneyService.TrySpend(_config.Price);
        }

        private void CheckButton()
        {
            var num = _estateManager.Estates.Count(estate => estate.Config.Type == _config.Type);
            countText.text =
                $"{num}/{_config.MaxCount}";
            if (_moneyService == null) return;
            buyButton.interactable = (_moneyService.CanSpend(_config.Income)&&num<_config.MaxCount);
        }
    }
}