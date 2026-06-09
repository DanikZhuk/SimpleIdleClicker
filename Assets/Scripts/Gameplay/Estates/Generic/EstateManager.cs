using System.Collections.Generic;
using JetBrains.Annotations;

namespace Gameplay.Estates
{
    public class EstateManager
    {
        private Dictionary<string, Estate> _estates;
        
        public EstateManager()
        {
            _estates = new Dictionary<string, Estate>();
        }

        public void AddEstate(Estate estate)
        {
            _estates.Add(estate.id, estate);
        }

        [CanBeNull]
        public Estate GetEstate(string id)
        {
            return _estates.GetValueOrDefault(id);
        }
    }
}