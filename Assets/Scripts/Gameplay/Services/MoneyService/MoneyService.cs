using System;
using System.Collections.Generic;
using Gameplay.Services.TimeService;
using Reflex.Attributes;

namespace Gameplay.Services.MoneyService
{
    public class MoneyService : IMoneyService
    {
        public event Action OnMoneyChanged;

        private float _money;
        private float _tapAmount = 10f;

        private readonly Dictionary<string, float> _incomes = new();
        private ITimeService _timeService;
        
        
        public float Money => _money;
        
        [Inject]
        public void Construct(ITimeService timeService)
        {
            _timeService = timeService;
            Initialize();
        }

        private void Initialize()
        {
            _timeService.OnTick += Income;
        }

        public void Earn()
        {
            _money += _tapAmount;
            OnMoneyChanged?.Invoke();
        }

        public void AddIncome(string id, float amount)
        {
            _incomes.TryAdd(id, amount);
        }

        public bool TrySpend(float amount)
        {
            if (_money < amount) return false;
            _money -= amount;
            OnMoneyChanged?.Invoke();
            return true;
        }

        public bool CanSpend(float amount)
        {
            return _money >= amount;
        }
        
        private void Income()
        {
            foreach (var income in _incomes.Values)
            {
                _money += income;
            }
            
            OnMoneyChanged?.Invoke();
        }
    }
}