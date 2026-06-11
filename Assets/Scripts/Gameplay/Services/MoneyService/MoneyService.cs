using System;
using System.Collections.Generic;
using System.IO;
using Core.SaveSystem;
using Gameplay.Services.TimeService;
using UnityEngine;
using Zenject;

namespace Gameplay.Services.MoneyService
{
    public class MoneyService : IMoneyService
    {
        public event Action OnMoneyChanged;

        private float _money;
        private float _tapAmount = 10f;

        private readonly Dictionary<string, float> _incomes = new();
        private ITimeService _timeService;
        
        private IDataService _dataService;
        private const string Path = "Money/Money.json";

        private class MoneyData
        {
            public Dictionary<string, float> Incomes;
            public float Money;
            public MoneyData(Dictionary<string, float> incomes, float money)
            {
                Incomes = incomes;
                Money = money;
            }
        }
        
        
        public float Money => _money;
        
        [Inject]
        private void Construct(ITimeService timeService, IDataService dataService)
        {
            _timeService = timeService;
            _dataService = dataService;
            Initialize();
        }

        private void Initialize()
        {
            _timeService.OnTick += Income;

            try
            {
                var data = _dataService.LoadData<MoneyData>(Path);

                _money = data.Money;

                _incomes.Clear();
                foreach (var income in data.Incomes)
                {
                    _incomes.TryAdd(income.Key, income.Value);
                }
            }
            catch (FileNotFoundException e)
            {
                
            }
        }

        public void TapEarn()
        {
            _money += _tapAmount;
            OnMoneyChanged?.Invoke();
        }

        public void Earn(float amount)
        {
            _money += amount;
            OnMoneyChanged?.Invoke();
        }

        public void AddIncome(string id, float amount)
        {
            _incomes.TryAdd(id, amount);
        }

        public void RemoveIncome(string id)
        {
            _incomes.Remove(id);
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
        
        public void Dispose()
        {
            Debug.Log("DISPOSE");
            _dataService.SaveData(Path, new MoneyData(_incomes, _money));
        }
    }
}