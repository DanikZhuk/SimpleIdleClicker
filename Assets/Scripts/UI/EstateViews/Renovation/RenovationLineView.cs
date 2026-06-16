using System;
using TMPro;
using UI.EstatePage.EstateViews.Renovation;
using UI.EstatePage.EstateViews.Renovation.Model;
using UnityEngine;
using UnityEngine.UI;
using Utils.String;

namespace UI.EstateViews.Renovation
{
    public class RenovationLineView: MonoBehaviour
    {
        [SerializeField] private Image image;
        [SerializeField] private Button renovateButton;
        [SerializeField] private TMP_Text renovationTime;
        [SerializeField] private TMP_Text renovationCost;
        [SerializeField] private Button sellButton;
        [SerializeField] private TMP_Text sellCost;
        
        public event Action<RenovationLineView> OnSellButtonClick;
        public event Action<RenovationLineView> OnRenovationButtonClick;
        
        private House _house;
        
        public House House=>_house;

        public void Initialize(House house)
        {
            _house = house;
            UpdateInfo();
        }

        public void SetImage(Sprite sprite)
        {
            image.sprite = sprite;
        }
        
        private void Start()
        {
            sellButton.onClick.AddListener(() => OnSellButtonClick?.Invoke(this));
            renovateButton.onClick.AddListener(() => OnRenovationButtonClick?.Invoke(this));
        }

        private void UpdateInfo()
        {
            renovationCost.text = StringCreator.MoneyString(_house.RenovationCost);
            sellCost.text = StringCreator.MoneyString(_house.Cost);
            switch (_house.HouseType)
            {
                case HouseType.Broken:
                    renovationTime.gameObject.SetActive(false);
                    
                    renovateButton.gameObject.SetActive(true);
                    renovateButton.interactable = true;
                    renovationCost.gameObject.SetActive(true);
                    
                    sellButton.interactable = true;
                    break;
                case HouseType.Renovating:
                    renovationTime.gameObject.SetActive(true);
                    
                    renovateButton.gameObject.SetActive(true);
                    renovateButton.interactable = false;
                    renovationCost.gameObject.SetActive(true);
                    
                    sellButton.interactable = false;
                    break;
                case HouseType.Renovated:
                    renovationTime.gameObject.SetActive(false);
                    
                    renovateButton.gameObject.SetActive(false);
                    renovationCost.gameObject.SetActive(false);
                    
                    sellButton.interactable = true;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void Update()
        {
            UpdateTime();
        }

        private void UpdateTime()
        {
            if (_house.HouseType == HouseType.Renovating)
            {
                renovationTime.text = StringCreator.TimeString(_house.RenovatingTime);
            }
        }
    }
}