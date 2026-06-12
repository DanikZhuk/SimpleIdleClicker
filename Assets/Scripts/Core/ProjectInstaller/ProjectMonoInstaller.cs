using Core.SaveSystem;
using Zenject;

namespace Core.ProjectInstaller
{
    public class ProjectMonoInstaller: MonoInstaller
    {
        public override void InstallBindings()
        {
            Container
                .BindInterfacesAndSelfTo<JsonDataService>()
                .AsSingle()
                .NonLazy();
        }
    }
}