using System;
using Gameplay.Businesses.BusinessControllers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace UI.BusinessScreen.Purchased
{
    public class PurchasedLineView : MonoBehaviour
    {
        [Header("Controlled elements")]
        [SerializeField] private Image image;
        [SerializeField] private TMP_Text estateName;
        [SerializeField] private TMP_Text income;
        [SerializeField] private Button panelAreaButton;

        private BusinessController _businessController;

        private void Awake()
        {
            panelAreaButton.onClick.AddListener(PanelAreaButton_OnClick);
        }

        public event Action<BusinessController> OnClick;

        public void Initialize(Sprite icon, BusinessController businessController)
        {
            image.sprite = icon;
            estateName.text = businessController.BusinessModel.Name;
            income.text = businessController.BusinessModel.Income > 0
                ? businessController.BusinessModel.Income.MoneySpeedString()
                : "";
            _businessController = businessController;
        }

        private void PanelAreaButton_OnClick()
        {
            OnClick?.Invoke(_businessController);
        }
    }
}