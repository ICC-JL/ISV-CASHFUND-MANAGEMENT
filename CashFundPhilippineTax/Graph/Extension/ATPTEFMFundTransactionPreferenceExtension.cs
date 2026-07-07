using ATPTPhilippineTax.DAC.Extensions;
using ATPTPhilippineTax.Helpers;
using CashFundManagement.BLC;
using CashFundManagement.Helper;
using PX.Data;
using PX.Objects.EP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashFundPhilippineTax.Graph.Extension
{
    public class ATPTEFMFundTransactionPreferenceExtension : PXGraphExtension<ATPTEFMFundTransactionPreference>
    {
#if Version23R2
        public static bool IsActive() => ATPTModule.IsActive;
#else
        public static bool IsActive() => ATPTModule.IsActive && ATPTEFMPrefetchSetup.IsActive;
#endif

        #region Override
        public delegate bool IsTERequireVendorDetailsDelegate(EPSetup te);
        [PXOverride]
        public bool IsTERequireVendorDetails(EPSetup te, IsTERequireVendorDetailsDelegate baseMethod)
        {
            ATPTEPSetup teExt = te.GetExtension<ATPTEPSetup>();
            return teExt.UsrATPTRequireVendorDetails ?? false;
        }
        #endregion
    }
}
