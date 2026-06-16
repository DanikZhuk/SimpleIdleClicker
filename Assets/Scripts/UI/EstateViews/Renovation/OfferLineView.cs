using System;
using TMPro;
using UI.EstatePage.EstateViews.Renovation;
using UnityEngine;
using UnityEngine.UI;
using Utils.String;

namespace UI.EstateViews.Renovation
{
    public class OfferLineView: MonoBehaviour
    {
        [SerializeField] private Image image;
        [SerializeField] private Button buyButton;
        [SerializeField] private TMP_Text buyCost;
        
        public event Action<OfferLineView> OnBuyButtonClick;
        
        private House _house;
        
        public House House=>_house;

        private void Awake()
        {
            buyButton.onClick.AddListener(() => OnBuyButtonClick?.Invoke(this));
        }

        public void Initialize(House house)
        {
            _house = house;
            buyCost.text=StringCreator.MoneyString(house.Cost);
        }

        public void SetImage(Sprite sprite)
        {
            image.sprite = sprite;
        }
    }
}