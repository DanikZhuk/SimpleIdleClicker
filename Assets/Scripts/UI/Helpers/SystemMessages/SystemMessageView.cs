using System;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace UI.Helpers.SystemMessages
{
    public class SystemMessageView : MonoBehaviour
    {
        [SerializeField] private RectTransform rectTransform;
        [SerializeField] private TMP_Text messageText;
        [SerializeField] private CanvasGroup canvasGroup;
        [SerializeField] private float slidingTime;
        [SerializeField] private float overallTime;
        [SerializeField] private Vector2 initialOffset;
        [SerializeField] private Vector2 offset;
        
        public bool IsRunning {get; private set;}

        private void Awake()
        {
            rectTransform.anchoredPosition = initialOffset;
        }

        public void Initialize(string message, Color textColor)
        {
            messageText.text = message;
            messageText.color = textColor;
            Moving().Forget();
        }

        private async UniTask Moving()
        {
            IsRunning = true;
            var delay = overallTime - slidingTime * 2f;

            rectTransform.anchoredPosition = initialOffset;
            canvasGroup.alpha = 0f;

            var time = 0f;
            while (time < slidingTime)
            {
                time += Time.deltaTime;
                var progress = time / slidingTime;
                var easedProgress = EaseInOut(progress);

                rectTransform.anchoredPosition = Vector2.Lerp(initialOffset, offset, easedProgress);
                canvasGroup.alpha = Mathf.Lerp(0f, 1f, easedProgress);

                await UniTask.Yield();
            }

            rectTransform.anchoredPosition = offset;
            canvasGroup.alpha = 1f;

            await UniTask.Delay(TimeSpan.FromSeconds(delay));

            time = 0f;
            while (time < slidingTime)
            {
                time += Time.deltaTime;
                var progress = time / slidingTime;
                var easedProgress = EaseInOut(progress);

                rectTransform.anchoredPosition = Vector2.Lerp(offset, initialOffset, easedProgress);
                canvasGroup.alpha = Mathf.Lerp(1f, 0f, easedProgress);

                await UniTask.Yield();
            }

            rectTransform.anchoredPosition = initialOffset;
            canvasGroup.alpha = 0f;
            IsRunning = false;
        }

        private float EaseInOut(float t)
        {
            return t < 0.5f ? 2f * t * t : 1f - Mathf.Pow(-2f * t + 2f, 2f) / 2f;
        }
    }
}