using Gameplay.Estates.Generic;
using Gameplay.Investitions;
using Gameplay.Services;
using UnityEngine;
using Zenject;

namespace Core.SceneInstallers
{
    public class GameplaySceneMonoInstaller: MonoInstaller
    {
        [SerializeField] private EstateManager em;
        [SerializeField] private InvestitionManager im;
        [SerializeField] private MoneyService ms;
        [SerializeField] private TimeService ts;
        public override void InstallBindings()
        {
            Container
                .BindInterfacesAndSelfTo<EstateManager>()
                .FromInstance(em)
                .AsSingle();
            Container
                .BindInterfacesAndSelfTo<InvestitionManager>()
                .FromInstance(im)
                .AsSingle();
            Container
                .BindInterfacesAndSelfTo<MoneyService>()
                .FromInstance(ms)
                .AsSingle();
            Container
                .BindInterfacesAndSelfTo<TimeService>()
                .FromInstance(ts)
                .AsSingle();
            Container
                .BindInterfacesAndSelfTo<OfflinePaymentService>()
                .AsSingle();
        }
    }
}