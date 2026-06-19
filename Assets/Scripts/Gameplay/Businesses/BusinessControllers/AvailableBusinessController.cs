using System;
using Configs;
using Gameplay.Businesses.Generic;
using Gameplay.Businesses.Generic.Models;

namespace Gameplay.Businesses.BusinessControllers
{
    public class AvailableBusinessController
    {
        public BusinessModel BusinessModel { get; }
        public BusinessConfig BusinessConfig { get; }
        public string UserBusinessName { get; set; }

        private bool _canBuy;

        public bool CanBuy
        {
            get => _canBuy;
            set
            {
                _canBuy = value;
                OnBuyStatusUpdated?.Invoke(value);
            }
        }

        public event Action<bool> OnBuyStatusUpdated;

        public AvailableBusinessController(BusinessConfig config)
        {
            BusinessModel = new BusinessModel(config, config.BusinessName);
            BusinessConfig = config;
        }
    }
}