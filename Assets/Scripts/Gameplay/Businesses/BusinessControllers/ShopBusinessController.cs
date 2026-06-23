using Configs;
using Gameplay.Businesses.Generic.Models;

namespace Gameplay.Businesses.BusinessControllers
{
    public class ShopBusinessController: BusinessController
    {
        public ShopBusinessController(BusinessConfig config, BusinessModel businessModel)
            : base(config, businessModel)
        { }

        public override void Update(float deltaTime)
        { }
    }
}