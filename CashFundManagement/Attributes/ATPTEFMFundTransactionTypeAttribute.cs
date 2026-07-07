using PX.Data;
using PX.Data.BQL;

namespace CashFundManagement.Attributes
{
    public class ATPTEFMFundTransactionTypeAttribute
    {
        #region Display
        public const string Establishment = Messages.ATPTEFMMessages.Establishment;
        public const string FundRequest = Messages.ATPTEFMMessages.FundRequest;
        public const string Reimbursement = "Reimbursement";
        public const string Reclassification = Messages.ATPTEFMMessages.Reclassification;
        public const string Replenishment = Messages.ATPTEFMMessages.Replenishment;
        public const string ExpenseReceipt = Messages.ATPTEFMMessages.ExpenseReceipt;
        public const string MonthEnd = Messages.ATPTEFMMessages.MonthEnd;
        public const string VoidedCheck = Messages.ATPTEFMMessages.VoidedCheck;
        public const string CloseFund = Messages.ATPTEFMMessages.CloseFund;
        public const string IncreaseFund = Messages.ATPTEFMMessages.IncreaseFund;
        public const string DecreaseFund = Messages.ATPTEFMMessages.DecreaseFund;
        public const string CashAdvance = "Request"; //"Cash Advance";
        #endregion

        #region Values
        public const string EstablishmentValue = "E";
        public const string FundRequestValue = "F";
        public const string ReclassificationValue = "Y";
        public const string ReimbursementValue = "R";
        public const string ReplenishmentValue = "T";
        public const string CloseFundValue = "Q";
        public const string ExpenseReceiptValue = "X";
        public const string VoidedCheckValue = "V";
        public const string MonthEndValue = "M";
        public const string IncreaseFundValue = "I";
        public const string DecreaseFundValue = "D";
        public const string CashAdvanceValue = "C";

        #endregion

        #region BQL Accessors
        public class establishment : BqlString.Constant<establishment>
        {
            public establishment() : base(Establishment) { }
        }
        public class fundRequest : BqlString.Constant<fundRequest>
        {
            public fundRequest() : base(FundRequest) { }
        }
        public class replenishment : BqlString.Constant<replenishment>
        {
            public replenishment() : base(Replenishment) { }
        }
        public class expenseReceipt : BqlString.Constant<expenseReceipt>
        {
            public expenseReceipt() : base(ExpenseReceipt) { }
        }
        public class voidedCheck : BqlString.Constant<voidedCheck>
        {
            public voidedCheck() : base(VoidedCheck) { }
        }
        public class increaseFundValue : BqlString.Constant<increaseFundValue>
        {
            public increaseFundValue() : base(IncreaseFundValue) { }
        }
        public class decreaseFundValue : BqlString.Constant<decreaseFundValue>
        {
            public decreaseFundValue() : base(DecreaseFundValue) { }
        }
        public class cashAdvanceValue : BqlString.Constant<cashAdvanceValue>
        {
            public cashAdvanceValue() : base(CashAdvanceValue) { }
        }
        public class reimbursementValue : BqlString.Constant<reimbursementValue>
        {
            public reimbursementValue() : base(ReimbursementValue) { }
        }
        #endregion
        public class ATPTEFMFundTransactionType : PXStringListAttribute
        {
            public ATPTEFMFundTransactionType() : base(
               new[]
               {
                    Pair(CashAdvanceValue, CashAdvance),
                    Pair(ReimbursementValue, Reimbursement),
               })
            { }
        }

        public class ATPTEFMFundTransactionHistoryType : PXStringListAttribute
        {
            public ATPTEFMFundTransactionHistoryType() : base(
               new[]
               {
                    Pair(EstablishmentValue, Establishment),
                    Pair(FundRequestValue, FundRequest),
                    Pair(ReclassificationValue, Reclassification),
                    Pair(ReimbursementValue, Reimbursement),
                    Pair(ReplenishmentValue, Replenishment),
                    Pair(CloseFundValue, CloseFund),
                    Pair(ExpenseReceiptValue, ExpenseReceipt),
                    Pair(VoidedCheckValue, VoidedCheck),
                    Pair(MonthEndValue, MonthEnd),
                    Pair(IncreaseFundValue, IncreaseFund),
                    Pair(DecreaseFundValue, DecreaseFund),
               })
            { }
        }
    }
}
