using System;
using Configs;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace UI.BusinessScreen.Store
{
    public class BusinessLineView : MonoBehaviour
    {
        [Header("Controlled elements")]
        [SerializeField] private Image image;
        [SerializeField] private TMP_Text businessName;
        [SerializeField] private TMP_Text price;
        [SerializeField] private Button panelAreaButton;

        private BusinessConfig _config;
        
        public event Action<BusinessConfig> OnClick;

        private void Awake()
        {
            panelAreaButton.onClick.AddListener(Button_OnClick);
        }
        
        public void Initialize(Sprite icon, BusinessConfig config)
        {
            image.sprite = icon;
            businessName.text = config.BusinessName;
            price.text = StringFormatUtility.MoneyString(config.Price);

            _config = config;
        }

        private void Button_OnClick()
        {
            OnClick?.Invoke(_config);
        }
    }
}