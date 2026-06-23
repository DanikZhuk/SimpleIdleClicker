using Configs;
using Gameplay.Businesses;
using Gameplay.Businesses.BusinessControllers;
using Gameplay.Services;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI.BusinessViews.Default
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
        [Inject]private BusinessManager _businessManager;

        private BusinessConfig _businessConfig;

        public void Initialize(Sprite icon, BusinessConfig businessConfig)
        {
            nameText.text = businessConfig.BusinessName;
            image.sprite = icon;
            priceText.text = $"{businessConfig.Price}$";
            incomeText.text = $"{businessConfig.Income}$";
            _businessConfig = businessConfig;
        }

        private void Start()
        {
            buyButton.onClick.AddListener(Buy);
            _businessManager.OnBusinessesChanged += OnBusinessesChanged;
            OnBusinessesChanged();
        }

        private void Update()
        {
            CheckButtonStatus();
        }

        private void OnDestroy()
        {
            _businessManager.OnBusinessesChanged -= OnBusinessesChanged;;
        }

        private void CheckButtonStatus()
        {
            buyButton.interactable = _moneyService.Money >= _businessConfig.Price;
        }

        private void Buy()
        {
            var text = nameInput.text;
            if (text.Length == 0)
                text = _businessConfig.BusinessName;
            if (!_businessManager.AddBusiness(_businessConfig.Type, text)) return;
            nameInput.text = "";
        }

        private void OnBusinessesChanged()
        {
            var num = _businessManager.GetTypeCount(_businessConfig.Type);
            countText.text =
                $"{num}/{_businessConfig.MaxCount}";
        }
    }
}