using System;
using System.Collections.Generic;
using Configs;
using Configs.BusinessConfigs;
using Gameplay.Businesses.BusinessModels;
using Gameplay.Businesses.Generic.Models;
using Gameplay.Services;
using UI.Helpers.SystemMessages;
using Random = UnityEngine.Random;

namespace Gameplay.Businesses.BusinessControllers
{
    public class RepairShopBusinessController : BusinessController
    {
        private readonly List<HouseController> _houseOffers = new();
        private readonly List<HouseController> _purchasedHouses = new();
        private readonly List<HouseController> _repairHouses = new();

        private RepairShopBusinessModel RepairShopBusinessModel => (RepairShopBusinessModel)businessModel;
        private RepairShopBusinessConfig RepairShopBusinessConfig => (RepairShopBusinessConfig)businessConfig;

        public IReadOnlyList<HouseController> HouseOffers => _houseOffers;
        public IReadOnlyList<HouseController> PurchasedHouses => _purchasedHouses;
        public IReadOnlyList<HouseController> RepairHouses => _repairHouses;

        public int MaxPurchasedHousesAmount => RepairShopBusinessConfig.MaxPurchasedHousesAmount;

        public event Action OnHousesUpdate;

        public RepairShopBusinessController(BusinessConfig businessConfig, BusinessModel businessModel)
            : base(businessConfig, businessModel)
        { }

        public override void Setup(MoneyService moneyService, SystemMessageManager systemMessageManager)
        {
            base.Setup(moneyService, systemMessageManager);

            foreach (var houseModel in RepairShopBusinessModel.HouseOffers)
            {
                _houseOffers.Add(new HouseController(houseModel));
            }

            foreach (var houseModel in RepairShopBusinessModel.PurchasedHouses)
            {
                var controller = new HouseController(houseModel);
                _purchasedHouses.Add(controller);

                if (controller.IsUnderRepair)
                    _repairHouses.Add(controller);
            }

            while (_houseOffers.Count < RepairShopBusinessConfig.HouseOffersAmount)
            {
                var model = GenerateHouseModel();
                RepairShopBusinessModel.HouseOffers.Add(model);
                _houseOffers.Add(new HouseController(model));
            }
        }

        public override void Update(float deltaTime)
        {
            UpdateRepairs(deltaTime);
        }

        public void BuyHouse(HouseController houseController)
        {
            RepairShopBusinessModel.PurchasedHouses.Add(houseController.Model);
            _purchasedHouses.Add(houseController);

            RepairShopBusinessModel.HouseOffers.Remove(houseController.Model);
            _houseOffers.Remove(houseController);

            var newModel = GenerateHouseModel();
            RepairShopBusinessModel.HouseOffers.Add(newModel);
            _houseOffers.Add(new HouseController(newModel));

            moneyService.Money -= houseController.Cost;
            OnHousesUpdate?.Invoke();
        }

        public void StartRepairHouse(HouseController houseController)
        {
            houseController.StartRepair(RepairShopBusinessConfig);
            _repairHouses.Add(houseController);

            moneyService.Money -= houseController.RepairCost;
            OnHousesUpdate?.Invoke();
            systemMessageManager.Log("Renovation has started");
        }

        public void SellHouse(HouseController houseController)
        {
            RepairShopBusinessModel.PurchasedHouses.Remove(houseController.Model);
            _purchasedHouses.Remove(houseController);

            moneyService.Money += houseController.SellPrice;
            OnHousesUpdate?.Invoke();
            systemMessageManager.Log("You sold the house");
        }

        private void UpdateRepairs(float deltaTime)
        {
            for (var i = _repairHouses.Count - 1; i >= 0; i--)
            {
                var houseController = _repairHouses[i];
                if (!houseController.UpdateRepair(deltaTime)) continue;
                houseController.CompleteRepair(RepairShopBusinessConfig.AfterRepairCostCoeff);
                _repairHouses.RemoveAt(i);
                OnHousesUpdate?.Invoke();
            }
        }

        private HouseModel GenerateHouseModel()
        {
            var cost = (long)Random.Range(RepairShopBusinessConfig.MinHouseCost,
                RepairShopBusinessConfig.MaxHouseCost);
            var repairCost = (long)Random.Range(RepairShopBusinessConfig.MinRepairCost,
                RepairShopBusinessConfig.MaxRepairCost);
            return new HouseModel(cost, repairCost);
        }
    }
    
    public class HouseController
    {
        public HouseModel Model { get; }

        public event Action OnConditionChanged;
        public string Id => Model.Id;
        public long Cost => Model.Cost;
        public long RepairCost => Model.RepairCost;
        public HouseCondition Condition
        {
            get => Model.Condition;
            private set
            {
                Model.Condition=value;
                OnConditionChanged?.Invoke();
            }
        }

        public bool IsUnderRepair => Model.Condition == HouseCondition.UnderRepair;
        public bool IsNeedRepair => Model.Condition == HouseCondition.NeedRepair;

        public long SellPrice => Model.Cost;
        public float RepairProgress
        {
            get => Model.RemainingRepairTime;
            private set => Model.RemainingRepairTime = value;
        }

        public bool CanBeRepaired => Model.Condition == HouseCondition.NeedRepair;
        public bool CanBeSold => Model.Condition != HouseCondition.UnderRepair;

        public HouseController(HouseModel model)
        {
            Model = model;
        }

        public void StartRepair(RepairShopBusinessConfig config)
        {
            if (Condition != HouseCondition.NeedRepair)
                return;

            var repairTime = Random.Range(config.MinRepairTime, config.MaxRepairTime);
            RepairProgress = repairTime;
            Condition = HouseCondition.UnderRepair;
        }

        public bool UpdateRepair(float deltaTime)
        {
            if (!IsUnderRepair)
                return false;

            RepairProgress -= deltaTime;

            if (RepairProgress > 0)
                return false;

            RepairProgress = 0;
            return true;
        }

        public void CompleteRepair(float costCoefficient)
        {
            Condition = HouseCondition.FullyRepaired;
            Model.Cost = (long)(Model.Cost * costCoefficient);
        }
    }
}