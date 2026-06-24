using System.Collections.Generic;
using Gameplay.Investments;
using Gameplay.Services;
using UnityEngine;
using UnityEngine.U2D.Animation;
using UnityEngine.UI;
using Zenject;

namespace UI.Investments
{
    public class InvestmentScreenView : MonoBehaviour
    {
        [Header("General")]
        [SerializeField] private SpriteLibrary library;
        [SerializeField] private Slider timeSlider;
        
        [Header("InvestmentModel List Line")]
        [SerializeField] private InvestmentLineView investmentLineViewPrefab;
        [SerializeField] private RectTransform viewsContainer;
        
        [Header("InvestmentModel View")]
        [SerializeField] private InvestmentPanelView investmentPanelViewPrefab;
        [SerializeField] private RectTransform popUpContainer;

        [Inject] private InvestmentManager _investmentManager;
        [Inject] private TimeService _timeService;
        
        private const string Category = "Investment";
        
        private readonly List<InvestmentLineView> _investmentLineViews = new();
        private InvestmentPanelView _investmentPanelView;

        private void Start()
        {
            InitializeListViews();
            _investmentManager.OnInvestmentsCostUpdate += OnInvestmentsCostUpdate;
        }

        private void Update()
        {
            timeSlider.value = _investmentManager.GetUpdateTimeProgress();
        }

        private void OnDestroy()
        {
            _investmentManager.OnInvestmentsCostUpdate -= OnInvestmentsCostUpdate;
        }

        private void InitializeListViews()
        {
            foreach (var investmentController in _investmentManager.InvestmentControllersList)
            {
                var investmentLineView = Instantiate(investmentLineViewPrefab, viewsContainer);

                var investmentSprite = GetSprite(investmentController.InvestmentConfig.Type);
                
                investmentLineView.Initialize(investmentController, investmentSprite);
                investmentLineView.OnClick += InitializeView;
                _investmentLineViews.Add(investmentLineView);
            }

            OnInvestmentsCostUpdate();
        }

        private Sprite GetSprite(InvestmentType type)
        {
            return library.GetSprite(Category, type.ToString());
        }

        private void InitializeView(InvestmentController investmentController)
        {
            _investmentPanelView = Instantiate(investmentPanelViewPrefab, popUpContainer);
            _investmentPanelView.Initialize(investmentController);
            _investmentPanelView.OnBuyButtonClick += OnBuyButtonClick;
            _investmentPanelView.OnSellButtonClick += OnSellButtonClick;
        }

        private void OnInvestmentsCostUpdate()
        {
            if (_investmentPanelView)
                _investmentPanelView.OnInvestmentsCostUpdate();
            foreach (var investmentLineView in _investmentLineViews)
                investmentLineView.OnInvestmentsCostUpdate();
        }

        private void OnBuyButtonClick(InvestmentController investmentController, long amount)
        {
            _investmentManager.BuyInvestment(investmentController, amount);
        }
        
        private void OnSellButtonClick(InvestmentController investmentController, long amount)
        {
            _investmentManager.SellInvestment(investmentController, amount);
        }
    }
}