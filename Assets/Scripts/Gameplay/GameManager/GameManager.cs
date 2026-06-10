using Gameplay.Services.MoneyService;
using Gameplay.Services.TimeService;
using Reflex.Attributes;
using UnityEngine;

namespace Gameplay.GameManager
{
    public class GameManager: MonoBehaviour
    {
        [Inject] private ITimeService _timeService;
        [Inject] private IMoneyService _moneyService;

        private void Awake()
        {
            Initialize();
        }
        
        private void Initialize()
        {
            _timeService.StartTicking();
        }
    }
}