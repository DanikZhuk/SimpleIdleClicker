using System;

namespace Gameplay.Estates
{
    public class Estate
    {
        public string id;
        public string name;
        public EstateType type;
        
        private static long _nextId = 0;

        public Estate(string name, EstateType type)
        {
            this.id = (_nextId++).ToString();
            this.name = name;
            this.type = type;
        }
    }
}