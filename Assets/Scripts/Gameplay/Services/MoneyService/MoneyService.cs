using System;

namespace Gameplay.Services.MoneyService
{
    public class MoneyService : IMoneyService
    {
        public event Action OnMoneyChanged;
        
        private float _money;
        private float _tapAmount = 10f;
        
        public float Money => _money;

        public void Earn()
        {
            _money += _tapAmount;
            OnMoneyChanged?.Invoke();
        }

        public void Income(float amount)
        {
            _money += amount;
            OnMoneyChanged?.Invoke();
        }

        public bool TrySpend(float amount)
        {
            if (_money < amount) return false;
            _money -= amount;
            OnMoneyChanged?.Invoke();
            return true;
        }
    }
}