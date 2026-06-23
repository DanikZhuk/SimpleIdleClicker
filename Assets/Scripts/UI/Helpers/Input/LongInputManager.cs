using TMPro;
using UnityEngine;

namespace UI.Helpers.Input
{
    public class LongInputManager : MonoBehaviour
    {
        [SerializeField] private TMP_InputField inputField;

        private long _lastValue = 1;
        private long _maxValue = 100;

        private long _minValue;

        public long Value
        {
            get => _lastValue;
            set
            {
                _lastValue = value;
                if (_lastValue > _maxValue) _lastValue = _maxValue;

                inputField.SetTextWithoutNotify(_lastValue.ToString());
            }
        }

        public void Awake()
        {
            inputField.onSelect.AddListener(Input_OnSelectAndDeselect);
            inputField.onValueChanged.AddListener(Input_OnValueChanged);
            inputField.onDeselect.AddListener(Input_OnSelectAndDeselect);
        }

        public void InitializeInput(long minValue, long maxValue)
        {
            _minValue = minValue;
            _maxValue = maxValue;
            _lastValue = _minValue;
            if (_lastValue > maxValue) _lastValue = maxValue;

            inputField.SetTextWithoutNotify(_lastValue.ToString());
        }

        public void ChangeBounds(long minValue, long maxValue)
        {
            _minValue = minValue;
            _maxValue = maxValue;
            if (_lastValue <= maxValue) return;
            _lastValue = maxValue;
            inputField.SetTextWithoutNotify(_lastValue.ToString());
        }

        private void Input_OnValueChanged(string value)
        {
            if (long.TryParse(value, out var newValue))
            {
                if (newValue < _minValue) newValue = _minValue;

                if (newValue > _maxValue) newValue = _maxValue;

                inputField.SetTextWithoutNotify(newValue.ToString());
                _lastValue = newValue;
            }
            else
            {
                if (inputField.text == string.Empty)
                    return;
                inputField.SetTextWithoutNotify(_lastValue.ToString());
            }
        }

        private void Input_OnSelectAndDeselect(string value)
        {
            if (value == string.Empty)
                inputField.SetTextWithoutNotify(_minValue.ToString());
        }
    }
}