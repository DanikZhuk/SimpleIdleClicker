using System;
using System.Collections.Generic;
using System.IO;
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
        
        public float Money
        {
            get=>_dataService.Money;
            set=>_dataService.Money = value;
        }

        public void TapEarn()
        {
            Money += click.TapAmount;
            OnMoneyChanged?.Invoke();
        }

        public void Earn(float amount)
        {
            Money += amount;
            OnMoneyChanged?.Invoke();
        }

        public bool TrySpend(float amount)
        {
            if (Money < amount) return false;
            Money -= amount;
            OnMoneyChanged?.Invoke();
            return true;
        }

        public bool CanSpend(float amount)
        {
            return Money >= amount;
        }
    }
}