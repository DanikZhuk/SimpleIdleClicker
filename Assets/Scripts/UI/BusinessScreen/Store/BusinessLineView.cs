using System;
using Gameplay.Businesses.BusinessControllers;
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
        [SerializeField] private TMP_Text estateName;
        [SerializeField] private TMP_Text price;
        [SerializeField] private Button panelAreaButton;

        public event Action<AvailableBusinessController> OnClick;
        
        private AvailableBusinessController _businessController;

        private void Awake()
        {
            panelAreaButton.onClick.AddListener(Button_OnClick);
        }
        
        public void Initialize(Sprite icon, AvailableBusinessController availableBusinessController)
        {
            image.sprite = icon;
            estateName.text = availableBusinessController.BusinessModel.Name;
            price.text = StringFormatUtility.MoneyString(availableBusinessController.BusinessConfig.Price);
            _businessController = availableBusinessController;
        }

        private void Button_OnClick()
        {
            OnClick?.Invoke(_businessController);
        }
    }
}