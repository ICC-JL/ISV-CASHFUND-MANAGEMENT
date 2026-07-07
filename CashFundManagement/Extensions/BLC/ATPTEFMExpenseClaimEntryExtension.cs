using CashFundManagement.Attributes;
using CashFundManagement.BLC;
using CashFundManagement.Classes;
using CashFundManagement.DAC;
using CashFundManagement.DAC.Setup;
using CashFundManagement.DAC.Unbound;
using CashFundManagement.Extensions.DAC;
using CashFundManagement.Helper;
using CashFundManagement.Messages;
using PX.Api;
using PX.Common;
using PX.Data;
using PX.Data.BQL;
using PX.Objects.AP;
using PX.Objects.AR;
using PX.Objects.CA;
using PX.Objects.CM;
using PX.Objects.Common;
using PX.Objects.CR;
using PX.Objects.CS;
using PX.Objects.EP;
using PX.Objects.FA;
using PX.Objects.GL;
using PX.Objects.GL.FinPeriods;
using PX.Objects.GL.FinPeriods.TableDefinition;
using PX.Objects.IN;
using PX.Objects.PM;
using PX.Objects.RQ;
using PX.Objects.TX;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using static PX.Data.Events;
using static PX.Objects.EP.ExpenseClaimEntry;
using static PX.Objects.TX.CSTaxCalcType;
using static PX.SM.EMailAccount;

namespace CashFundManagement.Extensions.BLC
{
    /// <remarks>
    /// 2024-08-14 : Adds multi-tenant support. {RRS}
    /// 2025-03-10 : Address and Location query updated : 010624 : RFS
    /// 2025-03-12 : Updated fieldupdated event for populating Vendor details : 010713 : RFS
    /// 2025-03-26 : Update Detail VendorID on Summary Vendor change : 010687 : RFS
    /// </remarks>
    public class ATPTEFMExpenseClaimEntry_Extension : PXGraphExtension<ExpenseClaimEntryReceiptExt, ExpenseClaimEntry>
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
            Base.OnBeforePersist += OnBeforeGraphPersist;
            Base.OnBeforeCommit += OnBeforeGraphCommit;
            /*ATPTEFMBudgetLibrary.BudgetVisibility(ATPTEFMBudget.Cache, ATPTEFMFeatureSetup?.Current, "P");
            ATPTEFMProjectBudgetLibrary.ProjectBudgetVisibility(ATPTEFMProjectBudget.Cache, ATPTEFMFeatureSetup?.Current, "P");*/

            if (!IsActive())
            {
                ATPTEFMBudget.AllowSelect = false;
                ATPTEFMProjectBudget.AllowSelect = false;
                ATPTEFMLiquidationForm.SetVisible(false);
                ATPTEFMRequestPaymentForm.SetVisible(false);
                ATPTEFMReports.SetVisible(false);
                return;
            }
            ATPTEFMReports.MenuAutoOpen = true;
            ATPTEFMReports.AddMenuAction(ATPTEFMLiquidationForm);
            ATPTEFMReports.AddMenuAction(ATPTEFMRequestPaymentForm);

            ATPTEFMProjectBudget.AllowSelect = false;
        }
        #endregion

        #region Views

        public PXSetup<ATPTEFMFeatures> ATPTEFMFeatureSetup;
        public PXSetup<ATPTEFMCASetup> ATPTEFMCAPreference;
        //public PXSetup<FeaturesSet> EnableFeatures;

        [PXCopyPasteHiddenView]
        public PXSelectReadonly<ATPTEFMBudget> ATPTEFMBudget;
        public PXSelect<ATPTEFMBudgetHistory> ATPTEFMHistory;

        [PXCopyPasteHiddenView]
        public PXSelectReadonly<ATPTEFMPBudget> ATPTEFMProjectBudget;
        public PXSelect<ATPTEFMProjectBudgetHistory> ATPTEFMProjectHistoryView;

        public PXSelect<
            ATPTEFMProjectBudgetLineSummary,
            Where<ATPTEFMProjectBudgetLineSummary.released, Equal<True>,
                And<ATPTEFMProjectBudgetLineSummary.projectID, Equal<Required<ATPTEFMProjectBudgetLineSummary.projectID>>,
                And<ATPTEFMProjectBudgetLineSummary.projectTaskID, Equal<Required<ATPTEFMProjectBudgetLineSummary.projectTaskID>>,
                And<ATPTEFMProjectBudgetLineSummary.costCodeID, Equal<Required<ATPTEFMProjectBudgetLineSummary.costCodeID>>,
                And<ATPTEFMProjectBudgetLineSummary.finYear, Equal<Required<ATPTEFMProjectBudgetLineSummary.finYear>>,
                And<ATPTEFMProjectBudgetLineSummary.accountGroupID, Equal<Required<ATPTEFMProjectBudgetLineSummary.accountGroupID>>>>>>>>>
            ATPTEFMProjectBudgetSummary;

        public PXSelect<APPayment> ATPTEFMAPPaymentDocument;

        /// <remarks>
        /// Vendor view that won't clear
        /// </remarks>
        public PXSelect<
                                                                                                        VendorR,
                                                                                                        Where<VendorR.bAccountID, Equal<Current<ATPTEFMEPExpenseClaimExt.usrATPTVendorID>>>>
                                                                                                        ATPTEFMCurrentVendor;

        public PXSelect<
            VendorR,
            Where<VendorR.bAccountID, Equal<Required<VendorR.bAccountID>>>>
            ATPTEFMVendor;

        public PXSetup<ATPTEFMReqClass> ATPTEFMEmployeeReqClass;

        public PXSelect<ATPTEFMCASetup> ATPTEFMLiquidationNumber;

        [PXViewName("Cash Advance Receipts")]
        public PXSelect<
            ATPTEFMCAReceiptDetail,
            Where<ATPTEFMCAReceiptDetail.expenseReceiptRefNbr, Equal<Required<ATPTEFMCAReceiptDetail.expenseReceiptRefNbr>>>>
            ATPTEFMCashAdvanceReceipts;

        public PXSelect<
            Numbering,
            Where<Numbering.numberingID, Equal<Required<Numbering.numberingID>>>>
            ATPTEFMnumbering;

        #endregion

        #region View Delegates
        public delegate IEnumerable receiptsforsubmitDelegate();
        [PXOverride]
        public IEnumerable receiptsforsubmit(receiptsforsubmitDelegate del)
        {
            if (!IsActive())
            {
                foreach (var item in del())
                {
                    yield return item;
                }
                yield break;
            }

            PXSelectBase<EPExpenseClaimDetailsForSubmit> receiptsForSubmit = new PXSelect<EPExpenseClaimDetailsForSubmit,
                                         Where2<
                                             Where<EPExpenseClaimDetailsForSubmit.refNbr, IsNull,
                                                 Or<EPExpenseClaimDetailsForSubmit.refNbr, Equal<Current<EPExpenseClaim.refNbr>>>>,
                                             And<Where<EPExpenseClaimDetailsForSubmit.released, NotEqual<True>,
                                                 And<EPExpenseClaimDetailsForSubmit.employeeID, Equal<Current<EPExpenseClaim.employeeID>>,
                                                 And<ATPTEFMEPExpenseClaimDetailsExt.usrATPTEFMTranType, Equal<Current<ATPTEFMEPExpenseClaimExt.usrATPTEFMTranType>>,
                                                 And<ATPTEFMEPExpenseClaimDetailsExt.usrATPTEFMReqType, Equal<Current<ATPTEFMEPExpenseClaimExt.usrATPTEFMReqType>>>>>>>>>(Base);

            if (Base.ExpenseClaim.Current.GetExtension<ATPTEFMEPExpenseClaimExt>().UsrATPTEFMTranType == ATPTEFMExpenseTypeAttribute.RequestforPayment)
            {
                receiptsForSubmit
                    .WhereAnd<
                    Where<ATPTEFMEPExpenseClaimDetailsExt.usrATPTVendorID, Equal<Current2<ATPTEFMEPExpenseClaimExt.usrATPTVendorID>>>>();
            }
            if (Base.epsetup.Current.AllowMixedTaxSettingInClaims != true)
            {
                receiptsForSubmit
                    .WhereAnd<
                    Where<EPExpenseClaimDetailsForSubmit.taxCalcMode, Equal<Current2<EPExpenseClaim.taxCalcMode>>,
                        And<Where<EPExpenseClaimDetailsForSubmit.taxZoneID, Equal<Current2<EPExpenseClaim.taxZoneID>>,
                            Or<Where<EPExpenseClaimDetailsForSubmit.taxZoneID, IsNull,
                                And<Current2<EPExpenseClaim.taxZoneID>, IsNull>>>>>>>();
            }

            HashSet<Int32?> receiptsInClaim = new HashSet<Int32?>();
            foreach (EPExpenseClaimDetails receiptInClaim in Base.ExpenseClaimDetails.Select())
            {
                receiptsInClaim.Add(receiptInClaim.ClaimDetailID);
            }
            foreach (EPExpenseClaimDetailsForSubmit receiptForSubmit in receiptsForSubmit.Select())
            {
                if (receiptsInClaim.Contains(receiptForSubmit.ClaimDetailID))
                {
                    continue;
                }

                if (receiptForSubmit.Status == ATPTEFMExpenseReceiptStatusAttribute.OpenValue)
                    yield return receiptForSubmit;
            }
        }

        /// <remarks>
        /// 2025-07-01 :  Not all Debit Adj. display in Link to Check. CASEID: 012218 {JLG} <br/>         
        /// </remarks>
        protected virtual IEnumerable aTPTEFMAPPaymentDocument()
        {
            APInvoice aPInvoice = Base.APDocuments.Current;
            EPExpenseClaim claim = Base.ExpenseClaim.Current;

            if (Base.ExpenseClaimDetails.SelectSingle() != null && aPInvoice != null)
            {
                return PXSelectJoinGroupBy<
                    APPayment,
                    InnerJoin<APAdjust,
                        On<APPayment.refNbr, Equal<APAdjust.adjgRefNbr>,
                        And<APPayment.docType, Equal<APAdjust.adjgDocType>>>,
                    InnerJoin<APInvoice,
                        On<APInvoice.refNbr, Equal<APAdjust.adjdRefNbr>,
                        And<APInvoice.docType, Equal<APAdjust.adjdDocType>>>,
                    InnerJoin<EPExpenseClaim,
                        On<EPExpenseClaim.refNbr, Equal<APInvoice.origRefNbr>>>>>,
                    Where<EPExpenseClaim.refNbr, Equal<Required<EPExpenseClaim.refNbr>>>,
                    Aggregate<
                        GroupBy<APPayment.docType,
                        GroupBy<APPayment.refNbr>>>>
                    .Select(Base, claim.RefNbr);
            }
            else
            {
                return PXSelectReadonly<
                    APPayment,
                    Where<APPayment.origModule, Equal<BatchModule.moduleEP>,
                        And<APPayment.origRefNbr, Equal<Current<EPExpenseClaim.refNbr>>,
                        And<APPayment.origDocType, Equal<EPExpenseClaim.docType>>>>>
                    .Select(Base);
            }
        }
        public virtual IEnumerable aTPTEFMBudget()
        {
            #region Variables
            EPExpenseClaim doc = Base.ExpenseClaim?.Current;
            ATPTEFMEPExpenseClaimExt rowExt = doc?.GetExtension<ATPTEFMEPExpenseClaimExt>();
            if (rowExt == null) yield break;

            rowExt.UsrATPTEFMIsOverbudget = false;
            rowExt.UsrATPTEFMHasInitialBudget = false;
            //ATPTEFMAPSetupExtension apSetupExt = Base.apsetup?.Current?.GetExtension<ATPTEFMAPSetupExtension>();
            var finID = $"{doc.DocDate.Value.Year}{doc.DocDate.Value.Month:00}";
            ATPTEFMBudgetLibrary.FinPeriodData fData = ATPTEFMBudgetLibrary.GetFinPeriod(Base, doc.FinPeriodID ?? finID, ATPTEFMFeatureSetup?.Current?.BudgetCalculation);
            List<ATPTEFMBudgetLibrary.BudgetParameters> parameterList = new List<ATPTEFMBudgetLibrary.BudgetParameters>();
            Company company = PXSelect<Company>.Select(Base);

            EPExpenseClaimDetails docDet = PXSelect<EPExpenseClaimDetails,
                Where<EPExpenseClaimDetails.refNbr, Equal<@P.AsString>,
                And<EPExpenseClaimDetails.contractID, Equal<@P.AsInt>,
                And<EPExpenseClaimDetails.expenseAccountID, IsNotNull,
                And<EPExpenseClaimDetails.expenseSubID, IsNotNull>>>>>
                .Select(Base, doc.RefNbr, ProjectDefaultAttribute.NonProject());
            bool showbudget = docDet != null;

            if (doc == null || fData == null || !((rowExt.UsrATPTEFMBudgetEnabled ?? false)
            && rowExt.UsrATPTEFMTranType == ATPTEFMExpenseTypeAttribute.RequestforPayment) || !showbudget) 
            {
                ATPTEFMBudget.AllowSelect = false;
                yield break;
            }
            #endregion

            #region Supply Parameters
            foreach (EPExpenseClaimDetails item in Base.ExpenseClaimDetails.Select())
            {
                if (item.ExpenseAccountID == null || item.ExpenseSubID == null) continue;
                if (item.ContractID != ProjectDefaultAttribute.NonProject()) continue;
                Account account = PXSelect<
                    Account,
                    Where<Account.accountID, Equal<Required<Account.accountID>>>>
                    .Select(Base, item.ExpenseAccountID);

                bool isReversed = false;

                if (item.APRefNbr != null)
                {
                    APInvoice invdebit = PXSelect<
                        APInvoice,
                        Where<APInvoice.docType, Equal<APDocType.debitAdj>,
                            And<APInvoice.origRefNbr, Equal<@P.AsString>,
                            And<APInvoice.status, Equal<APDocStatus.closed>>>>>
                        .Select(Base, item.APRefNbr);
                    isReversed = invdebit != null;
                }

                parameterList.Add(new ATPTEFMBudgetLibrary.BudgetParameters()
                {
                    LedgerID = ATPTEFMFeatureSetup?.Current?.BudgetLedgerID,
                    BranchID = item.BranchID,
                    RefNbr = item.RefNbr,
                    CuryID = account.CuryID ?? company?.BaseCuryID,
                    OriginType = ATPTEFMBudgetLibrary.OriginTypes.RequestForPayment,
                    AccountID = item.ExpenseAccountID,
                    SubID = item.ExpenseSubID,
                    FinYear = fData.FinYear,
                    FromFinPeriodID = fData.StartPeriod,
                    ToFinPeriodID = fData.EndPeriod,
                    FinPeriodID = finID,
                    Amount = isReversed ? 0 : item.CuryExtCost,
                    Approved = doc.Approved ?? false,
                });
            }
            #endregion

            ATPTEFMBudget.AllowSelect = (rowExt.UsrATPTEFMBudgetEnabled ?? false) && parameterList.Any();
            if (!parameterList.Any()) yield break;

            bool HasChanges = Base.Caches[typeof(EPExpenseClaimDetails)].GetStatus(Base.ExpenseClaimDetails.Current ?? Base.ExpenseClaimDetails.Select().FirstOrDefault()) != PXEntryStatus.Notchanged;

            foreach (ATPTEFMBudget item in ATPTEFMBudgetLibrary.GenerateBudget(Base, parameterList))
            {
                yield return item;

                if (!rowExt.UsrATPTEFMIsOverbudget ?? false)
                {
                    // CASE: 003020
                    // 1. Is Over Budget will only be tick if Request Amount (includes approved and unapproved) is greater than Initial Budget
                    if ((item.ApprovedAmt + item.UnapprovedAmt) > item.InitialBudget) rowExt.UsrATPTEFMIsOverbudget = true;
                    // END CASE: 003020
                }

                if (!rowExt.UsrATPTEFMHasInitialBudget ?? false)
                {
                    if (item.InitialBudget > 0) rowExt.UsrATPTEFMHasInitialBudget = true;
                }

                // CASE: 003020 - Temp Implementation
                // 2. Remaining Budget column is equal to Initial Budget less all approved Amount.
                if (item.ApprovedAmt > 0 && ATPTEFMFeatureSetup?.Current?.BudgetBudgetAmount == "F3") item.BudgetAmt = item.InitialBudget - item.ApprovedAmt;
                // END CASE: 003020

                if (HasChanges)
                {
                    if (item.BudgetAmt < 0 && ATPTEFMFeatureSetup?.Current?.BudgetValidation != RQRequestClassBudget.None)
                    {
                        this.ATPTEFMBudget.Cache.RaiseExceptionHandling<ATPTEFMBudget.budgetAmt>(item, item.BudgetAmt,
                                ATPTEFMHelper.GetPropertyException(item, ATPTEFMMessages.RemainingBudgetMustNotBeLessThanZero,
                                    ATPTEFMFeatureSetup?.Current?.BudgetValidation == RQRequestClassBudget.Warning ? PXErrorLevel.Warning : PXErrorLevel.Error));

                    }
                }
            }
        }

        public virtual IEnumerable aTPTEFMProjectBudget()
        {
            #region Variables
            EPExpenseClaim doc = Base.ExpenseClaim?.Current;
            ATPTEFMEPExpenseClaimExt rowExt = doc?.GetExtension<ATPTEFMEPExpenseClaimExt>();

            ATPTEFMAPSetupExtension apSetupExt = Base.apsetup?.Current?.GetExtension<ATPTEFMAPSetupExtension>();
            var finID = $"{doc.DocDate.Value.Year}{doc.DocDate.Value.Month:00}";
            ATPTEFMBudgetLibrary.FinPeriodData fData = ATPTEFMBudgetLibrary.GetFinPeriod(Base, doc.FinPeriodID ?? finID, ATPTEFMFeatureSetup?.Current?.ProjectBudgetCalculation);
            List<ATPTEFMProjectBudgetLibrary.ProjectBudgetParameters> parameterList = new List<ATPTEFMProjectBudgetLibrary.ProjectBudgetParameters>();
            Company company = PXSelect<Company>.Select(Base);

            if (doc == null || fData == null || !(rowExt?.UsrATPTEFMProjectBudgetEnabled ?? false)) yield break;
            #endregion

            #region Supply Parameters
            foreach (EPExpenseClaimDetails item in Base.ExpenseClaimDetails.Select())
            {
                ATPTEFMEPExpenseClaimDetailsExt itemExt = item.GetExtension<ATPTEFMEPExpenseClaimDetailsExt>();

                if (ATPTEFMBudgetLibrary.HasNull(item.ContractID, item.TaskID, item.CostCodeID, itemExt.UsrATPTEFMAccountGroup, item.InventoryID)) continue;

                Account account = PXSelect<
                    Account,
                    Where<Account.accountID, Equal<Required<Account.accountID>>>>
                    .Select(Base, item.ExpenseAccountID);

                APTran reversed = PBudgetIsReversed(item);

                parameterList.Add(new ATPTEFMProjectBudgetLibrary.ProjectBudgetParameters()
                {
                    RefNbr = doc.RefNbr,
                    CuryID = account.CuryID ?? company?.BaseCuryID,
                    ProjectID = item.ContractID,
                    LedgerID = ATPTEFMFeatureSetup?.Current?.ProjectBudgetLedgerID,
                    ProjectTaskID = item.TaskID,
                    CostCodeID = item.CostCodeID,
                    InventoryID = item.InventoryID,
                    AccountGroupID = itemExt.UsrATPTEFMAccountGroup,
                    FinYear = fData.FinYear,
                    FromFinPeriodID = fData.StartPeriod,
                    ToFinPeriodID = fData.EndPeriod,
                    FinPeriodID = finID,
                    Amount = reversed != null ? (item.CuryExtCost - reversed.CuryTranAmt) : item.CuryExtCost,
                    Approved = doc.Approved ?? false,
                    Released = doc.Released ?? false,
                    OriginType = ATPTEFMBudgetLibrary.OriginTypes.RequestForPayment
                });
            }
            #endregion

            ATPTEFMProjectBudget.AllowSelect = parameterList.Any();
            if (!parameterList.Any()) yield break;

            foreach (ATPTEFMPBudget item in ATPTEFMProjectBudgetLibrary.GenerateProjectBudget(this.Base, parameterList))
            {
                yield return item;

                bool isError = ATPTEFMFeatureSetup?.Current?.ProjectBudgetValidation == RQRequestClassBudget.Error;
                bool isWarning = ATPTEFMFeatureSetup?.Current?.ProjectBudgetValidation == RQRequestClassBudget.Warning;

                if (isError || isWarning)
                {
                    ATPTEFMProjectBudgetLineSummary PBSummary = ATPTEFMProjectBudgetSummary.Select(item.ProjectID, item.ProjectTaskID, item.CostCodeID, Base.ExpenseClaim.Current.DocDate.Value.Year.ToString(), item.AccountGroupID);

                    if (PBSummary == null)
                    {
                        this.ATPTEFMProjectBudget.Cache.RaiseExceptionHandling<ATPTEFMPBudget.projectID>(item, item.ProjectID,
                            ATPTEFMHelper.GetPropertyException(item, ATPTEFMMessages.NotInProjectBudget,
                                isError ? PXErrorLevel.RowError : PXErrorLevel.Warning));
                    }
                    else if (item.BudgetAmt < 0)
                    {
                        this.ATPTEFMProjectBudget.Cache.RaiseExceptionHandling<ATPTEFMPBudget.budgetAmt>(item, item.BudgetAmt,
                                ATPTEFMHelper.GetPropertyException(item, ATPTEFMMessages.RemainingBudgetMustNotBeLessThanZero,
                                    isError ? PXErrorLevel.RowError : PXErrorLevel.Warning));

                    }
                }
            }
        }

        #endregion

        #region Method Overrides (Standard)
        public delegate void VerifyEmployeeAndClaimCurrenciesForCashDelegate(EPExpenseClaimDetails receipt,
            string receiptsPaidWith,
            EPExpenseClaim claim,
            Action substituteNewValue = null);
        [PXOverride]
        public void VerifyEmployeeAndClaimCurrenciesForCash(
            EPExpenseClaimDetails receipt,
            string receiptsPaidWith,
            EPExpenseClaim claim,
            Action substituteNewValue,
            VerifyEmployeeAndClaimCurrenciesForCashDelegate baseMethod)
        {
            if (claim == null)
                return;

            ATPTEFMEPExpenseClaimExt rowExt = claim.GetExtension<ATPTEFMEPExpenseClaimExt>();

            Numbering numbering = PXSelect<
                Numbering,
                Where<Numbering.numberingID, Equal<Required<Numbering.numberingID>>>>
                .Select(Base, Base.epsetup.Current.ClaimNumberingID);
            if (numbering is null) return;

            if (rowExt.UsrATPTEFMTranType.Equals(ATPTEFMExpenseTypeAttribute.Liquidation))
            {
                if (receiptsPaidWith.Equals(EPExpenseClaimDetails.paidWith.PersonalAccount))
                {
                    EPEmployee employee = PXSelect<
                        EPEmployee,
                        Where<EPEmployee.bAccountID, Equal<Required<EPEmployee.bAccountID>>>>
                        .Select(Base, receipt.EmployeeID);

                    if (claim.RefNbr.Trim() != numbering.NewSymbol)
                    {
                        if (employee.AllowOverrideCury != true
                            && employee.CuryID != null
                            && employee.CuryID != claim.CuryID)
                        {
                            substituteNewValue?.Invoke();
                            string message = "The expense receipt cannot be added to the claim because the employee currency differs from the receipt currency and cannot be overridden. Enable currency override for the employee on the Employees (EP203000) form first.";
                            throw ATPTEFMHelper.GetPropertyException(claim, message, PXErrorLevel.Error);
                        }
                    }
                }
            }
            else
            {
                if (receiptsPaidWith != null && receiptsPaidWith.Equals(EPExpenseClaimDetails.paidWith.PersonalAccount))
                {
                    VendorR vendor = ATPTEFMVendor.Select(rowExt.UsrATPTVendorID);
                    if (claim.RefNbr.Trim() != numbering.NewSymbol)
                    {
                        if (vendor.AllowOverrideCury != true
                            && vendor.CuryID != null
                            && vendor.CuryID != claim.CuryID)
                        {
                            substituteNewValue?.Invoke();
                            string message = "The expense receipt cannot be added to the claim because the employee currency differs from the receipt currency and cannot be overridden. Enable currency override for the vendor on the Vendors (AP303000) form first.";
                            throw ATPTEFMHelper.GetPropertyException(claim, message, PXErrorLevel.Error);
                        }
                    }
                }
            }
        }
        #endregion

        #region CommitTaxes Override
        /// <remarks>
        /// 03-31-2026 : Fix claim header CuryTaxTotal not recalculated after manual tax deletion from smart panel {RFS}
        /// </remarks>
        public delegate IEnumerable commitTaxesDelegate(PXAdapter adapter);
        [PXOverride]
        public IEnumerable commitTaxes(PXAdapter adapter, commitTaxesDelegate baseMethod)
        {
            var result = baseMethod(adapter);

            EPExpenseClaimDetails current = Base.ExpenseClaimDetails.Current;
            if (current != null)
            {
                Base1.RecalcAmountInClaimCury(current);
            }

            return result;
        }
        #endregion

        #region Action
        /// <remarks>
        /// 2025-06-27 : Allow the user to cancel RFP type when all bills are reversed. CASEID: 012144 {JLG}
        /// 2025-10-20 : Remove approvers when cancelling expense claim. CASEID: 014026 {JLG}
        /// </remarks>
        public PXAction<EPExpenseClaim> ATPTEFMCancel;
        [PXProcessButton(Category = "Processing"), PXUIField(DisplayName = "Cancel")]
        public IEnumerable aTPTEFMCancel(PXAdapter adapter)
        {
            ATPTEFMCashAdvanceEntry caGraph = PXGraph.CreateInstance<ATPTEFMCashAdvanceEntry>();
            ExpenseClaimDetailEntry ecEntry = PXGraph.CreateInstance<ExpenseClaimDetailEntry>();

            ATPTEFMHelper.StartLongOperation(Base, adapter, delegate ()
            {
                using (PXTransactionScope ts = new PXTransactionScope())
                {
                    EPExpenseClaim row = Base.ExpenseClaim.Current;
                    ATPTEFMEPExpenseClaimExt rowExt = row.GetExtension<ATPTEFMEPExpenseClaimExt>();

                    foreach (EPExpenseClaimDetails value in Base.ExpenseClaimDetails.Select())
                    {
                        if (rowExt.UsrATPTEFMTranType == ATPTEFMExpenseTypeAttribute.Liquidation)
                        {
                            caGraph.Clear();

                            ATPTEFMCAReceiptDetail caReceipt = PXSelect<
                                ATPTEFMCAReceiptDetail,
                                Where<ATPTEFMCAReceiptDetail.expenseReceiptRefNbr, Equal<Required<ATPTEFMCAReceiptDetail.expenseReceiptRefNbr>>>>
                                .Select(Base, value.ClaimDetailCD);
                            if (caReceipt != null)
                            {
                                ATPTEFMCashAdvance ca = PXSelect<
                                    ATPTEFMCashAdvance,
                                    Where<ATPTEFMCashAdvance.cashAdvanceNbr, Equal<Required<ATPTEFMCashAdvance.cashAdvanceNbr>>>>
                                    .Select(Base, caReceipt.CashAdvanceNbr);
                                caGraph.CashAdvances.Current = ca;

                                caReceipt.LiquidationRef = null;
                                caReceipt.Reversed = false;

                                caGraph.CashAdvanceReceiptLines.Update(caReceipt);
                                caGraph.Save.Press();
                            }

                            value.RefNbr = null;
                            value.Released = false;
                            value.Status = EPExpenseClaimDetailsStatus.ApprovedStatus;
                            Base.ExpenseClaimDetails.Update(value);
                        }

                        if (rowExt.UsrATPTEFMTranType == ATPTEFMExpenseTypeAttribute.RequestforPayment)
                        {
                            ecEntry.Clear();
                            ecEntry.ClaimDetails.Current = value;
                            ecEntry.ClaimDetails.Current.Status = ATPTEFMExpenseReceiptStatusAttribute.CancelledValue;
                            ecEntry.ClaimDetails.UpdateCurrent();
                            ecEntry.Save.Press();

                            // Alternative approach - direct status update not working as expected
                            //value.Status = EPExpenseClaimDetailsStatus.CancelledValue;
                            //Base.ExpenseClaimDetails.Update(value);
                        }
                    }

                    #region Remove Approvals
                    foreach (EPApproval approval in Base.Approval.Select())
                    {
                        Base.Approval.Delete(approval);
                    }
                    #endregion

                    row.Status = ATPTEFMExpenseClaimStatusAttribute.CancelledValue;
                    Base.ExpenseClaim.Update(row);
                    Base.Save.Press();

                    ts.Complete();
                }
            });
            return adapter.Get();
        }
        #endregion

        #region Action Delegate

        public PXAction<EPExpenseClaim> ATPTEFMViewRequestReference;
        [PXUIField(
            DisplayName = "View Req Reference",
            MapEnableRights = PXCacheRights.Select,
            MapViewRights = PXCacheRights.Select,
            Visible = false)]
        [PXLookupButton]
        public virtual IEnumerable aTPTEFMViewRequestReference(PXAdapter adapter)
        {
            ATPTEFMEPExpenseClaimExt epclaims = Base.ExpenseClaim.Current.GetExtension<ATPTEFMEPExpenseClaimExt>();
            ATPTEFMEPExpenseClaimDetailsExt claimDetails = Base.ExpenseClaimDetails.Current.GetExtension<ATPTEFMEPExpenseClaimDetailsExt>();

            if (epclaims != null && claimDetails != null && epclaims.UsrATPTEFMTranType == ATPTEFMExpenseTypeAttribute.Liquidation)
            {
                ATPTEFMCashAdvance cashAdvance = PXSelect<
                    ATPTEFMCashAdvance,
                    Where<ATPTEFMCashAdvance.cashAdvanceNbr,
                  Equal<Required<ATPTEFMCashAdvance.cashAdvanceNbr>>,
                        And<ATPTEFMCashAdvance.reqClassID, Equal<Required<ATPTEFMCashAdvance.reqClassID>>>>>
                    .Select(Base, claimDetails.UsrATPTEFMReqRef, epclaims.UsrATPTEFMReqClass);

                if (cashAdvance != null)
                {
                    PXRedirectHelper.TryRedirect(Base.Caches[typeof(ATPTEFMCashAdvance)], cashAdvance, "View Req Reference", PXRedirectHelper.WindowMode.NewWindow);
                }
            }
            return adapter.Get();
        }


        /// <remarks>
        /// 2024-10-07 : Employee Financial Settings: Tax Zone must not be empty. CASEID: 007961 {JLG} <br/>         
        /// 2025-01-15 : CASE ID: 009581 {JLG} Changes made:
        /// 1. Refactored financial settings validation into separate methods for better organization and maintainability
        /// 2. Added validation for inactive employees and vendors
        /// 3. Improved error handling with specific error messages
        /// 4. From Pk.Find to BQL query for retrieving inactive vendors and employees
        /// 5. Added null checks and improved error messages for better user experience <br/>
        /// </remarks>
        public delegate IEnumerable ReleaseDelegate(PXAdapter adapter);
        [PXOverride]
        public IEnumerable Release(PXAdapter adapter, ReleaseDelegate baseMethod)
        {
            ATPTEFMHelper.StartLongOperation(Base, delegate ()
            {
                using (PXTransactionScope ts = new PXTransactionScope())
                {
                    EPExpenseClaim claim = Base.ExpenseClaim.Current;
                    ATPTEFMEPExpenseClaimExt claimExt = claim.GetExtension<ATPTEFMEPExpenseClaimExt>();

                    ValidateFinancialSettings(claim, claimExt);

                    //Update CA Receipt Details (Liquidation Reference number added in database) - moved to CA Liquidate Button; Temporary comment out in case issue is encountered
                    //UpdateCAReceiptDetails();

                    #region Base Release

                    foreach (PXResult<EPExpenseClaim> rec in adapter.Get())
                    {
                        TryValidateRequireUniqueVendorRefWhenCardInvolved();

                        EPExpenseClaim claimBase = rec;
                        if (claim.Approved == false || claim.Released == true)
                        {
                            throw new PXException(PX.Objects.EP.Messages.Document_Status_Invalid);
                        }
                        Base.Save.Press();
                        EPDocumentRelease.ReleaseDoc(claim);
                    }

                    #endregion

                    #region EFMChanges
                    //TimeSpan timespan;
                    //Exception ex;
                    //PXLongRunStatus status = PXLongOperation.GetStatus(this.Base.UID, out timespan, out ex);

                    //while (status == PXLongRunStatus.InProcess)
                    //{
                    //    status = PXLongOperation.GetStatus(this.Base.UID, out timespan, out ex);
                    //}

                    List<EPExpenseClaim> list = new List<EPExpenseClaim>();
                    if (this.Base.ExpenseClaim.Current != null)
                        list.Add(this.Base.ExpenseClaim.Current);

                    #endregion

                    ts.Complete();
                }
            });

            return adapter.Get();
        }

        public PXAction<EPExpenseClaim> ATPTEFMReports;
        [PXButton()]
        [PXUIField(DisplayName = "Reports")]
        public IEnumerable aTPTEFMReports(PXAdapter adapter)
        {
            return adapter.Get();
        }

        public PXAction<EPExpenseClaim> ATPTEFMLiquidationForm;
        [PXButton()]
        [PXUIField(DisplayName = "Liquidation")]
        public IEnumerable aTPTEFMLiquidationForm(PXAdapter adapter)
        {
            foreach (EPExpenseClaim item in adapter.Get<EPExpenseClaim>())
            {
                ExpenseClaimEntry graph = PXGraph.CreateInstance<ExpenseClaimEntry>();
                ATPTEFMEPExpenseClaimExt epclaims = item.GetExtension<ATPTEFMEPExpenseClaimExt>();

                Dictionary<string, string> parameters = new Dictionary<string, string>();
                parameters["ReferenceNbr"] = epclaims.UsrATPTEFMLiqNbr;

                var report = new PXReportRequiredException(parameters, "ATPT6414", "Liquidation");

                throw new PXRedirectWithReportException(graph, report, "Preview");
            }
            return adapter.Get();
        }

        public PXAction<EPExpenseClaim> ATPTEFMRequestPaymentForm;
        [PXButton()]
        [PXUIField(DisplayName = ATPTEFMMessages.RequestForPayment, Visible = false)]
        public IEnumerable aTPTEFMRequestPaymentForm(PXAdapter adapter)
        {
            foreach (EPExpenseClaim item in adapter.Get<EPExpenseClaim>())
            {
                ExpenseClaimEntry graph = PXGraph.CreateInstance<ExpenseClaimEntry>();
                ATPTEFMEPExpenseClaimExt epclaims = item.GetExtension<ATPTEFMEPExpenseClaimExt>();

                Dictionary<string, string> parameters = new Dictionary<string, string>();
                parameters["ReferenceNbr"] = epclaims.UsrATPTEFMRFPReqRef;

                var report = new PXReportRequiredException(parameters, "ATPT6416", "Request for Payment");

                throw new PXRedirectWithReportException(graph, report, "Preview");
            }
            return adapter.Get();
        }

        public PXAction<EPExpenseClaim> ATPTEFMViewCheck;
        [PXUIField(DisplayName = ATPTEFMMessages.ViewCheck, MapEnableRights = PXCacheRights.Select, MapViewRights = PXCacheRights.Select, Visible = false)]
        [PXButton]
        public virtual IEnumerable aTPTEFMViewCheck(PXAdapter adapter)
        {
            if (ATPTEFMAPPaymentDocument.Current != null)
            {
                RedirectionToOrigDoc.TryRedirect(ATPTEFMAPPaymentDocument.Current.DocType, ATPTEFMAPPaymentDocument.Current.RefNbr, BatchModule.AP, preferPrimaryDocForm: true);
            }
            return adapter.Get();
        }

        #endregion

        #region Public Properties
        public bool IsEnabled { get; set; } = true;

        #endregion

        #region Events

        #region Row
        private void OnBeforeGraphPersist(PXGraph obj)
        {
            //EPExpenseClaim claim = Base.ExpenseClaim.Current;
            //if (claim != null)
            //{
            //    ATPTEFMEPExpenseClaimExt claimExt = claim.GetExtension<ATPTEFMEPExpenseClaimExt>();
            //    HashSet<string> DeletedReceipts = new HashSet<string>();

            //    DateTime finPeriodID = (DateTime)claim.DocDate;
            //    var finID = $"{finPeriodID.Year}{finPeriodID.Month:00}";

            //    MasterFinPeriod period = PXSelect<
            //        MasterFinPeriod,
            //        Where<MasterFinPeriod.finPeriodID, Equal<Required<MasterFinPeriod.finPeriodID>>>>
            //        .Select(Base, finID);

            //    #region Budget Validation

            //    (bool, bool) validateBudget = (claimExt.UsrATPTEFMBudgetEnabled ?? false, ATPTEFMProjectBudgetLibrary.ProjectBudgetVisible(ATPTEFMFeatureSetup.Current, "P"));
            //    bool BudgetValidate = (ATPTEFMFeatureSetup?.Current?.BudgetValidation ?? RQRequestClassBudget.None) == RQRequestClassBudget.Error;

            //    if (BudgetValidate && (validateBudget.Item1 || validateBudget.Item2))
            //    {
            //        using (PXTransactionScope ts = new PXTransactionScope())
            //        {
            //            (bool, bool) isOverbudget = (
            //                ATPTEFMBudget?.Select()?.RowCast<ATPTEFMBudget>()?.ToList()?.Any(x => (x.BudgetAmt ?? 0m) < 0) ?? false,
            //                ATPTEFMProjectBudget?.Select()?.RowCast<ATPTEFMPBudget>()?.ToList()?.Any(x => (x.BudgetAmt ?? 0m) < 0) ?? false
            //            );

            //            if (validateBudget.Item1)
            //            {
            //                if (isOverbudget.Item1)
            //                    throw new PXRowPersistedException(typeof(ATPTEFMBudget).Name, ts, ATPTEFMMessages.CheckBudget);
            //                ATPTEFMBudget.Cache.Persist(PXDBOperation.Insert);
            //                ATPTEFMBudget.Cache.Persist(PXDBOperation.Update);
            //            }
            //            if (validateBudget.Item2)
            //            {
            //                foreach (ATPTEFMPBudget row in ATPTEFMProjectBudget.Select())
            //                {
            //                    if (row.ProjectID == null || row.ProjectTaskID == null || row.CostCodeID == null) continue;

            //                    ATPTEFMProjectBudgetLineSummary PBSummary = ATPTEFMProjectBudgetSummary.Select(row.ProjectID, row.ProjectTaskID, row.CostCodeID, period.FinYear.ToString());

            //                    if (PBSummary == null)
            //                        throw new PXRowPersistedException(typeof(ATPTEFMPBudget).Name, ts, ATPTEFMMessages.NotInProjectBudget);
            //                }

            //                if (isOverbudget.Item2)
            //                    throw new PXRowPersistedException(typeof(ATPTEFMPBudget).Name, ts, ATPTEFMMessages.CheckProjectBudget);
            //                ATPTEFMProjectBudget.Cache.Persist(PXDBOperation.Insert);
            //                ATPTEFMProjectBudget.Cache.Persist(PXDBOperation.Update);
            //            }

            //            ts.Complete(Base);
            //        }
            //        if (validateBudget.Item1) ATPTEFMBudget.Cache.Persisted(false);
            //        if (validateBudget.Item2) ATPTEFMProjectBudget.Cache.Persisted(false);
            //    }

            //    #endregion

            //    #region Financial Period Validation
            //    bool isMultipleCalendarSupport = PXAccess.FeatureInstalled<FeaturesSet.multipleCalendarsSupport>();
            //    if (claimExt.UsrATPTEFMTranType == ATPTEFMExpenseTypeAttribute.RequestforPayment)
            //    {
            //        DateTime periodDate = (DateTime)period.StartDate;

            //        if (isMultipleCalendarSupport == false && period.Status == FinPeriod.status.Inactive)
            //        {
            //            string finPeriodErrMsg = $"The {periodDate.ToString("MM-yyyy")} financial period is inactive.";

            //            Base.ExpenseClaim.Cache.RaiseExceptionHandling<EPExpenseClaim.docDate>(claim,
            //                   claim.DocDate,
            //                   ATPTEFMHelper.GetPropertyException(claim, finPeriodErrMsg, PXErrorLevel.Error));
            //        }
            //    }
            //    #endregion

            //    #region Generate Autonumber For New Request Class ID
            //    if (Base.ExpenseClaim.Cache.GetStatus(Base.ExpenseClaim.Current) == PXEntryStatus.Updated)
            //    {
            //        EPExpenseClaim currentClaim = PXSelectReadonly<
            //            EPExpenseClaim,
            //            Where<EPExpenseClaim.refNbr, Equal<Required<EPExpenseClaim.refNbr>>>>
            //            .Select(Base, Base.ExpenseClaim.Current.RefNbr);
            //        if (currentClaim != null)
            //        {
            //            ATPTEFMEPExpenseClaimExt currentClaimExt = currentClaim.GetExtension<ATPTEFMEPExpenseClaimExt>();
            //            if (Base.ExpenseClaim.Current.GetExtension<ATPTEFMEPExpenseClaimExt>().UsrATPTEFMReqClass != currentClaimExt.UsrATPTEFMReqClass)
            //            {
            //                ATPTEFMReqClass numberingType = PXSelect<
            //                    ATPTEFMReqClass,
            //                    Where<ATPTEFMReqClass.reqClassID, Equal<Required<ATPTEFMReqClass.reqClassID>>>>
            //                    .Select(Base, Base.ExpenseClaim.Current.GetExtension<ATPTEFMEPExpenseClaimExt>().UsrATPTEFMReqClass);
            //                ATPTEFMReqClass numberingTypeZ = PXSelect<
            //                    ATPTEFMReqClass,
            //                    Where<ATPTEFMReqClass.reqClassID, Equal<Required<ATPTEFMReqClass.reqClassID>>>>
            //                    .Select(Base, currentClaimExt.UsrATPTEFMReqClass);
            //                if (Base.ExpenseClaim.Current.GetExtension<ATPTEFMEPExpenseClaimExt>().UsrATPTEFMTranType == ATPTEFMExpenseTypeAttribute.RequestforPayment && (numberingType.ReqClassID != numberingTypeZ.ReqClassID))
            //                {
            //                    foreach (EPExpenseClaimDetails claimDetail in Base.ExpenseClaimDetails.Select())
            //                    {
            //                        Base.ExpenseClaimDetails.Current = claimDetail;
            //                        Base.ExpenseClaimDetails.Current.GetExtension<ATPTEFMEPExpenseClaimDetailsExt>().UsrATPTEFMReqClass = claimExt.UsrATPTEFMReqClass;
            //                        Base.ExpenseClaimDetails.UpdateCurrent();
            //                    }
            //                    Base.ExpenseClaim.Current.GetExtension<ATPTEFMEPExpenseClaimExt>().UsrATPTEFMRFPReqRef = AutoNumberAttribute.GetNextNumber(Base.ExpenseClaim.Cache, Base.ExpenseClaim.Current, numberingType.NumberingID, Base.Accessinfo.BusinessDate);
            //                    Base.ExpenseClaim.UpdateCurrent();
            //                }
            //                else if (Base.ExpenseClaim.Current.GetExtension<ATPTEFMEPExpenseClaimExt>().UsrATPTEFMTranType == ATPTEFMExpenseTypeAttribute.Liquidation && (numberingType.ReqClassID != numberingTypeZ.ReqClassID))
            //                {
            //                    Base.ExpenseClaim.Current.GetExtension<ATPTEFMEPExpenseClaimExt>().UsrATPTEFMLiqNbr = AutoNumberAttribute.GetNextNumber(Base.ExpenseClaim.Cache, Base.ExpenseClaim.Current, numberingType.NumberingID, Base.Accessinfo.BusinessDate);
            //                    Base.ExpenseClaim.UpdateCurrent();
            //                }
            //            }
            //        }
            //    }
            //    #endregion 
            //}
        }
        private void OnBeforeGraphCommit(PXGraph obj)
        {
            //List<ATPTEFMBudget> BudgetList = new List<ATPTEFMBudget>();
            //List<ATPTEFMPBudget> PBudgetList = new List<ATPTEFMPBudget>();
            //ATPTEFMBudgetEntry graph = PXGraph.CreateInstance<ATPTEFMBudgetEntry>();
            //bool isDeleted = Base.ExpenseClaim.Cache.Deleted.Any_() ? true : false;
            //EPExpenseClaim claim = isDeleted ? Base.ExpenseClaim.Cache.Deleted.FirstOrDefault_() as EPExpenseClaim : Base.ExpenseClaim.Current;
            //bool isCancelled = claim == null ? false : claim.Status == EPExpenseClaimStatus.RejectedStatus ? true : false; //Review logic from budget/pbudget, claim is only cancelled but data condition specifies deleted records when there is none
            //HashSet<string> DeletedReceipts = new HashSet<string>();

            //if (claim != null)
            //{
            //    ATPTEFMEPExpenseClaimExt claimExt = claim.GetExtension<ATPTEFMEPExpenseClaimExt>();

            //    //Budget And Pbudget NOTE: This deletion logic does not work because the EC details does not get deleted, only the EC.
            //    if (isDeleted)
            //    {
            //        List<EPExpenseClaimDetails> curLines = new List<EPExpenseClaimDetails>();
            //        foreach (EPExpenseClaimDetails item in Base.ExpenseClaimDetails.Cache.Deleted) { curLines.Add(item); }

            //        #region BudgetHistory
            //        foreach (EPExpenseClaimDetails item in curLines)
            //        {
            //            var row = new ATPTEFMBudget();
            //            row.AcctID = item.ExpenseAccountID;
            //            row.SubID = item.ExpenseSubID;
            //            row.RefNbr = item.RefNbr;
            //            row.Origin = (int)ATPTEFMBudgetLibrary.OriginTypes.RequestForPayment;
            //            BudgetList.Add(row);

            //            var pRow = new ATPTEFMPBudget();
            //            pRow.ProjectID = item.ContractID;
            //            pRow.ProjectTaskID = item.TaskID;
            //            pRow.CostCodeID = item.CostCodeID;
            //            pRow.RefNbr = item.RefNbr;
            //            pRow.Origin = (int)ATPTEFMBudgetLibrary.OriginTypes.RequestForPayment;
            //            PBudgetList.Add(pRow);
            //        }
            //        graph.DeleteBudgetHistory(BudgetList);
            //        graph.DeletePBudgetHistory(PBudgetList);

            //        ATPTEFMBudget.Select();

            //        if (claimExt.UsrATPTEFMTranType == ATPTEFMExpenseTypeAttribute.Liquidation)
            //        {
            //            #region Retrieve all deleted rows in claim details
            //            foreach (EPExpenseClaimDetails vals in Base.ExpenseClaimDetails.Cache.Updated.RowCast<EPExpenseClaimDetails>()
            //                                           .Where(p => string.IsNullOrEmpty(p.RefNbr)))
            //            {
            //                DeletedReceipts.Add(vals.ClaimDetailCD);
            //            }
            //            #endregion
            //            if (DeletedReceipts.Count > 0)
            //            {
            //                ATPTEFMCashAdvanceEntry caEntry = PXGraph.CreateInstance<ATPTEFMCashAdvanceEntry>();

            //                foreach (var deletedReceiptCD in DeletedReceipts)
            //                {
            //                    PXResultset<ATPTEFMCAReceiptDetail, ATPTEFMCashAdvance> details = PXSelectJoin<
            //                        ATPTEFMCAReceiptDetail,
            //                        InnerJoin<ATPTEFMCashAdvance,
            //                            On<ATPTEFMCashAdvance.cashAdvanceNbr, Equal<ATPTEFMCAReceiptDetail.cashAdvanceNbr>>>,
            //                        Where<ATPTEFMCAReceiptDetail.expenseReceiptRefNbr, Equal<Required<ATPTEFMCAReceiptDetail.expenseReceiptRefNbr>>>>
            //                        .Select<PXResultset<ATPTEFMCAReceiptDetail, ATPTEFMCashAdvance>>
            //                          (Base, deletedReceiptCD);

            //                    ATPTEFMCashAdvance ca = details;
            //                    ATPTEFMCAReceiptDetail caDetails = details;

            //                    if (ca != null && caDetails != null)
            //                    {
            //                        caEntry.CashAdvances.Current = ca;
            //                        caDetails.LiquidationRef = null;
            //                        caEntry.CashAdvanceReceiptLines.Update(caDetails);
            //                        caEntry.Save.Press();
            //                    }
            //                }
            //            }
            //        }
            //        #endregion
            //    }
            //    else
            //    {
            //        DateTime finPeriodID = (DateTime)claim.DocDate;
            //        var finID = $"{finPeriodID.Year}{finPeriodID.Month:00}";

            //        MasterFinPeriod period = PXSelect<
            //            MasterFinPeriod,
            //            Where<MasterFinPeriod.finPeriodID, Equal<Required<MasterFinPeriod.finPeriodID>>>>
            //            .Select(Base, finID);

            //        #region Budget History
            //        if (claimExt.UsrATPTEFMBudgetEnabled ?? false)
            //        {
            //            //Inserts a list with no account id and sub id, causes error
            //            //BudgetList.Add(new ATPTEFMBudget() { RefNbr = curRecord.RefNbr, Origin = (int)ATPTEFMBudgetLibrary.OriginTypes.RequestForPayment });
            //            foreach (ATPTEFMBudget item in ATPTEFMBudget.Select())
            //            {
            //                var row = item;
            //                row.IsApproved = claim.Approved ?? false;
            //                BudgetList.Add(row);
            //            }
            //            graph.AddBudgetHistory(BudgetList);
            //        }

            //        if (ATPTEFMProjectBudgetLibrary.ProjectBudgetVisible(ATPTEFMFeatureSetup.Current, "P"))
            //        {
            //            PBudgetList.Add(new ATPTEFMPBudget() { RefNbr = claim.RefNbr, Origin = (int)ATPTEFMBudgetLibrary.OriginTypes.RequestForPayment });
            //            foreach (ATPTEFMPBudget item in ATPTEFMProjectBudget.Select())
            //            {
            //                var row = item;
            //                row.IsApproved = claim.Approved ?? false;
            //                PBudgetList.Add(row);
            //            }
            //            graph.AddPBudgetHistory(PBudgetList);
            //        }
            //        ATPTEFMBudget.Select();
            //        #endregion

            //        #region CA Update
            //        if (claimExt.UsrATPTEFMTranType == ATPTEFMExpenseTypeAttribute.Liquidation)
            //        {
            //            #region Retrieve All Empty Liquidation Ref Nbr
            //            var ErWithNoLiqNbr = PXSelectJoin<
            //                                                                                    ATPTEFMCAReceiptDetail,
            //                                                                                    InnerJoin<EPExpenseClaimDetails,
            //                                                                                        On<EPExpenseClaimDetails.claimDetailCD, Equal<ATPTEFMCAReceiptDetail.expenseReceiptRefNbr>>,
            //                                                                                    InnerJoin<EPExpenseClaim,
            //                                                                                        On<EPExpenseClaim.refNbr, Equal<EPExpenseClaimDetails.refNbr>>>>,
            //                                                                                    Where<EPExpenseClaim.refNbr, Equal<Required<EPExpenseClaim.refNbr>>,
            //                                                                                        And<ATPTEFMCAReceiptDetail.liquidationRef, IsNull>>>
            //                                                                                    .Select(Base, claim.RefNbr)
            //                                                                                    .ToList();

            //            if (ErWithNoLiqNbr.Count > 0)
            //            {
            //                ATPTEFMCashAdvanceEntry caEntry = PXGraph.CreateInstance<ATPTEFMCashAdvanceEntry>();


            //                foreach (ATPTEFMCAReceiptDetail receipt in ErWithNoLiqNbr)
            //                {

            //                    PXResultset<ATPTEFMCAReceiptDetail, ATPTEFMCashAdvance> details = PXSelectJoin<
            //                        ATPTEFMCAReceiptDetail,
            //                        InnerJoin<ATPTEFMCashAdvance,
            //                            On<ATPTEFMCashAdvance.cashAdvanceNbr, Equal<ATPTEFMCAReceiptDetail.cashAdvanceNbr>>>,
            //                        Where<ATPTEFMCAReceiptDetail.expenseReceiptRefNbr, Equal<Required<ATPTEFMCAReceiptDetail.expenseReceiptRefNbr>>>>
            //                        .Select<PXResultset<ATPTEFMCAReceiptDetail, ATPTEFMCashAdvance>>
            //                          (Base, receipt.ExpenseReceiptRefNbr);

            //                    ATPTEFMCashAdvance ca = details;
            //                    ATPTEFMCAReceiptDetail caDetails = details;

            //                    if (ca != null && caDetails != null)
            //                    {
            //                        caEntry.CashAdvances.Current = ca;
            //                        caDetails.LiquidationRef = claimExt.UsrATPTEFMLiqNbr;
            //                        caEntry.CashAdvanceReceiptLines.Update(caDetails);
            //                        caEntry.Save.Press();
            //                    }
            //                }
            //            }

            //            #endregion
            //        }
            //        #endregion

            //        #region Vendor Details

            //        Numbering claimNumbering = PXSelect<
            //            Numbering,
            //            Where<Numbering.numberingID, Equal<Required<Numbering.numberingID>>>>
            //            .Select(Base, Base.epsetup.Current.ClaimNumberingID);

            //        if (claimExt?.UsrATPTEFMTranType == ATPTEFMExpenseTypeAttribute.RequestforPayment && claim.Status == ATPTEFMExpenseClaimStatusAttribute.HoldValue && (Base.ExpenseClaim.Current.RefNbr.Trim() != claimNumbering.NewSymbol.Trim()))
            //        {
            //            ExpenseClaimDetailEntry entry = PXGraph.CreateInstance<ExpenseClaimDetailEntry>();

            //            if (Base.ExpenseClaimDetails.Select().Count != 0)
            //            {
            //                Numbering numbering = PXSelect<
            //                    Numbering,
            //                    Where<Numbering.numberingID, Equal<Required<Numbering.numberingID>>>>
            //                    .Select(Base, Base.epsetup.Current.ReceiptNumberingID);
            //                foreach (EPExpenseClaimDetails receipts in Base.ExpenseClaimDetails.Select())
            //                {
            //                    entry.Clear();
            //                    if (Base.ExpenseClaimDetails.Cache.GetStatus(receipts) == PXEntryStatus.Updated)
            //                    {
            //                        ATPTEFMEPExpenseClaimDetailsExt receiptsExt = receipts.GetExtension<ATPTEFMEPExpenseClaimDetailsExt>();
            //                        if (receipts.ClaimDetailCD.Trim() == numbering.NewSymbol.Trim()) continue;

            //                        EPExpenseClaimDetails epDetails = PXSelect<
            //                            EPExpenseClaimDetails,
            //                            Where<EPExpenseClaimDetails.claimDetailCD, Equal<Required<EPExpenseClaimDetails.claimDetailCD>>>>
            //                            .Select(Base, receipts.ClaimDetailCD);

            //                        ATPTEFMEPExpenseClaimDetailsExt epDetailsExt = epDetails.GetExtension<ATPTEFMEPExpenseClaimDetailsExt>();
            //                        if (epDetails != null && receiptsExt.UsrATPTVendID != null)
            //                        {
            //                            entry.CurrentClaimDetails.Current = epDetails;
            //                            entry.CurrentClaimDetails.Cache.SetValueExt<ATPTEFMEPExpenseClaimDetailsExt.usrATPTVendID>(epDetails, receiptsExt.UsrATPTVendID);
            //                            entry.CurrentClaimDetails.Update(epDetails);
            //                            entry.CurrentClaimDetails.UpdateCurrent();
            //                        }
            //                        else
            //                        {
            //                            entry.CurrentClaimDetails.Current = epDetails;
            //                            entry.CurrentClaimDetails.Update(epDetails);
            //                            entry.CurrentClaimDetails.UpdateCurrent();
            //                        }
            //                    }
            //                }
            //            }
            //        }
            //        #endregion
            //    }
            //}
        }
        protected virtual void _(Events.RowSelecting<EPExpenseClaim> e, PXRowSelecting baseEvent)
        {
            if (baseEvent != null) baseEvent.Invoke(e.Cache, e.Args);
            if (!IsActive()) return;

            EPExpenseClaim ec = e.Row;
            ATPTEFMEPExpenseClaimExt rowExt = ec.GetExtension<ATPTEFMEPExpenseClaimExt>();
            if (ec == null) return;

            using (new PXConnectionScope())
            {
                ATPTEFMVendor.Current = ATPTEFMVendor.Select(rowExt.UsrATPTVendorID);
                ATPTEFMnumbering.Current = ATPTEFMnumbering.Select(Base.epsetup.Current.ReceiptNumberingID);
            }
        }

        /// <remarks>
        /// 2024-10-07: Disable add receipts grid button when status is cancelled. CASEID: 007954  {JLG} <br/>
        /// 2026-03-10: 015602 - CFM 2025R2 - RFP Go to Next Record [Error] {JCL} <br/>
        /// 03-23-2026 : Disable RFP Ref Nbr field on header to prevent clearing; add RowPersisting validation {RFS}
        /// </remarks>

        protected virtual void _(Events.RowSelected<EPExpenseClaim> e, PXRowSelected baseMethod)
        {
            if (baseMethod != null) baseMethod(e.Cache, e.Args);
            if (!IsActive()) return;

            EPExpenseClaim row = (EPExpenseClaim)e.Row;
            ATPTEFMEPExpenseClaimExt rowExt = row.GetExtension<ATPTEFMEPExpenseClaimExt>();

            if (row == null || rowExt == null) return;

            bool isNotCancelled = row.Status != ATPTEFMExpenseClaimStatusAttribute.CancelledValue;

            #region Budget Visibility

            //ATPTEFMBudget.AllowSelect = rowExt.UsrATPTEFMBudgetEnabled ?? false;

            #endregion

            #region Project Budget Visibility

            //ATPTEFMProjectBudget.AllowSelect = rowExt.UsrATPTEFMProjectBudgetEnabled ?? false;

            #endregion

            #region Enable/Disable Fields

            PXUIFieldAttribute.SetEnabled(ATPTEFMAPPaymentDocument.Cache, null, false);
            PXUIFieldAttribute.SetEnabled(Base.ExpenseClaimDetails.Cache, "UsrATPTEFMRFPReqRef", false);
            PXUIFieldAttribute.SetEnabled(Base.ExpenseClaim.Cache, "UsrATPTEFMReqType", false);
            PXUIFieldAttribute.SetEnabled<ATPTEFMEPExpenseClaimExt.usrATPTEFMReqType>(e.Cache, e.Row, false);
            PXUIFieldAttribute.SetEnabled<ATPTEFMEPExpenseClaimExt.usrATPTEFMLiqNbr>(e.Cache, e.Row, false);

            bool isRFP = rowExt.UsrATPTEFMTranType == ATPTEFMExpenseTypeAttribute.RequestforPayment;
            PXDefaultAttribute.SetPersistingCheck<ATPTEFMEPExpenseClaimExt.usrATPTEFMRFPReqRef>(e.Cache, e.Row,
                isRFP ? PXPersistingCheck.NullOrBlank : PXPersistingCheck.Nothing);
            PXUIFieldAttribute.SetRequired<ATPTEFMEPExpenseClaimExt.usrATPTEFMRFPReqRef>(e.Cache, isRFP);

            #endregion

            #region Set Field Visibility

            PXUIFieldAttribute.SetVisible(Base.ExpenseClaimDetails.Cache, "UsrATPTEFMReqRef", rowExt.UsrATPTEFMTranType == ATPTEFMExpenseTypeAttribute.Liquidation);
            PXUIFieldAttribute.SetVisible(Base.ExpenseClaimDetails.Cache, "UsrATPTEFMRFPReqRef", rowExt.UsrATPTEFMTranType == ATPTEFMExpenseTypeAttribute.RequestforPayment);

            PXUIFieldAttribute.SetVisible<ATPTEFMEPExpenseClaimExt.usrATPTEFMIsOverbudget>(e.Cache, row, Classes.ATPTEFMBudgetLibrary.BudgetVisible(ATPTEFMFeatureSetup?.Current, "P"));
            PXUIFieldAttribute.SetVisible<ATPTEFMEPExpenseClaimExt.usrATPTEFMHasInitialBudget>(e.Cache, row, Classes.ATPTEFMBudgetLibrary.BudgetVisible(ATPTEFMFeatureSetup?.Current, "P"));

            #endregion

            #region Views Control

            Base.ExpenseClaim.AllowUpdate = isNotCancelled;
            Base.ExpenseClaimDetails.AllowUpdate = isNotCancelled;
            Base.ExpenseClaimDetails.AllowInsert = isNotCancelled;
            Base.ExpenseClaimDetails.AllowDelete = isNotCancelled;
            Base.ExpenseClaimCurrent.AllowUpdate = isNotCancelled;

            #endregion

            #region Enable/Disable Action Buttons
            bool manualNumbering = ATPTEFMnumbering.Current != null && (ATPTEFMnumbering.Current.UserNumbering ?? false);
            Base.createNew.SetEnabled(isNotCancelled && Base.ExpenseClaimDetails.Cache.AllowInsert && e.Cache.GetStatus(row) != PXEntryStatus.Inserted && !manualNumbering);
            Base.showSubmitReceipt.SetEnabled(isNotCancelled && Base.ExpenseClaimDetails.Cache.AllowInsert);
            bool isLiquidation = rowExt.UsrATPTEFMTranType == ATPTEFMExpenseTypeAttribute.Liquidation;
            bool isRFPType = rowExt.UsrATPTEFMTranType == ATPTEFMExpenseTypeAttribute.RequestforPayment;
            ATPTEFMLiquidationForm.SetEnabled(isLiquidation && IsEnabled);
            ATPTEFMLiquidationForm.SetVisible(isLiquidation);
            ATPTEFMRequestPaymentForm.SetEnabled(isRFPType && IsEnabled);
            ATPTEFMRequestPaymentForm.SetVisible(isRFPType);

            ATPTEFMCancel.SetEnabled(row.Status == ATPTEFMExpenseClaimStatusAttribute.HoldValue || (rowExt.UsrATPTEFMEnableCancel ?? false && row.Status == EPExpenseClaimStatus.ReleasedStatus) || row.Status == EPExpenseClaimStatus.ApprovedStatus || row.Status == EPExpenseClaimStatus.RejectedStatus);

            #endregion

            #region Currency
            if (!String.IsNullOrEmpty(rowExt.UsrATPTEFMTranType))
            {
                if (rowExt.UsrATPTEFMTranType.Equals(ATPTEFMExpenseTypeAttribute.RequestforPayment))
                {
                    VendorR vendor = ATPTEFMCurrentVendor.Current;

                    if (vendor != null)
                    {
                        PXUIFieldAttribute.SetEnabled<EPExpenseClaim.curyID>(e.Cache, row, vendor.AllowOverrideCury ?? false);
                    }
                }
            }
            #endregion
        }
        protected virtual void _(Events.RowUpdated<EPExpenseClaimDetails> e, PXRowUpdated baseEvent)
        {
            if (baseEvent != null) baseEvent(e.Cache, e.Args);

            EPExpenseClaimDetails row = Base.ExpenseClaimDetails.Current;
            if (row == null) return;

            //object refRow = row.ExpenseRefNbr;
            //Base.ExpenseClaimDetails.Cache.RaiseFieldVerifying<EPExpenseClaimDetails.expenseRefNbr>(row, ref refRow);
        }
        /// <remarks>
        /// 010393 - CFM 2024R1 : RFP Population of Vendor Details
        /// </remarks>
        protected virtual void _(Events.RowInserting<EPExpenseClaimDetails> e, PXRowInserting baseMethod)
        {
            if (baseMethod != null) baseMethod(e.Cache, e.Args);
            if (!IsActive()) return;

            EPExpenseClaim row = Base.ExpenseClaim.Current;
            ATPTEFMEPExpenseClaimExt rowExt = row.GetExtension<ATPTEFMEPExpenseClaimExt>();

            if (row == null) return;

            EPExpenseClaimDetails details = (EPExpenseClaimDetails)e.Row;
            ATPTEFMEPExpenseClaimDetailsExt detailsExt = details.GetExtension<ATPTEFMEPExpenseClaimDetailsExt>();
            if (rowExt.UsrATPTEFMTranType == ATPTEFMExpenseTypeAttribute.Liquidation)
            {
                detailsExt.UsrATPTEFMTranType = ATPTEFMExpenseTypeAttribute.Liquidation;
                detailsExt.UsrATPTEFMReqType = ATPTEFMTranTypeAttribute.CashAdvance;
            }
            else
            {
                detailsExt.UsrATPTEFMTranType = ATPTEFMExpenseTypeAttribute.RequestforPayment;
                detailsExt.UsrATPTEFMReqType = ATPTEFMTranTypeAttribute.RequestforPayment;
                detailsExt.UsrATPTVendorID = rowExt.UsrATPTVendorID;

                if (rowExt.UsrATPTVendorID != null)
                {
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

                        Base.ExpenseClaimDetails.Cache.SetValueExt<Extensions.DAC.ATPTEFMEPExpenseClaimDetailsExt.usrATPTEFMDetailVendorID>(details, vendor.BAccountID);
                        Base.ExpenseClaimDetails.Cache.SetValueExt<Extensions.DAC.ATPTEFMEPExpenseClaimDetailsExt.usrATPTVendID>(details, vendor.AcctCD);
                        Base.ExpenseClaimDetails.Cache.SetValueExt<Extensions.DAC.ATPTEFMEPExpenseClaimDetailsExt.usrATPTVendName>(details, vendor.AcctName);
                        Base.ExpenseClaimDetails.Cache.SetValueExt<Extensions.DAC.ATPTEFMEPExpenseClaimDetailsExt.usrATPTAddress>(details, address?.AddressLine1);
                        Base.ExpenseClaimDetails.Cache.SetValueExt<Extensions.DAC.ATPTEFMEPExpenseClaimDetailsExt.usrATPTVendTIN>(details, location?.TaxRegistrationID);
                    }
                }
            }
            detailsExt.UsrATPTEFMReqClass = rowExt.UsrATPTEFMReqClass;
        }
        protected virtual void _(Events.RowPersisting<EPExpenseClaim> e, PXRowPersisting baseMethod)
        {
            if (baseMethod != null) baseMethod(e.Cache, e.Args);
            if (!IsActive()) return;

            ATPTEFMEPExpenseClaimExt rowExt = e.Row.GetExtension<ATPTEFMEPExpenseClaimExt>();
            ATPTEFMReqClass reqclass = ATPTEFMReqClass.PK.Find(Base, rowExt.UsrATPTEFMReqType, rowExt.UsrATPTEFMReqClass);
            if (e.Row != null && e.Row.LastModifiedByScreenID != "ATPT9201")
            {
                #region Check if same Transaction type, Request Type and Request Class of receipts . this is only triggered from Expense receipts process page
                if (Base.UnattendedMode)
                {
                    EPExpenseClaimDetails details = Base.ExpenseClaimDetails.Select().TopFirst;
                    ATPTEFMEPExpenseClaimDetailsExt detailsExt = details.GetExtension<ATPTEFMEPExpenseClaimDetailsExt>();
                    var results = Base.ExpenseClaimDetails.Select().RowCast<EPExpenseClaimDetails>().
                        Where(x => x.GetExtension<ATPTEFMEPExpenseClaimDetailsExt>().UsrATPTEFMTranType != detailsExt.UsrATPTEFMTranType &&
                                   x.GetExtension<ATPTEFMEPExpenseClaimDetailsExt>().UsrATPTEFMReqType != detailsExt.UsrATPTEFMReqType &&
                                   x.GetExtension<ATPTEFMEPExpenseClaimDetailsExt>().UsrATPTEFMReqClass != detailsExt.UsrATPTEFMReqClass).ToList();

                    if (results.Count != 0)
                    {
                        throw new Exception(ATPTEFMMessages.NotTheSameTypes);
                    }

                    rowExt.UsrATPTEFMTranType = detailsExt.UsrATPTEFMTranType;
                    rowExt.UsrATPTEFMReqType = detailsExt.UsrATPTEFMReqType;
                    rowExt.UsrATPTEFMReqClass = detailsExt.UsrATPTEFMReqClass;


                    //Commented this code for allowing different vendors on RFP transactions
                    /*if (detailsExt.UsrATPTEFMTranType == ATPTEFMExpenseTypeAttribute.RequestforPayment)
                    {
                        results = Base.ExpenseClaimDetails.Select().RowCast<EPExpenseClaimDetails>().Where(x => x.GetExtension<ATPTEFMEPExpenseClaimDetailsExt>().UsrATPTVendorID != detailsExt.UsrATPTVendorID).ToList();
                        if (results.Count != 0)
                        {
                            throw new Exception(ATPTEFMMessages.NotTheSameVendors);
                        }
                        rowExt.UsrATPTVendorID = detailsExt.UsrATPTVendorID;
                    }*/
                }
                #endregion


                if (rowExt.UsrATPTEFMTranType.Equals(ATPTEFMExpenseTypeAttribute.RequestforPayment))
                {
                    if ((e.Operation & PXDBOperation.Command) == PXDBOperation.Update
                        && string.IsNullOrWhiteSpace(rowExt.UsrATPTEFMRFPReqRef))
                    {
                        e.Cache.RaiseExceptionHandling<ATPTEFMEPExpenseClaimExt.usrATPTEFMRFPReqRef>(
                            e.Row, rowExt.UsrATPTEFMRFPReqRef,
                            new PXSetPropertyException(ATPTEFMMessages.RFPRefNbrCannotBeBlank, PXErrorLevel.Error));
                    }

                    if (reqclass != null)
                    {
                        if (reqclass.NumberingID != null)
                            ATPTEFMRFPRefNbrAutonumberAttribute.SetNumberingId<ATPTEFMEPExpenseClaimExt.usrATPTEFMRFPReqRef>(e.Cache, reqclass.NumberingID);

                        if (reqclass.RestrictMultInvIns == true)
                        {
                            List<int> inventoryids = new List<int>();
                            foreach (EPExpenseClaimDetails det in Base.ExpenseClaimDetails.Select())
                            {
                                inventoryids.Add(det.InventoryID ?? 0);
                            }

                            IEnumerable<int> duplicates = inventoryids.GroupBy(x => x).Where(g => g.Count() > 1).Select(x => x.Key);

                            if (duplicates != null && duplicates.Any())
                            {
                                throw new Exception(Messages.ATPTEFMMessages.DuplicateInventory);
                            }
                        }
                    }

                }
                if (rowExt.UsrATPTEFMTranType != ATPTEFMExpenseTypeAttribute.RequestforPayment)
                {
                    ATPTEFMCASetup liquidationNbr = ATPTEFMLiquidationNumber.SelectSingle();
                    if (liquidationNbr != null)
                    {
                        if (liquidationNbr.LiquidationNumberingID != null)
                            ATPTEFMLiquidationNbrAutonumberAttribute.SetNumberingId<ATPTEFMEPExpenseClaimExt.usrATPTEFMLiqNbr>(e.Cache, liquidationNbr.LiquidationNumberingID);
                    }
                }

                if (Base.ExpenseClaimDetails.Select().Count == 0 && Base.ExpenseClaim.Current.Status != ATPTEFMExpenseClaimStatusAttribute.CancelledValue) throw new PXException(ATPTEFMMessages.DetailsCannotBeEmpty);
                if (Base?.ExpenseClaim?.Current?.CuryDocBal == 0m && Base.ExpenseClaim.Current.Status != ATPTEFMExpenseClaimStatusAttribute.CancelledValue) throw new PXException(ATPTEFMMessages.ClaimTotalIsZero);
            }

            if ((rowExt.UsrATPTEFMBudgetEnabled ?? false) && (rowExt.UsrATPTEFMIsOverbudget ?? false) && ATPTEFMFeatureSetup.Current.BudgetValidation == RQRequestClassBudget.Error)
            {
                throw new PXException(ATPTEFMMessages.OverbudgetWarning);
            }
        }
        protected virtual void _(Events.RowPersisting<EPExpenseClaimDetails> e, PXRowPersisting baseMethod)
        {
            if (baseMethod != null) baseMethod(e.Cache, e.Args);
            if (!IsActive()) return;

            EPExpenseClaimDetails row = (EPExpenseClaimDetails)e.Row;
            EPExpenseClaim claim = Base.ExpenseClaimCurrent.Current;
            ATPTEFMEPExpenseClaimExt claimExt = claim.GetExtension<ATPTEFMEPExpenseClaimExt>();

            if (row is null) return;

            if (claimExt.UsrATPTEFMTranType.Equals(ATPTEFMExpenseTypeAttribute.RequestforPayment))
            {
                ATPTEFMReqClassItems reqClassItems = PXSelect<
                    ATPTEFMReqClassItems,
                    Where<ATPTEFMReqClassItems.tranType, Equal<Required<ATPTEFMReqClassItems.tranType>>,
                        And<ATPTEFMReqClassItems.reqClassID, Equal<Required<ATPTEFMReqClassItems.reqClassID>>,
                        And<ATPTEFMReqClassItems.inventoryID, Equal<Required<ATPTEFMReqClassItems.inventoryID>>>>>>
                    .Select(Base, ATPTEFMTranTypeAttribute.RequestforPayment, claimExt.UsrATPTEFMReqClass, row.InventoryID);

                if (reqClassItems != null)
                {
                    if (reqClassItems.IsPerDiem == true && row.CuryUnitCost > reqClassItems.Amount)
                    {
                        e.Cache.RaiseExceptionHandling<EPExpenseClaimDetails.curyUnitCost>(row, ((EPExpenseClaimDetails)e?.Row)?.CuryUnitCost, ATPTEFMHelper.GetPropertyException(row, ATPTEFMMessages.RequestAmountMustBeWithinLimitAmt, PXErrorLevel.Error));
                        throw new Exception(ATPTEFMMessages.RequestAmountMustBeWithinLimitAmt);
                    }
                }
            }
            BudgetValidationRaiseErrorForRequestAmountGreaterThanRemainingBudget(row, claimExt?.UsrATPTEFMBudgetEnabled ?? false);
            PBudgetValidationRaiseErrorForRequestAmountGreaterThanRemainingBudget(row, claimExt?.UsrATPTEFMProjectBudgetEnabled ?? false);

            //object refRow = row.ExpenseRefNbr;
            //Base.ExpenseClaimDetails.Cache.RaiseFieldVerifying<EPExpenseClaimDetails.expenseRefNbr>(row, ref refRow);
        }
        protected virtual void _(Events.RowDeleting<EPExpenseClaimDetails> e, PXRowDeleting baseMethod)
        {
            if (baseMethod != null) baseMethod(e.Cache, e.Args);
            if (!IsActive()) return;
            EPExpenseClaimDetails row = e.Row;
            if (row == null) return;

            #region Clear all Claim related Values
            row.ClaimCuryInfoID = null;
            row.ClaimCuryTaxRoundDiff = 0m;
            row.ClaimCuryTaxTotal = 0m;
            row.ClaimCuryTranAmt = 0m;
            row.ClaimCuryTranAmtWithTaxes = 0m;
            row.ClaimCuryVatExemptTotal = 0m;
            row.ClaimCuryVatTaxableTotal = 0m;
            row.ClaimTaxRoundDiff = 0m;
            row.ClaimTaxTotal = 0m;
            row.ClaimTranAmt = 0m;
            row.ClaimTranAmtWithTaxes = 0m;
            row.ClaimVatExemptTotal = 0m;
            row.ClaimVatTaxableTotal = 0m;
            #endregion
        }
        #endregion

        #region Fields
        /// <remarks>
        /// 03-23-2026 : RFP Ref Nbr acts as navigation-only on saved records, matching standard Acumatica RefNbr behavior (Bills and Adjustments AP301000) {RFS}
        /// </remarks>
        protected virtual void _(Events.FieldVerifying<EPExpenseClaim, ATPTEFMEPExpenseClaimExt.usrATPTEFMRFPReqRef> e)
        {
            if (!IsActive()) return;
            if (e.Row == null) return;
            if (!e.ExternalCall) return; // Skip programmatic changes (formulas, defaults, auto-number)

            ATPTEFMEPExpenseClaimExt rowExt = e.Row.GetExtension<ATPTEFMEPExpenseClaimExt>();
            if (rowExt.UsrATPTEFMTranType != ATPTEFMExpenseTypeAttribute.RequestforPayment) return;

            // On existing records, field acts as navigation-only (like standard Acumatica RefNbr)
            if (e.Cache.GetStatus(e.Row) != PXEntryStatus.Inserted)
            {
                string oldValue = rowExt.UsrATPTEFMRFPReqRef;
                string newValue = e.NewValue as string;

                if (!string.Equals(oldValue, newValue))
                {
                    e.NewValue = oldValue; // Silently revert to preserve current value

                    // If user typed a valid RFP Ref Nbr, navigate to that expense claim
                    if (!string.IsNullOrWhiteSpace(newValue))
                    {
                        EPExpenseClaim target = PXSelect<EPExpenseClaim,
                            Where<ATPTEFMEPExpenseClaimExt.usrATPTEFMRFPReqRef, Equal<Required<ATPTEFMEPExpenseClaimExt.usrATPTEFMRFPReqRef>>>>
                            .Select(Base, newValue);

                        if (target != null && target.RefNbr != e.Row.RefNbr)
                        {
                            ExpenseClaimEntry graph = PXGraph.CreateInstance<ExpenseClaimEntry>();
                            graph.ExpenseClaim.Current = graph.ExpenseClaim.Search<EPExpenseClaim.refNbr>(target.RefNbr);
                            throw new PXRedirectRequiredException(graph, true, ATPTEFMMessages.RFPRequestReference)
                            {
                                Mode = PXBaseRedirectException.WindowMode.Same
                            };
                        }
                    }
                }
            }
        }

        protected virtual void _(Events.FieldUpdated<EPExpenseClaim, ATPTEFMEPExpenseClaimExt.usrATPTEFMTranType> e)
        {
            if (!IsActive()) return;
            if (e.Row == null) return;
            EPExpenseClaim row = (EPExpenseClaim)e.Row;
            ATPTEFMEPExpenseClaimExt rowExt = row.GetExtension<ATPTEFMEPExpenseClaimExt>();
            if (row == null) return;

            if (Base.ExpenseClaim.Cache.GetStatus(Base.ExpenseClaim.Current) == PXEntryStatus.Updated)
            {
                row.RefNbr = AutoNumberAttribute.GetNewNumberSymbol<EPSetup.claimNumberingID>(Base.ExpenseClaim.Cache, row);
                Base.ExpenseClaim.Cache.Clear();
                Base.ExpenseClaimCurrent.Cache.Clear();
                DeleteDetails();

            }
            else if (Base.ExpenseClaim.Cache.GetStatus(Base.ExpenseClaim.Current) == PXEntryStatus.Inserted)
            {
                if (e.OldValue.ToString() == ATPTEFMExpenseTypeAttribute.RequestforPayment)
                {
                    rowExt.UsrATPTVendorID = null;
                    rowExt.UsrATPTEFMRFPReqRef = AutoNumberAttribute.GetNewNumberSymbol<ATPTEFMReqClass.numberingID>(Base.ExpenseClaim.Cache, row);
                }
                else
                {
                    rowExt.UsrATPTEFMLiqNbr = AutoNumberAttribute.GetNewNumberSymbol<ATPTEFMReqClass.numberingID>(Base.ExpenseClaim.Cache, row);
                }
                rowExt.UsrATPTEFMReqClass = null;

                DeleteDetails();
                //Base.ExpenseClaimDetails.Cache.Clear();
                //Base.ExpenseClaimDetailsCurrent.Cache.Clear();

                if (rowExt.UsrATPTEFMTranType == ATPTEFMExpenseTypeAttribute.RequestforPayment)
                {
                    if (ATPTEFMBudgetLibrary.BudgetVisible(ATPTEFMFeatureSetup.Current, "P"))
                        rowExt.UsrATPTEFMBudgetEnabled = true;

                    if (ATPTEFMProjectBudgetLibrary.ProjectBudgetVisible(ATPTEFMFeatureSetup.Current, "P"))
                        rowExt.UsrATPTEFMProjectBudgetEnabled = true;
                }
                else
                {
                    rowExt.UsrATPTEFMBudgetEnabled = false;
                    rowExt.UsrATPTEFMProjectBudgetEnabled = false;
                }
            }

            if (rowExt.UsrATPTEFMTranType == ATPTEFMExpenseTypeAttribute.RequestforPayment)
            {
                rowExt.UsrATPTEFMReqType = ATPTEFMTranTypeAttribute.RequestforPayment;
            }
            else
            {
                rowExt.UsrATPTEFMReqType = ATPTEFMTranTypeAttribute.CashAdvance;
            }

        }
        protected virtual void _(Events.FieldUpdated<EPExpenseClaim, ATPTEFMEPExpenseClaimExt.usrATPTEFMReqClass> e)
        {
            if (!IsActive()) return;
            if (e.Row == null) return;
            var row = e.Row;
            ATPTEFMEPExpenseClaimExt rowExt = row.GetExtension<ATPTEFMEPExpenseClaimExt>();
            var reqClass = CashFundManagement.DAC.Setup.ATPTEFMReqClass.PK.Find(Base, rowExt.UsrATPTEFMReqType, e.NewValue.ToString());
            if (reqClass != null)
            {
                if (reqClass.RestrictItemList == true)
                {
                    DeleteDetails();
                }
                else
                {
                    foreach (EPExpenseClaimDetails item in Base.ExpenseClaimDetails.Select())
                    {
                        Base.ExpenseClaimDetails.Cache.SetDefaultExt<EPExpenseClaimDetails.expenseAccountID>(item);
                        Base.ExpenseClaimDetails.Cache.SetDefaultExt<EPExpenseClaimDetails.expenseSubID>(item);
                    }
                }

            }
        }
        /// <remarks>
        /// 2025-03-26 : Remove FieldVerifying, transfer logic to Field Attribute - 010685 - RFS
        /// </remarks>
        //protected virtual void _(Events.FieldVerifying<EPExpenseClaimDetails, EPExpenseClaimDetails.expenseRefNbr> e, PXFieldVerifying baseEvent)
        //{
        //    if (baseEvent != null) baseEvent.Invoke(e.Cache, e.Args);
        //    if (!IsActive()) return;

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

        protected virtual void _(Events.FieldUpdated<EPExpenseClaimDetails, EPExpenseClaimDetails.curyExtCost> e, PXFieldUpdated baseEvent)
        {
            if (baseEvent != null) baseEvent(e.Cache, e.Args);
            if (!IsActive()) return;

            EPExpenseClaimDetails row = e.Row;
            if (row == null) return;

            EPExpenseClaim claim = Base.ExpenseClaim.Current;
            ATPTEFMEPExpenseClaimExt claimExt = claim.GetExtension<ATPTEFMEPExpenseClaimExt>();

            BudgetValidationRaiseErrorForRequestAmountGreaterThanRemainingBudget(row, claimExt?.UsrATPTEFMBudgetEnabled ?? false);
            PBudgetValidationRaiseErrorForRequestAmountGreaterThanRemainingBudget(row, claimExt?.UsrATPTEFMProjectBudgetEnabled ?? false);
        }
        protected virtual void _(Events.FieldUpdated<EPExpenseClaimDetails, EPExpenseClaimDetails.inventoryID> e)
        {
            if (!IsActive()) return;
            EPExpenseClaimDetails row = e.Row;
            EPExpenseClaim claim = Base.ExpenseClaimCurrent.Current;
            ATPTEFMEPExpenseClaimExt claimExt = claim.GetExtension<ATPTEFMEPExpenseClaimExt>();
            ATPTEFMEPExpenseClaimDetailsExt claimDetailsExt = row.GetExtension<ATPTEFMEPExpenseClaimDetailsExt>();

            if (row is null) return;

            if (claim != null)
            {
                ATPTEFMReqClassItems reqClassItems = PXSelect<
                    ATPTEFMReqClassItems,
                    Where<ATPTEFMReqClassItems.tranType, Equal<Required<ATPTEFMReqClassItems.tranType>>,
                        And<ATPTEFMReqClassItems.reqClassID, Equal<Required<ATPTEFMReqClassItems.reqClassID>>,
                        And<ATPTEFMReqClassItems.inventoryID, Equal<Required<ATPTEFMReqClassItems.inventoryID>>>>>>
                    .Select(Base, ATPTEFMTranTypeAttribute.RequestforPayment, claimExt.UsrATPTEFMReqClass, row.InventoryID);

                if (reqClassItems != null)
                {
                    if (reqClassItems.IsPerDiem == true)
                    {
                        row.CuryUnitCost = reqClassItems.Amount;
                        row.CuryExtCost = row.Qty * row.CuryUnitCost;
                    }
                    else
                    {
                        row.UnitCost = decimal.Zero;
                        row.CuryUnitCost = decimal.Zero;
                        row.CuryExtCost = row.Qty * row.CuryUnitCost;
                    }
                }
            }

            e?.Cache?.SetDefaultExt<EPExpenseClaimDetails.curyExtCost>(e?.Row);
            e?.Cache?.SetDefaultExt<EPExpenseClaimDetails.expenseAccountID>(e?.Row);
            e?.Cache?.SetDefaultExt<EPExpenseClaimDetails.expenseSubID>(e?.Row);
        }

        /// <remarks>
        /// 2025-01-24: Commented out this event handler as the expense account defaulting is now triggered 
        /// in the InventoryID_FieldUpdated event handler to ensure proper sequence of field updates.
        /// The SetDefaultExt for expenseAccountID is now called after inventory selection. CASE:009821 {JLTG}
        /// </remarks>
        /*protected virtual void _(Events.FieldUpdated<EPExpenseClaimDetails, EPExpenseClaimDetails.expenseAccountID> e, PXFieldUpdated baseMethod)
        {
            if (baseMethod != null) baseMethod(e.Cache, e.Args);
            if (!IsActive()) return;

            e.Cache.SetDefaultExt<EPExpenseClaimDetails.expenseSubID>(e.Row);
        }*/
        public void _(Events.FieldUpdated<EPExpenseClaimDetails, ATPTEFMEPExpenseClaimDetailsExt.usrATPTVendID> e)
        {
            EPExpenseClaimDetails row = e.Row;
            if (row == null) return;
            ATPTEFMEPExpenseClaimDetailsExt efmExt = row.GetExtension<ATPTEFMEPExpenseClaimDetailsExt>();

            EPExpenseClaim currentClaim = Base.ExpenseClaimCurrent.Current;
            ATPTEFMEPExpenseClaimExt claimExt = currentClaim.GetExtension<ATPTEFMEPExpenseClaimExt>();

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
                Base.ExpenseClaimDetails.Cache.SetValueExt<Extensions.DAC.ATPTEFMEPExpenseClaimDetailsExt.usrATPTVendName>(row, vendor.AcctName);
                Base.ExpenseClaimDetails.Cache.SetValueExt<Extensions.DAC.ATPTEFMEPExpenseClaimDetailsExt.usrATPTAddress>(row, address?.AddressLine1);
                Base.ExpenseClaimDetails.Cache.SetValueExt<Extensions.DAC.ATPTEFMEPExpenseClaimDetailsExt.usrATPTVendTIN>(row, location?.TaxRegistrationID);

                if ((Base.epsetup.Current.GetExtension<ATPTEFMEPSetupExtension>().UsrATPTEFMUseFinancialTabTaxZoneForDetails ?? false) && claimExt?.UsrATPTEFMTranType == ATPTEFMExpenseTypeAttribute.RequestforPayment)
                {
                    row.TaxZoneID = currentClaim.TaxZoneID;
                }
                else
                {
                    row.TaxZoneID = location?.VTaxZoneID;
                }
            }
            else
            {
                row.TaxZoneID = currentClaim.TaxZoneID;
            }
        }
        //protected virtual void _(Events.FieldDefaulting<EPExpenseClaim, ATPTEFMEPExpenseClaimExt.usrATPTEFMBudgetEnabled> e)
        //{
        //    EPExpenseClaim row = e.Row;
        //    if (row == null) return;

        //    ATPTEFMEPExpenseClaimExt rowExt = row.GetExtension<ATPTEFMEPExpenseClaimExt>();

        //    if (ATPTEFMBudgetLibrary.BudgetVisible(ATPTEFMFeatureSetup.Current, "P") && rowExt.UsrATPTEFMBudgetEnabled == null && rowExt.UsrATPTEFMTranType == ATPTEFMExpenseTypeAttribute.RequestforPayment)
        //    {
        //        e.NewValue = true;
        //        e.Cancel = true;
        //    }
        //    else
        //    {
        //        e.NewValue = false;
        //        e.Cancel = true;
        //    }
        //}
        //protected virtual void _(Events.FieldDefaulting<EPExpenseClaim, ATPTEFMEPExpenseClaimExt.usrATPTEFMProjectBudgetEnabled> e)
        //{
        //    EPExpenseClaim row = e.Row;
        //    if (row == null) return;

        //    ATPTEFMEPExpenseClaimExt rowExt = row.GetExtension<ATPTEFMEPExpenseClaimExt>();

        //    bool inserted = Base.ExpenseClaim.Cache.GetStatus(row) == PXEntryStatus.Inserted;

        //    if (ATPTEFMProjectBudgetLibrary.ProjectBudgetVisible(ATPTEFMFeatureSetup.Current, "P") && inserted && rowExt.UsrATPTEFMTranType == ATPTEFMExpenseTypeAttribute.RequestforPayment)
        //    {
        //        e.NewValue = true;
        //    }
        //    else
        //    {
        //        e.NewValue = false;
        //    }
        //}
        protected virtual void _(Events.FieldDefaulting<EPExpenseClaimDetails, EPExpenseClaimDetails.taxZoneID> e, PXFieldDefaulting baseMethod)
        {
            if (!IsActive())
            {
                baseMethod?.Invoke(e.Cache, e.Args);
                return;
            }
            EPExpenseClaimDetails row = (EPExpenseClaimDetails)e.Row;
            ATPTEFMEPExpenseClaimDetailsExt rowExt = row.GetExtension<ATPTEFMEPExpenseClaimDetailsExt>();

            EPExpenseClaim claim = Base.ExpenseClaimCurrent.Current;
            ATPTEFMEPExpenseClaimExt claimExt = claim.GetExtension<ATPTEFMEPExpenseClaimExt>();

            if (row == null) return;

            if (claimExt.UsrATPTVendorID != null)
            {
                Location taxZoneID = PXSelect<
                    Location,
                    Where<Location.bAccountID, Equal<Required<Location.bAccountID>>>>
                    .Select(Base, claimExt.UsrATPTVendorID);

                if ((Base.epsetup.Current.GetExtension<ATPTEFMEPSetupExtension>().UsrATPTEFMUseFinancialTabTaxZoneForDetails ?? false) && claimExt?.UsrATPTEFMTranType == ATPTEFMExpenseTypeAttribute.RequestforPayment)
                {
                    e.NewValue = claim.TaxZoneID;
                }
                else
                {
                    e.NewValue = taxZoneID?.VTaxZoneID;
                }

                e.Cancel = true;
            }
            else
            {
                baseMethod?.Invoke(e.Cache, e.Args);
            }

            e.Cancel = true;
        }
        /// <remarks>
        /// 2024-10-17 : This is for Employee Request Class Same Request Class ID produces wrong behavior in fetching expense accounts. CASEID: 008288 {JLTG} <br/>             
        /// </remarks>
        protected virtual void _(Events.FieldDefaulting<EPExpenseClaimDetails, EPExpenseClaimDetails.expenseAccountID> e, PXFieldDefaulting baseMethod)
        {
            if (!IsActive())
            {
                baseMethod?.Invoke(e.Cache, e.Args);
                return;
            }
            //Set Default Account ID

            EPExpenseClaim ec = Base.ExpenseClaim.Current;
            ATPTEFMEPExpenseClaimExt ecExt = ec.GetExtension<ATPTEFMEPExpenseClaimExt>();

            EPExpenseClaimDetails ecDet = e.Row;
            ATPTEFMEPExpenseClaimDetailsExt ecDetExt = ecDet.GetExtension<ATPTEFMEPExpenseClaimDetailsExt>();

            ATPTEFMReqClass reqclass = ATPTEFMReqClass.PK.Find(Base, ecExt.UsrATPTEFMReqType, ecExt.UsrATPTEFMReqClass);

            if (ecDet == null || ec == null) return;

            if (ecExt.UsrATPTEFMTranType == ATPTEFMExpenseTypeAttribute.Liquidation && ecDetExt.UsrATPTEFMReqRef != null && ecDet.InventoryID != null)
            {
                int? existingAccountID = null;

                foreach (ATPTEFMCARequestDetail caReq in PXSelectJoin<
                    ATPTEFMCARequestDetail,
                    InnerJoin<ATPTEFMCashAdvance,
                        On<ATPTEFMCashAdvance.cashAdvanceNbr, Equal<ATPTEFMCARequestDetail.cashAdvanceNbr>>>,
                    Where<ATPTEFMCashAdvance.cashAdvanceNbr, Equal<Required<ATPTEFMCashAdvance.cashAdvanceNbr>>>>
                    .Select(Base, ecDetExt.UsrATPTEFMReqRef))
                {
                    if (caReq.InventoryID == ecDet.InventoryID)
                    {
                        existingAccountID = caReq.AccountID;
                        break;
                    }
                }

                if (existingAccountID != null)
                {
                    e.NewValue = existingAccountID;
                    e.Cancel = true;
                }
                else
                {
                    e.NewValue = SetAccountID(reqclass, ec, ecDet);
                    e.Cancel = true;
                }
            }
            else if (ecExt.UsrATPTEFMTranType == ATPTEFMExpenseTypeAttribute.RequestforPayment)
            {
                e.NewValue = SetAccountID(reqclass, ec, ecDet);
                e.Cancel = true;
            }
            else
            {
                baseMethod(e.Cache, e.Args);
            }
        }
        /// <remarks>
        /// 2024-10-17 : This is for Employee Request Class Same Request Class ID produces wrong behavior in fetching expense sub accounts. CASEID: 008288 {JLTG} <br/>             
        /// </remarks>
        protected virtual void _(Events.FieldDefaulting<EPExpenseClaimDetails, EPExpenseClaimDetails.expenseSubID> e, PXFieldDefaulting baseMethod)
        {
            if (!IsActive())
            {
                baseMethod?.Invoke(e.Cache, e.Args);
                return;
            }
            EPExpenseClaimDetails row = (EPExpenseClaimDetails)e.Row;
            ATPTEFMEPExpenseClaimDetailsExt rowExt = row.GetExtension<ATPTEFMEPExpenseClaimDetailsExt>();

            EPExpenseClaim claim = Base.ExpenseClaimCurrent.Current;
            ATPTEFMEPExpenseClaimExt claimExt = claim.GetExtension<ATPTEFMEPExpenseClaimExt>();

            ATPTEFMReqClass reqClass = ATPTEFMReqClass.PK.Find(Base, claimExt.UsrATPTEFMReqType, claimExt.UsrATPTEFMReqClass);

            if (row == null || claim == null) return;

            if (claimExt.UsrATPTEFMTranType == ATPTEFMExpenseTypeAttribute.Liquidation && rowExt.UsrATPTEFMReqRef != null && row.InventoryID != null)
            {
                int? existingSubID = null;

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

                if (existingSubID != null)
                {
                    e.NewValue = existingSubID;
                    e.Cancel = true;
                }
                else
                {
                    object newSubID = SetSubID(reqClass, claim, row);
                    e.Cache.RaiseFieldUpdating<EPExpenseClaimDetails.expenseSubID>(e.Row, ref newSubID);
                    e.NewValue = (int?)newSubID;
                    e.Cancel = true;
                }
            }
            else if (claimExt.UsrATPTEFMTranType == ATPTEFMExpenseTypeAttribute.RequestforPayment)
            {
                object newSubID = SetSubID(reqClass, claim, row);
                e.Cache.RaiseFieldUpdating<EPExpenseClaimDetails.expenseSubID>(e.Row, ref newSubID);
                e.NewValue = (int?)newSubID;
                e.Cancel = true;
            }
            else
            {
                baseMethod(e.Cache, e.Args);
            }
        }
        protected virtual void _(Events.FieldUpdated<EPExpenseClaimDetails, ATPTEFMEPExpenseClaimDetailsExt.usrATPTEFMReqRef> e)
        {
            if (!IsActive()) return;
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

            e.Cache.SetDefaultExt<EPExpenseClaimDetails.expenseSubID>(e.Row);
        }
        #endregion

        #endregion

        #region Currency Events
        protected virtual void _(Events.FieldDefaulting<CurrencyInfo, CurrencyInfo.curyID> e, PXFieldDefaulting baseMethod)
        {
            if (baseMethod != null) baseMethod(e.Cache, e.Args);
            if (!IsActive()) return;
            EPExpenseClaim row = Base.ExpenseClaim.Current;
            ATPTEFMEPExpenseClaimExt rowExt = row.GetExtension<ATPTEFMEPExpenseClaimExt>();

            if (row != null)
            {
                if (rowExt.UsrATPTEFMTranType.Equals(ATPTEFMExpenseTypeAttribute.Liquidation))
                {
                    EPEmployee employee = Base.EPEmployee.Select(row.EmployeeID);

                    if (employee != null && !string.IsNullOrEmpty(employee.CuryID))
                    {
                        e.NewValue = employee.CuryID;
                        e.Cancel = true;
                    }
                }
                else
                {
                    if (PXAccess.FeatureInstalled<FeaturesSet.multicurrency>())
                    {
                        VendorR vendor = ATPTEFMVendor.Select(rowExt.UsrATPTVendorID);
                        if (vendor != null && !string.IsNullOrEmpty(vendor.CuryID))
                        {
                            e.NewValue = vendor.CuryID;
                            e.Cancel = true;
                        }
                    }
                }
            }
        }
        protected virtual void _(Events.FieldDefaulting<CurrencyInfo, CurrencyInfo.curyRateTypeID> e, PXFieldDefaulting baseMethod)
        {
            if (!IsActive())
            {
                baseMethod?.Invoke(e.Cache, e.Args);
                return;
            }
            EPExpenseClaim row = Base.ExpenseClaim.Current;
            ATPTEFMEPExpenseClaimExt rowExt = row.GetExtension<ATPTEFMEPExpenseClaimExt>();

            if (row is null) return;

            if (rowExt.UsrATPTEFMTranType.Equals(ATPTEFMExpenseTypeAttribute.Liquidation))
            {
                if (baseMethod != null) baseMethod(e.Cache, e.Args);
            }
            else
            {
                if (PXAccess.FeatureInstalled<FeaturesSet.multicurrency>())
                {
                    VendorR vendor = ATPTEFMVendor.Select(rowExt.UsrATPTVendorID);
                    if (vendor != null && !string.IsNullOrEmpty(vendor.CuryRateTypeID))
                    {
                        e.NewValue = vendor.CuryRateTypeID;
                        e.Cancel = true;
                    }
                }
            }
        }
        /// <remarks>
        /// Case 7771: Wrong setup (AllowOverrideCury) was used in enabling CuryRate fields <br>
        /// 2026-03-10: 015602 - CFM 2025R2 - RFP Go to Next Record [Error] {JCL} <br/>
        /// </remarks>
        protected virtual void _(Events.RowSelected<CurrencyInfo> e, PXRowSelected baseMethod)
        {
            if (!IsActive())
            {
                baseMethod?.Invoke(e.Cache, e.Args);
                return;
            }
            CurrencyInfo info = e.Row as CurrencyInfo;
            EPExpenseClaim row = Base.ExpenseClaim.Current;
            ATPTEFMEPExpenseClaimExt rowExt = row.GetExtension<ATPTEFMEPExpenseClaimExt>();

            if (info is null) return;
            if (row is null) return;

            if(!String.IsNullOrEmpty(rowExt.UsrATPTEFMTranType))
            {
                if (rowExt.UsrATPTEFMTranType.Equals(ATPTEFMExpenseTypeAttribute.Liquidation))
                {
                    if (baseMethod != null) baseMethod(e.Cache, e.Args);
                }
                else
                {
                    bool curyenabled = info.AllowUpdate(Base.ExpenseClaim.Cache);

                    VendorR vendor = ATPTEFMCurrentVendor.Current;

                    if (vendor != null && !(bool)vendor.AllowOverrideRate)
                    {
                        curyenabled = false;
                    }

                    PXUIFieldAttribute.SetEnabled<CurrencyInfo.curyRateTypeID>(e.Cache, info, curyenabled);
                    PXUIFieldAttribute.SetEnabled<CurrencyInfo.curyEffDate>(e.Cache, info, curyenabled);
                    PXUIFieldAttribute.SetEnabled<CurrencyInfo.sampleCuryRate>(e.Cache, info, curyenabled);
                    PXUIFieldAttribute.SetEnabled<CurrencyInfo.sampleRecipRate>(e.Cache, info, curyenabled);
                }
            }
        }
        protected virtual void _(Events.FieldUpdated<EPExpenseClaim, ATPTEFMEPExpenseClaimExt.usrATPTVendorID> e)
        {
            if (!IsActive()) return;
            EPExpenseClaim row = (EPExpenseClaim)e.Row;
            ATPTEFMEPExpenseClaimExt rowExt = row.GetExtension<ATPTEFMEPExpenseClaimExt>();
            if (row == null) return;

            if (rowExt.UsrATPTEFMTranType.Equals(ATPTEFMExpenseTypeAttribute.RequestforPayment))
            {
                VendorR vendor = ATPTEFMVendor.Select(rowExt.UsrATPTVendorID);
                ATPTEFMCurrentVendor.Current = vendor;
                if (PXAccess.FeatureInstalled<FeaturesSet.multicurrency>())
                {
                    CurrencyInfo info = CurrencyInfoAttribute.SetDefaults<EPExpenseClaim.curyInfoID>(e.Cache, e.Row);
                    if (vendor != null && !string.IsNullOrEmpty(vendor.CuryID))
                    {
                        e.Cache.SetValueExt<EPExpenseClaim.curyID>(e.Row, info.CuryID);
                    }
                }

                PXResultset<VendorR, Address, Location> vendorInfo = PXSelectJoin<
                    VendorR,
                    LeftJoin<Address,
                        On<Address.addressID, Equal<VendorR.defAddressID>>,
                    LeftJoin<Location,
                        On<Location.bAccountID, Equal<VendorR.defLocationID>>>>,
                    Where<VendorR.bAccountID, Equal<Required<VendorR.bAccountID>>>>
                    .Select<PXResultset<VendorR, Address, Location>>(Base, rowExt.UsrATPTVendorID);

                Vendor svendor = (Vendor)vendorInfo;
                Address address = (Address)vendorInfo;
                Location extAddress = (Location)vendorInfo;

                foreach (EPExpenseClaimDetails epDetail in Base.ExpenseClaimDetails.Select())
                {
                    Base.ExpenseClaimDetails.Cache.SetValueExt<Extensions.DAC.ATPTEFMEPExpenseClaimDetailsExt.usrATPTVendID>(epDetail, vendor.AcctCD);
                    Base.ExpenseClaimDetails.UpdateCurrent();
                }
            }
        }
        #endregion

        #region Methods
        public virtual APTran PBudgetIsReversed(EPExpenseClaimDetails receipt) => null;
        private void PBudgetValidationRaiseErrorForRequestAmountGreaterThanRemainingBudget(EPExpenseClaimDetails row, bool PBudgetEnabled)
        {
            ATPTEFMEPExpenseClaimDetailsExt rowExt = row.GetExtension<ATPTEFMEPExpenseClaimDetailsExt>();

            if (PBudgetEnabled && ATPTEFMFeatureSetup.Current.ProjectBudgetValidation == RQRequestClassBudget.Error)
            {
                foreach (ATPTEFMPBudget result in ATPTEFMProjectBudget.Select())
                {
                    if (ATPTEFMBudgetLibrary.HasNull(result.ProjectID, result.ProjectTaskID, result.CostCodeID, result.AccountGroupID)) continue;

                    if (row.ContractID == result.ProjectID && row.TaskID == result.ProjectTaskID && row.CostCodeID == result.CostCodeID && rowExt.UsrATPTEFMAccountGroup == result.AccountGroupID)
                    {
                        if (result.BudgetAmt < 0 && row.CuryExtCost > 0) Base.ExpenseClaimDetails.Cache.RaiseExceptionHandling<EPExpenseClaimDetails.curyExtCost>(row, row?.CuryExtCost, ATPTEFMHelper.GetPropertyException(row, ATPTEFMMessages.AmountRequestedIsGreaterThanRemainingBudget, PXErrorLevel.Error));
                    }
                }
            }
        }
        /// <remarks>
        /// 2024-09-30 : Missing claim details, when openned a record then adding new EC, this is due to Acumatica caching behavior. CASEID: 007320 {RRS} <br/>             
        /// </remarks>
        private void DeleteDetails()
        {
            foreach (EPExpenseClaimDetails item in Base.ExpenseClaimDetails.Cache.Cached)
            {
                #region skip records that is not related to the current records, cache.cahed retrieves records from previously opened transactions within the cache session. 
                var status = Base.ExpenseClaimDetails.Cache.GetStatus(item);
                if (status.Equals(PXEntryStatus.Notchanged)) continue;
                #endregion
                Base.ExpenseClaimDetails.Cache.Delete(item);
            }
        }
        public virtual string GetVendor(EPExpenseClaimDetails row) => string.Empty;
        public virtual string GetDefATC(EPExpenseClaimDetails row) => string.Empty;
        public virtual bool DuplicateERRefNbr(EPExpenseClaimDetails row, string checkRefNbr, string TranType)
        {
            return false;
        }
        private void BudgetValidationRaiseErrorForRequestAmountGreaterThanRemainingBudget(EPExpenseClaimDetails row, bool BudgetEnabled)
        {
            if (BudgetEnabled && ATPTEFMFeatureSetup.Current.BudgetValidation == RQRequestClassBudget.Error)
            {
                foreach (ATPTEFMBudget result in ATPTEFMBudget.Select())
                {
                    if (row.ExpenseAccountID == result.AcctID && row.ExpenseSubID == result.SubID)
                    {
                        if (result.BudgetAmt < 0 && row.CuryExtCost > 0) Base.ExpenseClaimDetails.Cache.RaiseExceptionHandling<EPExpenseClaimDetails.curyExtCost>(row, row?.CuryExtCost, ATPTEFMHelper.GetPropertyException(row, ATPTEFMMessages.AmountRequestedIsGreaterThanRemainingBudget, PXErrorLevel.Error));
                    }
                }
            }
        }
        /// <remarks>
        /// 2024-09-11 : "Exp Claim receipt cancelled but the row in the CA cannot be deleted" - CASE: 007130 {JLG}
        /// 2025-03-26 : Remove Persist Override for proper trace logs. TO DO - Remove Old Persist Delegate after 5 Months
        /// 2026-01-02 : [DLS] [Show Stopper] Cannot Save an Expense Claim Document: Incorrect Financial Period appearing as error CASE: 014743 {JLTG}
        /// </remarks>
        #region Old PersistDelegate Method
        public delegate void PersistDelegate();
        [PXOverride]
        public void Persist(PersistDelegate baseMethod)
        {
            EPExpenseClaim claim = Base.ExpenseClaim.Current;
            if (claim == null)
            {
                baseMethod();
                return;
            }
            ATPTEFMEPExpenseClaimExt claimExt = claim.GetExtension<ATPTEFMEPExpenseClaimExt>();
            HashSet<string> DeletedReceipts = new HashSet<string>();

            string finID = ATPTEFMHelper.GetFinPeriod(Base, claim.DocDate);

            MasterFinPeriod period = PXSelect<MasterFinPeriod,
                Where<MasterFinPeriod.finPeriodID, Equal<Required<MasterFinPeriod.finPeriodID>>>>
                .Select(Base, finID);

            FinPeriod finPeriod = FinPeriod.PK.FindByBranch(Base, Base.Accessinfo.BranchID, finID);

            #region Budget Validation

            bool BudgetValidate = (ATPTEFMFeatureSetup?.Current?.BudgetValidation ?? RQRequestClassBudget.None) == RQRequestClassBudget.Error;
            bool validateBudget = claimExt.UsrATPTEFMBudgetEnabled ?? false;

            bool PBudgetValidate = (ATPTEFMFeatureSetup?.Current?.ProjectBudgetValidation ?? RQRequestClassBudget.None) == RQRequestClassBudget.Error;
            bool validatePBudget = claimExt.UsrATPTEFMProjectBudgetEnabled ?? false;

            if (BudgetValidate && validateBudget)
            {
                using (PXTransactionScope ts = new PXTransactionScope())
                {
                    bool isOverbudget = ATPTEFMBudget?.Select()?.RowCast<ATPTEFMBudget>()?.ToList()?.Any(x => (x.BudgetAmt ?? 0m) < 0) ?? false;

                    if (isOverbudget)
                        throw new PXRowPersistedException(typeof(ATPTEFMBudget).Name, ts, ATPTEFMMessages.CheckBudget);

                    ATPTEFMBudget.Cache.Persist(PXDBOperation.Insert);
                    ATPTEFMBudget.Cache.Persist(PXDBOperation.Update);

                    ts.Complete(Base);
                }
                ATPTEFMBudget.Cache.Persisted(false);
            }
            if (PBudgetValidate && validatePBudget)
            {
                using (PXTransactionScope ts = new PXTransactionScope())
                {
                    bool isOverbudget = ATPTEFMProjectBudget?.Select()?.RowCast<ATPTEFMPBudget>()?.ToList()?.Any(x => (x.BudgetAmt ?? 0m) < 0) ?? false;

                    foreach (ATPTEFMPBudget row in ATPTEFMProjectBudget.Select())
                    {
                        if (row.ProjectID == null || row.ProjectTaskID == null || row.CostCodeID == null || row.AccountGroupID == null) continue;

                        ATPTEFMProjectBudgetLineSummary PBSummary = ATPTEFMProjectBudgetSummary.Select(row.ProjectID, row.ProjectTaskID, row.CostCodeID, period.FinYear.ToString(), row.AccountGroupID);

                        if (PBSummary == null)
                            throw new PXRowPersistedException(typeof(ATPTEFMPBudget).Name, ts, ATPTEFMMessages.NotInProjectBudget);
                    }

                    if (isOverbudget)
                        throw new PXRowPersistedException(typeof(ATPTEFMPBudget).Name, ts, ATPTEFMMessages.CheckProjectBudget);

                    ATPTEFMProjectBudget.Cache.Persist(PXDBOperation.Insert);
                    ATPTEFMProjectBudget.Cache.Persist(PXDBOperation.Update);

                    ts.Complete(Base);
                }
                ATPTEFMProjectBudget.Cache.Persisted(false);
            }

            #endregion

            #region Budget Requirements
            List<ATPTEFMBudget> BudgetList = new List<ATPTEFMBudget>();
            List<ATPTEFMPBudget> PBudgetList = new List<ATPTEFMPBudget>();

            ATPTEFMBudgetEntry graph = PXGraph.CreateInstance<ATPTEFMBudgetEntry>();
            bool isDeleted = Base.ExpenseClaim.Cache.Deleted.Any_() ? true : false;
            EPExpenseClaim curRecord = isDeleted ? Base.ExpenseClaim.Cache.Deleted.FirstOrDefault_() as EPExpenseClaim : Base.ExpenseClaim.Current;
            bool isCancelled = curRecord == null ? false : curRecord.Status == EPExpenseClaimStatus.RejectedStatus ? true : false;

            List<EPExpenseClaimDetails> curLines = new List<EPExpenseClaimDetails>();
            foreach (EPExpenseClaimDetails item in Base.ExpenseClaimDetails.Cache.Deleted) { curLines.Add(item); }
            #endregion

            #region Financial Period Validation

            bool isMultipleCalendarSupport = PXAccess.FeatureInstalled<FeaturesSet.multipleCalendarsSupport>();  //EnableFeatures?.Current?.MultipleCalendarsSupport ?? false;
            if (claimExt.UsrATPTEFMTranType == ATPTEFMExpenseTypeAttribute.RequestforPayment)
            {
                DateTime periodDate = (DateTime)period.StartDate;

                if (isMultipleCalendarSupport == false && period.Status == FinPeriod.status.Inactive)
                {
                    string finPeriodErrMsg = $"The {periodDate.ToString("MM-yyyy")} financial period is inactive.";

                    Base.ExpenseClaim.Cache.RaiseExceptionHandling<EPExpenseClaim.docDate>(claim,
                           claim.DocDate,
                           ATPTEFMHelper.GetPropertyException(claim, finPeriodErrMsg, PXErrorLevel.Error));
                }
            }
            #endregion

            #region Generate Autonumber For New Request Class ID
            if (Base.ExpenseClaim.Cache.GetStatus(Base.ExpenseClaim.Current) == PXEntryStatus.Updated)
            {
                EPExpenseClaim currentClaim = PXSelectReadonly<
                    EPExpenseClaim,
                    Where<EPExpenseClaim.refNbr, Equal<Required<EPExpenseClaim.refNbr>>>>
                    .Select(Base, Base.ExpenseClaim.Current.RefNbr);
                if (currentClaim != null)
                {
                    ATPTEFMEPExpenseClaimExt currentClaimExt = currentClaim.GetExtension<ATPTEFMEPExpenseClaimExt>();
                    if (Base.ExpenseClaim.Current.GetExtension<ATPTEFMEPExpenseClaimExt>().UsrATPTEFMReqClass != currentClaimExt.UsrATPTEFMReqClass)
                    {
                        ATPTEFMReqClass numberingType = PXSelect<
                            ATPTEFMReqClass,
                            Where<ATPTEFMReqClass.reqClassID, Equal<Required<ATPTEFMReqClass.reqClassID>>>>
                            .Select(Base, Base.ExpenseClaim.Current.GetExtension<ATPTEFMEPExpenseClaimExt>().UsrATPTEFMReqClass);
                        ATPTEFMReqClass numberingTypeZ = PXSelect<
                            ATPTEFMReqClass,
                            Where<ATPTEFMReqClass.reqClassID, Equal<Required<ATPTEFMReqClass.reqClassID>>>>
                            .Select(Base, currentClaimExt.UsrATPTEFMReqClass);
                        if (Base.ExpenseClaim.Current.GetExtension<ATPTEFMEPExpenseClaimExt>().UsrATPTEFMTranType == ATPTEFMExpenseTypeAttribute.RequestforPayment && (numberingType.ReqClassID != numberingTypeZ.ReqClassID))
                        {
                            foreach (EPExpenseClaimDetails claimDetail in Base.ExpenseClaimDetails.Select())
                            {
                                Base.ExpenseClaimDetails.Current = claimDetail;
                                Base.ExpenseClaimDetails.Current.GetExtension<ATPTEFMEPExpenseClaimDetailsExt>().UsrATPTEFMReqClass = claimExt.UsrATPTEFMReqClass;
                                Base.ExpenseClaimDetails.UpdateCurrent();
                            }
                            Base.ExpenseClaim.Current.GetExtension<ATPTEFMEPExpenseClaimExt>().UsrATPTEFMRFPReqRef = AutoNumberAttribute.GetNextNumber(Base.ExpenseClaim.Cache, Base.ExpenseClaim.Current, numberingType.NumberingID, Base.Accessinfo.BusinessDate);
                            Base.ExpenseClaim.UpdateCurrent();
                        }
                        else if (Base.ExpenseClaim.Current.GetExtension<ATPTEFMEPExpenseClaimExt>().UsrATPTEFMTranType == ATPTEFMExpenseTypeAttribute.Liquidation && (numberingType.ReqClassID != numberingTypeZ.ReqClassID))
                        {
                            Base.ExpenseClaim.Current.GetExtension<ATPTEFMEPExpenseClaimExt>().UsrATPTEFMLiqNbr = AutoNumberAttribute.GetNextNumber(Base.ExpenseClaim.Cache, Base.ExpenseClaim.Current, numberingType.NumberingID, Base.Accessinfo.BusinessDate);
                            Base.ExpenseClaim.UpdateCurrent();
                        }
                    }
                }
            }
            #endregion

            #region Retrieve all deleted rows in claim details
            foreach (EPExpenseClaimDetails vals in Base.ExpenseClaimDetails.Cache.Updated.RowCast<EPExpenseClaimDetails>()
                                           .Where(p => string.IsNullOrEmpty(p.RefNbr)))
            {
                DeletedReceipts.Add(vals.ClaimDetailCD);
            }
            #endregion

            baseMethod();

            #region BudgetHistory
            if (isDeleted || isCancelled)
            {
                foreach (EPExpenseClaimDetails item in curLines)
                {
                    var row = new ATPTEFMBudget();
                    row.AcctID = item.ExpenseAccountID;
                    row.SubID = item.ExpenseSubID;
                    row.RefNbr = item.RefNbr;
                    row.Origin = (int)ATPTEFMBudgetLibrary.OriginTypes.RequestForPayment;
                    BudgetList.Add(row);

                    var pRow = new ATPTEFMPBudget();
                    pRow.ProjectID = item.ContractID;
                    pRow.ProjectTaskID = item.TaskID;
                    pRow.CostCodeID = item.CostCodeID;
                    pRow.RefNbr = item.RefNbr;
                    pRow.Origin = (int)ATPTEFMBudgetLibrary.OriginTypes.RequestForPayment;
                    PBudgetList.Add(pRow);
                }
                graph.DeleteBudgetHistory(BudgetList);
                graph.DeletePBudgetHistory(PBudgetList);
            }
            else
            {
                if (claimExt.UsrATPTEFMBudgetEnabled ?? false)
                {
                    BudgetList.Add(new ATPTEFMBudget() { RefNbr = curRecord.RefNbr, Origin = (int)ATPTEFMBudgetLibrary.OriginTypes.RequestForPayment });
                    foreach (ATPTEFMBudget item in ATPTEFMBudget.Select())
                    {
                        var row = item;
                        row.IsApproved = curRecord.Approved ?? false;
                        BudgetList.Add(row);
                    }
                    graph.AddBudgetHistory(BudgetList);
                }

                if (claimExt.UsrATPTEFMProjectBudgetEnabled ?? false)
                {
                    PBudgetList.Add(new ATPTEFMPBudget() { RefNbr = curRecord.RefNbr, Origin = (int)ATPTEFMBudgetLibrary.OriginTypes.RequestForPayment });
                    foreach (ATPTEFMPBudget item in ATPTEFMProjectBudget.Select())
                    {
                        var row = item;
                        row.IsApproved = curRecord.Approved ?? false;
                        PBudgetList.Add(row);
                    }
                    graph.AddPBudgetHistory(PBudgetList);
                }
            }
            ATPTEFMBudget.Select();
            #endregion

            #region CA Update
            if (claimExt.UsrATPTEFMTranType == ATPTEFMExpenseTypeAttribute.Liquidation)
            {
                if (DeletedReceipts.Count > 0)
                {
                    ATPTEFMCashAdvanceEntry caEntry = PXGraph.CreateInstance<ATPTEFMCashAdvanceEntry>();

                    foreach (var deletedReceiptCD in DeletedReceipts)
                    {
                        PXResultset<ATPTEFMCAReceiptDetail, ATPTEFMCashAdvance> details = PXSelectJoin<
                            ATPTEFMCAReceiptDetail,
                            InnerJoin<ATPTEFMCashAdvance,
                                On<ATPTEFMCashAdvance.cashAdvanceNbr, Equal<ATPTEFMCAReceiptDetail.cashAdvanceNbr>>>,
                            Where<ATPTEFMCAReceiptDetail.expenseReceiptRefNbr, Equal<Required<ATPTEFMCAReceiptDetail.expenseReceiptRefNbr>>>>
                            .Select<PXResultset<ATPTEFMCAReceiptDetail, ATPTEFMCashAdvance>>
                              (Base, deletedReceiptCD);

                        ATPTEFMCashAdvance ca = details;
                        ATPTEFMCAReceiptDetail caDetails = details;

                        if (ca != null && caDetails != null)
                        {
                            caEntry.CashAdvances.Current = ca;
                            caDetails.LiquidationRef = null;
                            caEntry.CashAdvanceReceiptLines.Update(caDetails);
                            caEntry.Save.Press();
                        }
                    }
                }

                #region Retrieve All Empty Liquidation Ref Nbr
                var ErWithNoLiqNbr = PXSelectJoin<
                                                                                        ATPTEFMCAReceiptDetail,
                                                                                        InnerJoin<EPExpenseClaimDetails,
                                                                                            On<EPExpenseClaimDetails.claimDetailCD, Equal<ATPTEFMCAReceiptDetail.expenseReceiptRefNbr>>,
                                                                                        InnerJoin<EPExpenseClaim,
                                                                                            On<EPExpenseClaim.refNbr, Equal<EPExpenseClaimDetails.refNbr>>>>,
                                                                                        Where<EPExpenseClaim.refNbr, Equal<Required<EPExpenseClaim.refNbr>>,
                                                                                            And<ATPTEFMCAReceiptDetail.liquidationRef, IsNull>>>
                                                                                        .Select(Base, claim.RefNbr)
                                                                                        .ToList();

                if (ErWithNoLiqNbr.Count > 0)
                {
                    ATPTEFMCashAdvanceEntry caEntry = PXGraph.CreateInstance<ATPTEFMCashAdvanceEntry>();


                    foreach (ATPTEFMCAReceiptDetail receipt in ErWithNoLiqNbr)
                    {

                        PXResultset<ATPTEFMCAReceiptDetail, ATPTEFMCashAdvance> details = PXSelectJoin<
                            ATPTEFMCAReceiptDetail,
                            InnerJoin<ATPTEFMCashAdvance,
                                On<ATPTEFMCashAdvance.cashAdvanceNbr, Equal<ATPTEFMCAReceiptDetail.cashAdvanceNbr>>>,
                            Where<ATPTEFMCAReceiptDetail.expenseReceiptRefNbr, Equal<Required<ATPTEFMCAReceiptDetail.expenseReceiptRefNbr>>>>
                            .Select<PXResultset<ATPTEFMCAReceiptDetail, ATPTEFMCashAdvance>>
                              (Base, receipt.ExpenseReceiptRefNbr);

                        ATPTEFMCashAdvance ca = details;
                        ATPTEFMCAReceiptDetail caDetails = details;

                        if (ca != null && caDetails != null)
                        {
                            caEntry.CashAdvances.Current = ca;
                            caDetails.LiquidationRef = claimExt.UsrATPTEFMLiqNbr;
                            caEntry.CashAdvanceReceiptLines.Update(caDetails);
                            caEntry.Save.Press();
                        }
                    }
                }

                #endregion
            }
            #endregion

            #region Vendor Details

            Numbering claimNumbering = PXSelect<
                Numbering,
                Where<Numbering.numberingID, Equal<Required<Numbering.numberingID>>>>
                .Select(Base, Base.epsetup.Current.ClaimNumberingID);

            if (claimExt?.UsrATPTEFMTranType == ATPTEFMExpenseTypeAttribute.RequestforPayment && claim.Status == ATPTEFMExpenseClaimStatusAttribute.HoldValue && (Base.ExpenseClaim.Current.RefNbr.Trim() != claimNumbering.NewSymbol.Trim()))
            {
                ExpenseClaimDetailEntry entry = PXGraph.CreateInstance<ExpenseClaimDetailEntry>();

                if (Base.ExpenseClaimDetails.Select().Count != 0)
                {
                    Numbering numbering = PXSelect<
                        Numbering,
                        Where<Numbering.numberingID, Equal<Required<Numbering.numberingID>>>>
                        .Select(Base, Base.epsetup.Current.ReceiptNumberingID);
                    foreach (EPExpenseClaimDetails receipts in Base.ExpenseClaimDetails.Select())
                    {
                        entry.Clear();
                        if (Base.ExpenseClaimDetails.Cache.GetStatus(receipts) == PXEntryStatus.Updated)
                        {
                            ATPTEFMEPExpenseClaimDetailsExt receiptsExt = receipts.GetExtension<ATPTEFMEPExpenseClaimDetailsExt>();
                            if (receipts.ClaimDetailCD.Trim() == numbering.NewSymbol.Trim()) continue;

                            EPExpenseClaimDetails epDetails = PXSelect<
                                EPExpenseClaimDetails,
                                Where<EPExpenseClaimDetails.claimDetailCD, Equal<Required<EPExpenseClaimDetails.claimDetailCD>>>>
                                .Select(Base, receipts.ClaimDetailCD);

                            ATPTEFMEPExpenseClaimDetailsExt epDetailsExt = epDetails.GetExtension<ATPTEFMEPExpenseClaimDetailsExt>();
                            if (epDetails != null && receiptsExt.UsrATPTVendID != null)
                            {
                                entry.CurrentClaimDetails.Current = epDetails;
                                entry.CurrentClaimDetails.Cache.SetValueExt<ATPTEFMEPExpenseClaimDetailsExt.usrATPTVendID>(epDetails, receiptsExt.UsrATPTVendID);
                                entry.CurrentClaimDetails.Update(epDetails);
                                entry.CurrentClaimDetails.UpdateCurrent();
                            }
                            else
                            {
                                entry.CurrentClaimDetails.Current = epDetails;
                                entry.CurrentClaimDetails.Update(epDetails);
                                entry.CurrentClaimDetails.UpdateCurrent();
                            }
                        }
                    }
                }
            }
            #endregion
        }
        #endregion
        public int? SetAccountID(ATPTEFMReqClass reqClass, EPExpenseClaim ec, EPExpenseClaimDetails ecDet)
        {
            int? retValue = 0;

            if (reqClass != null)
            {
                EPEmployee requestBy = PXSelect<
                    EPEmployee,
                    Where<EPEmployee.bAccountID, Equal<Required<EPEmployee.bAccountID>>>>
                    .Select(Base, ec?.EmployeeID);

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

        public object SetSubID(ATPTEFMReqClass reqClass, EPExpenseClaim ec, EPExpenseClaimDetails ecDet)
        {
            if (reqClass == null) return null;
            object retValue = null;

            ATPTEFMEPExpenseClaimExt claimExt = ec.GetExtension<ATPTEFMEPExpenseClaimExt>();
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
                .Select(Base, ec.EmployeeID);
            int? requesterSubID = requestBy?.ExpenseSubID;

            //Department
            EPDepartment department = PXSelect<
                EPDepartment,
                Where<EPDepartment.departmentID, Equal<Required<EPDepartment.departmentID>>>>
                .Select(Base, ec.DepartmentID);
            int? departmentSubID = department?.ExpenseSubID;

            retValue = PX.Objects.RQ.SubAccountMaskAttribute.MakeSub<ATPTEFMReqClass.combineExpSub>(Base, reqClass.CombineExpSub,
                                new object[] { requestclassSubID, departmentSubID, inventorySubID, requesterSubID },
                                new Type[] { typeof(ATPTEFMReqClass.expenseSubID), typeof(EPDepartment.expenseSubID), typeof(InventoryItem.cOGSSubID), typeof(EPEmployee.expenseSubID) });

            return retValue;
        }
        public void ATPTEFMApplyPPT(List<EPExpenseClaim> list)
        {
            foreach (EPExpenseClaim claim in list)
            {
                ATPTEFMEPExpenseClaimExt claimExt = claim.GetExtension<ATPTEFMEPExpenseClaimExt>();
                //Apply Prepayment
                if (claimExt.UsrATPTEFMTranType == ATPTEFMExpenseTypeAttribute.Liquidation)
                {
                    foreach (EPExpenseClaimDetails rct in PXSelect<
                        EPExpenseClaimDetails,
                        Where<EPExpenseClaimDetails.refNbr, Equal<Required<EPExpenseClaimDetails.refNbr>>>>
                        .Select(this.Base, claim.RefNbr))
                    {

                        ATPTEFMEPExpenseClaimDetailsExt rctExt = rct.GetExtension<ATPTEFMEPExpenseClaimDetailsExt>();

                        ATPTEFMCashAdvance cadv = PXSelect<
                            ATPTEFMCashAdvance,
                            Where<ATPTEFMCashAdvance.cashAdvanceNbr, Equal<Required<ATPTEFMCashAdvance.cashAdvanceNbr>>>>
                            .Select(this.Base, rctExt.UsrATPTEFMReqRef);
                        if (cadv != null)
                        {
                            //PXTrace.WriteInformation("5");

                            APPaymentEntry paymentGraph = PXGraph.CreateInstance<APPaymentEntry>();
                            paymentGraph.Document.Current = paymentGraph.Document.Search<APPayment.refNbr>(cadv.BillRefNbr, cadv.BillType);
                            if (paymentGraph.Document.Current != null)
                            {
                                //PXTrace.WriteInformation("6");

                                foreach (APInvoice inv in PXSelectReadonly<
                                    APInvoice,
                                    Where<APInvoice.origModule, Equal<BatchModule.moduleEP>,
                                        And<APInvoice.origRefNbr, Equal<Required<APInvoice.origRefNbr>>>>>
                                    .Select(this.Base, claim.RefNbr))
                                {
                                    //PXTrace.WriteInformation("7");
                                    bool hasRow = false;
                                    if (inv.DocType == APDocType.Invoice)
                                    {
                                        APAdjust newadj = new APAdjust();
                                        newadj.AdjdDocType = inv.DocType;
                                        newadj.AdjdRefNbr = inv.RefNbr;
                                        //newadj.CuryAdjgAmt = inv.CuryDocBal ?? decimal.Zero;
                                        paymentGraph.Adjustments_Invoices.Insert(newadj);
                                        paymentGraph.Save.Press();
                                        hasRow = true;
                                    }
                                    if (hasRow == true)
                                    {
                                        paymentGraph.Actions["Release"].Press();
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
        public void TryValidateRequireUniqueVendorRefWhenCardInvolved()
        {
            bool hasErrorLines = false;
            foreach (PX.Objects.EP.EPExpenseClaimDetails item in Base.ExpenseClaimDetails.Select())
            {
                if (!item.IsPaidWithCard)
                {
                    continue;
                }

                CashAccount cashAccount = PXSelect<
                    CashAccount,
                    Where<CashAccount.accountID, Equal<Required<EPExpenseClaimDetails.expenseAccountID>>>>
                    .Select(Base, item.ExpenseAccountID);
                if (cashAccount == null)
                {
                    return;
                }

                PaymentMethod paymentMethod = PXSelectJoin<
                    PaymentMethod,
                    InnerJoin<PaymentMethodAccount,
                        On<PaymentMethodAccount.paymentMethodID, Equal<PaymentMethod.paymentMethodID>>>,
                    Where<PaymentMethodAccount.cashAccountID, Equal<Required<CashAccount.cashAccountID>>>>
                    .Select(Base, cashAccount.CashAccountID);

                if (paymentMethod != null && paymentMethod.APRequirePaymentRef.GetValueOrDefault(false))
                {
                    CACorpCard card = CACorpCard.PK.Find(Base, item.CorpCardID);
                    if (card == null)
                    {
                        return;
                    }

                    Base.ExpenseClaimDetails.Cache.RaiseExceptionHandling<PX.Objects.EP.EPExpenseClaimDetails.expenseAccountID>(item, item.ExpenseAccountID,
                        ATPTEFMHelper.GetPropertyException(item, string.Format(PX.Objects.EP.Messages.ExpenseClaimCannotBeReleased, cashAccount.CashAccountCD, card.Name, paymentMethod.PaymentMethodID), PXErrorLevel.RowError));
                    hasErrorLines = true;
                }
            }
            if (hasErrorLines)
            {
                throw new PXException(PX.Objects.EP.Messages.ExpenseClaimCannotBeReleasedSummary);
            }
        }
        //public void UpdateCAReceiptDetails()
        //{
        //    EPExpenseClaim claim = Base.ExpenseClaim.Current;
        //    ATPTEFMEPExpenseClaimExt claimExt = claim.GetExtension<ATPTEFMEPExpenseClaimExt>();
        //    foreach (EPExpenseClaimDetails receipts in Base.ExpenseClaimDetails.Select().ToList())
        //    {
        //        ATPTEFMCashAdvanceReceipts.Current = ATPTEFMCashAdvanceReceipts.Select(receipts.ClaimDetailCD);

        //        if (ATPTEFMCashAdvanceReceipts.Current != null)
        //        {
        //            ATPTEFMCashAdvanceReceipts.Current.LiquidationRef = claimExt.UsrATPTEFMLiqNbr;
        //            ATPTEFMCashAdvanceReceipts.UpdateCurrent();
        //        }
        //    }
        //}

        /// <summary>
        /// case 009010 -> let baseMethod run first before any custom CFM changes <br/>
        /// case 009107 -> Liquidation Amount of CA on Summary area does not update once the Liquidation is modified on Expense Claim {JTG}
        /// case 012187 -> CFM: Unable to Submit Liquidation {JTG} <br/>
        /// 2025-09-22 : Add null checker if Receipt row's DFT ATC is empty. This only occurs if PT is turned off. 013475 {RRS}
        /// </summary>
        public delegate void SubmitClaimDelegate(EPExpenseClaim claim);
        [PXOverride]
        public void SubmitClaim(EPExpenseClaim claim, SubmitClaimDelegate baseMethod)
        {
            using (PXTransactionScope ts = new PXTransactionScope())
            {
                baseMethod(claim);

                if (claim is null) return;

                ATPTEFMCASetup caSetup = ATPTEFMCAPreference.Select();
                ATPTEFMEPExpenseClaimExt claimExt = claim.GetExtension<ATPTEFMEPExpenseClaimExt>();

                foreach (PXResult<EPExpenseClaimDetails> item in Base.ExpenseClaimDetails.Select())
                {
                    EPExpenseClaimDetails ePExpenseClaimDetails = item;
                    ATPTEFMEPExpenseClaimDetailsExt rowExt = ePExpenseClaimDetails.GetExtension<ATPTEFMEPExpenseClaimDetailsExt>();

                    if (rowExt.UsrATPTEFMTranType == ATPTEFMExpenseTypeAttribute.Liquidation)
                    {
                        #region Cash Advance
                        ATPTEFMCARequestDetail requestDetail = PXSelect<
                                                                            ATPTEFMCARequestDetail,
                                                                            Where<ATPTEFMCARequestDetail.cashAdvanceNbr, Equal<Required<ATPTEFMCARequestDetail.cashAdvanceNbr>>,
                                                                                And<ATPTEFMCARequestDetail.inventoryID, Equal<Required<ATPTEFMCARequestDetail.inventoryID>>>>>
                                                                            .Select(Base, rowExt.UsrATPTEFMReqRef, ePExpenseClaimDetails.InventoryID)
                                                                            .TopFirst;

                        int? CARequestDetailID = 0;
                        if (requestDetail != null)
                        {
                            CARequestDetailID = requestDetail.CashAdvanceRequestDetailID;
                        }
                        ATPTEFMCashAdvanceEntry graph = PXGraph.CreateInstance<ATPTEFMCashAdvanceEntry>();

                        graph.CashAdvances.Current = PXSelect<
                            ATPTEFMCashAdvance,
                            Where<ATPTEFMCashAdvance.cashAdvanceNbr, Equal<Required<ATPTEFMCashAdvance.cashAdvanceNbr>>>>
                            .Select(Base, rowExt.UsrATPTEFMReqRef);

                        ATPTEFMCAReceiptDetail caReceiptDetailLines = graph.CashAdvanceReceiptLines.Search<ATPTEFMCAReceiptDetail.expenseReceiptRefNbr>(ePExpenseClaimDetails.ClaimDetailCD);
                        caReceiptDetailLines = caReceiptDetailLines ?? graph.CashAdvanceReceiptLines.Insert();

                        caReceiptDetailLines.CashAdvanceRequestDetailID = CARequestDetailID;
                        graph.CashAdvanceReceiptLines.Cache.SetValueExt<ATPTEFMCAReceiptDetail.inventoryID>(caReceiptDetailLines, ePExpenseClaimDetails.InventoryID);
                        graph.CashAdvanceReceiptLines.Cache.Update(caReceiptDetailLines);
                        caReceiptDetailLines.ExpenseReceiptRefNbr = ePExpenseClaimDetails.ClaimDetailCD;
                        caReceiptDetailLines.TaxZoneID = ePExpenseClaimDetails.TaxZoneID;
                        caReceiptDetailLines.TaxCategoryID = ePExpenseClaimDetails.TaxCategoryID;
                        var dftATC = GetDefATC(ePExpenseClaimDetails); //If PT is turned off, this will return empty, must retain the current value
                        caReceiptDetailLines.AtcCode = string.IsNullOrEmpty(dftATC) ? caReceiptDetailLines.AtcCode : dftATC;
                        caReceiptDetailLines.Date = ePExpenseClaimDetails.ExpenseDate;
                        caReceiptDetailLines.ProjectID = ePExpenseClaimDetails.ContractID;
                        caReceiptDetailLines.ProjectTaskID = ePExpenseClaimDetails.TaskID;
                        caReceiptDetailLines.CostCodeID = ePExpenseClaimDetails.CostCodeID;
                        caReceiptDetailLines.RefNbr = ePExpenseClaimDetails.ExpenseRefNbr;
                        caReceiptDetailLines.VendID = rowExt.UsrATPTEFMDetailVendorID;
                        caReceiptDetailLines.VendorName = rowExt.UsrATPTVendName;
                        caReceiptDetailLines.VendorAddress = rowExt.UsrATPTAddress;
                        caReceiptDetailLines.VendorTin = rowExt.UsrATPTVendTIN;
                        caReceiptDetailLines.NetQty = ePExpenseClaimDetails.Qty;
                        caReceiptDetailLines.CuryNetUnitCost = ePExpenseClaimDetails.CuryUnitCost;
                        caReceiptDetailLines.AccountID = ePExpenseClaimDetails.ExpenseAccountID;
                        caReceiptDetailLines.SubID = ePExpenseClaimDetails.ExpenseSubID;
                        caReceiptDetailLines.LineDescription = ePExpenseClaimDetails.TranDesc;
                        graph.CashAdvanceReceiptLines.Update(caReceiptDetailLines);
                        graph.Save.Press();
                        #endregion
                    }
                }
                ts.Complete();
            }
        }

        protected virtual void ValidateFinancialSettings(EPExpenseClaim claim, ATPTEFMEPExpenseClaimExt claimExt)
        {
            int employeeID = (int)((claimExt.UsrATPTEFMTranType == ATPTEFMExpenseTypeAttribute.RequestforPayment)
                ? claimExt.UsrATPTVendorID
                : claim.EmployeeID);

            // Get all required data in one go
            var location = PXSelect<
                Location,
                Where<Location.bAccountID, Equal<Required<Location.bAccountID>>,
                    And<Location.isDefault, Equal<True>>>>
                .Select(Base, employeeID);


            // Validate Vendor settings
            if (claimExt.UsrATPTVendorID != null)
            {
                VendorR vendor = PXSelect<
                    VendorR,
                    Where<VendorR.bAccountID, Equal<Required<VendorR.bAccountID>>>>
                    .Select(Base, claimExt.UsrATPTVendorID);

                if (vendor != null)
                {
                    ValidateVendorStatus(vendor);

                    if (vendor.TermsID == null)
                        throw new PXException(Messages.ATPTEFMMessages.VendorTermsIsEmpty);
                }
            }

            if (location != null)
            {
                // Validate Location settings
                ValidateLocationSettings(location);

                // Validate Employee settings
                if (claim.EmployeeID != null)
                {
                    EPEmployee employee = PXSelect<
                        EPEmployee,
                        Where<EPEmployee.bAccountID, Equal<Required<EPEmployee.bAccountID>>>>
                        .Select(Base, claim.EmployeeID);
                    if (employee != null)
                    {
                        ValidateEmployeeStatus(employee);

                        if (employee.TermsID == null)
                            throw new PXException(Messages.ATPTEFMMessages.TermsIsEmpty);
                    }
                }
            }
        }
        protected virtual void ValidateEmployeeStatus(EPEmployee employee)
        {
            var bAccount = BAccount.PK.Find(Base, employee.BAccountID);
            if (bAccount?.VStatus == CustomerStatus.Inactive)
                throw new PXException(Messages.ATPTEFMMessages.EmployeeInactive, employee.AcctCD);
        }

        protected virtual void ValidateVendorStatus(VendorR vendor)
        {
            if (vendor.VStatus == CustomerStatus.Inactive)
                throw new PXException(Messages.ATPTEFMMessages.VendorInactive, vendor.AcctCD);
        }

        protected virtual void ValidateLocationSettings(Location location)
        {
            var errors = new List<string>();

            if (location.VAPAccountID == null && location.VAPSubID == null)
                errors.Add(Messages.ATPTEFMMessages.APAccountAndAPSubError);
            else
            {
                if (location.VAPAccountID == null)
                    errors.Add(Messages.ATPTEFMMessages.APAccountError);
                if (location.VAPSubID == null)
                    errors.Add(Messages.ATPTEFMMessages.APSubError);
            }

            if (location.VTaxZoneID == null)
                errors.Add(Messages.ATPTEFMMessages.TaxZoneIsEmpty);

            if (errors.Any())
                throw new PXException(string.Join(Environment.NewLine, errors));
        }
        #endregion

        #region CacheAttached
        [PXMergeAttributes(Method = MergeMethod.Replace)]
        [PXDBString(10, IsUnicode = true)]
        [PXDefault(typeof(Search2<
            Location.vTaxZoneID,
            RightJoin<EPEmployee,
                On<EPEmployee.defLocationID, Equal<Location.locationID>>>,
            Where<EPEmployee.bAccountID, Equal<Current<EPExpenseClaim.employeeID>>>>))]
        [PXSelector(typeof(TaxZone.taxZoneID), DescriptionField = typeof(TaxZone.descr), Filterable = true)]
        [PXUIField(DisplayName = "Tax Zone", Visibility = PXUIVisibility.Visible)]
        protected virtual void EPExpenseClaim_TaxZoneID_CacheAttached(PXCache sender) { }

        /// <remarks>
        /// 2024-08-02 : Include Liquidation # on approval screen. CaseID : 006721 {RRS}
        /// </remarks>
        [PXMergeAttributes(Method = MergeMethod.Merge)]
        [PXDefault(typeof(Switch<Case<
            Where<ATPTEFMEPExpenseClaimExt.usrATPTEFMReqType.FromCurrent.
                IsEqual<ATPTEFMTranTypeAttribute.cashAdvance>>,
            ATPTEFMEPExpenseClaimExt.usrATPTEFMLiqNbr.FromCurrent>,
            ATPTEFMEPExpenseClaimExt.usrATPTEFMRFPReqRef.FromCurrent>), PersistingCheck = PXPersistingCheck.Nothing)]
        protected virtual void _(Events.CacheAttached<ATPTEFMEPApprovalExtension.usrATPTEFMRFPReqRef> e) { }
        #endregion
    }
}