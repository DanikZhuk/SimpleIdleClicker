using System;
using Configs;
using Gameplay.Businesses.BusinessModels;
using Gameplay.Businesses.Enums;
using Gameplay.Services;
using UI.Helpers.SystemMessages;
using Zenject;

namespace Gameplay.Businesses.BusinessControllers
{
    public abstract class BusinessController
    {
        protected BusinessConfig businessConfig;
        protected BusinessModel businessModel;

        [Inject] protected MoneyService moneyService;
        [Inject] protected SystemMessageManager systemMessageManager;
        
        public BusinessModel BusinessModel => businessModel;
        public long IncomePerHour => businessConfig.Income;
        public BusinessType Type => businessConfig.Type;

        protected BusinessController(BusinessConfig businessConfig, BusinessModel businessModel)
        {
            this.businessModel = businessModel;
            this.businessConfig = businessConfig;
        }

        public virtual void Setup()
        { }

        public abstract void Update(float deltaTime);

        public virtual long GetSellPrice()
        {
            return (long)(businessConfig.Price * businessConfig.SellPercentage);
        }

        public virtual long GetIncome(DateTime startTime, DateTime endTime, float incomeHourInSeconds)
        {
            var duration = endTime - startTime;
            var totalSeconds = (float)duration.TotalSeconds;

            var baseIncomePerSecond = businessConfig.Income / incomeHourInSeconds;

            return (long)(baseIncomePerSecond * totalSeconds);
        }
    }
}