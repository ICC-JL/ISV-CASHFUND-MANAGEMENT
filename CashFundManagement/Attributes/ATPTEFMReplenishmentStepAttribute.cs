using PX.Data;
using PX.Data.BQL;

namespace CashFundManagement.Attributes {
    public class ATPTEFMReplenishmentStepAttribute
    {
        #region Display
        public const string Default = "";
        public const string SubmitReceipt = "Submit Reciept";
        public const string Release = "Release";
        public const string Cancelled = "Cancelled";
        #endregion

        #region Values
        public const string DefaultValue = "D";
        public const string SubmitReceiptValue = "S";
        public const string ReleaseValue = "R";
        public const string CancelledValue = "X";
        #endregion

        #region BQL Accessors
        public class defaultValue : BqlString.Constant<defaultValue> {
            public defaultValue() : base(DefaultValue) { }
        }

        public class submitReceiptValue : BqlString.Constant<submitReceiptValue> {
            public submitReceiptValue() : base(SubmitReceiptValue) { }
        }
        public class releaseValue : BqlString.Constant<submitReceiptValue> {
            public releaseValue() : base(ReleaseValue) { }
        }
        public class cancelledValue : BqlString.Constant<cancelledValue> {
            public cancelledValue() : base(CancelledValue) { }
        } 
        #endregion

        public class ATPTEFMReplenishmentStep : PXStringListAttribute
        {
            public ATPTEFMReplenishmentStep() : base
               (
                   new[]
                   {
                        Pair(DefaultValue, Default),
                        Pair(SubmitReceiptValue, SubmitReceipt),
                        Pair(ReleaseValue, Release),
                        Pair(CancelledValue, Cancelled)
                   }
               )
            { }
        }
    }
}
