using PX.Data;
using PX.Data.BQL;

namespace CashFundManagement.Attributes {
    public class ATPTEFMFundTypeAttribute 
    {
        #region Display
        public const string PettyCash = "Petty Cash";
        public const string RevolvingFund = "Revolving Fund";
        #endregion

        #region Values
        public const string PettyCashValue = "P";
        public const string RevolvingFundValue = "R";
        #endregion

        #region BQL Accessors
        public class pettyCashValue : BqlString.Constant<pettyCashValue> {
            public pettyCashValue() : base(PettyCashValue) { }
        }

        public class revolvingFundValue : BqlString.Constant<revolvingFundValue> {
            public revolvingFundValue() : base(RevolvingFundValue) { }
        } 
        #endregion

        public class ATPTEFMFundType : PXStringListAttribute
        {
            public ATPTEFMFundType() : base(
                new[]
                {
                    Pair(PettyCashValue, PettyCash),
                    Pair(RevolvingFundValue, RevolvingFund),
                })
            { }   
        }
    }
}
