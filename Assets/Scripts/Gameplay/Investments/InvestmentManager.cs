using System;
using System.Collections.Generic;
using System.Linq;
using Configs;
using Core.SaveSystem;
using Gameplay.Services;
using UI.Helpers.SystemMessages;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

namespace Gameplay.Investments
{
    public class InvestmentManager : MonoBehaviour
    {
        [FormerlySerializedAs("list")] [SerializeField]
        private InvestitionListConfig investmentListConfig;

        [Inject] private TimeService _timeService;
        [Inject] private MoneyService _moneyService;
        [Inject] private SaveDataService _saveDataService;
        [Inject] private SystemMessageManager _smm;

        private readonly List<InvestmentController> _investmentControllers = new();
        public IReadOnlyList<InvestmentController> InvestmentControllersList => _investmentControllers;
        public event Action OnInvestmentsUpdate;

        private float _runningUpdateTime;

        private void Start()
        {
            Initialize();
        }

        private void Initialize()
        {
            var configs = investmentListConfig.InvestmentConfigs.ToDictionary(investment => investment.Type);

            foreach (var investment in _saveDataService.Investments)
            {
                _investmentControllers.Add(new InvestmentController(investment, configs[investment.Type]));
                configs.Remove(investment.Type);
            }

            if (configs.Count > 0)
            {
                foreach (var investmentConfig in configs.Values)
                {
                    var investment = new InvestmentModel(investmentConfig.Type, investmentConfig.InitialCost);
                    _investmentControllers.Add(new InvestmentController(investment, investmentConfig));
                }
            }
            
            configs.Clear();
            
            _moneyService.OnMoneyChanged += InvestmentUpdate;
            InvestmentUpdate();
        }

        private void UpdateValues()
        {
            foreach (var investmentController in _investmentControllers)
            {
                investmentController.UpdateInvestment();
            }

            InvestmentUpdate();
        }

        private void InvestmentUpdate()
        {
            foreach (var investmentController in _investmentControllers)
            {
                investmentController.OnInvestmentUpdate(_moneyService.Money);
            }
            OnInvestmentsUpdate?.Invoke();
        }

        public void BuyInvestment(InvestmentController investmentController, long amount)
        {
            if(amount==0) return;
            
            investmentController.Amount += amount;
            
            _moneyService.Money -= investmentController.InvestmentModel.CurrentCost*amount;
            _smm.Log($"You successfully bought {amount} {investmentController.InvestmentConfig.Name}");
        }

        public void SellInvestment(InvestmentController investmentController, long amount)
        {
            if(amount==0) return;

            investmentController.Amount -= amount;

            _moneyService.Money += investmentController.InvestmentModel.CurrentCost*amount;
            _smm.Log($"You successfully sold {amount} {investmentController.InvestmentConfig.Name}");
        }

        private void Update()
        {
            foreach (var investmentController in _investmentControllers)
            {
                investmentController.Update(Time.deltaTime);
            }
            _runningUpdateTime+=Time.deltaTime;
            if (_runningUpdateTime <= investmentListConfig.UpdateTime) return;
            UpdateValues();
            OnInvestmentsUpdate?.Invoke();
            _runningUpdateTime = 0;
        }

        public float GetUpdateTimeProgress()
        {
            return _runningUpdateTime/investmentListConfig.UpdateTime;
        }
        
        private void OnDestroy()
        {
            _moneyService.OnMoneyChanged -= InvestmentUpdate;
        }
    }
}