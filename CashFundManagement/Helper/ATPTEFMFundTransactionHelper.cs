using CashFundManagement.Attributes;
using CashFundManagement.DAC;

namespace CashFundManagement.Helper {
    internal class ATPTEFMFundTransactionHelper
    {
        #region Conditional Processes
        internal static bool IsSubmitReceipt(ATPTEFMFundTransaction FundTransactions) => FundTransactions.Step.Equals(ATPTEFMFundTransactionStepAttribute.SubmitReceiptValue);
        internal static bool IsFundUnliquidated(ATPTEFMFundTransaction FundTransactions) => FundTransactions.CashAdvanceStatus.Equals(ATPTEFMFundTransactionCashAdvanceStatusAttribute.UnliquidatedValue);
        internal static bool IsFundReclassification(ATPTEFMFundTransaction FundTransactions) => FundTransactions.CashAdvanceStatus.Equals(ATPTEFMFundTransactionCashAdvanceStatusAttribute.ForReclassificationValue);
        internal static bool IsFundStatusClosed(ATPTEFMFundTransaction FundTransactions) => FundTransactions.Status.Equals(ATPTEFMFundStatusAttribute.ClosedValue);
        internal static bool IsFundReimbursement(ATPTEFMFundTransaction FundTransactions) => FundTransactions.FundTransactionType.Equals(ATPTEFMFundTransactionTypeAttribute.ReimbursementValue);
        internal static bool IsFundLiquidated(ATPTEFMFundTransaction FundTransactions) => FundTransactions.Step.Equals(ATPTEFMFundTransactionStepAttribute.LiquidatedReceiptValue);
        internal static bool IsFundRequest(ATPTEFMFundTransaction FundTransactions) => FundTransactions.FundTransactionType.Equals(ATPTEFMFundTransactionTypeAttribute.CashAdvanceValue);
        internal static bool IsReleaseCash(ATPTEFMFundTransaction FundTransactions) => FundTransactions.IsReleasedCash.Equals(true);
        internal static bool IsUnReleased(ATPTEFMFundTransaction FundTransactions) => FundTransactions.Step.Equals(ATPTEFMFundTransactionCashAdvanceStatusAttribute.UnreleasedValue);

        #endregion

        #region Summary Area (Get Old Values)
        internal static decimal? GetOldActualSpentAmt(ATPTEFMFundTransaction FundTransactions) => FundTransactions.ActualSpentAmount;
        internal static decimal? GetOldBalanceAmt(ATPTEFMFundTransaction FundTransactions) => FundTransactions.Balance;
        internal static decimal? GetOldTotalWht(ATPTEFMFundTransaction FundTransactions) => FundTransactions.TotalWhtAmount;
        internal static decimal? GetOldWht(ATPTEFMFundTransactionReceiptDetail fundTransactionReceiptDetail) => fundTransactionReceiptDetail.WhtAmount;
        internal static decimal? GetOldNetUnitCost(ATPTEFMFundTransactionReceiptDetail fundTransactionReceiptDetail) => fundTransactionReceiptDetail.NetUnitCost;
        internal static decimal? GetOldtNetAmt(ATPTEFMFundTransactionReceiptDetail fundTransactionReceiptDetail) => fundTransactionReceiptDetail.NetAmt;
        internal static decimal? GetOldQty(ATPTEFMFundTransactionReceiptDetail fundTransactionReceiptDetail) => fundTransactionReceiptDetail.Qty;
        internal static decimal? GetOldUnitCost(ATPTEFMFundTransactionReceiptDetail fundTransactionReceiptDetail) => fundTransactionReceiptDetail.UnitCost;
        internal static decimal? GetOldAmount(ATPTEFMFundTransactionReceiptDetail fundTransactionReceiptDetail) => fundTransactionReceiptDetail.Amount;
        #endregion
    }
}
