using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Gameplay.Investitions;
using Gameplay.Services;
using UnityEngine;
using UnityEngine.U2D.Animation;
using UnityEngine.UI;
using Zenject;

namespace UI.Investitions
{
    public class InvestitionListView : MonoBehaviour
    {
        [Header("General")]
        [SerializeField] private SpriteLibrary library;
        [SerializeField] private Slider timeSlider;
        [Header("Investition List  Line")]
        [SerializeField] private InvestitionLineView investitionLineViewPrefab;
        [SerializeField] private Transform viewsContainer;
        [Header("Investition View")]
        [SerializeField] private InvestitionView investitionViewPrefab;
        [SerializeField] private Transform popUpContainer;
        [Inject] private InvestitionManager _investitionManager;
        [Inject] private TimeService _timeService;
        
        private const string Category = "Crypt";

        private readonly List<InvestitionLineView> _investitionViews = new();

        private InvestitionView _investitionView;

        private void Start()
        {
            InitializeListViews();
            _timeService.OnUpdate += UpdateList;
            _timeService.OnTickElapsed += OnTick;
        }

        private void OnDestroy()
        {
            _timeService.OnUpdate -= UpdateList;
            _timeService.OnTickElapsed -= OnTick;
        }

        private void InitializeListViews()
        {
            foreach (var investition in _investitionManager.InvestitionsList)
            {
                var investitionLineView = Instantiate(investitionLineViewPrefab, viewsContainer);
                investitionLineView.Initialize(investition,
                    _investitionManager.GetConfig(investition.Type),
                    GetSprite(investition.Type));
                investitionLineView.OnClick += InitializeView;
                _investitionViews.Add(investitionLineView);
            }
        }

        private Sprite GetSprite(InvestitionType type)
        {
            return library.GetSprite(Category, type.ToString());
        }

        private void InitializeView(Investition investition)
        {
            _investitionView = Instantiate(investitionViewPrefab, popUpContainer);
            _investitionView.Initialize(investition, _investitionManager.GetConfig(investition.Type));
            _investitionView.OnBuyButtonClick += _investitionManager.BuyInvestitions;
            _investitionView.OnSellButtonClick += _investitionManager.SellInvestitions;
        }

        private void UpdateList()
        {
            if (_investitionView)
            {
                _investitionView.UpdateValues();
            }
            foreach (var investitionLine in _investitionViews)
            {
                investitionLine.UpdateValues();
            }
        }

        private void OnTick()
        {
            AnimateSlider(_timeService.CurrentUpdateProgress).Forget();
        }

        private async UniTask AnimateSlider(float newValue, float time = 0.3f)
        {
            var oldValue = timeSlider.value;
            var elapsedTime = 0f;
            while (elapsedTime < time)
            {
                elapsedTime+= Time.deltaTime;
                timeSlider.value = Mathf.Lerp(oldValue, newValue, elapsedTime/time);
                await UniTask.Yield();
            }
            timeSlider.value = newValue;
        }
    }
}