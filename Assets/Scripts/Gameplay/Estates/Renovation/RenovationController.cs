using System;
using System.Collections.Generic;
using System.Linq;
using Configs;
using Core.SaveSystem;
using Gameplay.Estates.Generic;
using Gameplay.Services;
using UI.EstatePage.EstateViews.Renovation;
using UI.EstatePage.EstateViews.Renovation.Model;
using UI.Helpers.SystemMessages;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace Gameplay.Estates.Renovation
{
    public class RenovationController : MonoBehaviour
    {
        [SerializeField] private HousesConfig config;

        [Inject] private IDataService _dataService;
        [Inject] private MoneyService _moneyService;
        [Inject] private TimeService _timeService;
        [Inject] private SystemMessageManager _smm;

        public IReadOnlyList<House> PurchasedHouses => _estate.PurchasedHouses;
        public IReadOnlyList<House> HouseOffers => _estate.HouseOffers;

        private RenovationEstate _estate;

        public int MaxPurchasedAmount => config.MaxPurchasedAmount;

        public event Action OnHousesUpdate;

        public void Initialize(Estate estate)
        {
            _estate = (RenovationEstate)estate;
            Initialize();
        }

        public bool TryBuyHouse(House house)
        {
            if (!_estate.HouseOffers.Contains(house))
            {
                _smm.Log("Offer is not found");
                return false;
            }

            if (_estate.PurchasedHouses.Count >= config.MaxPurchasedAmount)
            {
                _smm.Log("You can't buy any more houses");
                return false;
            }

            if (!_moneyService.TrySpend(house.Cost))
            {
                _smm.Log("You don't have enough money");
                return false;
            }

            _estate.PurchasedHouses.Add(house);
            _estate.HouseOffers.Remove(house);
            _estate.HouseOffers.Add(GenerateHouse());
            _smm.Log($"You bought a new house");
            OnHousesUpdate?.Invoke();
            return true;
        }

        public bool TryRenovateHouse(House house)
        {
            if (!_estate.PurchasedHouses.Contains(house))
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
            _smm.Log($"Renovation has started");
            return true;
        }

        public bool TrySellHouse(House house)
        {
            if (!_estate.PurchasedHouses.Contains(house))
            {
                _smm.Log("Error! House is not found");
                return false;
            }

            _moneyService.Earn(house.Cost);
            _estate.PurchasedHouses.Remove(house);
            OnHousesUpdate?.Invoke();
            _smm.Log($"You sold the house");
            return true;
        }

        private void Initialize()
        {
            _estate.HouseOffers ??= new List<House>();
            _estate.PurchasedHouses ??= new List<House>();

            while (_estate.HouseOffers.Count < config.OffersAmount)
            {
                _estate.HouseOffers.Add(GenerateHouse());
            }

            CheckTime();
            _timeService.OnTickElapsed += RenovationStep;
        }

        private void OnDestroy()
        {
            _timeService.OnTickElapsed -= RenovationStep;
            _estate.TimeData = _timeService.Now();
        }

        private void CheckTime()
        {
            var diff = (float)_timeService.ElapsedTimeSince(_estate.TimeData).TotalSeconds;
            foreach (var house in
                     _estate.PurchasedHouses.Where(house => house.HouseType == HouseType.Renovating))
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
                     _estate.PurchasedHouses.Where(house => house.HouseType == HouseType.Renovating)
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