using Gameplay.Estates.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI.EstateTab
{
    public class ReviseView : MonoBehaviour
    {
        [SerializeField] private TMP_Text nameText;
        [SerializeField] private Image image;
        [SerializeField] private TMP_Text incomeText;
        [SerializeField] private TMP_Text priceText;
        [SerializeField] private Button sellButton;
        
        [Inject] private EstateManager _estateManager;
        
        private Estate _estate;

        public void Initialize(Sprite icon, Estate estate)
        {
            nameText.text = estate.name;
            image.sprite = icon;
            incomeText.text = $"{estate.Config.Income}$";
            priceText.text = $"{estate.sellPrice}$";
            _estate = estate;
        }
        private void Start()
        {
            sellButton.onClick.AddListener(SellButton_OnClick);
        }

        private void SellButton_OnClick()
        {
            _estateManager.SellEstate(_estate);
            Destroy(gameObject);
        }
    }
}