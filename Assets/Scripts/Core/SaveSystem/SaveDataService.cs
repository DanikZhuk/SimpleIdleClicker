using System;
using System.Collections.Generic;
using System.IO;
using Core.SaveSystem.Data;
using Gameplay.Businesses.BusinessModels;
using Gameplay.Investments;
using Gameplay.Services;
using Newtonsoft.Json;
using UnityEngine;
using Zenject;

namespace Core.SaveSystem
{
    public class SaveDataService : MonoBehaviour
    {
        [Inject] private TimeService _timeService;
        public event Action OnDataLoaded;

        private SaveData _offlineData;
        private SaveData _onlineData;

        private string _offlineDataPath;
        private string _onlineDataPath;

        private static readonly JsonSerializerSettings Settings = new()
        {
            TypeNameHandling = TypeNameHandling.Auto,
            Formatting = Formatting.Indented
        };

        public IReadOnlyList<BusinessModel> BusinessModels => _offlineData.BusinessModels;
        public List<InvestmentModel> Investments => _offlineData.Investments;

        public long Money
        {
            get => _offlineData.Money;
            set => _offlineData.Money = value;
        }

        public DateTime RecordTime => _offlineData.RecordTime;
        public TimeSpan ServerTimeOffset => _offlineData.ServerTimeOffset;
        public bool Initialized => _offlineData != null;
        public bool TimeDataInitialized => _offlineData.TimeDataInitialized;

        private void Awake()
        {
            _offlineDataPath = Path.Combine(Application.persistentDataPath, "offlineData.json");
            _onlineDataPath = Path.Combine(Application.persistentDataPath, "onlineData.json");

            LoadFiles();
        }

        private void OnApplicationPause(bool isPaused)
        {
            if (!isPaused) return;

            SaveFiles();
        }

        private void OnApplicationQuit()
        {
            SaveFiles();
        }

        public void AddBusiness(BusinessModel businessModel)
        {
            _offlineData.BusinessModels.Add(businessModel);
            SaveFiles();
        }

        public void RemoveBusiness(BusinessModel businessModel)
        {
            _offlineData.BusinessModels.Remove(businessModel);
            SaveFiles();
        }

        public void SyncOnlineData(DateTime recordTime, TimeSpan serverTimeOffset)
        {
            _offlineData.TimeDataInitialized = true;
            _offlineData.RecordTime = recordTime;
            _offlineData.ServerTimeOffset = serverTimeOffset;

            _onlineData =
                JsonConvert.DeserializeObject<SaveData>(JsonConvert.SerializeObject(_offlineData, Settings), Settings);
        }

        public void RecoverOnlineData(TimeSpan serverTimeOffset)
        {
            _onlineData.ServerTimeOffset = serverTimeOffset;

            _offlineData =
                JsonConvert.DeserializeObject<SaveData>(JsonConvert.SerializeObject(_onlineData, Settings), Settings);
            OnDataLoaded?.Invoke();
        }

        private void SaveFiles()
        {
            SaveDataFile(_offlineData, _offlineDataPath, true);
            SaveDataFile(_onlineData, _onlineDataPath, false);
        }

        private void LoadFiles()
        {
            LoadDataFile(out _offlineData, _offlineDataPath);
            LoadDataFile(out _onlineData, _onlineDataPath);
            OnDataLoaded?.Invoke();
        }

        private void SaveDataFile(SaveData saveData, string filePath, bool updateTime)
        {
            try
            {
                if (updateTime)
                    saveData.RecordTime = _timeService.Now;
                File.WriteAllText(filePath, JsonConvert.SerializeObject(saveData, Settings));
            }
            catch (Exception e)
            {
                Debug.LogError($"Unable to save data due to: {e.Message} {e.StackTrace}");
            }
        }

        private void LoadDataFile(out SaveData saveData, string filePath)
        {
            try
            {
                var data = JsonConvert.DeserializeObject<SaveData>(File.ReadAllText(filePath), Settings);
                saveData = data;
            }
            catch (Exception e)
            {
                Debug.LogError($"Failed to load data due to: {e.Message} {e.StackTrace}");
                saveData = new SaveData
                {
                    RecordTime = _timeService.Now
                };
                Debug.Log(saveData.RecordTime);
            }
        }
    }
}