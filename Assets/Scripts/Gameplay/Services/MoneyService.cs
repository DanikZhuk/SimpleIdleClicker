using System;
using Configs;
using Core.SaveSystem;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

namespace Gameplay.Services
{
    public class MoneyService : MonoBehaviour
    {
        [FormerlySerializedAs("click")] [SerializeField] private ClickConfig clickConfig;

        [Inject] private SaveDataService _saveDataService;

        public event Action OnMoneyChanged;

        public long Money
        {
            get => _saveDataService.Money;
            set
            {
                _saveDataService.Money = value;
                OnMoneyChanged?.Invoke();
            }
        }

        public void TapEarn()
        {
            Money += clickConfig.TapAmount;
        }
    }
}