using System;

namespace Gameplay.Services.TimeService
{
    public interface ITimeService
    {
        public event Action OnTick;
        public void StartTicking();
        public void StopTicking();
    }
}