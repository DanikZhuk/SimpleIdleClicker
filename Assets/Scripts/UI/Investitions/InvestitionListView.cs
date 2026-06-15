using System;
using System.Collections.Generic;
using Gameplay.Investitions;
using Gameplay.Services;
using UnityEngine;
using Zenject;

namespace UI.Investitions
{
    public class InvestitionListView: MonoBehaviour
    {
        [Inject]  private InvestitionManager _investitionManager;
        [Inject] private TimeService _timeService;
        [SerializeField] private InvestitionView investitionViewPrefab;
        [SerializeField] private Transform viewsContainer;
        private List<InvestitionView> _investitionViews = new();
        
        private void Start()
        {
            InitializeListView();
            _timeService.OnInvestitionUpdate+=UpdateList;
        }

        private void OnDestroy()
        {
            _timeService.OnInvestitionUpdate -= UpdateList;
        }

        private void InitializeListView()
        {
            foreach (var investition in _investitionManager.InvestitionsList)
            {
                var investitionView = Instantiate(investitionViewPrefab, viewsContainer);
                investitionView.Initialize(_investitionManager.GetConfig(investition.Type), investition);
                investitionView.OnBuyButtonClick += _investitionManager.BuyInvestitions;
                investitionView.OnSellButtonClick += _investitionManager.SellInvestitions;
                _investitionViews.Add(investitionView);
            }
        }

        private void UpdateList()
        {
            foreach (var investitionView in _investitionViews)
            {
                investitionView.UpdateValues();
            }
        }
    }
}