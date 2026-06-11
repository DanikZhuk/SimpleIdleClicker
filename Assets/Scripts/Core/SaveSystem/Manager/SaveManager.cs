using System;
using System.Collections.Generic;
using System.IO;
using Core.SaveSystem.Interfaces;
using UnityEngine;

namespace Core.SaveSystem.Services
{
    public class SaveManager: MonoBehaviour
    {
        [SerializeField] private ISavable[] savables;
        private string savePath;

        private void Awake()
        {
            savePath=Path.Combine(Application.persistentDataPath, "save.json");
        }

        public void SaveGame()
        {
            List<string> jsonList = new List<string>();

            foreach (var savable in savables)
            {
                string json = JsonUtility.ToJson(savable);
            }
        }
    }
}