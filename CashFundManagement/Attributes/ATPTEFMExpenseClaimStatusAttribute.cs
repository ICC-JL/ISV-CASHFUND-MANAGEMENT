using PX.Data;
using PX.Data.BQL;

namespace CashFundManagement.Attributes {
    public class ATPTEFMExpenseClaimStatusAttribute
    {
        #region Display
        public const string Hold = "On Hold";
        public const string Pending = "Pending Approval";
        public const string Approved = "Approved";
        public const string Rejected = "Rejected";
        public const string Released = "Released";
        public const string Cancelled = "Cancelled";
        #endregion

        #region Values
        public const string HoldValue = "H";
        public const string PendingValue = "O";
        public const string ApprovedValue = "A";
        public const string RejectedValue = "C";
        public const string ReleasedValue = "R";
        public const string CancelledValue = "X";
        #endregion

        #region BQL Accessors
        public class holdValue : BqlString.Constant<holdValue> {
            public holdValue() : base(HoldValue) { }
        }

        public class pendingValue : BqlString.Constant<pendingValue> {
            public pendingValue() : base(PendingValue) { }
        }

        public class approvedValue : BqlString.Constant<approvedValue> {
            public approvedValue() : base(ApprovedValue) { }
        }

        public class rejectedValue : BqlString.Constant<rejectedValue> {
            public rejectedValue() : base(RejectedValue) { }
        }

        public class releasedValue : BqlString.Constant<releasedValue> {
            public releasedValue() : base(ReleasedValue) { }
        }

        public class cancelledValue : BqlString.Constant<cancelledValue> {
            public cancelledValue() : base(CancelledValue) { }
        }

        #endregion
        public class ATPTEFMExpenseClaimStatus : PXStringListAttribute
        {
            public ATPTEFMExpenseClaimStatus() : base
                (
                    new[]
                    {
                        Pair(HoldValue, Hold),
                        Pair(PendingValue, Pending),
                        Pair(ApprovedValue, Approved),
                        Pair(RejectedValue, Rejected),
                        Pair(ReleasedValue, Released),
                        Pair(CancelledValue, Cancelled)
                    }
                )
            { }
        }

    }
}
