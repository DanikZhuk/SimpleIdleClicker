using System;
using Gameplay.Investments;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace UI.Investments
{
    public class InvestmentLineView : MonoBehaviour
    {
        [SerializeField] private Button panelButton;
        [SerializeField] private Image image;
        [SerializeField] private TMP_Text nameText;
        [SerializeField] private TMP_Text currentCost;

        public event Action<InvestmentController> OnClick;
        
        private InvestmentController _investmentController;

        private void Awake()
        {
            panelButton.onClick.AddListener(PanelButton_OnClick);
        }
        
        public void Initialize(InvestmentController investmentController, Sprite investmentImage)
        {
            image.sprite = investmentImage;
            nameText.text = investmentController.InvestmentConfig.name;
            _investmentController = investmentController;
        }

        public void UpdateValues()
        {
            currentCost.text = _investmentController.InvestmentModel.CurrentCost.MoneyString();
        }

        private void PanelButton_OnClick()
        {
            if (_investmentController != null) OnClick?.Invoke(_investmentController);
        }
    }
}