using System;
using Core.SaveSystem;
using Gameplay.Estates.Generic;
using Gameplay.Services.MoneyService;
using Gameplay.Services.TimeService;
using Zenject;

namespace Core.Installers
{
    public class GameplaySceneMonoInstaller: MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<IDataService>().To<JsonDataService>().AsSingle();
            Container.Bind(typeof(IEstateManager),typeof(IDisposable)).To<EstateManager>().AsSingle();
            Container.Bind(typeof(IMoneyService),typeof(IDisposable)).To<MoneyService>().AsSingle();
            Container.Bind<ITimeService>().To<TimeService>().AsSingle();
        }
    }
}