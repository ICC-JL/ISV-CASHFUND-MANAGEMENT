using ATPTPhilippineTax.Attributes;
using ATPTPhilippineTax.DAC.Extensions;
using ATPTPhilippineTax.Helpers;
using CashFundManagement.Attributes;
using CashFundManagement.Extensions.BLC;
using CashFundManagement.Helper;
using PX.Data;
using PX.Objects.AP;
using PX.Objects.EP;

namespace CashFundPhilippineTax.Graph.Extension 
{
    /// <remarks>
    /// 2024-08-14 : Adds multi-tenant support. {RRS} <br/>
    /// 2025-02-13:  Apply multi tenant to 24r1 CASE: 010269 {JLTG}
    /// 010396 - (CFM24R1) Request for Payment> Expense Claim: Bill WHT Recognition should be based on the Vendor's WHT Recognition setup.
    /// </remarks>
    public class ATPTEFMEPReleaseProcess : PXGraphExtension<ATPTEFMEPReleaseProcess_Extension, ATPTPhilippineTax.Graph.Extensions.ATPTEPReleaseProcess ,EPReleaseProcess>
    {
#if Version23R2
        public static bool IsActive() => ATPTModule.IsActive;
#else
        public static bool IsActive() => ATPTModule.IsActive && ATPTEFMPrefetchSetup.IsActive;
#endif

        public delegate APInvoice DoAdditionalCreateApBillProcessDelegate(APInvoice row, string type);
        [PXOverride]
        public APInvoice DoAdditionalCreateApBillProcess(APInvoice row, string type, DoAdditionalCreateApBillProcessDelegate baseMethod)
        {
            ATPTPhilippineTax.DAC.Extensions.ATPTAPRegister rowExt = row.GetExtension<ATPTPhilippineTax.DAC.Extensions.ATPTAPRegister>();
            if (type == ATPTEFMExpenseTypeAttribute.RequestforPayment)
            {
                Vendor vendor = Vendor.PK.Find(Base, row.VendorID);
                if (vendor != null)
                {
                    ATPTBAccount vendorBaccount = vendor.GetExtension<ATPTBAccount>();
                    if (vendorBaccount != null)
                    {
                        rowExt.UsrATPTWHTRecog = vendorBaccount.UsrATPTWHTRecog;
                    }
                }
            }
            else
                rowExt.UsrATPTWHTRecog = ATPTWHTRecognitionAttribute.UponInvoice;

            return row;
        }

        /// <remarks>
        /// This method is not needed anymore implementation is handled on Philtax.
        /// </remarks>
        public delegate void DoAdditionalOperationDelegate(APTran apTran, EPExpenseClaimDetails claimdetail);
        [PXOverride]
        public virtual void DoAdditionalOperation(APTran apTran, EPExpenseClaimDetails claimdetail, DoAdditionalOperationDelegate baseMethod)
        {
            baseMethod?.Invoke(apTran, claimdetail);

            ATPTPhilippineTax.Graph.Extensions.ATPTEPReleaseProcess graphExt = Base.GetExtension<ATPTPhilippineTax.Graph.Extensions.ATPTEPReleaseProcess>();
            graphExt?.ApplyVendorDetails(apTran, claimdetail);
        }
    }
}
