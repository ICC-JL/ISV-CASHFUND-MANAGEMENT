using CashFundManagement.Attributes;
using CashFundManagement.Classes;
using CashFundManagement.DAC;
using CashFundManagement.DAC.Setup;
using CashFundManagement.DAC.Unbound;
using CashFundManagement.Extensions.Attribute;
using CashFundManagement.Extensions.DAC;
using CashFundManagement.Helper;
using CashFundManagement.Messages;
using CashFundManagement.MethodExtensions;
using PX.Api;
using PX.Common;
using PX.Data;
using PX.Data.BQL;
using PX.Data.ReferentialIntegrity;
using PX.Data.WorkflowAPI;
using PX.Objects.AP;
using PX.Objects.CA;
using PX.Objects.CM;
using PX.Objects.Common;
using PX.Objects.Common.Extensions;
using PX.Objects.CR;
using PX.Objects.CS;
using PX.Objects.EP;
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
using System.Text;
using static CashFundManagement.BLC.ATPTEFMFundMaint;
using static CashFundManagement.Classes.ATPTEFMBudgetLibrary;
using static CashFundManagement.Classes.ATPTEFMProjectBudgetLibrary;
using static CashFundManagement.Helper.ATPTEFMShared;
using static PX.SM.EMailAccount;

namespace CashFundManagement.BLC
{
    /// <remarks>
    /// 010267 - (CFM2024R1/2024R2) Fund Management Preferences>Other Settings: Additional fields for 'Use Expense Account From:' and Combine Expense Sub From:
    /// </remarks>
    public class ATPTEFMFundTransactionEntry : ATPTPXGraphWithWorkflow<ATPTEFMFundTransactionEntry, ATPTEFMFundTransaction>
    {
        #region Constructor

        public ATPTEFMFundTransactionEntry()
        {
#if !Version23R2
            if (!(CASetupPreferences?.Current?.EnableCFM ?? false))
            {
                throw new PXException(ATPTEFMMessages.CashFundManagementFeatureDisabled);
            }
#endif
            CreateAPBill.SetEnabled(false);
            Liquidate.SetEnabled(false);
            ReleaseCash.SetEnabled(false);
            SubmitReceipts.SetEnabled(false);
            FTCancel.SetEnabled(false);

            //BudgetVisibility(Budget.Cache, FeatureSetup?.Current, "F");
            //ProjectBudgetVisibility(ProjectBudget.Cache, FeatureSetup?.Current, "F");
            //ProjectBudget.AllowSelect = false;
        }

#endregion Constructor

        #region Views

        public PXSetup<ATPTEFMFeatures> FeatureSetup;
        public PXSetup<CASetup> Setup;
        public PXSelect<ATPTEFM2023R2Enhancements> Enhancements;
        public PXSetup<ATPTEFMSetup> Preferences;
        public PXSetup<ATPTEFMCASetup> CASetupPreferences;
        public PXSetup<FeaturesSet> EnableFeatures;
        public PXSetup<APSetup> APSetup;
        public PXSetup<MasterFinPeriod>.Where<
            Where<MasterFinPeriod.finPeriodID, Equal<Current<ATPTEFMFundTransaction.finPeriodID>>>>
            finperiod;
        public PXSetup<Company> company;

        [PXViewName("Transaction History")]
        public PXSelect<
            ATPTEFMFundTransactionHistoryView,
            Where<ATPTEFMFundTransactionHistoryView.refNbr, Equal<Required<ATPTEFMFundTransaction.refNbr>>>>
            TransactionHistoryView;

        [PXViewName("Fund")]
        public PXSelect<
            ATPTEFMFund,
            Where<ATPTEFMFund.fundCD, Equal<Required<ATPTEFMFundTransaction.fundID>>>>
            Fund;

        [PXViewName("Fund Transaction")]
        public PXSelect<ATPTEFMFundTransaction> FundTransactions;

        public PXSelectReadonly<
            ATPTEFMFundTransaction,
            Where<ATPTEFMFundTransaction.refNbr, Equal<Current<ATPTEFMFundTransaction.refNbr>>>>
            FundTransactionCash;

        [PXImport(typeof(ATPTEFMFundTransaction))]
        [PXViewName("Fund Transaction Details")]
        public PXSelectJoin<
            ATPTEFMFundTransactionDetail,
            LeftJoin<InventoryItem,
                On<InventoryItem.inventoryID, Equal<ATPTEFMFundTransactionDetail.inventoryID>>>,
            Where<ATPTEFMFundTransactionDetail.fundTransactionRefNbr, Equal<Current<ATPTEFMFundTransaction.refNbr>>>>
            FundTransactionDetails;

        [PXImport(typeof(ATPTEFMFundTransaction))]
        [PXCopyPasteHiddenView]
        public PXSelectJoin<
            ATPTEFMFundTransactionReceiptDetail,
            LeftJoin<InventoryItem,
                On<InventoryItem.inventoryID, Equal<ATPTEFMFundTransactionReceiptDetail.inventoryID>>,
            LeftJoin<Vendor,
                On<Vendor.bAccountID, Equal<ATPTEFMFundTransactionReceiptDetail.vendorID>>,
            LeftJoin<BAccount,
                On<BAccount.bAccountID, Equal<Vendor.bAccountID>>,
            LeftJoin<Address,
                On<Address.addressID, Equal<BAccount.defAddressID>>,
            LeftJoin<LocationExtAddress,
                On<LocationExtAddress.locationID, Equal<BAccount.defLocationID>>>>>>>,
            Where<ATPTEFMFundTransactionReceiptDetail.fundTransactionRefNbr, Equal<Current<ATPTEFMFundTransaction.refNbr>>>>
            FundTransactionReceiptLines;

        [PXCopyPasteHiddenView]
        public PXSelectReadonly2<
            ATPTEFMFundTransactionReclassficationReceiptDetail,
            LeftJoin<InventoryItem,
                On<InventoryItem.inventoryID, Equal<ATPTEFMFundTransactionReclassficationReceiptDetail.inventoryID>>>,
            Where<ATPTEFMFundTransactionReclassficationReceiptDetail.fundTransactionRefNbr, Equal<Current<ATPTEFMFundTransaction.refNbr>>>>
            FundTransactionReclassficationReceiptDetail;

        [PXViewName("Setup Approval")]
        public PXSelect<
            ATPTEFMFundTransactionSetupApproval,
            Where<ATPTEFMFundTransactionSetupApproval.fundTransactionType, Equal<Current<ATPTEFMFundTransaction.fundTransactionType>>,
                And<ATPTEFMFundTransactionSetupApproval.isActive, Equal<True>>>>
            SetupApproval;

        [PXViewName("Approval")]
        public EPApprovalAutomation<
            ATPTEFMFundTransaction, ATPTEFMFundTransaction.approved, ATPTEFMFundTransaction.rejected, ATPTEFMFundTransaction.hold, ATPTEFMFundTransactionSetupApproval>
            Approval;

        public PXSelect<
            InventoryItem,
            Where<True, Equal<False>>>
            Items;

        public PXSelect<
            Vendor,
            Where<True, Equal<False>>>
            Vendors;

        public PXSelect<
            Address,
            Where<True, Equal<False>>>
            Addresses;

        public PXSelect<
            LocationExtAddress,
            Where<True, Equal<False>>>
            ExtAddresses;

        public PXSelect<ATPTEFMFundTransactionDetail> ReceiptsForSubmit;

        [PXCopyPasteHiddenView]
        public PXSelectReadonly<ATPTEFMBudget> Budget;

        public PXSelect<ATPTEFMBudgetHistory> HistoryView;

        [PXCopyPasteHiddenView]
        public PXSelectReadonly<ATPTEFMPBudget> ProjectBudget;

        public PXSelect<ATPTEFMProjectBudgetHistory> ProjectHistoryView;
        public PXFilter<ATPTEFMProjectBudgetFilter> PBFilter;

        public PXSelect<
            ATPTEFMProjectBudgetLineSummary,
            Where<ATPTEFMProjectBudgetLineSummary.released, Equal<True>,
                And<ATPTEFMProjectBudgetLineSummary.projectID, Equal<Required<ATPTEFMProjectBudgetLineSummary.projectID>>,
                And<ATPTEFMProjectBudgetLineSummary.projectTaskID, Equal<Required<ATPTEFMProjectBudgetLineSummary.projectTaskID>>,
                And<ATPTEFMProjectBudgetLineSummary.costCodeID, Equal<Required<ATPTEFMProjectBudgetLineSummary.costCodeID>>,
                And<ATPTEFMProjectBudgetLineSummary.finYear, Equal<Required<ATPTEFMProjectBudgetLineSummary.finYear>>,
                And<ATPTEFMProjectBudgetLineSummary.accountGroupID, Equal<Required<ATPTEFMProjectBudgetLineSummary.accountGroupID>>>>>>>>>
            ProjectBudgetSummary;


        [PXViewName("Employee")]
        public PXSelect<
            EPEmployee,
            Where<EPEmployee.bAccountID, Equal<Optional<ATPTEFMFundTransaction.requestedByID>>>>
            EPEmployee;

        //this is required for approval
        public PXSetup<EPEmployee>.Where<EPEmployee.bAccountID.IsEqual<ATPTEFMFundTransaction.requestedByID.FromCurrent>> EPEmployeeSetup;

        public PXSelect<
            EPEmployee,
            Where<EPEmployee.bAccountID, Equal<Current<ATPTEFMFundTransaction.requestedByID>>>>
            RequestorEmployee;

        public PXSelectReadonly<
            EPEmployee,
            Where<EPEmployee.userID, Equal<Current<AccessInfo.userName>>>>
            CurrentEmployee;
        #endregion Views

        #region View Delegates
        public IEnumerable receiptsForSubmit()
        {
            PXResultset<ATPTEFMFundTransactionDetail> requests = PXSelect<
                ATPTEFMFundTransactionDetail,
                Where<ATPTEFMFundTransactionDetail.fundTransactionRefNbr, Equal<Current<ATPTEFMFundTransaction.refNbr>>>>
                .Select(this);

            foreach (ATPTEFMFundTransactionDetail r in requests)
            {
                List<ATPTEFMFundTransactionReceiptDetail> receipts = FundTransactionReceiptLines.Select().RowCast<ATPTEFMFundTransactionReceiptDetail>().Where(w => w.FundTransactionDetailID == r.FundTransactionDetailID).ToList();
                decimal? totalReceipts = receipts.Sum(s => s.NetAmt);
                r.Balance = r.Amount - totalReceipts ?? 0;
                decimal? totalQty = receipts.Sum(s => s.NetQty);
                r.RunningQty = r.Qty - totalQty ?? 0;
            }

            return requests.RowCast<ATPTEFMFundTransactionDetail>();
        }
        /// <remarks>
        /// 2025-10-16: [CBE 2024R2] Fund Transactions - certain transactions cannot be opened (Object Reference error) CASE: 013958 {JLTG}
        /// </remarks>
        public IEnumerable budget()
        {
            #region Variables

            ATPTEFMFundTransaction parent = FundTransactions?.Current;
            parent.IsOverbudget = false;
            parent.HasInitialBudget = false;
            var _Types = new string[] { ATPTEFMFundTransactionTypeAttribute.CashAdvanceValue, ATPTEFMFundTransactionTypeAttribute.ReimbursementValue };
            var _IsCashAdvance = parent?.FundTransactionType == ATPTEFMFundTransactionTypeAttribute.CashAdvanceValue;
            int? ledgerID = FeatureSetup?.Current?.BudgetLedgerID;
            ATPTEFMBudgetLibrary.FinPeriodData fData = ATPTEFMBudgetLibrary.GetFinPeriod(this, parent?.FinPeriodID, FeatureSetup?.Current?.BudgetCalculation);

            bool showbudget = false;

            if (parent.FundTransactionType == ATPTEFMFundTransactionTypeAttribute.CashAdvanceValue)
            {
                ATPTEFMFundTransactionDetail parentDet = PXSelect<
                    ATPTEFMFundTransactionDetail,
                    Where<ATPTEFMFundTransactionDetail.fundTransactionRefNbr, Equal<@P.AsString>,
                        And<ATPTEFMFundTransactionDetail.projectID, Equal<@P.AsInt>,
                        And<ATPTEFMFundTransactionDetail.accountID, IsNotNull,
                        And<ATPTEFMFundTransactionDetail.subID, IsNotNull>>>>>
                    .Select(this, parent.RefNbr, ProjectDefaultAttribute.NonProject());

                showbudget = parentDet != null;
            }
            else
            {
                ATPTEFMFundTransactionReceiptDetail parentDet = PXSelect<
                    ATPTEFMFundTransactionReceiptDetail,
                    Where<ATPTEFMFundTransactionReceiptDetail.fundTransactionRefNbr, Equal<@P.AsString>,
                        And<ATPTEFMFundTransactionReceiptDetail.projectID, Equal<@P.AsInt>,
                        And<ATPTEFMFundTransactionReceiptDetail.accountID, IsNotNull,
                        And<ATPTEFMFundTransactionReceiptDetail.subID, IsNotNull>>>>>
                    .Select(this, parent.RefNbr, ProjectDefaultAttribute.NonProject());

                showbudget = parentDet != null;
            }

            if (HasNull(parent, ledgerID, fData) || !_Types.Contains(parent.FundTransactionType) || !showbudget || parent.BudgetEnabled == false) 
            {
                Budget.AllowSelect = false;
                yield break; 
            }

            List<BudgetParameters> parameterList = new List<BudgetParameters>();
            List<ATPTEFMFADetailSummary> detailList = new List<ATPTEFMFADetailSummary>();
            bool isApproved = parent.Approved ?? false;
            bool isReleased = parent.Status == ATPTEFMFundStatusAttribute.ClosedValue
                           && parent.CashAdvanceStatus == ATPTEFMFundTransactionCashAdvanceStatusAttribute.LiquidatedValue;

            #endregion Variables

            #region Supply Parameters

            #region Request

            PXResultset<ATPTEFMFundTransactionDetail> requests = PXSelect<
                ATPTEFMFundTransactionDetail,
                Where<ATPTEFMFundTransactionDetail.fundTransactionRefNbr, Equal<Current<ATPTEFMFundTransaction.refNbr>>>>
                .Select(this);
            foreach (ATPTEFMFundTransactionDetail item in requests)
            {
                Account account = (Account)PXSelectorAttribute.Select<ATPTEFMFundTransactionDetail.accountID>(FundTransactionDetails.Cache, item);
                detailList.Add(new ATPTEFMFADetailSummary
                {
                    RequestID = item.FundTransactionDetailID,
                    AcctID = item.AccountID,
                    SubID = item.SubID,
                    Qty = item.Qty,
                    UnitCost = item.UnitCost,
                    NetQty = 0,
                    NetAmt = 0,
                    CuryID = account?.CuryID ?? company?.Current?.BaseCuryID
                });
            }

            #endregion Request

            #region Receipt

            IEnumerable<PXResult<ATPTEFMFundTransactionReceiptDetail>> receipt =
                PXSelect<
                    ATPTEFMFundTransactionReceiptDetail,
                    Where<ATPTEFMFundTransactionReceiptDetail.fundTransactionRefNbr, Equal<Current<ATPTEFMFundTransaction.refNbr>>>>
                    .Select(this)
                    .AsEnumerable();
            foreach (var items in receipt.GroupBy(x => new
            {
                ((ATPTEFMFundTransactionReceiptDetail)x).FundTransactionDetailID,
                ((ATPTEFMFundTransactionReceiptDetail)x).AccountID,
                ((ATPTEFMFundTransactionReceiptDetail)x).SubID
            }))
            {
                ATPTEFMFADetailSummary result = detailList
                    .Where(x => x.RequestID == items.Key.FundTransactionDetailID
                             && x.AcctID == items.Key.AccountID
                             && x.SubID == items.Key.SubID)
                    .FirstOrDefault();
                //if (result == null) continue;

                foreach (ATPTEFMFundTransactionReceiptDetail item in items)
                {
                    ATPTEFMFundTransactionReceiptDetail rec = item;

                    if (items.Key.FundTransactionDetailID != null)
                    {
                        result.NetQty += rec.NetQty;
                        result.NetAmt += rec.NetAmt > 0 ? rec.NetAmt : rec.Amount;
                        continue;
                    }

                    Account account = (Account)PXSelectorAttribute.Select<ATPTEFMFundTransactionReceiptDetail.accountID>(FundTransactionReceiptLines.Cache, item);
                    detailList.Add(new ATPTEFMFADetailSummary()
                    {
                        RequestID = null,
                        AcctID = items.Key.AccountID,
                        SubID = items.Key.SubID,
                        Qty = rec.Qty,
                        UnitCost = rec.UnitCost,
                        NetQty = 0,
                        NetAmt = rec.NetAmt > 0 ? rec.NetAmt : rec.Amount,
                        CuryID = account?.CuryID ?? company?.Current?.BaseCuryID
                    });
                }
            }

            #endregion Receipt

            foreach (ATPTEFMFADetailSummary detail in detailList)
            {
                //if (detail.RequestID != null && parent?.Status == ATPTEFMCashAdvanceStatusAttribute.ClosedValue) netAmt = detail.NetAmt;
                //else netAmt = detail.Qty * detail.UnitCost;

                parameterList.Add(new BudgetParameters()
                {
                    LedgerID = ledgerID,
                    BranchID = parent.BranchID,
                    RefNbr = parent.RefNbr,
                    CuryID = detail.CuryID,
                    AccountID = detail.AcctID,
                    SubID = detail.SubID,
                    FinYear = fData.FinYear,
                    FromFinPeriodID = fData.StartPeriod,
                    ToFinPeriodID = fData.EndPeriod,
                    FinPeriodID = parent.FinPeriodID,
                    //Amount = parent?.Status == ATPTEFMFundStatusAttribute.CancelledValue ? 0 : netAmt ?? 0,
                    Amount = parent?.Status == ATPTEFMFundStatusAttribute.CancelledValue ? 0 : detail.Qty * detail.UnitCost,
                    Approved = parent.Approved ?? false,
                    Released = parent.Approved ?? false,
                    OriginType = OriginTypes.FundTransaction
                });
            }

            #endregion Supply Parameters

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

                if (item.BudgetAmt >= 0) continue;

                bool isError = FeatureSetup?.Current?.BudgetValidation == RQRequestClassBudget.Error
                    && (new PXEntryStatus[] {
                        FundTransactions.Cache.GetStatus(FundTransactions.Current),
                        FundTransactionDetails.Cache.GetStatus(FundTransactionDetails.Current),
                        FundTransactionReceiptLines.Cache.GetStatus(FundTransactionReceiptLines.Current)
                    }.Any(x => x != PXEntryStatus.Notchanged));
                bool isWarning = FeatureSetup?.Current?.BudgetValidation == RQRequestClassBudget.Warning;

                if (isError || isWarning)
                {
                    this.Budget.Cache.RaiseExceptionHandling<ATPTEFMBudget.budgetAmt>(item, item.BudgetAmt,
                            ATPTEFMHelper.GetPropertyException(item, Messages.ATPTEFMMessages.OverbudgetWarning,
                                isError ? PXErrorLevel.Error : PXErrorLevel.Warning));
                }
            }
        }
        public virtual IEnumerable projectBudget()
        {
            #region Variables
            ATPTEFMFundTransaction parent = this.FundTransactions.Current;

            if (!(parent?.ProjectBudgetEnabled ?? false))
            {
                ProjectBudget.AllowSelect = false;
                yield break;
            }

            var _Types = new string[] { ATPTEFMFundTransactionTypeAttribute.CashAdvanceValue, ATPTEFMFundTransactionTypeAttribute.ReimbursementValue };
            var _IsCashAdvance = parent?.FundTransactionType == ATPTEFMFundTransactionTypeAttribute.CashAdvanceValue;
            ATPTEFMBudgetLibrary.FinPeriodData fData = ATPTEFMBudgetLibrary.GetFinPeriod(this, parent?.FinPeriodID ?? finperiod?.Current?.FinPeriodID, FeatureSetup?.Current?.ProjectBudgetCalculation);

            if (HasNull(parent, fData)) yield break;

            List<ProjectBudgetParameters> parameterList = new List<ProjectBudgetParameters>();
            List<ATPTEFMFADetailSummary> detailList = new List<ATPTEFMFADetailSummary>();
            bool isApproved = parent != null && (parent.Approved ?? false);
            bool isReleased = parent.Status == ATPTEFMFundStatusAttribute.ClosedValue
                           && parent.CashAdvanceStatus == ATPTEFMFundTransactionCashAdvanceStatusAttribute.LiquidatedValue;

            #endregion Variables

            #region Supply Parameters

            #region Request

            PXResultset<ATPTEFMFundTransactionDetail> requests = PXSelect<
                ATPTEFMFundTransactionDetail,
                Where<ATPTEFMFundTransactionDetail.fundTransactionRefNbr, Equal<Current<ATPTEFMFundTransaction.refNbr>>>>
                .Select(this);
            foreach (ATPTEFMFundTransactionDetail item in requests)
            {
                if (HasNull(item.ProjectID, item.ProjectTaskID, item.CostCodeID, item.AccountGroup)) continue;
                Account account = (Account)PXSelectorAttribute.Select<ATPTEFMFundTransactionDetail.accountID>(FundTransactionDetails.Cache, item);
                detailList.Add(new ATPTEFMFADetailSummary
                {
                    RequestID = item.FundTransactionDetailID,
                    ProjectID = item.ProjectID,
                    ProjectTaskID = item.ProjectTaskID,
                    InventoryID = item.InventoryID,
                    CostCodeID = item.CostCodeID,
                    AccountGroupID = item.AccountGroup,
                    Qty = item.Qty,
                    UnitCost = item.UnitCost,
                    NetQty = 0,
                    NetAmt = 0,
                    CuryID = account?.CuryID ?? company?.Current?.BaseCuryID
                });
            }

            #endregion Request

            #region Receipt

            IEnumerable<PXResult<ATPTEFMFundTransactionReceiptDetail>> receipt =
                PXSelect<
                    ATPTEFMFundTransactionReceiptDetail,
                    Where<ATPTEFMFundTransactionReceiptDetail.fundTransactionRefNbr, Equal<Current<ATPTEFMFundTransaction.refNbr>>>>
                    .Select(this)
                    .AsEnumerable();
            foreach (var items in receipt.GroupBy(x => new
            {
                ((ATPTEFMFundTransactionReceiptDetail)x).FundTransactionDetailID,
                ((ATPTEFMFundTransactionReceiptDetail)x).ProjectID,
                ((ATPTEFMFundTransactionReceiptDetail)x).ProjectTaskID,
                ((ATPTEFMFundTransactionReceiptDetail)x).CostCodeID,
                ((ATPTEFMFundTransactionReceiptDetail)x).AccountGroup
            }))
            {
                ATPTEFMFADetailSummary result = detailList
                    .Where(x => x.RequestID == items.Key.FundTransactionDetailID
                             && x.ProjectID == items.Key.ProjectID
                             && x.ProjectTaskID == items.Key.ProjectTaskID
                             && x.CostCodeID == items.Key.CostCodeID
                             && x.AccountGroupID == items.Key.AccountGroup)
                    .FirstOrDefault();

                foreach (ATPTEFMFundTransactionReceiptDetail item in items)
                {
                    ATPTEFMFundTransactionReceiptDetail rec = item;

                    if (HasNull(item.ProjectID, item.ProjectTaskID, item.CostCodeID, item.AccountGroup)) continue;

                    if (items.Key.FundTransactionDetailID != null && result != null)
                    {
                        result.NetQty += rec.NetQty;
                        result.NetAmt += rec.NetAmt > 0 ? rec.NetAmt : rec.Amount;
                        continue;
                    }

                    Account account = (Account)PXSelectorAttribute.Select<ATPTEFMFundTransactionReceiptDetail.accountID>(FundTransactionReceiptLines.Cache, item);
                    detailList.Add(new ATPTEFMFADetailSummary()
                    {
                        RequestID = null,
                        ProjectID = items.Key.ProjectID,
                        ProjectTaskID = items.Key.ProjectTaskID,
                        CostCodeID = items.Key.CostCodeID,
                        AccountGroupID = items.Key.AccountGroup,
                        Qty = rec.Qty,
                        UnitCost = rec.UnitCost,
                        NetQty = 0,
                        NetAmt = rec.NetAmt > 0 ? rec.NetAmt : rec.Amount,
                        CuryID = account?.CuryID ?? company?.Current?.BaseCuryID
                    });
                }
            }

            #endregion Receipt

            foreach (ATPTEFMFADetailSummary detail in detailList)
            {
                //decimal? netAmt = detail.NetAmt;
                //if (detail.RequestID != null)
                //{
                //    netAmt = ((detail.Qty - detail.NetQty) * detail.UnitCost) + detail.NetAmt;
                //}

                parameterList.Add(new ProjectBudgetParameters()
                {
                    RefNbr = parent.RefNbr,
                    CuryID = detail.CuryID,
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
                    //Amount = parent?.Status == ATPTEFMCashAdvanceStatusAttribute.CancelledValue ? 0 : parent?.Status == ATPTEFMCashAdvanceStatusAttribute.ClosedValue ? detail.NetAmt : detail.Qty * detail.UnitCost,
                    Amount = parent?.Status == ATPTEFMCashAdvanceStatusAttribute.CancelledValue ? 0 : detail.Qty * detail.UnitCost,
                    Approved = parent.Approved ?? false,
                    Released = parent.Approved ?? false,
                    OriginType = OriginTypes.FundTransaction
                });
            }

            #endregion Supply Parameters

            //ProjectBudget.AllowSelect = ShowBudget() && parameterList.Any();
            ProjectBudget.AllowSelect = parameterList.Any();
            if (!parameterList.Any()) yield break;

            foreach (ATPTEFMPBudget item in GenerateProjectBudget(this, parameterList))
            {
                yield return item;

                bool isError = FeatureSetup?.Current?.ProjectBudgetValidation == RQRequestClassBudget.Error
                    && (new PXEntryStatus[] {
                        FundTransactions.Cache.GetStatus(FundTransactions.Current),
                        FundTransactionDetails.Cache.GetStatus(FundTransactionDetails.Current),
                        FundTransactionReceiptLines.Cache.GetStatus(FundTransactionReceiptLines.Current)
                    }.Any(x => x != PXEntryStatus.Notchanged));
                bool isWarning = FeatureSetup?.Current?.ProjectBudgetValidation == RQRequestClassBudget.Warning;

                if (isError || isWarning)
                {
                    ATPTEFMProjectBudgetLineSummary PBSummary = ProjectBudgetSummary.Select(item.ProjectID, item.ProjectTaskID, item.CostCodeID, FundTransactions.Current.Date.Value.Year.ToString(), item.AccountGroupID);

                    if (PBSummary == null)
                    {
                        this.ProjectBudget.Cache.RaiseExceptionHandling<ATPTEFMPBudget.projectID>(item, item.ProjectID,
                            ATPTEFMHelper.GetPropertyException(item, Messages.ATPTEFMMessages.NotInProjectBudget,
                                isError ? PXErrorLevel.RowError : PXErrorLevel.Warning));
                    }
                    else if (item.BudgetAmt < 0)
                    {
                        this.ProjectBudget.Cache.RaiseExceptionHandling<ATPTEFMPBudget.budgetAmt>(item, item.BudgetAmt,
                            ATPTEFMHelper.GetPropertyException(item, Messages.ATPTEFMMessages.OverbudgetWarning,
                               isError ? PXErrorLevel.Error : PXErrorLevel.Warning));
                    }
                }
            }
        }

        #endregion View Delegates

        #region Overrides
        /// <remarks>
        /// 2024-09-23 : " Error in Fund Transaction can't release cash." - CASE: 007641 {JLG}
        /// 2025-09-10 : Update step value updated to also include request type : RFS
        /// </remarks>
        public override void Persist()
        {
            ATPTEFMFundTransaction ft = FundTransactions.Current;
            //changes and validations before saving
            #region Budget Validation

            bool validateBudget = ft.BudgetEnabled ?? false;
            bool BudgetValidate = ShowBudget() && (FeatureSetup?.Current?.BudgetValidation ?? RQRequestClassBudget.None) == RQRequestClassBudget.Error;

            bool validatePBudget = ft.ProjectBudgetEnabled ?? false;
            bool PBudgetValidation = (FeatureSetup?.Current?.ProjectBudgetValidation ?? RQRequestClassBudget.None) == RQRequestClassBudget.Error;

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
            if (PBudgetValidation && validatePBudget)
            {
                using (PXTransactionScope ts = new PXTransactionScope())
                {
                    bool isOverbudget = ProjectBudget?.Select()?.RowCast<ATPTEFMPBudget>()?.ToList()?.Any(x => (x.BudgetAmt ?? 0m) < 0) ?? false;

                    foreach (ATPTEFMPBudget row in ProjectBudget.Select())
                    {
                        if (HasNull(row.ProjectID, row.ProjectTaskID, row.CostCodeID, row.AccountGroupID)) continue;

                        ATPTEFMProjectBudgetLineSummary PBSummary = PXSelect<
                            ATPTEFMProjectBudgetLineSummary,
                            Where<ATPTEFMProjectBudgetLineSummary.released, Equal<True>,
                                And<ATPTEFMProjectBudgetLineSummary.projectID, Equal<Required<ATPTEFMProjectBudgetLineSummary.projectID>>,
                                And<ATPTEFMProjectBudgetLineSummary.projectTaskID, Equal<Required<ATPTEFMProjectBudgetLineSummary.projectTaskID>>,
                                And<ATPTEFMProjectBudgetLineSummary.costCodeID, Equal<Required<ATPTEFMProjectBudgetLineSummary.costCodeID>>,
                                And<ATPTEFMProjectBudgetLineSummary.finYear, Equal<Required<ATPTEFMProjectBudgetLineSummary.finYear>>,
                                And<ATPTEFMProjectBudgetLineSummary.accountGroupID, Equal<Required<ATPTEFMProjectBudgetLineSummary.accountGroupID>>>>>>>>>
                            .Select(this, row.ProjectID, row.ProjectTaskID, row.CostCodeID, FundTransactions.Current.Date.Value.Year.ToString(), row.AccountGroupID);

                        if (PBSummary == null)
                            throw new PXRowPersistedException(typeof(ATPTEFMPBudget).Name, ts, Messages.ATPTEFMMessages.NotInProjectBudget);
                    }

                    if (isOverbudget)
                        throw new PXRowPersistedException(typeof(ATPTEFMPBudget).Name, ts, ATPTEFMMessages.AmountRequestedIsGreaterThanRemainingBudget);
                    ProjectBudget.Cache.Persist(PXDBOperation.Insert);
                    ProjectBudget.Cache.Persist(PXDBOperation.Update);

                    ts.Complete(this);
                }
                ProjectBudget.Cache.Persisted(false);
            }

            #endregion Budget Validation

            #region Budget Requirements

            List<ATPTEFMBudget> BudgetList = new List<ATPTEFMBudget>();
            List<ATPTEFMPBudget> PBudgetList = new List<ATPTEFMPBudget>();

            ATPTEFMBudgetEntry graph = PXGraph.CreateInstance<ATPTEFMBudgetEntry>();
            bool isDeleted = FundTransactions.Cache.Deleted.Any_();
            ATPTEFMFundTransaction curRecord = isDeleted ? FundTransactions.Cache.Deleted.FirstOrDefault_() as ATPTEFMFundTransaction : FundTransactions.Current;
            bool isCancelled = curRecord == null ? false : curRecord.Status == ATPTEFMFundStatusAttribute.Rejected ? true : false;

            List<ATPTEFMFundTransactionReceiptDetail> curLines = new List<ATPTEFMFundTransactionReceiptDetail>();
            foreach (ATPTEFMFundTransactionReceiptDetail item in FundTransactionReceiptLines.Cache.Deleted) { curLines.Add(item); }
            List<ATPTEFMFundTransactionDetail> curReqLines = new List<ATPTEFMFundTransactionDetail>();
            if (curRecord?.FundTransactionType == ATPTEFMFundTransactionTypeAttribute.CashAdvanceValue)
            {
                foreach (ATPTEFMFundTransactionDetail item in FundTransactionDetails.Cache.Deleted) { curReqLines.Add(item); }
            }

            #endregion Budget Requirements

            UpdateApprovalAmount();

            bool hasChanges = FundTransactions.Cache.GetStatus(FundTransactions.Current) == PXEntryStatus.Updated;

            #region Update Fund Transaction History
            if (hasChanges)
            {
                UpdateFundTransactionHistory();
            }
            #endregion
            //saving
            #region Update Step Value
            if (FundTransactions.Current.Step != ATPTEFMFundTransactionStepAttribute.LiquidatedReceiptValue)
            {
                var receiptsWithoutErNbr = FundTransactionReceiptLines.Select().RowCast<ATPTEFMFundTransactionReceiptDetail>().Where(w => string.IsNullOrEmpty(w.ExpenseReceiptRefNbr)).Count();
                var receiptsWithErNbr = FundTransactionReceiptLines.Select().RowCast<ATPTEFMFundTransactionReceiptDetail>().Where(w => !string.IsNullOrEmpty(w.ExpenseReceiptRefNbr)).Count();
                if (receiptsWithoutErNbr > 0 && receiptsWithErNbr > 0)
                {
                    if (FundTransactions.Current.Step != ATPTEFMFundTransactionStepAttribute.DefaultValue)
                    {
                        FundTransactions.Current.Step = ATPTEFMFundTransactionStepAttribute.DefaultValue;
                        FundTransactions.UpdateCurrent();
                    }
                }
                if (receiptsWithoutErNbr == 0 && receiptsWithErNbr > 0)
                {
                    if (FundTransactions.Current.Step != ATPTEFMFundTransactionStepAttribute.SubmitReceiptValue)
                    {
                        FundTransactions.Current.Step = ATPTEFMFundTransactionStepAttribute.SubmitReceiptValue;
                        FundTransactions.UpdateCurrent();
                    }
                }
            }
            #endregion

            base.Persist();
            //changes after saving
            //Update or Updatecurrent for the Fund Transaction graph should be placed before saving
            #region Transaction History (Updates)

            #region Conditional variables
            if (FundTransactions.Current != null && FundTransactions.Current.LastModifiedByScreenID != "ATPT9201")
            {
                bool isFundLiquidated = ATPTEFMFundTransactionHelper.IsFundLiquidated(FundTransactions.Current);
                bool isFundUnliquidated = ATPTEFMFundTransactionHelper.IsFundUnliquidated(FundTransactions.Current);
                bool isFundRequest = ATPTEFMFundTransactionHelper.IsFundRequest(FundTransactions.Current);
                bool isSubmitReceipt = ATPTEFMFundTransactionHelper.IsSubmitReceipt(FundTransactions.Current);
                bool isFundReimbursement = ATPTEFMFundTransactionHelper.IsFundReimbursement(FundTransactions.Current);
                bool isFundStatusClosed = ATPTEFMFundTransactionHelper.IsFundStatusClosed(FundTransactions.Current);
                bool isFundTransactionRecordExist = IsFundTransactionRecordExist();
                #endregion

                #region Execute this code if has changes
                if (isFundTransactionRecordExist && hasChanges)
                {
                    Fund.Current = Fund.Select(FundTransactions.Current.FundID);
                    if (Fund.Current != null)
                    {
                        #region Running Balance For Release Cash (Reimbursement Type)
                        if (isFundReimbursement && isFundStatusClosed)
                        {
                            ReleaseCashReimbursementTypeRunninBalance();
                        }
                        #endregion
                    }
                }
                #endregion

                #endregion

                #region Transaction History (Insertion of Fund Request Type)
                if (!isFundTransactionRecordExist)
                {
                    if (isFundRequest)
                    {
                        CreateTransactionHistoryRecord();
                        base.Persist();
                    }
                }
                #endregion
            }

            #region BudgetHistory

            if (isDeleted || isCancelled)
            {
                foreach (ATPTEFMFundTransactionReceiptDetail item in curLines)
                {
                    var row = new ATPTEFMBudget();
                    row.AcctID = item.AccountID;
                    row.SubID = item.SubID;
                    row.RefNbr = item.FundTransactionRefNbr;
                    row.Origin = (int)OriginTypes.FundTransaction;
                    BudgetList.Add(row);

                    var pRow = new ATPTEFMPBudget();
                    pRow.ProjectID = item.ProjectID;
                    pRow.ProjectTaskID = item.ProjectTaskID;
                    pRow.CostCodeID = item.CostCodeID;
                    pRow.RefNbr = item.FundTransactionRefNbr;
                    pRow.Origin = (int)OriginTypes.FundTransaction;
                    PBudgetList.Add(pRow);
                }
                foreach (ATPTEFMFundTransactionDetail item in curReqLines)
                {
                    var row = new ATPTEFMBudget();
                    row.AcctID = item.AccountID;
                    row.SubID = item.SubID;
                    row.RefNbr = item.FundTransactionRefNbr;
                    row.Origin = (int)OriginTypes.FundTransaction;
                    BudgetList.Add(row);

                    var pRow = new ATPTEFMPBudget();
                    pRow.ProjectID = item.ProjectID;
                    pRow.ProjectTaskID = item.ProjectTaskID;
                    pRow.CostCodeID = item.CostCodeID;
                    pRow.RefNbr = item.FundTransactionRefNbr;
                    pRow.Origin = (int)OriginTypes.FundTransaction;
                    PBudgetList.Add(pRow);
                }
                graph.DeleteBudgetHistory(BudgetList);
                graph.DeletePBudgetHistory(PBudgetList);
            }
            else
            {
                if ((curRecord.BudgetEnabled ?? false) && curRecord != null)
                {
                    BudgetList.Add(new ATPTEFMBudget() { RefNbr = curRecord.RefNbr, Origin = (int)OriginTypes.FundTransaction });
                    foreach (ATPTEFMBudget item in Budget.Select())
                    {
                        var row = item;
                        row.IsApproved = curRecord.Approved ?? false;
                        BudgetList.Add(row);
                    }
                    graph.AddBudgetHistory(BudgetList);
                    Budget.Select();
                }
                if ((curRecord.ProjectBudgetEnabled ?? false) && curRecord != null)
                {
                    PBudgetList.Add(new ATPTEFMPBudget() { RefNbr = curRecord?.RefNbr, Origin = (int)OriginTypes.FundTransaction });
                    foreach (ATPTEFMPBudget item in ProjectBudget.Select())
                    {
                        var row = item;
                        row.IsApproved = curRecord?.Approved ?? false;
                        PBudgetList.Add(row);
                    }
                    graph.AddPBudgetHistory(PBudgetList);
                    ProjectBudget.Select();
                }
            }

            #endregion BudgetHistory
        }
        #endregion

        #region Methods
        private void PBudgetValidationRaiseErrorForLiquidationAmtGreaterThanRequestAmt(bool PBudgetEnabled)
        {
            #region Raise Error if Receipt > Request grouped by Project, Task, CostCode, Account Group
            if (PBudgetEnabled)
            {
                var reqDetGrouped = FundTransactionDetails.Select().RowCast<ATPTEFMFundTransactionDetail>().GroupBy(x => new { x.ProjectID, x.ProjectTaskID, x.CostCodeID, x.AccountGroup }).Select(x => new { ProjectID = x.Key.ProjectID, TaskID = x.Key.ProjectTaskID, CostCodeID = x.Key.CostCodeID, AccountGroup = x.Key.AccountGroup, CuryAmount = x.Sum(y => y.Amount) });

                foreach (var reqDet in reqDetGrouped)
                {
                    if (HasNull(reqDet.ProjectID, reqDet.TaskID, reqDet.CostCodeID, reqDet.AccountGroup)) continue;
                    decimal totalNetAmtBy = 0;
                    foreach (ATPTEFMFundTransactionReceiptDetail recDet in FundTransactionReceiptLines.Select())
                    {
                        if (recDet.ProjectID == reqDet.ProjectID && recDet.ProjectTaskID == reqDet.TaskID && recDet.CostCodeID == reqDet.CostCodeID && recDet.AccountGroup == reqDet.AccountGroup)
                        {
                            totalNetAmtBy += recDet.NetAmt ?? 0m;

                            if (totalNetAmtBy > reqDet.CuryAmount)
                            {
                                PMProject proj = PMProject.PK.Find(this, recDet.ProjectID);
                                PMTask task = PMTask.PK.Find(this, recDet.ProjectID, recDet.ProjectTaskID);
                                PMCostCode costcode = PMCostCode.PK.Find(this, recDet.CostCodeID);
                                PMAccountGroup accgroup = PMAccountGroup.PK.Find(this, recDet.AccountGroup);

                                FundTransactionReceiptLines.Cache.ClearFieldErrors<ATPTEFMFundTransactionReceiptDetail.netAmt>(recDet);
                                FundTransactionReceiptLines.Cache.RaiseExceptionHandling<ATPTEFMFundTransactionReceiptDetail.netAmt>(recDet, recDet?.NetAmt,
                                            ATPTEFMHelper.GetPropertyException(recDet, ATPTEFMMessages.MessagesWithParameters.PBudgetReceiptAmtGreaterThanRequestAmt(proj?.ContractCD, task?.TaskCD, costcode?.CostCodeCD, accgroup?.GroupCD), PXErrorLevel.Error));
                            }
                        }
                    }
                }
            }
            #endregion
        }
        private void PBudgetValidationRaiseErrorForRequestAmountGreaterThanRemainingBudget(ATPTEFMFundTransactionDetail row, ATPTEFMFundTransactionReceiptDetail receiptRow, bool PBudgetEnabled)
        {
            if (PBudgetEnabled && FeatureSetup.Current.ProjectBudgetValidation == RQRequestClassBudget.Error)
            {
                foreach (ATPTEFMPBudget result in ProjectBudget.Select())
                {
                    if (HasNull(result.ProjectID, result.ProjectTaskID, result.CostCodeID, result.AccountGroupID)) continue;

                    if (row != null)
                    {
                        if (row.ProjectID == result.ProjectID && row.ProjectTaskID == result.ProjectTaskID && row.CostCodeID == result.CostCodeID && row.AccountGroup == result.AccountGroupID)
                        {
                            if (result.BudgetAmt < 0 && row.Amount > 0) FundTransactionDetails.Cache.RaiseExceptionHandling<ATPTEFMFundTransactionDetail.amount>(row, row?.Amount, ATPTEFMHelper.GetPropertyException(row, ATPTEFMMessages.AmountRequestedIsGreaterThanRemainingBudget, PXErrorLevel.Error));
                        }
                    }
                    else if (receiptRow != null)
                    {
                        if (receiptRow.ProjectID == result.ProjectID && receiptRow.ProjectTaskID == result.ProjectTaskID && receiptRow.CostCodeID == result.CostCodeID && receiptRow.AccountGroup == result.AccountGroupID)
                        {
                            if (result.BudgetAmt < 0 && receiptRow.Amount > 0) FundTransactionReceiptLines.Cache.RaiseExceptionHandling<ATPTEFMFundTransactionReceiptDetail.amount>(row, row?.Amount, ATPTEFMHelper.GetPropertyException(row, ATPTEFMMessages.AmountRequestedIsGreaterThanRemainingBudget, PXErrorLevel.Error));
                        }
                    }
                }
            }
        }
        public virtual bool DuplicateERRefNbr(ATPTEFMFundTransactionReceiptDetail row, string checkRefNbr)
        {
            return false;
        }
        private void BudgetValidationRaiseErrorForRequestAmountGreaterThanRemainingBudget(ATPTEFMFundTransactionDetail row, ATPTEFMFundTransactionReceiptDetail receiptRow, bool BudgetEnabled)
        {
            if (BudgetEnabled)
            {
                foreach (ATPTEFMBudget result in Budget.Select())
                {
                    if (row != null)
                    {
                        if (row.AccountID == result.AcctID && row.SubID == result.SubID)
                        {
                            if (result.BudgetAmt < 0 && row.Amount > 0)
                            {
                                if (FeatureSetup.Current.BudgetValidation == RQRequestClassBudget.Error)
                                    FundTransactionDetails.Cache.RaiseExceptionHandling<ATPTEFMFundTransactionDetail.amount>(row, row?.Amount, ATPTEFMHelper.GetPropertyException(row, ATPTEFMMessages.AmountRequestedIsGreaterThanRemainingBudget, PXErrorLevel.Error));
                                else if (FeatureSetup.Current.BudgetValidation == RQRequestClassBudget.Warning)
                                    FundTransactionDetails.Cache.RaiseExceptionHandling<ATPTEFMFundTransactionDetail.amount>(row, row?.Amount, ATPTEFMHelper.GetPropertyException(row, ATPTEFMMessages.AmountRequestedIsGreaterThanRemainingBudget, PXErrorLevel.Warning));
                            }
                        }
                    }
                    else if (receiptRow != null)
                    {
                        if (receiptRow.AccountID == result.AcctID && receiptRow.SubID == result.SubID)
                        {
                            if (result.BudgetAmt < 0 && receiptRow.Amount > 0)
                            {
                                if (FeatureSetup.Current.BudgetValidation == RQRequestClassBudget.Error)
                                    FundTransactionReceiptLines.Cache.RaiseExceptionHandling<ATPTEFMFundTransactionReceiptDetail.amount>(receiptRow, receiptRow?.Amount, ATPTEFMHelper.GetPropertyException(row, ATPTEFMMessages.AmountRequestedIsGreaterThanRemainingBudget, PXErrorLevel.Error));
                                else if (FeatureSetup.Current.BudgetValidation == RQRequestClassBudget.Warning)
                                    FundTransactionReceiptLines.Cache.RaiseExceptionHandling<ATPTEFMFundTransactionReceiptDetail.amount>(receiptRow, receiptRow?.Amount, ATPTEFMHelper.GetPropertyException(row, ATPTEFMMessages.AmountRequestedIsGreaterThanRemainingBudget, PXErrorLevel.Warning));
                            }
                        }
                    }
                }
            }
        }
        private void BudgetValidationRaiseErrorForLiquidationGreaterThanRequest(bool BudgetEnabled)
        {
            #region Raise Error if Receipt > Request grouped by Account and Sub Account
            if (BudgetEnabled)
            {
                var reqDetGrouped = FundTransactionDetails.Select().RowCast<ATPTEFMFundTransactionDetail>().GroupBy(x => new { x.AccountID, x.SubID }).Select(x => new { AccountID = x.Key.AccountID, SubID = x.Key.SubID, Amount = x.Sum(y => y.Amount) });

                foreach (var reqDet in reqDetGrouped)
                {
                    decimal totalNetAmtByBudgetAccounts = 0;
                    foreach (ATPTEFMFundTransactionReceiptDetail recDet in FundTransactionReceiptLines.Select())
                    {
                        if (recDet.AccountID == reqDet.AccountID && recDet.SubID == reqDet.SubID)
                        {
                            totalNetAmtByBudgetAccounts += recDet.NetAmt ?? 0m;

                            if (totalNetAmtByBudgetAccounts > reqDet.Amount)
                            {
                                Account acc = PXSelect<
                                    Account,
                                    Where<Account.accountID, Equal<Required<Account.accountID>>>>
                                    .Select(this, recDet.AccountID);
                                Sub subAcc = PXSelect<
                                    Sub,
                                    Where<Sub.subID, Equal<Required<Sub.subID>>>>
                                    .Select(this, recDet.SubID);

                                FundTransactionReceiptLines.Cache.ClearFieldErrors<ATPTEFMFundTransactionReceiptDetail.netAmt>(recDet);
                                FundTransactionReceiptLines.Cache.RaiseExceptionHandling<ATPTEFMFundTransactionReceiptDetail.netAmt>(recDet, recDet?.NetAmt,
                                            ATPTEFMHelper.GetPropertyException(recDet, ATPTEFMMessages.MessagesWithParameters.ReceiptAmtGreaterThanRequestAmt(acc?.AccountCD, subAcc?.SubCD), PXErrorLevel.Error));
                            }
                        }
                    }
                }
            }
            #endregion
        }
        public virtual EPExpenseClaimDetails AddAtcCode(EPExpenseClaimDetails row, ATPTEFMFundTransactionReceiptDetail receipt) { return row; }
        private void ReleaseCashReimbursementTypeRunninBalance()
        {
            ATPTEFMFundTransaction fundTransaction = FundTransactions.Current;

            #region Update Running Balance    

            #region Select all records for current fund
            var getTransactionHistory =
            PXSelect<
                ATPTEFMFundTransactionHistoryView,
                Where<ATPTEFMFundTransactionHistoryView.fundRefNbr, Equal<Required<ATPTEFMFundTransactionHistoryView.fundRefNbr>>>,
                OrderBy<
                    Asc<ATPTEFMFundTransactionHistoryView.sortNbr>>>
                .Select(this, FundTransactions.Current.FundID);

            var getResult = ATPTEFMFundHistoryPaginationHelper.GetRecordPosition(getTransactionHistory.RowCast<ATPTEFMFundTransactionHistoryView>(), fundTransaction.RefNbr);
            int totalRows = getResult.TotalRows;
            #endregion

            #region Select the previous balance of this transaction
            ATPTEFMFundTransactionHistoryView getPreviousBalance =
            PXSelect<
                ATPTEFMFundTransactionHistoryView,
                Where<ATPTEFMFundTransactionHistoryView.refNbr, Equal<Required<ATPTEFMFundTransactionHistoryView.refNbr>>>>
                .Select(this, FundTransactions.Current.RefNbr);
            #endregion

            #region Get the last receipt of this transaction for starting index
            ATPTEFMFundTransactionReceiptDetail lastRow = FundTransactionReceiptLines.Select().LastOrDefault();
            var startIndex = getTransactionHistory.RowCast<ATPTEFMFundTransactionHistoryView>().FindIndex(x => x.RefNbr == lastRow.ExpenseReceiptRefNbr);
            #endregion

            #region Default running balance
            decimal? runningBalance = getPreviousBalance.CuryBalanceAmt;
            #endregion

            #region Running Balance Calculations
            foreach (ATPTEFMFundTransactionHistoryView tran in PXSelect<
                                                                    ATPTEFMFundTransactionHistoryView,
                                                                    Where<ATPTEFMFundTransactionHistoryView.fundRefNbr, Equal<Required<ATPTEFMFundTransactionHistoryView.fundRefNbr>>>,
                                                                    OrderBy<
                                                                        Asc<ATPTEFMFundTransactionHistoryView.sortNbr>>>
                                                                    .SelectWindowed(this, startIndex, totalRows, FundTransactions.Current.FundID))
            {
                TransactionHistoryView.Current = TransactionHistoryView.Select(tran.RefNbr);

                if (TransactionHistoryView.Current != null)
                {
                    #region Fund Request
                    if (tran.TransactionType.Equals(ATPTEFMTransactionHistoryView.transactionType.FundRequest))
                    {
                        ATPTEFMFundTransaction currentFundTransaction = PXSelect<
                            ATPTEFMFundTransaction,
                            Where<ATPTEFMFundTransaction.refNbr, Equal<Required<ATPTEFMFundTransaction.refNbr>>>>
                            .Select(this, tran.RefNbr);

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

                        TransactionHistoryView.Current.CuryBalanceAmt = runningBalance;
                        TransactionHistoryView.UpdateCurrent();
                        TransactionHistoryView.Cache.Persist(PXDBOperation.Update);
                        continue;
                    }
                    #endregion

                    #region Fund Reimbursement
                    if (tran.TransactionType.Equals(ATPTEFMTransactionHistoryView.transactionType.FundReimbursment))
                    {

                        if (tran.RefNbr == FundTransactions.Current.RefNbr)
                        {
                            continue;
                        }

                        ATPTEFMFundTransaction currentFundTransaction = PXSelect<
                            ATPTEFMFundTransaction,
                            Where<ATPTEFMFundTransaction.refNbr, Equal<Required<ATPTEFMFundTransaction.refNbr>>>>
                            .Select(this, tran.RefNbr);

                        if (currentFundTransaction != null)
                        {
                            runningBalance -= currentFundTransaction.ActualSpentAmount;
                            runningBalance += currentFundTransaction.TotalWhtAmount;
                        }

                        TransactionHistoryView.Current.CuryBalanceAmt = runningBalance;
                        TransactionHistoryView.UpdateCurrent();
                        TransactionHistoryView.Cache.Persist(PXDBOperation.Update);
                        continue;
                    }
                    #endregion

                    #region Replenishment
                    if (tran.TransactionType.Equals(ATPTEFMTransactionHistoryView.transactionType.Replenishment))
                    {
                        runningBalance += TransactionHistoryView.Current.CuryCheckAmt ?? decimal.Zero;
                        TransactionHistoryView.Current.CuryBalanceAmt = runningBalance;
                        TransactionHistoryView.UpdateCurrent();
                        TransactionHistoryView.Cache.Persist(PXDBOperation.Update);
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
                            .Select(this, tran.RefNbr);

                        if (lastRow.ExpenseReceiptRefNbr == tran.RefNbr)
                        {
                            continue;
                        }

                        if (receipt != null)
                        {
                            decimal? fundBalanceAmount = runningBalance;
                            TransactionHistoryView.Current = TransactionHistoryView.Select(receipt.FundTransactionRefNbr);
                            if (TransactionHistoryView.Current != null)
                            {
                                fundBalanceAmount = TransactionHistoryView.Current.CuryBalanceAmt;
                            }

                            TransactionHistoryView.Current = TransactionHistoryView.Select(tran.RefNbr);
                            TransactionHistoryView.Current.CuryBalanceAmt = fundBalanceAmount;
                            TransactionHistoryView.UpdateCurrent();
                            TransactionHistoryView.Cache.Persist(PXDBOperation.Update);
                            continue;
                        }
                    }
                    #endregion

                    #region IncreaseFund
                    if (tran.TransactionType.Equals(ATPTEFMFundTransactionTypeAttribute.IncreaseFundValue))
                    {
                        if (tran.Status == APDocStatus.Closed)
                            runningBalance += tran.CuryCheckAmt;

                        TransactionHistoryView.Current.CuryBalanceAmt = runningBalance;
                        TransactionHistoryView.UpdateCurrent();
                        continue;
                    }
                    #endregion

                    #region DecreaseFund
                    if (tran.TransactionType.Equals(ATPTEFMFundTransactionTypeAttribute.DecreaseFundValue))
                    {
                        if (tran.Status == APDocStatus.Closed)
                            runningBalance -= tran.CuryCheckAmt;

                        TransactionHistoryView.Current.CuryBalanceAmt = runningBalance;
                        TransactionHistoryView.UpdateCurrent();
                        continue;
                    }
                    #endregion

                    TransactionHistoryView.Current.CuryBalanceAmt = runningBalance;
                    TransactionHistoryView.UpdateCurrent();
                    TransactionHistoryView.Cache.Persist(PXDBOperation.Update);
                }
            }
            #endregion

            TransactionHistoryView.UpdateCurrent();
            TransactionHistoryView.Cache.Persist(PXDBOperation.Update);

            #endregion
        }
        private void ReleaseCashRequestTypeRunningBalance()
        {
            ATPTEFMFundTransaction fundTransaction = FundTransactions.Current;
            #region Update Running Balance                  
            var getTransactionHistory =
                PXSelect<
                    ATPTEFMFundTransactionHistoryView,
                    Where<ATPTEFMFundTransactionHistoryView.fundRefNbr, Equal<Required<ATPTEFMFundTransactionHistoryView.fundRefNbr>>>,
                    OrderBy<
                        Asc<ATPTEFMFundTransactionHistoryView.sortNbr>>>
                    .Select(this, FundTransactions.Current.FundID);

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
                .SelectWindowed(this, startIndex, totalRows, FundTransactions.Current.FundID))
            {
                TransactionHistoryView.Current = TransactionHistoryView.Select(tran.RefNbr);

                if (TransactionHistoryView.Current != null)
                {
                    #region Fund Request
                    if (tran.TransactionType.Equals(ATPTEFMTransactionHistoryView.transactionType.FundRequest))
                    {

                        if (tran.RefNbr == fundTransaction.RefNbr)
                        {
                            runningBalance -= fundTransaction.RequestedAmount;
                            TransactionHistoryView.Current.CuryUnliquidatedAmt = fundTransaction.RequestedAmount;
                        }
                        else
                        {
                            ATPTEFMFundTransaction currentFundTransaction = PXSelect<
                                ATPTEFMFundTransaction,
                                Where<ATPTEFMFundTransaction.refNbr, Equal<Required<ATPTEFMFundTransaction.refNbr>>>>
                                .Select(this, tran.RefNbr);

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
                                        runningBalance -= currentFundTransaction.ReclassificationAmt;
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

                        TransactionHistoryView.Current.CuryBalanceAmt = runningBalance;
                        TransactionHistoryView.UpdateCurrent();
                        continue;
                    }
                    #endregion

                    #region Fund Reimbursement
                    if (tran.TransactionType.Equals(ATPTEFMTransactionHistoryView.transactionType.FundReimbursment))
                    {
                        ATPTEFMFundTransaction currentFundTransaction = PXSelect<
                            ATPTEFMFundTransaction,
                            Where<ATPTEFMFundTransaction.refNbr, Equal<Required<ATPTEFMFundTransaction.refNbr>>>>
                            .Select(this, tran.RefNbr);

                        if (currentFundTransaction != null)
                        {
                            runningBalance -= currentFundTransaction.ActualSpentAmount;
                            runningBalance += currentFundTransaction.TotalWhtAmount;
                        }

                        TransactionHistoryView.Current.CuryBalanceAmt = runningBalance;
                        TransactionHistoryView.UpdateCurrent();
                        continue;
                    }
                    #endregion

                    #region Replenishment
                    if (tran.TransactionType.Equals(ATPTEFMTransactionHistoryView.transactionType.Replenishment))
                    {
                        runningBalance += TransactionHistoryView.Current.CuryCheckAmt ?? decimal.Zero;
                        TransactionHistoryView.Current.CuryBalanceAmt = runningBalance;
                        TransactionHistoryView.UpdateCurrent();
                        continue;
                    }
                    #endregion

                    #region Receipts
                    if (tran.TransactionType.Equals(ATPTEFMTransactionHistoryView.transactionType.ExpenseReceipt) && tran.Status != ATPTEFMExpenseReceiptStatusAttribute.CancelledValue)
                    {
                        ATPTEFMFundTransactionReceiptDetail receipt = PXSelect<
                            ATPTEFMFundTransactionReceiptDetail,
                            Where<ATPTEFMFundTransactionReceiptDetail.expenseReceiptRefNbr,
                            Equal<Required<ATPTEFMFundTransactionReceiptDetail.expenseReceiptRefNbr>>>>
                            .Select(this, tran.RefNbr);

                        if (receipt != null)
                        {
                            #region Get Fund Balance Amount
                            decimal? fundBalanceAmount = runningBalance;
                            TransactionHistoryView.Current = TransactionHistoryView.Select(receipt.FundTransactionRefNbr);
                            if (TransactionHistoryView.Current != null)
                            {
                                fundBalanceAmount = TransactionHistoryView.Current.CuryBalanceAmt;
                            }
                            #endregion

                            TransactionHistoryView.Current = TransactionHistoryView.Select(tran.RefNbr);
                            TransactionHistoryView.Current.CuryBalanceAmt = fundBalanceAmount;
                            TransactionHistoryView.UpdateCurrent();
                            continue;
                        }
                    }

                    #endregion

                    #region IncreaseFund
                    if (tran.TransactionType.Equals(ATPTEFMTransactionHistoryView.transactionType.IncreaseFund))
                    {
                        runningBalance += tran.CuryCheckAmt;
                        TransactionHistoryView.Current.CuryBalanceAmt = runningBalance;
                        TransactionHistoryView.UpdateCurrent();
                        continue;
                    }
                    #endregion

                    #region DecreaseFund
                    if (tran.TransactionType.Equals(ATPTEFMTransactionHistoryView.transactionType.DecreaseFund))
                    {
                        runningBalance -= tran.CuryCheckAmt;
                        TransactionHistoryView.Current.CuryBalanceAmt = runningBalance;
                        TransactionHistoryView.UpdateCurrent();
                        continue;
                    }
                    #endregion

                    TransactionHistoryView.Current.CuryBalanceAmt = runningBalance;
                    TransactionHistoryView.UpdateCurrent();
                }
            }
            #endregion
        }
        private void AllReceiptsMustOpenStatus(string refNbr)
        {
            EPExpenseClaimDetails claim = PXSelectJoin<
                EPExpenseClaimDetails,
                InnerJoin<ATPTEFMFundTransactionReceiptDetail,
                    On<ATPTEFMFundTransactionReceiptDetail.expenseReceiptRefNbr, Equal<EPExpenseClaimDetails.claimDetailCD>,
                    And<ATPTEFMFundTransactionReceiptDetail.fundTransactionRefNbr, Equal<Required<ATPTEFMFundTransactionReceiptDetail.fundTransactionRefNbr>>>>>,
                Where<EPExpenseClaimDetails.status, NotEqual<ATPTEFMExpenseReceiptStatusAttribute.openValue>>>
                .Select(this, refNbr);

            if (claim != null)
                throw new PXException(ATPTEFMMessages.AllReceiptsMustOpenStatus);
        }
        private void UpdateFundTransactionHistory()
        {
            string transactionType = TransactionType();
            bool isFundRequest = ATPTEFMFundTransactionHelper.IsFundRequest(FundTransactions.Current);

            TransactionHistoryView.Current = TransactionHistoryView.Select(FundTransactions.Current.RefNbr);
            if (TransactionHistoryView.Current != null)
            {
                TransactionHistoryView.Current.TransactionType = transactionType;
                TransactionHistoryView.Current.FundType = FundTransactions.Current.FundType;
                TransactionHistoryView.Current.TransactionDate = FundTransactions.Current.Date;
                TransactionHistoryView.Current.CuryFundTransactionDocumentAmt = (isFundRequest) ? FundTransactions.Current.RequestedAmount.GetValueOrDefault() : FundTransactions.Current.ActualSpentAmount.GetValueOrDefault();
                TransactionHistoryView.Current.Status = FundTransactions.Current.Status;
                TransactionHistoryView.Current.CuryFundAmt = (isFundRequest) ? FundTransactions.Current.RequestedAmount.GetValueOrDefault()
                    : FundTransactions.Current.ActualSpentAmount.GetValueOrDefault();
                TransactionHistoryView.UpdateCurrent();
            }
        }
        private string TransactionType()
        {
            var isFundRequest = ATPTEFMFundTransactionHelper.IsFundRequest(FundTransactions.Current);

            string transactionType = ((isFundRequest) ?
                               ATPTEFMTransactionHistoryView.transactionType.FundRequest :
                               ATPTEFMTransactionHistoryView.transactionType.FundReimbursment);
            return transactionType;
        }
        private bool IsFundTransactionRecordExist()
        {
            TransactionHistoryView.Current = TransactionHistoryView.Select(FundTransactions.Current.RefNbr);

            return (TransactionHistoryView.Current != null) ? true : false;
        }
        private void CreateTransactionHistoryRecord()
        {
            ATPTEFMFundTransaction fundTransaction = FundTransactions.Current;
            Fund.Current = Fund.Select(FundTransactions.Current.FundID);
            string transactionType = TransactionType();
            bool isFundRequest = ATPTEFMFundTransactionHelper.IsFundRequest(FundTransactions.Current);
            bool isFundReimbursement = ATPTEFMFundTransactionHelper.IsFundRequest(FundTransactions.Current);
            int rowCount = FundTransactionReceiptLines.Select().Count() + 1;

            ATPTEFMFundTransactionHistoryView transactionHistory = TransactionHistoryView.Insert();
            transactionHistory.FundRefNbr = FundTransactions.Current.FundID;
            transactionHistory.TransactionType = transactionType;
            transactionHistory.OrderDate = FundTransactions.Current.Date;
            transactionHistory.RefNbr = FundTransactions.Current.RefNbr;
            transactionHistory.FundBranchID = FundTransactions.Current.BranchID;
            transactionHistory.FundType = FundTransactions.Current.FundType;
            transactionHistory.TransactionDate = FundTransactions.Current.Date;
            transactionHistory.CuryFundTransactionDocumentAmt = (isFundRequest) ? FundTransactions.Current.RequestedAmount : FundTransactions.Current.ActualSpentAmount;
            transactionHistory.Status = FundTransactions.Current.Status;
            transactionHistory.Source = ATPTEFMFundTransactionHistoryView.source.FundTransaction;
            transactionHistory.CuryUnliquidatedAmt = decimal.Zero;
            transactionHistory.CashAdvanceStatus = FundTransactions.Current.CashAdvanceStatus;
            transactionHistory.CuryFundAmt = (isFundRequest) ? FundTransactions.Current.RequestedAmount.GetValueOrDefault() : FundTransactions.Current.ActualSpentAmount.GetValueOrDefault();
            transactionHistory.CuryBalanceAmt = Fund.Current.CuryBalanceAmt;
            transactionHistory.SortNbr = (isFundRequest) ? $"FT-{FundTransactions.Current.RefNbr}" : $"FT-{FundTransactions.Current.RefNbr}-{rowCount}";
            TransactionHistoryView.Update(transactionHistory);
        }
        public bool ShowBudget()
        {
            ATPTEFMFundTransaction doc = FundTransactions.Current;
            ATPTEFMFeatures feat = FeatureSetup.Current;
            bool limitValidation = feat?.LimitValidation ?? false;
            decimal? limitValidationAmt = feat?.LimitValidationAmt ?? 0m;

            if (doc?.FundTransactionType == ATPTEFMFundTransactionTypeAttribute.CashAdvanceValue && doc?.CashAdvanceStatus == ATPTEFMFundTransactionCashAdvanceStatusAttribute.UnliquidatedValue
                && ((limitValidation == false) || (limitValidation == true && doc?.RequestedAmount <= limitValidationAmt))) return true;
            if (doc?.FundTransactionType == ATPTEFMFundTransactionTypeAttribute.ReimbursementValue) return true;
            return false;
        }
        //public bool ShowBudgetDetail(bool IsProject = false)
        //{
        //    bool showBudget = false;
        //    if (IsProject)
        //    {
        //        showBudget = (FeatureSetup?.Current?.ProjectBudgetModules?.Split(',').Contains("F") ?? false) && ShowBudget();
        //        ProjectBudget.AllowSelect = showBudget;
        //    }
        //    else
        //    {
        //        showBudget = (FeatureSetup?.Current?.BudgetModules?.Split(',').Contains("F") ?? false) && ShowBudget();
        //        FundTransactions.Current.ShowBudgetValidation = showBudget;
        //    }
        //    return showBudget;
        //}
        public void CalFundReturn()
        {
            ATPTEFMFundTransaction receipt = FundTransactions.Current;
            receipt.ChangeAmount = 0m;
            if (receipt.FundTransactionType == ATPTEFMFundTransactionTypeAttribute.CashAdvanceValue)
            {
                foreach (PXResult<ATPTEFMFundTransactionReceiptDetail> item in FundTransactionReceiptLines.Select())
                {
                    ATPTEFMFundTransactionReceiptDetail rcItem = (ATPTEFMFundTransactionReceiptDetail)item;
                    receipt.ChangeAmount += rcItem.FundReturn;
                }
            }
        }
        private int GetAllFundTransactionCount(string fundTran)
        {
            return PXSelect<
                ATPTEFMFundTransaction,
                Where<ATPTEFMFundTransaction.fundID, Equal<Required<ATPTEFMFundTransaction.fundID>>>>
                .Select(this, fundTran)
                .RowCast<ATPTEFMFundTransaction>()
                .Count();
        }
        private void UpdateFundDetailsAtrributes()
        {
            if (FundTransactions.Current?.FundTransactionType != ATPTEFMFundTransactionTypeAttribute.CashAdvanceValue)
            {
                PXDefaultAttribute.SetPersistingCheck<ATPTEFMFundTransactionDetail.particulars>(FundTransactionDetails.Cache, null, PXPersistingCheck.NullOrBlank);
                PXDefaultAttribute.SetPersistingCheck<ATPTEFMFundTransactionDetail.inventoryID>(FundTransactionDetails.Cache, null, PXPersistingCheck.Nothing);

                PXUIFieldAttribute.SetEnabled<ATPTEFMFundTransactionDetail.inventoryID>(FundTransactionDetails.Cache, null, false);
                PXUIFieldAttribute.SetEnabled<ATPTEFMFundTransactionDetail.subID>(FundTransactionDetails.Cache, null, false);

                PXUIFieldAttribute.SetVisible<ATPTEFMFundTransactionDetail.particulars>(FundTransactionDetails.Cache, null, true);
                PXUIFieldAttribute.SetVisible<ATPTEFMFundTransactionDetail.inventoryID>(FundTransactionDetails.Cache, null, false);
            }
            else
            {
                PXDefaultAttribute.SetPersistingCheck<ATPTEFMFundTransactionDetail.particulars>(FundTransactionDetails.Cache, null, PXPersistingCheck.Nothing);
                PXDefaultAttribute.SetPersistingCheck<ATPTEFMFundTransactionDetail.inventoryID>(FundTransactionDetails.Cache, null, PXPersistingCheck.NullOrBlank);

                PXUIFieldAttribute.SetEnabled<ATPTEFMFundTransactionDetail.inventoryID>(FundTransactionDetails.Cache, null, true);
                PXUIFieldAttribute.SetEnabled<ATPTEFMFundTransactionDetail.subID>(FundTransactionDetails.Cache, null, true);

                PXUIFieldAttribute.SetVisible<ATPTEFMFundTransactionDetail.particulars>(FundTransactionDetails.Cache, null, false);
                PXUIFieldAttribute.SetVisible<ATPTEFMFundTransactionDetail.inventoryID>(FundTransactionDetails.Cache, null, true);
            }

            if (FundTransactions.Current?.FundTransactionType == ATPTEFMFundTransactionTypeAttribute.IncreaseFundValue || FundTransactions.Current.FundTransactionType == ATPTEFMFundTransactionTypeAttribute.DecreaseFundValue)
            {
                PXUIFieldAttribute.SetVisible<ATPTEFMFundTransactionDetail.unitRecordID>(FundTransactionDetails.Cache, null, false);
                PXUIFieldAttribute.SetVisible<ATPTEFMFundTransactionDetail.description>(FundTransactionDetails.Cache, null, false);
            }
            else
            {
                PXUIFieldAttribute.SetVisible<ATPTEFMFundTransactionDetail.unitRecordID>(FundTransactionDetails.Cache, null, true);
                PXUIFieldAttribute.SetVisible<ATPTEFMFundTransactionDetail.description>(FundTransactionDetails.Cache, null, true);
            }
        }
        private void UpdateApprovalAmount()
        {
            EPApproval approval = Approval.Current;

            if (approval != null)
            {
                ATPTEFMFundTransaction ft = (ATPTEFMFundTransaction)FundTransactions.Current;

                if (ft != null)
                {
                    decimal? amount = null;

                    if (ft.FundTransactionType == ATPTEFMFundTransactionTypeAttribute.ReimbursementValue)
                    {
                        PXResultset<ATPTEFMFundTransactionReceiptDetail> receiptDetails = FundTransactionReceiptLines.Select();
                        amount = receiptDetails.RowCast<ATPTEFMFundTransactionReceiptDetail>().Sum(s => s.Amount);
                    }
                    else
                    {
                        PXResultset<ATPTEFMFundTransactionDetail> details = FundTransactionDetails.Select();
                        amount = details.RowCast<ATPTEFMFundTransactionDetail>().Sum(s => s.Amount);
                    }

                    ft.TotalApprovalAmount = amount;
                    FundTransactions.Update(ft);

                    approval.CuryTotalAmount = amount;
                    Approval.Update(approval);
                }
            }
        }
        private void CheckFundOnHandAmount()
        {
            ATPTEFMFundTransaction ft = (ATPTEFMFundTransaction)FundTransactions.Current;
            if (ft != null && ft.NoFund == false)
            {
                ATPTEFMFund funds = PXSelect<
                    ATPTEFMFund,
                    Where<ATPTEFMFund.fundCD, Equal<Required<ATPTEFMFund.fundCD>>>>
                    .Select(this, ft.FundID);

                StringBuilder restriction = new StringBuilder();

                if (funds != null)
                {
                    restriction.Clear();
                    restriction.Append(funds.ReplenishmentRestriction);

                    if (funds.FundAmt == null || funds.CuryFundAmt == decimal.Zero)
                    { 
                        throw new PXException(ATPTEFMMessages.ImportFundFirst);
                    }

                    if (ft.FundTransactionType.Equals(ATPTEFMFundTransactionTypeAttribute.CashAdvanceValue))
                    {
                        if (ft.IsReleasedCash != true && ft.RequestedAmount > funds.CuryBalanceAmt)
                        {
                            if (restriction.ToString() == ATPTEFMReplenishmentStringListAttribute.ErrorRestrictValue)
                            {
                                throw ATPTEFMHelper.GetPropertyException(ft, Messages.ATPTEFMMessages.TotalAmountIsGreaterThanOnHandAmount, PXErrorLevel.Error);
                            }
                            else
                            {
                                this.FundTransactions.Cache.RaiseExceptionHandling<ATPTEFMFundTransaction.requestedAmount>(ft, ft.RequestedAmount,
                                              ATPTEFMHelper.GetPropertyException(ft, Messages.ATPTEFMMessages.TotalAmountIsGreaterThanOnHandAmount, PXErrorLevel.Warning));
                            }
                        }

                        if (ft.IsReleasedCash == true && (ft.ActualSpentAmount - ft.RequestedAmount) > funds.CuryBalanceAmt)
                        {
                            if (restriction.ToString() == ATPTEFMReplenishmentStringListAttribute.ErrorRestrictValue)
                            {
                                throw ATPTEFMHelper.GetPropertyException(ft, Messages.ATPTEFMMessages.LiquidationAmountIsGreaterThanOnHandAmount, PXErrorLevel.Error);
                            }
                            else
                            {
                                this.FundTransactions.Cache.RaiseExceptionHandling<ATPTEFMFundTransaction.actualSpentAmount>(ft, ft.ActualSpentAmount,
                                              ATPTEFMHelper.GetPropertyException(ft, Messages.ATPTEFMMessages.LiquidationAmountIsGreaterThanOnHandAmount, PXErrorLevel.Warning));
                            }
                        }
                    }

                    if (ft.FundTransactionType.Equals(ATPTEFMFundTransactionTypeAttribute.ReimbursementValue))
                    {
                        if (ft.IsReleasedCash != true && ft.ActualSpentAmount > funds.CuryBalanceAmt)
                        {
                            if (restriction.ToString() == ATPTEFMReplenishmentStringListAttribute.ErrorRestrictValue)
                            {
                                throw ATPTEFMHelper.GetPropertyException(ft, Messages.ATPTEFMMessages.LiquidationAmountIsGreaterThanOnHandAmount, PXErrorLevel.Error);
                            }
                            else
                            {
                                this.FundTransactions.Cache.RaiseExceptionHandling<ATPTEFMFundTransaction.actualSpentAmount>(ft, ft.ActualSpentAmount,
                                              ATPTEFMHelper.GetPropertyException(ft, Messages.ATPTEFMMessages.LiquidationAmountIsGreaterThanOnHandAmount, PXErrorLevel.Warning));
                            }
                        }
                    }
                }
            }
        }

        private void CheckReplenishmentAmtWarning()
        {
            ATPTEFMFundTransaction ft = (ATPTEFMFundTransaction)FundTransactions.Current;
            if (ft != null && ft.NoFund == false)
            {
                StringBuilder restriction = new StringBuilder();

                ATPTEFMFund funds = PXSelect<
                    ATPTEFMFund,
                    Where<ATPTEFMFund.fundCD, Equal<Required<ATPTEFMFund.fundCD>>>>
                    .Select(this, ft.FundID);

                restriction.Clear();
                restriction.Append(funds.ReplenishmentRestriction);

                if (ft.FundTransactionType.Equals(ATPTEFMFundTransactionTypeAttribute.CashAdvanceValue))
                {
                    if (ft.IsReleasedCash != true && (funds.CuryBalanceAmt - ft.RequestedAmount) < funds.ReplenishmentAmt)
                    {
                        if (restriction.ToString() == ATPTEFMReplenishmentStringListAttribute.ErrorRestrictValue)
                            throw ATPTEFMHelper.GetPropertyException(ft, Messages.ATPTEFMMessages.ReplenishmentAmtLimit, PXErrorLevel.Error);
                    }

                    if (ft.IsReleasedCash == true && (funds.CuryBalanceAmt - (ft.ActualSpentAmount - ft.RequestedAmount)) < funds.ReplenishmentAmt)
                    {
                        if (restriction.ToString() == ATPTEFMReplenishmentStringListAttribute.ErrorRestrictValue)
                            throw ATPTEFMHelper.GetPropertyException(ft, Messages.ATPTEFMMessages.ReplenishmentAmtLimit, PXErrorLevel.Error);

                    }
                }

                if (ft.FundTransactionType.Equals(ATPTEFMFundTransactionTypeAttribute.ReimbursementValue))
                {
                    if (ft.IsReleasedCash != true && (funds.CuryBalanceAmt - ft.ActualSpentAmount) < funds.ReplenishmentAmt)
                    {
                        if (restriction.ToString() == ATPTEFMReplenishmentStringListAttribute.ErrorRestrictValue)
                            throw ATPTEFMHelper.GetPropertyException(ft, Messages.ATPTEFMMessages.ReplenishmentAmtLimit, PXErrorLevel.Error);
                    }
                }
            }

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

        public DateTime GetLiquidationDateWorkCalendar()
        {
            DateTime liquidationDate = new DateTime();
            ATPTEFMFundTransaction fundTransaction = FundTransactions.Current;

            if (fundTransaction != null)
            {
                EPEmployee employee = this.RequestorEmployee.Select();
                CSCalendar calendar = PXSelect<
                    CSCalendar,
                    Where<CSCalendar.calendarID,
                    Equal<Required<CSCalendar.calendarID>>>>
                    .Select(this, employee.CalendarID);

                bool isNonWorkDay = false;
                liquidationDate = fundTransaction.DateOfUse.Value;
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

        public int GetNumberOfLiquidationDays() => Preferences.Current.NoOfDaysToLiquidate ?? 1;

        #endregion

        #region CachedAttached

        [PXMergeAttributes(Method = MergeMethod.Merge)]
        [PXRemoveBaseAttribute(typeof(INUnitAttribute))]
        [INUnit(DisplayName = ATPTEFMMessages.UOM, Visibility = PXUIVisibility.SelectorVisible)]
        protected virtual void InventoryItem_BaseUnit_CacheAttached(PXCache sender)
        { }

        [PXMergeAttributes(Method = MergeMethod.Merge)]
        [PXRemoveBaseAttribute(typeof(AccountAttribute))]
        [Account(DisplayName = ATPTEFMMessages.AccountID, Visibility = PXUIVisibility.Visible, DescriptionField = typeof(Account.description), AvoidControlAccounts = true)]
        protected virtual void InventoryItem_COGSAcctID_CacheAttached(PXCache sender)
        { }

        [PXMergeAttributes(Method = MergeMethod.Merge)]
        [PXRemoveBaseAttribute(typeof(SubAccountAttribute))]
        [SubAccount(typeof(InventoryItem.cOGSAcctID), DisplayName = ATPTEFMMessages.SubAct, Visibility = PXUIVisibility.Visible, DescriptionField = typeof(Sub.description))]
        protected virtual void InventoryItem_COGSSubID_CacheAttached(PXCache sender)
        { }

        [PXMergeAttributes(Method = MergeMethod.Merge)]
        [PXRemoveBaseAttribute(typeof(PXUIFieldAttribute))]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.Vendor)]
        protected virtual void Vendor_AcctName_CacheAttached(PXCache sender)
        { }

        [PXMergeAttributes(Method = MergeMethod.Merge)]
        [PXRemoveBaseAttribute(typeof(PXUIFieldAttribute))]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.Address)]
        protected virtual void Address_AddressLine1_CacheAttached(PXCache sender)
        { }

        [PXMergeAttributes(Method = MergeMethod.Merge)]
        [PXRemoveBaseAttribute(typeof(PXUIFieldAttribute))]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.TIN)]
        protected virtual void LocationExtAddress__TaxRegistrationID_CacheAttached(PXCache sender)
        { }

        #endregion CachedAttached

        #region Events
        protected virtual void _(Events.RowInserted<ATPTEFMFundTransaction> e)
        {
            ATPTEFMFundTransaction ft = e.Row;

            if (ft is null) return;

            if (ft.CreatedByScreenID.Equals(ATPTEFMMessages.FromImportScenarioScreenID))
                ft.IsImported = true;

        }
        protected virtual void _(Events.RowInserted<ATPTEFMFundTransactionDetail> e)
        {
            ATPTEFMFundTransactionDetail ftDet = e.Row;
            if (ftDet is null) return;

            if (IsImportFromExcel) ftDet.LoadedFromExcel = true;
        }
        protected virtual void _(Events.RowInserted<ATPTEFMFundTransactionReceiptDetail> e)
        {
            ATPTEFMFundTransactionReceiptDetail ftrec = e.Row;
            if (ftrec is null) return;

            if (IsImportFromExcel) ftrec.LoadedFromExcel = true;
        }
        protected virtual void _(Events.RowSelecting<ATPTEFMFundTransaction> e)
        {
            ATPTEFMFundTransaction ft = e.Row;
            if (ft == null || (ft.IsImported ?? false)) return;
            using (new PXConnectionScope())
            {
                Fund.Current = Fund.Select(ft.FundID);

                ft.ReceiptsWithNoERRefNbrs = PXSelect<
                    ATPTEFMFundTransactionReceiptDetail,
                    Where<ATPTEFMFundTransactionReceiptDetail.fundTransactionRefNbr, Equal<Required<ATPTEFMFundTransactionReceiptDetail.fundTransactionRefNbr>>,
                        And<Where<ATPTEFMFundTransactionReceiptDetail.expenseReceiptRefNbr, IsNull,
                            Or<ATPTEFMFundTransactionReceiptDetail.expenseReceiptRefNbr, Equal<Empty>>>>>>
                    .Select(this, ft.RefNbr)
                    .Count();

                ft.ReceiptsWithERRefNbrs = PXSelect<
                    ATPTEFMFundTransactionReceiptDetail,
                    Where<ATPTEFMFundTransactionReceiptDetail.fundTransactionRefNbr, Equal<Required<ATPTEFMFundTransactionReceiptDetail.fundTransactionRefNbr>>,
                        And<Where<ATPTEFMFundTransactionReceiptDetail.expenseReceiptRefNbr, IsNotNull,
                            Or<ATPTEFMFundTransactionReceiptDetail.expenseReceiptRefNbr, NotEqual<Empty>>>>>>
                    .Select(this, ft.RefNbr)
                    .Count();
            }
        }
        /// <remarks>
        /// case 009168 - force disable add row if ft is budget enabled
        /// 2024-09-23 : " Error in Fund Transaction can't release cash." - CASE: 007641 {JLG}
        /// 2025-05-09 : " (CFM2023R2 and UP) Fund Transaction Request> Enable the 'Add Row' button in the Receipts tab when the CA status is 'Unliquidated', even if the 'Submit Receipt' button has already been clicked." - CASE: 011533 {JLG}
        /// 2025-05-28 : " (CFM2024R2) Liquidation form in Fund Transaction " - CASE: 011681  {JLG}
        /// 2025-05-29 : Liquidation Form should be enabled regardless if Validate Amount Release/Received is enabled or not. - CASE: 011775  {RFS}
        /// 2025-05-20 : " A Warning message upon saving of fund request below the replenishment limit, 'The request amount exceeds replenishment limit" - CASE: 011537 {JLG}
        /// 2025-09-04 : CFM 2024R1: Fund Transaction Adding of Manual Receipts [Error] CASE 013343 {JLTG}
        /// 03-30-2026 : CaseID - 015748 : Release Cash disabled when approval is enabled - replaced Approved field check with Status check for ReleaseCash and Liquidate buttons {RFS}
        /// </remarks>
        protected virtual void ATPTEFMFundTransaction_RowSelected(PXCache sender, PXRowSelectedEventArgs e)
        {
            ATPTEFMFundTransaction fundTransaction = (ATPTEFMFundTransaction)e.Row;
            ATPTEFMFundTransactionReceiptDetail receiptDetail = FundTransactionReceiptLines.Current;
            ATPTEFMSetup SetUp = Preferences.Current;
            if (fundTransaction == null) return;

            ATPTEFMFund fund = Fund.Current;

            #region Conditional Variables

            //var receipts = FundTransactionReceiptLines.Select().RowCast<ATPTEFMFundTransactionReceiptDetail>().Where(w => string.IsNullOrEmpty(w.ExpenseReceiptRefNbr));
            //(bool, bool) EnableBudget = (ShowBudgetDetail(), ShowBudgetDetail(true));
            bool allowUpdate = (!fundTransaction?.Approved ?? true) && fundTransaction?.Status != ATPTEFMFundStatusAttribute.PendingValue;

            #endregion

            #region Enable/Disable Buttons

            FTCancel.SetEnabled(fundTransaction.Status != ATPTEFMFundStatusAttribute.ClosedValue);
            CreateAPBill.SetEnabled(fundTransaction?.Approved ?? false);
            Delete.SetEnabled(allowUpdate);
            FundLiquidationReport.SetEnabled(false);

            if (fund != null && fund.ATPTEFMValidateAmountReceivedAndAmountReleased == true)
            {
                LoadRequest.SetEnabled(fundTransaction.FundTransactionType.Equals(ATPTEFMFundTransactionTypeAttribute.CashAdvanceValue)
                    && fundTransaction.AmountReceived == 0
                    && (fundTransaction.CashAdvanceStatus.Equals(ATPTEFMFundTransactionCashAdvanceStatusAttribute.UnliquidatedValue)
                    || fundTransaction.CashAdvanceStatus.Equals(ATPTEFMFundTransactionCashAdvanceStatusAttribute.ForReclassificationValue)));
            }
            else
            {
                LoadRequest.SetEnabled(fundTransaction.FundTransactionType == ATPTEFMFundTransactionTypeAttribute.CashAdvanceValue && fundTransaction.CashAdvanceStatus == ATPTEFMFundTransactionCashAdvanceStatusAttribute.UnliquidatedValue);
            }

            if (fundTransaction?.Approved ?? false)
            {
                CreateAPBill.SetEnabled(string.IsNullOrEmpty(fundTransaction?.InvoiceRefNbr ?? string.Empty));
            }

            if (fundTransaction.FundTransactionType == ATPTEFMFundTransactionTypeAttribute.CashAdvanceValue ||
                fundTransaction.FundTransactionType == ATPTEFMFundTransactionTypeAttribute.ReimbursementValue)
            {
                CreateAPBill.SetEnabled(false);
            }

            if (fundTransaction.FundTransactionType == ATPTEFMFundTransactionTypeAttribute.CashAdvanceValue)
            {
                if (fund != null && fund.ATPTEFMValidateAmountReceivedAndAmountReleased == true)
                {
                    SubmitReceipts?.SetEnabled(fundTransaction.ReceiptsWithNoERRefNbrs != 0 && (fundTransaction.CashAdvanceStatus.Equals(ATPTEFMFundTransactionCashAdvanceStatusAttribute.UnliquidatedValue)
                         || fundTransaction.CashAdvanceStatus.Equals(ATPTEFMFundTransactionCashAdvanceStatusAttribute.ForReclassificationValue)));
                }
                else
                {
                    bool containsNoErRefNbr = fundTransaction.ReceiptsWithNoERRefNbrs != 0;
                    SubmitReceipts?.SetEnabled(fundTransaction.ReceiptsWithNoERRefNbrs != 0 && fundTransaction.CashAdvanceStatus == ATPTEFMFundTransactionCashAdvanceStatusAttribute.UnliquidatedValue);
                }
                FundLiquidationReport.SetEnabled(fundTransaction.CashAdvanceStatus == ATPTEFMFundTransactionCashAdvanceStatusAttribute.UnliquidatedValue || fundTransaction.CashAdvanceStatus == ATPTEFMFundTransactionCashAdvanceStatusAttribute.LiquidatedValue);
                ReleaseCash?.SetEnabled(fundTransaction?.Status == ATPTEFMFundStatusAttribute.OpenValue && (fundTransaction?.IsReleasedCash ?? false) == false);
                Liquidate?.SetEnabled(fundTransaction?.Status == ATPTEFMFundStatusAttribute.OpenValue && fundTransaction.Step == ATPTEFMFundTransactionStepAttribute.SubmitReceiptValue && fundTransaction.CashAdvanceStatus != ATPTEFMFundTransactionCashAdvanceStatusAttribute.LiquidatedValue);
            }

            if (fundTransaction.FundTransactionType == ATPTEFMFundTransactionTypeAttribute.ReimbursementValue)
            {
                SubmitReceipts?.SetEnabled((fundTransaction.Approved ?? false) && fundTransaction.ReceiptsWithNoERRefNbrs != 0 && fundTransaction.Status != ATPTEFMFundStatusAttribute.CancelledValue && fundTransaction.Status != ATPTEFMFundStatusAttribute.ClosedValue);
                ReleaseCash?.SetEnabled(fundTransaction.Step == ATPTEFMFundTransactionStepAttribute.SubmitReceiptValue);
                FTCancel.SetEnabled(fundTransaction.Status != ATPTEFMFundStatusAttribute.CancelledValue);
                Liquidate.SetEnabled(false);
            }

            #endregion

            #region Field Defaulting

            FundTransactions.Current.ShowBudgetValidation = false;

            #endregion

            #region Set Field Visible

            if (Preferences?.Current?.FundTransactionRequestApproval != true && fundTransaction.Approved == true)
                PXUIFieldAttribute.SetVisible<ATPTEFMFundTransaction.approved>(sender, fundTransaction, false);
            else if (Preferences?.Current?.FundTransactionRequestApproval != true && fundTransaction.Approved == false)
                PXUIFieldAttribute.SetVisible<ATPTEFMFundTransaction.approved>(sender, fundTransaction, false);
            else
            {
                PXUIFieldAttribute.SetVisible<ATPTEFMFundTransaction.approved>(sender, fundTransaction, true);
            }

            PXUIFieldAttribute.SetVisible<ATPTEFMFundTransaction.cashAdvanceStatus>(sender, fundTransaction, fundTransaction.FundTransactionType == ATPTEFMFundTransactionTypeAttribute.CashAdvanceValue);
            PXUIFieldAttribute.SetVisible<ATPTEFMFundTransaction.requestedAmount>(sender, fundTransaction, fundTransaction.FundTransactionType == ATPTEFMFundTransactionTypeAttribute.CashAdvanceValue);
            PXUIFieldAttribute.SetVisible<ATPTEFMFundTransaction.actualSpentAmount>(sender, fundTransaction, fundTransaction.FundTransactionType == ATPTEFMFundTransactionTypeAttribute.CashAdvanceValue || fundTransaction.FundTransactionType == ATPTEFMFundTransactionTypeAttribute.ReimbursementValue);
            PXUIFieldAttribute.SetVisible<ATPTEFMFundTransaction.changeAmount>(sender, fundTransaction, fundTransaction.FundTransactionType == ATPTEFMFundTransactionTypeAttribute.CashAdvanceValue);

            PXUIFieldAttribute.SetVisible<ATPTEFMFundTransaction.isOverbudget>(sender, fundTransaction, Classes.ATPTEFMBudgetLibrary.BudgetVisible(FeatureSetup?.Current, "F"));
            PXUIFieldAttribute.SetVisible<ATPTEFMFundTransaction.hasInitialBudget>(sender, fundTransaction, Classes.ATPTEFMBudgetLibrary.BudgetVisible(FeatureSetup?.Current, "F"));

            PXUIFieldAttribute.SetVisible<ATPTEFMFundTransaction.reclassificationAmt>(sender, fundTransaction, (fundTransaction.FundTransactionType == ATPTEFMFundTransactionTypeAttribute.CashAdvanceValue && (fundTransaction.LiqDate < Accessinfo.BusinessDate || fundTransaction.ReclassificationAmt > 0m)));
            PXUIFieldAttribute.SetVisible<ATPTEFMFundTransaction.balance>(sender, fundTransaction, fund != null && (fund.ATPTEFMValidateAmountReceivedAndAmountReleased ?? false));
            #endregion

            #region Set Field DisplayName

            //if (EnableBudget.Item2 && fundTransaction.FundTransactionType == ATPTEFMFundTransactionTypeAttribute.CashAdvanceValue)
            //{
            //    PXUIFieldAttribute.SetDisplayName<ATPTEFMPBudget.docAmt>(ProjectBudget.Cache, "CA Amount");
            //}
            //else if (EnableBudget.Item2 && fundTransaction.FundTransactionType == ATPTEFMFundTransactionTypeAttribute.ReimbursementValue)
            //{
            //    PXUIFieldAttribute.SetDisplayName<ATPTEFMPBudget.docAmt>(ProjectBudget.Cache, "Reimbursement Amount");
            //}

            #endregion

            #region Set Field Required

            PXUIFieldAttribute.SetRequired<ATPTEFMFundTransactionReceiptDetail.vendorName>(FundTransactionReceiptLines.Cache, Preferences.Current.RequireVendorDetails ?? false);
            PXUIFieldAttribute.SetRequired<ATPTEFMFundTransactionReceiptDetail.vendorAddress>(FundTransactionReceiptLines.Cache, Preferences.Current.RequireVendorDetails ?? false);
            PXUIFieldAttribute.SetRequired<ATPTEFMFundTransactionReceiptDetail.vendorTin>(FundTransactionReceiptLines.Cache, Preferences.Current.RequireVendorDetails ?? false);
            PXUIFieldAttribute.SetRequired<ATPTEFMFundTransactionReceiptDetail.refNbr>(FundTransactionReceiptLines.Cache, Preferences.Current.RequireExternalReferenceNbr ?? false);

            #endregion

            #region Set Field Warning/Error

            if (fundTransaction.Step == ATPTEFMFundTransactionStepAttribute.SubmitReceiptValue && fundTransaction.ChangeAmount > 0m)
                PXUIFieldAttribute.SetWarning<ATPTEFMFundTransaction.amountReceived>(sender, fundTransaction, ATPTEFMMessages.PleaseInputActualAmountReceivedFromRequestor);

            else if (fundTransaction.Step == ATPTEFMFundTransactionStepAttribute.SubmitReceiptValue && fundTransaction.ChangeAmount < 0m)
                PXUIFieldAttribute.SetWarning<ATPTEFMFundTransaction.amountReleased>(sender, fundTransaction, ATPTEFMMessages.PleaseInputActualAmountReleasedToRequestor);

            if (fundTransaction.FundTransactionType.Equals(ATPTEFMFundTransactionTypeAttribute.CashAdvanceValue) && Fund.Current != null)
            {
                if (fundTransaction.IsReleasedCash != true && (Fund.Current.CuryBalanceAmt - fundTransaction.RequestedAmount) < Fund.Current.ReplenishmentAmt)
                {
                    if (Fund.Current.ReplenishmentRestriction.Equals(ATPTEFMReplenishmentStringListAttribute.WarningValue))
                        PXUIFieldAttribute.SetWarning<ATPTEFMFundTransaction.requestedAmount>(sender, fundTransaction, ATPTEFMMessages.ReplenishmentAmtLimit);
                }

                if (fundTransaction.IsReleasedCash == true && (Fund.Current.CuryBalanceAmt - (fundTransaction.ActualSpentAmount - fundTransaction.RequestedAmount)) < Fund.Current.ReplenishmentAmt)
                {
                    if (Fund.Current.ReplenishmentRestriction.Equals(ATPTEFMReplenishmentStringListAttribute.WarningValue))
                        PXUIFieldAttribute.SetWarning<ATPTEFMFundTransaction.actualSpentAmount>(sender, fundTransaction, ATPTEFMMessages.ReplenishmentAmtLimit);
                }
            }

            if (fundTransaction.FundTransactionType.Equals(ATPTEFMFundTransactionTypeAttribute.ReimbursementValue) && Fund.Current != null)
            {
                if (fundTransaction.IsReleasedCash != true && (Fund.Current.CuryBalanceAmt - fundTransaction.ActualSpentAmount) < Fund.Current.ReplenishmentAmt)
                {
                    if (Fund.Current.ReplenishmentRestriction.Equals(ATPTEFMReplenishmentStringListAttribute.WarningValue))
                        PXUIFieldAttribute.SetWarning<ATPTEFMFundTransaction.actualSpentAmount>(sender, fundTransaction, ATPTEFMMessages.ReplenishmentAmtLimit);
                }
            }

            #endregion

            #region Views Allow Insert/Update/Delete/Select

            //ProjectBudget.AllowSelect = fundTransaction.ProjectBudgetEnabled ?? false;

            FundTransactionDetails.AllowInsert = allowUpdate;
            FundTransactionDetails.AllowUpdate = allowUpdate;
            FundTransactionDetails.AllowDelete = allowUpdate;
            FundTransactionDetails.AllowSelect = fundTransaction.FundTransactionType == ATPTEFMFundTransactionTypeAttribute.CashAdvanceValue;

            if (fundTransaction.FundTransactionType == ATPTEFMFundTransactionTypeAttribute.CashAdvanceValue)
            {
                if ((fundTransaction.BudgetEnabled ?? false) || (fundTransaction.ProjectBudgetEnabled ?? false))
                {
                    FundTransactionReceiptLines.AllowInsert = false;
                }
                else if (fund != null && fund.ATPTEFMValidateAmountReceivedAndAmountReleased == true)
                {
                    FundTransactionReceiptLines.AllowInsert = ((SetUp.AllowManualReceipts ?? false) && fundTransaction.AmountReceived == 0
                        && (fundTransaction.CashAdvanceStatus.Equals(ATPTEFMFundTransactionCashAdvanceStatusAttribute.UnliquidatedValue)
                        || fundTransaction.CashAdvanceStatus.Equals(ATPTEFMFundTransactionCashAdvanceStatusAttribute.ForReclassificationValue)));

                    FundTransactionReceiptLines.AllowUpdate = fundTransaction.AmountReceived == 0
                        && (fundTransaction.CashAdvanceStatus.Equals(ATPTEFMFundTransactionCashAdvanceStatusAttribute.UnliquidatedValue)
                        || fundTransaction.CashAdvanceStatus.Equals(ATPTEFMFundTransactionCashAdvanceStatusAttribute.ForReclassificationValue));

                    FundTransactionReceiptLines.AllowDelete = fundTransaction.AmountReceived == 0
                        && (fundTransaction.CashAdvanceStatus.Equals(ATPTEFMFundTransactionCashAdvanceStatusAttribute.UnliquidatedValue)
                        || fundTransaction.CashAdvanceStatus.Equals(ATPTEFMFundTransactionCashAdvanceStatusAttribute.ForReclassificationValue));
                }
                else
                {
                    FundTransactionReceiptLines.AllowInsert = ((SetUp.AllowManualReceipts ?? false)
                           && fundTransaction.CashAdvanceStatus == ATPTEFMFundTransactionCashAdvanceStatusAttribute.UnliquidatedValue);
                    FundTransactionReceiptLines.AllowUpdate = fundTransaction.CashAdvanceStatus == ATPTEFMFundTransactionCashAdvanceStatusAttribute.UnliquidatedValue;
                    FundTransactionReceiptLines.AllowDelete = fundTransaction.CashAdvanceStatus == ATPTEFMFundTransactionCashAdvanceStatusAttribute.UnliquidatedValue;
                }
            }
            else if (fundTransaction.FundTransactionType == ATPTEFMFundTransactionTypeAttribute.ReimbursementValue)
            {
                FundTransactionReceiptLines.AllowInsert = fundTransaction.Status == ATPTEFMFundStatusAttribute.HoldValue;
                FundTransactionReceiptLines.AllowDelete = fundTransaction.Status != ATPTEFMFundStatusAttribute.ClosedValue && fundTransaction.Status != ATPTEFMFundStatusAttribute.CancelledValue;

                if (fundTransaction.ReceiptsWithERRefNbrs > 0)
                {
                    PXUIFieldAttribute.SetEnabled<ATPTEFMFundTransaction.fundType>(sender, fundTransaction, false);
                    PXUIFieldAttribute.SetEnabled<ATPTEFMFundTransaction.fundID>(sender, fundTransaction, false);
                    PXUIFieldAttribute.SetEnabled<ATPTEFMFundTransaction.fundTransactionType>(sender, fundTransaction, false);
                    PXUIFieldAttribute.SetEnabled<ATPTEFMFundTransaction.date>(sender, fundTransaction, false);
                    PXUIFieldAttribute.SetEnabled<ATPTEFMFundTransaction.finPeriodID>(sender, fundTransaction, false);
                    PXUIFieldAttribute.SetEnabled<ATPTEFMFundTransaction.requestedByID>(sender, fundTransaction, false);
                    PXUIFieldAttribute.SetEnabled<ATPTEFMFundTransaction.departmentID>(sender, fundTransaction, false);
                }
            }
            FundTransactionReclassficationReceiptDetail.AllowSelect = fundTransaction.FundTransactionType == ATPTEFMFundTransactionTypeAttribute.CashAdvanceValue
                && (fundTransaction.LiqDate < Accessinfo.BusinessDate || fundTransaction.ReclassificationAmt > 0m);

            #endregion

            #region Raise Field Events

            sender.RaiseFieldUpdated<ATPTEFMFundTransaction.date>(fundTransaction, null);

            #endregion

            #region Calculate Balance if Validate Amount Received/Released = True
            if (fund != null)
            {
                if (fund.ATPTEFMValidateAmountReceivedAndAmountReleased ?? false)
                {
                    if (fundTransaction.ActualSpentAmount == 0)
                        FundTransactions.Current.Balance = fundTransaction.RequestedAmount - fundTransaction.ReclassificationAmt;

                    if (fundTransaction.Status == ATPTEFMFundStatusAttribute.OpenValue || fundTransaction.Status == ATPTEFMFundStatusAttribute.ClosedValue)
                    {
                        if (fundTransaction.ActualSpentAmount > 0)
                            FundTransactions.Current.Balance = (fundTransaction.ChangeAmount - (fundTransaction.AmountReceived + fundTransaction.ReclassificationAmt)) + fundTransaction.AmountReleased;
                    }
                }
                else
                {
                    if (fundTransaction.ReceiptsWithERRefNbrs > 0)
                        FundTransactions.Current.Balance = decimal.Zero;

                    if (fundTransaction.ReceiptsWithERRefNbrs == 0 && fundTransaction.CashAdvanceStatus.Equals(ATPTEFMFundTransactionCashAdvanceStatusAttribute.UnliquidatedValue))
                        FundTransactions.Current.Balance = fundTransaction.RequestedAmount;
                }
            }
            PXUIFieldAttribute.SetVisible<ATPTEFMFundTransaction.amountReceived>(sender, fundTransaction, (fund != null && (fund.ATPTEFMValidateAmountReceivedAndAmountReleased ?? false) && fundTransaction.FundTransactionType == ATPTEFMFundTransactionTypeAttribute.CashAdvanceValue));
            PXUIFieldAttribute.SetVisible<ATPTEFMFundTransaction.amountReleased>(sender, fundTransaction, (fund != null && (fund.ATPTEFMValidateAmountReceivedAndAmountReleased ?? false) && fundTransaction.FundTransactionType == ATPTEFMFundTransactionTypeAttribute.CashAdvanceValue));

            PXUIFieldAttribute.SetVisible<ATPTEFMFundTransaction.dateOfUse>(sender, fundTransaction, (fundTransaction.FundTransactionType == ATPTEFMFundTransactionTypeAttribute.CashAdvanceValue));
            PXUIFieldAttribute.SetRequired<ATPTEFMFundTransaction.dateOfUse>(sender, (fundTransaction.FundTransactionType == ATPTEFMFundTransactionTypeAttribute.CashAdvanceValue));
            PXUIFieldAttribute.SetVisible<ATPTEFMFundTransaction.liqDate>(sender, fundTransaction, (fundTransaction.FundTransactionType == ATPTEFMFundTransactionTypeAttribute.CashAdvanceValue));
            #endregion

            #region Set Project Fields Column Configuration Visibility
            PXUIFieldAttribute.SetVisibility<ATPTEFMFundTransactionDetail.projectID>(FundTransactionDetails.Cache, FundTransactionDetails.Current, PXUIVisibility.Invisible);
            PXUIFieldAttribute.SetVisibility<ATPTEFMFundTransactionDetail.projectTaskID>(FundTransactionDetails.Cache, FundTransactionDetails.Current, PXUIVisibility.Invisible);

            PXUIFieldAttribute.SetVisibility<ATPTEFMFundTransactionReceiptDetail.projectID>(FundTransactionReceiptLines.Cache, FundTransactionReceiptLines.Current, PXUIVisibility.Invisible);
            PXUIFieldAttribute.SetVisibility<ATPTEFMFundTransactionReceiptDetail.projectTaskID>(FundTransactionReceiptLines.Cache, FundTransactionReceiptLines.Current, PXUIVisibility.Invisible);

            /*PXUIFieldAttribute.SetVisibility<ATPTEFMFundTransactionReceiptDetail.netQty>(FundTransactionReceiptLines.Cache, FundTransactionReceiptLines.Current, PXUIVisibility.Invisible);
            PXUIFieldAttribute.SetVisibility<ATPTEFMFundTransactionReceiptDetail.netUnitCost>(FundTransactionReceiptLines.Cache, FundTransactionReceiptLines.Current, PXUIVisibility.Invisible);
            PXUIFieldAttribute.SetVisibility<ATPTEFMFundTransactionReceiptDetail.netAmt>(FundTransactionReceiptLines.Cache, FundTransactionReceiptLines.Current, PXUIVisibility.Invisible);*/
            #endregion

            #region Display Buttons on Main Toolbar

            SubmitReceipts?.SetDisplayOnMainToolbar(SubmitReceipts.GetEnabled());
            Liquidate?.SetDisplayOnMainToolbar(Liquidate.GetEnabled());

            #endregion

            #region Raise Field Errors/Warnings
            if (fundTransaction.UnmodifiedLiqDate != fundTransaction.LiqDate)
                sender.RaiseExceptionHandling<ATPTEFMFundTransaction.liqDate>(fundTransaction, fundTransaction.LiqDate,
                    ATPTEFMHelper.GetPropertyException(fundTransaction, ATPTEFMMessages.LiquidationDateHasBeenManuallyAdjusted, PXErrorLevel.Warning));
            #endregion

            UpdateFundDetailsAtrributes();
        }
        protected virtual void ATPTEFMFundTransaction_Date_FieldUpdated(PXCache sender, PXFieldUpdatedEventArgs e)
        {
            CurrencyInfoAttribute.SetEffectiveDate<ATPTEFMFundTransaction.date>(sender, e);
        }
        protected virtual void ATPTEFMFundTransactionDetail_RowInserting(PXCache sender, PXRowInsertingEventArgs e)
        {
            ATPTEFMFundTransactionDetail row = (ATPTEFMFundTransactionDetail)e.Row;

            ATPTEFMFund fund = PXSelect<
                ATPTEFMFund,
                Where<ATPTEFMFund.fundCD, Equal<Current<ATPTEFMFundTransaction.fundID>>>>
                .SelectWindowed(this, 0, 1);

            if (fund != null)
            {
                if (fund.FundType == ATPTEFMFundTypeAttribute.PettyCashValue)
                {
                    row.Particulars = ATPTEFMFundTypeAttribute.PettyCash;
                }
                if (fund.FundType == ATPTEFMFundTypeAttribute.RevolvingFundValue)
                {
                    row.Particulars = ATPTEFMFundTypeAttribute.RevolvingFund;
                }

                if (FundTransactions.Current.FundTransactionType != ATPTEFMFundTransactionTypeAttribute.CashAdvanceValue)
                {
                    row.AccountID = fund.AccountID;
                }
            }
        }
        protected virtual void ATPTEFMFundTransaction_FundID_FieldUpdated(PXCache sender, PXFieldUpdatedEventArgs e)
        {
            ATPTEFMFundTransaction fundTransaction = (ATPTEFMFundTransaction)e.Row;

            if (fundTransaction is null) return;

            Fund.Current = PXSelect<
                ATPTEFMFund,
                Where<ATPTEFMFund.fundCD, Equal<Current<ATPTEFMFundTransaction.fundID>>>>
                .SelectWindowed(this, 0, 1);

            if (Fund.Current != null)
            {
                fundTransaction.BranchID = Fund.Current.BranchID;

                foreach (ATPTEFMFundTransactionDetail row in FundTransactionDetails.Select())
                {
                    if (Fund.Current.FundType == ATPTEFMFundTypeAttribute.PettyCashValue)
                    {
                        row.Particulars = ATPTEFMFundTypeAttribute.PettyCash;
                    }
                    if (Fund.Current.FundType == ATPTEFMFundTypeAttribute.RevolvingFundValue)
                    {
                        row.Particulars = ATPTEFMFundTypeAttribute.RevolvingFund;
                    }

                    if (FundTransactions.Current.FundTransactionType != ATPTEFMFundTransactionTypeAttribute.CashAdvanceValue)
                    {
                        row.AccountID = Fund.Current.AccountID;
                    }
                }
            }
        }
        protected virtual void ATPTEFMFundTransaction_FundTransactionType_FieldUpdated(PXCache sender, PXFieldUpdatedEventArgs e)
        {
            ATPTEFMFund fund = PXSelect<
                ATPTEFMFund,
                Where<ATPTEFMFund.fundCD, Equal<Current<ATPTEFMFundTransaction.fundID>>>>
                .SelectWindowed(this, 0, 1);

            if (fund == null) { return; }

            foreach (ATPTEFMFundTransactionDetail row in FundTransactionDetails.Select())
            {
                if (FundTransactions.Current.FundTransactionType != ATPTEFMFundTransactionTypeAttribute.CashAdvanceValue)
                {
                    row.AccountID = fund.AccountID;
                }
            }

            if (FundTransactions.Current.FundTransactionType == ATPTEFMFundTransactionTypeAttribute.ReimbursementValue)
            {
                foreach (ATPTEFMFundTransactionDetail detail in FundTransactionDetails.Select())
                {
                    FundTransactionDetails.Cache.Delete(detail);
                }
            }

            if (FundTransactions.Current.FundTransactionType == ATPTEFMFundTransactionTypeAttribute.CashAdvanceValue)
            {
                foreach (ATPTEFMFundTransactionReceiptDetail detail in FundTransactionReceiptLines.Select())
                {
                    FundTransactionReceiptLines.Cache.Delete(detail);
                }
            }
        }
        protected virtual void ATPTEFMFundTransaction_DepartmentID_FieldUpdated(PXCache sender, PXFieldUpdatedEventArgs e)
        {
            ATPTEFMFund fund = PXSelect<
                ATPTEFMFund,
                Where<ATPTEFMFund.fundCD, Equal<Current<ATPTEFMFundTransaction.fundID>>>>
                .SelectWindowed(this, 0, 1);

            foreach (ATPTEFMFundTransactionDetail row in FundTransactionDetails.Select())
            {
                if (FundTransactions.Current.FundTransactionType != ATPTEFMFundTransactionTypeAttribute.CashAdvanceValue)
                {
                    row.AccountID = fund.AccountID;
                }
            }
        }
        protected virtual void ATPTEFMFundTransaction_NoFund_FieldUpdated(PXCache sender, PXFieldUpdatedEventArgs e)
        {
            ATPTEFMFundTransaction row = e.Row as ATPTEFMFundTransaction;

            if (row != null)
            {
                if (row.NoFund ?? false)
                {
                    row.FundID = null;
                }
            }
        }

        //TODO : Transfer logic to field updated event
        protected virtual void ATPTEFMSetup_FundTransactionRequestApproval_FieldUpdating(PXCache sender, PXFieldUpdatingEventArgs e)
        {
            PXCache cache = this.Caches[typeof(ATPTEFMFundTransactionSetupApproval)];
            PXResultset<ATPTEFMFundTransactionSetupApproval> setups = PXSelect<ATPTEFMFundTransactionSetupApproval>.Select(sender.Graph, null);
            foreach (ATPTEFMFundTransactionSetupApproval setup in setups)
            {
                setup.IsActive = (bool?)e.NewValue;
                cache.Update(setup);
            }
        }
        protected virtual void ATPTEFMFundTransactionDetail_RowSelected(PXCache sender, PXRowSelectedEventArgs e)
        {
            ATPTEFMFundTransactionDetail row = e.Row as ATPTEFMFundTransactionDetail;

            if (row != null)
            {
                if (row.ProjectID != ProjectDefaultAttribute.NonProject())
                {
                    PXDefaultAttribute.SetPersistingCheck<ATPTEFMFundTransactionDetail.projectTaskID>(sender, row, PXPersistingCheck.NullOrBlank);
                    PXDefaultAttribute.SetPersistingCheck<ATPTEFMFundTransactionDetail.costCodeID>(sender, row, PXPersistingCheck.NullOrBlank);
                }

                bool isNonProject = ProjectDefaultAttribute.IsNonProject(row.ProjectID);
                PXUIFieldAttribute.SetEnabled<ATPTEFMFundTransactionDetail.projectTaskID>(sender, e.Row, !isNonProject);
                PXUIFieldAttribute.SetEnabled<ATPTEFMFundTransactionDetail.costCodeID>(sender, e.Row, !isNonProject);

                UpdateFundDetailsAtrributes();
            }
        }
        protected virtual void _(Events.FieldDefaulting<ATPTEFMFundTransaction, ATPTEFMFundTransaction.budgetEnabled> e)
        {
            ATPTEFMFundTransaction row = e.Row;
            if (row == null || (row.IsImported ?? false)) return;

            if (BudgetVisible(FeatureSetup.Current, "F") && row.BudgetEnabled == null)
            {
                e.NewValue = true;
                e.Cancel = true;
            }
            else if ((!BudgetVisible(FeatureSetup.Current, "F")) && row.BudgetEnabled == null)
            {
                e.NewValue = false;
                e.Cancel = true;
            }
        }
        protected virtual void _(Events.FieldDefaulting<ATPTEFMFundTransaction, ATPTEFMFundTransaction.projectBudgetEnabled> e)
        {
            ATPTEFMFundTransaction row = e.Row;
            if (row == null) return;

            if (ProjectBudgetVisible(FeatureSetup.Current, "F") && row.ProjectBudgetEnabled == null)
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
        protected virtual void ATPTEFMFundTransactionDetail_Date_FieldDefaulting(PXCache sender, PXFieldDefaultingEventArgs e)
        {
            e.NewValue = ((DateTime)Accessinfo.BusinessDate);
        }
        protected virtual void _(Events.RowInserting<ATPTEFMFundTransactionReceiptDetail> e)
        {
            ATPTEFMFundTransactionReceiptDetail row = e.Row;
            if (row == null || (row.IsImported ?? false)) return;

            ATPTEFMFundTransaction ft = FundTransactions.Current;

            if (row.ExpenseReceiptRefNbr.IsNullOrEmpty()) ft.ReceiptsWithNoERRefNbrs++;
        }
        /// <remarks>
        /// 2025-02-04 : "Project settings (disables ProjectTask and CostCode when NonProject is selected) CASE: 010041 {JLTG}
        /// </remarks>
        protected virtual void ATPTEFMFundTransactionReceiptDetail_RowSelected(PXCache sender, PXRowSelectedEventArgs e)
        {
            ATPTEFMFundTransactionReceiptDetail row = e.Row as ATPTEFMFundTransactionReceiptDetail;
            ATPTEFMFundTransaction ft = FundTransactions.Current;

            if (row != null)
                PXUIFieldAttribute.SetEnabled(sender, row, string.IsNullOrEmpty(row.ExpenseReceiptRefNbr));


            #region Disable Budget Related Fields if Budget is Enabled for Fund Transaction
            if (FundTransactions?.Current?.FundTransactionType == ATPTEFMFundTransactionTypeAttribute.CashAdvanceValue && ft.BudgetEnabled == true)
            {
                PXUIFieldAttribute.SetEnabled<ATPTEFMFundTransactionReceiptDetail.accountID>(sender, row, !((ft.BudgetEnabled ?? false) || (ft.ProjectBudgetEnabled ?? false)));
                PXUIFieldAttribute.SetEnabled<ATPTEFMFundTransactionReceiptDetail.subID>(sender, row, !((ft.BudgetEnabled ?? false) || (ft.ProjectBudgetEnabled ?? false)));
                PXUIFieldAttribute.SetEnabled<ATPTEFMFundTransactionReceiptDetail.projectID>(sender, row, !((ft.BudgetEnabled ?? false) || (ft.ProjectBudgetEnabled ?? false)));
                PXUIFieldAttribute.SetEnabled<ATPTEFMFundTransactionReceiptDetail.projectTaskID>(sender, row, !((ft.BudgetEnabled ?? false) || (ft.ProjectBudgetEnabled ?? false)));
                PXUIFieldAttribute.SetEnabled<ATPTEFMFundTransactionReceiptDetail.costCodeID>(sender, row, !((ft.BudgetEnabled ?? false) || (ft.ProjectBudgetEnabled ?? false)));
            }
            else
            {
                bool isNonProject = ProjectDefaultAttribute.IsNonProject(row.ProjectID);
                PXUIFieldAttribute.SetEnabled<ATPTEFMFundTransactionReceiptDetail.projectTaskID>(sender, e.Row, !isNonProject);
                PXUIFieldAttribute.SetEnabled<ATPTEFMFundTransactionReceiptDetail.costCodeID>(sender, e.Row, !isNonProject);
            }

            #endregion
        }
        protected virtual void ATPTEFMFundTransactionReceiptDetail_RowDeleted(PXCache sender, PXRowDeletedEventArgs e)
        {
            ATPTEFMFundTransaction receipt = FundTransactions.Current;
            if (receipt.FundTransactionType == ATPTEFMFundTransactionTypeAttribute.CashAdvanceValue)
            {
                if (!FundTransactionReceiptLines.Select().Any())
                {
                    receipt.ChangeAmount = decimal.Zero;
                    receipt.Balance = receipt.RequestedAmount - (receipt.ActualSpentAmount + receipt.ChangeAmount);
                }
                else
                {
                    receipt.ChangeAmount = receipt.RequestedAmount - FundTransactionReceiptLines.Select().RowCast<ATPTEFMFundTransactionReceiptDetail>().Sum(s => s.NetAmt);
                }
            }
        }
        protected virtual void _(Events.RowDeleting<ATPTEFMFundTransactionReceiptDetail> e)
        {
            ATPTEFMFundTransactionReceiptDetail row = e.Row as ATPTEFMFundTransactionReceiptDetail;
            if (row == null || (row.IsImported ?? false)) return;

            if (!string.IsNullOrEmpty(row.ExpenseReceiptRefNbr))
                throw new Exception(Messages.ATPTEFMMessages.CannotDeleteReceipt);

            ATPTEFMFundTransaction ft = FundTransactions.Current;

            ft.ReceiptsWithNoERRefNbrs--;
        }
        protected virtual void _(Events.RowPersisting<ATPTEFMFundTransactionDetail> e)
        {
            ATPTEFMFundTransactionDetail row = e.Row;
            if (row == null || (row.IsImported ?? false)) return;

            ATPTEFMFundTransaction ft = FundTransactions.Current;

            BudgetValidationRaiseErrorForRequestAmountGreaterThanRemainingBudget(row, null, ft?.BudgetEnabled ?? false);
            //PBudgetValidationRaiseErrorForRequestAmountGreaterThanRemainingBudget(row, null, ft?.ProjectBudgetEnabled ?? false);
        }

        /// <remarks>
        /// 2024-09-11 : "Display an error message if the receipt (OR/CR) date falls outside the document date and liquidation date." - CASE: 007578 {JLG}
        /// </remarks>
        protected virtual void _(Events.RowPersisting<ATPTEFMFundTransactionReceiptDetail> e)
        {
            ATPTEFMFundTransactionReceiptDetail row = e.Row;
            ATPTEFMFundTransaction ft = FundTransactions.Current;

            if (row is null || (row.IsImported ?? false)) return;
            if (ft is null || (row.IsImported ?? false)) return;

            if (ft.FundTransactionType == ATPTEFMFundTransactionTypeAttribute.ReimbursementValue)
            {
                if (row.Amount == decimal.Zero)
                {
                    e.Cache.RaiseExceptionHandling<ATPTEFMFundTransactionReceiptDetail.amount>(row, ((ATPTEFMFundTransactionReceiptDetail)e?.Row)?.Amount, ATPTEFMHelper.GetPropertyException((ATPTEFMFundTransactionReceiptDetail)e?.Row, ATPTEFMMessages.AmountMustBeGreaterThanZero, PXErrorLevel.Error));
                    throw new Exception(ATPTEFMMessages.AmountMustBeGreaterThanZero);
                }
                BudgetValidationRaiseErrorForRequestAmountGreaterThanRemainingBudget(null, row, ft?.BudgetEnabled ?? false);
                //PBudgetValidationRaiseErrorForRequestAmountGreaterThanRemainingBudget(null, row, ft?.ProjectBudgetEnabled ?? false);
            }
            else
            {
                BudgetValidationRaiseErrorForLiquidationGreaterThanRequest(ft.BudgetEnabled ?? false);
                PBudgetValidationRaiseErrorForLiquidationAmtGreaterThanRequestAmt(ft.ProjectBudgetEnabled ?? false);
            }

            #region Receipts Tab Validations

            var finID = ATPTEFMHelper.GetFinPeriod(this, ft.Date);

            MasterFinPeriod period = PXSelect<
                MasterFinPeriod,
                Where<MasterFinPeriod.finPeriodID, Equal<Required<MasterFinPeriod.finPeriodID>>>>
                .Select(this, finID);

            #region Financial Period Validation
            bool isMultipleCalendarSupport = EnableFeatures?.Current?.MultipleCalendarsSupport ?? false;

            DateTime periodDate = (DateTime)period.StartDate;

            if (isMultipleCalendarSupport == false && period.Status == FinPeriod.status.Inactive)
            {
                string finPeriodErrMsg = $"The {periodDate.ToString("MM-yyyy")} financial period is inactive.";

                FundTransactions.Cache.RaiseExceptionHandling<ATPTEFMFundTransaction.date>(ft,
                        ft.Date,
                        ATPTEFMHelper.GetPropertyException(ft, finPeriodErrMsg, PXErrorLevel.Error));
            }
            #endregion

            if (row != null && ft?.FundTransactionType == ATPTEFMFundTransactionTypeAttribute.CashAdvanceValue)
            {
                if (row.NetQty == decimal.Zero)
                {
                    FundTransactionReceiptLines.Cache.RaiseExceptionHandling<ATPTEFMFundTransactionReceiptDetail.netQty>(row,
                            row.NetQty,
                            ATPTEFMHelper.GetPropertyException(row, Messages.ATPTEFMMessages.QuantityMustBeGreaterThanZero, PXErrorLevel.Error));
                }

                if (row.NetAmt == decimal.Zero)
                {
                    FundTransactionReceiptLines.Cache.RaiseExceptionHandling<ATPTEFMFundTransactionReceiptDetail.netUnitCost>(row,
                                row.NetUnitCost,
                                ATPTEFMHelper.GetPropertyException(row, Messages.ATPTEFMMessages.AmountMustBeGreaterThanZero, PXErrorLevel.Error));
                }

                if (ft.Date > row.Date)
                {
                    e.Cache.RaiseExceptionHandling<ATPTEFMFundTransactionReceiptDetail.date>(row, ((ATPTEFMFundTransactionReceiptDetail)e?.Row)?.Date, ATPTEFMHelper.GetPropertyException(row, ATPTEFMMessages.ReceiptDateNotLessThanCADocumentDate, PXErrorLevel.Error));
                    throw new Exception(ATPTEFMMessages.ReceiptDateNotLessThanCADocumentDate);
                }

                if (row.Date > ft.LiqDate)
                {
                    e.Cache.RaiseExceptionHandling<ATPTEFMFundTransactionReceiptDetail.date>(row, ((ATPTEFMFundTransactionReceiptDetail)e?.Row)?.Date, ATPTEFMHelper.GetPropertyException(row, ATPTEFMMessages.ReceiptDateNotGreaterThanLiqDate, PXErrorLevel.Error));
                    throw new Exception(ATPTEFMMessages.ReceiptDateNotGreaterThanLiqDate);
                }
            }

            #endregion Receipts Tab Validations


            CheckReplenishmentAmtWarning();

            //object refRow = row.RefNbr;
            //FundTransactionReceiptLines.Cache.RaiseFieldVerifying<ATPTEFMFundTransactionReceiptDetail.refNbr>(row, ref refRow);
        }
        protected virtual void _(Events.RowPersisting<ATPTEFMFundTransaction> e)
        {
            ATPTEFMFundTransaction row = e.Row;
            if (row == null) return;

            if (FundTransactions?.Current?.FundTransactionType == ATPTEFMFundTransactionTypeAttribute.CashAdvanceValue)
            {
                if (FundTransactionDetails.Select().Count == 0) throw new PXException(ATPTEFMMessages.DocumentDetailsShouldNotBeEmpty);

                if (row.DateOfUse == null)
                {
                    e.Cache.RaiseExceptionHandling<ATPTEFMFundTransaction.dateOfUse>(
                        row, null,
                        new PXSetPropertyException(ATPTEFMMessages.DateOfUseCannotBeEmpty, PXErrorLevel.Error));
                    throw new PXException(ATPTEFMMessages.DateOfUseCannotBeEmpty);
                }
            }
            else if (FundTransactions?.Current?.FundTransactionType == ATPTEFMFundTransactionTypeAttribute.ReimbursementValue)
            {
                if (FundTransactionReceiptLines.Select().Count == 0) throw new PXException(ATPTEFMMessages.DocumentDetailsShouldNotBeEmpty);
            }

#if Version24R1
            ATPTEFMFund currentFund = PXSelect<
                                    ATPTEFMFund,
                                    Where<ATPTEFMFund.fundCD, Equal<Required<ATPTEFMFund.fundCD>>>>
                                    .Select(this, row.FundID);
            if (currentFund != null)
            {
                if (row.FundTransactionType == ATPTEFMFundTransactionTypeAttribute.CashAdvanceValue)
                {
                    if (row.ActualSpentAmount == decimal.Zero && (currentFund.FundTransactionAmt != null && currentFund.FundTransactionAmt > 0m) && row.RequestedAmount > currentFund.FundTransactionAmt)
                    {
                        if (currentFund.FundTransactionRestriction == ATPTEFMReplenishmentStringListAttribute.WarningValue)
                            PXUIFieldAttribute.SetWarning<ATPTEFMFundTransaction.requestedAmount>(e.Cache, row, ATPTEFMMessages.FundTransactionGreaterThanFundLimit);

                        else if (currentFund.FundTransactionRestriction == ATPTEFMReplenishmentStringListAttribute.ErrorRestrictValue)
                        {
                            FundTransactions.Cache.RaiseExceptionHandling<ATPTEFMFundTransaction.requestedAmount>(row, row.RequestedAmount, ATPTEFMHelper.GetPropertyException(row, Messages.ATPTEFMMessages.FundTransactionGreaterThanFundLimit, PXErrorLevel.Error));
                        }
                    }
                    else if (row.RequestedAmount == decimal.Zero) throw new PXException(ATPTEFMMessages.TotalAmountCannotBeZero);
                    else if (row.ActualSpentAmount != decimal.Zero && (currentFund.FundTransactionAmt != null && currentFund.FundTransactionAmt > 0m) && row.ActualSpentAmount > currentFund.FundTransactionAmt)
                    {
                        if (currentFund.FundTransactionRestriction == ATPTEFMReplenishmentStringListAttribute.ErrorRestrictValue)
                        {
                            FundTransactions.Cache.RaiseExceptionHandling<ATPTEFMFundTransaction.actualSpentAmount>(row, row.ActualSpentAmount, ATPTEFMHelper.GetPropertyException(row, Messages.ATPTEFMMessages.FundTransactionGreaterThanFundLimit, PXErrorLevel.Error));
                        }
                    }
                }
                else if (row.FundTransactionType == ATPTEFMFundTransactionTypeAttribute.ReimbursementValue)
                {
                    if ((currentFund.FundTransactionAmt != null && currentFund.FundTransactionAmt > 0m) && row.ActualSpentAmount > currentFund.FundTransactionAmt)
                    {
                        if (currentFund.FundTransactionRestriction == ATPTEFMReplenishmentStringListAttribute.WarningValue)
                            PXUIFieldAttribute.SetWarning<ATPTEFMFundTransaction.actualSpentAmount>(e.Cache, row, ATPTEFMMessages.FundTransactionGreaterThanFundLimit);

                        else if (currentFund.FundTransactionRestriction == ATPTEFMReplenishmentStringListAttribute.ErrorRestrictValue)
                        {
                            FundTransactions.Cache.RaiseExceptionHandling<ATPTEFMFundTransaction.actualSpentAmount>(row, row.ActualSpentAmount, ATPTEFMHelper.GetPropertyException(row, Messages.ATPTEFMMessages.FundTransactionGreaterThanFundLimit, PXErrorLevel.Error));
                        }
                    }
                    else if (row.ActualSpentAmount == decimal.Zero) throw new PXException(ATPTEFMMessages.LiquidationAmountCannotBeZero);
                }
            }
#else
            if (Preferences?.Current?.EnableFundTransactionLimit ?? false)
            {
                ATPTEFMFund currentFund = PXSelect<
                    ATPTEFMFund,
                    Where<ATPTEFMFund.fundCD, Equal<Required<ATPTEFMFund.fundCD>>>>
                    .Select(this, row.FundID);
                if (currentFund != null)
                {
                    if (row.FundTransactionType == ATPTEFMFundTransactionTypeAttribute.CashAdvanceValue)
                    {
                        if (row.ActualSpentAmount == decimal.Zero && (currentFund.FundTransactionAmt != null && currentFund.FundTransactionAmt > 0m) && row.RequestedAmount > currentFund.FundTransactionAmt)
                        {
                            if (currentFund.FundTransactionRestriction == ATPTEFMReplenishmentStringListAttribute.WarningValue)
                                PXUIFieldAttribute.SetWarning<ATPTEFMFundTransaction.requestedAmount>(e.Cache, row, ATPTEFMMessages.FundTransactionGreaterThanFundLimit);

                            else if (currentFund.FundTransactionRestriction == ATPTEFMReplenishmentStringListAttribute.ErrorRestrictValue)
                            {
                                FundTransactions.Cache.RaiseExceptionHandling<ATPTEFMFundTransaction.requestedAmount>(row, row.RequestedAmount, ATPTEFMHelper.GetPropertyException(row, Messages.ATPTEFMMessages.FundTransactionGreaterThanFundLimit, PXErrorLevel.Error));
                            }
                        }
                        else if (row.RequestedAmount == decimal.Zero) throw new PXException(ATPTEFMMessages.TotalAmountCannotBeZero);
                        else if (row.ActualSpentAmount != decimal.Zero && (currentFund.FundTransactionAmt != null && currentFund.FundTransactionAmt > 0m) && row.ActualSpentAmount > currentFund.FundTransactionAmt)
                        {
                            if (currentFund.FundTransactionRestriction == ATPTEFMReplenishmentStringListAttribute.ErrorRestrictValue)
                            {
                                FundTransactions.Cache.RaiseExceptionHandling<ATPTEFMFundTransaction.actualSpentAmount>(row, row.ActualSpentAmount, ATPTEFMHelper.GetPropertyException(row, Messages.ATPTEFMMessages.FundTransactionGreaterThanFundLimit, PXErrorLevel.Error));
                            }
                        }
                    }
                    else if (row.FundTransactionType == ATPTEFMFundTransactionTypeAttribute.ReimbursementValue)
                    {
                        if ((currentFund.FundTransactionAmt != null && currentFund.FundTransactionAmt > 0m) && row.ActualSpentAmount > currentFund.FundTransactionAmt)
                        {
                            if (currentFund.FundTransactionRestriction == ATPTEFMReplenishmentStringListAttribute.WarningValue)
                                PXUIFieldAttribute.SetWarning<ATPTEFMFundTransaction.actualSpentAmount>(e.Cache, row, ATPTEFMMessages.FundTransactionGreaterThanFundLimit);

                            else if (currentFund.FundTransactionRestriction == ATPTEFMReplenishmentStringListAttribute.ErrorRestrictValue)
                            {
                                FundTransactions.Cache.RaiseExceptionHandling<ATPTEFMFundTransaction.actualSpentAmount>(row, row.ActualSpentAmount, ATPTEFMHelper.GetPropertyException(row, Messages.ATPTEFMMessages.FundTransactionGreaterThanFundLimit, PXErrorLevel.Error));
                            }
                        }
                        else if (row.ActualSpentAmount == decimal.Zero) throw new PXException(ATPTEFMMessages.LiquidationAmountCannotBeZero);
                    }
                }
            }
#endif
            if ((row.BudgetEnabled ?? false) && (row.IsOverbudget ?? false) && FeatureSetup.Current.BudgetValidation == RQRequestClassBudget.Error)
            {
                throw new PXException(ATPTEFMMessages.OverbudgetWarning);
            }
        }
        protected virtual void ATPTEFMFundTransactionReceiptDetail_RowUpdated(PXCache sender, PXRowUpdatedEventArgs e)
        {
            ATPTEFMFundTransaction receipt = FundTransactions.Current;
            ATPTEFMFundTransactionReceiptDetail row = (ATPTEFMFundTransactionReceiptDetail)e.Row;
            if (row == null) return;

            if (receipt?.FundTransactionType == ATPTEFMFundTransactionTypeAttribute.CashAdvanceValue)
            {
                if (receipt?.ActualSpentAmount != decimal.Zero)
                {
                    receipt.ChangeAmount = (receipt?.RequestedAmount - FundTransactionReceiptLines.Select().RowCast<ATPTEFMFundTransactionReceiptDetail>().Sum(s => s?.NetAmt)) + FundTransactionReceiptLines.Select().RowCast<ATPTEFMFundTransactionReceiptDetail>().Sum(s => s?.WhtAmount);
                    receipt.TotalWhtAmount = FundTransactionReceiptLines.Select().RowCast<ATPTEFMFundTransactionReceiptDetail>().Sum(s => s?.WhtAmount);
                }
                BudgetValidationRaiseErrorForLiquidationGreaterThanRequest(receipt.BudgetEnabled ?? false);
                PBudgetValidationRaiseErrorForLiquidationAmtGreaterThanRequestAmt(receipt.ProjectBudgetEnabled ?? false);
            }

            if (receipt?.FundTransactionType == ATPTEFMFundTransactionTypeAttribute.ReimbursementValue)
            {
                if (receipt?.ActualSpentAmount != decimal.Zero)
                    receipt.TotalWhtAmount = FundTransactionReceiptLines.Select().RowCast<ATPTEFMFundTransactionReceiptDetail>().Sum(s => s?.WhtAmount);
            }

            //object refRow = row.RefNbr;
            //FundTransactionReceiptLines.Cache.RaiseFieldVerifying<ATPTEFMFundTransactionReceiptDetail.refNbr>(row, ref refRow);
        }
        protected virtual void ATPTEFMFundTransactionReceiptDetail_Date_FieldDefaulting(PXCache sender, PXFieldDefaultingEventArgs e)
        {
            e.NewValue = ((DateTime)Accessinfo.BusinessDate);
        }
        protected virtual void ATPTEFMFundTransactionDetail_InventoryID_FieldUpdated(PXCache sender, PXFieldUpdatedEventArgs e)
        {
            ATPTEFMFundTransactionDetail row = e.Row as ATPTEFMFundTransactionDetail;
            if (row == null) return;

            ATPTEFMSetup currentFTSetup = Preferences.Current;

            if (row.InventoryID != null && currentFTSetup.UseExpenseAcctFrom == ATPTEFMFTAccountSource.PurchaseItem)
            {
                InventoryItem item = PXSelect<
                    InventoryItem,
                    Where<InventoryItem.inventoryID, Equal<Required<InventoryItem.inventoryID>>>>
                    .Select(this, row.InventoryID);
                Account accounts = PXSelect<
                    Account,
                    Where<Account.accountID, Equal<Required<Account.accountID>>>>
                    .Select(this, item.COGSAcctID);
                if (item != null)
                {
                    FundTransactionDetails.Cache.SetValueExt<ATPTEFMFundTransactionDetail.accountID>(row, item.COGSAcctID);
                    //row.AccountID = item.COGSAcctID;
                    row.AccountDescription = accounts.Description;
                }
            }
            //Trigger Combine-Sub behavior in Sub ID field defaulting event
            sender.SetDefaultExt<ATPTEFMFundTransactionDetail.subID>(e?.Row);
        }
        /// <remarks>
        /// 2025-03-26 : Remove FieldVerifying, transfer logic to Field Attribute - 010685 - RFS
        /// </remarks>
        //protected virtual void _(Events.FieldVerifying<ATPTEFMFundTransactionReceiptDetail, ATPTEFMFundTransactionReceiptDetail.refNbr> e)
        //{
        //    ATPTEFMFundTransactionReceiptDetail row = e.Row;
        //    if (row == null) return;

        //    if (e.NewValue != null)
        //    {
        //        string checkRefnbr = e.NewValue.ToString();

        //        EPSetup expenseSetup = PXSelect<EPSetup>.Select(this);
        //        ATPTEFMEPSetupExtension expenseSetupExt = expenseSetup.GetExtension<ATPTEFMEPSetupExtension>();

        //        if (expenseSetupExt.UsrATPTEFMRaiseErrorOnDuplicateRefNbr ?? false)
        //        {
        //            ATPTEFMFundTransactionReceiptDetail dup = null;

        //            if (row.VendID == null)
        //            {
        //                dup = PXSelect<
        //                    ATPTEFMFundTransactionReceiptDetail,
        //                    Where<ATPTEFMFundTransactionReceiptDetail.refNbr, Equal<@P.AsString>,
        //                        And<ATPTEFMFundTransactionReceiptDetail.fundTransactionReceiptDetailID, NotEqual<@P.AsInt>,
        //                        And<ATPTEFMFundTransactionReceiptDetail.inventoryID, Equal<@P.AsInt>,
        //                        And<Where<ATPTEFMFundTransactionReceiptDetail.vendID, Equal<Empty>,
        //                            Or<ATPTEFMFundTransactionReceiptDetail.vendID, IsNull>>>>>>>
        //                    .Select(this, checkRefnbr, row.FundTransactionReceiptDetailID, row.InventoryID);
        //            }
        //            else
        //            {
        //                dup = PXSelect<
        //                    ATPTEFMFundTransactionReceiptDetail,
        //                    Where<ATPTEFMFundTransactionReceiptDetail.refNbr, Equal<@P.AsString>,
        //                        And<ATPTEFMFundTransactionReceiptDetail.vendID, Equal<@P.AsInt>,
        //                        And<ATPTEFMFundTransactionReceiptDetail.fundTransactionReceiptDetailID, NotEqual<@P.AsInt>,
        //                        And<ATPTEFMFundTransactionReceiptDetail.inventoryID, Equal<@P.AsInt>>>>>>
        //                    .Select(this, checkRefnbr, row.VendID, row.FundTransactionReceiptDetailID, row.InventoryID);
        //            }

        //            bool? dupEr = DuplicateERRefNbr(row, checkRefnbr);

        //            if (dup != null || (dupEr ?? false))
        //            {
        //                //throw ATPTEFMHelper.GetPropertyException(row, ATPTEFMMessages.RefNbrDuplicate, PXErrorLevel.Error);
        //                e.Cache.RaiseExceptionHandling<ATPTEFMFundTransactionReceiptDetail.refNbr>(row, row.RefNbr, ATPTEFMHelper.GetPropertyException(row, ATPTEFMMessages.RefNbrDuplicate, PXErrorLevel.Error));
        //            }
        //        }
        //    }
        //}
        protected virtual void _(Events.FieldVerifying<ATPTEFMFundTransaction, ATPTEFMFundTransaction.amountReleased> e)
        {
            ATPTEFMFundTransaction row = e.Row;
            if (row == null || (row.IsImported ?? false)) return;

            if (row.Status == ATPTEFMFundStatusAttribute.OpenValue)
            {
                if ((row.ChangeAmount + (decimal)e.NewValue) > 0m)
                {
                    FundTransactions.Cache.RaiseExceptionHandling<ATPTEFMFundTransaction.amountReleased>(row, row.AmountReleased, ATPTEFMHelper.GetPropertyException(row, Messages.ATPTEFMMessages.AmountReleasedCannotBeGreaterThanFundReturn, PXErrorLevel.Error));
                }
            }
        }
        protected virtual void _(Events.FieldVerifying<ATPTEFMFundTransaction, ATPTEFMFundTransaction.amountReceived> e)
        {
            ATPTEFMFundTransaction row = e.Row;
            if (row == null || (row.IsImported ?? false)) return;

            if (row.Status == ATPTEFMFundStatusAttribute.OpenValue)
            {
                if ((decimal)e.NewValue > row.ChangeAmount)
                {
                    FundTransactions.Cache.RaiseExceptionHandling<ATPTEFMFundTransaction.amountReceived>(row, row.AmountReceived, ATPTEFMHelper.GetPropertyException(row, Messages.ATPTEFMMessages.AmountReceivedCannotBeGreaterThanFundReturn, PXErrorLevel.Error));
                }
            }
        }
        protected virtual void _(Events.FieldUpdated<ATPTEFMFundTransaction, ATPTEFMFundTransaction.actualSpentAmount> e)
        {
            ATPTEFMFundTransaction row = e.Row;
            if (row == null || (row.IsImported ?? false)) return;

#if Version24R1
            ATPTEFMFund currentFund = PXSelect<
                                    ATPTEFMFund,
                                    Where<ATPTEFMFund.fundCD, Equal<Required<ATPTEFMFund.fundCD>>>>
                                    .Select(this, row.FundID);
            if (currentFund != null)
            {
                if (row.ActualSpentAmount != decimal.Zero && (currentFund.FundTransactionAmt != null && currentFund.FundTransactionAmt > 0m) && row.ActualSpentAmount > currentFund.FundTransactionAmt && row.FundTransactionType == ATPTEFMFundTransactionTypeAttribute.CashAdvanceValue)
                {
                    if (currentFund.FundTransactionRestriction == ATPTEFMReplenishmentStringListAttribute.WarningValue)
                    {
                        PXUIFieldAttribute.SetWarning<ATPTEFMFundTransaction.actualSpentAmount>(e.Cache, row, ATPTEFMMessages.FundTransactionGreaterThanFundLimit);
                    }
                }
            }
#else
            if (Preferences?.Current?.EnableFundTransactionLimit ?? false)
            {
                ATPTEFMFund currentFund = PXSelect<
                    ATPTEFMFund,
                    Where<ATPTEFMFund.fundCD, Equal<Required<ATPTEFMFund.fundCD>>>>
                    .Select(this, row.FundID);
                if (currentFund != null)
                {
                    if (row.ActualSpentAmount != decimal.Zero && (currentFund.FundTransactionAmt != null && currentFund.FundTransactionAmt > 0m) && row.ActualSpentAmount > currentFund.FundTransactionAmt && row.FundTransactionType == ATPTEFMFundTransactionTypeAttribute.CashAdvanceValue)
                    {
                        if (currentFund.FundTransactionRestriction == ATPTEFMReplenishmentStringListAttribute.WarningValue)
                        {
                            PXUIFieldAttribute.SetWarning<ATPTEFMFundTransaction.actualSpentAmount>(e.Cache, row, ATPTEFMMessages.FundTransactionGreaterThanFundLimit);
                        }
                    }
                }
            }
#endif
        }
        /// <remarks>
        /// 2025-08-28 : Once the vendor ID is cleared in the Receipts tab, the vendor name, TIN  and address should also be cleared. CASEID: 012141 {JLG} <br/>             
        /// </remarks>
        protected virtual void _(Events.FieldUpdated<ATPTEFMFundTransactionReceiptDetail, ATPTEFMFundTransactionReceiptDetail.vendID> e)
        {
            var row = e.Row;

            if (row == null) return;

            if (row.VendID != null)
            {
                Location taxZoneID = PXSelectJoin<
                    Location,
                    LeftJoin<Vendor,
                        On<Location.bAccountID, Equal<Vendor.bAccountID>,
                        And<Vendor.defLocationID, Equal<Location.locationID>>>>,
                    Where<Vendor.bAccountID, Equal<Required<Vendor.bAccountID>>>>
                    .Select(this, row.VendID);

                row.TaxZoneID = taxZoneID.VTaxZoneID;

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

                row.VendorName = vendor.AcctName;
                row.VendorAddress = address.AddressLine1;
                row.VendorTin = taxZoneID.TaxRegistrationID;
            }
            else 
            {
                if(!string.IsNullOrEmpty(e.OldValue.ToString()) && string.IsNullOrEmpty(row.VendID.ToString())) 
                {
                    row.VendorName = string.Empty;
                    row.VendorAddress = string.Empty;
                    row.VendorTin = string.Empty;
                }
            
            }
        }
        protected virtual void _(Events.FieldUpdated<ATPTEFMFundTransactionReceiptDetail, ATPTEFMFundTransactionReceiptDetail.projectID> e)
        {
            var row = e.Row;

            if (row is null || (row.IsImported ?? false)) return;

            row.ProjectTaskID = null;
            row.CostCodeID = null;
        }
        protected virtual void _(Events.RowDeleted<ATPTEFMFundTransactionReceiptDetail> e)
        {
            ATPTEFMFundTransaction receipt = FundTransactions.Current;
            receipt.Step = ATPTEFMFundTransactionStepAttribute.DefaultValue;
        }
        protected virtual void _(Events.FieldUpdated<ATPTEFMFundTransaction, ATPTEFMFundTransaction.dateOfUse> e)
        {
            ATPTEFMFundTransaction row = e.Row;
            if (row == null) return;

            var liquidationDate = (Preferences.Current.LiquidationDateBasedOnWorkCalendar ?? false) ? GetLiquidationDateWorkCalendar() : row.DateOfUse.Value.AddDays(GetNumberOfLiquidationDays());

            row.LiqDate = liquidationDate;
            row.InitialLiqDate = liquidationDate;
            row.UnmodifiedLiqDate = liquidationDate;
        }
        /// <remarks>
        /// 2026-01-07 : )Fund transaction: Clearing of fund ID once the request by is overridden by another requestor. CASEID: 0014747 {JLG} <br/>             
        /// </remarks>
        protected virtual void _(Events.FieldUpdated<ATPTEFMFundTransaction, ATPTEFMFundTransaction.requestedByID> e)
        {
            ATPTEFMFundTransaction fundTran = e.Row;
            if (fundTran is null) return;

            if (e.OldValue != null && !object.Equals(e.OldValue, fundTran.RequestedByID))
            {
                e.Cache.SetValueExt<ATPTEFMFundTransaction.fundID>(fundTran, null);
            }

            EPEmployee oldEmployeeObj = PXSelect<
                EPEmployee,
                Where<EPEmployee.bAccountID, Equal<Required<EPEmployee.bAccountID>>>>
                .Select(this, e.OldValue);

            EPEmployee currentEmployeeObj = RequestorEmployee.Select();

            if (currentEmployeeObj != null && oldEmployeeObj != null)
            {
                if (oldEmployeeObj.CalendarID != currentEmployeeObj.CalendarID)
                {
                    fundTran.DateOfUse = null;
                    fundTran.InitialLiqDate = null;
                    fundTran.LiqDate = null;
                }
            }
        }
        protected virtual void _(Events.FieldUpdated<ATPTEFMFundTransaction, ATPTEFMFundTransaction.liqDate> e)
        {
            ATPTEFMFundTransaction row = e.Row;
            if (row != null)
            {
                if (row.Hold ?? false)
                    row.InitialLiqDate = row.LiqDate;
            }
        }
        protected virtual void _(Events.FieldUpdated<ATPTEFMFundTransaction, ATPTEFMFundTransaction.initialLiqDate> e)
        {
            ATPTEFMFundTransaction row = e.Row;
            if (row != null)
            {
                if (row.Hold ?? false)
                    row.LiqDate = row.InitialLiqDate;
            }
        }
        protected virtual void _(Events.FieldUpdated<ATPTEFMFundTransactionDetail, ATPTEFMFundTransactionDetail.amount> e)
        {
            ATPTEFMFundTransactionDetail row = e.Row;
            if (row == null || (row.IsImported ?? false)) return;

            ATPTEFMFundTransaction ft = FundTransactions.Current;

            BudgetValidationRaiseErrorForRequestAmountGreaterThanRemainingBudget(row, null, ft.BudgetEnabled ?? false);
            PBudgetValidationRaiseErrorForRequestAmountGreaterThanRemainingBudget(row, null, ft?.ProjectBudgetEnabled ?? false);
        }
        protected virtual void _(Events.FieldUpdated<ATPTEFMFundTransactionReceiptDetail, ATPTEFMFundTransactionReceiptDetail.amount> e)
        {
            ATPTEFMFundTransactionReceiptDetail row = e.Row;
            if (row == null || (row.IsImported ?? false)) return;

            ATPTEFMFundTransaction ft = FundTransactions.Current;

            BudgetValidationRaiseErrorForRequestAmountGreaterThanRemainingBudget(null, row, ft?.BudgetEnabled ?? false);
            PBudgetValidationRaiseErrorForRequestAmountGreaterThanRemainingBudget(null, row, ft?.ProjectBudgetEnabled ?? false);
        }
        protected virtual void _(Events.FieldDefaulting<ATPTEFMFundTransactionDetail, ATPTEFMFundTransactionDetail.accountID> e)
        {
            ATPTEFMFundTransactionDetail row = e.Row;
            if (row is null) return;

            ATPTEFMFundTransaction ft = FundTransactions.Current;
            if (ft != null)
            {
                ATPTEFMSetup currentFTSetup = Preferences.Current;
                if (currentFTSetup != null)
                {
                    if (currentFTSetup.UseExpenseAcctFrom.IsNullOrEmpty())
                        throw new PXException(ATPTEFMMessages.FTSetupGLAccountValidation);

                    #region Default AccountID
                    switch (currentFTSetup.UseExpenseAcctFrom)
                    {
                        case RQAccountSource.None:
                            e.NewValue = null;
                            break;
                        case RQAccountSource.Requester:
                            EPEmployee requestBy = PXSelect<
                                EPEmployee,
                                Where<EPEmployee.bAccountID, Equal<Required<EPEmployee.bAccountID>>>>
                                .Select(this, ft.RequestedByID);
                            e.NewValue = requestBy?.ExpenseAcctID;
                            break;
                        case RQAccountSource.Department:
                            EPDepartment department = PXSelect<
                                EPDepartment,
                                Where<EPDepartment.departmentID, Equal<Required<EPDepartment.departmentID>>>>
                                .Select(this, ft.DepartmentID);
                            e.NewValue = department?.ExpenseAccountID;
                            break;
                            //For Item, auto populate behavior is found on InventoryID field updated. Reason for this is that InventoryID has no default value meaning this case in the switch statement will always be null
                            //case RQAccountSource.PurchaseItem:
                            //    InventoryItem item = InventoryItem.PK.Find(this, row?.InventoryID);
                            //    e.NewValue = item?.COGSAcctID;
                            //    break;
                    }
                    #endregion
                }
            }
        }
        protected virtual void _(Events.FieldDefaulting<ATPTEFMFundTransactionDetail, ATPTEFMFundTransactionDetail.subID> e)
        {
            ATPTEFMFundTransactionDetail row = e.Row;
            if (row is null) return;

            ATPTEFMFundTransaction ft = FundTransactions.Current;
            ATPTEFMSetup ftSetup = Preferences.Current;

            if (ft != null)
            {
                if ((ftSetup.CombineExpSub.Contains(ATPTEFMFTAcctSubDefault.MaskItem) && row.InventoryID != null) || (!ftSetup.CombineExpSub.Contains(ATPTEFMFTAcctSubDefault.MaskItem)))
                {
                    //Purchase Item
                    InventoryItem item = InventoryItem.PK.Find(this, row.InventoryID);
                    int? inventorySubID = item?.COGSSubID;

                    //Department
                    EPDepartment department = PXSelect<
                        EPDepartment,
                        Where<EPDepartment.departmentID, Equal<Required<EPDepartment.departmentID>>>>
                        .Select(this, ft?.DepartmentID);
                    int? departmentSubID = department?.ExpenseSubID;

                    //Requesters
                    EPEmployee requestBy = PXSelect<
                        EPEmployee,
                        Where<EPEmployee.bAccountID, Equal<Required<EPEmployee.bAccountID>>>>
                        .Select(this, ft?.RequestedByID);
                    int? requesterSubID = requestBy?.ExpenseSubID;

                    object value = ATPTEFMSubAccountMaskExtension.MakeSub<ATPTEFMSetup.combineExpSub>(this, ftSetup.CombineExpSub,
                                    new object[] { departmentSubID, inventorySubID, requesterSubID },
                                    new Type[] { typeof(EPDepartment.expenseSubID), typeof(InventoryItem.cOGSSubID), typeof(EPEmployee.expenseSubID) });

                    e.Cache.RaiseFieldUpdating<ATPTEFMFundTransactionDetail.subID>(e?.Row, ref value);
                    e.NewValue = (int?)value;
                }
            }
        }
        protected virtual void _(Events.FieldDefaulting<ATPTEFMFundTransactionReceiptDetail, ATPTEFMFundTransactionReceiptDetail.accountID> e)
        {
            ATPTEFMFundTransactionReceiptDetail row = e.Row;
            if (row is null) return;

            ATPTEFMFundTransaction ft = FundTransactions.Current;
            if (ft != null)
            {
                ATPTEFMSetup currentFTSetup = Preferences.Current;
                if (currentFTSetup != null)
                {
                    if (currentFTSetup.UseExpenseAcctFrom.IsNullOrEmpty())
                        throw new PXException(ATPTEFMMessages.FTSetupGLAccountValidation);

                    #region Default AccountID
                    switch (currentFTSetup.UseExpenseAcctFrom)
                    {
                        case RQAccountSource.None:
                            e.NewValue = null;
                            break;
                        case RQAccountSource.Requester:
                            EPEmployee requestBy = PXSelect<
                                EPEmployee,
                                Where<EPEmployee.bAccountID, Equal<Required<EPEmployee.bAccountID>>>>
                                .Select(this, ft.RequestedByID);
                            e.NewValue = requestBy?.ExpenseAcctID;
                            break;
                        case RQAccountSource.Department:
                            EPDepartment department = PXSelect<
                                EPDepartment,
                                Where<EPDepartment.departmentID, Equal<Required<EPDepartment.departmentID>>>>
                                .Select(this, ft.DepartmentID);
                            e.NewValue = department?.ExpenseAccountID;
                            break;
                            //For Item, auto populate behavior is found on InventoryID field updated. Reason for this is that InventoryID has no default value meaning this case in the switch statement will always be null
                            //case RQAccountSource.PurchaseItem:
                            //    InventoryItem item = InventoryItem.PK.Find(this, row?.InventoryID);
                            //    e.NewValue = item?.COGSAcctID;
                            //    break;
                    }
                    #endregion
                }
            }
        }
        protected virtual void _(Events.FieldDefaulting<ATPTEFMFundTransactionReceiptDetail, ATPTEFMFundTransactionReceiptDetail.subID> e)
        {
            ATPTEFMFundTransactionReceiptDetail row = e.Row;
            if (row is null) return;

            ATPTEFMFundTransaction ft = FundTransactions.Current;
            ATPTEFMSetup ftSetup = Preferences.Current;

            if (ft != null)
            {
                if ((ftSetup.CombineExpSub.Contains(ATPTEFMFTAcctSubDefault.MaskItem) && row.InventoryID != null) || (!ftSetup.CombineExpSub.Contains(ATPTEFMFTAcctSubDefault.MaskItem)))
                {
                    //Purchase Item
                    InventoryItem item = InventoryItem.PK.Find(this, row?.InventoryID);
                    int? inventorySubID = item?.COGSSubID;

                    //Department
                    EPDepartment department = PXSelect<
                        EPDepartment,
                        Where<EPDepartment.departmentID, Equal<Required<EPDepartment.departmentID>>>>
                        .Select(this, ft?.DepartmentID);
                    int? departmentSubID = department?.ExpenseSubID;

                    //Requesters
                    EPEmployee requestBy = PXSelect<
                        EPEmployee,
                        Where<EPEmployee.bAccountID, Equal<Required<EPEmployee.bAccountID>>>>
                        .Select(this, ft?.RequestedByID);
                    int? requesterSubID = requestBy?.ExpenseSubID;

                    object value = ATPTEFMSubAccountMaskExtension.MakeSub<ATPTEFMSetup.combineExpSub>(this, ftSetup.CombineExpSub,
                                    new object[] { departmentSubID, inventorySubID, requesterSubID },
                                    new Type[] { typeof(EPDepartment.expenseSubID), typeof(InventoryItem.cOGSSubID), typeof(EPEmployee.expenseSubID) });

                    e.Cache.RaiseFieldUpdating<ATPTEFMFundTransactionReceiptDetail.subID>(e?.Row, ref value);
                    e.NewValue = (int?)value;
                }
            }
        }
        protected virtual void _(Events.FieldUpdated<ATPTEFMFundTransactionReceiptDetail, ATPTEFMFundTransactionReceiptDetail.inventoryID> e)
        {
            ATPTEFMFundTransactionReceiptDetail row = e.Row;
            if (row == null) return;

            ATPTEFMSetup currentFTSetup = Preferences.Current;

            if (row.InventoryID != null && currentFTSetup.UseExpenseAcctFrom == ATPTEFMFTAccountSource.PurchaseItem)
            {
                InventoryItem item = PXSelect<
                    InventoryItem,
                    Where<InventoryItem.inventoryID, Equal<Required<InventoryItem.inventoryID>>>>
                    .Select(this, row.InventoryID);
                Account accounts = PXSelect<
                    Account,
                    Where<Account.accountID, Equal<Required<Account.accountID>>>>
                    .Select(this, item.COGSAcctID);
                if (item != null)
                {
                    row.AccountID = item.COGSAcctID;
                    row.AccountDescription = accounts.Description;
                }
            }
            //Trigger Combine-Sub behavior in Sub ID field defaulting event
            e.Cache.SetDefaultExt<ATPTEFMFundTransactionReceiptDetail.subID>(e?.Row);
        }

        #endregion Events

        #region Action

        public PXInitializeState<ATPTEFMFundTransaction> InitializeState;

        public PXAction<ATPTEFMFundTransaction> CreateAPBill;
        [PXProcessButton()]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.CreateAPBill, Visible = false)]
        public IEnumerable createAPBill(PXAdapter adapter)
        {
            ATPTEFMHelper.StartLongOperation(this, adapter, delegate ()
            {
                string docType = string.Empty;
                using (PXTransactionScope ts = new PXTransactionScope())
                {
                    ATPTEFMFundTransaction ft = FundTransactions.Current;

                    if (ft != null)
                    {
                        APInvoiceEntry invEntry = PXGraph.CreateInstance<APInvoiceEntry>();
                        APInvoice inv = new APInvoice();

                        inv = (APInvoice)invEntry.Document.Cache.Insert(inv);

                        if (ft.FundTransactionType == ATPTEFMFundTransactionTypeAttribute.IncreaseFundValue)
                        {
                            docType = APDocType.Invoice;
                        }

                        if (ft.FundTransactionType == ATPTEFMFundTransactionTypeAttribute.DecreaseFundValue)
                        {
                            docType = APDocType.DebitAdj;
                        }

                        invEntry.Document.Cache.SetValueExt<APInvoice.docType>(inv, docType);
                        invEntry.Document.Cache.SetValueExt<APInvoice.docDate>(inv, ft.Date);

                        if (ft.FundTransactionType == ATPTEFMFundTransactionTypeAttribute.IncreaseFundValue || ft.FundTransactionType == ATPTEFMFundTransactionTypeAttribute.DecreaseFundValue)
                        {
                            ATPTEFMFundMaint fundGraph = PXGraph.CreateInstance<ATPTEFMFundMaint>();
                            ATPTEFMFund fund = fundGraph.Document.Search<ATPTEFMFund.fundCD>(FundTransactions.Current.FundID);

                            invEntry.Document.Cache.SetValueExt<APInvoice.vendorID>(inv, fund.PayeeID);
                        }
                        else
                        {
                            invEntry.Document.Cache.SetValueExt<APInvoice.vendorID>(inv, ft.RequestedByID);
                        }

                        invEntry.Document.Cache.SetValueExt<APInvoice.invoiceNbr>(inv, ft.RefNbr);

                        Decimal? totalAmount = 0;

                        inv.PaymentsByLinesAllowed = false;

                        foreach (ATPTEFMFundTransactionDetail detail in FundTransactionDetails.Select())
                        {
                            APTran tranDoc = new APTran();

                            tranDoc = (APTran)invEntry.Transactions.Insert(tranDoc);

                            tranDoc.TranDesc = detail.Particulars;
                            tranDoc.Qty = detail.Qty;
                            tranDoc.CuryUnitCost = detail.UnitCost;
                            tranDoc.UnitCost = detail.UnitCost;
                            tranDoc.CuryTranAmt = ((decimal?)detail.Qty * detail.UnitCost).RoundDecimal(2);
                            tranDoc.AccountID = detail.AccountID;
                            tranDoc.SubID = detail.SubID;
                            tranDoc.ProjectID = detail.ProjectID;
                            tranDoc.TaskID = detail.ProjectTaskID;
                            tranDoc.CostCodeID = detail.CostCodeID;
                            totalAmount += ((decimal?)detail.Qty * detail.UnitCost).RoundDecimal(2);

                            invEntry.Transactions.Update(tranDoc);
                        }

                        inv.CuryOrigDocAmt = totalAmount;

                        inv = invEntry.Document.Update(inv);

                        invEntry.Save.Press();

                        ft.Status = ATPTEFMFundStatusAttribute.ClosedValue;
                        ft.InvoiceRefNbr = inv.RefNbr;
                        FundTransactions.Update(ft);
                        this.Save.Press();
                    }
                    ts.Complete();

                    //Open AP Bill
                    APInvoiceEntry graphAP = PXGraph.CreateInstance<APInvoiceEntry>();

                    graphAP.Document.Current = graphAP.Document.Search<APInvoice.refNbr>(ft.InvoiceRefNbr, docType);

                    throw new PXRedirectRequiredException(graphAP, true, string.Empty) { Mode = PXBaseRedirectException.WindowMode.NewWindow };
                }
            });
            return adapter.Get();
        }

        public PXAction<ATPTEFMFundTransaction> ReleaseCash;
        [PXButton(Connotation = PX.Data.WorkflowAPI.ActionConnotation.Success)]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.ReleaseCash, MapEnableRights = PXCacheRights.Update, MapViewRights = PXCacheRights.Update)]
        public virtual IEnumerable releaseCash(PXAdapter adapter)
        {
            #region Validations
            CheckFundOnHandAmount();
            CheckReplenishmentAmtWarning();
            #endregion

            if (FundTransactionDetails.Select().Count == 0 && FundTransactions?.Current?.FundTransactionType == ATPTEFMFundTransactionTypeAttribute.CashAdvanceValue) throw new PXException(ATPTEFMMessages.DocumentDetailsShouldNotBeEmpty);
            ATPTEFMHelper.StartLongOperation(this, adapter, delegate ()
            {
                using (PXTransactionScope ts = new PXTransactionScope())
                {
                    ATPTEFMFundTransaction fundTransaction = FundTransactions.Current;

                    if (fundTransaction == null) return;

                    #region Get Fund views
                    Fund.Current = Fund.Select(fundTransaction.FundID);
                    TransactionHistoryView.Current = TransactionHistoryView.Select(fundTransaction.RefNbr);
                    #endregion

                    #region Conditional Variables
                    bool isFundLiquidated = ATPTEFMFundTransactionHelper.IsFundLiquidated(FundTransactions.Current);
                    bool isFundUnliquidated = ATPTEFMFundTransactionHelper.IsFundUnliquidated(FundTransactions.Current);
                    bool isFundRequest = ATPTEFMFundTransactionHelper.IsFundRequest(FundTransactions.Current);
                    bool isFundReimbursement = ATPTEFMFundTransactionHelper.IsFundReimbursement(FundTransactions.Current);
                    #endregion

                    #region Request Type
                    if (fundTransaction.FundTransactionType == ATPTEFMFundTransactionTypeAttribute.CashAdvanceValue && fundTransaction.CashAdvanceStatus == ATPTEFMFundTransactionCashAdvanceStatusAttribute.UnreleasedValue)
                    {
                        fundTransaction.CashAdvanceStatus = ATPTEFMFundTransactionCashAdvanceStatusAttribute.UnliquidatedValue;
                        fundTransaction.ShowBudgetValidation = true;
                        fundTransaction.ReleasedAmount = fundTransaction.RequestedAmount;

                        #region Subject for removal for optimization
                        /*CheckReplenishmentAmtWarning();
                        CheckFundOnHandAmount();*/
                        #endregion

                        #region Fund Updates

                        if (Fund.Current != null)
                        {
                            Fund.Current.CuryBalanceAmt -= fundTransaction.RequestedAmount;
                            Fund.Current.CuryUnliquidatedAmt += fundTransaction.RequestedAmount;
                            Fund.UpdateCurrent();
                        }

                        if (TransactionHistoryView.Current != null)
                        {
                            #region Update Running Balance
                            ReleaseCashRequestTypeRunningBalance();
                            #endregion
                        }
                        #endregion
                    }
                    #endregion

                    #region Reimbursement Type
                    if (fundTransaction.FundTransactionType == ATPTEFMFundTransactionTypeAttribute.ReimbursementValue && fundTransaction.Step == ATPTEFMFundTransactionStepAttribute.SubmitReceiptValue)
                    {
                        AllReceiptsMustOpenStatus(fundTransaction.RefNbr);
                        StringBuilder lastReceiptNbr = new StringBuilder();
                        foreach (ATPTEFMFundTransactionReceiptDetail receipt in FundTransactionReceiptLines.Select())
                        {
                            lastReceiptNbr.Clear();
                            lastReceiptNbr.Append(receipt.ExpenseReceiptRefNbr);
                            receipt.IsLiquidated = true;
                            FundTransactionReceiptLines.Update(receipt);

                            TransactionHistoryView.Current = TransactionHistoryView.Select(receipt.ExpenseReceiptRefNbr);
                            if (TransactionHistoryView.Current != null)
                            {
                                TransactionHistoryView.Current.Status = ATPTEFMFundStatusAttribute.LiquidatedValue;
                                TransactionHistoryView.UpdateCurrent();
                            }

                        }
                        fundTransaction.Status = ATPTEFMFundStatusAttribute.ClosedValue;
                        fundTransaction.CashAdvanceStatus = ATPTEFMFundTransactionCashAdvanceStatusAttribute.LiquidatedValue;
                        fundTransaction.ReceivedAmount = fundTransaction.ActualSpentAmount;
                        fundTransaction.Step = ATPTEFMFundTransactionStepAttribute.LiquidatedReceiptValue;

                        #region Deduct On Hand
                        decimal? liquidatedAmt = fundTransaction.ActualSpentAmount - fundTransaction.TotalWhtAmount;

                        if (isFundReimbursement)
                        {
                            Fund.Current.CuryBalanceAmt -= liquidatedAmt;
                        }
                        Fund.UpdateCurrent();
                        #endregion

                        #region Fund Reimbursement Insertion
                        TransactionHistoryView.Current = TransactionHistoryView.Select(lastReceiptNbr.ToString());
                        if (TransactionHistoryView.Current != null)
                        {
                            decimal? lastBalanceAmt = TransactionHistoryView.Current.CuryBalanceAmt;
                            string transactionType = TransactionType();
                            int rowCount = FundTransactionReceiptLines.Select().Count() + 1;

                            ATPTEFMFundTransactionHistoryView transactionHistory = TransactionHistoryView.Insert();
                            transactionHistory.FundRefNbr = FundTransactions.Current.FundID;
                            transactionHistory.TransactionType = transactionType;
                            transactionHistory.OrderDate = FundTransactions.Current.Date;
                            transactionHistory.RefNbr = FundTransactions.Current.RefNbr;
                            transactionHistory.FundBranchID = FundTransactions.Current.BranchID;
                            transactionHistory.FundType = FundTransactions.Current.FundType;
                            transactionHistory.TransactionDate = FundTransactions.Current.Date;
                            transactionHistory.CuryFundTransactionDocumentAmt = FundTransactions.Current.ActualSpentAmount;
                            transactionHistory.Status = FundTransactions.Current.Status;
                            transactionHistory.Source = ATPTEFMFundTransactionHistoryView.source.FundTransaction;
                            transactionHistory.CuryUnliquidatedAmt = decimal.Zero;
                            transactionHistory.CashAdvanceStatus = FundTransactions.Current.CashAdvanceStatus;
                            transactionHistory.CuryFundAmt = FundTransactions.Current.ActualSpentAmount.GetValueOrDefault();
                            transactionHistory.CuryBalanceAmt = (lastBalanceAmt - FundTransactions.Current.ActualSpentAmount) + FundTransactions.Current.TotalWhtAmount;
                            transactionHistory.SortNbr = $"FT-{FundTransactions.Current.RefNbr}-{rowCount}";
                            TransactionHistoryView.Update(transactionHistory);
                        }
                        #endregion
                    }
                    #endregion

                    #region Update Current Transaction
                    fundTransaction.IsReleasedCash = true;
                    FundTransactions.Update(fundTransaction);
                    #endregion

                    Save.Press();
                    ts.Complete();
                }
            });
            return adapter.Get();
        }

        /// <remarks>
        /// 2025-02-21 : "Distorted fund history occurs after multiple replenishment bills are processed and paid" - CASE: 010429  {JLG} <br/> 
        /// 2025-10-01 : 013750 - LFC - Line Attachments from Fund Transaction to Replenishment. {JLTG}
        /// </remarks>
        public PXAction<ATPTEFMFundTransaction> SubmitReceipts;
        [PXButton(Category = "Actions", Connotation = PX.Data.WorkflowAPI.ActionConnotation.Success)]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.SubmitReceipts, MapEnableRights = PXCacheRights.Update, MapViewRights = PXCacheRights.Update)]
        public virtual IEnumerable submitReceipts(PXAdapter adapter)
        {
            #region Validitions
            CheckFundOnHandAmount();
            CheckReplenishmentAmtWarning();
            #endregion

            ATPTEFMHelper.StartLongOperation(this, adapter, delegate ()
            {
                using (PXTransactionScope ts = new PXTransactionScope())
                {
                    ATPTEFMFundTransaction fundTransaction = FundTransactions.Current;
                    if (fundTransaction == null) return;

                    ATPTEFMFund fund = Fund.Select(fundTransaction.FundID);

                    #region Variables
                    int sortCount = FundTransactionReceiptLines.Select().RowCast<ATPTEFMFundTransactionReceiptDetail>().Where(r => !string.IsNullOrEmpty(r.ExpenseReceiptRefNbr)).Count();
                    bool isFundRequest = ATPTEFMFundTransactionHelper.IsFundRequest(FundTransactions.Current);
                    bool isFundReimbursement = ATPTEFMFundTransactionHelper.IsFundReimbursement(FundTransactions.Current);
                    #endregion

                    #region If one of the receipts of this fund transaction has already been canceled. (FOR REIMBURSEMENT ONLY)
                    //Dont Remove this code for future scenario canellation of reimbursement
                    /*decimal? netAmt = 0m;
                    if (isFundReimbursement)
                    {

                        foreach (ATPTEFMFundTransactionReceiptDetail receipt in FundTransactionReceiptLines.Select().RowCast<ATPTEFMFundTransactionReceiptDetail>().Where(r => !string.IsNullOrEmpty(r.ExpenseReceiptRefNbr)))
                        {
                            netAmt += (receipt.Amount - receipt.WhtAmount);
                        }
                    }*/
                    #endregion

                    #region To determine if the receipts tab already has processed receipts with some logics for calculations. 
                    var containReceiptNbrCount = FundTransactionReceiptLines.Select().RowCast<ATPTEFMFundTransactionReceiptDetail>().Where(x => !string.IsNullOrEmpty(x.ExpenseReceiptRefNbr)).Count();
                    var containLiquidatedReceiptNbrCount = FundTransactionReceiptLines.Select().RowCast<ATPTEFMFundTransactionReceiptDetail>().Where(x => !string.IsNullOrEmpty(x.ExpenseReceiptRefNbr) && x.IsLiquidated == true).Count();

                    decimal? sumAmtOfReceiptWithoutErNbr = FundTransactionReceiptLines.Select().FirstTableItems.ToList()
                            .Where(x => string.IsNullOrEmpty(x.ExpenseReceiptRefNbr))
                            .Select(x => x.NetAmt).Sum();

                    decimal? sumAmtOfLiquidatedReceipts = FundTransactionReceiptLines.Select().FirstTableItems.ToList()
                            .Where(x => !string.IsNullOrEmpty(x.ExpenseReceiptRefNbr) && x.IsLiquidated == true)
                            .Select(x => x.NetAmt).Sum();

                    decimal? sumOfWhtWithLiquidatedReceipts = FundTransactionReceiptLines.Select().FirstTableItems.ToList()
                            .Where(x => !string.IsNullOrEmpty(x.ExpenseReceiptRefNbr) && x.IsLiquidated == true)
                            .Select(x => x.WhtAmount).Sum();

                    decimal? totalWhtAmountForReimbursement = decimal.Zero;
                    #endregion

                    string fundType = (fundTransaction.FundType.Equals(ATPTEFMFundTypeAttribute.PettyCashValue)) ? "PCF" : "RVF";
                    string fundTransactionType = (fundTransaction.FundTransactionType.Equals(ATPTEFMFundTransactionTypeAttribute.CashAdvanceValue)) ? "Request" : "Reimbursement";

                    ExpenseClaimDetailEntry entry = PXGraph.CreateInstance<ExpenseClaimDetailEntry>();
                    
                    foreach (ATPTEFMFundTransactionReceiptDetail receipt in FundTransactionReceiptLines.Select().RowCast<ATPTEFMFundTransactionReceiptDetail>().Where(r => string.IsNullOrEmpty(r.ExpenseReceiptRefNbr)))
                    {
                        entry.Clear();

                        if (receipt == null) continue;
                        sortCount++;

                        InventoryItem item = InventoryItem.PK.Find(this, receipt.InventoryID);

                        EPExpenseClaimDetails claim = new EPExpenseClaimDetails();

                        claim = entry.ClaimDetails.Insert(claim);
                        ATPTEFMEPExpenseClaimDetailsExt claimExt = claim.GetExtension<ATPTEFMEPExpenseClaimDetailsExt>();

                        claimExt.UsrATPTEFMTranType = ATPTEFMExpenseTypeAttribute.Replenishment;
                        claimExt.UsrATPTEFMFundType = fundTransaction.FundType;
                        claim.EmployeeID = fundTransaction.RequestedByID;
                        claim = entry.ClaimDetails.Update(claim);

                        claimExt.UsrATPTEFMRequestRefNbr = fundTransaction.RefNbr;
                        claimExt.UsrATPTEFMFundReturn = receipt.FundReturn;
                        claim.BranchID = receipt.BranchID;
                        claim.ExpenseDate = receipt.Date;
                        entry.ClaimDetails.Cache.SetValueExt<EPExpenseClaimDetails.inventoryID>(claim, receipt.InventoryID);
                        claim.TranDesc = $"{fundType} {fundTransactionType} {fundTransaction.RefNbr} {receipt.LineDescription}";

                        if (fundTransaction.FundTransactionType == ATPTEFMFundTransactionTypeAttribute.CashAdvanceValue)
                        {
                            entry.ClaimDetails.Cache.SetValueExt<EPExpenseClaimDetails.qty>(claim, receipt.NetQty);
                            entry.ClaimDetails.Cache.SetValueExt<EPExpenseClaimDetails.curyUnitCost>(claim, receipt.NetUnitCost);
                        }
                        else if (fundTransaction.FundTransactionType == ATPTEFMFundTransactionTypeAttribute.ReimbursementValue)
                        {
                            entry.ClaimDetails.Cache.SetValueExt<EPExpenseClaimDetails.qty>(claim, receipt.Qty);
                            entry.ClaimDetails.Cache.SetValueExt<EPExpenseClaimDetails.curyUnitCost>(claim, receipt.UnitCost);
                        }

                        claim.UOM = item.BaseUnit;
                        claim.ExpenseRefNbr = receipt.RefNbr;
                        claim.ContractID = receipt.ProjectID;
                        claim.TaskID = receipt.ProjectTaskID;
                        claim.CostCodeID = receipt.CostCodeID;
                        claim.ExpenseAccountID = receipt.AccountID;
                        claim.ExpenseSubID = receipt.SubID;

                        if (receipt.VendID != null)
                        {
                            PXResultset<Vendor, BAccount, Address, LocationExtAddress> vendorInfo = PXSelectJoin<
                                Vendor,
                                InnerJoin<BAccount,
                                    On<BAccount.bAccountID, Equal<Vendor.bAccountID>>,
                                LeftJoin<Address,
                                    On<Address.addressID, Equal<BAccount.defAddressID>>,
                                LeftJoin<LocationExtAddress,
                                    On<LocationExtAddress.bAccountID, Equal<BAccount.bAccountID>>>>>,
                                Where<Vendor.bAccountID, Equal<Required<Vendor.bAccountID>>>>
                                .Select<PXResultset<Vendor, BAccount, Address, LocationExtAddress>>(this, receipt.VendID);

                            Vendor vendor = (Vendor)vendorInfo;
                            Address address = (Address)vendorInfo;
                            LocationExtAddress extAddress = (LocationExtAddress)vendorInfo;

                            //claimExt.UsrATPTEFMDetailVendorID = receipt.VendID;
                            //claimExt.UsrATPTVendID = vendor.AcctCD;
                            //claimExt.UsrATPTVendName = receipt.VendorName;
                            //claimExt.UsrATPTAddress = receipt.VendorAddress;
                            //claimExt.UsrATPTVendTIN = receipt.VendorTin;
                            entry.ClaimDetails.Cache.SetValueExt<Extensions.DAC.ATPTEFMEPExpenseClaimDetailsExt.usrATPTEFMDetailVendorID>(claim, receipt.VendID);
                            entry.ClaimDetails.Cache.SetValueExt<Extensions.DAC.ATPTEFMEPExpenseClaimDetailsExt.usrATPTVendID>(claim, vendor.AcctCD);
                            entry.ClaimDetails.Cache.SetValueExt<Extensions.DAC.ATPTEFMEPExpenseClaimDetailsExt.usrATPTVendName>(claim, receipt.VendorName);
                            entry.ClaimDetails.Cache.SetValueExt<Extensions.DAC.ATPTEFMEPExpenseClaimDetailsExt.usrATPTAddress>(claim, receipt.VendorAddress);
                            entry.ClaimDetails.Cache.SetValueExt<Extensions.DAC.ATPTEFMEPExpenseClaimDetailsExt.usrATPTVendTIN>(claim, receipt.VendorTin);
                        }
                        else if (receipt.VendID == null)
                        {
                            entry.ClaimDetails.Cache.SetValueExt<Extensions.DAC.ATPTEFMEPExpenseClaimDetailsExt.usrATPTVendName>(claim, receipt.VendorName);
                            entry.ClaimDetails.Cache.SetValueExt<Extensions.DAC.ATPTEFMEPExpenseClaimDetailsExt.usrATPTAddress>(claim, receipt.VendorAddress);
                            entry.ClaimDetails.Cache.SetValueExt<Extensions.DAC.ATPTEFMEPExpenseClaimDetailsExt.usrATPTVendTIN>(claim, receipt.VendorTin);
                        }

                        claim = entry.ClaimDetails.Update(claim);

                        PXNoteAttribute.CopyNoteAndFiles(FundTransactionReceiptLines.Cache, receipt, entry.ClaimDetails.Cache, claim);

                        claim.CuryID = fund.CuryID;
                        claim.TaxCalcMode = item.TaxCalcMode;
                        claim.TaxZoneID = receipt.TaxZoneID;
                        claim.TaxCategoryID = receipt.TaxCategoryID;
                        claim = AddAtcCode(claim, receipt);
                        claimExt.UsrATPTEFMATCCode = receipt.AtcCode;
                        entry.ClaimDetails.Update(claim);

                        entry.Save.Press();

                        decimal totalWhtAmt = 0M;
                        foreach (EPTaxTran tax in entry.Taxes.Select())
                        {
                            Tax tx = Tax.PK.Find(this, tax.TaxID);

                            if (tx != null && tx.TaxType == CSTaxType.Withholding)
                            {
                                totalWhtAmt += tax.CuryTaxAmt ?? 0m;
                                totalWhtAmountForReimbursement += tax.CuryTaxAmt ?? 0m;
                            }
                        }

                        receipt.ExpenseReceiptRefNbr = claim.ClaimDetailCD;
                        fundTransaction.ReceiptsWithNoERRefNbrs--;

                        receipt.WhtAmount = totalWhtAmt;
                        FundTransactionReceiptLines.Update(receipt);

                        #region Insert Receipt to Transaction History

                        decimal? fundTranBalanceAmt = decimal.Zero;

                        #region Get Balance Amount
                        if (isFundRequest)
                        {
                            ATPTEFMFundTransactionHistoryView currentFundTranHistory = PXSelect<
                                ATPTEFMFundTransactionHistoryView,
                                Where<ATPTEFMFundTransactionHistoryView.refNbr, Equal<Required<ATPTEFMFundTransactionHistoryView.refNbr>>>>
                                .Select(this, fundTransaction.RefNbr);

                            if (currentFundTranHistory != null)
                            {
                                fundTranBalanceAmt = currentFundTranHistory.CuryBalanceAmt;
                            }
                        }

                        if (isFundReimbursement)
                        {
                            //Get only the first index of receipt detail for getting the previous transaction balance amount in transaction history
                            int firstErCounter = 1;

                            if (firstErCounter == 1)
                            {
                                var getTransactionHistory =
                               PXSelect<
                                   ATPTEFMFundTransactionHistoryView,
                                   Where<ATPTEFMFundTransactionHistoryView.fundRefNbr, Equal<Required<ATPTEFMFundTransactionHistoryView.fundRefNbr>>>,
                                   OrderBy<
                                       Asc<ATPTEFMFundTransactionHistoryView.sortNbr>>>
                                   .Select(this, fundTransaction.FundID);

                                if (getTransactionHistory != null)
                                {
                                    var getResult = ATPTEFMFundHistoryPaginationHelper.GetRecordPosition(getTransactionHistory.RowCast<ATPTEFMFundTransactionHistoryView>(), claim.ClaimDetailCD);
                                    var prevRecord = (ATPTEFMFundTransactionHistoryView)getResult.PreviousRecord;

                                    fundTranBalanceAmt = prevRecord.CuryBalanceAmt;
                                }

                                firstErCounter++;
                            }
                        }

                        #endregion

                        ATPTEFMFundTransactionHistoryView transactionHistory = TransactionHistoryView.Insert();
                        transactionHistory.FundRefNbr = fundTransaction.FundID;
                        transactionHistory.TransactionType = ATPTEFMTransactionHistoryView.transactionType.ExpenseReceipt;
                        transactionHistory.RefNbr = claim.ClaimDetailCD;
                        transactionHistory.OrderDate = claim.ExpenseDate;
                        transactionHistory.FundBranchID = claim.BranchID;
                        transactionHistory.FundType = claimExt.UsrATPTEFMFundType;
                        transactionHistory.TransactionDate = claim.ExpenseDate;
                        transactionHistory.CuryFundTransactionDocumentAmt = claim.CuryExtCost;
                        transactionHistory.Status = (claim.Status == ATPTEFMExpenseReceiptStatusAttribute.OpenValue) ? EPExpenseClaimDetailsStatus.OpenStatus : claim.Status;
                        if (isFundRequest)
                        {
                            transactionHistory.CuryLiquidatedAmt = decimal.Zero;
                            transactionHistory.CuryUnliquidatedAmt = claim.CuryExtCost.GetValueOrDefault() - totalWhtAmt;
                        }
                        else
                        {
                            transactionHistory.CuryLiquidatedAmt = claim.CuryExtCost.GetValueOrDefault() - totalWhtAmt;
                            transactionHistory.CuryUnliquidatedAmt = decimal.Zero;
                        }

                        transactionHistory.CuryFundReturnAmt = decimal.Zero;
                        transactionHistory.IsReimbursement = !(isFundRequest) ? true : false;
                        transactionHistory.ProjectID = claim.ContractID;
                        transactionHistory.ProjectTaskID = claim.TaskID;
                        transactionHistory.CostCodeID = claim.CostCodeID;
                        transactionHistory.CuryWithholdingTax = totalWhtAmt;
                        transactionHistory.CuryBalanceAmt = fundTranBalanceAmt;
                        transactionHistory.Source = ATPTEFMTransactionHistoryView.source.ExpenseReceipt;
                        transactionHistory.SortNbr = $"FT-{fundTransaction.RefNbr}-{sortCount}";
                        transactionHistory.FundTransactionSortNbr = $"FT-{fundTransaction.RefNbr}";
                        TransactionHistoryView.Update(transactionHistory);
                        #endregion

                    }

                    Fund.Cache.Clear();
                    Fund.Current = PXSelect<
                        ATPTEFMFund,
                        Where<ATPTEFMFund.fundCD, Equal<Required<ATPTEFMFund.fundCD>>>>
                        .Select(this, fundTransaction.FundID);

                    #region Insert Summary liquidated Amount (Reimbursement Type)
                    if (isFundReimbursement)
                    {
                        if (containReceiptNbrCount > 0)
                            fundTransaction.IsSubmitReceiptFlag = true;

                        if (fundTransaction.IsSubmitReceiptFlag == true)
                        {
                            //Scenario: If nag submit receipt na pero nag add nasad usab ug receipt para i add
                            Fund.Current.CuryLiquidatedAmt += (sumAmtOfReceiptWithoutErNbr - totalWhtAmountForReimbursement);
                            fundTransaction.IsSubmitReceiptFlag = false;
                        }
                        else
                        {
                            Fund.Current.CuryLiquidatedAmt += (fundTransaction.ActualSpentAmount - fundTransaction.TotalWhtAmount);
                        }

                        Fund.UpdateCurrent();
                    }
                    #endregion

                    #region Insert Summary Unliquidated Amount
                    if (!isFundReimbursement)
                    {
                        if (Fund.Current.ATPTEFMValidateAmountReceivedAndAmountReleased == true
                        && fundTransaction.CashAdvanceStatus.Equals(ATPTEFMFundTransactionCashAdvanceStatusAttribute.ForReclassificationValue)
                        && containLiquidatedReceiptNbrCount > 0)
                        {
                            //Scenario:  If nag submit receipt na pero nag load nasad usab ug receipt para i add pero naa na mga receipts na liquidated
                            Fund.Current.CuryLiquidatedAmt -= (sumAmtOfLiquidatedReceipts - sumOfWhtWithLiquidatedReceipts);
                            Fund.Current.CuryUnliquidatedAmt -= fundTransaction.RequestedAmount - (sumAmtOfLiquidatedReceipts - sumOfWhtWithLiquidatedReceipts);
                            Fund.Current.CuryUnliquidatedAmt += fundTransaction.RequestedAmount;

                            //Update FT cash advance status to unliquidated
                            fundTransaction.CashAdvanceStatus = ATPTEFMFundTransactionCashAdvanceStatusAttribute.UnliquidatedValue;
                        }

                        Fund.UpdateCurrent();
                    }
                    #endregion

                    fundTransaction.Step = ATPTEFMFundTransactionStepAttribute.SubmitReceiptValue;
                    FundTransactions.Update(fundTransaction);
                    Save.Press();
                    ts.Complete();
                }
            });
            return adapter.Get();
        }

        /// <remarks>
        /// 2025-05-09 : "(CFM2023R2 and UP) Fund Transaction Request> Enable the 'Add Row' button in the Receipts tab when the CA status is 'Unliquidated', even if the 'Submit Receipt' button has already been clicked." - CASE: 011533 {JLG}  <br/> 
        /// </remarks>
        public PXAction<ATPTEFMFundTransaction> Liquidate;
        [PXButton(Category = "Actions", Connotation = PX.Data.WorkflowAPI.ActionConnotation.Success)]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.Liquidate, MapEnableRights = PXCacheRights.Update, MapViewRights = PXCacheRights.Update)]
        public virtual IEnumerable liquidate(PXAdapter adapter)
        {
            ATPTEFMFundTransaction fundTransaction = FundTransactions.Current;

            if (FundTransactions?.Current?.Status == ATPTEFMFundStatusAttribute.OpenValue)
            {
                if (FundTransactions?.Current?.Balance < 0m)
                    FundTransactions.Cache.RaiseExceptionHandling<ATPTEFMFundTransaction.balance>(FundTransactions?.Current, FundTransactions?.Current.Balance, ATPTEFMHelper.GetPropertyException(FundTransactions?.Current, Messages.ATPTEFMMessages.BalanceShouldNotBeNegative, PXErrorLevel.Error));

                if ((fundTransaction.RequestedAmount < fundTransaction.ActualSpentAmount) && fundTransaction.Balance > 0)
                {
                    FundTransactions.Cache.RaiseExceptionHandling<ATPTEFMFundTransaction.balance>(FundTransactions?.Current, FundTransactions?.Current.Balance, ATPTEFMHelper.GetPropertyException(FundTransactions?.Current, Messages.ATPTEFMMessages.AmountReleaseCannotBeGreaterThanFundReturn, PXErrorLevel.Error));
                }
            }

            if (SubmitReceipts.GetEnabled())
            {
                SubmitReceipts.PressButton();
                PXLongOperation.WaitCompletion(this.UID);
            }

            ATPTEFMHelper.StartLongOperation(this, adapter, delegate ()
            {
                using (PXTransactionScope ts = new PXTransactionScope())
                {
                    if (fundTransaction == null) return;

                    #region Validations
                    AllReceiptsMustOpenStatus(fundTransaction.RefNbr);
                    #endregion

                    #region BQL
                    ATPTEFMFund currentFund =
                         PXSelect<
                             ATPTEFMFund,
                             Where<ATPTEFMFund.fundCD, Equal<Required<ATPTEFMFund.fundCD>>>>
                             .Select(this, fundTransaction.FundID);
                    #endregion

                    #region Liquidated receipts
                    foreach (ATPTEFMFundTransactionReceiptDetail receipt in FundTransactionReceiptLines.Select())
                    {
                        if (receipt == null) continue;

                        receipt.IsLiquidated = true;
                        FundTransactionReceiptLines.Update(receipt);
                    }
                    #endregion

                    #region Reclassifications
                    if ((currentFund.ATPTEFMValidateAmountReceivedAndAmountReleased ?? false) && ((fundTransaction.ChangeAmount - fundTransaction.AmountReceived) + fundTransaction.AmountReleased) > 0m)
                    {
                        fundTransaction.CashAdvanceStatus = ATPTEFMFundTransactionCashAdvanceStatusAttribute.ForReclassificationValue;
                    }
                    else
                    {
                        fundTransaction.CashAdvanceStatus = ATPTEFMFundTransactionCashAdvanceStatusAttribute.LiquidatedValue;
                        fundTransaction.Status = ATPTEFMFundStatusAttribute.ClosedValue;
                    }
                    #endregion

                    #region Update Current Transaction
                    fundTransaction.ReceivedAmount = fundTransaction.ActualSpentAmount;
                    fundTransaction.Step = ATPTEFMFundTransactionStepAttribute.LiquidatedReceiptValue;
                    FundTransactions.Update(fundTransaction);
                    #endregion

                    #region Update Fund Summary Balances
                    Fund.Cache.Clear();
                    Fund.Current = PXSelect<
                        ATPTEFMFund,
                        Where<ATPTEFMFund.fundCD, Equal<Required<ATPTEFMFund.fundCD>>>>
                        .Select(this, fundTransaction.FundID);
                    if (Fund.Current != null)
                    {
                        //ValidateAmountReceivedAndAmountReleased = true, and transaction is for Reclassification
                        if ((currentFund.ATPTEFMValidateAmountReceivedAndAmountReleased ?? false) && (fundTransaction.Balance > 0))
                        {
                            Fund.Current.CuryLiquidatedAmt += (fundTransaction.ActualSpentAmount - fundTransaction.TotalWhtAmount);
                            Fund.Current.CuryUnliquidatedAmt -= fundTransaction.RequestedAmount;
                            Fund.Current.CuryUnliquidatedAmt += fundTransaction.Balance;

                            Fund.Current.CuryBalanceAmt += fundTransaction.RequestedAmount;
                            Fund.Current.CuryBalanceAmt -= ((fundTransaction.Balance - fundTransaction.TotalWhtAmount) + fundTransaction.ActualSpentAmount);
                        }
                        //ValidateAmountReceivedAndAmountReleased = true or false, and its not for reclassification
                        else
                        {
                            Fund.Current.CuryUnliquidatedAmt -= fundTransaction.RequestedAmount;
                            Fund.Current.CuryLiquidatedAmt += (fundTransaction.ActualSpentAmount - fundTransaction.TotalWhtAmount);
                            #region Calculations For OnHand

                            Fund.Current.CuryBalanceAmt += fundTransaction.RequestedAmount;
                            Fund.Current.CuryBalanceAmt -= fundTransaction.ActualSpentAmount;
                            Fund.Current.CuryBalanceAmt += fundTransaction.TotalWhtAmount;
                        }
                        Fund.UpdateCurrent();
                        #endregion
                    }
                    #endregion

                    #region Update Running Balance                  
                    var getTransactionHistory =
                    PXSelect<
                        ATPTEFMFundTransactionHistoryView,
                        Where<ATPTEFMFundTransactionHistoryView.fundRefNbr, Equal<Required<ATPTEFMFundTransactionHistoryView.fundRefNbr>>>,
                        OrderBy<
                            Asc<ATPTEFMFundTransactionHistoryView.sortNbr>>>
                        .Select(this, FundTransactions.Current.FundID);

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
                        .SelectWindowed(this, startIndex, totalRows, FundTransactions.Current.FundID))
                    {
                        TransactionHistoryView.Current = tran;

                        if (TransactionHistoryView.Current != null)
                        {
                            #region Fund Request
                            if (tran.TransactionType.Equals(ATPTEFMTransactionHistoryView.transactionType.FundRequest))
                            {
                                if (fundTransaction.RefNbr == tran.RefNbr)
                                {
                                    if ((currentFund.ATPTEFMValidateAmountReceivedAndAmountReleased ?? false) && (fundTransaction.Balance > 0))
                                    {
                                        runningBalance -= (fundTransaction.ActualSpentAmount + (fundTransaction.Balance - fundTransaction.TotalWhtAmount));

                                        #region Update Unliquidated and Fund Return
                                        TransactionHistoryView.Current.CuryUnliquidatedAmt = fundTransaction.Balance;
                                        TransactionHistoryView.Current.CuryFundReturnAmt = fundTransaction.AmountReceived;
                                        #endregion
                                    }
                                    else
                                    {
                                        runningBalance -= FundTransactions.Current.ActualSpentAmount;
                                        runningBalance += FundTransactions.Current.TotalWhtAmount;

                                        #region Update Unliquidated and Fund Return
                                        TransactionHistoryView.Current.CuryUnliquidatedAmt = decimal.Zero;
                                        TransactionHistoryView.Current.CuryFundReturnAmt = fundTransaction.ChangeAmount;
                                        #endregion
                                    }
                                }
                                else
                                {
                                    ATPTEFMFundTransaction currentFundTransaction = PXSelect<
                                        ATPTEFMFundTransaction,
                                        Where<ATPTEFMFundTransaction.refNbr, Equal<Required<ATPTEFMFundTransaction.refNbr>>>>
                                        .Select(this, tran.RefNbr);

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
                                                runningBalance -= currentFundTransaction.ReclassificationAmt;
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

                                TransactionHistoryView.Current.CuryBalanceAmt = runningBalance;
                                TransactionHistoryView.UpdateCurrent();
                                continue;
                            }
                            #endregion

                            #region Fund Reimbursement
                            if (tran.TransactionType.Equals(ATPTEFMTransactionHistoryView.transactionType.FundReimbursment))
                            {
                                if (tran.Status != ATPTEFMFundStatusAttribute.CancelledValue)
                                {
                                    ATPTEFMFundTransaction currentFundTransaction = PXSelect<
                                        ATPTEFMFundTransaction,
                                        Where<ATPTEFMFundTransaction.refNbr, Equal<Required<ATPTEFMFundTransaction.refNbr>>>>
                                        .Select(this, tran.RefNbr);

                                    if (currentFundTransaction != null)
                                    {
                                        runningBalance -= currentFundTransaction.ActualSpentAmount;
                                        runningBalance += currentFundTransaction.TotalWhtAmount;
                                    }
                                }
                                TransactionHistoryView.Current.CuryBalanceAmt = runningBalance;
                                TransactionHistoryView.UpdateCurrent();
                                continue;
                            }
                            #endregion

                            #region Replenishment
                            if (tran.TransactionType.Equals(ATPTEFMTransactionHistoryView.transactionType.Replenishment))
                            {
                                runningBalance += TransactionHistoryView.Current.CuryCheckAmt ?? decimal.Zero;
                                TransactionHistoryView.Current.CuryBalanceAmt = runningBalance;
                                TransactionHistoryView.UpdateCurrent();
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
                                    .Select(this, tran.RefNbr);

                                if (receipt != null)
                                {
                                    decimal? fundBalanceAmount = runningBalance;

                                    if (tran.Status != ATPTEFMFundStatusAttribute.CancelledValue)
                                    {
                                        #region Get Fund Balance Amount
                                        TransactionHistoryView.Current = PXSelect<
                                                                                                                        ATPTEFMFundTransactionHistoryView,
                                                                                                                        Where<ATPTEFMFundTransactionHistoryView.refNbr, Equal<Required<ATPTEFMFundTransactionHistoryView.refNbr>>>>
                                                                                                                        .Select(this, receipt.FundTransactionRefNbr);
                                        if (TransactionHistoryView.Current != null)
                                        {
                                            fundBalanceAmount = TransactionHistoryView.Current.CuryBalanceAmt;
                                        }
                                        #endregion

                                        TransactionHistoryView.Current = TransactionHistoryView.Current = PXSelect<
                                            ATPTEFMFundTransactionHistoryView,
                                            Where<ATPTEFMFundTransactionHistoryView.refNbr, Equal<Required<ATPTEFMFundTransactionHistoryView.refNbr>>>>
                                            .Select(this, tran.RefNbr);

                                        if (receipt.FundTransactionRefNbr == fundTransaction.RefNbr)
                                        {
                                            TransactionHistoryView.Current.Status = ATPTEFMFundStatusAttribute.LiquidatedValue;
                                            TransactionHistoryView.Current.CuryWithholdingTax = receipt.WhtAmount;
                                            TransactionHistoryView.Current.CuryUnliquidatedAmt = decimal.Zero;
                                            TransactionHistoryView.Current.CuryLiquidatedAmt = receipt.NetAmt - receipt.WhtAmount;
                                        }
                                    }
                                    TransactionHistoryView.Current.CuryBalanceAmt = fundBalanceAmount;
                                    TransactionHistoryView.UpdateCurrent();
                                    continue;
                                }
                            }

                            #endregion

                            #region IncreaseFund
                            if (tran.TransactionType.Equals(ATPTEFMTransactionHistoryView.transactionType.IncreaseFund))
                            {
                                runningBalance += tran.CuryCheckAmt;
                                TransactionHistoryView.Current.CuryBalanceAmt = runningBalance;
                                TransactionHistoryView.UpdateCurrent();
                                continue;
                            }
                            #endregion

                            #region DecreaseFund
                            if (tran.TransactionType.Equals(ATPTEFMTransactionHistoryView.transactionType.DecreaseFund))
                            {
                                runningBalance -= tran.CuryCheckAmt;
                                TransactionHistoryView.Current.CuryBalanceAmt = runningBalance;
                                TransactionHistoryView.UpdateCurrent();
                                continue;
                            }
                            #endregion

                            TransactionHistoryView.Current.CuryBalanceAmt = runningBalance;
                            TransactionHistoryView.UpdateCurrent();
                        }
                    }
                    #endregion

                    Save.Press();
                    ts.Complete();
                }
            });
            return adapter.Get();
        }

        public PXAction<ATPTEFMFundTransaction> FundTransactionReport;

        [PXButton(Category = "Reports")]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.FundTransactionReport)]

        /// <remarks>   
        /// 04-08-2026: 015822 - CFM 2025R2: Fund Transaction Form. Related case 015821. {JCL} </br>
        /// </remarks>>
        public IEnumerable fundTransactionReport(PXAdapter adapter)
        {
            foreach (ATPTEFMFundTransaction funds in adapter.Get<ATPTEFMFundTransaction>())
            {
                Dictionary<string, string> parameters = new Dictionary<string, string>
                {
                    ["RefNbrUnb"] = funds.RefNbr
                };

                var report = new PXReportRequiredException(parameters, "ATPT6417", "Fund Transaction Form");

                throw new PXRedirectWithReportException(this, report, "Preview");
            }
            return adapter.Get();
        }

        public PXAction<ATPTEFMFundTransaction> FundLiquidationReport;

        [PXButton(Category = "Reports")]
        [PXUIField(DisplayName = "Liquidation")]

        /// <remarks>   
        /// 04-08-2026: 015822 - CFM 2025R2: Fund Transaction Form. Related case 015821. {JCL} </br>
        /// </remarks>>
        public IEnumerable fundLiquidationReport(PXAdapter adapter)
        {
            foreach (ATPTEFMFundTransaction fund in adapter.Get<ATPTEFMFundTransaction>())
            {
                Dictionary<string, string> parameters = new Dictionary<string, string>
                {
                    ["RefNbrUnb"] = fund.RefNbr
                };

                var report = new PXReportRequiredException(parameters, "ATPT6426", "Liquidation Form");

                throw new PXRedirectWithReportException(this, report, "Preview");
            }
            return adapter.Get();
        }

        public PXAction<ATPTEFMFundTransaction> openTransaction;
        [PXButton(Tooltip = Messages.ATPTEFMMessages.OpenTransaction, CommitChanges = true)]
        [PXUIField(DisplayName = "Open Transaction", Visible = false)]
        public virtual void OpenTransaction()
        {
            ATPTEFMFundTransactionReceiptDetail row = FundTransactionReceiptLines.Current;

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

        public PXAction<ATPTEFMFundTransaction> openReclassReceipt;
        [PXButton(Tooltip = "Open Reclass Receipt", CommitChanges = true)]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.OpenReclassReceipt, Visible = false)]
        public virtual void OpenReclassReceipt()
        {
            ATPTEFMFundTransactionReclassficationReceiptDetail row = FundTransactionReclassficationReceiptDetail.Current;

            if (row != null)
            {
                ExpenseClaimDetailEntry graph = PXGraph.CreateInstance<ExpenseClaimDetailEntry>();
                graph.ClaimDetails.Current = graph.ClaimDetails.Search<EPExpenseClaimDetails.claimDetailCD>(row.ExpenseReceiptRefNbr);

                if (graph.ClaimDetails.Current != null)
                {
                    throw new PXRedirectRequiredException(graph, true, "Expense Receipt") { Mode = PXBaseRedirectException.WindowMode.NewWindow };
                }
            }
        }

        public PXAction<ATPTEFMFundTransaction> LoadRequest;
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

        public PXAction<ATPTEFMFundTransaction> submitReceipt;

        [PXUIField(DisplayName = Messages.ATPTEFMMessages.SubmitReceipt)]
        [PXButton()]
        protected virtual IEnumerable SubmitReceipt(PXAdapter adapter)
        {
            ATPTEFMFundTransaction ft = FundTransactions.Current;
            foreach (ATPTEFMFundTransactionDetail item in ReceiptsForSubmit.Select())
            {
                if (item.Selected == true)
                {
                    FundTransactions.Current.Step = ATPTEFMFundTransactionStepAttribute.DefaultValue;
                    FundTransactions.Current.AmountReceived = decimal.Zero;
                    FundTransactions.UpdateCurrent();

                    ATPTEFMFundTransactionReceiptDetail receipt = new ATPTEFMFundTransactionReceiptDetail();

                    receipt.FundTransactionDetailID = item.FundTransactionDetailID;
                    FundTransactionReceiptLines.Cache.SetValueExt<ATPTEFMFundTransactionReceiptDetail.inventoryID>(receipt, item.InventoryID);


                    receipt.Descr = item.Description;
                    receipt.LineDescription = item.LineDescription;
                    receipt.Qty = item.Qty;
                    receipt.UnitCost = item.UnitCost;
                    receipt.Amount = item.Amount;

                    receipt.AccountID = item.AccountID;
                    receipt.SubID = item.SubID;

                    receipt.ProjectID = item.ProjectID;
                    receipt.ProjectTaskID = item.ProjectTaskID;
                    receipt.CostCodeID = item.CostCodeID;

                    FundTransactionReceiptLines.Cache.Insert(receipt);
                }
            }

            ReceiptsForSubmit.Cache.Clear();
            return adapter.Get();
        }

        public PXAction<ATPTEFMFundTransaction> cancelSubmitReceipt;

        [PXUIField(DisplayName = Messages.ATPTEFMMessages.CancelSubmitReceipt)]
        [PXButton()]
        protected virtual IEnumerable CancelSubmitReceipt(PXAdapter adapter)
        {
            ReceiptsForSubmit.Cache.Clear();
            return adapter.Get();
        }

        /// <remarks>
        /// 2024-09-11 : "Cancelling a Fund Request with CA status 'For Reclassifications' should clear both the unliquidated, liquidated fields and including the cash returned" - CASE: 007474  {JLG} <br/> 
        /// 2024-10-07 : "Throws an error if receipt is already added in replenishment" - CASE: 007509 {JLG}  <br/> 
        /// 2024-12-23 : "Cancelled fund request shld. not be added to the fund balance." - CASE: 009339 {JLG}
        /// 2025-10-08 : "Error on Fund Transaction Cancellation" - CASE: 013875 {JLG} <br/>
        /// </remarks>
        public PXAction<ATPTEFMFundTransaction> FTCancel;

        [PXUIField(DisplayName = Messages.ATPTEFMMessages.CACancel)]
        [PXButton]
        protected virtual IEnumerable ftCancel(PXAdapter adapter)
        {
            ATPTEFMFundTransaction fundTransaction = FundTransactions.Current;
            bool isFundRequest = ATPTEFMFundTransactionHelper.IsFundRequest(fundTransaction);
            bool isFundReimbursement = ATPTEFMFundTransactionHelper.IsFundReimbursement(fundTransaction);
            bool isSubmitReceipt = ATPTEFMFundTransactionHelper.IsSubmitReceipt(fundTransaction);
            bool isLiquidated = ATPTEFMFundTransactionHelper.IsFundLiquidated(fundTransaction);
            if (fundTransaction != null)
            {


                ATPTEFMHelper.StartLongOperation(this, adapter, delegate ()
                {
                    using (PXTransactionScope ts = new PXTransactionScope())
                    {
                        #region Validations if receipt is already added in replenishment
                        bool alreadyAddedInReplenishment = PXSelectJoin<
                                                                                                                        ATPTEFMFundTransaction,
                                                                                                                        InnerJoin<ATPTEFMFundTransactionReceiptDetail,
                                                                                                                            On<ATPTEFMFundTransactionReceiptDetail.fundTransactionRefNbr, Equal<ATPTEFMFundTransaction.refNbr>>,
                                                                                                                        InnerJoin<ATPTEFMReplenishmentDetail,
                                                                                                                            On<ATPTEFMReplenishmentDetail.expenseReceiptNbr, Equal<ATPTEFMFundTransactionReceiptDetail.expenseReceiptRefNbr>>,
                                                                                                                        InnerJoin<ATPTEFMReplenishment,
                                                                                                                            On<ATPTEFMReplenishment.replenishmentNbr, Equal<ATPTEFMReplenishmentDetail.replenishmentNbr>>>>>,
                                                                                                                        Where<ATPTEFMFundTransaction.refNbr, Equal<Required<ATPTEFMFundTransaction.refNbr>>,
                                                                                                                            And<ATPTEFMReplenishment.status, NotEqual<ATPTEFMReplenishmentStatusAttribute.cancelledValue>>>>
                                                                                                                        .Select(this, fundTransaction.RefNbr)
                                                                                                                        .Count > 0;

                        if (fundTransaction.FundTransactionType.Equals(ATPTEFMFundTransactionTypeAttribute.ReimbursementValue)
                        && fundTransaction.Status.Equals(ATPTEFMFundStatusAttribute.ClosedValue)
                        && alreadyAddedInReplenishment
                        )
                        {
                            throw new PXException(ATPTEFMMessages.ErAlreadyAddedInReplenishment);
                        }

                        #endregion

                        #region Conditional Variables
                        bool isReceiptSubmitted = ATPTEFMFundTransactionHelper.IsSubmitReceipt(FundTransactions.Current);
                        bool isFundStatusClosed = ATPTEFMFundTransactionHelper.IsFundStatusClosed(FundTransactions.Current);
                        #endregion

                        foreach (EPApproval approval in Approval.Select())
                        {
                            Approval.Delete(approval);
                        }

                        ExpenseClaimDetailEntry graph = PXGraph.CreateInstance<ExpenseClaimDetailEntry>();
                        foreach (ATPTEFMFundTransactionReceiptDetail item in FundTransactionReceiptLines.Select())
                        {
                            if (!item.ExpenseReceiptRefNbr.IsNullOrEmpty())
                            {
                                EPExpenseClaimDetails er = EPExpenseClaimDetails.PK.Find(graph, item.ExpenseReceiptRefNbr);
                                graph.ClaimDetails.Current = er;

                                er.Status = ATPTEFMExpenseReceiptStatusAttribute.CancelledValue;
                                graph.ClaimDetails.Update(er);

                                graph.Save.Press();
                            }
                        }

                        decimal? runningBalance = 0m;

                        if (isSubmitReceipt || isLiquidated || fundTransaction.Step == ATPTEFMFundTransactionStepAttribute.DefaultValue)
                        {
                            #region Execute running balance             
                            var getTransactionHistory =
                                         PXSelect<
                                             ATPTEFMFundTransactionHistoryView,
                                             Where<ATPTEFMFundTransactionHistoryView.fundRefNbr, Equal<Required<ATPTEFMFundTransactionHistoryView.fundRefNbr>>>,
                                             OrderBy<
                                                 Asc<ATPTEFMFundTransactionHistoryView.sortNbr>>>
                                             .Select(this, fundTransaction.FundID);



                            ATPTEFMFundTransactionReceiptDetail refNbrDetail = FundTransactionReceiptLines.Select().FirstOrDefault();
                            string getReimbursementFirstErNbr = string.Empty;

                            if (refNbrDetail != null && !string.IsNullOrEmpty(refNbrDetail.ExpenseReceiptRefNbr))
                                getReimbursementFirstErNbr = refNbrDetail.ExpenseReceiptRefNbr;

                            string positionRefNbr = (isFundRequest) ? fundTransaction.RefNbr : getReimbursementFirstErNbr;

                            var getResult = ATPTEFMFundHistoryPaginationHelper.GetRecordPosition(getTransactionHistory.RowCast<ATPTEFMFundTransactionHistoryView>(), positionRefNbr);

                            int startIndex = getResult.StartIndex;
                            int totalRows = getResult.TotalRows;

                            var prevRecord = (ATPTEFMFundTransactionHistoryView)getResult.PreviousRecord;

                            #region Get fund transaction running balance
                            if (isFundRequest)
                            {
                                TransactionHistoryView.Current = TransactionHistoryView.Select(fundTransaction.RefNbr);
                            }
                            #endregion

                            runningBalance = (isFundRequest) ? TransactionHistoryView.Current.CuryBalanceAmt : prevRecord.CuryBalanceAmt;

                            foreach (ATPTEFMFundTransactionHistoryView tran in PXSelect<
                                ATPTEFMFundTransactionHistoryView,
                                Where<ATPTEFMFundTransactionHistoryView.fundRefNbr, Equal<Required<ATPTEFMFundTransactionHistoryView.fundRefNbr>>>,
                                OrderBy<
                                    Asc<ATPTEFMFundTransactionHistoryView.sortNbr>>>
                                .SelectWindowed(this, startIndex, totalRows, fundTransaction.FundID))
                            {
                                TransactionHistoryView.Current = TransactionHistoryView.Select(tran.RefNbr);

                                if (TransactionHistoryView.Current != null)
                                {
                                    #region Fund Request
                                    if (tran.TransactionType.Equals(ATPTEFMTransactionHistoryView.transactionType.FundRequest)
                                        && !tran.Status.Equals(ATPTEFMFundStatusAttribute.RejectedValue))
                                    {
                                        if (fundTransaction.RefNbr == tran.RefNbr)
                                        {
                                            if (fundTransaction.CashAdvanceStatus.Equals(ATPTEFMFundTransactionCashAdvanceStatusAttribute.ForReclassificationValue))
                                                runningBalance += ((fundTransaction.ActualSpentAmount - fundTransaction.TotalWhtAmount) + fundTransaction.Balance);
                                            else if (fundTransaction.CashAdvanceStatus.Equals(ATPTEFMFundTransactionCashAdvanceStatusAttribute.UnreleasedValue))
                                                runningBalance += decimal.Zero;
                                            else
                                                runningBalance += fundTransaction.RequestedAmount;

                                            TransactionHistoryView.Current.CuryFundReturnAmt = decimal.Zero;
                                            TransactionHistoryView.Current.CuryUnliquidatedAmt = decimal.Zero;
                                        }
                                        else
                                        {
                                            ATPTEFMFundTransaction currentFundTransaction = PXSelect<
                                                ATPTEFMFundTransaction,
                                                Where<ATPTEFMFundTransaction.refNbr, Equal<Required<ATPTEFMFundTransaction.refNbr>>>>
                                                .Select(this, tran.RefNbr);

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
                                                        runningBalance -= currentFundTransaction.ReclassificationAmt;
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

                                        TransactionHistoryView.Current.CuryBalanceAmt = runningBalance;
                                        TransactionHistoryView.UpdateCurrent();
                                        continue;
                                    }
                                    #endregion

                                    #region Fund Reimbursement
                                    if (tran.TransactionType.Equals(ATPTEFMTransactionHistoryView.transactionType.FundReimbursment)
                                        && !tran.Status.Equals(ATPTEFMFundStatusAttribute.RejectedValue))
                                    {

                                        if (tran.RefNbr != fundTransaction.RefNbr)
                                        {
                                            ATPTEFMFundTransaction currentFundTransaction = PXSelect<
                                                ATPTEFMFundTransaction,
                                                Where<ATPTEFMFundTransaction.refNbr, Equal<Required<ATPTEFMFundTransaction.refNbr>>>>
                                                .Select(this, tran.RefNbr);

                                            if (currentFundTransaction != null)
                                            {
                                                runningBalance -= currentFundTransaction.ActualSpentAmount;
                                                runningBalance += currentFundTransaction.TotalWhtAmount;
                                            }
                                        }

                                        TransactionHistoryView.Current.CuryBalanceAmt = runningBalance;
                                        TransactionHistoryView.UpdateCurrent();
                                        continue;
                                    }
                                    #endregion

                                    #region Replenishment
                                    if (tran.TransactionType.Equals(ATPTEFMTransactionHistoryView.transactionType.Replenishment))
                                    {
                                        runningBalance += TransactionHistoryView.Current.CuryCheckAmt ?? decimal.Zero;
                                        TransactionHistoryView.Current.CuryBalanceAmt = runningBalance;
                                        TransactionHistoryView.UpdateCurrent();
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
                                            .Select(this, tran.RefNbr);

                                        if (receipt != null)
                                        {
                                            #region Get Fund Balance Amount

                                            decimal? fundBalanceAmount = runningBalance;

                                            TransactionHistoryView.Current = TransactionHistoryView.Select(receipt.FundTransactionRefNbr);

                                            if (TransactionHistoryView.Current != null)
                                            {
                                                fundBalanceAmount = TransactionHistoryView.Current.CuryBalanceAmt;
                                            }
                                            #endregion

                                            TransactionHistoryView.Current = TransactionHistoryView.Select(tran.RefNbr);

                                            if (receipt.FundTransactionRefNbr != fundTransaction.RefNbr && tran.Status.Equals(ATPTEFMExpenseReceiptStatusAttribute.CancelledValue))
                                            {
                                                continue;
                                            }

                                            if (receipt.FundTransactionRefNbr == fundTransaction.RefNbr)
                                            {
                                                TransactionHistoryView.Current.Status = ATPTEFMExpenseReceiptStatusAttribute.CancelledValue;
                                                TransactionHistoryView.Current.CuryWithholdingTax = decimal.Zero;
                                                TransactionHistoryView.Current.CuryUnliquidatedAmt = decimal.Zero;
                                                TransactionHistoryView.Current.CuryLiquidatedAmt = decimal.Zero;

                                                if (isFundReimbursement)
                                                    fundBalanceAmount = runningBalance;
                                            }
                                            TransactionHistoryView.Current.CuryBalanceAmt = fundBalanceAmount;
                                            TransactionHistoryView.UpdateCurrent();
                                            continue;
                                        }
                                    }

                                    #endregion

                                    #region IncreaseFund
                                    if (tran.TransactionType.Equals(ATPTEFMFundTransactionTypeAttribute.IncreaseFundValue))
                                    {
                                        if (tran.Status == APDocStatus.Closed)
                                            runningBalance += tran.CuryCheckAmt;

                                        TransactionHistoryView.Current.CuryBalanceAmt = runningBalance;
                                        TransactionHistoryView.UpdateCurrent();
                                        continue;
                                    }
                                    #endregion

                                    #region DecreaseFund
                                    if (tran.TransactionType.Equals(ATPTEFMFundTransactionTypeAttribute.DecreaseFundValue))
                                    {
                                        if (tran.Status == APDocStatus.Closed)
                                            runningBalance -= tran.CuryCheckAmt;

                                        TransactionHistoryView.Current.CuryBalanceAmt = runningBalance;
                                        TransactionHistoryView.UpdateCurrent();
                                        continue;
                                    }
                                    #endregion

                                    TransactionHistoryView.Current.CuryBalanceAmt = runningBalance;
                                    TransactionHistoryView.UpdateCurrent();
                                }
                            }

                            #endregion
                        }

                        #region Update Fund Summary
                        Fund.Cache.Clear();
                        Fund.Current = PXSelect<
                            ATPTEFMFund,
                            Where<ATPTEFMFund.fundCD, Equal<Required<ATPTEFMFund.fundCD>>>>
                            .Select(this, fundTransaction.FundID);
                        if (Fund.Current != null)
                        {
                            if (isFundRequest)
                            {

                                if (fundTransaction.CashAdvanceStatus.Equals(ATPTEFMFundTransactionCashAdvanceStatusAttribute.UnreleasedValue))
                                {
                                    Fund.Current.CuryBalanceAmt += decimal.Zero;
                                    Fund.Current.CuryUnliquidatedAmt -= decimal.Zero;
                                }
                                else if (fundTransaction.CashAdvanceStatus.Equals(ATPTEFMFundTransactionCashAdvanceStatusAttribute.ForReclassificationValue))
                                {
                                    Fund.Current.CuryLiquidatedAmt -= (fundTransaction.ActualSpentAmount - fundTransaction.TotalWhtAmount);
                                    Fund.Current.CuryUnliquidatedAmt -= fundTransaction.Balance;
                                    Fund.Current.CuryBalanceAmt += ((fundTransaction.ActualSpentAmount - fundTransaction.TotalWhtAmount) + fundTransaction.Balance);
                                }
                                else
                                {
                                    Fund.Current.CuryBalanceAmt += fundTransaction.RequestedAmount;
                                    Fund.Current.CuryUnliquidatedAmt -= fundTransaction.RequestedAmount;
                                }
                            }

                            if (isFundReimbursement)
                            {
                                Fund.Current.CuryBalanceAmt += (isLiquidated) ? fundTransaction.ActualSpentAmount - fundTransaction.TotalWhtAmount : decimal.Zero;
                                Fund.Current.CuryLiquidatedAmt -= (fundTransaction.Step.Equals(ATPTEFMFundTransactionCashAdvanceStatusAttribute.DefaultValue)) ? decimal.Zero :
                                    fundTransaction.ActualSpentAmount - fundTransaction.TotalWhtAmount;
                            }
                            Fund.UpdateCurrent();
                        }
                        #endregion

                        fundTransaction.Status = ATPTEFMCashAdvanceStatusAttribute.CancelledValue;
                        fundTransaction.CashAdvanceStatus = ATPTEFMFundTransactionCashAdvanceStatusAttribute.CancelledValue;
                        FundTransactions.Update(fundTransaction);
                        this.Save.Press();
                        ts.Complete();
                    }
                });
            }
            return adapter.Get();
        }

        #endregion Action

        #region InternalTypes

        [Serializable]
        [PXHidden]
        public class ATPTEFMFADetailSummary : CashFundManagement.DAC.Base.ATPTEFMBase, IBqlTable
        {
            #region RequestID

            [PXInt]
            public int? RequestID { get; set; }

            public abstract class requestID : PX.Data.BQL.BqlInt.Field<requestID>
            { }

            #endregion RequestID

            #region AccountID

            [PXInt]
            public int? AcctID { get; set; }

            public abstract class acctID : PX.Data.BQL.BqlInt.Field<acctID>
            { }

            #endregion AccountID

            #region SubID

            [PXInt]
            public int? SubID { get; set; }

            public abstract class subID : PX.Data.BQL.BqlInt.Field<subID>
            { }

            #endregion SubID

            #region ProjectID

            [PXInt]
            public int? ProjectID { get; set; }

            public abstract class projectID : PX.Data.BQL.BqlInt.Field<projectID>
            { }

            #endregion ProjectID

            #region ProjectTaskID

            [PXInt]
            public int? ProjectTaskID { get; set; }

            public abstract class projectTaskID : PX.Data.BQL.BqlInt.Field<projectTaskID>
            { }

            #endregion ProjectTaskID

            #region CostCodeID

            [PXInt]
            public int? CostCodeID { get; set; }

            public abstract class costCodeID : PX.Data.BQL.BqlInt.Field<costCodeID>
            { }

            #endregion CostCodeID

            #region InventoryID
            [PXInt]
            public int? InventoryID { get; set; }
            public abstract class inventoryID : PX.Data.BQL.BqlInt.Field<inventoryID> { }
            #endregion

            #region AccountGroupID
            [PXInt]
            public int? AccountGroupID { get; set; }
            public abstract class accountGroupID : PX.Data.BQL.BqlInt.Field<accountGroupID> { }
            #endregion

            #region Qty

            [PXDecimal]
            public decimal? Qty { get; set; }

            public abstract class qty : PX.Data.BQL.BqlDecimal.Field<qty>
            { }

            #endregion Qty

            #region UnitCost

            [PXDecimal]
            public decimal? UnitCost { get; set; }

            public abstract class unitCost : PX.Data.BQL.BqlDecimal.Field<unitCost>
            { }

            #endregion UnitCost

            #region NetQty

            [PXDecimal]
            public decimal? NetQty { get; set; }

            public abstract class netQty : PX.Data.BQL.BqlDecimal.Field<netQty>
            { }

            #endregion NetQty

            #region NetAmt

            [PXDecimal]
            public decimal? NetAmt { get; set; }

            public abstract class netAmt : PX.Data.BQL.BqlDecimal.Field<netAmt>
            { }

            #endregion NetAmt

            #region CuryID

            [PXString]
            public string CuryID { get; set; }

            public abstract class curyID : PX.Data.BQL.BqlString.Field<curyID>
            { }

            #endregion CuryID
        }

        [Serializable]
        [PXCacheName("Project Budget Filter")]
        public class ATPTEFMProjectBudgetFilter : CashFundManagement.DAC.Base.ATPTEFMBase, IBqlTable
        {
            #region Year

            [PXString(4)]
            [PXSelector(typeof(Search3<
                MasterFinYear.year,
                OrderBy<
                    Desc<MasterFinYear.year>>>))]
            [PXUIField(DisplayName = Messages.ATPTEFMMessages.Year)]
            public virtual string Year { get; set; }

            public abstract class year : PX.Data.BQL.BqlString.Field<year>
            { }

            #endregion Year
        }

        #endregion InternalTypes

        #region EPApproval Cache Attached

        [PXDBDate()]
        [PXDefault(typeof(ATPTEFMFundTransaction.date), PersistingCheck = PXPersistingCheck.Nothing)]
        protected virtual void EPApproval_DocDate_CacheAttached(PXCache sender)
        {
        }

        [PXDBInt()]
        [PXDefault(typeof(ATPTEFMFundTransaction.ownerID), PersistingCheck = PXPersistingCheck.Nothing)]
        protected virtual void EPApproval_DocumentOwnerID_CacheAttached(PXCache sender)
        {
        }

        [PXDBInt()]
        [PXDefault(typeof(ATPTEFMFundTransaction.requestedByID), PersistingCheck = PXPersistingCheck.Nothing)]
        protected virtual void EPApproval_BAccountID_CacheAttached(PXCache sender)
        {
        }

        [PXDBString(60, IsUnicode = true)]
        [PXDefault(typeof(ATPTEFMFundTransaction.descr), PersistingCheck = PXPersistingCheck.Nothing)]
        protected virtual void EPApproval_Descr_CacheAttached(PXCache sender)
        {
        }

        [PXDBDecimal(4)]
        [PXDefault(typeof(ATPTEFMFundTransaction.totalApprovalAmount), PersistingCheck = PXPersistingCheck.Nothing)]
        protected virtual void EPApproval_CuryTotalAmount_CacheAttached(PXCache sender)
        {
        }

        #endregion EPApproval Cache Attached
    }
}