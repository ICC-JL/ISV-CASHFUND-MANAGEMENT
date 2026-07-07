using ATPTPhilippineTax.DAC.Extensions;
using ATPTPhilippineTax.Helpers;
using CashFundManagement.Attributes;
using CashFundManagement.DAC;
using CashFundManagement.Extensions.DAC;
using CashFundManagement.Helper;
using CashFundManagement.Messages;
using PX.Data;
using PX.Data.BQL;
using PX.Data.BQL.Fluent;
using PX.Objects.AP;
using PX.Objects.EP;
using PX.Objects.IN;
using PX.Objects.TX;
using System;

namespace CashFundPhilippineTax.Graph.Extension 
{
    /// <remarks>
    /// 2024-08-14 : Adds multi-tenant support. {RRS} <br/>
    /// 2025-02-13:  Apply multi tenant to 24r1 CASE: 010269 {JLTG}
    /// </remarks>
    public class ATPTEFMFundTransactionEntryExtension : PXGraphExtension<CashFundManagement.BLC.ATPTEFMFundTransactionEntry>
    {
#if Version23R2
        public static bool IsActive() => ATPTModule.IsActive;
#else
        public static bool IsActive() => ATPTModule.IsActive && ATPTEFMPrefetchSetup.IsActive;
#endif

        public delegate bool DuplicateERRefNbrDelegate(ATPTEFMFundTransactionReceiptDetail row, string checkRefNbr);
        [PXOverride]
        public bool DuplicateERRefNbr(ATPTEFMFundTransactionReceiptDetail row, string checkRefNbr, DuplicateERRefNbrDelegate baseMethod)
        {
            Vendor vendor = PXSelect<Vendor, Where<Vendor.bAccountID, Equal<Required<Vendor.bAccountID>>>>.Select(Base, row.VendID);
            string vendorCD = vendor != null ? vendor.AcctCD : null;

            EPExpenseClaimDetails dup = null;

            if (vendorCD == null)
            {
                dup = PXSelect<
                    EPExpenseClaimDetails, 
                    Where<EPExpenseClaimDetails.expenseRefNbr, Equal<@P.AsString>, 
                        And<ATPTEFMEPExpenseClaimDetailsExt.usrATPTEFMTranType, Equal<@P.AsString>, 
                        And<EPExpenseClaimDetails.claimDetailCD, NotEqual<@P.AsString>,
                        And<EPExpenseClaimDetails.inventoryID, Equal<@P.AsInt>,
                        And<Where<ATPTEPExpenseClaimDetails.usrATPTVendID, IsNull, 
                            Or<ATPTEPExpenseClaimDetails.usrATPTVendID, Equal<Empty>>>>>>>>>
                    .Select(Base, checkRefNbr, ATPTEFMExpenseTypeAttribute.Replenishment, row.ExpenseReceiptRefNbr, row.InventoryID);
            }
            else
            {
                dup = PXSelect<
                    EPExpenseClaimDetails, 
                    Where<EPExpenseClaimDetails.expenseRefNbr, Equal<@P.AsString>, 
                        And<ATPTEPExpenseClaimDetails.usrATPTVendID, Equal<@P.AsString>, 
                        And<ATPTEFMEPExpenseClaimDetailsExt.usrATPTEFMTranType, Equal<@P.AsString>, 
                        And<EPExpenseClaimDetails.claimDetailCD, NotEqual<@P.AsString>,
                        And<EPExpenseClaimDetails.inventoryID, Equal<@P.AsInt>>>>>>>
                    .Select(Base, checkRefNbr, vendorCD, ATPTEFMExpenseTypeAttribute.Replenishment, row.ExpenseReceiptRefNbr, row.InventoryID);
            }

            return dup != null;
        }

        public delegate EPExpenseClaimDetails AddAtcCodeProcessDelegate(EPExpenseClaimDetails row, ATPTEFMFundTransactionReceiptDetail receipt);
        [PXOverride]
        public EPExpenseClaimDetails AddAtcCode(EPExpenseClaimDetails row, ATPTEFMFundTransactionReceiptDetail receipt, AddAtcCodeProcessDelegate baseMethod)
        {
            ATPTEPExpenseClaimDetails expenseClaimExt = row.GetExtension<ATPTEPExpenseClaimDetails>();
            expenseClaimExt.UsrATPTATCCode = receipt.AtcCode;
            return row;
        }

        protected virtual void _(Events.RowPersisting<ATPTEFMFundTransactionReceiptDetail> e)
        {
            ATPTEFMFundTransactionReceiptDetail row = e.Row;

            if (row is null) return;

            if (!string.IsNullOrEmpty(row.AtcCode))
            {
                var matchCount = SelectFrom<Tax>.
                    InnerJoin<TaxCategoryDet>.On<Tax.taxID.IsEqual<TaxCategoryDet.taxID>>.
                    InnerJoin<TaxZoneDet>.On<Tax.taxID.IsEqual<TaxZoneDet.taxID>>.
                    Where<Tax.taxID.IsEqual<@P.AsString>.
                        And<TaxCategoryDet.taxCategoryID.IsEqual<@P.AsString>.
                        And<TaxZoneDet.taxZoneID.IsEqual<@P.AsString>>>>.View.Select(Base, row.AtcCode, row.TaxCategoryID, row.TaxZoneID).Count;

                if (matchCount == 0)
                {
                    Base.FundTransactionReceiptLines.Cache.RaiseExceptionHandling<ATPTEFMFundTransactionReceiptDetail.atcCode>(row, row.AtcCode, ATPTEFMHelper.GetPropertyException(row, String.Format(ATPTEFMMessages.NoLinesMatchTax, row.AtcCode), PXErrorLevel.Error));
                }
            }
        }
        protected virtual void _(Events.FieldUpdated<ATPTEFMFundTransactionReceiptDetail, ATPTEFMFundTransactionReceiptDetail.inventoryID> e)
        {
            var row = e.Row;

            if (row is null) return;

            InventoryItem item = PXSelect<
                InventoryItem,
                Where<InventoryItem.inventoryID, Equal<Required<InventoryItem.inventoryID>>>>
                .Select(Base, row.InventoryID);

            if (item != null)
            {
                ATPTPhilippineTax.DAC.Extensions.ATPTInventoryItem itemExt = item.GetExtension<ATPTPhilippineTax.DAC.Extensions.ATPTInventoryItem>();
                row.AtcCode = itemExt.UsrATPTDefaultATC;
            }
        }

    }
}
