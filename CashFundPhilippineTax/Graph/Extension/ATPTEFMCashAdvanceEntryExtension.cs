using ATPTPhilippineTax.Attributes;
using ATPTPhilippineTax.DAC.Extensions;
using ATPTPhilippineTax.Helpers;
using CashFundManagement.Attributes;
using CashFundManagement.DAC;
using CashFundManagement.Extensions.DAC;
using CashFundManagement.Helper;
using CashFundManagement.Messages;
using PX.Api;
using PX.Data;
using PX.Data.BQL;
using PX.Data.BQL.Fluent;
using PX.Objects.AP;
using PX.Objects.Common.Extensions;
using PX.Objects.EP;
using PX.Objects.IN;
using PX.Objects.TX;
using System.Linq;

namespace CashFundPhilippineTax.Graph.Extension 
{
    //TODO : 2025-07-01: Remove Dead Codes
    /// <remarks>
    /// 2024-08-14 : Adds multi-tenant support. {RRS} <br/>
    /// 2025-02-13:  Apply multi tenant to 24r1 CASE: 010269 {JLTG}
    /// </remarks>
    public class ATPTEFMCashAdvanceEntryExtension : PXGraphExtension<CashFundManagement.BLC.ATPTEFMCashAdvanceEntry>
    {
#if Version23R2
        public static bool IsActive() => ATPTModule.IsActive;
#else
        public static bool IsActive() => ATPTModule.IsActive && ATPTEFMPrefetchSetup.IsActive;
#endif

        //public delegate string ATCCodeDefaultDelegate(int? inventoryID);
        //[PXOverride]
        //public string ATCCodeDefault(int? inventoryID, ATCCodeDefaultDelegate baseMethod)
        //{
        //    InventoryItem item = PXSelect<
        //        InventoryItem,
        //        Where<InventoryItem.inventoryID, Equal<Required<InventoryItem.inventoryID>>>>
        //        .Select(Base, inventoryID);

        //    if (item != null)
        //    {
        //        ATPTPhilippineTax.DAC.Extensions.ATPTInventoryItem itemExt = item.GetExtension<ATPTPhilippineTax.DAC.Extensions.ATPTInventoryItem>();
        //        return itemExt.UsrATPTDefaultATC;
        //    }

        //    return null;
        //}
        public delegate APTran BeforeAPTranUpdateDelegate(APTran row);
        [PXOverride]
        public APTran BeforeAPTranUpdate(APTran row, BeforeAPTranUpdateDelegate baseMethod)
        {
            ATPTAPTran rowExt = row.GetExtension<ATPTAPTran>();
            rowExt.UsrATPTKeepSourceTax = true;
            rowExt.UsrATPTExpenseReceiptNbr = "FROM-CFM-CA";
            return row;
        }

        public delegate void SetDefATCDelegate(EPExpenseClaimDetails row, string defATC);
        [PXOverride]
        public void SetDefATC(EPExpenseClaimDetails row, string defATC, SetDefATCDelegate baseMethod)
        {
            ATPTEPExpenseClaimDetails rowExt = row.GetExtension<ATPTEPExpenseClaimDetails>();
            rowExt.UsrATPTATCCode = defATC;
        }

        public delegate string GetDefATCDelegate(EPExpenseClaimDetails row);
        [PXOverride]
        public string GetDefATC(EPExpenseClaimDetails row, GetDefATCDelegate baseMethod)
        {
            ATPTEPExpenseClaimDetails rowExt = row.GetExtension<ATPTEPExpenseClaimDetails>();
            return rowExt.UsrATPTATCCode;
        }

        //public delegate bool DuplicateERRefNbrDelegate(ATPTEFMCAReceiptDetail row, string checkRefNbr);
        //[PXOverride]
        //public bool DuplicateERRefNbr(ATPTEFMCAReceiptDetail row, string checkRefNbr, DuplicateERRefNbrDelegate baseMethod)
        //{
        //    Vendor vendor = PXSelect<Vendor, Where<Vendor.bAccountID, Equal<Required<Vendor.bAccountID>>>>.Select(Base, row.VendID);
        //    string vendorCD = vendor != null ? vendor.AcctCD : null;

        //    EPExpenseClaimDetails dup = null;

        //    if (vendorCD == null)
        //    {
        //        dup = PXSelect<
        //            EPExpenseClaimDetails,
        //            Where<EPExpenseClaimDetails.expenseRefNbr, Equal<@P.AsString>,
        //                And<ATPTEFMEPExpenseClaimDetailsExt.usrATPTEFMTranType, Equal<@P.AsString>,
        //                And<EPExpenseClaimDetails.claimDetailCD, NotEqual<@P.AsString>,
        //                And<EPExpenseClaimDetails.inventoryID, Equal<@P.AsInt>,
        //                And<Where<ATPTEPExpenseClaimDetails.usrATPTVendID, IsNull,
        //                    Or<ATPTEPExpenseClaimDetails.usrATPTVendID, Equal<Empty>>>>>>>>>
        //            .Select(Base, checkRefNbr, ATPTEFMExpenseTypeAttribute.Liquidation, row.ExpenseReceiptRefNbr, row.InventoryID);
        //    }
        //    else if (row.VendorName != null && row.VendorTin != null)
        //    {
        //        dup = PXSelect<
        //            EPExpenseClaimDetails,
        //            Where<EPExpenseClaimDetails.expenseRefNbr, Equal<@P.AsString>,
        //                And<ATPTEFMEPExpenseClaimDetailsExt.usrATPTEFMTranType, Equal<@P.AsString>,
        //                And<EPExpenseClaimDetails.claimDetailCD, NotEqual<@P.AsString>,
        //                And<EPExpenseClaimDetails.inventoryID, Equal<@P.AsInt>,
        //                And< ATPTEFMEPExpenseClaimDetailsExt.usrATPTVendName, Equal<@P.AsString>,
        //                And<ATPTEFMEPExpenseClaimDetailsExt.usrATPTVendTIN, Equal<@P.AsString>,
        //                And<Where<ATPTEPExpenseClaimDetails.usrATPTVendID, IsNull,
        //                    Or<ATPTEPExpenseClaimDetails.usrATPTVendID, Equal<Empty>>>>>>>>>>>
        //            .Select(Base, checkRefNbr, ATPTEFMExpenseTypeAttribute.Liquidation, row.ExpenseReceiptRefNbr, row.InventoryID, row.VendorName, row.VendorTin);
        //    }
        //    else
        //    {
        //        dup = PXSelect<
        //            EPExpenseClaimDetails, 
        //            Where<EPExpenseClaimDetails.expenseRefNbr, Equal<@P.AsString>, 
        //                And<ATPTEPExpenseClaimDetails.usrATPTVendID, Equal<@P.AsString>, 
        //                And<ATPTEFMEPExpenseClaimDetailsExt.usrATPTEFMTranType, Equal<@P.AsString>, 
        //                And<EPExpenseClaimDetails.claimDetailCD, NotEqual<@P.AsString>,
        //                And<EPExpenseClaimDetails.inventoryID, Equal<@P.AsInt>>>>>>>
        //            .Select(Base, checkRefNbr, vendorCD, ATPTEFMExpenseTypeAttribute.Liquidation, row.ExpenseReceiptRefNbr, row.InventoryID);
        //    }

        //    return dup != null;
        //}
        /// <remarks>
        /// When Cash Fund Management is integrated with Philippine Tax: Case: 009970 {JLTG}
        /// - For Prepayment type documents, WHT Recognition should be set to "Upon Payment"
        /// - This ensures proper WHT handling for prepayment scenarios
        /// </remarks>
        public delegate APInvoice DoAdditionalCreateApBillProcessDelegate(APInvoice row);
        [PXOverride]
        public APInvoice DoAdditionalCreateApBillProcess(APInvoice row, DoAdditionalCreateApBillProcessDelegate baseMethod)
        {
            ATPTPhilippineTax.DAC.Extensions.ATPTAPRegister rowExt = row.GetExtension<ATPTPhilippineTax.DAC.Extensions.ATPTAPRegister>();
            rowExt.UsrATPTWHTRecog = ATPTWHTRecognitionAttribute.UponPayment;
            return row;
        }

        /// <remarks>
        /// 2025-09-05 : return null if receipt.AtcCode is empty to avoid populating empty string. 013367 : RFS
        /// </remarks>
        public delegate EPExpenseClaimDetails AddAtcCodeProcessDelegate(EPExpenseClaimDetails row, ATPTEFMCAReceiptDetail receipt);
        [PXOverride]
        public EPExpenseClaimDetails AddAtcCode(EPExpenseClaimDetails row, ATPTEFMCAReceiptDetail receipt, AddAtcCodeProcessDelegate baseMethod)
        {
            ATPTEPExpenseClaimDetails expenseClaimExt = row.GetExtension<ATPTEPExpenseClaimDetails>();
            expenseClaimExt.UsrATPTATCCode = string.IsNullOrEmpty(receipt.AtcCode) ? null : receipt.AtcCode;
            return row;
        }

        protected virtual void _(Events.RowPersisting<ATPTEFMCAReceiptDetail> e)
        {
            ATPTEFMCAReceiptDetail row = e.Row;

            if (row is null) return;

            if(!string.IsNullOrEmpty(row.AtcCode)) 
            {
                var matchCount = SelectFrom<Tax>.
                    InnerJoin<TaxCategoryDet>.On<Tax.taxID.IsEqual<TaxCategoryDet.taxID>>.
                    InnerJoin<TaxZoneDet>.On<Tax.taxID.IsEqual<TaxZoneDet.taxID>>.
                    Where<Tax.taxID.IsEqual<@P.AsString>.
                        And<TaxCategoryDet.taxCategoryID.IsEqual<@P.AsString>.
                        And<TaxZoneDet.taxZoneID.IsEqual<@P.AsString>>>>.View.Select(Base, row.AtcCode, row.TaxCategoryID, row.TaxZoneID).Count;

                if (matchCount == 0)
                {
                    Base.CashAdvanceReceiptLines.Cache.RaiseExceptionHandling<ATPTEFMCAReceiptDetail.atcCode>(row, row.AtcCode, ATPTEFMHelper.GetPropertyException(row, string.Format(ATPTEFMMessages.NoLinesMatchTax, row.AtcCode), PXErrorLevel.Error));
                }
            }
        }

        //protected virtual void _(Events.FieldUpdated<ATPTEFMCAReceiptDetail, ATPTEFMCAReceiptDetail.inventoryID> e)
        //{
        //    var row = e.Row;

        //    if (row is null) return;

        //    InventoryItem item = PXSelect<
        //        InventoryItem, 
        //        Where<InventoryItem.inventoryID, Equal<Required<InventoryItem.inventoryID>>>>
        //        .Select(Base, row.InventoryID);

        //    if (item != null)
        //    {
        //        ATPTPhilippineTax.DAC.Extensions.ATPTInventoryItem itemExt = item.GetExtension<ATPTPhilippineTax.DAC.Extensions.ATPTInventoryItem>();
        //        row.AtcCode = itemExt.UsrATPTDefaultATC;
        //    }
        //}
    }
}
