using Configs;
using Reflex.Core;
using UI.TabControls.NewTab;

namespace UI.EstateTab
{
    public class PurchaseTabOpener: TabOpener
    {
        private EstateConfig _config;
        public EstateConfig Config {get=>_config;set=>_config=value;}
        protected override void OpenTab()
        {
            base.OpenTab();
            Tab.GetComponent<PurchaseController>().Config = _config;
        }
    }
}