using System;
using Configs;
using Core.SaveSystem;
using UnityEngine;
using Zenject;

namespace Gameplay.Services
{
    public class MoneyService : MonoBehaviour
    {
        [SerializeField] private ClickConfig click;
        
        [Inject] private IDataService _dataService;

        public event Action OnMoneyChanged;
        
        public long Money
        {
            get=>_dataService.Money;
            set=>_dataService.Money = value;
        }

        public void TapEarn()
        {
            Money += click.TapAmount;
            OnMoneyChanged?.Invoke();
        }

        public void Earn(long amount)
        {
            Money += amount;
            OnMoneyChanged?.Invoke();
        }

        public bool TrySpend(long amount)
        {
            if (Money < amount) return false;
            Money -= amount;
            OnMoneyChanged?.Invoke();
            return true;
        }

        public bool CanSpend(long amount)
        {
            return Money >= amount;
        }
    }
}