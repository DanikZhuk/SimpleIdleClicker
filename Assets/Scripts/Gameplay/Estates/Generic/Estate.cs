namespace Gameplay.Estates.Generic
{
    public class Estate
    {
        public string Id;
        public string Name;
        public EstateType Type;
        public float SellPrice;
        
        private static long _nextId = 0;

        public Estate(string name, EstateType type, float sellPrice)
        {
            Id = (_nextId++).ToString();
            Name = name;
            Type = type;
            SellPrice = sellPrice;
        }
    }
}