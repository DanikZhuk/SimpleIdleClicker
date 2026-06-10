using System;
using System.Collections.Generic;
using System.Linq;
using Configs;
using Gameplay.Services.MoneyService;
using JetBrains.Annotations;
using Reflex.Attributes;

namespace Gameplay.Estates.Generic
{
    public class EstateManager:  IEstateManager
    {
        public event Action OnEstatesChanged;
        
        private readonly Dictionary<string, Estate> _estates = new();
        [Inject] private IMoneyService _moneyService;

        public List<Estate> Estates => _estates.Values.ToList();

        public bool TryAddEstate(string name,EstateConfig config)
        {
            if(_estates.Count(estate1 => estate1.Value.Config.Type == config.Type)
               >=config.MaxCount)
                return false;
            var estate = new Estate(name, config);
            _estates.Add(estate.id, estate);
            _moneyService.AddIncome(estate.id, estate.Config.Income);
            OnEstatesChanged?.Invoke();
            return true;
        }

        [CanBeNull]
        public Estate GetEstate(string id)
        {
            return _estates.GetValueOrDefault(id);
        }
    }
}