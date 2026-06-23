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

        public virtual void Initialize(Sprite icon, BusinessController businessController)
        {
            if (nameText)
                nameText.text = businessController.BusinessModel.Name;
            if (image)
                image.sprite = icon;
            if (incomeText)
                incomeText.text = businessController.GetIncome() > 0
                    ? businessController.GetIncome().MoneySpeedString()
                    : "";
            if (priceText)
                priceText.text = businessController.GetSellPrice().MoneyString();
            BusinessController = businessController;
        }

        protected virtual void Start()
        {
            sellButton.onClick.AddListener(SellButton_OnClick);
        }

        protected virtual void SellButton_OnClick()
        {
            _businessManager.SellBusiness(BusinessController);
            Destroy(gameObject);
        }
    }
}