using PX.Data;
using PX.Objects.AP;
using PX.Objects.CS;
using PX.Objects.GL;
using PX.Objects.GL.FinPeriods;
using PX.Objects.RQ;
using Branch = PX.Objects.GL.Branch;

namespace CashFundManagement.MethodExtensions {
    public class ATPTEFMBudgetExtension
    {
        /// <summary>
        /// Budget BQL View Library
        /// </summary>
        public class BudgetLibrary
        {
            public RQBudget BLBudget { get; set; }
            public GLBudgetLineDetail BLBudgetDetail { get; set; }
            public GLHistory BLBudgetHistory { get; set; }

            public APTran BLAPbudget { get; set; }
            public APTran BLAPbudgetExtensionApprove { get; set; }
            public APTran BLAPbudgetExtensionUnApprove { get; set; }
        }

        /// <summary>
        /// Budget Parameters/Arguments for BQL Views
        /// </summary>
        public class BudgetParameters
        {
            public int? LedgerID { get; set; }
            public int? BranchID { get; set; }
            public string RefNbr { get; set; }

            public int? AccountID { get; set; }
            public int? SubID { get; set; }

            public string FinYear { get; set; }
            public string FromFinPeriodID { get; set; }
            public string ToFinPeriodID { get; set; }
        }

        /// <summary>
        /// Returns a Budget BQL View Library Instance
        /// </summary>
        /// <param name="graph">PXGraph cache</param>
        /// <param name="args">BQL View Parameters</param>
        /// <returns></returns>
        public static BudgetLibrary GetBudgetViews(PXGraph graph, BudgetParameters args)
        {
            BudgetLibrary BL = new BudgetLibrary();

            #region Budget
            BL.BLBudget =
                PXSelectJoinGroupBy<RQBudget,
                    InnerJoin<MasterFinPeriod, On<MasterFinPeriod.finPeriodID, Equal<RQBudget.finPeriodID>>>,
                    Where<RQBudget.expenseAcctID, Equal<Required<RQBudget.expenseAcctID>>,
                        And<RQBudget.expenseSubID, Equal<Required<RQBudget.expenseSubID>>,
                        And<MasterFinPeriod.finYear, Equal<Required<MasterFinPeriod.finYear>>,
                        And<MasterFinPeriod.finPeriodID, Between<Required<MasterFinPeriod.finPeriodID>, Required<MasterFinPeriod.finPeriodID>>,
                        And<RQBudget.orderNbr, NotEqual<Required<RQBudget.orderNbr>>>>>>>,
                Aggregate<
                    GroupBy<RQBudget.expenseAcctID,
                    GroupBy<RQBudget.expenseSubID,
                    Sum<RQBudget.requestAmt,
                    Sum<RQBudget.curyRequestAmt,
                    Sum<RQBudget.aprovedAmt,
                    Sum<RQBudget.curyAprovedAmt,
                    Sum<RQBudget.unaprovedAmt,
                    Sum<RQBudget.curyUnaprovedAmt>>>>>>>>>>
                .SelectWindowed(graph, 0, 1,
                    args.AccountID,
                    args.SubID,
                    args.FinYear,
                    args.FromFinPeriodID,
                    args.ToFinPeriodID,
                    args.RefNbr);
            #endregion
            #region BudgetDetail
            BL.BLBudgetDetail =
                PXSelectGroupBy<GLBudgetLineDetail,
                    Where<GLBudgetLineDetail.ledgerID, Equal<Required<GLBudgetLineDetail.ledgerID>>,
                        And<GLBudgetLineDetail.finYear, Equal<Required<GLBudgetLineDetail.finYear>>,
                        And<GLBudgetLineDetail.accountID, Equal<Required<GLBudgetLineDetail.accountID>>,
                        And<GLBudgetLineDetail.subID, Equal<Required<GLBudgetLineDetail.subID>>,
                        And<GLBudgetLineDetail.finPeriodID, Between<Required<GLBudgetLineDetail.finPeriodID>, Required<GLBudgetLineDetail.finPeriodID>>>>>>>,
                Aggregate<
                    Sum<GLBudgetLineDetail.amount,
                    Sum<GLBudgetLineDetail.releasedAmount>>>>
                .SelectWindowed(graph, 0, 1,
                    args.LedgerID,
                    args.FinYear,
                    args.AccountID,
                    args.SubID,
                    args.FromFinPeriodID,
                    args.ToFinPeriodID);
            #endregion
            #region BudgetHistory
            BL.BLBudgetHistory =
                PXSelectJoin<GLHistory,
                    InnerJoin<Branch, On<Branch.ledgerID, Equal<GLHistory.ledgerID>, And<Branch.branchID, Equal<GLHistory.branchID>>>>,
                    Where<GLHistory.branchID, Equal<Required<GLHistory.branchID>>,
                        And<GLHistory.accountID, Equal<Required<GLHistory.accountID>>,
                        And<GLHistory.subID, Equal<Required<GLHistory.subID>>,
                        And<GLHistory.finPeriodID, Between<Required<GLHistory.finPeriodID>, Required<GLHistory.finPeriodID>>>>>>,
                    OrderBy<Desc<GLHistory.finPeriodID>>>
                .SelectWindowed(graph, 0, 1,
                    args.BranchID,
                    args.AccountID,
                    args.SubID,
                    args.FromFinPeriodID,
                    args.ToFinPeriodID);
            #endregion
            #region APbudget
            BL.BLAPbudget =
                PXSelectJoinGroupBy<APTran,
                    InnerJoin<MasterFinPeriod, On<MasterFinPeriod.finPeriodID, Equal<APTran.finPeriodID>>,
                    InnerJoin<APRegister, On<APRegister.refNbr, Equal<APTran.refNbr>>>>,
                    Where<APTran.accountID, Equal<Required<APTran.accountID>>,
                        And<APTran.subID, Equal<Required<APTran.subID>>,
                        And<MasterFinPeriod.finYear, Equal<Required<MasterFinPeriod.finYear>>,
                        And<MasterFinPeriod.finPeriodID, Between<Required<MasterFinPeriod.finPeriodID>, Required<MasterFinPeriod.finPeriodID>>,
                    And<APTran.refNbr, NotEqual<Required<APTran.refNbr>>>>>>>,
                Aggregate<
                    GroupBy<APTran.accountID,
                    GroupBy<APTran.subID,
                    Sum<APTran.curyTranAmt,
                    Sum<APTran.tranAmt>>>>>>
                .SelectWindowed(graph, 0, 1,
                    args.AccountID,
                    args.SubID,
                    args.FinYear,
                    args.FromFinPeriodID,
                    args.ToFinPeriodID,
                    args.RefNbr);
            #endregion
            #region APbudgetExtensionApprove
            BL.BLAPbudgetExtensionApprove =
                PXSelectJoinGroupBy<APTran,
                    InnerJoin<MasterFinPeriod, On<MasterFinPeriod.finPeriodID, Equal<APTran.finPeriodID>>,
                    InnerJoin<APRegister, On<APRegister.refNbr, Equal<APTran.refNbr>>>>,
                    Where<APTran.accountID, Equal<Required<APTran.accountID>>,
                        And<APTran.subID, Equal<Required<APTran.subID>>,
                        And<MasterFinPeriod.finYear, Equal<Required<MasterFinPeriod.finYear>>,
                        And<APRegister.approved, Equal<boolTrue>,
                        And<MasterFinPeriod.finPeriodID, Between<Required<MasterFinPeriod.finPeriodID>, Required<MasterFinPeriod.finPeriodID>>,
                        And<APTran.refNbr, NotEqual<Required<APTran.refNbr>>>>>>>>,
                Aggregate<
                    GroupBy<APTran.accountID,
                    GroupBy<APTran.subID,
                    Sum<APTran.curyTranAmt,
                    Sum<APTran.tranAmt>>>>>>
                .SelectWindowed(graph, 0, 1,
                    args.AccountID,
                    args.SubID,
                    args.FinYear,
                    args.FromFinPeriodID,
                    args.ToFinPeriodID,
                    args.RefNbr);
            #endregion
            #region APbudgetExtensionUnApprove
            BL.BLAPbudgetExtensionUnApprove =
                PXSelectJoinGroupBy<APTran,
                    InnerJoin<MasterFinPeriod, On<MasterFinPeriod.finPeriodID, Equal<APTran.finPeriodID>>,
                    InnerJoin<APRegister, On<APRegister.refNbr, Equal<APTran.refNbr>>>>,
                    Where<APTran.accountID, Equal<Required<APTran.accountID>>,
                        And<APTran.subID, Equal<Required<APTran.subID>>,
                        And<MasterFinPeriod.finYear, Equal<Required<MasterFinPeriod.finYear>>,
                        And<APRegister.approved, Equal<boolFalse>,
                       And<MasterFinPeriod.finPeriodID, Between<Required<MasterFinPeriod.finPeriodID>, Required<MasterFinPeriod.finPeriodID>>,
                       And<APTran.refNbr, NotEqual<Required<APTran.refNbr>>>>>>>>,
                Aggregate<
                    GroupBy<APTran.accountID,
                    GroupBy<APTran.subID,
                    Sum<APTran.curyTranAmt,
                    Sum<APTran.tranAmt>>>>>>
                .SelectWindowed(graph, 0, 1,
                    args.AccountID,
                    args.SubID,
                    args.FinYear,
                    args.FromFinPeriodID,
                    args.ToFinPeriodID,
                    args.RefNbr);
            #endregion

            return BL;
        }
    }
}