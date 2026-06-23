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
        
        private SaveData _data;
        private string _filePath;
        
        private static readonly JsonSerializerSettings Settings = new()
        {
            TypeNameHandling = TypeNameHandling.Auto,
            Formatting = Formatting.Indented
        };
        
        public IReadOnlyList<BusinessModel> BusinessModels => _data.BusinessModels;
        public List<InvestmentModel> Investments => _data.Investments;
        
        public long Money
        {
            get => _data.Money;
            set => _data.Money = value;
        }

        public DateTime RecordTime => _data.RecordTime;

        private void Awake()
        {
            _filePath = Path.Combine(Application.persistentDataPath, "data.json");
            LoadData();
        }

        private void OnApplicationPause(bool isPaused)
        {
            if (isPaused)
                SaveData();
        }

        private void OnApplicationQuit()
        {
            SaveData();
        }

        public void AddBusiness(BusinessModel businessModel)
        {
            _data.BusinessModels.Add(businessModel);
            SaveData();
        }

        public void RemoveBusiness(BusinessModel businessModel)
        {
            _data.BusinessModels.Remove(businessModel);
            SaveData();
        }

        private void SaveData()
        {
            try
            {
                _data.RecordTime = _timeService.Now;
                File.WriteAllText(_filePath, JsonConvert.SerializeObject(_data, Settings));
            }
            catch (Exception e)
            {
                Debug.LogError($"Unable to save data due to: {e.Message} {e.StackTrace}");
            }
        }

        private void LoadData()
        {
            if (!File.Exists(_filePath))
            {
                Debug.Log($"Cannot load file at {_filePath}. File does not exist yet");
                _data = new SaveData();
                return;
            }

            try
            {
                var data = JsonConvert.DeserializeObject<SaveData>(File.ReadAllText(_filePath), Settings);
                _data = data;
            }
            catch (Exception e)
            {
                Debug.LogError($"Failed to load data due to: {e.Message} {e.StackTrace}");
                _data = new SaveData();
            }
        }
    }
}