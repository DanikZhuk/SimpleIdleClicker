using System;
using Configs;
using Gameplay.Estates.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.EstateList.Purchased
{
    public class PurchasedLineView : MonoBehaviour
    {
        [Header("Controlled elements")]
        [SerializeField] private Image image;
        [SerializeField] private TMP_Text estateName;
        [SerializeField] private TMP_Text income;
        [SerializeField] private Button button;

        public event Action<Estate> OnClick;

        private Estate _estate;

        private void Awake()
        {
            button.onClick.AddListener(Button_OnClick);
        }
        
        public void Initialize(Sprite icon, Estate estate, EstateConfig config)
        {
            image.sprite = icon;
            estateName.text = estate.Name;
            income.text = $"{config.Income}$";
            _estate = estate;
        }

        private void Button_OnClick()
        {
            OnClick?.Invoke(_estate);
        }
    }
}