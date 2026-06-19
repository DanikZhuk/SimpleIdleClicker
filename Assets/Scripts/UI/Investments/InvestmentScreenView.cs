using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Gameplay.Investments;
using Gameplay.Services;
using UnityEngine;
using UnityEngine.Serialization;
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
        [FormerlySerializedAs("investitionLineViewPrefab")]
        [Header("InvestmentModel List  Line")]
        [SerializeField] private InvestmentLineView investmentLineViewPrefab;
        [SerializeField] private Transform viewsContainer;
        [FormerlySerializedAs("investitionViewPrefab")]
        [Header("InvestmentModel View")]
        [SerializeField] private InvestmentPanelView investmentPanelViewPrefab;
        [SerializeField] private Transform popUpContainer;
        [Inject] private InvestmentManager _investmentManager;
        [Inject] private TimeService _timeService;
        
        private const string Category = "Crypt";

        private readonly List<InvestmentLineView> _investmentLineViews = new();

        private InvestmentPanelView _investmentPanelView;

        private void Start()
        {
            InitializeListViews();
            _investmentManager.OnInvestmentsUpdate += OnInvestmentsUpdate;
        }

        private void OnDestroy()
        {
            _investmentManager.OnInvestmentsUpdate -= OnInvestmentsUpdate;
        }

        private void InitializeListViews()
        {
            foreach (var investmentController in _investmentManager.InvestmentControllersList)
            {
                var investmentLineView = Instantiate(investmentLineViewPrefab, viewsContainer);
                investmentLineView.Initialize(investmentController,
                    GetSprite(investmentController.InvestmentConfig.Type));
                investmentLineView.OnClick += InitializeView;
                _investmentLineViews.Add(investmentLineView);
            }
            
            OnInvestmentsUpdate();
        }

        private Sprite GetSprite(InvestmentType type)
        {
            return library.GetSprite(Category, type.ToString());
        }

        private void InitializeView(InvestmentController investmentController)
        {
            _investmentPanelView = Instantiate(investmentPanelViewPrefab, popUpContainer);
            _investmentPanelView.Initialize(investmentController);
            _investmentPanelView.OnBuyButtonClick += _investmentManager.BuyInvestment;
            _investmentPanelView.OnSellButtonClick += _investmentManager.SellInvestment;
        }

        private void OnInvestmentsUpdate()
        {
            if (_investmentPanelView)
            {
                _investmentPanelView.UpdateValues();
            }
            foreach (var investmentLineView in _investmentLineViews)
            {
                investmentLineView.UpdateValues();
            }
        }

        private void Update()
        {
            timeSlider.value = _investmentManager.GetUpdateTimeProgress();
        }
    }
}