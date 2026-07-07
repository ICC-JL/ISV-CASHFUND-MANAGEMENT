using ATPTPhilippineTax.Helpers;
using CashFundManagement.Helper;
using PX.Data;
using PX.Objects.EP;

namespace CashFundPhilippineTax.Graph.Extension 
{
    /// <remarks>
    /// 2024-08-14 : Adds multi-tenant support. {RRS} <br/>
    /// 2025-02-13:  Apply multi tenant to 24r1 CASE: 010269 {JLTG}
    /// </remarks>
    public class ATPTEFMFundRequestReclassifyProcessExtension : PXGraphExtension<CashFundManagement.BLC.ATPTEFMFundRequestReclassifyProcess>
    {
#if Version23R2
        public static bool IsActive() => ATPTModule.IsActive;
#else
        public static bool IsActive() => ATPTModule.IsActive && ATPTEFMPrefetchSetup.IsActive;
#endif

        public delegate EPExpenseClaimDetails ClearATCCodeDelegate(EPExpenseClaimDetails row);
        [PXOverride]
        public EPExpenseClaimDetails ClearATCCode(EPExpenseClaimDetails row, ClearATCCodeDelegate baseMethod)
        {
            ATPTPhilippineTax.DAC.Extensions.ATPTEPExpenseClaimDetails rowExt = row.GetExtension<ATPTPhilippineTax.DAC.Extensions.ATPTEPExpenseClaimDetails>();
            rowExt.UsrATPTATCCode = null;
            return row;
        }
    }
}
