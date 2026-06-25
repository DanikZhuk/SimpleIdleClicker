using System;
using System.Net;
using Core.SaveSystem;
using UnityEngine;
using Zenject;

namespace Gameplay.Services
{
    public class TimeService : MonoBehaviour
    {
        [Inject] SaveDataService _saveDataService;
        public DateTime Now => _loadedTime + TimeSpan.FromSeconds(_elapsedTime);
        public event Action OnTimeLoaded;

        private DateTime _loadedTime;
        private float _elapsedTime = 0;

        private readonly ServerTimeManager _serverTimeManager = new();

        private void Awake()
        {
            //Initializing time
            _loadedTime = _serverTimeManager.FetchServerTime();
        }

        private void OnApplicationPause(bool isPaused)
        {
            if (isPaused) return;
            LoadTime();
        }

        private void LoadTime()
        {
            _loadedTime = _serverTimeManager.FetchServerTime();
            _elapsedTime = 0f;
            if (_loadedTime < _saveDataService.RecordTime)
                _loadedTime = _saveDataService.RecordTime;
            OnTimeLoaded?.Invoke();
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
            private const string TimeApiUrl = "https://timeapi.io/api/v1/time/current/utc";

            public DateTime FetchServerTime()
            {
                try
                {
                    var myHttpWebRequest = (HttpWebRequest)WebRequest.Create(TimeApiUrl);
                    var response = myHttpWebRequest.GetResponse();
                    var nowDateTime = response.Headers["date"];
                    if (DateTimeOffset.TryParse(nowDateTime, out DateTimeOffset serverDateTimeOffset))
                    {
                        var serverTime = serverDateTimeOffset.UtcDateTime;
                        Debug.Log($"Successfully fetched server time (UTC): {serverTime}");
                        return serverTime;
                    }
                }
                catch (Exception e)
                {
                    Debug.LogError($"Error fetching server time due to {e.Message}");
                }

                return DateTime.UtcNow;
            }
        }
    }
}