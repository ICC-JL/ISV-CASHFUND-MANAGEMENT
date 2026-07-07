using PX.Data;
using PX.Data.BQL;

namespace CashFundManagement.Attributes {
    public class ATPTEFMExpenseReceiptStatusAttribute
    {
        #region Display
        public const string Open = "Open";
        public const string Pending = "Pending Approval";
        public const string Rejected = "Rejected";
        public const string Hold = "On Hold";
        public const string Release = "Released";
        public const string Cancelled = "Cancelled";
        #endregion

        #region Values
        public const string OpenValue = "A";
        public const string PendingValue = "O";
        public const string RejectedValue = "C";
        public const string HoldValue = "H";
        public const string ReleaseValue = "R";
        public const string CancelledValue = "X";
        #endregion

        #region BQL Accessors
        public class cancelledValue : BqlString.Constant<cancelledValue> {
            public cancelledValue() : base(CancelledValue) { }
        }

        public class rejectedValue : BqlString.Constant<rejectedValue> {
            public rejectedValue() : base(RejectedValue) { }
        }

        public class openValue : BqlString.Constant<openValue> {
            public openValue() : base(OpenValue) { }
        }

        public class pendingValue : BqlString.Constant<pendingValue> {
            public pendingValue() : base(PendingValue) { }
        }

        public class holdValue : BqlString.Constant<holdValue> {
            public holdValue() : base(HoldValue) { }
        }
        public class releaseValue : BqlString.Constant<releaseValue> {
            public releaseValue() : base(ReleaseValue) { }
        } 
        #endregion

        public class ATPTEFMExpenseReceiptStatus : PXStringListAttribute
        {
            public ATPTEFMExpenseReceiptStatus() : base
                (
                    new[]
                    {
                        Pair(HoldValue, Hold),
                        Pair(PendingValue, Pending),
                        Pair(OpenValue, Open),
                        Pair(RejectedValue, Rejected),
                        Pair(ReleaseValue, Release),
                        Pair(CancelledValue, Cancelled)
                    }
                )
            { }
        }
    }
}