using System;
using Gameplay.Investments;
using TMPro;
using UI.Graph;
using UI.Helpers.Input;
using UnityEngine;
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
        [SerializeField] private Button buyButton;
        [SerializeField] private TMP_Text buyTimerText;

        [Header("Max Amount Button")]
        [SerializeField] private Button maxAmountBuyButton;
        [SerializeField] private Image maxAmountBuyImage;
        [SerializeField] private Color maxAmountBuyButtonInactiveColor;
        [SerializeField] private Color maxAmountBuyButtonActiveColor;

        [Header("Sell")]
        [SerializeField] private LongInputManager sellAmountInput;
        [SerializeField] private Button sellButton;
        [SerializeField] private TMP_Text sellTimerText;

        [Header("Max Amount Button")]
        [SerializeField] private Button maxAmountSellButton;
        [SerializeField] private Image maxAmountSellImage;
        [SerializeField] private Color maxAmountSellButtonInactiveColor;
        [SerializeField] private Color maxAmountSellButtonActiveColor;
        
        public event Action<InvestmentController, long> OnBuyButtonClick;
        public event Action<InvestmentController, long> OnSellButtonClick;

        private InvestmentController _investmentController;
        private bool _maxBuyAmount;
        private bool _maxSellAmount;

        private void Update()
        {
            UpdateResumptionTime();
            UpdateButtons();
        }

        private void OnDestroy()
        {
            _investmentController.OnInvestmentUpdate -= OnInvestmentUpdate;
        }

        public void Initialize(InvestmentController investmentController)
        {
            _investmentController = investmentController;
            investmentName.text = investmentController.InvestmentConfig.Name;

            buyAmountInput.InitializeInput(1, investmentController.MaxAmountCanBuy);
            sellAmountInput.InitializeInput(1, investmentController.Amount);
            
            chartManager.Initialize();

            UpdateBoughtAmount();
            OnInvestmentsCostUpdate();

            buyButton.onClick.AddListener(BuyButton_OnClick);
            sellButton.onClick.AddListener(SellButton_OnClick);

            maxAmountBuyButton.onClick.AddListener(MaxAmountBuyButton_OnClick);
            maxAmountBuyImage.color = maxAmountBuyButtonInactiveColor;

            maxAmountSellButton.onClick.AddListener(MaxAmountSellButton_OnClick);
            maxAmountSellImage.color = maxAmountSellButtonInactiveColor;

            _investmentController.OnInvestmentUpdate += OnInvestmentUpdate;
            OnInvestmentUpdate();
        }

        public void OnInvestmentsCostUpdate()
        {
            chartManager.UpdateValue(_investmentController.InvestmentModel.History);
            currentCost.text = _investmentController.InvestmentModel.CurrentCost.MoneyString();
            lastChange.text = $"{_investmentController.InvestmentModel.LastChange:F1}%";
            lastChange.color = _investmentController.InvestmentModel.LastChange > 0 ? Color.green : Color.red;
        }

        private void MaxAmountBuyButton_OnClick()
        {
            SetActiveMaxAmountBuyButton(!_maxBuyAmount);
        }

        private void SetActiveMaxAmountBuyButton(bool active)
        {
            if (active)
            {
                buyAmountInput.Value = _investmentController.MaxAmountCanBuy;
                maxAmountBuyImage.color = maxAmountBuyButtonActiveColor;
            }
            else
            {
                maxAmountBuyImage.color = maxAmountBuyButtonInactiveColor;
            }

            _maxBuyAmount = active;
        }

        private void MaxAmountSellButton_OnClick()
        {
            SetActiveMaxAmountSellButton(!_maxSellAmount);
        }

        private void SetActiveMaxAmountSellButton(bool active)
        {
            if (active)
            {
                sellAmountInput.Value = _investmentController.Amount;
                maxAmountSellImage.color = maxAmountSellButtonActiveColor;
            }
            else
            {
                maxAmountSellImage.color = maxAmountSellButtonInactiveColor;
            }

            _maxSellAmount = active;
        }

        private void UpdateButtons()
        {
            if (buyAmountInput.Value != _investmentController.MaxAmountCanBuy) SetActiveMaxAmountBuyButton(false);
            if (sellAmountInput.Value != _investmentController.Amount) SetActiveMaxAmountSellButton(false);

            buyButton.interactable =
                _investmentController.MaxAmountCanBuy > 0
                && buyAmountInput.Value > 0
                && !(_investmentController.InvestmentModel.ResumptionTime > 0);
            
            sellButton.interactable =
                _investmentController.Amount > 0
                && sellAmountInput.Value > 0
                && !(_investmentController.InvestmentModel.ResumptionTime > 0);
        }

        private void UpdateResumptionTime()
        {
            if (_investmentController.InvestmentModel.ResumptionTime > 0)
            {
                var time = _investmentController.InvestmentModel.ResumptionTime.TimeString();
                buyTimerText.text = time;
                sellTimerText.text = time;
            }
            else
            {
                buyTimerText.text = string.Empty;
                sellTimerText.text = string.Empty;
            }
        }

        private void OnInvestmentUpdate()
        {
            buyAmountInput.ChangeBounds(1, _investmentController.MaxAmountCanBuy);
            if (_maxBuyAmount) buyAmountInput.Value = _investmentController.MaxAmountCanBuy;

            sellAmountInput.ChangeBounds(1, _investmentController.Amount);
            if (_maxSellAmount) sellAmountInput.Value = _investmentController.Amount;
        }

        private void BuyButton_OnClick()
        {
            OnBuyButtonClick?.Invoke(_investmentController, buyAmountInput.Value);
            UpdateBoughtAmount();
        }

        private void SellButton_OnClick()
        {
            OnSellButtonClick?.Invoke(_investmentController, sellAmountInput.Value);
            UpdateBoughtAmount();
        }

        private void UpdateBoughtAmount()
        {
            amount.text =
                $"{_investmentController.Amount.ToString()}/{_investmentController.InvestmentConfig.MaxAmount}";
        }
    }
}