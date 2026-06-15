using System;
using System.Collections.Generic;
using System.Linq;
using Configs;
using Core.SaveSystem;
using Gameplay.Estates.Generic;
using Gameplay.Services;
using UnityEngine;
using Zenject;

namespace Gameplay.Investitions
{
    public class InvestitionManager: MonoBehaviour
    {
        [SerializeField] private InvestitionListConfig list;

        [Inject] private TimeService _timeService;
        [Inject]  private MoneyService _moneyService;
        [Inject] private IDataService _dataService;

        private readonly Dictionary<InvestitionType, InvestitionConfig> _configs = new();
        private readonly InvestitionCalculator _investitionCalculator= new();

        private List<Investition> Investitions => _dataService.Investitions;
        public IReadOnlyList<Investition> InvestitionsList => _dataService.Investitions;

        private void Awake()
        {
            Initialize();
        }

        private void Initialize()
        {
            foreach (var config in list.InvestitionConfigs)
            {
                _configs.Add(config.Type, config);
            }
            
            _timeService.OnInvestitionUpdate += OnInvestitionsUpdate;
            
            Debug.Log(Investitions.Count);
            
            if (Investitions.Count == _configs.Count) return;
            foreach (var investition in list.InvestitionConfigs.Where(investition => Investitions.Find(investition1 => investition1.Type == investition.Type) == null))
            {
                Investitions.Add(new Investition(investition.Type, investition.InitialCost));
            }
            Debug.Log(Investitions.Count);
        }

        private void OnInvestitionsUpdate()
        {
            foreach (var investition in Investitions)
            {
                _investitionCalculator.UpdateInvestition(investition, _configs[investition.Type], list.HistorySize);
            }
        }
        
        public void BuyInvestitions(InvestitionType type, int amount)
        {
            var investition = Investitions.Find(investition => investition.Type == type);
            var config = _configs[type];
            if (investition == null)
                return;
            if (investition.ResumptionTime > 0)
                return;
            if (investition.PurchasedAmount + amount > config.MaxAmount)
                return;
            var cost = (int)(amount * investition.CurrentCost);
            if (!_moneyService.TrySpend(cost)) return;
            
            investition.Add(amount, config.ResumptionTime);
        }
        
        public void SellInvestitions(InvestitionType type, int amount)
        {
            var investition = Investitions.Find(investition => investition.Type == type);
            var config = _configs[type];
            if (investition == null)
                return;
            if (investition.ResumptionTime > 0)
                return;
            if (investition.PurchasedAmount - amount < 0)
                return;
            
            investition.Add(-amount, config.ResumptionTime);
            _moneyService.Earn((int)(amount * investition.CurrentCost));
        }

        public InvestitionConfig GetConfig(InvestitionType type)
        {
            return _configs[type];
        }
    }
}