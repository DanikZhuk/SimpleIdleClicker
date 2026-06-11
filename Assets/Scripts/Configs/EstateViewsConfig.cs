using System;
using System.Collections.Generic;
using Gameplay.Estates.Generic;
using UI.EstateTab;
using UnityEngine;

namespace Configs
{
    [CreateAssetMenu(fileName = "EstateViewsConfig", menuName = "Configs/Estate Views Config")]
    public class EstateViewsConfig: ScriptableObject
    {
        public List<EstateView> EstateViews;
        
        [Serializable]
        public struct EstateView
        {
            public EstateType Type;
            public PurchaseView PurchasePrefab;
            public ReviseView ViewPrefab;
        }
    }
}