using PX.Data;
using PX.Data.BQL;

namespace CashFundManagement.Attributes {
    public class ATPTEFMModuleTypeAttribute
    {
        #region Display
        public const string Funds = "Funds";
        public const string FundTransafer = "Fund Transfer";
        public const string Replenishment = "Replenishment";

        #endregion

        #region Values
        public const string FundsValue = "F";
        public const string FundTransaferValue = "T";
        public const string ReplenishmentValue = "R";
        #endregion

        #region BQL Accessors
        public class fundsValue : BqlString.Constant<fundsValue> {
            public fundsValue() : base(FundsValue) { }
        }

        public class fundTransaferValue : BqlString.Constant<fundTransaferValue> {
            public fundTransaferValue() : base(FundTransaferValue) { }
        }

        public class replenishmentValue : BqlString.Constant<replenishmentValue> {
            public replenishmentValue() : base(ReplenishmentValue) { }
        } 
        #endregion

        public class ATPTEFMModuleType : PXStringListAttribute
        {
            public ATPTEFMModuleType() : base(
                new[]
                {
                    Pair(FundsValue, Funds),
                    Pair(FundTransaferValue, FundTransafer),
                    Pair(ReplenishmentValue, Replenishment)
                })
            { }
        }
    }
}
