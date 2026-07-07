using PX.Data;
using PX.Data.BQL;

namespace CashFundManagement.Attributes {
    public class ATPTEFMFundTransactionStepAttribute
    {
        #region Display
        public const string Default = " ";
        public const string SubmitReceipt = "Submit Reciept";
        public const string Liquidated = "Liquidated";
        public const string Cancelled = "Cancelled";
        #endregion

        #region Values
        public const string DefaultValue = "D";
        public const string SubmitReceiptValue = "S";
        public const string LiquidatedReceiptValue = "L";
        public const string CancelledValue = "X";
        #endregion

        #region BQL Accessors
        public class defaultValue : BqlString.Constant<defaultValue> {
            public defaultValue() : base(DefaultValue) { }
        }
        public class submitReceiptValue : BqlString.Constant<submitReceiptValue> {
            public submitReceiptValue() : base(SubmitReceiptValue) { }
        }
        public class liquidatedValue : BqlString.Constant<liquidatedValue> {
            public liquidatedValue() : base(LiquidatedReceiptValue) { }
        }

        public class cancelledValue : BqlString.Constant<cancelledValue> {
            public cancelledValue() : base(CancelledValue) { }
        } 
        #endregion

        public class ATPTEFMFundTransactionStep : PXStringListAttribute
        {
            public ATPTEFMFundTransactionStep() : base
                (
                    new[]
                    {
                        Pair(DefaultValue, Default),
                        Pair(SubmitReceiptValue, SubmitReceipt),
                        Pair(LiquidatedReceiptValue, Liquidated),
                        Pair(CancelledValue, Cancelled)
                    }
                )

            { }
        }
    }
}
