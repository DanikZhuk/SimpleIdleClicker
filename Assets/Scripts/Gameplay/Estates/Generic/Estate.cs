namespace Gameplay.Estates.Generic
{
    public class Estate
    {
        public string Id;
        public string Name;
        public EstateType Type;
        public long SellPrice;
        
        private static long _nextId = 0;

        public Estate(string name, EstateType type, long sellPrice)
        {
            Id = (_nextId++).ToString();
            Name = name;
            Type = type;
            SellPrice = sellPrice;
        }
    }
}