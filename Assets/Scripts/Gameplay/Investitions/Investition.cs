using System.Collections.Generic;
using Configs;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Gameplay.Investitions
{
    public class Investition
    {
        public InvestitionType Type;
        public List<float> History = new();

        private long _currentCost;
        private float _lastChange;
        private int _purchasedAmount;
        private float _resumptionTime;

        public long CurrentCost
        {
            get => _currentCost;
            set
            {
                _lastChange = (value*100) / (float)_currentCost;
                _lastChange -= 100f;
                _currentCost = value;
            }
        }

        public float LastChange => _lastChange;
        public float ResumptionTime => _resumptionTime;
        public int PurchasedAmount => _purchasedAmount;

        public Investition(InvestitionType type, long initialCost)
        {
            Type = type;
            _currentCost = initialCost;
        }

        public void Add(int amount, float resumptionTime=1f)
        {
            _purchasedAmount += amount;
            Resumption(resumptionTime).Forget();
        }

        private async UniTask Resumption(float time)
        {
            _resumptionTime = time;
            while (_resumptionTime > 0)
            {
                _resumptionTime -= Time.deltaTime;
                await UniTask.Yield();
            }

            _resumptionTime = 0;
        }
    }
}