using Configs;
using Gameplay.Businesses.Generic.Models;

namespace Gameplay.Businesses.Generic
{
    public interface IBusinessController
    {
        public BusinessModel BusinessModel { get; }
        public BusinessConfig BusinessConfig { get; }
        
        public void Setup(BusinessManager businessManager);
        public void OnMoneyChanged(long money);
        public void OnRemove();
        public void Update(float deltaTime);
    }
}