using CashFundManagement.Attributes;
using CashFundManagement.DAC;
using CashFundManagement.DAC.Setup;
using CashFundManagement.DAC.Unbound;
using CashFundManagement.Extensions.DAC;
using CashFundManagement.Helper;
using CashFundManagement.Messages;
using CashFundManagement.MethodExtensions;
using PX.Api;
using PX.Common;
using PX.Data;
using PX.Data.BQL;
using PX.Data.WorkflowAPI;
using PX.Objects.AP;
using PX.Objects.CM;
using PX.Objects.Common;
using PX.Objects.Common.Extensions;
using PX.Objects.CR;
using PX.Objects.CS;
using PX.Objects.CS.DAC;
using PX.Objects.CT;
using PX.Objects.EP;
using PX.Objects.GL;
using PX.Objects.GL.DAC;
using PX.Objects.GL.FinPeriods;
using PX.Objects.GL.FinPeriods.TableDefinition;
using PX.Objects.IN;
using PX.Objects.PM;
using PX.Objects.RQ;
using PX.SM;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using static CashFundManagement.Classes.ATPTEFMBudgetLibrary;
using static CashFundManagement.Classes.ATPTEFMProjectBudgetLibrary;
using static CashFundManagement.Helper.ATPTEFMShared;
using SubAccountMaskAttribute = PX.Objects.RQ.SubAccountMaskAttribute;

namespace CashFundManagement.BLC
{
    /// <summary>
    /// ATPT3103
    /// </summary>
    public class ATPTEFMCashAdvanceEntry : ATPTPXGraphWithWorkflow<ATPTEFMCashAdvanceEntry, ATPTEFMCashAdvance>
    {
        #region ctor + views

        public ATPTEFMCashAdvanceEntry()
        {
            ReturnExcessCashAdvance?.SetEnabled(false);
            CreateAPBill?.SetEnabled(false);
            CreateAPBillImport?.SetVisible(false);
            LoadRequest?.SetEnabled(false);
            SubmitReceipts?.SetEnabled(false);
            SubmitReceipts?.SetEnabled(false);
            Liquidate?.SetEnabled(false);
            CACancel?.SetEnabled(false);

            CashAdvanceReceiptLines.AllowInsert = Preferences.Current.AllowManualReceipts ?? false;

            ProjectBudget.AllowSelect = false;

#if !Version23R2
            if (!(Preferences?.Current?.EnableCFM ?? false))
            {
                throw new PXException(ATPTEFMMessages.CashFundManagementFeatureDisabled);
            }
#endif
        }

        //Setup
        public PXSetup<ATPTEFMFeatures> FeatureSetup;
        public PXSetup<FeaturesSet> EnableFeatures;
        public PXSetup<APSetup> Setup;
        public PXSetup<ATPTEFMCASetup> Preferences;
        public PXSetup<Company> company;
        public PXSetup<EPSetup> EPSetup;

        public PXSetup<MasterFinPeriod>.Where<
            Where<MasterFinPeriod.finPeriodID, Equal<Current<ATPTEFMCashAdvance.finPeriodID>>>>
            PeriodSetup;

        [PXViewName("Cash Advance")]
        public PXSelect<
            ATPTEFMCashAdvance,
            Where<ATPTEFMCashAdvance.reqClassID, Equal<Optional<ATPTEFMCashAdvance.reqClassID>>>>
            CashAdvances;

        public PXSelect<
            ATPTEFMCashAdvance,
            Where<ATPTEFMCashAdvance.reqClassID, Equal<Current<ATPTEFMCashAdvance.reqClassID>>,
                And<ATPTEFMCashAdvance.cashAdvanceNbr, Equal<Current<ATPTEFMCashAdvance.cashAdvanceNbr>>>>>
            CurrentCashAdvance;

        public PXSelect<
            ATPTEFMCashAdvance,
            Where<ATPTEFMCashAdvance.reqClassID, Equal<Current<ATPTEFMCashAdvance.reqClassID>>,
                And<ATPTEFMCashAdvance.cashAdvanceNbr, Equal<Current<ATPTEFMCashAdvance.cashAdvanceNbr>>>>>
            MigrationMode;

        public PXSelectJoin<
            ATPTEFMCARequestDetail,
            InnerJoin<InventoryItem,
                On<InventoryItem.inventoryID, Equal<ATPTEFMCARequestDetail.inventoryID>>,
            LeftJoin<Contract,
                On<Contract.contractID, Equal<ATPTEFMCARequestDetail.projectID>>,
            LeftJoin<PMTask,
                On<PMTask.taskID, Equal<ATPTEFMCARequestDetail.projectTaskID>>>>>,
            Where<ATPTEFMCARequestDetail.cashAdvanceNbr, Equal<Current<ATPTEFMCashAdvance.cashAdvanceNbr>>>>
            CashAdvanceRequestLines;

        [PXImport(typeof(ATPTEFMCAReceiptDetail))]
        [PXCopyPasteHiddenView]
        public PXSelectJoin<
            ATPTEFMCAReceiptDetail,
            InnerJoin<InventoryItem,
                On<InventoryItem.inventoryID, Equal<ATPTEFMCAReceiptDetail.inventoryID>>,
            LeftJoin<ATPTEFMCARequestDetail,
                On<ATPTEFMCARequestDetail.cashAdvanceRequestDetailID, Equal<ATPTEFMCAReceiptDetail.cashAdvanceRequestDetailID>>,
            LeftJoin<Contract,
                On<Contract.contractID, Equal<ATPTEFMCARequestDetail.projectID>>,
            LeftJoin<PMTask,
                On<PMTask.taskID, Equal<ATPTEFMCARequestDetail.projectTaskID>>,
            LeftJoin<PMCostCode,
                On<PMCostCode.costCodeID, Equal<ATPTEFMCARequestDetail.costCodeID>>,
            LeftJoin<Vendor,
                On<Vendor.bAccountID, Equal<ATPTEFMCAReceiptDetail.vendorID>>,
            LeftJoin<BAccount,
                On<BAccount.bAccountID, Equal<Vendor.bAccountID>>,
            LeftJoin<Address,
                On<Address.addressID, Equal<BAccount.defAddressID>>,
            LeftJoin<LocationExtAddress,
                On<LocationExtAddress.locationID, Equal<BAccount.defLocationID>>>>>>>>>>>,
            Where<ATPTEFMCAReceiptDetail.cashAdvanceNbr, Equal<Current<ATPTEFMCashAdvance.cashAdvanceNbr>>>>
            CashAdvanceReceiptLines;

        public PXSelect<
            Vendor,
            Where<True, Equal<False>>>
            Vendors;

        [PXViewName(ATPTEFMMessages.EmployeeID)]
        public PXSelect<
            EPEmployee,
            Where<EPEmployee.bAccountID, Equal<Optional<ATPTEFMCashAdvance.requestedByID>>>>
            EPEmployee;

        //this is required for approval
        public PXSetup<EPEmployee>.Where<EPEmployee.bAccountID.IsEqual<ATPTEFMCashAdvance.requestedByID.FromCurrent>> EPEmployeeSetup;

        public PXSelect<
            EPExpenseClaimDetails,
            Where<EPExpenseClaimDetails.claimDetailCD, Equal<Current<ATPTEFMCAReceiptDetail.expenseReceiptRefNbr>>>>
            Receipt;

        public PXSelect<
            InventoryItem,
            Where<True, Equal<False>>>
            Items;
         
        public PXSelect<
            Contract,
            Where<True, Equal<False>>>
            Contracts;

        public PXSelect<
            PMTask,
            Where<True, Equal<False>>>
            Tasks;

        public PXSelect<
            Address,
            Where<True, Equal<False>>>
            Addresses;

        public PXSelect<
            LocationExtAddress,
            Where<True, Equal<False>>>
            ExtAddresses;

        [PXViewName(Messages.ATPTEFMMessages.ATPTEFMSetupApproval)]
        public PXSelect<ATPTEFMCASetupApproval> SetupApproval;

        [PXViewName(ATPTEFMMessages.Approval)]
        public EPApprovalAutomation<
            ATPTEFMCashAdvance, ATPTEFMCashAdvance.approved, ATPTEFMCashAdvance.rejected, ATPTEFMCashAdvance.hold, ATPTEFMCASetupApproval>
            Approval;

        public PXSelect<ATPTEFMCARequestDetail> ReceiptsForSubmit;



        [PXCopyPasteHiddenView]
        public PXSelectReadonly<ATPTEFMBudget> Budget;
        public PXSelect<ATPTEFMBudgetHistory> History;

        [PXCopyPasteHiddenView]
        public PXSelectReadonly<ATPTEFMPBudget> ProjectBudget;
        public PXSelect<ATPTEFMProjectBudgetHistory> ProjectHistoryView;

        public PXSelect<
            ATPTEFMProjectBudgetLineSummary,
            Where<ATPTEFMProjectBudgetLineSummary.released, Equal<True>,
                And<ATPTEFMProjectBudgetLineSummary.projectID, Equal<Required<ATPTEFMProjectBudgetLineSummary.projectID>>,
                And<ATPTEFMProjectBudgetLineSummary.projectTaskID, Equal<Required<ATPTEFMProjectBudgetLineSummary.projectTaskID>>,
                And<ATPTEFMProjectBudgetLineSummary.costCodeID, Equal<Required<ATPTEFMProjectBudgetLineSummary.costCodeID>>,
                And<ATPTEFMProjectBudgetLineSummary.finYear, Equal<Required<ATPTEFMProjectBudgetLineSummary.finYear>>,
                And<ATPTEFMProjectBudgetLineSummary.accountGroupID, Equal<Required<ATPTEFMProjectBudgetLineSummary.accountGroupID>>>>>>>>>
            ProjectBudgetSummary;

        public PXSelect<
            EPEmployee,
            Where<EPEmployee.bAccountID, Equal<Current<ATPTEFMCashAdvance.requestedByID>>>>
            RequesterEmployee;

        public PXSelectReadonly<
            EPEmployee,
            Where<EPEmployee.userID, Equal<Current<AccessInfo.userName>>>>
            CurrentEmployee;

        public ToggleCurrency<ATPTEFMCashAdvance> CurrencyView;
        public PXSelect<
            CurrencyInfo,
            Where<CurrencyInfo.curyInfoID, Equal<Current<ATPTEFMCashAdvance.curyInfoID>>>>
            currencyinfo;

        //public PXSelect<
        //    ATPTEFMReqClass,
        //    Where<ATPTEFMReqClass.reqClassID, Equal<Optional<ATPTEFMCashAdvance.reqClassID>>>>
        //    ReqClass;
        public PXSetup<ATPTEFMReqClass>.
            Where<ATPTEFMReqClass.reqClassID.IsEqual<ATPTEFMCashAdvance.reqClassID.FromCurrent>>
            ReqClass;

        public PXSelect<
            ATPTEFMVoidedDocument,
            Where<False, Equal<True>>,
            OrderBy<
                Asc<ATPTEFMVoidedDocument.createdDateTime>>>
            VoidedDocuments;

        #endregion

        #region Methods 
        public virtual void SetDefATC(EPExpenseClaimDetails row, string defATC)
        {
        }
        public virtual string GetDefATC(EPExpenseClaimDetails row)
        {
            return "";
        }
        public virtual bool DuplicateERRefNbr(ATPTEFMCAReceiptDetail row, string checkRefNbr)
        {
            return false;
        }
        protected string GetInvoiceNbr() => IsRaiseErrorDuplicateVendorRef() ? $"{CashAdvances.Current.CashAdvanceNbr}-{GetBillCount()}" : CashAdvances.Current.CashAdvanceNbr;
        protected int GetBillCount() => PXSelectJoin<APInvoice, InnerJoin<APRegister, On<APRegister.refNbr, Equal<APInvoice.refNbr>,
                And<APRegister.docType, Equal<APInvoice.docType>>>>,
                Where<APRegister.origRefNbr, Equal<Required<APRegister.origRefNbr>>>>.Select(this, CashAdvances.Current.CashAdvanceNbr).Count + 1;
        protected bool IsRaiseErrorDuplicateVendorRef() => Setup.Current.RaiseErrorOnDoubleInvoiceNbr ?? false;
        private void EnableDisableReturnExcessCa(ATPTEFMCashAdvance cashAdvance)
        {
            var receipts = PXSelect<ATPTEFMCAReceiptDetail, Where<ATPTEFMCAReceiptDetail.cashAdvanceNbr, Equal<@P.AsString>>>.Select(this, cashAdvance.CashAdvanceNbr);

            cashAdvance.CAPendingforLiquidationAndEmptyReceipts = cashAdvance.Status == ATPTEFMCashAdvanceStatusAttribute.PendingLiquidationValue && receipts.Count == 0;
            cashAdvance.AnyUnprocessedEC = false;
            cashAdvance.AnyUnprocessedReceipts = false;
            cashAdvance.AnyUnprocessedClaim = false;
            cashAdvance.HasRefund = cashAdvance.VendorRefundRefNbr != null;
            cashAdvance.HasBalance = cashAdvance.CuryChangeAmount > 0;

            if (cashAdvance.Status == ATPTEFMCashAdvanceStatusAttribute.PendingLiquidationValue)
            {
                if (receipts.Count != 0)
                {
                    foreach (ATPTEFMCAReceiptDetail result in receipts)
                    {
                        if (result.ExpenseReceiptRefNbr == null)
                            cashAdvance.AnyUnprocessedReceipts = true;

                        else if (result.LiquidationRef == null)
                            cashAdvance.AnyUnprocessedClaim = true;

                        else if (result.LiquidationRef != null)
                        {
                            EPExpenseClaim claim = PXSelect<
                                EPExpenseClaim,
                                Where<ATPTEFMEPExpenseClaimExt.usrATPTEFMLiqNbr, Equal<Required<ATPTEFMEPExpenseClaimExt.usrATPTEFMLiqNbr>>>>
                                .Select(this, result.LiquidationRef);

                            if (claim != null && claim.Status == EPExpenseClaimStatus.ReleasedStatus)
                            {
                                APInvoice inv = PXSelect<
                                    APInvoice,
                                    Where<APInvoice.origRefNbr, Equal<Required<APInvoice.origRefNbr>>,
                                        And<APInvoice.docType, Equal<APDocType.invoice>,
                                        And<APInvoice.status, NotEqual<APDocStatus.closed>>>>>
                                    .Select(this, claim.RefNbr);

                                if (inv != null)
                                    cashAdvance.AnyUnprocessedEC = true;
                            }
                            else
                                cashAdvance.AnyUnprocessedEC = true;
                        }
                    }
                }
            }
        }
        private void BudgetValidationRaiseErrorForLiquidationAmtGreaterThanRequestAmt(bool BudgetEnabled, bool fromPersistingEvent = false)
        {
            #region Raise Error if Receipt > Request grouped by Account and Sub Account
            if (BudgetEnabled)
            {
                bool hasError = false;

                var reqDetGrouped = CashAdvanceRequestLines.Select().RowCast<ATPTEFMCARequestDetail>()
                    .GroupBy(x => new { x.AccountID, x.SubID })
                    .Select(x => new { AccountID = x.Key.AccountID, SubID = x.Key.SubID, CuryAmount = x.Sum(y => y.CuryAmount) });

                foreach (var reqDet in reqDetGrouped)
                {
                    decimal totalNetAmtByBudgetAccounts = CashAdvanceReceiptLines.Select()
                        .RowCast<ATPTEFMCAReceiptDetail>()
                        .Where(recDet => recDet.AccountID == reqDet.AccountID && recDet.SubID == reqDet.SubID && recDet.Reversed == false)
                        .Sum(recDet => recDet.CuryNetAmt ?? 0m);

                    if (totalNetAmtByBudgetAccounts > reqDet.CuryAmount)
                    {
                        hasError = true;

                        foreach (ATPTEFMCAReceiptDetail recDet in CashAdvanceReceiptLines.Select()
                        .RowCast<ATPTEFMCAReceiptDetail>()
                        .Where(recDet => recDet.AccountID == reqDet.AccountID && recDet.SubID == reqDet.SubID))
                        {
                            Account acc = PXSelect<
                                Account,
                                Where<Account.accountID, Equal<Required<Account.accountID>>>>
                                .Select(this, recDet.AccountID);
                            Sub subAcc = PXSelect<
                                Sub,
                                Where<Sub.subID, Equal<Required<Sub.subID>>>>
                                .Select(this, recDet.SubID);

                            CashAdvanceReceiptLines.Cache.ClearFieldErrors<ATPTEFMCAReceiptDetail.curyNetAmt>(recDet);
                            CashAdvanceReceiptLines.Cache.RaiseExceptionHandling<ATPTEFMCAReceiptDetail.curyNetAmt>(recDet, recDet?.CuryNetAmt,
                                        ATPTEFMHelper.GetPropertyException(recDet, ATPTEFMMessages.MessagesWithParameters.ReceiptAmtGreaterThanRequestAmt(acc?.AccountCD, subAcc?.SubCD), PXErrorLevel.Error));
                        }
                    }
                }

                if (hasError && fromPersistingEvent)
                    throw new PXException(ATPTEFMMessages.BudgetCAReceiptGreaterThanRequest);
            }
            #endregion
        }
        private void BudgetValidationRaiseErrorForRequestAmountGreaterThanRemainingBudget(ATPTEFMCARequestDetail row, bool BudgetEnabled)
        {
            if (BudgetEnabled)
            {
                foreach (ATPTEFMBudget result in Budget.Select())
                {
                    if (row.AccountID == result.AcctID && row.SubID == result.SubID)
                    {
                        if (result.BudgetAmt < 0 && row.Amount > 0)
                        {
                            if(FeatureSetup.Current.BudgetValidation == RQRequestClassBudget.Error)
                                CashAdvanceRequestLines.Cache.RaiseExceptionHandling<ATPTEFMCARequestDetail.curyAmount>
                                    (row, row?.Amount, ATPTEFMHelper.GetPropertyException(row, ATPTEFMMessages.AmountRequestedIsGreaterThanRemainingBudget, PXErrorLevel.Error));
                            else if (FeatureSetup.Current.BudgetValidation == RQRequestClassBudget.Warning)
                                CashAdvanceRequestLines.Cache.RaiseExceptionHandling<ATPTEFMCARequestDetail.curyAmount>
                                    (row, row?.Amount, ATPTEFMHelper.GetPropertyException(row, ATPTEFMMessages.AmountRequestedIsGreaterThanRemainingBudget, PXErrorLevel.Warning));
                        }
                    }
                }
            }
        }
        /// <remarks>
        /// 2024-11-11 : PROD: CFM - CA error. CASEID: 014199 {JLG} <br/>     
        /// </remarks>
        private void PBudgetValidationRaiseErrorForLiquidationAmtGreaterThanRequestAmt(bool PBudgetEnabled)
        {
            #region Raise Error if Receipt > Request grouped by Project, Task, CostCode, Account Group
            if (PBudgetEnabled)
            {
                var reqDetGrouped = CashAdvanceRequestLines.Select()
                    .RowCast<ATPTEFMCARequestDetail>()
                    .Where(x => x.ProjectID != ProjectDefaultAttribute.NonProject())
                    .GroupBy(x => new { x.ProjectID, x.ProjectTaskID, x.CostCodeID, x.AccountGroup })
                    .Select(x => new {
                        ProjectID = x.Key.ProjectID,
                        TaskID = x.Key.ProjectTaskID,
                        CostCodeID = x.Key.CostCodeID,
                        AccountGroup = x.Key.AccountGroup,
                        CuryAmount = x.Sum(y => y.CuryAmount)
                    })
                    .ToList();

                foreach (ATPTEFMCAReceiptDetail recDet in CashAdvanceReceiptLines.Select())
                {
                    CashAdvanceReceiptLines.Cache.ClearFieldErrors<ATPTEFMCAReceiptDetail.curyNetAmt>(recDet);
                }

                foreach (var reqDet in reqDetGrouped)
                {
                    var receiptDetails = CashAdvanceReceiptLines.Select()
                        .RowCast<ATPTEFMCAReceiptDetail>()
                        .Where(x => x.ProjectID == reqDet.ProjectID
                        && x.ProjectTaskID == reqDet.TaskID
                        && x.CostCodeID == reqDet.CostCodeID
                        && x.AccountGroup == reqDet.AccountGroup
                        && x.Reversed == false)
                        .ToList();

                    decimal totalNetAmtByBudgetAccounts = receiptDetails.Sum(x => x.CuryNetAmt ?? 0m);

                    if (totalNetAmtByBudgetAccounts > reqDet.CuryAmount)
                    {
                        PMProject proj = PMProject.PK.Find(this, reqDet.ProjectID);
                        PMTask task = PMTask.PK.Find(this, reqDet.ProjectID, reqDet.TaskID);
                        PMCostCode costcode = PMCostCode.PK.Find(this, reqDet.CostCodeID);
                        PMAccountGroup accgroup = PMAccountGroup.PK.Find(this, reqDet.AccountGroup);

                        string errorMessage = ATPTEFMMessages.MessagesWithParameters.PBudgetReceiptAmtGreaterThanRequestAmt(
                            proj?.ContractCD ?? "", task?.TaskCD ?? "", costcode?.CostCodeCD ?? "", accgroup?.GroupCD ?? "");

                        foreach (ATPTEFMCAReceiptDetail recDet in receiptDetails)
                        {
                            CashAdvanceReceiptLines.Cache.RaiseExceptionHandling<ATPTEFMCAReceiptDetail.curyNetAmt>(
                                recDet,
                                recDet?.CuryNetAmt,
                                ATPTEFMHelper.GetPropertyException(recDet, errorMessage, PXErrorLevel.Error));
                        }
                    }
                }
            }
            #endregion
        }
        private void PBudgetValidationRaiseErrorForRequestAmountGreaterThanRemainingBudget(ATPTEFMCARequestDetail row, bool PBudgetEnabled)
        {
            if (PBudgetEnabled)
            {
                foreach (ATPTEFMPBudget result in ProjectBudget.Select())
                {
                    if (HasNull(result.ProjectID, result.ProjectTaskID, result.CostCodeID, result.AccountGroupID)) continue;

                    if (row.ProjectID == result.ProjectID && row.ProjectTaskID == result.ProjectTaskID && row.CostCodeID == result.CostCodeID && row.AccountGroup == result.AccountGroupID)
                    {
                        if (result.BudgetAmt < 0 && row.Amount > 0)
                        {
                            if (FeatureSetup.Current.ProjectBudgetValidation == RQRequestClassBudget.Error)
                                CashAdvanceRequestLines.Cache.RaiseExceptionHandling<ATPTEFMCARequestDetail.curyAmount>
                                    (row, row?.Amount, ATPTEFMHelper.GetPropertyException(row, ATPTEFMMessages.AmountRequestedIsGreaterThanRemainingBudget, PXErrorLevel.Error));
                            else if (FeatureSetup.Current.ProjectBudgetValidation == RQRequestClassBudget.Warning)
                                CashAdvanceRequestLines.Cache.RaiseExceptionHandling<ATPTEFMCARequestDetail.curyAmount>
                                    (row, row?.Amount, ATPTEFMHelper.GetPropertyException(row, ATPTEFMMessages.AmountRequestedIsGreaterThanRemainingBudget, PXErrorLevel.Warning));
                        }
                    }
                }
            }
        }
        public virtual APInvoice DoAdditionalCreateApBillProcess(APInvoice row) { return row; }
        public virtual EPExpenseClaimDetails AddAtcCode(EPExpenseClaimDetails row, ATPTEFMCAReceiptDetail receipt) { return row; }
        /// <remarks>
        /// 2024-11-18 :  Saving should be disallowed if 'Restrict Allowable Open CA' is selected and the set limits are exceeded. CASEID: 008723 {JLG} <br/>
        /// 2025-09-05 : base.Persist called even if cadv is null for delete operation. 013298 : RFS    
        /// 2025-09-10 : Cash Advance Financial Period Inactive CASE: 013395 {JLTG}
        /// </remarks>
        public override void Persist()
        {
            ATPTEFMCashAdvance cadv = this.CashAdvances.Current;
            if (cadv != null)
            {
                
                #region Budget Requirements
                List<ATPTEFMBudget> BudgetList = new List<ATPTEFMBudget>();
                List<ATPTEFMPBudget> PBudgetList = new List<ATPTEFMPBudget>();

                ATPTEFMBudgetEntry graph = PXGraph.CreateInstance<ATPTEFMBudgetEntry>();
                bool isDeleted = CashAdvances.Cache.Deleted.Any_();
                ATPTEFMCashAdvance curRecord = isDeleted ? CashAdvances?.Cache?.Deleted?.FirstOrDefault_() as ATPTEFMCashAdvance : CashAdvances?.Current;
                bool isCancelled = curRecord == null ? false : curRecord?.Status == ATPTEFMCashAdvanceStatusAttribute.Rejected ? true : false;

                List<ATPTEFMCARequestDetail> curLines = new List<ATPTEFMCARequestDetail>();
                foreach (ATPTEFMCARequestDetail item in CashAdvanceRequestLines.Cache.Deleted) { curLines?.Add(item); }
                #endregion

                #region Budget Validation

                bool validateBudget = cadv.BudgetEnabled ?? false;
                bool BudgetValidate = (FeatureSetup?.Current?.BudgetValidation ?? RQRequestClassBudget.None) == RQRequestClassBudget.Error;

                bool validatePBudget = cadv.ProjectBudgetEnabled ?? false;
                bool PBudgetValidate = (FeatureSetup?.Current?.ProjectBudgetValidation ?? RQRequestClassBudget.None) == RQRequestClassBudget.Error;

                if (BudgetValidate && validateBudget)
                {
                    using (PXTransactionScope ts = new PXTransactionScope())
                    {
                        bool isOverbudget = Budget?.Select()?.RowCast<ATPTEFMBudget>()?.ToList()?.Any(x => (x.BudgetAmt ?? 0m) < 0) ?? false;

                        if (isOverbudget)
                            throw new PXRowPersistedException(typeof(ATPTEFMBudget).Name, ts, Messages.ATPTEFMMessages.CheckBudget);

                        Budget.Cache.Persist(PXDBOperation.Insert);
                        Budget.Cache.Persist(PXDBOperation.Update);

                        ts.Complete(this);
                    }
                    Budget.Cache.Persisted(false);
                }
                if (PBudgetValidate && validatePBudget)
                {
                    using (PXTransactionScope ts = new PXTransactionScope())
                    {
                        bool isOverbudget = ProjectBudget?.Select()?.RowCast<ATPTEFMPBudget>()?.ToList()?.Any(x => (x.BudgetAmt ?? 0m) < 0) ?? false;

                        foreach (ATPTEFMPBudget row in ProjectBudget.Select())
                        {
                            if (HasNull(row?.ProjectID, row?.ProjectTaskID, row?.CostCodeID, row?.AccountGroupID)) continue;

                            ATPTEFMProjectBudgetLineSummary PBSummary = ProjectBudgetSummary.Select(row?.ProjectID, row?.ProjectTaskID, row?.CostCodeID, CashAdvances?.Current.Date.Value.Year.ToString(), row?.AccountGroupID);

                            if (PBSummary == null)
                                throw new PXRowPersistedException(typeof(ATPTEFMPBudget).Name, ts, Messages.ATPTEFMMessages.NotInProjectBudget);
                        }

                        if (isOverbudget)
                            throw new PXRowPersistedException(typeof(ATPTEFMPBudget).Name, ts, Messages.ATPTEFMMessages.CheckProjectBudget);

                        ProjectBudget.Cache.Persist(PXDBOperation.Insert);
                        ProjectBudget.Cache.Persist(PXDBOperation.Update);

                        ts.Complete(this);
                    }
                    ProjectBudget.Cache.Persisted(false);
                }
                #endregion
                
                #region Raise Error if Receipt > Request grouped by Account and Sub Account
                if (CashAdvanceReceiptLines.Cache.GetStatus(CashAdvanceReceiptLines.Current) == PXEntryStatus.Updated)
                {
                    CashAdvanceReceiptLines.Cache.RaiseRowUpdated(CashAdvanceReceiptLines.Current, null);
                }
                #endregion
                
                #region CA Setup Restrictions

                ATPTEFMCASetup setup = this.Preferences.Select();
                if (setup != null)
                {
                    EPEmployee employee = this.RequesterEmployee?.Select();

                    if (setup.AllowableCAAmt == true && employee != null)
                    {
                        ATPTEFMEPEmployeeExtension employeeExt = employee.GetExtension<ATPTEFMEPEmployeeExtension>();
                        if (cadv?.CuryRequestedAmount > (employeeExt?.UsrATPTEFMMaxCAAmt ?? decimal.Zero))
                        {
                            if (setup?.RVAllowableCAAmt == RQRequestClassBudget.Warning)
                                PXUIFieldAttribute.SetWarning<ATPTEFMCashAdvance.curyRequestedAmount>(CashAdvances.Cache, cadv, Messages.ATPTEFMMessages.MaxCAAmount);

                            else if (setup.RVAllowableCAAmt == RQRequestClassBudget.Error)
                            {
                                PXUIFieldAttribute.SetError<ATPTEFMCashAdvance.curyRequestedAmount>(CashAdvances.Cache, cadv, Messages.ATPTEFMMessages.MaxCAAmount);
                                throw new Exception(Messages.ATPTEFMMessages.MaxCAAmount);
                            }

                        }
                    }

                    if (setup?.AllowableOpenCA == true && employee != null)
                    {
                        ATPTEFMEPEmployeeExtension employeeExt = employee.GetExtension<ATPTEFMEPEmployeeExtension>();
                        if (employeeExt?.UsrATPTEFMOpenCA != null)
                        {
                            int cnt = 0;
                            foreach (ATPTEFMCashAdvance cad in PXSelect<
                                ATPTEFMCashAdvance,
                                Where2<
                                    Where<ATPTEFMCashAdvance.status, Equal<ATPTEFMCashAdvanceStatusAttribute.pendingValue>,
                                        Or<ATPTEFMCashAdvance.status, Equal<ATPTEFMCashAdvanceStatusAttribute.openValue>,
                                        Or<ATPTEFMCashAdvance.status, Equal<ATPTEFMCashAdvanceStatusAttribute.pendingLiquidationValue>>>>,
                                    And<ATPTEFMCashAdvance.requestedByID, Equal<Required<ATPTEFMCashAdvance.requestedByID>>>>>
                                .Select(this, cadv?.RequestedByID))
                            {
                                cnt++;
                            }
                            if (cnt > employeeExt?.UsrATPTEFMOpenCA)
                            {
                                if (setup.RVAllowableOpenCA == RQRequestClassBudget.Warning)
                                    PXUIFieldAttribute.SetWarning<ATPTEFMCashAdvance.requestedByID>(CashAdvances.Cache, cadv, Messages.ATPTEFMMessages.MaxCAOpen);

                                else if (setup?.RVAllowableOpenCA == RQRequestClassBudget.Error)
                                {
                                    PXUIFieldAttribute.SetError<ATPTEFMCashAdvance.requestedByID>(CashAdvances.Cache, cadv, Messages.ATPTEFMMessages.MaxCAOpen);
                                    throw new Exception(Messages.ATPTEFMMessages.MaxCAOpen);
                                }
                            }
                        }
                    }

                    if (setup?.RestrictCAEmployees == true && (cadv.ExecuteValidations ?? true))
                    {
                        if (employee != null)
                        {
                            ATPTEFMEPEmployeeExtension employeeExt = employee.GetExtension<ATPTEFMEPEmployeeExtension>();
                            if (employeeExt?.UsrATPTEFMCAUser != true)
                                throw new PXException(Messages.ATPTEFMMessages.CAUserError);
                        }
                    }
                }

                #endregion

                base.Persist();

                #region BudgetHistory
                
                if (curRecord != null) //RED-2904
                {
                    if (curRecord.BudgetEnabled ?? false)
                    {
                        BudgetList.Add(new ATPTEFMBudget() { RefNbr = curRecord?.CashAdvanceNbr, Origin = (int)OriginTypes.CashAdvance });
                        foreach (ATPTEFMBudget item in Budget.Select())
                        {
                            var row = item;
                            row.IsApproved = curRecord?.Approved ?? false;
                            BudgetList.Add(row);
                        }
                        graph.AddBudgetHistory(BudgetList);
                        Budget.Select();
                    }

                    if (curRecord.ProjectBudgetEnabled ?? false)
                    {
                        PBudgetList.Add(new ATPTEFMPBudget() { RefNbr = curRecord?.CashAdvanceNbr, Origin = (int)OriginTypes.CashAdvance });
                        foreach (ATPTEFMPBudget item in ProjectBudget?.Select())
                        {
                            var row = item;
                            row.IsApproved = curRecord?.Approved ?? false;
                            PBudgetList.Add(row);
                        }
                        graph.AddPBudgetHistory(PBudgetList);
                        ProjectBudget.Select();
                    }
                }

                if (isDeleted || isCancelled)
                {
                    foreach (ATPTEFMCARequestDetail item in curLines)
                    {
                        var row = new ATPTEFMBudget();
                        row.AcctID = item?.AccountID;
                        row.SubID = item?.SubID;
                        row.RefNbr = item?.CashAdvanceNbr;
                        row.Origin = (int)Classes.ATPTEFMBudgetLibrary.OriginTypes.CashAdvance;
                        BudgetList.Add(row);

                        var pRow = new ATPTEFMPBudget();
                        pRow.ProjectID = item?.ProjectID;
                        pRow.ProjectTaskID = item?.ProjectTaskID;
                        pRow.CostCodeID = item?.CostCodeID;
                        pRow.RefNbr = item?.CashAdvanceNbr;
                        pRow.Origin = (int)Classes.ATPTEFMBudgetLibrary.OriginTypes.CashAdvance;
                        PBudgetList.Add(pRow);
                    }
                    graph.DeleteBudgetHistory(BudgetList);
                    graph.DeletePBudgetHistory(PBudgetList);
                }
                
                #endregion
            }
            base.Persist();
        }
        public static void CloseTransaction(string paymentNbr)
        {
            var graph = PXGraph.CreateInstance<ATPTEFMCashAdvanceEntry>();

            IEnumerable<ATPTEFMCashAdvanceSummary> summaryCollection = graph.CashAdvanceSummary(graph, paymentNbr);

            if (summaryCollection == null) return;

            ATPTEFMCashAdvanceSummary summary = summaryCollection.FirstOrDefault();

            ATPTEFMCashAdvance document = graph.CashAdvances.Search<ATPTEFMCashAdvance.cashAdvanceNbr>(summary.CashAdvanceNbr);

            if (document == null) return;

            #region Create Budget History

            ATPTEFMCAReceiptDetail receipt = graph.CashAdvanceReceiptLines.Search<ATPTEFMCAReceiptDetail.cashAdvanceReceiptDetailIID>(summary.ReceiptDetailIID);

            List<ATPTEFMBudget> budgetList = new List<ATPTEFMBudget>();

            foreach (var item in graph.Budget
                                      .Select()
                                      .FirstTableItems
                                      .ToList()
                                      .Where(x => x.AcctID == summary.AcctID &&
                                                  x.SubID == summary.SubID))
            {
                budgetList.Add(new ATPTEFMBudget
                {
                    AcctID = item.AcctID,
                    SubID = item.SubID,
                    Origin = (int?)OriginTypes.CashAdvance,
                    RefNbr = summary.CashAdvanceNbr,
                    CuryID = item.CuryID,
                    DocAmt = summary.Amount,
                    RequestAmt = item.RequestAmt,
                    BudgetAmt = item.BudgetAmt,
                    SpentAmt = item.SpentAmt,
                    ApprovedAmt = item.DocAmt,
                    UnapprovedAmt = 0M,
                    FinPeriodID = item.FinPeriodID
                });
            }

            graph.CreateBudgetHistory(graph.History, budgetList);
            #endregion

            #region Close Cash Advance Request

            foreach (ATPTEFMCAReceiptDetail item in graph.CashAdvanceReceiptLines.Select()
                                                             .FirstTableItems.Where(x => x.CashAdvanceRequestDetailID != summary.RequestDetailID))
            {
                if (graph.RequestHasOpenAP(graph, item.ExpenseReceiptRefNbr))
                {
                    break;
                }
            }

            #endregion

            graph.Actions.PressSave();
        }

        public IEnumerable<ATPTEFMCashAdvanceSummary> CashAdvanceSummary(PXGraph graph, string paymentNbr)
        {
            PXResult<ATPTEFMCAReceiptDetail, ATPTEFMCARequestDetail, ATPTEFMCashAdvance, EPExpenseClaimDetails> detail =
                PXSelectJoin<
                    ATPTEFMCAReceiptDetail,
                    InnerJoin<ATPTEFMCARequestDetail,
                        On<ATPTEFMCARequestDetail.cashAdvanceRequestDetailID, Equal<ATPTEFMCAReceiptDetail.cashAdvanceRequestDetailID>>,
                    InnerJoin<ATPTEFMCashAdvance,
                        On<ATPTEFMCashAdvance.cashAdvanceNbr, Equal<ATPTEFMCARequestDetail.cashAdvanceNbr>>,
                    InnerJoin<EPExpenseClaimDetails,
                        On<EPExpenseClaimDetails.claimDetailCD, Equal<ATPTEFMCAReceiptDetail.expenseReceiptRefNbr>>,
                    InnerJoin<APInvoice,
                        On<APInvoice.refNbr, Equal<EPExpenseClaimDetails.aPRefNbr>>>>>>,
                    Where<APInvoice.refNbr, Equal<Required<APInvoice.refNbr>>>>
                    .Select<PXResultset<ATPTEFMCAReceiptDetail, ATPTEFMCARequestDetail, ATPTEFMCashAdvance, EPExpenseClaimDetails>>(graph, paymentNbr);

            if (detail == null) return null;

            ATPTEFMCAReceiptDetail rec = detail;
            ATPTEFMCashAdvance ca = detail;
            ATPTEFMCARequestDetail req = detail;
            EPExpenseClaimDetails claim = detail;

            List<ATPTEFMCashAdvanceSummary> summary = new List<ATPTEFMCashAdvanceSummary>
            {
                new ATPTEFMCashAdvanceSummary
                {
                    CashAdvanceNbr = ca.CashAdvanceNbr,
                    RequestDetailID = req.CashAdvanceRequestDetailID,
                    ReceiptDetailIID = rec.CashAdvanceReceiptDetailIID,
                    AcctID = claim.ExpenseAccountID,
                    SubID = claim.ExpenseSubID,
                    Amount = claim.CuryExtCost
                }
            };

            return summary;
        }

        public bool RequestHasOpenAP(PXGraph graph, string expenseNbr)
        {
            PXResult<ATPTEFMCAReceiptDetail, ATPTEFMCARequestDetail, ATPTEFMCashAdvance, EPExpenseClaimDetails, APInvoice> detail =
                PXSelectJoin<
                    ATPTEFMCAReceiptDetail,
                    InnerJoin<ATPTEFMCARequestDetail,
                        On<ATPTEFMCARequestDetail.cashAdvanceRequestDetailID, Equal<ATPTEFMCAReceiptDetail.cashAdvanceRequestDetailID>>,
                    InnerJoin<ATPTEFMCashAdvance,
                        On<ATPTEFMCashAdvance.cashAdvanceNbr, Equal<ATPTEFMCARequestDetail.cashAdvanceNbr>>,
                    InnerJoin<EPExpenseClaimDetails,
                        On<EPExpenseClaimDetails.claimDetailCD, Equal<ATPTEFMCAReceiptDetail.expenseReceiptRefNbr>>,
                    InnerJoin<APInvoice,
                        On<APInvoice.refNbr, Equal<EPExpenseClaimDetails.aPRefNbr>>>>>>,
                    Where<EPExpenseClaimDetails.claimDetailCD, Equal<Required<EPExpenseClaimDetails.claimDetailCD>>>>
                    .Select<PXResultset<ATPTEFMCAReceiptDetail, ATPTEFMCARequestDetail, ATPTEFMCashAdvance, EPExpenseClaimDetails, APInvoice>>(graph, expenseNbr);

            APInvoice invoice = detail;

            if (invoice == null) return true;

            return (invoice.Status != APDocStatus.Closed);
        }

        public void CreateBudgetHistory(PXSelect<ATPTEFMBudgetHistory> historyView, List<ATPTEFMBudget> items)
        {
            //AddBudgetHistory(historyView, items);
        }

        private bool IsNonWorkDay(CSCalendar calendar, string calendarId, DateTime date)
        {
            var isHoliday = CalendarHelper.IsHoliday(this, calendarId, date);
            return (date.DayOfWeek == DayOfWeek.Monday && (calendar.MonWorkDay == false || isHoliday))
                || (date.DayOfWeek == DayOfWeek.Tuesday && (calendar.TueWorkDay == false || isHoliday))
                || (date.DayOfWeek == DayOfWeek.Wednesday && (calendar.WedWorkDay == false || isHoliday))
                || (date.DayOfWeek == DayOfWeek.Thursday && (calendar.ThuWorkDay == false || isHoliday))
                || (date.DayOfWeek == DayOfWeek.Friday && (calendar.FriWorkDay == false || isHoliday))
                || (date.DayOfWeek == DayOfWeek.Saturday && (calendar.SatWorkDay == false || isHoliday))
                || (date.DayOfWeek == DayOfWeek.Sunday && (calendar.SunWorkDay == false || isHoliday));
        }

        public int GetNumberOfLiquidationDays()
        {
            int numDays = 1;
            ATPTEFMCASetup setup = this.Preferences.Current;

            if (setup.LiquidationRule == ATPTEFMLiquidationRuleAttribute.RequestClass)
            {
                ATPTEFMReqClass reqClass = PXSelect<
                    ATPTEFMReqClass,
                    Where<ATPTEFMReqClass.reqClassID, Equal<Required<ATPTEFMReqClass.reqClassID>>>>
                    .Select(this, this.CashAdvances.Current.ReqClassID);
                if (reqClass != null)
                    numDays = reqClass.NoDaysLiquidate ?? 1;
            }
            else if (setup.LiquidationRule == ATPTEFMLiquidationRuleAttribute.Employee)
            {
                EPEmployee employee = this.RequesterEmployee.Select();
                if (employee != null)
                {
                    ATPTEFMEPEmployeeExtension employeeExt = employee.GetExtension<ATPTEFMEPEmployeeExtension>();
                    numDays = employeeExt.UsrATPTEFMAllowableDays ?? 1;
                }
            }
            else
            {
                numDays = setup.StandardAllowableDays ?? 1;
            }

            return numDays;
        }

        public DateTime GetLiquidationDateWorkCalendar()
        {
            DateTime liquidationDate = new DateTime();
            ATPTEFMCashAdvance ca = CashAdvances.Current;

            if (ca != null)
            {
                EPEmployee employee = this.RequesterEmployee.Select();
                CSCalendar calendar = PXSelect<
                    CSCalendar,
                    Where<CSCalendar.calendarID,
                    Equal<Required<CSCalendar.calendarID>>>>
                    .Select(this, employee.CalendarID);

                bool isNonWorkDay = false;
                liquidationDate = ca.DateOfUse.Value;
                int dayCounter = 0;
                int? liquidationDays = GetNumberOfLiquidationDays();

                do
                {
                    liquidationDate = liquidationDate.AddDays(1);
                    isNonWorkDay = IsNonWorkDay(calendar, employee.CalendarID, liquidationDate);

                    if (isNonWorkDay)
                        continue;

                    dayCounter++;
                }
                while (dayCounter < liquidationDays);
            }

            return liquidationDate;
        }
        public void CheckInventoryDuplicate()
        {
            List<int> inventoryids = new List<int>();
            foreach (ATPTEFMCARequestDetail det in this.CashAdvanceRequestLines.Select())
            {
                inventoryids.Add(det.InventoryID ?? 0);
            }
            IEnumerable<int> duplicates = inventoryids.GroupBy(x => x).Where(g => g.Count() > 1).Select(x => x.Key);

            if (duplicates != null && duplicates.Any())
                throw new Exception(Messages.ATPTEFMMessages.DuplicateInventory);
        }
        public void CheckExistingOpenCA()
        {
            //bool ret = false;
            ATPTEFMCashAdvance cadv = PXSelect<
                ATPTEFMCashAdvance,
                Where<ATPTEFMCashAdvance.status, Equal<ATPTEFMCashAdvanceStatusAttribute.openValue>,
                    And<ATPTEFMCashAdvance.cashAdvanceNbr, NotEqual<Required<ATPTEFMCashAdvance.cashAdvanceNbr>>,
                    And<ATPTEFMCashAdvance.requestedByID, Equal<Required<ATPTEFMCashAdvance.requestedByID>>>>
                    >>
                .Select(this, this.CashAdvances.Current.CashAdvanceNbr, this.CashAdvances.Current.RequestedByID);
            if (cadv != null)
                throw new Exception(Messages.ATPTEFMMessages.EnabledDocumentOverrid);
        }

        #endregion

        #region CachedAttached

        [PXMergeAttributes(Method = MergeMethod.Merge)]
        [PXRemoveBaseAttribute(typeof(INUnitAttribute))]
        [INUnit(DisplayName = ATPTEFMMessages.UOM, Visibility = PXUIVisibility.SelectorVisible)]
        protected virtual void InventoryItem_BaseUnit_CacheAttached(PXCache sender) { }

        [PXMergeAttributes(Method = MergeMethod.Merge)]
        [PXRemoveBaseAttribute(typeof(AccountAttribute))]
        [Account(DisplayName = ATPTEFMMessages.AccountID, Visibility = PXUIVisibility.Visible, DescriptionField = typeof(Account.description), AvoidControlAccounts = true)]
        protected virtual void InventoryItem_COGSAcctID_CacheAttached(PXCache sender) { }

        [PXMergeAttributes(Method = MergeMethod.Merge)]
        [PXRemoveBaseAttribute(typeof(SubAccountAttribute))]
        [SubAccount(typeof(InventoryItem.cOGSAcctID), DisplayName = ATPTEFMMessages.SubAct, Visibility = PXUIVisibility.Visible, DescriptionField = typeof(Sub.description))]
        protected virtual void InventoryItem_COGSSubID_CacheAttached(PXCache sender) { }

        [PXMergeAttributes(Method = MergeMethod.Merge)]
        [PXRemoveBaseAttribute(typeof(PXUIFieldAttribute))]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.ProjectID)]
        protected virtual void Contract_Description_CacheAttached(PXCache sender) { }

        [PXMergeAttributes(Method = MergeMethod.Merge)]
        [PXRemoveBaseAttribute(typeof(PXUIFieldAttribute))]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.ProjectTaskid)]
        protected virtual void PMTask_Description_CacheAttached(PXCache sender) { }

        [PXMergeAttributes(Method = MergeMethod.Merge)]
        [PXRemoveBaseAttribute(typeof(PXUIFieldAttribute))]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.Vendor)]
        protected virtual void Vendor_AcctName_CacheAttached(PXCache sender) { }

        [PXMergeAttributes(Method = MergeMethod.Merge)]
        [PXRemoveBaseAttribute(typeof(PXUIFieldAttribute))]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.Address)]
        protected virtual void Address_AddressLine1_CacheAttached(PXCache sender) { }

        [PXMergeAttributes(Method = MergeMethod.Merge)]
        [PXRemoveBaseAttribute(typeof(PXUIFieldAttribute))]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.TIN)]
        protected virtual void LocationExtAddress__TaxRegistrationID_CacheAttached(PXCache sender) { }

        #endregion

        #region Select Delegate
        public IEnumerable receiptsForSubmit()
        {
            List<ATPTEFMCARequestDetail> listOfReceipts = new List<ATPTEFMCARequestDetail>();

            foreach (ATPTEFMCARequestDetail result in PXSelect<
                ATPTEFMCARequestDetail,
                Where<ATPTEFMCARequestDetail.cashAdvanceNbr, Equal<Current<ATPTEFMCashAdvance.cashAdvanceNbr>>>>
                .Select(this)
                .ToList())
            {
                ATPTEFMCARequestDetail r = (ATPTEFMCARequestDetail)result;
                List<ATPTEFMCAReceiptDetail> receipts = CashAdvanceReceiptLines.Select().RowCast<ATPTEFMCAReceiptDetail>().Where(w => w.CashAdvanceRequestDetailID == r.CashAdvanceRequestDetailID).ToList();

                decimal? totalReceipts = receipts.Where(s => s.Reversed != true).Sum(s => s.CuryNetAmt);
                r.Balance = r.CuryAmount - totalReceipts ?? 0;
                decimal? totalQty = receipts.Sum(s => s.NetQty);
                r.RunningQty = r.Qty;

                listOfReceipts.Add(r);
            }
            return listOfReceipts;
        }
        /// <remarks>
        /// 009892 - [DLS] To display the Voided Refund on Voided Tab of CA Screen
        /// </remarks>
        protected virtual IEnumerable voidedDocuments()
        {
            List<ATPTEFMVoidedDocument> docs = new List<ATPTEFMVoidedDocument>();

            //Payment
            foreach (APInvoice voidedPayment in PXSelect<
                APInvoice,
                Where<APInvoice.docType, Equal<APDocType.prepayment>,
                    And<APInvoice.status, Equal<APDocStatus.voided>,
                    And<ATPTEFMAPRegisterExt.usrATPTEFMSourceRef, Equal<Current<ATPTEFMCashAdvance.cashAdvanceNbr>>>>>>
                .Select(this))
            {
                ATPTEFMVoidedDocument vDocs = new ATPTEFMVoidedDocument();
                vDocs.BranchID = voidedPayment.BranchID;
                vDocs.DocType = voidedPayment.DocType;
                vDocs.RefNbr = voidedPayment.RefNbr;
                vDocs.Date = voidedPayment.DocDate;
                vDocs.VendorID = voidedPayment.VendorID;
                vDocs.Descr = voidedPayment.DocDesc;
                vDocs.Amount = voidedPayment.CuryOrigDocAmt;
                vDocs.CreatedDateTime = voidedPayment.CreatedDateTime;
                docs.Add(vDocs);
            }

            //Check
            foreach (APPayment voidedCheck in PXSelect<
                APPayment,
                Where<APPayment.docType, Equal<APDocType.check>,
                    And<APPayment.status, Equal<APDocStatus.voided>,
                    And<ATPTEFMAPRegisterExt.usrATPTEFMSourceRef, Equal<Current<ATPTEFMCashAdvance.cashAdvanceNbr>>>>>>
                .Select(this))
            {
                ATPTEFMVoidedDocument vDocs = new ATPTEFMVoidedDocument();
                vDocs.BranchID = voidedCheck.BranchID;
                vDocs.DocType = voidedCheck.DocType;
                vDocs.RefNbr = voidedCheck.RefNbr;
                vDocs.Date = voidedCheck.DocDate;
                vDocs.VendorID = voidedCheck.VendorID;
                vDocs.Descr = voidedCheck.DocDesc;
                vDocs.Amount = voidedCheck.CuryOrigDocAmt;
                vDocs.CreatedDateTime = voidedCheck.CreatedDateTime;
                docs.Add(vDocs);
            }

            //Refund (voided status)
            foreach (APPayment voidedCheck in PXSelect<
                APPayment,
                Where<APPayment.docType, Equal<APDocType.refund>,
                    And<APPayment.status, Equal<APDocStatus.voided>,
                    And<ATPTEFMAPRegisterExt.usrATPTEFMSourceRef, Equal<Current<ATPTEFMCashAdvance.cashAdvanceNbr>>>>>>
                .Select(this))
            {
                ATPTEFMVoidedDocument vDocs = new ATPTEFMVoidedDocument();
                vDocs.BranchID = voidedCheck.BranchID;
                vDocs.DocType = voidedCheck.DocType;
                vDocs.RefNbr = voidedCheck.RefNbr;
                vDocs.Date = voidedCheck.DocDate;
                vDocs.VendorID = voidedCheck.VendorID;
                vDocs.Descr = voidedCheck.DocDesc;
                vDocs.Amount = voidedCheck.CuryOrigDocAmt;
                vDocs.CreatedDateTime = voidedCheck.CreatedDateTime;
                docs.Add(vDocs);
            }

            List<ATPTEFMVoidedDocument> orderedView = docs.OrderByDescending(o => o.CreatedDateTime).ToList();
            int i = 1;
            foreach (ATPTEFMVoidedDocument ov in orderedView)
            {
                ov.SortID = i;
                i++;
                yield return ov;
            }
        }
        public IEnumerable budget()
        {
            #region Variables
            ATPTEFMCashAdvance parent = CashAdvances?.Current;
            //if (!(parent.BudgetEnabled ?? false)) yield break;

            parent.IsOverbudget = false;
            parent.HasInitialBudget = false;
            int? ledgerID = FeatureSetup?.Current?.BudgetLedgerID;
            FinPeriodData fData = GetFinPeriod(this, parent?.FinPeriodID, FeatureSetup?.Current?.BudgetCalculation);
            ATPTEFMCARequestDetail parentDet = PXSelect<ATPTEFMCARequestDetail,
                Where<ATPTEFMCARequestDetail.cashAdvanceNbr, Equal<@P.AsString>,
                And<ATPTEFMCARequestDetail.projectID, Equal<@P.AsInt>,
                And<ATPTEFMCARequestDetail.accountID, IsNotNull,
                And<ATPTEFMCARequestDetail.subID, IsNotNull>>>>>
                .Select(this, parent.CashAdvanceNbr, ProjectDefaultAttribute.NonProject());
            bool showbudget = parentDet != null;

            if (HasNull(parent, ledgerID, fData) || !(parent.BudgetEnabled ?? false) || !showbudget)
            {
                Budget.AllowSelect = false;
                yield break;
            }

            List<BudgetParameters> parameterList = new List<BudgetParameters>();
            List<ATPTEFMCADetailSummary> detailList = new List<ATPTEFMCADetailSummary>();
            #endregion

            #region Supply Parameters

            #region Request
            PXResultset<ATPTEFMCARequestDetail> requests = PXSelect<
                                                ATPTEFMCARequestDetail,
                                                Where<ATPTEFMCARequestDetail.cashAdvanceNbr, Equal<Current<ATPTEFMCashAdvance.cashAdvanceNbr>>>>
                                                .Select(this);
            foreach (ATPTEFMCARequestDetail item in requests)
            {
                if (item.ProjectID != ProjectDefaultAttribute.NonProject()) continue;
                detailList.Add(new ATPTEFMCADetailSummary
                {
                    RequestID = item.CashAdvanceRequestDetailID,
                    AcctID = item.AccountID,
                    SubID = item.SubID,
                    Qty = item.Qty,
                    UnitCost = item.UnitCost,
                    NetQty = 0,
                    NetAmt = 0
                });
            }
            #endregion

            #region Receipt
            IEnumerable<PXResult<ATPTEFMCAReceiptDetail, ATPTEFMCARequestDetail>> receipt =
                PXSelectJoin<
                    ATPTEFMCAReceiptDetail,
                    InnerJoin<ATPTEFMCARequestDetail,
                        On<ATPTEFMCAReceiptDetail.cashAdvanceRequestDetailID, Equal<ATPTEFMCARequestDetail.cashAdvanceRequestDetailID>>>,
                    Where<ATPTEFMCARequestDetail.cashAdvanceNbr, Equal<Current<ATPTEFMCashAdvance.cashAdvanceNbr>>>>
                    .Select(this)
                    .AsEnumerable()
                    .Select(x => (PXResult<ATPTEFMCAReceiptDetail, ATPTEFMCARequestDetail>)x);
            foreach (var items in receipt.GroupBy(x => new { ((ATPTEFMCARequestDetail)x).CashAdvanceRequestDetailID }))
            {
                var result = detailList
                    .Where(x => x.RequestID == items.Key.CashAdvanceRequestDetailID)
                    .FirstOrDefault();

                foreach (PXResult<ATPTEFMCAReceiptDetail, ATPTEFMCARequestDetail> item in items)
                {
                    ATPTEFMCAReceiptDetail rec = item;

                    if (rec.ProjectID != ProjectDefaultAttribute.NonProject()) continue;

                    result.NetQty += rec.NetQty;

                    if (!rec.Reversed ?? false)
                        result.NetAmt += rec.CuryNetAmt;
                }
            }
            #endregion

            foreach (ATPTEFMCADetailSummary detail in detailList)
            {
                parameterList.Add(new BudgetParameters()
                {
                    LedgerID = ledgerID,
                    BranchID = parent.BranchID,
                    RefNbr = parent.CashAdvanceNbr,
                    CuryID = company.Current.BaseCuryID,
                    AccountID = detail.AcctID,
                    SubID = detail.SubID,
                    FinYear = fData.FinYear,
                    FromFinPeriodID = fData.StartPeriod,
                    ToFinPeriodID = fData.EndPeriod,
                    FinPeriodID = parent.FinPeriodID,
                    //Amount = parent?.Status == ATPTEFMCashAdvanceStatusAttribute.CancelledValue ? 0 : parent?.Status == ATPTEFMCashAdvanceStatusAttribute.ClosedValue ? detail.NetAmt : detail.Qty * detail.UnitCost,
                    Amount = parent?.Status == ATPTEFMCashAdvanceStatusAttribute.CancelledValue ? 0 : detail.Qty * detail.UnitCost,
                    Approved = parent.Approved ?? false,
                    Released = parent.Approved ?? false,
                    OriginType = OriginTypes.CashAdvance
                });
            }
            #endregion

            Budget.AllowSelect = (parent.BudgetEnabled ?? false) && parameterList.Any();
            if (!parameterList.Any()) yield break;

            foreach (ATPTEFMBudget item in GenerateBudget(this, parameterList))
            {
                yield return item;

                if (!parent.IsOverbudget ?? false)
                {
                    // CASE: 003020
                    // 1. Is Over Budget will only be tick if Request Amount (includes approved and unapproved) is greater than Initial Budget
                    if ((item.ApprovedAmt + item.UnapprovedAmt) > item.InitialBudget) parent.IsOverbudget = true;
                    // END CASE: 003020
                }

                if (!parent.HasInitialBudget ?? false)
                {
                    if (item.InitialBudget > 0) parent.HasInitialBudget = true;
                }

                // CASE: 003020 - Temp Implementation
                // 2. Remaining Budget column is equal to Initial Budget less all approved Amount.
                if (item.ApprovedAmt > 0 && FeatureSetup?.Current?.BudgetBudgetAmount == "F3") item.BudgetAmt = item.InitialBudget - item.ApprovedAmt;
                // END CASE: 003020

                if (item.BudgetAmt >= 0 || Budget.AllowSelect == false) continue;

                bool isError = FeatureSetup?.Current?.BudgetValidation == RQRequestClassBudget.Error
                    && (new PXEntryStatus[] {
                        CashAdvances.Cache.GetStatus(CashAdvances.Current),
                        CashAdvanceRequestLines.Cache.GetStatus(CashAdvanceRequestLines.Current),
                        CashAdvanceReceiptLines.Cache.GetStatus(CashAdvanceReceiptLines.Current)
                    }.Any(x => x != PXEntryStatus.Notchanged));
                bool isWarning = FeatureSetup?.Current?.BudgetValidation == RQRequestClassBudget.Warning;

                if (isError || isWarning)
                {
                    Budget.Cache.RaiseExceptionHandling<ATPTEFMBudget.budgetAmt>(item, item.BudgetAmt,
                        ATPTEFMHelper.GetPropertyException(item, Messages.ATPTEFMMessages.RemainingBudgetMustNotBeLessThanZero,
                            isError ? PXErrorLevel.RowError : PXErrorLevel.Warning));
                }
            }
        }
        public virtual IEnumerable projectBudget()
        {
            #region Variables
            ATPTEFMCashAdvance parent = this.CashAdvances?.Current;

            if (!(parent?.ProjectBudgetEnabled ?? false)) yield break;

            FinPeriodData fData = GetFinPeriod(this, parent?.FinPeriodID, FeatureSetup?.Current?.ProjectBudgetCalculation);

            //if (HasNull(parent, fData) || !ShowProjectBudget) yield break;
            if (HasNull(parent, fData)) yield break;

            List<ProjectBudgetParameters> parameterList = new List<ProjectBudgetParameters>();
            List<ATPTEFMCADetailSummary> detailList = new List<ATPTEFMCADetailSummary>();

            #endregion

            #region Supply Parameters

            #region Request
            PXResultset<ATPTEFMCARequestDetail> requests = PXSelect<
                                                ATPTEFMCARequestDetail,
                                                Where<ATPTEFMCARequestDetail.cashAdvanceNbr, Equal<Current<ATPTEFMCashAdvance.cashAdvanceNbr>>>>
                                                .Select(this);
            foreach (ATPTEFMCARequestDetail item in requests)
            {
                if (HasNull(item.ProjectID, item.ProjectTaskID, item.CostCodeID, item.AccountGroup, item.InventoryID)) continue;

                detailList.Add(new ATPTEFMCADetailSummary
                {
                    RequestID = item.CashAdvanceRequestDetailID,
                    ProjectID = item.ProjectID,
                    ProjectTaskID = item.ProjectTaskID,
                    InventoryID = item.InventoryID,
                    CostCodeID = item.CostCodeID,
                    AccountGroupID = item.AccountGroup,
                    Qty = item.Qty,
                    UnitCost = item.UnitCost,
                    NetQty = 0,
                    NetAmt = 0,
                    CuryID = parent.CuryID
                });
            }
            #endregion

            #region Receipt
            IEnumerable<PXResult<ATPTEFMCAReceiptDetail, ATPTEFMCARequestDetail>> receipt =
                PXSelectJoin<
                    ATPTEFMCAReceiptDetail,
                    InnerJoin<ATPTEFMCARequestDetail,
                        On<ATPTEFMCAReceiptDetail.cashAdvanceRequestDetailID, Equal<ATPTEFMCARequestDetail.cashAdvanceRequestDetailID>>>,
                    Where<ATPTEFMCARequestDetail.cashAdvanceNbr, Equal<Current<ATPTEFMCashAdvance.cashAdvanceNbr>>,
                    And<ATPTEFMCAReceiptDetail.reversed, Equal<False>>>>
                    .Select(this)
                    .AsEnumerable()
                    .Select(x => (PXResult<ATPTEFMCAReceiptDetail, ATPTEFMCARequestDetail>)x);
            foreach (var items in receipt.GroupBy(x => new { ((ATPTEFMCARequestDetail)x).CashAdvanceRequestDetailID }))
            {
                var result = detailList
                    .Where(x => x.RequestID == items.Key.CashAdvanceRequestDetailID)
                    .FirstOrDefault();

                foreach (PXResult<ATPTEFMCAReceiptDetail, ATPTEFMCARequestDetail> item in items)
                {
                    ATPTEFMCAReceiptDetail rec = item;
                    if (HasNull(rec.ProjectID, rec.ProjectTaskID, rec.CostCodeID)) continue;
                    result.NetQty += rec.NetQty;
                    result.NetAmt += rec.CuryNetAmt;
                }
            }
            #endregion

            foreach (ATPTEFMCADetailSummary detail in detailList)
            {
                parameterList.Add(new ProjectBudgetParameters()
                {
                    RefNbr = parent.CashAdvanceNbr,
                    CuryID = company.Current.BaseCuryID,
                    ProjectID = detail.ProjectID,
                    LedgerID = FeatureSetup?.Current?.ProjectBudgetLedgerID,
                    ProjectTaskID = detail.ProjectTaskID,
                    CostCodeID = detail.CostCodeID,
                    InventoryID = detail.InventoryID,
                    AccountGroupID = detail.AccountGroupID,
                    FinYear = fData.FinYear,
                    FromFinPeriodID = fData.StartPeriod,
                    ToFinPeriodID = fData.EndPeriod,
                    FinPeriodID = parent.FinPeriodID,
                    //Amount = ((detail.Qty - detail.NetQty) * detail.UnitCost) + detail.NetAmt,
                    //Amount = parent?.Status == ATPTEFMCashAdvanceStatusAttribute.CancelledValue ? 0 : parent?.Status == ATPTEFMCashAdvanceStatusAttribute.ClosedValue ? detail.NetAmt : detail.Qty * detail.UnitCost,
                    Amount = parent?.Status == ATPTEFMCashAdvanceStatusAttribute.CancelledValue ? 0 :detail.Qty * detail.UnitCost,
                    Approved = parent.Approved ?? false,
                    Released = parent.Approved ?? false,
                    OriginType = OriginTypes.CashAdvance
                });
            }

            #endregion

            ProjectBudget.AllowSelect = parameterList.Any();
            if (!parameterList.Any()) yield break;

            foreach (ATPTEFMPBudget item in GenerateProjectBudget(this, parameterList))
            {
                yield return item;

                bool isError = FeatureSetup?.Current?.ProjectBudgetValidation == RQRequestClassBudget.Error
                    && (new PXEntryStatus[] {
                        CashAdvances.Cache.GetStatus(CashAdvances.Current),
                        CashAdvanceRequestLines.Cache.GetStatus(CashAdvanceRequestLines.Current),
                        CashAdvanceReceiptLines.Cache.GetStatus(CashAdvanceReceiptLines.Current)
                    }.Any(x => x != PXEntryStatus.Notchanged));
                bool isWarning = FeatureSetup?.Current?.ProjectBudgetValidation == RQRequestClassBudget.Warning;

                if (isError || isWarning)
                {
                    ATPTEFMProjectBudgetLineSummary PBSummary = ProjectBudgetSummary.Select(item.ProjectID, item.ProjectTaskID, item.CostCodeID, CashAdvances.Current.Date.Value.Year.ToString(), item.AccountGroupID);

                    if (PBSummary == null)
                    {
                        ProjectBudget.Cache.RaiseExceptionHandling<ATPTEFMPBudget.projectID>(item, item.ProjectID,
                            ATPTEFMHelper.GetPropertyException(item, Messages.ATPTEFMMessages.NotInProjectBudget,
                                isError ? PXErrorLevel.RowError : PXErrorLevel.Warning));
                    }
                    else if (item.BudgetAmt < 0)
                    {
                        ProjectBudget.Cache.RaiseExceptionHandling<ATPTEFMPBudget.budgetAmt>(item, item.BudgetAmt,
                            ATPTEFMHelper.GetPropertyException(item, Messages.ATPTEFMMessages.RemainingBudgetMustNotBeLessThanZero,
                                isError ? PXErrorLevel.RowError : PXErrorLevel.Warning));
                    }
                }
            }
        }

        #endregion

        #region Actions
        public PXInitializeState<ATPTEFMCashAdvance> InitializeState;

        public PXAction<ATPTEFMCashAdvance> ReturnExcessCashAdvance;
        [PXButton(Category = Messages.ATPTEFMMessages.Action), PXUIField(DisplayName = Messages.ATPTEFMMessages.ReturnExcessCashAdvance)]
        public virtual IEnumerable returnExcessCashAdvance(PXAdapter adapter)
        {
            ATPTEFMHelper.StartLongOperation(this, adapter, delegate ()
            {
                using (PXTransactionScope ts = new PXTransactionScope())
                {
                    ATPTEFMCashAdvance ca = CashAdvances.Current;
                    string docdesc = $"{Messages.ATPTEFMMessages.ReturnExcessCashAdvance} {ca.CashAdvanceNbr ?? ""}";
                    APPaymentEntry paymentEntry = PXGraph.CreateInstance<APPaymentEntry>();

                    if (ca != null)
                    {
                        APPayment pay = paymentEntry.Document.Insert(new APPayment
                        {
                            DocType = APDocType.Refund,
                            VendorID = ca.RequestedByID,
                            ExtRefNbr = ca.CashAdvanceNbr + "-2",
                            CuryOrigDocAmt = ca.PpmBalance,
                            DocDesc = docdesc,
                            Hold = true,
                            Status = APDocStatus.Hold,
                            OrigRefNbr = ca.CashAdvanceNbr,
                        });

                        ATPTEFMAPRegisterExt payExt = pay.GetExtension<ATPTEFMAPRegisterExt>();

                        payExt.UsrATPTEFMSourceType = ATPTEFMSourceTypeAttribute.CashAdvance;
                        payExt.UsrATPTEFMSourceRef = ca.CashAdvanceNbr;

                        paymentEntry.Document.Update(pay);
                        paymentEntry.Save.Press();

                        APAdjust adj = paymentEntry.Adjustments.Insert(new APAdjust
                        {
                            AdjdDocType = APDocType.Prepayment,
                            AdjdRefNbr = ca.PpmRefNbr,
                            CuryAdjgAmt = ca.PpmBalance,
                        });
                        paymentEntry.Adjustments.Update(adj);
                        paymentEntry.Save.Press();
                    }

                    ca.VendorRefundRefNbr = paymentEntry.Document.Current.RefNbr;
                    CashAdvances.Update(ca);
                    Save.Press();
                    CashAdvances.View.RequestRefresh();
                    ts.Complete();
                    //OpenCheck
                    throw new PXRedirectRequiredException(paymentEntry, true, "Check") { Mode = PXBaseRedirectException.WindowMode.NewWindow };
                }
            });
            return adapter.Get();
        }

        public PXAction<ATPTEFMCashAdvance> CreateAPBill;
        /// <remarks>
        /// 2025-07-08 : 012116 - Retrigger Tax Category to remove all taxes. See on ATPTAPInvoiceDefaultATCAttribute of Philtax {RRS}
        /// </remarks>
        [PXProcessButton(Category = "Actions", IsLockedOnToolbar = true, Connotation = PX.Data.WorkflowAPI.ActionConnotation.Success)]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.CreateAPBill)]
        public virtual IEnumerable createAPBill(PXAdapter adapter)
        {
            if (Save.GetEnabled())
            {
                Save.Press();
            }
            ATPTEFMCashAdvance ca = CashAdvances.Current;
            APInvoice createdPPM = null;
            var graph = this;
            Guid key1 = new Guid();
            PXLongOperation.StartOperation(key1, () =>
            {
                using (PXTransactionScope ts = new PXTransactionScope())
                {
                    if (ca != null)
                    {
                        APInvoiceEntry invEntry = PXGraph.CreateInstance<APInvoiceEntry>();
                        string docdesc = string.Format(ATPTEFMMessages.CAPrepaymentBillDesc, ca?.CashAdvanceNbr, ca?.Descr ?? "");
                        APInvoice inv = invEntry.Document.Insert(new APInvoice
                        {
                            DocType = APDocType.Prepayment,
                            VendorID = ca?.RequestedByID,
                            InvoiceNbr = graph.GetInvoiceNbr(),
                            DocDesc = docdesc,
                            OpenDoc = true,
                            Released = false,
                            Hold = true,
                            PaymentsByLinesAllowed = false,
                            CuryID = ca?.CuryID,
                            CuryInfoID = ca?.CuryInfoID,
                            OrigRefNbr = ca?.CashAdvanceNbr
                        });
                        ATPTEFMAPRegisterExt invExt = inv.GetExtension<ATPTEFMAPRegisterExt>();
                        invExt.UsrATPTEFMSourceType = Attributes.ATPTEFMSourceTypeAttribute.CashAdvance;
                        invExt.UsrATPTEFMSourceRef = ca?.CashAdvanceNbr;
                        invExt.UsrATPTEFMIsAmountRestrictedBill = true;
                        invExt.UsrATPTEFMIsCaPrepaymentBill = true;
                        invExt.UsrATPTEFMSourceReqClass = ca?.ReqClassID;

                        Decimal? totalAmount = 0;

                        foreach (ATPTEFMCARequestDetail request in graph.CashAdvanceRequestLines.Select())
                        {
                            if (request is null) continue;
                            APTran tranDoc = invEntry.Transactions.Insert(new APTran
                            {
                                BranchID = ca?.BranchID,
                                InventoryID = request?.InventoryID,
                                ManualPrice = true,
                                CuryUnitCost = request?.CuryUnitCost,
                                Qty = request?.Qty,
                                UnitCost = request?.UnitCost,
                                CuryTranAmt = ((decimal?)request?.Qty * request?.CuryUnitCost).RoundDecimal(2),
                                CuryLineAmt = ((decimal?)request?.Qty * request?.CuryUnitCost).RoundDecimal(2),
                                ProjectID = request?.ProjectID,
                                TaskID = request?.ProjectTaskID,
                                CostCodeID = request?.CostCodeID,
                                AccountID = request?.AccountID,
                                SubID = request?.SubID,
                                TranDesc = request?.Remarks
                            });

                            totalAmount += ((decimal?)request?.Qty * request?.CuryUnitCost).RoundDecimal(2);

                            if (graph.Preferences.Current.CopyAPNotes == true)
                            {
                                PXNoteAttribute.CopyNoteAndFiles(graph.CashAdvanceRequestLines.Cache, request, invEntry.Transactions.Cache, tranDoc);
                            }
                            tranDoc = graph.BeforeAPTranUpdate(tranDoc);
                            invEntry.Transactions.Update(tranDoc);

                            #region Retrigger Tax Category to remove all taxes. See on ATPTAPInvoiceDefaultATCAttribute of Philtax
                            var currentTaxCategory = tranDoc.TaxCategoryID;
                            invEntry.Transactions.Cache.SetValueExt<APTran.taxCategoryID>(tranDoc, null);
                            invEntry.Transactions.Cache.Update(tranDoc);
                            invEntry.Transactions.Cache.SetValueExt<APTran.taxCategoryID>(tranDoc, currentTaxCategory);
                            invEntry.Transactions.Cache.Update(tranDoc);
                            #endregion
                        }

                        //Require Approval in Cash Advance AP Bill
                        inv.Hold = (graph.Preferences?.Current?.IsRequireApprovalCashAdvanceBill ?? false) ? true : false;
                        inv.CuryDocBal = totalAmount;
                        inv.CuryOrigDocAmt = totalAmount;
                        inv.CuryLineTotal = totalAmount;

                        if ((graph.Preferences?.Current?.IsRequireApprovalCashAdvanceBill ?? false) == false)
                        {
                            APInvoiceEntry.APInvoiceEntryDocumentExtension invoiceBaseGraphExtension = invEntry.GetExtension<APInvoiceEntry.APInvoiceEntryDocumentExtension>();
                            invoiceBaseGraphExtension.SuppressApproval();
                        }

                        inv = graph.DoAdditionalCreateApBillProcess(inv);

                        invEntry.Document.Update(inv);
                        if (graph.Preferences.Current.AutoReleaseAP == true)
                        {
                            invEntry.release.Press();
                        }
                        else
                        {
                            invEntry.Save.Press();
                        }

                        PXLongOperation.WaitCompletion(invEntry.UID);
                        ca.InvoiceRefNbr = inv?.RefNbr;
                        ca.BillType = inv?.DocType;
                        ca.BillRefNbr = inv?.RefNbr;
                        createdPPM = inv;
                    };

                    ts.Complete();
                }
            });

            PXLongOperation.StartOperation(this, () =>
            {
                PXLongOperation.WaitCompletion(key1);
                graph.CashAdvances.Cache.MarkUpdated(ca);//skip events
                graph.CashAdvances.Cache.PersistUpdated(ca); //replace Save.press as the update doesn't need to trigger any graph events again

                //Open AP Bill
                APInvoiceEntry graphAP = CreateInstance<APInvoiceEntry>();
                graphAP.Document.Current = createdPPM;

                //Autoreleasing
                if (createdPPM != null)
                {
                    if (graph.Preferences.Current.CopyAPNotes == true)
                    {
                        PXNoteAttribute.CopyNoteAndFiles(graph.CashAdvances.Cache,
                            ca, graphAP.Document.Cache, graphAP.Document.Current);
                    }
                }

                throw new PXRedirectRequiredException(graphAP, true, "Bill") { Mode = PXBaseRedirectException.WindowMode.NewWindow };
            });
            
            return adapter.Get();
        }

        /// <remarks>
        /// 2025-07-08 : 012116 - create method to populate necessary fields in APTran before inserting it into APInvoice. {RRS}
        /// </remarks>
        public virtual APTran BeforeAPTranUpdate(APTran tranDoc) => tranDoc;

        public PXAction<ATPTEFMCashAdvance> PendingForLiquidationImport;
        [PXButton(Category = "Actions", IsLockedOnToolbar = true, Connotation = PX.Data.WorkflowAPI.ActionConnotation.Success)]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.PendingForLiquidation)]
        public virtual IEnumerable pendingForLiquidationImport(PXAdapter adapter)
        {
            return adapter.Get();
        }

        public PXAction<ATPTEFMCashAdvance> CreateAPBillImport;
        [PXProcessButton()]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.CreateAPBillImport)]
        public IEnumerable createAPBillImport(PXAdapter adapter)
        {
            ATPTEFMHelper.StartLongOperation(this, adapter, delegate ()
            {
                using (PXTransactionScope ts = new PXTransactionScope())
                {

                    ATPTEFMCashAdvance ca = CashAdvances.Current;

                    if (ca != null)
                    {
                        APInvoiceEntry invEntry = PXGraph.CreateInstance<APInvoiceEntry>();
                        APInvoice inv = new APInvoice();



                        inv = (APInvoice)invEntry.Document.Cache.Insert(inv);

                        invEntry.Document.Cache.SetValueExt<APInvoice.docType>(inv, APDocType.Prepayment);
                        invEntry.Document.Cache.SetValueExt<APInvoice.vendorID>(inv, ca.RequestedByID);
                        invEntry.Document.Cache.SetValueExt<APInvoice.invoiceNbr>(inv, ca.CashAdvanceNbr);
                        inv.PaymentsByLinesAllowed = false;

                        Decimal? totalAmount = 0;

                        foreach (PXResult<ATPTEFMCARequestDetail, InventoryItem> detail in CashAdvanceRequestLines.Select())
                        {
                            ATPTEFMCARequestDetail request = detail;

                            InventoryItem inventory = detail;


                            APTran tranDoc = new APTran();

                            tranDoc = (APTran)invEntry.Transactions.Insert(tranDoc);

                            invEntry.Transactions.SetValueExt<APTran.inventoryID>(tranDoc, request.InventoryID);

                            invEntry.Transactions.SetValueExt<APTran.projectID>(tranDoc, request.ProjectID);

                            invEntry.Transactions.SetValueExt<APTran.taskID>(tranDoc, request.ProjectTaskID);

                            tranDoc.Qty = request.Qty;

                            tranDoc.CuryUnitCost = request.UnitCost;

                            tranDoc.UnitCost = request.UnitCost;

                            tranDoc.CuryTranAmt = ((decimal?)request.Qty * request.UnitCost).RoundDecimal(2);

                            tranDoc.CuryLineAmt = tranDoc.CuryTranAmt;

                            tranDoc.CostCodeID = request.CostCodeID;

                            tranDoc.AccountID = inventory.COGSAcctID;

                            tranDoc.SubID = inventory.COGSSubID;

                            totalAmount += ((decimal?)request.Qty * request.UnitCost).RoundDecimal(2);
                        }

                        inv.Hold = false;
                        inv.Status = APDocStatus.Balanced;
                        inv.CuryDocBal = totalAmount;
                        inv.CuryOrigDocAmt = totalAmount;
                        inv.CuryLineTotal = totalAmount;

                        inv = invEntry.Document.Update(inv);

                        invEntry.Save.Press();

                        ca.InvoiceRefNbr = inv.RefNbr;
                        CashAdvances.Update(ca);
                        this.Save.Press();
                    }
                    ;
                    APInvoiceEntry graphAP = PXGraph.CreateInstance<APInvoiceEntry>();

                    graphAP.Document.Current = graphAP.Document.Search<APInvoice.refNbr>(ca.InvoiceRefNbr, APDocType.Prepayment);

                    graphAP.Save.Press();

                    ts.Complete();
                }
            });
            return adapter.Get();
        }

        /// <remarks>
        /// 010248 - (CFM24R1) Cash Advance>Receipts tab: Project, Task Code, Cost Code should copy the Project related values from the Request Details tab.
        /// </remarks>
        public PXAction<ATPTEFMCashAdvance> LoadRequest;
        [PXButton()]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.LoadRequest)]
        public virtual IEnumerable loadRequest(PXAdapter adapter)
        {
            if (ReceiptsForSubmit.AskExt(true) == WebDialogResult.OK)
            {
                return SubmitReceipt(adapter);
            }
            ReceiptsForSubmit.Cache.Clear();
            return adapter.Get();
        }

        public PXAction<ATPTEFMCashAdvance> submitReceipt;
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.SubmitReceipt)]
        [PXButton()]
        protected virtual IEnumerable SubmitReceipt(PXAdapter adapter)
        {
            ATPTEFMCashAdvance cashAdvance = CashAdvances.Current;

            foreach (ATPTEFMCARequestDetail item in ReceiptsForSubmit.Select())
            {
                if (item.Selected == true)  //&& item.RunningQty > 0) -- RED-1438
                {
                    ATPTEFMCAReceiptDetail receipt = new ATPTEFMCAReceiptDetail();

                    receipt.CashAdvanceRequestDetailID = item.CashAdvanceRequestDetailID;
                    receipt.InventoryID = item.InventoryID;
                    receipt.LineDescription = item.Remarks;
                    receipt.AccountID = item.AccountID;
                    receipt.SubID = item.SubID;
                    receipt.ProjectID = item.ProjectID;
                    receipt.ProjectTaskID = item.ProjectTaskID;
                    receipt.CostCodeID = item.CostCodeID;

                    #region Default Tax Zone
                    ATPTEFMCashAdvance ca = CashAdvances.Current;
                    Location requestBy = PXSelect<
                        Location,
                        Where<Location.bAccountID, Equal<Required<Location.bAccountID>>>>
                        .Select(this, ca.RequestedByID);
                    receipt.TaxZoneID = requestBy.VTaxZoneID;
                    #endregion

                    CashAdvanceReceiptLines.Cache.Insert(receipt);
                }
            }

            ReceiptsForSubmit.Cache.Clear();
            return adapter.Get();
        }

        public PXAction<ATPTEFMCashAdvance> cancelSubmitReceipt;
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.CancelSubmitReceipt)]
        [PXButton()]
        protected virtual IEnumerable CancelSubmitReceipt(PXAdapter adapter)
        {
            ReceiptsForSubmit.Cache.Clear();
            return adapter.Get();
        }

        public PXAction<ATPTEFMCashAdvance> SubmitReceipts;
        [PXProcessButton(Category = "Actions", Connotation = PX.Data.WorkflowAPI.ActionConnotation.Warning)]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.SubmitReceipts)]
        public virtual IEnumerable submitReceipts(PXAdapter adapter)
        {
            this.Save.PressButton();
            ATPTEFMReqClass reqclas = ReqClass.Current;

            ATPTEFMHelper.StartLongOperation(this, adapter, delegate ()
            {
                using (PXTransactionScope ts = new PXTransactionScope())
                {
                    ATPTEFMCashAdvance ca = CashAdvances.Current;

                    // Check invoice if released
                    if (ca.InvoiceRefNbr != null)
                    {
                        APInvoice invoice = PXSelect<
                            APInvoice,
                            Where<APInvoice.refNbr, Equal<Required<APInvoice.refNbr>>>>
                            .Select(this, ca.InvoiceRefNbr);

                        if (!invoice.Released ?? false)
                            throw new Exception(Messages.ATPTEFMMessages.ReleaseBill);
                    }


                    ATPTEFMReqClass reqclass = ReqClass.Current;

                    ExpenseClaimDetailEntry entry = PXGraph.CreateInstance<ExpenseClaimDetailEntry>();

                    foreach (PXResult<ATPTEFMCAReceiptDetail, InventoryItem, ATPTEFMCARequestDetail> d in CashAdvanceReceiptLines.Select())
                    {

                        ATPTEFMCARequestDetail request = (ATPTEFMCARequestDetail)d;
                        ATPTEFMCAReceiptDetail receipt = (ATPTEFMCAReceiptDetail)d;
                        InventoryItem item = (InventoryItem)d;

                        if (string.IsNullOrEmpty(receipt.ExpenseReceiptRefNbr))
                        {
                            EPExpenseClaimDetails claim = new EPExpenseClaimDetails();

                            claim = (EPExpenseClaimDetails)entry.ClaimDetails.Insert(claim);

                            #region EFMChanges

                            ATPTEFMEPExpenseClaimDetailsExt claimExt = claim.GetExtension<ATPTEFMEPExpenseClaimDetailsExt>();

                            if (reqclass.TranType == ATPTEFMTranTypeAttribute.CashAdvance)
                                claimExt.UsrATPTEFMTranType = ATPTEFMExpenseTypeAttribute.Liquidation;
                            if (reqclass.TranType == ATPTEFMTranTypeAttribute.RequestforPayment)
                                claimExt.UsrATPTEFMTranType = ATPTEFMExpenseTypeAttribute.RequestforPayment;

                            claimExt.UsrATPTEFMReqType = reqclass.TranType;

                            claim.EmployeeID = ca.RequestedByID;

                            claim = entry.ClaimDetails.Update(claim);

                            claim.BranchID = ca.BranchID;
                            claimExt.UsrATPTEFMReqClass = ca.ReqClassID;
                            entry.ClaimDetails.Cache.SetValueExt<Extensions.DAC.ATPTEFMEPExpenseClaimDetailsExt.usrATPTEFMReqRef>(claim, ca.CashAdvanceNbr);

                            #endregion

                            claim.ExpenseDate = receipt.Date;

                            entry.ClaimDetails.Cache.SetValueExt<EPExpenseClaimDetails.inventoryID>(claim, receipt.InventoryID);
                            claim.TranDesc = receipt.LineDescription;

                            #region Expense Amount
                            entry.ClaimDetails.Cache.SetValueExt<EPExpenseClaimDetails.qty>(claim, receipt.NetQty);
                            entry.ClaimDetails.Cache.SetValueExt<EPExpenseClaimDetails.curyUnitCost>(claim, receipt.CuryNetUnitCost);
                            #endregion

                            claim.UOM = item.BaseUnit;
                            claim.ExpenseRefNbr = receipt.RefNbr;
                            claim.ContractID = receipt.ProjectID;
                            claim.TaskID = receipt.ProjectTaskID;
                            claim.CostCodeID = receipt.CostCodeID;


                            claim.ExpenseAccountID = receipt.AccountID;
                            claim.ExpenseSubID = receipt.SubID;
                            claim.CuryID = ca.CuryID;
                            claim.CuryInfoID = ca.CuryInfoID;

                            Location extAddress = null;

                            if (receipt.VendID != null)
                            {
                                PXResultset<Vendor, BAccount, Address, Location> vendorInfo = PXSelectJoin<
                                    Vendor,
                                    InnerJoin<BAccount,
                                        On<BAccount.bAccountID, Equal<Vendor.bAccountID>>,
                                    LeftJoin<Address,
                                        On<Address.addressID, Equal<BAccount.defAddressID>>,
                                    LeftJoin<Location,
                                        On<Location.bAccountID, Equal<BAccount.defLocationID>>>>>,
                                    Where<Vendor.bAccountID, Equal<Required<Vendor.bAccountID>>>>
                                    .Select<PXResultset<Vendor, BAccount, Address, Location>>(this, receipt.VendID);

                                Vendor vendor = (Vendor)vendorInfo;
                                Address address = (Address)vendorInfo;
                                extAddress = (Location)vendorInfo;

                                entry.ClaimDetails.Cache.SetValueExt<Extensions.DAC.ATPTEFMEPExpenseClaimDetailsExt.usrATPTEFMDetailVendorID>(claim, receipt.VendID);
                                entry.ClaimDetails.Cache.SetValueExt<Extensions.DAC.ATPTEFMEPExpenseClaimDetailsExt.usrATPTVendID>(claim, vendor.AcctCD);
                                entry.ClaimDetails.Cache.SetValueExt<Extensions.DAC.ATPTEFMEPExpenseClaimDetailsExt.usrATPTVendName>(claim, receipt.VendorName);
                                entry.ClaimDetails.Cache.SetValueExt<Extensions.DAC.ATPTEFMEPExpenseClaimDetailsExt.usrATPTAddress>(claim, receipt.VendorAddress);
                                entry.ClaimDetails.Cache.SetValueExt<Extensions.DAC.ATPTEFMEPExpenseClaimDetailsExt.usrATPTVendTIN>(claim, receipt.VendorTin);
                            }
                            else
                            {
                                #region EFMChanges

                                entry.ClaimDetails.Cache.SetValueExt<Extensions.DAC.ATPTEFMEPExpenseClaimDetailsExt.usrATPTVendName>(claim, receipt.VendorName);
                                entry.ClaimDetails.Cache.SetValueExt<Extensions.DAC.ATPTEFMEPExpenseClaimDetailsExt.usrATPTAddress>(claim, receipt.VendorAddress);
                                entry.ClaimDetails.Cache.SetValueExt<Extensions.DAC.ATPTEFMEPExpenseClaimDetailsExt.usrATPTVendTIN>(claim, receipt.VendorTin);

                                #endregion
                            }

                            claim = entry.ClaimDetails.Update(claim);

                            #region EFMChanges

                            claim.PaidWith = EPExpenseClaimDetails.paidWith.PersonalAccount;
                            claim = entry.ClaimDetails.Update(claim);
                            PXNoteAttribute.CopyNoteAndFiles(CashAdvanceReceiptLines.Cache, receipt, entry.ClaimDetails.Cache, claim);

                            #endregion

                            entry.ClaimDetails.Current.TaxCalcMode = item.TaxCalcMode;
                            claim.TaxZoneID = receipt.TaxZoneID;
                            claim.TaxCategoryID = receipt.TaxCategoryID;
                            claim = AddAtcCode(claim, receipt);
                            entry.CurrentClaimDetails.Update(claim);
                            entry.Save.Press();

                            receipt.ExpenseReceiptRefNbr = claim.ClaimDetailCD;
                            CashAdvanceReceiptLines.Update(receipt);
                            ca.ReceiptsWithNoERRefNbrs--;
                            this.Save.Press();
                        }
                    }
                    ts.Complete();
                }
            }
            );
            return adapter.Get();
        }

        public PXAction<ATPTEFMCashAdvance> openTransaction;
        [PXButton(Tooltip = Messages.ATPTEFMMessages.OpenTransaction, CommitChanges = true)]
        [PXUIField(DisplayName = "Open Transaction", Visible = false)]
        public virtual void OpenTransaction()
        {
            ATPTEFMCAReceiptDetail row = CashAdvanceReceiptLines?.Current;


            if (row?.ExpenseReceiptRefNbr != null)
            {
                ExpenseClaimDetailEntry graph = PXGraph.CreateInstance<ExpenseClaimDetailEntry>();
                graph.ClaimDetails.Current = graph.ClaimDetails.Search<EPExpenseClaimDetails.claimDetailCD>(row.ExpenseReceiptRefNbr);

                if (graph.ClaimDetails.Current != null)
                {
                    throw new PXRedirectRequiredException(graph, true, "Expense Receipt") { Mode = PXBaseRedirectException.WindowMode.NewWindow };
                }
            }
        }

        public PXAction<ATPTEFMCashAdvance> openExpClaim;
        [PXButton(Tooltip = "Open Expense Claim", CommitChanges = true)]
        [PXUIField(DisplayName = "Open Transaction", Visible = false)]
        public virtual void OpenExpClaim()
        {
            ATPTEFMCAReceiptDetail row = CashAdvanceReceiptLines?.Current;

            if (row?.LiquidationRef != null)
            {
                ExpenseClaimEntry graph = PXGraph.CreateInstance<ExpenseClaimEntry>();
                graph.Clear();

                EPExpenseClaimDetails claimDetails = PXSelect<
                    EPExpenseClaimDetails,
                    Where<EPExpenseClaimDetails.claimDetailCD, Equal<Required<EPExpenseClaimDetails.claimDetailCD>>>>
                    .Select(this, row.ExpenseReceiptRefNbr);


                graph.ExpenseClaim.Current = graph.ExpenseClaim.Search<EPExpenseClaim.refNbr>(claimDetails.RefNbr);

                if (graph.ExpenseClaim.Current != null)
                {
                    throw new PXRedirectRequiredException(graph, true, "Expense Claim") { Mode = PXBaseRedirectException.WindowMode.NewWindow };
                }
            }
        }

        public PXAction<ATPTEFMCashAdvance> PrintCashAdvance;
        [PXButton(Category = "Reports")]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.PrintCashAdvance)]
        /// <remarks>   
        /// 04-08-2026: 015822 - CFM 2025R2: Fund Transaction Form. Related case 015821. {JCL} </br>
        /// </remarks>>
        public IEnumerable printCashAdvance(PXAdapter adapter)
        {
            foreach (ATPTEFMCashAdvance cashadvance in adapter.Get<ATPTEFMCashAdvance>())
            {
                Dictionary<string, string> parameters = new Dictionary<string, string>();
                parameters["RefNbr"] = cashadvance.CashAdvanceNbr;
                parameters["RequestedBy"] = Accessinfo.UserName;

                var report = new PXReportRequiredException(parameters, "ATPT6415", "Cash Advance Request");

                throw new PXRedirectWithReportException(this, report, "Preview");
            }

            return adapter.Get();
        }

        public PXAction<APInvoice> ViewReclassifyBill;
        [PXUIField(
            DisplayName = Messages.ATPTEFMMessages.ViewBill,
            MapEnableRights = PXCacheRights.Select,
            MapViewRights = PXCacheRights.Select,
            Visible = false)]
        [PXLookupButton]
        public virtual IEnumerable viewReclassifyBill(PXAdapter adapter)
        {
            if (this.CurrentCashAdvance.Current != null)
            {
                APInvoice bill = PXSelect<
                    APInvoice,
                    Where<APInvoice.docType, Equal<APDocType.creditAdj>,
                        And<APInvoice.refNbr, Equal<Required<APInvoice.refNbr>>>>>
                    .Select(this, this.CurrentCashAdvance.Current.ReclassifiedInvoiceRefNbr);

                if (bill != null)
                {
                    PXRedirectHelper.TryRedirect(Caches[typeof(APPayment)], bill, Messages.ATPTEFMMessages.ViewBill, PXRedirectHelper.WindowMode.NewWindow);
                }
            }

            return adapter.Get();
        }

        public PXAction<APInvoice> viewBill;
        [PXUIField(
            DisplayName = Messages.ATPTEFMMessages.ViewBill,
            MapEnableRights = PXCacheRights.Select,
            MapViewRights = PXCacheRights.Select,
            Visible = false)]
        [PXLookupButton]
        public virtual IEnumerable ViewBill(PXAdapter adapter)
        {
            if (this.CurrentCashAdvance.Current != null)
            {
                APInvoice bill = PXSelect<
                    APInvoice,
                    Where<APInvoice.docType, Equal<Required<APInvoice.docType>>,
                        And<APInvoice.refNbr, Equal<Required<APInvoice.refNbr>>>>>
                    .Select(this, this.CurrentCashAdvance.Current.BillType, this.CurrentCashAdvance.Current.BillRefNbr);
                if (bill != null)
                {
                    PXRedirectHelper.TryRedirect(Caches[typeof(APInvoice)], bill, Messages.ATPTEFMMessages.ViewBill, PXRedirectHelper.WindowMode.NewWindow);
                }
            }

            return adapter.Get();
        }

        public PXAction<APInvoice> viewPayment;
        [PXUIField(
            DisplayName = Messages.ATPTEFMMessages.ViewBill,
            MapEnableRights = PXCacheRights.Select,
            MapViewRights = PXCacheRights.Select,
            Visible = false)]
        [PXLookupButton]
        public virtual IEnumerable ViewPayment(PXAdapter adapter)
        {
            if (this.CurrentCashAdvance.Current != null)
            {
                APPayment pmt = PXSelect<
                    APPayment,
                    Where<APPayment.docType, Equal<Required<APPayment.docType>>,
                        And<APPayment.refNbr, Equal<Required<APPayment.refNbr>>>>>
                    .Select(this, this.CurrentCashAdvance.Current.PmtType, this.CurrentCashAdvance.Current.PmtRefNbr);

                if (pmt != null)
                {
                    PXRedirectHelper.TryRedirect(Caches[typeof(APPayment)], pmt, Messages.ATPTEFMMessages.ViewCheck, PXRedirectHelper.WindowMode.NewWindow);
                }
            }

            return adapter.Get();
        }

        public PXAction<APInvoice> viewRefund;
        [PXUIField(
            DisplayName = Messages.ATPTEFMMessages.ViewBill,
            MapEnableRights = PXCacheRights.Select,
            MapViewRights = PXCacheRights.Select,
            Visible = false)]
        [PXLookupButton]
        public virtual IEnumerable ViewRefund(PXAdapter adapter)
        {
            if (this.CurrentCashAdvance.Current != null)
            {
                APPayment refund = PXSelect<
                    APPayment,
                    Where<APPayment.docType, Equal<Required<APPayment.docType>>,
                        And<APPayment.refNbr, Equal<Required<APPayment.refNbr>>>>>
                    .Select(this, this.CurrentCashAdvance.Current.VendorRefundType, this.CurrentCashAdvance.Current.VendorRefundRefNbr);

                if (refund != null)
                {
                    PXRedirectHelper.TryRedirect(Caches[typeof(APPayment)], refund, Messages.ATPTEFMMessages.ViewCheck, PXRedirectHelper.WindowMode.NewWindow);
                }
            }

            return adapter.Get();
        }

        public PXAction<APInvoice> viewPrepayment;
        [PXUIField(
            DisplayName = Messages.ATPTEFMMessages.ViewBill,
            MapEnableRights = PXCacheRights.Select,
            MapViewRights = PXCacheRights.Select,
            Visible = false)]
        [PXLookupButton]
        public virtual IEnumerable ViewPrepayment(PXAdapter adapter)
        {
            if (this.CurrentCashAdvance.Current != null)
            {
                APPayment ppm = PXSelect<
                    APPayment,
                    Where<APPayment.docType, Equal<Required<APPayment.docType>>,
                        And<APPayment.refNbr, Equal<Required<APPayment.refNbr>>>>>
                    .Select(this, this.CurrentCashAdvance.Current.BillType, this.CurrentCashAdvance.Current.BillRefNbr);

                if (ppm != null)
                {
                    PXRedirectHelper.TryRedirect(Caches[typeof(APPayment)], ppm, Messages.ATPTEFMMessages.ViewPrepayment, PXRedirectHelper.WindowMode.NewWindow);
                }
            }

            return adapter.Get();
        }
        /// <remarks>
        /// 010357 - CFM 2024R1 - Voided Refund in Cash Advance
        /// </remarks>
        public PXAction<ATPTEFMCashAdvance> viewVoidCheck;
        [PXButton(Tooltip = Messages.ATPTEFMMessages.OpenTransaction, CommitChanges = true)]
        [PXUIField(DisplayName = "Open Transaction", Visible = false)]
        public virtual void ViewVoidCheck()
        {
            ATPTEFMVoidedDocument row = VoidedDocuments?.Current;

            if (row?.RefNbr != null)
            {
                if (row?.DocType == APDocType.Check)
                {
                    APPaymentEntry graph = PXGraph.CreateInstance<APPaymentEntry>();
                    graph.Document.Current = graph.Document.Search<APPayment.refNbr>(row.RefNbr, APDocType.Check);
                    if (graph.Document.Current != null)
                        throw new PXRedirectRequiredException(graph, true, "Voided Document") { Mode = PXBaseRedirectException.WindowMode.NewWindow };
                }
                else if (row?.DocType == APDocType.Prepayment)
                {
                    APInvoiceEntry graph = PXGraph.CreateInstance<APInvoiceEntry>();
                    graph.Document.Current = graph.Document.Search<APInvoice.refNbr>(row.RefNbr, APDocType.Prepayment);
                    if (graph.Document.Current != null)
                        throw new PXRedirectRequiredException(graph, true, "Voided Document") { Mode = PXBaseRedirectException.WindowMode.NewWindow };
                }
                else
                {
                    APPaymentEntry graph = PXGraph.CreateInstance<APPaymentEntry>();
                    graph.Document.Current = APPayment.PK.Find(this, APDocType.Refund, row?.RefNbr);
                    if (graph.Document.Current != null && graph.Document.Current.Status == APDocStatus.Voided)
                        throw new PXRedirectRequiredException(graph, true, "Voided Refund") { Mode = PXBaseRedirectException.WindowMode.NewWindow };
                }
            }
        }

        public PXAction<ATPTEFMCashAdvance> Liquidate;
        [PXButton(Category = "Actions", Connotation = PX.Data.WorkflowAPI.ActionConnotation.Success)]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.Liquidate)]
        public virtual IEnumerable liquidate(PXAdapter adapter)
        {
            if (SubmitReceipts.GetEnabled())
            {
                SubmitReceipts.PressButton();
                PXLongOperation.WaitCompletion(this.UID);
            }
            ATPTEFMHelper.StartLongOperation(this, adapter, delegate ()
            {
                using (PXTransactionScope ts = new PXTransactionScope())
                {
                    if (CashAdvanceReceiptLines.Current != null)
                    {
                        ATPTEFMCashAdvance cashAdvance = CashAdvances.Current;

                        ExpenseClaimEntry graph = PXGraph.CreateInstance<ExpenseClaimEntry>();
                        graph.Clear();

                        Save.Press();

                        #region Vendor
                        Location venLoc = PXSelect<
                                                                        Location,
                                                                        Where<Location.bAccountID, Equal<Required<Location.bAccountID>>>>
                                                                        .Select(this, cashAdvance.RequestedByID);
                        #endregion

                        EPExpenseClaim claim = graph.ExpenseClaim.Insert();
                        ATPTEFMEPExpenseClaimExt claimExt = claim.GetExtension<ATPTEFMEPExpenseClaimExt>();
                        claim.DocDesc = String.Format("Submitted Receipt(s) for CA {0}  ", CashAdvances.Current.CashAdvanceNbr);
                        claimExt.UsrATPTEFMReqClass = cashAdvance.ReqClassID;
                        claim.EmployeeID = cashAdvance.RequestedByID;
                        claim.LocationID = venLoc.LocationID;
                        claim.CuryID = cashAdvance.CuryID;
                        claim.CuryInfoID = cashAdvance.CuryInfoID;

                        #region Copy Notes and Files to Expense Claim
                        if (Preferences.Current.CopyECNotes ?? false)
                            PXNoteAttribute.CopyNoteAndFiles(CashAdvances.Cache, CashAdvances.Current, graph.ExpenseClaim.Cache, claim);
                        #endregion

                        graph.ExpenseClaim.Update(claim);
                        List<EPExpenseClaimDetails> claimDetails = new List<EPExpenseClaimDetails>();

                        foreach (ATPTEFMCAReceiptDetail detail in CashAdvanceReceiptLines.Select())
                        {
                            if (detail.LiquidationRef == null)
                            {
                                EPExpenseClaimDetails receipts = PXSelect<
                                    EPExpenseClaimDetails,
                                    Where<EPExpenseClaimDetails.claimDetailCD,
                                       Equal<Required<EPExpenseClaimDetails.claimDetailCD>>>>
                                    .Select(this, detail.ExpenseReceiptRefNbr);
                                claimDetails.Add(receipts);
                            }
                        }

                        foreach (EPExpenseClaimDetails item in claimDetails)
                        {
                            var key = new EPExpenseClaimDetails { ClaimDetailCD = item.ClaimDetailCD, ClaimDetailID = item.ClaimDetailID };
                            EPExpenseClaimDetails origDetails = graph.ExpenseClaimDetails.Locate(key) ?? item;
                            EPExpenseClaimDetails details = (EPExpenseClaimDetails)graph.ExpenseClaimDetails.Cache.CreateCopy(origDetails);
                            graph.FindImplementation<ExpenseClaimEntry.ExpenseClaimEntryReceiptExt>().SubmitReceiptExt(graph.ExpenseClaim.Cache, graph.ExpenseClaimDetails.Cache, graph.ExpenseClaim.Current, details);
                        }

                        graph.Save.Press();

                        foreach (ATPTEFMCAReceiptDetail detail in CashAdvanceReceiptLines.Select())
                        {
                            if (detail.LiquidationRef == null)
                            {
                                detail.LiquidationRef = graph.ExpenseClaim.Current.GetExtension<ATPTEFMEPExpenseClaimExt>().UsrATPTEFMLiqNbr;
                                CashAdvanceReceiptLines.Update(detail);
                            }
                        }

                        cashAdvance.ReceiptsWithNoLiquidationRefNbr--;
                        Save.Press();

                        ts.Complete();
                        PXRedirectHelper.TryRedirect(graph, graph.ExpenseClaim.Current, PXRedirectHelper.WindowMode.InlineWindow);
                    }
                }
            });
            return adapter.Get();
        }

        public PXAction<ATPTEFMCashAdvance> CACancel;
        [PXButton(Category = "Actions", Connotation = PX.Data.WorkflowAPI.ActionConnotation.Warning)]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.CACancel)]
        public virtual IEnumerable caCancel(PXAdapter adapter)
        {
            ATPTEFMHelper.StartLongOperation(this, adapter, delegate ()
            {
                using (PXTransactionScope ts = new PXTransactionScope())
                {
                    ATPTEFMCashAdvance cashAdvance = CashAdvances.Current;
                    if (cashAdvance != null)
                    {
                        cashAdvance.Status = ATPTEFMCashAdvanceStatusAttribute.CancelledValue;
                        cashAdvance.IsCancelled = true;
                        CashAdvances.Update(cashAdvance);
                        this.Save.Press();

                        foreach (EPApproval approval in Approval.Select())
                        {
                            Approval.Delete(approval);
                        }
                    }
                    ts.Complete();
                }
            });
            return adapter.Get();
        }
        #endregion

        #region Events
        protected virtual void _(Events.FieldUpdated<ATPTEFMCARequestDetail, ATPTEFMCARequestDetail.curyAmount> e)
        {
            ATPTEFMCARequestDetail row = e.Row;
            if (row == null) return;

            ATPTEFMCashAdvance ca = CashAdvances.Current;

            BudgetValidationRaiseErrorForRequestAmountGreaterThanRemainingBudget(row, ca?.BudgetEnabled ?? false);
            PBudgetValidationRaiseErrorForRequestAmountGreaterThanRemainingBudget(row, ca?.ProjectBudgetEnabled ?? false);
        }
        /// <remarks>
        /// 2024-10-28 : This code executes only in the cash advance screen, and the unbound field for the PPM balance is not working properly if liquidation bill is released. CASEID: 008442 {JLG} <br/>             
        /// </remarks>
        protected virtual void _(Events.FieldUpdated<ATPTEFMCashAdvance, ATPTEFMCashAdvance.ppmBalance> e)
        {
            ATPTEFMCashAdvance row = e.Row;
            if (row == null) return;

            if (this.Accessinfo.ScreenID == ATPTEFMMessages.CashAdvanceScreenID && (row.IsImported ?? false))
            {
                row.CuryChangeAmount = row.PpmBalance;
            }
        }
        /// <remarks>
        /// 2025-08-28 : Once the vendor ID is cleared in the Receipts tab, the vendor name, TIN  and address should also be cleared. CASEID: 012141 {JLG} <br/>             
        /// </remarks>
        protected virtual void _(Events.FieldUpdated<ATPTEFMCAReceiptDetail, ATPTEFMCAReceiptDetail.vendID> e)
        {
            var row = e.Row;

            if (row == null) return;

            bool isNumber = true; //int.TryParse(row.VendID, out int numericValue);

            if (row.VendID != null && isNumber)
            {
                Location taxZoneID = PXSelectJoin<
                    Location,
                    LeftJoin<Vendor,
                        On<Location.bAccountID, Equal<Vendor.bAccountID>,
                        And<Vendor.defLocationID, Equal<Location.locationID>>>>,
                    Where<Vendor.bAccountID, Equal<Required<Vendor.bAccountID>>>>
                    .Select(this, row.VendID);

                row.TaxZoneID = taxZoneID?.VTaxZoneID;

                PXResultset<Vendor, BAccount, Address, Location> vendorInfo = PXSelectJoin<
                    Vendor,
                    InnerJoin<BAccount,
                        On<BAccount.bAccountID, Equal<Vendor.bAccountID>>,
                    LeftJoin<Address,
                        On<Address.addressID, Equal<BAccount.defAddressID>>,
                    LeftJoin<Location,
                        On<Location.bAccountID, Equal<BAccount.defLocationID>>>>>,
                    Where<Vendor.bAccountID, Equal<Required<Vendor.bAccountID>>>>
                    .Select<PXResultset<Vendor, BAccount, Address, Location>>(this, row.VendID);

                Vendor vendor = (Vendor)vendorInfo;
                Address address = (Address)vendorInfo;

                row.VendorName = vendor?.AcctName;
                row.VendorAddress = address?.AddressLine1;
                row.VendorTin = taxZoneID?.TaxRegistrationID;
            }
            else
            {
                if (!string.IsNullOrEmpty(e.OldValue.ToString()) && string.IsNullOrEmpty(row.VendID.ToString()))
                {
                    row.VendorName = string.Empty;
                    row.VendorAddress = string.Empty;
                    row.VendorTin = string.Empty;
                }
            }
        }
        protected virtual void _(Events.RowSelecting<ATPTEFMCashAdvance> e)
        {
            ATPTEFMCashAdvance cashAdvance = e.Row;
            if (cashAdvance == null) return;
            using (new PXConnectionScope())
            {
                if (cashAdvance.Status == ATPTEFMCashAdvanceStatusAttribute.PendingLiquidationValue)
                {
                    cashAdvance.ReceiptsWithNoERRefNbrs = PXSelect<
                    ATPTEFMCAReceiptDetail,
                    Where<ATPTEFMCAReceiptDetail.cashAdvanceNbr, Equal<Required<ATPTEFMCAReceiptDetail.cashAdvanceNbr>>,
                        And<Where<ATPTEFMCAReceiptDetail.expenseReceiptRefNbr, IsNull,
                            Or<ATPTEFMCAReceiptDetail.expenseReceiptRefNbr, Equal<Empty>>>>>>
                    .Select(this, cashAdvance.CashAdvanceNbr)
                    .Count();

                    cashAdvance.ReceiptsWithNoLiquidationRefNbr = PXSelect<
                    ATPTEFMCAReceiptDetail,
                    Where<ATPTEFMCAReceiptDetail.cashAdvanceNbr, Equal<Required<ATPTEFMCAReceiptDetail.cashAdvanceNbr>>,
                        And<Where<ATPTEFMCAReceiptDetail.liquidationRef, IsNull,
                            Or<ATPTEFMCAReceiptDetail.liquidationRef, Equal<Empty>>>>>>
                    .Select(this, cashAdvance.CashAdvanceNbr)
                    .Count();
                }

                EnableDisableReturnExcessCa(cashAdvance);
            }
        }
        protected string reqClassID;
        /// <remarks>
        /// 2024-12-10 : "Cannot create new CA transaction or open existing CA transactions - CASE: 009177 {JLG}
        /// 2025-04-15 : Create AP Bill and CA Cancel Adjustment, enable buttons if PPM bill is rejected - 011135 - RFS
        /// 2025-07-15 : [RFC] Add button under the Receipts tab in CA is disabled for CAs dated below 7/14/2025. 012475 {RRS}
        /// 2025-08-06 : CA Optimization {JLTG}
        /// 2025-08-28 : Plus button in Receipts tab is still visible even if Allow Manual Receipt is UNTICKED in CA Preference CASE: 013245 {JLTG}
        /// 2025-07-29 : CA BegBal: Disable Remove Hold Button, Disable Link to Bills and Link to Checks fields CASE: 012707 {JLTG}
        /// 2025-08-01 : (CFM2023R2) Add Load Records from File Button under Receipts tab of Cash Advance CASE:012776 {JLTG}
        /// </remarks>
        protected virtual void ATPTEFMCashAdvance_RowSelected(PXCache sender, PXRowSelectedEventArgs e)
        {
            ATPTEFMCashAdvance cashAdvance = (ATPTEFMCashAdvance)e.Row;
            if (cashAdvance == null) return;

            #region Import/Migration

            bool IsMigration = (Preferences?.Current?.IsCashAdvanceMigration ?? false);
            PXUIFieldAttribute.SetEnabled<ATPTEFMCashAdvance.ppmRefNbr>(sender, null, IsMigration);
            PXUIFieldAttribute.SetEnabled<ATPTEFMCashAdvance.ppmType>(sender, null, IsMigration);
            PXUIFieldAttribute.SetEnabled<ATPTEFMCashAdvance.status>(sender, null, IsMigration);
            PXUIFieldAttribute.SetEnabled<ATPTEFMCashAdvance.invoiceRefNbr>(sender, null, IsMigration);
            PXUIFieldAttribute.SetVisible<ATPTEFMCashAdvance.isImported>(sender, null, IsMigration || (cashAdvance.IsImported ?? false));
            PXUIFieldAttribute.SetVisible<ATPTEFMCashAdvance.invoiceRefNbr>(sender, null, IsMigration);
            if (cashAdvance?.IsImported ?? false)
            {
                CreateAPBill?.SetEnabled(false);
            }

            #endregion

            #region Conditional Variables

            reqClassID = cashAdvance?.ReqClassID;

            #endregion

            #region Enable or Disable Fields

            PXUIFieldAttribute.SetEnabled<ATPTEFMCashAdvance.hold>(sender, null, (cashAdvance?.Status != ATPTEFMCashAdvanceStatusAttribute.ClosedValue && (string.IsNullOrEmpty(cashAdvance?.BillRefNbr))));
            PXUIFieldAttribute.SetEnabled<ATPTEFMCashAdvance.taxZoneID>(sender, cashAdvance, cashAdvance.Status != ATPTEFMCashAdvanceStatusAttribute.PendingLiquidationValue);

            PXUIFieldAttribute.SetEnabled<ATPTEFMCARequestDetail.accountID>(CashAdvanceRequestLines?.Cache, null, Preferences?.Current?.CashAdvanceAccountEnable ?? false);
            PXUIFieldAttribute.SetEnabled<ATPTEFMCARequestDetail.subID>(CashAdvanceRequestLines?.Cache, null, Preferences?.Current?.CashAdvanceSubAccountEnable ?? true);

            //  Cash Advance Approve
            if (cashAdvance?.Approved == true && cashAdvance?.Status == ATPTEFMCashAdvanceStatusAttribute.OpenValue || cashAdvance?.Status == ATPTEFMCashAdvanceStatusAttribute.ClosedValue)
            {
                PXUIFieldAttribute.SetEnabled<ATPTEFMCashAdvance.branchID>(sender, null, false);
                PXUIFieldAttribute.SetEnabled<ATPTEFMCashAdvance.liqDate>(sender, null, false);
            }
            else
            {
                PXUIFieldAttribute.SetEnabled<ATPTEFMCashAdvance.branchID>(sender, null, true);
                PXUIFieldAttribute.SetEnabled<ATPTEFMCashAdvance.liqDate>(sender, null, true);
            }

            PXUIFieldAttribute.SetEnabled<ATPTEFMCashAdvance.curyID>(sender, cashAdvance, RequesterEmployee?.Current?.AllowOverrideCury ?? false);
            #endregion

            #region Show or Hide Fields

            if (Preferences?.Current?.CashAdvanceRequestApproval != true && cashAdvance?.Approved == true)
                PXUIFieldAttribute.SetVisible<ATPTEFMCashAdvance.approved>(sender, cashAdvance, false);
            else if (Preferences?.Current?.CashAdvanceRequestApproval != true && cashAdvance?.Approved == false)
                PXUIFieldAttribute.SetVisible<ATPTEFMCashAdvance.approved>(sender, cashAdvance, false);
            else
            {
                PXUIFieldAttribute.SetVisible<ATPTEFMCashAdvance.approved>(sender, cashAdvance, true);
            }

            PXUIFieldAttribute.SetVisible<ATPTEFMCashAdvance.isOverbudget>(sender, cashAdvance, Classes.ATPTEFMBudgetLibrary.BudgetVisible(FeatureSetup?.Current, "C"));
            PXUIFieldAttribute.SetVisible<ATPTEFMCashAdvance.hasInitialBudget>(sender, cashAdvance, Classes.ATPTEFMBudgetLibrary.BudgetVisible(FeatureSetup?.Current, "C"));
            #endregion

            #region Project Column Config Visibility For Project Fields
            if (!PXAccess.FeatureInstalled<FeaturesSet.projectAccounting>())
            {
                PXUIFieldAttribute.SetVisibility<ATPTEFMCARequestDetail.projectID>(CashAdvanceRequestLines.Cache, CashAdvanceRequestLines.Current, PXUIVisibility.Invisible);
                PXUIFieldAttribute.SetVisibility<ATPTEFMCARequestDetail.projectTaskID>(CashAdvanceRequestLines.Cache, CashAdvanceRequestLines.Current, PXUIVisibility.Invisible);

                PXUIFieldAttribute.SetVisibility<ATPTEFMCAReceiptDetail.projectID>(CashAdvanceReceiptLines.Cache, CashAdvanceReceiptLines.Current, PXUIVisibility.Invisible);
                PXUIFieldAttribute.SetVisibility<ATPTEFMCAReceiptDetail.projectTaskID>(CashAdvanceReceiptLines.Cache, CashAdvanceReceiptLines.Current, PXUIVisibility.Invisible);
            }
            #endregion

            #region Set Required Fields 

            PXUIFieldAttribute.SetRequired<ATPTEFMCAReceiptDetail.vendorName>(CashAdvanceReceiptLines?.Cache, Preferences?.Current?.RequireVendorDetails ?? false);
            PXUIFieldAttribute.SetRequired<ATPTEFMCAReceiptDetail.vendorAddress>(CashAdvanceReceiptLines?.Cache, Preferences?.Current?.RequireVendorDetails ?? false);
            PXUIFieldAttribute.SetRequired<ATPTEFMCAReceiptDetail.vendorTin>(CashAdvanceReceiptLines?.Cache, Preferences?.Current?.RequireVendorDetails ?? false);
            PXUIFieldAttribute.SetRequired<ATPTEFMCAReceiptDetail.refNbr>(CashAdvanceReceiptLines?.Cache, Preferences?.Current?.RequireExtRef ?? false);

            #endregion

            #region Views Allow Insert/Update/Delete/Select

            CashAdvanceReceiptLines.Cache.SetAllEditPermissions(cashAdvance?.Status != ATPTEFMCashAdvanceStatusAttribute.ClosedValue && cashAdvance?.Status != ATPTEFMCashAdvanceStatusAttribute.CancelledValue && (cashAdvance.VendorRefundRefNbr.IsNullOrEmpty() || cashAdvance.VendorRefundStatus == APDocStatus.Voided));

            CashAdvanceRequestLines.AllowInsert = !cashAdvance?.Approved ?? true;
            CashAdvanceRequestLines.AllowUpdate = !cashAdvance?.Approved ?? true;
            CashAdvanceRequestLines.AllowDelete = !cashAdvance?.Approved ?? true;

            CashAdvances.AllowUpdate = !cashAdvance.Rejected ?? false;

            //Budget.AllowSelect = cashAdvance.BudgetEnabled ?? false;

            bool isBudgetEnabled = cashAdvance.BudgetEnabled ?? false;
            bool isProjectBudgetEnabled = cashAdvance.ProjectBudgetEnabled ?? false;

            if (isBudgetEnabled || isProjectBudgetEnabled)
                CashAdvanceReceiptLines.AllowInsert = false;

            PXImportAttribute.SetEnabled(this, "CashAdvanceReceiptLines", !isProjectBudgetEnabled);
            #endregion

            #region Enable or Disable Buttons


            ReturnExcessCashAdvance?.SetEnabled((Preferences?.Current?.AllowSubmissionExcessCA ?? false) && (((cashAdvance.CAPendingforLiquidationAndEmptyReceipts ?? false) && (!cashAdvance.HasRefund ?? false)) || ((!cashAdvance.AnyUnprocessedEC ?? false) && (!cashAdvance.HasRefund ?? false) && (!cashAdvance.AnyUnprocessedClaim ?? false) && (!cashAdvance.AnyUnprocessedReceipts ?? false) && (cashAdvance.HasBalance ?? false))));
            CreateAPBill?.SetEnabled((cashAdvance?.Approved ?? false == true) && (string.IsNullOrEmpty(cashAdvance?.BillRefNbr) || (cashAdvance?.BillStatus == APDocStatus.Voided) || (cashAdvance?.BillStatus == APDocStatus.Rejected)));
            LoadRequest?.SetEnabled(cashAdvance?.Status == ATPTEFMCashAdvanceStatusAttribute.PendingLiquidationValue && (cashAdvance.VendorRefundRefNbr.IsNullOrEmpty() || cashAdvance.VendorRefundStatus == APDocStatus.Voided));
            CACancel?.SetEnabled((cashAdvance?.Approved ?? false == true) && (string.IsNullOrEmpty(cashAdvance?.BillRefNbr) || cashAdvance?.BillStatus == APDocStatus.Voided || (cashAdvance?.BillStatus == APDocStatus.Rejected)));
            Delete?.SetEnabled(!cashAdvance?.Approved ?? false);
            PendingForLiquidationImport?.SetEnabled(IsMigration);
            SubmitReceipts?.SetEnabled(cashAdvance.ReceiptsWithNoERRefNbrs != 0 && cashAdvance.ReceiptsWithNoERRefNbrs != null && cashAdvance?.Status == ATPTEFMCashAdvanceStatusAttribute.PendingLiquidationValue);
            Liquidate?.SetEnabled((cashAdvance.ReceiptsWithNoLiquidationRefNbr ?? 0) != 0 && cashAdvance?.Status == ATPTEFMCashAdvanceStatusAttribute.PendingLiquidationValue);

            #endregion

            #region Display Buttons on Main Toolbar

            SubmitReceipts?.SetDisplayOnMainToolbar(SubmitReceipts.GetEnabled());
            Liquidate?.SetDisplayOnMainToolbar(Liquidate.GetEnabled());

            #endregion

            #region Raise Field Errors/Warnings
            if (cashAdvance.UnmodifiedLiqDate != cashAdvance.LiqDate)
                sender.RaiseExceptionHandling<ATPTEFMCashAdvance.liqDate>(cashAdvance, cashAdvance.LiqDate,
                    ATPTEFMHelper.GetPropertyException(cashAdvance, ATPTEFMMessages.LiquidationDateHasBeenManuallyAdjusted, PXErrorLevel.Warning));
            #endregion
        }

        /// <remarks>
        /// 2025-02-04 : "Project settings (disables ProjectTask and CostCode when NonProject is selected) CASE: 010041 {JLTG}
        /// </remarks>
        protected virtual void ATPTEFMCARequestDetail_RowSelected(PXCache sender, PXRowSelectedEventArgs e)
        {
            ATPTEFMCARequestDetail row = (ATPTEFMCARequestDetail)e.Row;

            if (row == null) return;

            bool isNonProject = ProjectDefaultAttribute.IsNonProject(row.ProjectID);
            PXUIFieldAttribute.SetEnabled<ATPTEFMCARequestDetail.projectTaskID>(sender, e.Row, !isNonProject);
            PXUIFieldAttribute.SetEnabled<ATPTEFMCARequestDetail.costCodeID>(sender, e.Row, !isNonProject);

        }

        protected virtual void _(Events.RowInserting<ATPTEFMCAReceiptDetail> e)
        {
            ATPTEFMCAReceiptDetail row = e.Row;
            if (row == null) return;

            ATPTEFMCashAdvance ca = CashAdvances.Current;

            if (row.ExpenseReceiptRefNbr.IsNullOrEmpty()) ca.ReceiptsWithNoERRefNbrs++;
            if (row.LiquidationRef.IsNullOrEmpty()) ca.ReceiptsWithNoLiquidationRefNbr++;
        }

        /// <remarks>
        /// 2025-02-04 : "Project settings (disables ProjectTask and CostCode when NonProject is selected) CASE: 010041 {JLTG}
        /// </remarks>
        protected virtual void ATPTEFMCAReceiptDetail_RowSelected(PXCache sender, PXRowSelectedEventArgs e)
        {
            ATPTEFMCARequestDetail row = e.Row as ATPTEFMCARequestDetail;
            ATPTEFMCAReceiptDetail receiptsRow = e.Row as ATPTEFMCAReceiptDetail;
            ATPTEFMCashAdvance ca = CashAdvances.Current;

            #region Enable/Disable Receipt Tab Fields

            if (row != null)
            {
                PXUIFieldAttribute.SetEnabled(sender, e.Row, string.IsNullOrEmpty(((ATPTEFMCAReceiptDetail)e.Row)?.ExpenseReceiptRefNbr));
            }

            if (receiptsRow?.Reversed == true || receiptsRow?.LiquidationRef != null)
            {
                PXUIFieldAttribute.SetEnabled(sender, e.Row, false);
            }

            if (receiptsRow?.VendID != null || receiptsRow?.ExpenseReceiptRefNbr != null)
            {
                PXUIFieldAttribute.SetEnabled<ATPTEFMCAReceiptDetail.vendorName>(sender, e.Row, false);
                PXUIFieldAttribute.SetEnabled<ATPTEFMCAReceiptDetail.vendorAddress>(sender, e.Row, false);
                PXUIFieldAttribute.SetEnabled<ATPTEFMCAReceiptDetail.vendorTin>(sender, e.Row, false);
            }
            else
            {
                PXUIFieldAttribute.SetEnabled<ATPTEFMCAReceiptDetail.vendorName>(sender, e.Row, true);
                PXUIFieldAttribute.SetEnabled<ATPTEFMCAReceiptDetail.vendorAddress>(sender, e.Row, true);
                PXUIFieldAttribute.SetEnabled<ATPTEFMCAReceiptDetail.vendorTin>(sender, e.Row, true);
            }

            #endregion

            #region Disable Budget/PBudget Related Fields
            if (ca?.BudgetEnabled ?? false)
            {
                PXUIFieldAttribute.SetEnabled<ATPTEFMCAReceiptDetail.accountID>(sender, e.Row, false);
                PXUIFieldAttribute.SetEnabled<ATPTEFMCAReceiptDetail.subID>(sender, e.Row, false);
            }

            if (ca?.ProjectBudgetEnabled ?? false)
            {
                PXUIFieldAttribute.SetEnabled<ATPTEFMCAReceiptDetail.projectID>(sender, e.Row, false);
                PXUIFieldAttribute.SetEnabled<ATPTEFMCAReceiptDetail.projectTaskID>(sender, e.Row, false);
                PXUIFieldAttribute.SetEnabled<ATPTEFMCAReceiptDetail.costCodeID>(sender, e.Row, false);
                PXUIFieldAttribute.SetEnabled<ATPTEFMCAReceiptDetail.accountID>(sender, e.Row, false);
                PXUIFieldAttribute.SetEnabled<ATPTEFMCAReceiptDetail.subID>(sender, e.Row, false);
            }
            else
            {
                if (receiptsRow != null)
                {
                    bool isNonProject = ProjectDefaultAttribute.IsNonProject(receiptsRow.ProjectID);
                    PXUIFieldAttribute.SetEnabled<ATPTEFMCAReceiptDetail.projectTaskID>(sender, e.Row, !isNonProject);
                    PXUIFieldAttribute.SetEnabled<ATPTEFMCAReceiptDetail.costCodeID>(sender, e.Row, !isNonProject);
                }
            }
            #endregion
        }
        protected virtual void _(Events.RowDeleted<ATPTEFMCAReceiptDetail> e)
        {
            ATPTEFMCAReceiptDetail row = e.Row;
            if (row is null) return;

            ATPTEFMCashAdvance ca = CashAdvances.Current;
            EnableDisableReturnExcessCa(ca);

            if (ca != null)
            {
                ca.CuryActualSpentAmount -= row.CuryNetAmt;
                CashAdvances.Update(ca);
            }
        }

        protected virtual void _(Events.RowPersisted<ATPTEFMCAReceiptDetail> e)
        {
            ATPTEFMCAReceiptDetail row = e.Row;
            if (row is null) return;

            ATPTEFMCashAdvance ca = CashAdvances.Current;

            if (CashAdvanceReceiptLines.Cache.GetStatus(row) == PXEntryStatus.Inserted)
                EnableDisableReturnExcessCa(ca);
        }
        protected virtual void _(Events.RowDeleting<ATPTEFMCAReceiptDetail> e)
        {
            ATPTEFMCAReceiptDetail row = e.Row;
            if (row is null) return;

            ATPTEFMCashAdvance ca = CashAdvances.Current;

            if (row?.Reversed == true || row?.LiquidationRef != null)
            {
                this.CashAdvanceReceiptLines?.Cache?.RaiseExceptionHandling<ATPTEFMCAReceiptDetail.reversed>(row, row?.Reversed,
                    ATPTEFMHelper.GetPropertyException(row, Messages.ATPTEFMMessages.ReleasedExpenseClaimCannotBeDeleted, PXErrorLevel.Error));
                throw new Exception(Messages.ATPTEFMMessages.ReleasedExpenseClaimCannotBeDeleted);
            }

            if (!string.IsNullOrEmpty(row?.ExpenseReceiptRefNbr))
                throw new Exception(Messages.ATPTEFMMessages.CannotDeleteReceipt);

            ca.ReceiptsWithNoERRefNbrs--;
            ca.ReceiptsWithNoLiquidationRefNbr--;
        }

        // Changes Francis for Liq Date Value
        protected virtual void _(Events.FieldUpdated<ATPTEFMCashAdvance, ATPTEFMCashAdvance.dateOfUse> e)
        {
            ATPTEFMCashAdvance row = (ATPTEFMCashAdvance)e.Row;
            if (row != null)
            {
                var liquidationDate = (Preferences.Current.LiquidationDateBasedOnWorkCalendar ?? false) ? GetLiquidationDateWorkCalendar() : row.DateOfUse.Value.AddDays(GetNumberOfLiquidationDays());

                row.LiqDate = liquidationDate;
                row.InitialLiqDate = liquidationDate;
                row.UnmodifiedLiqDate = liquidationDate;
            }
        }
        protected virtual void _(Events.FieldUpdated<ATPTEFMCashAdvance, ATPTEFMCashAdvance.liqDate> e)
        {
            ATPTEFMCashAdvance row = (ATPTEFMCashAdvance)e.Row;
            if (row != null)
            {
                if (row.Hold ?? false)
                    row.InitialLiqDate = row.LiqDate;
            }
        }
        protected virtual void _(Events.FieldUpdated<ATPTEFMCashAdvance, ATPTEFMCashAdvance.initialLiqDate> e)
        {
            ATPTEFMCashAdvance row = (ATPTEFMCashAdvance)e.Row;
            if (row != null)
            {
                if (row.Hold ?? false)
                    row.LiqDate = row.InitialLiqDate;
            }
        }

        protected virtual void ATPTEFMCashAdvance_Date_FieldUpdated(PXCache sender, PXFieldUpdatedEventArgs e)
        {
            CurrencyInfoAttribute.SetEffectiveDate<ATPTEFMCashAdvance.date>(sender, e);
        }
        protected virtual void ATPTEFMCashAdvance_Descr_FieldVerifying(PXCache sender, PXFieldVerifyingEventArgs e)
        {
            ATPTEFMCashAdvance row = (ATPTEFMCashAdvance)e.Row;
            if (e.NewValue.ToString().Length > 194)
            {
                sender.RaiseExceptionHandling<ATPTEFMCashAdvance.descr>(row, ((ATPTEFMCashAdvance)e?.Row)?.Descr, ATPTEFMHelper.GetPropertyException((ATPTEFMCashAdvance)e?.Row, ATPTEFMMessages.DescCharExceeds, PXErrorLevel.Error));
                throw ATPTEFMHelper.GetPropertyException((ATPTEFMCashAdvance)e?.Row, ATPTEFMMessages.DescCharExceeds, PXErrorLevel.Error);
            }
        }
        protected virtual void _(Events.FieldUpdated<ATPTEFMCARequestDetail, ATPTEFMCARequestDetail.inventoryID> e)
        {
            ATPTEFMCARequestDetail row = e.Row;
            ATPTEFMCashAdvance ca = CashAdvances.Current;
            if (row is null) return;

            if (ca != null)
            {
                ATPTEFMReqClassItems reqClassItems = PXSelect<
                    ATPTEFMReqClassItems,
                    Where<ATPTEFMReqClassItems.tranType, Equal<Required<ATPTEFMReqClassItems.tranType>>,
                        And<ATPTEFMReqClassItems.reqClassID, Equal<Required<ATPTEFMReqClassItems.reqClassID>>,
                        And<ATPTEFMReqClassItems.inventoryID, Equal<Required<ATPTEFMReqClassItems.inventoryID>>>>>>
                    .Select(this, ATPTEFMTranTypeAttribute.CashAdvance, ca.ReqClassID, row.InventoryID);
                if (reqClassItems != null)
                {
                    if (reqClassItems.IsPerDiem == true)
                    {
                        row.UnitCost = reqClassItems.Amount;
                        row.CuryUnitCost = reqClassItems.Amount;
                        row.CuryAmount = row.Qty * row.CuryUnitCost;
                        row.Amount = row.Qty * row.CuryUnitCost;
                    }
                    else
                    {
                        row.UnitCost = decimal.Zero;
                        row.CuryUnitCost = decimal.Zero;
                        row.CuryAmount = row.Qty * row.CuryUnitCost;
                        row.Amount = row.Qty * row.CuryUnitCost;
                    }
                }
            }

            e?.Cache?.SetDefaultExt<ATPTEFMCARequestDetail.unitCost>(e?.Row);
            e?.Cache?.SetDefaultExt<ATPTEFMCARequestDetail.accountID>(e?.Row);
            e?.Cache?.SetDefaultExt<ATPTEFMCARequestDetail.subID>(e?.Row);
        }

        protected virtual void _(Events.FieldDefaulting<ATPTEFMCARequestDetail, ATPTEFMCARequestDetail.accountID> e)
        {
            ATPTEFMCARequestDetail row = e.Row as ATPTEFMCARequestDetail;
            ATPTEFMCashAdvance ca = CashAdvances.Current;

            if (row is null) return;

            if (ca != null)
            {
                ATPTEFMReqClass reqclass = ReqClass.Current;
                if (reqclass != null)
                {
                    #region Default AccountID and SubID
                    switch (reqclass?.UseExpenseAcctFrom)
                    {
                        case RQAccountSource.None:
                            e.NewValue = null;
                            break;
                        case RQAccountSource.Requester:
                            EPEmployee requestBy = PXSelect<
                                EPEmployee,
                                Where<EPEmployee.bAccountID, Equal<Required<EPEmployee.bAccountID>>>>
                                .Select(this, ca?.RequestedByID);
                            e.NewValue = requestBy?.ExpenseAcctID;
                            break;
                        case RQAccountSource.RequestClass:
                            e.NewValue = reqclass?.ExpenseAcctID;
                            break;
                        case RQAccountSource.Department:
                            EPDepartment department = PXSelect<
                                EPDepartment,
                                Where<EPDepartment.departmentID, Equal<Required<EPDepartment.departmentID>>>>
                                .Select(this, ca?.DepartmentID);
                            e.NewValue = department?.ExpenseAccountID;
                            break;
                        case RQAccountSource.PurchaseItem:
                            InventoryItem item = InventoryItem.PK.Find(this, row?.InventoryID);
                            e.NewValue = item?.COGSAcctID;
                            break;
                    }
                    #endregion

                    //if (reqclass.RestrictMultInvIns.GetValueOrDefault())
                    //{
                    //    CheckInventoryDuplicate();
                    //}
                }
            }
        }
        /// <remarks>
        /// 2025-06-20: Prevent validation if it is migrated record CASE: 012042 {JLG}
        /// </remarks>
        protected virtual void _(Events.RowPersisting<ATPTEFMCARequestDetail> e)
        {
            ATPTEFMCARequestDetail row = e.Row;
            ATPTEFMCashAdvance ca = CashAdvances.Current;
            if (row is null) return;

            if (ca != null)
            {
                ATPTEFMReqClassItems reqClassItems = PXSelect<
                    ATPTEFMReqClassItems,
                    Where<ATPTEFMReqClassItems.tranType, Equal<Required<ATPTEFMReqClassItems.tranType>>,
                        And<ATPTEFMReqClassItems.reqClassID, Equal<Required<ATPTEFMReqClassItems.reqClassID>>,
                        And<ATPTEFMReqClassItems.inventoryID, Equal<Required<ATPTEFMReqClassItems.inventoryID>>>>>>
                    .Select(this, ATPTEFMTranTypeAttribute.CashAdvance, ca.ReqClassID, row.InventoryID);
                if (reqClassItems != null)
                {
                    if (reqClassItems.IsPerDiem == true && row.CuryUnitCost > reqClassItems.Amount && (ca.ExecuteValidations ?? true))
                    {
                        e.Cache.RaiseExceptionHandling<ATPTEFMCARequestDetail.curyUnitCost>(row, ((ATPTEFMCARequestDetail)e?.Row)?.UnitCost, ATPTEFMHelper.GetPropertyException(row, ATPTEFMMessages.RequestAmountMustBeWithinLimitAmt, PXErrorLevel.Error));
                        throw new Exception(ATPTEFMMessages.RequestAmountMustBeWithinLimitAmt);
                    }
                }

                var reqClass = ReqClass.Current;
                if (reqClass?.RestrictMultInvIns.GetValueOrDefault() ?? false)
                {
                    var hasDuplicateInventory = CashAdvanceRequestLines.Select().RowCast<ATPTEFMCARequestDetail>()
                        .Where(r => r.InventoryID == row.InventoryID)
                        .Count() > 1;
                    if (hasDuplicateInventory)
                    {
                        e.Cancel = true;
                        throw new Exception(Messages.ATPTEFMMessages.DuplicateInventory);
                    }
                }

                BudgetValidationRaiseErrorForRequestAmountGreaterThanRemainingBudget(row, ca.BudgetEnabled ?? false);
                PBudgetValidationRaiseErrorForRequestAmountGreaterThanRemainingBudget(row, ca?.ProjectBudgetEnabled ?? false);
            }
        }
        protected virtual void _(Events.FieldDefaulting<ATPTEFMCashAdvance, ATPTEFMCashAdvance.budgetEnabled> e)
        {
            ATPTEFMCashAdvance row = e.Row;
            if (row == null) return;

            if (BudgetVisible(FeatureSetup.Current, "C") && row.BudgetEnabled == null)
            {
                e.NewValue = true;
                e.Cancel = true;
            }
            else if ((!BudgetVisible(FeatureSetup.Current, "C")) && row.BudgetEnabled == null)
            {
                e.NewValue = false;
                e.Cancel = true;
            }
        }
        protected virtual void _(Events.FieldDefaulting<ATPTEFMCashAdvance, ATPTEFMCashAdvance.projectBudgetEnabled> e)
        {
            ATPTEFMCashAdvance row = e.Row;
            if (row == null) return;

            if (ProjectBudgetVisible(FeatureSetup.Current, "C") && row.ProjectBudgetEnabled == null)
            {
                e.NewValue = true;
                e.Cancel = true;
            }
            else
            {
                e.NewValue = false;
                e.Cancel = true;
            }
        }
        protected virtual void _(Events.RowPersisting<ATPTEFMCAReceiptDetail> e)
        {
            ATPTEFMCAReceiptDetail row = e.Row;
            ATPTEFMCashAdvance ca = CashAdvances.Current;

            if (row is null) return;

            if (ca != null && (ca.ExecuteValidations ?? true))
            {
                if (ca.Date > row.Date)
                {
                    e.Cache.RaiseExceptionHandling<ATPTEFMCAReceiptDetail.date>(row, ((ATPTEFMCAReceiptDetail)e?.Row)?.Date, ATPTEFMHelper.GetPropertyException(row, ATPTEFMMessages.ReceiptDateNotLessThanCADocumentDate, PXErrorLevel.Error));
                    e.Cancel = true;
                    throw new Exception(ATPTEFMMessages.ReceiptDateNotLessThanCADocumentDate);
                }

                if (row.Date > ca.LiqDate)
                {
                    e.Cache.RaiseExceptionHandling<ATPTEFMCAReceiptDetail.date>(row, ((ATPTEFMCAReceiptDetail)e?.Row)?.Date, ATPTEFMHelper.GetPropertyException(row, ATPTEFMMessages.ReceiptDateNotGreaterThanLiqDate, PXErrorLevel.Error));
                    e.Cancel = true;
                    throw new Exception(ATPTEFMMessages.ReceiptDateNotGreaterThanLiqDate);
                }

                if (row.NetQty == decimal.Zero)
                {
                    e.Cache.RaiseExceptionHandling<ATPTEFMCAReceiptDetail.netQty>(row, row?.NetQty, ATPTEFMHelper.GetPropertyException(row, Messages.ATPTEFMMessages.QuantityMustBeGreaterThanZero, PXErrorLevel.Error));
                    e.Cancel = true;
                }

                if (row.CuryNetUnitCost == decimal.Zero)
                {
                    e.Cache.RaiseExceptionHandling<ATPTEFMCAReceiptDetail.curyNetUnitCost>(row, row?.CuryNetUnitCost, ATPTEFMHelper.GetPropertyException(row, Messages.ATPTEFMMessages.UnitCostMustBeGreaterThanZero, PXErrorLevel.Error));
                    e.Cancel = true;
                }
            }

            BudgetValidationRaiseErrorForLiquidationAmtGreaterThanRequestAmt(ca.BudgetEnabled ?? false, true);
            PBudgetValidationRaiseErrorForLiquidationAmtGreaterThanRequestAmt(ca.ProjectBudgetEnabled ?? false);

        }
        /// <remarks>
        /// 010461 - (CFM2024R1, CFM2024R1A & B) Error upon changing the Vendor in the Expense Receipts from profiled to non-profiled
        /// CASE 012187 -> CFM: Unable to Submit Liquidation {JTG} <br/>
        /// CASE 012719 : Expense Claim - Liquidation error {JTG}
        /// </remarks>
        protected virtual void _(Events.RowUpdated<ATPTEFMCAReceiptDetail> e)
        {
            ATPTEFMCAReceiptDetail row = e.Row;
            if (row == null) return;

            ATPTEFMCashAdvance ca = CashAdvances.Current;

            if (row.ProjectID == ProjectDefaultAttribute.NonProject())
                BudgetValidationRaiseErrorForLiquidationAmtGreaterThanRequestAmt(ca?.BudgetEnabled ?? false);
            else
                PBudgetValidationRaiseErrorForLiquidationAmtGreaterThanRequestAmt(ca.ProjectBudgetEnabled ?? false);

            #region Update related fields on ER 
            if (row.ExpenseReceiptRefNbr != null && e.Cache.GetStatus(row) == PXEntryStatus.Updated && e.ExternalCall)
            {
                EPExpenseClaimDetails details = Receipt.Select();
                ATPTEFMEPExpenseClaimDetailsExt detailsExt = details.GetExtension<ATPTEFMEPExpenseClaimDetailsExt>();

                bool hasChanges = false;

                if (details != null)
                {
                    if (details.ExpenseRefNbr != row.RefNbr)
                    {
                        details.ExpenseRefNbr = row.RefNbr;
                        hasChanges = true;
                    }

                    if (details.ExpenseDate != row.Date)
                    {
                        details.ExpenseDate = row.Date;
                        hasChanges = true;
                    }

                    if (details.InventoryID != row.InventoryID)
                    {
                        details.InventoryID = row.InventoryID;
                        hasChanges = true;
                    }

                    if (details.Qty != row.NetQty)
                    {
                        details.Qty = row.NetQty;
                        hasChanges = true;
                    }

                    if (details.CuryUnitCost != row.CuryNetUnitCost)
                    {
                        details.CuryUnitCost = row.CuryNetUnitCost;
                        hasChanges = true;
                    }

                    if (details.ExpenseAccountID != row.AccountID)
                    {
                        details.ExpenseAccountID = row.AccountID;
                        hasChanges = true;
                    }

                    if (details.ExpenseSubID != row.SubID)
                    {
                        details.ExpenseSubID = row.SubID;
                        hasChanges = true;
                    }

                    if (details.ContractID != row.ProjectID)
                    {
                        details.ContractID = row.ProjectID;
                        hasChanges = true;
                    }

                    if (details.TaskID != row.ProjectTaskID)
                    {
                        details.TaskID = row.ProjectTaskID;
                    }

                    if (details.TaxCategoryID != row.TaxCategoryID)
                    {
                        details.TaxCategoryID = row.TaxCategoryID;
                        hasChanges = true;
                    }

                    if (details.TaxZoneID != row.TaxZoneID)
                    {
                        details.TaxZoneID = row.TaxZoneID;
                        hasChanges = true;
                    }

                    if (GetDefATC(details) != row.AtcCode)
                    {
                        SetDefATC(details, row.AtcCode);
                        hasChanges = true;
                    }

                    if (details.TranDesc != row.LineDescription)
                    {
                        details.TranDesc = row.LineDescription;
                        hasChanges = true;
                    }

                    Location extAddress = null;
                    if (row.VendID != null)
                    {
                        PXResultset<Vendor, BAccount, Address, Location> vendorInfo = PXSelectJoin<
                            Vendor,
                            InnerJoin<BAccount,
                                On<BAccount.bAccountID, Equal<Vendor.bAccountID>>,
                            LeftJoin<Address,
                                On<Address.addressID, Equal<BAccount.defAddressID>>,
                            LeftJoin<Location,
                                On<Location.bAccountID, Equal<BAccount.defLocationID>>>>>,
                            Where<Vendor.bAccountID, Equal<Required<Vendor.bAccountID>>>>
                            .Select<PXResultset<Vendor, BAccount, Address, Location>>(this, row.VendID);

                        Vendor vendor = (Vendor)vendorInfo;
                        Address address = (Address)vendorInfo;
                        extAddress = (Location)vendorInfo;

                        if (detailsExt.UsrATPTEFMDetailVendorID != row.VendID)
                        {
                            detailsExt.UsrATPTEFMDetailVendorID = row.VendID;
                            hasChanges = true;
                        }

                        if (detailsExt?.UsrATPTVendID.Trim() != vendor?.AcctCD.Trim())
                        {
                            detailsExt.UsrATPTVendID = vendor.AcctCD;
                            hasChanges = true;
                        }

                        if (detailsExt.UsrATPTVendName != row.VendorName)
                        {
                            detailsExt.UsrATPTVendName = row.VendorName;
                            hasChanges = true;
                        }

                        if (detailsExt.UsrATPTAddress != row.VendorAddress)
                        {
                            detailsExt.UsrATPTAddress = row.VendorAddress;
                            hasChanges = true;
                        }

                        if (detailsExt.UsrATPTVendTIN != row.VendorTin)
                        {
                            detailsExt.UsrATPTVendTIN = row.VendorTin;
                            hasChanges = true;
                        }
                    }
                    else
                    {
                        #region EFMChanges
                        if (detailsExt.UsrATPTVendName != row.VendorName)
                        {
                            detailsExt.UsrATPTVendName = row.VendorName;
                            hasChanges = true;
                        }

                        if (detailsExt.UsrATPTAddress != row.VendorAddress)
                        {
                            detailsExt.UsrATPTAddress = row.VendorAddress;
                            hasChanges = true;
                        }

                        if (detailsExt.UsrATPTVendTIN != row.VendorTin)
                        {
                            detailsExt.UsrATPTVendTIN = row.VendorTin;
                            hasChanges = true;
                        }
                        #endregion
                    }

                    if (hasChanges)
                        Receipt.Update(details);
                }
            }
            #endregion

            if (ca != null)
            {
                var totalReceiptsAmt = CashAdvanceReceiptLines.Select().RowCast<ATPTEFMCAReceiptDetail>().Where(s => s.Reversed != true).Sum(s => s.CuryNetAmt);

                if (ca.CuryActualSpentAmount != totalReceiptsAmt)
                {
                    ca.CuryActualSpentAmount = totalReceiptsAmt;
                    CashAdvances.Update(ca);
                }
            }
        }
        protected virtual void _(Events.FieldDefaulting<ATPTEFMCARequestDetail, ATPTEFMCARequestDetail.subID> e)
        {
            ATPTEFMCARequestDetail row = e.Row;
            ATPTEFMCashAdvance ca = CashAdvances.Current;

            if (row is null) return;

            if (ca != null && row.InventoryID != null)
            {
                ATPTEFMReqClass reqclass = ReqClass.Current;
                int? requestclassSubID = reqclass?.ExpenseSubID;

                //Purchase Item
                InventoryItem item = InventoryItem.PK.Find(this, row.InventoryID);
                int? inventorySubID = item?.COGSSubID;

                //Department
                EPDepartment department = PXSelect<
                    EPDepartment,
                    Where<EPDepartment.departmentID, Equal<Required<EPDepartment.departmentID>>>>
                    .Select(this, ca?.DepartmentID);
                int? departmentSubID = department?.ExpenseSubID;

                //Requesters
                EPEmployee requestBy = PXSelect<
                    EPEmployee,
                    Where<EPEmployee.bAccountID, Equal<Required<EPEmployee.bAccountID>>>>
                    .Select(this, ca?.RequestedByID);
                int? requesterSubID = requestBy?.ExpenseSubID;

                if (reqclass != null)
                {
                    object value = SubAccountMaskAttribute.MakeSub<ATPTEFMReqClass.combineExpSub>(this, reqclass.CombineExpSub,
                                    new object[] { requestclassSubID, departmentSubID, inventorySubID, requesterSubID },
                                    new Type[] { typeof(ATPTEFMReqClass.expenseSubID), typeof(EPDepartment.expenseSubID), typeof(InventoryItem.cOGSSubID), typeof(EPEmployee.expenseSubID) });

                    e.Cache.RaiseFieldUpdating<ATPTEFMCARequestDetail.subID>(e?.Row, ref value);
                    e.NewValue = (int?)value;
                }
                e.Cancel = true;
            }
        }
        protected virtual void _(Events.FieldUpdated<ATPTEFMCARequestDetail, ATPTEFMCARequestDetail.accountID> e)
        {
            e?.Cache?.SetDefaultExt<ATPTEFMCARequestDetail.subID>(e?.Row);
        }

        /// <remarks>
        /// 2024-10-17 : Removed the code to default the atc code. It is transfered on DACExt on Philtax-Connector. {RRS}
        /// </remarks>
        protected virtual void _(Events.FieldUpdated<ATPTEFMCAReceiptDetail, ATPTEFMCAReceiptDetail.inventoryID> e)
        {
            ATPTEFMCAReceiptDetail row = e.Row;
            if (row is null) return;

            if (!e.Cache.Graph.IsImportFromExcel) 
            {
                e?.Cache.SetDefaultExt<ATPTEFMCAReceiptDetail.accountID>(e?.Row);
                e?.Cache.SetDefaultExt<ATPTEFMCAReceiptDetail.subID>(e?.Row);
            }
        }
        protected virtual void _(Events.FieldUpdated<ATPTEFMCAReceiptDetail, ATPTEFMCAReceiptDetail.accountID> e)
        {
            e?.Cache.SetDefaultExt<ATPTEFMCAReceiptDetail.subID>(e?.Row);
        }
        protected virtual void _(Events.FieldDefaulting<ATPTEFMCAReceiptDetail, ATPTEFMCAReceiptDetail.accountID> e)
        {
            ATPTEFMCAReceiptDetail row = e.Row;
            ATPTEFMCashAdvance ca = CashAdvances.Current;

            if (row is null) return;

            if (ca != null)
            {
                ATPTEFMReqClass reqclass = ReqClass.Current;
                if (reqclass != null)
                {
                    #region Default AccountID and SubID
                    switch (reqclass.UseExpenseAcctFrom)
                    {
                        case RQAccountSource.None:
                            e.NewValue = null;
                            break;
                        case RQAccountSource.Requester:
                            EPEmployee requestBy = PXSelect<
                                EPEmployee,
                                Where<EPEmployee.bAccountID, Equal<Required<EPEmployee.bAccountID>>>>
                                .Select(this, ca?.RequestedByID);
                            e.NewValue = requestBy?.ExpenseAcctID;
                            break;
                        case RQAccountSource.RequestClass:
                            e.NewValue = reqclass.ExpenseAcctID;
                            break;
                        case RQAccountSource.Department:
                            EPDepartment department = PXSelect<
                                EPDepartment,
                                Where<EPDepartment.departmentID, Equal<Required<EPDepartment.departmentID>>>>
                                .Select(this, ca?.DepartmentID);
                            e.NewValue = department?.ExpenseAccountID;
                            break;
                        case RQAccountSource.PurchaseItem:
                            InventoryItem item = InventoryItem.PK.Find(this, row.InventoryID);
                            e.NewValue = item?.COGSAcctID;
                            break;
                    }
                    #endregion
                }
            }
        }
        protected virtual void _(Events.FieldDefaulting<ATPTEFMCAReceiptDetail, ATPTEFMCAReceiptDetail.subID> e)
        {
            ATPTEFMCAReceiptDetail row = e.Row;
            ATPTEFMCashAdvance ca = CashAdvances.Current;

            if (row is null) return;

            if (ca != null && row?.InventoryID != null)
            {
                ATPTEFMReqClass reqclass = ReqClass.Current;
                int? requestclassSubID = reqclass?.ExpenseSubID;

                //Purchase Item
                InventoryItem item = InventoryItem.PK.Find(this, row?.InventoryID);
                int? inventorySubID = item?.COGSSubID;

                //Department
                EPDepartment department = PXSelect<
                    EPDepartment,
                    Where<EPDepartment.departmentID, Equal<Required<EPDepartment.departmentID>>>>
                    .Select(this, ca?.DepartmentID);
                int? departmentSubID = department?.ExpenseSubID;

                //Requesters
                EPEmployee requestBy = PXSelect<
                    EPEmployee,
                    Where<EPEmployee.bAccountID, Equal<Required<EPEmployee.bAccountID>>>>
                    .Select(this, ca?.RequestedByID);
                int? requesterSubID = requestBy?.ExpenseSubID;

                if (reqclass != null)
                {
                    object value = SubAccountMaskAttribute.MakeSub<ATPTEFMReqClass.combineExpSub>(this, reqclass.CombineExpSub,
                                    new object[] { requestclassSubID, departmentSubID, inventorySubID, requesterSubID },
                                    new Type[] { typeof(ATPTEFMReqClass.expenseSubID), typeof(EPDepartment.expenseSubID), typeof(InventoryItem.cOGSSubID), typeof(EPEmployee.expenseSubID) });

                    e.Cache.RaiseFieldUpdating<ATPTEFMCAReceiptDetail.subID>(e.Row, ref value);
                    e.NewValue = (int?)value;
                }
                e.Cancel = true;
            }
        }
        protected virtual void ATPTEFMCashAdvance_ReqClassID_FieldDefaulting(PXCache sender, PXFieldDefaultingEventArgs e)
        {
            if (reqClassID != null)
            {
                e.NewValue = reqClassID;
                e.Cancel = true;
            }
            else
            {
                foreach (ATPTEFMReqClass item in PXSelectOrderBy<
                    ATPTEFMReqClass,
                    OrderBy<
                        Asc<ATPTEFMReqClass.reqClassID>>>
                    .Select(this))
                {
                    if (item != null && item.TranType == ATPTEFMTranTypeAttribute.CashAdvance)
                    {

                        e.NewValue = item?.ReqClassID;
                        e.Cancel = true;
                        break;
                    }
                }

            }
        }
        /// <remarks>
        /// 2024-01-10: When save or remove hold is clicked, "Error: Another process has added the 'ATPTEFMCashAdvance' record.". 007827 {JLG}
        /// Related case: Case: 007394 
        /// </remarks>
        protected virtual void _(Events.RowPersisting<ATPTEFMCashAdvance> e)
        {
            ATPTEFMCashAdvance row = e.Row;
            if (row != null)
            {
                if (row.CuryRequestedAmount <= 0 && (!Preferences.Current.IsCashAdvanceMigration ?? false))
                {
                    e.Cache.RaiseExceptionHandling<ATPTEFMCashAdvance.curyRequestedAmount>(row, row?.CuryRequestedAmount,
                                    ATPTEFMHelper.GetPropertyException(row, ATPTEFMMessages.CannotBeZero, PXErrorLevel.Error));
                    e.Cancel = true;
                    throw new Exception(ATPTEFMMessages.CannotBeZero);
                }

                if ((row.BudgetEnabled ?? false) && (row.IsOverbudget ?? false) && FeatureSetup.Current.BudgetValidation == RQRequestClassBudget.Error)
                {
                    e.Cancel = true;
                    throw new PXException(ATPTEFMMessages.OverbudgetWarning);
                }

                if (row.Status.Equals(ATPTEFMCashAdvanceStatusAttribute.PendingLiquidationValue))
                {
                    row.CuryActualSpentAmount = CashAdvanceReceiptLines.Select().RowCast<ATPTEFMCAReceiptDetail>().Where(s => s.Reversed != true).Sum(s => s.CuryNetAmt);
                    CashAdvances.Update(row);
                }
            }
        }
        protected bool customerChanged = false;

        protected virtual void _(Events.FieldUpdated<ATPTEFMCashAdvance, ATPTEFMCashAdvance.requestedByID> e)
        {
            ATPTEFMCashAdvance ca = e.Row;
            if (ca is null) return;

            EPEmployee oldEmployeeObj = PXSelect<
                EPEmployee,
                Where<EPEmployee.bAccountID, Equal<Required<EPEmployee.bAccountID>>>>
                .Select(this, e.OldValue);

            EPEmployee currentEmployeeObj = this.RequesterEmployee.Select();

            if (currentEmployeeObj != null && oldEmployeeObj != null)
            {
                if (oldEmployeeObj.CalendarID != currentEmployeeObj.CalendarID)
                {
                    ca.DateOfUse = null;
                    ca.InitialLiqDate = null;
                    ca.LiqDate = null;
                }
            }

            if (PXAccess.FeatureInstalled<FeaturesSet.multicurrency>())
            {
                CurrencyInfo info = CurrencyInfoAttribute.SetDefaults<ATPTEFMCashAdvance.curyInfoID>(e.Cache, e.Row);

                string message = PXUIFieldAttribute.GetError<CurrencyInfo.curyEffDate>(currencyinfo.Cache, info);
                if (string.IsNullOrEmpty(message) == false)
                {
                    e.Cache.RaiseExceptionHandling<ATPTEFMCashAdvance.date>(e.Row, ((ATPTEFMCashAdvance)e.Row).Date, ATPTEFMHelper.GetPropertyException((ATPTEFMCashAdvance)e.Row, message, PXErrorLevel.Warning));
                }
                if (info != null)
                {
                    ca.CuryID = info.CuryID;
                }
            }
        }
        #endregion

        #region CurrencyInfo events
        protected virtual void CurrencyInfo_CuryID_FieldDefaulting(PXCache sender, PXFieldDefaultingEventArgs e)
        {
            if (PXAccess.FeatureInstalled<FeaturesSet.multicurrency>())
            {
                EPEmployee employee = EPEmployee.Select();
                if (employee != null && !string.IsNullOrEmpty(employee.CuryID))
                {
                    e.NewValue = employee.CuryID;
                    e.Cancel = true;
                }
            }
        }
        protected virtual void CurrencyInfo_CuryRateTypeID_FieldDefaulting(PXCache sender, PXFieldDefaultingEventArgs e)
        {
            if (PXAccess.FeatureInstalled<FeaturesSet.multicurrency>())
            {
                if (RequesterEmployee?.Current != null && !string.IsNullOrEmpty(RequesterEmployee?.Current?.CuryRateTypeID))
                {
                    e.NewValue = RequesterEmployee?.Current?.CuryRateTypeID;
                    e.Cancel = true;
                }
                else
                {
                    CMSetup cmsetup = PXSelect<CMSetup>.Select(this);
                    if (cmsetup != null)
                    {
                        e.NewValue = cmsetup?.ARRateTypeDflt;
                        e.Cancel = true;
                    }
                }

            }
        }
        protected virtual void CurrencyInfo_CuryEffDate_FieldDefaulting(PXCache sender, PXFieldDefaultingEventArgs e)
        {
            if (this.CashAdvances.Cache.Current != null)
            {
                e.NewValue = ((ATPTEFMCashAdvance)CashAdvances?.Cache?.Current)?.Date;
                e.Cancel = true;
            }
        }
        protected virtual void CurrencyInfo_RowSelected(PXCache sender, PXRowSelectedEventArgs e)
        {
            CurrencyInfo info = e.Row as CurrencyInfo;
            if (info != null)
            {
                bool curyenabled = info.AllowUpdate(this.CashAdvanceRequestLines.Cache);

                if (RequesterEmployee?.Current != null && !(bool)RequesterEmployee?.Current?.AllowOverrideRate)
                {
                    curyenabled = false;
                }

                PXUIFieldAttribute.SetEnabled<CurrencyInfo.curyRateTypeID>(sender, info, curyenabled);
                PXUIFieldAttribute.SetEnabled<CurrencyInfo.curyEffDate>(sender, info, curyenabled);
                PXUIFieldAttribute.SetEnabled<CurrencyInfo.sampleCuryRate>(sender, info, curyenabled);
                PXUIFieldAttribute.SetEnabled<CurrencyInfo.sampleRecipRate>(sender, info, curyenabled);
            }
        }
        private ATPTEFMCashAdvance _deletedCashAdvance = null;
        protected virtual void _(Events.RowDeleting<ATPTEFMCashAdvance> e)
        {
            if (e.Row == null) return;
            _deletedCashAdvance = e.Row;
        }
        #endregion

        #region EPApproval Cache Attached
        [PXDBDate()]
        [PXDefault(typeof(ATPTEFMCashAdvance.date), PersistingCheck = PXPersistingCheck.Nothing)]
        protected virtual void EPApproval_DocDate_CacheAttached(PXCache sender)
        {
        }

        [PXDBInt()]
        [PXDefault(typeof(ATPTEFMCashAdvance.requestedByID), PersistingCheck = PXPersistingCheck.Nothing)]
        protected virtual void EPApproval_BAccountID_CacheAttached(PXCache sender)
        {
        }

        [PXDBString(60, IsUnicode = true)]
        [PXDefault(typeof(ATPTEFMCashAdvance.descr), PersistingCheck = PXPersistingCheck.Nothing)]
        protected virtual void EPApproval_Descr_CacheAttached(PXCache sender)
        {
        }

        [PXDBDecimal(4)]
        [PXDefault(typeof(ATPTEFMCashAdvance.curyRequestedAmount), PersistingCheck = PXPersistingCheck.Nothing)]
        protected virtual void EPApproval_CuryTotalAmount_CacheAttached(PXCache sender)
        {
        }

        #endregion

        #region Internal Types

        [Serializable]
        [PXHidden]
        public class ATPTEFMCashAdvanceSummary : CashFundManagement.DAC.Base.ATPTEFMBase, IBqlTable
        {
            public abstract class cashAdvanceNbr : PX.Data.BQL.BqlString.Field<cashAdvanceNbr> { }
            [PXString(15)]
            public virtual string CashAdvanceNbr { get; set; }

            public abstract class requestDetailID : PX.Data.BQL.BqlInt.Field<requestDetailID> { }
            [PXInt]
            public virtual int? RequestDetailID { get; set; }

            public abstract class receiptDetailIID : PX.Data.BQL.BqlInt.Field<receiptDetailIID> { }
            [PXInt]
            public virtual int? ReceiptDetailIID { get; set; }

            public abstract class acctID : PX.Data.BQL.BqlInt.Field<acctID> { }
            [PXInt]
            public virtual int? AcctID { get; set; }

            public abstract class subID : PX.Data.BQL.BqlInt.Field<subID> { }
            [PXInt]
            public virtual int? SubID { get; set; }

            public abstract class amount : PX.Data.BQL.BqlDecimal.Field<amount> { }
            [PXDecimal(2)]
            public virtual decimal? Amount { get; set; }
        }

        [Serializable]
        [PXHidden]
        public class ATPTEFMCADetailSummary : CashFundManagement.DAC.Base.ATPTEFMBase, IBqlTable
        {
            #region RequestID
            [PXInt]
            public int? RequestID { get; set; }
            public abstract class requestID : PX.Data.BQL.BqlInt.Field<requestID> { }
            #endregion
            #region AccountID
            [PXInt]
            public int? AcctID { get; set; }
            public abstract class acctID : PX.Data.BQL.BqlInt.Field<acctID> { }
            #endregion
            #region SubID
            [PXInt]
            public int? SubID { get; set; }
            public abstract class subID : PX.Data.BQL.BqlInt.Field<subID> { }
            #endregion
            #region ProjectID
            [PXInt]
            public int? ProjectID { get; set; }
            public abstract class projectID : PX.Data.BQL.BqlInt.Field<projectID> { }
            #endregion
            #region ProjectTaskID
            [PXInt]
            public int? ProjectTaskID { get; set; }
            public abstract class projectTaskID : PX.Data.BQL.BqlInt.Field<projectTaskID> { }
            #endregion
            #region InventoryID
            [PXInt]
            public int? InventoryID { get; set; }
            public abstract class inventoryID : PX.Data.BQL.BqlInt.Field<inventoryID> { }
            #endregion
            #region CostCodeID
            [PXInt]
            public int? CostCodeID { get; set; }
            public abstract class costCodeID : PX.Data.BQL.BqlInt.Field<costCodeID> { }
            #endregion
            #region AccountGroupID
            [PXInt]
            public int? AccountGroupID { get; set; }
            public abstract class accountGroupID : PX.Data.BQL.BqlInt.Field<accountGroupID> { }
            #endregion
            #region Qty
            [PXDecimal]
            public decimal? Qty { get; set; }
            public abstract class qty : PX.Data.BQL.BqlDecimal.Field<qty> { }
            #endregion
            #region UnitCost
            [PXDecimal]
            public decimal? UnitCost { get; set; }
            public abstract class unitCost : PX.Data.BQL.BqlDecimal.Field<unitCost> { }
            #endregion
            #region NetQty
            [PXDecimal]
            public decimal? NetQty { get; set; }
            public abstract class netQty : PX.Data.BQL.BqlDecimal.Field<netQty> { }
            #endregion
            #region NetAmt
            [PXDecimal]
            public decimal? NetAmt { get; set; }
            public abstract class netAmt : PX.Data.BQL.BqlDecimal.Field<netAmt> { }
            #endregion
            #region CuryID

            [PXString]
            public string CuryID { get; set; }

            public abstract class curyID : PX.Data.BQL.BqlString.Field<curyID>
            { }

            #endregion CuryID
        }

        [Serializable]
        [PXCacheName("Voided Document History")]
        public class ATPTEFMVoidedDocument : CashFundManagement.DAC.Base.ATPTEFMBase, IBqlTable
        {
            #region SortID
            [PXInt(IsKey = true)]
            public virtual int? SortID { get; set; }
            public abstract class sortID : PX.Data.BQL.BqlInt.Field<sortID> { }
            #endregion
            #region BranchID
            [Branch(DisplayName = ATPTEFMMessages.Branch, Enabled = false)]
            //[PXUIField(DisplayName = ATPTEFMMessages.Branch, Enabled = false)]
            public int? BranchID { get; set; }
            public abstract class branchID : PX.Data.BQL.BqlInt.Field<branchID> { }
            #endregion
            #region DocType
            [PXString(10, IsUnicode = true)]
            [PXUIField(DisplayName = ATPTEFMMessages.Type, Enabled = false)]
            [APDocType.List()]
            public string DocType { get; set; }
            public abstract class docType : PX.Data.BQL.BqlString.Field<docType> { }
            #endregion
            #region RefNbr
            [PXString(15, IsUnicode = true, IsKey = true)]
            [PXUIField(DisplayName = ATPTEFMMessages.RefNbr, Enabled = false)]
            public string RefNbr { get; set; }
            public abstract class refNbr : PX.Data.BQL.BqlString.Field<refNbr> { }
            #endregion
            #region Date
            [PXDate]
            [PXUIField(DisplayName = ATPTEFMMessages.Date, Enabled = false)]
            public virtual DateTime? Date { get; set; }
            public abstract class date : PX.Data.BQL.BqlDateTime.Field<date> { }
            #endregion
            #region VendorID
            [VendorActive(DisplayName = ATPTEFMMessages.Vendor, Enabled = false)]
            //[PXUIField(DisplayName = ATPTEFMMessages.Vendor, Enabled = false)]
            public virtual int? VendorID { get; set; }
            public abstract class vendorID : PX.Data.BQL.BqlInt.Field<vendorID> { }
            #endregion
            #region Descr
            [PXString(255, IsUnicode = true)]
            [PXUIField(DisplayName = ATPTEFMMessages.Description, Enabled = false)]
            public virtual string Descr { get; set; }
            public abstract class descr : PX.Data.BQL.BqlString.Field<descr> { }
            #endregion
            #region Amount
            [PXDecimal]
            [PXUnboundDefault("0.00")]
            [PXUIField(DisplayName = ATPTEFMMessages.Amount, Enabled = false)]
            public decimal? Amount { get; set; }
            public abstract class amount : PX.Data.BQL.BqlDecimal.Field<amount> { }
            #endregion
            #region CreatedDateTime
            [PXDateAndTime]
            public DateTime? CreatedDateTime { get; set; }
            public abstract class createdDateTime : PX.Data.BQL.BqlDateTime.Field<createdDateTime> { }
            #endregion
        }
        #endregion
    }
}