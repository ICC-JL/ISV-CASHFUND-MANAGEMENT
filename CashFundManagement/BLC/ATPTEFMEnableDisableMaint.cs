using CashFundManagement.DAC.Setup;
using PX.Caching;
using PX.Data;

namespace CashFundManagement.BLC {
    public class ATPTEFMEnableDisableMaint : PXGraph<ATPTEFMEnableDisableMaint>
    {
        public PXSave<ATPTEFMEnableDisable> Save;
        public PXCancel<ATPTEFMEnableDisable> Cancel;
        public PXSelect<ATPTEFMEnableDisable> Preferences;

        // Acuminator disable once PX1076 CallToInternalApi [Use in PXDatabase.GetSlot]
        [InjectDependency]
        protected ICacheControl<PageCache> PageCacheControl { get; set; }

        public override void Persist()
        {
            base.Persist();
            PXDatabase.ResetSlots();
            // Acuminator disable once PX1076 CallToInternalApi [Use in PXDatabase.GetSlot]
            PageCacheControl.InvalidateCache();
        }
    }
}