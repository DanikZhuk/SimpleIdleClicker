using Gameplay.Estates.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utils.String;
using Zenject;

namespace UI.EstateViews.Default
{
    public class ReviseView : MonoBehaviour
    {
        [SerializeField] private TMP_Text nameText;
        [SerializeField] private Image image;
        [SerializeField] private TMP_Text incomeText;
        [SerializeField] private TMP_Text priceText;
        [SerializeField] private Button sellButton;
        
        [Inject] private EstateManager _estateManager;
        
        protected Estate Estate;

        public virtual void Initialize(Sprite icon, Estate estate, long income)
        {
            if(nameText)
                nameText.text = estate.Name;
            if(image)
                image.sprite = icon;
            if(incomeText)
                incomeText.text = StringCreator.MoneyString(income);
            if(priceText)
                priceText.text = StringCreator.MoneyString(estate.SellPrice);
            Estate = estate;
        }
        protected virtual void Start()
        {
            sellButton.onClick.AddListener(SellButton_OnClick);
        }

        protected virtual void SellButton_OnClick()
        {
            _estateManager.SellEstate(Estate);
            Destroy(gameObject);
        }
    }
}