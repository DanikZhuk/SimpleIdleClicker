using Configs;
using UI.BusinessViews.Default;
using UnityEngine;
using UnityEngine.U2D.Animation;

namespace UI.BusinessScreen.Store
{
    public class BusinessListView : MonoBehaviour
    {
        private const string Category = "Business";

        [Header("Config")]
        [SerializeField] private BusinessListConfig businessListConfig;

        [Header("Instance Settings")]
        [SerializeField] private BusinessLineView businessLinePrefab;
        [SerializeField] private RectTransform lineContainer;
        [SerializeField] private SpriteLibrary library;

        [Header("PopUp Settings")]
        [SerializeField] private PurchaseView purchaseView;
        [SerializeField] private RectTransform popUpContainer;

        private void Start()
        {
            Initialize();
        }

        private void Initialize()
        {
            foreach (var businessConfig in businessListConfig.Businesses)
            {
                var line = Instantiate(businessLinePrefab, lineContainer);
                line.Initialize(
                    library.GetSprite(Category,
                        businessConfig.Type.ToString()),
                    businessConfig);
                line.OnClick += ShowPopUp;
            }
        }

        private void ShowPopUp(BusinessConfig businessConfig)
        {
            Instantiate(purchaseView, popUpContainer)
                .Initialize(
                    library.GetSprite(Category,
                        businessConfig.Type.ToString()),
                    businessConfig);
        }
    }
}