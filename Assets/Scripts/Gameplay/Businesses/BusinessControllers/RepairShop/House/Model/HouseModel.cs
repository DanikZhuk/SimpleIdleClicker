namespace Gameplay.Businesses.BusinessControllers.RepairShop.House.Model
{
    public enum HouseCondition
    {
        NeedRepair,
        UnderRepair,
        FullyRepaired
    }
    public class HouseModel
    {
        public string Id;
        public HouseCondition Condition;
        public long Cost;
        public long RepairCost;
        public float RepairTime;
        private static long _nextId = 0;

        public HouseModel()
        { }

        public HouseModel(long cost, long repairCost)
        {
            Id=_nextId++.ToString();
            Condition = HouseCondition.NeedRepair;
            Cost = cost;
            RepairCost = repairCost;
        }
    }
}