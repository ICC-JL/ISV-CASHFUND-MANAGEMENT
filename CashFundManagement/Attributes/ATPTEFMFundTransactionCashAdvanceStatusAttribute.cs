using PX.Data;
using PX.Data.BQL;

namespace CashFundManagement.Attributes {
    public class ATPTEFMFundTransactionCashAdvanceStatusAttribute
    {
        #region Display
        public const string Default = " ";
        public const string Unreleased = "Unreleased";
        public const string Unliquidated = "Unliquidated";
        public const string Liquidated = "Liquidated";
        public const string Cancelled = "Cancelled";
        public const string ForReclassification = "For Reclassification";
        #endregion

        #region Values
        public const string DefaultValue = "D";
        public const string UnreleasedValue = "N";
        public const string UnliquidatedValue = "U";
        public const string LiquidatedValue = "L";
        public const string CancelledValue = "X";
        public const string ForReclassificationValue = "R";
        #endregion

        #region BQL Accessors
        public class defaultValue : BqlString.Constant<defaultValue> {
            public defaultValue() : base(DefaultValue) { }
        }

        public class unreleasedValue : BqlString.Constant<unreleasedValue> {
            public unreleasedValue() : base(UnreleasedValue) { }
        }

        public class unliquidatedValue : BqlString.Constant<unliquidatedValue> {
            public unliquidatedValue() : base(UnliquidatedValue) { }
        }

        public class liquidatedValue : BqlString.Constant<liquidatedValue> {
            public liquidatedValue() : base(LiquidatedValue) { }
        }

        public class cancelledValue : BqlString.Constant<cancelledValue> {
            public cancelledValue() : base(CancelledValue) { }
        }
        public class forReclassificationValue : BqlString.Constant<forReclassificationValue> {
            public forReclassificationValue() : base(ForReclassificationValue) { }
        } 
        #endregion
        public class ATPTEFMFundTrandactionCashAdvanceStatus : PXStringListAttribute
        {
            public ATPTEFMFundTrandactionCashAdvanceStatus() : base
                (
                    new[]
                    {
                        Pair(DefaultValue, Default),
                        Pair(UnreleasedValue, Unreleased),
                        Pair(UnliquidatedValue, Unliquidated),
                        Pair(LiquidatedValue, Liquidated),
                        Pair(CancelledValue, Cancelled),
                        Pair(ForReclassificationValue, ForReclassification)
                    }
                )
            { }
        }
    }
}
