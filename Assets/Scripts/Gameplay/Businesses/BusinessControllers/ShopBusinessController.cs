using System;
using Configs;
using Gameplay.Businesses.Generic;
using Gameplay.Businesses.Generic.Models;
using Gameplay.Services;
using UI.Helpers.SystemMessages;
using UnityEngine;

namespace Gameplay.Businesses.BusinessControllers
{
    public class ShopBusinessController: IBusinessController
    {
        public BusinessModel BusinessModel { get; }
        public BusinessConfig BusinessConfig { get; }
        
        public ShopBusinessController(BusinessConfig config, string businessName)
        {
            BusinessModel = new BusinessModel(config, businessName);
            BusinessConfig = config;
        }
        
        public ShopBusinessController(BusinessConfig config, BusinessModel businessModel)
        {
            BusinessModel = businessModel;
            BusinessConfig = config;
        }

        public void Setup(MoneyService moneyService, SystemMessageManager systemMessageManager)
        { }

        public void CalculateOfflineImpact(TimeSpan offlineTime)
        { }

        public void OnRemove()
        { }

        public void Update(float deltaTime)
        { }
    }
}