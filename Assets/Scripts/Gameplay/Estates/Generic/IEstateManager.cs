using System;
using System.Collections.Generic;
using Configs;

namespace Gameplay.Estates.Generic
{
    public interface IEstateManager
    {
        public event Action OnEstatesChanged;
        public List<Estate> Estates { get; }
        public bool TryAddEstate(string name, EstateConfig config);
        public Estate GetEstate(string id);
    }
}