using System;
using System.Collections.Generic;
using System.Linq;
using Configs;
using Core.SaveSystem;
using Gameplay.Businesses.BusinessControllers;
using Gameplay.Businesses.BusinessControllers.RepairShop;
using Gameplay.Businesses.Enums;
using Gameplay.Businesses.Generic;
using Gameplay.Businesses.Generic.Models;
using Gameplay.Services;
using UI.Helpers.SystemMessages;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

namespace Gameplay.Businesses
{
    public class BusinessManager : MonoBehaviour
    {
        [SerializeField] private BusinessListConfig businessListConfig;

        [Inject] private MoneyService _moneyService;
        [Inject] private SaveDataService _saveDataService;
        [Inject] private SystemMessageManager _systemMessageManager;
        [Inject] private TimeService _timeService;

        private readonly List<AvailableBusinessController> _availableBusinessControllers = new();
        private readonly List<IBusinessController> _purchasedBusinessControllers = new();
        private readonly Dictionary<BusinessType, int> _typeCounts = new();

        public event Action OnBusinessesChanged;
        public IReadOnlyList<AvailableBusinessController> AvailableBusinessControllers => _availableBusinessControllers;
        public IReadOnlyList<IBusinessController> PurchasedBusinessControllers => _purchasedBusinessControllers;

        public bool AddBusiness(AvailableBusinessController availableBusinessController)
        {
            CreateBusinessController(availableBusinessController);

            _moneyService.Money -= availableBusinessController.BusinessConfig.Price;
            _systemMessageManager.Log($"You bought {availableBusinessController.BusinessModel.Name}" +
                                      $" with name {availableBusinessController.UserBusinessName}");
            return true;
        }

        public void SellBusiness(IBusinessController businessController)
        {
            RemoveBusinessController(businessController);

            _moneyService.Money += businessController.BusinessConfig.Price;
            _systemMessageManager.Log($"You sold {businessController.BusinessModel.Name}");
        }

        public int GetTypeCount(BusinessType businessType)
        {
            return _typeCounts.GetValueOrDefault(businessType);
        }

        private void Awake()
        {
            Initialize();
        }

        private void Initialize()
        {
            foreach (var config in businessListConfig.Businesses)
            {
                var businessController = new AvailableBusinessController(config);
                _availableBusinessControllers.Add(businessController);
            }

            var configDict = businessListConfig.Businesses.ToDictionary(config => config.Type);

            foreach (var businessModel in _saveDataService.BusinessModels)
            {
                CreateBusinessController(configDict[businessModel.BusinessType], businessModel);
            }

            configDict.Clear();

            OnMoneyChanged();
            _moneyService.OnMoneyChanged += OnMoneyChanged;
            
            CalculateOfflineImpact();
            _timeService.OnOfflineTime += CalculateOfflineImpact;
        }

        private void CalculateOfflineImpact()
        {
            foreach (var businessController in _purchasedBusinessControllers)
            {
                businessController.CalculateOfflineImpact(_timeService.OfflineTime);
            }
        }

    private void Update()
        {
            foreach (var purchasedBusinessController in _purchasedBusinessControllers)
            {
                purchasedBusinessController.Update(Time.deltaTime);
            }
        }

        private void OnMoneyChanged()
        {
            foreach (var businessController in _availableBusinessControllers)
            {
                var config = businessController.BusinessConfig;
                var count = _typeCounts[config.Type];
                if (count >= config.MaxCount
                    || _moneyService.Money < config.Price)
                {
                    businessController.CanBuy = false;
                    continue;
                }

                businessController.CanBuy = true;
            }

            OnBusinessesChanged?.Invoke();
        }

        private void CreateBusinessController(AvailableBusinessController availableBusinessController)
        {
            IBusinessController businessController = availableBusinessController.BusinessConfig.Type switch
            {
                BusinessType.Shop => new ShopBusinessController(availableBusinessController.BusinessConfig,
                    availableBusinessController.UserBusinessName),
                BusinessType.RepairShop => new RepairShopBusinessController(availableBusinessController.BusinessConfig,
                    availableBusinessController.UserBusinessName),
                _ => throw new ArgumentOutOfRangeException(
                    $"The type {availableBusinessController.BusinessConfig.Type} is not defined")
            };
            _saveDataService.AddBusiness(businessController.BusinessModel);
            businessController.Setup(_moneyService, _systemMessageManager);
            _purchasedBusinessControllers.Add(businessController);
            UpdateBusinessTypeCounts();
        }
        
        private void CreateBusinessController(BusinessConfig businessConfig, BusinessModel businessModel)
        {
            IBusinessController businessController = businessConfig.Type switch
            {
                BusinessType.Shop => new ShopBusinessController(businessConfig, businessModel),
                BusinessType.RepairShop => new RepairShopBusinessController(businessConfig, businessModel),
                _ => throw new ArgumentOutOfRangeException(
                    $"The type {businessConfig.Type} is not defined")
            };
            businessController.Setup(_moneyService, _systemMessageManager);
            _purchasedBusinessControllers.Add(businessController);
            UpdateBusinessTypeCounts();
        }

        private void RemoveBusinessController(IBusinessController businessController)
        {
            _saveDataService.RemoveBusiness(businessController.BusinessModel);
            _purchasedBusinessControllers.Remove(businessController);
            businessController.OnRemove();
            UpdateBusinessTypeCounts();
        }

        private void UpdateBusinessTypeCounts()
        {
            foreach (BusinessType type in Enum.GetValues(typeof(BusinessType)))
            {
                _typeCounts[type] = _purchasedBusinessControllers.Count(businessController =>
                    businessController.BusinessConfig.Type == type);
            }
        }
    }
}