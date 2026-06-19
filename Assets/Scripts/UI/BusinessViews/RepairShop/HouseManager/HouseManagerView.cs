using System.Collections.Generic;
using Gameplay.Businesses.BusinessControllers.RepairShop;
using Gameplay.Businesses.BusinessControllers.RepairShop.House.Model;
using Gameplay.Businesses.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.U2D.Animation;

namespace UI.BusinessViews.RepairShop.HouseManager
{
    public class HouseManagerView : MonoBehaviour
    {
        [Header("Offers")] [SerializeField] OfferLineView offerLineViewPrefab;
        [SerializeField] Transform offerLineContainer;
        [SerializeField] SpriteLibrary library;

        [FormerlySerializedAs("renovationLineViewPrefab")] [Header("Renovation")] [SerializeField]
        RepairLineView repairLineViewPrefab;

        [SerializeField] Transform renovationLineContainer;

        [Header("Info Elements")] [SerializeField]
        TMP_Text counter;

        private const string Category = "House";

        private List<RepairLineView> _repairLines = new();
        private List<OfferLineView> _offerLines = new();
        private RepairShopBusinessController _businessController;

        public void Initialize(IBusinessController businessController)
        {
            _businessController = businessController as RepairShopBusinessController;
        }

        private void Start()
        {
            UpdateInfo();
            _businessController.OnHousesUpdate += UpdateInfo;
        }

        private void OnDestroy()
        {
            foreach (var line in _offerLines)
            {
                line.Clear();
            }

            foreach (var line in _repairLines)
            {
                line.Clear();
            }
            _businessController.OnHousesUpdate -= UpdateInfo;
        }

        private void UpdateInfo()
        {
            UpdateOffersInfo();
            UpdateRenovationInfo();
        }

        private void UpdateOffersInfo()
        {
            var index = 0;
            for (; index < _businessController.HouseOffers.Count; index++)
            {
                var houseController = _businessController.HouseOffers[index];

                OfferLineView line;
                if (index < _offerLines.Count)
                {
                    line = _offerLines[index];
                    line.Clear();
                }
                else
                {
                    line = Instantiate(offerLineViewPrefab, offerLineContainer);
                    line.OnBuyButtonClick += BuyHouse;
                    _offerLines.Add(line);
                }

                line.Initialize(houseController);
                line.SetImage(
                    GetImage(houseController.HouseModel.Condition)
                );
            }

            for (var i = _offerLines.Count - 1; i >= index; i--)
            {
                var line = _offerLines[i];
                line.Clear();
                Destroy(line.gameObject);
                _offerLines.RemoveAt(i);
            }
        }

        private void UpdateRenovationInfo()
        {
            var index = 0;
            for (; index < _businessController.PurchasedHouses.Count; index++)
            {
                var purchasedHouseController = _businessController.PurchasedHouses[index];

                RepairLineView line;
                if (index < _repairLines.Count)
                {
                    line = _repairLines[index];
                    line.Clear();
                }
                else
                {
                    line = Instantiate(repairLineViewPrefab, renovationLineContainer);
                    line.OnSellButtonClick += SellHouse;
                    line.OnRenovationButtonClick += OnRepairButton;

                    _repairLines.Add(line);
                }

                line.Initialize(purchasedHouseController);
                line.SetImage(
                    GetImage(purchasedHouseController.HouseModel.Condition)
                );
            }

            for (var i = _repairLines.Count - 1; i >= index; i--)
            {
                var line = _repairLines[i];
                line.Clear();
                Destroy(line.gameObject);
                _repairLines.RemoveAt(i);
            }

            counter.text = $"{_repairLines.Count}/{_businessController.MaxPurchasedHousesAmount}";
        }

        private Sprite GetImage(HouseCondition type)
        {
            if (type == HouseCondition.UnderRepair)
                type = HouseCondition.NeedRepair;
            return library.GetSprite(Category, type.ToString());
        }

        private void BuyHouse(OfferLineView line)
        {
            _businessController.BuyHouse(line.HouseController);
        }

        private void SellHouse(RepairLineView line)
        {
            _businessController.SellHouse(line.HouseController);
        }

        private void OnRepairButton(RepairLineView line)
        {
            _businessController.RepairHouse(line.HouseController);
        }
    }
}