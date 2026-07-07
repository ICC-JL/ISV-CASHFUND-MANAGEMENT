using PX.Data;


namespace CashFundManagement.Attributes {
    public static class ATPTEFMTranTypeAttribute
    {
		public class ATPTEFMListAttribute : PXStringListAttribute
		{
			public ATPTEFMListAttribute() : base(
				new[]
				{
					Pair(CashAdvance, "Cash Advance"),
					Pair(RequestforPayment, "Request for Payment"),					
				})
			{ }
		}

		public const string CashAdvance = "CAD";
		public const string RequestforPayment = "RFP";

        #region BQL Accessors
        public class cashAdvance : PX.Data.BQL.BqlString.Constant<cashAdvance> {
			public cashAdvance() : base(CashAdvance) {; }
		}
		public class requestforPayment : PX.Data.BQL.BqlString.Constant<requestforPayment> {
			public requestforPayment() : base(RequestforPayment) {; }
		} 
		#endregion

	}
}