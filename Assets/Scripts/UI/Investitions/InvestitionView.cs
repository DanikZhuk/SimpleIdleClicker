using System;
using System.Globalization;
using Configs;
using DefaultNamespace;
using Gameplay.Investitions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InvestitionView : MonoBehaviour
{
    [Header("General")]
    [SerializeField] private TMP_Text investitionName;
    [SerializeField] private AbsoluteLineChartManager chartManager;
    [SerializeField] private TMP_Text currentCost;
    [SerializeField] private TMP_Text lastChange;
    [SerializeField] private TMP_Text amount;
    [Header("Buy")]
    [SerializeField] private Button buyButton;
    [SerializeField] private IntInputManager buyAmountInput;
    [SerializeField] private TMP_Text buyTimerText;
    [Header("Sell")]
    [SerializeField] private Button sellButton;
    [SerializeField] private IntInputManager sellAmountInput;
    [SerializeField] private TMP_Text sellTimerText;
    
    public event Action<InvestitionType, int> OnBuyButtonClick;
    public event Action<InvestitionType, int> OnSellButtonClick;
    
    private InvestitionConfig _investitionConfig;
    private Investition _investition;

    public void Initialize(InvestitionConfig investitionConfig, Investition investition)
    {
        _investitionConfig = investitionConfig;
        _investition = investition;
        investitionName.text = _investitionConfig.Name;
        buyAmountInput.InitializeInput(1, _investitionConfig.MaxAmount);
        sellAmountInput.InitializeInput(1, _investition.PurchasedAmount);
        
        buyButton.onClick.AddListener(BuyButton_OnClick);
        sellButton.onClick.AddListener(SellButton_OnClick);
    }

    private void Update()
    {
        UpdateTime();
    }

    public void UpdateValues()
    {
        chartManager.UpdateValue(_investition.History);
        currentCost.text = _investition.CurrentCost.ToString(CultureInfo.InvariantCulture);
        lastChange.text = _investition.LastChange.ToString(CultureInfo.InvariantCulture);
    }

    private void UpdateTime()
    {
        if (_investition.ResumptionTime > 0)
        {
            int minutes = (int)(_investition.ResumptionTime / 60);
            int seconds = (int)(_investition.ResumptionTime % 60);
            buyTimerText.text = $"{minutes}:{seconds:D2}";
            sellTimerText.text = $"{minutes}:{seconds:D2}";
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
        buyAmountInput.ChangeBounds(1, _investitionConfig.MaxAmount-_investition.PurchasedAmount);
        sellAmountInput.ChangeBounds(1, _investition.PurchasedAmount);
        amount.text = $"{_investition.PurchasedAmount.ToString()}/{_investitionConfig.MaxAmount}";
    }
    
    private void SellButton_OnClick()
    {
        OnSellButtonClick?.Invoke(_investition.Type, sellAmountInput.GetValue());
        buyAmountInput.ChangeBounds(1, _investitionConfig.MaxAmount-_investition.PurchasedAmount);
        sellAmountInput.ChangeBounds(1, _investition.PurchasedAmount);
        amount.text = $"{_investition.PurchasedAmount.ToString()}/{_investitionConfig.MaxAmount}";
    }

}
