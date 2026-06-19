using Configs;
using Gameplay.Businesses.Generic;
using Gameplay.Businesses.Generic.Models;

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

        public void Setup(BusinessManager businessManager)
        { }

        public void OnMoneyChanged(long money)
        { }
        
        public void OnRemove()
        { }

        public void Update(float deltaTime)
        { }
    }
}