using System;
using UnityEngine;

namespace Gameplay.Services
{
    public class TimeService : MonoBehaviour
    {
        public TimeSpan ElapsedTimeSince(DateTime? oldTime)
        {
            if (oldTime == null)
                return TimeSpan.Zero;
            return Now - (DateTime)oldTime;
        }
        
        public DateTime Now => DateTime.UtcNow;
    }
}