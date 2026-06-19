using System;
using System.Globalization;
using Configs;
using Gameplay.Investitions;
using TMPro;
using UI.Graph;
using UnityEngine;
using UnityEngine.UI;
using Utils;

public class InvestitionView : MonoBehaviour
{
    [Header("General")]
    [SerializeField] private TMP_Text investitionName;
    [SerializeField] private AbsoluteLineChartManager chartManager;
    [SerializeField] private TMP_Text currentCost;
    [SerializeField] private TMP_Text lastChange;
    [SerializeField] private TMP_Text amount;
    [Header("Buy")] [SerializeField] private Button buyButton;
    [SerializeField] private IntInputManager buyAmountInput;
    [SerializeField] private TMP_Text buyTimerText;
    [Header("Sell")] [SerializeField] private Button sellButton;
    [SerializeField] private IntInputManager sellAmountInput;
    [SerializeField] private TMP_Text sellTimerText;

    public event Action<InvestitionType, int> OnBuyButtonClick;
    public event Action<InvestitionType, int> OnSellButtonClick;

    private InvestitionConfig _investitionConfig;
    private Investition _investition;

    public void Initialize(Investition investition, InvestitionConfig investitionConfig)
    {
        _investitionConfig = investitionConfig;
        _investition = investition;
        investitionName.text = _investitionConfig.Name;
        buyAmountInput.InitializeInput(1, _investitionConfig.MaxAmount);
        sellAmountInput.InitializeInput(1, _investition.PurchasedAmount);
        UpdateAmount();
        UpdateValues();
        buyButton.onClick.AddListener(BuyButton_OnClick);
        sellButton.onClick.AddListener(SellButton_OnClick);
    }

    private void Update()
    {
        UpdateTime();
    }

    private void UpdateAmount()
    {
        amount.text = $"{_investition.PurchasedAmount.ToString()}/{_investitionConfig.MaxAmount}";
    }

    public void UpdateValues()
    {
        chartManager.UpdateValue(_investition.History);
        currentCost.text = StringFormatUtility.MoneyString(_investition.CurrentCost);
        lastChange.text = $"{_investition.LastChange:F1}%";
        lastChange.color = _investition.LastChange > 0 ? Color.green : Color.red;
    }

    private void UpdateTime()
    {
        if (_investition.ResumptionTime > 0)
        {
            var time = StringFormatUtility.TimeString(_investition.ResumptionTime);
            buyTimerText.text = time;
            sellTimerText.text = time;
        }
        else
        {
            buyTimerText.text = "";
            sellTimerText.text = "";
        }
    }

    private void BuyButton_OnClick()
    {
        OnBuyButtonClick?.Invoke(_investition.Type, buyAmountInput.GetValue());
        buyAmountInput.ChangeBounds(1, _investitionConfig.MaxAmount - _investition.PurchasedAmount);
        sellAmountInput.ChangeBounds(1, _investition.PurchasedAmount);
        UpdateAmount();
    }

    private void SellButton_OnClick()
    {
        OnSellButtonClick?.Invoke(_investition.Type, sellAmountInput.GetValue());
        buyAmountInput.ChangeBounds(1, _investitionConfig.MaxAmount - _investition.PurchasedAmount);
        sellAmountInput.ChangeBounds(1, _investition.PurchasedAmount);
        UpdateAmount();
    }
}