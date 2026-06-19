using System;
using System.Collections.Generic;
using System.Threading;
using Configs;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Gameplay.Services
{
    public class TimeService : MonoBehaviour
    {
        [SerializeField] private TimeConfig config;
        public event Action OnHourElapsed;
        
        private int _hourTimeSeconds;
        
        private CancellationTokenSource _cancellationTokenSource;

        public TimeSpan ElapsedTimeSince(DateTime? oldTime)
        {
            if (oldTime == null)
                return TimeSpan.Zero;
            return Now() - (DateTime)oldTime;
        }
        public DateTime Now()
        {
            return DateTime.UtcNow;
        }
        
        private void Awake()
        {
            _hourTimeSeconds = config.HourInSeconds;
            StartTicking();
        }
        
        private void StartTicking()
        {
            _cancellationTokenSource = new CancellationTokenSource();
            Ticking().Forget();
        }

        private async UniTask Ticking()
        {
            float runningHourTimeSeconds = _hourTimeSeconds;
            while (!_cancellationTokenSource.IsCancellationRequested)
            {
                runningHourTimeSeconds -= Time.deltaTime;
                if (runningHourTimeSeconds < 0)
                {
                    runningHourTimeSeconds = _hourTimeSeconds;
                    OnHourElapsed?.Invoke();
                }
                await UniTask.Yield();
            }
        }
        
    }
}