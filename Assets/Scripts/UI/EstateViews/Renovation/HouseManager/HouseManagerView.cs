using System;
using System.Collections.Generic;
using System.Linq;
using Gameplay.Estates.Generic;
using Gameplay.Estates.Renovation;
using TMPro;
using UI.EstatePage.EstateViews.Renovation.Model;
using UI.EstateViews.Renovation;
using UnityEngine;
using UnityEngine.U2D.Animation;

namespace UI.EstatePage.EstateViews.Renovation.HouseManager
{
    public class HouseManagerView: MonoBehaviour
    {
        [Header("Offers")]
        [SerializeField] OfferLineView offerLineViewPrefab;
        [SerializeField] Transform offerLineContainer;
        [SerializeField] SpriteLibrary library;
        [Header("Renovation")]
        [SerializeField] RenovationController renovationController;
        [SerializeField] RenovationLineView renovationLineViewPrefab;
        [SerializeField] Transform renovationLineContainer;
        [Header("Info Elements")]
        [SerializeField] TMP_Text counter;

        private const string Category = "House";

        private List<RenovationLineView> _renovationLines = new();
        private List<OfferLineView> _offerLines = new();

        public void Initialize(Estate estate)
        {
            renovationController.Initialize(estate);
        }
        
        private void Start()
        {
            UpdateInfo();
            renovationController.OnHousesUpdate+=UpdateInfo;
        }

        private void OnDestroy()
        {
            renovationController.OnHousesUpdate-=UpdateInfo;
        }

        private void UpdateInfo()
        {
            UpdateOffersInfo();
            UpdateRenovationInfo();
        }

        private void UpdateOffersInfo()
        {
            var index = 0;
            for (; index < renovationController.HouseOffers.Count; index++)
            {
                var house = renovationController.HouseOffers[index];

                OfferLineView line;
                if (index < _offerLines.Count)
                    line = _offerLines[index];
                else
                {
                    line = Instantiate(offerLineViewPrefab, offerLineContainer);
                    line.OnBuyButtonClick += BuyHouse;
                    _offerLines.Add(line);
                }

                line.Initialize(house);
                line.SetImage(
                    GetImage(house.HouseType)
                );
            }
            for (var i = _offerLines.Count - 1; i >= index; i--)
            {
                var line = _offerLines[i];
                Destroy(line.gameObject);
                _offerLines.RemoveAt(i);
            }
        }
        
        private void UpdateRenovationInfo()
        {
            var index = 0;
            for (; index < renovationController.PurchasedHouses.Count; index++)
            {
                var house = renovationController.PurchasedHouses[index];

                RenovationLineView line;
                if (index < _renovationLines.Count)
                    line = _renovationLines[index];
                else
                {
                    line = Instantiate(renovationLineViewPrefab, renovationLineContainer);
                    line.OnSellButtonClick += SellHouse;
                    line.OnRenovationButtonClick += OnRenovationButton;
            
                    _renovationLines.Add(line);
                }

                line.Initialize(house);
                line.SetImage(
                    GetImage(house.HouseType)
                );
            }
            for (var i = _renovationLines.Count - 1; i >= index; i--)
            {
                var line = _renovationLines[i];
                Destroy(line.gameObject);
                _renovationLines.RemoveAt(i);
            }

            counter.text = $"{_renovationLines.Count}/{renovationController.MaxPurchasedAmount}";
        }
        
        private Sprite GetImage(HouseType type)
        {
            if (type == HouseType.Renovating)
                type = HouseType.Broken;
            return library.GetSprite(Category, type.ToString());
        }

        private void BuyHouse(OfferLineView line)
        {
            if (!renovationController.TryBuyHouse(line.House))
            {
                Debug.Log("You don't have enough  money");
            }
        }
        
        private void SellHouse(RenovationLineView line)
        {
            if (!renovationController.TrySellHouse(line.House))
            {
                Debug.Log("Error");
            }
        }
        
        private void OnRenovationButton(RenovationLineView line)
        {
            if (!renovationController.TryRenovateHouse(line.House))
            {
                Debug.Log("You don't have enough  money");
            }
        }
    }
}