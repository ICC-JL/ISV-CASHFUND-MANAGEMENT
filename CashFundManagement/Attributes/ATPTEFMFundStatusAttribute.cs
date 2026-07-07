using PX.Data;
using PX.Data.BQL;

namespace CashFundManagement.Attributes {
    public class ATPTEFMFundStatusAttribute : Base.ATPTEFMBaseStatus
    {
        #region Display
        public const string Reversed = "Reversed";
        #endregion

        #region Values
        public const string ReversedValue = "S";
        #endregion

        public class reversedValue : BqlString.Constant<reversedValue>
        {
            public reversedValue() : base(ReversedValue) { }
        }

        public class ATPTEFMFundStatus : PXStringListAttribute
        {
            public ATPTEFMFundStatus() : base
                (
                    new[]
                    {
                        Pair(HoldValue, Hold),
                        Pair(PendingValue, Pending),
                        Pair(RejectedValue, Rejected),
                        Pair(BalancedValue, Balanced),
                        Pair(ActiveValue, Active),
                        Pair(ClosedValue, Closed),
                        Pair(OpenValue, Open),
                        Pair(LiquidatedValue, Liquidated),
                        Pair(CancelledValue, Cancelled),
                        Pair(ReleaseValue, Release),
                        Pair(ReversedValue, Reversed),
                        Pair(PendingCloseValue, PendingClose)
                    }
                )
            { }
        }

        public class ATPTEFMFundTransactionStatus : PXStringListAttribute
        {
            public ATPTEFMFundTransactionStatus() : base
                (
                    new[]
                    {
                        Pair(OpenValue, Open),
                        Pair(PendingValue, Pending),
                        Pair(ClosedValue, Closed),
                        Pair(RejectedValue, Rejected),
                        Pair(HoldValue, Hold),
                        Pair(CancelledValue, Cancelled),
                        Pair(ReleaseValue, Release)
                    }
                )

            { }
        }
    }
}
