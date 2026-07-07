using PX.Data;

namespace CashFundManagement.Attributes {
    public static class ATPTEFMSourceTranTypeAttribute
    {
        public class ATPTEFMListAttribute : PXStringListAttribute
        {
            public ATPTEFMListAttribute() : base(
                new[]
                {
                    Pair(IncreaseFund, "Increase Fund"),
                    Pair(DecreaseFund, "Decrease Fund"),
                })
            { }
        }

        public const string IncreaseFund = "ICR";
        public const string DecreaseFund = "DCR";

        public class increaseFund : PX.Data.BQL.BqlString.Constant<increaseFund>
        {
            public increaseFund() : base(IncreaseFund) {; }
        }

        public class decreaseFund : PX.Data.BQL.BqlString.Constant<decreaseFund>
        {
            public decreaseFund() : base(DecreaseFund) {; }
        }
    }
}
