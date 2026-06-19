using Gameplay.Businesses.Generic;
using UI.BusinessViews.Default;
using UI.BusinessViews.RepairShop.HouseManager;
using UnityEngine;
using UnityEngine.UI;

namespace UI.BusinessViews.RepairShop
{
    public class RepairView: ReviseView
    {
        [SerializeField] private Button housesManagementButton;
        [SerializeField] private HouseManagerView houseManagerViewPrefab;
        
        public override void Initialize(Sprite icon, IBusinessController businessController)
        {
            base.Initialize(icon, businessController);
            housesManagementButton.onClick.AddListener(OpenHousesManagement);
        }

        private void OpenHousesManagement()
        {
            Instantiate(houseManagerViewPrefab, transform.parent).Initialize(BusinessController);
        }
    }
}