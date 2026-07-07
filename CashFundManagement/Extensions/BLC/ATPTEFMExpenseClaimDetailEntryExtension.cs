using CashFundManagement.Attributes;
using CashFundManagement.BLC;
using CashFundManagement.DAC;
using CashFundManagement.DAC.Setup;
using CashFundManagement.Extensions.Attribute;
using CashFundManagement.Extensions.DAC;
using CashFundManagement.Helper;
using CashFundManagement.Messages;
using PX.Api;
using PX.Common;
using PX.Data;
using PX.Data.BQL;
using PX.Data.BQL.Fluent;
using PX.Objects.AP;
using PX.Objects.CR;
using PX.Objects.CS;
using PX.Objects.EP;
using PX.Objects.FA;
using PX.Objects.GL;
using PX.Objects.IN;
using PX.Objects.RQ;
using PX.Objects.TX;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using static CashFundManagement.BLC.ATPTEFMFundMaint;

namespace CashFundManagement.Extensions.BLC
{
    /// <summary>
    /// Expense Receipt Extension
    /// </summary>
    /// <remarks>
    /// 2024-08-14 : Adds multi-tenant support. {RRS}
    /// 010043 - CFM 2024R1: Expense Receipts > Expense Sub.
    /// 2025-03-10 : Added new fieldupdated event for Vendor Population : 010624 : RFS
    /// </remarks>
    public class ATPTEFMExpenseClaimDetailEntry_Extension : PXGraphExtension<ExpenseClaimDetailEntry>
    {
#if Version23R2
        public static bool IsActive() => true;
#else
        public static bool IsActive() => ATPTEFMPrefetchSetup.IsActive;
#endif

        #region Views
        public PXSetup<ATPTEFMReqClass> ATPTEFMEmployeeReqClass;
        public PXSetup<ATPTEFMCASetup> ATPTEFMPreferences;
        public PXSetup<ATPTEFMSetup> ATPTEFMFundPreferences;
        public PXSetup<ATPTEFMFeatures> ATPTEFMFeatureSetup;

        public PXSelect<
            ATPTEFMReqClass,
            Where<ATPTEFMReqClass.reqClassID, Equal<Optional<ATPTEFMEPExpenseClaimDetailsExt.usrATPTEFMReqClass>>>>
            ATPTEFMReqClass;

        public PXSelect<
            ATPTEFMFundTransactionReceiptDetail,
            Where<ATPTEFMFundTransactionReceiptDetail.expenseReceiptRefNbr, Equal<Required<EPExpenseClaimDetails.claimDetailCD>>>>
            ATPTEFMFundTransactionReceiptDetail;

        [PXViewName("Fund Transaction Reclassification Receipt")]
        public PXSelect<
            ATPTEFMFundTransactionReclassficationReceiptDetail,
            Where<ATPTEFMFundTransactionReclassficationReceiptDetail.expenseReceiptRefNbr, Equal<Required<ATPTEFMFundTransactionReceiptDetail.expenseReceiptRefNbr>>>>
            ATPTEFMFundTransactionReclassificationReceiptDetail;

        //TODO : change required to current claimdetailsExt.UsrATPTEFMRequestRefNbr
        public PXSelect<
            ATPTEFMFundTransaction,
            Where<ATPTEFMFundTransaction.refNbr, Equal<Required<ATPTEFMFundTransaction.refNbr>>>>
            ATPTEFMFundTransaction;

        public PXSelect<
            ATPTEFMFundTransactionHistoryView,
            Where<ATPTEFMFundTransactionHistoryView.refNbr, Equal<Required<ATPTEFMFundTransactionHistoryView.refNbr>>>>
            ATPTEFMFundTransactionHistory;


        [PXViewName("Fund")]
        public PXSelect<
            ATPTEFMFund,
            Where<ATPTEFMFund.fundCD, Equal<Required<ATPTEFMFund.fundCD>>>>
            ATPTEFMFund;

        public PXSelect<
            Account,
            Where<Account.accountID, Equal<Required<EPExpenseClaimDetails.expenseAccountID>>>>
            ATPTEFMAccountDescription;
        #endregion

        #region Events
        /// <remarks>
        /// 2025-03-26 : Remove FieldVerifying, transfer logic to Field Attribute - 010685 - RFS
        /// </remarks>
        //protected virtual void _(Events.FieldVerifying<EPExpenseClaimDetails, EPExpenseClaimDetails.expenseRefNbr> e, PXFieldVerifying baseEvent)
        //{
        //    if (baseEvent != null) baseEvent.Invoke(e.Cache, e.Args);

        //    EPExpenseClaimDetails row = e.Row;
        //    if (row == null) return;
        //    ATPTEFMEPExpenseClaimDetailsExt rowExt = row.GetExtension<ATPTEFMEPExpenseClaimDetailsExt>();

        //    EPSetup expenseSetup = Base.epsetup.Current;
        //    ATPTEFMEPSetupExtension expenseSetupExt = expenseSetup.GetExtension<ATPTEFMEPSetupExtension>();

        //    if (e.NewValue != null)
        //    {
        //        string checkRefnbr = e.NewValue.ToString();

        //        if (expenseSetupExt.UsrATPTEFMRaiseErrorOnDuplicateRefNbr ?? false)
        //        {
        //            string vendorCD = GetVendor(row);

        //            if (rowExt.UsrATPTEFMTranType == ATPTEFMExpenseTypeAttribute.Liquidation)
        //            {
        //                ATPTEFMCAReceiptDetail dup = null;

        //                if (vendorCD.IsNullOrEmpty())
        //                {
        //                    dup = PXSelect<
        //                        ATPTEFMCAReceiptDetail,
        //                        Where<ATPTEFMCAReceiptDetail.refNbr, Equal<@P.AsString>,
        //                            And<ATPTEFMCAReceiptDetail.expenseReceiptRefNbr, NotEqual<@P.AsString>,
        //                            And<ATPTEFMCAReceiptDetail.inventoryID, Equal<@P.AsInt>,
        //                            And<Where<ATPTEFMCAReceiptDetail.vendID, Equal<Empty>,
        //                                Or<ATPTEFMCAReceiptDetail.vendID, IsNull>>>>>>>
        //                        .Select(Base, checkRefnbr, row.ClaimDetailCD, row.InventoryID);
        //                }
        //                else
        //                {
        //                    VendorR vendor = PXSelect<
        //                        VendorR,
        //                        Where<VendorR.acctCD, Equal<Required<VendorR.acctCD>>>>
        //                        .Select(Base, vendorCD);
        //                    dup = PXSelect<
        //                        ATPTEFMCAReceiptDetail,
        //                        Where<ATPTEFMCAReceiptDetail.refNbr, Equal<@P.AsString>,
        //                            And<ATPTEFMCAReceiptDetail.vendID, Equal<@P.AsInt>,
        //                            And<ATPTEFMCAReceiptDetail.expenseReceiptRefNbr, NotEqual<@P.AsString>,
        //                            And<ATPTEFMCAReceiptDetail.inventoryID, Equal<@P.AsInt>>>>>>
        //                        .Select(Base, checkRefnbr, vendor.BAccountID, row.ClaimDetailCD, row.InventoryID);
        //                }

        //                bool? dupEr = DuplicateERRefNbr(row, checkRefnbr, rowExt.UsrATPTEFMTranType);

        //                if (dup != null || (dupEr ?? false))
        //                {
        //                    throw ATPTEFMHelper.GetPropertyException(row, ATPTEFMMessages.RefNbrDuplicate, PXErrorLevel.Error);
        //                }
        //            }
        //            else if (rowExt.UsrATPTEFMTranType == ATPTEFMExpenseTypeAttribute.Replenishment)
        //            {
        //                ATPTEFMFundTransactionReceiptDetail dup = null;

        //                if (vendorCD.IsNullOrEmpty())
        //                {
        //                    dup = PXSelect<
        //                        ATPTEFMFundTransactionReceiptDetail,
        //                        Where<ATPTEFMFundTransactionReceiptDetail.refNbr, Equal<@P.AsString>,
        //                            And<ATPTEFMFundTransactionReceiptDetail.expenseReceiptRefNbr, NotEqual<@P.AsString>,
        //                            And<ATPTEFMFundTransactionReceiptDetail.inventoryID, Equal<@P.AsInt>,
        //                            And<Where<ATPTEFMFundTransactionReceiptDetail.vendID, Equal<Empty>,
        //                                Or<ATPTEFMFundTransactionReceiptDetail.vendID, IsNull>>>>>>>
        //                        .Select(Base, checkRefnbr, row.ClaimDetailCD, row.InventoryID);
        //                }
        //                else
        //                {
        //                    VendorR vendor = PXSelect<
        //                        VendorR,
        //                        Where<VendorR.acctCD, Equal<Required<VendorR.acctCD>>>>
        //                        .Select(Base, vendorCD);
        //                    dup = PXSelect<
        //                        ATPTEFMFundTransactionReceiptDetail,
        //                        Where<ATPTEFMFundTransactionReceiptDetail.refNbr, Equal<@P.AsString>,
        //                            And<ATPTEFMFundTransactionReceiptDetail.vendID, Equal<@P.AsInt>,
        //                            And<ATPTEFMFundTransactionReceiptDetail.expenseReceiptRefNbr, NotEqual<@P.AsString>,
        //                            And<ATPTEFMFundTransactionReceiptDetail.inventoryID, Equal<@P.AsInt>>>>>>
        //                        .Select(Base, checkRefnbr, vendor.BAccountID, row.ClaimDetailCD, row.InventoryID);
        //                }

        //                bool? dupEr = DuplicateERRefNbr(row, checkRefnbr, rowExt.UsrATPTEFMTranType);

        //                if (dup != null || (dupEr ?? false))
        //                {
        //                    throw ATPTEFMHelper.GetPropertyException(row, ATPTEFMMessages.RefNbrDuplicate, PXErrorLevel.Error);
        //                }
        //            }
        //            else
        //            {
        //                bool? dupEr = DuplicateERRefNbr(row, checkRefnbr, rowExt.UsrATPTEFMTranType);

        //                if (dupEr ?? false)
        //                {
        //                    throw ATPTEFMHelper.GetPropertyException(row, ATPTEFMMessages.RefNbrDuplicate, PXErrorLevel.Error);
        //                }
        //            }
        //        }
        //    }
        //}
        protected virtual void _(Events.RowUpdated<EPExpenseClaimDetails> e, PXRowUpdated baseEvent)
        {
            if (baseEvent != null) baseEvent.Invoke(e.Cache, e.Args);

            EPExpenseClaimDetails er = e.Row;
            ATPTEFMEPExpenseClaimDetailsExt rowExt = er.GetExtension<ATPTEFMEPExpenseClaimDetailsExt>();
            if (er == null) return;

            //object refRow = er.ExpenseRefNbr;
            //Base.CurrentClaimDetails.Cache.RaiseFieldVerifying<EPExpenseClaimDetails.expenseRefNbr>(er, ref refRow);
        }

        /// <remarks>
        /// 2025-07-28 : Updated join conditions to filter non-cancelled records. CASE: 012688 {JLTG}
        /// </remarks>
        protected virtual void _(Events.RowSelecting<EPExpenseClaimDetails> e, PXRowSelecting baseEvent)
        {
            if (baseEvent != null) baseEvent.Invoke(e.Cache, e.Args);

            EPExpenseClaimDetails er = e.Row;
            ATPTEFMEPExpenseClaimDetailsExt rowExt = er.GetExtension<ATPTEFMEPExpenseClaimDetailsExt>();
            if (er == null) return;

            using (new PXConnectionScope())
            {
                ATPTEFMFundTransaction.Current = ATPTEFMFundTransaction.Select(rowExt.UsrATPTEFMRequestRefNbr);
                ATPTEFMFund.Current = ATPTEFMFund.Select(ATPTEFMFundTransaction.Current?.FundID);
                ATPTEFMFundTransactionReceiptDetail.Current = PXSelectJoin<
                    ATPTEFMFundTransactionReceiptDetail,
                    InnerJoin<ATPTEFMReplenishmentDetail,
                        On<ATPTEFMReplenishmentDetail.expenseReceiptNbr, Equal<ATPTEFMFundTransactionReceiptDetail.expenseReceiptRefNbr>>,
                    InnerJoin<ATPTEFMReplenishment,
                        On<ATPTEFMReplenishment.replenishmentNbr, Equal<ATPTEFMReplenishmentDetail.replenishmentNbr>>>>,
                    Where<ATPTEFMFundTransactionReceiptDetail.expenseReceiptRefNbr, Equal<Required<ATPTEFMFundTransactionReceiptDetail.expenseReceiptRefNbr>>,
                        And<ATPTEFMReplenishment.status, NotEqual<ATPTEFMReplenishmentStatusAttribute.cancelledValue>>>>
                    .Select(Base, er.ClaimDetailCD);

                ATPTEFMFundTransactionReclassificationReceiptDetail.Current = PXSelectJoin<
                    ATPTEFMFundTransactionReclassficationReceiptDetail,
                    InnerJoin<ATPTEFMReplenishmentDetail,
                        On<ATPTEFMReplenishmentDetail.expenseReceiptNbr, Equal<ATPTEFMFundTransactionReclassficationReceiptDetail.expenseReceiptRefNbr>>>,
                    Where<ATPTEFMFundTransactionReclassficationReceiptDetail.expenseReceiptRefNbr, Equal<Required<ATPTEFMFundTransactionReclassficationReceiptDetail.expenseReceiptRefNbr>>>>
                    .Select(Base, er.ClaimDetailCD);

                //ATPTEFMFundTransactionReceiptDetail.Select(er.ClaimDetailCD);
            }
        }
        protected virtual void _(Events.RowInserted<EPExpenseClaimDetails> e, PXRowInserted baseEvent)
        {
            if (baseEvent != null) baseEvent.Invoke(e.Cache, e.Args);

            EPExpenseClaimDetails row = e.Row;
            if (row == null) return;
            ATPTEFMEPExpenseClaimDetailsExt rowExt = row.GetExtension<ATPTEFMEPExpenseClaimDetailsExt>();

            if (row.RefNbr != null)
            {
                EPExpenseClaim claim = EPExpenseClaim.PK.Find(Base, row.RefNbr);
                if (claim != null)
                {
                    ATPTEFMEPExpenseClaimExt claimExt = claim.GetExtension<ATPTEFMEPExpenseClaimExt>();
                    rowExt.UsrATPTEFMTranType = claimExt.UsrATPTEFMTranType;
                    rowExt.UsrATPTEFMReqType = claimExt.UsrATPTEFMReqType;
                    rowExt.UsrATPTEFMReqClass = claimExt.UsrATPTEFMReqClass;

                    if (rowExt.UsrATPTEFMTranType == ATPTEFMExpenseTypeAttribute.RequestforPayment)
                        rowExt.UsrATPTVendorID = claimExt.UsrATPTVendorID;
                }
            }
        }
        /// <remarks>
        /// 2024-10-07 : If the receipt is from FT and FT contains reclass receipts, the reclass receipt should be cancelled first before the request receipts.. CASEID: 007642 {JLG} <br/>
        /// 2024-10-08 : Related case [007642]. CASEID: 007642 {JLG}
        /// </remarks>
        protected virtual void _(Events.RowSelected<EPExpenseClaimDetails> e, PXRowSelected baseMethod)
        {
            baseMethod.Invoke(e.Cache, e.Args);

            EPExpenseClaimDetails row = (EPExpenseClaimDetails)e.Row;
            ATPTEFMEPExpenseClaimDetailsExt rowExt = row.GetExtension<ATPTEFMEPExpenseClaimDetailsExt>();

            ATPTEFMFundTransaction fundTransaction = ATPTEFMFundTransaction.Current;
            ATPTEFMFund funds = ATPTEFMFund.Current;
            ATPTEFMFundTransactionReceiptDetail ftReceipt = ATPTEFMFundTransactionReceiptDetail.Current;
            ATPTEFMFundTransactionReclassficationReceiptDetail recReceipt = ATPTEFMFundTransactionReclassificationReceiptDetail.Current;

            Base.Claim.SetEnabled(rowExt.UsrATPTEFMTranType != ATPTEFMExpenseTypeAttribute.Replenishment);
            PXUIFieldAttribute.SetEnabled(Base.ClaimDetails.Cache, "UsrATPTEFMReqType", false);
            PXUIFieldAttribute.SetRequired<EPExpenseClaimDetails.expenseRefNbr>(e.Cache, Base.epsetup.Current.RequireRefNbrInExpenseReceipts ?? false);

            bool isClaimEmpty = (string.IsNullOrEmpty(row.RefNbr)) ? true : false;
            bool enableCancelForLiqAndRFP = row.Status != ATPTEFMExpenseReceiptStatusAttribute.CancelledValue
                     && row.Status != ATPTEFMExpenseReceiptStatusAttribute.ReleaseValue
                     && isClaimEmpty;

            if (rowExt.UsrATPTEFMTranType == ATPTEFMExpenseTypeAttribute.Liquidation)
            {
                ATPTEFMSubmitReceipt.SetEnabled(true);
                ATPTEFMCancel.SetEnabled(enableCancelForLiqAndRFP);

                rowExt.UsrATPTEFMIsBudgetNotCashAdvance = !Classes.ATPTEFMBudgetLibrary.BudgetVisible(ATPTEFMFeatureSetup.Current, "C");
            }

            else if (rowExt.UsrATPTEFMTranType == ATPTEFMExpenseTypeAttribute.RequestforPayment)
            {
                ATPTEFMSubmitReceipt.SetEnabled(false);
                ATPTEFMCancel.SetEnabled(enableCancelForLiqAndRFP);
            }

            else if (rowExt.UsrATPTEFMTranType == ATPTEFMExpenseTypeAttribute.Replenishment)
            {
                bool isAllowUpdate = (row.Status != ATPTEFMExpenseReceiptStatusAttribute.ReleaseValue && row.Status != ATPTEFMExpenseReceiptStatusAttribute.CancelledValue);
                Base.ClaimDetails.AllowUpdate = (row.Status != ATPTEFMExpenseReceiptStatusAttribute.ReleaseValue && row.Status != ATPTEFMExpenseReceiptStatusAttribute.CancelledValue);
                Base.CurrentClaimDetails.AllowUpdate = (row.Status != ATPTEFMExpenseReceiptStatusAttribute.ReleaseValue && row.Status != ATPTEFMExpenseReceiptStatusAttribute.CancelledValue);
                ATPTEFMSubmitReceipt.SetEnabled(row.Status != ATPTEFMExpenseReceiptStatusAttribute.CancelledValue && row.Status != ATPTEFMExpenseReceiptStatusAttribute.ReleaseValue);

                if (rowExt.UsrATPTEFMRequestRefNbr != null
                    && fundTransaction != null
                    && funds != null)
                {
                    //Check if receipt is already added in replenishment
                    if (ftReceipt != null)
                    {
                        //isNotCancelled = !(row.Status == ATPTEFMExpenseReceiptStatusAttribute.CancelledValue || row.Status == ATPTEFMExpenseReceiptStatusAttribute.ReleaseValue || (!string.IsNullOrEmpty(ftReceipt.ReplenishmentRefNbr)) || fundTransaction.Status == ATPTEFMFundStatusAttribute.ClosedValue);
                        ATPTEFMSubmitReceipt.SetEnabled((!string.IsNullOrEmpty(ftReceipt.ReplenishmentRefNbr)) || fundTransaction.Status == ATPTEFMFundStatusAttribute.ClosedValue);
                        ATPTEFMCancel.SetEnabled(false);
                    }

                    if (fundTransaction.FundTransactionType == ATPTEFMFundTransactionTypeAttribute.ReimbursementValue)
                    {
                        ATPTEFMCancel.SetEnabled(false);
                        Base.Delete.SetEnabled(false);
                    }
                    else if (fundTransaction.FundTransactionType == ATPTEFMFundTransactionTypeAttribute.CashAdvanceValue)
                    {
                        if (fundTransaction.ReclassificationAmt > 0m)
                        {
                            Base.ClaimDetails.AllowUpdate = fundTransaction.CashAdvanceStatus != ATPTEFMFundTransactionCashAdvanceStatusAttribute.LiquidatedValue && (!rowExt.UsrATPTEFMIsReclassifyDoc ?? false) && row.Status != ATPTEFMExpenseReceiptStatusAttribute.ReleaseValue && row.Status != ATPTEFMExpenseReceiptStatusAttribute.CancelledValue;
                            Base.CurrentClaimDetails.AllowUpdate = fundTransaction.CashAdvanceStatus != ATPTEFMFundTransactionCashAdvanceStatusAttribute.LiquidatedValue && (!rowExt.UsrATPTEFMIsReclassifyDoc ?? false) && row.Status != ATPTEFMExpenseReceiptStatusAttribute.ReleaseValue && row.Status != ATPTEFMExpenseReceiptStatusAttribute.CancelledValue;
                            Base.Delete.SetEnabled(fundTransaction.CashAdvanceStatus != ATPTEFMFundTransactionCashAdvanceStatusAttribute.LiquidatedValue);
                        }

                        ATPTEFMSubmitReceipt.SetEnabled(fundTransaction.CashAdvanceStatus != ATPTEFMFundTransactionCashAdvanceStatusAttribute.ForReclassificationValue
                           && row.Status != ATPTEFMExpenseReceiptStatusAttribute.CancelledValue
                           && row.Status != ATPTEFMExpenseReceiptStatusAttribute.ReleaseValue);

                        if (fundTransaction.Status.Equals(ATPTEFMFundStatusAttribute.ClosedValue) && funds.ATPTEFMValidateAmountReceivedAndAmountReleased == true)
                        {
                            Base.ClaimDetails.AllowUpdate = false;
                            Base.CurrentClaimDetails.AllowUpdate = false;
                            Base.Delete.SetEnabled(false);
                            ATPTEFMSubmitReceipt.SetEnabled(false);
                        }

                        #region Disabling Cancel button

                        if (rowExt.UsrATPTEFMIsUnreplenish == true)
                            ATPTEFMCancel.SetEnabled(false);
                        else
                        {
                            bool isReclassifyDoc = rowExt.UsrATPTEFMIsReclassifyDoc ?? false;
                            bool isReclassReceiptInReplenishment = recReceipt != null;
                            bool isRequestReceiptInReplenishment = ftReceipt != null;

                            bool shouldEnableCancel = isReclassifyDoc
                                ? !isReclassReceiptInReplenishment
                                : !isRequestReceiptInReplenishment && fundTransaction.ReclassificationAmt == 0;

                            ATPTEFMCancel.SetEnabled(shouldEnableCancel);
                        }

                        #endregion
                    }

                    Base.hold.SetEnabled(fundTransaction.Status != ATPTEFMFundStatusAttribute.ClosedValue);
                }

                ATPTEFMSubmitReceipt.SetEnabled(row.Status != ATPTEFMExpenseReceiptStatusAttribute.CancelledValue && row.Status != ATPTEFMExpenseReceiptStatusAttribute.ReleaseValue);
                //if (row.Status == ATPTEFMExpenseReceiptStatusAttribute.CancelledValue || row.Status == ATPTEFMExpenseReceiptStatusAttribute.ReleaseValue)
                //{
                //    //ATPTEFMCancel.SetEnabled(false);
                //    //ATPTEFMSubmitReceipt.SetEnabled(false);
                //}

                rowExt.UsrATPTEFMIsBudgetNotCashAdvance = !Classes.ATPTEFMBudgetLibrary.BudgetVisible(ATPTEFMFeatureSetup.Current, "F");

                #region Require Vendor Details for Replenishment Type
                // -> Behavior transferred on Connector ER extension
                //if ((ATPTEFMFundPreferences.Current.RequireVendorDetails ?? false) && (!rowExt.UsrATPTEFMIsReclassifyDoc ?? false))
                //{
                //    PXUIFieldAttribute.SetRequired<ATPTEFMEPExpenseClaimDetailsExt.usrATPTVendName>(e.Cache, true);
                //    PXDefaultAttribute.SetPersistingCheck<ATPTEFMEPExpenseClaimDetailsExt.usrATPTVendName>(e.Cache, row, PXPersistingCheck.NullOrBlank);

                //    PXUIFieldAttribute.SetRequired<ATPTEFMEPExpenseClaimDetailsExt.usrATPTAddress>(e.Cache, true);
                //    PXDefaultAttribute.SetPersistingCheck<ATPTEFMEPExpenseClaimDetailsExt.usrATPTAddress>(e.Cache, row, PXPersistingCheck.NullOrBlank);

                //    PXUIFieldAttribute.SetRequired<ATPTEFMEPExpenseClaimDetailsExt.usrATPTVendTIN>(e.Cache, true);
                //    PXDefaultAttribute.SetPersistingCheck<ATPTEFMEPExpenseClaimDetailsExt.usrATPTVendTIN>(e.Cache, row, PXPersistingCheck.NullOrBlank);
                //}
                //else
                //{
                //    PXUIFieldAttribute.SetRequired<ATPTEFMEPExpenseClaimDetailsExt.usrATPTVendName>(e.Cache, false);
                //    PXDefaultAttribute.SetPersistingCheck<ATPTEFMEPExpenseClaimDetailsExt.usrATPTVendName>(e.Cache, row, PXPersistingCheck.Nothing);

                //    PXUIFieldAttribute.SetRequired<ATPTEFMEPExpenseClaimDetailsExt.usrATPTAddress>(e.Cache, false);
                //    PXDefaultAttribute.SetPersistingCheck<ATPTEFMEPExpenseClaimDetailsExt.usrATPTAddress>(e.Cache, row, PXPersistingCheck.Nothing);

                //    PXUIFieldAttribute.SetRequired<ATPTEFMEPExpenseClaimDetailsExt.usrATPTVendTIN>(e.Cache, false);
                //    PXDefaultAttribute.SetPersistingCheck<ATPTEFMEPExpenseClaimDetailsExt.usrATPTVendTIN>(e.Cache, row, PXPersistingCheck.Nothing);
                //}

                #endregion
            }

            #region Hide RFP from ER dropdown if FT is enabled on Budget
            if ((Classes.ATPTEFMBudgetLibrary.BudgetVisible(ATPTEFMFeatureSetup?.Current, "P") || Classes.ATPTEFMProjectBudgetLibrary.ProjectBudgetVisible(ATPTEFMFeatureSetup?.Current, "P")) && row.RefNbr == null)
            {
                PXStringListAttribute.SetList<ATPTEFMEPExpenseClaimDetailsExt.usrATPTEFMTranType>(
                   e.Cache,
                   e.Row,
                   new string[] { "LIQ", "REP" },
                   new string[] { "Liquidation", "Replenishment" }
               );
            }
            #endregion
        }

        /// <remarks>
        /// 2024-09-16 : Update Fund transaction history project fields when receipt is updated. 007469 {RRS}
        /// 2025-09-17 : This code should be removed because the custom selector for the inventory ID(Expense Receipt) can handle the logic. If the 'Allow Manual Receipt' option in Fund Management Preference is unticked, only the requested items will be shown. CASE: 013539 {JLTG}
        /// </remarks>
        protected virtual void _(Events.RowPersisting<EPExpenseClaimDetails> e, PXRowPersisting baseMethod)
        {
            baseMethod(e.Cache, e.Args);

            EPExpenseClaimDetails row = (EPExpenseClaimDetails)e.Row;
            if (row == null) return;

            /*if (rowExt.UsrATPTEFMTranType == ATPTEFMExpenseTypeAttribute.Replenishment)
            {
                if ((!ATPTEFMFundPreferences.Current.AllowManualReceipts ?? false) && (!rowExt.UsrATPTEFMIsReclassifyDoc ?? false))
                {
                    ATPTEFMFundTransaction ft = PXSelect<
                        ATPTEFMFundTransaction,
                        Where<ATPTEFMFundTransaction.refNbr, Equal<Required<ATPTEFMFundTransaction.refNbr>>>>
                        .Select(this.Base, rowExt.UsrATPTEFMRequestRefNbr);

                    if (ft.FundTransactionType == ATPTEFMFundTransactionTypeAttribute.CashAdvanceValue)
                    {
                        ATPTEFMFundTransactionDetail transactionDetail = PXSelect<
                            ATPTEFMFundTransactionDetail,
                            Where<ATPTEFMFundTransactionDetail.fundTransactionRefNbr, Equal<Required<ATPTEFMFundTransactionDetail.fundTransactionRefNbr>>,
                                And<ATPTEFMFundTransactionDetail.inventoryID, Equal<Required<ATPTEFMFundTransactionDetail.inventoryID>>>>>
                            .Select(this.Base, rowExt.UsrATPTEFMRequestRefNbr, row.InventoryID);

                        if (transactionDetail == null) throw new Exception(Messages.ATPTEFMMessages.RequestedItemNotTheSame);
                    }
                }
            }*/

            //object refRow = row.ExpenseRefNbr;
            //Base.CurrentClaimDetails.Cache.RaiseFieldVerifying<EPExpenseClaimDetails.expenseRefNbr>(row, ref refRow);
            UpdateFundTransactionHistory(row);
        }

        protected virtual void _(Events.FieldUpdated<EPExpenseClaimDetails, EPExpenseClaimDetails.inventoryID> e, PXFieldUpdated baseMethod)
        {
            baseMethod(e.Cache, e.Args);

            EPExpenseClaimDetails row = e.Row;
            if (row is null) return;

            ATPTEFMEPExpenseClaimDetailsExt rowExt = row.GetExtension<ATPTEFMEPExpenseClaimDetailsExt>();

            e.Cache.SetDefaultExt<EPExpenseClaimDetails.expenseAccountID>(e.Row);
            e.Cache.SetDefaultExt<EPExpenseClaimDetails.expenseSubID>(e.Row);

            if (rowExt.UsrATPTEFMTranType == ATPTEFMExpenseTypeAttribute.Liquidation)
            {
                if (!(rowExt.UsrATPTEFMIsBudgetNotCashAdvance) ?? false)
                {
                    ATPTEFMCARequestDetail reqDet = PXSelect<
                        ATPTEFMCARequestDetail,
                        Where<ATPTEFMCARequestDetail.cashAdvanceNbr, Equal<Required<ATPTEFMCARequestDetail.cashAdvanceNbr>>>>
                        .Select(Base, rowExt.UsrATPTEFMReqRef);
                    if (reqDet != null)
                    {
                        row.ContractID = reqDet.ProjectID;
                        row.TaskID = reqDet.ProjectTaskID;
                        row.CostCodeID = reqDet.CostCodeID;
                        //row.ExpenseAccountID = reqDet.AccountID;
                        //row.ExpenseSubID = reqDet.SubID;
                    }
                }
            }
            else if (rowExt.UsrATPTEFMTranType == ATPTEFMExpenseTypeAttribute.Replenishment && (!rowExt.UsrATPTEFMIsReclassifyDoc ?? false))
            {
                ATPTEFMFundTransactionDetail ftDet = PXSelect<
                    ATPTEFMFundTransactionDetail,
                    Where<ATPTEFMFundTransactionDetail.fundTransactionRefNbr, Equal<Required<ATPTEFMFundTransactionDetail.fundTransactionRefNbr>>>>
                    .Select(Base, rowExt.UsrATPTEFMRequestRefNbr);

                if (ftDet != null)
                {
                    row.ContractID = ftDet.ProjectID;
                    row.TaskID = ftDet.ProjectTaskID;
                    row.CostCodeID = ftDet.CostCodeID;
                    //row.ExpenseAccountID = ftDet.AccountID;
                    //row.ExpenseSubID = ftDet.SubID;
                }
            }
        }
        protected virtual void _(Events.FieldUpdated<EPExpenseClaimDetails, EPExpenseClaimDetails.expenseAccountID> e, PXFieldUpdated baseMethod)
        {
            baseMethod(e.Cache, e.Args);
            e.Cache.SetDefaultExt<EPExpenseClaimDetails.expenseSubID>(e.Row);
        }
        /// <remarks>
        /// 2025-02-28---Account ID for replenishment to follow new setup---010515---RFS
        /// </remarks>
        protected virtual void _(Events.FieldDefaulting<EPExpenseClaimDetails, EPExpenseClaimDetails.expenseAccountID> e, PXFieldDefaulting baseMethod)
        {
            //Set Default Account ID
            EPExpenseClaimDetails ecDet = e.Row;
            ATPTEFMEPExpenseClaimDetailsExt ecDetExt = ecDet.GetExtension<ATPTEFMEPExpenseClaimDetailsExt>();

            //Numbering liqNumbering = PXSelect<
            //    Numbering,
            //    Where<Numbering.numberingID, Equal<Required<Numbering.numberingID>>>>
            //    .Select(Base, Base.epsetup.Current.ReceiptNumberingID);

            if (ecDet == null) return;

            if (ecDetExt.UsrATPTEFMTranType != null)
            {
                ATPTEFMReqClass reqClass = ATPTEFMReqClass.Select(ecDetExt.UsrATPTEFMReqClass);
                if (ecDetExt.UsrATPTEFMTranType == ATPTEFMExpenseTypeAttribute.Liquidation && ecDetExt.UsrATPTEFMReqRef != null && ecDet.InventoryID != null)
                {
                    int? existingAccID = null;

                    ATPTEFMCAReceiptDetail caReceipt = PXSelect<
                        ATPTEFMCAReceiptDetail,
                        Where<ATPTEFMCAReceiptDetail.expenseReceiptRefNbr, Equal<@P.AsString>,
                            And<ATPTEFMCAReceiptDetail.inventoryID, Equal<@P.AsInt>>>>
                        .Select(Base, ecDet.ClaimDetailCD, ecDet.InventoryID);
                    if (caReceipt != null)
                        existingAccID = caReceipt.AccountID;
                    else
                    {
                        foreach (ATPTEFMCARequestDetail caReq in PXSelectJoin<
                            ATPTEFMCARequestDetail,
                            InnerJoin<ATPTEFMCashAdvance,
                                On<ATPTEFMCashAdvance.cashAdvanceNbr, Equal<ATPTEFMCARequestDetail.cashAdvanceNbr>>>,
                            Where<ATPTEFMCashAdvance.cashAdvanceNbr, Equal<Required<ATPTEFMCashAdvance.cashAdvanceNbr>>>>
                            .Select(Base, ecDetExt.UsrATPTEFMReqRef))
                        {
                            if (caReq.InventoryID == ecDet.InventoryID)
                            {
                                existingAccID = caReq.AccountID;
                                break;
                            }
                        }
                    }

                    if (existingAccID != null)
                    {
                        e.NewValue = existingAccID;
                        e.Cancel = true;
                    }
                    else
                    {
                        object newAccID = SetAccountID(reqClass, ecDet);
                        e.Cache.RaiseFieldUpdating<EPExpenseClaimDetails.expenseAccountID>(e.Row, ref newAccID);
                        e.NewValue = (int?)newAccID;
                        e.Cancel = true;
                    }
                }
                else if (ecDetExt.UsrATPTEFMTranType == ATPTEFMExpenseTypeAttribute.RequestforPayment)
                {
                    object newAccID = SetAccountID(reqClass, ecDet);
                    e.Cache.RaiseFieldUpdating<EPExpenseClaimDetails.expenseAccountID>(e.Row, ref newAccID);
                    e.NewValue = (int?)newAccID;
                    e.Cancel = true;
                }
                else if (ecDetExt.UsrATPTEFMTranType == ATPTEFMExpenseTypeAttribute.Replenishment && ecDetExt.UsrATPTEFMRequestRefNbr != null && ecDet.InventoryID != null)
                {
                    int? existingAccID = null;

                    ATPTEFMFundTransactionReceiptDetail ftReceipt = PXSelect<
                        ATPTEFMFundTransactionReceiptDetail,
                        Where<ATPTEFMFundTransactionReceiptDetail.expenseReceiptRefNbr, Equal<@P.AsString>,
                            And<ATPTEFMFundTransactionReceiptDetail.inventoryID, Equal<@P.AsInt>>>>
                        .Select(Base, ecDet.ClaimDetailCD, ecDet.InventoryID);

                    if (ftReceipt != null)
                        existingAccID = ftReceipt.AccountID;
                    else
                    {
                        foreach (ATPTEFMFundTransactionDetail ftDetail in PXSelect<
                            ATPTEFMFundTransactionDetail,
                            Where<ATPTEFMFundTransactionDetail.fundTransactionRefNbr, Equal<@P.AsString>>>
                            .Select(Base, ecDetExt.UsrATPTEFMRequestRefNbr))
                        {
                            if (ftDetail.InventoryID == ecDet.InventoryID)
                            {
                                existingAccID = ftDetail.AccountID;
                                break;
                            }
                        }
                    }

                    if (existingAccID != null)
                    {
                        e.NewValue = existingAccID;
                        e.Cancel = true;
                    }
                    else
                    {
                        object newAccID = SetAccountID(ATPTEFMFundPreferences.Current, ecDet, ecDetExt);
                        e.Cache.RaiseFieldUpdating<EPExpenseClaimDetails.expenseAccountID>(e.Row, ref newAccID);
                        e.NewValue = (int?)newAccID;
                        e.Cancel = true;
                    }
                }
            }
        }
        /// <remarks>
        /// Complete Override of the FieldDefaulting event for the Expense SubID field. <br/>
        /// Removed condition where event only further executes if Document refnbr is equal to New Symbol, field defaulting event of Expense Sub ID gets called when the Contract ID is changed. <br/>
        /// 2025-02-28---Add inventory as condition whether to copy parent subID or not---010515---RFS <br/>
        /// </remarks>
        protected virtual void _(Events.FieldDefaulting<EPExpenseClaimDetails, EPExpenseClaimDetails.expenseSubID> e, PXFieldDefaulting baseMethod)
        {
            EPExpenseClaimDetails row = (EPExpenseClaimDetails)e.Row;
            if (row == null) return;

            ATPTEFMEPExpenseClaimDetailsExt rowExt = row.GetExtension<ATPTEFMEPExpenseClaimDetailsExt>();

            //Numbering liqNumbering = PXSelect<
            //    Numbering,
            //    Where<Numbering.numberingID, Equal<Required<Numbering.numberingID>>>>
            //    .Select(Base, Base.epsetup.Current.ReceiptNumberingID);

            if (rowExt.UsrATPTEFMTranType != null)
            {
                ATPTEFMReqClass reqClass = ATPTEFMReqClass.Select(rowExt.UsrATPTEFMReqClass);

                if (rowExt.UsrATPTEFMTranType == ATPTEFMExpenseTypeAttribute.Liquidation && rowExt.UsrATPTEFMReqRef != null && row.InventoryID != null)
                {
                    int? existingSubID = null;

                    ATPTEFMCAReceiptDetail caReceipt = PXSelect<
                        ATPTEFMCAReceiptDetail,
                        Where<ATPTEFMCAReceiptDetail.expenseReceiptRefNbr, Equal<@P.AsString>,
                            And<ATPTEFMCAReceiptDetail.inventoryID, Equal<@P.AsInt>>>>
                        .Select(Base, row.ClaimDetailCD, row.InventoryID);
                    if (caReceipt != null)
                        existingSubID = caReceipt.SubID;
                    else
                    {
                        foreach (ATPTEFMCARequestDetail caReq in PXSelectJoin<
                            ATPTEFMCARequestDetail,
                            InnerJoin<ATPTEFMCashAdvance,
                                On<ATPTEFMCashAdvance.cashAdvanceNbr, Equal<ATPTEFMCARequestDetail.cashAdvanceNbr>>>,
                            Where<ATPTEFMCashAdvance.cashAdvanceNbr, Equal<Required<ATPTEFMCashAdvance.cashAdvanceNbr>>>>
                            .Select(Base, rowExt.UsrATPTEFMReqRef))
                        {
                            if (caReq.InventoryID == row.InventoryID)
                            {
                                existingSubID = caReq.SubID;
                                break;
                            }
                        }
                    }

                    if (existingSubID != null)
                    {
                        e.NewValue = existingSubID;
                        e.Cancel = true;
                    }
                    else
                    {
                        object newSubID = SetSubID(reqClass, row);
                        e.Cache.RaiseFieldUpdating<EPExpenseClaimDetails.expenseSubID>(e.Row, ref newSubID);
                        e.NewValue = (int?)newSubID;
                        e.Cancel = true;
                    }
                }
                else if (rowExt.UsrATPTEFMTranType == ATPTEFMExpenseTypeAttribute.RequestforPayment)
                {
                    object newSubID = SetSubID(reqClass, row);
                    e.Cache.RaiseFieldUpdating<EPExpenseClaimDetails.expenseSubID>(e.Row, ref newSubID);
                    e.NewValue = (int?)newSubID;
                    e.Cancel = true;
                }
                else if (rowExt.UsrATPTEFMTranType == ATPTEFMExpenseTypeAttribute.Replenishment && rowExt.UsrATPTEFMRequestRefNbr != null && row.InventoryID != null)
                {
                    int? existingSubID = null;

                    ATPTEFMFundTransactionReceiptDetail ftReceipt = PXSelect<
                        ATPTEFMFundTransactionReceiptDetail,
                        Where<ATPTEFMFundTransactionReceiptDetail.expenseReceiptRefNbr, Equal<@P.AsString>,
                            And<ATPTEFMFundTransactionReceiptDetail.inventoryID, Equal<@P.AsInt>>>>
                        .Select(Base, row.ClaimDetailCD, row.InventoryID);

                    if (ftReceipt != null)
                        existingSubID = ftReceipt.SubID;
                    else
                    {
                        foreach (ATPTEFMFundTransactionDetail ftDetail in PXSelect<
                            ATPTEFMFundTransactionDetail,
                            Where<ATPTEFMFundTransactionDetail.fundTransactionRefNbr, Equal<@P.AsString>>>
                            .Select(Base, rowExt.UsrATPTEFMRequestRefNbr))
                        {
                            if (ftDetail.InventoryID == row.InventoryID)
                            {
                                existingSubID = ftDetail.SubID;
                                break;
                            }
                        }
                    }

                    if (existingSubID != null)
                    {
                        e.NewValue = existingSubID;
                        e.Cancel = true;
                    }
                    else
                    {
                        object newSubID = SetSubID(ATPTEFMFundPreferences.Current, row);
                        e.Cache.RaiseFieldUpdating<EPExpenseClaimDetails.expenseSubID>(e.Row, ref newSubID);
                        e.NewValue = (int?)newSubID;
                        e.Cancel = true;
                    }
                }
            }
        }
        protected virtual void _(Events.FieldUpdated<EPExpenseClaimDetails, ATPTEFMEPExpenseClaimDetailsExt.usrATPTEFMReqRef> e)
        {
            EPExpenseClaimDetails row = e.Row;
            ATPTEFMEPExpenseClaimDetailsExt rowExt = row.GetExtension<ATPTEFMEPExpenseClaimDetailsExt>();

            if (row is null) return;

            if (rowExt.UsrATPTEFMTranType == ATPTEFMExpenseTypeAttribute.Liquidation)
            {
                ATPTEFMCashAdvance ca = PXSelect<
                    ATPTEFMCashAdvance,
                    Where<ATPTEFMCashAdvance.cashAdvanceNbr, Equal<Required<ATPTEFMCashAdvance.cashAdvanceNbr>>>>
                    .Select(Base, rowExt.UsrATPTEFMReqRef);

                if (ca != null)
                {
                    row.CuryID = ca.CuryID;
                    row.CuryInfoID = ca.CuryInfoID;
                }
            }
        }

        /// <remarks>
        /// 010924 - Taxes should appear in the Taxes tab if expense item is entered before the Vendor field. {JCL} 
        /// </remarks>       
        protected virtual void _(Events.FieldUpdated<EPExpenseClaimDetails, ATPTEFMEPExpenseClaimDetailsExt.usrATPTVendorID> e)
        {
            EPExpenseClaimDetails row = e.Row;
            if (row is null) return;
            ATPTEFMEPExpenseClaimDetailsExt rowExt = row.GetExtension<ATPTEFMEPExpenseClaimDetailsExt>();

            VendorR vendor = PXSelect<
                VendorR,
                Where<VendorR.bAccountID, Equal<Required<VendorR.bAccountID>>>>
                .Select(this.Base, rowExt.UsrATPTVendorID);

            if (vendor != null)
            {
                Address address = PXSelect<
                    Address,
                    Where<Address.addressID, Equal<Required<Address.addressID>>>>
                    .Select(this.Base, vendor.DefAddressID);

                Location location = PXSelect<
                    Location,
                    Where<Location.locationID, Equal<Required<Location.locationID>>>>
                    .Select(this.Base, vendor.DefLocationID);

                Base.ClaimDetails.Cache.SetValueExt<Extensions.DAC.ATPTEFMEPExpenseClaimDetailsExt.usrATPTEFMDetailVendorID>(row, vendor.BAccountID);
                Base.ClaimDetails.Cache.SetValueExt<Extensions.DAC.ATPTEFMEPExpenseClaimDetailsExt.usrATPTVendID>(row, vendor.AcctCD);
                Base.ClaimDetails.Cache.SetValueExt<Extensions.DAC.ATPTEFMEPExpenseClaimDetailsExt.usrATPTVendName>(row, vendor.AcctName);
                Base.ClaimDetails.Cache.SetValueExt<Extensions.DAC.ATPTEFMEPExpenseClaimDetailsExt.usrATPTAddress>(row, address?.AddressLine1);
                Base.ClaimDetails.Cache.SetValueExt<Extensions.DAC.ATPTEFMEPExpenseClaimDetailsExt.usrATPTVendTIN>(row, location?.TaxRegistrationID);

                #region Retrigger Tax Zone
                Base.Caches[typeof(EPExpenseClaimDetails)].RaiseFieldUpdated<EPExpenseClaimDetails.taxZoneID>(e.Row, null);
                Base.Caches[typeof(EPExpenseClaimDetails)].SetValueExt<EPExpenseClaimDetails.taxZoneID>(e.Row, location?.VTaxZoneID);
                #endregion
            }
        }
        public void _(Events.FieldUpdated<EPExpenseClaimDetails, ATPTEFMEPExpenseClaimDetailsExt.usrATPTVendID> e)
        {
            EPExpenseClaimDetails row = e.Row;
            if (row == null) return;
            ATPTEFMEPExpenseClaimDetailsExt efmExt = row.GetExtension<ATPTEFMEPExpenseClaimDetailsExt>();

            VendorR vendor = PXSelect<
                VendorR,
                Where<VendorR.acctCD, Equal<Required<VendorR.acctCD>>>>
                .Select(this.Base, efmExt.UsrATPTVendID);

            if (vendor != null)
            {
                Address address = PXSelect<
                    Address,
                    Where<Address.addressID, Equal<Required<Address.addressID>>>>
                    .Select(this.Base, vendor.DefAddressID);

                Location location = PXSelect<
                    Location,
                    Where<Location.locationID, Equal<Required<Location.locationID>>>>
                    .Select(this.Base, vendor.DefLocationID);

                if (efmExt.UsrATPTEFMTranType == ATPTEFMExpenseTypeAttribute.RequestforPayment)
                    efmExt.UsrATPTVendorID = vendor.BAccountID;

                efmExt.UsrATPTEFMDetailVendorID = vendor.BAccountID;
                efmExt.UsrATPTEFMVendorBAccountID = vendor.BAccountID;
                Base.ClaimDetails.Cache.SetValueExt<Extensions.DAC.ATPTEFMEPExpenseClaimDetailsExt.usrATPTVendName>(row, vendor.AcctName);
                Base.ClaimDetails.Cache.SetValueExt<Extensions.DAC.ATPTEFMEPExpenseClaimDetailsExt.usrATPTAddress>(row, address?.AddressLine1);
                Base.ClaimDetails.Cache.SetValueExt<Extensions.DAC.ATPTEFMEPExpenseClaimDetailsExt.usrATPTVendTIN>(row, location?.TaxRegistrationID);
                row.TaxZoneID = location.VTaxZoneID;
            }
        }
        protected virtual void _(Events.FieldUpdated<EPExpenseClaimDetails, ATPTEFMEPExpenseClaimDetailsExt.usrATPTEFMTranType> e)
        {
            EPExpenseClaimDetails row = e.Row;
            ATPTEFMEPExpenseClaimDetailsExt rowExt = row.GetExtension<ATPTEFMEPExpenseClaimDetailsExt>();
            if (row is null) return;
            if (Base.ClaimDetails.Cache.GetStatus(Base.ClaimDetails.Current) == PXEntryStatus.Updated)
            {
                row.ClaimDetailCD = AutoNumberAttribute.GetNewNumberSymbol<EPSetup.receiptNumberingID>(Base.ClaimDetails.Cache, row);
                Base.ClaimDetails.Cache.Clear();
                Base.CurrentClaimDetails.Cache.Clear();
            }
        }
        protected virtual void _(Events.FieldUpdated<EPExpenseClaimDetails, ATPTEFMEPExpenseClaimDetailsExt.usrATPTEFMRequestRefNbr> e)
        {
            EPExpenseClaimDetails row = e.Row;
            ATPTEFMEPExpenseClaimDetailsExt rowExt = row.GetExtension<ATPTEFMEPExpenseClaimDetailsExt>();
            if (row is null) return;

            if (rowExt.UsrATPTEFMTranType.Equals(ATPTEFMExpenseTypeAttribute.Replenishment) && e.ExternalCall)
            {
                ATPTEFMFund fund = PXSelectJoin<
                    ATPTEFMFund,
                    InnerJoin<ATPTEFMFundTransaction,
                        On<ATPTEFMFundTransaction.fundID, Equal<ATPTEFMFund.fundCD>>>,
                    Where<ATPTEFMFundTransaction.refNbr, Equal<Required<ATPTEFMFundTransaction.refNbr>>>>
                    .Select(Base, rowExt.UsrATPTEFMRequestRefNbr);

                if (fund == null) return;

                row.CuryID = fund.CuryID;
                row.CuryInfoID = fund.CuryInfoID;
            }
        }
        #endregion

        #region Action

        public PXAction<EPExpenseClaimDetails> ATPTEFMSubmitReceipt;
        /// <remarks>
        /// 2024-09-13 : Extract the code to insert transaction history of funds to another method <b>UpdateFundsAndHistory</b>. {RRS} <br/>
        /// 2024-10-07 : Extract the code to update CA receipt due to redunduncy <b>UpdateCAReceiptDetail</b>, implemented while fixing case 007915 {RRS} <br/>
        /// 2024-11-14 : Passing the value of current line description to FT or CA receipt line description {JLTG} <br/>
        /// 2024-12-12 : Fix for discrepancy in unliquidated amount if requested amount is greater than liquidation amount CASE: 009222 {JLTG}
        /// </remarks>
        [PXProcessButton]
        [PXUIField(DisplayName = "Submit Receipt")]
        public IEnumerable aTPTEFMSubmitReceipt(PXAdapter adapter)
        {
            ATPTEFMHelper.StartLongOperation(Base, adapter, delegate ()
            {
                using (PXTransactionScope ts = new PXTransactionScope())
                {
                    EPExpenseClaimDetails row = Base.ClaimDetails.Current;
                    ATPTEFMCASetup caSetup = ATPTEFMPreferences.Select();
                    ATPTEFMSetup fundSetup = ATPTEFMFundPreferences.Current;

                    ATPTEFMEPExpenseClaimDetailsExt rowExt = row.GetExtension<ATPTEFMEPExpenseClaimDetailsExt>();

                    if (rowExt.UsrATPTEFMTranType == ATPTEFMExpenseTypeAttribute.Replenishment)
                    {
                        ATPTEFMFundTransactionEntry fundTransactionEntry = PXGraph.CreateInstance<ATPTEFMFundTransactionEntry>();

                        fundTransactionEntry.FundTransactions.Current = PXSelect<
                            ATPTEFMFundTransaction,
                            Where<ATPTEFMFundTransaction.refNbr, Equal<Required<ATPTEFMFundTransaction.refNbr>>>>
                            .Select(Base, rowExt.UsrATPTEFMRequestRefNbr);

                        int rowCount = fundTransactionEntry.FundTransactionReceiptLines.Select().Count() + 1;

                        bool isFundRequest = (fundTransactionEntry.FundTransactions.Current.FundTransactionType.Equals(ATPTEFMFundTransactionTypeAttribute.CashAdvanceValue)) ? true : false;
                        bool isUnliquidated = (fundTransactionEntry.FundTransactions.Current.CashAdvanceStatus.Equals(ATPTEFMFundTransactionCashAdvanceStatusAttribute.UnliquidatedValue));


                        ATPTEFMFundTransactionReceiptDetail receiptLines = PXSelect<
                            ATPTEFMFundTransactionReceiptDetail,
                            Where<ATPTEFMFundTransactionReceiptDetail.expenseReceiptRefNbr, Equal<Required<ATPTEFMFundTransactionReceiptDetail.expenseReceiptRefNbr>>>>
                            .Select(Base, row.ClaimDetailCD);

                        if (receiptLines == null)
                        {
                            #region Calculate total tax withholding 
                            decimal totalWhtAmt = decimal.Zero;

                            foreach (EPTaxTran tax in Base.Taxes.Select())
                            {
                                Tax tx = PXSelect<
                                    Tax,
                                    Where<Tax.taxID, Equal<Required<Tax.taxID>>>>
                                    .Select(Base, tax.TaxID);

                                if (tx != null && tx.TaxType == CSTaxType.Withholding)
                                {
                                    totalWhtAmt += (decimal)tax.CuryTaxAmt;
                                }
                            }
                            #endregion

                            #region Get current Fund view
                            ATPTEFMFund.Current = ATPTEFMFund.Select(fundTransactionEntry.FundTransactions.Current.FundID);
                            #endregion

                            if (ATPTEFMFund.Current.FundAmt == null || ATPTEFMFund.Current.CuryFundAmt == decimal.Zero)
                            {
                                throw new PXException(Messages.ATPTEFMMessages.ImportFundFirst);
                            }

                            #region Insert Receipt to Funds Summary and Transaction History
                            UpdateFundsAndHistory(row, rowExt, fundTransactionEntry, rowCount, isFundRequest, totalWhtAmt);
                            #endregion

                            #region Insert data to Fund transaction receipt details
                            ATPTEFMFundTransactionReceiptDetail receipts = fundTransactionEntry.FundTransactionReceiptLines.Insert(new ATPTEFMFundTransactionReceiptDetail
                            {
                                ExpenseReceiptRefNbr = row.ClaimDetailCD,
                                InventoryID = row.InventoryID,
                                LineDescription = row.TranDesc,
                                Date = row.ExpenseDate,
                                Qty = row.Qty,
                                UnitCost = row.UnitCost,
                                NetQty = (isFundRequest) ? row.Qty : 0,
                                NetUnitCost = (isFundRequest) ? row.CuryUnitCost : 0,
                                WhtAmount = totalWhtAmt,
                                AccountID = row.ExpenseAccountID,
                                RefNbr = row.ExpenseRefNbr,
                                SubID = row.ExpenseSubID,
                                ProjectID = row.ContractID,
                                ProjectTaskID = row.TaskID,
                                VendID = rowExt.UsrATPTEFMDetailVendorID,
                                VendorName = rowExt.UsrATPTVendName,
                                VendorAddress = rowExt.UsrATPTAddress,
                                VendorTin = rowExt.UsrATPTVendTIN,
                                TaxZoneID = row.TaxZoneID,
                                TaxCategoryID = row.TaxCategoryID,
                                CostCodeID = row.CostCodeID,
                                FundTransactionRefNbr = rowExt.UsrATPTEFMRequestRefNbr,
                            });
                            fundTransactionEntry.FundTransactionReceiptLines.Update(receipts);

                            fundTransactionEntry.FundTransactions.Current.Step = ATPTEFMFundTransactionStepAttribute.SubmitReceiptValue;
                            fundTransactionEntry.FundTransactions.UpdateCurrent();
                            fundTransactionEntry.Save.Press();

                            #endregion
                        }
                    }

                    if (rowExt.UsrATPTEFMTranType == ATPTEFMExpenseTypeAttribute.Liquidation)
                    {
                        #region Cash Advance
                        ATPTEFMCARequestDetail requestDetail = PXSelect<
                                                                            ATPTEFMCARequestDetail,
                                                                            Where<ATPTEFMCARequestDetail.cashAdvanceNbr, Equal<Required<ATPTEFMCARequestDetail.cashAdvanceNbr>>,
                                                                                And<ATPTEFMCARequestDetail.inventoryID, Equal<Required<ATPTEFMCARequestDetail.inventoryID>>>>>
                                                                            .Select(Base, rowExt.UsrATPTEFMReqRef, row.InventoryID)
                                                                            .TopFirst;

                        int? CARequestDetailID = requestDetail?.CashAdvanceRequestDetailID ?? 0;

                        ATPTEFMCashAdvanceEntry graph = PXGraph.CreateInstance<ATPTEFMCashAdvanceEntry>();
                        //graph.CashAdvances.Current = graph.CashAdvances.Search<ATPTEFMCashAdvance.cashAdvanceNbr, ATPTEFMCashAdvance.reqClassID>(rowExt.UsrATPTEFMReqRef, rowExt.UsrATPTEFMReqClass);
                        graph.CashAdvances.Current = PXSelect<
                            ATPTEFMCashAdvance,
                            Where<ATPTEFMCashAdvance.cashAdvanceNbr, Equal<Required<ATPTEFMCashAdvance.cashAdvanceNbr>>>>
                            .Select(Base, rowExt.UsrATPTEFMReqRef);

                        ATPTEFMCAReceiptDetail caReceiptDetailLines = graph.CashAdvanceReceiptLines.Search<ATPTEFMCAReceiptDetail.expenseReceiptRefNbr>(row.ClaimDetailCD);

                        Account accDesc = PXSelect<
                            Account,
                            Where<Account.accountID, Equal<Required<Account.accountID>>>>
                            .Select(Base, row.ExpenseAccountID);


                        if (graph.CashAdvances.Current.Date > row.ExpenseDate)
                            throw new Exception(Messages.ATPTEFMMessages.ReceiptDateNotLessThanCADocumentDate);

                        if (row.ExpenseDate > graph.CashAdvances.Current.LiqDate)
                            throw new Exception(Messages.ATPTEFMMessages.ReceiptDateNotGreaterThanLiqDate);


                        caReceiptDetailLines = caReceiptDetailLines ?? graph.CashAdvanceReceiptLines.Insert();

                        if (caSetup.AllowManualReceipts == true)
                        {
                            UpdateCAReceiptDetail(row, rowExt, CARequestDetailID, graph, caReceiptDetailLines, accDesc);
                        }
                        else
                        {
                            if (requestDetail == null)
                            {
                                throw new Exception(Messages.ATPTEFMMessages.RequestedItemNotTheSame);
                            }
                            if (requestDetail.InventoryID == row.InventoryID)
                            {
                                UpdateCAReceiptDetail(row, rowExt, CARequestDetailID, graph, caReceiptDetailLines, accDesc);
                            }
                        }
                        #endregion
                    }
                    ts.Complete();
                }
            });
            return adapter.Get();
        }

        /// <remarks>
        /// 2024-10-07 : Extract code from aTPTEFMSubmitReceipt() that updates CA receipt details because it is redundunt. <br/> 
        ///             Modify and simplify implementation. Updated during fix of case 007915 {RRS}
        /// </remarks>
        private void UpdateCAReceiptDetail(EPExpenseClaimDetails row, ATPTEFMEPExpenseClaimDetailsExt rowExt, int? CARequestDetailID, ATPTEFMCashAdvanceEntry graph, ATPTEFMCAReceiptDetail caReceiptDetailLines, Account accDesc)
        {
            caReceiptDetailLines.CashAdvanceRequestDetailID = CARequestDetailID;
            graph.CashAdvanceReceiptLines.Cache.SetValueExt<ATPTEFMCAReceiptDetail.inventoryID>(caReceiptDetailLines, row.InventoryID);
            graph.CashAdvanceReceiptLines.Cache.Update(caReceiptDetailLines);
            caReceiptDetailLines.ExpenseReceiptRefNbr = row.ClaimDetailCD;
            caReceiptDetailLines.TaxZoneID = row.TaxZoneID;
            caReceiptDetailLines.TaxCategoryID = row.TaxCategoryID;
            caReceiptDetailLines.AtcCode = GetDefATC(row);
            caReceiptDetailLines.Date = row.ExpenseDate;
            caReceiptDetailLines.ProjectID = row.ContractID;
            caReceiptDetailLines.ProjectTaskID = row.TaskID;
            caReceiptDetailLines.RefNbr = row.ExpenseRefNbr;
            caReceiptDetailLines.VendID = rowExt.UsrATPTEFMDetailVendorID;
            caReceiptDetailLines.VendorName = rowExt.UsrATPTVendName;
            caReceiptDetailLines.VendorAddress = rowExt.UsrATPTAddress;
            caReceiptDetailLines.VendorTin = rowExt.UsrATPTVendTIN;
            caReceiptDetailLines.NetQty = row.Qty;
            caReceiptDetailLines.CuryNetUnitCost = row.CuryUnitCost;
            caReceiptDetailLines.AccountID = row.ExpenseAccountID;
            caReceiptDetailLines.AccountDescription = accDesc.Description;
            caReceiptDetailLines.SubID = row.ExpenseSubID;
            caReceiptDetailLines.LineDescription = row.TranDesc;
            graph.CashAdvanceReceiptLines.Update(caReceiptDetailLines);
            PXNoteAttribute.CopyNoteAndFiles(Base.ClaimDetails.Cache, row, graph.CashAdvanceReceiptLines.Cache, caReceiptDetailLines);
            graph.Save.Press();
        }

        /// <remarks>
        /// 2024-09-13 : Extract this code from submit receipt to isolate function. {RRS}
        /// </remarks>
        private void UpdateFundsAndHistory(EPExpenseClaimDetails row, ATPTEFMEPExpenseClaimDetailsExt rowExt, ATPTEFMFundTransactionEntry fundTransactionEntry, int rowCount, bool isFundRequest, decimal totalWhtAmt)
        {
            ATPTEFMFundTransactionHistoryView transactionHistory = ATPTEFMFundTransactionHistory.Insert();
            transactionHistory.FundRefNbr = fundTransactionEntry.FundTransactions.Current.FundID;
            transactionHistory.TransactionType = ATPTEFMTransactionHistoryView.transactionType.ExpenseReceipt;
            transactionHistory.RefNbr = row.ClaimDetailCD;
            transactionHistory.OrderDate = row.ExpenseDate;
            transactionHistory.FundBranchID = row.BranchID;
            transactionHistory.FundType = rowExt.UsrATPTEFMFundType;
            transactionHistory.TransactionDate = row.ExpenseDate;
            transactionHistory.CuryFundTransactionDocumentAmt = row.CuryExtCost;
            transactionHistory.Status = (isFundRequest) ? ATPTEFMFundStatusAttribute.OpenValue : ATPTEFMFundStatusAttribute.LiquidatedValue;
            if (isFundRequest)
            {
                transactionHistory.CuryLiquidatedAmt = decimal.Zero;
                transactionHistory.CuryUnliquidatedAmt = row.CuryExtCost.GetValueOrDefault() - totalWhtAmt;
            }
            else
            {
                transactionHistory.CuryLiquidatedAmt = row.CuryExtCost.GetValueOrDefault();
                transactionHistory.CuryUnliquidatedAmt = decimal.Zero;
            }

            transactionHistory.CuryFundReturnAmt = decimal.Zero;
            transactionHistory.IsReimbursement = !(isFundRequest) ? true : false;
            transactionHistory.ProjectID = row.ContractID;
            transactionHistory.ProjectTaskID = row.TaskID;
            transactionHistory.CostCodeID = row.CostCodeID;
            transactionHistory.CuryWithholdingTax = totalWhtAmt;
            transactionHistory.CuryBalanceAmt = ATPTEFMFund.Current.CuryBalanceAmt;
            //transactionHistory.SortNbr = $"{rowExt.UsrATPTEFMRequestRefNbr}-{rowCount}";
            transactionHistory.SortNbr = $"FT-{rowExt.UsrATPTEFMRequestRefNbr}-{rowCount}";
            ATPTEFMFundTransactionHistory.Update(transactionHistory);
            Base.Save.Press();
        }

        /// <remarks>
        /// 2025-01-08 : (KING25R1 Staging)Fund transaction: When an expense receipt is cancelled, the system must not add the fund return balance to the unliquidated field of the fund profile. CASE: 014796 {JLTG}
        /// </remarks>
        public PXAction<EPExpenseClaimDetails> ATPTEFMCancel;
        [PXProcessButton(Category = "Processing"), PXUIField(DisplayName = "Cancel")]
        public IEnumerable aTPTEFMCancel(PXAdapter adapter)
        {
            ATPTEFMHelper.StartLongOperation(Base, adapter, delegate ()
            {
                EPExpenseClaimDetails row = Base.ClaimDetails.Current;
                ATPTEFMEPExpenseClaimDetailsExt rowExt = row.GetExtension<ATPTEFMEPExpenseClaimDetailsExt>();

                using (PXTransactionScope ts = new PXTransactionScope())
                {
                    #region Liquidation
                    if (rowExt.UsrATPTEFMTranType == ATPTEFMExpenseTypeAttribute.Liquidation)
                    {
                        ATPTEFMCashAdvanceEntry caEntry = PXGraph.CreateInstance<ATPTEFMCashAdvanceEntry>();

                        ATPTEFMCAReceiptDetail details = PXSelect<
                            ATPTEFMCAReceiptDetail,
                            Where<ATPTEFMCAReceiptDetail.expenseReceiptRefNbr, Equal<Required<ATPTEFMCAReceiptDetail.expenseReceiptRefNbr>>>>
                            .Select(Base, row.ClaimDetailCD);

                        ATPTEFMCashAdvance ca = PXSelect<
                            ATPTEFMCashAdvance,
                            Where<ATPTEFMCashAdvance.cashAdvanceNbr, Equal<Required<ATPTEFMCashAdvance.cashAdvanceNbr>>>>
                            .Select(Base, details.CashAdvanceNbr);

                        if (!string.IsNullOrEmpty(row.ClaimDetailCD))
                        {
                            if (details != null)
                            {
                                caEntry.CashAdvances.Current = ca;
                                caEntry.CashAdvanceReceiptLines.Current = details;
                                details.ExpenseReceiptRefNbr = string.Empty;
                                caEntry.CashAdvanceReceiptLines.Update(details);
                                caEntry.Save.Press();
                            }
                        }
                    }

                    #endregion

                    #region Replenishment
                    if (rowExt.UsrATPTEFMTranType == ATPTEFMExpenseTypeAttribute.Replenishment)
                    {
                        #region Cancelled Receipts

                        #region Current Views
                        ATPTEFMFundTransactionHistory.Current = ATPTEFMFundTransactionHistory.Select(row.ClaimDetailCD);
                        ATPTEFMFundTransactionReceiptDetail.Current = ATPTEFMFundTransactionReceiptDetail.Select(row.ClaimDetailCD);
                        ATPTEFMFundTransactionReclassificationReceiptDetail.Current = ATPTEFMFundTransactionReclassificationReceiptDetail.Select(row.ClaimDetailCD);

                        #region Get Fund Transaction Reference Number
                        string fundTranRefNbr = (ATPTEFMFundTransactionReceiptDetail.Current != null) ? ATPTEFMFundTransactionReceiptDetail.Current.FundTransactionRefNbr
                        : ATPTEFMFundTransactionReclassificationReceiptDetail.Current.FundTransactionRefNbr;
                        #endregion

                        ATPTEFMFundTransaction.Current = ATPTEFMFundTransaction.Select(fundTranRefNbr);
                        ATPTEFMFund.Current = ATPTEFMFund.Select(ATPTEFMFundTransaction.Current.FundID);
                        #endregion

                        #region Validations
                        //Validates if siblings receipts is already added in replenishment

                        //Normal Receipt
                        bool isExistInReplenishment = PXSelectJoin<
                            ATPTEFMFundTransaction,
                            InnerJoin<ATPTEFMFundTransactionReceiptDetail,
                                On<ATPTEFMFundTransactionReceiptDetail.fundTransactionRefNbr, Equal<ATPTEFMFundTransaction.refNbr>>,
                            InnerJoin<ATPTEFMReplenishmentDetail,
                                On<ATPTEFMReplenishmentDetail.expenseReceiptNbr, Equal<ATPTEFMFundTransactionReceiptDetail.expenseReceiptRefNbr>>,
                            InnerJoin<ATPTEFMReplenishment,
                                On<ATPTEFMReplenishment.replenishmentNbr, Equal<ATPTEFMReplenishmentDetail.replenishmentNbr>>>>>,
                            Where<ATPTEFMFundTransaction.refNbr, Equal<Required<ATPTEFMFundTransaction.refNbr>>,
                            And<ATPTEFMReplenishment.status, NotEqual<ATPTEFMReplenishmentStatusAttribute.cancelledValue>>>>
                            .Select(Base, fundTranRefNbr)
                            .Count != 0;

                        //Reclass Receipt
                        bool isReclassExistInReplenishment = PXSelectJoin<
                            ATPTEFMFundTransaction,
                            InnerJoin<ATPTEFMFundTransactionReclassficationReceiptDetail,
                                On<ATPTEFMFundTransactionReclassficationReceiptDetail.fundTransactionRefNbr, Equal<ATPTEFMFundTransaction.refNbr>>,
                            InnerJoin<ATPTEFMReplenishmentDetail,
                                On<ATPTEFMReplenishmentDetail.expenseReceiptNbr, Equal<ATPTEFMFundTransactionReclassficationReceiptDetail.expenseReceiptRefNbr>>>>,
                            Where<ATPTEFMFundTransaction.refNbr, Equal<Required<ATPTEFMFundTransaction.refNbr>>>>
                            .Select(Base, fundTranRefNbr)
                            .Count != 0;

                        if (isExistInReplenishment || isReclassExistInReplenishment)
                            throw new PXException(Messages.ATPTEFMMessages.ExpenseReceiptIsAlreadyAddedInReplenishment);

                        #endregion

                        #region Conditional variables
                        bool isFundRequest = ATPTEFMFundTransactionHelper.IsFundRequest(ATPTEFMFundTransaction.Current);
                        bool isFundUnliquidated = ATPTEFMFundTransactionHelper.IsFundUnliquidated(ATPTEFMFundTransaction.Current);
                        bool isFundLiquidated = ATPTEFMFundTransactionHelper.IsFundLiquidated(ATPTEFMFundTransaction.Current);
                        bool isFundReimbursement = ATPTEFMFundTransactionHelper.IsFundReimbursement(ATPTEFMFundTransaction.Current);
                        bool isFundClosed = ATPTEFMFundTransactionHelper.IsFundStatusClosed(ATPTEFMFundTransaction.Current);
                        #endregion

                        if (ATPTEFMFundTransactionHistory.Current != null)
                        {
                            #region Do not remove this  code for future scenario of cancellation of reimbursement type
                            /* if (isFundReimbursement)
                            {
                             decimal? currentWht = CalculateTotalWHT();
                             ATPTEFMFund.Current.BalanceAmt += (ATPTEFMFundTransaction.Current.ActualSpentAmount - ATPTEFMFundTransaction.Current.TotalWhtAmount);
                             ATPTEFMFund.Current.LiquidatedAmt -= (row.CuryExtCost - currentWht);
                             ATPTEFMFund.UpdateCurrent();
                            }*/
                            #endregion

                            #region Execute running balance
                            var getTransactionHistory =
                                 PXSelect<
                                     ATPTEFMFundTransactionHistoryView,
                                     Where<ATPTEFMFundTransactionHistoryView.fundRefNbr, Equal<Required<ATPTEFMFundTransactionHistoryView.fundRefNbr>>>,
                                     OrderBy<
                                         Asc<ATPTEFMFundTransactionHistoryView.sortNbr>>>
                                     .Select(Base, ATPTEFMFundTransaction.Current.FundID);

                            string positionRefNbr = (isFundRequest) ? ATPTEFMFundTransaction.Current.RefNbr : row.ClaimDetailCD;

                            var getResult = ATPTEFMFundHistoryPaginationHelper.GetRecordPosition(getTransactionHistory.RowCast<ATPTEFMFundTransactionHistoryView>(), positionRefNbr);

                            int startIndex = getResult.StartIndex;
                            int totalRows = getResult.TotalRows;

                            decimal? runningBalance = ATPTEFMFundTransactionHistory.Current.CuryBalanceAmt;

                            foreach (ATPTEFMFundTransactionHistoryView tran in PXSelect<
                                ATPTEFMFundTransactionHistoryView,
                                Where<ATPTEFMFundTransactionHistoryView.fundRefNbr, Equal<Required<ATPTEFMFundTransactionHistoryView.fundRefNbr>>>,
                                OrderBy<
                                    Asc<ATPTEFMFundTransactionHistoryView.sortNbr>>>
                                .SelectWindowed(Base, startIndex, totalRows, ATPTEFMFundTransaction.Current.FundID))
                            {
                                ATPTEFMFundTransactionHistory.Current = ATPTEFMFundTransactionHistory.Select(tran.RefNbr);

                                if (ATPTEFMFundTransactionHistory.Current != null)
                                {
                                    #region Fund Request
                                    if (tran.TransactionType.Equals(ATPTEFMTransactionHistoryView.transactionType.FundRequest))
                                    {
                                        if (ATPTEFMFundTransaction.Current.RefNbr == tran.RefNbr)
                                        {
                                            if (isFundClosed)
                                            {
                                                if (rowExt.UsrATPTEFMIsReclassifyDoc == true)
                                                {
                                                    ATPTEFMFundTransactionHistory.Current.CuryUnliquidatedAmt = row.CuryExtCost;
                                                }
                                                else
                                                {
                                                    ATPTEFMFundTransactionHistory.Current.Status = EPExpenseClaimDetailsStatus.OpenStatus;
                                                    ATPTEFMFundTransactionHistory.Current.CuryUnliquidatedAmt = ATPTEFMFundTransaction.Current.ActualSpentAmount;
                                                    runningBalance += (ATPTEFMFundTransaction.Current.ActualSpentAmount - ATPTEFMFundTransaction.Current.TotalWhtAmount);
                                                    runningBalance -= ATPTEFMFundTransaction.Current.RequestedAmount;
                                                }
                                            }
                                            // 005986 - (CFM2023R2) Fund is unbalanced due to cancellation of document
                                            else if (ATPTEFMFundTransaction.Current.CashAdvanceStatus == ATPTEFMFundTransactionCashAdvanceStatusAttribute.ForReclassificationValue)
                                            {
                                                runningBalance -= ATPTEFMFundTransaction.Current.AmountReceived;
                                            }
                                        }
                                        else
                                        {
                                            ATPTEFMFundTransaction currentFundTransaction = PXSelect<
                                                ATPTEFMFundTransaction,
                                                Where<ATPTEFMFundTransaction.refNbr, Equal<Required<ATPTEFMFundTransaction.refNbr>>>>
                                                .Select(Base, tran.RefNbr);

                                            if (currentFundTransaction != null)
                                            {
                                                switch (currentFundTransaction.CashAdvanceStatus)
                                                {
                                                    case ATPTEFMFundTransactionCashAdvanceStatusAttribute.UnliquidatedValue:
                                                        runningBalance -= currentFundTransaction.RequestedAmount;
                                                        break;
                                                    case ATPTEFMFundTransactionCashAdvanceStatusAttribute.ForReclassificationValue:
                                                        runningBalance -= currentFundTransaction.RequestedAmount;
                                                        runningBalance += currentFundTransaction.AmountReceived;
                                                        break;
                                                    case ATPTEFMFundTransactionCashAdvanceStatusAttribute.LiquidatedValue:
                                                        runningBalance -= currentFundTransaction.ActualSpentAmount;
                                                        runningBalance += currentFundTransaction.TotalWhtAmount;
                                                        break;
                                                    case ATPTEFMFundTransactionCashAdvanceStatusAttribute.UnreleasedValue:
                                                        runningBalance += decimal.Zero;
                                                        break;
                                                    default:
                                                        runningBalance += decimal.Zero;
                                                        break;
                                                }
                                            }
                                        }

                                        ATPTEFMFundTransactionHistory.Current.CuryBalanceAmt = runningBalance;
                                        ATPTEFMFundTransactionHistory.UpdateCurrent();
                                        continue;
                                    }
                                    #endregion

                                    #region Fund Reimbursement
                                    if (tran.TransactionType.Equals(ATPTEFMTransactionHistoryView.transactionType.FundReimbursment))
                                    {

                                        if (ATPTEFMFundTransaction.Current.RefNbr == tran.RefNbr)
                                        {
                                            ATPTEFMFundTransactionHistory.Current.Status = EPExpenseClaimDetailsStatus.OpenStatus;
                                        }
                                        else
                                        {
                                            ATPTEFMFundTransaction currentFundTransaction = PXSelect<
                                                ATPTEFMFundTransaction,
                                                Where<ATPTEFMFundTransaction.refNbr, Equal<Required<ATPTEFMFundTransaction.refNbr>>>>
                                                .Select(Base, tran.RefNbr);

                                            if (currentFundTransaction != null)
                                            {
                                                runningBalance -= currentFundTransaction.ActualSpentAmount;
                                                runningBalance += currentFundTransaction.TotalWhtAmount;
                                            }
                                        }

                                        ATPTEFMFundTransactionHistory.Current.CuryBalanceAmt = runningBalance;
                                        ATPTEFMFundTransactionHistory.UpdateCurrent();
                                        continue;
                                    }
                                    #endregion

                                    #region Replenishment
                                    if (tran.TransactionType.Equals(ATPTEFMTransactionHistoryView.transactionType.Replenishment))
                                    {
                                        runningBalance += ATPTEFMFundTransactionHistory.Current.CuryCheckAmt ?? decimal.Zero;
                                        ATPTEFMFundTransactionHistory.Current.CuryBalanceAmt = runningBalance;
                                        ATPTEFMFundTransactionHistory.UpdateCurrent();
                                        continue;
                                    }
                                    #endregion

                                    #region Receipts
                                    if (tran.TransactionType.Equals(ATPTEFMTransactionHistoryView.transactionType.ExpenseReceipt))
                                    {
                                        ATPTEFMFundTransactionReceiptDetail receipt = PXSelect<
                                            ATPTEFMFundTransactionReceiptDetail,
                                            Where<ATPTEFMFundTransactionReceiptDetail.expenseReceiptRefNbr,
                                            Equal<Required<ATPTEFMFundTransactionReceiptDetail.expenseReceiptRefNbr>>>>
                                            .Select(Base, tran.RefNbr);

                                        if (receipt != null)
                                        {
                                            #region Get Fund Balance Amount
                                            decimal? fundBalanceAmount = 0m;
                                            if (isFundRequest)
                                            {
                                                ATPTEFMFundTransactionHistory.Current = ATPTEFMFundTransactionHistory.Select(receipt.FundTransactionRefNbr);
                                                if (ATPTEFMFundTransactionHistory.Current != null)
                                                {
                                                    fundBalanceAmount = ATPTEFMFundTransactionHistory.Current.CuryBalanceAmt;
                                                }
                                                ATPTEFMFundTransactionHistory.Current = ATPTEFMFundTransactionHistory.Select(tran.RefNbr);
                                            }
                                            #endregion

                                            if (receipt.FundTransactionRefNbr == ATPTEFMFundTransaction.Current.RefNbr && rowExt.UsrATPTEFMIsReclassifyDoc != true)
                                            {
                                                ATPTEFMFundTransactionHistory.Current.Status = ATPTEFMFundStatusAttribute.OpenValue;
                                                ATPTEFMFundTransactionHistory.Current.CuryWithholdingTax = decimal.Zero;
                                                ATPTEFMFundTransactionHistory.Current.CuryUnliquidatedAmt = decimal.Zero;
                                                ATPTEFMFundTransactionHistory.Current.CuryLiquidatedAmt = decimal.Zero;
                                            }

                                            ATPTEFMFundTransactionHistory.Current.CuryBalanceAmt = (isFundRequest) ? fundBalanceAmount : runningBalance;
                                            ATPTEFMFundTransactionHistory.UpdateCurrent();
                                            continue;
                                        }
                                    }

                                    #endregion

                                    #region IncreaseFund
                                    if (tran.TransactionType.Equals(ATPTEFMFundTransactionTypeAttribute.IncreaseFundValue))
                                    {
                                        if (tran.Status == APDocStatus.Closed)
                                            runningBalance += tran.CuryCheckAmt;

                                        ATPTEFMFundTransactionHistory.Current.CuryBalanceAmt = runningBalance;
                                        ATPTEFMFundTransactionHistory.UpdateCurrent();
                                        continue;
                                    }
                                    #endregion

                                    #region DecreaseFund
                                    if (tran.TransactionType.Equals(ATPTEFMFundTransactionTypeAttribute.DecreaseFundValue))
                                    {
                                        if (tran.Status == APDocStatus.Closed)
                                            runningBalance -= tran.CuryCheckAmt;

                                        ATPTEFMFundTransactionHistory.Current.CuryBalanceAmt = runningBalance;
                                        ATPTEFMFundTransactionHistory.UpdateCurrent();
                                        continue;
                                    }
                                    #endregion

                                    ATPTEFMFundTransactionHistory.Current.CuryBalanceAmt = runningBalance;
                                    ATPTEFMFundTransactionHistory.UpdateCurrent();
                                }
                            }
                            #endregion

                        }
                        #endregion

                        #region Update Fund Transaction Receipt Detail
                        decimal? oldWhtAmount = decimal.Zero;
                        if (ATPTEFMFundTransactionReceiptDetail.Current != null)
                        {
                            oldWhtAmount = ATPTEFMFundTransactionHelper.GetOldWht(ATPTEFMFundTransactionReceiptDetail.Current);
                            ATPTEFMFundTransactionReceiptDetail.Current.ExpenseReceiptRefNbr = string.Empty;
                            ATPTEFMFundTransactionReceiptDetail.Current.WhtAmount = decimal.Zero;
                            ATPTEFMFundTransactionReceiptDetail.UpdateCurrent();
                        }
                        #endregion

                        #region Update Fund Transaction Reclassification receipt
                        if (ATPTEFMFundTransactionReclassificationReceiptDetail.Current != null)
                        {
                            ATPTEFMFundTransactionReclassificationReceiptDetail.Current.ExpenseReceiptRefNbr = string.Empty;
                            ATPTEFMFundTransactionReclassificationReceiptDetail.UpdateCurrent();
                        }
                        #endregion

                        #region Set cancelled status (Fund transaction History)
                        ATPTEFMFundTransactionHistory.Current = ATPTEFMFundTransactionHistory.Select(row.ClaimDetailCD);
                        if (ATPTEFMFundTransactionHistory.Current != null && ATPTEFMFundTransactionHistory.Current.RefNbr == row.ClaimDetailCD)
                        {
                            ATPTEFMFundTransactionHistory.Current.Status = ATPTEFMExpenseReceiptStatusAttribute.CancelledValue;
                            ATPTEFMFundTransactionHistory.Current.CuryLiquidatedAmt = decimal.Zero;
                            ATPTEFMFundTransactionHistory.UpdateCurrent();
                        }
                        #endregion

                        #region Update Fund Balances

                        if (isFundRequest && isFundClosed)
                        {
                            if (rowExt.UsrATPTEFMIsReclassifyDoc == true)
                            {
                                ATPTEFMFund.Current.CuryLiquidatedAmt -= row.CuryExtCost;
                                ATPTEFMFund.Current.CuryUnliquidatedAmt += row.CuryExtCost;
                            }
                            else
                            {
                                ATPTEFMFund.Current.CuryBalanceAmt += (ATPTEFMFundTransaction.Current.ActualSpentAmount - ATPTEFMFundTransaction.Current.TotalWhtAmount);
                                ATPTEFMFund.Current.CuryBalanceAmt -= ATPTEFMFundTransaction.Current.RequestedAmount;
                                ATPTEFMFund.Current.CuryLiquidatedAmt -= (ATPTEFMFundTransaction.Current.ActualSpentAmount - ATPTEFMFundTransaction.Current.TotalWhtAmount);
                                ATPTEFMFund.Current.CuryUnliquidatedAmt += ATPTEFMFundTransaction.Current.RequestedAmount;
                            }

                            ATPTEFMFund.UpdateCurrent();
                        }
                        else if (ATPTEFMFundTransaction.Current.CashAdvanceStatus.Equals(ATPTEFMFundTransactionCashAdvanceStatusAttribute.ForReclassificationValue))
                        {
                            decimal? balanceAmt = (ATPTEFMFundTransaction.Current.ChangeAmount - ATPTEFMFundTransaction.Current.AmountReceived);

                            ATPTEFMFund.Current.CuryLiquidatedAmt -= (ATPTEFMFundTransaction.Current.ActualSpentAmount - ATPTEFMFundTransaction.Current.TotalWhtAmount);
                            ATPTEFMFund.Current.CuryBalanceAmt -= ATPTEFMFundTransaction.Current.AmountReceived;
                            ATPTEFMFund.Current.CuryUnliquidatedAmt -= balanceAmt;
                            ATPTEFMFund.Current.CuryUnliquidatedAmt += (ATPTEFMFundTransaction.Current.RequestedAmount);
                            ATPTEFMFund.UpdateCurrent();
                        }
                        #endregion

                        #region Update Fund Transaction For Reprocessing FT
                        if (ATPTEFMFundTransaction.Current != null && ATPTEFMFund.Current != null)
                        {

                            if (ATPTEFMFundTransactionReclassificationReceiptDetail.Current != null &&
                               ATPTEFMFund.Current.ATPTEFMValidateAmountReceivedAndAmountReleased == true &&
                               ATPTEFMFundTransaction.Current.ReclassificationAmt > 0)
                            {
                                ATPTEFMFundTransaction.Current.Status = ATPTEFMFundStatusAttribute.OpenValue;
                                ATPTEFMFundTransaction.Current.CashAdvanceStatus = ATPTEFMFundTransactionCashAdvanceStatusAttribute.ForReclassificationValue;
                                ATPTEFMFundTransaction.Current.ReclassificationAmt = decimal.Zero;
                            }
                            else
                            {
                                ATPTEFMFundTransaction.Current.Status = ATPTEFMFundStatusAttribute.OpenValue;
                                ATPTEFMFundTransaction.Current.CashAdvanceStatus = ATPTEFMFundTransactionCashAdvanceStatusAttribute.UnliquidatedValue;
                                ATPTEFMFundTransaction.Current.AmountReceived = decimal.Zero;
                                ATPTEFMFundTransaction.Current.AmountReleased = decimal.Zero;
                                ATPTEFMFundTransaction.Current.ReclassificationAmt = decimal.Zero;
                                ATPTEFMFundTransaction.Current.TotalWhtAmount -= oldWhtAmount;
                                if (isFundRequest)
                                {
                                    ATPTEFMFundTransaction.Current.ChangeAmount -= oldWhtAmount;
                                }

                                ATPTEFMFundTransaction.Current.Step = ATPTEFMFundTransactionStepAttribute.DefaultValue;
                            }

                            ATPTEFMFundTransaction.UpdateCurrent();

                            #region Do not remove this  code for future scenario of cancellation of reimbursement type
                            /*if (isFundReimbursement)
                            {
                                ATPTEFMFundTransactionHistory.Current = ATPTEFMFundTransactionHistory.Select(ATPTEFMFundTransaction.Current.RefNbr);

                                if (ATPTEFMFundTransactionHistory.Current != null)
                                {
                                    ATPTEFMFundTransactionHistory.DeleteCurrent();
                                }
                            }*/
                            #endregion
                        }
                        #endregion

                        #region Update Fund Transaction Fund Return Amt in Fund Transaction History
                        if (isFundRequest)
                        {
                            ATPTEFMFundTransactionHistory.Current = ATPTEFMFundTransactionHistory.Select(rowExt.UsrATPTEFMRequestRefNbr);

                            if (ATPTEFMFundTransactionHistory.Current.CuryFundReturnAmt > 0)
                            {
                                ATPTEFMFundTransactionHistory.Current.CuryFundReturnAmt = 0m;
                                ATPTEFMFundTransactionHistory.UpdateCurrent();
                            }
                        }
                        #endregion
                    }
                    #endregion

                    row.Status = ATPTEFMExpenseReceiptStatusAttribute.CancelledValue;
                    Base.ClaimDetails.Update(row);
                    Base.Save.Press();
                    ts.Complete();
                }
            });
            return adapter.Get();
        }

        #endregion

        #region Overrides
        public delegate void PersistDelegate();
        [PXOverride]
        public void Persist(PersistDelegate baseMethod)
        {
            EPExpenseClaimDetails row = Base.CurrentClaimDetails.Current;
            ATPTEFMEPExpenseClaimDetailsExt rowExt = row.GetExtension<ATPTEFMEPExpenseClaimDetailsExt>();

            if (row is null) return;

            #region Conditional variables
            bool hasChanges = Base.CurrentClaimDetails.Cache.GetStatus(Base.CurrentClaimDetails.Current) == PXEntryStatus.Updated || Base.ClaimDetails.Cache.GetStatus(Base.ClaimDetails.Current) == PXEntryStatus.Updated;
            #endregion

            #region Updates in Fund transaction, Funds History and Fund summary
            if (rowExt.UsrATPTEFMTranType == ATPTEFMExpenseTypeAttribute.Replenishment)
            {
                if (row.Status != ATPTEFMExpenseReceiptStatusAttribute.CancelledValue && hasChanges && Base.Accessinfo.ScreenID == "EP.30.10.20")
                {
                    ATPTEFMFundTransactionReceiptDetail.Current = ATPTEFMFundTransactionReceiptDetail.Select(row.ClaimDetailCD);
                    if (ATPTEFMFundTransactionReceiptDetail.Current != null)
                    {
                        ATPTEFMFundTransaction.Current = ATPTEFMFundTransaction.Select(ATPTEFMFundTransactionReceiptDetail.Current.FundTransactionRefNbr);

                        #region Old data for Fund transaction Receipt Details
                        decimal? oldWHT = ATPTEFMFundTransactionHelper.GetOldWht(ATPTEFMFundTransactionReceiptDetail.Current);
                        decimal? oldAmount = ATPTEFMFundTransactionHelper.GetOldAmount(ATPTEFMFundTransactionReceiptDetail.Current);
                        decimal? oldNetAmount = ATPTEFMFundTransactionHelper.GetOldtNetAmt(ATPTEFMFundTransactionReceiptDetail.Current);
                        #endregion

                        #region Old data for Fund summary balances
                        decimal? oldActualSpentAmt = ATPTEFMFundTransactionHelper.GetOldActualSpentAmt(ATPTEFMFundTransaction.Current);
                        decimal? oldTotalWHT = ATPTEFMFundTransactionHelper.GetOldTotalWht(ATPTEFMFundTransaction.Current);

                        #endregion

                        //UpdateFundTranscationReceiptDetailRecord(row);

                        if (ATPTEFMFundTransactionReceiptDetail.Current != null)
                        {
                            UpdateFundTranscationReceiptDetailRecord(row);

                            if (ATPTEFMFundTransaction.Current != null)
                            {
                                #region Update Fund Transactions
                                UpdateFundReimbursementTransactionRecord(row, oldWHT, oldActualSpentAmt, oldAmount);
                                UpdateFundRequestTransactionRecord(row, oldWHT);
                                #endregion                          

                                #region Update Fund Summary
                                ATPTEFMFundTransaction fundTran = ATPTEFMFundTransaction.Current;
                                ATPTEFMFund.Current = ATPTEFMFund.Current = ATPTEFMFund.Select(fundTran.FundID);
                                ATPTEFMFund fund = ATPTEFMFund.Current;

                                bool isValidateAmountReceivedAndAmountReleased = (fund.ATPTEFMValidateAmountReceivedAndAmountReleased ?? false);

                                if (isValidateAmountReceivedAndAmountReleased
                                    && fundTran.CashAdvanceStatus != ATPTEFMFundTransactionCashAdvanceStatusAttribute.UnliquidatedValue
                                    && (fundTran != null && ((fundTran.ChangeAmount - fundTran.AmountReceived) + fundTran.AmountReleased) < 0m))
                                {
                                    throw new PXException(ATPTEFMMessages.UnableToProceedBalanceNegative);
                                }

                                UpdateFundRequestUnliquidatedField(oldActualSpentAmt, oldTotalWHT);
                                UpdateFundSummaryBalances(oldActualSpentAmt, oldTotalWHT, oldAmount, oldNetAmount, oldWHT);

                                #endregion

                                #region Update Funds History
                                UpdateReceiptTransactionHistoryRecord(row);
                                UpdateFundTransactionHistoryRecord(fundTran, isValidateAmountReceivedAndAmountReleased);
                                #endregion
                            }
                        }
                        #region Transaction History Running Balance
                        if (ATPTEFMFundTransaction.Current != null)
                        {
                            #region Variables
                            ATPTEFMFundTransaction fundTransaction = ATPTEFMFundTransaction.Current;
                            bool isReadyForComputation = false;
                            bool isFundLiquidated = ATPTEFMFundTransactionHelper.IsFundLiquidated(ATPTEFMFundTransaction.Current);
                            bool isFundRequest = ATPTEFMFundTransactionHelper.IsFundRequest(ATPTEFMFundTransaction.Current);
                            bool isFundReimbursement = ATPTEFMFundTransactionHelper.IsFundReimbursement(ATPTEFMFundTransaction.Current);
                            bool isReleaseCash = ATPTEFMFundTransactionHelper.IsReleaseCash(ATPTEFMFundTransaction.Current);
                            bool isFundStatusClosed = ATPTEFMFundTransactionHelper.IsFundStatusClosed(ATPTEFMFundTransaction.Current);
                            #endregion

                            #region Conditions for executing running balance
                            if (isFundRequest && isFundLiquidated && isFundStatusClosed)
                            {
                                isReadyForComputation = true;
                            }

                            if (isFundReimbursement && isReleaseCash && isFundStatusClosed)
                            {
                                isReadyForComputation = true;
                            }

                            if (fundTransaction.CashAdvanceStatus.Equals(ATPTEFMFundTransactionCashAdvanceStatusAttribute.ForReclassificationValue))
                            {
                                isReadyForComputation = true;
                            }
                            #endregion

                            if (isReadyForComputation)
                            {
                                var getTransactionHistory = PXSelect<
                                    ATPTEFMFundTransactionHistoryView,
                                    Where<ATPTEFMFundTransactionHistoryView.fundRefNbr, Equal<Required<ATPTEFMFundTransactionHistoryView.fundRefNbr>>>,
                                    OrderBy<
                                        Asc<ATPTEFMFundTransactionHistoryView.sortNbr>>>
                                    .Select(Base, ATPTEFMFundTransaction.Current.FundID);

                                var getResult = ATPTEFMFundHistoryPaginationHelper.GetRecordPosition(getTransactionHistory.RowCast<ATPTEFMFundTransactionHistoryView>(), fundTransaction.RefNbr);

                                int startIndex = getResult.StartIndex;
                                int totalRows = getResult.TotalRows;
                                var prevRecord = (ATPTEFMFundTransactionHistoryView)getResult.PreviousRecord;

                                decimal? runningBalance = prevRecord.CuryBalanceAmt;

                                foreach (ATPTEFMFundTransactionHistoryView tran in PXSelect<
                                    ATPTEFMFundTransactionHistoryView,
                                    Where<ATPTEFMFundTransactionHistoryView.fundRefNbr, Equal<Required<ATPTEFMFundTransactionHistoryView.fundRefNbr>>>,
                                    OrderBy<
                                        Asc<ATPTEFMFundTransactionHistoryView.sortNbr>>>
                                    .SelectWindowed(Base, startIndex, totalRows, fundTransaction.FundID))
                                {
                                    ATPTEFMFundTransactionHistory.Current = ATPTEFMFundTransactionHistory.Select(tran.RefNbr);

                                    if (ATPTEFMFundTransactionHistory.Current != null)
                                    {
                                        #region Fund Request
                                        if (tran.TransactionType.Equals(ATPTEFMTransactionHistoryView.transactionType.FundRequest))
                                        {
                                            if (fundTransaction.RefNbr == tran.RefNbr)
                                            {
                                                decimal? fundTransactionBalance = ((fundTransaction.ChangeAmount - fundTransaction.AmountReceived) + fundTransaction.AmountReleased);
                                                //decimal? fundTransactionBalance = (fundTransaction.RequestedAmount - fundTransaction.ActualSpentAmount) + fundTransaction.TotalWhtAmount;
                                                if ((ATPTEFMFund.Current.ATPTEFMValidateAmountReceivedAndAmountReleased ?? false) && (fundTransactionBalance > 0m))
                                                {
                                                    runningBalance -= (fundTransaction.ActualSpentAmount + (fundTransactionBalance - fundTransaction.TotalWhtAmount));

                                                    #region Update Unliquidated and Fund Return
                                                    ATPTEFMFundTransactionHistory.Current.CuryUnliquidatedAmt = fundTransactionBalance;
                                                    ATPTEFMFundTransactionHistory.Current.CuryFundReturnAmt = fundTransaction.AmountReceived;
                                                    #endregion
                                                }
                                                else
                                                {
                                                    runningBalance -= fundTransaction.ActualSpentAmount;
                                                    runningBalance += fundTransaction.TotalWhtAmount;

                                                    #region Update Unliquidated and Fund Return
                                                    ATPTEFMFundTransactionHistory.Current.CuryUnliquidatedAmt = decimal.Zero;
                                                    ATPTEFMFundTransactionHistory.Current.CuryFundReturnAmt = fundTransaction.ChangeAmount;
                                                    #endregion
                                                }
                                            }
                                            else
                                            {
                                                ATPTEFMFundTransaction.Current = ATPTEFMFundTransaction.Select(tran.RefNbr);

                                                if (ATPTEFMFundTransaction.Current != null)
                                                {
                                                    switch (ATPTEFMFundTransaction.Current.CashAdvanceStatus)
                                                    {
                                                        case ATPTEFMFundTransactionCashAdvanceStatusAttribute.UnliquidatedValue:
                                                            runningBalance -= ATPTEFMFundTransaction.Current.RequestedAmount;
                                                            break;
                                                        case ATPTEFMFundTransactionCashAdvanceStatusAttribute.ForReclassificationValue:
                                                            runningBalance -= ATPTEFMFundTransaction.Current.RequestedAmount;
                                                            runningBalance += ATPTEFMFundTransaction.Current.AmountReceived;
                                                            break;
                                                        case ATPTEFMFundTransactionCashAdvanceStatusAttribute.LiquidatedValue:
                                                            runningBalance -= ATPTEFMFundTransaction.Current.ActualSpentAmount;
                                                            runningBalance += ATPTEFMFundTransaction.Current.TotalWhtAmount;
                                                            runningBalance -= ATPTEFMFundTransaction.Current.ReclassificationAmt;
                                                            break;
                                                        case ATPTEFMFundTransactionCashAdvanceStatusAttribute.UnreleasedValue:
                                                            runningBalance += decimal.Zero;
                                                            break;
                                                        default:
                                                            runningBalance += decimal.Zero;
                                                            break;
                                                    }
                                                }
                                            }

                                            ATPTEFMFundTransactionHistory.Current.CuryBalanceAmt = runningBalance;
                                            ATPTEFMFundTransactionHistory.UpdateCurrent();
                                            continue;
                                        }
                                        #endregion

                                        #region Fund Reimbursement
                                        if (tran.TransactionType.Equals(ATPTEFMTransactionHistoryView.transactionType.FundReimbursment))
                                        {
                                            ATPTEFMFundTransaction.Current = ATPTEFMFundTransaction.Select(tran.RefNbr);

                                            if (ATPTEFMFundTransaction.Current != null)
                                            {
                                                runningBalance -= ATPTEFMFundTransaction.Current.ActualSpentAmount;
                                                runningBalance += ATPTEFMFundTransaction.Current.TotalWhtAmount;
                                            }

                                            ATPTEFMFundTransactionHistory.Current.CuryBalanceAmt = runningBalance;
                                            ATPTEFMFundTransactionHistory.UpdateCurrent();
                                            continue;
                                        }
                                        #endregion

                                        #region Replenishment
                                        if (tran.TransactionType.Equals(ATPTEFMTransactionHistoryView.transactionType.Replenishment))
                                        {
                                            runningBalance += ATPTEFMFundTransactionHistory.Current.CuryCheckAmt ?? decimal.Zero;
                                            ATPTEFMFundTransactionHistory.Current.CuryBalanceAmt = runningBalance;
                                            ATPTEFMFundTransactionHistory.UpdateCurrent();
                                            continue;
                                        }
                                        #endregion

                                        #region IncreaseFund
                                        if (tran.TransactionType.Equals(ATPTEFMFundTransactionTypeAttribute.IncreaseFundValue))
                                        {
                                            if (tran.Status == APDocStatus.Closed)
                                                runningBalance += tran.CuryCheckAmt;

                                            ATPTEFMFundTransactionHistory.Current.CuryBalanceAmt = runningBalance;
                                            ATPTEFMFundTransactionHistory.UpdateCurrent();
                                            continue;
                                        }
                                        #endregion

                                        #region DecreaseFund
                                        if (tran.TransactionType.Equals(ATPTEFMFundTransactionTypeAttribute.DecreaseFundValue))
                                        {
                                            if (tran.Status == APDocStatus.Closed)
                                                runningBalance -= tran.CuryCheckAmt;

                                            ATPTEFMFundTransactionHistory.Current.CuryBalanceAmt = runningBalance;
                                            ATPTEFMFundTransactionHistory.UpdateCurrent();
                                            continue;
                                        }
                                        #endregion

                                        ATPTEFMFundTransactionHistory.Current.CuryBalanceAmt = runningBalance;
                                        ATPTEFMFundTransactionHistory.UpdateCurrent();
                                    }
                                }
                            }
                        }
                        #endregion
                    }
                }
            }
            #endregion

            baseMethod();

            #region Updates in Cash Advance
            if (rowExt.UsrATPTEFMTranType == ATPTEFMExpenseTypeAttribute.Liquidation && hasChanges)
            {
                ATPTEFMCashAdvanceEntry caEntry = PXGraph.CreateInstance<ATPTEFMCashAdvanceEntry>();
                caEntry.Clear();

                var accDesc = ATPTEFMAccountDescription.Select(row.ExpenseAccountID);

                caEntry.CashAdvances.Current = PXSelect<
                    ATPTEFMCashAdvance,
                    Where<ATPTEFMCashAdvance.cashAdvanceNbr, Equal<Required<ATPTEFMCashAdvance.cashAdvanceNbr>>>>
                    .Select(Base, rowExt.UsrATPTEFMReqRef);

                ATPTEFMCAReceiptDetail caReceiptDetails = PXSelect<
                    ATPTEFMCAReceiptDetail,
                    Where<ATPTEFMCAReceiptDetail.expenseReceiptRefNbr, Equal<Required<ATPTEFMCAReceiptDetail.expenseReceiptRefNbr>>>>
                    .Select(Base, row.ClaimDetailCD);

                if (caReceiptDetails != null)
                {
                    caEntry.CashAdvanceReceiptLines.Current = caReceiptDetails;
                    caReceiptDetails.InventoryID = row.InventoryID;
                    caReceiptDetails.ExpenseReceiptRefNbr = row.ClaimDetailCD;
                    caReceiptDetails.Date = row.ExpenseDate;
                    caReceiptDetails.ProjectID = row.ContractID;
                    caReceiptDetails.ProjectTaskID = row.TaskID;
                    caReceiptDetails.RefNbr = row.ExpenseRefNbr;
                    caReceiptDetails.VendID = rowExt.UsrATPTEFMDetailVendorID;
                    caReceiptDetails.VendorName = rowExt.UsrATPTVendName;
                    caReceiptDetails.VendorAddress = rowExt.UsrATPTAddress;
                    caReceiptDetails.VendorTin = rowExt.UsrATPTVendTIN;
                    caReceiptDetails.NetQty = row.Qty;
                    caReceiptDetails.CuryNetUnitCost = row.CuryUnitCost;
                    caReceiptDetails.AccountID = row.ExpenseAccountID;
                    caReceiptDetails.AccountDescription = accDesc.TopFirst.Description;
                    caReceiptDetails.SubID = row.ExpenseSubID;
                    caReceiptDetails.TaxZoneID = row.TaxZoneID;
                    caReceiptDetails.TaxCategoryID = row.TaxCategoryID;
                    caReceiptDetails.AtcCode = rowExt.UsrATPTEFMATCCode;
                    caReceiptDetails.LineDescription = row.TranDesc;
                    caEntry.CashAdvanceReceiptLines.Update(caReceiptDetails);

                    caEntry.Save.Press();
                }
            }
            #endregion

            #region Updates and insertion of taxes in Replenishment
            if (rowExt.UsrATPTEFMTranType == ATPTEFMExpenseTypeAttribute.Replenishment)
            {
                ATPTEFMReplenishmentEntry repEntry = PXGraph.CreateInstance<ATPTEFMReplenishmentEntry>();
                repEntry.Clear();

                PXResultset<ATPTEFMReplenishmentDetail, ATPTEFMReplenishment> replenishDetails = PXSelectJoin<
                    ATPTEFMReplenishmentDetail,
                    InnerJoin<ATPTEFMReplenishment,
                        On<ATPTEFMReplenishment.replenishmentNbr, Equal<ATPTEFMReplenishmentDetail.replenishmentNbr>,
                        And<ATPTEFMReplenishment.status, NotEqual<ATPTEFMReplenishmentStatusAttribute.cancelledValue>>>>,
                    Where<ATPTEFMReplenishmentDetail.expenseReceiptNbr, Equal<Required<ATPTEFMReplenishmentDetail.expenseReceiptNbr>>>>
                    .Select<PXResultset<ATPTEFMReplenishmentDetail, ATPTEFMReplenishment>>(Base, row.ClaimDetailCD);

                if (replenishDetails.Count > decimal.Zero)
                {
                    ATPTEFMReplenishment currentReplenishment = (ATPTEFMReplenishment)replenishDetails;
                    repEntry.Replenishments.Current = currentReplenishment;

                    List<EPTaxTran> taxLists = new List<EPTaxTran>();
                    HashSet<string> uniqueTaxList = new HashSet<string>();

                    decimal? totalVatAmt = 0M;
                    decimal? totalWhtAmt = 0M;
                    decimal? totalClaim = 0M;

                    foreach (ATPTEFMReplenishmentDetail receipt in repEntry.ReplenishmentDetails.Select())
                    {
                        EPExpenseClaimDetails ecDetails = PXSelect<
                            EPExpenseClaimDetails,
                            Where<EPExpenseClaimDetails.claimDetailCD, Equal<Required<EPExpenseClaimDetails.claimDetailCD>>>>
                            .Select(Base, receipt.ExpenseReceiptNbr);

                        if (ecDetails != null)
                        {
                            totalClaim += ecDetails.CuryExtCost;

                            PXResultset<EPTaxTran> taxes = PXSelectJoinGroupBy<
                                EPTaxTran,
                                InnerJoin<Tax,
                                    On<Tax.taxID, Equal<EPTaxTran.taxID>>>,
                                Where<EPTaxTran.claimDetailID, Equal<Required<EPExpenseClaimDetails.claimDetailID>>>,
                                Aggregate<
                                    GroupBy<Tax.taxID,
                                        Sum<EPTaxTran.curyTaxAmt,
                                        Sum<EPTaxTran.curyTaxableAmt>>>>>
                                .Select(Base, ecDetails.ClaimDetailID);

                            foreach (PXResult<EPTaxTran, Tax> t in taxes)
                            {
                                EPTaxTran epTax = t;
                                Tax tax = t;

                                if (tax.TaxType == CSTaxType.VAT)
                                    totalVatAmt += epTax.CuryTaxAmt;

                                if (tax.TaxType == CSTaxType.Withholding)
                                    totalWhtAmt += epTax.CuryTaxAmt;

                                taxLists.Add(epTax);
                            }
                        }
                    }

                    var groupedTaxes = taxLists.GroupBy(x => x.TaxID)
                       .Select(group => new EPTaxTran
                       {
                           TaxID = group.Key,
                           TaxRate = group.First().TaxRate,
                           CuryTaxAmt = group.Sum(t => t.CuryTaxAmt),
                           CuryTaxableAmt = group.Sum(t => t.CuryTaxableAmt),
                       })
                       .ToList();

                    foreach (var receiptWithTax in groupedTaxes)
                    {
                        uniqueTaxList.Add(receiptWithTax.TaxID);

                        ATPTEFMReplenishmentTaxDetail detExist = PXSelect<
                            ATPTEFMReplenishmentTaxDetail,
                            Where<ATPTEFMReplenishmentTaxDetail.replenishmentNbr, Equal<Required<ATPTEFMReplenishmentTaxDetail.replenishmentNbr>>,
                                And<ATPTEFMReplenishmentTaxDetail.taxID, Equal<Required<ATPTEFMReplenishmentTaxDetail.taxID>>>>>
                            .Select(Base, currentReplenishment.ReplenishmentNbr, receiptWithTax.TaxID);

                        if (detExist != null)
                        {
                            detExist.TaxableAmt = receiptWithTax.CuryTaxableAmt;
                            detExist.TaxAmt = receiptWithTax.CuryTaxAmt;
                            repEntry.Taxes.Update(detExist);
                        }
                        else
                        {
                            ATPTEFMReplenishmentTaxDetail repTaxDetails = repEntry.Taxes.Insert();
                            repTaxDetails.ReplenishmentNbr = currentReplenishment.ReplenishmentNbr;
                            repTaxDetails.TaxID = receiptWithTax.TaxID;
                            repTaxDetails.TaxRate = receiptWithTax.TaxRate;
                            repTaxDetails.TaxableAmt = receiptWithTax.CuryTaxableAmt;
                            repTaxDetails.TaxAmt = receiptWithTax.CuryTaxAmt;
                            repEntry.Taxes.Update(repTaxDetails);
                        }
                    }

                    foreach (ATPTEFMReplenishmentTaxDetail removeTax in repEntry.Taxes.Select())
                    {
                        if (!(uniqueTaxList.Contains(removeTax.TaxID)))
                        {
                            repEntry.Taxes.Cache.Delete(removeTax);
                        }
                    }

                    repEntry.Replenishments.Current.ClaimAmount = totalClaim;
                    repEntry.Replenishments.Current.WithholdingTaxAmount = totalWhtAmt;
                    repEntry.Replenishments.Current.VatAmount = totalVatAmt;
                    repEntry.Replenishments.UpdateCurrent();
                    repEntry.Save.Press();
                }
            }
            #endregion

        }

        public delegate IEnumerable ClaimDelegate(PXAdapter adapter);
        [PXOverride]
        public IEnumerable claim(PXAdapter adapter, ClaimDelegate baseMethod)
        {
            EPExpenseClaimDetails currentRow = Base.CurrentClaimDetails.Current;

            if (ATPTEFMSubmitReceipt.GetEnabled() == true)
            {
                ATPTEFMSubmitReceipt.PressButton();
                PXLongOperation.WaitCompletion(Base.UID);
            }
            ATPTEFMHelper.StartLongOperation(Base, adapter, delegate ()
            {
                ATPTEFMExpenseClaimDetailMaintExt.CFMClaimSingleDetail(currentRow, Base.IsContractBasedAPI);
            });
            return adapter.Get();
        }
        #endregion

        #region Methods

        /// <summary>
        /// Update Fund Transaction History if receipt is updated.
        /// </summary>
        /// <remarks>
        /// 2024-09-16 : Created this method to cater case 007469. {RRS}
        /// </remarks>
        private void UpdateFundTransactionHistory(EPExpenseClaimDetails row)
        {
            if (row?.LastModifiedByScreenID == "EP301020")
            {
                if (row.ClaimDetailCD != null)
                {
                    ATPTEFMFundTransactionHistoryView relatedTransactionHistory = ATPTEFMFundTransactionHistory.Select(row.ClaimDetailCD);
                    if (relatedTransactionHistory == null) return;
                    relatedTransactionHistory.ProjectID = row.ContractID;
                    relatedTransactionHistory.ProjectTaskID = row.TaskID;
                    relatedTransactionHistory.CostCodeID = row.CostCodeID;
                    ATPTEFMFundTransactionHistory.Update(relatedTransactionHistory);
                }
            }
        }
        public virtual string GetDefATC(EPExpenseClaimDetails row)
        {
            return "";
        }
        public virtual string GetVendor(EPExpenseClaimDetails row)
        {
            return "";
        }
        public virtual bool DuplicateERRefNbr(EPExpenseClaimDetails row, string checkRefNbr, string TranType)
        {
            return false;
        }
        private bool WithoutReplenishmentNbr(EPExpenseClaimDetails row)
        {
            ATPTEFMFundTransactionHistory.Current = ATPTEFMFundTransactionHistory.Select(row.ClaimDetailCD);
            return string.IsNullOrEmpty(ATPTEFMFundTransactionHistory.Current.ReplenishmentRefNbr) ? true : false;
        }
        private void UpdateFundSummaryBalances(decimal? oldActualSpentAmt, decimal? oldTotalWHT, decimal? oldAmt, decimal? oldNetAmt, decimal? oldWht)
        {
            ATPTEFMFundTransaction fundTransaction = ATPTEFMFundTransaction.Current;

            bool isFundStatusClosed = ATPTEFMFundTransactionHelper.IsFundStatusClosed(fundTransaction);
            bool isFundRequest = ATPTEFMFundTransactionHelper.IsFundRequest(fundTransaction);
            bool isFundReimbursement = ATPTEFMFundTransactionHelper.IsFundReimbursement(fundTransaction);
            bool withoutReplenishmentNbr = WithoutReplenishmentNbr(Base.CurrentClaimDetails.Current);
            bool isFundReclassification = ATPTEFMFundTransactionHelper.IsFundReclassification(fundTransaction);

            ATPTEFMFund.Current = ATPTEFMFund.Current = ATPTEFMFund.Select(fundTransaction.FundID);

            if (ATPTEFMFund.Current != null)
            {
                bool isValidateAmountReceiveAndRelease = (ATPTEFMFund.Current.ATPTEFMValidateAmountReceivedAndAmountReleased ?? false);

                if (isFundStatusClosed)
                {
                    #region Calculations for Liquidated Field

                    if (withoutReplenishmentNbr)
                    {
                        UpdateFundLiquidatedField(oldActualSpentAmt, oldTotalWHT);

                        #region For Reclassification
                        if ((isValidateAmountReceiveAndRelease) && ((fundTransaction.ChangeAmount - fundTransaction.AmountReceived) + fundTransaction.AmountReleased) > 0m && withoutReplenishmentNbr)
                        {
                            #region Update Summary Fund Unliquidated
                            decimal? oldBalanceAmt = (fundTransaction.RequestedAmount - oldActualSpentAmt) + oldTotalWHT;
                            decimal? newBalanceAmt = (fundTransaction.RequestedAmount - fundTransaction.ActualSpentAmount) + fundTransaction.TotalWhtAmount;
                            ATPTEFMFund.Current.CuryUnliquidatedAmt -= oldBalanceAmt;
                            ATPTEFMFund.Current.CuryUnliquidatedAmt += newBalanceAmt;
                            ATPTEFMFund.UpdateCurrent();
                            #endregion

                            #region Update Transaction History View
                            ATPTEFMFundTransactionHistory.Current = ATPTEFMFundTransactionHistory.Select(fundTransaction.RefNbr);
                            ATPTEFMFundTransactionHistory.Current.Status = ATPTEFMFundStatusAttribute.OpenValue;
                            ATPTEFMFundTransactionHistory.UpdateCurrent();
                            #endregion

                            #region Update Fund Transaction Status to For Reclassification
                            ATPTEFMFundTransaction.Current.CashAdvanceStatus = ATPTEFMFundTransactionCashAdvanceStatusAttribute.ForReclassificationValue;
                            ATPTEFMFundTransaction.Current.Status = ATPTEFMFundStatusAttribute.OpenValue;
                            ATPTEFMFundTransaction.UpdateCurrent();
                            #endregion
                        }
                        else
                        {
                            #region Calculations For OnHand
                            ATPTEFMFund.Current.CuryBalanceAmt += oldActualSpentAmt;
                            ATPTEFMFund.Current.CuryBalanceAmt -= oldTotalWHT;
                            ATPTEFMFund.Current.CuryBalanceAmt -= ATPTEFMFundTransaction.Current.ActualSpentAmount;
                            ATPTEFMFund.Current.CuryBalanceAmt += ATPTEFMFundTransaction.Current.TotalWhtAmount;
                            ATPTEFMFund.UpdateCurrent();
                            #endregion
                        }
                        #endregion
                    }

                    if (!withoutReplenishmentNbr)
                    {
                        #region Calculations For OnHand And On Replenishment
                        ATPTEFMFund.Current.CuryBalanceAmt += oldActualSpentAmt;
                        ATPTEFMFund.Current.CuryBalanceAmt -= oldTotalWHT;
                        ATPTEFMFund.Current.CuryBalanceAmt -= ATPTEFMFundTransaction.Current.ActualSpentAmount;
                        ATPTEFMFund.Current.CuryBalanceAmt += ATPTEFMFundTransaction.Current.TotalWhtAmount;
                        ATPTEFMFund.Current.CuryOnReplenishmentAmt -= (isFundRequest) ? (oldNetAmt - oldWht) : (oldAmt - oldWht);
                        ATPTEFMFund.Current.CuryOnReplenishmentAmt += (Base.ClaimDetails.Current.CuryExtCost - ATPTEFMFundTransactionReceiptDetail.Current.WhtAmount);
                        ATPTEFMFund.UpdateCurrent();
                        #endregion
                    }
                    #endregion

                }

                if (isFundReclassification)
                {
                    UpdateFundLiquidatedField(oldActualSpentAmt, oldTotalWHT);

                    if (((fundTransaction.ChangeAmount - fundTransaction.AmountReceived) + fundTransaction.AmountReleased) == 0m)
                    {
                        #region Update Fund Transaction Status to For Reclassification
                        ATPTEFMFundTransaction.Current.CashAdvanceStatus = ATPTEFMFundTransactionCashAdvanceStatusAttribute.LiquidatedValue;
                        ATPTEFMFundTransaction.Current.Status = ATPTEFMFundStatusAttribute.ClosedValue;
                        ATPTEFMFundTransaction.UpdateCurrent();
                        #endregion

                        #region Update Transaction History View
                        ATPTEFMFundTransactionHistory.Current = ATPTEFMFundTransactionHistory.Select(fundTransaction.RefNbr);
                        ATPTEFMFundTransactionHistory.Current.Status = ATPTEFMFundStatusAttribute.ClosedValue;
                        ATPTEFMFundTransactionHistory.UpdateCurrent();
                        #endregion
                    }
                }

                if (isFundReimbursement && !isFundStatusClosed)
                {
                    UpdateFundLiquidatedField(oldActualSpentAmt, oldTotalWHT);
                }

                /*#region Calculation for On Replenishment
                PXResultset<ATPTEFMReplenishmentDetail, ATPTEFMReplenishment> replenishDetails = PXSelectJoin<
                                                                                ATPTEFMReplenishmentDetail,
                                                                                InnerJoin<ATPTEFMReplenishment,
                                                                                    On<ATPTEFMReplenishment.replenishmentNbr, Equal<ATPTEFMReplenishmentDetail.replenishmentNbr>,
                                                                                    And<ATPTEFMReplenishment.status, NotEqual<ATPTEFMReplenishmentStatusAttribute.cancelledValue>>>>,
                                                                                Where<ATPTEFMReplenishmentDetail.expenseReceiptNbr, Equal<Required<ATPTEFMReplenishmentDetail.expenseReceiptNbr>>>>
                                                                                .Select<PXResultset<ATPTEFMReplenishmentDetail, ATPTEFMReplenishment>>(Base, Base.ClaimDetails.Current.ClaimDetailCD);

                if (replenishDetails.Count > 0 && Base.ClaimDetails.Current.Status != ATPTEFMExpenseReceiptStatusAttribute.ReleaseValue)
                {
                    ATPTEFMFund.Current.OnReplenishmentAmt -= (isFundRequest) ? (oldNetAmt - oldWht) : (oldAmt - oldWht);
                    ATPTEFMFund.Current.OnReplenishmentAmt += (Base.ClaimDetails.Current.CuryExtCost - ATPTEFMFundTransactionReceiptDetail.Current.WhtAmount);
                    ATPTEFMFund.UpdateCurrent();
                }
                #endregion*/

            }
        }
        private void UpdateFundLiquidatedField(decimal? oldActualSpentAmt, decimal? oldTotalWHT)
        {
            #region Calculations for Liquidated Field
            ATPTEFMFund.Current.CuryLiquidatedAmt -= (oldActualSpentAmt - oldTotalWHT);
            ATPTEFMFund.Current.CuryLiquidatedAmt += (ATPTEFMFundTransaction.Current.ActualSpentAmount - ATPTEFMFundTransaction.Current.TotalWhtAmount);
            ATPTEFMFund.UpdateCurrent();
            #endregion
        }
        private void UpdateFundRequestUnliquidatedField(decimal? oldActualSpentAmt, decimal? oldTotalWht)
        {
            ATPTEFMFundTransaction fundTransaction = ATPTEFMFundTransaction.Current;

            bool isFundRequest = ATPTEFMFundTransactionHelper.IsFundRequest(fundTransaction);
            bool isFundUnliquidated = ATPTEFMFundTransactionHelper.IsFundUnliquidated(fundTransaction);
            bool isFundReclassification = ATPTEFMFundTransactionHelper.IsFundReclassification(fundTransaction);
            bool withoutReplenishmentNbr = WithoutReplenishmentNbr(Base.ClaimDetails.Current);

            if (isFundRequest && withoutReplenishmentNbr)
            {
                ATPTEFMFund.Current = ATPTEFMFund.Current = ATPTEFMFund.Select(ATPTEFMFundTransaction.Current.FundID);

                if (isFundReclassification)
                {
                    decimal? oldBalanceAmt = (fundTransaction.RequestedAmount - oldActualSpentAmt) + oldTotalWht;
                    decimal? newBalanceAmt = (fundTransaction.RequestedAmount - fundTransaction.ActualSpentAmount) + fundTransaction.TotalWhtAmount;
                    ATPTEFMFund.Current.CuryUnliquidatedAmt -= oldBalanceAmt;
                    ATPTEFMFund.Current.CuryUnliquidatedAmt += newBalanceAmt;
                    ATPTEFMFund.UpdateCurrent();
                }
            }
        }
        private decimal? CalculateTotalWHT()
        {
            decimal totalWhtAmt = decimal.Zero;

            foreach (EPTaxTran tax in Base.Taxes.Select())
            {
                Tax tx = PXSelect<
                    Tax,
                    Where<Tax.taxID, Equal<Required<Tax.taxID>>>>
                    .Select(Base, tax.TaxID);
                if (tx != null && tx.TaxType == CSTaxType.Withholding)
                {
                    totalWhtAmt += (decimal)tax.CuryTaxAmt;
                }
            }

            return totalWhtAmt;
        }
        private void UpdateFundTranscationReceiptDetailRecord(EPExpenseClaimDetails row)
        {
            ATPTEFMEPExpenseClaimDetailsExt rowExt = row.GetExtension<ATPTEFMEPExpenseClaimDetailsExt>();

            bool isFundRequest = ATPTEFMFundTransactionHelper.IsFundRequest(ATPTEFMFundTransaction.Current);
            var accDesc = ATPTEFMAccountDescription.Select(row.ExpenseAccountID);

            #region Old Values
            decimal? oldQty = ATPTEFMFundTransactionHelper.GetOldQty(ATPTEFMFundTransactionReceiptDetail.Current);
            decimal? oldUnitCost = ATPTEFMFundTransactionHelper.GetOldUnitCost(ATPTEFMFundTransactionReceiptDetail.Current);
            decimal? oldAmount = ATPTEFMFundTransactionHelper.GetOldAmount(ATPTEFMFundTransactionReceiptDetail.Current);
            decimal? totalWhtAmt = CalculateTotalWHT();
            #endregion

            #region Assigning New Values
            ATPTEFMFundTransactionReceiptDetail.Current.ExpenseReceiptRefNbr = row.ClaimDetailCD;
            ATPTEFMFundTransactionReceiptDetail.Current.Date = row.ExpenseDate;
            ATPTEFMFundTransactionReceiptDetail.Current.LineDescription = row.TranDesc;
            ATPTEFMFundTransactionReceiptDetail.Current.Qty = (isFundRequest) ? oldQty : row.Qty;
            ATPTEFMFundTransactionReceiptDetail.Current.UnitCost = (isFundRequest) ? oldUnitCost : row.CuryUnitCost;
            ATPTEFMFundTransactionReceiptDetail.Current.Amount = (isFundRequest) ? oldAmount : row.CuryExtCost;
            ATPTEFMFundTransactionReceiptDetail.Current.NetQty = (isFundRequest) ? row.Qty : decimal.Zero;
            ATPTEFMFundTransactionReceiptDetail.Current.NetUnitCost = (isFundRequest) ? row.CuryUnitCost : decimal.Zero;
            ATPTEFMFundTransactionReceiptDetail.Current.NetAmt = (isFundRequest) ? row.CuryExtCost : decimal.Zero;
            ATPTEFMFundTransactionReceiptDetail.Current.InventoryID = row.InventoryID;
            ATPTEFMFundTransactionReceiptDetail.Current.WhtAmount = totalWhtAmt;
            ATPTEFMFundTransactionReceiptDetail.Current.FundReturn = 0m;
            ATPTEFMFundTransactionReceiptDetail.Current.AccountID = row.ExpenseAccountID;
            ATPTEFMFundTransactionReceiptDetail.Current.AccountDescription = accDesc.TopFirst.Description;
            ATPTEFMFundTransactionReceiptDetail.Current.RefNbr = row.ExpenseRefNbr;
            ATPTEFMFundTransactionReceiptDetail.Current.SubID = row.ExpenseSubID;
            ATPTEFMFundTransactionReceiptDetail.Current.ProjectID = row.ContractID;
            ATPTEFMFundTransactionReceiptDetail.Current.ProjectTaskID = row.TaskID;
            ATPTEFMFundTransactionReceiptDetail.Current.VendID = rowExt.UsrATPTEFMDetailVendorID;
            ATPTEFMFundTransactionReceiptDetail.Current.VendorName = rowExt.UsrATPTVendName;
            ATPTEFMFundTransactionReceiptDetail.Current.VendorAddress = rowExt.UsrATPTAddress;
            ATPTEFMFundTransactionReceiptDetail.Current.VendorTin = rowExt.UsrATPTVendTIN;
            ATPTEFMFundTransactionReceiptDetail.Current.TaxZoneID = row.TaxZoneID;
            ATPTEFMFundTransactionReceiptDetail.Current.TaxCategoryID = row.TaxCategoryID;
            ATPTEFMFundTransactionReceiptDetail.Current.AtcCode = rowExt.UsrATPTEFMATCCode;
            ATPTEFMFundTransactionReceiptDetail.Current.CostCodeID = row.CostCodeID;
            ATPTEFMFundTransactionReceiptDetail.Current.FundTransactionRefNbr = rowExt.UsrATPTEFMRequestRefNbr;
            #region Replenishment Limit Validation
            ATPTEFMFundTransaction ft = ATPTEFMFundTransaction.Current;
            if (ft != null && ft.NoFund == false && (ft.Step == ATPTEFMFundTransactionStepAttribute.SubmitReceiptValue || ft.Step == ATPTEFMFundTransactionStepAttribute.LiquidatedReceiptValue))
            {
                StringBuilder restriction = new StringBuilder();

                ATPTEFMFund funds = PXSelect<
                    ATPTEFMFund,
                    Where<ATPTEFMFund.fundCD, Equal<Required<ATPTEFMFund.fundCD>>>>
                    .Select(Base, ft.FundID);

                restriction.Clear();
                restriction.Append(funds.ReplenishmentRestriction);

                if (ft.FundTransactionType.Equals(ATPTEFMFundTransactionTypeAttribute.CashAdvanceValue))
                {
                    if (ft.IsReleasedCash == true && (funds.CuryBalanceAmt - ((ATPTEFMFundTransactionReceiptDetail.Current.NetAmt - ft.RequestedAmount) * 2)) < funds.ReplenishmentAmt)
                    {
                        if (restriction.ToString() == ATPTEFMReplenishmentStringListAttribute.ErrorRestrictValue)
                        {
                            throw ATPTEFMHelper.GetPropertyException(ft, Messages.ATPTEFMMessages.ReplenishmentAmtLimit, PXErrorLevel.Error);
                        }
                    }
                }

                if (ft.FundTransactionType.Equals(ATPTEFMFundTransactionTypeAttribute.ReimbursementValue))
                {
                    if (ft.IsReleasedCash != true && (funds.CuryBalanceAmt - ATPTEFMFundTransactionReceiptDetail.Current.NetAmt) < funds.ReplenishmentAmt)
                    {
                        if (restriction.ToString() == ATPTEFMReplenishmentStringListAttribute.ErrorRestrictValue)
                        {
                            throw ATPTEFMHelper.GetPropertyException(ft, Messages.ATPTEFMMessages.ReplenishmentAmtLimit, PXErrorLevel.Error);
                        }
                    }
                }
            }
            #endregion
            #region Budget Validation Liquidation Amt greater than Request
            if (ATPTEFMFundTransaction.Current.BudgetEnabled ?? false)
            {
                var reqRows = PXSelect<
                    ATPTEFMFundTransactionDetail,
                    Where<ATPTEFMFundTransactionDetail.fundTransactionRefNbr, Equal<@P.AsString>>>
                    .Select(Base, ATPTEFMFundTransaction.Current.RefNbr);

                var receiptRows = PXSelect<
                    ATPTEFMFundTransactionReceiptDetail,
                    Where<ATPTEFMFundTransactionReceiptDetail.fundTransactionRefNbr, Equal<@P.AsString>>>
                    .Select(Base, ATPTEFMFundTransaction.Current.RefNbr);

                var reqDetGrouped = reqRows.RowCast<ATPTEFMFundTransactionDetail>().GroupBy(x => new { x.AccountID, x.SubID }).Select(x => new { AccountID = x.Key.AccountID, SubID = x.Key.SubID, Amount = x.Sum(y => y.Amount) });

                foreach (var reqDet in reqDetGrouped)
                {
                    decimal totalNetAmtByBudgetAccounts = 0;
                    foreach (ATPTEFMFundTransactionReceiptDetail recDet in receiptRows)
                    {
                        if (recDet.AccountID == reqDet.AccountID && recDet.SubID == reqDet.SubID)
                        {
                            if (recDet.ExpenseReceiptRefNbr == row.ClaimDetailCD)
                                totalNetAmtByBudgetAccounts += (isFundRequest) ? row.CuryExtCost ?? 0m : decimal.Zero;
                            else
                                totalNetAmtByBudgetAccounts += recDet.NetAmt ?? 0m;

                            if (totalNetAmtByBudgetAccounts > reqDet.Amount)
                            {
                                Account acc = PXSelect<
                                    Account,
                                    Where<Account.accountID, Equal<Required<Account.accountID>>>>
                                    .Select(Base, recDet.AccountID);
                                Sub subAcc = PXSelect<
                                    Sub,
                                    Where<Sub.subID, Equal<Required<Sub.subID>>>>
                                    .Select(Base, recDet.SubID);

                                Base.CurrentClaimDetails.Cache.RaiseExceptionHandling<EPExpenseClaimDetails.curyExtCost>(row, row?.CuryExtCost,
                                            ATPTEFMHelper.GetPropertyException(recDet, ATPTEFMMessages.MessagesWithParameters.ReceiptAmtGreaterThanRequestAmt(acc?.AccountCD, subAcc?.SubCD), PXErrorLevel.Error));
                            }
                        }
                    }
                }
            }
            #endregion
            ATPTEFMFundTransactionReceiptDetail.UpdateCurrent();

            #endregion
        }
        private void UpdateFundRequestTransactionRecord(EPExpenseClaimDetails row, decimal? oldWHT)
        {
            bool isFundRequest = ATPTEFMFundTransactionHelper.IsFundRequest(ATPTEFMFundTransaction.Current);

            if (isFundRequest)
            {
                decimal? totalWhtAmt = CalculateTotalWHT();

                ATPTEFMFundTransaction.Current.TotalWhtAmount = (ATPTEFMFundTransaction.Current.TotalWhtAmount - oldWHT) + totalWhtAmt;
                ATPTEFMFundTransaction.Current.ChangeAmount = (ATPTEFMFundTransaction.Current.RequestedAmount - ATPTEFMFundTransaction.Current.ActualSpentAmount) + ATPTEFMFundTransaction.Current.TotalWhtAmount;
                ATPTEFMFundTransaction.UpdateCurrent();
            }
        }
        private void UpdateFundReimbursementTransactionRecord(EPExpenseClaimDetails row, decimal? oldWHT, decimal? oldActualSpentAmt, decimal? oldAmount)
        {
            bool isFundRequest = ATPTEFMFundTransactionHelper.IsFundRequest(ATPTEFMFundTransaction.Current);

            if (!isFundRequest)
            {
                #region Old Values
                decimal? totalWhtAmt = CalculateTotalWHT();
                #endregion

                ATPTEFMFundTransaction.Current.TotalWhtAmount = (ATPTEFMFundTransaction.Current.TotalWhtAmount - oldWHT) + totalWhtAmt;
                ATPTEFMFundTransaction.Current.ActualSpentAmount = (oldActualSpentAmt - oldAmount) + row.CuryExtCost;
                ATPTEFMFundTransaction.UpdateCurrent();
            }
        }
        private void UpdateReceiptTransactionHistoryRecord(EPExpenseClaimDetails row)
        {
            bool isFundRequest = ATPTEFMFundTransactionHelper.IsFundRequest(ATPTEFMFundTransaction.Current);
            bool isFundLiquidated = ATPTEFMFundTransactionHelper.IsFundLiquidated(ATPTEFMFundTransaction.Current);

            decimal? totalWhtAmt = CalculateTotalWHT();

            ATPTEFMFundTransactionHistory.Current = ATPTEFMFundTransactionHistory.Select(row.ClaimDetailCD);
            ATPTEFMFundTransactionHistory.Current.CuryFundTransactionDocumentAmt = (isFundRequest) ? ATPTEFMFundTransactionReceiptDetail.Current.NetAmt : ATPTEFMFundTransactionReceiptDetail.Current.Amount;
            ATPTEFMFundTransactionHistory.Current.FundBranchID = row.BranchID;
            ATPTEFMFundTransactionHistory.Current.CuryWithholdingTax = totalWhtAmt;
            ATPTEFMFundTransactionHistory.Current.TransactionDate = row.ExpenseDate;
            if (isFundRequest)
            {
                ATPTEFMFundTransactionHistory.Current.CuryUnliquidatedAmt = (!isFundLiquidated) ? ATPTEFMFundTransactionReceiptDetail.Current.NetAmt - ATPTEFMFundTransactionReceiptDetail.Current.WhtAmount : decimal.Zero;
                ATPTEFMFundTransactionHistory.Current.CuryLiquidatedAmt = (isFundLiquidated) ? ATPTEFMFundTransactionReceiptDetail.Current.NetAmt - ATPTEFMFundTransactionReceiptDetail.Current.WhtAmount : decimal.Zero;
            }
            else
            {
                ATPTEFMFundTransactionHistory.Current.CuryLiquidatedAmt = row.CuryExtCost - totalWhtAmt;
            }
            ATPTEFMFundTransactionHistory.UpdateCurrent();
        }
        private void UpdateFundTransactionHistoryRecord(ATPTEFMFundTransaction fundTran, bool isValidateAmount)
        {
            bool isFundRequest = ATPTEFMFundTransactionHelper.IsFundRequest(fundTran);

            ATPTEFMFundTransactionHistory.Current = ATPTEFMFundTransactionHistory.Select(fundTran.RefNbr);

            if (fundTran != null && ATPTEFMFundTransactionHistory.Current != null)
            {
                if (isValidateAmount)
                {
                    ATPTEFMFundTransactionHistory.Current.CuryFundReturnAmt = fundTran.AmountReceived;
                }
                else
                {
                    ATPTEFMFundTransactionHistory.Current.CuryFundReturnAmt = (isFundRequest) ? fundTran.ChangeAmount.GetValueOrDefault() : decimal.Zero;
                }

                ATPTEFMFundTransactionHistory.Current.CuryFundTransactionDocumentAmt = (isFundRequest) ? fundTran.RequestedAmount.GetValueOrDefault()
                    : fundTran.ActualSpentAmount.GetValueOrDefault();

                ATPTEFMFundTransactionHistory.UpdateCurrent();
            }
        }
        public int? SetAccountID(ATPTEFMReqClass reqClass, EPExpenseClaimDetails ecDet)
        {
            int? retValue = 0;

            if (reqClass != null)
            {
                EPEmployee requestBy = PXSelect<
                    EPEmployee,
                    Where<EPEmployee.bAccountID, Equal<Required<EPEmployee.bAccountID>>>>
                    .Select(Base, ecDet?.EmployeeID);

                EPDepartment department = PXSelect<
                    EPDepartment,
                    Where<EPDepartment.departmentID, Equal<Required<EPDepartment.departmentID>>>>
                    .Select(Base, requestBy?.DepartmentID);

                switch (reqClass.UseExpenseAcctFrom)
                {
                    case RQAccountSource.None:
                        retValue = null;
                        break;
                    case RQAccountSource.Requester:
                        retValue = requestBy?.ExpenseAcctID;
                        break;
                    case RQAccountSource.RequestClass:
                        retValue = reqClass.ExpenseAcctID;
                        break;
                    case RQAccountSource.Department:
                        retValue = department?.ExpenseAccountID;
                        break;
                    case RQAccountSource.PurchaseItem:
                        InventoryItem item = InventoryItem.PK.Find(Base, ecDet?.InventoryID);
                        retValue = item?.COGSAcctID;
                        break;
                }
            }
            return retValue;
        }
        public int? SetAccountID(ATPTEFMSetup fmSetup, EPExpenseClaimDetails ecDet, ATPTEFMEPExpenseClaimDetailsExt ecDetExt)
        {
            ATPTEFMFundTransaction ft = PXSelect<
                ATPTEFMFundTransaction, 
                Where<ATPTEFMFundTransaction.refNbr, Equal<@P.AsString>>>
                .Select(Base, ecDetExt.UsrATPTEFMRequestRefNbr);
            int? retValue = 0;

            if (fmSetup != null && ft != null)
            {
                if (fmSetup.UseExpenseAcctFrom.IsNullOrEmpty())
                    throw new PXException(ATPTEFMMessages.FTSetupGLAccountValidation);

                #region Default AccountID
                switch (fmSetup.UseExpenseAcctFrom)
                {
                    case RQAccountSource.None:
                        retValue = null;
                        break;
                    case RQAccountSource.Requester:
                        EPEmployee requestBy = PXSelect<
                            EPEmployee,
                            Where<EPEmployee.bAccountID, Equal<Required<EPEmployee.bAccountID>>>>
                            .Select(Base, ft.RequestedByID);
                        retValue = requestBy?.ExpenseAcctID;
                        break;
                    case RQAccountSource.Department:
                        EPDepartment department = PXSelect<
                            EPDepartment,
                            Where<EPDepartment.departmentID, Equal<Required<EPDepartment.departmentID>>>>
                            .Select(Base, ft.DepartmentID);
                        retValue = department?.ExpenseAccountID;
                        break;
                    case RQAccountSource.PurchaseItem:
                        InventoryItem item = InventoryItem.PK.Find(Base, ecDet?.InventoryID);
                        retValue = item?.COGSAcctID;
                        break;
                }
                #endregion
            }
            return retValue;
        }
        public object SetSubID(ATPTEFMSetup fmSetup, EPExpenseClaimDetails ecDet)
        {
            object retValue = null;

            ATPTEFMEPExpenseClaimDetailsExt claimDetExt = ecDet.GetExtension<ATPTEFMEPExpenseClaimDetailsExt>();

            //Purchase Item
            InventoryItem item = InventoryItem.PK.Find(Base, ecDet.InventoryID);
            int? inventorySubID = item?.COGSSubID;

            //Requesters
            EPEmployee requestBy = PXSelect<
                EPEmployee,
                Where<EPEmployee.bAccountID, Equal<Required<EPEmployee.bAccountID>>>>
                .Select(Base, ecDet.EmployeeID);
            int? requesterSubID = requestBy?.ExpenseSubID;

            //Department
            EPDepartment department = PXSelect<
                EPDepartment,
                Where<EPDepartment.departmentID, Equal<Required<EPDepartment.departmentID>>>>
                .Select(Base, requestBy.DepartmentID);
            int? departmentSubID = department?.ExpenseSubID;

            retValue = ATPTEFMSubAccountMaskExtension.MakeSub<ATPTEFMSetup.combineExpSub>(Base, fmSetup.CombineExpSub,
                                    new object[] { departmentSubID, inventorySubID, requesterSubID },
                                    new Type[] { typeof(EPDepartment.expenseSubID), typeof(InventoryItem.cOGSSubID), typeof(EPEmployee.expenseSubID) });

            return retValue;
        }
        public object SetSubID(ATPTEFMReqClass reqClass, EPExpenseClaimDetails ecDet)
        {
            object retValue = null;

            ATPTEFMEPExpenseClaimDetailsExt claimDetExt = ecDet.GetExtension<ATPTEFMEPExpenseClaimDetailsExt>();

            //Request Class
            int? requestclassSubID = reqClass.ExpenseSubID;

            //Purchase Item
            InventoryItem item = InventoryItem.PK.Find(Base, ecDet.InventoryID);
            int? inventorySubID = item?.COGSSubID;

            //Requesters
            EPEmployee requestBy = PXSelect<
                EPEmployee,
                Where<EPEmployee.bAccountID, Equal<Required<EPEmployee.bAccountID>>>>
                .Select(Base, ecDet.EmployeeID);
            int? requesterSubID = requestBy?.ExpenseSubID;

            //Department
            EPDepartment department = PXSelect<
                EPDepartment,
                Where<EPDepartment.departmentID, Equal<Required<EPDepartment.departmentID>>>>
                .Select(Base, requestBy.DepartmentID);
            int? departmentSubID = department?.ExpenseSubID;

            retValue = PX.Objects.RQ.SubAccountMaskAttribute.MakeSub<ATPTEFMReqClass.combineExpSub>(Base, reqClass.CombineExpSub,
                                new object[] { requestclassSubID, departmentSubID, inventorySubID, requesterSubID },
                                new Type[] { typeof(ATPTEFMReqClass.expenseSubID), typeof(EPDepartment.expenseSubID), typeof(InventoryItem.cOGSSubID), typeof(EPEmployee.expenseSubID) });

            return retValue;
        }
        //-> Transferred to CFM ExpenseClaimDetailMaint Extension
        //public static void RFPClaimDetail(List<EPExpenseClaimDetails> details, bool isApiContext, bool singleOperation)
        //{
        //    ExpenseClaimEntry expenseClaimEntry = PXGraph.CreateInstance<ExpenseClaimEntry>();
        //    PXSetup<EPSetup> epsetup = new PXSetup<EPSetup>(PXGraph.CreateInstance(typeof(ExpenseClaimDetailEntry)));
        //    bool enabledApprovalReceipt = PXAccess.FeatureInstalled<FeaturesSet.approvalWorkflow>() && epsetup.Current.ClaimDetailsAssignmentMapID != null;
        //    bool isError = false;
        //    bool notAllApproved = false;
        //    Dictionary<string, EPExpenseClaim> result = new Dictionary<string, EPExpenseClaim>();

        //    var origDetails = details.ToDictionary(d => d.ClaimDetailCD);
        //    details = details.Select(d => EPExpenseClaimDetails.PK.Find(expenseClaimEntry, d)).ToList();

        //    IEnumerable<Receipts> List;

        //    if (epsetup.Current.AllowMixedTaxSettingInClaims == true)
        //    {
        //        List = details.Where(item => string.IsNullOrEmpty(item.RefNbr)).OrderBy(detail => detail.ClaimDetailID).GroupBy(
        //                                    item => new
        //                                    {
        //                                        item.EmployeeID,
        //                                        item.BranchID,
        //                                        item.CustomerID,
        //                                        item.CustomerLocationID,
        //                                        ClaimCuryID = ExpenseClaimDetailMaint.GetClaimCuryID(expenseClaimEntry, item)
        //                                    },
        //                                    (key, item) => new Receipts
        //                                    {
        //                                        employee = key.EmployeeID,
        //                                        branch = key.BranchID,
        //                                        customer = key.CustomerID,
        //                                        customerLocation = key.CustomerLocationID,
        //                                        claimCuryID = key.ClaimCuryID,
        //                                        details = item
        //                                    }
        //                                    );
        //    }
        //    else
        //    {
        //        List = details.Where(item => string.IsNullOrEmpty(item.RefNbr)).OrderBy(detail => detail.ClaimDetailID).GroupBy(
        //                                    item => new
        //                                    {
        //                                        item.EmployeeID,
        //                                        item.BranchID,
        //                                        item.CustomerID,
        //                                        item.CustomerLocationID,
        //                                        item.TaxZoneID,
        //                                        item.TaxCalcMode,
        //                                        ClaimCuryID = ExpenseClaimDetailMaint.GetClaimCuryID(expenseClaimEntry, item)
        //                                    },
        //                                    (key, item) => new Receipts
        //                                    {
        //                                        employee = key.EmployeeID,
        //                                        branch = key.BranchID,
        //                                        customer = key.CustomerID,
        //                                        customerLocation = key.CustomerLocationID,
        //                                        claimCuryID = key.ClaimCuryID,
        //                                        details = item
        //                                    }
        //                                    );
        //    }

        //    string errorMessage = null;

        //    foreach (Receipts item in List)
        //    {
        //        isError = false;
        //        notAllApproved = false;
        //        using (PXTransactionScope ts = new PXTransactionScope())
        //        {
        //            expenseClaimEntry.Clear();
        //            expenseClaimEntry.SelectTimeStamp();
        //            EPExpenseClaim expenseClaim = (EPExpenseClaim)expenseClaimEntry.ExpenseClaim.Cache.CreateInstance();
        //            expenseClaim.EmployeeID = item.employee;
        //            expenseClaim.BranchID = item.branch;
        //            expenseClaim.CustomerID = item.customer;
        //            expenseClaim.DocDesc = PX.Objects.EP.Messages.SubmittedReceipt;
        //            expenseClaim = expenseClaimEntry.ExpenseClaim.Update(expenseClaim);
        //            expenseClaim.CuryID = item.claimCuryID;
        //            expenseClaim = expenseClaimEntry.ExpenseClaim.Update(expenseClaim);
        //            expenseClaim.CustomerLocationID = item.customerLocation;
        //            expenseClaim.TaxCalcMode = item.details.First().TaxCalcMode;
        //            expenseClaim.TaxZoneID = item.details.First().TaxZoneID;

        //            foreach (EPExpenseClaimDetails detail in item.details)
        //            {
        //                if (origDetails.TryGetValue(detail.ClaimDetailCD, out EPExpenseClaimDetails origRow))
        //                {
        //                    PXProcessing<EPExpenseClaimDetails>.SetCurrentItem(origRow);
        //                }
        //                else
        //                {
        //                    PXProcessing<EPExpenseClaimDetails>.SetCurrentItem(detail);
        //                }


        //                if (detail.Approved ?? false)
        //                {
        //                    try
        //                    {
        //                        if (detail.IsPaidWithCard)
        //                        {
        //                            EPEmployee employee =
        //                                PXSelect<
        //                                    EPEmployee,
        //                                    Where<EPEmployee.bAccountID, Equal<Required<EPEmployee.bAccountID>>>>
        //                                    .Select(expenseClaimEntry, item.employee);

        //                            if (employee.AllowOverrideCury != true && detail.CardCuryID != employee.CuryID)
        //                            {
        //                                errorMessage = PXMessages.Localize(PX.Objects.EP.Messages.ClaimCannotBeCreatedForReceiptBecauseCuryCannotBeOverriden);

        //                                isError = true;
        //                            }
        //                        }

        //                        if (!isError && detail.TipAmt != 0 && epsetup.Current.NonTaxableTipItem == null)
        //                        {
        //                            errorMessage = PX.Objects.EP.Messages.TipItemIsNotDefined;
        //                            isError = true;
        //                        }

        //                        if (!isError)
        //                        {
        //                            expenseClaimEntry.ReceiptEntryExt.SubmitReceiptExt(expenseClaimEntry.ExpenseClaim.Cache,
        //                                expenseClaimEntry.ExpenseClaimDetails.Cache, expenseClaimEntry.ExpenseClaim.Current, detail);

        //                            #region Transfer customized field value from receipt to claim
        //                            ATPTEFMEPExpenseClaimExt claimExt = expenseClaimEntry.ExpenseClaim.Current.GetExtension<ATPTEFMEPExpenseClaimExt>();
        //                            ATPTEFMEPExpenseClaimDetailsExt receiptExt = detail.GetExtension<ATPTEFMEPExpenseClaimDetailsExt>();
        //                            claimExt.UsrATPTEFMTranType = receiptExt.UsrATPTEFMTranType;
        //                            claimExt.UsrATPTEFMReqType = receiptExt.UsrATPTEFMReqType;
        //                            claimExt.UsrATPTVendorID = receiptExt.UsrATPTVendorID;
        //                            claimExt.UsrATPTEFMReqClass = receiptExt.UsrATPTEFMReqClass;
        //                            #endregion

        //                            expenseClaimEntry.Save.Press();
        //                            if (!result.ContainsKey(expenseClaim.RefNbr))
        //                            {
        //                                result.Add(expenseClaim.RefNbr, expenseClaim);
        //                            }
        //                            detail.RefNbr = expenseClaim.RefNbr;

        //                            // Display refNbr on processing grid
        //                            if (origRow != null)
        //                            {
        //                                origRow.RefNbr = expenseClaim.RefNbr;
        //                            }

        //                            PXProcessing<EPExpenseClaimDetails>.SetProcessed();
        //                        }

        //                    }
        //                    catch (Exception ex)
        //                    {
        //                        errorMessage = ex.Message;
        //                        isError = true;
        //                    }
        //                }
        //                else
        //                {
        //                    errorMessage = enabledApprovalReceipt
        //                        ? PX.Objects.EP.Messages.ReceiptNotApproved
        //                        : PX.Objects.EP.Messages.ReceiptTakenOffHold;

        //                    notAllApproved = true;
        //                }

        //                if (errorMessage != null)
        //                {
        //                    PXProcessing<EPExpenseClaimDetails>.SetError(errorMessage);
        //                }
        //            }
        //            if (!isError)
        //            {
        //                ts.Complete();
        //            }
        //        }
        //    }

        //    if (!isError && !notAllApproved)
        //    {
        //        if (result.Count == 1 && isApiContext == false)
        //        {
        //            expenseClaimEntry = PXGraph.CreateInstance<ExpenseClaimEntry>();
        //            PXRedirectHelper.TryRedirect(expenseClaimEntry, result.First().Value, PXRedirectHelper.WindowMode.InlineWindow);
        //        }
        //    }
        //    else
        //    {
        //        PXProcessing<EPExpenseClaimDetails>.SetCurrentItem(null);
        //        if (singleOperation)
        //        {
        //            throw new PXException(errorMessage);
        //        }
        //        else
        //        {
        //            throw new PXException(PX.Objects.EP.Messages.ErrorProcessingReceipts);
        //        }
        //    }
        //}
        #endregion

        #region Internal class
        private class Receipts
        {
            public int? employee;
            public int? branch;
            public int? customer;
            public int? customerLocation;
            public string claimCuryID;
            public IEnumerable<EPExpenseClaimDetails> details;
        };
        #endregion
    }
}