using Gameplay.Businesses;
using Gameplay.Businesses.Generic;
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

        protected IBusinessController BusinessController;

        public virtual void Initialize(Sprite icon, IBusinessController businessController)
        {
            if (nameText)
                nameText.text = businessController.BusinessModel.Name;
            if (image)
                image.sprite = icon;
            if (incomeText)
                incomeText.text = BusinessController.BusinessModel.Income > 0
                    ? StringFormatUtility.MoneySpeedString(businessController.BusinessModel.Income)
                    : "";
            if (priceText)
                priceText.text = StringFormatUtility.MoneyString(businessController.BusinessModel.Cost);
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