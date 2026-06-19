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
        [FormerlySerializedAs("estateListConfig")] [SerializeField]
        private BusinessListConfig businessListConfig;

        [Inject] private MoneyService _moneyService;
        [Inject] private OfflinePaymentService _offlinePaymentService;
        [Inject] private SaveDataService _saveDataService;
        [Inject] private SystemMessageManager _systemMessageManager;
        
        public MoneyService MoneyService => _moneyService;
        public SystemMessageManager SystemMessageManager => _systemMessageManager;
        
        public event Action OnBusinessesChanged;

        private readonly List<AvailableBusinessController> _availableBusinessControllers = new();
        private readonly List<IBusinessController> _purchasedBusinessControllers = new();

        private Dictionary<BusinessType, int> _typeCounts = new();

        public IReadOnlyList<AvailableBusinessController> AvailableBusinessControllers => _availableBusinessControllers;
        public IReadOnlyList<IBusinessController> PurchasedBusinessControllers => _purchasedBusinessControllers;

        public bool AddBusiness(AvailableBusinessController availableBusinessController)
        {
            var businessController = CreateBusinessController(availableBusinessController);
            _purchasedBusinessControllers.Add(businessController);
            UpdateBusinessTypeCounts();
            _moneyService.Money -= availableBusinessController.BusinessConfig.Price;
            _systemMessageManager.Log($"You bought {availableBusinessController.BusinessModel.Name}" +
                                      $" with name {availableBusinessController.UserBusinessName}");
            return true;
        }

        public void SellBusiness(IBusinessController businessController)
        {
            _saveDataService.RemoveBusiness(businessController.BusinessModel);
            _purchasedBusinessControllers.Remove(businessController);
            UpdateBusinessTypeCounts();
            businessController.OnRemove();
            _moneyService.Money += businessController.BusinessConfig.Price;
            _systemMessageManager.Log($"You sold {businessController.BusinessModel.Name}");
        }

        private void Start()
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
            
            foreach (var businessController in 
                     _saveDataService.BusinessModels.Select(
                         businessModel => CreateBusinessController(
                             configDict[businessModel.BusinessType],businessModel)))
            {
                _purchasedBusinessControllers.Add(businessController);
            }
            
            configDict.Clear();

            UpdateBusinessTypeCounts();
            OnMoneyChanged();
            _moneyService.OnMoneyChanged += OnMoneyChanged;
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

            foreach (var businessController in _purchasedBusinessControllers)
            {
                businessController.OnMoneyChanged(_moneyService.Money);
            }

            OnBusinessesChanged?.Invoke();
        }

        private IBusinessController CreateBusinessController(AvailableBusinessController availableBusinessController)
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
            businessController.Setup(this);
            return businessController;
        }
        
        private IBusinessController CreateBusinessController(BusinessConfig businessConfig, BusinessModel businessModel)
        {
            IBusinessController businessController = businessConfig.Type switch
            {
                BusinessType.Shop => new ShopBusinessController(businessConfig, businessModel),
                BusinessType.RepairShop => new RepairShopBusinessController(businessConfig, businessModel),
                _ => throw new ArgumentOutOfRangeException(
                    $"The type {businessConfig.Type} is not defined")
            };
            businessController.Setup(this);
            return businessController;
        }

        public void UpdateBusinessTypeCounts()
        {
            foreach (BusinessType type in Enum.GetValues(typeof(BusinessType)))
            {
                _typeCounts[type] = _purchasedBusinessControllers.Count(businessController =>
                    businessController.BusinessConfig.Type == type);
            }
        }

        public int GetTypeCount(BusinessType businessType)
        {
            return _typeCounts.GetValueOrDefault(businessType);
        }
    }
}