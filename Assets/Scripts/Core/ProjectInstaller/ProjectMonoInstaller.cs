using Core.SaveSystem;
using Gameplay.Services;
using UnityEngine;
using Zenject;

namespace Core.ProjectInstaller
{
    public class ProjectMonoInstaller : MonoInstaller
    {
        [SerializeField] private TimeService timeServicePrefab;
        [SerializeField] private SaveDataService saveDataServicePrefab;

        public override void InstallBindings()
        {
            Container.Bind<TimeService>()
                .FromComponentInNewPrefab(timeServicePrefab)
                .AsSingle()
                .NonLazy();
            Container.Bind<SaveDataService>()
                .FromComponentInNewPrefab(saveDataServicePrefab)
                .AsSingle()
                .NonLazy();
        }
    }
}