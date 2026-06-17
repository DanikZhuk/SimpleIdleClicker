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
    public class InvestitionManager : MonoBehaviour
    {
        [SerializeField] private InvestitionListConfig list;

        [Inject] private TimeService _timeService;
        [Inject] private MoneyService _moneyService;
        [Inject] private IDataService _dataService;
        [Inject] private SystemMessageManager _smm;

        private readonly Dictionary<InvestitionType, InvestitionConfig> _configs = new();
        private readonly InvestitionCalculator _investitionCalculator = new();

        private List<Investition> _investitions;
        public IReadOnlyList<Investition> InvestitionsList => _dataService.Investitions;

        private void Start()
        {
            Initialize();
        }

        private void Initialize()
        {
            _investitions = _dataService.Investitions;

            foreach (var config in list.InvestitionConfigs)
            {
                _configs.Add(config.Type, config);
            }

            _timeService.OnUpdate += OnUpdate;

            if (_investitions.Count == _configs.Count) return;

            foreach (var investitionConfig in list.InvestitionConfigs)
            {
                Investition investition =
                    _investitions.Find(investition1 => investition1.Type == investitionConfig.Type);
                if (investition == null)
                {
                    investition = new Investition(investitionConfig.Type, investitionConfig.InitialCost);
                    _investitions.Add(investition);
                }

                _investitionCalculator.InitializeValues(investition, investitionConfig, list.HistorySize);
            }
        }

        private void OnUpdate()
        {
            foreach (var investition in _investitions)
            {
                _investitionCalculator.UpdateInvestition(investition, _configs[investition.Type], list.HistorySize);
            }
        }

        public void BuyInvestitions(InvestitionType type, int amount)
        {
            var investition = _investitions.Find(investition => investition.Type == type);
            var config = _configs[type];
            if (amount == 0)
            {
                _smm.Log("The amount to buy equals 0");
                return;
            }

            if (investition == null)
            {
                _smm.Log("Error! Investment is not fount");
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
            _smm.Log($"You successfully bought {amount} {config.Name}");
        }

        public void SellInvestitions(InvestitionType type, int amount)
        {
            var investition = _investitions.Find(investition => investition.Type == type);
            var config = _configs[type];
            if (amount == 0)
            {
                _smm.Log("The amount to sell equals 0");
                return;
            }

            if (investition == null)
            {
                _smm.Log("Error! Investment is not found");
                return;
            }

            if (investition.ResumptionTime > 0)
            {
                _smm.Log("You can't sell anything now. You should wait.");
                return;
            }

            if (investition.PurchasedAmount - amount < 0)
            {
                _smm.Log("You can't sell more than you have");
                return;
            }

            investition.Add(-amount, config.ResumptionTime);
            _moneyService.Earn((int)(amount * investition.CurrentCost));
            _smm.Log($"You successfully sold {amount} {config.Name}");
        }

        public InvestitionConfig GetConfig(InvestitionType type)
        {
            return _configs[type];
        }
    }
}