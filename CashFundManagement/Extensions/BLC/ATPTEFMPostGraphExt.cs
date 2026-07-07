using CashFundManagement.Attributes;
using CashFundManagement.BLC;
using CashFundManagement.DAC;
using PX.Data;
using PX.Objects.GL;
using PX.Objects.EP;
using CashFundManagement.Extensions.DAC;
using CashFundManagement.Helper;

namespace CashFundManagement.Extensions.BLC
{
    /// <remarks>
    /// 2024-08-14 : Adds multi-tenant support. {RRS}
    /// </remarks>
    public class ATPTEFMPostGraphExt : PXGraphExtension<PostGraph>
    {
#if Version23R2
        public static bool IsActive() => true;
#else
        public static bool IsActive() => ATPTEFMPrefetchSetup.IsActive;
#endif

        #region Views
        [PXViewName("Expense Receipts")]
        public PXSelect<
           EPExpenseClaimDetails,
           Where<EPExpenseClaimDetails.claimDetailCD, Equal<Required<EPExpenseClaimDetails.claimDetailCD>>>>
           ATPTEFMExpenseReceiptSingle;
        #endregion

        #region Overrides
        public delegate Batch reverseBatchProc(Batch b);
        [PXOverride]
        public Batch ReverseBatchProc(Batch b, reverseBatchProc baseMethod)
        {
            ATPTEFMMonthEnd monthEnd = PXSelect<
                ATPTEFMMonthEnd, 
                Where<ATPTEFMMonthEnd.journalBatchNbr, Equal<Required<ATPTEFMMonthEnd.journalBatchNbr>>>>
                .Select(Base, b.BatchNbr);

            var result = baseMethod(b);

            if (monthEnd != null)
            {
                if (result != null)
                {
                    //Month-End
                    ATPTEFMMonthEndEntry monthEndGraph = PXGraph.CreateInstance<ATPTEFMMonthEndEntry>();
                    monthEndGraph.Clear();

                    monthEnd.ReversingJournalBatchNbr = result.BatchNbr;
                    monthEndGraph.CurrentDocument.Update(monthEnd);
                    monthEndGraph.Save.Press();

                    //Expense Receipt

                    foreach (EPExpenseClaimDetails er in PXSelectJoin<
                        EPExpenseClaimDetails,
                        InnerJoin<ATPTEFMMonthEndDetail,
                            On<EPExpenseClaimDetails.claimDetailCD, Equal<ATPTEFMMonthEndDetail.expenseReceiptRefNbr>>>,
                        Where<ATPTEFMMonthEndDetail.monthEndRefNbr, Equal<Required<ATPTEFMMonthEndDetail.monthEndRefNbr>>>>
                        .Select(Base, monthEnd.RefNbr))
                    {
                        ATPTEFMExpenseReceiptSingle.Current = er;
                        if (ATPTEFMExpenseReceiptSingle.Current != null)
                        {
                            ATPTEFMEPExpenseClaimDetailsExt erExt = ATPTEFMExpenseReceiptSingle.Current.GetExtension<ATPTEFMEPExpenseClaimDetailsExt>();
                            erExt.UsrATPTEFMMonthEnd = false;
                            ATPTEFMExpenseReceiptSingle.UpdateCurrent();
                            ATPTEFMExpenseReceiptSingle.Cache.Persist(PXDBOperation.Update);
                        }
                    }
                }               
            }
            return result;
        }
        public delegate void postBatchProc(Batch b, bool createintercompany);
        [PXOverride]
        public void PostBatchProc(Batch b, bool createintercompany, postBatchProc baseMethod)
        {
            baseMethod(b, createintercompany);

            ATPTEFMMonthEnd me = PXSelect<
                ATPTEFMMonthEnd, 
                Where<ATPTEFMMonthEnd.journalBatchNbr, Equal<Required<ATPTEFMMonthEnd.journalBatchNbr>>>>
                .Select(Base, b.BatchNbr);

            if (me != null)
            {
                if (b.Status == BatchStatus.Posted)
                {
                    //Month-End
                    ATPTEFMMonthEndEntry meGraph = PXGraph.CreateInstance<ATPTEFMMonthEndEntry>();
                    meGraph.Clear();

                    me.Status = ATPTEFMMonthEndStatusAttribute.ClosedValue;
                    meGraph.Document.Update(me);
                    meGraph.Save.Press();

                    //Expense Receipt
                    foreach (EPExpenseClaimDetails er in PXSelectJoin<
                        EPExpenseClaimDetails,
                        InnerJoin<ATPTEFMMonthEndDetail,
                            On<EPExpenseClaimDetails.claimDetailCD, Equal<ATPTEFMMonthEndDetail.expenseReceiptRefNbr>>>,
                        Where<ATPTEFMMonthEndDetail.monthEndRefNbr, Equal<Required<ATPTEFMMonthEndDetail.monthEndRefNbr>>>>
                        .Select(Base, me.RefNbr))
                    {
                        ATPTEFMExpenseReceiptSingle.Current = er;
                        if (ATPTEFMExpenseReceiptSingle.Current != null)
                        {
                            ATPTEFMEPExpenseClaimDetailsExt erExt = ATPTEFMExpenseReceiptSingle.Current.GetExtension<ATPTEFMEPExpenseClaimDetailsExt>();
                            erExt.UsrATPTEFMMonthEnd = false;
                            ATPTEFMExpenseReceiptSingle.UpdateCurrent();
                            ATPTEFMExpenseReceiptSingle.Cache.Persist(PXDBOperation.Update);
                        }
                    }
                }
            }
        }
        #endregion
    }
}
