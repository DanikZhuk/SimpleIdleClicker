using Gameplay.Businesses.BusinessControllers.RepairShop.House.Model;

namespace Gameplay.Businesses.BusinessControllers.RepairShop.House.Controller
{
    public class BaseHouseController
    {
        protected readonly HouseModel houseModel;
        public HouseModel HouseModel=>houseModel;

        public BaseHouseController(HouseModel houseModel)
        {
            this.houseModel = houseModel;
        }
        
        public BaseHouseController(BaseHouseController other)
        {
            houseModel = other.houseModel;
        }
    }
}