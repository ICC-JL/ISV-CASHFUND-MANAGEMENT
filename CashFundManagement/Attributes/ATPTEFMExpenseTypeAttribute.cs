using PX.Data;


namespace CashFundManagement.Attributes {

    public static class ATPTEFMExpenseTypeAttribute
	{
		public class ATPTEFMListAttribute : PXStringListAttribute
		{
			public ATPTEFMListAttribute() : base(
				new[]
				{
					Pair(Liquidation, "Liquidation"),
					Pair(RequestforPayment, "Request for Payment"),
					Pair(Replenishment, "Replenishment") //Added for Expense Receipts
				})
			{ }
		}

		public class ATPTEFMExpenseClaimListAttribute : PXStringListAttribute
		{
			public ATPTEFMExpenseClaimListAttribute() : base(
				new[]
				{
					Pair(Liquidation, "Liquidation"),
					Pair(RequestforPayment, "Request for Payment")
				})
			{ }
		}

		public const string Liquidation = "LIQ";
		public const string RequestforPayment = "RFP";
		public const string Replenishment = "REP";

        #region BQL Accessors
        public class liquidation : PX.Data.BQL.BqlString.Constant<liquidation> {
			public liquidation() : base(Liquidation) {; }
		}
		public class requestforPayment : PX.Data.BQL.BqlString.Constant<requestforPayment> {
			public requestforPayment() : base(RequestforPayment) {; }
		}

		public class replenishment : PX.Data.BQL.BqlString.Constant<replenishment> {
			public replenishment() : base(Replenishment) {; }
		} 
		#endregion

	}
}