using PX.Data;
using PX.Data.BQL;

namespace CashFundManagement.Attributes {
    public class ATPTEFMFeatureBudgetModuleListAttribute
    {
        #region Display
        //public const string BillsAndAdjustments = "Bills and Adjustments";
        //public const string Requests = "Requests";
        public const string CashAdvance = "Cash Advance";
        public const string FundTransaction = "Fund Transaction";
        public const string RequestforPayment = "Request for Payment";
        #endregion

        #region Values
        //public const string BillsAndAdjustmentsValue = "B";
        //public const string RequestsValue = "R";
        public const string CashAdvanceValue = "C";
        public const string FundTransactionValue = "F";
        public const string RequestforPaymentValue = "P";
        #endregion

        #region BQL Accessors
        //public class billsAndAdjustmentsValue : BqlType<IBqlInt, string>.Constant<billsAndAdjustmentsValue> {
        //    public billsAndAdjustmentsValue() : base(BillsAndAdjustmentsValue) { }
        //}
        //public class requestsValue : BqlType<IBqlInt, string>.Constant<requestsValue> {
        //    public requestsValue() : base(RequestsValue) { }
        //}
        public class cashAdvanceValue : BqlType<IBqlInt, string>.Constant<cashAdvanceValue> {
            public cashAdvanceValue() : base(CashAdvanceValue) { }
        }
        public class fundTransactionValue : BqlType<IBqlInt, string>.Constant<fundTransactionValue> {
            public fundTransactionValue() : base(FundTransactionValue) { }
        }
        public class requestforPaymentValue : BqlType<IBqlInt, string>.Constant<requestforPaymentValue> {
            public requestforPaymentValue() : base(RequestforPaymentValue) { }
        } 
        #endregion

        public class ATPTEFMFeatureBudgetModuleList : PXStringListAttribute
        {
            public ATPTEFMFeatureBudgetModuleList()
                : base(
                    new[]
                    {
                        CashAdvanceValue, FundTransactionValue,RequestforPaymentValue
                    },
                    new[]
                    {
                        CashAdvance,FundTransaction,RequestforPayment
                    })
            {

            }
        }
    }
}