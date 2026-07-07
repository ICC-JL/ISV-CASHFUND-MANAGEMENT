using CashFundManagement.Messages;
using PX.Data;
using PX.Data.BQL;

namespace CashFundManagement.Attributes {
    public class ATPTEFMApprovalModuleAttribute : PXStringListAttribute
    {
        #region Values
        public const string Funds = "FU";
        public const string FundTransaction = "FT";
        public const string Replenishment = "RE";
        public const string MonthEnd = "ME";
        #endregion

        #region BQL Accessors
        public class funds : BqlString.Constant<funds> { public funds() : base(Funds) { } }
        public class fundsTransaction : BqlString.Constant<fundsTransaction> { public fundsTransaction() : base(FundTransaction) { } }
        public class replenishment : BqlString.Constant<replenishment> { public replenishment() : base(Replenishment) { } }
        public class monthend : BqlString.Constant<monthend> { public monthend() : base(MonthEnd) { } }
        #endregion

        #region Constructor
        public ATPTEFMApprovalModuleAttribute()
            : base(new[] {
                Pair(Funds, ATPTEFMMessages.ATPTEFMFund),
                Pair(FundTransaction, ATPTEFMMessages.ATPTEFMFundTransaction),
                Pair(Replenishment, ATPTEFMMessages.ATPTEFMReplenishment),
                Pair(MonthEnd, ATPTEFMMessages.ATPTEFMMonthEnd)
            })
        { }
        #endregion
    }
}
