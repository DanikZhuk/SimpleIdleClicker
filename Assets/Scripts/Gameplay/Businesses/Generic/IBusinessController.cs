using System;
using Configs;
using Gameplay.Businesses.Generic.Models;
using Gameplay.Services;
using UI.Helpers.SystemMessages;

namespace Gameplay.Businesses.Generic
{
    public interface IBusinessController
    {
        public BusinessModel BusinessModel { get; }
        public BusinessConfig BusinessConfig { get; }

        public void Setup(MoneyService moneyService, SystemMessageManager systemMessageManager);
        public void CalculateOfflineImpact(TimeSpan offlineTime);
        public void OnRemove();
        public void Update(float deltaTime);
    }
}