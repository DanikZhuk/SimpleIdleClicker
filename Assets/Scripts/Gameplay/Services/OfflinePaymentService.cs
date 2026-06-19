using System.Linq;
using Gameplay.Businesses;
using Zenject;

namespace Gameplay.Services
{
    public class OfflinePaymentService : IInitializable
    {
        [Inject] private TimeService _timeService;
        [Inject] private MoneyService _moneyService;
        [Inject] BusinessManager _businessManager;

        public void Initialize()
        {
            _timeService.OnHourElapsed += AddIncomes;
        }

        private void AddIncomes()
        {
            var incomes = _businessManager.PurchasedBusinessControllers
                .Sum(controller => controller.BusinessModel.Income);

            _moneyService.Money += incomes;
        }
    }
}