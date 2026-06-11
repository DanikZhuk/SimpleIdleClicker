using System.Collections.Generic;
using System.Linq;
using Configs;
using Gameplay.Estates.Generic;
using UnityEngine;
using UnityEngine.U2D.Animation;
using Zenject;

namespace UI.EstateList
{
    public class PurchasedListView : MonoBehaviour
    {
        [Header("Instance Settings")]
        [SerializeField] private PurchasedLineView estateLinePrefab;
        [SerializeField] private Transform lineContainer;
        [SerializeField] private SpriteLibrary library;
        
        [Header("PopUp Settings")]
        [SerializeField] private EstateViewsConfig viewsConfig;
        [SerializeField] private Transform popUpContainer;
        
        private IEstateManager _estateManager;
        
        private const string Category = "Estate";

        private readonly List<PurchasedLineView> _lineViews = new();

        [Inject]
        private void Construct(IEstateManager estateManager)
        {
            _estateManager = estateManager;
        }

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
                PurchasedLineView view;
                if (_lineViews.Count > index)
                    view = _lineViews[index];
                else
                {
                    view = Instantiate(estateLinePrefab, lineContainer);
                    _lineViews.Add(view);
                    view.OnClick += ShowPopUp;
                }

                var estate = _estateManager.Estates[index];
                view.Initialize(library.GetSprite(Category, estate.Config.Type.ToString()), estate);
            }

            for (var i = _lineViews.Count - 1; i >= index; i--)
            {
                var view = _lineViews[i];
                view.OnClick -= ShowPopUp;
                _lineViews.Remove(view);
                Destroy(view.gameObject);
            }
        }
        
        private void ShowPopUp(Estate estate)
        {
            foreach (var view in viewsConfig.EstateViews.Where(view => view.Type == estate.Config.Type).ToList())
            {
                Instantiate(view.ViewPrefab, popUpContainer).Initialize(library.GetSprite(Category, view.Type.ToString()), estate);
                break;
            }
        }
    }
}