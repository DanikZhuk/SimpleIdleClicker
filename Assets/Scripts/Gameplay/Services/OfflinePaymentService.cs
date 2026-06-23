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

        [Inject] private SaveDataService _saveDataService;
        [Inject] private MoneyService _moneyService;
        [Inject] private BusinessManager _businessManager;
        [Inject] private TimeService _timeService;

        private DateTime _nextIncomeTime;

        private void Start()
        {
            AddOfflineIncomes();
        }
        
        private void OnApplicationPause(bool isPaused)
        {
            if (!isPaused)
            {
                AddOfflineIncomes();
            }
        }

        private void Update()
        {
            if (Time.time >= _nextIncomeTime)
            {
                AddIncomes();
                _nextIncomeTime += paymentConfig.IncomeIntervalSeconds;
            }
        }

        private void AddIncomes()
        {
            var incomes = _businessManager.PurchasedBusinessControllers
                .Sum(controller => controller.GetIncome());

            _moneyService.Money += incomes;
        }
        
        private void AddOfflineIncomes()
        {
            var offlineTime = _timeService.ElapsedTimeSince(_saveDataService.RecordTime);
            var offlineTimeSeconds = (float)offlineTime.TotalSeconds;
            var count = (int)(offlineTimeSeconds/paymentConfig.IncomeIntervalSeconds);

            for (var i = 0; i < count; i++)
                AddIncomes();
        }
    }
}