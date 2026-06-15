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
        
        private int _incomeTime;
        private int _investitionTime;
        public event Action OnTick;

        public event Action OnInvestitionUpdate;
        
        private CancellationTokenSource _cancellationTokenSource;
        
        private void Awake()
        {
            _incomeTime = (int)(config.IncomeSeconds*1000);
            _investitionTime = (int)(config.InvestitionUpdateSeconds*1000);
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
            var incTime = _incomeTime;
            var invTime = _investitionTime;
            while (!_cancellationTokenSource.IsCancellationRequested)
            {
                var delay = Mathf.Min(incTime, invTime);
                await UniTask.Delay(delay);
                incTime -= delay;
                invTime -= delay;
                if (incTime <= 0)
                {
                    incTime = _incomeTime;
                    OnTick?.Invoke();
                }
                if (invTime <= 0)
                {
                    invTime = _investitionTime;
                    OnInvestitionUpdate?.Invoke();
                }
            }
        }
    }
}