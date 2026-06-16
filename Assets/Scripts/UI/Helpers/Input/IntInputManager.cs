using System;
using TMPro;
using UnityEngine;

public class IntInputManager : MonoBehaviour
{
    [SerializeField] TMP_InputField inputField;
    
    private int _minValue = 0;
    private int _maxValue = 100;
    
    private int _lastValue = 1;

    public void Awake()
    {
        inputField.onSelect.AddListener(Input_OnSelectAndDeselect);
        inputField.onValueChanged.AddListener(Input_OnValueChanged);
        inputField.onDeselect.AddListener(Input_OnSelectAndDeselect);
    }

    public void InitializeInput(int minValue, int maxValue)
    {
        _minValue = minValue;
        _maxValue = maxValue;
        _lastValue = _minValue;
        if (_lastValue > maxValue)
        {
            _lastValue = maxValue;
        }
        inputField.SetTextWithoutNotify(_lastValue.ToString());
    }

    public void ChangeBounds(int minValue, int maxValue)
    {
        _minValue = minValue;
        _maxValue = maxValue;
        if (_lastValue > maxValue)
        {
            _lastValue = maxValue;
            inputField.SetTextWithoutNotify(_lastValue.ToString());
        }
    }

    public int GetValue()
    {
        return _lastValue;
    }

    private void Input_OnValueChanged(string value)
    {
        if (int.TryParse(value, out var newValue))
        {
            if (newValue < _minValue)
            {
                newValue = _minValue;
            }
            else if (newValue > _maxValue)
            {
                newValue = _maxValue;
            }
            inputField.SetTextWithoutNotify(newValue.ToString());
            _lastValue = newValue;
        }
        else
        {
            if(inputField.text == "")
                return;
            inputField.SetTextWithoutNotify(_lastValue.ToString());
        }
    }

    private void Input_OnSelectAndDeselect(string value)
    {
        if(value == "")
            inputField.SetTextWithoutNotify(_minValue.ToString());
    }
}
