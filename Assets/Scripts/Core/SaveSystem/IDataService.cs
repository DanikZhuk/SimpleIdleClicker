using System.Collections.Generic;
using Gameplay.Estates.Generic;

namespace Core.SaveSystem
{
    public interface IDataService
    {
        public List<Estate> Estates { get; }
        public void AddEstate(Estate estate);
        public void RemoveEstate(Estate estate);
        
        public float Money {get; set;}
    }
}