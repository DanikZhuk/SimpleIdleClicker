using System;
using Gameplay.Businesses.BusinessControllers.RepairShop.House.Controller;
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
        
        private AvailableHouseController _houseController;
        
        public AvailableHouseController HouseController=>_houseController;

        private void Awake()
        {
            buyButton.onClick.AddListener(() => OnBuyButtonClick?.Invoke(this));
        }

        public void Initialize(AvailableHouseController houseController)
        {
            _houseController = houseController;
            buyCost.text=StringFormatUtility.MoneyString(houseController.HouseModel.Cost);
            _houseController.OnCanBuyUpdate += OnCanBuyUpdate;
            OnCanBuyUpdate();
        }

        public void Clear()
        {
            _houseController.OnCanBuyUpdate -= OnCanBuyUpdate;
        }

        private void OnDestroy()
        {
            Clear();
        }

        private void OnCanBuyUpdate()
        {
            buyButton.interactable = _houseController.CanBuy;
        }

        public void SetImage(Sprite sprite)
        {
            image.sprite = sprite;
        }
    }
}