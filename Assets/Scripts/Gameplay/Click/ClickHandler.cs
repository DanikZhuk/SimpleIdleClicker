using UnityEngine;
using UnityEngine.UI;

namespace Gameplay.Click
{
    public class ClickHandler : MonoBehaviour
    {
        [SerializeField] private GameManager.GameManager gameManager;
        [SerializeField] private Button button;

        private void Awake()
        {
            button.onClick.AddListener(OnClick);
        }

        private void OnDestroy()
        {
            button.onClick.RemoveAllListeners();
        }

        private void OnClick()
        {
            gameManager.MoneyService.Earn();
        }
    }
}