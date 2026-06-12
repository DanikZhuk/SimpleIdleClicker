using System;
using System.Collections.Generic;
using System.Linq;
using Configs;
using Core.SaveSystem;
using Gameplay.Services;
using UnityEngine;
using Zenject;

namespace Gameplay.Estates.Generic
{
    public class EstateManager: MonoBehaviour
    {
        [Inject] private MoneyService _moneyService;
        [Inject] private OfflinePaymentService _offlinePaymentService;
        [Inject] private IDataService _dataService;
        public event Action OnEstatesChanged;
        
        public IReadOnlyList<Estate> Estates => _dataService.Estates;

        private void Start()
        {
            OnEstatesChanged += CalculateIncome;
            CalculateIncome();
        }

        public bool TryAddEstate(string name, EstateConfig config)
        {
            if (_dataService.Estates.Count(estate1 => estate1.Config.Type == config.Type)
                >= config.MaxCount)
                return false;
            if (!_moneyService.TrySpend(config.Price)) return false;

            var estate = new Estate(name, config, config.Price * config.SellPercentage);
            _dataService.AddEstate(estate);
            OnEstatesChanged?.Invoke();
            return true;
        }

        public void SellEstate(Estate estate)
        {
            if (estate == null) return;
            _moneyService.Earn(estate.sellPrice);
            _dataService.RemoveEstate(estate);
            OnEstatesChanged?.Invoke();
        }

        private void CalculateIncome()
        {
            _offlinePaymentService.EstateIncome=Estates.Sum(estate => estate.Config.Income);
        }
    }
}