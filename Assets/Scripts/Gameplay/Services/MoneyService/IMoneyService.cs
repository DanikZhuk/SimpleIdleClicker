using System;

namespace Gameplay.Services.MoneyService
{
    public interface IMoneyService
    {
        public event Action OnMoneyChanged;
        public void Earn();
        public void Income(float amount);
        public bool TrySpend(float amount);
    }
}