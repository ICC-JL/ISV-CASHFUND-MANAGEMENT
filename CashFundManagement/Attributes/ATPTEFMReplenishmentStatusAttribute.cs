using PX.Data;

namespace CashFundManagement.Attributes {
    public class ATPTEFMReplenishmentStatusAttribute : Base.ATPTEFMBaseStatus
    {
        public class ATPTEFMReplenishmentStatus : PXStringListAttribute
        {
            public ATPTEFMReplenishmentStatus() : base
                (
                    new[]
                    {
                        Pair(OpenValue, Open),
                        Pair(PendingValue, Pending),
                        Pair(ClosedValue, Closed),
                        Pair(RejectedValue, Rejected),
                        Pair(HoldValue, Hold),
                        Pair(ReleaseValue, Release),
                        Pair(CancelledValue, Cancelled)
                    }
                )
            { }
        }
    }
}
