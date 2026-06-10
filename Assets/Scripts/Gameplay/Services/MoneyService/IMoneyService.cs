using System;

namespace Gameplay.Services.MoneyService
{
    public interface IMoneyService
    {
        public event Action OnMoneyChanged;
        public float Money { get; }
        public void Earn();
        public void AddIncome(string id, float amount);
        public bool TrySpend(float amount);
        public bool CanSpend(float amount);
    }
}