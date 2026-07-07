using CashFundManagement.DAC.Setup;
using PX.Data;

namespace CashFundManagement.BLC {
    [PXCacheName(Messages.ATPTEFMMessages.ATPTEFM2023EnhancementsPreference)]
    public class ATPTEFM2023EnhancementsPreference : PXGraph<ATPTEFM2023EnhancementsPreference>
    {
        #region Views
        public PXSelect<ATPTEFM2023R2Enhancements> Preference;

        public PXCancel<ATPTEFM2023R2Enhancements> Cancel;
        public PXSave<ATPTEFM2023R2Enhancements> Save;
        #endregion
    }
}