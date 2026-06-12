using System;
using System.Collections.Generic;
using System.IO;
using Core.SaveSystem.Data;
using Gameplay.Estates.Generic;
using Newtonsoft.Json;
using UnityEngine;
using Zenject;

namespace Core.SaveSystem
{
    public class JsonDataService : IInitializable, IDataService, IDisposable
    {
        private static readonly string FilePath = Path.Combine(Application.persistentDataPath, "data.json");
        private GameData _data = new();
        public List<Estate> Estates
        {
            get => _data.Estates;
        }
        
        public void AddEstate(Estate estate)
        {
            Estates.Add(estate);
            SaveData();
        }
        
        public void RemoveEstate(Estate estate)
        {
            Estates.Remove(estate);
            SaveData();
        }

        public float Money
        {
            get => _data.Money;
            set => _data.Money = value;
        }
        
        public void Initialize()
        {
            LoadData();
        }

        public void Dispose()
        {
            SaveData();
        }
        
        private void SaveData()
        {
            try
            {
                if (File.Exists(FilePath))
                {
                    Debug.Log("Data exists. Deleting old file and writing a new one!");
                    File.Delete(FilePath);
                }
                else
                {
                    Debug.Log("Writing file for the first time!");
                    
                    string directory = Path.GetDirectoryName(FilePath);
                    if (!Directory.Exists(directory))
                    {
                        Directory.CreateDirectory(directory);
                    }
                }

                using FileStream stream = File.Create(FilePath);
                stream.Close();
                File.WriteAllText(FilePath, JsonConvert.SerializeObject(_data));
            }
            catch (Exception e)
            {
                Debug.LogError($"Unable to save data due to: {e.Message} {e.StackTrace}");
            }
        }


        private void LoadData()
        {
            if (!Directory.Exists(Path.GetDirectoryName(FilePath)))
            {
                Debug.Log($"Cannot load file at {FilePath}. Directory does not exist yet");
                return;
            }
            
            if (!File.Exists(FilePath))
            {
                Debug.Log($"Cannot load file at {FilePath}. File does not exist yet");
                return;
            }

            try
            {
                _data = JsonConvert.DeserializeObject<GameData>(File.ReadAllText(FilePath));
            }
            catch (Exception e)
            {
                Debug.LogError($"Failed to load data due to: {e.Message} {e.StackTrace}");
                throw e;
            }
        }
    }
}