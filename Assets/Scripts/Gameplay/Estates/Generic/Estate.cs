using Configs;

namespace Gameplay.Estates.Generic
{
    public class Estate
    {
        public string id;
        public string name;
        public EstateConfig Config;
        public float sellPrice;
        
        private static long _nextId = 0;

        public Estate(string name, EstateConfig config, float sellPrice)
        {
            this.id = (_nextId++).ToString();
            this.name = name;
            this.Config = config;
            this.sellPrice = sellPrice;
        }
    }
}