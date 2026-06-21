using System;
using UnityEngine;

namespace Gameplay.Services
{
    public class TimeService : MonoBehaviour
    {
        public TimeSpan OfflineTime { get; private set; }
        public event Action OnOfflineTime;

        public void SetLastRunTime(DateTime time)
        {
            OfflineTime = ElapsedTimeSince(time);
            OnOfflineTime?.Invoke();
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
    }
}