using PX.Data.BQL;

namespace CashFundManagement.Attributes.Base {
    public abstract class ATPTEFMBaseStatus
    {
        #region Display
        public const string Active = "Active";
        public const string Open = "Open";
        public const string Pending = "Pending Approval";
        public const string Closed = "Closed";
        public const string PendingClose = "Pending Close";
        public const string Rejected = "Rejected";
        public const string Hold = "On Hold";
        public const string Release = "Pending Release";
        public const string Approved = "Approved";
        public const string Balanced = "Balanced";
        public const string Dismissed = "Dismissed";
        public const string Voided = "Voided";
        public const string Liquidated = "Liquidated";
        public const string Cancelled = "Cancelled";
        #endregion

        #region Values
        public const string Initial = "_";
        public const string ActiveValue = "T";
        public const string OpenValue = "O";
        public const string PendingValue = "P";
        public const string ClosedValue = "C";
        public const string RejectedValue = "R";
        public const string HoldValue = "H";
        public const string ReleaseValue = "E";
        public const string ApprovedValue = "A";
        public const string BalancedValue = "B";
        public const string DismissedValue = "D";
        public const string VoidedValue = "V";
        public const string LiquidatedValue = "L";
        public const string CancelledValue = "X";
        public const string PendingCloseValue = "M";
        #endregion

        #region BQL Accessors
        public class initial : BqlString.Constant<initial> {
            public initial() : base(Initial) { }
        }
        public class activeValue : BqlString.Constant<activeValue> {
            public activeValue() : base(ActiveValue) { }
        }
        public class openValue : BqlString.Constant<openValue> {
            public openValue() : base(OpenValue) { }
        }
        public class pendingValue : BqlString.Constant<pendingValue> {
            public pendingValue() : base(PendingValue) { }
        }
        public class closedValue : BqlString.Constant<closedValue> {
            public closedValue() : base(ClosedValue) { }
        }
        public class rejectedValue : BqlString.Constant<rejectedValue> {
            public rejectedValue() : base(RejectedValue) { }
        }
        public class holdValue : BqlString.Constant<holdValue> {
            public holdValue() : base(HoldValue) { }
        }
        public class releaseValue : BqlString.Constant<releaseValue> {
            public releaseValue() : base(ReleaseValue) { }
        }
        public class approvedValue : BqlString.Constant<approvedValue> {
            public approvedValue() : base(ApprovedValue) { }
        }
        public class balancedValue : BqlString.Constant<balancedValue> {
            public balancedValue() : base(BalancedValue) { }
        }
        public class dismissedValue : BqlString.Constant<dismissedValue> {
            public dismissedValue() : base(DismissedValue) { }
        }
        public class voidedValue : BqlString.Constant<voidedValue> {
            public voidedValue() : base(VoidedValue) { }
        }
        public class liquidatedValue : BqlString.Constant<liquidatedValue> {
            public liquidatedValue() : base(LiquidatedValue) { }
        }
        public class cancelledValue : BqlString.Constant<cancelledValue> {
            public cancelledValue() : base(CancelledValue) { }
        }

        public class pendingCloseValue : BqlString.Constant<pendingCloseValue>
        {
            public pendingCloseValue() : base(PendingCloseValue) { }
        }
        #endregion
    } 
}