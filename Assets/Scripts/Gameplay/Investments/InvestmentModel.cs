using System.Collections.Generic;

namespace Gameplay.Investments
{
    public class InvestmentModel
    {
        public InvestmentType Type;
        public List<float> History = new();

        private long _currentCost;
        private float _lastChange;
        private long _purchasedAmount;
        private float _resumptionTime;

        public long CurrentCost
        {
            get => _currentCost;
            set
            {
                _lastChange = (value * 100) / (float)_currentCost;
                _lastChange -= 100f;
                _currentCost = value;
            }
        }

        public float LastChange => _lastChange;
        public float ResumptionTime
        {
            get => _resumptionTime;
            set
            {
                _resumptionTime = value;
            }
        }
        public long PurchasedAmount
        {
            get => _purchasedAmount;
            set
            {
                _purchasedAmount = value;
            }
        }

        public InvestmentModel(InvestmentType type, long initialCost)
        {
            Type = type;
            _currentCost = initialCost;
        }
    }
}