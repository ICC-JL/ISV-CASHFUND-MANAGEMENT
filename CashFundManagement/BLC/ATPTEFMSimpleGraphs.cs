using CashFundManagement.DAC;
using PX.Data;
using PX.Data.BQL.Fluent;

namespace CashFundManagement.BLC
{
    public class ATPTEFMCASimple : PXGraph<ATPTEFMCASimple>
    {
        public PXSave<ATPTEFMCashAdvance> Save;

        public SelectFrom<ATPTEFMCashAdvance>.View CashAdvances;
        public SelectFrom<ATPTEFMCAReceiptDetail>.
            Where<ATPTEFMCAReceiptDetail.cashAdvanceNbr.IsEqual<ATPTEFMCashAdvance.cashAdvanceNbr.FromCurrent>>.View CashAdvanceReceiptLines;
    }

    public class ATPTEFMFundHistorySimple : PXGraph<ATPTEFMFundHistorySimple>
    {
        public PXSavePerRow<ATPTEFMFundTransactionHistoryView> Save;
        public PXCancel<ATPTEFMFundTransactionHistoryView> Cancel;

        public PXFilter<ATPTEFMFund> Filter;
        public SelectFrom<ATPTEFMFundTransactionHistoryView>.
            Where<ATPTEFMFundTransactionHistoryView.fundRefNbr.IsEqual<ATPTEFMFund.fundCD.FromCurrent>>.View Records;
    }

    public class ATPTEFMFundSimple : PXGraph<ATPTEFMFundSimple>
    {
        public PXSave<ATPTEFMFund> Save;

        public SelectFrom<ATPTEFMFund>.View Funds;
        public SelectFrom<ATPTEFMFundTransactionHistoryView>.
            Where<ATPTEFMFundTransactionHistoryView.fundRefNbr.IsEqual<ATPTEFMFund.fundCD.FromCurrent>>.View Records;
    }
}
