using System;
using Configs;
using Gameplay.Businesses.BusinessModels;
using Gameplay.Businesses.Enums;
using Gameplay.Services;
using UI.Helpers.SystemMessages;

namespace Gameplay.Businesses.BusinessControllers
{
    public abstract class BusinessController
    {
        protected BusinessModel businessModel;
        protected BusinessConfig businessConfig;
        
        protected MoneyService  moneyService;
        protected SystemMessageManager systemMessageManager;
        
        public BusinessModel BusinessModel => businessModel;
        public BusinessType Type => businessConfig.Type;

        protected BusinessController(BusinessConfig businessConfig, BusinessModel businessModel)
        {
            this.businessModel = businessModel;
            this.businessConfig = businessConfig;
        }

        public virtual void Setup(MoneyService moneyService, SystemMessageManager systemMessageManager)
        {
            this.moneyService=moneyService;
            this.systemMessageManager=systemMessageManager;
        }
        public abstract void Update(float deltaTime);

        public virtual long GetSellPrice()
        {
            return (long)(businessConfig.Price * businessConfig.SellPercentage);
        }

        public virtual long GetIncome(DateTime startTime,
                                      DateTime endTime)
        {

            return businessConfig.Income;
        }
    }
}