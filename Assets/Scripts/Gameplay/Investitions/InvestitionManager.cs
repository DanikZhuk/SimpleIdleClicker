using System;
using System.Collections.Generic;
using System.Linq;
using Configs;
using Core.SaveSystem;
using Gameplay.Estates.Generic;
using Gameplay.Services;
using UI.Helpers.SystemMessages;
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
        [Inject] private SystemMessageManager _smm;

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

            _timeService.OnUpdate += OnUpdate;
            
            if (Investitions.Count == _configs.Count) return;
            
            foreach (var investitionConfig in list.InvestitionConfigs)
            {
                var investition = Investitions.Find(investition1 => investition1.Type == investitionConfig.Type) ?? new Investition(investitionConfig.Type, investitionConfig.InitialCost);
                Investitions.Add(investition);
                _investitionCalculator.InitializeValues(investition, investitionConfig, list.HistorySize);
            }
        }

        private void OnUpdate()
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
            if (amount == 0)
            {
                _smm.Log("The amount to buy equals 0");
                return;
            }

            if (investition == null)
            {
                _smm.Log("Error! Investition is not fount");
                return;
            }
            if (investition.ResumptionTime > 0)
            {
                _smm.Log("You can't buy anything now");
                return;
            }
            if (investition.PurchasedAmount + amount > config.MaxAmount)
            {
                _smm.Log("Amount is more than maximum");
                return;
            }
            var cost = (int)(amount * investition.CurrentCost);
            if (!_moneyService.TrySpend(cost))
            {
                _smm.Log("You don't have enough money");
                return;
            }
            
            investition.Add(amount, config.ResumptionTime);
        }
        
        public void SellInvestitions(InvestitionType type, int amount)
        {
            var investition = Investitions.Find(investition => investition.Type == type);
            var config = _configs[type];
            if(amount==0)
                return;
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