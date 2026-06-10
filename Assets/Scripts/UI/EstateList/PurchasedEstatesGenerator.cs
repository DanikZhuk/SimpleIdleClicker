using System.Collections.Generic;
using Configs;
using Gameplay.Estates.Generic;
using Reflex.Attributes;
using Unity.VisualScripting;
using UnityEngine;

namespace UI.EstateList
{
    public class PurchasedEstatesGenerator : MonoBehaviour
    {
        [Header("Настройки инстанцирования")] [SerializeField]
        private GameObject estateLinePrefab;

        [SerializeField] private Transform container;

        [Inject] IEstateManager _estateManager;

        private readonly List<PurchasedLineController> _purchasedLineControllers = new();

        private void Start()
        {
            Initialize();
        }

        private void Initialize()
        {
            _estateManager.OnEstatesChanged += UpdateInfo;
            UpdateInfo();
        }

        private void OnDestroy()
        {
            if (_estateManager != null)
                _estateManager.OnEstatesChanged -= UpdateInfo;
        }

        private void UpdateInfo()
        {
            int index;
            for (index = 0; index < _estateManager.Estates.Count; index++)
            {
                PurchasedLineController controller;
                if (_purchasedLineControllers.Count > index)
                    controller = _purchasedLineControllers[index];
                else
                {
                    controller = Instantiate(estateLinePrefab, container).GetComponent<PurchasedLineController>();
                    _purchasedLineControllers.Add(controller);
                }

                var estate = _estateManager.Estates[index];
                var config = Instantiate(estate.Config);
                config.EstateName = estate.name;
                controller.Config = config;
            }

            for (var i = _purchasedLineControllers.Count - 1; i >= index; i--)
            {
                Destroy(_purchasedLineControllers[i].gameObject);
                _purchasedLineControllers.RemoveAt(i);
            }
        }
    }
}