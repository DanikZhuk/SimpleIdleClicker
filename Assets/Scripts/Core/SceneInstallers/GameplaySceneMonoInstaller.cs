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
            Container.Bind<IEstateManager>().To<EstateManager>().AsSingle();
            Container.Bind<IMoneyService>().To<MoneyService>().AsSingle();
            Container.Bind<ITimeService>().To<TimeService>().AsSingle();
        }
    }
}