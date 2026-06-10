using Gameplay.Services.MoneyService;
using Gameplay.Services.TimeService;
using Reflex.Core;
using Reflex.Enums;
using UnityEngine;
using Resolution = Reflex.Enums.Resolution;

namespace Core
{
    public class RootInstaller : MonoBehaviour, IInstaller
    {
        public void InstallBindings(ContainerBuilder builder)
        {
            builder.RegisterType(typeof(TimeService), new[] { typeof(ITimeService), typeof(TimeService) }, Lifetime.Singleton, Resolution.Lazy);
            builder.RegisterType(typeof(MoneyService), new[] { typeof(IMoneyService),typeof(MoneyService) }, Lifetime.Singleton, Resolution.Lazy);
        }
    }
}