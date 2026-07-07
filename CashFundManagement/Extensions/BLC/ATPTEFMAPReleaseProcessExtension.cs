using CashFundManagement.Attributes;
using CashFundManagement.BLC;
using CashFundManagement.DAC;
using CashFundManagement.DAC.Setup;
using CashFundManagement.Extensions.DAC;
using CashFundManagement.Helper;
using PX.Data;
using PX.Data.BQL;
using PX.Objects.AP;
using PX.Objects.Common.Extensions;
using PX.Objects.EP;
using PX.Objects.GL;
using PX.Objects.IN;
using PX.Objects.IN.GraphExtensions.INRegisterEntryBaseExt;
using System;
using System.Collections.Generic;
using System.Linq;
using static PX.Objects.FA.FABookSettings.midMonthType;

namespace CashFundManagement.Extensions.BLC
{
    /// <remarks>
    /// 2024-08-14 : Adds multi-tenant support. {RRS}
    /// </remarks>
    public class ATPTEFMAPReleaseProcess_Extension : PXGraphExtension<APReleaseProcess>
    {
#if Version23R2
        public static bool IsActive() => true;
#else
        public static bool IsActive() => ATPTEFMPrefetchSetup.IsActive;
#endif
        #region View
        public PXSetup<ATPTEFMCASetup> ATPTEFMPreferences;

        [PXViewName("Transaction History")]
        public PXSelect<
            ATPTEFMFundTransactionHistoryView,
            Where<ATPTEFMFundTransactionHistoryView.refNbr, Equal<Required<ATPTEFMFundTransactionHistoryView.refNbr>>>>
            ATPTEFMTransactionHistoryView;

        [PXViewName("Check and Payment Ref Nbr")]
        public PXSelect<
            ATPTEFMFundTransactionHistoryView,
            Where<ATPTEFMFundTransactionHistoryView.checkNbr, Equal<Required<ATPTEFMFundTransactionHistoryView.checkNbr>>>>
            ATPTEFMTransactionHistoryCheckNbrView;

        [PXViewName("Replenishment")]
        public PXSelect<
            ATPTEFMReplenishment,
            Where<ATPTEFMReplenishment.replenishmentNbr, Equal<Required<ATPTEFMReplenishment.replenishmentNbr>>>>
            ATPTEFMReplenishmentView;

        [PXViewName("Replenishment Detail")]
        public PXSelect<
            ATPTEFMReplenishmentDetail,
            Where<ATPTEFMReplenishmentDetail.replenishmentNbr, Equal<Required<ATPTEFMReplenishmentDetail.replenishmentNbr>>>>
            ATPTEFMReplenishmentDetailView;

        [PXViewName("Replenishment Detail With AP Ref Nbr")]
        public PXSelect<
            ATPTEFMReplenishmentDetail,
            Where<ATPTEFMReplenishmentDetail.invoiceRefNbr, Equal<Required<ATPTEFMReplenishmentDetail.invoiceRefNbr>>>>
            ATPTEFMReplenishmentAPDetailView;

        [PXViewName("Fund")]
        public PXSelect<
            ATPTEFMFund,
            Where<ATPTEFMFund.fundCD, Equal<Required<ATPTEFMFund.fundCD>>>>
            ATPTEFMFundView;

        [PXViewName("Fund")]
        public PXSelect<
            ATPTEFMFund,
            Where<ATPTEFMFund.establishmentRefNbr, Equal<Required<ATPTEFMFund.establishmentRefNbr>>>>
            ATPTEFMFundEstablishmentView;

        public PXSelect<
            ATPTEFMFundTransaction,
            Where<ATPTEFMFundTransaction.refNbr, Equal<Required<ATPTEFMFundTransaction.refNbr>>>>
            ATPTEFMFundTransactionView;

        [PXViewName("Fund Receipt Details")]
        public PXSelect<
            ATPTEFMFundTransactionReceiptDetail,
            Where<ATPTEFMFundTransactionReceiptDetail.expenseReceiptRefNbr, Equal<Required<ATPTEFMFundTransactionReceiptDetail.expenseReceiptRefNbr>>>>
            ATPTEFMFundTransactionReceiptDetailView;

        [PXViewName("Expense Receipts")]
        public PXSelect<
            EPExpenseClaimDetails,
            Where<EPExpenseClaimDetails.claimDetailCD, Equal<Required<EPExpenseClaimDetails.claimDetailCD>>>>
            ATPTEFMExpenseReceipts;
        #endregion

        #region Overrides
        public delegate void closeInvoiceAndClearBalancesDelegate(APRegister apdoc);
        [PXOverride]
        public void CloseInvoiceAndClearBalances(APRegister apdoc, closeInvoiceAndClearBalancesDelegate baseMethod)
        {
            baseMethod(apdoc);

            if (apdoc != null)
            {
                ATPTEFMCashAdvanceEntry.CloseTransaction(apdoc.RefNbr);
                ATPTEFMCashAdvance ca = null;

                if (apdoc.DocType == APDocType.Prepayment)
                {
                    ATPTEFMCashAdvanceEntry caEntry = PXGraph.CreateInstance<ATPTEFMCashAdvanceEntry>();
                    ca = caEntry.CashAdvances.Search<ATPTEFMCashAdvance.invoiceRefNbr>(apdoc.RefNbr);

                    if (ca != null)
                    {
                        ca.Status = ATPTEFMCashAdvanceStatusAttribute.ClosedValue;
                        caEntry.CashAdvances.Update(ca);
                        caEntry.Save.Press();
                    }
                }
            }
        }
        /// <remarks>
        /// 2024-11-26 : Reversal of liquidation bill: WHT related to the row should also be deducted in the WHT field {JLTG} <br/>
        /// 2024-10-29 : pon voiding of a Refund related to CA Begbal, the amount on 'Return' Field on Summary area becomes negative. CASE: 008510 {JLTG}
        /// 2025-05-23 : Fund decrease process should not have the cash advance error message ' You are not authorized to process a cash advance'. {JLTG}
        /// 2025-08-28 : [De La Salle] Cancel is disable even the RFP is already reverse with closed status CASE: 012144 {JLTG}
        /// </remarks>
        public delegate void PersistDelegate();
        [PXOverride]
        public void Persist(PersistDelegate baseMethod)
        {
            baseMethod();

            APRegister doc = Base.APDocument.Current;
            ATPTEFMAPRegisterExt docExt = doc.GetExtension<ATPTEFMAPRegisterExt>();
            APPaymentEntry paymentEntry = PXGraph.CreateInstance<APPaymentEntry>();
            paymentEntry.Clear();
            ATPTEFMCashAdvanceEntry caEntry = PXGraph.CreateInstance<ATPTEFMCashAdvanceEntry>();
            caEntry.Clear();
            APInvoiceEntry invEntry = PXGraph.CreateInstance<APInvoiceEntry>();
            ATPTEFMCASetup caSetup = ATPTEFMPreferences.Select();
            APInvoice inv = new APInvoice();

            //Allow Correction bill of cash advance
            if (doc.Status == APDocStatus.Closed && doc.DocType == APDocType.DebitAdj)
            {
                EPExpenseClaimDetails claimDetails = PXSelect<
                    EPExpenseClaimDetails,
                    Where<EPExpenseClaimDetails.aPRefNbr, Equal<Required<EPExpenseClaimDetails.aPRefNbr>>>>
                    .Select(this.Base, doc.OrigRefNbr);

                ATPTEFMEPExpenseClaimDetailsExt claimDetailsExt = claimDetails.GetExtension<ATPTEFMEPExpenseClaimDetailsExt>();

                if (claimDetails != null)
                {
                    if (claimDetailsExt.UsrATPTEFMTranType == ATPTEFMExpenseTypeAttribute.Liquidation)
                    {
                        ATPTEFMCashAdvance cashAdvance = PXSelect<
                            ATPTEFMCashAdvance,
                            Where<ATPTEFMCashAdvance.cashAdvanceNbr, Equal<Required<ATPTEFMCashAdvance.cashAdvanceNbr>>>>
                            .Select(this.Base, claimDetailsExt.UsrATPTEFMReqRef);
                        if (cashAdvance != null)
                        {
                            APAdjust adjust = PXSelect<
                                APAdjust,
                                Where<APAdjust.adjgRefNbr, Equal<Required<APAdjust.adjgRefNbr>>,
                                    And<APAdjust.adjgDocType, Equal<Required<APAdjust.adjgDocType>>,
                                    And<APAdjust.adjdRefNbr, Equal<Required<APAdjust.adjdRefNbr>>>>>>
                                .Select(this.Base, doc.RefNbr, APPaymentType.DebitAdj, doc.OrigRefNbr);

                            if (adjust != null)
                            {
                                caEntry.CashAdvances.Current = cashAdvance;
                                HashSet<string> receiptsList = new HashSet<string>();
                                HashSet<string> listOfLiquidationRef = new HashSet<string>();
                                bool isReverse = false;

                                EPExpenseClaim claim = EPExpenseClaim.PK.Find(Base, claimDetails.RefNbr);
                                ATPTEFMEPExpenseClaimExt claimExt = claim.GetExtension<ATPTEFMEPExpenseClaimExt>();
                                var liquidationRefNbrs = caEntry.CashAdvanceReceiptLines.Select().RowCast<ATPTEFMCAReceiptDetail>().Where(r => !string.IsNullOrEmpty(r.LiquidationRef) && r.LiquidationRef != claimExt.UsrATPTEFMLiqNbr).Select(r => r.LiquidationRef).ToList();
                                listOfLiquidationRef.AddRange(liquidationRefNbrs);

                                #region Calculations for WHT
                                decimal? totalWht = decimal.Zero;
                                foreach (var liquidatioNRefNbr in listOfLiquidationRef)
                                {
                                    foreach (APInvoice apInvoice in PXSelectJoin<
                                        APInvoice,
                                        InnerJoin<APRegister,
                                            On<APRegister.refNbr, Equal<APInvoice.refNbr>,
                                            And<APRegister.docType, Equal<APInvoice.docType>>>>,
                                        Where<APInvoice.docType, Equal<APDocType.invoice>,
                                            And<ATPTEFMAPRegisterExt.usrATPTEFMLiqNbr, Equal<Required<ATPTEFMAPRegisterExt.usrATPTEFMLiqNbr>>>>>
                                        .Select(Base, liquidatioNRefNbr))
                                    {
                                        if (apInvoice != null)
                                        {
                                            totalWht += apInvoice.CuryOrigWhTaxAmt;
                                        }
                                    }
                                }
                                #endregion

                                foreach (EPExpenseClaimDetails receiptItem in PXSelect<
                                    EPExpenseClaimDetails,
                                    Where<EPExpenseClaimDetails.aPRefNbr, Equal<Required<EPExpenseClaimDetails.aPRefNbr>>>>
                                    .Select(this.Base, doc.OrigRefNbr))
                                {
                                    receiptsList.Add(receiptItem.ClaimDetailCD);
                                }

                                foreach (ATPTEFMCAReceiptDetail detail in caEntry.CashAdvanceReceiptLines.Select())
                                {
                                    if (receiptsList.Contains(detail.ExpenseReceiptRefNbr))
                                    {
                                        if (!isReverse)
                                            isReverse = true;


                                        //Receipts Details
                                        detail.Reversed = true;
                                        caEntry.CashAdvanceReceiptLines.Update(detail);
                                    }
                                }

                                if (isReverse)
                                {
                                    cashAdvance.Status = ATPTEFMCashAdvanceStatusAttribute.PendingLiquidationValue;
                                    cashAdvance.CuryWhtTaxAmount = totalWht;
                                    cashAdvance.WhtTaxAmount = totalWht;
                                    caEntry.CashAdvances.Update(cashAdvance);
                                    caEntry.Save.Press();
                                }

                            }
                        }

                        #region Update Receipt After EC Bill is Reversed
                        ExpenseClaimDetailEntry erGraph = PXGraph.CreateInstance<ExpenseClaimDetailEntry>();

                        foreach (EPExpenseClaimDetails receiptItem in PXSelect<
                            EPExpenseClaimDetails,
                            Where<EPExpenseClaimDetails.aPRefNbr, Equal<Required<EPExpenseClaimDetails.aPRefNbr>>>>
                            .Select(this.Base, doc.OrigRefNbr))
                        {
                            ATPTEFMEPExpenseClaimDetailsExt receiptItemExt = receiptItem.GetExtension<ATPTEFMEPExpenseClaimDetailsExt>();

                            erGraph.Clear();
                            erGraph.ClaimDetails.Current = claimDetails;

                            receiptItem.APRefNbr = null;
                            receiptItem.APDocType = null;
                            receiptItem.APLineNbr = null;

                            erGraph.ClaimDetails.Update(receiptItem);
                            erGraph.Save.Press();
                        }
                        #endregion
                    }
                    else if (claimDetailsExt.UsrATPTEFMTranType == ATPTEFMExpenseTypeAttribute.RequestforPayment)
                    {
                        //Trigger Save to trigger Budget Persist behavior test
                        ExpenseClaimEntry ecGraph = PXGraph.CreateInstance<ExpenseClaimEntry>();
                        ecGraph.Clear();

                        EPExpenseClaim claim = EPExpenseClaim.PK.Find(Base, claimDetails.RefNbr);
                        ecGraph.ExpenseClaim.Current = claim;

                        ecGraph.ExpenseClaim.Update(claim);
                        ecGraph.Save.Press();
                    }

                    #region Update Claim to Enable Cancel
                    var checkReceipts = PXSelect<
                                                                                    EPExpenseClaimDetails,
                                                                                    Where<EPExpenseClaimDetails.refNbr, Equal<Required<EPExpenseClaimDetails.refNbr>>>>
                                                                                    .Select(Base, claimDetails.RefNbr);
                    bool isAllBillReversedOrRejected = false;
                    int lastIterationCount = 0;
                    int totalCount = checkReceipts.Count();

                    foreach (EPExpenseClaimDetails result in checkReceipts)
                    {
                        ATPTEFMEPExpenseClaimDetailsExt resultExt = result.GetExtension<ATPTEFMEPExpenseClaimDetailsExt>();

                        lastIterationCount++;

                        APInvoice erInv = APInvoice.PK.Find(Base, result.APDocType, result.APRefNbr);
                        if (erInv != null)
                        {
                            APInvoice erInvDebit = PXSelect<
                                APInvoice,
                                Where<APInvoice.origDocType, Equal<Required<APInvoice.origDocType>>,
                                    And<APInvoice.origRefNbr, Equal<Required<APInvoice.origRefNbr>>,
                                    And<APInvoice.status, Equal<APDocStatus.closed>>>>>
                                .Select(Base, erInv.DocType, erInv.RefNbr);

                            if (erInv.Status != APDocStatus.Rejected && erInvDebit == null)
                            {
                                break;
                            }
                        }

                        if (lastIterationCount == totalCount)
                            isAllBillReversedOrRejected = true;
                    }

                    if (isAllBillReversedOrRejected)
                    {
                        ExpenseClaimEntry ecGraph = PXGraph.CreateInstance<ExpenseClaimEntry>();
                        ecGraph.Clear();

                        EPExpenseClaim claim = EPExpenseClaim.PK.Find(Base, claimDetails.RefNbr);
                        ecGraph.ExpenseClaim.Current = claim;

                        ATPTEFMEPExpenseClaimExt claimExt = claim.GetExtension<ATPTEFMEPExpenseClaimExt>();

                        claimExt.UsrATPTEFMEnableCancel = true;

                        ecGraph.ExpenseClaim.Update(claim);
                        ecGraph.Save.Press();
                    }
                    #endregion
                }
                APInvoice invoice = APInvoice.PK.Find(Base, doc.OrigDocType, doc.OrigRefNbr);
                if (invoice != null)
                {
                    ATPTEFMAPInvoiceExtension invoiceExt = invoice.GetExtension<ATPTEFMAPInvoiceExtension>();

                    if (invoiceExt.UsrATPTEFMBudgetEnabled ?? false)
                    {
                        APInvoiceEntry apGraph = PXGraph.CreateInstance<APInvoiceEntry>();
                        apGraph.Clear();

                        apGraph.Document.Current = invoice;

                        apGraph.Document.Update(invoice);
                        apGraph.Save.Press();
                    }
                }
            }
            int flagCount = 1;
            foreach (APAdjust invoice in PXSelect<
                APAdjust,
                Where<APAdjust.adjgRefNbr, Equal<Required<APAdjust.adjgRefNbr>>>>
                .Select<PXResultset<APAdjust>>(this.Base, doc.RefNbr))
            {

                ATPTEFMCashAdvance ca = PXSelect<
                    ATPTEFMCashAdvance,
                    Where<ATPTEFMCashAdvance.billType, Equal<Required<ATPTEFMCashAdvance.billType>>,
                        And<ATPTEFMCashAdvance.billRefNbr, Equal<Required<ATPTEFMCashAdvance.billRefNbr>>>>
                       >
                    .Select(this.Base, APDocType.Prepayment, invoice.AdjdRefNbr);

                if (doc.DocType == APDocType.Check)
                {
                    paymentEntry.Document.Current = paymentEntry.Document.Search<APPayment.refNbr>(doc.RefNbr);

                    if (ca != null)
                    {
                        caEntry.CashAdvances.Current = ca;
                        if (ca.Status == ATPTEFMCashAdvanceStatusAttribute.OpenValue)
                        {
                            ca.Status = ATPTEFMCashAdvanceStatusAttribute.PendingLiquidationValue;
                            ca.PmtType = paymentEntry.Document.Current.DocType;
                            ca.PmtRefNbr = paymentEntry.Document.Current.RefNbr;
                            ca.PpmType = caEntry.CurrentCashAdvance.Current.BillType;
                            ca.PpmRefNbr = caEntry.CurrentCashAdvance.Current.BillRefNbr;
                            ca.CuryChangeAmount = caEntry.CurrentCashAdvance.Current.BillBalance;
                            ca.ChangeAmount = caEntry.CurrentCashAdvance.Current.BillBalance;
                            caEntry.CashAdvances.Update(ca);
                        }
                        if (caEntry.IsDirty)
                        {
                            caEntry.Save.Press();
                        }
                    }

                }
                if (doc.DocType == APDocType.Refund)
                {
                    if (ca != null)
                    {
                        caEntry.Clear();
                        if (ca.Status == ATPTEFMCashAdvanceStatusAttribute.ClosedValue)
                        {
                            //ca.VendorRefundType = doc.DocType;
                            //ca.VendorRefundRefNbr = doc.RefNbr;
                            ca.RefundAmount += doc.CuryOrigDocAmt;
                            ca.CuryChangeAmount -= doc.CuryOrigDocAmt;
                            ca.ChangeAmount -= doc.OrigDocAmt;
                        }

                        if (ca.Status == ATPTEFMCashAdvanceStatusAttribute.PendingLiquidationValue)
                        {
                            //ca.VendorRefundType = doc.DocType;
                            //ca.VendorRefundRefNbr = doc.RefNbr;
                            ca.RefundAmount += doc.CuryOrigDocAmt;
                            ca.CuryChangeAmount -= doc.CuryOrigDocAmt;
                            ca.ChangeAmount -= doc.OrigDocAmt;

                            //-> 003975 issue
                            //if (invoice.DocBal == decimal.Zero)
                            //    ca.Status = ATPTEFMCashAdvanceStatusAttribute.ClosedValue;

                            if (ca.CuryChangeAmount == 0m)
                                ca.Status = ATPTEFMCashAdvanceStatusAttribute.ClosedValue;
                        }

                        caEntry.CashAdvances.Update(ca);
                        caEntry.Save.Press();
                    }
                    //-> CA Migration Mode
                    else if (ca == null)
                    {
                        ATPTEFMCashAdvance caMigration = PXSelect<
                            ATPTEFMCashAdvance,
                            Where<ATPTEFMCashAdvance.ppmType, Equal<Required<ATPTEFMCashAdvance.ppmType>>,
                                And<ATPTEFMCashAdvance.ppmRefNbr, Equal<Required<ATPTEFMCashAdvance.ppmRefNbr>>>>
                                        >
                            .Select(this.Base, APDocType.Prepayment, invoice.AdjdRefNbr);

                        if (caMigration != null && (caMigration.IsImported ?? false))
                        {
                            caEntry.Clear();
                            caEntry.CashAdvances.Current = caMigration;

                            if (caMigration.Status == ATPTEFMCashAdvanceStatusAttribute.ClosedValue)
                            {
                                //caMigration.VendorRefundType = doc.DocType;
                                //caMigration.VendorRefundRefNbr = doc.RefNbr;
                                caMigration.RefundAmount += doc.CuryOrigDocAmt;
                                caMigration.CuryChangeAmount -= doc.CuryOrigDocAmt;
                                caMigration.ChangeAmount -= doc.OrigDocAmt;
                            }

                            if (caMigration.Status == ATPTEFMCashAdvanceStatusAttribute.PendingLiquidationValue)
                            {
                                //caMigration.VendorRefundType = doc.DocType;
                                //caMigration.VendorRefundRefNbr = doc.RefNbr;
                                caMigration.RefundAmount += doc.CuryOrigDocAmt;
                                caMigration.CuryChangeAmount -= doc.CuryOrigDocAmt;
                                caMigration.ChangeAmount -= doc.OrigDocAmt;

                                //-> 003975 issue
                                //if (invoice.DocBal == decimal.Zero)
                                //    ca.Status = ATPTEFMCashAdvanceStatusAttribute.ClosedValue;

                                if (caMigration.CuryChangeAmount == 0m)
                                    caMigration.Status = ATPTEFMCashAdvanceStatusAttribute.ClosedValue;
                            }

                            caEntry.CashAdvances.Update(caMigration);
                            caEntry.Save.Press();
                        }
                    }
                }

                if (doc.DocType == APDocType.VoidRefund)
                {
                    if (flagCount == 1)
                    {

                        if (ca != null)
                        {
                            if (ca.VendorRefundRefNbr != null)
                            {
                                caEntry.Clear();

                                ca.VendorRefundType = null;
                                ca.VendorRefundRefNbr = null;
                                ca.RefundAmount += doc.CuryOrigDocAmt;
                                ca.CuryChangeAmount -= doc.CuryOrigDocAmt;
                                ca.ChangeAmount -= doc.OrigDocAmt;

                                if (ca.Status != ATPTEFMCashAdvanceStatusAttribute.PendingLiquidationValue)
                                    ca.Status = ATPTEFMCashAdvanceStatusAttribute.PendingLiquidationValue;

                                caEntry.CashAdvances.Update(ca);
                                caEntry.Save.Press();
                            }
                        }
                        else if (ca == null)
                        {
                            ATPTEFMCashAdvance caMigration = PXSelect<
                                ATPTEFMCashAdvance,
                                Where<ATPTEFMCashAdvance.ppmType, Equal<Required<ATPTEFMCashAdvance.ppmType>>,
                                    And<ATPTEFMCashAdvance.ppmRefNbr, Equal<Required<ATPTEFMCashAdvance.ppmRefNbr>>>>
                                           >
                                .Select(this.Base, APDocType.Prepayment, invoice.AdjdRefNbr);

                            if (caMigration != null && (caMigration.IsImported ?? false))
                            {
                                caEntry.Clear();

                                caMigration.VendorRefundType = null;
                                caMigration.VendorRefundRefNbr = null;
                                caMigration.RefundAmount += doc.CuryOrigDocAmt;
                                caMigration.CuryChangeAmount -= doc.CuryOrigDocAmt;
                                caMigration.ChangeAmount -= doc.OrigDocAmt;

                                if (caMigration.Status != ATPTEFMCashAdvanceStatusAttribute.PendingLiquidationValue)
                                    caMigration.Status = ATPTEFMCashAdvanceStatusAttribute.PendingLiquidationValue;

                                caEntry.CashAdvances.Update(caMigration);
                                caEntry.Save.Press();
                            }
                        }
                    }
                    flagCount++;
                }
            }

            ATPTEFMCashAdvance cAdv = PXSelect<
                ATPTEFMCashAdvance,
                Where<ATPTEFMCashAdvance.billRefNbr, Equal<Required<ATPTEFMCashAdvance.billRefNbr>>>>
                .Select(this.Base, doc.RefNbr);

            if (cAdv != null && doc.DocType.Equals(APDocType.Prepayment))
            {
                caEntry.CashAdvances.Current = cAdv;
                if (cAdv.Status == ATPTEFMCashAdvanceStatusAttribute.PendingLiquidationValue)
                {
                    if (cAdv.BillBalance == decimal.Zero && doc.DocBal == decimal.Zero)
                    {
                        cAdv.Status = ATPTEFMCashAdvanceStatusAttribute.ClosedValue;
                        cAdv.CuryChangeAmount = doc.CuryDocBal;
                        cAdv.ChangeAmount = doc.DocBal;
                    }

                    if (doc.CuryDocBal > decimal.Zero)
                    {
                        cAdv.CuryChangeAmount = doc.CuryDocBal;
                        cAdv.ChangeAmount = doc.DocBal;
                    }

                    caEntry.CashAdvances.Update(cAdv);
                    caEntry.Save.Press();
                }
            }
            else if (cAdv == null && doc.DocType.Equals(APDocType.Prepayment))
            {
                ATPTEFMCashAdvance caMigrated = PXSelect<
                    ATPTEFMCashAdvance,
                    Where<ATPTEFMCashAdvance.ppmType, Equal<Required<ATPTEFMCashAdvance.ppmType>>,
                        And<ATPTEFMCashAdvance.ppmRefNbr, Equal<Required<ATPTEFMCashAdvance.ppmRefNbr>>>>>
                    .Select(this.Base, doc.DocType, doc.RefNbr);

                if (caMigrated != null && (caMigrated.IsImported ?? false))
                {
                    caEntry.Clear();
                    caEntry.CashAdvances.Current = caMigrated;
                    if (caMigrated.Status == ATPTEFMCashAdvanceStatusAttribute.PendingLiquidationValue)
                    {
                        if (doc.CuryDocBal == decimal.Zero)
                        {
                            caMigrated.Status = ATPTEFMCashAdvanceStatusAttribute.ClosedValue;
                            caMigrated.CuryChangeAmount = doc.CuryDocBal;
                            caMigrated.ChangeAmount = doc.DocBal;
                        }

                        if (doc.CuryDocBal > decimal.Zero)
                        {
                            caMigrated.CuryChangeAmount = doc.CuryDocBal;
                            caMigrated.ChangeAmount = doc.DocBal;
                        }

                        caEntry.CashAdvances.Update(caMigrated);
                        caEntry.Save.Press();
                    }
                }
            }
            //Auto Apply Credit Adj To Prepayment
            if (doc.Released == true && doc.Status == APDocStatus.Open && doc.DocType == APDocType.CreditAdj && docExt.UsrATPTEFMSourceType == ATPTEFMSourceTypeAttribute.CashAdvance)
            {
                if (caSetup.AutoApplyCredAdjPPT == true)
                {
                    bool hasRow = false;

                    ATPTEFMCashAdvance cashAdvance = PXSelect<
                        ATPTEFMCashAdvance,
                        Where<ATPTEFMCashAdvance.cashAdvanceNbr, Equal<Required<ATPTEFMCashAdvance.cashAdvanceNbr>>>>
                        .Select(this.Base, docExt.UsrATPTEFMSourceRef);

                    if (cashAdvance != null)
                    {
                        APPayment payment = PXSelect<
                            APPayment,
                            Where<APPayment.docType, Equal<APDocType.prepayment>,
                                And<APPayment.refNbr, Equal<Required<APPayment.refNbr>>>>>
                            .Select(this.Base, cashAdvance.PpmRefNbr);

                        if (payment != null && payment.Status == APDocStatus.Open)
                        {
                            paymentEntry.Document.Current = payment;

                            paymentEntry.Document.Cache.SetValueExt<APPayment.adjDate>(payment, Base.Accessinfo.BusinessDate);

                            APAdjust newadj = new APAdjust();
                            newadj.AdjdDocType = doc.DocType;
                            newadj.AdjdRefNbr = doc.RefNbr;
                            newadj = paymentEntry.Adjustments.Insert(newadj);
                            //paymentEntry.Document.Current.AdjDate = Base.Accessinfo.BusinessDate;
                            paymentEntry.Document.Update(payment);
                            hasRow = true;
                        }
                        if (hasRow == true)
                        {
                            paymentEntry.Save.Press();
                            paymentEntry.release.Press();
                        }
                    }
                }
            }

            //Liquidate
            if (doc.Released == true && doc.Status == APDocStatus.Open && doc.DocType == APDocType.Invoice && docExt.UsrATPTEFMTranType == ATPTEFMExpenseTypeAttribute.Liquidation)
            {
                EPExpenseClaimDetails claimDetails = PXSelect<
                    EPExpenseClaimDetails,
                    Where<EPExpenseClaimDetails.refNbr, Equal<Required<EPExpenseClaimDetails.refNbr>>>>
                    .Select(this.Base, doc.OrigRefNbr);
                if (claimDetails != null)
                {
                    if (caSetup.AutoApplyPPT == true)
                    {
                        bool hasRow = false;
                        ATPTEFMEPExpenseClaimDetailsExt claimDetailsExt = claimDetails.GetExtension<ATPTEFMEPExpenseClaimDetailsExt>();
                        ATPTEFMCashAdvance cashAdvance = PXSelect<
                            ATPTEFMCashAdvance,
                            Where<ATPTEFMCashAdvance.cashAdvanceNbr, Equal<Required<ATPTEFMCashAdvance.cashAdvanceNbr>>>>
                            .Select(this.Base, claimDetailsExt.UsrATPTEFMReqRef);

                        if (cashAdvance != null)
                        {
                            APPayment payment = PXSelect<
                                APPayment,
                                Where<APPayment.docType, Equal<APDocType.prepayment>,
                                    And<APPayment.refNbr, Equal<Required<APPayment.refNbr>>>>>
                                .Select(this.Base, cashAdvance.PpmRefNbr);

                            if (payment != null && payment.Status == APDocStatus.Open)
                            {
                                paymentEntry.Document.Current = payment;

                                //payment.AdjDate = Base.Accessinfo.BusinessDate;
                                paymentEntry.Document.Cache.SetValueExt<APPayment.adjDate>(payment, Base.Accessinfo.BusinessDate);

                                APAdjust newadj = new APAdjust();

                                newadj.AdjdDocType = doc.DocType;
                                newadj.AdjdRefNbr = doc.RefNbr;

                                //This condition is for SO Error: Balance will negative if actual amount(liquidation amount) is greater than requested amount
                                decimal currentBalance = (decimal)(doc.CuryDocBal - doc.CuryOrigWhTaxAmt);

                                if (currentBalance > cashAdvance.BillBalance)
                                {
                                    newadj.CuryAdjgAmt = cashAdvance.BillBalance;
                                    newadj.CuryAdjdAmt = cashAdvance.BillBalance;
                                    newadj.AdjAmt = cashAdvance.BillBalance;
                                }
                                //

                                newadj = paymentEntry.Adjustments_Invoices.Insert(newadj);
                                paymentEntry.Document.Update(payment);
                                hasRow = true;
                            }
                            if (hasRow == true)
                            {
                                paymentEntry.Save.Press();
                                paymentEntry.release.Press();
                            }
                        }
                    }
                }
            }

            //Checks
            //Subject for Removal for optimization
            /* if (doc.Status == APDocStatus.Closed && doc.DocType == APDocType.Check)
             {
                 //Establishment
                 APAdjust adjust = PXSelect<
                     APAdjust,
                     Where<APAdjust.adjgRefNbr, Equal<Required<APAdjust.adjgRefNbr>>,
                         And<APAdjust.adjgDocType, Equal<Required<APAdjust.adjgDocType>>>>>
                     .Select(this.Base, doc.RefNbr, APPaymentType.Check);

                 ATPTEFMFundMaint fundMaint = PXGraph.CreateInstance<ATPTEFMFundMaint>();
                 if (adjust != null)
                 {
                     ATPTEFMFund funds = PXSelect<ATPTEFMFund, Where<ATPTEFMFund.establishmentRefNbr, Equal<Required<ATPTEFMFund.establishmentRefNbr>>>>.Select(this.Base, adjust.AdjdRefNbr);
                     if (funds == null) return;

                     if (funds != null)
                     {
                         fundMaint.Document.Current = funds;
                         if (funds.Status == ATPTEFMFundStatusAttribute.OpenValue)
                         {
                             funds.Status = ATPTEFMFundStatusAttribute.ActiveValue;
                             funds.IsActive = true;
                             fundMaint.Document.Update(funds);
                             fundMaint.Save.Press();
                         }
                     }
                 }
             }*/

            if (doc.Status == APDocStatus.Closed && doc.DocType == APDocType.VoidCheck)
            {
                //Establishment
                APAdjust adjust = PXSelect<
                    APAdjust,
                    Where<APAdjust.adjgRefNbr, Equal<Required<APAdjust.adjgRefNbr>>,
                        And<APAdjust.adjgDocType, Equal<Required<APAdjust.adjgDocType>>>>>
                    .Select(this.Base, doc.RefNbr, APPaymentType.VoidCheck);

                if (adjust != null)
                {
                    ATPTEFMCashAdvanceEntry cashAdvanceEntry = PXGraph.CreateInstance<ATPTEFMCashAdvanceEntry>();
                    ATPTEFMCashAdvance cashAdvance = PXSelect<
                        ATPTEFMCashAdvance,
                        Where<ATPTEFMCashAdvance.pmtRefNbr, Equal<Required<ATPTEFMCashAdvance.pmtRefNbr>>>>
                        .Select(this.Base, doc.RefNbr);
                    if (cashAdvance != null)
                    {
                        cashAdvanceEntry.CashAdvances.Current = cashAdvance;
                        cashAdvance.Status = ATPTEFMCashAdvanceStatusAttribute.OpenValue;
                        cashAdvance.BillType = string.Empty;
                        cashAdvance.BillRefNbr = string.Empty;
                        cashAdvance.PmtType = string.Empty;
                        cashAdvance.PmtRefNbr = string.Empty;
                        cashAdvance.PpmType = string.Empty;
                        cashAdvance.PpmRefNbr = string.Empty;
                        cashAdvanceEntry.CashAdvances.Update(cashAdvance);
                        cashAdvanceEntry.Save.Press();
                    }
                }
            }

            //Subject for Removal for optimization
            /*if (doc.Status == APDocStatus.Closed && doc.DocType == APDocType.DebitAdj)
            {
                //Establishment
                #region Subject for removal for Optimization
                *//*ATPTEFMFundMaint fundMaint = PXGraph.CreateInstance<ATPTEFMFundMaint>();
               ATPTEFMFund funds = PXSelect<ATPTEFMFund, Where<ATPTEFMFund.establishmentRefNbr, Equal<Required<ATPTEFMFund.establishmentRefNbr>>>>.Select(this.Base, doc.OrigRefNbr);

               if (funds != null)
               {
                   fundMaint.Document.Current = funds;
                   if (funds.Status == ATPTEFMFundStatusAttribute.ActiveValue || funds.Status == ATPTEFMFundStatusAttribute.OpenValue)
                   {
                       funds.Status = ATPTEFMFundStatusAttribute.BalancedValue;
                       funds.Released = false;
                       fundMaint.Document.Update(funds);
                       fundMaint.Save.Press();
                   }
               }*//*
                #endregion

                //Replenishment
                ATPTEFMReplenishmentEntry replenishmentEntry = PXGraph.CreateInstance<ATPTEFMReplenishmentEntry>();
                ATPTEFMReplenishment replenishment = PXSelect<ATPTEFMReplenishment, Where<ATPTEFMReplenishment.invoiceRefNbr, Equal<Required<ATPTEFMReplenishment.invoiceRefNbr>>>>.Select(this.Base, doc.OrigRefNbr);

                if (replenishment != null)
                {
                    replenishmentEntry.Replenishments.Current = replenishment;

                    replenishment.Status = ATPTEFMReplenishmentStatusAttribute.OpenValue;
                    replenishment.Step = ATPTEFMReplenishmentStepAttribute.SubmitReceiptValue;
                    replenishment.IsReleased = false;
                    replenishmentEntry.Replenishments.Update(replenishment);
                    replenishmentEntry.Save.Press();

                    var replenishmentDetails = PXSelect<
                        ATPTEFMReplenishmentDetail,
                        Where<ATPTEFMReplenishmentDetail.replenishmentNbr, Equal<Required<ATPTEFMReplenishmentDetail.replenishmentNbr>>>>
                        .Select(this.Base, replenishment.ReplenishmentNbr);

                    foreach (ATPTEFMReplenishmentDetail item in replenishmentDetails)
                    {
                        ATPTEFMFundTransactionEntry graph = PXGraph.CreateInstance<ATPTEFMFundTransactionEntry>();

                        ATPTEFMFundTransactionReceiptDetail detail = PXSelect<
                            ATPTEFMFundTransactionReceiptDetail,
                            Where<ATPTEFMFundTransactionReceiptDetail.expenseReceiptRefNbr, Equal<Required<ATPTEFMFundTransactionReceiptDetail.expenseReceiptRefNbr>>>>
                            .Select(this.Base, item.ExpenseReceiptNbr);

                        detail.ReplenishmentRefNbr = null;
                        graph.FundTransactionReceiptLines.Update(detail);
                        graph.Save.Press();

                        ExpenseClaimDetailEntry expenseGraph = PXGraph.CreateInstance<ExpenseClaimDetailEntry>();
                        EPExpenseClaimDetails expenseClaimDetails = PXSelect<
                            EPExpenseClaimDetails,
                            Where<EPExpenseClaimDetails.claimDetailCD, Equal<Required<EPExpenseClaimDetails.claimDetailCD>>>>
                            .Select(this.Base, item.ExpenseReceiptNbr);
                        expenseClaimDetails.Status = EPExpenseClaimDetailsStatus.ApprovedStatus;
                        expenseGraph.ClaimDetails.Update(expenseClaimDetails);
                        expenseGraph.Save.Press();
                    }
                }
            }*/
        }
        #endregion

        #region Methods
        public delegate List<APRegister> ReleaseDocProcDelegate(JournalEntry je, APRegister doc, bool isPrebooking, out List<INRegister> inDocs);
        [PXOverride]
        public List<APRegister> ReleaseDocProc(JournalEntry je, APRegister doc, bool isPrebooking, out List<INRegister> inDocs, ReleaseDocProcDelegate baseMethod)
        {
            List<APRegister> r = baseMethod(je, doc, isPrebooking, out inDocs);

            switch (doc.DocType)
            {
                case APDocType.Check:
                    IncreaseFundPaymentRelease(doc);
                    FundEstablishmentPayment(doc);
                    ReplenishmentPayment(doc);
                    break;
                case APDocType.DebitAdj:
                    if (Base.Accessinfo.ScreenID == "AP.30.20.00")
                    {
                        FundDebitAdjustment(doc);
                        ReplenishmentDebitAdjustment(doc);
                        IncreaseFundReversed(doc);
                        DecreaseFundReversed(doc);
                    }
                    break;
                case APDocType.VoidCheck:
                    ReplenishmentVoidPayment(doc);
                    FundVoidEstablishmentPayment(doc);
                    break;
                case APDocType.Refund:
                    CloseFund(doc);
                    DecreaseFundRefundRelease(doc);
                    break;
                case APDocType.Prepayment:
                    ClosedCAReversedApplication(doc);
                    break;
                default: break;
            }
            return r;
        }


        #region Funds
        /// <summary>
        /// Handles the processing of Fund Establishment Payments upon release.
        /// Recent Changes:
        /// 1. Changed from single record to multiple records processing
        /// 2. Removed redundant code block
        /// </summary>
        /// <remarks>
        /// 2025-07-15 :Added filtering criteria based on `transactionType` to ensure correct data retrieval. {JLG}
        /// 2025-11-06 : Change implementation from cache update to graph instance to handle multiple records. {JLG}
        /// </remarks>
        private void FundEstablishmentPayment(APRegister doc)
        {
            ATPTEFMFundSimple fundMaint = PXGraph.CreateInstance<ATPTEFMFundSimple>();

            PXResultset<APAdjust> adjustments = PXSelect<
                APAdjust,
                Where<APAdjust.adjgRefNbr, Equal<Required<APAdjust.adjgRefNbr>>,
                    And<APAdjust.voided, NotEqual<True>,
                    And<APAdjust.adjgDocType, NotEqual<APPaymentType.debitAdj>>>>>
                .Select(Base, doc.RefNbr);

            foreach (APAdjust adj in adjustments)
            {
                var fund = PXSelect<
                    ATPTEFMFund,
                    Where<ATPTEFMFund.establishmentRefNbr,
                        Equal<Required<ATPTEFMFund.establishmentRefNbr>>>>
                    .Select(Base, adj.AdjdRefNbr)
                    .RowCast<ATPTEFMFund>()
                    .FirstOrDefault();

                if (fund != null)
                {
                    var transHistory = PXSelect<
                        ATPTEFMFundTransactionHistoryView,
                        Where<ATPTEFMFundTransactionHistoryView.refNbr, Equal<Required<ATPTEFMFundTransactionHistoryView.refNbr>>,
                            And<ATPTEFMFundTransactionHistoryView.transactionType, Equal<CashFundManagement.BLC.ATPTEFMFundMaint.ATPTEFMTransactionHistoryView.transactionType.establishment>>>>
                        .Select(Base, adj.AdjdRefNbr)
                        .RowCast<ATPTEFMFundTransactionHistoryView>()
                        .FirstOrDefault();

                    if (transHistory != null)
                    {
                        fundMaint.Clear();
                        fundMaint.Funds.Current = fund;

                        #region Update Fund Summary
                        fund.Status = ATPTEFMFundStatusAttribute.ActiveValue;
                        fund.IsActive = true;
                        fund.CuryFundAmt = adj.CuryAdjgAmt;
                        fund.CuryBalanceAmt = adj.CuryAdjgAmt;
                        fundMaint.Funds.Update(fund);
                        #endregion

                        #region Transaction History Establishment
                        transHistory.Status = APDocStatus.Closed;
                        transHistory.CuryBalanceAmt = adj.CuryAdjgAmt;
                        transHistory.CheckNbr = doc.RefNbr;
                        transHistory.CuryCheckAmt = adj.CuryAdjgAmt;
                        fundMaint.Records.Update(transHistory);
                        #endregion

                        fundMaint.Save.Press();
                    }
                }
            }
        }
        private void FundDebitAdjustment(APRegister doc)
        {
            ATPTEFMAPRegisterExt apRegisterExt = doc.GetExtension<ATPTEFMAPRegisterExt>();
            ATPTEFMFundView.Current = ATPTEFMFundView.Select(apRegisterExt.UsrATPTEFMSourceRef);

            if (ATPTEFMFundView.Current != null)
            {
                APAdjust apAdjust = PXSelect<
                    APAdjust,
                    Where<APAdjust.adjdRefNbr, Equal<Required<APAdjust.adjdRefNbr>>,
                        And<APAdjust.voided, NotEqual<True>,
                        And<APAdjust.adjgDocType, Equal<APPaymentType.debitAdj>>>>>
                    .Select(Base, ATPTEFMFundView.Current.EstablishmentRefNbr);

                if (apAdjust != null)
                {
                    ATPTEFMTransactionHistoryView.Current = PXSelect<
                        ATPTEFMFundTransactionHistoryView,
                        Where<ATPTEFMFundTransactionHistoryView.refNbr, Equal<Required<ATPTEFMFundTransactionHistoryView.refNbr>>,
                        And<ATPTEFMFundTransactionHistoryView.transactionType, Equal<CashFundManagement.BLC.ATPTEFMFundMaint.ATPTEFMTransactionHistoryView.transactionType.establishment>>>>
                        .Select(Base, apAdjust.AdjdRefNbr);

                    #region Set status to balance and set release to false
                    ATPTEFMFundView.Current.Status = ATPTEFMFundStatusAttribute.BalancedValue;
                    ATPTEFMFundView.Current.Released = false;
                    ATPTEFMFundView.UpdateCurrent();
                    ATPTEFMFundView.Cache.Persist(PXDBOperation.Update);
                    #endregion

                    #region Update Transaction History (Establishment)
                    if (ATPTEFMTransactionHistoryView.Current != null)
                    {
                        ATPTEFMTransactionHistoryView.Current.Status = ATPTEFMFundStatusAttribute.ReversedValue;
                        ATPTEFMTransactionHistoryView.UpdateCurrent();
                        ATPTEFMTransactionHistoryView.Cache.Persist(PXDBOperation.Update);
                    }
                    #endregion
                }
            }
        }

        private void FundVoidEstablishmentPayment(APRegister doc)
        {
            APAdjust adjust = PXSelectJoin<
                APAdjust,
                InnerJoin<ATPTEFMFund,
                    On<ATPTEFMFund.establishmentRefNbr, Equal<APAdjust.adjdRefNbr>>>,
                Where<APAdjust.adjgRefNbr, Equal<Required<APAdjust.adjgRefNbr>>>>
                .Select(Base, doc.RefNbr);

            if (adjust != null)
            {
                ATPTEFMFundView.Current = PXSelect<
                    ATPTEFMFund,
                    Where<ATPTEFMFund.establishmentRefNbr,
                    Equal<Required<ATPTEFMFund.establishmentRefNbr>>>>
                    .Select(Base, adjust.AdjdRefNbr);

                if (ATPTEFMFundView.Current != null)
                {
                    #region Summary Area
                    if (ATPTEFMFundView.Current != null)
                    {
                        ATPTEFMFundView.Current.CuryBalanceAmt -= adjust.CuryAdjdAmt;
                        ATPTEFMFundView.Current.Status = ATPTEFMFundStatusAttribute.OpenValue;
                        ATPTEFMFundView.Current.IsActive = false;
                        ATPTEFMFundView.UpdateCurrent();
                        ATPTEFMFundView.Cache.Persist(PXDBOperation.Update);
                    }
                    #endregion

                    #region Transaction History

                    ATPTEFMTransactionHistoryView.Current = PXSelect<
                        ATPTEFMFundTransactionHistoryView,
                        Where<ATPTEFMFundTransactionHistoryView.checkNbr, Equal<Required<ATPTEFMFundTransactionHistoryView.checkNbr>>>>
                        .Select(Base, doc.RefNbr);

                    if (ATPTEFMTransactionHistoryView.Current != null)
                    {
                        ATPTEFMTransactionHistoryView.Current.CheckNbr = null;
                        ATPTEFMTransactionHistoryView.Current.CuryCheckAmt = null;
                        ATPTEFMTransactionHistoryView.Current.Status = ATPTEFMFundStatusAttribute.OpenValue;
                        ATPTEFMTransactionHistoryView.Current.CuryBalanceAmt = decimal.Zero;
                        ATPTEFMTransactionHistoryView.UpdateCurrent();
                        ATPTEFMTransactionHistoryView.Cache.Persist(PXDBOperation.Update);
                    }
                    #endregion
                }
            }
        }
        #endregion

        #region Replenishment
        /// <remarks>
        /// 2025-02-06 : Considering the multiple bills in one payment. Case: 009686 {JLTG}
        /// </remarks>
        private void ReplenishmentPayment(APRegister doc)
        {
            PXResultset<APAdjust, APInvoice> adjustments = PXSelectJoinGroupBy<
                APAdjust,
                InnerJoin<APInvoice,
                    On<APInvoice.refNbr, Equal<APAdjust.adjdRefNbr>,
                    And<APInvoice.docType, Equal<APAdjust.adjdDocType>>>>,
                Where<APAdjust.adjgRefNbr, Equal<Required<APAdjust.adjgRefNbr>>,
                    And<APAdjust.voided, NotEqual<True>,
                    And<APAdjust.adjgDocType, NotEqual<APPaymentType.debitAdj>>>>,
                Aggregate<
                    GroupBy<APInvoice.docType,
                    GroupBy<APInvoice.refNbr>>>>
                .Select<PXResultset<APAdjust, APInvoice>>(Base, doc.RefNbr);

            var fundEntry = PXGraph.CreateInstance<ATPTEFMFundMaint>();

            foreach (PXResult<APAdjust, APInvoice> result in adjustments)
            {
                APAdjust adjustment = result;
                APInvoice invoice = result;

                ATPTEFMReplenishment replenishment = PXSelect<
                    ATPTEFMReplenishment,
                    Where<ATPTEFMReplenishment.replenishmentNbr,
                        Equal<Required<ATPTEFMReplenishment.replenishmentNbr>>>>
                    .Select(Base, invoice.OrigRefNbr);

                if (replenishment != null)
                {
                    var fund = PXSelect<
                        ATPTEFMFund,
                        Where<ATPTEFMFund.fundCD, Equal<Required<ATPTEFMFund.fundCD>>>>
                        .Select(Base, replenishment.FundID)
                        .RowCast<ATPTEFMFund>()
                        .FirstOrDefault();

                    if (fund != null)
                    {
                        fund.CuryOnReplenishmentAmt -= adjustment.CuryAdjgAmt;
                        fund.CuryBalanceAmt += adjustment.CuryAdjgAmt;
                        fundEntry.Document.Update(fund);

                        // Get Transaction History
                        var transHistory = PXSelect<
                            ATPTEFMFundTransactionHistoryView,
                            Where<ATPTEFMFundTransactionHistoryView.refNbr, Equal<Required<ATPTEFMFundTransactionHistoryView.refNbr>>,
                                And<ATPTEFMFundTransactionHistoryView.checkNbr, IsNull>>>
                            .Select(Base, replenishment.ReplenishmentNbr)
                            .RowCast<ATPTEFMFundTransactionHistoryView>()
                            .FirstOrDefault();

                        if (transHistory != null)
                        {
                            transHistory.CheckNbr = doc.RefNbr;
                            transHistory.CuryCheckAmt = adjustment.CuryAdjgAmt;
                            transHistory.HasReplenishemtCheckNbr = true;
                            fundEntry.CurrentTransactionHistoryView.Update(transHistory);
                            fundEntry.Save.Press();

                            // Update Running Balance
                            //RunningBalance(fund, doc, adjustment, replenishment.ReplenishmentNbr);
                            OptimizedRunningBalance(fund, doc, adjustment, replenishment.ReplenishmentNbr);

                        }
                        else
                        {
                            var existingHistory = PXSelect<
                                ATPTEFMFundTransactionHistoryView,
                                Where<ATPTEFMFundTransactionHistoryView.refNbr, Equal<Required<ATPTEFMFundTransactionHistoryView.refNbr>>>>
                                .Select(Base, replenishment.ReplenishmentNbr)
                                .RowCast<ATPTEFMFundTransactionHistoryView>()
                                .FirstOrDefault();

                            if (existingHistory != null)
                            {
                                var newHistory = new ATPTEFMFundTransactionHistoryView
                                {
                                    FundRefNbr = replenishment.FundID,
                                    TransactionType = ATPTEFMFundMaint.ATPTEFMTransactionHistoryView.transactionType.Replenishment,
                                    OrderDate = replenishment.Date,
                                    RefNbr = replenishment.ReplenishmentNbr,
                                    FundBranchID = replenishment.BranchID,
                                    FundType = replenishment.FundType,
                                    TransactionDate = replenishment.Date,
                                    CuryFundTransactionDocumentAmt = replenishment.ClaimAmount,
                                    Status = replenishment.Status,
                                    Source = ATPTEFMFundMaint.ATPTEFMTransactionHistoryView.source.Replenishment,
                                    SortNbr = existingHistory.SortNbr
                                };

                                fundEntry.CurrentTransactionHistoryView.Insert(newHistory);
                                fundEntry.Save.Press();

                                OptimizedRunningBalance(fund, doc, adjustment, replenishment.ReplenishmentNbr);
                            }
                        }
                    }
                    fundEntry.Save.Press();
                }
            }
        }
        /*private void ReplenishmentPayment(APRegister doc)
        {
            PXResultset<APAdjust, APInvoice> adjustments = PXSelectJoinGroupBy<
                APAdjust,
                InnerJoin<APInvoice,
                    On<APInvoice.refNbr, Equal<APAdjust.adjdRefNbr>,
                    And<APInvoice.docType, Equal<APAdjust.adjdDocType>>>>,
                Where<APAdjust.adjgRefNbr, Equal<Required<APAdjust.adjgRefNbr>>,
                    And<APAdjust.voided, NotEqual<True>,
                    And<APAdjust.adjgDocType, NotEqual<APPaymentType.debitAdj>>>>,
                Aggregate<
                    GroupBy<APInvoice.docType,
                    GroupBy<APInvoice.refNbr>>>>
                .Select<PXResultset<APAdjust, APInvoice>>(Base, doc.RefNbr);
            int counter = 0;

            int totalReplenishmentBills = 0;
            foreach (PXResult<APAdjust, APInvoice> result in adjustments)
            {
                APInvoice invoice = result;
                ATPTEFMAPRegisterExt invExt = invoice.GetExtension<ATPTEFMAPRegisterExt>();

                if (invExt != null && invExt.UsrATPTEFMIsFromReplenishment == true)
                {
                    totalReplenishmentBills++;
                }
            }

            foreach (PXResult<APAdjust, APInvoice> result in adjustments)
            {
                APAdjust adjustment = result;
                APInvoice invoice = result;

                ATPTEFMReplenishment replenishment = PXSelect<
                    ATPTEFMReplenishment,
                    Where<ATPTEFMReplenishment.replenishmentNbr,
                        Equal<Required<ATPTEFMReplenishment.replenishmentNbr>>>>
                    .Select(Base, invoice.OrigRefNbr);

                if (replenishment != null)
                {
                    counter++;

                    ATPTEFMFundView.Current = PXSelect<
                        ATPTEFMFund,
                        Where<ATPTEFMFund.fundCD, Equal<Required<ATPTEFMFund.fundCD>>>>
                        .Select(Base, replenishment.FundID);

                    #region Summary Area
                    if (ATPTEFMFundView.Current != null)
                    {
                        ATPTEFMFundView.Current.CuryOnReplenishmentAmt -= adjustment.CuryAdjgAmt;
                        ATPTEFMFundView.Current.CuryBalanceAmt += adjustment.CuryAdjgAmt;
                        ATPTEFMFundView.UpdateCurrent();
                        if (counter == totalReplenishmentBills)
                            ATPTEFMFundView.Cache.Persist(PXDBOperation.Update);
                    }
                    #endregion

                    #region Transaction History
                    ATPTEFMTransactionHistoryView.Current = PXSelect<
                                                                            ATPTEFMFundTransactionHistoryView,
                                                                            Where<ATPTEFMFundTransactionHistoryView.refNbr, Equal<Required<ATPTEFMFundTransactionHistoryView.refNbr>>,
                                                                                And<ATPTEFMFundTransactionHistoryView.checkNbr, IsNull>>>
                                                                            .Select(Base, replenishment.ReplenishmentNbr)
                                                                            .FirstOrDefault();

                    if (ATPTEFMTransactionHistoryView.Current != null)
                    {
                        #region Add Check Nbr and Check Amount
                        ATPTEFMTransactionHistoryView.Current.CheckNbr = doc.RefNbr;
                        ATPTEFMTransactionHistoryView.Current.CuryCheckAmt = adjustment.CuryAdjgAmt;
                        ATPTEFMTransactionHistoryView.Current.HasReplenishmentCheckNbr = true;
                        ATPTEFMTransactionHistoryView.UpdateCurrent();
                        #endregion

                        RunningBalance(ATPTEFMFundView.Current, doc, adjustment, replenishment.ReplenishmentNbr);
                        if (counter == totalReplenishmentBills)
                            ATPTEFMTransactionHistoryView.Cache.Persist(PXDBOperation.Update);
                    }
                    else
                    {
                        ATPTEFMFundMaint fundEntry = PXGraph.CreateInstance<ATPTEFMFundMaint>();
                        ATPTEFMTransactionHistoryView.Current = PXSelect<
                            ATPTEFMFundTransactionHistoryView,
                            Where<ATPTEFMFundTransactionHistoryView.refNbr, Equal<Required<ATPTEFMFundTransactionHistoryView.refNbr>>>>
                            .Select(Base, replenishment.ReplenishmentNbr);
                        var sortNbr = ATPTEFMTransactionHistoryView.Current.SortNbr;

                        ATPTEFMFundTransactionHistoryView tranHistory = new ATPTEFMFundTransactionHistoryView();
                        tranHistory.FundRefNbr = replenishment.FundID;
                        tranHistory.TransactionType = ATPTEFMFundMaint.ATPTEFMTransactionHistoryView.transactionType.Replenishment;
                        tranHistory.OrderDate = replenishment.Date;
                        tranHistory.RefNbr = replenishment.ReplenishmentNbr;
                        tranHistory.FundBranchID = replenishment.BranchID;
                        tranHistory.FundType = replenishment.FundType;
                        tranHistory.TransactionDate = replenishment.Date;
                        tranHistory.CuryFundTransactionDocumentAmt = replenishment.ClaimAmount;
                        tranHistory.Status = replenishment.Status;
                        tranHistory.Source = ATPTEFMFundMaint.ATPTEFMTransactionHistoryView.source.Replenishment;
                        tranHistory.CheckNbr = doc.RefNbr;
                        tranHistory.CuryCheckAmt = adjustment.CuryAdjgAmt;
                        tranHistory.HasReplenishmentCheckNbr = true;
                        tranHistory.SortNbr = sortNbr;
                        fundEntry.CurrentTransactionHistoryView.Insert(tranHistory);
                        fundEntry.Save.Press();

                        var getTransactionHistory = PXSelect<
                            ATPTEFMFundTransactionHistoryView,
                            Where<ATPTEFMFundTransactionHistoryView.fundRefNbr, Equal<Required<ATPTEFMFundTransactionHistoryView.fundRefNbr>>>,
                            OrderBy<
                                Asc<ATPTEFMFundTransactionHistoryView.sortNbr>>>
                            .Select(Base, replenishment.FundID);

                        int startIndex = getTransactionHistory.RowCast<ATPTEFMFundTransactionHistoryView>().FindIndex(x => x.CheckNbr == doc.RefNbr);
                        ATPTEFMFundTransactionHistoryView prevRecord = getTransactionHistory.RowCast<ATPTEFMFundTransactionHistoryView>().TakeWhile(x => x.CheckNbr != doc.RefNbr).LastOrDefault();
                        int totalRows = getTransactionHistory.Count;

                        decimal? runningBalance = prevRecord.CuryBalanceAmt;

                        foreach (ATPTEFMFundTransactionHistoryView tran in PXSelect<
                            ATPTEFMFundTransactionHistoryView,
                            Where<ATPTEFMFundTransactionHistoryView.fundRefNbr, Equal<Required<ATPTEFMFundTransactionHistoryView.fundRefNbr>>>,
                            OrderBy<
                                Asc<ATPTEFMFundTransactionHistoryView.sortNbr>>>
                            .SelectWindowed(Base, startIndex, totalRows, replenishment.FundID))
                        {
                            ATPTEFMTransactionHistoryView.Current = PXSelect<
                                ATPTEFMFundTransactionHistoryView,
                                Where<ATPTEFMFundTransactionHistoryView.refNbr, Equal<Required<ATPTEFMFundTransactionHistoryView.refNbr>>>>
                                .Select(Base, tran.RefNbr);

                            if (ATPTEFMTransactionHistoryView.Current != null)
                            {
                                #region Fund Request
                                if (tran.TransactionType.Equals(ATPTEFMFundTransactionTypeAttribute.FundRequestValue))
                                {
                                    ATPTEFMFundTransaction currentFundTransaction = PXSelect<
                                        ATPTEFMFundTransaction,
                                        Where<ATPTEFMFundTransaction.refNbr, Equal<Required<ATPTEFMFundTransaction.refNbr>>>>
                                        .Select(Base, tran.RefNbr);

                                    if (currentFundTransaction != null)
                                    {
                                        switch (currentFundTransaction.CashAdvanceStatus)
                                        {
                                            case ATPTEFMFundTransactionCashAdvanceStatusAttribute.UnliquidatedValue:
                                                runningBalance -= currentFundTransaction.RequestedAmount;
                                                break;
                                            case ATPTEFMFundTransactionCashAdvanceStatusAttribute.ForReclassificationValue:
                                                runningBalance -= currentFundTransaction.RequestedAmount;
                                                runningBalance += currentFundTransaction.AmountReceived;
                                                break;
                                            case ATPTEFMFundTransactionCashAdvanceStatusAttribute.LiquidatedValue:
                                                runningBalance -= currentFundTransaction.ActualSpentAmount;
                                                runningBalance += currentFundTransaction.TotalWhtAmount;
                                                break;
                                            case ATPTEFMFundTransactionCashAdvanceStatusAttribute.UnreleasedValue:
                                                runningBalance += decimal.Zero;
                                                break;
                                            default:
                                                runningBalance += decimal.Zero;
                                                break;
                                        }
                                    }

                                    ATPTEFMTransactionHistoryView.Current.CuryBalanceAmt = runningBalance;
                                    ATPTEFMTransactionHistoryView.UpdateCurrent();
                                    continue;

                                }
                                #endregion

                                #region Fund Reimbursement
                                if (tran.TransactionType.Equals(ATPTEFMFundTransactionTypeAttribute.ReimbursementValue))
                                {
                                    ATPTEFMFundTransaction currentFundTransaction = PXSelect<
                                        ATPTEFMFundTransaction,
                                        Where<ATPTEFMFundTransaction.refNbr, Equal<Required<ATPTEFMFundTransaction.refNbr>>>>
                                        .Select(Base, tran.RefNbr);

                                    if (currentFundTransaction != null)
                                    {
                                        runningBalance -= currentFundTransaction.ActualSpentAmount;
                                        runningBalance += currentFundTransaction.TotalWhtAmount;
                                    }

                                    ATPTEFMTransactionHistoryView.Current.CuryBalanceAmt = runningBalance;
                                    ATPTEFMTransactionHistoryView.UpdateCurrent();
                                    continue;
                                }
                                #endregion

                                #region Replenishment
                                if (tran.TransactionType.Equals(ATPTEFMFundTransactionTypeAttribute.ReplenishmentValue))
                                {
                                    ATPTEFMTransactionHistoryCheckNbrView.Current = PXSelect<
                                        ATPTEFMFundTransactionHistoryView,
                                        Where<ATPTEFMFundTransactionHistoryView.checkNbr, Equal<Required<ATPTEFMFundTransactionHistoryView.checkNbr>>>>
                                        .Select(Base, doc.RefNbr);

                                    if (ATPTEFMTransactionHistoryCheckNbrView.Current != null && ATPTEFMTransactionHistoryCheckNbrView.Current.CheckNbr.Equals(doc.RefNbr))
                                    {
                                        runningBalance += ATPTEFMTransactionHistoryCheckNbrView.Current.CuryCheckAmt ?? decimal.Zero;
                                        ATPTEFMTransactionHistoryCheckNbrView.Current.CuryBalanceAmt = runningBalance;
                                        ATPTEFMTransactionHistoryCheckNbrView.UpdateCurrent();
                                    }
                                    else
                                    {
                                        runningBalance += ATPTEFMTransactionHistoryView.Current.CuryCheckAmt ?? decimal.Zero;
                                        ATPTEFMTransactionHistoryView.Current.CuryBalanceAmt = runningBalance;
                                        ATPTEFMTransactionHistoryView.UpdateCurrent();
                                    }
                                    continue;
                                }
                                #endregion

                                #region Receipts
                                if (tran.TransactionType.Equals(ATPTEFMFundTransactionTypeAttribute.ExpenseReceiptValue))
                                {
                                    ATPTEFMFundTransactionReceiptDetail receipt = PXSelect<
                                        ATPTEFMFundTransactionReceiptDetail,
                                        Where<ATPTEFMFundTransactionReceiptDetail.expenseReceiptRefNbr,
                                        Equal<Required<ATPTEFMFundTransactionReceiptDetail.expenseReceiptRefNbr>>>>
                                        .Select(Base, tran.RefNbr);

                                    if (receipt != null)
                                    {
                                        #region Get Fund Balance Amount
                                        decimal? fundBalanceAmount = runningBalance;
                                        ATPTEFMTransactionHistoryView.Current = ATPTEFMTransactionHistoryView.Select(receipt.FundTransactionRefNbr);
                                        if (ATPTEFMTransactionHistoryView.Current != null)
                                        {
                                            fundBalanceAmount = ATPTEFMTransactionHistoryView.Current.CuryBalanceAmt;
                                        }
                                        #endregion

                                        ATPTEFMTransactionHistoryView.Current = ATPTEFMTransactionHistoryView.Select(tran.RefNbr);
                                        ATPTEFMTransactionHistoryView.Current.CuryBalanceAmt = fundBalanceAmount;
                                        ATPTEFMTransactionHistoryView.UpdateCurrent();
                                        continue;
                                    }
                                }

                                #endregion

                                ATPTEFMTransactionHistoryView.Current.CuryBalanceAmt = runningBalance;
                                ATPTEFMTransactionHistoryView.UpdateCurrent();
                            }
                        }
                        ATPTEFMTransactionHistoryView.Cache.Persist(PXDBOperation.Update);

                    }
                    #endregion
                }
            }
        }*/
        private void ReplenishmentVoidPayment(APRegister doc)
        {

            PXResultset<APAdjust, APInvoice, ATPTEFMReplenishment> apDetails = PXSelectJoinGroupBy<
                APAdjust,
                InnerJoin<APInvoice,
                    On<APInvoice.refNbr, Equal<APAdjust.adjdRefNbr>,
                    And<APInvoice.docType, Equal<APAdjust.adjdDocType>>>,
                InnerJoin<ATPTEFMReplenishment,
                    On<ATPTEFMReplenishment.replenishmentNbr, Equal<APInvoice.origRefNbr>>>>,
                Where<APAdjust.adjgRefNbr, Equal<Required<APAdjust.adjgRefNbr>>,
                    And<APAdjust.voided, Equal<True>,
                    And<APAdjust.adjgDocType, NotEqual<APPaymentType.debitAdj>>>>,
                Aggregate<
                    GroupBy<APInvoice.docType,
                    GroupBy<APInvoice.refNbr>>>>
                .Select<PXResultset<APAdjust, APInvoice, ATPTEFMReplenishment>>(Base, doc.RefNbr);

            if (apDetails.Count > 0)
            {
                int billCount = 1;
                foreach (PXResult<APAdjust, APInvoice, ATPTEFMReplenishment> ap in apDetails)
                {
                    APAdjust apAdjust = ap;
                    ATPTEFMReplenishment replenishment = ap;

                    if (replenishment != null)
                    {
                        if (ATPTEFMFundView.Current == null)
                            ATPTEFMFundView.Current = ATPTEFMFundView.Select(replenishment.FundID);

                        #region Summary Area
                        if (ATPTEFMFundView.Current != null)
                        {
                            ATPTEFMFundView.Current.CuryOnReplenishmentAmt += Math.Abs((decimal)apAdjust.CuryAdjgAmt);
                            ATPTEFMFundView.Current.CuryBalanceAmt -= Math.Abs((decimal)apAdjust.CuryAdjgAmt);
                            ATPTEFMFundView.UpdateCurrent();

                            if (billCount == apDetails.Count)
                                ATPTEFMFundView.Cache.Persist(PXDBOperation.Update);
                        }
                        billCount++;
                        #endregion

                        #region Transaction History
                        ATPTEFMTransactionHistoryCheckNbrView.Current = ATPTEFMTransactionHistoryCheckNbrView.Select(doc.RefNbr);

                        if (ATPTEFMTransactionHistoryCheckNbrView.Current != null)
                        {
                            var getTransactionHistory = PXSelect<
                                ATPTEFMFundTransactionHistoryView,
                                Where<ATPTEFMFundTransactionHistoryView.fundRefNbr, Equal<Required<ATPTEFMFundTransactionHistoryView.fundRefNbr>>>,
                                OrderBy<
                                    Asc<ATPTEFMFundTransactionHistoryView.sortNbr>>>
                                .Select(Base, replenishment.FundID);

                            var getResult = ATPTEFMFundHistoryPaginationHelper.GetRecordPosition(getTransactionHistory.RowCast<ATPTEFMFundTransactionHistoryView>(), ATPTEFMTransactionHistoryCheckNbrView.Current.RefNbr);

                            var prevRecord = (ATPTEFMFundTransactionHistoryView)getResult.PreviousRecord;

                            ATPTEFMTransactionHistoryView.Current.CheckNbr = null;
                            ATPTEFMTransactionHistoryView.Current.CuryCheckAmt = null;
                            ATPTEFMTransactionHistoryView.Current.HasReplenishemtCheckNbr = false;
                            ATPTEFMTransactionHistoryView.Current.CuryBalanceAmt = prevRecord.CuryBalanceAmt;
                            ATPTEFMTransactionHistoryView.UpdateCurrent();
                            ATPTEFMTransactionHistoryView.Cache.Persist(PXDBOperation.Update);
                        }
                        #endregion
                    }
                }
            }
        }
        /// <remarks>
        /// 2025-01-15 : This will update the fund status to closed if fund debit adjustment balance equal to zero. Case: 007444 {JLTG} <br/>
        /// 2025-02-26 : The replenishment bill should be applied first to the debit adjustment (close fund) and the fund status should not be closed before the application of the refund. {JLTG}
        /// </remarks>
        private void ReplenishmentDebitAdjustment(APRegister doc)
        {
            foreach (PXResult<APAdjust, APInvoice, ATPTEFMReplenishment> ds in PXSelectJoinGroupBy<
                APAdjust,
                InnerJoin<APInvoice,
                    On<APInvoice.refNbr, Equal<APAdjust.adjdRefNbr>,
                    And<APInvoice.docType, Equal<APAdjust.adjdDocType>>>,
                InnerJoin<ATPTEFMReplenishment,
                    On<ATPTEFMReplenishment.replenishmentNbr, Equal<APInvoice.origRefNbr>>>>,
                Where<APAdjust.adjgRefNbr, Equal<Required<APAdjust.adjgRefNbr>>,
                    And<APAdjust.voided, NotEqual<True>,
                    And<APAdjust.adjgDocType, Equal<APPaymentType.debitAdj>>>>,
                Aggregate<
                    GroupBy<APInvoice.docType,
                    GroupBy<APInvoice.refNbr>>>>
                .Select<PXResultset<APAdjust, APInvoice, ATPTEFMReplenishment>>(Base, doc.RefNbr))
            {
                ATPTEFMReplenishment replenishment = ds;
                APInvoice aPInvoice = ds;
                APAdjust apAdjust = ds;

                ATPTEFMReplenishmentView.Current = ATPTEFMReplenishmentView.Select(aPInvoice.OrigRefNbr);

                if (ATPTEFMReplenishmentView.Current != null)
                {
                    ATPTEFMFund fund = PXSelect<
                        ATPTEFMFund,
                        Where<ATPTEFMFund.fundCD, Equal<Required<ATPTEFMFund.fundCD>>>>
                        .Select(Base, ATPTEFMReplenishmentView.Current.FundID);

                    if (fund.CloseFundRefNbr != doc.RefNbr)
                    {
                        foreach (ATPTEFMReplenishmentDetail item in ATPTEFMReplenishmentDetailView.Select(replenishment.ReplenishmentNbr))
                        {
                            PXResultset<ATPTEFMFundTransactionReceiptDetail, EPExpenseClaimDetails> details = PXSelectJoin<
                                ATPTEFMFundTransactionReceiptDetail,
                                InnerJoin<EPExpenseClaimDetails,
                                    On<EPExpenseClaimDetails.claimDetailCD, Equal<ATPTEFMFundTransactionReceiptDetail.expenseReceiptRefNbr>>>,
                                Where<ATPTEFMFundTransactionReceiptDetail.expenseReceiptRefNbr, Equal<Required<ATPTEFMFundTransactionReceiptDetail.expenseReceiptRefNbr>>>>
                                .Select<PXResultset<ATPTEFMFundTransactionReceiptDetail, EPExpenseClaimDetails>>(Base, item.ExpenseReceiptNbr);

                            ATPTEFMFundTransactionReceiptDetail ftReceipts = details;
                            EPExpenseClaimDetails claimDetails = details;

                            ATPTEFMFundTransactionReceiptDetailView.Current = ftReceipts;
                            if (ATPTEFMFundTransactionReceiptDetailView.Current != null)
                            {
                                ATPTEFMFundTransactionReceiptDetailView.Current.ReplenishmentRefNbr = null;
                                ATPTEFMFundTransactionReceiptDetailView.UpdateCurrent();
                                ATPTEFMFundTransactionReceiptDetailView.Cache.Persist(PXDBOperation.Update);
                            }

                            ATPTEFMExpenseReceipts.Current = claimDetails;
                            if (ATPTEFMExpenseReceipts.Current != null)
                            {
                                ATPTEFMExpenseReceipts.Current.Status = EPExpenseClaimDetailsStatus.ApprovedStatus;
                                ATPTEFMExpenseReceipts.UpdateCurrent();
                                ATPTEFMExpenseReceipts.Cache.Persist(PXDBOperation.Update);
                            }

                            ATPTEFMTransactionHistoryView.Current = PXSelect<
                                ATPTEFMFundTransactionHistoryView,
                                Where<ATPTEFMFundTransactionHistoryView.refNbr,
                                Equal<Required<ATPTEFMFundTransactionHistoryView.refNbr>>>>
                                .Select(Base, replenishment.ReplenishmentNbr);
                            if (ATPTEFMTransactionHistoryView.Current != null)
                            {
                                ATPTEFMTransactionHistoryView.Current.Status = ATPTEFMFundStatusAttribute.OpenValue;
                                ATPTEFMTransactionHistoryView.UpdateCurrent();
                                ATPTEFMTransactionHistoryView.Cache.Persist(PXDBOperation.Update);
                            }

                            ATPTEFMReplenishmentView.Current = replenishment;

                            foreach (ATPTEFMReplenishmentDetail detail in PXSelect<
                                ATPTEFMReplenishmentDetail,
                                Where<ATPTEFMReplenishmentDetail.invoiceRefNbr, Equal<Required<ATPTEFMReplenishmentDetail.invoiceRefNbr>>>>
                                .Select(Base, aPInvoice.RefNbr))
                            {
                                ATPTEFMReplenishmentAPDetailView.Current = detail;

                                if (ATPTEFMReplenishmentAPDetailView.Current != null)
                                {
                                    ATPTEFMReplenishmentAPDetailView.Current.InvoiceRefNbr = null;
                                    ATPTEFMReplenishmentAPDetailView.UpdateCurrent();
                                    ATPTEFMReplenishmentAPDetailView.Cache.Persist(PXDBOperation.Update);
                                }
                            }

                            if (ATPTEFMReplenishmentView.Current.Status != ATPTEFMReplenishmentStatusAttribute.OpenValue
                                && ATPTEFMReplenishmentView.Current.Step != ATPTEFMReplenishmentStepAttribute.SubmitReceiptValue
                                && ATPTEFMReplenishmentView.Current.IsReleased != false)
                            {
                                ATPTEFMReplenishmentView.Current.Status = ATPTEFMReplenishmentStatusAttribute.OpenValue;
                                ATPTEFMReplenishmentView.Current.Step = ATPTEFMReplenishmentStepAttribute.SubmitReceiptValue;
                                ATPTEFMReplenishmentView.Current.IsReleased = false;
                                ATPTEFMReplenishmentView.UpdateCurrent();
                                ATPTEFMReplenishmentView.Cache.Persist(PXDBOperation.Update);
                            }
                        }
                    }
                    else if (fund.CloseFundRefNbr == doc.RefNbr)
                    {
                        if (doc.CuryDocBal == decimal.Zero)
                        {
                            ATPTEFMTransactionHistoryView.Current = PXSelect<
                                ATPTEFMFundTransactionHistoryView,
                                Where<ATPTEFMFundTransactionHistoryView.refNbr, Equal<Required<ATPTEFMFundTransactionHistoryView.refNbr>>>>
                                .Select(Base, doc.RefNbr);

                            if (ATPTEFMTransactionHistoryView.Current != null)
                            {
                                ATPTEFMFundView.Current = ATPTEFMFundView.Select(ATPTEFMTransactionHistoryView.Current.FundRefNbr);

                                if (ATPTEFMFundView.Current != null)
                                {
                                    #region Update Fund Transaction History
                                    ATPTEFMTransactionHistoryView.Current.Status = APDocStatus.Closed;
                                    ATPTEFMTransactionHistoryView.UpdateCurrent();
                                    ATPTEFMTransactionHistoryView.Cache.Persist(PXDBOperation.Update);
                                    #endregion

                                    #region Update Fund Summary On hand
                                    ATPTEFMFundView.Current.BalanceAmt = decimal.Zero;
                                    ATPTEFMFundView.Current.Status = ATPTEFMFundStatusAttribute.ClosedValue;
                                    ATPTEFMFundView.Current.Closed = true;
                                    ATPTEFMFundView.UpdateCurrent();
                                    ATPTEFMFundView.Cache.Persist(PXDBOperation.Update);
                                    #endregion
                                }
                            }
                        }
                    }
                }
            }
        }
        private void ClearInvoiceFieldsForReplenishmentER(APRegister doc)
        {
            ExpenseClaimDetailEntry erGraph = PXGraph.CreateInstance<ExpenseClaimDetailEntry>();

            var receiptsToClear = PXSelect<EPExpenseClaimDetails, 
                Where<EPExpenseClaimDetails.aPRefNbr, Equal<@P.AsString>,
                And<EPExpenseClaimDetails.aPDocType, Equal<@P.AsString>>>>.Select(Base, doc.OrigRefNbr, doc.OrigDocType);

            foreach (EPExpenseClaimDetails er in receiptsToClear)
            {
                erGraph.Clear();
                erGraph.ClaimDetails.Current = er;

                er.APDocType = null;
                er.APRefNbr = null;
                er.APLineNbr = null;

                erGraph.ClaimDetails.Update(er);
                erGraph.Save.Press();
            }
        }
        #endregion

        #region Close Fund
        /// <summary>
        /// Processes the closing of funds when a payment is released.
        /// </summary>
        /// <param name="doc">The AP Register document being processed</param>
        /// <remarks>
        /// 2024-02-13 : Optimized CloseFund method CASE: 009801 {JLTG}
        /// Changes made:
        /// 1. Replaced direct Cache.Persist with graph instance Save.Press()
        /// 2. Improved transaction handling using proper graph instance
        /// 3. Better null checking and LINQ usage
        /// 4. Removed counter logic for more reliable saving
        /// </remarks>
        private void CloseFund(APRegister doc)
        {
            ATPTEFMAPRegisterExt apRegisterExt = doc.GetExtension<ATPTEFMAPRegisterExt>();

            PXResultset<APAdjust, APInvoice> adjustments = PXSelectJoinGroupBy<
                APAdjust,
                InnerJoin<APInvoice,
                    On<APInvoice.refNbr, Equal<APAdjust.adjdRefNbr>>>,
                Where<APAdjust.adjgRefNbr, Equal<Required<APAdjust.adjgRefNbr>>,
                    And<APAdjust.voided, NotEqual<True>,
                    And<APAdjust.adjdDocType, Equal<APPaymentType.debitAdj>>>>,
                Aggregate<
                    GroupBy<APInvoice.docType,
                    GroupBy<APInvoice.refNbr>>>>
                .Select<PXResultset<APAdjust, APInvoice>>(Base, doc.RefNbr);

            foreach (PXResult<APAdjust, APInvoice> result in adjustments)
            {
                APAdjust apAdjust = result;
                APInvoice apInvoice = result;

                var fundEntry = PXGraph.CreateInstance<ATPTEFMFundMaint>();

                var transHistory = PXSelect<
                    ATPTEFMFundTransactionHistoryView,
                    Where<ATPTEFMFundTransactionHistoryView.refNbr,
                        Equal<Required<ATPTEFMFundTransactionHistoryView.refNbr>>>>
                    .Select(Base, apAdjust.AdjdRefNbr)
                    .RowCast<ATPTEFMFundTransactionHistoryView>()
                    .FirstOrDefault();

                if (transHistory?.TransactionType == ATPTEFMFundMaint.ATPTEFMTransactionHistoryView.transactionType.CloseFund)
                {
                    var fund = PXSelect<
                        ATPTEFMFund,
                        Where<ATPTEFMFund.fundCD,
                            Equal<Required<ATPTEFMFund.fundCD>>>>
                        .Select(Base, transHistory.FundRefNbr)
                        .RowCast<ATPTEFMFund>()
                        .FirstOrDefault();

                    if (fund != null)
                    {
                        transHistory.Status = APDocStatus.Closed;
                        transHistory.CuryBalanceAmt = apInvoice.CuryDocBal;
                        transHistory.CheckNbr = doc.RefNbr;
                        transHistory.CuryCheckAmt = apAdjust.CuryAdjgAmt;
                        if (apInvoice.CuryDocBal == decimal.Zero)
                        {
                            transHistory.Status = APDocStatus.Closed;
                        }
                        fundEntry.CurrentTransactionHistoryView.Update(transHistory);

                        fund.CuryBalanceAmt = decimal.Zero;
                        if (apInvoice.CuryDocBal == decimal.Zero)
                        {
                            fund.Status = ATPTEFMFundStatusAttribute.ClosedValue;
                            fund.Closed = true;
                        }
                        fundEntry.Document.Update(fund);

                        fundEntry.Save.Press();
                    }
                }
            }
        }
        #endregion

        #region OptimizedRunningBalance
        /// <summary>
        /// Calculates and updates running balances for fund transactions in an optimized way.
        /// 
        /// Flow:
        /// 1. Creates fund maintenance graph instance
        /// 2. Gets transaction history ordered by sort number
        /// 3. Finds starting position based on pagination
        /// 4. Processes transactions sequentially:
        ///    - Fund Request: Deducts from balance
        ///    - Reimbursement: Gets previous balance
        ///    - Replenishment: Adds check amount
        ///    - Expense Receipt: Gets linked transaction balance
        ///    - Increase Fund: Adds adjustment amount
        /// 
        /// Purpose:
        /// - Maintains accurate running balances
        /// - Handles different transaction types
        /// - Ensures data consistency
        /// - Optimizes database operations
        /// 
        /// Business Rules:
        /// - Each transaction type affects balance differently
        /// - Balance calculations must be sequential
        /// - Previous balances must be considered
        /// - Single save operation at the end
        /// 
        /// Transaction Types:
        /// - Fund Request: Decreases balance
        /// - Reimbursement: Uses previous balance
        /// - Replenishment: Increases balance
        /// - Expense Receipt: Links to original transaction
        /// - Increase Fund: Increases balance
        /// </summary>
        /// <param name="fund">Fund record being processed</param>
        /// <param name="doc">AP Register document</param>
        /// <param name="apAdjust">AP Adjustment record</param>
        /// <param name="paginationRefNbr">Reference number for pagination</param>
        private void OptimizedRunningBalance(ATPTEFMFund fund, APRegister doc, APAdjust apAdjust, string paginationRefNbr)
        {
            var fundMaint = PXGraph.CreateInstance<ATPTEFMFundMaint>();

            fundMaint.Document.Current = fund;

            if (fundMaint.Document.Current == null) return;

            var transactionHistory = PXSelect<
                ATPTEFMFundTransactionHistoryView,
                Where<ATPTEFMFundTransactionHistoryView.fundRefNbr, Equal<Required<ATPTEFMFundTransactionHistoryView.fundRefNbr>>>,
                OrderBy<
                    Asc<ATPTEFMFundTransactionHistoryView.sortNbr>>>
                .Select(Base, fund.FundCD)
                .RowCast<ATPTEFMFundTransactionHistoryView>()
                .ToList();

            var getResult = ATPTEFMFundHistoryPaginationHelper.GetRecordPosition(transactionHistory, paginationRefNbr);
            var prevRecord = (ATPTEFMFundTransactionHistoryView)getResult.PreviousRecord;
            decimal? runningBalance = prevRecord != null ? prevRecord.CuryBalanceAmt : 0m;

            var recordsToProcess = transactionHistory
                .Skip(getResult.StartIndex)
                .Take(getResult.TotalRows - getResult.StartIndex);

            foreach (var tran in recordsToProcess)
            {
                ATPTEFMFundTransactionHistoryView currentTransaction = PXSelect<
                    ATPTEFMFundTransactionHistoryView,
                    Where<ATPTEFMFundTransactionHistoryView.refNbr,
               Equal<Required<ATPTEFMFundTransactionHistoryView.refNbr>>>>
                    .Select(Base, tran.RefNbr);

                if (currentTransaction == null) continue;

                fundMaint.CurrentTransactionHistoryView.Current = currentTransaction;

                switch (tran.TransactionType)
                {
                    case ATPTEFMFundTransactionTypeAttribute.FundRequestValue:
                        runningBalance = ProcessFundRequest(fundMaint, tran, runningBalance);
                        fundMaint.CurrentTransactionHistoryView.Current.CuryBalanceAmt = runningBalance;
                        fundMaint.CurrentTransactionHistoryView.Update(fundMaint.CurrentTransactionHistoryView.Current);
                        break;

                    case ATPTEFMFundTransactionTypeAttribute.ReimbursementValue:
                        runningBalance = ProcessReimbursement(fundMaint, tran, runningBalance);
                        fundMaint.CurrentTransactionHistoryView.Current.CuryBalanceAmt = runningBalance;
                        fundMaint.CurrentTransactionHistoryView.Update(fundMaint.CurrentTransactionHistoryView.Current);
                        break;

                    case ATPTEFMFundTransactionTypeAttribute.ReplenishmentValue:
                        if (tran.RefNbr.Equals(paginationRefNbr))
                        {
                            var currentReplenishment = PXSelect<
                                ATPTEFMFundTransactionHistoryView,
                                Where<ATPTEFMFundTransactionHistoryView.refNbr, Equal<Required<ATPTEFMFundTransactionHistoryView.refNbr>>,
                                    And<ATPTEFMFundTransactionHistoryView.checkNbr, Equal<Required<ATPTEFMFundTransactionHistoryView.checkNbr>>>>>
                                .Select(Base, paginationRefNbr, tran.CheckNbr)
                                .RowCast<ATPTEFMFundTransactionHistoryView>()
                                .FirstOrDefault();

                            fundMaint.CurrentTransactionHistoryView.Current = currentReplenishment ??
                                GetEmptyCheckReplenishment(fundMaint, paginationRefNbr);

                            if (string.IsNullOrEmpty(fundMaint.CurrentTransactionHistoryView.Current.CheckNbr))
                            {
                                fundMaint.CurrentTransactionHistoryView.Current.CheckNbr = doc.RefNbr;
                                fundMaint.CurrentTransactionHistoryView.Current.CuryCheckAmt = apAdjust.CuryAdjgAmt;
                                fundMaint.CurrentTransactionHistoryView.Current.HasReplenishemtCheckNbr = true;
                                fundMaint.CurrentTransactionHistoryView.Update(fundMaint.CurrentTransactionHistoryView.Current);
                            }

                        }
                        runningBalance += tran.CuryCheckAmt ?? 0m;
                        fundMaint.CurrentTransactionHistoryView.Current.CuryBalanceAmt = runningBalance;
                        fundMaint.CurrentTransactionHistoryView.Update(fundMaint.CurrentTransactionHistoryView.Current);
                        break;

                    case ATPTEFMFundTransactionTypeAttribute.ExpenseReceiptValue:
                        runningBalance = ProcessExpenseReceipt(fundMaint, tran, runningBalance);
                        fundMaint.CurrentTransactionHistoryView.Current.CuryBalanceAmt = runningBalance;
                        fundMaint.CurrentTransactionHistoryView.Update(fundMaint.CurrentTransactionHistoryView.Current);
                        break;

                    case ATPTEFMFundTransactionTypeAttribute.IncreaseFundValue:
                        runningBalance += apAdjust.CuryAdjgAmt;
                        fundMaint.CurrentTransactionHistoryView.Current.CuryBalanceAmt = runningBalance;
                        fundMaint.CurrentTransactionHistoryView.Update(fundMaint.CurrentTransactionHistoryView.Current);
                        break;
                }
            }

            if (fundMaint.CurrentTransactionHistoryView.Cache.IsDirty)
            {
                fundMaint.Save.Press();
            }
        }

        private decimal? ProcessFundRequest(ATPTEFMFundMaint fundMaint, ATPTEFMFundTransactionHistoryView tran, decimal? runningBalance)
        {
            ATPTEFMFundTransaction fundTransaction = ATPTEFMFundTransactionView.Select(tran.RefNbr);
            if (fundTransaction == null) return runningBalance;

            switch (fundTransaction.CashAdvanceStatus)
            {
                case ATPTEFMFundTransactionCashAdvanceStatusAttribute.UnliquidatedValue:
                    return runningBalance - fundTransaction.RequestedAmount;

                case ATPTEFMFundTransactionCashAdvanceStatusAttribute.ForReclassificationValue:
                    return runningBalance - fundTransaction.RequestedAmount + fundTransaction.AmountReceived;

                case ATPTEFMFundTransactionCashAdvanceStatusAttribute.LiquidatedValue:
                    return runningBalance - fundTransaction.ActualSpentAmount
                           + fundTransaction.TotalWhtAmount
                           - fundTransaction.ReclassificationAmt;

                case ATPTEFMFundTransactionCashAdvanceStatusAttribute.UnreleasedValue:
                default:
                    return runningBalance;
            }
        }

        private decimal? ProcessReimbursement(ATPTEFMFundMaint fundMaint, ATPTEFMFundTransactionHistoryView tran, decimal? runningBalance)
        {
            ATPTEFMFundTransaction fundTransaction = ATPTEFMFundTransactionView.Select(tran.RefNbr);
            if (fundTransaction == null) return runningBalance;

            return runningBalance - fundTransaction.ActualSpentAmount + fundTransaction.TotalWhtAmount;
        }

        private ATPTEFMFundTransactionHistoryView GetEmptyCheckReplenishment(ATPTEFMFundMaint fundEntry, string paginationRefNbr)
        {
            return PXSelect<
                ATPTEFMFundTransactionHistoryView,
                Where<ATPTEFMFundTransactionHistoryView.refNbr, Equal<Required<ATPTEFMFundTransactionHistoryView.refNbr>>,
                    And<ATPTEFMFundTransactionHistoryView.checkNbr, IsNull>>>
                .Select(Base, paginationRefNbr)
                .RowCast<ATPTEFMFundTransactionHistoryView>()
                .FirstOrDefault();
        }
        private decimal? ProcessExpenseReceipt(ATPTEFMFundMaint fundMaint, ATPTEFMFundTransactionHistoryView tran, decimal? runningBalance)
        {
            ATPTEFMFundTransactionReceiptDetail receipt = ATPTEFMFundTransactionReceiptDetailView.Select(tran.RefNbr);
            if (receipt == null) return runningBalance;

            ATPTEFMFundTransactionHistoryView fundTransaction = PXSelect<
                ATPTEFMFundTransactionHistoryView,
                Where<ATPTEFMFundTransactionHistoryView.refNbr, Equal<Required<ATPTEFMFundTransactionHistoryView.refNbr>>,
                    And<ATPTEFMFundTransactionHistoryView.transactionType, Equal<ATPTEFMFundTransactionTypeAttribute.fundRequest>>>>
                .Select(Base, receipt.FundTransactionRefNbr);

            return fundTransaction?.CuryBalanceAmt ?? runningBalance;
        }

        #endregion

        #region Running Balance

        private void RunningBalance(ATPTEFMFund fund, APRegister doc, APAdjust apAdjust, string paginationRefNbr)
        {
            #region Update Running Balance                  
            var getTransactionHistory =
            PXSelect<
                ATPTEFMFundTransactionHistoryView,
                Where<ATPTEFMFundTransactionHistoryView.fundRefNbr, Equal<Required<ATPTEFMFundTransactionHistoryView.fundRefNbr>>>,
                OrderBy<
                    Asc<ATPTEFMFundTransactionHistoryView.sortNbr>>>
                .Select(Base, fund.FundCD);

            var getResult = ATPTEFMFundHistoryPaginationHelper.GetRecordPosition(getTransactionHistory.RowCast<ATPTEFMFundTransactionHistoryView>(), paginationRefNbr);

            int startIndex = getResult.StartIndex;
            int totalRows = getResult.TotalRows;

            var prevRecord = (ATPTEFMFundTransactionHistoryView)getResult.PreviousRecord;

            decimal? runningBalance = prevRecord.CuryBalanceAmt;

            foreach (ATPTEFMFundTransactionHistoryView tran in PXSelect<
                ATPTEFMFundTransactionHistoryView,
                Where<ATPTEFMFundTransactionHistoryView.fundRefNbr, Equal<Required<ATPTEFMFundTransactionHistoryView.fundRefNbr>>>,
                OrderBy<
                    Asc<ATPTEFMFundTransactionHistoryView.sortNbr>>>
                .SelectWindowed(Base, startIndex, totalRows, fund.FundCD))
            {
                ATPTEFMTransactionHistoryView.Current = ATPTEFMTransactionHistoryView.Select(tran.RefNbr);

                if (ATPTEFMTransactionHistoryView.Current != null)
                {
                    #region Fund Request
                    if (tran.TransactionType.Equals(ATPTEFMFundTransactionTypeAttribute.FundRequestValue))
                    {
                        ATPTEFMFundTransaction currentFundTransaction = ATPTEFMFundTransactionView.Select(tran.RefNbr);

                        if (currentFundTransaction != null)
                        {
                            switch (currentFundTransaction.CashAdvanceStatus)
                            {
                                case ATPTEFMFundTransactionCashAdvanceStatusAttribute.UnliquidatedValue:
                                    runningBalance -= currentFundTransaction.RequestedAmount;
                                    break;
                                case ATPTEFMFundTransactionCashAdvanceStatusAttribute.ForReclassificationValue:
                                    runningBalance -= currentFundTransaction.RequestedAmount;
                                    runningBalance += currentFundTransaction.AmountReceived;
                                    break;
                                case ATPTEFMFundTransactionCashAdvanceStatusAttribute.LiquidatedValue:
                                    runningBalance -= currentFundTransaction.ActualSpentAmount;
                                    runningBalance += currentFundTransaction.TotalWhtAmount;
                                    runningBalance -= currentFundTransaction.ReclassificationAmt;
                                    break;
                                case ATPTEFMFundTransactionCashAdvanceStatusAttribute.UnreleasedValue:
                                    runningBalance += decimal.Zero;
                                    break;
                                default:
                                    runningBalance += decimal.Zero;
                                    break;
                            }
                        }

                        ATPTEFMTransactionHistoryView.Current.CuryBalanceAmt = runningBalance;
                        ATPTEFMTransactionHistoryView.UpdateCurrent();
                        continue;

                    }
                    #endregion

                    #region Fund Reimbursement
                    if (tran.TransactionType.Equals(ATPTEFMFundTransactionTypeAttribute.ReimbursementValue))
                    {
                        ATPTEFMFundTransaction currentFundTransaction = ATPTEFMFundTransactionView.Select(tran.RefNbr);

                        if (currentFundTransaction != null)
                        {
                            runningBalance -= currentFundTransaction.ActualSpentAmount;
                            runningBalance += currentFundTransaction.TotalWhtAmount;
                        }

                        ATPTEFMTransactionHistoryView.Current.CuryBalanceAmt = runningBalance;
                        ATPTEFMTransactionHistoryView.UpdateCurrent();
                        continue;
                    }
                    #endregion

                    #region Replenishment
                    if (tran.TransactionType.Equals(ATPTEFMFundTransactionTypeAttribute.ReplenishmentValue))
                    {
                        if (tran.RefNbr.Equals(paginationRefNbr))
                        {
                            ATPTEFMFundTransactionHistoryView currentReplenishment = PXSelect<
                                ATPTEFMFundTransactionHistoryView,
                                Where<ATPTEFMFundTransactionHistoryView.refNbr, Equal<Required<ATPTEFMFundTransactionHistoryView.refNbr>>,
                                    And<ATPTEFMFundTransactionHistoryView.checkNbr, Equal<Required<ATPTEFMFundTransactionHistoryView.checkNbr>>>>>
                                .Select(Base, paginationRefNbr, tran.CheckNbr);

                            if (currentReplenishment != null)
                            {
                                ATPTEFMTransactionHistoryView.Current = currentReplenishment;
                            }
                            else
                            {
                                ATPTEFMFundTransactionHistoryView currentReplenishmentEmptyCheck = PXSelect<
                                    ATPTEFMFundTransactionHistoryView,
                                    Where<ATPTEFMFundTransactionHistoryView.refNbr, Equal<Required<ATPTEFMFundTransactionHistoryView.refNbr>>,
                                        And<ATPTEFMFundTransactionHistoryView.checkNbr, IsNull>>>
                                    .Select(Base, paginationRefNbr);

                                if (currentReplenishmentEmptyCheck != null)
                                {
                                    ATPTEFMTransactionHistoryView.Current = currentReplenishmentEmptyCheck;
                                }
                            }
                        }

                        runningBalance += ATPTEFMTransactionHistoryCheckNbrView.Current.CuryCheckAmt ?? decimal.Zero;
                        ATPTEFMTransactionHistoryView.Current.CuryBalanceAmt = runningBalance;
                        ATPTEFMTransactionHistoryView.UpdateCurrent();
                        continue;
                    }
                    #endregion

                    #region Receipts
                    if (tran.TransactionType.Equals(ATPTEFMFundTransactionTypeAttribute.ExpenseReceiptValue))
                    {
                        ATPTEFMFundTransactionReceiptDetail receipt = PXSelect<
                            ATPTEFMFundTransactionReceiptDetail,
                            Where<ATPTEFMFundTransactionReceiptDetail.expenseReceiptRefNbr,
                            Equal<Required<ATPTEFMFundTransactionReceiptDetail.expenseReceiptRefNbr>>>>
                            .Select(Base, tran.RefNbr);

                        if (receipt != null)
                        {
                            #region Get Fund Balance Amount
                            decimal? fundBalanceAmount = runningBalance;
                            ATPTEFMTransactionHistoryView.Current = ATPTEFMTransactionHistoryView.Select(receipt.FundTransactionRefNbr);
                            if (ATPTEFMTransactionHistoryView.Current != null)
                            {
                                fundBalanceAmount = ATPTEFMTransactionHistoryView.Current.CuryBalanceAmt;
                            }
                            #endregion

                            ATPTEFMTransactionHistoryView.Current = ATPTEFMTransactionHistoryView.Select(tran.RefNbr);
                            ATPTEFMTransactionHistoryView.Current.CuryBalanceAmt = fundBalanceAmount;
                            ATPTEFMTransactionHistoryView.UpdateCurrent();
                            continue;
                        }
                    }

                    #endregion

                    #region IncreaseFund
                    if (tran.TransactionType.Equals(ATPTEFMFundTransactionTypeAttribute.IncreaseFundValue))
                    {
                        runningBalance += apAdjust.CuryAdjgAmt;
                        ATPTEFMTransactionHistoryView.Current.CuryBalanceAmt = runningBalance;
                        ATPTEFMTransactionHistoryView.UpdateCurrent();
                        continue;
                    }
                    #endregion

                    ATPTEFMTransactionHistoryView.Current.CuryBalanceAmt = runningBalance;
                    ATPTEFMTransactionHistoryView.UpdateCurrent();
                }
            }
            #endregion
        }
        #endregion
        private ATPTEFMFund UpdateFundSettings(ATPTEFMFund fund, decimal? fundAmt)
        {
            #region Replenishment
            if (fund.ReplenishmentLimit == ATPTEFMReplenishmentStringListAttribute.PercentValue)
            {
                fund.ReplenishmentAmt = fundAmt * (fund.ReplenishPointPercent / 100);
            }
            else if (fund.ReplenishmentLimit == ATPTEFMReplenishmentStringListAttribute.AmountValue)
            {
                fund.ReplenishPointPercent = (fund.ReplenishmentAmt / fundAmt) * 100;
            }
            #endregion
            #region Fund Transaction
            if (fund.FundTransactionLimit == ATPTEFMReplenishmentStringListAttribute.PercentValue)
            {
                fund.FundTransactionAmt = fundAmt * (fund.FundTransactionPointPercent / 100);
            }
            else if (fund.FundTransactionLimit == ATPTEFMReplenishmentStringListAttribute.AmountValue)
            {
                fund.FundTransactionPointPercent = (fund.FundTransactionAmt / fundAmt) * 100;
            }
            #endregion
            return fund;
        }
        #region Increase/Decrease Fund
        /// <summary>
        /// Processes Fund Increase transactions when a payment is released.
        /// 
        /// Purpose:
        /// - Handles the release of payments for Fund Increase transactions
        /// - Updates both Transaction History and Fund Summary records
        /// 
        /// Flow:
        /// 1. Gets all adjustments for the payment that are:
        ///    - Not voided
        ///    - Not debit adjustments
        ///    - Related to credit adjustments
        /// 
        /// 2. For each adjustment:
        ///    - Gets related Transaction History record
        ///    - Verifies if it's an Increase Fund transaction
        ///    - Gets related Fund record
        /// 
        /// 3. Updates Transaction History:
        ///    - Sets status to Closed
        ///    - Records check number and amount
        ///    - Updates running balance
        ///    - Persists changes
        /// 
        /// 4. Updates Fund Summary:
        ///    - Increases fund amount
        ///    - Updates balance amount
        ///    - Persists changes
        ///    
        /// </summary>
        /// 
        /// <remarks>
        /// 009723 - URGENT: After increasing fund amount, the setting did not update. Cannot issue amount in Fund Transaction due to amount limitation
        /// 2025-04-03: Wrong amount used in computing new Replenishment/Fund transaction limit. - 009723 - RFS
        /// 2025-08-28:  Increase Fund Balance Amount (Incorrect) CASE: 012708 {JLTG}
        /// </remarks>
        private void IncreaseFundPaymentRelease(APRegister doc)
        {
            PXResultset<APAdjust> adjustments = PXSelect<
                APAdjust,
                Where<APAdjust.adjgRefNbr, Equal<Required<APAdjust.adjgRefNbr>>,
                    And<APAdjust.voided, NotEqual<True>,
                    And<APAdjust.adjgDocType, NotEqual<APPaymentType.debitAdj>,
                    And<APAdjust.adjdDocType, Equal<APDocType.creditAdj>>>>>>
                .Select(Base, doc.RefNbr);

            int totalBillcounter = 0;
            foreach (APAdjust adj in adjustments)
            {
                ATPTEFMTransactionHistoryView.Current = PXSelect<
                    ATPTEFMFundTransactionHistoryView,
                    Where<ATPTEFMFundTransactionHistoryView.refNbr, Equal<Required<ATPTEFMFundTransactionHistoryView.refNbr>>>>
                    .Select(Base, adj.AdjdRefNbr);

                #region Transaction History Establishment
                if (ATPTEFMTransactionHistoryView.Current != null && ATPTEFMTransactionHistoryView.Current.TransactionType == CashFundManagement.BLC.ATPTEFMFundMaint.ATPTEFMTransactionHistoryView.transactionType.IncreaseFund)
                {
                    totalBillcounter++;
                    ATPTEFMFundView.Current = PXSelect<
                        ATPTEFMFund,
                        Where<ATPTEFMFund.fundCD,
                       Equal<Required<ATPTEFMFund.fundCD>>>>
                        .Select(Base, ATPTEFMTransactionHistoryView.Current.FundRefNbr);

                    if (ATPTEFMFundView.Current != null)
                    {
                        ATPTEFMTransactionHistoryView.Current.Status = APDocStatus.Closed;
                        ATPTEFMTransactionHistoryView.Current.CheckNbr = doc.RefNbr;
                        ATPTEFMTransactionHistoryView.Current.CuryCheckAmt = adj.CuryAdjgAmt;
                        ATPTEFMTransactionHistoryView.UpdateCurrent();
                        OptimizedRunningBalance(ATPTEFMFundView.Current, doc, adj, ATPTEFMTransactionHistoryView.Current.RefNbr);
                        ATPTEFMTransactionHistoryView.Cache.Persist(PXDBOperation.Update);

                        #region Update Fund Summary
                        ATPTEFMFundView.Current.CuryFundAmt += adj.CuryAdjgAmt;
                        ATPTEFMFundView.Current.CuryBalanceAmt += adj.CuryAdjgAmt;
                        ATPTEFMFundView.Current = UpdateFundSettings(ATPTEFMFundView.Current, ATPTEFMFundView.Current.CuryFundAmt);
                        ATPTEFMFundView.UpdateCurrent();
                        if (totalBillcounter == adjustments.Count)
                            ATPTEFMFundView.Cache.Persist(PXDBOperation.Update);
                        #endregion
                    }
                }
                #endregion

            }
        }
        private void IncreaseFundReversed(APRegister doc)
        {
            APAdjust apAdjust = PXSelect<
                APAdjust,
                Where<APAdjust.adjgRefNbr, Equal<Required<APAdjust.adjgRefNbr>>,
                    And<APAdjust.voided, NotEqual<True>,
                    And<APAdjust.adjgDocType, Equal<APPaymentType.debitAdj>,
                    And<APAdjust.adjdDocType, Equal<APDocType.creditAdj>>>>>>
                .Select(Base, doc.RefNbr);
            if (apAdjust != null)
            {
                ATPTEFMTransactionHistoryView.Current = ATPTEFMTransactionHistoryView.Select(apAdjust.AdjdRefNbr);

                #region Transaction History Establishment
                if (ATPTEFMTransactionHistoryView.Current != null && ATPTEFMTransactionHistoryView.Current.TransactionType == CashFundManagement.BLC.ATPTEFMFundMaint.ATPTEFMTransactionHistoryView.transactionType.IncreaseFund)
                {
                    ATPTEFMTransactionHistoryView.Current.Status = ATPTEFMFundStatusAttribute.ReversedValue;
                    ATPTEFMTransactionHistoryView.UpdateCurrent();
                    ATPTEFMTransactionHistoryView.Cache.Persist(PXDBOperation.Update);
                }
                #endregion
            }
        }
        private void DecreaseFundRefundRelease(APRegister doc)
        {
            PXResultset<APAdjust> adjustments = PXSelect<
                APAdjust,
                Where<APAdjust.adjgRefNbr, Equal<Required<APAdjust.adjgRefNbr>>,
                    And<APAdjust.voided, NotEqual<True>,
                    And<APAdjust.adjgDocType, NotEqual<APPaymentType.debitAdj>,
                    And<APAdjust.adjdDocType, Equal<APDocType.debitAdj>>>>>>
                .Select(Base, doc.RefNbr);


            int totalBillcounter = 0;
            foreach (APAdjust adj in adjustments)
            {

                ATPTEFMTransactionHistoryView.Current = PXSelect<
                    ATPTEFMFundTransactionHistoryView,
                    Where<ATPTEFMFundTransactionHistoryView.refNbr, Equal<Required<ATPTEFMFundTransactionHistoryView.refNbr>>>>
                    .Select(Base, adj.AdjdRefNbr);

                #region Transaction History Establishment
                if (ATPTEFMTransactionHistoryView.Current != null && ATPTEFMTransactionHistoryView.Current.TransactionType == CashFundManagement.BLC.ATPTEFMFundMaint.ATPTEFMTransactionHistoryView.transactionType.DecreaseFund)
                {
                    totalBillcounter++;
                    ATPTEFMFundView.Current = PXSelect<
                        ATPTEFMFund,
                        Where<ATPTEFMFund.fundCD,
                        Equal<Required<ATPTEFMFund.fundCD>>>>
                        .Select(Base, ATPTEFMTransactionHistoryView.Current.FundRefNbr);

                    if (ATPTEFMFundView.Current != null)
                    {
                        ATPTEFMTransactionHistoryView.Current.Status = APDocStatus.Closed;
                        ATPTEFMTransactionHistoryView.Current.CuryBalanceAmt -= adj.CuryAdjgAmt;
                        ATPTEFMTransactionHistoryView.Current.CheckNbr = doc.RefNbr;
                        ATPTEFMTransactionHistoryView.Current.CuryCheckAmt = adj.CuryAdjgAmt;
                        ATPTEFMTransactionHistoryView.UpdateCurrent();
                        if (totalBillcounter == adjustments.Count)
                            ATPTEFMTransactionHistoryView.Cache.Persist(PXDBOperation.Update);

                        #region Update Fund Summary
                        ATPTEFMFundView.Current.CuryFundAmt -= adj.CuryAdjgAmt;
                        ATPTEFMFundView.Current.CuryBalanceAmt -= adj.CuryAdjgAmt;
                        ATPTEFMFundView.Current = UpdateFundSettings(ATPTEFMFundView.Current, ATPTEFMFundView.Current.CuryFundAmt);
                        ATPTEFMFundView.UpdateCurrent();
                        if (totalBillcounter == adjustments.Count)
                            ATPTEFMFundView.Cache.Persist(PXDBOperation.Update);
                        #endregion
                    }
                }
                #endregion
            }
        }
        private void DecreaseFundReversed(APRegister doc)
        {
            ATPTEFMTransactionHistoryView.Current = ATPTEFMTransactionHistoryView.Select(doc.RefNbr);

            #region Transaction History Establishment
            if (ATPTEFMTransactionHistoryView.Current != null && ATPTEFMTransactionHistoryView.Current.TransactionType == CashFundManagement.BLC.ATPTEFMFundMaint.ATPTEFMTransactionHistoryView.transactionType.DecreaseFund)
            {
                ATPTEFMTransactionHistoryView.Current.Status = ATPTEFMFundStatusAttribute.ReversedValue;
                ATPTEFMTransactionHistoryView.UpdateCurrent();
                ATPTEFMTransactionHistoryView.Cache.Persist(PXDBOperation.Update);
            }
            #endregion
        }
        private void ClosedCAReversedApplication(APRegister doc)
        {
            ATPTEFMCashAdvance cAdv = PXSelect<
                ATPTEFMCashAdvance,
                Where<ATPTEFMCashAdvance.billType, Equal<Required<ATPTEFMCashAdvance.billType>>,
                    And<ATPTEFMCashAdvance.billRefNbr, Equal<Required<ATPTEFMCashAdvance.billRefNbr>>>>>
                .Select(this.Base, APDocType.Prepayment, doc.RefNbr);

            if (cAdv != null)
            {
                if (cAdv.Status == ATPTEFMCashAdvanceStatusAttribute.ClosedValue && doc.CuryDocBal > decimal.Zero)
                {
                    ATPTEFMCashAdvanceEntry caEntry = PXGraph.CreateInstance<ATPTEFMCashAdvanceEntry>();
                    caEntry.Clear();

                    cAdv.Status = ATPTEFMCashAdvanceStatusAttribute.PendingLiquidationValue;
                    cAdv.CuryChangeAmount = doc.CuryDocBal;
                    cAdv.ChangeAmount = doc.DocBal;

                    caEntry.CashAdvances.Update(cAdv);
                    caEntry.Save.Press();
                }
            }
        }
        #endregion
        #endregion
    }
}