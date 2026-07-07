using PX.Data;


namespace CashFundManagement.Attributes {

    public static class ATPTEFMLiquidationRuleAttribute
	{
		public class ATPTEFMListAttribute : PXStringListAttribute
		{
			public ATPTEFMListAttribute() : base(
				new[]
				{
					Pair(Employee, "Employee"),
					Pair(RequestClass, "Request Class"),
					Pair(Standard, "Standard"),
				})
			{ }
		}

		public const string Employee = "E";
		public const string RequestClass = "R";
		public const string Standard = "S";

        #region BQL Accessors
        public class employee : PX.Data.BQL.BqlString.Constant<employee> {
			public employee() : base(Employee) {; }
		}
		public class requestClass : PX.Data.BQL.BqlString.Constant<requestClass> {
			public requestClass() : base(RequestClass) {; }
		}
		public class standard : PX.Data.BQL.BqlString.Constant<standard> {
			public standard() : base(Standard) {; }
		} 
		#endregion

	}

}