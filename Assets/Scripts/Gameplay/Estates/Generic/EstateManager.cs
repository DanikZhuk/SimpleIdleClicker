using System;
using System.Collections.Generic;
using System.Linq;
using Configs;
using Gameplay.Services.MoneyService;
using JetBrains.Annotations;
using Zenject;

namespace Gameplay.Estates.Generic
{
    public class EstateManager:  IEstateManager
    {
        public event Action OnEstatesChanged;
        
        private readonly Dictionary<string, Estate> _estates = new();
        private IMoneyService _moneyService;
        
        private EconomyConfig _config;

        [Inject]
        private void Construct(IMoneyService moneyService, EconomyConfig econConfig)
        {
            _moneyService = moneyService;
            _config = econConfig;
        }

        public List<Estate> Estates => _estates.Values.ToList();

        public bool TryAddEstate(string name,EstateConfig config)
        {
            if(_estates.Count(estate1 => estate1.Value.Config.Type == config.Type)
               >=config.MaxCount)
                return false;
            if (!_moneyService.TrySpend(config.Price)) return false;
            
            var estate = new Estate(name, config, config.Price*_config.sellPercentage);
            _estates.Add(estate.id, estate);
            _moneyService.AddIncome(estate.id, estate.Config.Income);
            OnEstatesChanged?.Invoke();
            return true;
        }
        
        public void SellEstate(string id)
        {
            var estate = _estates.GetValueOrDefault(id);
            if (estate == null) return;
            _moneyService.RemoveIncome(id);
            _moneyService.Earn(estate.sellPrice);
            _estates.Remove(id);
            OnEstatesChanged?.Invoke();
        }

        [CanBeNull]
        public Estate GetEstate(string id)
        {
            return _estates.GetValueOrDefault(id);
        }
    }
}