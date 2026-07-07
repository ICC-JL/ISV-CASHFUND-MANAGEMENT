using PX.Data;
using PX.Data.BQL;

namespace CashFundManagement.Attributes {
    public class ATPTEFMCashAdvanceStatusAttribute : Base.ATPTEFMBaseStatus
    {
        public const string PendingLiquidation = "Pending for Liquidation";
        public const string PendingLiquidationValue = "U";

        #region BQL Accessors
        public class pendingLiquidationValue : BqlString.Constant<pendingLiquidationValue> {
            public pendingLiquidationValue() : base(PendingLiquidationValue) { }
        } 
        #endregion
        public class ATPTEFMCashAdvanceStatus : PXStringListAttribute
        {
            public ATPTEFMCashAdvanceStatus()
                : base(
                    new[]
                    {
                        OpenValue, PendingValue,ClosedValue, RejectedValue,HoldValue,ReleaseValue,PendingLiquidationValue,CancelledValue
                    },
                    new[]
                    {
                        Open,Pending,Closed,Rejected,Hold,Release,PendingLiquidation,Cancelled
                    })
            {

            }
        }
    }
}