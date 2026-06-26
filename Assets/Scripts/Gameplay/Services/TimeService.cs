using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using Configs;
using Core.SaveSystem;
using Cysharp.Threading.Tasks;
using UI.Helpers.SystemMessages;
using UI.WarningViews.TimeWarning;
using UnityEngine;
using Zenject;

namespace Gameplay.Services
{
    public class TimeService : MonoBehaviour
    {
        [SerializeField] private ServerTimeConfing serverTimeConfig;
        [Inject] private SaveDataService _saveDataService;
        [Inject] private SystemMessageManager _systemMessageManager;
        public event Action OnOfflineTime;
        public event Action OnTimeWarning;

        public DateTime Now
        {
            get
            {
                if (_saveDataService.Initialized)
                {
                    return DateTime.UtcNow + _saveDataService.ServerTimeOffset;
                }

                return DateTime.UtcNow;
            }
        }

        private readonly ServerTimeManager _serverTimeManager = new();

        private bool _isUpdatingTime;
        private bool _isRecovering;
        private CancellationTokenSource _updateCancellationTokenSource;

        public TimeSpan ElapsedTimeSince(DateTime? oldTime)
        {
            if (oldTime == null)
                return TimeSpan.Zero;
            return Now - (DateTime)oldTime;
        }

        public async UniTask<bool> OnRecover()
        {
            var serverTime = await _serverTimeManager.FetchServerTimeAsync();
            if (!serverTime.HasValue)
            {
                _systemMessageManager.Log("Can't reach server time");
                return false;
            }

            var timeOffset = serverTime.Value - DateTime.UtcNow;
            _saveDataService.RecoverOnlineData(timeOffset);

            _isRecovering = false;
            _updateCancellationTokenSource = new CancellationTokenSource();
            UpdateTimeLoop().Forget();
            return true;
        }

        private void OnApplicationPause(bool isPaused)
        {
            if (!isPaused)
            {
                if (_isRecovering) return;
                _updateCancellationTokenSource = new CancellationTokenSource();
                UpdateTimeLoop().Forget();
            }
            else
            {
                try
                {
                    _updateCancellationTokenSource?.Cancel();
                }
                catch (Exception e)
                {
                    Debug.LogError($"Can't cancel update due to {e.Message}");
                }
            }
        }
        
        private void OnApplicationQuit()
        {
            if(!_isRecovering)
                ForceUpdateTime().Forget();
        }

        private async UniTask UpdateTimeLoop()
        {
            OnOfflineTime?.Invoke();

            while (!_updateCancellationTokenSource.Token.IsCancellationRequested)
            {
                try
                {
                    await ForceUpdateTime();

                    await UniTask.Delay(serverTimeConfig.ServerTimeUpdateMilliseconds,
                        cancellationToken: _updateCancellationTokenSource.Token);
                }
                catch (OperationCanceledException)
                {
                    break;
                }
            }

            _updateCancellationTokenSource?.Dispose();
        }

        private async UniTask ForceUpdateTime()
        {
            if (_isUpdatingTime)
                return;

            _isUpdatingTime = true;


            try
            {
                if (!CheckOfflineTime())
                {
                    _updateCancellationTokenSource?.Cancel();

                    _isRecovering = true;
                    OnTimeWarning?.Invoke();
                    return;
                }

                var serverTime = await _serverTimeManager.FetchServerTimeAsync();

                if (serverTime.HasValue)
                {
                    if (CheckServerTime(serverTime.Value))
                    {
                        var timeOffset = serverTime.Value - DateTime.UtcNow;
                        _saveDataService.SyncOnlineData(serverTime.Value, timeOffset);
                        Debug.Log($"Server time updated: {serverTime.Value}");
                    }
                    else
                    {
                        Debug.LogWarning($"Server time is out of tolerance range: {serverTime}, {Now}");
                        _updateCancellationTokenSource?.Cancel();

                        _isRecovering = true;
                        OnTimeWarning?.Invoke();
                    }
                }
                else
                {
                    Debug.LogWarning("Failed to fetch server time");
                }
            }
            catch (Exception e)
            {
                Debug.LogError($"Error updating time: {e.Message}");
            }
            finally
            {
                _isUpdatingTime = false;
            }
        }

        private bool CheckServerTime(DateTime serverTime)
        {
            if (!_saveDataService.TimeDataInitialized)
                return true;

            var timeDifference = serverTime - Now;
            var timeDifferenceInSeconds = (float)timeDifference.TotalSeconds;
            return Mathf.Abs(timeDifferenceInSeconds) < serverTimeConfig.MaxSecondsTolerance;
        }

        private bool CheckOfflineTime()
        {
            return _saveDataService.RecordTime <= Now;
        }

        private void OnDestroy()
        {
            _updateCancellationTokenSource?.Cancel();
            _updateCancellationTokenSource?.Dispose();
        }

        private class ServerTimeManager
        {
            private const string TimeApiUrl = "https://timeapi.io/api/v1/time/current/utc";
            private readonly HttpClient _httpClient = new();

            public async UniTask<DateTime?> FetchServerTimeAsync()
            {
                try
                {
                    using var request = new HttpRequestMessage(HttpMethod.Head, TimeApiUrl);
                    var response = await _httpClient.SendAsync(request);

                    if (response.Headers.TryGetValues("date", out var dateValues))
                    {
                        var dateHeader = dateValues.FirstOrDefault();
                        if (DateTimeOffset.TryParse(dateHeader, out DateTimeOffset serverDateTimeOffset))
                        {
                            return serverDateTimeOffset.UtcDateTime;
                        }
                    }
                }
                catch (Exception e)
                {
                    Debug.LogError($"Error fetching server time: {e.Message}");
                }

                return null;
            }
        }
    }
}