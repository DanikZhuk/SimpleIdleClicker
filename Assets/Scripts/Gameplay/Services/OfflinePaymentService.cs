using System.Linq;
using Configs;
using Gameplay.Businesses;
using UnityEngine;
using Zenject;

namespace Gameplay.Services
{
    public class OfflinePaymentService : MonoBehaviour
    {
        [SerializeField] private OfflinePaymentConfig paymentConfig;

        [Inject] private MoneyService _moneyService;
        [Inject] private BusinessManager _businessManager;
        [Inject] private TimeService _timeService;

        private float _incomesRunningTime;

        private void Start()
        {
            AddOfflineIncomes();
            _timeService.OnOfflineTime += AddOfflineIncomes;
        }

        private void AddIncomes()
        {
            var incomes = _businessManager.PurchasedBusinessControllers
                .Sum(controller => controller.BusinessModel.Income);

            _moneyService.Money += incomes;
        }
        
        private void AddOfflineIncomes()
        {
            var incomes = _businessManager.PurchasedBusinessControllers
                .Sum(controller => controller.BusinessModel.Income);
            var count = (int)(_timeService.OfflineTime.TotalSeconds
                              /paymentConfig.IncomeIntervalSeconds);

            _moneyService.Money += incomes*count;
        }

        private void Update()
        {
            _incomesRunningTime += Time.deltaTime;

            if (!(_incomesRunningTime >= paymentConfig.IncomeIntervalSeconds)) return;
            AddIncomes();
            _incomesRunningTime = 0f;
        }
    }
}