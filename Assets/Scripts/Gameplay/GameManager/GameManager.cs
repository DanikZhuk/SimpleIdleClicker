using Gameplay.Services.TimeService;
using UnityEngine;
using Zenject;

namespace Gameplay.GameManager
{
    public class GameManager: MonoBehaviour
    {
        private ITimeService _timeService;

        [Inject]
        private void Construct(ITimeService timeService)
        {
            _timeService = timeService;
        }

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