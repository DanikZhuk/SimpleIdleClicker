using System.Collections.Generic;
using Gameplay.Estates.Generic;
using Gameplay.Investitions;

namespace Core.SaveSystem
{
    public interface IDataService
    {
        public List<Estate> Estates { get; }
        public List<Investition> Investitions { get; }
        public void AddEstate(Estate estate);
        public void RemoveEstate(Estate estate);
        
        public long Money {get; set;}
    }
}