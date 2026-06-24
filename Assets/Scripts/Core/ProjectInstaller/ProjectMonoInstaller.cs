using Core.SaveSystem;
using Gameplay.Services;
using UI.Helpers.SystemMessages;
using UnityEngine;
using Zenject;

namespace Core.ProjectInstaller
{
    public class ProjectMonoInstaller : MonoInstaller
    {
        [SerializeField] private TimeService timeServicePrefab;
        [SerializeField] private SaveDataService saveDataServicePrefab;
        [SerializeField] private MoneyService moneyServicePrefab;
        [SerializeField] private SystemMessageManager systemMessageManagerPrefab;

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
            Container.Bind<MoneyService>()
                .FromComponentInNewPrefab(moneyServicePrefab)
                .AsSingle()
                .NonLazy();
            Container.Bind<SystemMessageManager>()
                .FromComponentInNewPrefab(systemMessageManagerPrefab)
                .AsSingle()
                .NonLazy();
        }
    }
}