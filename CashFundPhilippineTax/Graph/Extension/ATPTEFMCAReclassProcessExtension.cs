using ATPTPhilippineTax.Attributes;
using ATPTPhilippineTax.Helpers;
using CashFundManagement.Helper;
using PX.Data;
using PX.Objects.AP;

namespace CashFundPhilippineTax.Graph.Extension 
{
    /// <remarks>
    /// 2024-08-14 : Adds multi-tenant support. {RRS} <br/>
    /// 2025-02-13:  Apply multi tenant to 24r1 CASE: 010269 {JLTG}
    /// </remarks>
    public class ATPTEFMCAReclassProcessExtension : PXGraphExtension<CashFundManagement.BLC.ATPTEFMCAReclassProcess>
    {
#if Version23R2
        public static bool IsActive() => ATPTModule.IsActive;
        
#else
        public static bool IsActive() => ATPTModule.IsActive && ATPTEFMPrefetchSetup.IsActive;
#endif

        public delegate APInvoice DoAdditionalInvoiceProcessDelegate(APInvoice row);
        [PXOverride]
        public APInvoice DoAdditionalInvoiceProcess(APInvoice row, DoAdditionalInvoiceProcessDelegate baseMethod)
        {
            ATPTPhilippineTax.DAC.Extensions.ATPTAPRegister rowExt = row.GetExtension<ATPTPhilippineTax.DAC.Extensions.ATPTAPRegister>();
            rowExt.UsrATPTWHTRecog = ATPTWHTRecognitionAttribute.UponInvoice;
            return row;
        }
    }
}
