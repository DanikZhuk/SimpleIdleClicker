using System;
using Gameplay.Businesses.BusinessControllers.RepairShop.House.Model;

namespace Gameplay.Businesses.BusinessControllers.RepairShop.House.Controller
{
    public class AvailableHouseController: BaseHouseController
    {
        public AvailableHouseController(HouseModel houseModel) : base(houseModel) {}

        private bool _canBuy;
        public bool CanBuy
        {
            get=>_canBuy;
            set
            {
                _canBuy=value;
                OnCanBuyUpdate?.Invoke();
            }
        }

        public event Action OnCanBuyUpdate;
    }
}