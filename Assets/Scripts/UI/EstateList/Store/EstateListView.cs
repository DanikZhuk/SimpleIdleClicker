using System.Linq;
using Configs;
using Gameplay.Estates.Generic;
using UnityEngine;
using UnityEngine.U2D.Animation;

namespace UI.EstateList.Store
{
    public class EstateListView : MonoBehaviour
    {
        [Header("Estate List Config")]
        [SerializeField] private Configs.EstateList config;
        
        [Header("Instance Settings")]
        [SerializeField] private EstateLineView estateLinePrefab;
        [SerializeField] private Transform lineContainer;
        [SerializeField] private SpriteLibrary library;
        
        [Header("PopUp Settings")]
        [SerializeField] private EstateViewsConfig viewsConfig;
        [SerializeField] private Transform popUpContainer;

        private const string Category = "Estate";

        private void Start()
        {
            Initialize();
        }

        private void Initialize()
        {
            foreach (var estate in config.estates)
            {
                var line = Instantiate(estateLinePrefab, lineContainer);
                line.Initialize(library.GetSprite(Category, estate.Type.ToString()), estate);
                line.OnClick += ShowPopUp;
            }
        }

        private void ShowPopUp(EstateType type)
        {
            foreach (var view in viewsConfig.EstateViews.Where(view => view.Type == type))
            {
                Instantiate(view.PurchasePrefab, popUpContainer)
                    .Initialize(library.GetSprite(Category, type.ToString()),
                        config.estates.Find(estate=>estate.Type==type));
                break;
            }
        }
    }
}