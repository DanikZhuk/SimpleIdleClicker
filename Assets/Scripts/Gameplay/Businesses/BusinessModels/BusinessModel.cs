using Configs;
using Gameplay.Businesses.Enums;

namespace Gameplay.Businesses.BusinessModels
{
    public class BusinessModel
    {
        public BusinessType BusinessType;
        public string Name;
        public long Income;

        public BusinessModel(BusinessConfig businessConfig, string businessName)
        {
            Name = businessName;
            BusinessType = businessConfig.Type;
            Income = businessConfig.Income;
        }

        public BusinessModel()
        { }
    }
}