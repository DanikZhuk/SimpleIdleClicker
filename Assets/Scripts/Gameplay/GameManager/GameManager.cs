using Gameplay.Services;
using UnityEngine;
using Zenject;

namespace Gameplay.GameManager
{
    public class GameManager: MonoBehaviour
    {
        [Inject] private TimeService _timeService;

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