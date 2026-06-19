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

        private AvailableBusinessController _businessController;

        public void Initialize(Sprite icon, AvailableBusinessController businessController)
        {
            nameText.text = businessController.BusinessConfig.BusinessName;
            image.sprite = icon;
            priceText.text = $"{businessController.BusinessConfig.Price}$";
            incomeText.text = $"{businessController.BusinessModel.Income}$";
            _businessController = businessController;
        }

        private void Start()
        {
            buyButton.onClick.AddListener(Buy);
            _businessController.OnBuyStatusUpdated += OnBuyStatusUpdated;
            OnBuyStatusUpdated(false);
        }

        private void OnDestroy()
        {
            _businessController.OnBuyStatusUpdated-=OnBuyStatusUpdated;
        }

        private void Buy()
        {
            var text = nameInput.text;
            if (text.Length == 0)
                text = "Unnamed";
            _businessController.UserBusinessName = text;
            if (!_businessManager.AddBusiness(_businessController)) return;
            nameInput.text = "";
        }

        private void OnBuyStatusUpdated(bool value)
        {
            var num = _businessManager.GetTypeCount(_businessController.BusinessConfig.Type);
            countText.text =
                $"{num}/{_businessController.BusinessConfig.MaxCount}";
            if (_moneyService == null) return;
            buyButton.interactable = 
                _businessController.CanBuy;
        }
    }
}