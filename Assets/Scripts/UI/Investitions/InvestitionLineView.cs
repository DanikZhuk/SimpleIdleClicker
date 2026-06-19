using System;
using Configs;
using Gameplay.Investitions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utils;

public class InvestitionLineView : MonoBehaviour
{
    [SerializeField] private Button panelButton;
    [SerializeField] private Image image;
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text currentCost;

    public event Action<Investition> OnClick;

    private Investition _investition;
    
    public void Initialize(Investition investition, InvestitionConfig config, Sprite investitionImage)
    {
        image.sprite = investitionImage;
        nameText.text = config.name;
        _investition = investition;
    }

    public void UpdateValues()
    {
        currentCost.text = StringFormatUtility.MoneyString(_investition.CurrentCost);
    }
    
    private void Awake()
    {
        panelButton.onClick.AddListener(PanelButton_OnClick);
    }

    private void PanelButton_OnClick()
    {
        if (_investition != null)
        {
            OnClick?.Invoke(_investition);
        }
    }
}