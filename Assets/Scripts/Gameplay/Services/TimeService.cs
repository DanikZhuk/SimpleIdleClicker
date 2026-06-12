using System;
using System.Threading;
using Configs;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Gameplay.Services
{
    public class TimeService: MonoBehaviour
    {
        [SerializeField] private TimeConfig config;
        
        private float _time;
        public event Action OnTick;
        
        private CancellationTokenSource _cancellationTokenSource;
        private int _delay;
        
        private void Awake()
        {
            _delay = (int)(config.IncomeSeconds*1000);
        }

        public void StartTicking()
        {
            _cancellationTokenSource= new CancellationTokenSource();
            Ticking().Forget();
        }

        public void StopTicking()
        {
            _cancellationTokenSource.Cancel();
        }

        private async UniTask Ticking()
        {
            while (!_cancellationTokenSource.IsCancellationRequested)
            {
                await UniTask.Delay(_delay);
                OnTick?.Invoke();
            }
        }
    }
}