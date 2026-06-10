using Configs;
using TMPro;
using UI.EstateTab;
using UnityEngine;
using UnityEngine.UI;

namespace UI.EstateList
{
    public class PurchasedLineController : MonoBehaviour
    {
        [Header("Файл конфигурации для имущества")] [SerializeField]
        private EstateConfig config;

        [Header("Управляемые элементы")] [SerializeField]
        private Image image;
        [SerializeField] private TMP_Text estateName;
        [SerializeField] private TMP_Text income;
        
        private PurchaseTabOpener _tabOpener;

        private PurchaseTabOpener TabOpener
        {
            get
            {
                if(_tabOpener)
                    return _tabOpener;
                return TryGetComponent(out _tabOpener) ? _tabOpener : null;
            }
        }

        public EstateConfig Config
        {
            get => config;
            set
            {
                config = value;
                UpdateInfo();
            }
        }

        private void Start()
        {
            Initialize();
        }

        private void Initialize()
        {
            UpdateInfo();
        }

        private void UpdateInfo()
        {
            if (!config) return;
            if(TabOpener)
                _tabOpener.Config=config;
            
            image.sprite = config.Icon;
            estateName.text = config.EstateName;
            income.text = $"+{config.Income}$";
        }
    }
}