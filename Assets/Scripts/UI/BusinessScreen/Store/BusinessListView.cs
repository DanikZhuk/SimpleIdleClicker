using Gameplay.Businesses;
using Gameplay.Businesses.BusinessControllers;
using UI.BusinessViews.Default;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.U2D.Animation;
using Zenject;

namespace UI.EstateList.Store
{
    public class BusinessListView : MonoBehaviour
    {
        [FormerlySerializedAs("estateLinePrefab")] [Header("Instance Settings")] [SerializeField]
        private BusinessLineView businessLinePrefab;

        [SerializeField] private Transform lineContainer;
        [SerializeField] private SpriteLibrary library;

        [Header("PopUp Settings")] [SerializeField]
        private PurchaseView purchaseView;

        [SerializeField] private Transform popUpContainer;

        [Inject] BusinessManager _businessManager;

        private const string Category = "Estate";

        private void Start()
        {
            Initialize();
        }

        private void Initialize()
        {
            foreach (var availableBusinessController in _businessManager.AvailableBusinessControllers)
            {
                var line = Instantiate(businessLinePrefab, lineContainer);
                line.Initialize(
                    library.GetSprite(Category,
                        availableBusinessController.BusinessConfig.Type.ToString())
                    , availableBusinessController);
                line.OnClick += ShowPopUp;
            }
        }

        private void ShowPopUp(AvailableBusinessController availableBusinessController)
        {
            Instantiate(purchaseView, popUpContainer)
                .Initialize(
                    library.GetSprite(Category, 
                        availableBusinessController.BusinessConfig.Type.ToString()),
                    availableBusinessController);
        }
    }
}