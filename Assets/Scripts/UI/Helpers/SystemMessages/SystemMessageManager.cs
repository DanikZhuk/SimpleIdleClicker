using System;
using System.Collections.Generic;
using UI.WarningViews;
using UI.WarningViews.TimeWarning;
using UnityEngine;

namespace UI.Helpers.SystemMessages
{
    public class SystemMessageManager : MonoBehaviour
    {
        [SerializeField] private SystemMessageView systemMessageViewPrefab;
        [SerializeField] private TimeWarningView  timeWarningViewPrefab;

        private RectTransform _rectTransform;
        
        private readonly Queue<string> _messagesQueue = new();
        private SystemMessageView _messageViewInstance;

        private void Awake()
        {
            _rectTransform = FindAnyObjectByType<Canvas>().transform as RectTransform;
            _messageViewInstance = Instantiate(systemMessageViewPrefab, _rectTransform);
        }

        private void Update()
        {
            if (_messagesQueue.Count <= 0) return;
            if (_messageViewInstance.IsRunning) return;
            var message = _messagesQueue.Dequeue();
            _messageViewInstance.Initialize(message, Color.white);
        }

        public void Log(string message)
        {
            _messagesQueue.Enqueue(message);
        }
    }
}