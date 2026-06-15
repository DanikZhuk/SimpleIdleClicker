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
    public class EstateManager : MonoBehaviour
    {
        [SerializeField] private EstateList list;

        [Inject] private MoneyService _moneyService;
        [Inject] private OfflinePaymentService _offlinePaymentService;
        [Inject] private IDataService _dataService;
        public event Action OnEstatesChanged;

        private readonly Dictionary<EstateType, EstateConfig> _configs = new();

        public IReadOnlyList<Estate> Estates => _dataService.Estates;

        public bool TryAddEstate(string name, EstateConfig config)
        {
            if (_dataService.Estates.Count(estate1 => estate1.Type == config.Type)
                >= config.MaxCount)
                return false;
            if (!_moneyService.TrySpend(config.Price)) return false;

            var estate = 
                new Estate(name, config.Type, (long)(config.Price * config.SellPercentage));
            _dataService.AddEstate(estate);
            OnEstatesChanged?.Invoke();
            return true;
        }

        public void SellEstate(Estate estate)
        {
            if (estate == null) return;
            _moneyService.Earn(estate.SellPrice);
            _dataService.RemoveEstate(estate);
            OnEstatesChanged?.Invoke();
        }

        public EstateConfig GetConfig(EstateType type)
        {
            return _configs[type];
        }

        private void Start()
        {
            OnEstatesChanged += CalculateIncome;
            PrepareConfigs();
            CalculateIncome();
        }

        private void CalculateIncome()
        {
            _offlinePaymentService.EstateIncome =
                Estates.Sum(estate => _configs[estate.Type].Income);
        }

        private void PrepareConfigs()
        {
            foreach (var estate in list.Estates)
            {
                _configs.Add(estate.Type, estate);
            }
        }
    }
}