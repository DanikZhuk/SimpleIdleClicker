using System;
using System.Threading;
using Configs;
using Cysharp.Threading.Tasks;

namespace Gameplay.Services.TimeService
{
    public class TimeService:ITimeService
    {
        private float _time;
        public event Action OnTick;
        
        private CancellationTokenSource _cancellationTokenSource;
        private int _delay;

        public void StartTicking()
        {
            _cancellationTokenSource= new CancellationTokenSource();
            InitializeTicking().Forget();
        }

        public void StopTicking()
        {
            _cancellationTokenSource.Cancel();
        }

        private async UniTask InitializeTicking()
        {
            var config = await ConfigLoader.LoadInternalAsync<TimeConfig>("Configs/Time/TimeConfig");
            _delay = (int)(config.incomeSeconds*1000);
            _ = Ticking();
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