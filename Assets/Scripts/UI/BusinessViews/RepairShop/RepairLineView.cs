using System;
using Gameplay.Businesses.BusinessControllers;
using Gameplay.Businesses.BusinessModels;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace UI.BusinessViews.RepairShop
{
    public class RepairLineView : MonoBehaviour
    {
        [SerializeField] private Image image;
        [SerializeField] private Button repairButton;
        [SerializeField] private TMP_Text repairTime;
        [SerializeField] private TMP_Text repairCost;
        [SerializeField] private Button sellButton;
        [SerializeField] private TMP_Text sellCost;
        
        public event Action<RepairLineView> OnSellButtonClick;
        public event Action<RepairLineView> OnRenovationButtonClick;
        
        private bool _hasMoneyForRepair;

        public HouseController HouseController { get; private set; }

        private void Start()
        {
            sellButton.onClick.AddListener(() => OnSellButtonClick?.Invoke(this));
            repairButton.onClick.AddListener(() => OnRenovationButtonClick?.Invoke(this));
        }

        private void Update()
        {
            UpdateInfo();
        }

        private void OnDestroy()
        {
            if (HouseController != null)
                HouseController.OnConditionChanged -= OnConditionChanged;
        }

        public void SetHouse(HouseController houseController)
        {
            if (HouseController != null)
                HouseController.OnConditionChanged -= OnConditionChanged;
            HouseController = houseController;
            HouseController.OnConditionChanged += OnConditionChanged;
            UpdateInfo();
            OnConditionChanged();
        }

        public void SetImage(Sprite sprite)
        {
            image.sprite = sprite;
        }

        private void OnConditionChanged()
        {
            repairCost.text = HouseController.RepairCost.MoneyString();
            sellCost.text = HouseController.SellPrice.MoneyString();
            switch (HouseController.Condition)
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

        public void CheckRepairAbility(long money)
        {
            _hasMoneyForRepair = money >= HouseController.RepairCost;
        }

        private void UpdateInfo()
        {
            sellButton.interactable = !HouseController.IsUnderRepair;
            repairButton.interactable = HouseController.IsNeedRepair && _hasMoneyForRepair;

            if (HouseController.IsUnderRepair) repairTime.text = HouseController.RepairProgress.TimeString();
        }
    }
}