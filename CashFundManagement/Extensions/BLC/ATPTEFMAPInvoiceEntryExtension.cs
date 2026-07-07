using System;
using CashFundManagement.Attributes;
using CashFundManagement.BLC;
using CashFundManagement.Classes;
using CashFundManagement.DAC;
using CashFundManagement.DAC.Setup;
using CashFundManagement.DAC.Unbound;
using CashFundManagement.Extensions.DAC;
using CashFundManagement.Helper;
using CashFundManagement.Messages;
using PX.Common;
using PX.Data;
using PX.Data.BQL;
using PX.Data.BQL.Fluent;
using PX.Objects.AP;
using PX.Objects.CA;
using PX.Objects.EP;
using PX.Objects.GL;
using PX.Objects.GL.FinPeriods;
using PX.Objects.IN;
using PX.Objects.PM;
using PX.Objects.RQ;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using static PX.Objects.TX.CSTaxCalcType;
using AR = PX.Objects.AR;


namespace CashFundManagement.Extensions.BLC
{
    /// <remarks>
    /// 2024-08-14 : Adds multi-tenant support. {RRS}
    /// 2025-04-15 : Rejected PPM bill that came from CA should now be unable to go to On-hold status - 011135 - RFS
    /// </remarks>
    public class ATPTEFMAPInvoiceEntry_Extension : PXGraphExtension<APInvoiceEntry>
    {
#if Version23R2
        public static bool IsActive() => true;
#else
        public static bool IsActive() => ATPTEFMPrefetchSetup.IsActive;
#endif
        #region Constructor
        public override void Initialize()
        {
            base.Initialize();
            Base.OnAfterPersist += OnAfterPersist;
        }
        #endregion

        #region Views
        public PXSetup<ATPTEFMCASetup> ATPTEFMPreferences;
        //public PXSelect<ATPTEFMFeatures> ATPTEFMFeatureSetup;
        public PXSelect<
            ATPTEFMFundTransactionHistoryView,
            Where<ATPTEFMFundTransactionHistoryView.refNbr, Equal<Required<APInvoice.refNbr>>>>
            ATPTEFMTransactionHistoryView;

        public PXSelect<
            ATPTEFMFund,
            Where<ATPTEFMFund.establishmentRefNbr, Equal<Required<APInvoice.refNbr>>>>
            ATPTEFMFund;

        public PXSelect<
            EPExpenseClaimDetails,
            Where<EPExpenseClaimDetails.claimDetailCD, Equal<Required<EPExpenseClaimDetails.claimDetailCD>>>>
            ATPTEFMExpenseReceipt;

        //[PXCopyPasteHiddenView]
        //public PXSelectReadonly<ATPTEFMBudget> ATPTEFMBudget;
        //public PXSelect<ATPTEFMBudgetHistory> ATPTEFMHistory;

        #endregion

        #region View Delegates
        //public virtual IEnumerable aTPTEFMBudget()
        //{
        //    #region Variables
        //    APInvoice doc = Base.Document?.Current;
        //    ATPTEFMAPInvoiceExtension docExt = doc?.GetExtension<ATPTEFMAPInvoiceExtension>();

        //    ATPTEFMFeatures eff = PXSelect<ATPTEFMFeatures>.Select(Base);

        //    APRegister aPRegister = Base.CurrentDocument.Current;
        //    ATPTEFMAPRegisterExt aPRegisterExt = aPRegister.GetExtension<ATPTEFMAPRegisterExt>();

        //    MasterFinPeriod period = PXSelect<MasterFinPeriod,
        //        Where<Current<APInvoice.docDate>, Between<MasterFinPeriod.startDate, MasterFinPeriod.endDate>>>.Select(Base);
        //    ATPTEFMBudgetLibrary.FinPeriodData fData = ATPTEFMBudgetLibrary.GetFinPeriod(Base, doc?.FinPeriodID ?? period.FinPeriodID, eff?.BudgetCalculation);
        //    List<ATPTEFMBudgetLibrary.BudgetParameters> parameterList = new List<ATPTEFMBudgetLibrary.BudgetParameters>();
        //    Company company = PXSelect<Company>.Select(Base);

        //    if (doc == null || fData == null || !(docExt.UsrATPTEFMBudgetEnabled ?? false)) yield break;
        //    #endregion

        //    #region Supply Parameters
        //    foreach (APTran item in Base.Transactions.Select())
        //    {
        //        if (item.AccountID == null || item.SubID == null) continue;
        //        if (item.ProjectID != ProjectDefaultAttribute.NonProject()) continue;
        //        Account account = PXSelect<Account,
        //            Where<Account.accountID, Equal<Required<Account.accountID>>>>.Select(Base, item.AccountID);

        //        APTran invDebitTran = PXSelectJoin<
        //            APTran,
        //            InnerJoin<APRegister,
        //                On<APTran.refNbr, Equal<APRegister.refNbr>,
        //                And<APTran.tranType, Equal<APRegister.docType>>>,
        //            InnerJoin<APAdjust,
        //                On<APAdjust.adjgRefNbr, Equal<APRegister.refNbr>,
        //                And<APAdjust.adjgDocType, Equal<APRegister.docType>>>>>,
        //            Where<APTran.tranType, Equal<APDocType.debitAdj>,
        //                And<APTran.accountID, Equal<@P.AsInt>,
        //                And<APTran.subID, Equal<@P.AsInt>,
        //                And<APRegister.origRefNbr, Equal<P.AsString>,
        //                And<APRegister.status, Equal<APDocStatus.closed>,
        //                And<APTran.lineNbr, Equal<P.AsInt>,
        //                And<APAdjust.adjdRefNbr, Equal<@P.AsString>,
        //                And<APAdjust.adjdDocType, Equal<@P.AsString>,
        //                And<APAdjust.released, Equal<True>>>>>>>>>>>
        //            .Select(Base, item.AccountID, item.SubID, doc.RefNbr, item.LineNbr, doc.RefNbr, doc.DocType);

        //        bool isReversed = invDebitTran != null;

        //        parameterList.Add(new ATPTEFMBudgetLibrary.BudgetParameters()
        //        {
        //            LedgerID = eff?.BudgetLedgerID,
        //            BranchID = item.BranchID,
        //            RefNbr = item.RefNbr,
        //            CuryID = account.CuryID ?? company?.BaseCuryID,
        //            OriginType = ATPTEFMBudgetLibrary.OriginTypes.Bills,
        //            AccountID = item.AccountID,
        //            SubID = item.SubID,
        //            FinYear = fData.FinYear,
        //            FromFinPeriodID = fData.StartPeriod,
        //            ToFinPeriodID = fData.EndPeriod,
        //            Amount = isReversed ? (item.CuryTranAmt - invDebitTran.CuryTranAmt) : item.CuryTranAmt,
        //            Approved = doc.Approved ?? false,
        //        });
        //    }
        //    #endregion

        //    if (!parameterList.Any()) yield break;

        //    bool HasChanges = Base.Caches[typeof(APTran)].GetStatus(Base.Transactions.Current ?? Base.Transactions.Select().FirstOrDefault()) != PXEntryStatus.Notchanged;

        //    foreach (ATPTEFMBudget item in ATPTEFMBudgetLibrary.GenerateBudget(Base, parameterList))
        //    {
        //        yield return item;

        //        #region Is Over Budget
        //        if (item.DocAmt < 0 || item.RequestAmt < 0 || item.BudgetAmt < 0 || item.SpentAmt < 0 || item.ApprovedAmt < 0 || item.UnapprovedAmt < 0) aPRegisterExt.UsrATPTEFMIsOverbudget = true;
        //        else aPRegisterExt.UsrATPTEFMIsOverbudget = false;
        //        #endregion

        //        if (HasChanges)
        //        {
        //            if (item.BudgetAmt < 0 && eff?.BudgetValidation != RQRequestClassBudget.None)
        //            {
        //                this.ATPTEFMBudget.Cache.RaiseExceptionHandling<ATPTEFMBudget.budgetAmt>(item, item.BudgetAmt,
        //                        ATPTEFMHelper.GetPropertyException(item, ATPTEFMMessages.RemainingBudgetMustNotBeLessThanZero,
        //                            eff?.BudgetValidation == RQRequestClassBudget.Warning ? PXErrorLevel.Warning : PXErrorLevel.Error));
        //            }
        //        }
        //    }
        //}
        #endregion

        #region Overrides

        /// <remarks>
        /// 2025-03-13 :When document amount of fund increase/decrease is changed, it should automatically update the doc amount when the credit/debit adjustment is released. - CASE: 010681  {JLTG}
        /// 2025-07-15 :Added filtering criteria based on `transactionType` to ensure correct data retrieval. {JLG}
        /// 2026-03-27 :Refactored to use graph instance saving pattern for explicit persistence. {JLG}
        /// </remarks>
        public delegate IEnumerable ReleaseDelegate(PXAdapter adapter);
        [PXOverride]
        public IEnumerable Release(PXAdapter adapter, ReleaseDelegate baseMethod)
        {
            #region Balance Summary
            APInvoice doc = Base.Document.Current;

            if (doc == null) return baseMethod(adapter);

            if (doc.DocType == APDocType.Invoice)
            {
                PXResultset<ATPTEFMFund, ATPTEFMFundTransactionHistoryView> result = SelectFrom<ATPTEFMFund>
                    .InnerJoin<ATPTEFMFundTransactionHistoryView>
                        .On<ATPTEFMFundTransactionHistoryView.fundRefNbr.IsEqual<ATPTEFMFund.fundCD>>
                    .Where<ATPTEFMFund.establishmentRefNbr.IsEqual<@P.AsString>
                        .And<ATPTEFMFundTransactionHistoryView.refNbr.IsEqual<@P.AsString>>
                        .And<ATPTEFMFundTransactionHistoryView.transactionType.IsEqual<CashFundManagement.BLC.ATPTEFMFundMaint.ATPTEFMTransactionHistoryView.transactionType.establishment>>>
                    .View.SelectSingleBound<PXResultset<ATPTEFMFund, ATPTEFMFundTransactionHistoryView>>(Base, null, doc.RefNbr, doc.RefNbr);

                if (result != null)
                {
                    ATPTEFMFund fund = result;
                    ATPTEFMFundTransactionHistoryView transHistory = result;

                    if (fund == null || transHistory == null) return baseMethod(adapter);

                    ATPTEFMFundSimple fundGraph = PXGraph.CreateInstance<ATPTEFMFundSimple>();
                    fundGraph.Funds.Current = fund;

                    if (doc.IsMigratedRecord ?? false)
                    {
                        transHistory.Status = ATPTEFMFundStatusAttribute.ClosedValue;
                        transHistory.CheckNbr = ATPTEFMMessages.MigratedBill;
                        transHistory.CuryBalanceAmt = doc.CuryDetailExtPriceTotal;
                        fundGraph.Records.Update(transHistory);

                        fund.CuryFundAmt = doc.CuryDetailExtPriceTotal;
                        fund.CuryBalanceAmt = doc.CuryDetailExtPriceTotal;
                        fund.Status = ATPTEFMFundStatusAttribute.ActiveValue;
                        fund.IsActive = true;
                        fundGraph.Funds.Update(fund);
                    }
                    else
                    {
                        transHistory.Status = ATPTEFMFundStatusAttribute.OpenValue;
                        fundGraph.Records.Update(transHistory);

                        fund.CuryFundAmt = doc.CuryDetailExtPriceTotal;
                        fundGraph.Funds.Update(fund);
                    }

                    fundGraph.Save.Press();
                }
            }
            else if (doc.DocType == APDocType.CreditAdj || doc.DocType == APDocType.DebitAdj)
            {
                ATPTEFMTransactionHistoryView.Current = ATPTEFMTransactionHistoryView.Select(doc.RefNbr);
                if (ATPTEFMTransactionHistoryView.Current != null)
                {
                    ATPTEFMTransactionHistoryView.Current.Status = ATPTEFMFundStatusAttribute.OpenValue;
                    ATPTEFMTransactionHistoryView.Current.CuryFundTransactionDocumentAmt = doc.CuryDetailExtPriceTotal;
                    ATPTEFMTransactionHistoryView.UpdateCurrent();
                }
            }
            #endregion

            return baseMethod(adapter);
        }

        public delegate IEnumerable PayInvoiceDelegate(PXAdapter adapter);
        [PXOverride]
        public IEnumerable PayInvoice(PXAdapter adapter, PayInvoiceDelegate baseMethod)
        {
            CurrencyValidation();
            return baseMethod(adapter);
        }

        public delegate IEnumerable VendorRefundDelegate(PXAdapter adapter);
        [PXOverride]
        public IEnumerable VendorRefund(PXAdapter adapter, VendorRefundDelegate baseMethod)
        {
            CurrencyValidation();
            return baseMethod(adapter);
        }
        #endregion

        #region Actions
        /// <remarks>
        /// 2025-09-23 : CFM 2024R1: Debit Adj [Transaction Doc Hyperlink] 013663 {JLG} <br/>
        /// </remarks>
        public PXAction<APInvoice> ATPTEFMViewFundTransaction;
        [PXUIField(DisplayName = ATPTEFMMessages.ViewFundRefNbr, Visible = false, Enabled = true, MapEnableRights = PXCacheRights.Select,
            MapViewRights = PXCacheRights.Select)]
        [PXLookupButton]

        protected virtual IEnumerable aTPTEFMViewFundTransaction(PXAdapter adapter)
        {
            APRegister aPRegister = Base.CurrentDocument.Current;
            ATPTEFMAPRegisterExt aPRegisterExt = aPRegister.GetExtension<ATPTEFMAPRegisterExt>();
            ExpenseClaimEntry graph = PXGraph.CreateInstance<ExpenseClaimEntry>();

            graph.ExpenseClaim.Current = PXSelect<
                EPExpenseClaim,
                Where<EPExpenseClaim.refNbr, Equal<Required<EPExpenseClaim.refNbr>>>>
                .Select(Base, aPRegister.OrigRefNbr);

            if (graph.ExpenseClaim.Current != null)
                throw new PXPopupRedirectException(graph, ATPTEFMMessages.ATPTEFMFundTransaction, true);
            else
            {
                graph.ExpenseClaim.Current = PXSelect<
                        EPExpenseClaim,
                        Where<ATPTEFMEPExpenseClaimExt.usrATPTEFMRFPReqRef, Equal<Required<ATPTEFMEPExpenseClaimExt.usrATPTEFMRFPReqRef>>>>
                        .Select(Base, aPRegisterExt.UsrATPTEFMLiqNbr);

                if (graph.ExpenseClaim.Current != null)
                    throw new PXPopupRedirectException(graph, ATPTEFMMessages.ATPTEFMFundTransaction, true);
            }

            return adapter.Get();
        }

        public PXAction<APInvoice> ATPTEFMViewDocuments;
        [PXButton]
        [PXUIField(Visible = false)]
        protected virtual IEnumerable aTPTEFMViewDocuments(PXAdapter adapter)
        {
            APRegister aPRegister = Base.CurrentDocument.Current;
            ATPTEFMAPRegisterExt aPRegisterExt = aPRegister.GetExtension<ATPTEFMAPRegisterExt>();

            if (aPRegister is null) return adapter.Get();

            if (aPRegisterExt.UsrATPTEFMSourceType == ATPTEFMSourceTypeAttribute.Funds)
            {
                ATPTEFMFundMaint graph = PXGraph.CreateInstance<ATPTEFMFundMaint>();

                var document = graph.Document.Search<ATPTEFMFund.fundCD>(aPRegisterExt.UsrATPTEFMSourceRef);

                if (document != null)
                    throw new PXPopupRedirectException(graph, ATPTEFMMessages.ATPTEFMFund, true);
            }
            if (aPRegisterExt.UsrATPTEFMSourceType == ATPTEFMSourceTypeAttribute.CashAdvance)
            {
                ATPTEFMCashAdvanceEntry graph = PXGraph.CreateInstance<ATPTEFMCashAdvanceEntry>();
                graph.Clear();

                ATPTEFMCashAdvance ca = PXSelect<
                    ATPTEFMCashAdvance,
                    Where<ATPTEFMCashAdvance.cashAdvanceNbr, Equal<Required<ATPTEFMCashAdvance.cashAdvanceNbr>>>>
                    .Select(Base, aPRegisterExt.UsrATPTEFMSourceRef);

                if (ca != null)
                {
                    graph.CashAdvances.Current = ca;

                    throw new PXPopupRedirectException(graph, ATPTEFMMessages.ATPTEFMCashAdvance, true);
                }
            }

            return adapter.Get();
        }

        /// <remarks>
        /// 2025-06-03 :Allow Cash Advance to create new prepayment if the previous one is deleted: 011686 {JLG} <br/>
        /// </remarks>
        public delegate IEnumerable VoidDocumentDelegate(PXAdapter adapter);
        [PXOverride]
        public IEnumerable VoidDocument(PXAdapter adapter, VoidDocumentDelegate baseMethod)
        {
            APInvoice doc = Base.Document.Current;
            baseMethod(adapter);

            UnlinkCashAdvanceFromBill(doc);

            return baseMethod(adapter);
        }

        public delegate APInvoiceState GetDocumentStateDelegate(PXCache cache, APInvoice document);
        [PXOverride]
        public APInvoiceState GetDocumentState(PXCache cache, APInvoice document, GetDocumentStateDelegate baseMethod)
        {
            APInvoiceState baseState = baseMethod(cache, document);

            if (document.OrigModule == "EP")
                baseState.DontApprove = !Base.IsApprovalRequired(document);

            return baseState;
        }
        #endregion

        #region Events
        //protected virtual void _(Events.RowUpdated<APInvoice> e, PXRowUpdated baseEvent)
        //{
        //    if(baseEvent != null) baseEvent(e.Cache, e.Args);

        //    APInvoice row = e.Row;
        //    if (row == null) return;
        //    ATPTEFMAPInvoiceExtension rowExt = row.GetExtension<ATPTEFMAPInvoiceExtension>();

        //    if(row.CreatedByScreenID != "AP301000" && (rowExt.UsrATPTEFMBudgetEnabled ?? false))
        //        rowExt.UsrATPTEFMBudgetEnabled = false;
        //}
        //protected virtual void _(Events.FieldDefaulting<APInvoice, ATPTEFMAPInvoiceExtension.usrATPTEFMBudgetEnabled> e)
        //{
        //    APInvoice row = e.Row;
        //    if (row == null) return;
        //    ATPTEFMAPInvoiceExtension rowExt = row.GetExtension<ATPTEFMAPInvoiceExtension>();

        //    ATPTEFMFeatures eff = PXSelect<ATPTEFMFeatures>.Select(Base);

        //    if (ATPTEFMBudgetLibrary.BudgetVisible(eff, "B") && rowExt.UsrATPTEFMBudgetEnabled == null && row.OrigRefNbr == null && (row.DocType == APDocType.Invoice || row.DocType == APDocType.CreditAdj))
        //    {
        //        e.NewValue = true;
        //    }
        //    else if ((!ATPTEFMBudgetLibrary.BudgetVisible(eff, "B")) && rowExt.UsrATPTEFMBudgetEnabled == null && row.OrigRefNbr == null && (row.DocType == APDocType.Invoice || row.DocType == APDocType.CreditAdj))
        //    {
        //        e.NewValue = false;
        //    }
        //}
        protected virtual void _(Events.RowInserting<APInvoice> e, PXRowInserting baseEvent)
        {
            if (baseEvent != null) baseEvent(e.Cache, e.Args);

            APInvoice row = (APInvoice)e.Row;
            if (row == null) return;
            ATPTEFMAPInvoiceExtension rowExt = row.GetExtension<ATPTEFMAPInvoiceExtension>();

            //Reverse Invoice scenario
            if (row.OrigRefNbr != null && (rowExt.UsrATPTEFMBudgetEnabled ?? false))
                rowExt.UsrATPTEFMBudgetEnabled = false;
        }
        // <remarks>
        /// 2024-02-13 :Disable fields based on document status for expense claim bills - CASE: 007977 {JLTG}
        /// </remarks>
        protected virtual void _(Events.RowSelected<APInvoice> e, PXRowSelected baseMethod)
        {
            if (baseMethod != null) baseMethod(e.Cache, e.Args);

            APInvoice row = (APInvoice)e.Row;
            if (row == null) return;


            ATPTEFMAPRegisterExt invExt = row.GetExtension<ATPTEFMAPRegisterExt>();
            ATPTEFMAPInvoiceExtension rowExt = row.GetExtension<ATPTEFMAPInvoiceExtension>();

            bool IsMigration = ATPTEFMPreferences.Current.IsCashAdvanceMigration ?? false;

            PXUIFieldAttribute.SetEnabled<ATPTEFMAPRegisterExt.usrATPTEFMSourceType>(e.Cache, null, IsMigration);
            PXUIFieldAttribute.SetEnabled<ATPTEFMAPRegisterExt.usrATPTEFMSourceRef>(e.Cache, null, IsMigration);

            //#region Budget Visibility
            //bool ShowBudget = rowExt.UsrATPTEFMBudgetEnabled ?? false;
            //ATPTEFMBudget.AllowSelect = ShowBudget;
            //#endregion

            PXUIFieldAttribute.SetRequired<APInvoice.invoiceNbr>(e.Cache, IsInvoiceNbrRequired(row));

            if (IsFromCfmBill(row))
                SetAPInvoiceFieldStates(e.Cache, row);

            Base.putOnHold.SetEnabled(!(row.Status == APDocStatus.Rejected && row.DocType == APDocType.Prepayment && invExt.UsrATPTEFMSourceType == ATPTEFMSourceTypeAttribute.CashAdvance));
        }

        protected virtual void _(Events.RowSelected<APTran> e, PXRowSelected baseMethod)
        {
            if (baseMethod != null) baseMethod(e.Cache, e.Args);

            var tran = e.Row;
            if (tran == null) return;

            APInvoice invoice = Base.Document.Current;
            if (invoice == null) return;

            ATPTEFMAPRegisterExt invExt = invoice.GetExtension<ATPTEFMAPRegisterExt>();

            if (IsFromCfmBill(invoice))
            {
                bool canEdit = CanEditDocument(invoice);
                if (!canEdit)
                {
                    PXUIFieldAttribute.SetEnabled(e.Cache, tran, false);
                    Base.Transactions.Cache.AllowDelete = false;
                    Base.Transactions.Cache.AllowInsert = false;
                }
            }

            bool isFundEstablishmentBill = invExt.UsrATPTEFMIsFundEstablishmentBill ?? false;
            bool isCaPrepaymentBill = invExt.UsrATPTEFMIsCaPrepaymentBill ?? false;
            bool isCloseFundDebitAdjBill = invExt.UsrATPTEFMIsCloseFundDebitAdjBill ?? false;

            if (isFundEstablishmentBill
                || isCaPrepaymentBill
                || isCloseFundDebitAdjBill)
            {
                Base.Transactions.Cache.AllowDelete = false;
                Base.Transactions.Cache.AllowInsert = false;
            }
        }
        protected virtual void _(Events.RowPersisted<APInvoice> e)
        {
            APInvoice row = (APInvoice)e.Row;

            if (row is null) return;
            #region Funds Update
            #region Transaction History Establishment
            ATPTEFMTransactionHistoryView.Current = PXSelect<
                        ATPTEFMFundTransactionHistoryView,
                        Where<ATPTEFMFundTransactionHistoryView.refNbr, Equal<Required<ATPTEFMFundTransactionHistoryView.refNbr>>,
                        And<ATPTEFMFundTransactionHistoryView.transactionType, Equal<CashFundManagement.BLC.ATPTEFMFundMaint.ATPTEFMTransactionHistoryView.transactionType.establishment>>>>
                        .Select(Base, row.RefNbr);

            if (ATPTEFMTransactionHistoryView.Current != null && row.IsMigratedRecord != true)
            {
                if (ATPTEFMTransactionHistoryView.Current.Status != ATPTEFMFundStatusAttribute.OpenValue)
                {
                    ATPTEFMTransactionHistoryView.Current.Status = row.Status;
                    ATPTEFMTransactionHistoryView.UpdateCurrent();
                }
            }
            #endregion
            #endregion
        }

        /// <remarks>
        /// 2025-03-07 : Throw an exception if detail total is not equal to CA requested amount or Fund initital amount Case: 010639  {JLG} <br/>
        /// </remarks>
        protected virtual void _(Events.RowPersisting<APInvoice> e, PXRowPersisting baseMethod)
        {
            if (baseMethod != null) baseMethod(e.Cache, e.Args);

            APInvoice document = e.Row;
            if (document is null) return;

            ATPTEFMAPRegisterExt invExt = document.GetExtension<ATPTEFMAPRegisterExt>();

            if (invExt is null) return;

            if (string.IsNullOrEmpty(document.InvoiceNbr) && IsInvoiceNbrRequired(document))
            {
                e.Cache.RaiseExceptionHandling<APInvoice.invoiceNbr>(document, null, ATPTEFMHelper.GetPropertyException(document, ErrorMessages.FieldIsEmpty, PXErrorLevel.Error));
            }

            #region CA Prepayment and Fund Establishment Bill Validation
            bool isFundEstablishmentBill = invExt.UsrATPTEFMIsFundEstablishmentBill ?? false;
            bool isCaPrepaymentBill = invExt.UsrATPTEFMIsCaPrepaymentBill ?? false;
            bool isCloseFundDebitAdjBill = invExt.UsrATPTEFMIsCloseFundDebitAdjBill ?? false;

            if (isFundEstablishmentBill)
            {
                ATPTEFMFund fund = PXSelect<ATPTEFMFund, Where<ATPTEFMFund.establishmentRefNbr, Equal<Required<ATPTEFMFund.establishmentRefNbr>>>>.Select(Base, document.RefNbr);

                if (fund is null) return;

                if (fund.CuryInitialFund != document.CuryDocBal)
                    throw new Exception(string.Format(ATPTEFMMessages.DetailTotalShouldBeEqualToDocumentTotal, "Initial Fund"));
            }

            if (isCaPrepaymentBill)
            {
                ATPTEFMCashAdvance cashadvance = PXSelect<ATPTEFMCashAdvance, Where<ATPTEFMCashAdvance.billRefNbr, Equal<Required<ATPTEFMCashAdvance.billRefNbr>>>>.Select(Base, document.RefNbr);
                if (cashadvance is null) return;

                if (cashadvance.CuryRequestedAmount != document.CuryDocBal && document.Released != true)
                    throw new Exception(string.Format(ATPTEFMMessages.DetailTotalShouldBeEqualToDocumentTotal, "CA"));
            }

            if (isCloseFundDebitAdjBill)
            {
                ATPTEFMFund fund = PXSelect<ATPTEFMFund, Where<ATPTEFMFund.closeFundRefNbr, Equal<Required<ATPTEFMFund.closeFundRefNbr>>>>.Select(Base, document.RefNbr);

                if (fund is null) return;

                if (fund.CuryFundAmt != document.CuryDocBal)
                    throw new Exception(string.Format(ATPTEFMMessages.DetailTotalShouldBeEqualToDocumentTotal, "Fund"));
            }
            #endregion
        }

        /// <remarks>
        /// 2024-11-07 : Bypass standard error implementation on duplicate invoice number. <see cref="APVendorRefNbrAttribute"/>. {RRS}  <br/>
        /// 2024-12-09 : Skip validation for reversed documents Case: 008995 {JLG} <br/>
        /// 2025-03-03 : Duplicate on Vendor Ref [Liquidation Bill] and skip validation for reversed documents Case: 010583 {JLG} <br/>
        /// 2025-05-20 : (CFM2024R2)Fund Decrease> Error during the reversal of the Debit Adj bill when 'Raise an error on Duplicate Vendor Reference Number' is selected. Case: 011621 {JLG} <br/>
        /// </remarks>
        protected virtual void _(Events.FieldVerifying<APInvoice, APInvoice.invoiceNbr> e)
        {
            var row = e.Row;
            if (row == null) return;
            if (Base.APSetup.Current.RaiseErrorOnDoubleInvoiceNbr ?? false)
            {
                if (row.DocType.Equals(APDocType.DebitAdj) || row.DocType.Equals(APDocType.CreditAdj)) return;
                if (row.InstallmentNbr != null) return;

                APInvoice duplicateInvoiceRefNbr = SelectFrom<APInvoice>.Where<APInvoice.vendorID.IsEqual<@P.AsInt>.
                    And<APInvoice.invoiceNbr.IsEqual<@P.AsString>>>.View.Select(Base, row.VendorID, e.NewValue.ToString());
                if (duplicateInvoiceRefNbr != null)
                {
                    string message = PXMessages.LocalizeFormatNoPrefixNLA(AR.Messages.EntityDuplicateInvoiceNbr, duplicateInvoiceRefNbr.InvoiceNbr, duplicateInvoiceRefNbr.RefNbr);
                    throw ATPTEFMHelper.GetPropertyException(row, message, PXErrorLevel.Error);
                }
            }
        }

        private List<APInvoice> _deletedInvoice = new List<APInvoice>();
        protected virtual void _(Events.RowDeleting<APInvoice> e, PXRowDeleting baseEvent)
        {
            if (baseEvent != null) baseEvent(e.Cache, e.Args);

            if (e.Row != null)
            {
                _deletedInvoice.Add(e.Row);
            }
        }

        #endregion

        #region Methods
        /// <remarks>
        /// 2025-06-03 :Allow Cash Advance to create new prepayment if the previous one is deleted: 011686 {JLG} <br/>
        /// 2025-12-09 : Used a simple graph for updating CA WHT field to skip heavy validation of CA transaction. 014607 {RRS}<br/>
        /// 2026-02-04 : Remove Base.expenseclaim.Current != null condition as it is sometimes resulting to null record. 014974 {RRS}<br/>
        /// </remarks>
        private void OnAfterPersist(PXGraph obj)
        {
            #region CA WHT Updates
            ATPTEFMCASimple caGraph = PXGraph.CreateInstance<ATPTEFMCASimple>();
            EPExpenseClaim claim = Base.expenseclaim.Select();
            ATPTEFMEPExpenseClaimExt claimExt = claim.GetExtension<ATPTEFMEPExpenseClaimExt>();

            if (claim != null && claimExt.UsrATPTEFMTranType.Equals(ATPTEFMExpenseTypeAttribute.Liquidation))
            {
                HashSet<string> listOfLiquidationRef = new HashSet<string>();
                ATPTEFMCashAdvance ca = PXSelectJoin<
                    ATPTEFMCashAdvance,
                    InnerJoin<ATPTEFMCAReceiptDetail,
                        On<ATPTEFMCAReceiptDetail.cashAdvanceNbr, Equal<ATPTEFMCashAdvance.cashAdvanceNbr>>>,
                    Where<ATPTEFMCAReceiptDetail.liquidationRef, Equal<Required<ATPTEFMCAReceiptDetail.liquidationRef>>>>
                    .Select(Base, claimExt.UsrATPTEFMLiqNbr);

                if (ca != null)
                {
                    caGraph.CashAdvances.Current = ca;
                    var liquidationRefNbrs = caGraph.CashAdvanceReceiptLines.Select().RowCast<ATPTEFMCAReceiptDetail>().Where(r => !string.IsNullOrEmpty(r.LiquidationRef)).Select(r => r.LiquidationRef).ToList();
                    listOfLiquidationRef.AddRange(liquidationRefNbrs);

                    decimal? totalWht = decimal.Zero;

                    foreach (var liquidatioNRefNbr in listOfLiquidationRef)
                    {
                        foreach (APInvoice apInvoice in PXSelectJoin<
                            APInvoice,
                            InnerJoin<APRegister,
                                On<APRegister.refNbr, Equal<APInvoice.refNbr>,
                                And<APRegister.docType, Equal<APInvoice.docType>>>>,
                            Where<APInvoice.docType, Equal<APDocType.invoice>,
                                And<ATPTEFMAPRegisterExt.usrATPTEFMLiqNbr, Equal<Required<ATPTEFMAPRegisterExt.usrATPTEFMLiqNbr>>>>>
                            .Select(Base, liquidatioNRefNbr))
                        {
                            if (apInvoice != null)
                            {
                                APInvoice reversedDoc = PXSelect<
                                    APInvoice,
                                    Where<APInvoice.origRefNbr, Equal<@P.AsString>,
                                        And<APInvoice.docType, Equal<APDocType.debitAdj>,
                                        And<APInvoice.status, Equal<APDocStatus.closed>>>>>
                                    .Select(Base, apInvoice.RefNbr);

                                if (reversedDoc == null)
                                    totalWht += apInvoice.CuryOrigWhTaxAmt;
                            }
                        }
                    }

                    ca.CuryWhtTaxAmount = totalWht;
                    ca.WhtTaxAmount = totalWht;
                    caGraph.CashAdvances.Update(ca);
                    caGraph.Save.Press();
                }
            }

            #endregion

            #region Delete Invoice (Replenishment and CA)
            if (_deletedInvoice.Any())
            {
                APInvoice invoice = _deletedInvoice.FirstOrDefault();

                if (invoice != null)
                {
                    #region Replenishment
                    UnlinkReplenishmentFromInvoice(invoice);
                    #endregion

                    #region Cash Advance
                    UnlinkCashAdvanceFromBill(invoice);
                    #endregion

                    #region Fund
                    UnlinkFundEstablishmentFromBill(invoice);
                    #endregion
                }

                _deletedInvoice.Clear();
            }
            #endregion
        }

        /// <remarks>
        /// 2024-10-14 : Enhancement: Update summary area CA WHT field if liquidation bill contains withholding tax - CASE: 008137 {JLG}
        /// 2025-01-13 : 009548 - [BK] Withholding tax field concern in the Cash Advance - Do not include reversed bills in the wht calculation
        /// 2025-04-10 : Commented this code for new implementation of overriding persist method. {JLTG}
        /// </remarks>
        /*public delegate void PersistDelegate();
        [PXOverride]
        public void Persist(PersistDelegate baseMethod)
        {
            APInvoice doc = Base.Document.Current;
            ATPTEFMAPInvoiceExtension docExt = doc.GetExtension<ATPTEFMAPInvoiceExtension>();

            ATPTEFMFeatures eff = PXSelect<ATPTEFMFeatures>.Select(Base);
            MasterFinPeriod period = PXSelect<MasterFinPeriod,
                   Where<Current<EPExpenseClaim.docDate>, Between<MasterFinPeriod.startDate, MasterFinPeriod.endDate>>>.Select(Base);

            #region Budget Validation

            //(bool, bool) validateBudget = (docExt?.UsrATPTEFMBudgetEnabled ?? false, ATPTEFMProjectBudgetLibrary.ProjectBudgetVisible(eff, "B"));
            //bool BudgetValidate = (eff?.BudgetValidation ?? RQRequestClassBudget.None) == RQRequestClassBudget.Error;

            //if (BudgetValidate && (validateBudget.Item1 || validateBudget.Item2))
            //{
            //    using (PXTransactionScope ts = new PXTransactionScope())
            //    {
            //        //(bool, bool) isOverbudget = (
            //        //    ATPTEFMBudget?.Select()?.RowCast<ATPTEFMBudget>()?.ToList()?.Any(x => (x.BudgetAmt ?? 0m) < 0) ?? false,
            //        //    ATPTEFMProjectBudget?.Select()?.RowCast<ATPTEFMPBudget>()?.ToList()?.Any(x => (x.BudgetAmt ?? 0m) < 0) ?? false
            //        //);
            //        (bool, bool) isOverbudget = (
            //            ATPTEFMBudget?.Select()?.RowCast<ATPTEFMBudget>()?.ToList()?.Any(x => (x.BudgetAmt ?? 0m) < 0) ?? false,
            //            false
            //        );

            //        if (validateBudget.Item1)
            //        {
            //            if (isOverbudget.Item1)
            //                throw new PXRowPersistedException(typeof(ATPTEFMBudget).Name, ts, ATPTEFMMessages.CheckBudget);
            //            ATPTEFMBudget.Cache.Persist(PXDBOperation.Insert);
            //            ATPTEFMBudget.Cache.Persist(PXDBOperation.Update);
            //        }
            //        //if (validateBudget.Item2)
            //        //{
            //        //    foreach (ATPTEFMPBudget row in ATPTEFMProjectBudget.Select())
            //        //    {
            //        //        if (row.ProjectID == null || row.ProjectTaskID == null || row.CostCodeID == null) continue;

            //        //        ATPTEFMProjectBudgetLineSummary PBSummary = ATPTEFMProjectBudgetSummary.Select(row.ProjectID, row.ProjectTaskID, row.CostCodeID, period.FinYear.ToString());

            //        //        if (PBSummary == null)
            //        //            throw new PXRowPersistedException(typeof(ATPTEFMPBudget).Name, ts, ATPTEFMMessages.NotInProjectBudget);
            //        //    }

            //        //    if (isOverbudget.Item2)
            //        //        throw new PXRowPersistedException(typeof(ATPTEFMPBudget).Name, ts, ATPTEFMMessages.CheckProjectBudget);
            //        //    ATPTEFMProjectBudget.Cache.Persist(PXDBOperation.Insert);
            //        //    ATPTEFMProjectBudget.Cache.Persist(PXDBOperation.Update);
            //        //}

            //        ts.Complete(Base);
            //    }
            //    if (validateBudget.Item1) ATPTEFMBudget.Cache.Persisted(false);
            //    //if (validateBudget.Item2) ATPTEFMProjectBudget.Cache.Persisted(false);
            //}

            #endregion

            #region Budget Requirements
            //List<ATPTEFMBudget> BudgetList = new List<ATPTEFMBudget>();
            //List<ATPTEFMPBudget> PBudgetList = new List<ATPTEFMPBudget>();

            //ATPTEFMBudgetEntry graph = PXGraph.CreateInstance<ATPTEFMBudgetEntry>();
            //bool isDeleted = Base.Document.Cache.Deleted.Any_() ? true : false;
            //APInvoice curRecord = isDeleted ? Base.Document.Cache.Deleted.FirstOrDefault_() as APInvoice : Base.Document.Current;
            //bool isCancelled = curRecord == null ? false : curRecord.Status == APDocStatus.Rejected ? true : false;

            //List<APTran> curLines = new List<APTran>();
            //foreach (APTran item in Base.Transactions.Cache.Deleted) { curLines.Add(item); }
            #endregion

            baseMethod();

            #region BudgetHistory
            //if (isDeleted || isCancelled)
            //{
            //    foreach (APTran item in curLines)
            //    {
            //        var row = new ATPTEFMBudget();
            //        row.AcctID = item.AccountID;
            //        row.SubID = item.SubID;
            //        row.RefNbr = item.RefNbr;
            //        row.Origin = (int)ATPTEFMBudgetLibrary.OriginTypes.Bills;
            //        BudgetList.Add(row);

            //        var pRow = new ATPTEFMPBudget();
            //        pRow.ProjectID = item.ProjectID;
            //        pRow.ProjectTaskID = item.TaskID;
            //        pRow.CostCodeID = item.CostCodeID;
            //        pRow.RefNbr = item.RefNbr;
            //        pRow.Origin = (int)ATPTEFMBudgetLibrary.OriginTypes.Bills;
            //        PBudgetList.Add(pRow);
            //    }
            //    graph.DeleteBudgetHistory(BudgetList);
            //    graph.DeletePBudgetHistory(PBudgetList);
            //}
            //else
            //{
            //    if (docExt?.UsrATPTEFMBudgetEnabled ?? false)
            //    {
            //        //Inserts row with null AcctID and SubID; causes error
            //        //BudgetList.Add(new ATPTEFMBudget() { RefNbr = curRecord.RefNbr, Origin = (int)ATPTEFMBudgetLibrary.OriginTypes.Bills });
            //        foreach (ATPTEFMBudget item in ATPTEFMBudget.Select())
            //        {
            //            var row = item;
            //            row.IsApproved = curRecord.Approved ?? false;
            //            BudgetList.Add(row);
            //        }
            //        graph.AddBudgetHistory(BudgetList);
            //    }

            //    //if (ATPTEFMProjectBudgetLibrary.ProjectBudgetVisible(ATPTEFMFeatureSetup.Current, "B"))
            //    //{
            //    //    PBudgetList.Add(new ATPTEFMPBudget() { RefNbr = curRecord.RefNbr, Origin = (int)ATPTEFMBudgetLibrary.OriginTypes.Bills });
            //    //    foreach (ATPTEFMPBudget item in ATPTEFMProjectBudget.Select())
            //    //    {
            //    //        var row = item;
            //    //        row.IsApproved = curRecord.Approved ?? false;
            //    //        PBudgetList.Add(row);
            //    //    }
            //    //    graph.AddPBudgetHistory(PBudgetList);
            //    //}
            //}
            //ATPTEFMBudget.Select();
            #endregion

            #region CA WHT Updates
            ATPTEFMCashAdvanceEntry caGraph = PXGraph.CreateInstance<ATPTEFMCashAdvanceEntry>();
            EPExpenseClaim claim = Base.expenseclaim.Select();
            ATPTEFMEPExpenseClaimExt claimExt = claim.GetExtension<ATPTEFMEPExpenseClaimExt>();

            if (claim != null && claimExt.UsrATPTEFMTranType.Equals(ATPTEFMExpenseTypeAttribute.Liquidation))
            {
                HashSet<string> listOfLiquidationRef = new HashSet<string>();
                ATPTEFMCashAdvance ca = PXSelectJoin<ATPTEFMCashAdvance, InnerJoin<ATPTEFMCAReceiptDetail,
                    On<ATPTEFMCAReceiptDetail.cashAdvanceNbr, Equal<ATPTEFMCashAdvance.cashAdvanceNbr>>>,
                    Where<ATPTEFMCAReceiptDetail.liquidationRef, Equal<Required<ATPTEFMCAReceiptDetail.liquidationRef>>>>.Select(Base, claimExt.UsrATPTEFMLiqNbr);

                if (ca != null)
                {
                    caGraph.CashAdvances.Current = ca;
                    var liquidationRefNbrs = caGraph.CashAdvanceReceiptLines.Select().RowCast<ATPTEFMCAReceiptDetail>().Where(r => !string.IsNullOrEmpty(r.LiquidationRef)).Select(r => r.LiquidationRef).ToList();
                    listOfLiquidationRef.AddRange(liquidationRefNbrs);

                    decimal? totalWht = decimal.Zero;

                    foreach (var liquidatioNRefNbr in listOfLiquidationRef)
                    {
                        foreach (APInvoice apInvoice in PXSelectJoin<
                            APInvoice,
                            InnerJoin<APRegister,
                                On<APRegister.refNbr, Equal<APInvoice.refNbr>,
                                And<APRegister.docType, Equal<APInvoice.docType>>>>,
                            Where<APInvoice.docType, Equal<APDocType.invoice>,
                                And<ATPTEFMAPRegisterExt.usrATPTEFMLiqNbr, Equal<Required<ATPTEFMAPRegisterExt.usrATPTEFMLiqNbr>>>>>
                            .Select(Base, liquidatioNRefNbr))
                        {
                            if (apInvoice != null)
                            {
                                APInvoice reversedDoc = PXSelect<
                                    APInvoice,
                                    Where<APInvoice.origRefNbr, Equal<@P.AsString>,
                                        And<APInvoice.docType, Equal<APDocType.debitAdj>,
                                        And<APInvoice.status, Equal<APDocStatus.closed>>>>>
                                    .Select(Base, apInvoice.RefNbr);

                                if (reversedDoc == null)
                                    totalWht += apInvoice.CuryOrigWhTaxAmt;
                            }
                        }
                    }

                    ca.CuryWhtTaxAmount = totalWht;
                    ca.WhtTaxAmount = totalWht;
                    caGraph.CashAdvances.Update(ca);
                    caGraph.Save.Press();
                }
            }
            #endregion
        }*/

        public delegate IEnumerable viewOriginalDocumentDelegate(PXAdapter adapter);
        /// <remarks>
        /// 2024-09-11 : "Fund Number Hyperlink added" - CASE: 007500 {JLG} <br />
        /// 2025-01-07 : "Cash Advance Hyperlink added" - CASE: 009427 {RRS} <br/>
        /// 2025-06-09 : Fix hyperlink for Reversed RFP Bills - CASE: 011811 {RRS} <br/>
        /// </remarks>
        [PXOverride]
        public IEnumerable viewOriginalDocument(PXAdapter adapter, viewOriginalDocumentDelegate baseMethod)
        {
            APInvoice doc = Base.Document.Current;


            if (doc?.OrigDocType == null && doc?.OrigRefNbr != null && doc?.OrigModule == BatchModule.EP)
            {
                var replenishment = ATPTEFMReplenishment.PK.Find(Base, doc.OrigRefNbr);


                if (replenishment != null)
                {
                    var graph = PXGraph.CreateInstance<ATPTEFMReplenishmentEntry>();
                    graph.Replenishments.Current = replenishment;

                    PXRedirectHelper.TryRedirect(graph);
                }
            }

            if (doc?.OrigDocType == null && doc?.OrigRefNbr != null)
            {
                switch (doc?.CreatedByScreenID)
                {
                    case "ATPT2012":
                        var fund = PXSelect<
                            ATPTEFMFund,
                            Where<ATPTEFMFund.fundCD, Equal<Required<ATPTEFMFund.fundCD>>>>
                            .Select(Base, doc.OrigRefNbr);
                        if (fund != null)
                        {
                            var graph = PXGraph.CreateInstance<ATPTEFMFundMaint>();
                            graph.Document.Current = fund;
                            PXRedirectHelper.TryRedirect(graph);
                        }
                        break;
                    case "ATPT3103":
                        var cashAdvance = ATPTEFMCashAdvance.UK.Find(Base, doc.OrigRefNbr);
                        if (cashAdvance != null)
                        {
                            PXTrace.WriteInformation($"Cash Advance: {cashAdvance.CashAdvanceNbr}");
                            var graph = PXGraph.CreateInstance<ATPTEFMCashAdvanceEntry>();
                            graph.CashAdvances.Current = cashAdvance;
                            PXRedirectHelper.TryRedirect(graph);
                        }
                        break;
                    default:
                        break;
                }
            }

            // Fix issue for Debit Adjustment from RFP Bills and/or fund bills. 011811 {RRS}
            #region 011811
            if (doc.DocType.Equals(APDocType.DebitAdj)
                    && doc.OrigModule.Equals(BatchModule.EP)
                    && doc.OrigDocType.Equals(APDocType.Invoice))
            {
                APInvoice sourceInvoice = APInvoice.PK.Find(Base, doc.OrigDocType, doc.OrigRefNbr);
                if (sourceInvoice != null)
                {
                    doc.OrigModule = BatchModule.AP;
                    Base.Document.Update(doc);
                    Base.Save.Press();
                }
            }
            #endregion 011811

            return baseMethod(adapter);
        }
        protected virtual bool IsInvoiceNbrRequired(APInvoice doc)
        {
            return Base.APSetup.Current.RequireVendorRef == true
                && doc.DocType != APDocType.DebitAdj
                && doc.DocType != APDocType.CreditAdj
                && doc.DocType != APDocType.Prepayment
                && (Base.vendor.Current == null
                || Base.vendor.Current.TaxAgency == false);
        }
        protected void CurrencyValidation()
        {
            APInvoice doc = Base.Document.Current;
            if (doc != null && (doc.GetExtension<ATPTEFMAPRegisterExt>().UsrATPTEFMSourceType == ATPTEFMSourceTypeAttribute.Funds || doc.GetExtension<ATPTEFMAPRegisterExt>().UsrATPTEFMIsFromReplenishment == true))
            {
                // Get the cash account currency
                CashAccount cashAccount = PXSelect<
                    CashAccount,
                    Where<CashAccount.cashAccountID, Equal<Required<CashAccount.cashAccountID>>>>
                    .Select(Base, doc.PayAccountID);

                if (cashAccount != null && doc.CuryID != cashAccount.CuryID)
                {
                    throw new PXException(Messages.ATPTEFMMessages.CurrencyMismatch, doc.CuryID, cashAccount.CuryID);
                }
            }
        }
        protected virtual bool IsFromCfmBill(APInvoice doc)
        {
            return doc.CreatedByScreenID == "EP301000"
                || doc.CreatedByScreenID == "ATPT3012"
                || doc.CreatedByScreenID == "ATPT2012"
                || doc.CreatedByScreenID == "ATPT3103"
                || doc.CreatedByScreenID == "ATPT5016"
                || doc.CreatedByScreenID == "ATPT5017";
        }

        /// <summary>
        /// Controls the enabled/disabled state of fields in expense claim or replenishment bills
        /// </summary>
        /// <param name="cache">The cache object for the APInvoice DAC</param>
        /// <param name="row">The current APInvoice record</param>
        /// <remarks>
        /// 2024-02-13 : Added field state control for expense claim or replenishment bills {JLTG}
        /// </remarks>
        protected virtual void SetAPInvoiceFieldStates(PXCache cache, APInvoice row)
        {
            bool IsMigration = ATPTEFMPreferences.Current.IsCashAdvanceMigration ?? false;
            bool canEdit = CanEditDocument(row);
            bool canEditMigrationFields = IsMigration && canEdit;

            PXUIFieldAttribute.SetEnabled<ATPTEFMAPRegisterExt.usrATPTEFMSourceType>(cache, row, canEditMigrationFields);
            PXUIFieldAttribute.SetEnabled<ATPTEFMAPRegisterExt.usrATPTEFMSourceRef>(cache, row, canEditMigrationFields);
            PXUIFieldAttribute.SetEnabled<ATPTEFMAPRegisterExt.usrATPTEFMLiqNbr>(cache, row, canEdit);
            PXUIFieldAttribute.SetEnabled<ATPTEFMAPInvoiceExtension.usrATPTEFMBudgetEnabled>(cache, row, canEdit);

            if (!canEdit)
            {
                bool isDocReleased = row.Released ?? false;

                PXUIFieldAttribute.SetEnabled(cache, row, false);
                PXUIFieldAttribute.SetEnabled<APInvoice.separateCheck>(cache, row, !isDocReleased);
                PXUIFieldAttribute.SetEnabled<APInvoice.docType>(cache, row, true);
                PXUIFieldAttribute.SetEnabled<APInvoice.refNbr>(cache, row, true);
                PXUIFieldAttribute.SetEnabled<APInvoice.refNbr>(cache, row, true);
                PXUIFieldAttribute.SetEnabled<APInvoice.paySel>(cache, row, true);
                PXUIFieldAttribute.SetEnabled<APInvoice.payDate>(cache, row, true);
                PXUIFieldAttribute.SetEnabled<APInvoice.payLocationID>(cache, row, true);
                PXUIFieldAttribute.SetEnabled<APInvoice.payTypeID>(cache, row, true);
                PXUIFieldAttribute.SetEnabled<APInvoice.payAccountID>(cache, row, true);
            }
        }

        /// <summary>
        /// Determines if the document can be edited based on its current state
        /// </summary>
        /// <param name="doc">The AP document to check</param>
        /// <returns>True if document can be edited, false otherwise</returns>
        /// <remarks>
        /// 2025-03-24 : pplicable to Pending Approval and Balanced Statuses, where the editing should align with the Acumatica Standard CASE:010815 {JLTG}
        /// </remarks>
        protected virtual bool CanEditDocument(APInvoice doc)
        {
            if (doc == null) return false;

            bool isHold = doc.Status == APDocStatus.Hold;
            bool isBalanced = doc.Status == APDocStatus.Balanced;
            bool isPendingApproval = doc.Status == APDocStatus.PendingApproval;
            bool isVoided = doc.Voided == true;
            bool isScheduled = doc.Scheduled == true;
            bool isPrebooked = doc.Prebooked == true;
            bool isDontApprove = doc.DontApprove == true;

            if (isBalanced && isDontApprove)
                return true;

            if (isPendingApproval && isDontApprove)
                return true;

            return isHold && !isVoided && !isScheduled && !isPrebooked;
        }
        private void UnlinkCashAdvanceFromBill(APInvoice invoice)
        {
            if (invoice == null) return;

            ATPTEFMCashAdvanceEntry cashAdvanceEntry = PXGraph.CreateInstance<ATPTEFMCashAdvanceEntry>();
            ATPTEFMCashAdvance cashAdvance = PXSelect<
                   ATPTEFMCashAdvance,
                   Where<ATPTEFMCashAdvance.billRefNbr, Equal<Required<ATPTEFMCashAdvance.billRefNbr>>>>
                   .Select(Base, invoice.RefNbr);

            if (cashAdvance != null)
            {
                cashAdvanceEntry.CashAdvances.Current = cashAdvance;
                cashAdvance.Status = ATPTEFMCashAdvanceStatusAttribute.OpenValue;
                cashAdvance.BillType = string.Empty;
                cashAdvance.BillRefNbr = string.Empty;
                cashAdvanceEntry.CashAdvances.Update(cashAdvance);
                cashAdvanceEntry.Save.Press();
            }
        }
        private void UnlinkReplenishmentFromInvoice(APInvoice invoice)
        {
            if (invoice == null) return;

            ATPTEFMReplenishmentEntry repGraph = PXGraph.CreateInstance<ATPTEFMReplenishmentEntry>();
            ExpenseClaimDetailEntry expenseClaimDetailEntry = PXGraph.CreateInstance<ExpenseClaimDetailEntry>();

            PXResultset<ATPTEFMReplenishmentDetail, ATPTEFMReplenishment, EPExpenseClaimDetails> replenishmentDetails = PXSelectJoin<
                ATPTEFMReplenishmentDetail,
                InnerJoin<ATPTEFMReplenishment,
                    On<ATPTEFMReplenishment.replenishmentNbr, Equal<ATPTEFMReplenishmentDetail.replenishmentNbr>>,
                InnerJoin<EPExpenseClaimDetails,
                    On<ATPTEFMReplenishmentDetail.expenseReceiptNbr, Equal<EPExpenseClaimDetails.claimDetailCD>>>>,
                Where<ATPTEFMReplenishmentDetail.invoiceRefNbr, Equal<Required<ATPTEFMReplenishmentDetail.invoiceRefNbr>>>>
                .Select<PXResultset<ATPTEFMReplenishmentDetail, ATPTEFMReplenishment, EPExpenseClaimDetails>>(Base, invoice.RefNbr);

            if (replenishmentDetails.Count > 0)
            {
                ATPTEFMReplenishment rep = replenishmentDetails;
                repGraph.Replenishments.Current = replenishmentDetails;

                foreach (PXResult<ATPTEFMReplenishmentDetail, ATPTEFMReplenishment, EPExpenseClaimDetails> result in replenishmentDetails)
                {
                    ATPTEFMReplenishmentDetail detail = result;
                    detail.InvoiceRefNbr = null;
                    repGraph.ReplenishmentDetails.Update(detail);

                    ATPTEFMExpenseReceipt.Current = result;

                    ATPTEFMExpenseReceipt.Current.APRefNbr = null;
                    ATPTEFMExpenseReceipt.Current.Status = EPExpenseClaimDetailsStatus.ApprovedStatus;
                    ATPTEFMExpenseReceipt.UpdateCurrent();
                    ATPTEFMExpenseReceipt.Cache.Persist(PXDBOperation.Update);
                }

                rep.Status = ATPTEFMReplenishmentStatusAttribute.OpenValue;
                rep.Step = ATPTEFMReplenishmentStepAttribute.SubmitReceiptValue;
                rep.IsReleased = false;

                repGraph.Replenishments.Update(rep);
                repGraph.Save.Press();
            }
        }
        private void UnlinkFundEstablishmentFromBill(APInvoice invoice)
        {
            if (invoice == null) return;

            var fundGraph = PXGraph.CreateInstance<ATPTEFMFundMaint>();
            ATPTEFMFund fund = PXSelect<ATPTEFMFund,
                Where<ATPTEFMFund.establishmentRefNbr, Equal<Required<ATPTEFMFund.establishmentRefNbr>>>>
                .Select(Base, invoice.RefNbr);

            if (fund != null)
            {
                ATPTEFMFund.Current = fund;

                ATPTEFMFundTransactionHistoryView establishmentBillHistory =
                    PXSelect<ATPTEFMFundTransactionHistoryView,
                        Where<ATPTEFMFundTransactionHistoryView.fundRefNbr, Equal<Required<ATPTEFMFundTransactionHistoryView.fundRefNbr>>,
                            And<ATPTEFMFundTransactionHistoryView.refNbr, Equal<Required<ATPTEFMFundTransactionHistoryView.refNbr>>,
                            And<ATPTEFMFundTransactionHistoryView.transactionType, Equal<CashFundManagement.BLC.ATPTEFMFundMaint.ATPTEFMTransactionHistoryView.transactionType.establishment>>>>>
                    .Select(Base, fund.FundCD, invoice.RefNbr);

                if (establishmentBillHistory != null)
                {
                    ATPTEFMTransactionHistoryView.Current = establishmentBillHistory;
                    ATPTEFMTransactionHistoryView.DeleteCurrent();
                    ATPTEFMTransactionHistoryView.Cache.Persist(PXDBOperation.Delete);
                }

                ATPTEFMFund.Current.Status = ATPTEFMFundStatusAttribute.BalancedValue;
                ATPTEFMFund.Current.EstablishmentRefNbr = null;
                ATPTEFMFund.Current.Released = false;
                ATPTEFMFund.UpdateCurrent();
                ATPTEFMFund.Cache.Persist(PXDBOperation.Update);
            }

        }
        #endregion
    }

}