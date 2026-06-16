using Gameplay.Estates.Generic;
using UI.EstatePage.EstateViews.Renovation.HouseManager;
using UI.EstateViews.Default;
using UnityEngine;
using UnityEngine.UI;

namespace UI.EstateViews.Renovation
{
    public class RenovationView: ReviseView
    {
        [SerializeField] private Button housesManagementButton;
        [SerializeField] private HouseManagerView houseManagerViewPrefab;
        
        public override void Initialize(Sprite icon, Estate estate, long income)
        {
            base.Initialize(icon, estate, income);
            housesManagementButton.onClick.AddListener(OpenHousesManagement);
        }

        private void OpenHousesManagement()
        {
            Instantiate(houseManagerViewPrefab, transform.parent).Initialize(Estate);
        }
    }
}