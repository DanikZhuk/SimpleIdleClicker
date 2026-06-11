using System;

namespace Gameplay.Services.MoneyService
{
    public interface IMoneyService: IDisposable
    {
        public event Action OnMoneyChanged;
        public float Money { get; }
        public void TapEarn();
        public void Earn(float amount);
        public void AddIncome(string id, float amount);
        public void RemoveIncome(string id);
        public bool TrySpend(float amount);
        public bool CanSpend(float amount);
    }
}