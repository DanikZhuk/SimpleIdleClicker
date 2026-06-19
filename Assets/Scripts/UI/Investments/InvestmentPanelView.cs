using System;
using Configs;
using Gameplay.Investments;
using TMPro;
using UI.Graph;
using UI.Helpers.Input;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Utils;

namespace UI.Investments
{
    public class InvestmentPanelView : MonoBehaviour
    {
        [Header("General")]
        [SerializeField] private TMP_Text investmentName;
        [SerializeField] private AbsoluteLineChartManager chartManager;
        [SerializeField] private TMP_Text currentCost;
        [SerializeField] private TMP_Text lastChange;
        [SerializeField] private TMP_Text amount;
        [Header("Buy")]
        [SerializeField] private LongInputManager buyAmountInput;
        [SerializeField]private Button maxAmountBuyButton;
        [SerializeField] private Button buyButton;
        [SerializeField] private TMP_Text buyTimerText;
        [Header("Sell")] 
        [SerializeField] private LongInputManager sellAmountInput;
        [SerializeField]private Button maxAmountSellButton;
        [SerializeField] private Button sellButton;
        [SerializeField] private TMP_Text sellTimerText;

        public event Action<InvestmentController, long> OnBuyButtonClick;
        public event Action<InvestmentController, long> OnSellButtonClick;

        private InvestmentController _investmentController;

        public void Initialize(InvestmentController investmentController)
        {
            _investmentController = investmentController;
            investmentName.text = investmentController.InvestmentConfig.Name;
            buyAmountInput.InitializeInput(1, investmentController.MaxAmountCanBuy);
            sellAmountInput.InitializeInput(1, investmentController.Amount);
            UpdateAmount();
            UpdateValues();
            buyButton.onClick.AddListener(BuyButton_OnClick);
            sellButton.onClick.AddListener(SellButton_OnClick);
            
            maxAmountBuyButton.onClick.AddListener(MaxAmountBuyButton_OnClick);
            maxAmountSellButton.onClick.AddListener(MaxAmountSellButton_OnClick);

            _investmentController.OnStatusChanged += OnStatusChanged;
            OnStatusChanged();
        }

        private void MaxAmountBuyButton_OnClick()
        {
            buyAmountInput.Value = _investmentController.MaxAmountCanBuy;
        }
        
        private void MaxAmountSellButton_OnClick()
        {
            sellAmountInput.Value = _investmentController.Amount;
        }

        private void Update()
        {
            UpdateTime();
            UpdateButtons();
        }

        private void UpdateButtons()
        {
            buyButton.interactable = _investmentController.CanBuy && buyAmountInput.Value> 0;
            sellButton.interactable = _investmentController.CanSell && sellAmountInput.Value > 0;
        }

        private void UpdateAmount()
        {
            amount.text = $"{_investmentController.Amount.ToString()}/{_investmentController.InvestmentConfig.MaxAmount}";
        }

        public void UpdateValues()
        {
            chartManager.UpdateValue(_investmentController.InvestmentModel.History);
            currentCost.text = StringFormatUtility.MoneyString(_investmentController.InvestmentModel.CurrentCost);
            lastChange.text = $"{_investmentController.InvestmentModel.LastChange:F1}%";
            lastChange.color = _investmentController.InvestmentModel.LastChange > 0 ? Color.green : Color.red;
        }

        private void UpdateTime()
        {
            if (_investmentController.InvestmentModel.ResumptionTime > 0)
            {
                var time = StringFormatUtility.TimeString(_investmentController.InvestmentModel.ResumptionTime);
                buyTimerText.text = time;
                sellTimerText.text = time;
            }
            else
            {
                buyTimerText.text = "";
                sellTimerText.text = "";
            }
        }

        private void OnStatusChanged()
        {
            buyAmountInput.ChangeBounds(1, _investmentController.MaxAmountCanBuy);
            sellAmountInput.ChangeBounds(1, _investmentController.Amount);
        }

        private void BuyButton_OnClick()
        {
            OnBuyButtonClick?.Invoke(_investmentController, buyAmountInput.Value);
            UpdateAmount();
        }

        private void SellButton_OnClick()
        {
            OnSellButtonClick?.Invoke(_investmentController, sellAmountInput.Value);
            UpdateAmount();
        }

        private void OnDestroy()
        {
            _investmentController.OnStatusChanged-= OnStatusChanged;
        }
    }
}