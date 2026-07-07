using ATPTPhilippineTax.Attributes;
using ATPTPhilippineTax.Helpers;
using CashFundManagement.Helper;
using PX.Data;
using PX.Objects.AP;
using PX.Objects.EP;

namespace CashFundPhilippineTax.Graph.Extension 
{
    /// <remarks>
    /// 2024-08-14 : Adds multi-tenant support. {RRS} <br/>
    /// 2025-02-13:  Apply multi tenant to 24r1 CASE: 010269 {JLTG}
    /// </remarks>
    public class ATPTEFMReplenishmentEntry : PXGraphExtension<CashFundManagement.BLC.ATPTEFMReplenishmentEntry>
    {
#if Version23R2
        public static bool IsActive() => ATPTModule.IsActive;
#else
        public static bool IsActive() => ATPTModule.IsActive && ATPTEFMPrefetchSetup.IsActive;
#endif

        public delegate APTran DoAdditionalReleaseDelegate(APTran row, EPExpenseClaimDetails er);
        [PXOverride]
        public APTran DoAdditionalRelease(APTran row, EPExpenseClaimDetails er, DoAdditionalReleaseDelegate baseMethod)
        {
            ATPTPhilippineTax.DAC.Extensions.ATPTAPTran rowExt = row.GetExtension<ATPTPhilippineTax.DAC.Extensions.ATPTAPTran>();
            ATPTPhilippineTax.DAC.Extensions.ATPTEPExpenseClaimDetails erExt = er.GetExtension<ATPTPhilippineTax.DAC.Extensions.ATPTEPExpenseClaimDetails>();
            CashFundManagement.Extensions.DAC.ATPTEFMEPExpenseClaimDetailsExt cfmExt = er.GetExtension<CashFundManagement.Extensions.DAC.ATPTEFMEPExpenseClaimDetailsExt>();

            rowExt.UsrATPTVendID = erExt.UsrATPTVendID;
            rowExt.UsrATPTVendName = erExt.UsrATPTVendName;
            rowExt.UsrATPTVendTin = erExt.UsrATPTVendTin;
            rowExt.UsrATPTAddress = erExt.UsrATPTAddress;
            rowExt.UsrATPTAddress1 = erExt.UsrATPTAddress1;
            rowExt.UsrATPTCity = erExt.UsrATPTCity;
            rowExt.UsrATPTCountryID = erExt.UsrATPTCountryID;
            rowExt.UsrATPTATCCode = string.IsNullOrEmpty(cfmExt.UsrATPTEFMATCCode) ? erExt.UsrATPTATCCode  : cfmExt.UsrATPTEFMATCCode;
            rowExt.UsrATPTExpenseReceiptNbr = er.ClaimDetailCD;
            rowExt.UsrATPTKeepSourceTax = true;

            return row;
        }

        public delegate APInvoice DoAdditionalCreateApBillProcessDelegate(APInvoice row);
        [PXOverride]
        public APInvoice DoAdditionalCreateApBillProcess(APInvoice row, DoAdditionalCreateApBillProcessDelegate baseMethod)
        {
            ATPTPhilippineTax.DAC.Extensions.ATPTAPRegister rowExt = row.GetExtension<ATPTPhilippineTax.DAC.Extensions.ATPTAPRegister>();
            rowExt.UsrATPTWHTRecog = ATPTWHTRecognitionAttribute.UponInvoice;
            return row;
        }

    }
}
