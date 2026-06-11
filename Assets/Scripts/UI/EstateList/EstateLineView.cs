using System;
using Configs;
using Gameplay.Estates.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.EstateList
{
    public class EstateLineView : MonoBehaviour
    {
        [Header("Controlled elements")]
        [SerializeField] private Image image;
        [SerializeField] private TMP_Text estateName;
        [SerializeField] private TMP_Text price;
        [SerializeField] private Button button;

        public event Action<EstateType> OnClick;

        private EstateType _type;

        private void Awake()
        {
            button.onClick.AddListener(Button_OnClick);
        }
        
        public void Initialize(Sprite icon, EstateConfig config)
        {
            image.sprite = icon;
            estateName.text = config.EstateName;
            price.text = $"{config.Price}$";
            _type = config.Type;
        }

        private void Button_OnClick()
        {
            OnClick?.Invoke(_type);
        }
    }
}