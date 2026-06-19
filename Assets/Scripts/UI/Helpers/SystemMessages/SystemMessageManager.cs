using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UI.Helpers.SystemMessages
{
    public class SystemMessageManager : MonoBehaviour
    {
        [SerializeField] SystemMessageView systemMessageViewPrefab;
        [SerializeField] Transform rectTransform;

        private readonly Queue<string> _messagesQueue = new();
        private SystemMessageView _messageViewInstance;

        private void Awake()
        {
            _messageViewInstance = Instantiate(systemMessageViewPrefab, rectTransform);
        }

        public void Log(string message)
        {
            _messagesQueue.Enqueue(message);
        }

        private void Update()
        {
            if (_messagesQueue.Count <= 0) return;
            if (_messageViewInstance.IsRunning) return;
            var message = _messagesQueue.Dequeue();
            _messageViewInstance.Initialize(message, Color.white);
        }
    }
}