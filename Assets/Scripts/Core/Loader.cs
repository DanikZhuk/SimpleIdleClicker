using Reflex.Core;
using UnityEngine;

namespace Core
{
    public class Loader : MonoBehaviour
    {
        private void Start()
        {
            void InstallExtra(UnityEngine.SceneManagement.Scene scene, ContainerBuilder builder)
            {
                builder.RegisterValue("of Developers");
            }
        
            // This way you can access ContainerBuilder of the scene that is currently building
            ContainerScope.OnSceneContainerBuilding += InstallExtra;

            // If you are loading scenes without addressables
            var loading =
                UnityEngine.SceneManagement.SceneManager.LoadSceneAsync("Game");
            if (loading != null)
            {
                loading.completed += _ => { ContainerScope.OnSceneContainerBuilding -= InstallExtra; };
            }
        }
    }
}
