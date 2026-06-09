using Gameplay.Services.MoneyService;
using Gameplay.Services.TimeService;
using UnityEngine;

namespace Gameplay.GameManager
{
    public class GameManager: MonoBehaviour
    {
        private TimeService _timeService;
        private MoneyService _moneyService;
        
        public TimeService TimeService=>_timeService;
        public MoneyService MoneyService => _moneyService;

        private void Awake()
        {
            Initialize();
        }
        
        private void Initialize()
        {
            _timeService = new TimeService();
            _moneyService = new MoneyService();
        }
    }
}