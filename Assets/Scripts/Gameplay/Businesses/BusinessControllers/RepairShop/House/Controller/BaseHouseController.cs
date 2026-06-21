using Gameplay.Businesses.BusinessControllers.RepairShop.House.Model;

namespace Gameplay.Businesses.BusinessControllers.RepairShop.House.Controller
{
    public abstract class BaseHouseController
    {
        protected readonly HouseModel houseModel;
        public HouseModel HouseModel=>houseModel;

        protected BaseHouseController(HouseModel houseModel)
        {
            this.houseModel = houseModel;
        }

        protected BaseHouseController(BaseHouseController other)
        {
            houseModel = other.houseModel;
        }
    }
}