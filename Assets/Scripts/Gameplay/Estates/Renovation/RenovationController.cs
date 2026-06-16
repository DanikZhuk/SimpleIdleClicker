using System;
using System.Collections.Generic;
using System.Linq;
using Configs;
using Core.SaveSystem;
using Gameplay.Estates.Generic;
using Gameplay.Services;
using UI.EstatePage.EstateViews.Renovation.Model;
using UI.Helpers.SystemMessages;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace UI.EstatePage.EstateViews.Renovation
{
    public class RenovationController : MonoBehaviour
    {
        [SerializeField] private HousesConfig config;

        [Inject] private IDataService _dataService;
        [Inject] private MoneyService _moneyService;
        [Inject] private TimeService _timeService;
        [Inject] private SystemMessageManager _smm;

        private const string KeyBase = "House";
        private const string PurchasedSubkey = "P";
        private const string OfferSubkey = "O";
        
        private string _key;

        private List<House> _purchasedHouses;
        private List<House> _houseOffers;

        public IReadOnlyList<House> PurchasedHouses => _purchasedHouses;
        public IReadOnlyList<House> HouseOffers => _houseOffers;
        
        public int MaxPurchasedAmount => config.MaxPurchasedAmount;

        public event Action OnHousesUpdate;

        public void Initialize(Estate estate)
        {
            _key = KeyBase + estate.Id;
            Initialize();
        }
        
        public bool TryBuyHouse(House house)
        {
            if (!_houseOffers.Contains(house))
            {
                _smm.Log("Offer is not found");
                return false;
            }
            if (_purchasedHouses.Count >= config.MaxPurchasedAmount)
            {
                _smm.Log("You can't buy any more houses");
                return false;
            }
            if (!_moneyService.TrySpend(house.Cost))
            {
                _smm.Log("You don't have enough money");
                return false;
            }
            _purchasedHouses.Add(house);
            _houseOffers.Remove(house);
            _houseOffers.Add(GenerateHouse());
            OnHousesUpdate?.Invoke();
            return true;
        }

        public bool TryRenovateHouse(House house)
        {
            if (!_purchasedHouses.Contains(house))
            {
                _smm.Log("Error! House is not found");
                return false;
            }
            if (!_moneyService.TrySpend(house.RenovationCost))
            {
                _smm.Log("You don't have enough money");
                return false;
            }
            house.HouseType = HouseType.Renovating;
            OnHousesUpdate?.Invoke();
            return true;
        }

        public bool TrySellHouse(House house)
        {
            if (!_purchasedHouses.Contains(house))
            {
                _smm.Log("Error! House is not found");
                return false;
            }
            _moneyService.Earn(house.Cost);
            _purchasedHouses.Remove(house);
            OnHousesUpdate?.Invoke();
            return true;
        }

        private void Initialize()
        {
            _purchasedHouses = _dataService.GetHousesList(_key+PurchasedSubkey);
            _houseOffers = _dataService.GetHousesList(_key+OfferSubkey);

            while (_houseOffers.Count < config.OffersAmount)
            {
                _houseOffers.Add(GenerateHouse());
            }

            CheckTime();
            _timeService.OnTickElapsed += RenovationStep;
        }

        private void OnDestroy()
        {
            _timeService.OnTickElapsed -= RenovationStep;
            _dataService.SetTimeData(_key, _timeService.Now());
        }

        private void CheckTime()
        {
            var diff = (float)_timeService.ElapsedTimeSince(_dataService.GetTimeData(_key)).TotalSeconds;
            foreach (var house in
                     _purchasedHouses.Where(house => house.HouseType == HouseType.Renovating))
            {
                house.RenovatingTime -= diff;
                if (house.RenovatingTime > 0) continue;
                house.HouseType = HouseType.Renovated;
                house.Cost = (int)(house.Cost * (1 + config.SellCoeff));
                OnHousesUpdate?.Invoke();
            }
        }

        private void RenovationStep()
        {
            foreach (var house in
                     _purchasedHouses.Where(house => house.HouseType == HouseType.Renovating)
                         .Where(house => --house.RenovatingTime <= 0))
            {
                house.HouseType = HouseType.Renovated;
                house.Cost = (int)(house.Cost * (1 + config.SellCoeff));
                OnHousesUpdate?.Invoke();
            }
        }

        private House GenerateHouse()
        {
            var house = new House()
            {
                HouseType = HouseType.Broken,
                Cost = Random.Range(config.MinCost, config.MaxCost),
                RenovationCost = Random.Range(config.MinRenovationCost, config.MaxRenovationCost),
            };
            house.RenovatingTime = Mathf.Lerp(config.MinRenovationTime, config.MaxRenovationTime,
                (house.Cost - config.MinCost) / (float)(config.MaxCost - config.MinCost));
            return house;
        }
    }
}