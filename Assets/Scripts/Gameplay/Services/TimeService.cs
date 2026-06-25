using System;
using System.Net;
using UnityEngine;

namespace Gameplay.Services
{
    public class TimeService : MonoBehaviour
    {
        public DateTime Now
        {
            get
            {
                Debug.Log(_loadedTime);
                return _loadedTime + TimeSpan.FromSeconds(_elapsedTime);
            }
        }

        private DateTime _loadedTime;
        private float _elapsedTime = 0;

        private readonly ServerTimeManager _serverTimeManager = new();


        private void Awake()
        {
            LoadTime();
        }

        private void OnApplicationPause(bool isPaused)
        {
            if (isPaused)
                LoadTime();
        }

        private void LoadTime()
        {
            _loadedTime = _serverTimeManager.FetchServerTime();
            _elapsedTime = 0f;
        }

        public void SetLoadTime(DateTime loadTime)
        {
            Debug.Log("Loading time: " + loadTime);
            _loadedTime = loadTime;
            _elapsedTime = 0f;
        }

        public TimeSpan ElapsedTimeSince(DateTime? oldTime)
        {
            if (oldTime == null)
                return TimeSpan.Zero;
            return Now - (DateTime)oldTime;
        }

        private void Update()
        {
            _elapsedTime += Time.deltaTime;
        }

        private class ServerTimeManager
        {
            // A free, public API that returns the current UTC time in JSON format
            private const string TimeApiUrl = "https://timeapi.io/api/v1/time/current/utc";

            [Serializable]
            public class WorldTimeResponse
            {
                public string utc_time; // Matches the JSON key from the API
            }

            public DateTime FetchServerTime()
            {
                try
                {
                    var myHttpWebRequest = (HttpWebRequest)WebRequest.Create(TimeApiUrl);
                    var response = myHttpWebRequest.GetResponse();
                    var todaysDates = response.Headers["date"];
                    if (DateTimeOffset.TryParse(todaysDates, out DateTimeOffset serverDateTimeOffset))
                    {
                        var serverTime = serverDateTimeOffset.UtcDateTime;
                        Debug.Log($"Successfully fetched server time (UTC): {serverTime}");
                        return serverTime;
                    }
                }
                catch (Exception e)
                {
                    Debug.LogError($"Error fetching server time.");
                }

                return DateTime.UtcNow;
            }
        }
    }
}