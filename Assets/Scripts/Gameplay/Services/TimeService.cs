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

        public event Action OnTickElapsed;
        public event Action OnHourElapsed;
        public event Action OnUpdate;
        
        private float _tickTime;
        private int _hourTime;
        private int _updateTime;
        
        private int _runningHourTime;
        private int _runningUpdateTime;
        

        public float CurrentHourProgress
        {
            get=>_runningHourTime/(_hourTime-1f);
        }
        
        public float CurrentUpdateProgress
        {
            get=>_runningUpdateTime/(_updateTime-1f);
        }
        
        private CancellationTokenSource _cancellationTokenSource;

        public void StartTicking()
        {
            _cancellationTokenSource = new CancellationTokenSource();
            Ticking().Forget();
        }

        public void StopTicking()
        {
            _cancellationTokenSource.Cancel();
        }

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
            _tickTime = config.RealSecondsInSecond;
            _hourTime = config.HourInSeconds;
            _updateTime= config.UpdateSeconds;
            OnTickElapsed+=OnTick;
            StartTicking();
        }

        private async UniTask Ticking()
        {
            float tickTime = _tickTime;
            while (!_cancellationTokenSource.IsCancellationRequested)
            {
                tickTime -= Time.deltaTime;
                if (tickTime < 0)
                {
                    tickTime = _tickTime;
                    OnTickElapsed?.Invoke();
                }
                await UniTask.Yield();
            }
        }

        private void OnTick()
        {
            _runningHourTime++;
            _runningUpdateTime++;
            if (_runningHourTime >= _hourTime)
            {
                _runningHourTime = 0;
                OnHourElapsed?.Invoke();
            }
            if (_runningUpdateTime >= _updateTime)
            {
                _runningUpdateTime = 0;
                OnUpdate?.Invoke();
            }
        }
    }
}