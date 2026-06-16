using Zenject;

namespace Gameplay.Services
{
    public class OfflinePaymentService: IInitializable
    {
        [Inject] private TimeService _timeService;
        [Inject] private MoneyService _moneyService;

        public long EstateIncome = 0;


        public void Initialize()
        {
            _timeService.OnHourElapsed += AddIncomes;
        }

        private void AddIncomes()
        {
            _moneyService.Earn(EstateIncome);
        }
    }
}