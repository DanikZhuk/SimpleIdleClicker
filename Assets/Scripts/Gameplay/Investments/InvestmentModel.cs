using System.Collections.Generic;

namespace Gameplay.Investments
{
    public class InvestmentModel
    {
        private long _currentCost;
        public List<float> History = new();
        public InvestmentType Type;
        
        public float LastChange { get; private set; }

        public float ResumptionTime { get; set; }

        public long PurchasedAmount { get; set; }

        public long CurrentCost
        {
            get => _currentCost;
            set
            {
                LastChange = value * 100 / (float)_currentCost;
                LastChange -= 100f;
                _currentCost = value;
            }
        }
        
        public InvestmentModel(InvestmentType type, long initialCost)
        {
            Type = type;
            _currentCost = initialCost;
        }
    }
}