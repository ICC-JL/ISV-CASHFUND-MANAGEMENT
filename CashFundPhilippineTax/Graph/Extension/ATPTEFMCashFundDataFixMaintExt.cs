using ATPTPhilippineTax.DAC.Extensions;
using ATPTPhilippineTax.Helpers;
using CashFundManagement.BLC;
using CashFundManagement.Helper;
using PX.Data;
using PX.Objects.AP;

namespace CashFundPhilippineTax.Graph.Extension
{
    /// <remarks>
    /// 2024-08-14 : Adds multi-tenant support. {RRS} <br/>
    /// 2025-02-13:  Apply multi tenant to 24r1 CASE: 010269 {JLTG}
    /// </remarks>
    public class ATPTEFMCashFundDataFixMaintExt : PXGraphExtension<ATPTEFMCashFundDataFixMaint>
    {
#if Version23R2
        public static bool IsActive() => ATPTModule.IsActive;
#else
        public static bool IsActive() => ATPTModule.IsActive && ATPTEFMPrefetchSetup.IsActive;
#endif

        public delegate string getAPTranExpenseReceiptNbrDelegate(APTran invoiceLine);
        [PXOverride]
        public string getAPTranExpenseReceiptNbr(APTran invoiceLine, getAPTranExpenseReceiptNbrDelegate baseMethod)
        {
            ATPTAPTran invoiceLineExt = invoiceLine.GetExtension<ATPTAPTran>();
            if (invoiceLineExt != null)
                return invoiceLineExt.UsrATPTExpenseReceiptNbr;

            return "";
        }
    }
}
