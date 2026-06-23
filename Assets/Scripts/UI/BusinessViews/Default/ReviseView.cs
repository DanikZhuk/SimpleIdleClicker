using Gameplay.Businesses;
using Gameplay.Businesses.BusinessControllers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utils;
using Zenject;

namespace UI.BusinessViews.Default
{
    public class ReviseView : MonoBehaviour
    {
        [SerializeField] private TMP_Text nameText;
        [SerializeField] private Image image;
        [SerializeField] private TMP_Text incomeText;
        [SerializeField] private TMP_Text priceText;
        [SerializeField] private Button sellButton;

        [Inject] private BusinessManager _businessManager;

        protected BusinessController BusinessController;

        protected virtual void Start()
        {
            sellButton.onClick.AddListener(SellButton_OnClick);
        }

        public virtual void Initialize(Sprite icon, BusinessController businessController)
        {
            if (nameText)
                nameText.text = businessController.BusinessModel.Name;
            if (image)
                image.sprite = icon;
            if (incomeText)
                incomeText.text = businessController.IncomePerHour > 0
                    ? businessController.IncomePerHour.MoneySpeedString()
                    : "";
            if (priceText)
                priceText.text = businessController.GetSellPrice().MoneyString();
            BusinessController = businessController;
        }

        protected virtual void SellButton_OnClick()
        {
            _businessManager.SellBusiness(BusinessController);
            Destroy(gameObject);
        }
    }
}