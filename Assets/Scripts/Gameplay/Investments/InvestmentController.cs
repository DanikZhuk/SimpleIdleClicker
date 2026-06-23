using System;
using Configs;
using Gameplay.Services;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace Gameplay.Investments
{
    public class InvestmentController
    {
        [Inject] private MoneyService _moneyService;
        
        public InvestmentModel InvestmentModel { get; }
        public InvestmentConfig InvestmentConfig { get; }
        public event Action OnInvestmentUpdate;

        public long MaxAmountCanBuy
        {
            get => _maxAmountCanBuy;
            private set
            {
                _maxAmountCanBuy = value;
                OnInvestmentUpdate?.Invoke();
            }
        }

        public long Amount
        {
            get => InvestmentModel.PurchasedAmount;
            set
            {
                InvestmentModel.PurchasedAmount = value;
                InvestmentModel.ResumptionTime = InvestmentConfig.ResumptionTime;
                OnInvestmentUpdate?.Invoke();
            }
        }

        public InvestmentController(InvestmentModel investmentModel, InvestmentConfig investmentConfig)
        {
            InvestmentModel = investmentModel;
            InvestmentConfig = investmentConfig;
        }

        public void Setup()
        {
            while (InvestmentModel.History.Count < InvestmentConfig.HistorySize)
            {
                GetNewValue();
                InvestmentModel.History.Add(InvestmentModel.CurrentCost);
            }

            _moneyService.OnMoneyChanged += OnMoneyChanged;
            OnMoneyChanged();
        }

        public void UpdateInvestmentCurrentCost()
        {
            GetNewValue();
            InvestmentModel.History.Add(InvestmentModel.CurrentCost);
            if (InvestmentModel.History.Count <= InvestmentConfig.HistorySize)
                return;
            InvestmentModel.History.RemoveAt(0);

            OnInvestmentUpdate?.Invoke();
        }
        
        public void Update(float deltaTime)
        {
            switch (InvestmentModel.ResumptionTime)
            {
                case > 0:
                    InvestmentModel.ResumptionTime -= deltaTime;
                    break;
                case < 0:
                    InvestmentModel.ResumptionTime = 0f;
                    break;
            }
        }

        public void OnRemove()
        {
            _moneyService.OnMoneyChanged -= OnMoneyChanged;
        }

        private void OnMoneyChanged()
        {
            var amountCanBuy = _moneyService.Money / InvestmentModel.CurrentCost;
            if (amountCanBuy > InvestmentConfig.MaxAmount - Amount)
                amountCanBuy = InvestmentConfig.MaxAmount - Amount;
            MaxAmountCanBuy = amountCanBuy;
        }

        #region CalculateNewValue

        private float _maxAllowedChange;
        private long _maxAmountCanBuy;

        private void GetNewValue()
        {
            var currentStep = CalculateCurrentStep();
            var change = GenerateRandomChange(currentStep);
            _maxAllowedChange = Mathf.Abs(InvestmentConfig.MinCost - InvestmentConfig.MaxCost) *
                                InvestmentConfig.MaxAllowedChangeCoeff;

            var targetMean = (InvestmentConfig.MinCost + InvestmentConfig.MaxCost) / 2f;
            var meanReversion = (targetMean - InvestmentModel.CurrentCost) * InvestmentConfig.MeanReversionStrength;

            change += meanReversion;

            if (InvestmentModel.CurrentCost > InvestmentConfig.MaxCost)
            {
                change = -Mathf.Abs(change);
                InvestmentModel.CurrentCost += (int)change;
                return;
            }

            if (InvestmentModel.CurrentCost < InvestmentConfig.MinCost)
            {
                change = Mathf.Abs(change);
                InvestmentModel.CurrentCost += (int)change;
                return;
            }

            var newValue = InvestmentModel.CurrentCost + (int)change;

            if (newValue < InvestmentConfig.MinCost)
            {
                var delta = InvestmentModel.CurrentCost - InvestmentConfig.MinCost;
                newValue = (int)(InvestmentModel.CurrentCost -
                                 Mathf.Lerp(0, delta, (InvestmentModel.CurrentCost - newValue) / _maxAllowedChange));
            }
            else if (newValue > InvestmentConfig.MaxCost)
            {
                var delta = InvestmentConfig.MaxCost - InvestmentModel.CurrentCost;
                newValue = (int)(InvestmentModel.CurrentCost +
                                 Mathf.Lerp(0, delta, (newValue - InvestmentModel.CurrentCost) / _maxAllowedChange));
            }

            InvestmentModel.CurrentCost = newValue;
        }

        private float GenerateRandomChange(float step)
        {
            var u1 = Random.Range(0.0001f, 1f);
            var u2 = Random.Range(0f, 1f);

            var normalRandom = Mathf.Sqrt(-2.0f * Mathf.Log(u1)) * Mathf.Sin(2.0f * Mathf.PI * u2);

            var change = normalRandom * step;

            return Mathf.Clamp(change, -_maxAllowedChange, _maxAllowedChange);
        }

        private float CalculateCurrentStep()
        {
            var cycleModulation =
                Mathf.Sin(Time.time * InvestmentConfig.NoiseFrequency) * InvestmentConfig.NoiseAmplitude;
            var currentStep = InvestmentConfig.BaseStep + cycleModulation;

            return Mathf.Max(0.1f, currentStep);
        }

        #endregion
    }
}