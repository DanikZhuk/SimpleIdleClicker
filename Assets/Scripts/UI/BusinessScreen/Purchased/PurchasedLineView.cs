using System;
using Gameplay.Businesses.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Utils;

namespace UI.EstateList.Purchased
{
    public class PurchasedLineView : MonoBehaviour
    {
        [Header("Controlled elements")]
        [SerializeField] private Image image;
        [SerializeField] private TMP_Text estateName;
        [SerializeField] private TMP_Text income;
        [FormerlySerializedAs("button")] [SerializeField] private Button panelAreaButton;

        public event Action<IBusinessController> OnClick;

        private IBusinessController _businessController;

        private void Awake()
        {
            panelAreaButton.onClick.AddListener(PanelAreaButton_OnClick);
        }
        
        public void Initialize(Sprite icon, IBusinessController businessController)
        {
            image.sprite = icon;
            estateName.text = businessController.BusinessModel.Name;
            income.text = StringFormatUtility.MoneyString(businessController.BusinessModel.Income);
            _businessController = businessController;
        }

        private void PanelAreaButton_OnClick()
        {
            OnClick?.Invoke(_businessController);
        }
    }
}