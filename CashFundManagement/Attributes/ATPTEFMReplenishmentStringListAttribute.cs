using PX.Data;
using PX.Data.BQL;

namespace CashFundManagement.Attributes {
    public class ATPTEFMReplenishmentStringListAttribute
    {
        #region Display
        public const string Amount = "By Amount";
        public const string Percent = "By Percent";
        public const string Warning = "Warning";
        public const string ErrorRestrict = "Error / Restrict";
        #endregion

        #region Values
        public const string AmountValue = "A";
        public const string PercentValue = "P";
        public const string WarningValue = "W";
        public const string ErrorRestrictValue = "E";
        #endregion

        #region BQL Accessors
        public class amountValue : BqlString.Constant<amountValue> {
            public amountValue() : base(AmountValue) { }
        }

        public class percentValue : BqlString.Constant<percentValue> {
            public percentValue() : base(PercentValue) { }
        }

        public class warningValue : BqlString.Constant<warningValue> {
            public warningValue() : base(WarningValue) { }
        }

        public class errorRestrictValue : BqlString.Constant<errorRestrictValue> {
            public errorRestrictValue() : base(ErrorRestrictValue) { }
        } 
        #endregion

        public class ATPTEFMReplenishmentLimitList : PXStringListAttribute
        {
            public ATPTEFMReplenishmentLimitList()
                : base(new[]
                {
                    Pair(AmountValue, Amount),
                    Pair(PercentValue, Percent)
                })
            { }
        }

        public class ATPTEFMReplenishmentRestrictionList : PXStringListAttribute
        {
            public ATPTEFMReplenishmentRestrictionList()
                : base(new[]
                {
                    Pair(WarningValue, Warning),
                    Pair(ErrorRestrictValue, ErrorRestrict)
                })
            {

            }
        }
        public class ATPTEFMFundTransactionLimitList : PXStringListAttribute
        {
            public ATPTEFMFundTransactionLimitList()
                : base(new[]
                {
                    Pair(AmountValue, Amount),
                    Pair(PercentValue, Percent)
                })
            { }
        }
        public class ATPTEFMFundTransactionRestrictionList : PXStringListAttribute
        {
            public ATPTEFMFundTransactionRestrictionList()
                : base(new[]
                {
                    Pair(WarningValue, Warning),
                    Pair(ErrorRestrictValue, ErrorRestrict)
                })
            {

            }
        }
    }
}
