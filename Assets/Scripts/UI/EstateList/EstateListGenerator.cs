using UnityEngine;

namespace UI.EstateList
{
    public class EstateListGenerator : MonoBehaviour
    {
        [Header("Файл конфигурации")]
        [SerializeField] private Configs.EstateList config;
        
        [Header("Настройки инстанцирования")]
        [SerializeField] private GameObject estateLinePrefab;
        [SerializeField] private Transform container;

        private void Start()
        {
            UpdateInfo();
        }
    
        private void UpdateInfo()
        {
            foreach (var estate in config.estates)
            {
                var gameObj = Instantiate(estateLinePrefab, container);
                gameObj.GetComponent<EstateLineController>().Config=estate;
            }
        }
    }
}
