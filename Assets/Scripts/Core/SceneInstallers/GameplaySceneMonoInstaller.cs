using Gameplay.Estates.Generic;
using Gameplay.Services;
using UnityEngine;
using Zenject;

namespace Core.SceneInstallers
{
    public class GameplaySceneMonoInstaller: MonoInstaller
    {
        [SerializeField] private Transform gameManager;
        public override void InstallBindings()
        {
            var em = gameManager.GetComponent<EstateManager>();
            var ms = gameManager.GetComponent<MoneyService>();
            var ts = gameManager.GetComponent<TimeService>();
            
            Container.BindInterfacesAndSelfTo<EstateManager>().FromInstance(em).AsSingle();
            Container.BindInterfacesAndSelfTo<MoneyService>().FromInstance(ms).AsSingle();
            Container.BindInterfacesAndSelfTo<TimeService>().FromInstance(ts).AsSingle();
            Container.BindInterfacesAndSelfTo<OfflinePaymentService>().AsSingle();
        }
    }
}