using System;
using Gameplay.Businesses.BusinessControllers.RepairShop.House.Model;

namespace Gameplay.Businesses.BusinessControllers.RepairShop.House.Controller
{
    public class PurchasedHouseController : BaseHouseController
    {
        public bool CanRepair
        {
            get => _canRepair;
            set
            {
                _canRepair = value;
                OnStatusChanged?.Invoke();
            }
        }

        public bool CanSell
        {
            get => _canSell;
            set
            {
                _canSell = value;
                OnStatusChanged?.Invoke();
            }
        }

        public event Action OnStatusChanged;

        private bool _canRepair;
        private bool _canSell = true;

        public PurchasedHouseController(HouseModel houseModel) : base(houseModel)
        {
            if (houseModel.Condition != HouseCondition.UnderRepair) return;
            CanRepair = false;
            CanSell = false;
        }

        public PurchasedHouseController(BaseHouseController houseController) : base(houseController)
        {
        }

        public void Repair(float repairTime)
        {
            if (!CanRepair) return;
            houseModel.Condition = HouseCondition.UnderRepair;
            CanSell = false;
            houseModel.RepairTime = repairTime;
        }

        public bool FinishRepair(float deltaTime)
        {
            if (houseModel.Condition != HouseCondition.UnderRepair)
            {
                if (CanSell)
                    CanSell = true;
                return true;
            }
            houseModel.RepairTime -= deltaTime;
            if (houseModel.RepairTime <= 0)
            {
                houseModel.Condition = HouseCondition.FullyRepaired;
                CanSell = true;
                return true;
            }

            if (CanSell)
                CanSell = false;
            return false;
        }
    }
}