using System;
using Gameplay.Businesses.BusinessControllers.RepairShop.House.Controller;
using Gameplay.Businesses.BusinessControllers.RepairShop.House.Model;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace UI.BusinessViews.RepairShop
{
    public class RepairLineView: MonoBehaviour
    {
        [SerializeField] private Image image;
        [SerializeField] private Button repairButton;
        [SerializeField] private TMP_Text repairTime;
        [SerializeField] private TMP_Text repairCost;
        [SerializeField] private Button sellButton;
        [SerializeField] private TMP_Text sellCost;
        
        public event Action<RepairLineView> OnSellButtonClick;
        public event Action<RepairLineView> OnRenovationButtonClick;
        
        private PurchasedHouseController _houseController;
        
        public PurchasedHouseController HouseController=>_houseController;

        public void Initialize(PurchasedHouseController houseController)
        {
            _houseController = houseController;
            _houseController.OnStatusChanged += UpdateInfo;
            UpdateInfo();
            UpdateButtons();
        }

        public void SetImage(Sprite sprite)
        {
            image.sprite = sprite;
        }
        
        private void Start()
        {
            sellButton.onClick.AddListener(() => OnSellButtonClick?.Invoke(this));
            repairButton.onClick.AddListener(() => OnRenovationButtonClick?.Invoke(this));
        }

        private void UpdateInfo()
        {
            repairCost.text = StringFormatUtility.MoneyString(_houseController.HouseModel.RepairCost);
            sellCost.text = StringFormatUtility.MoneyString(_houseController.HouseModel.Cost);
            switch (_houseController.HouseModel.Condition)
            {
                case HouseCondition.NeedRepair:
                    repairTime.gameObject.SetActive(false);
                    
                    repairButton.gameObject.SetActive(true);
                    repairCost.gameObject.SetActive(true);
                    break;
                case HouseCondition.UnderRepair:
                    repairTime.gameObject.SetActive(true);
                    
                    repairButton.gameObject.SetActive(true);
                    repairCost.gameObject.SetActive(false);
                    break;
                case HouseCondition.FullyRepaired:
                    repairTime.gameObject.SetActive(false);
                    
                    repairButton.gameObject.SetActive(false);
                    repairCost.gameObject.SetActive(false);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            UpdateButtons();
        }
        
        private void UpdateButtons()
        {
            sellButton.interactable = _houseController.CanSell;
            repairButton.interactable = _houseController.CanRepair;
        }

        private void Update()
        {
            UpdateTime();
        }

        private void UpdateTime()
        {
            if (_houseController.HouseModel.Condition == HouseCondition.UnderRepair)
            {
                repairTime.text = StringFormatUtility.TimeString(_houseController.HouseModel.RepairTimeSeconds);
            }
        }

        public void Clear()
        {
            _houseController.OnStatusChanged -= UpdateInfo;
        }

        private void OnDestroy()
        {
            Clear();
        }
    }
}