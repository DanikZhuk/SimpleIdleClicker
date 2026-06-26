using System;
using System.Linq;
using Configs;
using Core.SaveSystem;
using Gameplay.Businesses;
using UnityEngine;
using Zenject;

namespace Gameplay.Services
{
    public class OfflinePaymentService : MonoBehaviour
    {
        [SerializeField] private OfflinePaymentConfig paymentConfig;
        [Inject] private BusinessManager _businessManager;
        [Inject] private MoneyService _moneyService;
        [Inject] private SaveDataService _saveDataService;
        [Inject] private TimeService _timeService;

        private TimeSpan _nextIncomeTime;

        private void Awake()
        {
            _timeService.OnOfflineTime += CalculateOfflineImpact;
        }

        private void Update()
        {
            if (!(Time.time >= _nextIncomeTime.TotalSeconds)) return;

            var endTime = DateTime.Now;
            var startTime = endTime - _nextIncomeTime;

            AddIncomes(startTime, endTime);
            _nextIncomeTime += TimeSpan.FromSeconds(paymentConfig.IncomeIntervalSeconds);
        }

        private void CalculateOfflineImpact()
        {
            AddIncomes(_saveDataService.RecordTime, _timeService.Now);
        }

        private void AddIncomes(DateTime startTime, DateTime endTime)
        {
            var incomes = _businessManager.PurchasedBusinessControllers
                .Sum(controller => controller.GetIncome(startTime, endTime, _businessManager.IncomeHourInSeconds));

            _moneyService.Money += incomes;
        }
    }
}