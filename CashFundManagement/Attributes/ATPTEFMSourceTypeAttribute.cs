using PX.Data;


namespace CashFundManagement.Attributes {


    public static class ATPTEFMSourceTypeAttribute
	{
		public class ATPTEFMListAttribute : PXStringListAttribute
		{
			public ATPTEFMListAttribute() : base(
				new[]
				{
					Pair(CashAdvance, "Cash Advance"),
					Pair(ExpenseClaim, "Expense Claim"),
					Pair(Funds, "Funds"),
				})
			{ }
		}

		public const string CashAdvance = "CAD";
		public const string ExpenseClaim = "EXP";
        public const string Funds = "FND";

        #region BQL Accessors
        public class cashAdvance : PX.Data.BQL.BqlString.Constant<cashAdvance> {
			public cashAdvance() : base(CashAdvance) {; }
		}
		public class expenseClaim : PX.Data.BQL.BqlString.Constant<expenseClaim> {
			public expenseClaim() : base(ExpenseClaim) {; }
		}
		public class funds : PX.Data.BQL.BqlString.Constant<funds> {
			public funds() : base(Funds) {; }
		} 
		#endregion
	}

}