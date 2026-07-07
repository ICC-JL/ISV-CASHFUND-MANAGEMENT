using CashFundManagement.Attributes;
using CashFundManagement.DAC;
using CashFundManagement.DAC.Setup;
using CashFundManagement.Extensions.DAC;
using PX.Data;
using PX.Data.BQL;
using PX.Data.BQL.Fluent;
using PX.Objects.AP;
using PX.Objects.EP;
using PX.Objects.GL;
using PX.Objects.GL.FinPeriods;
using System.Collections.Generic;
using System.Linq;
using static CashFundManagement.Classes.ATPTEFMBudgetLibrary;

namespace CashFundManagement.Classes {
    public class ATPTEFMProjectBudgetLibrary
    {
        /// <summary>
        /// Budget Parameters/Arguments for BQL Views
        /// </summary>
        /// <remarks>
        /// 2025-08-08 : Make method for getting receipts for efficient updates on query : RFS
        /// 2025-09-01 : Updated APPayment existence check to use .Any() instead of != null for accuracy and clarity in HasClosedReclassificationOrRefund. : RFS
        /// </remarks>
        public class ProjectBudgetParameters
        {
            public string RefNbr { get; set; }
            public string CuryID { get; set; }

            public int? ProjectID { get; set; }
            public int? LedgerID { get; set; }
            public int? ProjectTaskID { get; set; }
            public int? CostCodeID { get; set; }
            public int? InventoryID { get; set; }
            public int? AccountGroupID { get; set; }

            public string FinYear { get; set; }
            public string FromFinPeriodID { get; set; }
            public string ToFinPeriodID { get; set; }
            public string FinPeriodID { get; set; }


            public decimal? Amount { get; set; }
            public bool Approved { get; set; }
            public bool Released { get; set; }

            public OriginTypes OriginType { get; set; }
        }
        public class ApproveAmountCalculation
        {
            public const string CurrentModule = "F1";
            public const string AllModule = "F2";
            public const string NotApplicable = "NA";
        }
        public class UnapproveAmountCalculation
        {
            public const string CurrentModule = "F1";
            public const string AllModule = "F2";
            public const string NotApplicable = "NA";
        }

        public class ReturnAmountCalculation
        {
            public const string TotalReturnedAmount = "F1";
            public const string NotApplicable = "NA";
        }
        public class DocAmountCalculation
        {
            public const string CurrentRecordAmount = "F1";
            public const string NotApplicable = "NA";
        }
        public class RequestAmountCalculation
        {
            public const string ApprovedAndUnapproved = "F1";
            public const string NotApplicable = "NA";
        }
        public class BudgetAmountCalculation
        {
            public const string InitialBudgetMinusAmountSpentPlusReturns = "F1";
            public const string InitialBudgetMinusTotalApprovedAndUnapprovedPlusReturns = "F2";
            public const string InitialBudgetMinusApprovedSpentUnapprovedPlusReturns = "F3";
            public const string NotApplicable = "NA";
        }
        public class SpentAmountCalculation
        {
            public const string TotalJournalTransactionSpentAmount = "F1";
            public const string NotApplicable = "NA";
        }
        public static decimal GetBudgetAmt(PXGraph graph, ProjectBudgetParameters item)
        {
            DAC.ATPTEFMProjectBudgetLine obj =
                PXSelectGroupBy<DAC.ATPTEFMProjectBudgetLine,
                Where<DAC.ATPTEFMProjectBudgetLine.projectID, Equal<Required<DAC.ATPTEFMProjectBudgetLine.projectID>>,
                    And<DAC.ATPTEFMProjectBudgetLine.finYear, Equal<Required<DAC.ATPTEFMProjectBudgetLine.finYear>>,
                    And<DAC.ATPTEFMProjectBudgetLine.projectTaskID, Equal<Required<DAC.ATPTEFMProjectBudgetLine.projectTaskID>>,
                    And<DAC.ATPTEFMProjectBudgetLine.costCodeID, Equal<Required<DAC.ATPTEFMProjectBudgetLine.costCodeID>>,
                    And<DAC.ATPTEFMProjectBudgetLine.released, Equal<Required<DAC.ATPTEFMProjectBudgetLine.released>>,
                    And<DAC.ATPTEFMProjectBudgetLine.finPeriodID, Between<Required<DAC.ATPTEFMProjectBudgetLine.finPeriodID>, Required<DAC.ATPTEFMProjectBudgetLine.finPeriodID>>,
                    And<DAC.ATPTEFMProjectBudgetLine.ledgerID, Equal<@P.AsInt>,
                    And<DAC.ATPTEFMProjectBudgetLine.accountGroupID, Equal<@P.AsInt>>>>>>>>>,
                Aggregate<
                    Sum<DAC.ATPTEFMProjectBudgetLine.amount>>>
                .SelectWindowed(graph, 0, 1, item.ProjectID, item.FinYear, item.ProjectTaskID, item.CostCodeID, true, item.FromFinPeriodID, item.ToFinPeriodID, item.LedgerID, item.AccountGroupID);
            return obj == null ? 0 : obj.Amount ?? 0;
        }

        public static decimal GetSpentAmt(PXGraph graph, ProjectBudgetParameters item)
        {
            var obj =
                PXSelectJoin<GLTran,
                InnerJoin<Account, On<GLTran.accountID, Equal<Account.accountID>>>,
                Where<GLTran.projectID, Equal<Required<GLTran.projectID>>,
                    And<GLTran.taskID, Equal<Required<GLTran.taskID>>,
                    And<GLTran.costCodeID, Equal<Required<GLTran.costCodeID>>,
                    And<Account.accountGroupID, Equal<Required<Account.accountGroupID>>,
                    And<GLTran.finPeriodID, Between<Required<GLTran.finPeriodID>, Required<GLTran.finPeriodID>>>>>>>>
                .Select(graph, item.ProjectID, item.ProjectTaskID, item.CostCodeID, item.AccountGroupID, item.FromFinPeriodID, item.ToFinPeriodID).FirstTableItems.ToList();

            return obj == null ? 0 : obj.Sum(x => x.DebitAmt) - obj.Sum(x => x.CreditAmt) ?? 0;
        }
        public static decimal GetApprovedAmt(PXGraph graph, ProjectBudgetParameters item, ATPTEFMFeatures features, PXResultset<ATPTEFMProjectBudgetHistory> baseHistory)
        {
            decimal? PbudgetTotal = 0M;

            IEnumerable<ATPTEFMProjectBudgetHistory> history = GetFilteredApprovedOrUnapprovedHistory(features.ProjectBudgetApprovedAmount, baseHistory, item, true);
            PbudgetTotal += history.Sum(h => h.Amount ?? 0);

            return PbudgetTotal ?? 0;
        }
        public static decimal GetApprovedAdjAmt(PXGraph graph, ProjectBudgetParameters item, ATPTEFMFeatures features, PXResultset<ATPTEFMProjectBudgetHistory> baseHistory, bool isCurrentDoc = false)
        {
            decimal? PbudgetTotal = 0M;

            IEnumerable<ATPTEFMProjectBudgetHistory> history = GetFilteredApprovedOrUnapprovedHistory(features.ProjectBudgetApprovedAmount, baseHistory, item, true, isCurrentDoc);

            foreach (ATPTEFMProjectBudgetHistory result in history)
            {
                if (result.Origin == (int)OriginTypes.CashAdvance)
                {
                    PbudgetTotal += ProcessCAApprovedAdj(graph, result, item);
                }
                else if (result.Origin == (int)OriginTypes.FundTransaction)
                {
                    PbudgetTotal += ProcessFTApprovedAdj(graph, result, item);
                }
                else if (result.Origin == (int)OriginTypes.RequestForPayment)
                {
                    PbudgetTotal += ProcessRFPApprovedAdj(graph, result, item);
                }
            }
            return PbudgetTotal ?? 0;
        }
        public static decimal GetUnapprovedAmt(PXGraph graph, ProjectBudgetParameters item, ATPTEFMFeatures features, PXResultset<ATPTEFMProjectBudgetHistory> baseHistory)
        {
            decimal? PbudgetTotal = 0M;

            IEnumerable<ATPTEFMProjectBudgetHistory> history = GetFilteredApprovedOrUnapprovedHistory(features.ProjectBudgetUnapprovedAmount, baseHistory, item, false);
            PbudgetTotal += history.Sum(h => h.Amount ?? 0);

            return PbudgetTotal ?? 0;
        }
        public static decimal GetReturnAmounts(PXGraph graph, ProjectBudgetParameters item, ATPTEFMFeatures features, PXResultset<ATPTEFMProjectBudgetHistory> baseHistory, bool isAdjusted, bool isCurrentDoc = false)
        {
            decimal? PbudgetTotal = 0M;

            IEnumerable<ATPTEFMProjectBudgetHistory> history = null;

            if (features.ProjectBudgetReturnAmount == ReturnAmountCalculation.TotalReturnedAmount)
            {
                history = GetFilteredApprovedOrUnapprovedHistory(string.Empty, baseHistory, item, true, isCurrentDoc);
            }

            foreach (ATPTEFMProjectBudgetHistory result in history)
            {
                if (result.Origin == (int)OriginTypes.CashAdvance)
                {
                    PbudgetTotal += ProcessCAReturn(graph, result, item, isAdjusted);
                }
                else if (result.Origin == (int)OriginTypes.FundTransaction)
                {
                    PbudgetTotal += ProcessFTReturn(graph, result, item, isAdjusted);
                }
                else if (result.Origin == (int)OriginTypes.RequestForPayment)
                {
                    PbudgetTotal += ProcessRFPReturn(graph, result, item, isAdjusted);
                }
            }

            return PbudgetTotal ?? 0;
        }
        public static decimal? ModifyProjectBudgetAmt(PXGraph graph, DAC.Unbound.ATPTEFMPBudget item)
        {
            var _BudgetAmt = item.BudgetAmt;
            _BudgetAmt = item.BudgetAmt - item.DocAmt;
            return _BudgetAmt;
        }

        public static IEnumerable<DAC.Unbound.ATPTEFMPBudget> GenerateProjectBudget(PXGraph graph, List<ProjectBudgetParameters> items)
        {
            DAC.Setup.ATPTEFMFeatures Features = PXSetup<DAC.Setup.ATPTEFMFeatures>.Select(graph);

            var budgetView = new List<DAC.Unbound.ATPTEFMPBudget>();
            foreach (ProjectBudgetParameters item in items)
            {
                PXResultset<ATPTEFMProjectBudgetHistory> history = PXSelectJoin<DAC.ATPTEFMProjectBudgetHistory,
                        InnerJoin<MasterFinPeriod, On<MasterFinPeriod.finPeriodID, Equal<DAC.ATPTEFMProjectBudgetHistory.finPeriodID>>>,
                    Where<DAC.ATPTEFMProjectBudgetHistory.projectID, Equal<Required<DAC.ATPTEFMProjectBudgetHistory.projectID>>,
                        And<DAC.ATPTEFMProjectBudgetHistory.projectTaskID, Equal<Required<DAC.ATPTEFMProjectBudgetHistory.projectTaskID>>,
                        And<DAC.ATPTEFMProjectBudgetHistory.costCodeID, Equal<Required<DAC.ATPTEFMProjectBudgetHistory.costCodeID>>,
                        And<MasterFinPeriod.finYear, Equal<Required<MasterFinPeriod.finYear>>,
                        And<MasterFinPeriod.finPeriodID, Between<Required<MasterFinPeriod.finPeriodID>, Required<MasterFinPeriod.finPeriodID>>,
                        And<DAC.ATPTEFMProjectBudgetHistory.ledgerID, Equal<@P.AsInt>,
                        And<DAC.ATPTEFMProjectBudgetHistory.accountGroupID, Equal<@P.AsInt>>>>>>>>>
                    .Select(graph, item.ProjectID, item.ProjectTaskID, item.CostCodeID, item.FinYear, item.FromFinPeriodID, item.ToFinPeriodID, item.LedgerID, item.AccountGroupID);

                var budget = new DAC.Unbound.ATPTEFMPBudget();
                budget.LedgerID = item.LedgerID;
                budget.ProjectID = item.ProjectID;
                budget.ProjectTaskID = item.ProjectTaskID;
                budget.CostCodeID = item.CostCodeID;
                budget.CuryID = item.CuryID;
                budget.DocAmt = Features.ProjectBudgetDocumentAmount != DocAmountCalculation.NotApplicable ? item.Amount : 0;
                budget.RequestAmt = Features.ProjectBudgetRequestAmount != RequestAmountCalculation.NotApplicable ? 0 : 0;
                budget.BudgetAmt = Features.ProjectBudgetBudgetAmount != BudgetAmountCalculation.NotApplicable ? GetBudgetAmt(graph, item) : 0;
                budget.SpentAmt = Features.ProjectBudgetSpentAmount != SpentAmountCalculation.NotApplicable ? GetSpentAmt(graph, item) : 0;
                budget.ApprovedAmt = Features.ProjectBudgetApprovedAmount != ApproveAmountCalculation.NotApplicable ? GetApprovedAmt(graph, item, Features, history) : 0;
                budget.ApprovedAdjAmt = Features.ProjectBudgetBudgetAmount == BudgetAmountCalculation.InitialBudgetMinusApprovedSpentUnapprovedPlusReturns ? GetApprovedAdjAmt(graph, item, Features, history) : 0;
                budget.CurrentApprovedAdjAmt = Features.ProjectBudgetBudgetAmount == BudgetAmountCalculation.InitialBudgetMinusApprovedSpentUnapprovedPlusReturns ? GetApprovedAdjAmt(graph, item, Features, history, true) : 0;
                budget.UnapprovedAmt = Features.ProjectBudgetUnapprovedAmount != UnapproveAmountCalculation.NotApplicable ? GetUnapprovedAmt(graph, item, Features, history) : 0;
                budget.ReturnAmt = GetReturnAmounts(graph, item, Features, history, false);
                budget.CurrentReturnAmt = GetReturnAmounts(graph, item, Features, history, false, true);
                budget.ReturnAmtAdj = Features.ProjectBudgetBudgetAmount == BudgetAmountCalculation.InitialBudgetMinusApprovedSpentUnapprovedPlusReturns ? GetReturnAmounts(graph, item, Features, history, true) : 0;
                budget.CurrentReturnAmtAdj = Features.ProjectBudgetBudgetAmount == BudgetAmountCalculation.InitialBudgetMinusApprovedSpentUnapprovedPlusReturns ? GetReturnAmounts(graph, item, Features, history, true, true) : 0;
                budget.Year = item.FinYear;
                budget.FinPeriodID = item.ToFinPeriodID;
                budget.RefNbr = item.RefNbr;
                budget.Origin = (int)item.OriginType;
                budget.IsApproved = item.Approved;
                budget.AccountGroupID = item.AccountGroupID;
                budget.InitialBudget = GetBudgetAmt(graph, item);
                budget.FinPeriodID = item.FinPeriodID;
                budgetView.Add(budget);
            }

            return budgetView
                .GroupBy(x => new
                {
                    x.ProjectID,
                    x.LedgerID,
                    x.ProjectTaskID,
                    x.CostCodeID,
                    x.AccountGroupID,
                    x.CuryID,
                    x.InitialBudget,
                    x.RequestAmt,
                    x.BudgetAmt,
                    x.SpentAmt,
                    x.Year,
                    x.FinPeriodID,
                    x.RefNbr,
                    x.Origin,
                    x.IsApproved,
                    x.ApprovedAmt,
                    x.ApprovedAdjAmt,
                    x.CurrentApprovedAdjAmt,
                    x.ReturnAmt,
                    x.CurrentReturnAmt,
                    x.ReturnAmtAdj,
                    x.CurrentReturnAmtAdj,
                    x.UnapprovedAmt
                })
                .Select(x => new DAC.Unbound.ATPTEFMPBudget
                {
                    LedgerID = x.Key.LedgerID,
                    ProjectID = x.Key.ProjectID,
                    ProjectTaskID = x.Key.ProjectTaskID,
                    CostCodeID = x.Key.CostCodeID,
                    AccountGroupID = x.Key.AccountGroupID,
                    CuryID = x.Key.CuryID,
                    InitialBudget = x.Key.InitialBudget,
                    DocAmt = ComputeDocAmt(Features.ProjectBudgetDocumentAmount, x.Sum(y => y.DocAmt)),
                    RequestAmt = ComputeRequestAmt(Features.ProjectBudgetRequestAmount, x.Sum(y => y.DocAmt), x.Key.ApprovedAmt, x.Key.UnapprovedAmt),
                    BudgetAmt = ComputeBudgetAmt(Features.ProjectBudgetBudgetAmount, x.Key.BudgetAmt, x.Sum(y => y.DocAmt), x.Key.SpentAmt, x.Key.ApprovedAmt, x.Key.UnapprovedAmt, x.Key.ApprovedAdjAmt, x.Key.CurrentApprovedAdjAmt, x.Key.ReturnAmt, x.Key.CurrentReturnAmt, x.Key.ReturnAmtAdj, x.Key.CurrentReturnAmtAdj, x.Key.IsApproved),
                    SpentAmt = ComputeSpentAmt(Features.ProjectBudgetSpentAmount, x.Key.SpentAmt),
                    ApprovedAmt = ComputeApprovedAmt(x.Key.ApprovedAmt, x.Sum(y => y.DocAmt), x.Key.IsApproved),
                    UnapprovedAmt = ComputeUnapprovedAmt(x.Key.UnapprovedAmt, x.Sum(y => y.DocAmt), x.Key.IsApproved),
                    ReturnAmt = ComputeReturnAmt(Features.ProjectBudgetReturnAmount, x.Key.ReturnAmt, x.Key.CurrentReturnAmt),
                    //DocAmt = ComputeAmounts("DocAmt", Features.ProjectBudgetDocumentAmount, x.Key.IsApproved, x.Key.BudgetAmt, x.Key.SpentAmt, x.Key.ApprovedAmt, x.Key.UnapprovedAmt, x.Sum(y => y.DocAmt), x.Sum(y => y.RequestAmt), x.Key.ApprovedAdjAmt, x.Key.CurrentApprovedAdjAmt, x.Key.ReturnAmt, x.Key.ReturnAmtAdj, x.Key.CurrentReturnAmt, x.Key.CurrentReturnAmtAdj),
                    //RequestAmt = ComputeAmounts("RequestAmt", Features.ProjectBudgetRequestAmount, x.Key.IsApproved, x.Key.BudgetAmt, x.Key.SpentAmt, x.Key.ApprovedAmt, x.Key.UnapprovedAmt, x.Sum(y => y.DocAmt), x.Sum(y => y.RequestAmt), x.Key.ApprovedAdjAmt, x.Key.CurrentApprovedAdjAmt, x.Key.ReturnAmt, x.Key.ReturnAmtAdj, x.Key.CurrentReturnAmt, x.Key.CurrentReturnAmtAdj),
                    //BudgetAmt = ComputeAmounts("BudgetAmt", Features.ProjectBudgetBudgetAmount, x.Key.IsApproved, x.Key.BudgetAmt, x.Key.SpentAmt, x.Key.ApprovedAmt, x.Key.UnapprovedAmt, x.Sum(y => y.DocAmt), x.Sum(y => y.RequestAmt), x.Key.ApprovedAdjAmt, x.Key.CurrentApprovedAdjAmt, x.Key.ReturnAmt, x.Key.ReturnAmtAdj, x.Key.CurrentReturnAmt, x.Key.CurrentReturnAmtAdj),
                    //SpentAmt = ComputeAmounts("SpentAmt", Features.ProjectBudgetSpentAmount, x.Key.IsApproved, x.Key.BudgetAmt, x.Key.SpentAmt, x.Key.ApprovedAmt, x.Key.UnapprovedAmt, x.Sum(y => y.DocAmt), x.Sum(y => y.RequestAmt), x.Key.ApprovedAdjAmt, x.Key.CurrentApprovedAdjAmt, x.Key.ReturnAmt, x.Key.ReturnAmtAdj, x.Key.CurrentReturnAmt, x.Key.CurrentReturnAmtAdj),
                    //ApprovedAmt = ComputeAmounts("ApprovedAmt", Features.ProjectBudgetApprovedAmount, x.Key.IsApproved, x.Key.BudgetAmt, x.Key.SpentAmt, x.Key.ApprovedAmt, x.Key.UnapprovedAmt, x.Sum(y => y.DocAmt), x.Sum(y => y.RequestAmt), x.Key.ApprovedAdjAmt, x.Key.CurrentApprovedAdjAmt, x.Key.ReturnAmt, x.Key.ReturnAmtAdj, x.Key.CurrentReturnAmt, x.Key.CurrentReturnAmtAdj),
                    //UnapprovedAmt = ComputeAmounts("UnapprovedAmt", Features.ProjectBudgetUnapprovedAmount, x.Key.IsApproved, x.Key.BudgetAmt, x.Key.SpentAmt, x.Key.ApprovedAmt, x.Key.UnapprovedAmt, x.Sum(y => y.DocAmt), x.Sum(y => y.RequestAmt), x.Key.ApprovedAdjAmt, x.Key.CurrentApprovedAdjAmt, x.Key.ReturnAmt, x.Key.ReturnAmtAdj, x.Key.CurrentReturnAmt, x.Key.CurrentReturnAmtAdj),
                    //ReturnAmt = ComputeAmounts("ReturnAmt", Features.ProjectBudgetReturnAmount, x.Key.IsApproved, x.Key.BudgetAmt, x.Key.SpentAmt, x.Sum(y => y.ApprovedAmt), x.Sum(y => y.UnapprovedAmt), x.Sum(y => y.DocAmt), x.Sum(y => y.RequestAmt), x.Key.ApprovedAdjAmt, x.Key.CurrentApprovedAdjAmt, x.Key.ReturnAmt, x.Key.ReturnAmtAdj, x.Key.CurrentReturnAmt, x.Key.CurrentReturnAmtAdj),
                    Year = x.Key.Year,
                    FinPeriodID = x.Key.FinPeriodID,
                    RefNbr = x.Key.RefNbr,
                    Origin = x.Key.Origin,
                    IsApproved = x.Key.IsApproved
                }).ToList();
        }
        private static decimal? ComputeDocAmt(string feature, decimal? docAmt)
        {
            decimal? retValue = 0;
            switch (feature)
            {
                case DocAmountCalculation.CurrentRecordAmount: retValue = docAmt; break;
                default: return 0;
            }
            return retValue;
        }
        private static decimal? ComputeRequestAmt(string feature, decimal? docAmt, decimal? approvedAmt, decimal? unapprovedAmt)
        {
            decimal? retValue = 0;
            switch (feature)
            {
                case RequestAmountCalculation.ApprovedAndUnapproved: retValue = approvedAmt + unapprovedAmt; break;
                default: return 0;
            }
            return retValue + docAmt;
        }
        private static decimal? ComputeBudgetAmt(string feature, decimal? budgetAmt, decimal? docAmt, decimal? spentAmt, decimal? approvedAmt, decimal? unapprovedAmt, decimal? approveAdj, decimal? currentApproveAdj, decimal? returnAmt, decimal? currentReturnAmt, decimal? returnAmtAdj, decimal? currentReturnAmtAdj, bool? isApproved)
        {
            decimal? retValue = 0;
            switch (feature)
            {
                case BudgetAmountCalculation.InitialBudgetMinusAmountSpentPlusReturns: retValue = (budgetAmt - spentAmt + (returnAmt + currentReturnAmt)); break;
                case BudgetAmountCalculation.InitialBudgetMinusTotalApprovedAndUnapprovedPlusReturns: retValue = (budgetAmt - ((approvedAmt + (isApproved == true ? docAmt : 0)) + (unapprovedAmt + (isApproved == false ? docAmt : 0))) + (returnAmt + currentReturnAmt)); break;
                case BudgetAmountCalculation.InitialBudgetMinusApprovedSpentUnapprovedPlusReturns: retValue = budgetAmt - (((approvedAmt + (isApproved == true ? (docAmt - currentApproveAdj) : docAmt)) - approveAdj) + spentAmt + unapprovedAmt) + ((returnAmt + currentReturnAmt) - (returnAmtAdj + currentReturnAmtAdj)); break;
                default: return 0;
            }
            PXTrace.WriteInformation($"ComputeAmounts Parameters: InitialBudget={budgetAmt}, appAdj={((approvedAmt + (isApproved == true ? (docAmt - currentApproveAdj) : docAmt)) - approveAdj)}, retAdj={((returnAmt + currentReturnAmt) - (returnAmtAdj + currentReturnAmtAdj))}, spentAmt={spentAmt}, approvedAmt={approvedAmt}, unapprovedAmt={unapprovedAmt}, docAmt={docAmt}, approveAdj={approveAdj}, currentApproveAdj={currentApproveAdj}, returnAmt={returnAmt}, returnAmtAdj={returnAmtAdj}, currentReturnAmt={currentReturnAmt}, currentReturnAmtAdj={currentReturnAmtAdj}");
            PXTrace.WriteInformation($"1st Calc={(budgetAmt - ((approvedAmt + (isApproved == true ? docAmt : 0)) + (unapprovedAmt + (isApproved == false ? docAmt : 0))) + (returnAmt + currentReturnAmt))}");
            return retValue;
        }
        private static decimal? ComputeSpentAmt(string feature, decimal? spentAmt)
        {
            decimal? retValue = 0;
            switch (feature)
            {
                case SpentAmountCalculation.TotalJournalTransactionSpentAmount: retValue = spentAmt; break;
                default: retValue = 0; break;
            }
            return retValue;
        }
        private static decimal? ComputeApprovedAmt(decimal? approvedAmt, decimal? docAmt, bool? isApproved)
        {
            return approvedAmt + (isApproved == true ? docAmt : 0);
        }
        private static decimal? ComputeUnapprovedAmt(decimal? unapprovedAmt, decimal? docAmt, bool? isApproved)
        {
            return unapprovedAmt + (isApproved == false ? docAmt : 0);
        }
        private static decimal? ComputeReturnAmt(string feature, decimal? returnAmt, decimal? currentReturnAmt)
        {
            decimal? retValue = 0;
            switch (feature)
            {
                case ReturnAmountCalculation.TotalReturnedAmount: retValue = returnAmt + currentReturnAmt; break;
                default: retValue = 0; break;
            }
            return retValue;
        }
        //private static decimal? ComputeAmounts(string amtType, string feature, bool? isApproved, decimal? budgetAmt, decimal? spentAmt, decimal? approvedAmt, decimal? unapprovedAmt, decimal? docAmt, decimal? requestAmt, decimal? approveAdj, decimal? currentApproveAdj, decimal? returnAmt, decimal? returnAmtAdj, decimal? currentReturnAmt, decimal? currentReturnAmtAdj)
        //{
        //    decimal? retValue = 0;
        //    switch (amtType)
        //    {
        //        case "DocAmt":
        //            switch (feature)
        //            {
        //                case "F1": retValue = docAmt; break;
        //                default: return 0;
        //            }
        //            return retValue;
        //        case "RequestAmt":
        //            switch (feature)
        //            {
        //                case "F1": retValue = approvedAmt + unapprovedAmt; break;
        //                default: return 0;
        //            }
        //            return retValue + docAmt;
        //        case "BudgetAmt":
        //            switch (feature)
        //            {
        //                case "F1": retValue = (budgetAmt - spentAmt + (returnAmt + currentReturnAmt)); break;
        //                case "F2": retValue = (budgetAmt - (approvedAmt + unapprovedAmt + docAmt) + (returnAmt + currentReturnAmt)); break;
        //                case "F3": retValue = budgetAmt - ((approvedAmt - approveAdj) + spentAmt + unapprovedAmt + (isApproved == true ? (docAmt - currentApproveAdj) : docAmt)) + ((returnAmt + currentReturnAmt) - (returnAmtAdj + currentReturnAmtAdj)); break;
        //                default: return 0;
        //            }
        //            PXTrace.WriteInformation($"Project - ComputeAmounts Parameters: InitialBudget={budgetAmt}, appAdj={((approvedAmt + (isApproved == true ? (docAmt - currentApproveAdj) : docAmt)) - approveAdj)}, retAdj={((returnAmt + currentReturnAmt) - (returnAmtAdj + currentReturnAmtAdj))}, spentAmt={spentAmt}, approvedAmt={approvedAmt}, unapprovedAmt={unapprovedAmt}, docAmt={docAmt}, requestAmt={requestAmt}, approveAdj={approveAdj}, currentApproveAdj={currentApproveAdj}, returnAmt={returnAmt}, returnAmtAdj={returnAmtAdj}, currentReturnAmt={currentReturnAmt}, currentReturnAmtAdj={currentReturnAmtAdj}");
        //            PXTrace.WriteInformation($"Project - 1st Calc={(budgetAmt - ((approvedAmt + (isApproved == true ? docAmt : 0)) + (unapprovedAmt + (isApproved == false ? docAmt : 0))) + (returnAmt + currentReturnAmt))}");
        //            return retValue;
        //        case "SpentAmt":
        //            switch (feature)
        //            {
        //                case "F1": retValue = spentAmt; break;
        //                default: retValue = 0; break;
        //            }
        //            return retValue;
        //        case "ApprovedAmt":
        //            return approvedAmt + (isApproved == true ? docAmt : 0);
        //        case "UnapprovedAmt":
        //            return unapprovedAmt + (isApproved == false ? docAmt : 0);
        //        case "ReturnAmt":
        //            switch (feature)
        //            {
        //                case "F1": retValue = returnAmt + currentReturnAmt; break;
        //                default: retValue = 0; break;
        //            }
        //            return retValue;
        //    }
        //    return retValue;
        //}

        public static void ProjectBudgetVisibility(PXCache cache, DAC.Setup.ATPTEFMFeatures featureSetup, string module)
        {
            PXUIFieldAttribute.SetVisible<DAC.Unbound.ATPTEFMPBudget.docAmt>(cache, null, featureSetup.ProjectBudgetDocumentAmount != DocAmountCalculation.NotApplicable ? true : false);
            PXUIFieldAttribute.SetVisible<DAC.Unbound.ATPTEFMPBudget.requestAmt>(cache, null, featureSetup.ProjectBudgetRequestAmount != RequestAmountCalculation.NotApplicable ? true : false);
            PXUIFieldAttribute.SetVisible<DAC.Unbound.ATPTEFMPBudget.budgetAmt>(cache, null, featureSetup.ProjectBudgetBudgetAmount != BudgetAmountCalculation.NotApplicable ? true : false);
            PXUIFieldAttribute.SetVisible<DAC.Unbound.ATPTEFMPBudget.spentAmt>(cache, null, featureSetup.ProjectBudgetSpentAmount != SpentAmountCalculation.NotApplicable ? true : false);
            PXUIFieldAttribute.SetVisible<DAC.Unbound.ATPTEFMPBudget.approvedAmt>(cache, null, featureSetup.ProjectBudgetApprovedAmount != ApproveAmountCalculation.NotApplicable ? true : false);
            PXUIFieldAttribute.SetVisible<DAC.Unbound.ATPTEFMPBudget.unapprovedAmt>(cache, null, featureSetup.ProjectBudgetUnapprovedAmount != UnapproveAmountCalculation.NotApplicable ? true : false);
            PXUIFieldAttribute.SetVisible<DAC.Unbound.ATPTEFMPBudget.returnAmt>(cache, null, featureSetup.ProjectBudgetReturnAmount != ReturnAmountCalculation.NotApplicable ? true : false);

            if (featureSetup.ProjectBudgetDocumentAmountLabel != null)
                PXUIFieldAttribute.SetDisplayName<DAC.Unbound.ATPTEFMPBudget.docAmt>(cache, featureSetup.ProjectBudgetDocumentAmountLabel);
            if (featureSetup.ProjectBudgetRequestAmountLabel != null)
                PXUIFieldAttribute.SetDisplayName<DAC.Unbound.ATPTEFMPBudget.requestAmt>(cache, featureSetup.ProjectBudgetRequestAmountLabel);
            if (featureSetup.ProjectBudgetBudgetAmountLabel != null)
                PXUIFieldAttribute.SetDisplayName<DAC.Unbound.ATPTEFMPBudget.budgetAmt>(cache, featureSetup.ProjectBudgetBudgetAmountLabel);
            if (featureSetup.ProjectBudgetSpentAmountLabel != null)
                PXUIFieldAttribute.SetDisplayName<DAC.Unbound.ATPTEFMPBudget.spentAmt>(cache, featureSetup.ProjectBudgetSpentAmountLabel);
            if (featureSetup.ProjectBudgetApprovedAmountLabel != null)
                PXUIFieldAttribute.SetDisplayName<DAC.Unbound.ATPTEFMPBudget.approvedAmt>(cache, featureSetup.ProjectBudgetApprovedAmountLabel);
            if (featureSetup.ProjectBudgetUnapprovedAmountLabel != null)
                PXUIFieldAttribute.SetDisplayName<DAC.Unbound.ATPTEFMPBudget.unapprovedAmt>(cache, featureSetup.ProjectBudgetUnapprovedAmountLabel);
            if (featureSetup.ProjectBudgetReturnAmountLabel != null)
                PXUIFieldAttribute.SetDisplayName<DAC.Unbound.ATPTEFMPBudget.returnAmt>(cache, featureSetup.ProjectBudgetReturnAmountLabel);

            cache.AllowSelect = featureSetup?.ProjectBudgetModules?.Split(',').Contains(module) ?? false;
        }

        public static void AddProjectBudgetHistory(PXSelect<DAC.ATPTEFMProjectBudgetHistory> historyView, List<DAC.Unbound.ATPTEFMPBudget> items)
        {
            foreach (DAC.Unbound.ATPTEFMPBudget item in items)
            {
                if (HasNull(item.ProjectID, item.ProjectTaskID, item.CostCodeID, item.AccountGroupID)) continue;
                //if ((item.DocAmt ?? 0m) <= 0m) continue;

                DAC.ATPTEFMProjectBudgetHistory history = historyView.Select()
                    .Where(x => x.GetItem<DAC.ATPTEFMProjectBudgetHistory>().ProjectID == item.ProjectID
                             && x.GetItem<DAC.ATPTEFMProjectBudgetHistory>().ProjectTaskID == item.ProjectTaskID
                             && x.GetItem<DAC.ATPTEFMProjectBudgetHistory>().CostCodeID == item.CostCodeID
                             && x.GetItem<DAC.ATPTEFMProjectBudgetHistory>().AccountGroupID == item.AccountGroupID
                             && x.GetItem<DAC.ATPTEFMProjectBudgetHistory>().Year == item.Year
                             && x.GetItem<DAC.ATPTEFMProjectBudgetHistory>().LedgerID == item.LedgerID
                             && x.GetItem<DAC.ATPTEFMProjectBudgetHistory>().FinPeriodID == item.FinPeriodID
                             && x.GetItem<DAC.ATPTEFMProjectBudgetHistory>().Origin == item.Origin
                             && x.GetItem<DAC.ATPTEFMProjectBudgetHistory>().RefNbr == item.RefNbr)
                    .SingleOrDefault();

                if (history == null)
                {
                    history = new DAC.ATPTEFMProjectBudgetHistory()
                    {
                        LedgerID = item.LedgerID,
                        ProjectID = item.ProjectID,
                        ProjectTaskID = item.ProjectTaskID,
                        CostCodeID = item.CostCodeID,
                        AccountGroupID = item.AccountGroupID,
                        Year = item.Year,
                        FinPeriodID = item.FinPeriodID,
                        Origin = item.Origin,
                        RefNbr = item.RefNbr,
                        IsActive = true
                    };
                }

                history.CuryID = item.CuryID;
                history.Amount = (history.Amount ?? 0) + item.DocAmt;
                history.CuryAmt = (history.CuryAmt ?? 0) + item.DocAmt;

                history.IsApproved = item.IsApproved ?? false;
                history.IsReleased = item.IsReleased ?? false;

                history = historyView.Update(history);
            }
        }

        public static void DeleteProjectBudgetHistory(PXSelect<DAC.ATPTEFMProjectBudgetHistory> historyView, List<DAC.Unbound.ATPTEFMPBudget> items, bool deleteAll = false)
        {
            if (!deleteAll)
            {
                foreach (DAC.Unbound.ATPTEFMPBudget item in items)
                {
                    //if ((item.DocAmt ?? 0m) <= 0m) continue;

                    DAC.ATPTEFMProjectBudgetHistory history = historyView.Select()
                        .Where(x => x.GetItem<DAC.ATPTEFMProjectBudgetHistory>().ProjectID == item.ProjectID
                                 && x.GetItem<DAC.ATPTEFMProjectBudgetHistory>().ProjectTaskID == item.ProjectTaskID
                                 && x.GetItem<DAC.ATPTEFMProjectBudgetHistory>().CostCodeID == item.CostCodeID
                                 && x.GetItem<DAC.ATPTEFMProjectBudgetHistory>().FinPeriodID == item.FinPeriodID
                                 && x.GetItem<DAC.ATPTEFMProjectBudgetHistory>().AccountGroupID == item.AccountGroupID
                                 && x.GetItem<DAC.ATPTEFMProjectBudgetHistory>().Year == item.Year
                                 && x.GetItem<DAC.ATPTEFMProjectBudgetHistory>().LedgerID == item.LedgerID
                                 && x.GetItem<DAC.ATPTEFMProjectBudgetHistory>().Origin == item.Origin
                                 && x.GetItem<DAC.ATPTEFMProjectBudgetHistory>().RefNbr == item.RefNbr)
                        .SingleOrDefault();

                    if (history != null)
                    {
                        history = historyView.Delete(history);
                        historyView.Cache.Persist(PXDBOperation.Delete);
                    }
                }
            }
            else
            {
                if (items.First() == null) return;

                DAC.Unbound.ATPTEFMPBudget item = items.First();

                var historyCollection = historyView.Select()
                    .Where(x => x.GetItem<DAC.ATPTEFMProjectBudgetHistory>().Origin == item.Origin
                             && x.GetItem<DAC.ATPTEFMProjectBudgetHistory>().RefNbr == item.RefNbr)
                    .ToList();

                foreach (DAC.ATPTEFMProjectBudgetHistory rows in historyCollection)
                {
                    historyView.Delete(rows);
                    historyView.Cache.Persist(PXDBOperation.Delete);
                }
            }
        }

        public static bool ProjectBudgetVisible(DAC.Setup.ATPTEFMFeatures featureSetup, string module)
            => featureSetup?.ProjectBudgetModules?.Split(',').Contains(module) ?? false;

        private static PXResultset<ATPTEFMCAReceiptDetail> GetCAReceipts(PXGraph graph, ATPTEFMProjectBudgetHistory history)
        {
            return PXSelectJoin<ATPTEFMCAReceiptDetail,
                        InnerJoin<Account,
                        On<ATPTEFMCAReceiptDetail.accountID, Equal<Account.accountID>>>,
                        Where<ATPTEFMCAReceiptDetail.cashAdvanceNbr, Equal<@P.AsString>,
                        And<ATPTEFMCAReceiptDetail.projectID, Equal<@P.AsInt>,
                        And<ATPTEFMCAReceiptDetail.projectTaskID, Equal<@P.AsInt>,
                        And<ATPTEFMCAReceiptDetail.costCodeID, Equal<@P.AsInt>,
                        And<Account.accountGroupID, Equal<@P.AsInt>,
                        And<ATPTEFMCAReceiptDetail.reversed, Equal<False>>>>>>>>
                        .Select(graph, history.RefNbr, history.ProjectID, history.ProjectTaskID, history.CostCodeID, history.AccountGroupID);
        }
        private static PXResultset<ATPTEFMFundTransactionReceiptDetail> GetFTReceipts(PXGraph graph, ATPTEFMProjectBudgetHistory history)
        {
            return PXSelectJoin<ATPTEFMFundTransactionReceiptDetail,
                        InnerJoin<Account,
                        On<ATPTEFMFundTransactionReceiptDetail.accountID, Equal<Account.accountID>>>,
                        Where<ATPTEFMFundTransactionReceiptDetail.fundTransactionRefNbr, Equal<@P.AsString>,
                        And<ATPTEFMFundTransactionReceiptDetail.projectID, Equal<@P.AsInt>,
                        And<ATPTEFMFundTransactionReceiptDetail.projectTaskID, Equal<@P.AsInt>,
                        And<ATPTEFMFundTransactionReceiptDetail.costCodeID, Equal<@P.AsInt>,
                        And<Account.accountGroupID, Equal<@P.AsInt>>>>>>>.
                        Select(graph, history.RefNbr, history.ProjectID, history.ProjectTaskID, history.CostCodeID, history.AccountGroupID);
        }
        //Get reclassified FT receipts without checking for acct and sub, Reclassification receipts use different acct and sub
        private static ATPTEFMFundTransactionReclassficationReceiptDetail GetReclassifiedFTReceipts(PXGraph graph, ATPTEFMProjectBudgetHistory history, ProjectBudgetParameters item)
        {
            return PXSelectJoin<ATPTEFMFundTransactionReclassficationReceiptDetail,
                InnerJoin<EPExpenseClaimDetails,
                On<ATPTEFMFundTransactionReclassficationReceiptDetail.expenseReceiptRefNbr, Equal<EPExpenseClaimDetails.claimDetailCD>>,
                InnerJoin<MasterFinPeriod, On<EPExpenseClaimDetails.expenseDate, Between<MasterFinPeriod.startDate, MasterFinPeriod.endDate>>>>,
                Where<ATPTEFMFundTransactionReclassficationReceiptDetail.fundTransactionRefNbr, Equal<@P.AsString>,
                And<MasterFinPeriod.finPeriodID, Between<@P.AsString, @P.AsString>>>>
                .Select(graph, history.RefNbr, item.FromFinPeriodID, item.ToFinPeriodID);
        }
        private static PXResultset<EPExpenseClaimDetails> GetRFPReceipts(PXGraph graph, ATPTEFMProjectBudgetHistory history)
        {
            return PXSelectJoin<EPExpenseClaimDetails,
                        InnerJoin<Account,
                        On<EPExpenseClaimDetails.expenseAccountID, Equal<Account.accountID>>>,
                        Where<EPExpenseClaimDetails.refNbr, Equal<@P.AsString>,
                        And<ATPTEFMEPExpenseClaimDetailsExt.usrATPTEFMTranType, Equal<@P.AsString>,
                        And<EPExpenseClaimDetails.contractID, Equal<@P.AsInt>,
                        And<EPExpenseClaimDetails.taskID, Equal<@P.AsInt>,
                        And<EPExpenseClaimDetails.costCodeID, Equal<@P.AsInt>,
                        And<Account.accountGroupID, Equal<@P.AsInt>>>>>>>>
                        .Select(graph, history.RefNbr, ATPTEFMExpenseTypeAttribute.RequestforPayment, history.ProjectID, history.ProjectTaskID, history.CostCodeID, history.AccountGroupID);
        }
        private static IEnumerable<ATPTEFMProjectBudgetHistory> GetFilteredApprovedOrUnapprovedHistory(string calcSetup, PXResultset<ATPTEFMProjectBudgetHistory> baseHistory, ProjectBudgetParameters item, bool isApproved, bool isCurrentDoc = false)
        {
            IEnumerable<ATPTEFMProjectBudgetHistory> history = null;

            if (isCurrentDoc)
            {
                if (calcSetup == ApproveAmountCalculation.CurrentModule)
                {
                    history = baseHistory
                        .Select(r => r.GetItem<ATPTEFMProjectBudgetHistory>())
                        .Where(h => h.IsApproved == isApproved && h.RefNbr == item.RefNbr && h.Origin == (int)item.OriginType);
                }
                else
                {
                    history = baseHistory
                        .Select(r => r.GetItem<ATPTEFMProjectBudgetHistory>())
                        .Where(h => h.IsApproved == isApproved && h.RefNbr == item.RefNbr);
                }
            }
            else
            {
                if (calcSetup == ApproveAmountCalculation.CurrentModule)
                {
                    history = baseHistory
                        .Select(r => r.GetItem<ATPTEFMProjectBudgetHistory>())
                        .Where(h => h.IsApproved == isApproved && h.RefNbr != item.RefNbr && h.Origin == (int)item.OriginType);
                }
                else
                {
                    history = baseHistory
                        .Select(r => r.GetItem<ATPTEFMProjectBudgetHistory>())
                        .Where(h => h.IsApproved == isApproved && h.RefNbr != item.RefNbr);
                }
            }

            return history;
        }
        private static bool HasValidAPDocumentInfo(EPExpenseClaimDetails expenseDetails)
        {
            return expenseDetails != null
                && expenseDetails.APDocType != null
                && expenseDetails.APRefNbr != null
                && expenseDetails.APLineNbr != null;
        }
        private static decimal GetReleasedNonDebitInvoiceAmount(PXGraph graph, ProjectBudgetParameters item, EPExpenseClaimDetails er)
        {
            return GetAPTransactionAmount(graph, item, er, isDebitAdjustment: false, requireReleased: true);
        }
        private static decimal GetAPTransactionAmount(PXGraph graph, ProjectBudgetParameters item, EPExpenseClaimDetails er, bool isDebitAdjustment, bool requireReleased = false)
        {
            if (isDebitAdjustment)
            {
                if (requireReleased)
                    return PXSelectJoin<
                        APTran,
                        InnerJoin<APInvoice,
                        On<APInvoice.refNbr, Equal<APTran.refNbr>,
                        And<APInvoice.docType, Equal<APTran.tranType>>>>,
                        Where<APInvoice.origRefNbr, Equal<@P.AsString>,
                            And<APTran.origLineNbr, Equal<@P.AsInt>,
                            And<APInvoice.origDocType, Equal<@P.AsString>,
                            And<APInvoice.docType, Equal<APDocType.debitAdj>,
                            And<APInvoice.released, Equal<True>,
                            And<APInvoice.finPeriodID, Between<@P.AsString, @P.AsString>>>>>>>>
                        .Select(graph, er.APRefNbr, er.APLineNbr, er.APDocType, item.FromFinPeriodID, item.ToFinPeriodID)
                        .RowCast<APTran>()
                        .Sum(t => (t.CuryTranAmt ?? 0m) + (t.CuryRetainageAmt ?? 0m));
                else
                    return PXSelectJoin<
                        APTran,
                        InnerJoin<APInvoice,
                        On<APInvoice.refNbr, Equal<APTran.refNbr>,
                        And<APInvoice.docType, Equal<APTran.tranType>>>>,
                        Where<APInvoice.origRefNbr, Equal<@P.AsString>,
                            And<APTran.origLineNbr, Equal<@P.AsInt>,
                            And<APInvoice.origDocType, Equal<@P.AsString>,
                            And<APInvoice.docType, Equal<APDocType.debitAdj>,
                            And<APInvoice.finPeriodID, Between<@P.AsString, @P.AsString>>>>>>>
                        .Select(graph, er.APRefNbr, er.APLineNbr, er.APDocType, item.FromFinPeriodID, item.ToFinPeriodID)
                        .RowCast<APTran>()
                        .Sum(t => (t.CuryTranAmt ?? 0m) + (t.CuryRetainageAmt ?? 0m));
            }
            else
            {
                return PXSelectJoin<
                    APTran,
                    InnerJoin<APInvoice,
                    On<APInvoice.refNbr, Equal<APTran.refNbr>,
                    And<APInvoice.docType, Equal<APTran.tranType>>>>,
                    Where<APTran.refNbr, Equal<@P.AsString>,
                        And<APTran.lineNbr, Equal<@P.AsInt>,
                        And<APTran.tranType, Equal<@P.AsString>,
                        And<APInvoice.released, Equal<True>,
                        And<APInvoice.finPeriodID, Between<@P.AsString, @P.AsString>>>>>>>
                    .Select(graph, er.APRefNbr, er.APLineNbr, er.APDocType, item.FromFinPeriodID, item.ToFinPeriodID)
                    .RowCast<APTran>()
                    .Sum(t => (t.CuryTranAmt ?? 0m) + (t.CuryRetainageAmt ?? 0m));
            }
        }
        private static ATPTEFMCashAdvance GetCashAdvanceByRefNumber(PXGraph graph, string refNumber)
        {
            return SelectFrom<ATPTEFMCashAdvance>
                .Where<ATPTEFMCashAdvance.cashAdvanceNbr.IsEqual<@P.AsString>>
                .View.Select(graph, refNumber);
        }
        private static bool IsCashAdvanceClosed(ATPTEFMCashAdvance cashAdvance)
        {
            return cashAdvance != null && cashAdvance.Status == ATPTEFMCashAdvanceStatusAttribute.ClosedValue;
        }
        private static decimal CalculateCADebitInvoiceAmount(PXGraph graph, ProjectBudgetParameters item, PXResultset<ATPTEFMCAReceiptDetail> receipts, bool isAdjusted)
        {
            decimal totalAmount = 0m;

            foreach (ATPTEFMCAReceiptDetail receipt in receipts)
            {
                EPExpenseClaimDetails expenseDetails = EPExpenseClaimDetails.PK.Find(graph, receipt.ExpenseReceiptRefNbr);
                if (HasValidAPDocumentInfo(expenseDetails))
                {
                    totalAmount += GetDebitInvoiceAmount(graph, item, expenseDetails, isAdjusted);
                }
            }

            return totalAmount;
        }
        private static decimal CalculateTotalCAReceiptAmount(PXResultset<ATPTEFMCAReceiptDetail> receipts)
        {
            decimal totalAmount = 0m;
            foreach (ATPTEFMCAReceiptDetail receipt in receipts)
            {
                totalAmount += receipt.CuryNetAmt ?? 0m;
            }
            return totalAmount;
        }
        private static decimal GetDebitInvoiceAmount(PXGraph graph, ProjectBudgetParameters item, EPExpenseClaimDetails er, bool isAdjusted)
        {
            return GetAPTransactionAmount(graph, item, er, isDebitAdjustment: true, requireReleased: isAdjusted);
        }
        private static decimal AddCARemainingBalanceToReturn(PXGraph graph, ATPTEFMProjectBudgetHistory result, ProjectBudgetParameters item, ATPTEFMCashAdvance ca, decimal totalRecAmt)
        {
            return HasClosedReclassificationOrRefund(graph, ca, item) ? ((result.Amount ?? 0m) - totalRecAmt) : 0m;
        }
        private static bool HasClosedReclassificationOrRefund(PXGraph graph, ATPTEFMCashAdvance ca, ProjectBudgetParameters item)
        {
            if (ca.Reclassified ?? false)
            {
                return SelectFrom<APInvoice>
                    .Where<APInvoice.docType.IsEqual<@P.AsString>
                        .And<APInvoice.refNbr.IsEqual<@P.AsString>>
                        .And<APInvoice.status.IsEqual<APDocStatus.closed>>
                        .And<APInvoice.finPeriodID.IsBetween<@P.AsString, @P.AsString>>>
                    .View.Select(graph, ca.ReclassifyType, ca.ReclassifiedInvoiceRefNbr, item.FromFinPeriodID, item.ToFinPeriodID).Any();
            }

            return SelectFrom<APPayment>
                .Where<APPayment.docType.IsEqual<@P.AsString>
                    .And<APPayment.refNbr.IsEqual<@P.AsString>>
                    .And<APPayment.status.IsEqual<APDocStatus.closed>>
                    .And<APPayment.finPeriodID.IsBetween<@P.AsString, @P.AsString>>>
                .View.Select(graph, ca.VendorRefundType, ca.VendorRefundRefNbr, item.FromFinPeriodID, item.ToFinPeriodID).Any();
        }
        private static ATPTEFMFundTransaction GetFundTransactionByRefNumber(PXGraph graph, string refNumber)
        {
            return SelectFrom<ATPTEFMFundTransaction>
                .Where<ATPTEFMFundTransaction.refNbr.IsEqual<@P.AsString>>
                .View.Select(graph, refNumber);
        }
        private static bool IsFundTransactionClosed(ATPTEFMFundTransaction fundTransaction)
        {
            return fundTransaction?.Status == ATPTEFMCashAdvanceStatusAttribute.ClosedValue;
        }
        private static decimal CalculateFTDebitInvoiceAmount(PXGraph graph, ProjectBudgetParameters item, PXResultset<ATPTEFMFundTransactionReceiptDetail> receipts, bool isAdjusted)
        {
            decimal totalAmount = 0m;

            foreach (ATPTEFMFundTransactionReceiptDetail receipt in receipts)
            {
                EPExpenseClaimDetails expenseDetails = EPExpenseClaimDetails.PK.Find(graph, receipt.ExpenseReceiptRefNbr);
                if (HasValidAPDocumentInfo(expenseDetails))
                {
                    totalAmount += GetDebitInvoiceAmount(graph, item, expenseDetails, isAdjusted);
                }
            }

            return totalAmount;
        }
        private static decimal CalculateTotalFTReceiptAmount(PXResultset<ATPTEFMFundTransactionReceiptDetail> receipts)
        {
            decimal totalAmount = 0m;
            foreach (ATPTEFMFundTransactionReceiptDetail result in receipts)
            {
                totalAmount += result.NetAmt ?? 0m;
            }
            return totalAmount;
        }
        private static decimal CalculateFTRemainingBalance(PXGraph graph, ATPTEFMProjectBudgetHistory result, ProjectBudgetParameters item, ATPTEFMFundTransaction fundTransaction, decimal totalReceiptAmount)
        {
            ATPTEFMFundTransactionReclassficationReceiptDetail reclassifiedReceipts = GetReclassifiedFTReceipts(graph, result, item);

            bool shouldAddDifference = (reclassifiedReceipts != null && fundTransaction.ReclassificationAmt > 0) ||
                                      fundTransaction.ReclassificationAmt == 0;

            return shouldAddDifference ? ((result.Amount ?? 0m) - totalReceiptAmount) : 0m;
        }
        private static decimal CalculateRFPDebitInvoiceAmount(PXGraph graph, ProjectBudgetParameters item, PXResultset<EPExpenseClaimDetails> receipts, bool isAdjusted)
        {
            decimal totalAmount = 0m;

            foreach (EPExpenseClaimDetails receipt in receipts)
            {
                if (HasValidAPDocumentInfo(receipt))
                {
                    totalAmount += GetDebitInvoiceAmount(graph, item, receipt, isAdjusted);
                }
            }

            return totalAmount;
        }
        private static decimal ProcessCAApprovedAdj(PXGraph graph, ATPTEFMProjectBudgetHistory result, ProjectBudgetParameters item)
        {
            decimal amt = 0m;

            var caReceipts = GetCAReceipts(graph, result);

            foreach (ATPTEFMCAReceiptDetail receipt in caReceipts)
            {
                EPExpenseClaimDetails er = EPExpenseClaimDetails.PK.Find(graph, receipt.ExpenseReceiptRefNbr);
                if (HasValidAPDocumentInfo(er))
                {
                    amt += GetReleasedNonDebitInvoiceAmount(graph, item, er);
                }
            }

            return amt;
        }
        private static decimal ProcessFTApprovedAdj(PXGraph graph, ATPTEFMProjectBudgetHistory result, ProjectBudgetParameters item)
        {
            decimal amt = 0m;

            var ftReceipts = GetFTReceipts(graph, result);

            foreach (ATPTEFMFundTransactionReceiptDetail receipt in ftReceipts)
            {
                EPExpenseClaimDetails er = EPExpenseClaimDetails.PK.Find(graph, receipt.ExpenseReceiptRefNbr);
                if (HasValidAPDocumentInfo(er))
                {
                    amt += GetReleasedNonDebitInvoiceAmount(graph, item, er);
                }
            }

            return amt;
        }
        private static decimal ProcessRFPApprovedAdj(PXGraph graph, ATPTEFMProjectBudgetHistory result, ProjectBudgetParameters item)
        {
            decimal amt = 0m;

            var rfpReceipts = GetRFPReceipts(graph, result);

            foreach (EPExpenseClaimDetails er in rfpReceipts)
            {
                if (HasValidAPDocumentInfo(er))
                {
                    amt += GetReleasedNonDebitInvoiceAmount(graph, item, er);
                }
            }

            return amt;
        }
        private static decimal ProcessCAReturn(PXGraph graph, ATPTEFMProjectBudgetHistory result, ProjectBudgetParameters item, bool isAdjusted)
        {
            ATPTEFMCashAdvance ca = GetCashAdvanceByRefNumber(graph, result.RefNbr);

            if (!IsCashAdvanceClosed(ca))
                return 0m;

            var receiptDetails = GetCAReceipts(graph, result);
            decimal debitInvoiceAmount = CalculateCADebitInvoiceAmount(graph, item, receiptDetails, isAdjusted);
            decimal totalReceiptAmount = CalculateTotalCAReceiptAmount(receiptDetails);
            decimal remainingBalance = isAdjusted ? 0 : AddCARemainingBalanceToReturn(graph, result, item, ca, totalReceiptAmount);

            return debitInvoiceAmount + remainingBalance;
        }
        private static decimal ProcessFTReturn(PXGraph graph, ATPTEFMProjectBudgetHistory result, ProjectBudgetParameters item, bool isAdjusted)
        {
            ATPTEFMFundTransaction fundTransaction = GetFundTransactionByRefNumber(graph, result.RefNbr);

            if (!IsFundTransactionClosed(fundTransaction))
                return 0m;

            var receipts = GetFTReceipts(graph, result);
            decimal debitInvoiceAmount = CalculateFTDebitInvoiceAmount(graph, item, receipts, isAdjusted);
            decimal totalReceiptAmount = CalculateTotalFTReceiptAmount(receipts);
            decimal remainingBalance = isAdjusted ? 0m : CalculateFTRemainingBalance(graph, result, item, fundTransaction, totalReceiptAmount);

            return debitInvoiceAmount + remainingBalance;
        }
        private static decimal ProcessRFPReturn(PXGraph graph, ATPTEFMProjectBudgetHistory result, ProjectBudgetParameters item, bool isAdjusted)
        {
            return CalculateRFPDebitInvoiceAmount(graph, item, GetRFPReceipts(graph, result), isAdjusted);
        }
    }
}