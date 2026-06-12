using System.Linq;
using Configs;
using Gameplay.Estates.Generic;
using UI.EstateTab;
using UnityEngine;
using UnityEngine.U2D.Animation;

namespace UI.EstateList.Store
{
    public class EstateListView : MonoBehaviour
    {
        [Header("Estate List Config")] [SerializeField]
        private Configs.EstateList config;

        [Header("Instance Settings")] [SerializeField]
        private EstateLineView estateLinePrefab;

        [SerializeField] private Transform lineContainer;
        [SerializeField] private SpriteLibrary library;

        [Header("PopUp Settings")] [SerializeField]
        private PurchaseView purchaseView;

        [SerializeField] private Transform popUpContainer;

        private const string Category = "Estate";

        private void Start()
        {
            Initialize();
        }

        private void Initialize()
        {
            foreach (var estate in config.Estates)
            {
                var line = Instantiate(estateLinePrefab, lineContainer);
                line.Initialize(library.GetSprite(Category, estate.Type.ToString()), estate);
                line.OnClick += ShowPopUp;
            }
        }

        private void ShowPopUp(EstateType type)
        {
            Instantiate(purchaseView, popUpContainer)
                .Initialize(library.GetSprite(Category, type.ToString()),
                    config.Estates.Find(estate => estate.Type == type));
        }
    }
}