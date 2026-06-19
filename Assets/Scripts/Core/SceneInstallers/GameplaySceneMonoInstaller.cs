using Gameplay.Businesses;
using Gameplay.Investitions;
using Gameplay.Services;
using UI.Helpers.SystemMessages;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

namespace Core.SceneInstallers
{
    public class GameplaySceneMonoInstaller: MonoInstaller
    {
        [FormerlySerializedAs("estateManager")] [SerializeField] private BusinessManager businessManager;
        [SerializeField] private InvestitionManager investitionManager;
        [SerializeField] private MoneyService moneyService;
        [SerializeField] private TimeService timeService;
        [SerializeField] private SystemMessageManager systemMessageManager;
        public override void InstallBindings()
        {
            Container
                .BindInterfacesAndSelfTo<BusinessManager>()
                .FromInstance(businessManager)
                .AsSingle();
            Container
                .BindInterfacesAndSelfTo<InvestitionManager>()
                .FromInstance(investitionManager)
                .AsSingle();
            Container
                .BindInterfacesAndSelfTo<MoneyService>()
                .FromInstance(moneyService)
                .AsSingle();
            Container
                .BindInterfacesAndSelfTo<TimeService>()
                .FromInstance(timeService)
                .AsSingle();
            Container
                .BindInterfacesAndSelfTo<OfflinePaymentService>()
                .AsSingle();
            Container
                .BindInterfacesAndSelfTo<SystemMessageManager>()
                .FromInstance(systemMessageManager)
                .AsSingle();
        }
    }
}