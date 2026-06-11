using Configs;
using UnityEngine;
using Zenject;

[CreateAssetMenu(fileName = "GameplaySettingsInstaller", menuName = "Installers/GameplaySettingsInstaller")]
public class GameplaySettingsInstaller : ScriptableObjectInstaller<GameplaySettingsInstaller>
{
    public TimeConfig timeConfig;
    public EconomyConfig economyConfig;
    public override void InstallBindings()
    {
        Container.BindInstances(timeConfig, economyConfig);
    }
}