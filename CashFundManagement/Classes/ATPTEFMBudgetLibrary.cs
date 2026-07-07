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
using PX.Objects.IN;
using PX.Objects.IN.GraphExtensions.INIssueEntryExt;
using PX.Objects.RQ;
using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using static PX.Objects.IN.INTranType;
namespace CashFundManagement.Classes {
    public static class ATPTEFMBudgetLibrary
    {
        /// <summary>
        /// Budget Parameters/Arguments for BQL Views
        /// </summary>
        /// <remarks>
        /// 2025-08-04 : Make method for getting receipts for efficient updates on query : RFS
        /// 2025-09-01 : HasClosedReclassificationOrRefund method to use .Any for clarity : RFS
        /// </remarks>
        public class BudgetParameters
        {
            public int? LedgerID { get; set; }
            public int? BranchID { get; set; }
            public string RefNbr { get; set; }
            public string CuryID { get; set; }
            public int? AccountID { get; set; }
            public int? SubID { get; set; }
            public string FinYear { get; set; }
            public string FromFinPeriodID { get; set; }
            public string ToFinPeriodID { get; set; }
            public string FinPeriodID { get; set; }
            public decimal? Amount { get; set; }
            public bool Approved { get; set; }
            public bool Released { get; set; }
            public OriginTypes OriginType { get; set; }
        }

        public class FinPeriodData
        {
            public string FinYear { get; set; }
            public DateTime? StartDate { get; set; }
            public DateTime? EndDate { get; set; }
            public string StartPeriod { get; set; }
            public string EndPeriod { get; set; }
        }

        public enum OriginTypes
        {
            CashAdvance = 0,
            Bills = 1,
            FundTransaction = 2,
            Requests = 3,
            RequestForPayment = 4
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
            public const string InitialBudgetMinusTotalApprovedPlusReturns = "F3";
            public const string InitialBudgetMinusApprovedSpentUnapprovedPlusReturns = "F4";
            public const string NotApplicable = "NA";
        }
        public class SpentAmountCalculation
        {
            public const string TotalJournalTransactionSpentAmount = "F1";
            public const string NotApplicable = "NA";
        }
        public static bool HasNull(params object[] values) => values.ToList().Contains(null);

        /// <remarks>
        /// 2025-06-02 : Undo Fiscal scenario cosnideration, Focus on using docdate as budget range : RFS
        /// </remarks>
        public static FinPeriodData GetFinPeriod(PXGraph graph, string finPeriodID, string calculationType)
        {
            MasterFinPeriod finPeriod = PXSetup<MasterFinPeriod>
                .Where<Where<MasterFinPeriod.finPeriodID, Equal<Required<MasterFinPeriod.finPeriodID>>>>
                .Select(graph, finPeriodID);
            if (HasNull(finPeriod, calculationType)) return null;
            FinPeriodData fData = new FinPeriodData() { FinYear = finPeriod.FinYear, StartPeriod = string.Empty };
            DateTime cDate = graph.Accessinfo.BusinessDate.Value;

            switch (calculationType)
            {
                case RQBudgetCalculationType.YTD:
                    fData.StartPeriod = $"{finPeriod.FinYear}01";
                    fData.EndPeriod = finPeriod.FinPeriodID;
                    break;
                case RQBudgetCalculationType.PTD:
                    fData.StartPeriod = finPeriod.FinPeriodID;
                    fData.EndPeriod = finPeriod.FinPeriodID;
                    break;
                case RQBudgetCalculationType.Annual:
                    fData.StartPeriod = $"{finPeriod.FinYear}01";
                    fData.EndPeriod = $"{finPeriod.FinYear}12";
                    break;
            }

            return fData;
        }

        public static decimal GetBudgetAmt(PXGraph graph, BudgetParameters item)
        {
            GLBudgetLineDetail obj =
                PXSelectGroupBy<GLBudgetLineDetail,
                Where<GLBudgetLineDetail.ledgerID, Equal<Required<GLBudgetLineDetail.ledgerID>>,
                    And<GLBudgetLineDetail.finYear, Equal<Required<GLBudgetLineDetail.finYear>>,
                    And<GLBudgetLineDetail.accountID, Equal<Required<GLBudgetLineDetail.accountID>>,
                    And<GLBudgetLineDetail.subID, Equal<Required<GLBudgetLineDetail.subID>>,
                    And<GLBudgetLineDetail.finPeriodID, Between<Required<GLBudgetLineDetail.finPeriodID>, Required<GLBudgetLineDetail.finPeriodID>>>>>>>,
                Aggregate<
                    Sum<GLBudgetLineDetail.amount,
                    Sum<GLBudgetLineDetail.releasedAmount>>>>
                .SelectWindowed(graph, 0, 1, item.LedgerID, item.FinYear, item.AccountID, item.SubID, item.FromFinPeriodID, item.ToFinPeriodID);

            return obj == null ? 0 : obj.ReleasedAmount ?? 0;
        }

        public static decimal GetSpentAmt(PXGraph graph, BudgetParameters item)
        {
            var obj =
                PXSelect<GLTran,
                Where<GLTran.accountID, Equal<Required<GLTran.accountID>>,
                    And<GLTran.subID, Equal<Required<GLTran.subID>>,
                    And<GLTran.finPeriodID, Between<Required<GLTran.finPeriodID>, Required<GLTran.finPeriodID>>>>>>
                .Select(graph, item.AccountID, item.SubID, item.FromFinPeriodID, item.ToFinPeriodID).FirstTableItems.ToList();

            return obj == null ? 0 : obj.Sum(x => x.DebitAmt) - obj.Sum(x => x.CreditAmt) ?? 0;
        }
        public static decimal GetApprovedAmt(PXGraph graph, BudgetParameters item, ATPTEFMFeatures features, PXResultset<ATPTEFMBudgetHistory> baseHistory)
        {
            decimal? budgetTotal = 0M;

            IEnumerable<ATPTEFMBudgetHistory> history = GetFilteredApprovedOrUnapprovedHistory(features.BudgetApprovedAmount, baseHistory, item, true);
            budgetTotal += history.Sum(h => h.BudgetAmt ?? 0);

            return budgetTotal ?? 0;
        }
        public static decimal GetApprovedAdjAmt(PXGraph graph, BudgetParameters item, ATPTEFMFeatures features, PXResultset<ATPTEFMBudgetHistory> baseHistory, bool isCurrentDoc = false)
        {
            decimal? budgetTotal = 0M;

            IEnumerable<ATPTEFMBudgetHistory> history = GetFilteredApprovedOrUnapprovedHistory(features.BudgetApprovedAmount, baseHistory, item, true, isCurrentDoc);

            foreach (ATPTEFMBudgetHistory result in history)
            {
                if (result.Origin == (int)OriginTypes.CashAdvance)
                {
                    budgetTotal += ProcessCAApprovedAdj(graph, result, item);
                }
                else if (result.Origin == (int)OriginTypes.FundTransaction)
                {
                    budgetTotal += ProcessFTApprovedAdj(graph, result, item);
                }
                else if (result.Origin == (int)OriginTypes.RequestForPayment)
                {
                    budgetTotal += ProcessRFPApprovedAdj(graph, result, item);
                }
            }

            return budgetTotal ?? 0;
        }
        public static decimal GetUnapprovedAmt(PXGraph graph, BudgetParameters item, ATPTEFMFeatures features, PXResultset<ATPTEFMBudgetHistory> baseHistory)
        {
            decimal? budgetTotal = 0M;

            IEnumerable<ATPTEFMBudgetHistory> history = GetFilteredApprovedOrUnapprovedHistory(features.BudgetUnapprovedAmount, baseHistory, item, false);
            budgetTotal += history.Sum(h => h.BudgetAmt ?? 0);

            return budgetTotal ?? 0;
        }
        public static decimal GetReturnAmounts(PXGraph graph, BudgetParameters item, ATPTEFMFeatures features, PXResultset<ATPTEFMBudgetHistory> baseHistory, bool isAdjusted, bool isCurrentDoc = false)
        {
            decimal? budgetTotal = 0M;

            IEnumerable<ATPTEFMBudgetHistory> history = null;

            if (features.BudgetReturnAmount == ReturnAmountCalculation.TotalReturnedAmount)
            {
                history = GetFilteredApprovedOrUnapprovedHistory(string.Empty, baseHistory, item, true, isCurrentDoc);

                foreach (ATPTEFMBudgetHistory result in history)
                {
                    if (result.Origin == (int)OriginTypes.CashAdvance)
                    {
                        budgetTotal += ProcessCAReturn(graph, result, item, isAdjusted);
                    }
                    else if (result.Origin == (int)OriginTypes.FundTransaction)
                    {
                        budgetTotal += ProcessFTReturn(graph, result, item, isAdjusted);
                    }
                    else if (result.Origin == (int)OriginTypes.RequestForPayment)
                    {
                        budgetTotal += ProcessRFPReturn(graph, result, item, isAdjusted);
                    }
                }
            }

            return budgetTotal ?? 0;
        }
        public static decimal? ModifyBudgetAmt(PXGraph graph, DAC.Unbound.ATPTEFMBudget item)
        {
            var _BudgetAmt = item.BudgetAmt;
            _BudgetAmt = item.BudgetAmt - item.DocAmt;
            return _BudgetAmt;
        }

        public static IEnumerable<DAC.Unbound.ATPTEFMBudget> GenerateBudget(PXGraph graph, List<BudgetParameters> items)
        {
            DAC.Setup.ATPTEFMFeatures Features = PXSetup<DAC.Setup.ATPTEFMFeatures>.Select(graph);

            var budgetView = new List<DAC.Unbound.ATPTEFMBudget>();
            foreach (BudgetParameters item in items)
            {
                PXResultset<ATPTEFMBudgetHistory> history = PXSelectJoin<DAC.ATPTEFMBudgetHistory,
                        InnerJoin<MasterFinPeriod, On<MasterFinPeriod.finPeriodID, Equal<DAC.ATPTEFMBudgetHistory.finPeriodID>>>,
                    Where<DAC.ATPTEFMBudgetHistory.acctID, Equal<Required<DAC.ATPTEFMBudgetHistory.acctID>>,
                        And<DAC.ATPTEFMBudgetHistory.subID, Equal<Required<DAC.ATPTEFMBudgetHistory.subID>>,
                        And<MasterFinPeriod.finYear, Equal<Required<MasterFinPeriod.finYear>>,
                        And<MasterFinPeriod.finPeriodID, Between<Required<MasterFinPeriod.finPeriodID>, Required<MasterFinPeriod.finPeriodID>>>>>>>
                    .Select(graph, item.AccountID, item.SubID, item.FinYear, item.FromFinPeriodID, item.ToFinPeriodID);

                var budget = new DAC.Unbound.ATPTEFMBudget();
                budget.AcctID = item.AccountID;
                budget.SubID = item.SubID;
                budget.CuryID = item.CuryID;
                budget.InitialBudget = GetBudgetAmt(graph, item);
                budget.DocAmt = Features.BudgetDocumentAmount != DocAmountCalculation.NotApplicable ? item.Amount : 0;
                budget.RequestAmt = Features.BudgetRequestAmount != RequestAmountCalculation.NotApplicable ? 0 : 0;
                budget.BudgetAmt = Features.BudgetBudgetAmount != BudgetAmountCalculation.NotApplicable ? GetBudgetAmt(graph, item) : 0;
                budget.SpentAmt = Features.BudgetSpentAmount != SpentAmountCalculation.NotApplicable ? GetSpentAmt(graph, item) : 0;
                budget.ApprovedAmt = Features.BudgetApprovedAmount != ApproveAmountCalculation.NotApplicable ? GetApprovedAmt(graph, item, Features, history) : 0;
                budget.ApprovedAdjAmt = Features.BudgetBudgetAmount == BudgetAmountCalculation.InitialBudgetMinusApprovedSpentUnapprovedPlusReturns ? GetApprovedAdjAmt(graph, item, Features, history) : 0;
                budget.CurrentApprovedAdjAmt = Features.BudgetBudgetAmount == BudgetAmountCalculation.InitialBudgetMinusApprovedSpentUnapprovedPlusReturns ? GetApprovedAdjAmt(graph, item, Features, history, true) : 0;
                budget.UnapprovedAmt = Features.BudgetUnapprovedAmount != UnapproveAmountCalculation.NotApplicable ? GetUnapprovedAmt(graph, item, Features, history) : 0;
                budget.ReturnAmt = GetReturnAmounts(graph, item, Features, history, false);
                budget.CurrentReturnAmt = GetReturnAmounts(graph, item, Features, history, false, true);
                budget.ReturnAmtAdj = Features.BudgetBudgetAmount == BudgetAmountCalculation.InitialBudgetMinusApprovedSpentUnapprovedPlusReturns ? GetReturnAmounts(graph, item, Features, history, true) : 0;
                budget.CurrentReturnAmtAdj = Features.BudgetBudgetAmount == BudgetAmountCalculation.InitialBudgetMinusApprovedSpentUnapprovedPlusReturns ? GetReturnAmounts(graph, item, Features, history, true, true) : 0;
                budget.FinPeriodID = item.FinPeriodID;
                budget.RefNbr = item.RefNbr;
                budget.Origin = (int)item.OriginType;
                budget.IsApproved = item.Approved;
                budgetView.Add(budget);
            }

            return budgetView
                .GroupBy(x => new
                {
                    x.AcctID,
                    x.SubID,
                    x.CuryID,
                    x.InitialBudget,
                    x.RequestAmt,
                    x.BudgetAmt,
                    x.SpentAmt,
                    x.ApprovedAmt,
                    x.ApprovedAdjAmt,
                    x.CurrentApprovedAdjAmt,
                    x.ReturnAmt,
                    x.CurrentReturnAmt,
                    x.ReturnAmtAdj,
                    x.CurrentReturnAmtAdj,
                    x.UnapprovedAmt,
                    x.FinPeriodID,
                    x.RefNbr,
                    x.Origin,
                    x.IsApproved
                })
                .Select(x => new DAC.Unbound.ATPTEFMBudget
                {
                    AcctID = x.Key.AcctID,
                    SubID = x.Key.SubID,
                    CuryID = x.Key.CuryID,
                    InitialBudget = x.Key.InitialBudget,
                    DocAmt = ComputeDocAmt(Features.BudgetDocumentAmount, x.Sum(y => y.DocAmt)),
                    RequestAmt = ComputeRequestAmt(Features.BudgetRequestAmount, x.Sum(y => y.DocAmt), x.Key.ApprovedAmt, x.Key.UnapprovedAmt),
                    BudgetAmt = ComputeBudgetAmt(Features.BudgetBudgetAmount, x.Key.BudgetAmt, x.Sum(y => y.DocAmt), x.Key.SpentAmt, x.Key.ApprovedAmt, x.Key.UnapprovedAmt, x.Key.ApprovedAdjAmt, x.Key.CurrentApprovedAdjAmt, x.Key.ReturnAmt, x.Key.CurrentReturnAmt, x.Key.ReturnAmtAdj, x.Key.CurrentReturnAmtAdj, x.Key.IsApproved),
                    SpentAmt = ComputeSpentAmt(Features.BudgetSpentAmount, x.Key.SpentAmt),
                    ApprovedAmt = ComputeApprovedAmt(x.Key.ApprovedAmt, x.Sum(y => y.DocAmt), x.Key.IsApproved),
                    UnapprovedAmt = ComputeUnapprovedAmt(x.Key.UnapprovedAmt, x.Sum(y => y.DocAmt), x.Key.IsApproved),
                    ReturnAmt = ComputeReturnAmt(Features.BudgetReturnAmount, x.Key.ReturnAmt, x.Key.CurrentReturnAmt),
                    //DocAmt = ComputeAmounts(BudgetAmountColumns.DocAmt, Features.BudgetDocumentAmount, x.Key.IsApproved, x.Key.BudgetAmt, x.Key.SpentAmt, x.Key.ApprovedAmt, x.Key.UnapprovedAmt, x.Sum(y => y.DocAmt), x.Sum(y => y.RequestAmt), x.Key.ApprovedAdjAmt, x.Key.CurrentApprovedAdjAmt, x.Key.ReturnAmt, x.Key.ReturnAmtAdj, x.Key.CurrentReturnAmt, x.Key.CurrentReturnAmtAdj),
                    //RequestAmt = ComputeAmounts(BudgetAmountColumns.RequestAmt, Features.BudgetRequestAmount, x.Key.IsApproved, x.Key.BudgetAmt, x.Key.SpentAmt, x.Key.ApprovedAmt, x.Key.UnapprovedAmt, x.Sum(y => y.DocAmt), x.Sum(y => y.RequestAmt), x.Key.ApprovedAdjAmt, x.Key.CurrentApprovedAdjAmt, x.Key.ReturnAmt, x.Key.ReturnAmtAdj, x.Key.CurrentReturnAmt, x.Key.CurrentReturnAmtAdj),
                    //BudgetAmt = ComputeAmounts(BudgetAmountColumns.BudgetAmt, Features.BudgetBudgetAmount, x.Key.IsApproved, x.Key.BudgetAmt, x.Key.SpentAmt, x.Key.ApprovedAmt, x.Key.UnapprovedAmt, x.Sum(y => y.DocAmt), x.Sum(y => y.RequestAmt), x.Key.ApprovedAdjAmt, x.Key.CurrentApprovedAdjAmt, x.Key.ReturnAmt, x.Key.ReturnAmtAdj, x.Key.CurrentReturnAmt, x.Key.CurrentReturnAmtAdj),
                    //SpentAmt = ComputeAmounts(BudgetAmountColumns.SpentAmt, Features.BudgetSpentAmount, x.Key.IsApproved, x.Key.BudgetAmt, x.Key.SpentAmt, x.Key.ApprovedAmt, x.Key.UnapprovedAmt, x.Sum(y => y.DocAmt), x.Sum(y => y.RequestAmt), x.Key.ApprovedAdjAmt, x.Key.CurrentApprovedAdjAmt, x.Key.ReturnAmt, x.Key.ReturnAmtAdj, x.Key.CurrentReturnAmt, x.Key.CurrentReturnAmtAdj),
                    //ApprovedAmt = ComputeAmounts(BudgetAmountColumns.ApprovedAmt, Features.BudgetApprovedAmount, x.Key.IsApproved, x.Key.BudgetAmt, x.Key.SpentAmt, x.Key.ApprovedAmt, x.Key.UnapprovedAmt, x.Sum(y => y.DocAmt), x.Sum(y => y.RequestAmt), x.Key.ApprovedAdjAmt, x.Key.CurrentApprovedAdjAmt, x.Key.ReturnAmt, x.Key.ReturnAmtAdj, x.Key.CurrentReturnAmt, x.Key.CurrentReturnAmtAdj),
                    //UnapprovedAmt = ComputeAmounts(BudgetAmountColumns.UnapprovedAmt, Features.BudgetUnapprovedAmount, x.Key.IsApproved, x.Key.BudgetAmt, x.Key.SpentAmt, x.Key.ApprovedAmt, x.Key.UnapprovedAmt, x.Sum(y => y.DocAmt), x.Sum(y => y.RequestAmt), x.Key.ApprovedAdjAmt, x.Key.CurrentApprovedAdjAmt, x.Key.ReturnAmt, x.Key.ReturnAmtAdj, x.Key.CurrentReturnAmt, x.Key.CurrentReturnAmtAdj),
                    //ReturnAmt = ComputeAmounts(BudgetAmountColumns.ReturnAmt, Features.BudgetReturnAmount, x.Key.IsApproved, x.Key.BudgetAmt, x.Key.SpentAmt, x.Key.ApprovedAmt, x.Key.UnapprovedAmt, x.Sum(y => y.DocAmt), x.Sum(y => y.RequestAmt), x.Key.ApprovedAdjAmt, x.Key.CurrentApprovedAdjAmt, x.Key.ReturnAmt, x.Key.ReturnAmtAdj, x.Key.CurrentReturnAmt, x.Key.CurrentReturnAmtAdj),
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
                case BudgetAmountCalculation.InitialBudgetMinusTotalApprovedPlusReturns: retValue = budgetAmt - (approvedAmt + (isApproved == true ? docAmt : 0)) + (returnAmt + currentReturnAmt); break;
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
        //                case "F2": retValue = (budgetAmt - ((approvedAmt + (isApproved == true ? docAmt : 0)) + (unapprovedAmt + (isApproved == false ? docAmt : 0))) + (returnAmt + currentReturnAmt)); break;
        //                case "F3": retValue = budgetAmt - (approvedAmt + (isApproved == true ? docAmt : 0)) + (returnAmt + currentReturnAmt); break;
        //                case "F4": retValue = budgetAmt - (((approvedAmt + (isApproved == true ? (docAmt - currentApproveAdj) : docAmt)) - approveAdj) + spentAmt + unapprovedAmt) + ((returnAmt + currentReturnAmt) - (returnAmtAdj + currentReturnAmtAdj)); break;
        //                default: return 0;
        //            }
        //            PXTrace.WriteInformation($"ComputeAmounts Parameters: InitialBudget={budgetAmt}, appAdj={((approvedAmt + (isApproved == true ? (docAmt - currentApproveAdj) : docAmt)) - approveAdj)}, retAdj={((returnAmt + currentReturnAmt) - (returnAmtAdj + currentReturnAmtAdj))}, spentAmt={spentAmt}, approvedAmt={approvedAmt}, unapprovedAmt={unapprovedAmt}, docAmt={docAmt}, requestAmt={requestAmt}, approveAdj={approveAdj}, currentApproveAdj={currentApproveAdj}, returnAmt={returnAmt}, returnAmtAdj={returnAmtAdj}, currentReturnAmt={currentReturnAmt}, currentReturnAmtAdj={currentReturnAmtAdj}");
        //            PXTrace.WriteInformation($"1st Calc={(budgetAmt - ((approvedAmt + (isApproved == true ? docAmt : 0)) + (unapprovedAmt + (isApproved == false ? docAmt : 0))) + (returnAmt + currentReturnAmt))}");
        //            return retValue;
        //        case "SpentAmt":
        //            switch (feature)
        //            {
        //                case "F1": retValue = spentAmt; break;
        //                default: retValue = 0; break;
        //            }
        //            return retValue;
        //        case "ReturnAmt":
        //            switch (feature)
        //            {
        //                case "F1": retValue = returnAmt + currentReturnAmt; break;
        //                default: retValue = 0; break;
        //            }
        //            return retValue;
        //        case "ApprovedAmt":
        //            return approvedAmt + (isApproved == true ? docAmt : 0);
        //        case "UnapprovedAmt":
        //            return unapprovedAmt + (isApproved == false ? docAmt : 0);
        //    }
        //    return retValue;
        //}

        public static void BudgetVisibility(PXCache cache, DAC.Setup.ATPTEFMFeatures featureSetup, string module)
        {
            PXUIFieldAttribute.SetVisible<DAC.Unbound.ATPTEFMBudget.docAmt>(cache, null, featureSetup.BudgetDocumentAmount != DocAmountCalculation.NotApplicable ? true : false);
            PXUIFieldAttribute.SetVisible<DAC.Unbound.ATPTEFMBudget.requestAmt>(cache, null, featureSetup.BudgetRequestAmount != RequestAmountCalculation.NotApplicable ? true : false);
            PXUIFieldAttribute.SetVisible<DAC.Unbound.ATPTEFMBudget.budgetAmt>(cache, null, featureSetup.BudgetBudgetAmount != BudgetAmountCalculation.NotApplicable ? true : false);
            PXUIFieldAttribute.SetVisible<DAC.Unbound.ATPTEFMBudget.spentAmt>(cache, null, featureSetup.BudgetSpentAmount != SpentAmountCalculation.NotApplicable ? true : false);
            PXUIFieldAttribute.SetVisible<DAC.Unbound.ATPTEFMBudget.approvedAmt>(cache, null, featureSetup.BudgetApprovedAmount != ApproveAmountCalculation.NotApplicable ? true : false);
            PXUIFieldAttribute.SetVisible<DAC.Unbound.ATPTEFMBudget.unapprovedAmt>(cache, null, featureSetup.BudgetUnapprovedAmount != UnapproveAmountCalculation.NotApplicable ? true : false);
            PXUIFieldAttribute.SetVisible<DAC.Unbound.ATPTEFMBudget.returnAmt>(cache, null, featureSetup.BudgetReturnAmount != ReturnAmountCalculation.NotApplicable ? true : false);

            if (featureSetup.BudgetDocumentAmountLabel != null)
                PXUIFieldAttribute.SetDisplayName<DAC.Unbound.ATPTEFMBudget.docAmt>(cache, featureSetup.BudgetDocumentAmountLabel);
            if (featureSetup.BudgetRequestAmountLabel != null)
                PXUIFieldAttribute.SetDisplayName<DAC.Unbound.ATPTEFMBudget.requestAmt>(cache, featureSetup.BudgetRequestAmountLabel);
            if (featureSetup.BudgetBudgetAmountLabel != null)
                PXUIFieldAttribute.SetDisplayName<DAC.Unbound.ATPTEFMBudget.budgetAmt>(cache, featureSetup.BudgetBudgetAmountLabel);
            if (featureSetup.BudgetSpentAmountLabel != null)
                PXUIFieldAttribute.SetDisplayName<DAC.Unbound.ATPTEFMBudget.spentAmt>(cache, featureSetup.BudgetSpentAmountLabel);
            if (featureSetup.BudgetApprovedAmountLabel != null)
                PXUIFieldAttribute.SetDisplayName<DAC.Unbound.ATPTEFMBudget.approvedAmt>(cache, featureSetup.BudgetApprovedAmountLabel);
            if (featureSetup.BudgetUnapprovedAmountLabel != null)
                PXUIFieldAttribute.SetDisplayName<DAC.Unbound.ATPTEFMBudget.unapprovedAmt>(cache, featureSetup.BudgetUnapprovedAmountLabel);
            if (featureSetup.BudgetReturnAmountLabel != null)
                PXUIFieldAttribute.SetDisplayName<DAC.Unbound.ATPTEFMBudget.returnAmt>(cache, featureSetup.BudgetReturnAmountLabel);

            cache.AllowSelect = featureSetup?.BudgetModules?.Split(',').Contains(module) ?? false;
        }

        public static void AddBudgetHistory(PXSelect<DAC.ATPTEFMBudgetHistory> historyView, List<DAC.Unbound.ATPTEFMBudget> items)
        {
            foreach (DAC.Unbound.ATPTEFMBudget item in items)
            {
                //Scenarios where budget document amt should be 0 example; Cash Advance Return Excess || RFP Bill Reversing
                //if ((item.DocAmt ?? 0m) <= 0m) continue;
                if (HasNull(item.AcctID, item.SubID)) continue;

                DAC.ATPTEFMBudgetHistory history = historyView.Select()
                    .Where(x => x.GetItem<DAC.ATPTEFMBudgetHistory>().AcctID == item.AcctID
                             && x.GetItem<DAC.ATPTEFMBudgetHistory>().SubID == item.SubID
                             && x.GetItem<DAC.ATPTEFMBudgetHistory>().FinPeriodID == item.FinPeriodID
                             && x.GetItem<DAC.ATPTEFMBudgetHistory>().Origin == item.Origin
                             && x.GetItem<DAC.ATPTEFMBudgetHistory>().RefNbr == item.RefNbr)
                    .SingleOrDefault();

                if (history == null)
                {
                    history = new DAC.ATPTEFMBudgetHistory()
                    {
                        AcctID = item.AcctID,
                        SubID = item.SubID,
                        FinPeriodID = item.FinPeriodID,
                        Origin = item.Origin,
                        RefNbr = item.RefNbr
                    };
                }

                history.CuryID = item.CuryID;
                history.BudgetAmt = item.DocAmt;
                history.CuryAmt = item.DocAmt;

                history.IsApproved = item.IsApproved ?? false;
                history.IsReleased = item.IsReleased ?? false;

                history = historyView.Update(history);
                historyView.Cache.Persist(PXDBOperation.Update);
            }
        }

        public static void DeleteBudgetHistory(PXSelect<DAC.ATPTEFMBudgetHistory> historyView, List<DAC.Unbound.ATPTEFMBudget> items, bool deleteAll = false)
        {
            if (!deleteAll)
            {
                //Delete based on Account ID, Sub Account ID, Origin, and Reference Number
                foreach (DAC.Unbound.ATPTEFMBudget item in items)
                {
                    if ((item.DocAmt ?? 0m) <= 0m) continue;

                    //Get record with the required parameters
                    DAC.ATPTEFMBudgetHistory history = historyView.Select()
                        .Where(x => x.GetItem<DAC.ATPTEFMBudgetHistory>().AcctID == item.AcctID
                                 && x.GetItem<DAC.ATPTEFMBudgetHistory>().SubID == item.SubID
                                 //&& x.GetItem<DAC.ATPTEFMBudgetHistory>().FinPeriodID == item.FinPeriodID
                                 && x.GetItem<DAC.ATPTEFMBudgetHistory>().Origin == item.Origin
                                 && x.GetItem<DAC.ATPTEFMBudgetHistory>().RefNbr == item.RefNbr)
                        .SingleOrDefault();

                    //if record exists delete
                    if (history != null)
                    {
                        history = historyView.Delete(history);
                        historyView.Cache.Persist(PXDBOperation.Delete);
                    }
                }
            }
            else
            {
                //Delete All based on Origin Type and Reference Number
                if (items.First() == null) return;

                //Get first collection
                DAC.Unbound.ATPTEFMBudget item = items.First();

                //Get all records with the same Origin and RefNbr
                var historyCollection = historyView.Select()
                    .Where(x => x.GetItem<DAC.ATPTEFMBudgetHistory>().Origin == item.Origin
                             && x.GetItem<DAC.ATPTEFMBudgetHistory>().RefNbr == item.RefNbr)
                    .ToList();

                //Iterate each record and delete
                foreach (DAC.ATPTEFMBudgetHistory rows in historyCollection)
                {
                    historyView.Delete(rows);
                    historyView.Cache.Persist(PXDBOperation.Delete);
                }
            }
        }

        public static bool BudgetVisible(DAC.Setup.ATPTEFMFeatures featureSetup, string module)
            => featureSetup?.BudgetModules?.Split(',').Contains(module) ?? false;

        private static PXResultset<ATPTEFMCAReceiptDetail> GetCAReceipts(PXGraph graph, ATPTEFMBudgetHistory history)
        {
            return PXSelect<ATPTEFMCAReceiptDetail,
                            Where<ATPTEFMCAReceiptDetail.cashAdvanceNbr, Equal<@P.AsString>,
                            And<ATPTEFMCAReceiptDetail.accountID, Equal<@P.AsInt>,
                            And<ATPTEFMCAReceiptDetail.subID, Equal<@P.AsInt>,
                            And<ATPTEFMCAReceiptDetail.reversed, Equal<False>>>>>>.Select(graph, history.RefNbr, history.AcctID, history.SubID);
        }
        private static PXResultset<ATPTEFMFundTransactionReceiptDetail> GetFTReceipts(PXGraph graph, ATPTEFMBudgetHistory history)
        {
            return PXSelect<ATPTEFMFundTransactionReceiptDetail,
                           Where<ATPTEFMFundTransactionReceiptDetail.fundTransactionRefNbr, Equal<@P.AsString>,
                           And<ATPTEFMFundTransactionReceiptDetail.accountID, Equal<@P.AsInt>,
                           And<ATPTEFMFundTransactionReceiptDetail.subID, Equal<@P.AsInt>>>>>.Select(graph, history.RefNbr, history.AcctID, history.SubID);
        }
        //Get reclassified FT receipts without checking for acct and sub, Reclassification receipts use different acct and sub
        private static ATPTEFMFundTransactionReclassficationReceiptDetail GetReclassifiedFTReceipts(PXGraph graph, ATPTEFMBudgetHistory history, BudgetParameters item)
        {
            return PXSelectJoin<ATPTEFMFundTransactionReclassficationReceiptDetail, 
                InnerJoin<EPExpenseClaimDetails, 
                On<ATPTEFMFundTransactionReclassficationReceiptDetail.expenseReceiptRefNbr, Equal<EPExpenseClaimDetails.claimDetailCD>>,
                InnerJoin<MasterFinPeriod, On<EPExpenseClaimDetails.expenseDate, Between<MasterFinPeriod.startDate, MasterFinPeriod.endDate>>>>,
                Where<ATPTEFMFundTransactionReclassficationReceiptDetail.fundTransactionRefNbr, Equal<@P.AsString>,
                And<MasterFinPeriod.finPeriodID, Between<@P.AsString, @P.AsString>>>>
                .Select(graph, history.RefNbr, item.FromFinPeriodID, item.ToFinPeriodID);
        }
        private static PXResultset<EPExpenseClaimDetails> GetRFPReceipts(PXGraph graph, ATPTEFMBudgetHistory history)
        {
            return PXSelect<EPExpenseClaimDetails,
                        Where<EPExpenseClaimDetails.refNbr, Equal<@P.AsString>,
                        And<ATPTEFMEPExpenseClaimDetailsExt.usrATPTEFMTranType, Equal<@P.AsString>,
                        And<EPExpenseClaimDetails.expenseAccountID, Equal<@P.AsInt>,
                        And<EPExpenseClaimDetails.expenseSubID, Equal<@P.AsInt>>>>>>.Select(graph, history.RefNbr, ATPTEFMExpenseTypeAttribute.RequestforPayment, history.AcctID, history.SubID);
        }
        private static IEnumerable<ATPTEFMBudgetHistory> GetFilteredApprovedOrUnapprovedHistory(string calcSetup, PXResultset<ATPTEFMBudgetHistory> baseHistory, BudgetParameters item, bool isApproved, bool isCurrentDoc = false)
        {
            IEnumerable<ATPTEFMBudgetHistory> history = null;

            if (isCurrentDoc)
            {
                if (calcSetup == ApproveAmountCalculation.CurrentModule)
                {
                    history = baseHistory
                        .Select(r => r.GetItem<ATPTEFMBudgetHistory>())
                        .Where(h => h.IsApproved == isApproved && h.RefNbr == item.RefNbr && h.Origin == (int)item.OriginType);
                }
                else
                {
                    history = baseHistory
                        .Select(r => r.GetItem<ATPTEFMBudgetHistory>())
                        .Where(h => h.IsApproved == isApproved && h.RefNbr == item.RefNbr);
                }
            }
            else
            {
                if (calcSetup == ApproveAmountCalculation.CurrentModule)
                {
                    history = baseHistory
                        .Select(r => r.GetItem<ATPTEFMBudgetHistory>())
                        .Where(h => h.IsApproved == isApproved && h.RefNbr != item.RefNbr && h.Origin == (int)item.OriginType);
                }
                else
                {
                    history = baseHistory
                        .Select(r => r.GetItem<ATPTEFMBudgetHistory>())
                        .Where(h => h.IsApproved == isApproved && h.RefNbr != item.RefNbr);
                }
            }

            return history;
        }
        private static decimal ProcessCAApprovedAdj(PXGraph graph, ATPTEFMBudgetHistory result, BudgetParameters item)
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
        private static decimal ProcessFTApprovedAdj(PXGraph graph, ATPTEFMBudgetHistory result, BudgetParameters item)
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
        private static decimal ProcessRFPApprovedAdj(PXGraph graph, ATPTEFMBudgetHistory result, BudgetParameters item)
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
        private static decimal GetAPTransactionAmount(PXGraph graph, BudgetParameters item, EPExpenseClaimDetails er, bool isDebitAdjustment, bool requireReleased = false)
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
        private static decimal GetReleasedNonDebitInvoiceAmount(PXGraph graph, BudgetParameters item, EPExpenseClaimDetails er)
        {
            return GetAPTransactionAmount(graph, item, er, isDebitAdjustment: false, requireReleased: true);
            //return PXSelectJoin<
            //            APTran,
            //            InnerJoin<APInvoice,
            //            On<APInvoice.refNbr, Equal<APTran.refNbr>,
            //            And<APInvoice.docType, Equal<APTran.tranType>>>>,
            //            Where<APTran.refNbr, Equal<@P.AsString>,
            //                And<APTran.lineNbr, Equal<@P.AsInt>,
            //                And<APTran.tranType, Equal<@P.AsString>,
            //                And<APInvoice.released, Equal<True>,
            //                And<APInvoice.finPeriodID, Between<@P.AsString, @P.AsString>>>>>>>
            //            .Select(graph, er.APRefNbr, er.APLineNbr, er.APDocType, item.FromFinPeriodID, item.ToFinPeriodID)
            //            .RowCast<APTran>()
            //            .Sum(t => (t.CuryTranAmt ?? 0m) + (t.CuryRetainageAmt ?? 0m));
        }
        private static decimal ProcessCAReturn(PXGraph graph, ATPTEFMBudgetHistory result, BudgetParameters item, bool isAdjusted)
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
        private static decimal CalculateCADebitInvoiceAmount(PXGraph graph, BudgetParameters item, PXResultset<ATPTEFMCAReceiptDetail> receipts, bool isAdjusted)
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
        private static bool HasValidAPDocumentInfo(EPExpenseClaimDetails expenseDetails)
        {
            return expenseDetails != null
                && expenseDetails.APDocType != null
                && expenseDetails.APRefNbr != null
                && expenseDetails.APLineNbr != null;
        }
        private static decimal AddCARemainingBalanceToReturn(PXGraph graph, ATPTEFMBudgetHistory result, BudgetParameters item, ATPTEFMCashAdvance ca, decimal totalRecAmt)
        {
            return HasClosedReclassificationOrRefund(graph, ca, item) ? ((result.BudgetAmt ?? 0m) - totalRecAmt) : 0m;
        }
        private static bool HasClosedReclassificationOrRefund(PXGraph graph, ATPTEFMCashAdvance ca, BudgetParameters item)
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
        private static decimal ProcessFTReturn(PXGraph graph, ATPTEFMBudgetHistory result, BudgetParameters item, bool isAdjusted)
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

        private static decimal CalculateFTDebitInvoiceAmount(PXGraph graph, BudgetParameters item, PXResultset<ATPTEFMFundTransactionReceiptDetail> receipts, bool isAdjusted)
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

        private static decimal CalculateFTRemainingBalance(PXGraph graph, ATPTEFMBudgetHistory result, BudgetParameters item, ATPTEFMFundTransaction fundTransaction, decimal totalReceiptAmount)
        {
            ATPTEFMFundTransactionReclassficationReceiptDetail reclassifiedReceipts = GetReclassifiedFTReceipts(graph, result, item);

            bool shouldAddDifference = (reclassifiedReceipts != null && fundTransaction.ReclassificationAmt > 0) ||
                                      fundTransaction.ReclassificationAmt == 0;

            return shouldAddDifference ? ((result.BudgetAmt ?? 0m) - totalReceiptAmount) : 0m;
        }
        private static decimal ProcessRFPReturn(PXGraph graph, ATPTEFMBudgetHistory result, BudgetParameters item, bool isAdjusted)
        {
            return CalculateRFPDebitInvoiceAmount(graph, item, GetRFPReceipts(graph, result), isAdjusted);
        }
        private static decimal CalculateRFPDebitInvoiceAmount(PXGraph graph, BudgetParameters item, PXResultset<EPExpenseClaimDetails> receipts, bool isAdjusted)
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
        private static decimal GetDebitInvoiceAmount(PXGraph graph, BudgetParameters item, EPExpenseClaimDetails er, bool isAdjusted)
        {
            return GetAPTransactionAmount(graph, item, er, isDebitAdjustment: true, requireReleased: isAdjusted);
            //if (isAdjusted)
            //{
            //    return PXSelectJoin<
            //                    APTran,
            //                    InnerJoin<APInvoice,
            //                    On<APInvoice.refNbr, Equal<APTran.refNbr>,
            //                    And<APInvoice.docType, Equal<APTran.tranType>>>>,
            //                    Where<APInvoice.origRefNbr, Equal<@P.AsString>,
            //                        And<APTran.origLineNbr, Equal<@P.AsInt>,
            //                        And<APInvoice.origDocType, Equal<@P.AsString>,
            //                        And<APInvoice.docType, Equal<APDocType.debitAdj>,
            //                        And<APInvoice.released, Equal<True>,
            //                        And<APInvoice.finPeriodID, Between<@P.AsString, @P.AsString>>>>>>>>
            //                    .Select(graph, er.APRefNbr, er.APLineNbr, er.APDocType, item.FromFinPeriodID, item.ToFinPeriodID)
            //                    .RowCast<APTran>()
            //                    .Sum(t => (t.CuryTranAmt ?? 0m) + (t.CuryRetainageAmt ?? 0m));
            //}
            //return PXSelectJoin<
            //                APTran,
            //                InnerJoin<APInvoice,
            //                On<APInvoice.refNbr, Equal<APTran.refNbr>,
            //                And<APInvoice.docType, Equal<APTran.tranType>>>>,
            //                Where<APInvoice.origRefNbr, Equal<@P.AsString>,
            //                    And<APTran.origLineNbr, Equal<@P.AsInt>,
            //                    And<APInvoice.origDocType, Equal<@P.AsString>,
            //                    And<APInvoice.docType, Equal<APDocType.debitAdj>,
            //                    And<APInvoice.finPeriodID, Between<@P.AsString, @P.AsString>>>>>>>
            //                .Select(graph, er.APRefNbr, er.APLineNbr, er.APDocType, item.FromFinPeriodID, item.ToFinPeriodID)
            //                .RowCast<APTran>()
            //                .Sum(t => (t.CuryTranAmt ?? 0m) + (t.CuryRetainageAmt ?? 0m));
        }
    }
}