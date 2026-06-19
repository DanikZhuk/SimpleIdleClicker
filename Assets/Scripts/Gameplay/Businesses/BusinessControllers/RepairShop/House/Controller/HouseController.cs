using Gameplay.Businesses.BusinessControllers.RepairShop.House.Model;

namespace Gameplay.Businesses.BusinessControllers.RepairShop.House.Controller
{
    public class HouseController
    {
        protected readonly HouseModel houseModel;
        public HouseModel HouseModel=>houseModel;

        public HouseController(HouseModel houseModel)
        {
            this.houseModel = houseModel;
        }
        
        public HouseController(HouseController other)
        {
            houseModel = other.houseModel;
        }
    }
}