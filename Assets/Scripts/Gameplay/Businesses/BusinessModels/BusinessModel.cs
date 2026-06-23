using Configs;
using Gameplay.Businesses.Enums;

namespace Gameplay.Businesses.Generic.Models
{
    public class BusinessModel
    {
        public string Id;
        public BusinessType BusinessType;
        public string Name;
        public long Income;

        private static long _nextId = 0;

        public BusinessModel(BusinessConfig businessConfig, string businessName)
        {
            Id = _nextId++.ToString();
            Name = businessName;
            BusinessType = businessConfig.Type;
            Income = businessConfig.Income;
        }

        public BusinessModel()
        { }
    }
}