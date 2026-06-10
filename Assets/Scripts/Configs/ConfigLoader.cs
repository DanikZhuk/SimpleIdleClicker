using System;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Configs
{
    public class ConfigLoader
    {
        public static async UniTask<T> LoadInternalAsync<T>(string path) where T : ScriptableObject
        {
            var request = Resources.LoadAsync<T>(path);
            await request.ToUniTask();
            var result = request.asset as T;
            return result == null ? throw new Exception($"Failed to load ScriptableObject at path: {path}") : result;
        }
    }
}