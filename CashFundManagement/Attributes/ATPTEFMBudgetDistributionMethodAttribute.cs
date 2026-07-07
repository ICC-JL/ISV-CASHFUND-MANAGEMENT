using PX.Data;
using PX.Data.BQL;

namespace CashFundManagement.Attributes {
    public class ATPTEFMBudgetDistributionMethodAttribute : PXStringListAttribute
    {
        #region Display
        public const string Evenly = "Evenly";
        public const string PreviousYear = "Proportionally to the Previous Year";
        public const string ComparedValues = "Proportionally to Compared Values";
        #endregion

        #region Values
        public const string EvenlyVal = "E";
        public const string PreviousYearVal = "P";
        public const string ComparedValuesVal = "C";
        #endregion

        #region BQL Accessors
        public class evenlyVal : BqlType<IBqlInt, string>.Constant<evenlyVal> {
            public evenlyVal() : base(EvenlyVal) { }
        }

        public class previousYearVal : BqlType<IBqlInt, string>.Constant<previousYearVal> {
            public previousYearVal() : base(PreviousYearVal) { }
        }

        public class comparedValuesVal : BqlType<IBqlInt, string>.Constant<comparedValuesVal> {
            public comparedValuesVal() : base(ComparedValuesVal) { }
        } 
        #endregion

        public class ATPTEFMBudgetDistributionMethod : PXStringListAttribute
        {
            public ATPTEFMBudgetDistributionMethod()
             : base(new[]
                 {
                      Pair(EvenlyVal, Evenly),
                      Pair(PreviousYearVal, PreviousYear),
                      Pair(ComparedValuesVal, ComparedValues)
                 })
            { }
        }
    }
}