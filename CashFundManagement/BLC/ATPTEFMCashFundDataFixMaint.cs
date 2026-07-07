using CashFundManagement.Attributes;
using CashFundManagement.DAC;
using CashFundManagement.DAC.Setup;
using CashFundManagement.Extensions.DAC;
using CashFundManagement.Helper;
using CashFundManagement.Messages;
using PX.Common;
using PX.Data;
using PX.Data.BQL;
using PX.Data.ReferentialIntegrity;
using PX.Objects.AP;
using PX.Objects.CS;
using PX.Objects.CM;
using PX.Objects.EP;
using PX.Objects.IN;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using static CashFundManagement.BLC.ATPTEFMFundMaint;
using System.Text;
using PX.Objects.TX;
using CashFundManagement.Extensions.Attribute;
using static PX.SM.AUAuditKeys;
using PX.Data.BQL.Fluent;
using PX.Objects.CR;
using PX.Objects.GL;

namespace CashFundManagement.BLC
{
    public class ATPTEFMCashFundDataFixMaint : PXGraph<ATPTEFMCashFundDataFixMaint>
    {
        List<FTUpdateRecord> updateFTRecords = new List<FTUpdateRecord>();

        List<CAUpdateRecord> updateCARecords = new List<CAUpdateRecord>();

        #region Views
        public PXSelect<ATPTEFMCashFundDataFix> DataFixDocument;
        #endregion

        #region CTOR
        public ATPTEFMCashFundDataFixMaint()
        {

        }
        #endregion

        #region Actions
        public PXAction<ATPTEFMCashFundDataFix> ValidatePassword;
        [PXButton()]
        [PXUIField(DisplayName = "Validate Password")]
        public IEnumerable validatePassword(PXAdapter adapter)
        {
            if (DataFixDocument.Current.Password.ToLower() == "manok123")
                DataFixDocument.Current.EnableFields = true;
            else
                throw new Exception(Messages.ATPTEFMMessages.InvalidPassword);

            return adapter.Get();
        }

        public PXAction<ATPTEFMCashFundDataFix> UpdateRecords;
        [PXButton()]
        [PXUIField(DisplayName = "Update Records")]
        /// <remarks>
        /// 02-27-2026 : 015477 - Added DeleteCAReceiptsWithRefs datafix option {RFS}
        /// </remarks>
        public IEnumerable updateRecords(PXAdapter adapter)
        {
            ATPTEFMHelper.StartLongOperation(this, adapter, delegate ()
            {
                using (PXTransactionScope ts = new PXTransactionScope())
                {
                    if (DataFixDocument.Current.CAPendingForLiquidationToClosed ?? false) { UpdateCashAdvanceStatusToClose(); }

                    if (DataFixDocument.Current.RefNbr != null && DataFixDocument.Current.Status != null) { UpdateExpenseClaimStatus(); }

                    if (DataFixDocument.Current.UnlinkCABillRefNbr != null) { UnlinkCABillRefNbr(); }

                    if (DataFixDocument.Current.InsertDefaultForOldFundsFundCD ?? false) { InsertDefaultsForOldFunds(); }

                    if (DataFixDocument.Current.UpdateFundClearingAccounts ?? false) { UpdateFundClearingAccounts(); }

                    if (DataFixDocument.Current.BlankUnitCost ?? false) { UpdateBlankUnitCost(); }

                    if (DataFixDocument.Current.BlankReceiptUnitCost ?? false) { UpdateReceiptBlankUnitCost(); }

                    if (DataFixDocument.Current.EmptyReplenishmentDetailReplenishmentRefNbr != null) { EmptyReplenishmentDetailReplenishment(); }

                    if (DataFixDocument.Current.DataFixForFTReceiptswithReplenishmentRefNbrbutalreadyDeletedinReplenishment ?? false) { DataFixForFTReceiptswithReplenishmentRefNbrbutalreadyDeletedinReplenishmentProcess(); }

                    if (DataFixDocument.Current.NullChangeAmountDataFix != null) { NullChangeAmountDataFixProcess(); }

                    if (DataFixDocument.Current.FundCD != null)
                    {
                        FundUnboundToBound(DataFixDocument.Current.FundCD, true);
                    }

                    if (DataFixDocument.Current.UpdateCABudgetEnabled ?? false) { UpdateCABudgetEnabledProcess(); }

                    if (DataFixDocument.Current.UpdateFTBudgetEnabled ?? false) { UpdateFTBudgetEnabledProcess(); }

                    if (DataFixDocument.Current.UpdateRFPBudgetEnabled ?? false) { UpdateRFPBudgetEnabledProcess(); }

                    if (DataFixDocument.Current.UpdateBillsBudgetEnabled ?? false) { UpdateBillsBudgetEnabledProcess(); }

                    if (DataFixDocument.Current.CAInitialLiquidationDatafixForOldData ?? false) { UpdateCAInitialLiquidationDateAndUnmodifiedLiqDate(); }

                    if (DataFixDocument.Current.EFFOverrideReceiptsSetToFalse ?? false) { OverrideReceiptsSetToFalse(); }

                    if (DataFixDocument.Current.FundReplenishPoint != null && DataFixDocument.Current.ReplenishPointPercent != null) { UpdateFundReplenishPoint(); }

                    if (DataFixDocument.Current.FTInitialLiquidationDatafixForOldData ?? false) { UpdateFTInitialLiquidationDate(); }

                    if (DataFixDocument.Current.ECMissingDetailsDataFix ?? false) { ECwithMissingDetails(); }

                    if (DataFixDocument.Current.CaPPMBalanceNotEqualToBalance == true) { UpdateBalanceEqualToPpmBalance(); }

                    if (DataFixDocument.Current.ReplenishmentNrbNeedsToBeOpen != null) { ReopenReplenishment(); }

                    if (DataFixDocument.Current.FTExpectedDateOfUseDatafixForOldData ?? false) { UpdateExpectedDateOfUse(); }

                    if (DataFixDocument.Current.FTwoLineDescriptionDatafix ?? false) { UpdateFTLineDescription(); }

                    if (DataFixDocument.Current.CAwoLineDescriptionDatafix ?? false) { UpdateCALineDescription(); }

                    if (DataFixDocument.Current.LEPSetup ?? false) { LEPSetupMethod(); }

                    if (DataFixDocument.Current.QmazFundCD != null) { QmazFundDataFix(); }

                    if (DataFixDocument.Current.ECDeleteApprovalsForCancelledStatus != null) { DeleteApprovalsOnCancelledECProcess(); }

                    if (DataFixDocument.Current.CloseCAWithBalance != null) { CloseCAWithBalanceProcess(); }

                    if (DataFixDocument.Current.FundHistoryBalanceAndSortingFixer != null)
                    {
                        FundTransactionHistoryBalanceAndSortingFixer();
                    }
                    if (DataFixDocument.Current.CAReceiptAlreadyCancelledButExistInCAReceiptsTab ?? false) { ProcessCAReceiptAlreadyCancelledButExistInCAReceiptsTab(); }

                    if (DataFixDocument.Current.FundCDCurrency != null) { UpdateTransactionHistoryCuryFields(); }

                    if (DataFixDocument.Current.DisableAutomaticReleaseAPAndRequireApprovalOnRFPBill ?? false) { DisableAutomaticReleaseAPAndRequireApprovalOnRFPBillProcess(); }

                    if (DataFixDocument.Current.CAVendorRefundWrongAmt != null) { UpdateCAVendorRefundWrongAmt(); }

                    if (DataFixDocument.Current.CleanUpCAReceiptsIssues != null) { ProcessCleanUpCA(); }

                    if (DataFixDocument.Current.DeleteCAReceiptsWithRefs != null) { ProcessDeleteCAReceiptsWithRefs(); }

                    if (DataFixDocument.Current.CAZeroLiquidationAmt != null) { ProcessSetLiquidationAmtToZero(); }

                    if (DataFixDocument.Current.CAStuckRefundFixer != null) { ProcessCAStuckRefundFixer(); }

                    if (DataFixDocument.Current.CARecalcLiquidationAmt != null) { ProcessCARecalcLiquidationAmt(); }

                    if (DataFixDocument.Current.FundManagementPreferencePopulateGLAccountsSetup ?? false) { UpdateFundManagementPrefereceGLAccountSetup(); }

                    if (DataFixDocument.Current.FundHistoryErNbr != null) { ForceReceiptStatusToLiquidated(this); }

                    if (DataFixDocument.Current.ClaimDetailCD != null) { ForceReceiptStatusToCancel(this); }

                    if (DataFixDocument.Current.FundHistoryBalanceFixer != null)
                    {
                        FundTransactionHistoryBalanceFixer(DataFixDocument.Current.FundHistoryBalanceFixer);
                    }

                    if (DataFixDocument.Current.FundHistoryDuplicateBalanceFixer != null)
                    {
                        FundTransactionHistoryRemoveDuplicateAndFixBalance();
                    }

                    if (DataFixDocument.Current.FTReceiptsBranchIDMigration ?? false) { FTReceiptsBranchMigrationForOldData(); }

                    if (DataFixDocument.Current.RemoveReimbursementFTDetails != null) { RemoveReimbursementFTDetailsProcess(); }

                    ts.Complete();
                }

                if (updateFTRecords.Any()) GenerateExcelFile(updateFTRecords, "FTLineDescriptionUpdates");

                if (updateCARecords.Any()) GenerateExcelFile(updateCARecords, "CALineDescriptionUpdates");
            });
            return adapter.Get();
        }
        #endregion

        #region Events
        protected virtual void _(Events.RowSelected<ATPTEFMCashFundDataFix> e)
        {
            ATPTEFMCashFundDataFix row = e.Row;

            if (row is null) return;
            ValidatePassword.SetEnabled(!row.EnableFields ?? false);
        }
        #endregion

        #region Methods
        /*This function, update the cash advance status pending liquidation to close
          NMD = Not Migrated Data*/
        public void UpdateCashAdvanceStatusToClose()
        {
            ATPTEFMCashAdvanceEntry graph = PXGraph.CreateInstance<ATPTEFMCashAdvanceEntry>();

            foreach (ATPTEFMCashAdvance result in PXSelectJoin<
                ATPTEFMCashAdvance,
                InnerJoin<APRegister,
                    On<APRegister.refNbr, Equal<ATPTEFMCashAdvance.billRefNbr>,
                    And<APRegister.status, Equal<APDocStatus.closed>,
                    And<APRegister.docType, Equal<APDocType.prepayment>>>>>,
                Where<DAC.ATPTEFMCashAdvance.status, Equal<ATPTEFMCashAdvanceStatusAttribute.pendingLiquidationValue>,
                    And<ATPTEFMCashAdvance.isImported, Equal<False>>>>
                .Select(this))
            {
                graph.Clear();
                graph.CashAdvances.Current = result;
                result.Status = ATPTEFMCashAdvanceStatusAttribute.ClosedValue;
                graph.CashAdvances.Update(result);
                graph.Actions.PressSave();
            }
        }

        private void UpdateBalanceEqualToPpmBalance()
        {
            ATPTEFMCashAdvanceEntry graph = PXGraph.CreateInstance<ATPTEFMCashAdvanceEntry>();

            foreach (PXResult<ATPTEFMCashAdvance, APRegister> obj in PXSelectJoin<
                ATPTEFMCashAdvance,
                InnerJoin<APRegister,
                    On<APRegister.refNbr, Equal<ATPTEFMCashAdvance.billRefNbr>,
                    And<APRegister.docType, Equal<APDocType.prepayment>>>>,
                Where<ATPTEFMCashAdvance.status, Equal<ATPTEFMCashAdvanceStatusAttribute.pendingLiquidationValue>,
                    And<ATPTEFMCashAdvance.curyChangeAmount, NotEqual<APRegister.curyDocBal>>>>
                .Select(this))
            {
                graph.Clear();
                ATPTEFMCashAdvance cashAdvance = obj;
                APRegister apRegister = obj;

                graph.CashAdvances.Current = cashAdvance;
                cashAdvance.CuryChangeAmount = apRegister.CuryDocBal;
                cashAdvance.ChangeAmount = apRegister.CuryDocBal;
                graph.CashAdvances.Update(cashAdvance);
                graph.Actions.PressSave();
            }
        }
        public void UpdateExpenseClaimStatus()
        {
            ExpenseClaimEntry graph = PXGraph.CreateInstance<ExpenseClaimEntry>();

            string[] refNbrList = DataFixDocument.Current.RefNbr.Split(';');

            foreach (var refNbr in refNbrList)
            {
                EPExpenseClaim claim = PXSelect<
                    EPExpenseClaim,
                    Where<EPExpenseClaim.refNbr, Equal<Required<EPExpenseClaim.refNbr>>>>
                    .Select(this, refNbr.Trim());

                if (claim != null)
                {
                    graph.Clear();
                    graph.ExpenseClaim.Current = PXSelect<
                        EPExpenseClaim,
                        Where<EPExpenseClaim.refNbr, Equal<Required<EPExpenseClaim.refNbr>>>>
                        .Select(this, refNbr.Trim());
                    graph.ExpenseClaim.Current.Status = this.DataFixDocument.Current.Status;
                    if (DataFixDocument.Current.Status == EPExpenseClaimStatus.HoldStatus)
                    {
                        graph.ExpenseClaim.Current.Hold = true;
                        graph.ExpenseClaim.Current.Approved = false;
                        graph.ExpenseClaim.Current.Released = false;
                    }
                    graph.ExpenseClaim.UpdateCurrent();
                    graph.Actions.PressSave();
                }
            }
        }
        public void InsertDefaultsForOldFunds()
        {
            ATPTEFMFundMaint graph = PXGraph.CreateInstance<ATPTEFMFundMaint>();
            ATPTEFMSetup setup = PXSelect<ATPTEFMSetup>.Select(this);

            foreach (ATPTEFMFund funds in PXSelect<
                ATPTEFMFund,
                Where<ATPTEFMFund.fundTransactionLimit, IsNull,
                    Or<ATPTEFMFund.fundTransactionRestriction, IsNull>>>
                .Select(this))
            {
                graph.Clear();
                funds.FundTransactionLimit = setup.FundTransactionLimit;
                funds.FundTransactionRestriction = ATPTEFMReplenishmentStringListAttribute.WarningValue;
                funds.FundTransactionPointPercent = 0;
                funds.FundTransactionAmt = 0m;
                graph.CurrentDocument.Update(funds);
                graph.Save.Press();
            }
        }
        /// <summary>
        /// Updates existing funds that have a null or empty clearing account or
        /// clearing subaccount with the values configured in Fund Management Preferences.
        /// Performs a direct database update to bypass business logic and improve performance.
        /// </summary>
        public void UpdateFundClearingAccounts()
        {
            ATPTEFMSetup setup = PXSelect<ATPTEFMSetup>.Select(this);

            if (setup == null || setup.ClearingAccount == null || setup.ClearingSubaccount == null)
            {
                throw new PXException(Messages.ATPTEFMMessages.ClearingAccountNotConfigured);
            }

            foreach (ATPTEFMFund fund in PXSelect<
                ATPTEFMFund,
                Where<ATPTEFMFund.clearingAccount, IsNull,
                    Or<ATPTEFMFund.clearingSubaccount, IsNull>>>
                .Select(this))
            {
                PXDatabase.Update<ATPTEFMFund>(
                    new PXDataFieldAssign<ATPTEFMFund.clearingAccount>(setup.ClearingAccount),
                    new PXDataFieldAssign<ATPTEFMFund.clearingSubaccount>(setup.ClearingSubaccount),
                    new PXDataFieldRestrict<ATPTEFMFund.fundID>(fund.FundID)
                );
            }
        }
        public void UpdateBlankUnitCost()
        {
            ATPTEFMCashAdvanceEntry graph = PXGraph.CreateInstance<ATPTEFMCashAdvanceEntry>();

            foreach (ATPTEFMCARequestDetail caRequest in PXSelect<
                ATPTEFMCARequestDetail,
                Where<ATPTEFMCARequestDetail.curyUnitCost, IsNull,
                    Or<ATPTEFMCARequestDetail.curyUnitCost, Equal<decimal0>>>>
                .SelectWindowed(this, 0, 5000))
            {
                graph.Clear();

                ATPTEFMCashAdvance ca = PXSelect<
                    ATPTEFMCashAdvance,
                    Where<ATPTEFMCashAdvance.cashAdvanceNbr, Equal<Required<ATPTEFMCashAdvance.cashAdvanceNbr>>>>
                    .Select(this, caRequest.CashAdvanceNbr);
                graph.CashAdvances.Current = ca;

                ca.ExecuteValidations = false;

                caRequest.CuryUnitCost = caRequest.CuryAmount / caRequest.Qty;

                graph.CashAdvanceRequestLines.Update(caRequest);
                graph.Actions.PressSave();
            }
        }
        public void UpdateReceiptBlankUnitCost()
        {
            ATPTEFMCashAdvanceEntry graph = PXGraph.CreateInstance<ATPTEFMCashAdvanceEntry>();

            foreach (PXResult<ATPTEFMCAReceiptDetail, ATPTEFMCashAdvance> obj in PXSelectJoin<
                ATPTEFMCAReceiptDetail,
                InnerJoin<ATPTEFMCashAdvance,
                    On<ATPTEFMCashAdvance.cashAdvanceNbr, Equal<ATPTEFMCAReceiptDetail.cashAdvanceNbr>>>,
                Where<ATPTEFMCAReceiptDetail.curyNetUnitCost, IsNull,
                    And<ATPTEFMCAReceiptDetail.taxZoneID, IsNotNull>>>
                .SelectWindowed(this, 0, 5000))
            {
                ATPTEFMCAReceiptDetail caReceipt = obj;
                ATPTEFMCashAdvance ca = obj;

                graph.CashAdvances.Current = ca;
                ca.ExecuteValidations = false;

                if (caReceipt.NetQty == 0)
                    caReceipt.CuryNetUnitCost = 0;
                else
                    caReceipt.CuryNetUnitCost = caReceipt.CuryNetAmt / caReceipt.NetQty;

                graph.CashAdvanceReceiptLines.Update(caReceipt);
                graph.Save.Press();
            }
        }
        public void UnlinkCABillRefNbr()
        {
            ATPTEFMCashAdvanceEntry graph = PXGraph.CreateInstance<ATPTEFMCashAdvanceEntry>();

            string[] refNbrList = DataFixDocument.Current.UnlinkCABillRefNbr.Split(';');

            foreach (var refNbr in refNbrList)
            {
                ATPTEFMCashAdvance ca = PXSelect<
                    ATPTEFMCashAdvance,
                    Where<ATPTEFMCashAdvance.cashAdvanceNbr, Equal<Required<ATPTEFMCashAdvance.cashAdvanceNbr>>>>
                    .Select(this, refNbr.Trim());

                if (ca != null)
                {
                    graph.Clear();
                    graph.CashAdvances.Current = ca;

                    ca.BillRefNbr = null;

                    graph.CashAdvances.Update(ca);
                    graph.Save.Press();
                }
            }
        }
        public void EmptyReplenishmentDetailReplenishment()
        {
            ATPTEFMReplenishmentEntry graph = PXGraph.CreateInstance<ATPTEFMReplenishmentEntry>();

            string[] refNbrList = DataFixDocument.Current.EmptyReplenishmentDetailReplenishmentRefNbr.Split(';');

            foreach (var refNbr in refNbrList)
            {
                ATPTEFMReplenishment rep = PXSelect<
                    ATPTEFMReplenishment,
                    Where<ATPTEFMReplenishment.replenishmentNbr, Equal<Required<ATPTEFMReplenishment.replenishmentNbr>>>>
                    .Select(this, refNbr.Trim());

                if (rep != null)
                {
                    graph.Clear();
                    graph.Replenishments.Current = rep;

                    rep.ClaimAmount = 0m;
                    rep.WithholdingTaxAmount = 0m;
                    rep.VatAmount = 0m;

                    graph.Replenishments.Update(rep);
                }

                foreach (ATPTEFMReplenishmentTaxDetail repTax in PXSelect<
                    ATPTEFMReplenishmentTaxDetail,
                    Where<ATPTEFMReplenishmentTaxDetail.replenishmentNbr, Equal<Required<ATPTEFMReplenishmentTaxDetail.replenishmentNbr>>>>
                    .Select(this, rep.ReplenishmentNbr))
                {
                    if (repTax != null)
                    {
                        graph.Taxes.Delete(repTax);
                    }
                }

                graph.Save.Press();
            }
        }
        public void DataFixForFTReceiptswithReplenishmentRefNbrbutalreadyDeletedinReplenishmentProcess()
        {
            ATPTEFMFundTransactionEntry graph = PXGraph.CreateInstance<ATPTEFMFundTransactionEntry>();

            foreach (ATPTEFMFundTransactionReceiptDetail ftReceipt in PXSelect<
                ATPTEFMFundTransactionReceiptDetail,
                Where<ATPTEFMFundTransactionReceiptDetail.replenishmentRefNbr, IsNotNull>>
                .Select(this))
            {

                ATPTEFMReplenishmentDetail repDet = PXSelect<
                    ATPTEFMReplenishmentDetail,
                    Where<ATPTEFMReplenishmentDetail.replenishmentNbr, Equal<Required<ATPTEFMReplenishmentDetail.replenishmentNbr>>,
                        And<ATPTEFMReplenishmentDetail.expenseReceiptNbr, Equal<Required<ATPTEFMReplenishmentDetail.expenseReceiptNbr>>>>>
                    .Select(this, ftReceipt.ReplenishmentRefNbr, ftReceipt.ExpenseReceiptRefNbr);

                if (repDet == null)
                {
                    graph.Clear();
                    graph.FundTransactions.Current = PXSelect<
                        ATPTEFMFundTransaction,
                        Where<ATPTEFMFundTransaction.refNbr, Equal<Required<ATPTEFMFundTransaction.refNbr>>>>
                        .Select(this, ftReceipt.FundTransactionRefNbr);

                    graph.FundTransactionReceiptLines.Current = ftReceipt;
                    graph.FundTransactionReceiptLines.Current.ReplenishmentRefNbr = null;
                    graph.FundTransactionReceiptLines.UpdateCurrent();

                    graph.Actions.PressSave();
                }
            }
        }
        public void NullChangeAmountDataFixProcess()
        {
            ATPTEFMFundTransactionEntry graph = PXGraph.CreateInstance<ATPTEFMFundTransactionEntry>();

            string[] refNbrList = DataFixDocument.Current.NullChangeAmountDataFix.Split(';');

            foreach (var refNbr in refNbrList)
            {
                foreach (ATPTEFMFundTransaction result in PXSelectJoin<
                    ATPTEFMFundTransaction,
                    InnerJoin<ATPTEFMFundTransactionReceiptDetail,
                        On<ATPTEFMFundTransaction.refNbr, Equal<ATPTEFMFundTransactionReceiptDetail.fundTransactionRefNbr>>,
                    InnerJoin<ATPTEFMReplenishmentDetail,
                        On<ATPTEFMFundTransactionReceiptDetail.replenishmentRefNbr, Equal<ATPTEFMReplenishmentDetail.replenishmentNbr>>>>,
                    Where<ATPTEFMReplenishmentDetail.replenishmentNbr, Equal<Required<ATPTEFMReplenishmentDetail.replenishmentNbr>>,
                        And<ATPTEFMFundTransaction.changeAmount, IsNull>>>
                    .Select(this, refNbr.Trim()))
                {
                    graph.FundTransactions.Current = result;

                    result.ChangeAmount = 0m;

                    graph.FundTransactions.Update(result);
                    graph.Save.Press();

                    graph.Clear();
                }
            }
        }
        public void UpdateCABudgetEnabledProcess()
        {
            ATPTEFMCashAdvanceEntry caGraph = PXGraph.CreateInstance<ATPTEFMCashAdvanceEntry>();

            foreach (ATPTEFMCashAdvance result in PXSelectJoinGroupBy<
                ATPTEFMCashAdvance,
                InnerJoin<ATPTEFMBudgetHistory,
                    On<ATPTEFMBudgetHistory.refNbr, Equal<ATPTEFMCashAdvance.cashAdvanceNbr>>>,
                Aggregate<
                    GroupBy<ATPTEFMBudgetHistory.refNbr>>>
                .Select(this))
            {
                caGraph.Clear();
                caGraph.CashAdvances.Current = result;

                result.BudgetEnabled = true;

                caGraph.CashAdvances.Update(result);
                caGraph.Save.Press();
            }
        }
        public void UpdateFTBudgetEnabledProcess()
        {
            ATPTEFMFundTransactionEntry ftGraph = PXGraph.CreateInstance<ATPTEFMFundTransactionEntry>();

            foreach (ATPTEFMFundTransaction result in PXSelectJoinGroupBy<
                ATPTEFMFundTransaction,
                InnerJoin<ATPTEFMBudgetHistory,
                    On<ATPTEFMBudgetHistory.refNbr, Equal<ATPTEFMFundTransaction.refNbr>>>,
                Aggregate<
                    GroupBy<ATPTEFMBudgetHistory.refNbr>>>
                .Select(this))
            {
                ftGraph.Clear();
                ftGraph.FundTransactions.Current = result;

                result.BudgetEnabled = true;

                ftGraph.FundTransactions.Update(result);
                ftGraph.Save.Press();
            }
        }
        public void UpdateRFPBudgetEnabledProcess()
        {
            ExpenseClaimEntry ecGraph = PXGraph.CreateInstance<ExpenseClaimEntry>();

            foreach (EPExpenseClaim result in PXSelectJoinGroupBy<
                EPExpenseClaim,
                InnerJoin<ATPTEFMBudgetHistory,
                    On<ATPTEFMBudgetHistory.refNbr, Equal<EPExpenseClaim.refNbr>>>,
                Where<ATPTEFMEPExpenseClaimExt.usrATPTEFMTranType, Equal<ATPTEFMExpenseTypeAttribute.requestforPayment>>,
                Aggregate<
                    GroupBy<ATPTEFMBudgetHistory.refNbr>>>
                .Select(this))
            {
                ATPTEFMEPExpenseClaimExt resultExt = result.GetExtension<ATPTEFMEPExpenseClaimExt>();
                ecGraph.Clear();
                ecGraph.ExpenseClaim.Current = result;

                resultExt.UsrATPTEFMBudgetEnabled = true;

                ecGraph.ExpenseClaim.Update(result);
                ecGraph.Save.Press();
            }
        }
        public void UpdateBillsBudgetEnabledProcess()
        {
            APInvoiceEntry apGraph = PXGraph.CreateInstance<APInvoiceEntry>();

            foreach (APInvoice result in PXSelectJoinGroupBy<
                APInvoice,
                InnerJoin<ATPTEFMBudgetHistory,
                    On<ATPTEFMBudgetHistory.refNbr, Equal<APInvoice.refNbr>>>,
                Aggregate<
                    GroupBy<ATPTEFMBudgetHistory.refNbr>>>
                .Select(this))
            {
                ATPTEFMAPInvoiceExtension resultExt = result.GetExtension<ATPTEFMAPInvoiceExtension>();
                apGraph.Clear();
                apGraph.Document.Current = result;

                resultExt.UsrATPTEFMBudgetEnabled = true;

                apGraph.Document.Update(result);
                apGraph.Save.Press();
            }
        }
        public void UpdateCAInitialLiquidationDateAndUnmodifiedLiqDate()
        {
            ATPTEFMCASetup caSetup = PXSelect<ATPTEFMCASetup>.Select(this);
            ATPTEFMCashAdvanceEntry graph = PXGraph.CreateInstance<ATPTEFMCashAdvanceEntry>();

            foreach (ATPTEFMCashAdvance result in PXSelect<
                ATPTEFMCashAdvance,
                Where<ATPTEFMCashAdvance.status, NotEqual<ATPTEFMCashAdvanceStatusAttribute.cancelledValue>,
                    And<ATPTEFMCashAdvance.initialLiqDate, IsNull>>>
                .Select(this))
            {
                graph.Clear();
                graph.CashAdvances.Current = result;

                var liquidationDate = (caSetup.LiquidationDateBasedOnWorkCalendar ?? false) ? graph.GetLiquidationDateWorkCalendar() : result.DateOfUse.Value.AddDays(graph.GetNumberOfLiquidationDays());

                PXDatabase.Update<ATPTEFMCashAdvance>(
                new PXDataFieldAssign<ATPTEFMCashAdvance.initialLiqDate>(liquidationDate),
                new PXDataFieldAssign<ATPTEFMCashAdvance.unmodifiedLiqDate>(liquidationDate),
                new PXDataFieldRestrict<ATPTEFMCashAdvance.cashAdvanceNbr>(result.CashAdvanceNbr)
                );
            }
        }
        public void UpdateFundReplenishPoint()
        {
            ATPTEFMFundMaint graph = PXGraph.CreateInstance<ATPTEFMFundMaint>();

            string[] refNbrList = DataFixDocument.Current.FundReplenishPoint.Split(';');

            foreach (var refNbr in refNbrList)
            {
                ATPTEFMFund fund = PXSelect<
                    ATPTEFMFund,
                    Where<ATPTEFMFund.fundCD, Equal<Required<ATPTEFMFund.fundCD>>>>
                    .Select(this, refNbr.Trim());

                if (fund != null)
                {
                    graph.Clear();
                    fund.ReplenishPointPercent = DataFixDocument.Current.ReplenishPointPercent;
                    fund.ReplenishmentAmt = (DataFixDocument.Current.ReplenishPointPercent / 100) * fund.InitialFund;
                    graph.CurrentDocument.Update(fund);
                    graph.Save.Press();
                }
            }
        }
        // case 006523 
        public void OverrideReceiptsSetToFalse()
        {
            ATPTEFMFeaturesMaint effGraph = PXGraph.CreateInstance<ATPTEFMFeaturesMaint>();
            effGraph.Clear();

            ATPTEFMFeatures eff = PXSelect<ATPTEFMFeatures>.Select(this);
            effGraph.Setup.Current = eff;

            eff.CashAdvanceOverride = false;

            effGraph.Setup.Update(eff);
            effGraph.Save.Press();
        }
        /// <remarks>
        /// 2025-10-10 :[Guevent] Concerns in closing Fund - CASE: 013476 {JLG}  <br/> 
        /// </remarks>
        public void UpdateFTInitialLiquidationDate()
        {
            ATPTEFMFundTransactionEntry graph = PXGraph.CreateInstance<ATPTEFMFundTransactionEntry>();

            foreach (ATPTEFMFundTransaction result in PXSelect<
                ATPTEFMFundTransaction,
                Where<ATPTEFMFundTransaction.status, NotEqual<ATPTEFMCashAdvanceStatusAttribute.cancelledValue>,
                    And<ATPTEFMFundTransaction.status, NotEqual<ATPTEFMCashAdvanceStatusAttribute.closedValue>,
                    And<ATPTEFMFundTransaction.initialLiqDate, IsNull,
                    And<ATPTEFMFundTransaction.dateOfUse, IsNotNull,
                    And<ATPTEFMFundTransaction.liqDate, IsNotNull>>>>>>
                .Select(this))
            {
                graph.Clear();
                graph.FundTransactions.Current = result;

                ATPTEFMSetup setup = PXSelect<ATPTEFMSetup>.Select(this);

                var liquidationDate = (setup.LiquidationDateBasedOnWorkCalendar ?? false) ? graph.GetLiquidationDateWorkCalendar() : result.DateOfUse.Value.AddDays(graph.GetNumberOfLiquidationDays());

                result.InitialLiqDate = liquidationDate;
                result.LiqDate = liquidationDate;
                result.UnmodifiedLiqDate = liquidationDate;

                graph.FundTransactions.Update(result);
                graph.Save.Press();
            }
        }
        public void ECwithMissingDetails()
        {
            PXResultset<APInvoice> rows = PXSelectJoin<
                APInvoice,
                InnerJoin<EPExpenseClaimDetails,
                    On<EPExpenseClaimDetails.aPRefNbr, Equal<APInvoice.refNbr>>>,
                Where<EPExpenseClaimDetails.refNbr, IsNull,
                    And<APInvoice.origRefNbr, IsNotNull>>>
                .Select(this);

            //ExpenseClaimDetailEntry erGraph = PXGraph.CreateInstance<ExpenseClaimDetailEntry>();
            ExpenseClaimEntry ecGraph = PXGraph.CreateInstance<ExpenseClaimEntry>();

            foreach (APInvoice invoice in PXSelectJoin<
                APInvoice,
                InnerJoin<EPExpenseClaimDetails,
                    On<EPExpenseClaimDetails.aPRefNbr, Equal<APInvoice.refNbr>,
                    And<EPExpenseClaimDetails.aPDocType, Equal<APInvoice.docType>>>>,
                Where<EPExpenseClaimDetails.refNbr, IsNull,
                    And<APInvoice.origRefNbr, IsNotNull>>>
                .Select(this))
            {
                EPExpenseClaim ec = EPExpenseClaim.PK.Find(this, invoice.OrigRefNbr);
                if (ec != null)
                {
                    ecGraph.Clear();
                    ecGraph.ExpenseClaim.Current = ec;

                    string tempStatus;
                    bool tempHold, tempReleased;

                    tempStatus = ec.Status;
                    tempHold = ec.Hold ?? false;
                    tempReleased = ec.Released ?? false;

                    ec.Status = EPExpenseClaimStatus.HoldStatus;
                    ec.Hold = true;
                    ec.Released = false;

                    ecGraph.ExpenseClaim.Update(ec);
                    ecGraph.Save.Press();

                    foreach (APTran invoiceLine in PXSelect<
                        APTran,
                        Where<APTran.refNbr, Equal<@P.AsString>,
                            And<APTran.tranType, Equal<P.AsString>>>>
                        .Select(this, invoice.RefNbr, invoice.DocType))
                    {
                        string apTranERRefNbr = getAPTranExpenseReceiptNbr(invoiceLine);

                        EPExpenseClaimDetails er = EPExpenseClaimDetails.PK.Find(this, apTranERRefNbr);

                        if (er != null)
                        {
                            //erGraph.Clear();
                            //erGraph.ClaimDetails.Current = er;

                            //er.RefNbr = invoice.OrigRefNbr;

                            //erGraph.ClaimDetails.Update(er);
                            //erGraph.Save.Press();

                            var key = new EPExpenseClaimDetails { ClaimDetailCD = er.ClaimDetailCD, ClaimDetailID = er.ClaimDetailID };
                            EPExpenseClaimDetails origDetails = ecGraph.ExpenseClaimDetails.Locate(key) ?? er;
                            EPExpenseClaimDetails details = (EPExpenseClaimDetails)ecGraph.ExpenseClaimDetails.Cache.CreateCopy(origDetails);
                            ecGraph.FindImplementation<ExpenseClaimEntry.ExpenseClaimEntryReceiptExt>().SubmitReceiptExt(ecGraph.ExpenseClaim.Cache, ecGraph.ExpenseClaimDetails.Cache, ecGraph.ExpenseClaim.Current, details);
                        }
                    }

                    ec.Status = tempStatus;
                    ec.Hold = tempHold;
                    ec.Released = tempReleased;

                    ecGraph.ExpenseClaim.Update(ec);
                    ecGraph.Save.Press();
                }
            }
        }

        //For those replenishment bills that have already been reversed but document still have a status of closed that needs to be reopen and its receipts.
        protected void ReopenReplenishment()
        {

            ExpenseClaimDetailEntry ecGraph = PXGraph.CreateInstance<ExpenseClaimDetailEntry>();
            ATPTEFMFundTransactionEntry ftGraph = PXGraph.CreateInstance<ATPTEFMFundTransactionEntry>();
            ATPTEFMReplenishmentEntry replenishGraph = PXGraph.CreateInstance<ATPTEFMReplenishmentEntry>();
            ATPTEFMFundMaint fundGraph = PXGraph.CreateInstance<ATPTEFMFundMaint>();

            string[] refNbrList = DataFixDocument.Current.ReplenishmentNrbNeedsToBeOpen.Split(';');

            foreach (var refNbr in refNbrList)
            {

                PXResultset<ATPTEFMReplenishmentDetail> RepDetails = PXSelect<
                    ATPTEFMReplenishmentDetail,
                    Where<ATPTEFMReplenishmentDetail.replenishmentNbr, Equal<Required<ATPTEFMReplenishmentDetail.replenishmentNbr>>>>
                    .Select(this, refNbr.Trim());

                foreach (PXResult<ATPTEFMReplenishmentDetail> rep in RepDetails)
                {
                    ATPTEFMReplenishmentDetail det = rep;

                    PXResultset<ATPTEFMReplenishment, ATPTEFMReplenishmentDetail, EPExpenseClaimDetails, ATPTEFMFundTransactionReceiptDetail, ATPTEFMFundTransaction, ATPTEFMFundTransactionHistoryView, ATPTEFMFund> ds =
                  PXSelectJoin<
                      ATPTEFMReplenishment,
                      InnerJoin<ATPTEFMReplenishmentDetail,
                          On<ATPTEFMReplenishmentDetail.replenishmentNbr, Equal<ATPTEFMReplenishment.replenishmentNbr>>,
                      InnerJoin<EPExpenseClaimDetails,
                          On<EPExpenseClaimDetails.claimDetailCD, Equal<ATPTEFMReplenishmentDetail.expenseReceiptNbr>>,
                      InnerJoin<ATPTEFMFundTransactionReceiptDetail,
                          On<ATPTEFMFundTransactionReceiptDetail.expenseReceiptRefNbr, Equal<EPExpenseClaimDetails.claimDetailCD>>,
                      InnerJoin<ATPTEFMFundTransaction,
                          On<ATPTEFMFundTransaction.refNbr, Equal<ATPTEFMFundTransactionReceiptDetail.fundTransactionRefNbr>>,
                      InnerJoin<ATPTEFMFundTransactionHistoryView,
                          On<ATPTEFMFundTransactionHistoryView.refNbr, Equal<ATPTEFMFundTransactionReceiptDetail.expenseReceiptRefNbr>>,
                      InnerJoin<ATPTEFMFund,
                          On<ATPTEFMFund.fundCD, Equal<ATPTEFMFundTransactionHistoryView.fundRefNbr>>>>>>>>,
                      Where<EPExpenseClaimDetails.claimDetailCD, Equal<Required<EPExpenseClaimDetails.claimDetailCD>>>>
                      .Select<PXResultset<ATPTEFMReplenishment, ATPTEFMReplenishmentDetail, EPExpenseClaimDetails, ATPTEFMFundTransactionReceiptDetail, ATPTEFMFundTransaction, ATPTEFMFundTransactionHistoryView, ATPTEFMFund>>(this, det.ExpenseReceiptNbr);


                    ATPTEFMReplenishment replenishment = (ATPTEFMReplenishment)ds;
                    EPExpenseClaimDetails claimDetails = (EPExpenseClaimDetails)ds;
                    ATPTEFMFundTransaction ft = (ATPTEFMFundTransaction)ds;
                    ATPTEFMFundTransactionReceiptDetail ftReceipts = (ATPTEFMFundTransactionReceiptDetail)ds;
                    ATPTEFMFund fund = (ATPTEFMFund)ds;
                    ATPTEFMFundTransactionHistoryView erHistory = (ATPTEFMFundTransactionHistoryView)ds;

                    ftGraph.FundTransactions.Current = ft;
                    ftGraph.FundTransactionReceiptLines.Current = ftReceipts;
                    if (ft != null && ftReceipts != null)
                    {
                        ftReceipts.ReplenishmentRefNbr = null;
                        ftGraph.FundTransactionReceiptLines.Update(ftReceipts);
                        ftGraph.Save.Press();
                    }

                    ecGraph.ClaimDetails.Current = claimDetails;
                    if (ecGraph.ClaimDetails.Current != null)
                    {
                        claimDetails.Status = EPExpenseClaimDetailsStatus.ApprovedStatus;
                        claimDetails.APRefNbr = null;
                        claimDetails.APDocType = null;
                        ecGraph.ClaimDetails.Update(claimDetails);
                        ecGraph.Save.Press();
                    }


                    fundGraph.Document.Current = fund;
                    fundGraph.CurrentTransactionHistoryView.Current = erHistory;
                    if (fundGraph.Document.Current != null)
                    {
                        erHistory.Status = ATPTEFMFundStatusAttribute.OpenValue;
                        fundGraph.CurrentTransactionHistoryView.Update(erHistory);
                        fundGraph.Save.Press();
                    }

                    replenishGraph.Replenishments.Current = replenishment;

                    if (replenishGraph.Replenishments.Current != null
                        && replenishGraph.Replenishments.Current.Status != ATPTEFMReplenishmentStatusAttribute.OpenValue
                        && replenishGraph.Replenishments.Current.Step != ATPTEFMReplenishmentStepAttribute.SubmitReceiptValue
                        && replenishGraph.Replenishments.Current.IsReleased != false)
                    {
                        replenishment.Status = ATPTEFMReplenishmentStatusAttribute.OpenValue;
                        replenishment.Step = ATPTEFMReplenishmentStepAttribute.SubmitReceiptValue;
                        replenishment.IsReleased = false;
                        replenishGraph.Replenishments.Update(replenishment);
                        replenishGraph.Save.Press();
                    }
                }
            }
        }

        public void UpdateExpectedDateOfUse()
        {
            ATPTEFMFundTransactionEntry graph = PXGraph.CreateInstance<ATPTEFMFundTransactionEntry>();

            foreach (ATPTEFMFundTransaction result in PXSelect<
                ATPTEFMFundTransaction,
                Where<ATPTEFMFundTransaction.status, NotEqual<ATPTEFMCashAdvanceStatusAttribute.cancelledValue>,
                    And<ATPTEFMFundTransaction.status, NotEqual<ATPTEFMCashAdvanceStatusAttribute.closedValue>,
                    And<ATPTEFMFundTransaction.fundTransactionType, Equal<ATPTEFMFundTransactionTypeAttribute.cashAdvanceValue>,
                    And<ATPTEFMFundTransaction.initialLiqDate, IsNull,
                    And<ATPTEFMFundTransaction.dateOfUse, IsNull,
                    And<ATPTEFMFundTransaction.liqDate, IsNull>>>>>>>
                .Select(this))
            {
                graph.Clear();
                graph.FundTransactions.Current = result;

                ATPTEFMSetup setup = PXSelect<ATPTEFMSetup>.Select(this);

                var liquidationDate = result.Date.Value.AddDays(graph.GetNumberOfLiquidationDays());

                result.DateOfUse = result.Date;
                result.InitialLiqDate = liquidationDate;
                result.UnmodifiedLiqDate = liquidationDate;

                PXDatabase.Update<ATPTEFMFundTransaction>(
                    new PXDataFieldAssign<ATPTEFMFundTransaction.dateOfUse>(result.Date),
                    new PXDataFieldAssign<ATPTEFMFundTransaction.initialLiqDate>(liquidationDate),
                    new PXDataFieldAssign<ATPTEFMFundTransaction.liqDate>(liquidationDate),
                    new PXDataFieldAssign<ATPTEFMFundTransaction.unmodifiedLiqDate>(liquidationDate),
                    new PXDataFieldRestrict<ATPTEFMFundTransaction.refNbr>(result.RefNbr)
                );
            }
        }
        public void UpdateFTLineDescription()
        {
            int detailsUpdated = 0;
            int receiptsUpdated = 0;

            // Update Fund Transaction Details
            foreach (ATPTEFMFundTransactionDetail result in PXSelect<
                ATPTEFMFundTransactionDetail,
                Where<ATPTEFMFundTransactionDetail.lineDescription, Equal<Empty>,
                    Or<ATPTEFMFundTransactionDetail.lineDescription, IsNull>>>
                .Select(this))
            {
                if (result.InventoryID != null)
                {
                    InventoryItem inv = InventoryItem.PK.Find(this, result.InventoryID);

                    PXDatabase.Update<ATPTEFMFundTransactionDetail>(
                        new PXDataFieldAssign<ATPTEFMFundTransactionDetail.lineDescription>(inv.Descr),
                        new PXDataFieldRestrict<ATPTEFMFundTransactionDetail.fundTransactionDetailID>(result.FundTransactionDetailID)
                    );
                    detailsUpdated++;
                    updateFTRecords.Add(new FTUpdateRecord
                    {
                        RefNbr = result.FundTransactionRefNbr,
                        Type = "Detail",
                        Description = inv.Descr,
                        UpdateDate = DateTime.Now
                    });
                }
            }

            // Update Fund Transaction Receipt Details
            foreach (ATPTEFMFundTransactionReceiptDetail result in PXSelect<
                ATPTEFMFundTransactionReceiptDetail,
                Where<ATPTEFMFundTransactionReceiptDetail.lineDescription, Equal<Empty>,
                    Or<ATPTEFMFundTransactionReceiptDetail.lineDescription, IsNull>>>
                .Select(this))
            {
                EPExpenseClaimDetails er = EPExpenseClaimDetails.PK.Find(this, result.ExpenseReceiptRefNbr);
                string description = er != null ? er.TranDesc : result.Descr;

                PXDatabase.Update<ATPTEFMFundTransactionReceiptDetail>(
                    new PXDataFieldAssign<ATPTEFMFundTransactionReceiptDetail.lineDescription>(description),
                    new PXDataFieldRestrict<ATPTEFMFundTransactionReceiptDetail.fundTransactionReceiptDetailID>(result.FundTransactionReceiptDetailID)
                );
                receiptsUpdated++;
                updateFTRecords.Add(new FTUpdateRecord
                {
                    RefNbr = result.FundTransactionRefNbr,
                    Type = "Receipt",
                    Description = description,
                    UpdateDate = DateTime.Now
                });
            }
        }
        public void UpdateCALineDescription()
        {
            int receiptsUpdated = 0;

            // Update Cash Advance Receipt Details 
            foreach (ATPTEFMCAReceiptDetail result in PXSelect<
                ATPTEFMCAReceiptDetail,
                Where<ATPTEFMCAReceiptDetail.lineDescription, Equal<Empty>,
                    Or<ATPTEFMCAReceiptDetail.lineDescription, IsNull>>>
                .SelectWindowed(this, 0, 5000))
            {
                EPExpenseClaimDetails er = EPExpenseClaimDetails.PK.Find(this, result.ExpenseReceiptRefNbr);
                InventoryItem inv = InventoryItem.PK.Find(this, result.InventoryID);

                string description = er != null ? er.TranDesc : inv.Descr;

                PXDatabase.Update<ATPTEFMCAReceiptDetail>(
                    new PXDataFieldAssign<ATPTEFMCAReceiptDetail.lineDescription>(description),
                    new PXDataFieldRestrict<ATPTEFMCAReceiptDetail.cashAdvanceReceiptDetailIID>(result.CashAdvanceReceiptDetailIID)
                );
                receiptsUpdated++;
                updateCARecords.Add(new CAUpdateRecord
                {
                    CashAdvanceNbr = result.CashAdvanceNbr,
                    Description = description,
                    UpdateDate = DateTime.Now
                });
            }

            foreach (ATPTEFMCARequestDetail result in PXSelect<
                ATPTEFMCARequestDetail,
                Where<ATPTEFMCARequestDetail.remarks, Equal<Empty>,
                    Or<ATPTEFMCARequestDetail.remarks, IsNull>>>
                .SelectWindowed(this, 0, 5000))
            {
                InventoryItem inv = InventoryItem.PK.Find(this, result.InventoryID);
                string description = inv.Descr;

                PXDatabase.Update<ATPTEFMCARequestDetail>(
                    new PXDataFieldAssign<ATPTEFMCARequestDetail.remarks>(inv.Descr),
                    new PXDataFieldRestrict<ATPTEFMCARequestDetail.cashAdvanceRequestDetailID>(result.CashAdvanceRequestDetailID)
                );
            }
        }
        public void LEPSetupMethod()
        {
            ATPTEFMCASetup casetup = PXSelect<ATPTEFMCASetup>.Select(this);
            if (casetup != null)
            {
                LEPMaint lepGraph = PXGraph.CreateInstance<LEPMaint>();
                #region CA
                ListEntryPoint caEntryPoint = ListEntryPoint.PK.Find(this, "ATPT3103");
                if (caEntryPoint != null)
                {
                    lepGraph.Clear();

                    if (caEntryPoint.ListScreenID != "ATPT2101")
                        caEntryPoint.ListScreenID = "ATPT2101";

                    if (!caEntryPoint.IsActive ?? false)
                        caEntryPoint.IsActive = true;

                    lepGraph.Items.Update(caEntryPoint);
                    lepGraph.Save.Press();
                }
                else
                {
                    ListEntryPoint insertNew = new ListEntryPoint();
                    insertNew.EntryScreenID = "ATPT3103";
                    insertNew.ListScreenID = "ATPT2101";
                    insertNew.IsActive = true;

                    lepGraph.Items.Insert(insertNew);
                    lepGraph.Save.Press();
                }
                #endregion
                #region Funds
                ListEntryPoint fundEntryPoint = ListEntryPoint.PK.Find(this, "ATPT2012");
                if (fundEntryPoint != null)
                {
                    lepGraph.Clear();

                    if (fundEntryPoint.ListScreenID != "ATPT2104")
                        fundEntryPoint.ListScreenID = "ATPT2104";

                    if (!fundEntryPoint.IsActive ?? false)
                        fundEntryPoint.IsActive = true;

                    lepGraph.Items.Update(fundEntryPoint);
                    lepGraph.Save.Press();
                }
                else
                {
                    ListEntryPoint insertNew = new ListEntryPoint();
                    insertNew.EntryScreenID = "ATPT2012";
                    insertNew.ListScreenID = "ATPT2104";
                    insertNew.IsActive = true;

                    lepGraph.Items.Insert(insertNew);
                    lepGraph.Save.Press();
                }
                #endregion
                #region FT
                ListEntryPoint ftEntryPoint = ListEntryPoint.PK.Find(this, "ATPT3011");
                if (ftEntryPoint != null)
                {
                    lepGraph.Clear();

                    if (ftEntryPoint.ListScreenID != "ATPT2102")
                        ftEntryPoint.ListScreenID = "ATPT2102";

                    if (!ftEntryPoint.IsActive ?? false)
                        ftEntryPoint.IsActive = true;

                    lepGraph.Items.Update(ftEntryPoint);
                    lepGraph.Save.Press();
                }
                else
                {
                    ListEntryPoint insertNew = new ListEntryPoint();
                    insertNew.EntryScreenID = "ATPT3011";
                    insertNew.ListScreenID = "ATPT2102";
                    insertNew.IsActive = true;

                    lepGraph.Items.Insert(insertNew);
                    lepGraph.Save.Press();
                }
                #endregion
                #region Replenishment
                ListEntryPoint replEntryPoint = ListEntryPoint.PK.Find(this, "ATPT3012");
                if (replEntryPoint != null)
                {
                    lepGraph.Clear();

                    if (replEntryPoint.ListScreenID != "ATPT2103")
                        replEntryPoint.ListScreenID = "ATPT2103";

                    if (!replEntryPoint.IsActive ?? false)
                        replEntryPoint.IsActive = true;

                    lepGraph.Items.Update(replEntryPoint);
                    lepGraph.Save.Press();
                }
                else
                {
                    ListEntryPoint insertNew = new ListEntryPoint();
                    insertNew.EntryScreenID = "ATPT3012";
                    insertNew.ListScreenID = "ATPT2103";
                    insertNew.IsActive = true;

                    lepGraph.Items.Insert(insertNew);
                    lepGraph.Save.Press();
                }
                #endregion
            }
        }
        public virtual string getAPTranExpenseReceiptNbr(APTran invoiceLine) => "";
        private class FTUpdateRecord
        {
            public string RefNbr { get; set; }
            public string Type { get; set; }
            public string Description { get; set; }
            public DateTime UpdateDate { get; set; }
        }

        private class CAUpdateRecord
        {
            public string CashAdvanceNbr { get; set; }
            public string Description { get; set; }
            public DateTime UpdateDate { get; set; }
        }
        private void GenerateExcelFile<T>(List<T> records, string fileName)
        {
            try
            {
                var sb = new StringBuilder();
                var properties = typeof(T).GetProperties();

                // Add headers
                sb.AppendLine(string.Join(",", properties.Select(p => p.Name)));

                // Add data rows
                foreach (var record in records)
                {
                    var values = properties.Select(p =>
                    {
                        var value = p.GetValue(record)?.ToString() ?? "";
                        // Escape commas and quotes in values
                        if (value.Contains(",") || value.Contains("\""))
                        {
                            value = value.Replace("\"", "\"\""); // Escape quotes
                            value = $"\"{value}\""; // Wrap in quotes
                        }
                        return value;
                    });
                    sb.AppendLine(string.Join(",", values));
                }

                var fullFileName = $"{fileName}_{DateTime.Now:yyyyMMdd_HHmmss}.csv";
                var bytes = Encoding.UTF8.GetBytes(sb.ToString());

                // Create FileInfo object
                var fileInfo = new PX.SM.FileInfo(fullFileName, null, bytes);
                fileInfo.UID = Guid.NewGuid();

                // Throw PXRedirectToFileException with FileInfo
                throw new PXRedirectToFileException(fileInfo, true);
            }
            catch (Exception ex)
            {
                // Only catch non-PXRedirectToFileException exceptions
                if (!(ex is PXRedirectToFileException))
                {
                    PXTrace.WriteError($"Error generating CSV file: {ex.Message}");
                    throw new PXException(Messages.ATPTEFMMessages.ErrorGeneratingCSVFile, ex.Message);
                }
                throw;
            }
        }
        public void QmazFundDataFix()
        {
            ATPTEFMFundMaint fundGraph = PXGraph.CreateInstance<ATPTEFMFundMaint>();
            string[] fundList = DataFixDocument.Current.QmazFundCD.Split(';');

            foreach (var fundCD in fundList)
            {
                fundGraph.Clear();

                string trimmedFundCD = fundCD.Trim();
                if (string.IsNullOrEmpty(trimmedFundCD)) continue;

                ATPTEFMFund fund = PXSelect<
                    ATPTEFMFund,
                    Where<ATPTEFMFund.fundCD, Equal<Required<ATPTEFMFund.fundCD>>>>
                    .Select(this, trimmedFundCD);

                if (fund == null) continue;

                fundGraph.Document.Current = fund;

                PXResultset<ATPTEFMFundTransactionHistoryView> historyRecords = PXSelect<
                    ATPTEFMFundTransactionHistoryView,
                    Where<ATPTEFMFundTransactionHistoryView.fundRefNbr, Equal<Required<ATPTEFMFundTransactionHistoryView.fundRefNbr>>,
                        And<Where<ATPTEFMFundTransactionHistoryView.transactionType,
            NotEqual<ATPTEFMFundTransactionTypeAttribute.increaseFundValue>,
                            And<ATPTEFMFundTransactionHistoryView.transactionType,
                NotEqual<ATPTEFMFundTransactionTypeAttribute.decreaseFundValue>>>>>>
                    .Select(this, trimmedFundCD);

                foreach (ATPTEFMFundTransactionHistoryView record in historyRecords)
                {
                    fundGraph.CurrentTransactionHistoryView.Delete(record);
                }
                fundGraph.Actions.PressSave();

                FundUnboundToBound(trimmedFundCD, false);

                //This is assuming that the fund has not been used
                fund.BalanceAmt = fund.FundAmt;
                fundGraph.Document.Update(fund);

                ATPTEFMFundTransactionHistoryView lasthistoryRecord = PXSelect<
                    ATPTEFMFundTransactionHistoryView,
                    Where<ATPTEFMFundTransactionHistoryView.fundRefNbr, Equal<Required<ATPTEFMFundTransactionHistoryView.fundRefNbr>>>,
                    OrderBy<
                        Desc<ATPTEFMFundTransactionHistoryView.sortNbr>>>
                    .SelectWindowed(this, 0, 1, trimmedFundCD);

                if (lasthistoryRecord != null)
                {
                    lasthistoryRecord.BalanceAmt = fund.BalanceAmt;
                    fundGraph.CurrentTransactionHistoryView.Update(lasthistoryRecord);
                }

                historyRecords = PXSelect<
                    ATPTEFMFundTransactionHistoryView,
                    Where<ATPTEFMFundTransactionHistoryView.fundRefNbr, Equal<Required<ATPTEFMFundTransactionHistoryView.fundRefNbr>>,
                        And<ATPTEFMFundTransactionHistoryView.transactionType, Equal<ATPTEFMFundTransactionTypeAttribute.establishment>>>>
                    .Select(this, trimmedFundCD);

                if (historyRecords != null && historyRecords.Count() > 1)
                {
                    foreach (ATPTEFMFundTransactionHistoryView record in historyRecords)
                    {
                        APAdjust apAdjust = PXSelect<
                            APAdjust,
                            Where<APAdjust.adjdRefNbr, Equal<Required<APAdjust.adjdRefNbr>>,
                                And<APAdjust.voided, NotEqual<True>,
                                And<APAdjust.adjgDocType, Equal<APPaymentType.debitAdj>>>>>
                            .Select(this, record.RefNbr);

                        if (apAdjust != null)
                        {
                            record.Status = ATPTEFMFundStatusAttribute.ReversedValue;
                            fundGraph.CurrentTransactionHistoryView.Update(record);
                        }
                    }
                }

                fundGraph.Actions.PressSave();
            }
        }
        protected void DeleteApprovalsOnCancelledECProcess()
        {
            string[] refNbrList = DataFixDocument.Current.ECDeleteApprovalsForCancelledStatus.Split(';');
            ExpenseClaimEntry ecGraph = PXGraph.CreateInstance<ExpenseClaimEntry>();
            foreach (var refNbr in refNbrList)
            {
                ecGraph.Clear();

                EPExpenseClaim claim = EPExpenseClaim.PK.Find(this, refNbr.Trim());
                if (claim == null) continue;

                ecGraph.ExpenseClaim.Current = claim;

                foreach (EPApproval approval in ecGraph.Approval.Select())
                {
                    ecGraph.Approval.Delete(approval);
                }

                ecGraph.Save.Press();
            }
        }
        public void FundUnboundToBound(string fundcd, bool isOldfund)
        {
            ATPTEFMFundMaint fundGraph = PXGraph.CreateInstance<ATPTEFMFundMaint>();

            #region MyRegion
            fundGraph.Document.Current = PXSelect<
                                                ATPTEFMFund,
                                                Where<ATPTEFMFund.fundCD, Equal<Required<ATPTEFMFund.fundCD>>>>
                                                .Select(this, fundcd);

            if (fundGraph.Document.Current != null)
            {
                foreach (ATPTEFMTransactionHistoryView transactionHistoryView in fundGraph.Transactions.Select().WhereNotNull())
                {
                    ATPTEFMFundTransactionHistoryView tranHistory = new ATPTEFMFundTransactionHistoryView();
                    #region Establishment
                    if (transactionHistoryView.TransactionType.Equals(ATPTEFMTransactionHistoryView.transactionType.Establishment))
                    {
                        tranHistory.FundRefNbr = fundGraph.Document.Current.FundCD;
                        tranHistory.TransactionType = transactionHistoryView.TransactionType;
                        tranHistory.OrderDate = transactionHistoryView.OrderDate;
                        tranHistory.RefNbr = transactionHistoryView.RefNbr;
                        tranHistory.SortNbr = transactionHistoryView.SortNbr;
                        tranHistory.FundBranchID = transactionHistoryView.FundBranchID;
                        tranHistory.FundType = transactionHistoryView.FundType;
                        tranHistory.TransactionDate = transactionHistoryView.TransactionDate;
                        tranHistory.FundTransactionDocumentAmt = transactionHistoryView.FundTransactionDocumentAmt;
                        tranHistory.Status = transactionHistoryView.Status;
                        tranHistory.CheckNbr = transactionHistoryView.CheckNbr;
                        tranHistory.CuryCheckAmt = transactionHistoryView.CheckAmt;
                        tranHistory.CuryBalanceAmt = transactionHistoryView.BalanceAmt;
                        tranHistory.ProjectID = null;
                        tranHistory.ProjectTaskID = null;
                        tranHistory.CostCodeID = null;
                        tranHistory.CuryDocumentBalanceAmt = transactionHistoryView.DocumentBalanceAmt;
                        fundGraph.CurrentTransactionHistoryView.Insert(tranHistory);
                    }
                    #endregion

                    #region Fund Request
                    if (transactionHistoryView.TransactionType.Equals(ATPTEFMTransactionHistoryView.transactionType.FundRequest))
                    {
                        tranHistory.FundRefNbr = fundGraph.Document.Current.FundCD;
                        tranHistory.TransactionType = transactionHistoryView.TransactionType;
                        tranHistory.OrderDate = transactionHistoryView.OrderDate;
                        tranHistory.RefNbr = transactionHistoryView.RefNbr;
                        tranHistory.SortNbr = transactionHistoryView.SortNbr;
                        tranHistory.FundBranchID = transactionHistoryView.FundBranchID;
                        tranHistory.FundType = transactionHistoryView.FundType;
                        tranHistory.TransactionDate = transactionHistoryView.TransactionDate;
                        tranHistory.FundTransactionDocumentAmt = transactionHistoryView.FundTransactionDocumentAmt;
                        tranHistory.Status = transactionHistoryView.Status;
                        tranHistory.Source = transactionHistoryView.Source;
                        tranHistory.IsUnliquidatedRequest = transactionHistoryView.IsUnliquidatedRequest;
                        tranHistory.CuryUnliquidatedAmt = transactionHistoryView.UnliquidatedAmt;
                        tranHistory.CashAdvanceStatus = transactionHistoryView.CashAdvanceStatus;
                        tranHistory.CuryFundReturnAmt = transactionHistoryView.FundReturnAmt;
                        tranHistory.CuryFundAmt = transactionHistoryView.FundAmt;
                        tranHistory.CuryBalanceAmt = transactionHistoryView.BalanceAmt;
                        tranHistory.ProjectID = null;
                        tranHistory.ProjectTaskID = null;
                        tranHistory.CostCodeID = null;
                        fundGraph.CurrentTransactionHistoryView.Insert(tranHistory);
                    }
                    #endregion

                    #region Fund Reimbursement
                    if (transactionHistoryView.TransactionType.Equals(ATPTEFMTransactionHistoryView.transactionType.FundReimbursment))
                    {
                        tranHistory.FundRefNbr = fundGraph.Document.Current.FundCD;
                        tranHistory.TransactionType = transactionHistoryView.TransactionType;
                        tranHistory.OrderDate = transactionHistoryView.OrderDate;
                        tranHistory.RefNbr = transactionHistoryView.RefNbr;
                        tranHistory.SortNbr = transactionHistoryView.SortNbr;
                        tranHistory.FundBranchID = transactionHistoryView.FundBranchID;
                        tranHistory.FundType = transactionHistoryView.FundType;
                        tranHistory.TransactionDate = transactionHistoryView.TransactionDate;
                        tranHistory.CuryFundTransactionDocumentAmt = transactionHistoryView.FundTransactionDocumentAmt;
                        tranHistory.Status = transactionHistoryView.Status;
                        tranHistory.Source = transactionHistoryView.Source;
                        tranHistory.CuryFundAmt = transactionHistoryView.FundAmt;
                        tranHistory.CuryBalanceAmt = transactionHistoryView.BalanceAmt;
                        tranHistory.ProjectID = null;
                        tranHistory.ProjectTaskID = null;
                        tranHistory.CostCodeID = null;
                        fundGraph.CurrentTransactionHistoryView.Insert(tranHistory);
                    }
                    #endregion

                    #region Expense Receipts
                    if (transactionHistoryView.TransactionType.Equals(ATPTEFMTransactionHistoryView.transactionType.ExpenseReceipt))
                    {
                        tranHistory.FundRefNbr = fundGraph.Document.Current.FundCD;
                        tranHistory.TransactionType = transactionHistoryView.TransactionType;
                        tranHistory.OrderDate = transactionHistoryView.OrderDate;
                        tranHistory.RefNbr = transactionHistoryView.RefNbr;
                        tranHistory.SortNbr = transactionHistoryView.SortNbr;
                        tranHistory.FundTransactionSortNbr = transactionHistoryView.FundTransactionSortNbr;
                        tranHistory.FundBranchID = transactionHistoryView.FundBranchID;
                        tranHistory.FundType = transactionHistoryView.FundType;
                        tranHistory.TransactionDate = transactionHistoryView.TransactionDate;
                        tranHistory.FundTransactionDocumentAmt = transactionHistoryView.FundTransactionDocumentAmt;
                        tranHistory.Status = transactionHistoryView.Status;
                        tranHistory.CuryLiquidatedAmt = transactionHistoryView.LiquidatedAmt;
                        tranHistory.CuryUnliquidatedAmt = transactionHistoryView.UnliquidatedAmt;
                        tranHistory.ReplenishmentRefNbr = transactionHistoryView.ReplenishmentRefNbr;
                        tranHistory.ProjectID = transactionHistoryView.ProjectID;
                        tranHistory.ProjectTaskID = transactionHistoryView.ProjectTaskID;
                        tranHistory.CostCodeID = transactionHistoryView.CostCodeID;
                        tranHistory.Source = transactionHistoryView.Source;
                        tranHistory.WithholdingTax = transactionHistoryView.WithholdingTax;
                        tranHistory.CuryBalanceAmt = transactionHistoryView.BalanceAmt;
                        fundGraph.CurrentTransactionHistoryView.Insert(tranHistory);
                    }
                    #endregion

                    #region Reclassification Receipts
                    if (transactionHistoryView.TransactionType.Equals(ATPTEFMTransactionHistoryView.transactionType.Reclassificaation))
                    {
                        tranHistory.FundRefNbr = fundGraph.Document.Current.FundCD;
                        tranHistory.TransactionType = transactionHistoryView.TransactionType;
                        tranHistory.OrderDate = transactionHistoryView.OrderDate;
                        tranHistory.RefNbr = transactionHistoryView.RefNbr;
                        tranHistory.SortNbr = transactionHistoryView.SortNbr;
                        tranHistory.FundTransactionSortNbr = transactionHistoryView.FundTransactionSortNbr;
                        tranHistory.FundBranchID = transactionHistoryView.FundBranchID;
                        tranHistory.FundType = transactionHistoryView.FundType;
                        tranHistory.TransactionDate = transactionHistoryView.TransactionDate;
                        tranHistory.FundTransactionDocumentAmt = transactionHistoryView.FundTransactionDocumentAmt;
                        tranHistory.Status = transactionHistoryView.Status;
                        tranHistory.CuryLiquidatedAmt = transactionHistoryView.LiquidatedAmt;
                        tranHistory.CuryUnliquidatedAmt = transactionHistoryView.UnliquidatedAmt;
                        tranHistory.ReplenishmentRefNbr = transactionHistoryView.ReplenishmentRefNbr;
                        tranHistory.ProjectID = transactionHistoryView.ProjectID;
                        tranHistory.ProjectTaskID = transactionHistoryView.ProjectTaskID;
                        tranHistory.CostCodeID = transactionHistoryView.CostCodeID;
                        tranHistory.Source = transactionHistoryView.Source;
                        tranHistory.WithholdingTax = transactionHistoryView.WithholdingTax;
                        tranHistory.CuryBalanceAmt = transactionHistoryView.BalanceAmt;
                        fundGraph.CurrentTransactionHistoryView.Insert(tranHistory);
                    }
                    #endregion

                    #region Replenishments
                    if (transactionHistoryView.TransactionType.Equals(ATPTEFMTransactionHistoryView.transactionType.Replenishment))
                    {
                        var prevRecord = fundGraph.Transactions.Select().RowCast<ATPTEFMTransactionHistoryView>()
                        .TakeWhile(x => x.RefNbr != transactionHistoryView.RefNbr)
                        .LastOrDefault();

                        if (prevRecord != null)
                        {
                            string transactionNbrSorting = string.Empty;

                            switch (transactionHistoryView.Source)
                            {
                                case ATPTEFMTransactionHistoryView.source.FundTransaction:
                                    transactionNbrSorting = $"FT-{prevRecord.RefNbr}";
                                    break;
                                case ATPTEFMTransactionHistoryView.source.ExpenseReceipt:
                                    transactionNbrSorting = prevRecord.FundTransactionSortNbr;
                                    break;
                                case ATPTEFMTransactionHistoryView.source.Replenishment:
                                    transactionNbrSorting = prevRecord.SortNbr;
                                    break;
                                case ATPTEFMTransactionHistoryView.source.MonthEnd:
                                    break;
                                default:
                                    break;
                            }

                            tranHistory.FundRefNbr = fundGraph.Document.Current.FundCD;
                            tranHistory.TransactionType = transactionHistoryView.TransactionType;
                            tranHistory.OrderDate = transactionHistoryView.OrderDate;
                            tranHistory.RefNbr = transactionHistoryView.RefNbr;
                            //tranHistory.SortNbr = transactionHistoryView.SortNbr;
                            tranHistory.SortNbr = $"{transactionNbrSorting}-R{transactionHistoryView.RefNbr}";
                            tranHistory.FundBranchID = transactionHistoryView.FundBranchID;
                            tranHistory.FundType = transactionHistoryView.FundType;
                            tranHistory.TransactionDate = transactionHistoryView.TransactionDate;
                            tranHistory.CuryFundTransactionDocumentAmt = transactionHistoryView.FundTransactionDocumentAmt;
                            tranHistory.Status = transactionHistoryView.Status;
                            tranHistory.Source = transactionHistoryView.Source;
                            tranHistory.HasReplenishemtCheckNbr = transactionHistoryView.HasReplenishmentCheckNbr;
                            tranHistory.CheckNbr = transactionHistoryView.CheckNbr;
                            tranHistory.CuryCheckAmt = transactionHistoryView.CheckAmt;
                            tranHistory.ProjectID = transactionHistoryView.ProjectID;
                            tranHistory.ProjectTaskID = transactionHistoryView.ProjectTaskID;
                            tranHistory.CostCodeID = transactionHistoryView.CostCodeID;
                            tranHistory.CuryBalanceAmt = transactionHistoryView.BalanceAmt;
                            fundGraph.CurrentTransactionHistoryView.Insert(tranHistory);
                        }
                    }
                    #endregion

                    #region CloseFund
                    if (transactionHistoryView.TransactionType.Equals(ATPTEFMTransactionHistoryView.transactionType.CloseFund))
                    {
                        var prevRecord = fundGraph.Transactions.Select().RowCast<ATPTEFMTransactionHistoryView>()
                        .TakeWhile(x => x.RefNbr != transactionHistoryView.RefNbr)
                        .LastOrDefault();

                        if (prevRecord != null)
                        {
                            string transactionNbrSorting = string.Empty;

                            switch (transactionHistoryView.Source)
                            {
                                case ATPTEFMTransactionHistoryView.source.FundTransaction:
                                    transactionNbrSorting = $"FT-{prevRecord.RefNbr}";
                                    break;
                                case ATPTEFMTransactionHistoryView.source.ExpenseReceipt:
                                    transactionNbrSorting = prevRecord.FundTransactionSortNbr;
                                    break;
                                case ATPTEFMTransactionHistoryView.source.Replenishment:
                                    transactionNbrSorting = prevRecord.SortNbr;
                                    break;
                                case ATPTEFMTransactionHistoryView.source.MonthEnd:
                                    break;
                                default:
                                    break;
                            }

                            tranHistory.FundRefNbr = fundGraph.Document.Current.FundCD;
                            tranHistory.TransactionType = transactionHistoryView.TransactionType;
                            tranHistory.OrderDate = transactionHistoryView.OrderDate;
                            tranHistory.RefNbr = transactionHistoryView.RefNbr;
                            tranHistory.SortNbr = $"{transactionNbrSorting}-C{transactionHistoryView.RefNbr}";
                            tranHistory.FundBranchID = transactionHistoryView.FundBranchID;
                            tranHistory.TransactionDate = transactionHistoryView.TransactionDate;
                            tranHistory.CuryFundTransactionDocumentAmt = transactionHistoryView.FundTransactionDocumentAmt;
                            tranHistory.Status = transactionHistoryView.Status;
                            tranHistory.CheckNbr = transactionHistoryView.CheckNbr;
                            tranHistory.CuryCheckAmt = transactionHistoryView.CheckAmt;
                            tranHistory.ProjectID = transactionHistoryView.ProjectID;
                            tranHistory.ProjectTaskID = transactionHistoryView.ProjectTaskID;
                            tranHistory.CostCodeID = transactionHistoryView.CostCodeID;
                            tranHistory.CuryBalanceAmt = transactionHistoryView.BalanceAmt;
                            fundGraph.CurrentTransactionHistoryView.Insert(tranHistory);
                        }
                    }
                    #endregion

                    #region Month End
                    if (transactionHistoryView.TransactionType.Equals(ATPTEFMTransactionHistoryView.transactionType.MonthEnd))
                    {
                        var prevRecord = fundGraph.Transactions.Select().RowCast<ATPTEFMTransactionHistoryView>()
                        .TakeWhile(x => x.RefNbr != transactionHistoryView.RefNbr)
                        .LastOrDefault();

                        if (prevRecord != null)
                        {
                            string transactionNbrSorting = string.Empty;

                            switch (transactionHistoryView.Source)
                            {
                                case ATPTEFMTransactionHistoryView.source.FundTransaction:
                                    transactionNbrSorting = $"FT-{prevRecord.RefNbr}";
                                    break;
                                case ATPTEFMTransactionHistoryView.source.ExpenseReceipt:
                                    transactionNbrSorting = prevRecord.FundTransactionSortNbr;
                                    break;
                                case ATPTEFMTransactionHistoryView.source.Replenishment:
                                    transactionNbrSorting = prevRecord.SortNbr;
                                    break;
                                case ATPTEFMTransactionHistoryView.source.MonthEnd:
                                    break;
                                default:
                                    break;
                            }

                            tranHistory.FundRefNbr = fundGraph.Document.Current.FundCD;
                            tranHistory.TransactionType = transactionHistoryView.TransactionType;
                            tranHistory.OrderDate = transactionHistoryView.OrderDate;
                            tranHistory.RefNbr = transactionHistoryView.RefNbr;
                            tranHistory.SortNbr = $"{transactionNbrSorting}-M{transactionHistoryView.RefNbr}";
                            tranHistory.FundBranchID = transactionHistoryView.FundBranchID;
                            tranHistory.TransactionDate = transactionHistoryView.TransactionDate;
                            tranHistory.CuryFundTransactionDocumentAmt = transactionHistoryView.FundTransactionDocumentAmt;
                            tranHistory.Status = transactionHistoryView.Status;
                            tranHistory.ReversingJournalBatchNbr = transactionHistoryView.ReversingJournalBatchNbr;
                            tranHistory.ProjectID = transactionHistoryView.ProjectID;
                            tranHistory.ProjectTaskID = transactionHistoryView.ProjectTaskID;
                            tranHistory.CostCodeID = transactionHistoryView.CostCodeID;
                            tranHistory.CuryBalanceAmt = transactionHistoryView.BalanceAmt;
                            fundGraph.CurrentTransactionHistoryView.Insert(tranHistory);
                        }
                    }
                    #endregion

                }

                var balanceSummary = fundGraph.BalanceSummary.Select().RowCast<ATPTEFMFundBalanceView>().ToList();

                // Get Employee Currency settings
                var employee = PXSelect<
                    EPEmployee,
                    Where<EPEmployee.bAccountID, Equal<Required<EPEmployee.bAccountID>>>>
                    .Select(this, fundGraph.Document.Current.CustodianID)
                    .RowCast<EPEmployee>()
                    .FirstOrDefault();

                if (employee != null)
                {
                    CurrencyInfo currencyInfo = PXSelect<
                        CurrencyInfo,
                        Where<CurrencyInfo.baseCuryID, Equal<Required<CurrencyInfo.baseCuryID>>>>
                        .Select(this, employee.BaseCuryID);
                    var restrictionAttr = fundGraph.Document.Current.FundTransactionRestriction;

                    fundGraph.Document.Current.CuryID = employee.CuryID;
                    fundGraph.Document.Current.CuryInfoID = currencyInfo.CuryInfoID;
                    fundGraph.Document.Current.CuryInitialFund = fundGraph.Document.Current.InitialFund;
                    fundGraph.Document.Current.CuryFundAmt = (fundGraph.Document.Current.CuryFundAmt != null && fundGraph.Document.Current.CuryFundAmt != fundGraph.Document.Current.CuryInitialFund) ? fundGraph.Document.Current.CuryFundAmt : balanceSummary?.LastOrDefault()?.FundAmt;
                    fundGraph.Document.Current.CuryBalanceAmt = balanceSummary?.LastOrDefault()?.BalanceAmt;
                    fundGraph.Document.Current.CuryOnReplenishmentAmt = balanceSummary?.LastOrDefault()?.OnReplenishmentAmt;
                    fundGraph.Document.Current.CuryLiquidatedAmt = balanceSummary?.LastOrDefault()?.LiquidatedAmt;
                    fundGraph.Document.Current.CuryUnliquidatedAmt = balanceSummary?.LastOrDefault()?.UnliquidatedAmt;
                    fundGraph.Document.Current.FundTransactionRestriction = (string.IsNullOrEmpty(fundGraph.Document.Current.FundTransactionRestriction)) ? ATPTEFMReplenishmentStringListAttribute.WarningValue : restrictionAttr;
                    fundGraph.Document.Current.IsOldFund = isOldfund;
                    fundGraph.Document.UpdateCurrent();
                    fundGraph.Save.Press();
                }
            }
            #endregion

        }
        public void CloseCAWithBalanceProcess()
        {
            ATPTEFMCashAdvanceEntry graph = PXGraph.CreateInstance<ATPTEFMCashAdvanceEntry>();

            string[] refNbrList = DataFixDocument.Current.CloseCAWithBalance.Split(';');

            foreach (var refNbr in refNbrList)
            {
                ATPTEFMCashAdvance ca = PXSelect<
                    ATPTEFMCashAdvance,
                    Where<ATPTEFMCashAdvance.cashAdvanceNbr, Equal<Required<ATPTEFMCashAdvance.cashAdvanceNbr>>>>
                    .Select(this, refNbr.Trim());

                if (ca != null)
                {
                    graph.Clear();
                    graph.CashAdvances.Current = ca;

                    ca.VendorRefundType = null;
                    ca.VendorRefundRefNbr = null;
                    ca.CuryChangeAmount += ca.RefundAmount;
                    ca.ChangeAmount += ca.RefundAmount;
                    ca.RefundAmount = 0m;

                    if (ca.Status != ATPTEFMCashAdvanceStatusAttribute.PendingLiquidationValue)
                        ca.Status = ATPTEFMCashAdvanceStatusAttribute.PendingLiquidationValue;


                    graph.CashAdvances.Update(ca);
                    graph.Save.Press();
                }
            }
        }
        private void FundTransactionHistoryBalanceAndSortingFixer()
        {
            ATPTEFMFundMaint fundGraph = PXGraph.CreateInstance<ATPTEFMFundMaint>();
            string fundCD = DataFixDocument.Current.FundHistoryBalanceAndSortingFixer.Trim();

            ATPTEFMFund fund = PXSelect<
                ATPTEFMFund,
                Where<ATPTEFMFund.fundCD, Equal<Required<ATPTEFMFund.fundCD>>>>
                .Select(this, fundCD);

            if (fund == null) return;

            fundGraph.Document.Current = fund;
            #region Delete Transaction History and Fix Sorting
            PXResultset<ATPTEFMFundTransactionHistoryView> DeletehistoryRecords = PXSelect<
                                                                                    ATPTEFMFundTransactionHistoryView,
                                                                                    Where<ATPTEFMFundTransactionHistoryView.fundRefNbr, Equal<Required<ATPTEFMFundTransactionHistoryView.fundRefNbr>>,
                                                                                        And<Where<ATPTEFMFundTransactionHistoryView.transactionType,
            NotEqual<ATPTEFMFundTransactionTypeAttribute.increaseFundValue>,
                                                                                            And<ATPTEFMFundTransactionHistoryView.transactionType,
                NotEqual<ATPTEFMFundTransactionTypeAttribute.decreaseFundValue>>>>>>
                                                                                    .Select(this, fundCD);

            foreach (ATPTEFMFundTransactionHistoryView record in DeletehistoryRecords)
            {
                fundGraph.CurrentTransactionHistoryView.Delete(record);
            }
            fundGraph.Actions.PressSave();

            FundUnboundToBound(fundCD, false);

            #region Check Establishment if there is reversed bill
            DeletehistoryRecords = PXSelect<
                                                                                    ATPTEFMFundTransactionHistoryView,
                                                                                    Where<ATPTEFMFundTransactionHistoryView.fundRefNbr, Equal<Required<ATPTEFMFundTransactionHistoryView.fundRefNbr>>,
                                                                                        And<ATPTEFMFundTransactionHistoryView.transactionType, Equal<ATPTEFMFundTransactionTypeAttribute.establishment>>>>
                                                                                    .Select(this, fundCD);

            if (DeletehistoryRecords != null && DeletehistoryRecords.Count() > 1)
            {
                foreach (ATPTEFMFundTransactionHistoryView record in DeletehistoryRecords)
                {
                    APAdjust apAdjust = PXSelect<
                        APAdjust,
                        Where<APAdjust.adjdRefNbr, Equal<Required<APAdjust.adjdRefNbr>>,
                            And<APAdjust.voided, NotEqual<True>,
                            And<APAdjust.adjgDocType, Equal<APPaymentType.debitAdj>>>>>
                        .Select(this, record.RefNbr);

                    if (apAdjust != null)
                    {
                        record.Status = ATPTEFMFundStatusAttribute.ReversedValue;
                        fundGraph.CurrentTransactionHistoryView.Update(record);
                    }
                }

                fundGraph.Actions.PressSave();
            }
            #endregion

            #endregion

            decimal? balance = fund.CuryInitialFund ?? 0m;

            PXResultset<ATPTEFMFundTransactionHistoryView> historyRecords = PXSelect<
                ATPTEFMFundTransactionHistoryView,
                Where<ATPTEFMFundTransactionHistoryView.fundRefNbr,
            Equal<Required<ATPTEFMFundTransactionHistoryView.fundRefNbr>>>,
                OrderBy<
                    Asc<ATPTEFMFundTransactionHistoryView.sortNbr>>>
                .Select(this, fundCD);

            if (historyRecords != null && historyRecords.Count > 0)
            {
                foreach (ATPTEFMFundTransactionHistoryView record in historyRecords)
                {
                    if (record.TransactionType.Equals(ATPTEFMTransactionHistoryView.transactionType.Establishment))
                    {
                        record.CuryFundTransactionDocumentAmt = fund.CuryInitialFund;
                        record.CuryBalanceAmt = balance;
                    }

                    if (record.TransactionType.Equals(ATPTEFMTransactionHistoryView.transactionType.ExpenseReceipt))
                    {
                        EPExpenseClaimDetails claimDetail = EPExpenseClaimDetails.PK.Find(this, record.RefNbr);

                        if (claimDetail != null)
                        {
                            ATPTEFMEPExpenseClaimDetailsExt claimDetailExt = claimDetail.GetExtension<ATPTEFMEPExpenseClaimDetailsExt>();

                            if (claimDetailExt != null)
                            {
                                var fundTran = GetFundTransaction(claimDetailExt.UsrATPTEFMRequestRefNbr);

                                if (fundTran != null)
                                {
                                    record.Status = claimDetail.Status;

                                    if (claimDetail.Status == ATPTEFMExpenseReceiptStatusAttribute.OpenValue)
                                        record.Status = ATPTEFMFundStatusAttribute.OpenValue;

                                    if (fundTran.Status == ATPTEFMFundStatusAttribute.ClosedValue)
                                        record.Status = ATPTEFMFundStatusAttribute.LiquidatedValue;


                                    record.CuryFundTransactionDocumentAmt = claimDetail.CuryExtCost;
                                    record.CuryBalanceAmt = balance;
                                }
                            }
                        }
                    }

                    if (record.TransactionType.Equals(ATPTEFMTransactionHistoryView.transactionType.FundRequest))
                    {
                        var fundTran = GetFundTransaction(record.RefNbr);

                        if (fundTran != null)
                        {
                            if (fundTran.Status == ATPTEFMFundStatusAttribute.ClosedValue)
                            {
                                decimal? totalFundTranWht = GetTotalWithholdingTaxFromReceipts(fundTran.RefNbr);
                                balance -= fundTran.ActualSpentAmount;
                                balance += totalFundTranWht;
                            }

                            if (fundTran.CashAdvanceStatus == ATPTEFMFundTransactionCashAdvanceStatusAttribute.UnliquidatedValue)
                            {
                                balance -= fundTran.RequestedAmount;
                            }

                            record.CuryFundTransactionDocumentAmt = fundTran.RequestedAmount;
                            record.CuryBalanceAmt = balance;
                        }
                    }

                    if (record.TransactionType.Equals(ATPTEFMTransactionHistoryView.transactionType.FundReimbursment))
                    {
                        var fundTran = GetFundTransaction(record.RefNbr);

                        if (fundTran != null && fundTran.Status == ATPTEFMFundStatusAttribute.ClosedValue)
                        {
                            decimal? totalFundTranWht = GetTotalWithholdingTaxFromReceipts(fundTran.RefNbr);

                            balance -= fundTran.ActualSpentAmount ?? 0m;
                            balance += totalFundTranWht;
                        }
                        record.CuryBalanceAmt = balance;
                        record.Status = fundTran.Status;
                    }

                    if (record.TransactionType.Equals(ATPTEFMTransactionHistoryView.transactionType.Replenishment))
                    {

                        ATPTEFMReplenishment replenishment = ATPTEFMReplenishment.PK.Find(this, record.RefNbr);
                        string transactionNbrSorting = string.Empty;

                        if (replenishment != null)
                        {
                            var prevRecord = fundGraph.Transactions.Select().RowCast<ATPTEFMTransactionHistoryView>()
                            .TakeWhile(x => x.RefNbr != record.RefNbr)
                            .LastOrDefault();

                            if (prevRecord != null)
                            {
                                EPExpenseClaimDetails claimDetail = EPExpenseClaimDetails.PK.Find(this, prevRecord.RefNbr);

                                if (claimDetail != null)
                                {
                                    ATPTEFMEPExpenseClaimDetailsExt claimDetailExt = claimDetail.GetExtension<ATPTEFMEPExpenseClaimDetailsExt>();

                                    if (claimDetailExt != null)
                                    {
                                        //TODO: NEED TO CONSIDER FT REQUEST TYPE
                                        ATPTEFMFundTransactionHistoryView hist = PXSelect<
                                            ATPTEFMFundTransactionHistoryView,
                                            Where<ATPTEFMFundTransactionHistoryView.refNbr, Equal<Required<ATPTEFMFundTransactionHistoryView.refNbr>>,
                                                And<ATPTEFMFundTransactionHistoryView.transactionType, Equal<ATPTEFMTransactionHistoryView.transactionType.fundReimbursment>>>>
                                            .Select(this, claimDetailExt.UsrATPTEFMRequestRefNbr);

                                        if (hist != null)
                                        {
                                            record.SortNbr = $"{hist.SortNbr}-R{replenishment.ReplenishmentNbr}";
                                        }
                                    }
                                }
                            }

                            PXResultset<APInvoice> invoices = PXSelectJoin<
                                APInvoice,
                                LeftJoin<APAdjust,
                                    On<APAdjust.adjdDocType, Equal<APInvoice.docType>,
                                    And<APAdjust.adjdRefNbr, Equal<APInvoice.refNbr>,
                                    And<APAdjust.released, Equal<True>,
                                    And<APAdjust.voided, Equal<False>,
                                    And<APAdjust.adjgDocType, Equal<APDocType.debitAdj>>>>>>>,
                                Where<
                                APInvoice.origRefNbr, Equal<Required<APInvoice.origRefNbr>>,
                                    And<APInvoice.docType, Equal<APDocType.invoice>,
                                    And<APAdjust.adjgRefNbr, IsNull>>>>
                                .Select(this, record.RefNbr);

                            if (invoices != null && invoices.Count > 0)
                            {
                                foreach (PXResult<APInvoice, APAdjust> result in invoices)
                                {
                                    APInvoice invoice = (APInvoice)result;

                                    if (invoice.Status.Equals(APDocStatus.Closed))
                                    {
                                        balance += invoice.CuryLineTotal;
                                        balance -= invoice.CuryOrigWhTaxAmt;
                                    }

                                }
                            }

                            record.CuryBalanceAmt = balance;
                            record.Status = replenishment.Status;
                        }

                    }

                    fundGraph.CurrentTransactionHistoryView.Update(record);
                }


                #region Get All FT Liquidated Amounts
                //TODO: FUND REQUEST
                decimal? liquidatedBalance = decimal.Zero;
                decimal? totalWhtAmt = decimal.Zero;

                #region Request Type Liquidated Amounts
                liquidatedBalance += GetRequestTypeLiqAmounts(fund.FundCD);
                #endregion

                #region Reimbursement
                liquidatedBalance += GetReimbursementLiqAmounts(fund.FundCD);
                #endregion

                #endregion

                #region Get All On Replenishment Amount
                decimal? onReplenishAmt = decimal.Zero;

                PXResultset<ATPTEFMReplenishment> replenishments = PXSelect<
                    ATPTEFMReplenishment,
                    Where<ATPTEFMReplenishment.fundID, Equal<Required<ATPTEFMReplenishment.fundID>>,
                        And<ATPTEFMReplenishment.status,
                        In3<
                            ATPTEFMReplenishmentStatusAttribute.openValue,
                            ATPTEFMReplenishmentStatusAttribute.holdValue,
                            ATPTEFMReplenishmentStatusAttribute.pendingValue>>>>
                    .Select(this, fund.FundCD);

                foreach (ATPTEFMReplenishment replenishment in replenishments)
                {
                    onReplenishAmt += replenishment.ClaimAmount;
                }


                var replenishmentNbrs = PXSelect<
                    ATPTEFMReplenishment,
                    Where<ATPTEFMReplenishment.fundID, Equal<Required<ATPTEFMReplenishment.fundID>>,
                        And<ATPTEFMReplenishment.status, Equal<ATPTEFMReplenishmentStatusAttribute.closedValue>>>>
                    .Select(this, fund.FundCD)
                    .RowCast<ATPTEFMReplenishment>()
                    .Select(r => r.ReplenishmentNbr)
                    .ToList();

                if (replenishmentNbrs.Any())
                {
                    // Get all invoices in one query
                    var invoiceTotal = PXSelectJoin<
                        APInvoice,
                        LeftJoin<APAdjust,
                            On<APAdjust.adjdDocType, Equal<APInvoice.docType>,
                            And<APAdjust.adjdRefNbr, Equal<APInvoice.refNbr>,
                            And<APAdjust.released, Equal<True>,
                            And<APAdjust.voided, Equal<False>,
                            And<APAdjust.adjgDocType, Equal<APDocType.debitAdj>>>>>>>,
                        Where<
                            APInvoice.origRefNbr, In<Required<APInvoice.origRefNbr>>,
                            And<APInvoice.docType, Equal<APDocType.invoice>,
                            And<APAdjust.adjgRefNbr, IsNull,
                            And<APInvoice.status, NotIn3<
                                APDocStatus.closed,
                                APDocStatus.voided,
                                APDocStatus.rejected>>>>>>
                        .Select(this, replenishmentNbrs)
                        .RowCast<APInvoice>()
                        .Sum(invoice => invoice.CuryLineTotal ?? 0m);

                    onReplenishAmt += invoiceTotal;
                }
                #endregion

                #region Update Fund Summary
                fund.CuryBalanceAmt = balance;
                fund.CuryOnReplenishmentAmt = onReplenishAmt;
                fund.CuryLiquidatedAmt = liquidatedBalance - totalWhtAmt;
                fundGraph.Document.Update(fund);
                #endregion

                fundGraph.Actions.PressSave();
            }
        }
        private void FundTransactionHistoryBalanceFixer(string fundCD)
        {
            ATPTEFMFundMaint fundGraph = PXGraph.CreateInstance<ATPTEFMFundMaint>();

            ATPTEFMFund fund = PXSelect<
                ATPTEFMFund,
                Where<ATPTEFMFund.fundCD, Equal<Required<ATPTEFMFund.fundCD>>>>
                .Select(this, fundCD);

            if (fund == null) return;

            fundGraph.Document.Current = fund;

            decimal? balance = fund.CuryInitialFund ?? 0m;

            PXResultset<ATPTEFMFundTransactionHistoryView> historyRecords = PXSelect<
                ATPTEFMFundTransactionHistoryView,
                Where<ATPTEFMFundTransactionHistoryView.fundRefNbr,
            Equal<Required<ATPTEFMFundTransactionHistoryView.fundRefNbr>>>,
                OrderBy<
                    Asc<ATPTEFMFundTransactionHistoryView.sortNbr>>>
                .Select(this, fundCD);

            if (historyRecords != null && historyRecords.Count > 0)
            {
                foreach (ATPTEFMFundTransactionHistoryView record in historyRecords)
                {
                    if (record.TransactionType.Equals(ATPTEFMTransactionHistoryView.transactionType.Establishment))
                    {
                        record.CuryFundTransactionDocumentAmt = fund.CuryInitialFund;
                        record.CuryBalanceAmt = balance;
                    }

                    if (record.TransactionType.Equals(ATPTEFMTransactionHistoryView.transactionType.ExpenseReceipt))
                    {
                        EPExpenseClaimDetails claimDetail = EPExpenseClaimDetails.PK.Find(this, record.RefNbr);

                        if (claimDetail != null)
                        {
                            ATPTEFMEPExpenseClaimDetailsExt claimDetailExt = claimDetail.GetExtension<ATPTEFMEPExpenseClaimDetailsExt>();

                            if (claimDetailExt != null)
                            {
                                var fundTran = GetFundTransaction(claimDetailExt.UsrATPTEFMRequestRefNbr);

                                if (fundTran != null)
                                {
                                    record.Status = claimDetail.Status;

                                    if (claimDetail.Status == ATPTEFMExpenseReceiptStatusAttribute.OpenValue)
                                        record.Status = ATPTEFMFundStatusAttribute.OpenValue;

                                    if (fundTran.Status == ATPTEFMFundStatusAttribute.ClosedValue)
                                        record.Status = ATPTEFMFundStatusAttribute.LiquidatedValue;


                                    record.CuryFundTransactionDocumentAmt = claimDetail.CuryExtCost;
                                    record.CuryBalanceAmt = balance;
                                }
                            }
                        }
                    }

                    if (record.TransactionType.Equals(ATPTEFMTransactionHistoryView.transactionType.FundRequest))
                    {
                        var fundTran = GetFundTransaction(record.RefNbr);

                        if (fundTran != null)
                        {
                            if (fundTran.Status == ATPTEFMFundStatusAttribute.ClosedValue)
                            {
                                decimal? totalFundTranWht = GetTotalWithholdingTaxFromReceipts(fundTran.RefNbr);
                                balance -= fundTran.ActualSpentAmount;
                                balance += totalFundTranWht;
                            }

                            if (fundTran.CashAdvanceStatus == ATPTEFMFundTransactionCashAdvanceStatusAttribute.UnliquidatedValue)
                            {
                                balance -= fundTran.RequestedAmount;
                            }

                            record.CuryFundTransactionDocumentAmt = fundTran.RequestedAmount;
                            record.CuryBalanceAmt = balance;
                        }
                    }

                    if (record.TransactionType.Equals(ATPTEFMTransactionHistoryView.transactionType.FundReimbursment))
                    {
                        var fundTran = GetFundTransaction(record.RefNbr);

                        if (fundTran != null && fundTran.Status == ATPTEFMFundStatusAttribute.ClosedValue)
                        {
                            decimal? totalFundTranWht = GetTotalWithholdingTaxFromReceipts(fundTran.RefNbr);

                            balance -= fundTran.ActualSpentAmount ?? 0m;
                            balance += totalFundTranWht;
                        }
                        record.CuryBalanceAmt = balance;
                        record.Status = fundTran.Status;
                    }

                    if (record.TransactionType.Equals(ATPTEFMTransactionHistoryView.transactionType.Replenishment))
                    {

                        ATPTEFMReplenishment replenishment = ATPTEFMReplenishment.PK.Find(this, record.RefNbr);
                        string transactionNbrSorting = string.Empty;

                        if (replenishment != null)
                        {
                            var addedAmt = balance += record.CuryCheckAmt;
                            record.CuryBalanceAmt = addedAmt;
                            record.Status = replenishment.Status;
                        }
                    }

                    if (record.TransactionType.Equals(ATPTEFMTransactionHistoryView.transactionType.IncreaseFund))
                    {
                        PXResultset<APInvoice, APAdjust> ds = PXSelectJoin<
                            APInvoice,
                            InnerJoin<APAdjust,
                                On<APAdjust.adjdDocType, Equal<APInvoice.docType>,
                                And<APAdjust.adjdRefNbr, Equal<APInvoice.refNbr>,
                                And<APAdjust.released, Equal<True>,
                                And<APAdjust.voided, Equal<False>,
                                And<APAdjust.adjdDocType, Equal<APDocType.creditAdj>>>>>>>,
                            Where<
                                APInvoice.refNbr, Equal<Required<APInvoice.refNbr>>,
                                And<APInvoice.docType, Equal<APDocType.creditAdj>>>>
                            .Select<PXResultset<APInvoice, APAdjust>>(this, record.RefNbr);

                        APInvoice inv = ds;
                        APAdjust adj = ds;

                        if (inv != null && inv.Status.Equals(APDocStatus.Closed))
                        {
                            balance += inv.CuryOrigDocAmt;

                            if (!record.Status.Equals(ATPTEFMFundStatusAttribute.ClosedValue))
                            {

                                record.Status = ATPTEFMFundStatusAttribute.ClosedValue;
                                record.CuryCheckAmt = adj.CuryAdjgAmt;
                                record.CheckNbr = adj.AdjgRefNbr;
                            }
                            record.CuryBalanceAmt = balance;
                        }
                    }
                    fundGraph.CurrentTransactionHistoryView.Update(record);
                }


                #region Get All FT Liquidated Amounts
                //TODO: FUND REQUEST
                decimal? liquidatedBalance = decimal.Zero;
                decimal? totalWhtAmt = decimal.Zero;

                #region Request Type Liquidated Amounts
                liquidatedBalance += GetRequestTypeLiqAmounts(fund.FundCD);
                #endregion

                #region Reimbursement
                liquidatedBalance += GetReimbursementLiqAmounts(fund.FundCD);
                #endregion

                #endregion

                #region Get All On Replenishment Amount
                decimal? onReplenishAmt = decimal.Zero;

                PXResultset<ATPTEFMReplenishment> replenishments = PXSelect<
                    ATPTEFMReplenishment,
                    Where<ATPTEFMReplenishment.fundID, Equal<Required<ATPTEFMReplenishment.fundID>>,
                        And<ATPTEFMReplenishment.status,
                        In3<
                            ATPTEFMReplenishmentStatusAttribute.openValue,
                            ATPTEFMReplenishmentStatusAttribute.holdValue,
                            ATPTEFMReplenishmentStatusAttribute.pendingValue>>>>
                    .Select(this, fund.FundCD);

                foreach (ATPTEFMReplenishment replenishment in replenishments)
                {
                    onReplenishAmt += replenishment.ClaimAmount;
                }


                var replenishmentNbrs = PXSelect<
                    ATPTEFMReplenishment,
                    Where<ATPTEFMReplenishment.fundID, Equal<Required<ATPTEFMReplenishment.fundID>>,
                        And<ATPTEFMReplenishment.status, Equal<ATPTEFMReplenishmentStatusAttribute.closedValue>>>>
                    .Select(this, fund.FundCD)
                    .RowCast<ATPTEFMReplenishment>()
                    .Select(r => r.ReplenishmentNbr)
                    .ToList();

                if (replenishmentNbrs.Any())
                {
                    // Get all invoices in one query
                    var invoiceTotal = PXSelectJoin<
                        APInvoice,
                        LeftJoin<APAdjust,
                            On<APAdjust.adjdDocType, Equal<APInvoice.docType>,
                            And<APAdjust.adjdRefNbr, Equal<APInvoice.refNbr>,
                            And<APAdjust.released, Equal<True>,
                            And<APAdjust.voided, Equal<False>,
                            And<APAdjust.adjgDocType, Equal<APDocType.debitAdj>>>>>>>,
                        Where<
                            APInvoice.origRefNbr, In<Required<APInvoice.origRefNbr>>,
                            And<APInvoice.docType, Equal<APDocType.invoice>,
                            And<APAdjust.adjgRefNbr, IsNull,
                            And<APInvoice.status, NotIn3<
                                APDocStatus.closed,
                                APDocStatus.voided,
                                APDocStatus.rejected>>>>>>
                        .Select(this, replenishmentNbrs)
                        .RowCast<APInvoice>()
                        .Sum(invoice => invoice.CuryLineTotal ?? 0m);

                    onReplenishAmt += invoiceTotal;
                }
                #endregion

                #region Update Fund Summary
                fund.CuryBalanceAmt = balance;
                fund.CuryOnReplenishmentAmt = onReplenishAmt;
                fund.CuryLiquidatedAmt = liquidatedBalance - totalWhtAmt;
                fundGraph.Document.Update(fund);
                #endregion

                fundGraph.Actions.PressSave();
            }
        }
        private decimal? GetRequestTypeLiqAmounts(string fundCD)
        {
            decimal? liquidatedAmt = decimal.Zero;
            decimal? totalWhtAmt = decimal.Zero;

            var receiptDetails = PXSelectJoin<
                ATPTEFMFundTransactionReceiptDetail,
                InnerJoin<ATPTEFMFundTransaction,
                    On<ATPTEFMFundTransaction.refNbr, Equal<ATPTEFMFundTransactionReceiptDetail.fundTransactionRefNbr>>>,
                Where<ATPTEFMFundTransaction.fundID, Equal<Required<ATPTEFMFundTransaction.fundID>>,
                    And<ATPTEFMFundTransaction.fundTransactionType,
                Equal<ATPTEFMFundTransactionTypeAttribute.cashAdvanceValue>,
                    And<Where<ATPTEFMFundTransaction.cashAdvanceStatus,
                Equal<ATPTEFMFundTransactionStepAttribute.liquidatedValue>,
                        Or<ATPTEFMFundTransaction.status,
                Equal<ATPTEFMFundStatusAttribute.closedValue>>>>>>>
                .Select(this, fundCD)
                .RowCast<ATPTEFMFundTransactionReceiptDetail>()
                .ToList();

            var existingReceipts = new HashSet<string>(
                    PXSelect<ATPTEFMReplenishmentDetail>
                    .Select(this)
                    .RowCast<ATPTEFMReplenishmentDetail>()
                    .Select(rd => rd.ExpenseReceiptNbr));

            var filteredReceipts = receiptDetails
                .Where(rd => !existingReceipts.Contains(rd.ExpenseReceiptRefNbr))
                .ToList();

            foreach (var receipt in filteredReceipts)
            {
                var expenseDetail = EPExpenseClaimDetails.PK.Find(this, receipt.ExpenseReceiptRefNbr);
                liquidatedAmt += expenseDetail.CuryExtCost;
                totalWhtAmt += GetWithholdingTaxAmount(expenseDetail.ClaimDetailID);
            }

            return liquidatedAmt - totalWhtAmt;
        }
        private decimal? GetReimbursementLiqAmounts(string fundCD)
        {
            decimal? liquidatedAmt = decimal.Zero;
            decimal? totalWhtAmt = decimal.Zero;

            var receiptDetails = PXSelectJoin<
                ATPTEFMFundTransactionReceiptDetail,
                InnerJoin<ATPTEFMFundTransaction,
                    On<ATPTEFMFundTransaction.refNbr, Equal<ATPTEFMFundTransactionReceiptDetail.fundTransactionRefNbr>>>,
                Where<ATPTEFMFundTransaction.fundID, Equal<Required<ATPTEFMFundTransaction.fundID>>,
                    And<ATPTEFMFundTransaction.fundTransactionType,
                Equal<ATPTEFMFundTransactionTypeAttribute.reimbursementValue>,
                    And<Where<ATPTEFMFundTransaction.step,
                Equal<ATPTEFMFundTransactionStepAttribute.submitReceiptValue>,
                        Or<ATPTEFMFundTransaction.status,
                Equal<ATPTEFMFundStatusAttribute.closedValue>>>>>>>
                .Select(this, fundCD)
                .RowCast<ATPTEFMFundTransactionReceiptDetail>()
                .ToList();

            var existingReceipts = new HashSet<string>(
                    PXSelect<ATPTEFMReplenishmentDetail>
                    .Select(this)
                    .RowCast<ATPTEFMReplenishmentDetail>()
                    .Select(rd => rd.ExpenseReceiptNbr));

            var filteredReceipts = receiptDetails
                .Where(rd => !existingReceipts.Contains(rd.ExpenseReceiptRefNbr))
                .ToList();

            foreach (var receipt in filteredReceipts)
            {
                var expenseDetail = EPExpenseClaimDetails.PK.Find(this, receipt.ExpenseReceiptRefNbr);
                liquidatedAmt += expenseDetail.CuryExtCost;
                totalWhtAmt += GetWithholdingTaxAmount(expenseDetail.ClaimDetailID);
            }

            return liquidatedAmt - totalWhtAmt;
        }
        private decimal? GetTotalWithholdingTaxFromReceipts(string fundTransactionRefNbr)
        {
            if (string.IsNullOrEmpty(fundTransactionRefNbr)) return 0m;

            decimal? totalWhtAmount = decimal.Zero;

            var reimbursementReceiptDetails = PXSelectJoin<
                ATPTEFMFundTransactionReceiptDetail,
                InnerJoin<EPExpenseClaimDetails,
                    On<ATPTEFMFundTransactionReceiptDetail.expenseReceiptRefNbr,
                    Equal<EPExpenseClaimDetails.claimDetailCD>>>,
                Where<
                    ATPTEFMFundTransactionReceiptDetail.fundTransactionRefNbr,
                    Equal<Required<ATPTEFMFundTransactionReceiptDetail.fundTransactionRefNbr>>,
                    And<Where<
                        ATPTEFMFundTransactionReceiptDetail.expenseReceiptRefNbr, IsNotNull,
                        And<ATPTEFMFundTransactionReceiptDetail.expenseReceiptRefNbr,
                        NotEqual<StringEmpty>>>>>>
                .Select(this, fundTransactionRefNbr);

            foreach (PXResult<ATPTEFMFundTransactionReceiptDetail, EPExpenseClaimDetails> result in reimbursementReceiptDetails)
            {
                EPExpenseClaimDetails expenseDetail = result;
                totalWhtAmount += GetWithholdingTaxAmount(expenseDetail.ClaimDetailID);
            }

            return totalWhtAmount;
        }
        private decimal GetWithholdingTaxAmount(int? claimDetailID)
        {
            return PXSelectJoin<
                EPTaxTran,
                InnerJoin<Tax,
                    On<Tax.taxID, Equal<EPTaxTran.taxID>>>,
                Where<Tax.taxType, Equal<CSTaxType.withholding>,
                    And<EPTaxTran.claimDetailID, Equal<Required<EPTaxTran.claimDetailID>>>>>
                .Select(this, claimDetailID)
                .RowCast<EPTaxTran>()
                .Sum(tax => tax.CuryTaxAmt ?? 0m);
        }
        private ATPTEFMFundTransaction GetFundTransaction(string refNbr)
        {
            if (string.IsNullOrEmpty(refNbr)) return null;

            return PXSelect<
                ATPTEFMFundTransaction,
                Where<ATPTEFMFundTransaction.refNbr,
                    Equal<Required<ATPTEFMFundTransaction.refNbr>>>>
                .Select(this, refNbr)
                .FirstOrDefault();
        }
        private void UpdateTransactionHistoryCuryFields()
        {
            var fund = PXSelect<
                ATPTEFMFund,
                Where<ATPTEFMFund.fundCD,
            Equal<Required<ATPTEFMFund.fundCD>>>>
                .Select(this, DataFixDocument.Current.FundCDCurrency)
                .RowCast<ATPTEFMFund>()
                .FirstOrDefault();

            if (fund != null)
            {
                // Get Employee Currency settings
                var employee = PXSelect<
                    EPEmployee,
                    Where<EPEmployee.bAccountID, Equal<Required<EPEmployee.bAccountID>>>>
                    .Select(this, fund.CustodianID)
                    .RowCast<EPEmployee>()
                    .FirstOrDefault();

                if (employee != null)
                {
                    CurrencyInfo currencyInfo = PXSelect<
                        CurrencyInfo,
                        Where<CurrencyInfo.baseCuryID, Equal<Required<CurrencyInfo.baseCuryID>>>>
                        .Select(this, employee.BaseCuryID);

                    if (currencyInfo != null)
                    {
                        // Update Fund CuryID, CuryInfoID and all Cury fields
                        PXDatabase.Update<ATPTEFMFund>(
                            new PXDataFieldAssign<ATPTEFMFund.curyID>(employee.CuryID),
                            new PXDataFieldAssign<ATPTEFMFund.curyInfoID>(currencyInfo.CuryInfoID),
                            new PXDataFieldAssign<ATPTEFMFund.curyInitialFund>(fund.InitialFund),
                            new PXDataFieldAssign<ATPTEFMFund.curyFundAmt>(fund.FundAmt),
                            new PXDataFieldAssign<ATPTEFMFund.curyBalanceAmt>(fund.BalanceAmt),
                            new PXDataFieldAssign<ATPTEFMFund.curyOnReplenishmentAmt>(fund.OnReplenishmentAmt),
                            new PXDataFieldAssign<ATPTEFMFund.curyLiquidatedAmt>(fund.LiquidatedAmt),
                            new PXDataFieldAssign<ATPTEFMFund.curyUnliquidatedAmt>(fund.UnliquidatedAmt),
                            new PXDataFieldRestrict<ATPTEFMFund.fundCD>(fund.FundCD)
                        );
                    }
                }

                foreach (ATPTEFMFundTransactionHistoryView history in PXSelect<
                    ATPTEFMFundTransactionHistoryView,
                    Where<ATPTEFMFundTransactionHistoryView.fundRefNbr,
     Equal<Required<ATPTEFMFundTransactionHistoryView.fundRefNbr>>>>
                    .Select(this, DataFixDocument.Current.FundCDCurrency))
                {
                    PXDatabase.Update<ATPTEFMFundTransactionHistoryView>(
                        new PXDataFieldAssign<ATPTEFMFundTransactionHistoryView.curyFundTransactionDocumentAmt>(history.FundTransactionDocumentAmt),
                        new PXDataFieldAssign<ATPTEFMFundTransactionHistoryView.curyWithholdingTax>(history.WithholdingTax),
                        new PXDataFieldAssign<ATPTEFMFundTransactionHistoryView.curyUnliquidatedAmt>(history.UnliquidatedAmt),
                        new PXDataFieldAssign<ATPTEFMFundTransactionHistoryView.curyLiquidatedAmt>(history.LiquidatedAmt),
                        new PXDataFieldAssign<ATPTEFMFundTransactionHistoryView.curyFundReturnAmt>(history.FundReturnAmt),
                        new PXDataFieldAssign<ATPTEFMFundTransactionHistoryView.curyBalanceAmt>(history.BalanceAmt),
                        new PXDataFieldAssign<ATPTEFMFundTransactionHistoryView.curyCheckAmt>(history.CheckAmt),
                        new PXDataFieldAssign<ATPTEFMFundTransactionHistoryView.curyDocumentBalanceAmt>(history.DocumentBalanceAmt),
                        new PXDataFieldAssign<ATPTEFMFundTransactionHistoryView.curyFundAmt>(history.FundAmt),
                        new PXDataFieldRestrict<ATPTEFMFundTransactionHistoryView.fundTransactionHistoryID>(history.FundTransactionHistoryID)
                    );
                }
            }


        }
        private void ProcessCAReceiptAlreadyCancelledButExistInCAReceiptsTab()
        {
            PXResultset<EPExpenseClaimDetails> results = PXSelectJoin<
                EPExpenseClaimDetails,
                InnerJoin<ATPTEFMCAReceiptDetail,
                    On<ATPTEFMCAReceiptDetail.expenseReceiptRefNbr, Equal<EPExpenseClaimDetails.claimDetailCD>>>,
                Where<EPExpenseClaimDetails.status, Equal<ATPTEFMExpenseReceiptStatusAttribute.cancelledValue>>>
                .Select(this);

            foreach (PXResult<EPExpenseClaimDetails, ATPTEFMCAReceiptDetail> result in results)
            {
                EPExpenseClaimDetails expenseDetail = (EPExpenseClaimDetails)result;
                ATPTEFMCAReceiptDetail caReceipt = (ATPTEFMCAReceiptDetail)result;

                PXDatabase.Update<ATPTEFMCAReceiptDetail>(
                  new PXDataFieldAssign<ATPTEFMCAReceiptDetail.expenseReceiptRefNbr>(null),
                  new PXDataFieldRestrict<ATPTEFMCAReceiptDetail.cashAdvanceReceiptDetailIID>(caReceipt.CashAdvanceReceiptDetailIID)
              );
            }
        }
        private void DisableAutomaticReleaseAPAndRequireApprovalOnRFPBillProcess()
        {
            EPSetup epsetup = PXSelect<EPSetup>.Select(this);
            if (epsetup != null)
            {
                ATPTEFMEPSetupExtension epsetupExt = epsetup.GetExtension<ATPTEFMEPSetupExtension>();

                EPSetupMaint epGraph = PXGraph.CreateInstance<EPSetupMaint>();
                epGraph.Clear();

                epGraph.Setup.Current = epsetup;

                epsetup.AutomaticReleaseAP = false;
                epsetupExt.UsrATPTEFMIsRequireApprovalRFPBill = false;

                epGraph.Setup.Update(epsetup);
                epGraph.Save.Press();
            }

            ATPTEFMCASetup caSetup = PXSelect<ATPTEFMCASetup>.Select(this);
            if (caSetup != null)
            {
                ATPTEFMCASetupMaint caGraph = PXGraph.CreateInstance<ATPTEFMCASetupMaint>();
                caGraph.Clear();

                caGraph.Preference.Current = caSetup;

                caSetup.IsRequireApprovalLiquidationBill = false;

                caGraph.Preference.Update(caSetup);
                caGraph.Save.Press();
            }
        }
        private void UpdateCAVendorRefundWrongAmt()
        {
            string caNbr = DataFixDocument.Current.CAVendorRefundWrongAmt.Trim();

            ATPTEFMCashAdvance cashAdvance = PXSelect<
                ATPTEFMCashAdvance,
                Where<ATPTEFMCashAdvance.cashAdvanceNbr, Equal<Required<ATPTEFMCashAdvance.cashAdvanceNbr>>>>
                .Select(this, caNbr);

            if (cashAdvance != null)
            {
                APPayment payment = PXSelect<
                    APPayment,
                    Where<APPayment.docType, Equal<APDocType.refund>,
                        And<APPayment.refNbr, Equal<Required<APPayment.refNbr>>>>>
                    .Select(this, cashAdvance.VendorRefundRefNbr);

                if (payment != null)
                {
                    APPaymentEntry paymentEntry = PXGraph.CreateInstance<APPaymentEntry>();
                    paymentEntry.Document.Current = payment;

                    foreach (APAdjust adjust in paymentEntry.Adjustments.Select())
                    {
                        paymentEntry.Adjustments.Delete(adjust);
                    }

                    APAdjust adj = paymentEntry.Adjustments.Insert(new APAdjust
                    {
                        AdjdDocType = APDocType.Prepayment,
                        AdjdRefNbr = cashAdvance.PpmRefNbr,
                        CuryAdjgAmt = cashAdvance.CuryChangeAmount,
                    });
                    paymentEntry.Adjustments.Update(adj);

                    payment.CuryOrigDocAmt = cashAdvance.CuryChangeAmount;
                    paymentEntry.Document.Update(payment);

                    paymentEntry.Save.Press();

                }
            }
        }

        /// <summary>
        /// Fixes stuck CA refunds by adjusting the APAdjust amount in Documents to Apply tab
        /// to match the remaining PPM Balance of the Cash Advance.
        /// </summary>
        /// <remarks>
        /// 2026-01-27 : Fix stuck CA refund by syncing APAdjust amounts with PPM Balance.
        /// This is a minimal data fix - it only updates the adjustment amount, not the payment amount.
        /// </remarks>
        private void ProcessCAStuckRefundFixer()
        {
            string caNbr = DataFixDocument.Current.CAStuckRefundFixer?.Trim();
            if (string.IsNullOrEmpty(caNbr)) return;

            ATPTEFMCashAdvance cashAdvance = PXSelect<
                ATPTEFMCashAdvance,
                Where<ATPTEFMCashAdvance.cashAdvanceNbr, Equal<Required<ATPTEFMCashAdvance.cashAdvanceNbr>>>>
                .Select(this, caNbr);

            if (cashAdvance == null)
            {
                throw new PXException(ATPTEFMMessages.CARefNbrNotFound, caNbr);
            }

            if (string.IsNullOrEmpty(cashAdvance.VendorRefundRefNbr))
            {
                throw new PXException(ATPTEFMMessages.CANoVendorRefund, caNbr);
            }

            if (string.IsNullOrEmpty(cashAdvance.PpmRefNbr))
            {
                throw new PXException(ATPTEFMMessages.CANoPpmRefNbr, caNbr);
            }

            // Get the PPM Balance (actual remaining balance on the prepayment)
            APPayment ppm = PXSelect<
                APPayment,
                Where<APPayment.docType, Equal<APDocType.prepayment>,
                    And<APPayment.refNbr, Equal<Required<APPayment.refNbr>>>>>
                .Select(this, cashAdvance.PpmRefNbr);

            if (ppm == null)
            {
                throw new PXException(ATPTEFMMessages.CAPpmNotFound, cashAdvance.PpmRefNbr);
            }

            decimal? ppmBalance = ppm.CuryDocBal ?? 0m;

            // Get the Vendor Refund
            APPayment refund = PXSelect<
                APPayment,
                Where<APPayment.docType, Equal<APDocType.refund>,
                    And<APPayment.refNbr, Equal<Required<APPayment.refNbr>>>>>
                .Select(this, cashAdvance.VendorRefundRefNbr);

            if (refund == null)
            {
                throw new PXException(ATPTEFMMessages.CARefundNotFound, cashAdvance.VendorRefundRefNbr);
            }

            APPaymentEntry paymentEntry = PXGraph.CreateInstance<APPaymentEntry>();
            paymentEntry.Document.Current = refund;

            // Delete existing adjustments
            foreach (APAdjust adjust in paymentEntry.Adjustments.Select())
            {
                paymentEntry.Adjustments.Delete(adjust);
            }

            // Insert new adjustment with the correct PPM Balance
            APAdjust adj = paymentEntry.Adjustments.Insert(new APAdjust
            {
                AdjdDocType = APDocType.Prepayment,
                AdjdRefNbr = cashAdvance.PpmRefNbr,
                CuryAdjgAmt = ppmBalance,
            });
            paymentEntry.Adjustments.Update(adj);

            // Update the refund payment amount to match the PPM Balance
            refund.CuryOrigDocAmt = ppmBalance;
            paymentEntry.Document.Update(refund);

            paymentEntry.Save.Press();

            PXTrace.WriteInformation($"Successfully fixed stuck CA refund for {caNbr}. Adjusted amount to {ppmBalance}.");
        }

        /// <summary>
        /// Recalculates <see cref="ATPTEFMCashAdvance.CuryActualSpentAmount"/> (the Liquidation
        /// amount shown on the Cash Advance summary) from the sum of non-reversed CA Receipt
        /// Details. This addresses cases where the summary field falls out of sync with the
        /// actual released liquidations, e.g. after running the Stuck Refund fixer or after
        /// a liquidation was processed on a CA that already had a vendor refund.
        /// </summary>
        /// <remarks>
        /// 2026-04-17 : Added recalc liquidation amount datafixer to sync CA summary
        /// Liquidation with non-reversed CA receipt totals (e.g. CA0000003655) : RY
        /// </remarks>
        private void ProcessCARecalcLiquidationAmt()
        {
            string caNbr = DataFixDocument.Current.CARecalcLiquidationAmt?.Trim();
            if (string.IsNullOrEmpty(caNbr)) return;

            ATPTEFMCashAdvance cashAdvance = PXSelect<
                ATPTEFMCashAdvance,
                Where<ATPTEFMCashAdvance.cashAdvanceNbr, Equal<Required<ATPTEFMCashAdvance.cashAdvanceNbr>>>>
                .Select(this, caNbr);

            if (cashAdvance == null)
            {
                throw new PXException(ATPTEFMMessages.CARefNbrNotFound, caNbr);
            }

            decimal newLiquidation = PXSelect<
                    ATPTEFMCAReceiptDetail,
                    Where<ATPTEFMCAReceiptDetail.cashAdvanceNbr, Equal<Required<ATPTEFMCAReceiptDetail.cashAdvanceNbr>>>>
                .Select(this, caNbr)
                .RowCast<ATPTEFMCAReceiptDetail>()
                .Where(r => r.Reversed != true)
                .Sum(r => r.CuryNetAmt ?? 0m);

            decimal previous = cashAdvance.CuryActualSpentAmount ?? 0m;

            // Update directly via PXDatabase to avoid triggering unrelated graph
            // handlers (e.g. AP invoice extensions) that can fail on unrelated
            // schema/data issues while persisting the Cash Advance graph.
            PXDatabase.Update<ATPTEFMCashAdvance>(
                new PXDataFieldAssign<ATPTEFMCashAdvance.curyActualSpentAmount>(newLiquidation),
                new PXDataFieldAssign<ATPTEFMCashAdvance.actualSpentAmount>(newLiquidation),
                new PXDataFieldRestrict<ATPTEFMCashAdvance.cashAdvanceNbr>(caNbr)
            );

            PXTrace.WriteInformation(
                $"Recalculated CA Liquidation amount for {caNbr}: {previous} -> {newLiquidation}.");
        }

        /// <remarks>
        /// 2025-07-10 : LIQ000002361 is not part of the liquidation on CA000002351 : 013767 : RFS
        /// 2026-02-26 : [CBE 2024R2] CA CBM-CA000000185 Receipts Tab - remove duplicate lines : 015430 : RFS
        /// </remarks>
        private void ProcessCleanUpCA()
        {
            string caNbr = DataFixDocument.Current.CleanUpCAReceiptsIssues.Trim();

            ATPTEFMCashAdvance cashAdvance = PXSelect<
                ATPTEFMCashAdvance,
                Where<ATPTEFMCashAdvance.cashAdvanceNbr, Equal<Required<ATPTEFMCashAdvance.cashAdvanceNbr>>>>
                .Select(this, caNbr);

            if (cashAdvance != null)
            {
                var caReceipts = PXSelect<
                    ATPTEFMCAReceiptDetail,
                    Where<ATPTEFMCAReceiptDetail.cashAdvanceNbr, Equal<Required<ATPTEFMCAReceiptDetail.cashAdvanceNbr>>>>
                    .Select(this, caNbr)
                    .RowCast<ATPTEFMCAReceiptDetail>()
                    .ToList();

                // determine lowest IID for each expense receipt ref nbr that has duplicates
                var minIdByRef = caReceipts
                    .Where(r => !string.IsNullOrEmpty(r.ExpenseReceiptRefNbr))
                    .GroupBy(r => r.ExpenseReceiptRefNbr)
                    .Where(g => g.Count() > 1)
                    .ToDictionary(g => g.Key, g => g.Min(r => r.CashAdvanceReceiptDetailIID ?? int.MaxValue));

                foreach (ATPTEFMCAReceiptDetail result in caReceipts)
                {
                    if (!string.IsNullOrEmpty(result.ExpenseReceiptRefNbr) &&
                        minIdByRef.TryGetValue(result.ExpenseReceiptRefNbr, out int minId) &&
                        (result.CashAdvanceReceiptDetailIID ?? 0) > minId)
                    {
                        // delete duplicate records keeping the one with lowest IID
                        PXDatabase.Delete<ATPTEFMCAReceiptDetail>(
                            new PXDataFieldRestrict<ATPTEFMCAReceiptDetail.cashAdvanceReceiptDetailIID>(result.CashAdvanceReceiptDetailIID)
                        );
                        continue; // skip ghost liquidation checks for deleted row
                    }

                    //Check ghost liquidation nbrs for the remaining rows
                    if (result.LiquidationRef != null)
                    {
                        EPExpenseClaim ec = PXSelect<
                            EPExpenseClaim,
                            Where<ATPTEFMEPExpenseClaimExt.usrATPTEFMLiqNbr, Equal<@P.AsString>>>
                            .Select(this, result.LiquidationRef);
                        if (ec == null || ec.Status == ATPTEFMExpenseClaimStatusAttribute.CancelledValue)
                        {
                            PXDatabase.Update<ATPTEFMCAReceiptDetail>(
                                new PXDataFieldAssign<ATPTEFMCAReceiptDetail.liquidationRef>(null),
                                new PXDataFieldRestrict<ATPTEFMCAReceiptDetail.cashAdvanceReceiptDetailIID>(result.CashAdvanceReceiptDetailIID)
                            );
                        }
                    }
                }

                // after removing duplicates, recompute the actual spent amount on the cash advance by re‑querying the database
                decimal total = PXSelect<ATPTEFMCAReceiptDetail,
                        Where<ATPTEFMCAReceiptDetail.cashAdvanceNbr, Equal<Required<ATPTEFMCAReceiptDetail.cashAdvanceNbr>>>>
                    .Select(this, caNbr)
                    .RowCast<ATPTEFMCAReceiptDetail>()
                    .Where(r => r.Reversed != true)
                    .Sum(r => r.CuryNetAmt ?? 0m);

                // update the header using the CA graph so that currency info fields are maintained
                ATPTEFMCashAdvanceEntry caGraph = PXGraph.CreateInstance<ATPTEFMCashAdvanceEntry>();
                caGraph.CashAdvances.Current = cashAdvance;
                cashAdvance.CuryActualSpentAmount = total;
                caGraph.CashAdvances.Update(cashAdvance);
                caGraph.Actions.PressSave();
            }
        }

        /// <remarks>
        /// 02-27-2026 : 015477 - Updated DeleteCAReceiptsWithRefs fixer to remove receipts with CashAdvanceRequestDetailID = 0 {RFS}
        /// </remarks>
        private void ProcessDeleteCAReceiptsWithRefs()
        {
            string caNbr = DataFixDocument.Current.DeleteCAReceiptsWithRefs.Trim();

            ATPTEFMCashAdvance cashAdvance = PXSelect<
                ATPTEFMCashAdvance,
                Where<ATPTEFMCashAdvance.cashAdvanceNbr, Equal<Required<ATPTEFMCashAdvance.cashAdvanceNbr>>>>
                .Select(this, caNbr);

            if (cashAdvance != null)
            {
                var caReceipts = PXSelect<
                    ATPTEFMCAReceiptDetail,
                    Where<ATPTEFMCAReceiptDetail.cashAdvanceNbr, Equal<Required<ATPTEFMCAReceiptDetail.cashAdvanceNbr>>>>
                    .Select(this, caNbr)
                    .RowCast<ATPTEFMCAReceiptDetail>()
                    .ToList();

                foreach (var result in caReceipts)
                {
                    if (result.CashAdvanceRequestDetailID == 0)
                    {
                        PXDatabase.Delete<ATPTEFMCAReceiptDetail>(
                            new PXDataFieldRestrict<ATPTEFMCAReceiptDetail.cashAdvanceReceiptDetailIID>(result.CashAdvanceReceiptDetailIID)
                        );
                    }
                }

                decimal total = PXSelect<ATPTEFMCAReceiptDetail,
                        Where<ATPTEFMCAReceiptDetail.cashAdvanceNbr, Equal<Required<ATPTEFMCAReceiptDetail.cashAdvanceNbr>>>>
                    .Select(this, caNbr)
                    .RowCast<ATPTEFMCAReceiptDetail>()
                    .Where(r => r.Reversed != true)
                    .Sum(r => r.CuryNetAmt ?? 0m);

                ATPTEFMCashAdvanceEntry caGraph = PXGraph.CreateInstance<ATPTEFMCashAdvanceEntry>();
                caGraph.CashAdvances.Current = cashAdvance;
                cashAdvance.CuryActualSpentAmount = total;
                caGraph.CashAdvances.Update(cashAdvance);
                caGraph.Actions.PressSave();
            }
        }
        private void ProcessSetLiquidationAmtToZero()
        {
            string caNbr = DataFixDocument.Current.CAZeroLiquidationAmt.Trim();

            ATPTEFMCashAdvance cashAdvance = SelectFrom<ATPTEFMCashAdvance>
                .Where<ATPTEFMCashAdvance.cashAdvanceNbr.IsEqual<@P.AsString>>
                .View.Select(this, caNbr);

            if (cashAdvance != null)
            {
                ATPTEFMCashAdvanceEntry caGraph = PXGraph.CreateInstance<ATPTEFMCashAdvanceEntry>();
                caGraph.CashAdvances.Current = cashAdvance;

                cashAdvance.CuryActualSpentAmount = 0m;

                caGraph.CashAdvances.Update(cashAdvance);
                caGraph.Save.Press();
            }
        }
        public void UpdateFundManagementPrefereceGLAccountSetup()
        {
            ATPTEFMSetup ftSetup = PXSelect<ATPTEFMSetup>.Select(this);
            if (ftSetup != null)
            {
                ATPTEFMFundTransactionPreference ftSetupGraph = PXGraph.CreateInstance<ATPTEFMFundTransactionPreference>();
                ftSetupGraph.Clear();

                ftSetupGraph.Preference.Current = ftSetup;

                if (ftSetup.UseExpenseAcctFrom == null)
                    ftSetup.UseExpenseAcctFrom = ATPTEFMFTAccountSource.PurchaseItem;

                if (ftSetup.CombineExpSub == null)
                    ftSetup.CombineExpSub = new string(char.Parse(ATPTEFMFTAcctSubDefault.MaskItem), PXDimensionAttribute.GetLength("SUBACCOUNT"));

                ftSetupGraph.Preference.Update(ftSetup);
                ftSetupGraph.Save.Press();
            }
        }
        public static void ForceReceiptStatusToLiquidated(ATPTEFMCashFundDataFixMaint Base)
        {
            ATPTEFMFundMaint graph = PXGraph.CreateInstance<ATPTEFMFundMaint>();

            string[] refNbrList = Base.DataFixDocument.Current.FundHistoryErNbr.Split(';');

            foreach (var refNbr in refNbrList)
            {
                PXResultset<ATPTEFMFundTransactionHistoryView, ATPTEFMFund> ds = PXSelectJoin<
                    ATPTEFMFundTransactionHistoryView,
                    InnerJoin<ATPTEFMFund,
                        On<ATPTEFMFund.fundCD, Equal<ATPTEFMFundTransactionHistoryView.fundRefNbr>>>,
                    Where<ATPTEFMFundTransactionHistoryView.refNbr,
                    Equal<Required<ATPTEFMFundTransactionHistoryView.refNbr>>>>
                    .Select<PXResultset<ATPTEFMFundTransactionHistoryView, ATPTEFMFund>>(Base, refNbr.Trim());

                ATPTEFMFundTransactionHistoryView receiptHistory = ds;
                ATPTEFMFund fund = ds;

                if (receiptHistory != null && fund != null)
                {
                    graph.Document.Current = fund;
                    receiptHistory.Status = ATPTEFMFundStatusAttribute.LiquidatedValue;
                    graph.CurrentTransactionHistoryView.Update(receiptHistory);
                    graph.Save.Press();
                }
            }
        }
        public static void ForceReceiptStatusToCancel(ATPTEFMCashFundDataFixMaint Base)
        {
            ATPTEFMFundMaint graph = PXGraph.CreateInstance<ATPTEFMFundMaint>();

            string[] refNbrList = Base.DataFixDocument.Current.ClaimDetailCD.Split(';');

            foreach (var refNbr in refNbrList)
            {
                PXResultset<ATPTEFMFundTransactionHistoryView, ATPTEFMFund> ds = PXSelectJoin<
                    ATPTEFMFundTransactionHistoryView,
                    InnerJoin<ATPTEFMFund,
                        On<ATPTEFMFund.fundCD, Equal<ATPTEFMFundTransactionHistoryView.fundRefNbr>>>,
                    Where<ATPTEFMFundTransactionHistoryView.refNbr,
                    Equal<Required<ATPTEFMFundTransactionHistoryView.refNbr>>>>
                    .Select<PXResultset<ATPTEFMFundTransactionHistoryView, ATPTEFMFund>>(Base, refNbr.Trim());

                ATPTEFMFundTransactionHistoryView receiptHistory = ds;
                ATPTEFMFund fund = ds;

                if (receiptHistory != null && fund != null)
                {
                    graph.Document.Current = fund;
                    receiptHistory.Status = ATPTEFMFundStatusAttribute.CancelledValue;
                    graph.CurrentTransactionHistoryView.Update(receiptHistory);
                    graph.Save.Press();
                }
            }
        }
        public void FundTransactionHistoryRemoveDuplicateAndFixBalance()
        {
            ATPTEFMFundMaint fundGraph = PXGraph.CreateInstance<ATPTEFMFundMaint>();
            string fundCD = DataFixDocument.Current.FundHistoryDuplicateBalanceFixer?.Trim();

            if (string.IsNullOrEmpty(fundCD)) return;

            ATPTEFMFund fund = PXSelect<
                ATPTEFMFund,
                Where<ATPTEFMFund.fundCD, Equal<Required<ATPTEFMFund.fundCD>>>>
                .Select(this, fundCD);

            if (fund == null) return;

            fundGraph.Document.Current = fund;

            // Get all transaction history records for this fund

            var transactions = fundGraph.CurrentTransactionHistoryView.Select()
                    .RowCast<ATPTEFMFundTransactionHistoryView>()
                    .ToList();

            var duplicateGroups = transactions
                   .GroupBy(t => new { t.RefNbr, t.TransactionType })
                   .Where(g => g.Count() > 1)
                   .ToList();

            if (transactions.Count == 0) return;

            if (duplicateGroups.Any())
            {
                foreach (var group in duplicateGroups)
                {
                    ATPTEFMFundTransaction ft = PXSelect<
                        ATPTEFMFundTransaction,
                        Where<ATPTEFMFundTransaction.refNbr,
                        Equal<Required<ATPTEFMFundTransaction.refNbr>>>>
                        .Select(this, group.Key.RefNbr);

                    if (ft != null)
                    {
                        foreach (ATPTEFMFundTransactionHistoryView fHistory in PXSelect<
                            ATPTEFMFundTransactionHistoryView,
                            Where<ATPTEFMFundTransactionHistoryView.refNbr,
                            Equal<Required<ATPTEFMFundTransactionHistoryView.refNbr>>,
                                And<ATPTEFMFundTransactionHistoryView.transactionType, Equal<Required<ATPTEFMFundTransactionHistoryView.transactionType>>>>>
                            .Select(this, group.Key.RefNbr, group.Key.TransactionType))
                        {
                            if (fHistory.Status.Equals(ft.Status)) continue;

                            PXDatabase.Delete<ATPTEFMFundTransactionHistoryView>(
                            new PXDataFieldRestrict<ATPTEFMFundTransactionHistoryView.fundTransactionHistoryID>(fHistory.FundTransactionHistoryID));
                        }
                    }
                }
            }

            FundTransactionHistoryBalanceFixer(fundCD);
        }

        private void FTReceiptsBranchMigrationForOldData()
        {
            ATPTEFMFundTransactionEntry ftGraph = PXGraph.CreateInstance<ATPTEFMFundTransactionEntry>();

            foreach (ATPTEFMFundTransaction ft in PXSelect<ATPTEFMFundTransaction>.Select(this))
            {
                ftGraph.Clear();
                ftGraph.FundTransactions.Current = ft;

                if (ftGraph.FundTransactionReceiptLines.Select().Count > 0)
                {
                    Branch employeeBranchID = PXSelectJoin<
                    Branch,
                    InnerJoin<EPEmployee,
                        On<EPEmployee.parentBAccountID, Equal<Branch.bAccountID>>>,
                    Where<EPEmployee.bAccountID, Equal<Required<EPEmployee.bAccountID>>>>
                    .Select(this, ft.RequestedByID);

                    var branchID = employeeBranchID.BranchID;

                    PXDatabase.Update<ATPTEFMFundTransactionReceiptDetail>(
                 new PXDataFieldAssign<ATPTEFMFundTransactionReceiptDetail.branchID>(branchID),
                 new PXDataFieldRestrict<ATPTEFMFundTransactionReceiptDetail.fundTransactionRefNbr>(ft.RefNbr));
                }
            }
        }

        /// <remarks>
        /// 2026-07-01 : Deletes leftover detail lines on the selected Reimbursement-type fund transactions (which should have none) so they no longer wrongly filter the Expense Receipt item selector. {RFS}
        /// </remarks>
        private void RemoveReimbursementFTDetailsProcess()
        {
            string[] refNbrList = DataFixDocument.Current.RemoveReimbursementFTDetails.Split(';');

            foreach (var refNbr in refNbrList)
            {
                foreach (ATPTEFMFundTransactionDetail detail in PXSelectJoin<
                    ATPTEFMFundTransactionDetail,
                    InnerJoin<ATPTEFMFundTransaction,
                        On<ATPTEFMFundTransaction.refNbr, Equal<ATPTEFMFundTransactionDetail.fundTransactionRefNbr>>>,
                    Where<ATPTEFMFundTransactionDetail.fundTransactionRefNbr, Equal<Required<ATPTEFMFundTransactionDetail.fundTransactionRefNbr>>,
                        And<ATPTEFMFundTransaction.fundTransactionType, Equal<ATPTEFMFundTransactionTypeAttribute.reimbursementValue>>>>
                    .Select(this, refNbr.Trim()))
                {
                    PXDatabase.Delete<ATPTEFMFundTransactionDetail>(
                        new PXDataFieldRestrict<ATPTEFMFundTransactionDetail.fundTransactionDetailID>(detail.FundTransactionDetailID));
                }
            }
        }

        #endregion
    }


    public sealed class ATPTEFMFundTransactionExt : PXCacheExtension<ATPTEFMFundTransaction>
    {
        public static bool IsActive() => true;

        #region DateOfUse
        [PXMergeAttributes(Method = MergeMethod.Append)]
        [PXRemoveBaseAttribute(typeof(PXUIRequiredAttribute))]
        [PXRemoveBaseAttribute(typeof(PXVerifyEndDateAttribute))]
        public DateTime? DateOfUse { get; set; }
        public abstract class dateOfUse : PX.Data.BQL.BqlDateTime.Field<dateOfUse> { }
        #endregion
    }
}
