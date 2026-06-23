using System;
using System.Collections.Generic;
using System.Linq;
using Configs;
using Core.SaveSystem;
using Gameplay.Businesses.BusinessControllers;
using Gameplay.Businesses.BusinessModels;
using Gameplay.Businesses.Enums;
using Gameplay.Services;
using UI.Helpers.SystemMessages;
using UnityEngine;
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

        public event Action OnBusinessesChanged;
        
        private readonly List<BusinessController> _purchasedBusinessControllers = new();
        private Dictionary<BusinessType, BusinessConfig> _configs = new();
        public IReadOnlyList<BusinessController> PurchasedBusinessControllers => _purchasedBusinessControllers;

        public void AddBusiness(BusinessType businessType, string userBusinessName)
        {
            var config = _configs[businessType];
            
            var businessModel = CreateBusinessModel(config, userBusinessName);
            _saveDataService.AddBusiness(businessModel);
            
            var businessController = CreateBusinessController(config, businessModel);
            _purchasedBusinessControllers.Add(businessController);

            _moneyService.Money -= config.Price;
            OnBusinessesChanged?.Invoke();
            _systemMessageManager.Log($"You bought {config.BusinessName}" +
                                      $" with name {userBusinessName}");
        }

        public void SellBusiness(BusinessController businessController)
        {
            RemoveBusinessController(businessController);

            _moneyService.Money += businessController.GetSellPrice();
            OnBusinessesChanged?.Invoke();
            _systemMessageManager.Log($"You sold {businessController.BusinessModel.Name}");
        }

        public int GetTypeCount(BusinessType businessType)
        {
            return _purchasedBusinessControllers
                .Count(businessController => 
                    businessController.BusinessModel.BusinessType == businessType);
        }

        private void Awake()
        {
            Initialize();
        }

        private void Initialize()
        {
            _configs = businessListConfig.Businesses.ToDictionary(config => config.Type);

            foreach (var businessModel in _saveDataService.BusinessModels)
            {
                var businessController =
                    CreateBusinessController(_configs[businessModel.BusinessType], businessModel);
                _purchasedBusinessControllers.Add(businessController);
            }

            CalculateOfflineImpact();
        }

        private void CalculateOfflineImpact()
        {
            var offlineTime = _timeService.ElapsedTimeSince(_saveDataService.RecordTime);
            var offlineTimeSeconds = (float)offlineTime.TotalSeconds;
            
            foreach (var businessController in _purchasedBusinessControllers)
            {
                businessController.Update(offlineTimeSeconds);
            }
        }

        private void OnApplicationPause(bool isPaused)
        {
            if (!isPaused)
            {
                CalculateOfflineImpact();
            }
        }

        private void Update()
        {
            foreach (var purchasedBusinessController in _purchasedBusinessControllers)
            {
                purchasedBusinessController.Update(Time.deltaTime);
            }
        }

        private BusinessModel CreateBusinessModel(BusinessConfig businessConfig, string businessModelName)
        {
            var businessModel = businessConfig.Type switch
            {
                BusinessType.Shop => new BusinessModel(businessConfig, businessModelName),
                BusinessType.RepairShop => new RepairShopBusinessModel(businessConfig, businessModelName),
                _ => throw new ArgumentOutOfRangeException(
                    $"The type {businessConfig.Type} is not defined")
            };
            return businessModel;
        }

        private BusinessController CreateBusinessController(BusinessConfig businessConfig, BusinessModel businessModel)
        {
            BusinessController businessController = businessConfig.Type switch
            {
                BusinessType.Shop => new ShopBusinessController(businessConfig, businessModel),
                BusinessType.RepairShop => new RepairShopBusinessController(businessConfig, businessModel),
                _ => throw new ArgumentOutOfRangeException(
                    $"The type {businessConfig.Type} is not defined")
            };
            businessController.Setup(_moneyService, _systemMessageManager);
            return businessController;
        }

        private void RemoveBusinessController(BusinessController businessController)
        {
            _saveDataService.RemoveBusiness(businessController.BusinessModel);
            _purchasedBusinessControllers.Remove(businessController);
        }
    }
}