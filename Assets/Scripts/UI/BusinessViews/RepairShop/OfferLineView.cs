using System;
using Gameplay.Businesses.BusinessControllers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace UI.BusinessViews.RepairShop
{
    public class OfferLineView: MonoBehaviour
    {
        [SerializeField] private Image image;
        [SerializeField] private Button buyButton;
        [SerializeField] private TMP_Text buyCost;
        
        public event Action<OfferLineView> OnBuyButtonClick;

        public HouseController HouseController { get; private set; }

        private void Awake()
        {
            buyButton.onClick.AddListener(() => OnBuyButtonClick?.Invoke(this));
        }

        public void Initialize(HouseController houseController)
        {
            HouseController = houseController;
            buyCost.text=StringFormatUtility.MoneyString(houseController.Cost);
        }

        public void UpdateBuyButton(long money, bool hasFreeSpace)
        {
            buyButton.interactable = money >= HouseController.Cost && hasFreeSpace;
        }

        public void SetImage(Sprite sprite)
        {
            image.sprite = sprite;
        }
    }
}