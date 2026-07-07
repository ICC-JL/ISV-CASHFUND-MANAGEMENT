using ATPTPhilippineTax.DAC.Extensions;
using ATPTPhilippineTax.Helpers;
using CashFundManagement.DAC.Setup;
using CashFundManagement.Helper;
using PX.Data;
using PX.Objects.EP;

namespace CashFundPhilippineTax.Graph.Extension
{
    /// <remarks>
    /// 2024-08-14 : Adds multi-tenant support. {RRS}
    /// </remarks>
    public class ATPTEFMEPSetupMaintExtension : PXGraphExtension<CashFundManagement.Extensions.BLC.ATPTEFMEPSetupMaintExtension, EPSetupMaint>
    {
#if Version23R2
        public static bool IsActive() => ATPTModule.IsActive;
#else
        public static bool IsActive() => ATPTModule.IsActive && ATPTEFMPrefetchSetup.IsActive;
#endif
        #region Events
        protected virtual void _(Events.FieldUpdated<EPSetup, ATPTEPSetup.usrATPTRequireVendorDetails> e)
        {
            EPSetup row = e.Row;
            ATPTEFMCASetup caPreference = Base1.ATPTEFMCAPreference.Select();
            ATPTEFMSetup fmPreference = Base1.ATPTEFMFMPreference.Select();

            if (row == null) return;
            ATPTEPSetup rowPhiltaxExt = row.GetExtension<ATPTEPSetup>();
            if (rowPhiltaxExt.UsrATPTRequireVendorDetails ?? false)
            {
                if (caPreference != null)
                {
                    PXCache cache = Base.Caches[typeof(ATPTEFMCASetup)];
                    caPreference.RequireVendorDetails = true;
                    cache.Update(caPreference);
                }

                if (fmPreference != null)
                {
                    PXCache fmCache = Base.Caches[typeof(ATPTEFMSetup)];
                    fmPreference.RequireVendorDetails = true;
                    fmCache.Update(fmPreference);
                }
            }
        }
        #endregion
    }

}
