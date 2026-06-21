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

        public void Repair(float repairSeconds)
        {
            if (!CanRepair) return;
            houseModel.Condition = HouseCondition.UnderRepair;
            CanSell = false;
            houseModel.RepairTimeSeconds = repairSeconds;
        }

        public bool FinishRepair(float seconds)
        {
            if (houseModel.Condition != HouseCondition.UnderRepair)
            {
                if (CanSell)
                    CanSell = true;
                return true;
            }
            houseModel.RepairTimeSeconds -= seconds;
            if (houseModel.RepairTimeSeconds <= 0)
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