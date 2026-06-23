using System;
using Gameplay.Businesses.BusinessControllers;
using Gameplay.Businesses.BusinessModels;
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
        
        private HouseController _houseController;

        private bool _hasMoneyForRepair;
        
        public HouseController HouseController=>_houseController;

        public void Initialize(HouseController houseController)
        {
            _houseController = houseController;
            _houseController.OnConditionChanged += OnConditionChanged;
            UpdateInfo();
            OnConditionChanged();
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

        private void OnConditionChanged()
        {
            repairCost.text = StringFormatUtility.MoneyString(_houseController.RepairCost);
            sellCost.text = StringFormatUtility.MoneyString(_houseController.SellPrice);
            switch (_houseController.Condition)
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
        }

        private void Update()
        {
            UpdateInfo();
        }

        public void CheckMoney(long money)
        {
            _hasMoneyForRepair=money>=HouseController.RepairCost;
        }

        private void UpdateInfo()
        {
            sellButton.interactable = !_houseController.IsUnderRepair;
            repairButton.interactable = _houseController.IsNeedRepair&&_hasMoneyForRepair;
            
            if (_houseController.IsUnderRepair)
            {
                repairTime.text = StringFormatUtility.TimeString(_houseController.RepairProgress);
            }
        }

        public void Clear()
        {
            _houseController.OnConditionChanged -= OnConditionChanged;
        }

        private void OnDestroy()
        {
            Clear();
        }
    }
}