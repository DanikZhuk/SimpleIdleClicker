using System;
using System.Collections.Generic;
using System.Linq;
using Configs;
using Configs.BusinessConfigs;
using Gameplay.Businesses.BusinessControllers.RepairShop.House.Controller;
using Gameplay.Businesses.BusinessControllers.RepairShop.House.Model;
using Gameplay.Businesses.BusinessModels;
using Gameplay.Businesses.Generic;
using Gameplay.Businesses.Generic.Models;
using Random = UnityEngine.Random;

namespace Gameplay.Businesses.BusinessControllers.RepairShop
{
    public class RepairShopBusinessController : IBusinessController
    {
        private readonly RepairShopBusinessModel _businessModel;
        private readonly RepairShopBusinessConfig _businessConfig;


        public BusinessModel BusinessModel => _businessModel;
        public BusinessConfig BusinessConfig => _businessConfig;

        private readonly List<AvailableHouseController> _houseOffers = new();
        private readonly List<PurchasedHouseController> _purchasedHouses = new();
        private readonly List<PurchasedHouseController> _repairHouses = new();
        private BusinessManager _businessManager;

        public IReadOnlyList<AvailableHouseController> HouseOffers => _houseOffers;
        public IReadOnlyList<PurchasedHouseController> PurchasedHouses => _purchasedHouses;

        public int MaxPurchasedHousesAmount => _businessConfig.MaxPurchasedHousesAmount;

        public event Action OnHousesUpdate;

        public RepairShopBusinessController(BusinessConfig businessConfig, string businessModelName)
        {
            if (businessConfig is RepairShopBusinessConfig repairShopConfig)
                _businessConfig = repairShopConfig;
            else
                throw new ArgumentException("businessConfig is not a repair shop config");

            _businessModel = new RepairShopBusinessModel(businessConfig, businessModelName);
        }

        public RepairShopBusinessController(BusinessConfig businessConfig, BusinessModel businessModel)
        {
            if (businessConfig is RepairShopBusinessConfig repairShopConfig)
                _businessConfig = repairShopConfig;
            else
                throw new ArgumentException("businessConfig is not a repair shop config");

            if (businessModel is RepairShopBusinessModel repairShopModel)
                _businessModel = repairShopModel;
            else
            {
                throw new ArgumentException("businessModel is not a repair shop model");
            }
        }

        public void Setup(BusinessManager businessManager)
        {
            _businessManager = businessManager;
            Setup();
        }

        private void Setup()
        {
            foreach (var houseController in
                     _businessModel.HouseOffers.Select(houseModel =>
                         new AvailableHouseController(houseModel)))
            {
                _houseOffers.Add(houseController);
            }

            foreach (var houseController in
                     _businessModel.PurchasedHouses.Select(houseModel =>
                         new PurchasedHouseController(houseModel)))
            {
                _purchasedHouses.Add(houseController);
            }

            while (_businessModel.HouseOffers.Count < _businessConfig.HouseOffersAmount)
            {
                var house = GenerateHouseModel();
                _businessModel.HouseOffers.Add(house);
                _houseOffers.Add(new AvailableHouseController(house));
            }

            OnMoneyChanged(_businessManager.MoneyService.Money);
        }

        public void OnMoneyChanged(long money)
        {
            foreach (var houseController in _houseOffers)
            {
                if (_purchasedHouses.Count >= _businessConfig.MaxPurchasedHousesAmount
                    || money < houseController.HouseModel.Cost)
                {
                    houseController.CanBuy = false;
                    continue;
                }

                houseController.CanBuy = true;
            }

            foreach (var houseController in _purchasedHouses)
            {
                if (houseController.HouseModel.Condition!=HouseCondition.NeedRepair||money < houseController.HouseModel.RepairCost)
                {
                    houseController.CanRepair = false;
                    continue;
                }

                houseController.CanRepair = true;
            }
        }

        public void OnRemove()
        {
        }

        public void OnBuy()
        {
        }

        public void OnSell()
        {
        }

        public void Update(float deltaTime)
        {
            foreach (var houseController in _repairHouses.ToArray())
            {
                if (houseController.FinishRepair(deltaTime))
                {
                    houseController.HouseModel.Cost =
                        (long)(houseController.HouseModel.Cost * _businessConfig.AfterRepairCostCoeff);
                    OnHousesUpdate?.Invoke();
                    _repairHouses.Remove(houseController);
                }
            }
        }

        public void BuyHouse(AvailableHouseController houseController)
        {
            var purchasedHouseController = new PurchasedHouseController(houseController);
            _businessModel.PurchasedHouses.Add(purchasedHouseController.HouseModel);
            _purchasedHouses.Add(purchasedHouseController);

            _businessModel.HouseOffers.Remove(houseController.HouseModel);
            _houseOffers.Remove(houseController);

            var houseModel = GenerateHouseModel();
            _businessModel.HouseOffers.Add(houseModel);
            var availableHouseController = new AvailableHouseController(houseModel);
            _houseOffers.Add(availableHouseController);
            
            _businessManager.MoneyService.Money -= houseController.HouseModel.Cost;
            OnHousesUpdate?.Invoke();
        }

        public void RepairHouse(PurchasedHouseController houseController)
        {
            var repairTime = Random.Range(_businessConfig.MinRepairTime, _businessConfig.MaxRepairTime);

            houseController.Repair(repairTime);
            _repairHouses.Add(houseController);
            
            
            _businessManager.MoneyService.Money -= houseController.HouseModel.RepairCost;
            OnHousesUpdate?.Invoke();
            _businessManager.SystemMessageManager.Log($"Renovation has started");
        }

        public void SellHouse(PurchasedHouseController houseController)
        {
            _businessModel.PurchasedHouses.Remove(houseController.HouseModel);
            _purchasedHouses.Remove(houseController);

            
            _businessManager.MoneyService.Money += houseController.HouseModel.Cost;
            OnHousesUpdate?.Invoke();
            _businessManager.SystemMessageManager.Log($"You sold the house");
        }

        private HouseModel GenerateHouseModel()
        {
            var cost = (long)Random.Range(_businessConfig.MinHouseCost, _businessConfig.MaxHouseCost);
            var repairCost = (long)Random.Range(_businessConfig.MinRepairCost, _businessConfig.MaxRepairCost);
            return new HouseModel(cost, repairCost);
        }
    }
}