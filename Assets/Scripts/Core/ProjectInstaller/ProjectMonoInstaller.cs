using Core.SaveSystem;
using UnityEngine;
using Zenject;

namespace Core.ProjectInstaller
{
    public class ProjectMonoInstaller: MonoInstaller
    {
        [SerializeField] private SaveDataService saveDataServicePrefab;
        public override void InstallBindings()
        {
            Container.Bind<SaveDataService>()
                .FromComponentInNewPrefab(saveDataServicePrefab)
                .AsSingle()
                .NonLazy();
        }
    }
}