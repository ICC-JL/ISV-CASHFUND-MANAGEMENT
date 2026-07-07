using System;
using System.Collections;
using System.Collections.Generic;
using PX.Data;
using PX.Objects.EP;
using PX.Objects.CM;
using PX.Objects.GL;
using CashFundManagement.DAC;
using CashFundManagement.Messages;
using CashFundManagement.Extensions.DAC;
using CashFundManagement.DAC.Setup;
using CashFundManagement.Attributes.Base;
using static CashFundManagement.Helper.ATPTEFMShared;
using PX.Data.WorkflowAPI;
using CashFundManagement.Attributes;
using CashFundManagement.Helper;
using static CashFundManagement.BLC.ATPTEFMFundMaint;
using System.Linq;
using PX.Objects.Common.Extensions;
using PX.Objects.PM;

namespace CashFundManagement.BLC
{
    /// <remarks>
    /// 2025-09-04 : Pass ER branch id value to MET detail branch id : 012847 : RFS
    /// </remarks>
    public class ATPTEFMMonthEndEntry : ATPTPXGraphWithWorkflow<ATPTEFMMonthEndEntry, ATPTEFMMonthEnd>
    {
        #region Views
        public PXSetup<ATPTEFMCASetup> Setup;
        public PXSetup<ATPTEFMSetup> Preferences;

        [PXViewName("Month-End Transaction")]
        public PXSelect<ATPTEFMMonthEnd> Document;
        public PXSelect<
            ATPTEFMMonthEnd,
            Where<ATPTEFMMonthEnd.refNbr, Equal<Current<ATPTEFMMonthEnd.refNbr>>>>
            CurrentDocument;

        [PXCopyPasteHiddenView]
        public PXSelect<
            ATPTEFMMonthEndDetail,
            Where<ATPTEFMMonthEndDetail.monthEndRefNbr, Equal<Current<ATPTEFMMonthEnd.refNbr>>>>
            Details;

        [PXViewName("Transaction History")]
        public PXSelect<
            ATPTEFMFundTransactionHistoryView, 
            Where<ATPTEFMFundTransactionHistoryView.refNbr, Equal<Required<ATPTEFMFundTransactionHistoryView.refNbr>>>> 
            TransactionHistoryView;


        [PXViewName("Fund")]
        public PXSelect<
            ATPTEFMFund,
            Where<ATPTEFMFund.fundCD, Equal<Required<ATPTEFMFundTransaction.fundID>>>>
            Fund;

        [PXViewName("Setup Approval")]
        public PXSelect<ATPTEFMMonthEndSetupApproval> SetupApproval;

        [PXViewName("Approval")]
        public EPApprovalAutomation<
            ATPTEFMMonthEnd, ATPTEFMMonthEnd.approved, ATPTEFMMonthEnd.rejected, ATPTEFMMonthEnd.hold, ATPTEFMMonthEndSetupApproval>
            Approval;

        [PXReadOnlyView]
        public PXSelect<EPExpenseClaimDetails> ExpenseReceipts;


        #endregion

        public ATPTEFMMonthEndEntry()
        {
            // Block access when Cash Fund Management is disabled in Preferences (ATPT3104)
#if !Version23R2
            if (!(Setup?.Current?.EnableCFM ?? false))
            {
                throw new PXException(ATPTEFMMessages.CashFundManagementFeatureDisabled);
            }
#endif
        }

        #region Overrides
        public override void Persist()
        {
            ATPTEFMMonthEnd monthEnd = Document.Current;
            base.Persist();

            bool isMonthEndRecordExist = IsMonthEndRecordExist();

            #region Update Month end record
            if (isMonthEndRecordExist)
            {
                UpdateMonthEndRecordHistory();
                base.Persist();
            }
            #endregion

            #region Insert Month end Record
            if (isMonthEndRecordExist == false)
            {
                ATPTEFMFundTransactionHistoryView transactionHistoryDetails = PXSelect<
                    ATPTEFMFundTransactionHistoryView,
                    Where<ATPTEFMFundTransactionHistoryView.fundRefNbr, Equal<Required<ATPTEFMFundTransactionHistoryView.fundRefNbr>>>,
                    OrderBy<
                        Asc<ATPTEFMFundTransactionHistoryView.sortNbr>>>
                    .Select(this, monthEnd.FundID)
                    .LastOrDefault();

                if (transactionHistoryDetails != null)
                {
                    string transactionNbrSorting = string.Empty;

                    switch (transactionHistoryDetails.Source)
                    {
                        case ATPTEFMTransactionHistoryView.source.FundTransaction:
                            transactionNbrSorting = $"FT-{transactionHistoryDetails.RefNbr}";
                            break;
                        case ATPTEFMTransactionHistoryView.source.ExpenseReceipt:
                            transactionNbrSorting = transactionHistoryDetails.FundTransactionSortNbr;
                            break;
                        case ATPTEFMTransactionHistoryView.source.Replenishment:
                            transactionNbrSorting = transactionHistoryDetails.SortNbr;
                            break;
                        case ATPTEFMTransactionHistoryView.source.MonthEnd:
                            break;
                        default:
                            break;
                    }
                    ATPTEFMFundTransactionHistoryView transactionHistory = TransactionHistoryView.Insert();
                    transactionHistory.FundRefNbr = monthEnd.FundID;
                    transactionHistory.TransactionType = ATPTEFMTransactionHistoryView.transactionType.MonthEnd;
                    transactionHistory.OrderDate = monthEnd.Date;
                    transactionHistory.RefNbr = monthEnd.RefNbr;
                    transactionHistory.SortNbr = $"{transactionNbrSorting}-M{monthEnd.RefNbr}";
                    transactionHistory.FundBranchID = monthEnd.BranchID;
                    transactionHistory.TransactionDate = monthEnd.Date;
                    transactionHistory.CuryFundTransactionDocumentAmt = monthEnd.Amount;
                    transactionHistory.Status = monthEnd.Status;
                    transactionHistory.Source = ATPTEFMTransactionHistoryView.source.MonthEnd;
                    transactionHistory.CuryBalanceAmt = transactionHistoryDetails.CuryBalanceAmt;
                    transactionHistory.ReversingJournalBatchNbr = string.Empty;
                    TransactionHistoryView.Update(transactionHistory);
                }
                base.Persist();
            }
            #endregion
        }
        #endregion

        #region Methods
        private bool IsMonthEndRecordExist()
        {
            ATPTEFMMonthEnd monthEnd = Document.Current;
            TransactionHistoryView.Current = TransactionHistoryView.Select(monthEnd.RefNbr);
            return (TransactionHistoryView.Current != null) ? true : false;
        }
        private void UpdateMonthEndRecordHistory()
        {
            ATPTEFMMonthEnd monthEnd = Document.Current;
            TransactionHistoryView.Current.CuryFundTransactionDocumentAmt = monthEnd.Amount;
            TransactionHistoryView.Current.Status = monthEnd.Status;
            TransactionHistoryView.Current.ReversingJournalBatchNbr = (monthEnd.Status.Equals(ATPTEFMMonthEndStatusAttribute.ClosedValue)) ? monthEnd.ReversingJournalBatchNbr : string.Empty;
            TransactionHistoryView.UpdateCurrent();
        }
        #endregion

        #region View Delegates
        public IEnumerable expenseReceipts()
        {
            HashSet<string> receiptsListToRemove = new HashSet<string>();

            //Remove receipts called in Replenishment
            foreach (ATPTEFMReplenishmentDetail replenish in PXSelectJoin<
                ATPTEFMReplenishmentDetail,
                InnerJoin<ATPTEFMReplenishment,
                    On<ATPTEFMReplenishment.replenishmentNbr, Equal<ATPTEFMReplenishmentDetail.replenishmentNbr>,
                    And<ATPTEFMReplenishment.fundID, Equal<Current<ATPTEFMMonthEnd.fundID>>,
                    And<ATPTEFMReplenishment.status, NotEqual<ATPTEFMReplenishmentStatusAttribute.cancelledValue>>>>>>
                .Select(this))
            {
                if (replenish != null)
                {
                    receiptsListToRemove.Add(replenish.ExpenseReceiptNbr);
                }
            }
            //Remove current receipts
            foreach (ATPTEFMMonthEndDetail med in Details.Select())
            {
                if (med != null)
                {
                    receiptsListToRemove.Add(med.ExpenseReceiptRefNbr);
                }
            }
            //Remove receipts called in other MET except for MET that has Reverse Batch
            foreach (ATPTEFMMonthEndDetail med in PXSelectJoin<
                ATPTEFMMonthEndDetail,
                InnerJoin<ATPTEFMMonthEnd,
                    On<ATPTEFMMonthEndDetail.monthEndRefNbr, Equal<ATPTEFMMonthEnd.refNbr>>>,
                Where<ATPTEFMMonthEnd.fundID, Equal<Current<ATPTEFMMonthEnd.fundID>>,
                    And<ATPTEFMMonthEnd.status, NotEqual<ATPTEFMMonthEndStatusAttribute.closedValue>>>>
                .Select(this))
            {
                if (med != null)
                {
                    receiptsListToRemove.Add(med.ExpenseReceiptRefNbr);
                }
            }

            //Show Receipts
            foreach (EPExpenseClaimDetails result in PXSelectJoin<
                EPExpenseClaimDetails,
                InnerJoin<ATPTEFMFundTransactionReceiptDetail,
                    On<ATPTEFMFundTransactionReceiptDetail.expenseReceiptRefNbr, Equal<EPExpenseClaimDetails.claimDetailCD>>,
                InnerJoin<ATPTEFMFundTransaction,
                    On<ATPTEFMFundTransactionReceiptDetail.fundTransactionRefNbr, Equal<ATPTEFMFundTransaction.refNbr>>>>,
                Where<ATPTEFMFundTransaction.fundID, Equal<Current<ATPTEFMMonthEnd.fundID>>,
                    And2<
                        Where<ATPTEFMEPExpenseClaimDetailsExt.usrATPTEFMMonthEnd, Equal<False>,
                            Or<ATPTEFMEPExpenseClaimDetailsExt.usrATPTEFMMonthEnd, IsNull>>,
                        And2<
                            Where<EPExpenseClaimDetails.status, Equal<ATPTEFMBaseStatus.openValue>,
                                Or<EPExpenseClaimDetails.status, Equal<ATPTEFMBaseStatus.approvedValue>>>,
                            And<ATPTEFMFundTransaction.cashAdvanceStatus, Equal<ATPTEFMFundTransactionCashAdvanceStatusAttribute.liquidatedValue>,
                            And<EPExpenseClaimDetails.expenseDate, LessEqual<Current<ATPTEFMMonthEnd.date>>>>>>>>
                .Select(this))
            {
                if (receiptsListToRemove.Contains(result.ClaimDetailCD))
                {
                    continue;
                }
                yield return result;
            }
        }
        #endregion

        #region Action
        public PXInitializeState<ATPTEFMMonthEnd> InitializeState;

        /// <remarks>
        /// 2025-05-09 : Force populate nonproject code for the credit line : 011518 : RFS
        /// </remarks>
        public PXAction<ATPTEFMMonthEnd> Release;

        [PXUIField(DisplayName = ATPTEFMMessages.Release)]
        [PXProcessButton()]
        public IEnumerable release(PXAdapter adapter)
        {
            ATPTEFMHelper.StartLongOperation(this, adapter, delegate ()
            {
                Actions.PressSave();
                using (PXTransactionScope ts = new PXTransactionScope())
                {
                    ATPTEFMMonthEnd me = Document.Current;

                    JournalEntry jeGraph = PXGraph.CreateInstance<JournalEntry>();

                    var insertJournal = jeGraph.BatchModule.Insert();
                    insertJournal.Hold = true;
                    insertJournal.Status = BatchStatus.Hold;
                    insertJournal.RefNbr = Document.Current.RefNbr;
                    insertJournal.AutoReverse = true;
                    insertJournal.Description = string.Format("Month-End Transaction {0}", Document.Current.RefNbr);
                    insertJournal.DateEntered = Document?.Current?.Date;
                    insertJournal.BranchID = Document?.Current?.BranchID;

                    jeGraph.BatchModule.Current = insertJournal;

                    insertJournal.CuryID = me.CuryID;
                    insertJournal.CuryInfoID = me.CuryInfoID;

                    jeGraph.BatchModule.Update(insertJournal);

                    decimal? totalAmount = 0;

                    ExpenseClaimDetailEntry ecGraph = PXGraph.CreateInstance<ExpenseClaimDetailEntry>();

                    foreach (ATPTEFMMonthEndDetail detail in Details.Select())
                    {
                        var insDebitJournal = jeGraph.GLTranModuleBatNbr.Insert(new GLTran()
                        {
                            AccountID = detail.AccountID,
                            SubID = detail.SubID,
                            ProjectID = detail.ContractID,
                            TaskID = detail.TaskID,
                            CostCodeID = detail.CostCodeID,
                            CuryDebitAmt = 0M,
                            CuryCreditAmt = 0M,
                            BranchID = detail.BranchID
                        });

                        jeGraph.GLTranModuleBatNbr.SetValueExt<GLTran.curyDebitAmt>(insDebitJournal, detail.Amount);

                        insDebitJournal = jeGraph.GLTranModuleBatNbr.Update(insDebitJournal);

                        totalAmount += detail.Amount;

                        EPExpenseClaimDetails er = PXSelect<
                            EPExpenseClaimDetails,
                            Where<EPExpenseClaimDetails.claimDetailCD, Equal<Required<EPExpenseClaimDetails.claimDetailCD>>>>
                            .Select(this, detail.ExpenseReceiptRefNbr);
                        ATPTEFMEPExpenseClaimDetailsExt erExt = er.GetExtension<ATPTEFMEPExpenseClaimDetailsExt>();

                        erExt.UsrATPTEFMMonthEnd = true;

                        ecGraph.ClaimDetails.Update(er);
                    }

                    ecGraph.Save.Press();

                    int? creditAccountId = null;

                    if (Document.Current.CreditAccountID is null)
                    {
                        creditAccountId = Document.Current.AccountID;
                    }
                    else
                    {
                        creditAccountId = Document.Current.CreditAccountID;
                    }

                    var insCreditJournal = jeGraph.GLTranModuleBatNbr.Insert(new GLTran()
                    {
                        AccountID = creditAccountId,
                        SubID = Document.Current.SubID,
                        CuryDebitAmt = 0M,
                        CuryCreditAmt = 0M
                    });

                    jeGraph.GLTranModuleBatNbr.SetValueExt<GLTran.curyCreditAmt>(insCreditJournal, totalAmount);

                    insCreditJournal.ProjectID = ProjectDefaultAttribute.NonProject();

                    insCreditJournal = jeGraph.GLTranModuleBatNbr.Update(insCreditJournal);

                    jeGraph.BatchModule.SetValueExt<Batch.curyControlTotal>(insertJournal, totalAmount);
                    insertJournal = jeGraph.BatchModule.Update(insertJournal);

                    jeGraph.Save.Press();

                    me.Status = ATPTEFMMonthEndStatusAttribute.OpenValue;
                    me.Released = true;
                    me.JournalBatchNbr = insertJournal.BatchNbr;

                    Document.Update(me);
                    this.Save.Press();

                    if (Preferences?.Current?.AutoReleaseMonthEndJournal ?? false)
                    {
                        if (jeGraph.releaseFromHold.GetEnabled())
                        {
                            jeGraph.releaseFromHold.Press();

                            if (insertJournal.Status != BatchStatus.PendingApproval)
                            {
                                jeGraph.release.Press();
                            }
                            else
                            {
                                jeGraph.putOnHold.Press();
                            }
                        }
                        else
                        {
                            jeGraph.release.Press();
                        }
                    }
                    jeGraph.Save.Press();

                    ts.Complete();
                }
            });

            return adapter.Get();
        }

        public PXAction<ATPTEFMMonthEnd> ViewBatch;
        [PXButton(Tooltip = "View Batch", CommitChanges = true)]
        [PXUIField(Visible = false)]
        public virtual void viewBatch()
        {
            ATPTEFMMonthEnd row = CurrentDocument.Current;

            if (row != null)
            {
                JournalEntry graph = PXGraph.CreateInstance<JournalEntry>();
                graph.BatchModule.Current = graph.BatchModule.Search<Batch.batchNbr>(row.JournalBatchNbr);

                if (graph.BatchModule.Current != null)
                {
                    throw new PXRedirectRequiredException(graph, true, "Journal Transaction") { Mode = PXBaseRedirectException.WindowMode.NewWindow };
                }
            }
        }

        public PXAction<ATPTEFMMonthEnd> ViewReversingBatch;
        [PXButton(Tooltip = "View Reversing Batch", CommitChanges = true)]
        [PXUIField(Visible = false)]
        public virtual void viewReversingBatch()
        {
            ATPTEFMMonthEnd row = CurrentDocument.Current;

            if (row != null)
            {
                JournalEntry graph = PXGraph.CreateInstance<JournalEntry>();
                graph.BatchModule.Current = graph.BatchModule.Search<Batch.batchNbr>(row.ReversingJournalBatchNbr);

                if (graph.BatchModule.Current != null)
                {
                    throw new PXRedirectRequiredException(graph, true, "Reversing Journal Transaction") { Mode = PXBaseRedirectException.WindowMode.NewWindow };
                }
            }
        }
        public PXAction<ATPTEFMMonthEnd> showSubmitReceipt;
        [PXUIField(DisplayName = ATPTEFMMessages.ShowSubmitReceipt)]
        [PXButton(Tooltip = "Add Receipts")]
        protected virtual IEnumerable ShowSubmitReceipt(PXAdapter adapter)
        {
            if (ExpenseReceipts.AskExt(true) == WebDialogResult.OK)
            {
                return SubmitReceipt(adapter);
            }
            ExpenseReceipts.Cache.Clear();
            return adapter.Get();
        }

        public PXAction<ATPTEFMMonthEnd> submitReceipt;
        [PXUIField(DisplayName = ATPTEFMMessages.SubmitReceipt)]
        [PXButton()]
        protected virtual IEnumerable SubmitReceipt(PXAdapter adapter)
        {
            foreach (EPExpenseClaimDetails item in ExpenseReceipts.Select())
            {
                if (item.Selected == true)
                {
                    item.Selected = false;
                    ATPTEFMMonthEndDetail detail = Details.Insert();

                    detail.ExpenseReceiptRefNbr = item.ClaimDetailCD;
                    detail.InventoryID = item.InventoryID;
                    detail.AccountID = item.ExpenseAccountID;
                    detail.SubID = item.ExpenseSubID;
                    detail.ContractID = item.ContractID;
                    detail.TaskID = item.TaskID;
                    detail.CostCodeID = item.CostCodeID;
                    detail.Amount = item.CuryExtCost;
                    detail.BranchID = item.BranchID;

                    Details.Update(detail);
                }
            }

            ExpenseReceipts.Cache.Clear();
            return adapter.Get();
        }

        public PXAction<ATPTEFMMonthEnd> cancelSubmitReceipt;
        [PXUIField(DisplayName = ATPTEFMMessages.CancelSubmitReceipt)]
        [PXButton()]
        protected virtual IEnumerable CancelSubmitReceipt(PXAdapter adapter)
        {
            ExpenseReceipts.Cache.Clear();
            return adapter.Get();
        }
        #endregion

        #region Events
        protected virtual void _(Events.RowSelected<ATPTEFMMonthEnd> e)
        {
            ATPTEFMMonthEnd monthEnd = e.Row;
            if (monthEnd == null) return;

            //Set Defaults
            //Details.Cache.SetAllEditPermissions(false);
            Details.AllowInsert = false;
            Details.AllowUpdate = false;

            bool approved = monthEnd?.Approved ?? false;
            bool released = monthEnd?.Released ?? false;
            bool isClosedDocument = monthEnd.Status != ATPTEFMMonthEndStatusAttribute.ClosedValue;

            Release.SetEnabled(approved);

            if (released) { Release.SetEnabled(false); }
            Document.AllowUpdate = !approved;
            CurrentDocument.AllowUpdate = isClosedDocument;
            this.Delete.SetEnabled(!approved);

            showSubmitReceipt.SetEnabled(!approved);

            #region Set Error For Date Field
            if (monthEnd != null)
            {
                if (monthEnd.Date != null)
                {
                    DateTime currentDate = (DateTime)monthEnd.Date;
                    DateTime currentLastDay = new DateTime(currentDate.Year, currentDate.Month, DateTime.DaysInMonth(currentDate.Year, currentDate.Month));
                    if (monthEnd.Date != currentLastDay) PXUIFieldAttribute.SetWarning<ATPTEFMMonthEnd.date>(e.Cache, monthEnd, ATPTEFMMessages.MonthEndShouldOnlyBeCreatedDuringTheEndOfTheMonth);
                    else PXUIFieldAttribute.SetWarning<ATPTEFMMonthEnd.date>(e.Cache, null, null);
                }
            }
            #endregion

            #region Project Fields Column Configuration Visibility
            PXUIFieldAttribute.SetVisibility<ATPTEFMMonthEndDetail.contractID>(Details.Cache, Details.Current, PXUIVisibility.Invisible);
            PXUIFieldAttribute.SetVisibility<ATPTEFMMonthEndDetail.taskID>(Details.Cache, Details.Current, PXUIVisibility.Invisible);
            #endregion

            //Create New -> Set Fin Period Default
            if (monthEnd.FinPeriodID == null)
            {
                e.Cache.RaiseFieldUpdated<ATPTEFMMonthEnd.date>(monthEnd, null);
            }
        }
        protected virtual void _(Events.FieldUpdated<ATPTEFMMonthEnd, ATPTEFMMonthEnd.date> e)
        {
            CurrencyInfoAttribute.SetEffectiveDate<ATPTEFMMonthEnd.date>(e.Cache, e.Args);
        }

        protected virtual void _(Events.FieldUpdated<ATPTEFMMonthEnd, ATPTEFMMonthEnd.fundID> e)
        {
            ATPTEFMMonthEnd row = e.Row;

            if (row is null) return;

            ATPTEFMFund fund = PXSelect<
                ATPTEFMFund,
                Where<ATPTEFMFund.fundCD, Equal<Required<ATPTEFMFund.fundCD>>>>
                .Select(this, row.FundID);

            if (fund == null) return;

            row.CuryID = fund.CuryID;
            row.CuryInfoID = fund.CuryInfoID;

            CurrencyInfoAttribute.SetEffectiveDate<ATPTEFMMonthEnd.date>(e.Cache, e.Args);
        }
        protected virtual void _(Events.RowPersisting<ATPTEFMMonthEnd> e)
        {
            ATPTEFMMonthEnd monthEnd = e.Row;
            if (monthEnd == null) return;

            if (Details.Select().Count == 0 && e.Operation != PXDBOperation.Delete)
                throw new PXException(ATPTEFMMessages.MonthEndDetailsCannotBeEmpty);
        }
        #endregion

        #region EPApproval Cache Attached
        [PXDBDate()]
        [PXDefault(typeof(ATPTEFMMonthEnd.date), PersistingCheck = PXPersistingCheck.Nothing)]
        protected virtual void EPApproval_DocDate_CacheAttached(PXCache sender)
        {
        }
        #endregion

    }
}