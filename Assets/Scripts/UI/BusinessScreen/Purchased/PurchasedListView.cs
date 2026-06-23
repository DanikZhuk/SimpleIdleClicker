using System;
using System.Collections.Generic;
using Gameplay.Businesses;
using Gameplay.Businesses.BusinessControllers;
using Gameplay.Businesses.Enums;
using UI.BusinessViews.Default;
using UnityEngine;
using UnityEngine.U2D.Animation;
using Zenject;

namespace UI.BusinessScreen.Purchased
{
    public class PurchasedListView : MonoBehaviour
    {
        [Serializable]
        private class CustomView{
            public BusinessType Type;
            public ReviseView View;
        }
        
        [Header("Instance Settings")]
        [SerializeField] private PurchasedLineView businessLinePrefab;
        [SerializeField] private Transform lineContainer;
        [SerializeField] private SpriteLibrary library;

        [Header("PopUp Settings")]
        [SerializeField] private ReviseView defaultReviseView;
        [SerializeField] private CustomView[] customViews;
        [SerializeField] private Transform popUpContainer;

        [Inject] private BusinessManager _businessManager;

        private const string Category = "Estate";

        private readonly List<PurchasedLineView> _lineViews = new();

        private void Start()
        {
            Initialize();
        }

        private void Initialize()
        {
            _businessManager.OnBusinessesChanged += UpdateInfo;
            UpdateInfo();
        }

        private void OnDestroy()
        {
            if (_businessManager != null)
                _businessManager.OnBusinessesChanged -= UpdateInfo;
        }

        private void UpdateInfo()
        {
            int index;
            for (index = 0; index < _businessManager.PurchasedBusinessControllers.Count; index++)
            {
                PurchasedLineView view;
                if (_lineViews.Count > index)
                    view = _lineViews[index];
                else
                {
                    view = Instantiate(businessLinePrefab, lineContainer);
                    _lineViews.Add(view);
                    view.OnClick += ShowPopUp;
                }

                var businessController = _businessManager.PurchasedBusinessControllers[index];
                view.Initialize(library.GetSprite(Category, businessController.Type.ToString()),
                    businessController);
            }

            for (var i = _lineViews.Count - 1; i >= index; i--)
            {
                var view = _lineViews[i];
                view.OnClick -= ShowPopUp;
                _lineViews.Remove(view);
                Destroy(view.gameObject);
            }
        }

        private void ShowPopUp(BusinessController businessController)
        {
            var view = defaultReviseView;
            foreach (var customView in customViews)
            {
                if (customView.Type != businessController.Type) continue;
                view = customView.View;
                break;
            }
            Instantiate(view, popUpContainer)
                .Initialize(
                    library.GetSprite(Category, businessController.Type.ToString()),
                    businessController);
        }
    }
}