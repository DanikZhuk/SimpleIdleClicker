using Gameplay.Estates;
using Gameplay.Estates.Generic;
using Reflex.Core;
using Reflex.Enums;
using UnityEngine;

namespace Core.SceneInstallers
{
    public class GameScopeInstaller : MonoBehaviour, IInstaller
    {
        public void InstallBindings(ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterType(typeof(EstateManager), new[] { typeof(IEstateManager), typeof(EstateManager) }, Lifetime.Singleton, Reflex.Enums.Resolution.Lazy);
        }
    }
}
