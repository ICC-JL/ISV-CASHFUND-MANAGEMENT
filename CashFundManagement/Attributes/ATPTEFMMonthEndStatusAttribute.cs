using PX.Data;

namespace CashFundManagement.Attributes {
    public class ATPTEFMMonthEndStatusAttribute : Base.ATPTEFMBaseStatus
    {
        public class ATPTEFMMonthEndStatus : PXStringListAttribute
        {
            public ATPTEFMMonthEndStatus() : base
                (
                    new[]
                    {
                        Pair(OpenValue, Open),
                        Pair(PendingValue, Pending),
                        Pair(ClosedValue, Closed),
                        Pair(RejectedValue, Rejected),
                        Pair(HoldValue, Hold),
                        Pair(ReleaseValue, Release)
                    }
                )
            { }
        }
    }
}
