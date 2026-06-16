using Configs;
using UnityEngine;

namespace Gameplay.Investitions
{
    public class InvestitionCalculator
    {
        private float _maxAllowedChange;

        public void InitializeValues(Investition investition, InvestitionConfig config, int historySize)
        {
            while (investition.History.Count < historySize)
            {
                GetNewValue(investition, config);
                investition.History.Add(investition.CurrentCost);
            }
        }
        
        public void UpdateInvestition(Investition investition, InvestitionConfig config, int historySize)
        {
            GetNewValue(investition, config);
            investition.History.Add(investition.CurrentCost);
            if (investition.History.Count <= historySize)
                return;
            investition.History.RemoveAt(0);
        }
        
        private void GetNewValue(Investition investition, InvestitionConfig config)
        {
            float currentStep = CalculateCurrentStep(config);
            float change = GenerateRandomChange(currentStep);
            _maxAllowedChange = Mathf.Abs(config.MinCost - config.MaxCost) * config.MaxAllowedChangeCoeff;

            float targetMean = (config.MinCost + config.MaxCost) / 2f;
            float meanReversion = (targetMean - investition.CurrentCost) * config.MeanReversionStrength;

            change += meanReversion;

            if (investition.CurrentCost > config.MaxCost)
            {
                change = -Mathf.Abs(change);
                investition.CurrentCost += (int)change;
                return;
            }

            if (investition.CurrentCost < config.MinCost)
            {
                change = Mathf.Abs(change);
                investition.CurrentCost += (int)change;
                return;
            }

            var newValue = investition.CurrentCost + (int)change;

            if (newValue < config.MinCost)
            {
                var delta = investition.CurrentCost - config.MinCost;
                newValue = (int)(investition.CurrentCost - Mathf.Lerp(0, delta, (investition.CurrentCost - newValue) / _maxAllowedChange));
            }
            else if (newValue > config.MaxCost)
            {
                var delta = config.MaxCost - investition.CurrentCost;
                newValue = (int)(investition.CurrentCost + Mathf.Lerp(0, delta, (newValue - investition.CurrentCost) / _maxAllowedChange));
            }

            investition.CurrentCost = newValue;
        }

        private float GenerateRandomChange(float step)
        {
            var u1 = Random.Range(0.0001f, 1f);
            var u2 = Random.Range(0f, 1f);

            var normalRandom = Mathf.Sqrt(-2.0f * Mathf.Log(u1)) * Mathf.Sin(2.0f * Mathf.PI * u2);

            var change = normalRandom * step;
            
            return Mathf.Clamp(change, -_maxAllowedChange, _maxAllowedChange);
        }

        private float CalculateCurrentStep(InvestitionConfig config)
        {
            float cycleModulation = Mathf.Sin(Time.time * config.NoiseFrequency) * config.NoiseAmplitude;
            float currentStep = config.BaseStep + cycleModulation;

            return Mathf.Max(0.1f, currentStep);
        }
    }
}