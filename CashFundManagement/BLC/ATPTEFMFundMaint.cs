using CashFundManagement.Attributes;
using CashFundManagement.DAC;
using CashFundManagement.DAC.Setup;
using CashFundManagement.Extensions.DAC;
using CashFundManagement.Helper;
using CashFundManagement.Messages;
using PX.Data;
using PX.Data.BQL;
using PX.Data.ReferentialIntegrity.Attributes;
using PX.Data.WorkflowAPI;
using PX.Objects.AP;
using PX.Objects.CM;
using PX.Objects.Common;
using PX.Objects.CR;
using PX.Objects.CS;
using PX.Objects.EP;
using PX.Objects.GL;
using PX.Objects.IN;
using PX.Objects.PM;
using PX.Objects.TX;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using static CashFundManagement.Helper.ATPTEFMShared;
using static PX.Data.BQL.BqlPlaceholder;
using static PX.Objects.CS.BranchMaint;
using static PX.Objects.FA.FABookSettings.midMonthType;

namespace CashFundManagement.BLC
{
    // Screen ID: ATPT2012
    /// <remarks>
    /// 2025-05-30 : Add LinkCommand for Link to Check in Financial Tab : 011800 : RFS
    /// </remarks>
    public class ATPTEFMFundMaint : ATPTPXGraphWithWorkflow<ATPTEFMFundMaint, ATPTEFMFund>
    {
        #region Views + Constructor
        private decimal balanceAmt = 0m;
        public PXSetup<ATPTEFMSetup> Setup;
        public PXSetup<ATPTEFMCASetup> Preferences;
        public PXSelect<ATPTEFM2023R2Enhancements> Enhancements;
        public PXSetup<APSetup> ApSetup;

        [PXViewName("Employee")]
        public PXSelect<
            EPEmployee,
            Where<EPEmployee.bAccountID, Equal<Optional<ATPTEFMFund.custodianID>>>>
            EPEmployee;

        public PXSelect<
            EPEmployee,
            Where<EPEmployee.bAccountID, Equal<Current<ATPTEFMFund.custodianID>>>>
            CurrentCustodian;

        public PXSelect<
           EPEmployee,
           Where<EPEmployee.bAccountID, Equal<Current<ATPTEFMFund.payeeID>>>>
           CurrentPayee;

        public ToggleCurrency<ATPTEFMFund> CurrencyView;
        public PXSelect<
            CurrencyInfo,
            Where<CurrencyInfo.curyInfoID, Equal<Current<ATPTEFMFund.curyInfoID>>>>
            currencyinfo;

        [PXCopyPasteHiddenFields(typeof(ATPTEFMFund.curyFundAmt))]
        [PXViewName("Funds")]
        public PXSelect<ATPTEFMFund> Document;

        public PXFilter<ATPTEFMIncreaseFund> IncreaseFundDocument;
        public PXFilter<ATPTEFMDecreaseFund> DecreaseFundDocument;

        public PXSelect<
            ATPTEFMFund,
            Where<ATPTEFMFund.fundID, Equal<Current<ATPTEFMFund.fundID>>>>
            CurrentDocument;

        public PXSelectReadonly<ATPTEFMTransactionHistoryView> Transactions;
        public PXSelectReadonly<ATPTEFMFundBalanceView> BalanceSummary;

        public PXSelectReadonly<ATPTEFMFundEstablishment> Establishment;

        public PXSelect<
            ATPTEFMFundEstablishment,
            Where<ATPTEFMFundEstablishment.fundRefNbr, Equal<Current<ATPTEFMFund.fundCD>>>>
            CurrentEstablishment;

        [PXCopyPasteHiddenView]
        public PXSelect<
            ATPTEFMFundTransactionHistoryView,
            Where<ATPTEFMFundTransactionHistoryView.fundRefNbr, Equal<Current<ATPTEFMFund.fundCD>>>,
            OrderBy<
                Asc<ATPTEFMFundTransactionHistoryView.sortNbr>>>
            CurrentTransactionHistoryView;

        [PXViewName("Expense Receipts")]
        public PXSelect<
            EPExpenseClaimDetails,
            Where<EPExpenseClaimDetails.claimDetailCD, Equal<Required<EPExpenseClaimDetails.claimDetailCD>>>>
            ATPTEFMExpenseClaimDetails;

        [PXViewName("Setup Approval")]
        public PXSelect<
            ATPTEFMFundsApprovalSetup,
            Where<ATPTEFMFundsApprovalSetup.fundType, Equal<Optional<ATPTEFMFund.fundType>>,
                And<ATPTEFMFundsApprovalSetup.isActive, Equal<True>>>>
            SetupApproval;

        [PXViewName("Approval")]
        public EPApprovalAutomation<
            ATPTEFMFund, ATPTEFMFund.approved, ATPTEFMFund.rejected, ATPTEFMFund.hold, ATPTEFMFundsApprovalSetup>
            Approval;

        [PXCopyPasteHiddenView]
        public PXSelectReadonly<
            APPayment,
            Where2<
                Where<APPayment.docType, Equal<APDocType.check>,
                    Or<APPayment.docType, Equal<APDocType.refund>>>,
                And<ATPTEFMAPRegisterExt.usrATPTEFMSourceRef, Equal<Current<ATPTEFMFund.fundCD>>>>,
            OrderBy<
                Asc<APPayment.refNbr>>>
            APPaymentDocument;

        [PXCopyPasteHiddenView]
        public PXSelect<APInvoice> APInvoiceDocument;

        public PXSelect<
            EPEmployee,
            Where<EPEmployee.bAccountID, Equal<Current<ATPTEFMFund.custodianID>>>>
            Custodian;

        public PXSelectReadonly<
            EPEmployee,
            Where<EPEmployee.userID, Equal<Current<AccessInfo.userName>>>>
            CurrentEmployee;

        public ATPTEFMFundMaint()
        {
#if !Version23R2
            if (!(Preferences?.Current?.EnableCFM ?? false))
            {
                throw new PXException(ATPTEFMMessages.CashFundManagementFeatureDisabled);
            }
#endif
        }
        #endregion 

        #region View Delegates

        /// <remarks>
        /// 016234 - Link to Check is not reflected when Check is created directly from Checks and Payments screen. {JCL}
        /// Includes checks applied directly to the Fund's establishment bill via APAdjust (not tagged via usrATPTEFMSourceRef).
        /// </remarks>
        protected virtual IEnumerable aPPaymentDocument()
        {
            ATPTEFMFund fund = Document.Current;
            if (fund == null) yield break;

            // Collect all checks already linked via the source-ref extension field (standard flow via Pay button)
            var taggedChecks = PXSelectReadonly<
                APPayment,
                Where2<
                    Where<APPayment.docType, Equal<APDocType.check>,
                        Or<APPayment.docType, Equal<APDocType.refund>>>,
                    And<ATPTEFMAPRegisterExt.usrATPTEFMSourceRef, Equal<Current<ATPTEFMFund.fundCD>>>>,
                OrderBy<
                    Asc<APPayment.refNbr>>>
                .Select(this);

            HashSet<string> yieldedRefNbrs = new HashSet<string>();

            foreach (APPayment payment in taggedChecks)
            {
                yieldedRefNbrs.Add(payment.RefNbr);
                yield return payment;
            }

            // Collect establishment bill ref numbers for this fund
            var establishmentRefNbrs = PXSelect<
                ATPTEFMFundEstablishment,
                Where<ATPTEFMFundEstablishment.fundRefNbr, Equal<Current<ATPTEFMFund.fundCD>>>>
                .Select(this)
                .RowCast<ATPTEFMFundEstablishment>()
                .Where(e => !string.IsNullOrEmpty(e.EstablishmentRefNbr))
                .Select(e => e.EstablishmentRefNbr)
                .ToHashSet();

            if (establishmentRefNbrs.Count == 0) yield break;

            // Find any check/payment applied to any of the fund's establishment bills via APAdjust
            // This covers the scenario where a check is created directly in Checks and Payments
            // and the establishment bill is applied under "Documents to Apply"
            foreach (string estRefNbr in establishmentRefNbrs)
            {
                foreach (PXResult<APPayment, APAdjust> result in PXSelectJoin<
                    APPayment,
                    InnerJoin<APAdjust,
                        On<APAdjust.adjgRefNbr, Equal<APPayment.refNbr>,
                        And<APAdjust.adjgDocType, Equal<APPayment.docType>>>>,
                    Where2<
                        Where<APPayment.docType, Equal<APDocType.check>,
                            Or<APPayment.docType, Equal<APDocType.refund>>>,
                        And<APAdjust.adjdRefNbr, Equal<Required<APAdjust.adjdRefNbr>>,
                        And<APAdjust.voided, Equal<False>>>>>
                    .Select(this, estRefNbr))
                {
                    APPayment payment = result;
                    if (!yieldedRefNbrs.Contains(payment.RefNbr))
                    {
                        yieldedRefNbrs.Add(payment.RefNbr);
                        yield return payment;
                    }
                }
            }
        }

        protected virtual IEnumerable transactions()
        {
            List<ATPTEFMTransactionHistoryView> result = new List<ATPTEFMTransactionHistoryView>();

            using (new PXConnectionScope())
            {

                decimal? balance = 0m;

                decimal? runningBalance = 0m;
                decimal? CheckAmount = 0m;

                #region Establishment of Funds

                var fundEstablishment = PXSelect<
                    ATPTEFMFundEstablishment,
                    Where<ATPTEFMFundEstablishment.fundRefNbr, Equal<Current<ATPTEFMFund.fundCD>>>>
                    .Select(this);

                foreach (ATPTEFMFundEstablishment establishment in fundEstablishment)
                {

                    APInvoice fundInvoice = PXSelect<
                        APInvoice,
                        Where<APInvoice.refNbr, Equal<Required<APInvoice.refNbr>>,
                            And<APInvoice.docType, Equal<APDocType.invoice>>>>
                        .Select(this, establishment.EstablishmentRefNbr);


                    if (fundInvoice != null && (!fundInvoice.IsMigratedRecord ?? false))
                    {
                        balance = fundInvoice.CuryOrigDocAmt;

                        runningBalance = balance;

                        APAdjust adjust = PXSelect<
                            APAdjust,
                            Where<APAdjust.adjdRefNbr, Equal<Required<APAdjust.adjdRefNbr>>,
                                And<APAdjust.voided, NotEqual<True>,
                                And<APAdjust.adjgDocType, NotEqual<APPaymentType.debitAdj>>>>>
                            .Select(this, establishment.EstablishmentRefNbr);

                        foreach (APAdjust check in PXSelect<
                            APAdjust,
                            Where<APAdjust.adjdRefNbr, Equal<Required<APAdjust.adjdRefNbr>>,
                                And<APAdjust.voided, NotEqual<True>,
                                And<APAdjust.adjgDocType, NotEqual<APPaymentType.debitAdj>>>>>
                            .Select(this, establishment.EstablishmentRefNbr))
                        {
                            if (check.AdjgDocType == APPaymentType.VoidCheck || check.AdjgDocType == APPaymentType.DebitAdj)
                            {
                                CheckAmount -= check.AdjAmt;
                            }
                            else
                            {
                                CheckAmount += check.AdjAmt;
                            }
                        }

                        ATPTEFMTransactionHistoryView record = InitFundsTransactionHistory();

                        record.OrderDate = fundInvoice.DocDate;
                        record.TransactionType = ATPTEFMTransactionHistoryView.transactionType.Establishment;
                        record.RefNbr = fundInvoice.RefNbr;
                        //record.SortNbr = fundInvoice.RefNbr;
                        record.SortNbr = $"EF-{fundInvoice.RefNbr}";
                        record.FundBranchID = fundInvoice.BranchID;
                        record.FundType = null;
                        record.TransactionDate = fundInvoice.DocDate;
                        record.FundTransactionDocumentAmt = fundInvoice.CuryOrigDocAmt;
                        record.Status = fundInvoice.Status;
                        record.CheckNbr = adjust?.AdjgRefNbr;
                        record.CheckAmt = adjust?.CuryAdjgAmt;
                        record.BalanceAmt = CheckAmount;
                        record.DocumentBalanceAmt = fundInvoice.CuryOrigDocAmt;
                        result.Add(record);
                    }
                    else if (fundInvoice != null && (fundInvoice.IsMigratedRecord ?? false))
                    {
                        ATPTEFMTransactionHistoryView record = InitFundsTransactionHistory();

                        record.OrderDate = fundInvoice.DocDate;
                        record.TransactionType = ATPTEFMTransactionHistoryView.transactionType.Establishment;
                        record.RefNbr = fundInvoice.RefNbr;
                        record.FundBranchID = fundInvoice.BranchID;
                        record.FundType = null;
                        record.TransactionDate = fundInvoice.DocDate;
                        record.FundTransactionDocumentAmt = fundInvoice.CuryOrigDocAmt;
                        record.Status = fundInvoice.Status;
                        record.CheckNbr = "MIGRATED BILL";
                        record.BalanceAmt = fundInvoice.CuryDocBal;
                        record.DocumentBalanceAmt = fundInvoice.CuryOrigDocAmt;
                        result.Add(record);
                    }
                }
                #endregion

                #region Fund Transactions - Request

                var fundTransactions = PXSelect<
                    ATPTEFMFundTransaction,
                    Where<ATPTEFMFundTransaction.fundID, Equal<Current<ATPTEFMFund.fundCD>>,
                        And<ATPTEFMFundTransaction.fundTransactionType, Equal<ATPTEFMFundTransactionTypeAttribute.cashAdvanceValue>>>>
                    .Select(this);

                foreach (ATPTEFMFundTransaction item in fundTransactions)
                {
                    string transactionType = ((item.FundTransactionType == ATPTEFMFundTransactionTypeAttribute.CashAdvanceValue) ?
                                ATPTEFMTransactionHistoryView.transactionType.FundRequest :
                                ATPTEFMTransactionHistoryView.transactionType.FundReimbursment);


                    var receipts = PXSelect<
                        ATPTEFMFundTransactionReceiptDetail,
                        Where<ATPTEFMFundTransactionReceiptDetail.fundTransactionRefNbr,
                            Equal<Required<ATPTEFMFundTransactionReceiptDetail.fundTransactionRefNbr>>>>
                        .Select(this, item.RefNbr);

                    decimal? _unliquidated = 0;
                    bool status = item.CashAdvanceStatus.Equals(ATPTEFMFundTransactionCashAdvanceStatusAttribute.UnliquidatedValue);
                    bool unliquidated = item.Step.Equals(ATPTEFMFundTransactionStepAttribute.SubmitReceiptValue);
                    bool forReclassification = item.CashAdvanceStatus.Equals(ATPTEFMFundTransactionCashAdvanceStatusAttribute.ForReclassificationValue);

                    #region Fund Request
                    _unliquidated = item.ReleasedAmount.GetValueOrDefault();

                    string[] typesToUpdateBalance = { ATPTEFMFundTransactionCashAdvanceStatusAttribute.LiquidatedValue, ATPTEFMFundTransactionCashAdvanceStatusAttribute.UnliquidatedValue };
                    runningBalance -= typesToUpdateBalance.Contains(item.CashAdvanceStatus) ? item.RequestedAmount.GetValueOrDefault() : 0m;

                    ATPTEFMTransactionHistoryView record = InitFundsTransactionHistory();

                    runningBalance += item.ChangeAmount;
                    record.OrderDate = item.Date;
                    record.TransactionType = transactionType;
                    record.RefNbr = item.RefNbr;
                    //record.SortNbr = item.RefNbr;
                    record.SortNbr = $"FT-{item.RefNbr}";
                    record.FundBranchID = item.BranchID;
                    record.FundType = item.FundType;
                    record.TransactionDate = item.Date;
                    record.FundTransactionDocumentAmt = item.RequestedAmount.GetValueOrDefault();
                    record.Status = item.Status;
                    record.Source = ATPTEFMTransactionHistoryView.source.FundTransaction;
                    record.IsUnliquidatedRequest = unliquidated;
                    record.UnliquidatedAmt = (status && unliquidated != true) ? _unliquidated : 0m;
                    record.CashAdvanceStatus = item.CashAdvanceStatus;
                    if (Document?.Current?.ATPTEFMValidateAmountReceivedAndAmountReleased ?? false)
                    {
                        if (item.AmountReceived != 0m)
                            record.FundReturnAmt = item.AmountReceived ?? 0m;

                        else if (item.AmountReleased != 0m)
                            record.FundReturnAmt = (item.AmountReleased ?? 0m) * -1;
                    }
                    else
                        record.FundReturnAmt = item.ChangeAmount ?? 0m;

                    record.FundAmt = item.RequestedAmount.GetValueOrDefault();
                    result.Add(record);
                    #endregion

                    #region Expense Receipt

                    bool isFundLiquidated = (item.Step == ATPTEFMFundTransactionStepAttribute.LiquidatedReceiptValue);
                    bool isUnliquidated = (item.Step == ATPTEFMFundTransactionStepAttribute.SubmitReceiptValue);

                    PXResultset<EPExpenseClaimDetails, ATPTEFMFundTransactionReceiptDetail, ATPTEFMFundTransaction> expenseClaimDetails = PXSelectJoin<
                        EPExpenseClaimDetails,
                        InnerJoin<ATPTEFMFundTransactionReceiptDetail,
                            On<ATPTEFMFundTransactionReceiptDetail.expenseReceiptRefNbr, Equal<EPExpenseClaimDetails.claimDetailCD>>,
                        InnerJoin<ATPTEFMFundTransaction,
                            On<ATPTEFMFundTransaction.refNbr, Equal<ATPTEFMFundTransactionReceiptDetail.fundTransactionRefNbr>>>>,
                        Where<ATPTEFMFundTransaction.refNbr, Equal<Required<ATPTEFMFundTransaction.refNbr>>>>
                        .Select<PXResultset<EPExpenseClaimDetails, ATPTEFMFundTransactionReceiptDetail, ATPTEFMFundTransaction>>(this, item.RefNbr);

                    int sortCounterRequest = 0;
                    string fundReqRefNbr = string.Empty;
                    foreach (PXResult<EPExpenseClaimDetails, ATPTEFMFundTransactionReceiptDetail, ATPTEFMFundTransaction> details in expenseClaimDetails)
                    {
                        decimal? wTaxAmount = 0m;

                        EPExpenseClaimDetails ecDetails = (EPExpenseClaimDetails)details;
                        ATPTEFMFundTransactionReceiptDetail ftrDetails = (ATPTEFMFundTransactionReceiptDetail)details;
                        ATPTEFMEPExpenseClaimDetailsExt claimExt = ecDetails.GetExtension<ATPTEFMEPExpenseClaimDetailsExt>();

                        ATPTEFMReplenishmentDetail repDetails = PXSelectJoin<
                            ATPTEFMReplenishmentDetail,
                            InnerJoin<ATPTEFMReplenishment,
                                On<ATPTEFMReplenishment.replenishmentNbr, Equal<ATPTEFMReplenishmentDetail.replenishmentNbr>,
                                And<ATPTEFMReplenishment.status, NotEqual<ATPTEFMReplenishmentStatusAttribute.cancelledValue>>>>,
                            Where<ATPTEFMReplenishmentDetail.expenseReceiptNbr, Equal<Required<ATPTEFMReplenishmentDetail.expenseReceiptNbr>>>>
                            .Select(this, ftrDetails.ExpenseReceiptRefNbr);

                        if (ecDetails.Status != ATPTEFMExpenseReceiptStatusAttribute.CancelledValue)
                        {
                            foreach (EPTaxTran taxTran in PXSelect<
                                EPTaxTran,
                                Where<EPTaxTran.claimDetailID, Equal<Required<EPTaxTran.claimDetailID>>>>
                                .Select(this, ecDetails.ClaimDetailID))
                            {
                                Tax tx = PXSelect<
                                    Tax,
                                    Where<Tax.taxID, Equal<Required<Tax.taxID>>>>
                                    .Select(this, taxTran.TaxID);

                                if (tx.TaxType == CSTaxType.Withholding)
                                {
                                    wTaxAmount += taxTran.TaxAmt;
                                }
                            }
                        }

                        // Initialize fund transaction reference number if counter is zero
                        if (sortCounterRequest == 0)
                        {
                            fundReqRefNbr = ftrDetails.FundTransactionRefNbr;
                        }
                        // Reset counter to zero when a new fund transaction reference number is encountered
                        // This ensures each fund transaction starts counting from 1 for proper sorting
                        if (fundReqRefNbr != ftrDetails.FundTransactionRefNbr)
                        {
                            sortCounterRequest = 0;
                            fundReqRefNbr = ftrDetails.FundTransactionRefNbr;
                        }

                        sortCounterRequest++;
                        ATPTEFMTransactionHistoryView receipt = InitFundsTransactionHistory();

                        bool isCancel = ecDetails.Status.Equals(ATPTEFMCashAdvanceStatusAttribute.CancelledValue);

                        receipt.OrderDate = ecDetails.ExpenseDate;
                        receipt.TransactionType = ATPTEFMTransactionHistoryView.transactionType.ExpenseReceipt;
                        receipt.RefNbr = ecDetails.ClaimDetailCD;
                        //receipt.SortNbr = $"{ftrDetails.FundTransactionRefNbr}-{sortCounterRequest}";
                        receipt.SortNbr = $"FT-{ftrDetails.FundTransactionRefNbr}-{sortCounterRequest}";
                        receipt.Source = ATPTEFMTransactionHistoryView.source.ExpenseReceipt;
                        receipt.FundTransactionSortNbr = $"FT-{ftrDetails.FundTransactionRefNbr}";
                        receipt.FundBranchID = ecDetails.BranchID;
                        receipt.FundType = claimExt.UsrATPTEFMFundType;
                        receipt.TransactionDate = ecDetails.ExpenseDate;
                        receipt.FundTransactionDocumentAmt = ecDetails.CuryExtCost;
                        receipt.Status = isFundLiquidated ? ATPTEFMFundStatusAttribute.LiquidatedValue : isCancel ? ATPTEFMFundStatusAttribute.CancelledValue : ATPTEFMFundStatusAttribute.OpenValue;
                        receipt.LiquidatedAmt = isFundLiquidated ? ftrDetails.NetAmt.GetValueOrDefault() - wTaxAmount : decimal.Zero;
                        receipt.UnliquidatedAmt = isUnliquidated ? ftrDetails.NetAmt.GetValueOrDefault() - wTaxAmount : decimal.Zero;
                        receipt.FundReturnAmt = decimal.Zero;
                        receipt.IsUnliquidatedRequest = unliquidated;
                        receipt.ReplenishmentRefNbr = (repDetails is null) ? string.Empty : repDetails.ReplenishmentNbr;
                        receipt.ProjectID = ecDetails.ContractID;
                        receipt.ProjectTaskID = ecDetails.TaskID;
                        receipt.CostCodeID = ecDetails.CostCodeID;
                        receipt.WithholdingTax = wTaxAmount;
                        result.Add(receipt);
                    }
                    #endregion

                    #region Reclass Receipt

                    PXResultset<EPExpenseClaimDetails, ATPTEFMFundTransactionReclassficationReceiptDetail, ATPTEFMFundTransaction> reclassExpenseClaimDetails = PXSelectJoin<
                        EPExpenseClaimDetails,
                        InnerJoin<ATPTEFMFundTransactionReclassficationReceiptDetail,
                            On<ATPTEFMFundTransactionReclassficationReceiptDetail.expenseReceiptRefNbr, Equal<EPExpenseClaimDetails.claimDetailCD>>,
                        InnerJoin<ATPTEFMFundTransaction,
                            On<ATPTEFMFundTransaction.refNbr, Equal<ATPTEFMFundTransactionReclassficationReceiptDetail.fundTransactionRefNbr>>>>,
                        Where<ATPTEFMFundTransaction.refNbr, Equal<Required<ATPTEFMFundTransaction.refNbr>>>>
                        .Select<PXResultset<EPExpenseClaimDetails, ATPTEFMFundTransactionReclassficationReceiptDetail, ATPTEFMFundTransaction>>(this, item.RefNbr);

                    foreach (PXResult<EPExpenseClaimDetails, ATPTEFMFundTransactionReclassficationReceiptDetail, ATPTEFMFundTransaction> details in reclassExpenseClaimDetails)
                    {

                        EPExpenseClaimDetails ecDetails = (EPExpenseClaimDetails)details;
                        ATPTEFMFundTransactionReclassficationReceiptDetail ftReclassReceiptDetails = (ATPTEFMFundTransactionReclassficationReceiptDetail)details;
                        ATPTEFMEPExpenseClaimDetailsExt claimExt = ecDetails.GetExtension<ATPTEFMEPExpenseClaimDetailsExt>();

                        ATPTEFMReplenishmentDetail repDetails = PXSelectJoin<
                            ATPTEFMReplenishmentDetail,
                            InnerJoin<ATPTEFMReplenishment,
                                On<ATPTEFMReplenishment.replenishmentNbr, Equal<ATPTEFMReplenishmentDetail.replenishmentNbr>,
                                And<ATPTEFMReplenishment.status, NotEqual<ATPTEFMReplenishmentStatusAttribute.cancelledValue>>>>,
                            Where<ATPTEFMReplenishmentDetail.expenseReceiptNbr, Equal<Required<ATPTEFMReplenishmentDetail.expenseReceiptNbr>>>>
                            .Select(this, ftReclassReceiptDetails.ExpenseReceiptRefNbr);

                        ATPTEFMTransactionHistoryView reclass = InitFundsTransactionHistory();

                        bool isCancel = ecDetails.Status.Equals(ATPTEFMCashAdvanceStatusAttribute.CancelledValue);

                        reclass.OrderDate = ecDetails.ExpenseDate;
                        reclass.TransactionType = ATPTEFMTransactionHistoryView.transactionType.Reclassificaation;
                        reclass.RefNbr = ecDetails.ClaimDetailCD;
                        //reclass.SortNbr = $"{ftReclassReceiptDetails.FundTransactionRefNbr}-{sortCounterRequest + 1}";
                        reclass.SortNbr = $"FT-{ftReclassReceiptDetails.FundTransactionRefNbr}-{sortCounterRequest + 1}";
                        reclass.Source = ATPTEFMTransactionHistoryView.source.ExpenseReceipt;
                        reclass.FundTransactionSortNbr = $"FT-{ftReclassReceiptDetails.FundTransactionRefNbr}";
                        reclass.FundBranchID = ecDetails.BranchID;
                        reclass.FundType = claimExt.UsrATPTEFMFundType;
                        reclass.TransactionDate = ecDetails.ExpenseDate;
                        reclass.FundTransactionDocumentAmt = ecDetails.CuryExtCost;
                        reclass.Status = isFundLiquidated ? ATPTEFMFundStatusAttribute.LiquidatedValue : isCancel ? ATPTEFMFundStatusAttribute.CancelledValue : ATPTEFMFundStatusAttribute.OpenValue;
                        reclass.LiquidatedAmt = isFundLiquidated ? ftReclassReceiptDetails.NetAmt.GetValueOrDefault() : decimal.Zero;
                        reclass.UnliquidatedAmt = isUnliquidated ? ftReclassReceiptDetails.NetAmt.GetValueOrDefault() : decimal.Zero;
                        reclass.FundReturnAmt = decimal.Zero;
                        reclass.IsUnliquidatedRequest = unliquidated;
                        reclass.ReplenishmentRefNbr = (repDetails is null) ? string.Empty : repDetails.ReplenishmentNbr;
                        reclass.ProjectID = ecDetails.ContractID;
                        reclass.ProjectTaskID = ecDetails.TaskID;
                        reclass.CostCodeID = ecDetails.CostCodeID;
                        result.Add(reclass);
                    }

                    #endregion
                }
                #endregion

                #region Fund Transaction - Reimbursement

                var reimbursement = PXSelect<
                    ATPTEFMFundTransaction,
                    Where<ATPTEFMFundTransaction.fundID, Equal<Current<ATPTEFMFund.fundCD>>,
                        And<ATPTEFMFundTransaction.fundTransactionType, Equal<ATPTEFMFundTransactionTypeAttribute.reimbursementValue>>>>
                    .Select(this);

                int sortCounterReimbursement = 0;
                foreach (ATPTEFMFundTransaction item in reimbursement)
                {
                    string transactionType = ATPTEFMTransactionHistoryView.transactionType.FundReimbursment;
                    var receipts = PXSelect<
                        ATPTEFMFundTransactionReceiptDetail,
                        Where<ATPTEFMFundTransactionReceiptDetail.fundTransactionRefNbr,
                                    Equal<Required<ATPTEFMFundTransactionReceiptDetail.fundTransactionRefNbr>>>>
                        .Select(this, item.RefNbr);
                    decimal? _liquidated = 0;
                    bool iscancel = item.Status.Equals(ATPTEFMCashAdvanceStatusAttribute.CancelledValue);

                    PXResultset<EPExpenseClaimDetails, ATPTEFMFundTransactionReceiptDetail> expenseClaimDetails = PXSelectJoin<
                        EPExpenseClaimDetails,
                        InnerJoin<ATPTEFMFundTransactionReceiptDetail,
                            On<ATPTEFMFundTransactionReceiptDetail.expenseReceiptRefNbr, Equal<EPExpenseClaimDetails.claimDetailCD>>>,
                        Where<ATPTEFMFundTransactionReceiptDetail.fundTransactionRefNbr, Equal<Required<ATPTEFMFundTransactionReceiptDetail.fundTransactionRefNbr>>>>
                        .Select<PXResultset<EPExpenseClaimDetails, ATPTEFMFundTransactionReceiptDetail>>(this, item.RefNbr);

                    #region Expense Receipt
                    string fundTranRefNbr = string.Empty;
                    foreach (PXResult<EPExpenseClaimDetails, ATPTEFMFundTransactionReceiptDetail> details in expenseClaimDetails)
                    {
                        decimal? wTaxAmount = 0m;

                        EPExpenseClaimDetails ecDetails = (EPExpenseClaimDetails)details;
                        ATPTEFMFundTransactionReceiptDetail ftrDetails = (ATPTEFMFundTransactionReceiptDetail)details;
                        ATPTEFMEPExpenseClaimDetailsExt claimExt = ecDetails.GetExtension<ATPTEFMEPExpenseClaimDetailsExt>();

                        ATPTEFMReplenishmentDetail repDetails = PXSelectJoin<
                            ATPTEFMReplenishmentDetail,
                            InnerJoin<ATPTEFMReplenishment,
                                On<ATPTEFMReplenishment.replenishmentNbr, Equal<ATPTEFMReplenishmentDetail.replenishmentNbr>,
                                And<ATPTEFMReplenishment.status, NotEqual<ATPTEFMReplenishmentStatusAttribute.cancelledValue>>>>,
                            Where<ATPTEFMReplenishmentDetail.expenseReceiptNbr, Equal<Required<ATPTEFMReplenishmentDetail.expenseReceiptNbr>>>>
                            .Select(this, ftrDetails.ExpenseReceiptRefNbr);

                        if ((claimExt.UsrATPTEFMFundReturn ?? 0.00m) > 0)
                        {
                            runningBalance += claimExt.UsrATPTEFMFundReturn.GetValueOrDefault();
                        }

                        if (ecDetails.Status != ATPTEFMExpenseReceiptStatusAttribute.CancelledValue)
                        {
                            foreach (EPTaxTran taxTran in PXSelect<
                                EPTaxTran,
                                Where<EPTaxTran.claimDetailID, Equal<Required<EPTaxTran.claimDetailID>>>>
                                .Select(this, ecDetails.ClaimDetailID))
                            {
                                Tax tx = PXSelect<
                                    Tax,
                                    Where<Tax.taxID, Equal<Required<Tax.taxID>>>>
                                    .Select(this, taxTran.TaxID);

                                if (tx.TaxType == CSTaxType.Withholding)
                                {
                                    wTaxAmount += taxTran.TaxAmt;
                                }
                            }
                        }

                        // Initialize fund transaction reference number if counter is zero
                        if (sortCounterReimbursement == 0)
                        {
                            fundTranRefNbr = ftrDetails.FundTransactionRefNbr;
                        }
                        // Reset counter to zero when a new fund transaction reference number is encountered
                        // This ensures each fund transaction starts counting from 1 for proper sorting
                        if (fundTranRefNbr != ftrDetails.FundTransactionRefNbr)
                        {
                            sortCounterReimbursement = 0;
                            fundTranRefNbr = ftrDetails.FundTransactionRefNbr;
                        }

                        sortCounterReimbursement++;
                        string formattedsortCounterReimbursement = sortCounterReimbursement.ToString("D2").Trim();

                        ATPTEFMTransactionHistoryView record = InitFundsTransactionHistory();

                        record.OrderDate = ecDetails.ExpenseDate;
                        record.TransactionType = ATPTEFMTransactionHistoryView.transactionType.ExpenseReceipt;
                        record.RefNbr = ecDetails.ClaimDetailCD;
                        //record.SortNbr = $"{ftrDetails.FundTransactionRefNbr}-{sortCounterReimbursement}";
                        record.Source = ATPTEFMTransactionHistoryView.source.ExpenseReceipt;
                        record.SortNbr = $"FT-{ftrDetails.FundTransactionRefNbr}-{formattedsortCounterReimbursement}";
                        record.FundTransactionSortNbr = $"FT-{ftrDetails.FundTransactionRefNbr}";
                        record.FundBranchID = ecDetails.BranchID;
                        record.FundType = claimExt.UsrATPTEFMFundType;
                        record.TransactionDate = ecDetails.ExpenseDate;
                        record.FundTransactionDocumentAmt = iscancel ? ecDetails.CuryExtCost : 0m;
                        record.Status = iscancel ? ATPTEFMFundStatusAttribute.CancelledValue : ATPTEFMFundStatusAttribute.LiquidatedValue;
                        record.LiquidatedAmt = iscancel ? 0m : ecDetails.CuryExtCost - wTaxAmount;
                        record.UnliquidatedAmt = 0.00m;
                        record.FundReturnAmt = 0.00m;
                        record.IsReimbursement = true;
                        record.ProjectID = ecDetails.ContractID;
                        record.ProjectTaskID = ecDetails.TaskID;
                        record.CostCodeID = ecDetails.CostCodeID;
                        record.WithholdingTax = ftrDetails.WhtAmount;
                        record.ReplenishmentRefNbr = (repDetails is null) ? string.Empty : repDetails.ReplenishmentNbr;
                        result.Add(record);
                    }
                    #endregion

                    #region Fund Reimbursement
                    if (item.Status == ATPTEFMFundStatusAttribute.ClosedValue || item.Status == ATPTEFMFundStatusAttribute.CancelledValue)
                    {
                        _liquidated = item.ActualSpentAmount.GetValueOrDefault();
                        runningBalance -= item.ActualSpentAmount.GetValueOrDefault();
                        var finalSortCounterReimbursement = sortCounterReimbursement + 1;
                        string fomrattedFinalSortCounterReimbursement = finalSortCounterReimbursement.ToString("D2").Trim();

                        ATPTEFMTransactionHistoryView record = InitFundsTransactionHistory();

                        record.OrderDate = item.Date;
                        record.TransactionType = transactionType;
                        record.RefNbr = item.RefNbr;
                        record.SortNbr = $"FT-{item.RefNbr}-{fomrattedFinalSortCounterReimbursement}";
                        record.FundBranchID = item.BranchID;
                        record.FundType = item.FundType;
                        record.TransactionDate = item.Date;
                        record.FundTransactionDocumentAmt = iscancel ? 0m : item.ActualSpentAmount.GetValueOrDefault();
                        record.ReimbursementWht = item.TotalWhtAmount.GetValueOrDefault();
                        record.Status = item.Status;
                        record.Source = ATPTEFMTransactionHistoryView.source.FundTransaction;
                        record.FundAmt = item.ActualSpentAmount.GetValueOrDefault();
                        result.Add(record);
                    }
                    #endregion
                }
                #endregion

                #region Replenishment Transactions

                var headers = PXSelect<
                    ATPTEFMReplenishment,
                    Where<ATPTEFMReplenishment.fundID, Equal<Current<ATPTEFMFund.fundCD>>>>
                    .Select(this);

                foreach (ATPTEFMReplenishment header in headers)
                {
                    if (header != null)
                    {
                        ATPTEFMReplenishmentDetail detail = PXSelect<
                            ATPTEFMReplenishmentDetail,
                            Where<ATPTEFMReplenishmentDetail.replenishmentNbr, Equal<Required<ATPTEFMReplenishmentDetail.replenishmentNbr>>>>
                            .Select(this, header.ReplenishmentNbr);

                        if (detail != null)
                        {

                            bool hasCheck = false;
                            decimal? checkAmount = 0;
                            string paymentRefnbr = string.Empty;
                            int paymentCount = 0;

                            List<APAdjust> payments = new List<APAdjust>();
                            List<string> duplicatePayment = new List<string>();


                            payments.Clear();

                            foreach (APAdjust adjustAmt in PXSelectJoinGroupBy<
                                APAdjust,
                                InnerJoin<ATPTEFMReplenishmentDetail,
                                    On<ATPTEFMReplenishmentDetail.invoiceRefNbr, Equal<APAdjust.adjdRefNbr>>>,
                                Where<ATPTEFMReplenishmentDetail.replenishmentNbr, Equal<Required<ATPTEFMReplenishmentDetail.replenishmentNbr>>,
                                    And<APAdjust.released, Equal<True>,
                                    And<APAdjust.adjgDocType, NotEqual<APPaymentType.debitAdj>,
                                    And<APAdjust.voided, Equal<False>>>>>,
                                Aggregate<
                                    GroupBy<ATPTEFMReplenishmentDetail.invoiceRefNbr>>>
                                .Select(this, header.ReplenishmentNbr))
                            {
                                paymentCount++;
                                payments.Add(adjustAmt);
                                duplicatePayment.Add(adjustAmt.AdjgRefNbr);
                                checkAmount += adjustAmt.CuryAdjgAmt;
                            }

                            hasCheck = paymentCount >= decimal.Zero;

                            var duplicate = duplicatePayment
                            .GroupBy(x => x)
                            .Where(group => group.Count() > 1)
                            .Select(group => group.Key)
                            .ToList();

                            if (paymentCount > decimal.Zero)
                            {
                                bool amtCopyAdded = true;
                                foreach (APAdjust payment in payments.GroupBy(p => p.AdjgRefNbr).Select(g => g.First()).ToList())
                                {
                                    ATPTEFMTransactionHistoryView record = InitFundsTransactionHistory();
                                    record.OrderDate = header.Date;
                                    record.TransactionType = ATPTEFMTransactionHistoryView.transactionType.Replenishment;
                                    record.RefNbr = header.ReplenishmentNbr;
                                    record.FundBranchID = header.BranchID;
                                    record.FundType = header.FundType;
                                    record.TransactionDate = header.Date;
                                    record.FundTransactionDocumentAmt = header.ClaimAmount.GetValueOrDefault();
                                    if (amtCopyAdded)
                                    {
                                        record.FundTransactionDocumentAmtCopy = header.ClaimAmount.GetValueOrDefault();
                                        record.OnReplenishmentWht = /*(string.IsNullOrEmpty(paymentRefnbr) ? decimal.Zero :*/ header.WithholdingTaxAmount.GetValueOrDefault();
                                    }
                                    record.Status = header.Status;
                                    record.Source = ATPTEFMTransactionHistoryView.source.Replenishment;
                                    record.HasReplenishmentCheckNbr = hasCheck;
                                    record.CheckNbr = payment.AdjgRefNbr;
                                    record.CheckAmt = (duplicate.Contains(payment.AdjgRefNbr)) ? checkAmount : payment.CuryAdjgAmt;
                                    record.ReplenishmentAmt = (duplicate.Contains(payment.AdjgRefNbr)) ? checkAmount : payment.CuryAdjgAmt;
                                    result.Add(record);
                                    amtCopyAdded = false;

                                }
                            }
                            else
                            {
                                ATPTEFMTransactionHistoryView record = InitFundsTransactionHistory();
                                record.OrderDate = header.Date;
                                record.TransactionType = ATPTEFMTransactionHistoryView.transactionType.Replenishment;
                                record.RefNbr = header.ReplenishmentNbr;
                                record.FundBranchID = header.BranchID;
                                record.FundType = header.FundType;
                                record.TransactionDate = header.Date;
                                record.FundTransactionDocumentAmt = header.ClaimAmount.GetValueOrDefault();
                                record.FundTransactionDocumentAmtCopy = header.ClaimAmount.GetValueOrDefault();
                                record.Status = header.Status;
                                record.Source = ATPTEFMTransactionHistoryView.source.Replenishment;
                                record.HasReplenishmentCheckNbr = hasCheck;
                                record.CheckNbr = paymentRefnbr;
                                record.OnReplenishmentWht = (paymentCount == decimal.Zero) ? decimal.Zero : header.WithholdingTaxAmount.GetValueOrDefault();
                                record.CheckAmt = checkAmount;
                                record.ReplenishmentAmt = checkAmount;
                                result.Add(record);
                            }
                        }
                    }
                }
                #endregion

                #region Close Fund
                APInvoice closeFundInvoice = PXSelect<
                                                            APInvoice,
                                                            Where<APInvoice.refNbr, Equal<Current<ATPTEFMFund.closeFundRefNbr>>,
                                                                And<APInvoice.docType, Equal<APDocType.debitAdj>>>>
                                                            .Select(this);
                if (closeFundInvoice != null)
                {
                    APAdjust adjust = PXSelect<
                        APAdjust,
                        Where<APAdjust.adjdRefNbr, Equal<Current<ATPTEFMFund.closeFundRefNbr>>>>
                        .Select(this);

                    ATPTEFMTransactionHistoryView record = InitFundsTransactionHistory();

                    record.OrderDate = closeFundInvoice.DocDate;
                    record.TransactionType = ATPTEFMTransactionHistoryView.transactionType.CloseFund;
                    record.RefNbr = closeFundInvoice.RefNbr;
                    record.FundBranchID = closeFundInvoice.BranchID;
                    record.TransactionDate = closeFundInvoice.DocDate;
                    record.FundTransactionDocumentAmt = closeFundInvoice.CuryLineTotal;
                    record.Status = closeFundInvoice.Status;
                    record.CheckNbr = adjust?.AdjgRefNbr;
                    record.CheckAmt = adjust?.CuryAdjgAmt;
                    result.Add(record);
                }
                #endregion

                #region Month-End Transactions
                foreach (ATPTEFMMonthEnd me in PXSelect<
                                                                        ATPTEFMMonthEnd,
                                                                        Where<ATPTEFMMonthEnd.fundID, Equal<Current<ATPTEFMFund.fundCD>>>>
                                                                        .Select(this))
                {
                    ATPTEFMTransactionHistoryView record = InitFundsTransactionHistory();
                    record.TransactionType = ATPTEFMTransactionHistoryView.transactionType.MonthEnd;
                    record.RefNbr = me.RefNbr;
                    record.SortNbr = me.RefNbr;
                    record.Status = me.Status;
                    record.FundBranchID = me.BranchID;
                    record.TransactionDate = me.Date;
                    record.OrderDate = me.Date;
                    record.FundTransactionDocumentAmt = me.Amount;
                    record.ReversingJournalBatchNbr = me.ReversingJournalBatchNbr;
                    result.Add(record);
                }
                #endregion

                #region Increase of Fund

                foreach (APInvoice creditData in PXSelect<
                    APInvoice,
                    Where<APInvoice.docType, Equal<APDocType.creditAdj>,
                        And<ATPTEFMAPRegisterExt.usrATPTEFMSourceRef, Equal<Current<ATPTEFMFund.fundCD>>,
                        And<ATPTEFMAPRegisterExt.usrATPTEFMSourceTranType, Equal<Attributes.ATPTEFMSourceTranTypeAttribute.increaseFund>>>>>
                    .Select(this))
                {
                    APAdjust adjust = PXSelect<
                        APAdjust,
                        Where<APAdjust.adjdRefNbr, Equal<Required<APAdjust.adjdRefNbr>>,
                            And<APAdjust.voided, NotEqual<True>,
                            And<APAdjust.adjdDocType, NotEqual<APPaymentType.debitAdj>>>>>
                        .Select(this, creditData.RefNbr);

                    ATPTEFMTransactionHistoryView record = InitFundsTransactionHistory();


                    record.TransactionType = ATPTEFMTransactionHistoryView.transactionType.IncreaseFund;
                    record.RefNbr = creditData.RefNbr;
                    record.FundBranchID = creditData.BranchID;
                    record.Status = creditData.Status;
                    record.TransactionDate = creditData.DocDate;
                    record.OrderDate = creditData.DocDate;
                    record.FundTransactionDocumentAmt = creditData.CuryLineTotal;
                    record.IncreaseFundAmt = (creditData.Status == APDocStatus.Closed) ? adjust.CuryAdjgAmt : 0m;
                    record.CheckNbr = adjust?.AdjgRefNbr;
                    record.CheckAmt = adjust?.CuryAdjgAmt;
                    result.Add(record);
                }
                #endregion

                #region Decrease of Fund

                foreach (APInvoice debitData in PXSelect<
                    APInvoice,
                    Where<APInvoice.docType, Equal<APDocType.debitAdj>,
                        And<ATPTEFMAPRegisterExt.usrATPTEFMSourceRef, Equal<Current<ATPTEFMFund.fundCD>>,
                        And<ATPTEFMAPRegisterExt.usrATPTEFMSourceTranType, Equal<Attributes.ATPTEFMSourceTranTypeAttribute.decreaseFund>>>>>
                    .Select(this))
                {
                    APAdjust adjust = PXSelect<
                        APAdjust,
                        Where<APAdjust.adjdRefNbr, Equal<Required<APAdjust.adjdRefNbr>>,
                            And<APAdjust.voided, NotEqual<True>,
                            And<APAdjust.adjdDocType, Equal<APPaymentType.debitAdj>,
                            And<APAdjust.adjgDocType, Equal<APDocType.refund>>>>>>
                        .Select(this, debitData.RefNbr);

                    ATPTEFMTransactionHistoryView record = InitFundsTransactionHistory();


                    record.TransactionType = ATPTEFMTransactionHistoryView.transactionType.DecreaseFund;
                    record.RefNbr = debitData.RefNbr;
                    record.FundBranchID = debitData.BranchID;
                    record.Status = debitData.Status;
                    record.TransactionDate = debitData.DocDate;
                    record.OrderDate = debitData.DocDate;
                    record.FundTransactionDocumentAmt = debitData.CuryLineTotal;
                    record.DecreaseFundAmt = (debitData.Status == APDocStatus.Closed) ? adjust.CuryAdjgAmt : 0m;
                    record.CheckNbr = adjust?.AdjgRefNbr;
                    record.CheckAmt = adjust?.CuryAdjgAmt;
                    result.Add(record);
                }
                #endregion

                int i = 0;
                List<ATPTEFMTransactionHistoryView> orderedView = result.OrderBy(o => o.OrderDate).ToList();
                foreach (ATPTEFMTransactionHistoryView ov in orderedView)
                {
                    if (i > 0)
                    {
                        ov.UnliquidatedBalanceAmt = orderedView[i - 1].UnliquidatedBalanceAmt + ov.UnliquidatedAmt;
                        ov.LiquidatedBalanceAmt = orderedView[i - 1].LiquidatedBalanceAmt + ov.LiquidatedAmt;
                        ov.FundReturnBalanceAmt = orderedView[i - 1].FundReturnBalanceAmt + ov.FundReturnAmt;

                        if (orderedView[i].TransactionType == ATPTEFMTransactionHistoryView.transactionType.ExpenseReceipt)
                        {
                            if (orderedView[i].IsReimbursement == true)
                            {
                                ov.DocumentBalanceAmt = orderedView[i - 1].DocumentBalanceAmt;
                                ov.BalanceAmt = ov.DocumentBalanceAmt;
                            }
                            else
                            {
                                if (ov.IsUnliquidatedRequest == true)
                                {
                                    ov.DocumentBalanceAmt = orderedView[i - 1].DocumentBalanceAmt + ov.FundReturnAmt;
                                    ov.BalanceAmt = ov.DocumentBalanceAmt;
                                }
                                else
                                {
                                    ov.DocumentBalanceAmt = (orderedView[i - 1].DocumentBalanceAmt + ov.FundReturnAmt) - ov.UnliquidatedAmt;
                                    ov.BalanceAmt = ov.DocumentBalanceAmt;
                                }
                            }

                        }
                        else if (orderedView[i].TransactionType == ATPTEFMTransactionHistoryView.transactionType.Reclassificaation)
                        {
                            ov.DocumentBalanceAmt = (orderedView[i - 1].DocumentBalanceAmt + ov.FundReturnAmt) - ov.UnliquidatedAmt;
                            ov.BalanceAmt = ov.DocumentBalanceAmt;
                        }
                        else if (orderedView[i].TransactionType == ATPTEFMTransactionHistoryView.transactionType.FundReimbursment)
                        {
                            ov.DocumentBalanceAmt = (orderedView[i - 1].DocumentBalanceAmt - ov.FundTransactionDocumentAmt) + ov.ReimbursementWht;
                            ov.BalanceAmt = ov.DocumentBalanceAmt;

                        }
                        else if (orderedView[i].TransactionType == ATPTEFMTransactionHistoryView.transactionType.Replenishment)
                        {
                            if (orderedView[i].HasReplenishmentCheckNbr == true)
                            {
                                ov.LiquidatedBalanceAmt = orderedView[i - 1].LiquidatedBalanceAmt - ov.CheckAmt;
                                ov.DocumentBalanceAmt = orderedView[i - 1].DocumentBalanceAmt + ov.ReplenishmentAmt;
                                ov.BalanceAmt = ov.DocumentBalanceAmt;
                            }
                            else
                            {
                                ov.DocumentBalanceAmt = orderedView[i - 1].DocumentBalanceAmt;
                                ov.BalanceAmt = ov.DocumentBalanceAmt;
                            }
                        }
                        else if (orderedView[i].TransactionType == ATPTEFMTransactionHistoryView.transactionType.FundRequest)
                        {
                            if (orderedView[i].Status == ATPTEFMFundStatusAttribute.ClosedValue || orderedView[i].CashAdvanceStatus == ATPTEFMFundTransactionCashAdvanceStatusAttribute.ForReclassificationValue)
                            {
                                ov.DocumentBalanceAmt = (orderedView[i - 1].DocumentBalanceAmt - ov.FundTransactionDocumentAmt) + ov.FundReturnAmt;
                                ov.BalanceAmt = ov.DocumentBalanceAmt;
                            }
                            else if (orderedView[i].Status == ATPTEFMFundStatusAttribute.CancelledValue)
                            {
                                ov.DocumentBalanceAmt = orderedView[i - 1].DocumentBalanceAmt;
                                ov.BalanceAmt = ov.DocumentBalanceAmt;
                            }
                            else
                            {
                                if (ov.IsUnliquidatedRequest == true)
                                {
                                    ov.DocumentBalanceAmt = orderedView[i - 1].DocumentBalanceAmt - ov.FundTransactionDocumentAmt;
                                    ov.BalanceAmt = ov.DocumentBalanceAmt;
                                }
                                else
                                {
                                    ov.DocumentBalanceAmt = orderedView[i - 1].DocumentBalanceAmt - ov.UnliquidatedAmt;
                                    ov.BalanceAmt = ov.DocumentBalanceAmt;
                                }
                            }
                        }
                        else if (orderedView[i].TransactionType == ATPTEFMTransactionHistoryView.transactionType.MonthEnd)
                        {
                            ov.DocumentBalanceAmt = orderedView[i - 1].DocumentBalanceAmt;
                            ov.BalanceAmt = ov.DocumentBalanceAmt;
                        }
                        else if (orderedView[i].TransactionType == ATPTEFMTransactionHistoryView.transactionType.IncreaseFund)
                        {
                            ov.DocumentBalanceAmt = orderedView[i - 1].DocumentBalanceAmt + ov.IncreaseFundAmt;
                            ov.BalanceAmt = ov.DocumentBalanceAmt;
                        }
                        else if (orderedView[i].TransactionType == ATPTEFMTransactionHistoryView.transactionType.DecreaseFund)
                        {
                            ov.DocumentBalanceAmt = orderedView[i - 1].DocumentBalanceAmt - ov.DecreaseFundAmt;
                            ov.BalanceAmt = ov.DocumentBalanceAmt;
                        }
                        else
                        {
                            if (orderedView[i].CheckNbr != null)
                            {
                                ov.DocumentBalanceAmt = orderedView[i].FundTransactionDocumentAmt;
                            }
                            else
                            {
                                ov.DocumentBalanceAmt = orderedView[i - 1].Status == ATPTEFMFundStatusAttribute.ClosedValue ? 0m : orderedView[i - 1].DocumentBalanceAmt;
                            }
                            ov.BalanceAmt = ov.DocumentBalanceAmt;
                        }


                    }
                    ov.SortID = ++i;
                    yield return ov;
                }

            }
        }

        protected virtual IEnumerable balanceSummary(PXAdapter adapter)
        {
            ATPTEFMFund document = Document.Current;

            List<ATPTEFMFundBalanceView> result = new List<ATPTEFMFundBalanceView>();

            if (document == null) return result;

            using (new PXConnectionScope())
            {
                var transactions = Transactions.Select().RowCast<ATPTEFMTransactionHistoryView>().ToList();

                decimal? unliquidatedAmt = 0m;
                decimal? liquidatedAmt = 0m;
                decimal? replenishmentAmt = 0m;
                decimal? fundAmt = 0m;
                decimal? unliquidateWhtAmt = 0m;
                decimal? liquidateWhtAmtWithoutReimbursementType = 0m;
                decimal? liquidateTotalWhtAmt = 0m;
                decimal? onReplenishmentAmt = 0m;
                decimal? onReplenishmentWhtAmt = 0m;

                if (transactions.Count != 0)
                {
                    ATPTEFMTransactionHistoryView lastRecord = transactions.LastOrDefault();

                    bool isCloseFund = lastRecord.TransactionType == ATPTEFMTransactionHistoryView.transactionType.CloseFund;

                    //Get the whole data fields of APInvoice DAC in the form of object
                    APInvoice apiObj = PXSelect<
                        APInvoice,
                        Where<APInvoice.refNbr, Equal<Required<APInvoice.refNbr>>,
                            And<APInvoice.docType, Equal<APDocType.invoice>>>>
                        .Select(this, document.EstablishmentRefNbr);

                    unliquidatedAmt = transactions.Where(_ => !(_.Status.Equals(ATPTEFMFundStatusAttribute.CancelledValue))).Sum(_ => _.UnliquidatedAmt.GetValueOrDefault());
                    liquidatedAmt = transactions.Where(_ => !(_.Status.Equals(ATPTEFMFundStatusAttribute.CancelledValue))).Sum(_ => _.LiquidatedAmt.GetValueOrDefault());
                    replenishmentAmt = transactions.Where(_ => _.HasReplenishmentCheckNbr.GetValueOrDefault() && !(_.Status.Equals(ATPTEFMFundStatusAttribute.CancelledValue))).Sum(_ => _.FundTransactionDocumentAmtCopy.GetValueOrDefault());

                    fundAmt = apiObj != null && (apiObj.Status == APDocStatus.Open || apiObj.Status == APDocStatus.Closed || apiObj.Status == APDocStatus.Balanced) ? document.InitialFund : 0m;
                    unliquidateWhtAmt = transactions.Where(_ => !(_.Status.Equals(ATPTEFMFundStatusAttribute.CancelledValue)) && _.Status.Equals(ATPTEFMFundStatusAttribute.OpenValue)).Sum(_ => _.WithholdingTax.GetValueOrDefault());

                    //Gina sum nya tanan WHT sa transaction history na ang fund transaction type kay Request type (Expense Receipts)
                    liquidateWhtAmtWithoutReimbursementType = transactions.Where(_ => !(_.Status.Equals(ATPTEFMFundStatusAttribute.CancelledValue)) && !string.IsNullOrEmpty(_.ReplenishmentRefNbr) && !(_.IsReimbursement.Equals(true))).Sum(_ => _.WithholdingTax.GetValueOrDefault());
                    //Gina sum nya tanan WHT its either Request and Reimbursement ang Fund transaction type (Expense Receipts)
                    liquidateTotalWhtAmt = transactions.Where(_ => !(_.Status.Equals(ATPTEFMFundStatusAttribute.CancelledValue)) && !string.IsNullOrEmpty(_.ReplenishmentRefNbr)).Sum(_ => _.WithholdingTax.GetValueOrDefault());

                    onReplenishmentAmt = transactions.Where(_ => _.HasReplenishmentCheckNbr.GetValueOrDefault() && string.IsNullOrEmpty(_.CheckNbr) && !(_.Status.Equals(ATPTEFMFundStatusAttribute.CancelledValue))).Sum(_ => _.FundTransactionDocumentAmt.GetValueOrDefault());
                    onReplenishmentWhtAmt = transactions.Where(_ => _.HasReplenishmentCheckNbr.GetValueOrDefault() && !(_.Status.Equals(ATPTEFMFundStatusAttribute.CancelledValue))).Sum(_ => _.OnReplenishmentWht.GetValueOrDefault());

                    if ((!isCloseFund) && (apiObj != null))
                    {
                        bool isRelease = false;
                        if (apiObj.Released ?? false)
                        {
                            APPayment appObj = PXSelectJoin<
                                APPayment,
                                InnerJoin<APAdjust,
                                    On<APAdjust.adjgDocType, Equal<APPayment.docType>,
                                    And<APAdjust.adjgRefNbr, Equal<APPayment.refNbr>>>>,
                                Where<APAdjust.adjdRefNbr, Equal<Required<APAdjust.adjdRefNbr>>,
                                    And<APAdjust.voided, Equal<False>>>>
                                .Select(this, document.EstablishmentRefNbr);

                            isRelease = appObj?.Status == APDocStatus.Closed ? true : false;
                        }
                        //balanceAmt = !isRelease ? 0m : transactions.LastOrDefault()?.BalanceAmt ?? 0m;
                        if (isRelease || (apiObj.IsMigratedRecord ?? false))
                            balanceAmt = transactions.LastOrDefault()?.BalanceAmt ?? 0m;
                    }
                }

                decimal? glTranLiquidatedAmt = 0m;
                decimal? glTranOnReplenishAmt = 0m;
                //Liquidated
                foreach (ATPTEFMTransactionHistoryView tranView in Transactions.Select().RowCast<ATPTEFMTransactionHistoryView>().Where(h =>
                !(h.Status.Equals(ATPTEFMFundStatusAttribute.CancelledValue)) && h.TransactionType == ATPTEFMTransactionHistoryView.transactionType.ExpenseReceipt
                && h.Status == ATPTEFMFundStatusAttribute.LiquidatedValue && string.IsNullOrEmpty(h.ReplenishmentRefNbr)).ToList())
                {
                    GLTran glTran = PXSelectJoin<
                        GLTran,
                        InnerJoin<Batch,
                            On<Batch.batchNbr, Equal<GLTran.batchNbr>>>,
                        Where<GLTran.batchNbr, Equal<Required<GLTran.batchNbr>>,
                            And<GLTran.refNbr, Equal<Required<GLTran.refNbr>>>>>
                        .Select(this, Document.Current.ExpenseBatchNbr, tranView.RefNbr);

                    if (glTran != null)
                        glTranLiquidatedAmt += glTran.CuryDebitAmt;

                }

                foreach (ATPTEFMTransactionHistoryView replenishmentHistory in Transactions.Select().RowCast<ATPTEFMTransactionHistoryView>().Where(h =>
                h.TransactionType == ATPTEFMTransactionHistoryView.transactionType.Replenishment && !(h.Status.Equals(ATPTEFMFundStatusAttribute.CancelledValue))
                && string.IsNullOrEmpty(h.CheckNbr)).ToList())
                {
                    foreach (ATPTEFMReplenishmentDetail repDetails
                        in PXSelect<
                            ATPTEFMReplenishmentDetail,
                            Where<ATPTEFMReplenishmentDetail.replenishmentNbr,
                        Equal<Required<ATPTEFMReplenishmentDetail.replenishmentNbr>>>>
                            .Select(this, replenishmentHistory.RefNbr))
                    {
                        GLTran glTran = PXSelectJoin<
                            GLTran,
                            InnerJoin<Batch,
                                On<Batch.batchNbr, Equal<GLTran.batchNbr>>>,
                            Where<GLTran.batchNbr, Equal<Required<GLTran.batchNbr>>,
                                And<GLTran.refNbr, Equal<Required<GLTran.refNbr>>>>>
                            .Select(this, Document.Current.ExpenseBatchNbr, repDetails.ExpenseReceiptNbr);

                        if (glTran != null)
                            glTranOnReplenishAmt += glTran.CuryDebitAmt;

                    }
                }
                //InsertView :
                ATPTEFMFundBalanceView item = new ATPTEFMFundBalanceView
                {
                    FundAmt = fundAmt,
                    LiquidatedAmt = (liquidatedAmt - (replenishmentAmt - liquidateWhtAmtWithoutReimbursementType)) - glTranLiquidatedAmt,
                    OnReplenishmentAmt = ((onReplenishmentAmt - liquidateTotalWhtAmt) + onReplenishmentWhtAmt) - glTranOnReplenishAmt,
                    UnliquidatedAmt = unliquidatedAmt + unliquidateWhtAmt,
                    BalanceAmt = balanceAmt
                };
                result.Add(item);
            }

            return result;
        }

        protected virtual IEnumerable aPInvoiceDocument()
        {
            // Retrieve establishment bills
            var establishmentBills = PXSelect<
                ATPTEFMFundEstablishment,
                Where<ATPTEFMFundEstablishment.fundRefNbr, Equal<Current<ATPTEFMFund.fundCD>>>>
                .Select(this)
                .Select(e => e.Record.EstablishmentRefNbr)
                .ToHashSet();

            // Retrieve AP registers
            var bills = PXSelectJoin<
                APRegister,
                InnerJoin<ATPTEFMFund,
                    On<ATPTEFMFund.fundCD, Equal<ATPTEFMAPRegisterExt.usrATPTEFMSourceRef>>>,
                Where<ATPTEFMAPRegisterExt.usrATPTEFMSourceRef, Equal<Current<ATPTEFMFund.fundCD>>,
                    And<APRegister.docType, Equal<APDocType.invoice>>>>
                .Select(this)
                .Where(b => !establishmentBills.Contains(b.Record.RefNbr))
                .OrderBy(b => b.Record.RefNbr)
                .ToList();

            return bills;
        }
        #endregion

        #region Actions
        public PXInitializeState<ATPTEFMFund> InitializeState;


        /// <remarks>
        /// 2024-12-19 :  Closing will be based on whether all statuses are 'Closed and Liquidated'. Case: 009331 {JLTG}
        /// </remarks>
        public PXAction<ATPTEFMFund> CloseFund;
        [PXButton(Connotation = PX.Data.WorkflowAPI.ActionConnotation.Warning)]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.CloseFund)]
        protected virtual IEnumerable closeFund(PXAdapter adapter)
        {
            ATPTEFMFund fund = Document.Current;
            PXCache cache = Document.Cache;

            WebDialogResult result = Document.Ask(ActionsMessages.Warning, ATPTEFMMessages.CloseFundValidation,
            MessageButtons.OKCancel, MessageIcon.Warning, true);
            //checking answer	
            if (result == WebDialogResult.OK)
            {
                ValidateCloseFund(fund);

                ATPTEFMHelper.StartLongOperation(this, adapter, delegate ()
                {
                    CloseDocEnhancement();
                });
            }

            return adapter.Get();
        }
        /// <remarks>
        /// 2025-02-04 : 009994 - (CFM2024R1/2024R2) Fund>. Add the Fund ID reference at the end of the description for fund increase, decrease and establishment.
        /// 2025-08-18 : The Branch specified in the Fund Profile should be carried under the Document Details Tab and Financial Tab of the Fund Increase Credit Adjustment. CASE: 012847 {JLG}
        /// </remarks>
        public PXAction<ATPTEFMFund> IncreaseFund;
        [PXButton(Category = "Processing"), PXUIField(DisplayName = ATPTEFMMessages.IncreaseFund)]
        public IEnumerable increaseFund(PXAdapter adapter)
        {
            if (IncreaseFundDocument.AskExt(true) == WebDialogResult.OK)
            {
                decimal increaseFundAmt = IncreaseFundDocument.Current.IncreaseFund ?? 0m;

                if (increaseFundAmt <= 0m)
                    throw new PXException(ATPTEFMMessages.AmountMustBeGreaterThanZero);

                bool requireControlTotal = ApSetup.Current.RequireControlTotal ?? false;

                ATPTEFMHelper.StartLongOperation(this, adapter, delegate ()
                {
                    using (PXTransactionScope ts = new PXTransactionScope())
                    {
                        #region Create Credit Adjustment
                        string fundType = Document.Current.FundType == ATPTEFMFundTypeAttribute.PettyCashValue ? "PCF" : "RF";

                        string description = $"{ATPTEFMMessages.IncreaseFund} of {fundType} {Document.Current.EmployeeName} {Document.Current.FundCD}";

                        APInvoiceEntry apEntry = PXGraph.CreateInstance<APInvoiceEntry>();
                        apEntry.Clear();

                        //APInvoice getRefNbr = apEntry.Document.Search<APInvoice.invoiceNbr>(Document.Current.FundCD);
                        int countBill = PXSelect<
                            APInvoice,
                            Where<APInvoice.invoiceNbr, Contains<Required<APInvoice.invoiceNbr>>>>
                            .Select(this, Document.Current.FundCD)
                            .Count;

                        APInvoice invoice = apEntry.Document.Insert(new APInvoice
                        {
                            DocType = APDocType.CreditAdj,
                            BranchID = Document.Current.BranchID,
                            VendorID = Document.Current.CustodianID,
                            InvoiceNbr = GetInvoiceNbr(),
                            DocDesc = description,
                            Hold = true,
                            OrigRefNbr = Document.Current.FundCD
                        });

                        ATPTEFMAPRegisterExt invExt = invoice.GetExtension<ATPTEFMAPRegisterExt>();
                        invExt.UsrATPTEFMSourceType = Attributes.ATPTEFMSourceTypeAttribute.Funds;
                        invExt.UsrATPTEFMSourceTranType = Attributes.ATPTEFMSourceTranTypeAttribute.IncreaseFund;
                        invExt.UsrATPTEFMSourceRef = Document.Current.FundCD;
                        invExt.UsrATPTEFMIsAmountRestrictedBill = true;

                        APTran transaction = apEntry.Transactions.Insert(new APTran
                        {
                            TranDesc = description,
                            BranchID = Document.Current.BranchID,
                            AccountID = Document.Current.AccountID,
                            SubID = Document.Current.SubID,
                            CuryLineAmt = increaseFundAmt,
                            ProjectID = Document.Current.ProjectID,
                            TaskID = Document.Current.ProjectTaskID,
                            CostCodeID = Document.Current.CostCodeID
                        });


                        transaction.TaxCategoryID = null;
                        apEntry.Transactions.Update(transaction);

                        invoice.CuryID = Document.Current.CuryID;
                        invoice.Hold = (Setup?.Current?.RequireApprovalOnFundIncreaseCredAdj ?? false) ? true : false;

                        if ((Setup?.Current?.RequireApprovalOnFundIncreaseCredAdj ?? false) == false)
                        {
                            APInvoiceEntry.APInvoiceEntryDocumentExtension invoiceBaseGraphExtension = apEntry.GetExtension<APInvoiceEntry.APInvoiceEntryDocumentExtension>();
                            invoiceBaseGraphExtension.SuppressApproval();
                        }

                        if (requireControlTotal)
                        {
                            decimal? curyDocBal = invoice.CuryDocBal;
                            invoice.CuryOrigDocAmt = curyDocBal;
                        }

                        apEntry.Document.Update(invoice);
                        apEntry.Save.Press();
                        #endregion

                        #region Establishment of Fund (transaction History)

                        ATPTEFMFundTransactionHistoryView sortNbr = CurrentTransactionHistoryView.Select().LastOrDefault();
                        ATPTEFMFundTransactionHistoryView establishHistory = CurrentTransactionHistoryView.Insert(new ATPTEFMFundTransactionHistoryView
                        {
                            FundRefNbr = Document.Current.FundCD,
                            TransactionType = ATPTEFMTransactionHistoryView.transactionType.IncreaseFund,
                            RefNbr = invoice.RefNbr,
                            OrderDate = invoice.DocDate,
                            Status = invoice.Status,
                            TransactionDate = invoice.DocDate,
                            CuryFundTransactionDocumentAmt = increaseFundAmt,
                            SortNbr = $"{sortNbr.SortNbr}-I{invoice.RefNbr}",
                            CuryBalanceAmt = Document.Current.CuryBalanceAmt,
                            FundBranchID = Document.Current.BranchID
                        });
                        CurrentTransactionHistoryView.Update(establishHistory);
                        Save.Press();
                        #endregion

                        ts.Complete();
                    }
                });
            }
            IncreaseFundDocument.Cache.Clear();

            return adapter.Get();
        }
        /// <remarks>
        /// 2025-02-04 : 009994 - (CFM2024R1/2024R2) Fund>. Add the Fund ID reference at the end of the description for fund increase, decrease and establishment.
        /// 2025-08-18 : The Branch specified in the Fund Profile should be carried under the Document Details Tab and Financial Tab of the Fund Decrease Debit Adjustment. CASE: 012847 {JLG}
        /// </remarks>
        public PXAction<ATPTEFMFund> DecreaseFund;
        [PXButton(Category = "Processing"), PXUIField(DisplayName = ATPTEFMMessages.DecreaseFund)]
        public IEnumerable decreaseFund(PXAdapter adapter)
        {
            if (DecreaseFundDocument.AskExt(true) == WebDialogResult.OK)
            {
                decimal decreaseFundAmt = DecreaseFundDocument.Current.DecreaseFund ?? 0m;

                if (decreaseFundAmt <= 0m)
                    throw new PXException(ATPTEFMMessages.AmountMustBeGreaterThanZero);

                bool requireControlTotal = ApSetup.Current.RequireControlTotal ?? false;

                ATPTEFMHelper.StartLongOperation(this, adapter, delegate ()
                {
                    using (PXTransactionScope ts = new PXTransactionScope())
                    {
                        var balanceSummary = Document.Current;

                        if (decreaseFundAmt > balanceSummary.CuryBalanceAmt)
                            throw new Exception(Messages.ATPTEFMMessages.DecreaseAmtIsGreaterThanBalance);

                        #region Create Debit Adjustment
                        string fundType = Document.Current.FundType == ATPTEFMFundTypeAttribute.PettyCashValue ? "PCF" : "RF";

                        string description = $"{ATPTEFMMessages.DecreaseFund} of {fundType} {Document.Current.EmployeeName} {Document.Current.FundCD}";

                        APInvoiceEntry apEntry = PXGraph.CreateInstance<APInvoiceEntry>();
                        apEntry.Clear();

                        //APInvoice getRefNbr = apEntry.Document.Search<APInvoice.invoiceNbr>(Document.Current.FundCD);
                        int countBill = PXSelect<
                            APInvoice,
                            Where<APInvoice.invoiceNbr, Contains<Required<APInvoice.invoiceNbr>>>>
                            .Select(this, Document.Current.FundCD)
                            .Count;

                        APInvoice invoice = apEntry.Document.Insert(new APInvoice
                        {
                            DocType = APDocType.DebitAdj,
                            BranchID = Document.Current.BranchID,
                            VendorID = Document.Current.CustodianID,
                            InvoiceNbr = GetInvoiceNbr(),
                            DocDesc = description,
                            Hold = true,
                            OrigRefNbr = Document.Current.FundCD
                        });

                        ATPTEFMAPRegisterExt invExt = invoice.GetExtension<ATPTEFMAPRegisterExt>();
                        invExt.UsrATPTEFMSourceType = Attributes.ATPTEFMSourceTypeAttribute.Funds;
                        invExt.UsrATPTEFMSourceTranType = Attributes.ATPTEFMSourceTranTypeAttribute.DecreaseFund;
                        invExt.UsrATPTEFMSourceRef = Document.Current.FundCD;
                        invExt.UsrATPTEFMIsAmountRestrictedBill = true;

                        APTran transaction = apEntry.Transactions.Insert(new APTran
                        {
                            TranDesc = description,
                            BranchID = Document.Current.BranchID,
                            AccountID = Document.Current.AccountID,
                            SubID = Document.Current.SubID,
                            CuryLineAmt = decreaseFundAmt,
                            ProjectID = Document.Current.ProjectID,
                            TaskID = Document.Current.ProjectTaskID,
                            CostCodeID = Document.Current.CostCodeID
                        });


                        transaction.TaxCategoryID = null;
                        apEntry.Transactions.Update(transaction);

                        invoice.CuryID = Document.Current.CuryID;
                        invoice.Hold = (Setup?.Current?.RequireApprovalOnFundDecreaseDebAdj ?? false) ? true : false;

                        if ((Setup?.Current?.RequireApprovalOnFundDecreaseDebAdj ?? false) == false)
                        {
                            APInvoiceEntry.APInvoiceEntryDocumentExtension invoiceBaseGraphExtension = apEntry.GetExtension<APInvoiceEntry.APInvoiceEntryDocumentExtension>();
                            invoiceBaseGraphExtension.SuppressApproval();
                        }

                        if (requireControlTotal)
                        {
                            decimal? curyDocBal = invoice.CuryDocBal;
                            invoice.CuryOrigDocAmt = curyDocBal;
                        }

                        apEntry.Document.Update(invoice);
                        apEntry.Save.Press();
                        #endregion

                        #region Insert Transaction History

                        ATPTEFMFundTransactionHistoryView lastRecord = CurrentTransactionHistoryView.Select().LastOrDefault();
                        string sortNbr = lastRecord.SortNbr;

                        if (lastRecord.TransactionType == ATPTEFMTransactionHistoryView.transactionType.ExpenseReceipt)
                        {
                            ATPTEFMFundTransaction fundTran = PXSelectJoin<
                                ATPTEFMFundTransaction,
                                InnerJoin<ATPTEFMFundTransactionReceiptDetail,
                                    On<ATPTEFMFundTransaction.refNbr, Equal<ATPTEFMFundTransactionReceiptDetail.fundTransactionRefNbr>>>,
                                Where<ATPTEFMFundTransactionReceiptDetail.expenseReceiptRefNbr, Equal<Required<ATPTEFMFundTransactionReceiptDetail.expenseReceiptRefNbr>>>>
                                .Select(this, lastRecord.RefNbr);


                            if (fundTran.FundTransactionType == ATPTEFMFundTransactionTypeAttribute.ReimbursementValue)
                            {
                                int countReceipts = PXSelect<
                                    ATPTEFMFundTransactionReceiptDetail,
                                    Where<ATPTEFMFundTransactionReceiptDetail.fundTransactionRefNbr, Equal<Required<ATPTEFMFundTransactionReceiptDetail.fundTransactionRefNbr>>>>
                                    .Select(this, fundTran.RefNbr)
                                    .Count + 1;

                                sortNbr = $"FT-{fundTran.RefNbr}-{countReceipts}";
                            }
                        }

                        ATPTEFMFundTransactionHistoryView establishHistory = CurrentTransactionHistoryView.Insert(new ATPTEFMFundTransactionHistoryView
                        {
                            FundRefNbr = Document.Current.FundCD,
                            TransactionType = ATPTEFMTransactionHistoryView.transactionType.DecreaseFund,
                            RefNbr = invoice.RefNbr,
                            OrderDate = invoice.DocDate,
                            Status = invoice.Status,
                            TransactionDate = invoice.DocDate,
                            CuryFundTransactionDocumentAmt = decreaseFundAmt,
                            SortNbr = $"{sortNbr}-D{invoice.RefNbr}",
                            CuryBalanceAmt = Document.Current.CuryBalanceAmt,
                            FundBranchID = Document.Current.BranchID
                        });
                        CurrentTransactionHistoryView.Update(establishHistory);
                        Save.Press();
                        #endregion
                        ts.Complete();
                    }
                });
            }
            DecreaseFundDocument.Cache.Clear();
            return adapter.Get();
        }

        /// <remarks>
        /// 2024-09-11 : "Current Date should be the date of Bill created" - CASE: 007499 {JLG}
        /// 2025-02-04 : 009994 - (CFM2024R1/2024R2) Fund>. Add the Fund ID reference at the end of the description for fund increase, decrease and establishment.
        /// 2025-08-18 : The Branch specified in the Fund Profile should be carried under the Document Details Tab and Financial Tab of the Fund Establishment Bill. CASE: 012847 {JLG}
        /// </remarks>
        public PXAction<ATPTEFMFund> ReleaseDocument;
        [PXButton()]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.Release)]
        protected virtual IEnumerable releaseDocument(PXAdapter adapter)
        {
            PXCache cache = Document.Cache;
            ATPTEFMFund fund = Document.Current;

            ATPTEFMHelper.StartLongOperation(this, adapter, delegate ()
            {
                using (PXTransactionScope ts = new PXTransactionScope())
                {
                    if (fund == null) return;

                    if (fund.ReplenishmentLimit == ATPTEFMReplenishmentStringListAttribute.PercentValue && fund.ReplenishPointPercent <= Decimal.Zero)
                    {
                        cache.RaiseExceptionHandling<ATPTEFMFund.replenishPointPercent>(fund, fund.ReplenishPointPercent, ATPTEFMHelper.GetPropertyException(fund, Messages.ATPTEFMMessages.ReplenishPointPercentMustBeGreaterThanZero, PXErrorLevel.Error));
                    }
                    if (fund.ReplenishmentLimit == ATPTEFMReplenishmentStringListAttribute.AmountValue && fund.ReplenishmentAmt <= Decimal.Zero)
                    {
                        cache.RaiseExceptionHandling<ATPTEFMFund.replenishmentAmt>(fund, fund.ReplenishmentAmt, ATPTEFMHelper.GetPropertyException(fund, Messages.ATPTEFMMessages.ReplenishAmountMustBeGreaterThanZero, PXErrorLevel.Error));
                    }

                    #region Create AP Bill
                    string fundType = fund.FundType == ATPTEFMFundTypeAttribute.PettyCashValue ? "PCF" : "RF";

                    string description = PXLocalizer.LocalizeFormat(Messages.ATPTEFMMessages.SystemGeneratedEstablishmentFund, fundType, fund.EmployeeName, fund.FundCD);

                    APInvoiceEntry apEntry = PXGraph.CreateInstance<APInvoiceEntry>();

                    APInvoice invoice = apEntry.Document.Insert(new APInvoice
                    {
                        DocType = APDocType.Invoice,
                        InvoiceNbr = GetInvoiceNbr(),
                        BranchID = fund.BranchID,
                        VendorID = fund.CustodianID,
                        DocDesc = description,
                        OpenDoc = true,
                        Released = false,
                        Hold = true,
                        OrigRefNbr = fund.FundCD
                    });

                    invoice = DoAdditionalCreateApBillProcess(invoice);
                    invoice = apEntry.Document.Update(invoice);

                    APTran transaction = apEntry.Transactions.Insert(new APTran
                    {
                        TranDesc = description,
                        AccountID = fund.AccountID,
                        SubID = fund.SubID,
                        CuryLineAmt = fund.CuryInitialFund,
                        ProjectID = fund.ProjectID,
                        TaskID = fund.ProjectTaskID,
                        CostCodeID = fund.CostCodeID
                    });

                    transaction.TaxCategoryID = null;
                    transaction = apEntry.Transactions.Update(transaction);

                    invoice.Hold = (Setup.Current?.IsRequireApprovalOnFundEstablishment ?? false) ? true : false;
                    invoice.CuryID = fund.CuryID;
                    invoice.CuryDocBal = fund.CuryInitialFund;
                    invoice.CuryOrigDocAmt = fund.CuryInitialFund; //AP document is out of balance

                    ATPTEFMAPRegisterExt invoiceExt = invoice.GetExtension<ATPTEFMAPRegisterExt>();

                    invoiceExt.UsrATPTEFMSourceType = ATPTEFMSourceTypeAttribute.Funds;
                    invoiceExt.UsrATPTEFMSourceRef = fund.FundCD;
                    invoiceExt.UsrATPTEFMIsAmountRestrictedBill = true;
                    invoiceExt.UsrATPTEFMIsFundEstablishmentBill = true;

                    if ((Setup.Current?.IsRequireApprovalOnFundEstablishment ?? false) == false)
                    {
                        APInvoiceEntry.APInvoiceEntryDocumentExtension invoiceBaseGraphExtension = apEntry.GetExtension<APInvoiceEntry.APInvoiceEntryDocumentExtension>();
                        invoiceBaseGraphExtension.SuppressApproval();
                    }

                    invoice = apEntry.Document.Update(invoice);
                    apEntry.Save.Press();

                    #endregion

                    #region Update Fund
                    ATPTEFMFundMaint fundMaint = PXGraph.CreateInstance<ATPTEFMFundMaint>();
                    fundMaint.Document.Current = fund;

                    ATPTEFMFundEstablishment establishment = fundMaint.Establishment.Insert(new ATPTEFMFundEstablishment
                    {
                        FundRefNbr = fundMaint.Document.Current.FundCD,
                        EstablishmentRefNbr = invoice.RefNbr
                    });
                    establishment = fundMaint.Establishment.Update(establishment);

                    #region Establishment of Fund (transaction History)

                    ATPTEFMFundTransactionHistoryView establishHistory = fundMaint.CurrentTransactionHistoryView.Insert(new ATPTEFMFundTransactionHistoryView
                    {
                        FundRefNbr = fundMaint.Document.Current.FundCD,
                        TransactionType = ATPTEFMTransactionHistoryView.transactionType.Establishment,
                        FundBranchID = fundMaint.Document.Current.BranchID,
                        RefNbr = invoice.RefNbr,
                        OrderDate = invoice.DocDate,
                        FundType = null,
                        Status = invoice.Status,
                        TransactionDate = invoice.DocDate,
                        CuryFundTransactionDocumentAmt = invoice.CuryOrigDocAmt,
                        CuryDocumentBalanceAmt = invoice.CuryOrigDocAmt,
                        ProjectID = fundMaint.Document.Current.ProjectID,
                        ProjectTaskID = fundMaint.Document.Current.ProjectTaskID,
                        CostCodeID = fundMaint.Document.Current.CostCodeID,
                        SortNbr = $"EF-{invoice.RefNbr}",
                    });

                    fundMaint.CurrentTransactionHistoryView.Update(establishHistory);
                    #endregion

                    fundMaint.Document.Current.EstablishmentRefNbr = invoice.RefNbr;
                    fundMaint.Document.Current.Status = ATPTEFMFundStatusAttribute.OpenValue;
                    fundMaint.Document.Current.Released = true;
                    fundMaint.Document.UpdateCurrent();
                    fundMaint.Save.Press();
                    #endregion
                    ts.Complete();
                }
            });

            return adapter.Get();
        }

        public PXAction<ATPTEFMFund> ViewInvoice;
        [PXButton]
        [PXUIField(Visible = false)]
        protected virtual IEnumerable viewInvoice(PXAdapter adapter)
        {
            ViewAPInvoice(Document.Current.EstablishmentRefNbr);

            return adapter.Get();
        }

        public PXAction<ATPTEFMFund> ViewUnreplenishedBill;
        [PXButton]
        [PXUIField(Visible = false)]
        protected virtual IEnumerable viewUnreplenishedBill(PXAdapter adapter)
        {
            if (APInvoiceDocument.Current != null)
            {
                RedirectionToOrigDoc.TryRedirect(APInvoiceDocument.Current.DocType, APInvoiceDocument.Current.RefNbr, BatchModule.AP, preferPrimaryDocForm: true);
            }
            return adapter.Get();
        }

        public PXAction<ATPTEFMFund> ViewLinkToCheck;
        [PXButton]
        [PXUIField(Visible = false)]
        protected virtual IEnumerable viewLinkToCheck(PXAdapter adapter)
        {
            if (APPaymentDocument.Current != null)
            {
                RedirectionToOrigDoc.TryRedirect(APPaymentDocument.Current.DocType, APPaymentDocument.Current.RefNbr, BatchModule.AP, preferPrimaryDocForm: true);
            }
            return adapter.Get();
        }

        public PXAction<ATPTEFMFund> ViewCloseFundInvoice;
        [PXButton]
        [PXUIField(Visible = false)]
        protected virtual IEnumerable viewCloseFundInvoice(PXAdapter adapter)
        {
            ATPTEFMFund document = Document.Current;

            if (document == null) return adapter.Get();

            if (document.CloseFundRefNbr == null) return adapter.Get();

            ViewAPInvoice(document.CloseFundRefNbr, APDocType.DebitAdj);

            return adapter.Get();
        }

        public PXAction<ATPTEFMFund> ViewExpenseBatchNbr;
        [PXButton]
        [PXUIField(Visible = false)]
        protected virtual IEnumerable viewExpenseBatchNbr(PXAdapter adapter)
        {
            ATPTEFMFund document = Document.Current;

            if (document == null) return adapter.Get();

            if (document.ExpenseBatchNbr == null) return adapter.Get();

            ViewBatchNbr(document.ExpenseBatchNbr);

            return adapter.Get();
        }

        public PXAction<ATPTEFMFund> ViewTransaction;
        [PXButton]
        [PXUIField(Visible = false)]
        protected virtual IEnumerable viewTransaction(PXAdapter adapter)
        {
            //ATPTEFMTransactionHistoryView transaction = Transactions.Current;
            ATPTEFMFundTransactionHistoryView transaction = CurrentTransactionHistoryView.Current;

            if (transaction == null) return adapter.Get();

            if (transaction.RefNbr == null) return adapter.Get();

            switch (transaction.TransactionType)
            {
                case ATPTEFMFundTransactionTypeAttribute.CloseFundValue:
                    ViewAPInvoice(transaction.RefNbr, APDocType.DebitAdj);
                    break;
                case ATPTEFMFundTransactionTypeAttribute.EstablishmentValue:
                    ViewAPInvoice(transaction.RefNbr);
                    break;
                case ATPTEFMFundTransactionTypeAttribute.ReplenishmentValue:
                    ViewReplenishment(transaction.RefNbr);
                    break;
                case ATPTEFMFundTransactionTypeAttribute.ExpenseReceiptValue:
                    ViewExpenseReceipt(transaction.RefNbr);
                    break;
                case ATPTEFMFundTransactionTypeAttribute.ReclassificationValue:
                    ViewExpenseReceipt(transaction.RefNbr);
                    break;
                case ATPTEFMFundTransactionTypeAttribute.MonthEndValue:
                    ViewMonthEnd(transaction.RefNbr);
                    break;
                case ATPTEFMFundTransactionTypeAttribute.IncreaseFundValue:
                    ViewIncreaseFund(transaction.RefNbr);
                    break;
                case ATPTEFMFundTransactionTypeAttribute.DecreaseFundValue:
                    ViewDecreaseFund(transaction.RefNbr);
                    break;
                default:
                    ViewFundTransaction(transaction.RefNbr);
                    break;
            }



            return adapter.Get();
        }

        /// <remarks>
        /// 2025-03-18 : Funds_Close Funds_Checks and Payments Navigation. CASEID: 010749  {JLG} <br/>             
        /// </remarks>
        public PXAction<ATPTEFMFund> ViewCheck;
        [PXButton]
        [PXUIField(Visible = false)]
        protected virtual IEnumerable viewCheck(PXAdapter adapter)
        {
            ATPTEFMFundTransactionHistoryView transaction = CurrentTransactionHistoryView.Current;

            if (transaction == null) return adapter.Get();

            if (transaction.CheckNbr == null) return adapter.Get();

            switch (transaction.TransactionType)
            {
                case ATPTEFMFundTransactionTypeAttribute.VoidedCheckValue:
                    ViewCheckTrasnction(transaction.CheckNbr, APDocType.VoidCheck);
                    break;
                case ATPTEFMFundTransactionTypeAttribute.DecreaseFundValue:
                case ATPTEFMFundTransactionTypeAttribute.CloseFundValue:
                    ViewCheckTrasnction(transaction.CheckNbr, APDocType.Refund);
                    break;
                default:
                    ViewCheckTrasnction(transaction.CheckNbr, APDocType.Check);
                    break;
            }
            return adapter.Get();
        }

        public PXAction<ATPTEFMFund> ViewReplenishmentER;
        [PXButton]
        [PXUIField(Visible = false)]
        protected virtual IEnumerable viewReplenishmentER(PXAdapter adapter)
        {
            ATPTEFMFundTransactionHistoryView transaction = CurrentTransactionHistoryView.Current;

            ATPTEFMReplenishmentEntry graph = PXGraph.CreateInstance<ATPTEFMReplenishmentEntry>();
            graph.Clear();
            graph.Replenishments.Current = graph.Replenishments.Search<ATPTEFMReplenishment.replenishmentNbr>(transaction.ReplenishmentRefNbr);
            throw new PXRedirectRequiredException(graph, false, "Replenishment") { Mode = PXBaseRedirectException.WindowMode.NewWindow };
        }

        public PXAction<ATPTEFMFund> ViewReverseBatch;
        [PXButton]
        [PXUIField(Visible = false)]
        protected virtual IEnumerable viewReverseBatch(PXAdapter adapter)
        {
            ATPTEFMFundTransactionHistoryView transaction = CurrentTransactionHistoryView.Current;

            if (transaction?.ReversingJournalBatchNbr != null)
            {
                ViewReverseBatchTransaction(transaction.ReversingJournalBatchNbr);
            }
            return adapter.Get();
        }

        public PXAction<ATPTEFMFund> PrintFundEstablishment;
        [PXButton(Category = "Reports")]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.PrintFundEstablishmentForm)]
        public IEnumerable printFundEstablishment(PXAdapter adapter)
        {
            foreach (ATPTEFMFund fund in adapter.Get<ATPTEFMFund>())
            {
                ATPTEFMFundMaint graph = PXGraph.CreateInstance<ATPTEFMFundMaint>();

                Dictionary<string, string> parameters = new Dictionary<string, string>();
                parameters["RefNbr"] = fund.FundCD;
                parameters["RequestedBy"] = Accessinfo.UserName;

                var report = new PXReportRequiredException(parameters, "ATPT6402", "Fund Establishment");

                throw new PXRedirectWithReportException(graph, report, "Preview");
            }

            return adapter.Get();
        }

        #endregion

        #region Event Handlers
        /// <remarks>
        /// 2025-02-13 :Fund Custodian Default Value [Enhancement]. CASEID: 009704  {JLG} <br/> 
        /// 2025-02-26 : Remove raise field updated. Change implementation to cury field defaulting.
        /// </remarks>
        protected virtual void _(Events.RowSelected<ATPTEFMFund> e)
        {
            ATPTEFMFund fund = e.Row;
            if (fund == null) return;

            #region Conditional variables
            bool isIncreaseDecreaseBtnEnable = (fund.Status == ATPTEFMFundStatusAttribute.ActiveValue) ? true : false;
            bool IsMigration = Preferences.Current.IsFundsMigration ?? false;
            #endregion

            #region Views Allow Insert/Update/Delete/Select
            Document.AllowUpdate = fund.Status == ATPTEFMFundStatusAttribute.HoldValue;
            Document.AllowDelete = fund.Status == ATPTEFMFundStatusAttribute.HoldValue;
            CurrentDocument.AllowUpdate = fund.Status == ATPTEFMFundStatusAttribute.HoldValue;
            CurrentEstablishment.AllowSelect = IsMigration;
            #endregion

            #region Enable/Disable/Visibility fields
            PXUIFieldAttribute.SetEnabled<ATPTEFMFund.status>(e.Cache, null, IsMigration);
            PXUIFieldAttribute.SetEnabled<ATPTEFMFund.establishmentRefNbr>(e.Cache, null, IsMigration);
            PXUIFieldAttribute.SetEnabled<ATPTEFMFund.closeFundRefNbr>(e.Cache, null, IsMigration);
            PXUIFieldAttribute.SetEnabled<ATPTEFMFund.expenseBatchNbr>(e.Cache, null, IsMigration);
            PXUIFieldAttribute.SetVisible<ATPTEFMFund.isImported>(e.Cache, null, IsMigration);
            PXUIFieldAttribute.SetVisible<ATPTEFMFund.isActive>(e.Cache, null, IsMigration);
            PXUIFieldAttribute.SetEnabled<ATPTEFMFund.curyID>(e.Cache, fund, CurrentCustodian?.Current?.AllowOverrideCury ?? false);

            #endregion

            #region Enable or Disable Buttons
            ReleaseDocument.SetEnabled((fund.Released ?? false) == false && (fund.Status == ATPTEFMFundStatusAttribute.BalancedValue));
            Delete.SetEnabled((bool)!fund.Released);

            CloseFund.SetEnabled(fund.Status == ATPTEFMFundStatusAttribute.ActiveValue);

            //Increase and Decrease Fund Enhancement
            IncreaseFund.SetEnabled(isIncreaseDecreaseBtnEnable);
            DecreaseFund.SetEnabled(isIncreaseDecreaseBtnEnable);
            IncreaseFund.SetVisible(isIncreaseDecreaseBtnEnable);
            DecreaseFund.SetVisible(isIncreaseDecreaseBtnEnable);

            #endregion
            //CurrentEstablishment.AllowSelect = IsMigration;
            //ActivateFundImport.SetEnabled(IsMigration);

            #region Persisting Check
            if (fund.ReplenishmentLimit == ATPTEFMReplenishmentStringListAttribute.PercentValue)
                PXDefaultAttribute.SetPersistingCheck<ATPTEFMFund.replenishPointPercent>(e.Cache, fund, PXPersistingCheck.NullOrBlank);
            else
                PXDefaultAttribute.SetPersistingCheck<ATPTEFMFund.replenishmentAmt>(e.Cache, fund, PXPersistingCheck.NullOrBlank);
            #endregion

            #region Conditional Display Name
            if (fund.ATPTEFMValidateAmountReceivedAndAmountReleased ?? false)
                PXUIFieldAttribute.SetDisplayName<ATPTEFMTransactionHistoryView.fundReturnAmt>(CurrentTransactionHistoryView.Cache, ATPTEFMMessages.ActualReturn);
            else PXUIFieldAttribute.SetDisplayName<ATPTEFMTransactionHistoryView.fundReturnAmt>(CurrentTransactionHistoryView.Cache, ATPTEFMMessages.FundReturn);
            #endregion

        }
        protected virtual void _(Events.FieldDefaulting<ATPTEFMFund, ATPTEFMFund.aTPTEFMValidateAmountReceivedAndAmountReleased> e)
        {
            ATPTEFMFund row = e.Row;
            if (row == null) return;

            e.NewValue = Setup.Current.ValidateAmountReceivedAndReleasedUponLiquidation;
            e.Cancel = true;
        }

        protected virtual void _(Events.FieldDefaulting<ATPTEFMFund, ATPTEFMFund.curyID> e)
        {
            ATPTEFMFund row = e.Row;
            if (row == null) return;

            CurrentCustodian.Current = PXSelect<
                EPEmployee,
                Where<EPEmployee.bAccountID, Equal<Required<EPEmployee.bAccountID>>>>
                .Select(this, row.CustodianID);

        }
        /// <remarks>
        /// 2024-10-31 : Throws an error if denominated GL account is different from the transaction currency.. CASEID: 008426 {JLG} <br/>   
        /// 2025-04-08 : 011108 - QMAZ 23R2: After decreasing fund amount, error message. CASEID: 011108 {JLG} <br/>
        /// 2025-08-18 : Allow tagging of Branch other than the Branch of the Custodian in the Fund Profile CASE: 012847 {JLG}
        protected virtual void _(Events.RowPersisting<ATPTEFMFund> e)
        {
#if Version24R1
            ATPTEFMFund row = e.Row;
            if (row == null) return;

            if (row.Status.Equals(ATPTEFMFundStatusAttribute.ActiveValue) && row.ReplenishmentAmt > row.CuryFundAmt)
            {
                e.Cache.RaiseExceptionHandling<ATPTEFMFund.replenishmentAmt>(row, row.ReplenishmentAmt, ATPTEFMHelper.GetPropertyException(row, Messages.ATPTEFMMessages.ReplenishAmountMustBeEqualLowerThanInitialFund, PXErrorLevel.Error));
            }

            ValidateGlAccountCurrency(row);
            ValidateCustodianByBranch(row);
            ValidatePayeeByBranch(row);
#else
            ATPTEFMFund row = e.Row;
            if (row == null) return;

            if (row.Status.Equals(ATPTEFMFundStatusAttribute.ActiveValue) && row.ReplenishmentAmt > row.CuryFundAmt)
            {
                e.Cache.RaiseExceptionHandling<ATPTEFMFund.replenishmentAmt>(row, row.ReplenishmentAmt, ATPTEFMHelper.GetPropertyException(row, Messages.ATPTEFMMessages.ReplenishAmountMustBeEqualLowerThanInitialFund, PXErrorLevel.Error));
            }

            ValidateGlAccountCurrency(row);
            ValidateCustodianByBranch(row);
            ValidatePayeeByBranch(row);
#endif
        }
        /// <remarks>
        /// 2024-12-12 : Change calculation to Multi Currency field Enhancement {JLTG} <br/>             
        /// </remarks>
        protected virtual void _(Events.FieldUpdated<ATPTEFMFund, ATPTEFMFund.replenishPointPercent> e)
        {
            ATPTEFMFund row = (ATPTEFMFund)e.Row;

            if (row.ReplenishmentLimit == ATPTEFMReplenishmentStringListAttribute.PercentValue)
                row.ReplenishmentAmt = (row.ReplenishPointPercent / 100) * row.CuryInitialFund;
        }
        /// <remarks>
        /// 2024-12-12 : Change calculation to Multi Currency field Enhancement {JLTG} <br/>             
        /// </remarks>
        protected virtual void _(Events.FieldUpdated<ATPTEFMFund, ATPTEFMFund.replenishmentAmt> e)
        {
            ATPTEFMFund row = (ATPTEFMFund)e.Row;

            if (row.ReplenishmentLimit == ATPTEFMReplenishmentStringListAttribute.AmountValue)
                row.ReplenishPointPercent = (row.ReplenishmentAmt / row.CuryInitialFund) * 100;
        }
        /// <remarks>
        /// 2024-12-12 : Change calculation to Multi Currency field Enhancement {JLTG} <br/>             
        /// </remarks>
        protected virtual void _(Events.FieldUpdated<ATPTEFMFund, ATPTEFMFund.fundTransactionPointPercent> e)
        {
#if Version24R1
            ATPTEFMFund row = (ATPTEFMFund)e.Row;

            if (row.FundTransactionLimit == ATPTEFMReplenishmentStringListAttribute.PercentValue)
                row.FundTransactionAmt = (row.FundTransactionPointPercent / 100) * row.CuryInitialFund;
#else
            if (Setup?.Current?.EnableFundTransactionLimit ?? false)
            {
                ATPTEFMFund row = (ATPTEFMFund)e.Row;

                if (row.FundTransactionLimit == ATPTEFMReplenishmentStringListAttribute.PercentValue)
                    row.FundTransactionAmt = (row.FundTransactionPointPercent / 100) * row.CuryInitialFund;
            }
#endif
        }
        /// <remarks>
        /// 2024-12-12 : Change calculation to Multi Currency field Enhancement {JLTG} <br/>             
        /// </remarks>
        protected virtual void _(Events.FieldUpdated<ATPTEFMFund, ATPTEFMFund.fundTransactionAmt> e)
        {
            ATPTEFMFund row = e.Row;
            if (row == null) return;

            if (row.FundTransactionLimit == ATPTEFMReplenishmentStringListAttribute.AmountValue)
                row.FundTransactionPointPercent = (row.FundTransactionAmt / row.CuryInitialFund) * 100;
        }
        /// <remarks>
        /// 2024-12-12 : Change calculation to Multi Currency field Enhancement {JLTG} <br/>    
        /// 2025-04-29 : When the initial fund is modified after the reversal of the establishment bill, both the replenishment and fund transaction are automatically adjusted whether by PERCENT or by AMOUNT. Case: 011412 {JLTG}
        /// </remarks>
        protected virtual void _(Events.FieldUpdated<ATPTEFMFund, ATPTEFMFund.curyInitialFund> e)
        {
            ATPTEFMFund row = (ATPTEFMFund)e.Row;

            if (row.CuryInitialFund == decimal.Zero) return;

            if (row.ReplenishmentLimit == ATPTEFMReplenishmentStringListAttribute.AmountValue)
                row.ReplenishPointPercent = (row.ReplenishmentAmt / row.CuryInitialFund) * 100;

            if (row.ReplenishmentLimit == ATPTEFMReplenishmentStringListAttribute.PercentValue)
                row.ReplenishmentAmt = (row.ReplenishPointPercent / 100) * row.CuryInitialFund;

            if (row.FundTransactionLimit == ATPTEFMReplenishmentStringListAttribute.AmountValue)
                row.FundTransactionPointPercent = (row.FundTransactionAmt / row.CuryInitialFund) * 100;

            if (row.FundTransactionLimit == ATPTEFMReplenishmentStringListAttribute.PercentValue)
                row.FundTransactionAmt = (row.FundTransactionPointPercent / 100) * row.CuryInitialFund;

        }
        protected virtual void _(Events.FieldUpdated<ATPTEFMFund, ATPTEFMFund.fundType> e)
        {
            ATPTEFMFund row = e.Row;
            if (row is null) return;

            if (Document.Cache.GetStatus(Document.Current) == PXEntryStatus.Updated && e.ExternalCall)
            {
                row.FundCD = AutoNumberAttribute.GetNewNumberSymbol<ATPTEFMSetup.fundNumberingID>(Document.Cache, row);
                Document.Cache.Clear();
                CurrentDocument.Cache.Clear();
            }
        }
        protected virtual void _(Events.FieldUpdated<ATPTEFMFund, ATPTEFMFund.custodianID> e)
        {
            ATPTEFMFund row = e.Row;
            if (row is null) return;

            CurrentCustodian.Current = PXSelect<
                EPEmployee,
                Where<EPEmployee.bAccountID, Equal<Required<EPEmployee.bAccountID>>>>
                .Select(this, e.NewValue);

            if (PXAccess.FeatureInstalled<FeaturesSet.multicurrency>())
            {
                CurrencyInfo info = CurrencyInfoAttribute.SetDefaults<ATPTEFMFund.curyInfoID>(e.Cache, e.Row);

                string message = PXUIFieldAttribute.GetError<CurrencyInfo.curyEffDate>(currencyinfo.Cache, info);
                if (string.IsNullOrEmpty(message) == false)
                {
                    e.Cache.RaiseExceptionHandling<ATPTEFMFund.documentDate>(e.Row, ((ATPTEFMFund)e.Row).DocumentDate, ATPTEFMHelper.GetPropertyException((ATPTEFMFund)e.Row, message, PXErrorLevel.Warning));
                }
                if (info != null)
                {
                    row.CuryID = info.CuryID;
                }
            }
        }

        protected virtual void _(Events.FieldUpdated<ATPTEFMFund, ATPTEFMFund.payeeID> e)
        {
            ATPTEFMFund row = e.Row;
            if (row is null) return;

            CurrentPayee.Current = PXSelect<
                EPEmployee,
                Where<EPEmployee.bAccountID, Equal<Required<EPEmployee.bAccountID>>>>
                .Select(this, e.NewValue);
        }

        protected virtual void ATPTEFMFund_DocumentDate_FieldUpdated(PXCache sender, PXFieldUpdatedEventArgs e)
        {
            CurrencyInfoAttribute.SetEffectiveDate<ATPTEFMFund.documentDate>(sender, e);
        }
        #endregion

        #region CurrencyInfo events
        protected virtual void CurrencyInfo_CuryID_FieldDefaulting(PXCache sender, PXFieldDefaultingEventArgs e)
        {
            if (PXAccess.FeatureInstalled<FeaturesSet.multicurrency>())
            {
                EPEmployee employee = EPEmployee.Select();
                if (employee != null && !string.IsNullOrEmpty(employee.CuryID))
                {
                    e.NewValue = employee.CuryID;
                    e.Cancel = true;
                }
            }
        }
        protected virtual void CurrencyInfo_CuryRateTypeID_FieldDefaulting(PXCache sender, PXFieldDefaultingEventArgs e)
        {
            if (PXAccess.FeatureInstalled<FeaturesSet.multicurrency>())
            {
                if (CurrentCustodian?.Current != null && !string.IsNullOrEmpty(CurrentCustodian?.Current?.CuryRateTypeID))
                {
                    e.NewValue = CurrentCustodian?.Current?.CuryRateTypeID;
                    e.Cancel = true;
                }
                else
                {
                    CMSetup cmsetup = PXSelect<CMSetup>.Select(this);
                    if (cmsetup != null)
                    {
                        e.NewValue = cmsetup?.ARRateTypeDflt;
                        e.Cancel = true;
                    }
                }

            }
        }
        protected virtual void CurrencyInfo_CuryEffDate_FieldDefaulting(PXCache sender, PXFieldDefaultingEventArgs e)
        {
            if (this.Document.Cache.Current != null)
            {
                e.NewValue = ((ATPTEFMFund)Document?.Cache?.Current)?.DocumentDate;
                e.Cancel = true;
            }
        }

        protected virtual void CurrencyInfo_RowSelected(PXCache cache, PXRowSelectedEventArgs e)
        {
            CurrencyInfo info = e.Row as CurrencyInfo;
            if (info != null)
            {
                bool curyEnabled = info.AllowUpdate(this.Document.Cache);

                // Get employee info to check rate override permission
                if (Document.Current?.CustodianID != null)
                {
                    if (CurrentCustodian.Current != null && !(bool)CurrentCustodian.Current.AllowOverrideRate)
                    {
                        curyEnabled = false;
                    }
                }

                PXUIFieldAttribute.SetEnabled<CurrencyInfo.curyRateTypeID>(cache, info, curyEnabled);
                PXUIFieldAttribute.SetEnabled<CurrencyInfo.curyEffDate>(cache, info, curyEnabled);
                PXUIFieldAttribute.SetEnabled<CurrencyInfo.sampleCuryRate>(cache, info, curyEnabled);
                PXUIFieldAttribute.SetEnabled<CurrencyInfo.sampleRecipRate>(cache, info, curyEnabled);
            }
        }

        #endregion

        #region Methods
        protected string GetInvoiceNbr() => IsRaiseErrorDuplicateVendorRef() ? $"{Document.Current.FundCD}-{GetBillCount()}" : Document.Current.FundCD;
        protected int GetBillCount() => PXSelectJoin<
            APInvoice,
            InnerJoin<APRegister,
                On<APRegister.refNbr, Equal<APInvoice.refNbr>,
                And<APRegister.docType, Equal<APInvoice.docType>>>>,
            Where<APRegister.origRefNbr, Equal<Required<APRegister.origRefNbr>>>>
            .Select(this, Document.Current.FundCD)
            .Count + 1;
        protected bool IsRaiseErrorDuplicateVendorRef() => ApSetup.Current.RaiseErrorOnDoubleInvoiceNbr ?? false;

        public virtual APInvoice DoAdditionalCreateApBillProcess(APInvoice row) { return row; }
        public virtual APTran DoAdditionalRelease(APTran row, EPExpenseClaimDetails er) { return row; }
        /// <remarks>
        /// 2025-07-08 : 012116 - Adds logic to retrigger Tax category ID to execute Philtax DFT ATC. {RRS}
        /// 2025-07-21 : (CFM2023R2 and UP) Fund> Show the Expense Receipt Details tab with the expense receipt in the Unreplenished expense bill generated from the closing of fund. CASEID: 011538 {JLG} <br/>
        /// 2025-10-10 : (CFM2024R1 staging)Fund closing> The reclass account should not appear in triplicate in the unreplenished receipts bill. CASEID: 013249 {JLG} <br/>
        /// 2025-09-05 : pass branchid value to invoice debit * receipt invoices summary and detail branchid field. 012847 : RFS
        /// 2025-10-10 : (CFM2024R1 staging)Fund closing> The reclass account should not appear in triplicate in the unreplenished receipts bill. CASEID: 013249 {JLG} <br/>
        /// </remarks>
        private void CloseDocEnhancement()
        {
            ATPTEFMFund fund = Document.Current;
            bool requireControlTotal = ApSetup.Current.RequireControlTotal ?? false;

            using (PXTransactionScope ts = new PXTransactionScope())
            {
                string fundType = fund.FundType == ATPTEFMFundTypeAttribute.PettyCashValue ? "PCF" : "RF";

                string description = PXLocalizer.LocalizeFormat(Messages.ATPTEFMMessages.SystemGeneratedCloseFund, fundType, fund.EmployeeName, fund.FundCD);

                string batchNbr = string.Empty;


                #region Create Debit Adjustment
                APInvoiceEntry apEntry = PXGraph.CreateInstance<APInvoiceEntry>();

                string debitRefNbr = string.Empty;

                APInvoice invoice = apEntry.Document.Insert(new APInvoice
                {
                    DocType = APDocType.DebitAdj,
                    InvoiceNbr = GetInvoiceNbr(),
                    VendorID = fund.CustodianID,
                    DocDesc = description,
                    OrigRefNbr = fund.FundCD,
                    BranchID = fund.BranchID
                });

                invoice = DoAdditionalCreateApBillProcess(invoice);
                ATPTEFMAPRegisterExt debitInv = invoice.GetExtension<ATPTEFMAPRegisterExt>();

                debitInv.UsrATPTEFMSourceType = ATPTEFMSourceTypeAttribute.Funds;
                debitInv.UsrATPTEFMSourceRef = fund.FundCD;
                debitInv.UsrATPTEFMIsCloseFundDebitAdjBill = true;
                debitInv.UsrATPTEFMIsAmountRestrictedBill = true;

                APTran transaction = apEntry.Transactions.Insert(new APTran
                {
                    TranDesc = description,
                    AccountID = fund.AccountID,
                    SubID = fund.SubID,
                    CuryLineAmt = fund.CuryFundAmt,
                    ProjectID = fund.ProjectID,
                    TaskID = fund.ProjectTaskID,
                    CostCodeID = fund.CostCodeID,
                    BranchID = fund.BranchID
                });


                transaction.TaxCategoryID = null;
                apEntry.Transactions.Update(transaction);

                if (requireControlTotal)
                {
                    decimal? curyDocBal = invoice.CuryDocBal;
                    invoice.CuryOrigDocAmt = curyDocBal;
                }

                invoice.CuryID = fund.CuryID;
                invoice = apEntry.Document.Update(invoice);

                apEntry.Save.Press();
                debitRefNbr = invoice.RefNbr;


                bool isRequireDebitApproval = (invoice.Status == APDocStatus.Hold) ? true : false;

                if (!isRequireDebitApproval)
                {
                    APDocumentRelease.ReleaseDoc(new List<APRegister>() { invoice }, isMassProcess: false);
                }
                #endregion

                #region Establishment of Fund (transaction History)

                ATPTEFMFundTransactionHistoryView sortNbr = this.CurrentTransactionHistoryView.Select().LastOrDefault();
                ATPTEFMFundTransactionHistoryView establishHistory = this.CurrentTransactionHistoryView.Insert(new ATPTEFMFundTransactionHistoryView
                {
                    FundRefNbr = fund.FundCD,
                    TransactionType = ATPTEFMTransactionHistoryView.transactionType.CloseFund,
                    RefNbr = invoice.RefNbr,
                    OrderDate = invoice.DocDate,
                    Status = invoice.Status,
                    TransactionDate = invoice.DocDate,
                    CuryFundTransactionDocumentAmt = invoice.CuryOrigDocAmt,
                    CuryBalanceAmt = fund.CuryBalanceAmt,
                    ProjectID = fund.ProjectID,
                    ProjectTaskID = fund.ProjectTaskID,
                    CostCodeID = fund.CostCodeID,
                    SortNbr = $"{sortNbr.SortNbr}-X{invoice.RefNbr}",
                });
                this.CurrentTransactionHistoryView.Update(establishHistory);
                #endregion

                #region Create Receipts Bill

                HashSet<string> receiptsList = new HashSet<string>();

                foreach (ATPTEFMFundTransactionHistoryView tranView in CurrentTransactionHistoryView.Select().RowCast<ATPTEFMFundTransactionHistoryView>()
                    .Where(h => h.TransactionType.Equals(ATPTEFMFundTransactionTypeAttribute.ExpenseReceiptValue)
                    && h.Status != ATPTEFMFundStatusAttribute.CancelledValue
                    && string.IsNullOrEmpty(h.ReplenishmentRefNbr)).ToList())
                {
                    receiptsList.Add(tranView.RefNbr);
                }

                foreach (ATPTEFMFundTransactionHistoryView tranView in CurrentTransactionHistoryView.Select().RowCast<ATPTEFMFundTransactionHistoryView>()
                    .Where(h => h.TransactionType.Equals(ATPTEFMFundTransactionTypeAttribute.ReclassificationValue)
                    && h.Status != ATPTEFMFundStatusAttribute.CancelledValue
                    && string.IsNullOrEmpty(h.ReplenishmentRefNbr)).ToList())
                {
                    receiptsList.Add(tranView.RefNbr);
                }

                List<EPExpenseClaimDetails> epDetails = new List<EPExpenseClaimDetails>();
                foreach (var receiptList in receiptsList)
                {
                    PXResultset<EPExpenseClaimDetails> listOfReceipts = PXSelect<
                        EPExpenseClaimDetails,
                        Where<EPExpenseClaimDetails.claimDetailCD, Equal<Required<EPExpenseClaimDetails.claimDetailCD>>>>
                        .Select<PXResultset<EPExpenseClaimDetails, ATPTEFMFundTransactionReceiptDetail, ATPTEFMFundTransaction>>(this, receiptList);

                    epDetails.Add(listOfReceipts);
                }

                var receiptsGrouped = epDetails.Select(
               result => (EPExpenseClaimDetails)result).GroupBy(
               item => Tuple.Create(
                   item.TaxZoneID,
                   item.TaxCalcMode
               )).ToDictionary(x => x.Key, group => group.ToList());

                foreach (var receipts in receiptsGrouped)
                {
                    #region Create Invoice
                    apEntry.Clear();
                    string receiptInvNbr = string.Empty;
                    APInvoice recInv = apEntry.Document.Insert(new APInvoice
                    {
                        OrigModule = "EP",
                        OrigRefNbr = fund.FundCD,
                        DocType = APDocType.Invoice,
                        DocDesc = $"Unreplenished Expenses {fundType} {fund.FundCD}",
                        PaymentsByLinesAllowed = false,
                        VendorID = fund.CustodianID,
                        InvoiceNbr = GetInvoiceNbr(),
                        TaxZoneID = receipts.Key.Item1,
                        TaxCalcMode = receipts.Key.Item2,
                        BranchID = fund.BranchID
                    });

                    Decimal? totalAmount = 0;

                    recInv.CuryID = fund.CuryID;
                    recInv = DoAdditionalCreateApBillProcess(recInv);
                    recInv = apEntry.Document.Update(recInv);

                    ATPTEFMAPRegisterExt invoiceExt = recInv.GetExtension<ATPTEFMAPRegisterExt>();

                    invoiceExt.UsrATPTEFMSourceType = ATPTEFMSourceTypeAttribute.Funds;
                    invoiceExt.UsrATPTEFMSourceRef = fund.FundCD;
                    invoiceExt.UsrATPTEFMIsUnreplenishedReceiptBill = true;

                    foreach (EPExpenseClaimDetails er in receipts.Value.ToList())
                    {
                        ATPTEFMEPExpenseClaimDetailsExt claimExt = er.GetExtension<ATPTEFMEPExpenseClaimDetailsExt>();

                        APTran tranDoc = new APTran();
                        tranDoc.TranDesc = er.TranDesc;
                        tranDoc.InventoryID = er.InventoryID;
                        tranDoc.Qty = er.Qty;
                        tranDoc.CuryUnitCost = er.CuryUnitCost;
                        tranDoc.CuryLineAmt = er.CuryExtCost;
                        tranDoc.AccountID = er.ExpenseAccountID;
                        tranDoc.SubID = er.ExpenseSubID;
                        tranDoc.ProjectID = er.ContractID;
                        tranDoc.TaskID = er.TaskID;
                        tranDoc.CostCodeID = er.CostCodeID;
                        tranDoc.BranchID = er.BranchID;
                        bool reTriggerTax = tranDoc?.TaxCategoryID == er?.TaxCategoryID;
                        tranDoc.TaxCategoryID = (claimExt.UsrATPTEFMIsReclassifyDoc ?? false) ? null : er.TaxCategoryID;
                        tranDoc = DoAdditionalRelease(tranDoc, er);

                        apEntry.Transactions.Update(tranDoc);
                        if (reTriggerTax && claimExt.UsrATPTEFMIsReclassifyDoc != true)
                        {
                            var currentTaxCategory = tranDoc.TaxCategoryID;
                            apEntry.Transactions.Cache.SetValueExt<APTran.taxCategoryID>(tranDoc, null);
                            apEntry.Transactions.Cache.Update(tranDoc);
                            apEntry.Transactions.Cache.SetValueExt<APTran.taxCategoryID>(tranDoc, currentTaxCategory);
                            apEntry.Transactions.Cache.Update(tranDoc);
                        }

                        totalAmount += er.CuryExtCost;
                    }

                    //recInv.TaxZoneID = receipts.Key.Item1;
                    if (requireControlTotal)
                    {
                        decimal? recCuryDocBal = recInv.CuryDocBal;
                        recInv.CuryOrigDocAmt = recCuryDocBal;
                        recInv.CuryTaxAmt = recInv.CuryTaxTotal;
                    }
                    recInv = apEntry.Document.Update(recInv);
                    apEntry.Save.Press();
                    receiptInvNbr = recInv.RefNbr;


                    foreach (EPExpenseClaimDetails er in receipts.Value.ToList())
                    {
                        ATPTEFMEPExpenseClaimDetailsExt claimExt = er.GetExtension<ATPTEFMEPExpenseClaimDetailsExt>();
                        #region Update Expense receipt status to released
                        ATPTEFMExpenseClaimDetails.Current = ATPTEFMExpenseClaimDetails.Select(er.ClaimDetailCD);

                        if (ATPTEFMExpenseClaimDetails.Current != null)
                        {
                            ATPTEFMExpenseClaimDetails.Current.Status = EPExpenseClaimDetailsStatus.ReleasedStatus;
                            ATPTEFMExpenseClaimDetails.Current.APDocType = recInv.DocType;
                            ATPTEFMExpenseClaimDetails.Current.APRefNbr = receiptInvNbr;
                            claimExt.UsrATPTEFMIsUnreplenish = true;

                            ATPTEFMExpenseClaimDetails.UpdateCurrent();
                        }

                        #endregion
                    }
                    bool isRequireBillApproval = (recInv.Status == APDocStatus.Hold) ? true : false;

                    if (!isRequireDebitApproval && !isRequireBillApproval)
                    {
                        APDocumentRelease.ReleaseDoc(new List<APRegister>() { recInv }, isMassProcess: false);

                        APPaymentEntry paymentEntryGraph = PXGraph.CreateInstance<APPaymentEntry>();
                        var debitAdjDoc = APPayment.PK.Find(paymentEntryGraph, APDocType.DebitAdj, debitRefNbr);

                        paymentEntryGraph.Document.Current = debitAdjDoc;

                        if (paymentEntryGraph.Document.Current != null)
                        {

                            APAdjust debitAdjDetail = new APAdjust
                            {
                                AdjdBranchID = debitAdjDoc.BranchID,
                                AdjdDocType = APDocType.Invoice,
                                AdjdRefNbr = receiptInvNbr,
                            };

                            debitAdjDetail = paymentEntryGraph.Adjustments.Insert(debitAdjDetail);
                            paymentEntryGraph.Adjustments.Update(debitAdjDetail);
                            paymentEntryGraph.Save.Press();
                            APDocumentRelease.ReleaseDoc(new List<APRegister>() { debitAdjDoc }, isMassProcess: false);
                        }
                    }
                    #endregion
                }

                #endregion

                #region Close Fund

                fund.CloseFundRefNbr = invoice.RefNbr;
                fund.Status = ATPTEFMFundStatusAttribute.PendingCloseValue;
                fund.CuryOnReplenishmentAmt = decimal.Zero;
                fund.CuryLiquidatedAmt = decimal.Zero;
                fund.ExpenseBatchNbr = batchNbr;

                Document.Update(fund);
                this.Save.Press();
                #endregion

                ts.Complete();
            }
        }
        public static PXRedirectRequiredException ViewAPInvoice(string refNbr, string docType = APDocType.Invoice)
        {
            APInvoiceEntry graph = PXGraph.CreateInstance<APInvoiceEntry>();

            graph.Clear();

            graph.Document.Current = graph.Document.Search<APInvoice.refNbr>(refNbr, docType);

            throw new PXRedirectRequiredException(graph, false, "Fund") { Mode = PXBaseRedirectException.WindowMode.NewWindow };
        }
        public static PXRedirectRequiredException ViewBatchNbr(string refNbr)
        {
            JournalEntry graph = PXGraph.CreateInstance<JournalEntry>();
            graph.Clear();
            graph.BatchModule.Current = Batch.PK.Find(graph, "GL", refNbr);

            throw new PXRedirectRequiredException(graph, false, "Journal Transactions") { Mode = PXBaseRedirectException.WindowMode.NewWindow };
        }
        public static PXRedirectRequiredException ViewFundTransaction(string refNbr)
        {
            ATPTEFMFundTransactionEntry graph = PXGraph.CreateInstance<ATPTEFMFundTransactionEntry>();

            graph.Clear();

            graph.FundTransactions.Current = graph.FundTransactions.Search<ATPTEFMFundTransaction.refNbr>(refNbr);

            throw new PXRedirectRequiredException(graph, false, Messages.ATPTEFMMessages.ATPTEFMFundTransaction) { Mode = PXBaseRedirectException.WindowMode.NewWindow };
        }
        public static PXRedirectRequiredException ViewMonthEnd(string refNbr)
        {
            ATPTEFMMonthEndEntry graph = PXGraph.CreateInstance<ATPTEFMMonthEndEntry>();

            graph.Clear();

            graph.Document.Current = graph.Document.Search<ATPTEFMMonthEnd.refNbr>(refNbr);

            throw new PXRedirectRequiredException(graph, false, Messages.ATPTEFMMessages.ATPTEFMMonthEnd) { Mode = PXBaseRedirectException.WindowMode.NewWindow };
        }
        public static PXRedirectRequiredException ViewIncreaseFund(string refNbr)
        {
            APInvoiceEntry graph = PXGraph.CreateInstance<APInvoiceEntry>();

            graph.Clear();

            graph.Document.Current = graph.Document.Search<APInvoice.refNbr>(refNbr, APDocType.CreditAdj);

            throw new PXRedirectRequiredException(graph, false, "Increase Fund") { Mode = PXBaseRedirectException.WindowMode.NewWindow };
        }
        public static PXRedirectRequiredException ViewDecreaseFund(string refNbr)
        {
            APInvoiceEntry graph = PXGraph.CreateInstance<APInvoiceEntry>();

            graph.Clear();

            graph.Document.Current = graph.Document.Search<APInvoice.refNbr>(refNbr, APDocType.DebitAdj);

            throw new PXRedirectRequiredException(graph, false, "Decrease Fund") { Mode = PXBaseRedirectException.WindowMode.NewWindow };
        }
        public static PXRedirectRequiredException ViewReplenishment(string refNbr)
        {
            ATPTEFMReplenishmentEntry graph = PXGraph.CreateInstance<ATPTEFMReplenishmentEntry>();

            graph.Clear();

            graph.Replenishments.Current = graph.Replenishments.Search<ATPTEFMReplenishment.replenishmentNbr>(refNbr);

            throw new PXRedirectRequiredException(graph, false, Messages.ATPTEFMMessages.ATPTEFMFundTransaction) { Mode = PXBaseRedirectException.WindowMode.NewWindow };
        }
        public static PXRedirectRequiredException ViewExpenseReceipt(string refNbr)
        {
            ExpenseClaimDetailEntry graph = PXGraph.CreateInstance<ExpenseClaimDetailEntry>();

            graph.Clear();

            graph.ClaimDetails.Current = graph.ClaimDetails.Search<EPExpenseClaimDetails.claimDetailCD>(refNbr);

            throw new PXRedirectRequiredException(graph, false, Messages.ATPTEFMMessages.ExpenseReceipt) { Mode = PXBaseRedirectException.WindowMode.NewWindow };
        }
        public static PXRedirectRequiredException ViewCheckTrasnction(string checkNbr, string docType)
        {
            APPaymentEntry graph = PXGraph.CreateInstance<APPaymentEntry>();

            graph.Clear();

            graph.Document.Current = graph.Document.Search<APPayment.refNbr>(checkNbr, docType);

            throw new PXRedirectRequiredException(graph, false, "Checks and Payments") { Mode = PXBaseRedirectException.WindowMode.NewWindow };
        }
        public static PXRedirectRequiredException ViewReverseBatchTransaction(string reverseBatchNbr)
        {
            JournalEntry graph = PXGraph.CreateInstance<JournalEntry>();
            graph.Clear();

            graph.BatchModule.Current = graph.BatchModule.Search<Batch.batchNbr>(reverseBatchNbr);

            throw new PXRedirectRequiredException(graph, true, "Journal Transaction") { Mode = PXBaseRedirectException.WindowMode.NewWindow };
        }
        private ATPTEFMTransactionHistoryView InitFundsTransactionHistory()
        {
            return new ATPTEFMTransactionHistoryView()
            {
                OrderDate = null,
                FundType = null,
                RefNbr = null,
                SortNbr = null,
                FundTransactionSortNbr = null,
                FundBranchID = null,
                TransactionType = null,
                Status = null,
                TransactionDate = null,
                FundTransactionDocumentAmt = 0,
                FundTransactionDocumentAmtCopy = 0,
                IncreaseFundAmt = 0,
                DecreaseFundAmt = 0,
                WithholdingTax = 0,
                OnReplenishmentWht = 0,
                ReimbursementWht = 0,
                UnliquidatedAmt = 0,
                LiquidatedAmt = 0,
                FundReturnAmt = 0,
                UnliquidatedBalanceAmt = 0,
                LiquidatedBalanceAmt = 0,
                FundReturnBalanceAmt = 0,
                DocumentBalanceAmt = 0,
                IsUnliquidatedRequest = false,
                BalanceAmt = 0,
                IsReimbursement = false,
                FundAmt = 0,
                CheckNbr = null,
                CheckAmt = null,
                ReplenishmentAmt = 0
            };
        }
        private void ValidateGlAccountCurrency(ATPTEFMFund fund)
        {
            Account account = PXSelect<
                Account,
                Where<Account.accountID, Equal<Required<Account.accountID>>>>
                .Select(this, fund.AccountID);

            if (account != null && account.CuryID != null)
            {
                if (account.CuryID != fund.CuryID)
                {
                    string errMsg = string.Format(Messages.ATPTEFMMessages.CurrencyAccountError, account.AccountCD);
                    this.Document.Cache.RaiseExceptionHandling<ATPTEFMFund.accountID>(fund, account.AccountCD,
                                    ATPTEFMHelper.GetPropertyException(fund, errMsg, PXErrorLevel.Error));
                    throw new Exception(errMsg);
                }
            }
        }
        protected virtual void ValidateCloseFund(ATPTEFMFund fund)
        {
            ValidatePendingIncreaseDecreaseFund(fund);
            ValidatePendingTransactions(fund);
            ValidateUnliquidatedAmount(fund);
            ValidateMonthEndReversal(fund);
            ValidateOpenReplenishment(fund);
        }
        protected virtual void ValidatePendingIncreaseDecreaseFund(ATPTEFMFund fund)
        {
            var pendingIncreaseDecreaseFund = CurrentTransactionHistoryView.Select()
                 .RowCast<ATPTEFMFundTransactionHistoryView>()
                 .Where(m => (m.TransactionType == ATPTEFMTransactionHistoryView.transactionType.IncreaseFund || m.TransactionType == ATPTEFMTransactionHistoryView.transactionType.DecreaseFund) &&
                             m.Status != ATPTEFMFundStatusAttribute.ClosedValue && m.Status != ATPTEFMFundStatusAttribute.ReversedValue)
                 .ToList();

            if (pendingIncreaseDecreaseFund.Count > 0)
            {
                string transactionNumbers = string.Join(", ",
                    pendingIncreaseDecreaseFund.Select(t => ((ATPTEFMFundTransactionHistoryView)t).RefNbr));
                throw new PXException(Messages.ATPTEFMMessages.CannotCloseFundWithPendingTransactions, transactionNumbers);
            }
        }

        /// <remarks>
        /// 2025-09-15 : Cannot Close Fund due to Rejected Fund Transactions CASE: 013494 : JLG
        /// </remarks>
        protected virtual void ValidatePendingTransactions(ATPTEFMFund fund)
        {
            var pendingTransactions = PXSelect<
                ATPTEFMFundTransaction,
                Where<ATPTEFMFundTransaction.fundID, Equal<Required<ATPTEFMFundTransaction.fundID>>,
                    And2<
                        Where2<
                            Where<ATPTEFMFundTransaction.fundTransactionType, Equal<ATPTEFMFundTransactionTypeAttribute.cashAdvanceValue>,
                                And<ATPTEFMFundTransaction.cashAdvanceStatus, Equal<ATPTEFMFundTransactionCashAdvanceStatusAttribute.unreleasedValue>>>,
                            Or<Where<ATPTEFMFundTransaction.fundTransactionType, Equal<ATPTEFMFundTransactionTypeAttribute.reimbursementValue>>>>,
                        And<Where<ATPTEFMFundTransaction.status, NotIn3<ATPTEFMFundStatusAttribute.cancelledValue,
                        ATPTEFMFundStatusAttribute.closedValue,
                        ATPTEFMFundStatusAttribute.rejectedValue>>>>>>
                .Select(this, fund.FundCD);

            if (pendingTransactions.Count > 0)
            {
                string transactionNumbers = string.Join(", ",
                    pendingTransactions.Select(t => ((ATPTEFMFundTransaction)t).RefNbr));
                throw new PXException(Messages.ATPTEFMMessages.CannotCloseFundWithPendingTransactions, transactionNumbers);
            }
        }

        protected virtual void ValidateUnliquidatedAmount(ATPTEFMFund fund)
        {
            if (fund?.CuryUnliquidatedAmt > decimal.Zero)
            {
                throw new Exception(Messages.ATPTEFMMessages.UnliquidatedAmtMustBeZero);
            }
        }

        protected virtual void ValidateMonthEndReversal(ATPTEFMFund fund)
        {
            var isMonthEndNoBatchNbr = CurrentTransactionHistoryView.Select()
                .RowCast<ATPTEFMFundTransactionHistoryView>()
                .Where(m => m.TransactionType == ATPTEFMTransactionHistoryView.transactionType.MonthEnd &&
                            string.IsNullOrEmpty(m.ReversingJournalBatchNbr))
                .ToList();

            if (!string.IsNullOrEmpty(isMonthEndNoBatchNbr?.LastOrDefault()?.RefNbr))
            {
                throw new Exception(Messages.ATPTEFMMessages.NoReversalEntry);
            }
        }

        protected virtual void ValidateOpenReplenishment(ATPTEFMFund fund)
        {
            bool hasOpenReplenishment = PXSelect<
                ATPTEFMReplenishment,
                Where<ATPTEFMReplenishment.fundID, Equal<Required<ATPTEFMReplenishment.fundID>>,
                    And<Where<ATPTEFMReplenishment.status, Equal<ATPTEFMReplenishmentStatusAttribute.openValue>,
                        Or<ATPTEFMReplenishment.status, Equal<ATPTEFMReplenishmentStatusAttribute.holdValue>,
                        Or<ATPTEFMReplenishment.status, Equal<ATPTEFMReplenishmentStatusAttribute.pendingValue>>>>>>>
                .Select(this, fund.FundCD)
                .Count() > 0;

            if (hasOpenReplenishment)
            {
                throw new Exception(Messages.ATPTEFMMessages.HasOpenReplenishment);
            }
        }

        protected virtual void ValidateCustodianByBranch(ATPTEFMFund fund)
        {
            if (fund is null) return;

            if (Setup?.Current?.RestrictCustodianByBranch ?? false)
            {
                EPEmployee currentCustodian = CurrentCustodian.Select().TopFirst;
                Branch empBranch = PXSelect<Branch, Where<Branch.bAccountID, Equal<Required<Branch.bAccountID>>>>.Select(this, currentCustodian?.ParentBAccountID);
                if (empBranch != null)
                {
                    if (fund.BranchID != empBranch.BranchID)
                    {
                       this.Document.Cache.RaiseExceptionHandling<ATPTEFMFund.custodianID>(fund, fund?.EmployeeName, ATPTEFMHelper.GetPropertyException(fund, string.Format(ATPTEFMMessages.CannotBeFoundInSystem, ATPTEFMMessages.CustodianID), PXErrorLevel.Error));
                    }
                }
            }
        }

        protected virtual void ValidatePayeeByBranch(ATPTEFMFund fund)
        {
            if (fund is null) return;

            if (Setup?.Current?.RestrictCustodianByBranch ?? false)
            {
                EPEmployee currentPayee = CurrentPayee.Select().TopFirst;
                Branch empBranch = PXSelect<Branch, Where<Branch.bAccountID, Equal<Required<Branch.bAccountID>>>>.Select(this, currentPayee?.ParentBAccountID);
                if (empBranch != null)
                {
                    if (fund.BranchID != empBranch.BranchID)
                    {
                        this.Document.Cache.RaiseExceptionHandling<ATPTEFMFund.payeeID>(fund, currentPayee?.AcctName, ATPTEFMHelper.GetPropertyException(fund, string.Format(ATPTEFMMessages.CannotBeFoundInSystem, ATPTEFMMessages.PayeeID), PXErrorLevel.Error));
                    }
                }
            }
        }

        #endregion

        #region EPApproval Cache Attached
        [PXDBDate()]
        [PXDefault(typeof(ATPTEFMFund.documentDate), PersistingCheck = PXPersistingCheck.Nothing)]
        protected virtual void EPApproval_DocDate_CacheAttached(PXCache sender)
        {
        }


        [PXDBInt()]
        [PXDefault(typeof(ATPTEFMFund.custodianID), PersistingCheck = PXPersistingCheck.Nothing)]
        protected virtual void EPApproval_BAccountID_CacheAttached(PXCache sender)
        {
        }

        [PXDBString(60, IsUnicode = true)]
        [PXDefault(typeof(ATPTEFMFund.descr), PersistingCheck = PXPersistingCheck.Nothing)]
        protected virtual void EPApproval_Descr_CacheAttached(PXCache sender)
        {
        }

        [PXDBDecimal(4)]
        [PXDefault(typeof(ATPTEFMFund.initialFund), PersistingCheck = PXPersistingCheck.Nothing)]
        protected virtual void EPApproval_CuryTotalAmount_CacheAttached(PXCache sender)
        {
        }

        [PXMergeAttributes(Method = MergeMethod.Append)]
        [PXDefault(typeof(Search<
            EPEmployee.defaultWorkgroupID,
            Where<EPEmployee.bAccountID, Equal<Current<ATPTEFMFund.custodianID>>>>), PersistingCheck = PXPersistingCheck.Nothing)]
        protected virtual void EPApproval_WorkgroupID_CacheAttached(PXCache sender)
        {
        }

        [PXMergeAttributes(Method = MergeMethod.Merge)]
        [PXUIField(DisplayName = ATPTEFMMessages.TaxZone, Visibility = PXUIVisibility.Visible)]
        [PXSelector(typeof(TaxZone.taxZoneID), DescriptionField = typeof(TaxZone.taxZoneID), Filterable = true)]
        protected virtual void APInvoice_TaxZoneID_CacheAttached(PXCache cache)
        {
        }
        #endregion

        #region Internal Types
        [Serializable]
        [PXCacheName("Transaction History")]
        public class ATPTEFMTransactionHistoryView : CashFundManagement.DAC.Base.ATPTEFMBase, IBqlTable
        {
            #region SortID
            [PXInt(IsKey = true)]
            [PXUIField(DisplayName = "ID")]
            public virtual int? SortID { get; set; }
            public abstract class sortID : BqlInt.Field<sortID> { }
            #endregion

            #region SortID2
            [PXInt(IsKey = true)]
            [PXUIField(DisplayName = "ID2")]
            public virtual int? SortID2 { get; set; }
            public abstract class sortID2 : BqlInt.Field<sortID2> { }
            #endregion

            #region OrderDate
            [PXDateAndTime]
            [PXUIField(DisplayName = "Order Date")]
            public virtual DateTime? OrderDate { get; set; }
            public abstract class orderDate : BqlDateTime.Field<orderDate> { }
            #endregion

            #region TransactionType
            [PXString(1, IsFixed = true)]
            [PXUIField(DisplayName = Messages.ATPTEFMMessages.FundTransactionType)]
            [transactionType.ATPTEFMList()]
            public virtual string TransactionType { get; set; }
            public abstract class transactionType : BqlString.Field<transactionType>
            {
                public const string Establishment = "E";
                public const string FundRequest = "F";
                public const string Reclassificaation = "Y";
                public const string FundReimbursment = "R";
                public const string Replenishment = "T";
                public const string CloseFund = "Q";
                public const string ExpenseReceipt = "X";
                public const string VoidedCheck = "V";
                public const string MonthEnd = "M";
                public const string IncreaseFund = "I";
                public const string DecreaseFund = "D";

                public class ATPTEFMListAttribute : PXStringListAttribute
                {
                    public ATPTEFMListAttribute()
                        : base(new[]
                        {
                            Pair(Establishment, Messages.ATPTEFMMessages.Establishment),
                            Pair(FundRequest, Messages.ATPTEFMMessages.FundRequest),
                            Pair(Reclassificaation, Messages.ATPTEFMMessages.Reclassification),
                            Pair(FundReimbursment, Messages.ATPTEFMMessages.FundReimbursment),
                            Pair(Replenishment, Messages.ATPTEFMMessages.Replenishment),
                            Pair(CloseFund, Messages.ATPTEFMMessages.CloseFund),
                            Pair(ExpenseReceipt, Messages.ATPTEFMMessages.ExpenseReceipt),
                            Pair(VoidedCheck, Messages.ATPTEFMMessages.VoidedCheck),
                            Pair(MonthEnd, Messages.ATPTEFMMessages.MonthEnd),
                            Pair(IncreaseFund, Messages.ATPTEFMMessages.IncreaseFund),
                            Pair(DecreaseFund, Messages.ATPTEFMMessages.DecreaseFund),
                        })
                    { }
                }

                public class establishment : BqlString.Constant<establishment>
                {
                    public establishment() : base(Establishment) { }
                }

                public class fundRequest : BqlString.Constant<fundRequest>
                {
                    public fundRequest() : base(FundRequest) { }
                }

                public class fundReimbursment : BqlString.Constant<fundReimbursment>
                {
                    public fundReimbursment() : base(FundReimbursment) { }
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
                public class increaseFund : BqlString.Constant<increaseFund>
                {
                    public increaseFund() : base(IncreaseFund) { }
                }
                public class decreaseFund : BqlString.Constant<decreaseFund>
                {
                    public decreaseFund() : base(DecreaseFund) { }
                }
            }
            #endregion

            #region ReferenceNbr
            [PXString(15, IsKey = true)]
            [PXUIField(DisplayName = Messages.ATPTEFMMessages.RefNbr)]
            public virtual string RefNbr { get; set; }
            public abstract class refNbr : BqlString.Field<refNbr> { }
            #endregion

            #region SortNbr
            [PXString(100, IsUnicode = true)]
            [PXUIField(DisplayName = "Sort Number")]
            public virtual string SortNbr { get; set; }
            public abstract class sortNbr : BqlString.Field<sortNbr> { }
            #endregion

            #region FundTransactionSortNbr
            [PXString(100, IsUnicode = true)]
            [PXUIField(DisplayName = Messages.ATPTEFMMessages.FundTransactionNbr)]
            public virtual string FundTransactionSortNbr { get; set; }
            public abstract class fundTransactionSortNbr : BqlString.Field<fundTransactionSortNbr> { }
            #endregion

            #region Branch
            [Branch(DisplayName = Messages.ATPTEFMMessages.Branch)]
            //[PXDefault(typeof(Default<ATPTEFMFund.branchID>))]
            //[PXUIField(DisplayName = Messages.ATPTEFMMessages.Branch)]
            public virtual int? FundBranchID { get; set; }
            public abstract class fundBranchID : BqlInt.Field<fundBranchID> { }
            #endregion

            #region Fund Type
            [PXString(1, IsFixed = true)]
            [ATPTEFMFundTypeAttribute.ATPTEFMFundType]
            //   [PXDefault(typeof(Default<ATPTEFMFund.fundType>))]
            [PXUIField(DisplayName = Messages.ATPTEFMMessages.FundType)]
            public virtual string FundType { get; set; }
            public abstract class fundType : BqlString.Field<fundType> { }
            #endregion

            #region TransactionDate
            [PXDate]
            [PXUIField(DisplayName = Messages.ATPTEFMMessages.Date)]
            public virtual DateTime? TransactionDate { get; set; }
            public abstract class transactionDate : BqlDateTime.Field<transactionDate> { }
            #endregion

            #region FundTransactionDocumentAmt
            [PXDecimal(2)]
            [PXUIField(DisplayName = Messages.ATPTEFMMessages.DocAmt)]
            [PXUnboundDefault(TypeCode.Decimal, "0.0", PersistingCheck = PXPersistingCheck.Nothing)]
            [PXFormula(null, typeof(SumCalc<ATPTEFMFund.curyBalanceAmt>))]
            public virtual decimal? FundTransactionDocumentAmt { get; set; }
            public abstract class fundTransactionDocumentAmt : BqlDecimal.Field<fundTransactionDocumentAmt> { }
            #endregion

            #region FundTransactionDocumentAmtCopy
            [PXDecimal(2)]
            [PXUIField(DisplayName = Messages.ATPTEFMMessages.DocAmt)]
            [PXUnboundDefault(TypeCode.Decimal, "0.0", PersistingCheck = PXPersistingCheck.Nothing)]
            [PXFormula(null, typeof(SumCalc<ATPTEFMFund.curyBalanceAmt>))]
            public virtual decimal? FundTransactionDocumentAmtCopy { get; set; }
            public abstract class fundTransactionDocumentAmtCopy : BqlDecimal.Field<fundTransactionDocumentAmtCopy> { }
            #endregion

            #region UnliquidatedAmt
            [PXDecimal(2)]
            [PXUnboundDefault(TypeCode.Decimal, "0.00", PersistingCheck = PXPersistingCheck.Nothing)]
            [PXUIField(DisplayName = Messages.ATPTEFMMessages.Unliquidated)]
            public virtual decimal? UnliquidatedAmt { get; set; }
            public abstract class unliquidatedAmt : BqlDecimal.Field<unliquidatedAmt> { }
            #endregion

            #region LiquidatedAmt
            [PXDecimal(2)]
            [PXUnboundDefault(TypeCode.Decimal, "0.00", PersistingCheck = PXPersistingCheck.Nothing)]
            [PXUIField(DisplayName = Messages.ATPTEFMMessages.Liquidated)]
            public virtual decimal? LiquidatedAmt { get; set; }
            public abstract class liquidatedAmt : BqlDecimal.Field<liquidatedAmt> { }
            #endregion

            #region FundReturnAmt
            [PXDecimal(2)]
            [PXUnboundDefault(TypeCode.Decimal, "0.00", PersistingCheck = PXPersistingCheck.Nothing)]
            [PXUIField(DisplayName = Messages.ATPTEFMMessages.ActualReturn)]
            public virtual decimal? FundReturnAmt { get; set; }
            public abstract class fundReturnAmt : BqlDecimal.Field<fundReturnAmt> { }
            #endregion

            #region IncreaseFundAmt
            [PXDecimal(2)]
            [PXUnboundDefault(TypeCode.Decimal, "0.00", PersistingCheck = PXPersistingCheck.Nothing)]
            [PXUIField(DisplayName = Messages.ATPTEFMMessages.IncreaseFund)]
            public virtual decimal? IncreaseFundAmt { get; set; }
            public abstract class increaseFundAmt : BqlDecimal.Field<increaseFundAmt> { }
            #endregion

            #region DecreaseFundAmt
            [PXDecimal(2)]
            [PXUnboundDefault(TypeCode.Decimal, "0.00", PersistingCheck = PXPersistingCheck.Nothing)]
            [PXUIField(DisplayName = Messages.ATPTEFMMessages.DecreaseFund)]
            public virtual decimal? DecreaseFundAmt { get; set; }
            public abstract class decreaseFundAmt : BqlDecimal.Field<decreaseFundAmt> { }
            #endregion

            #region WithholdingTax
            [PXDecimal(2)]
            [PXUnboundDefault(TypeCode.Decimal, "0.00", PersistingCheck = PXPersistingCheck.Nothing)]
            [PXUIField(DisplayName = Messages.ATPTEFMMessages.WithholdingTax)]
            public virtual decimal? WithholdingTax { get; set; }
            public abstract class withholdingTax : BqlDecimal.Field<withholdingTax> { }
            #endregion

            #region OnReplenishmentWht
            [PXDecimal(2)]
            [PXUnboundDefault(TypeCode.Decimal, "0.00", PersistingCheck = PXPersistingCheck.Nothing)]
            [PXUIField(DisplayName = Messages.ATPTEFMMessages.WithholdingTax)]
            public virtual decimal? OnReplenishmentWht { get; set; }
            public abstract class onReplenishmentWht : BqlDecimal.Field<onReplenishmentWht> { }
            #endregion

            #region ReimbursementWht
            [PXDecimal(2)]
            [PXUnboundDefault(TypeCode.Decimal, "0.00", PersistingCheck = PXPersistingCheck.Nothing)]
            [PXUIField(DisplayName = Messages.ATPTEFMMessages.WithholdingTax)]
            public virtual decimal? ReimbursementWht { get; set; }
            public abstract class reimbursementWht : BqlDecimal.Field<reimbursementWht> { }
            #endregion

            #region BalanceAmt
            [PXDecimal(2)]
            [PXUIField(DisplayName = Messages.ATPTEFMMessages.Balance)]
            [PXUnboundDefault(TypeCode.Decimal, "0.0", PersistingCheck = PXPersistingCheck.Nothing)]
            public virtual decimal? BalanceAmt { get; set; }
            public abstract class balanceAmt : BqlDecimal.Field<balanceAmt> { }
            #endregion

            #region ReplenishmentRefNbr
            [PXString]
            [PXUIField(DisplayName = Messages.ATPTEFMMessages.ReplenishmentRefNbrDetail)]
            public virtual string ReplenishmentRefNbr { get; set; }
            public abstract class replenishmentRefNbr : BqlString.Field<replenishmentRefNbr> { }
            #endregion

            #region CheckNbr
            [PXString]
            [PXUIField(DisplayName = Messages.ATPTEFMMessages.UsrRLCheckNbr)]
            public virtual string CheckNbr { get; set; }
            public abstract class checkNbr : BqlString.Field<checkNbr> { }
            #endregion

            #region CheckAmt
            [PXDecimal(2)]
            [PXUIField(DisplayName = Messages.ATPTEFMMessages.CheckAmount)]
            [PXUnboundDefault(TypeCode.Decimal, "0.0", PersistingCheck = PXPersistingCheck.Nothing)]
            public virtual decimal? CheckAmt { get; set; }
            public abstract class checkAmt : BqlDecimal.Field<checkAmt> { }
            #endregion

            #region Status
            [PXString(1, IsFixed = true)]
            [ATPTEFMFundStatusAttribute.ATPTEFMFundStatus()]
            [PXUIField(DisplayName = Messages.ATPTEFMMessages.Status)]
            public virtual string Status { get; set; }
            public abstract class status : BqlString.Field<status> { }
            #endregion

            #region Source
            [PXString(1, IsFixed = true)]
            [PXUIField(DisplayName = Messages.ATPTEFMMessages.Source)]
            [source.ATPTEFMList()]
            public virtual string Source { get; set; }
            public abstract class source : BqlString.Field<source>
            {
                public const string FundTransaction = "F";
                public const string Replenishment = "R";
                public const string ExpenseReceipt = "E";
                public const string MonthEnd = "M";

                public class ATPTEFMListAttribute : PXStringListAttribute
                {
                    public ATPTEFMListAttribute()
                        : base(new[] {  Pair(FundTransaction, Messages.ATPTEFMMessages.ATPTEFMFundTransaction),
                                        Pair(Replenishment, Messages.ATPTEFMMessages.Replenishment),
                                        Pair(ExpenseReceipt, Messages.ATPTEFMMessages.ExpenseReceipt),
                                        Pair(MonthEnd, Messages.ATPTEFMMessages.MonthEnd),})
                    { }
                }

                public class fundTransaction : BqlString.Constant<fundTransaction>
                {
                    public fundTransaction() : base(FundTransaction) { }
                }

                public class replenishment : BqlString.Constant<replenishment>
                {
                    public replenishment() : base(Replenishment) { }
                }

                public class expenseReceipt : BqlString.Constant<expenseReceipt>
                {
                    public expenseReceipt() : base(ExpenseReceipt) { }
                }

                public class monthEnd : BqlString.Constant<monthEnd>
                {
                    public monthEnd() : base(MonthEnd) { }
                }
            }
            #endregion

            #region HasReplenishmentCheckNbr
            [PXBool]
            [PXUIField(DisplayName = Messages.ATPTEFMMessages.ATPTEEFMReplinesmentDetail)]
            [PXUnboundDefault(false, PersistingCheck = PXPersistingCheck.Nothing)]
            public virtual bool? HasReplenishmentCheckNbr { get; set; }
            public abstract class hasReplenishmentCheckNbr : BqlBool.Field<hasReplenishmentCheckNbr> { }
            #endregion

            #region UnliquidatedBalanceAmt
            [PXDecimal(2)]
            [PXUnboundDefault(TypeCode.Decimal, "0.0", PersistingCheck = PXPersistingCheck.Nothing)]
            public virtual decimal? UnliquidatedBalanceAmt { get; set; }
            public abstract class unliquidatedBalanceAmt : BqlDecimal.Field<unliquidatedBalanceAmt> { }
            #endregion

            #region LiquidatedBalanceAmt
            [PXDecimal(2)]
            [PXUnboundDefault(TypeCode.Decimal, "0.0", PersistingCheck = PXPersistingCheck.Nothing)]
            public virtual decimal? LiquidatedBalanceAmt { get; set; }
            public abstract class liquidatedBalanceAmt : BqlDecimal.Field<liquidatedBalanceAmt> { }
            #endregion

            #region FundReturnBalanceAmt
            [PXDecimal(2)]
            [PXUnboundDefault(TypeCode.Decimal, "0.0", PersistingCheck = PXPersistingCheck.Nothing)]
            public virtual decimal? FundReturnBalanceAmt { get; set; }
            public abstract class fundReturnBalanceAmt : BqlDecimal.Field<fundReturnBalanceAmt> { }
            #endregion

            #region DocumentBalanceAmt
            [PXDecimal(2)]
            [PXUnboundDefault(TypeCode.Decimal, "0.0", PersistingCheck = PXPersistingCheck.Nothing)]
            public virtual decimal? DocumentBalanceAmt { get; set; }
            public abstract class documentBalanceAmt : BqlDecimal.Field<documentBalanceAmt> { }
            #endregion

            #region IsUnliquidatedRequest
            [PXBool]
            [PXUnboundDefault(false, PersistingCheck = PXPersistingCheck.Nothing)]
            public virtual bool? IsUnliquidatedRequest { get; set; }
            public abstract class isUnliquidatedRequest : BqlBool.Field<isUnliquidatedRequest> { }
            #endregion

            #region FundAmt
            [PXDecimal(2)]
            [PXUnboundDefault(TypeCode.Decimal, "0.00", PersistingCheck = PXPersistingCheck.Nothing)]
            public virtual decimal? FundAmt { get; set; }
            public abstract class fundAmt : BqlDecimal.Field<fundAmt> { }
            #endregion

            #region IsReimbursement
            [PXBool]
            [PXUIField(DisplayName = "Is Reimbursement")]
            [PXUnboundDefault(false, PersistingCheck = PXPersistingCheck.Nothing)]
            public virtual bool? IsReimbursement { get; set; }
            public abstract class isReimbursement : BqlBool.Field<isReimbursement> { }
            #endregion

            #region ReplenishmentAmt
            [PXDecimal(2)]
            [PXUnboundDefault(TypeCode.Decimal, "0.0", PersistingCheck = PXPersistingCheck.Nothing)]
            public virtual decimal? ReplenishmentAmt { get; set; }
            public abstract class replenishmentAmt : BqlDecimal.Field<replenishmentAmt> { }
            #endregion

            #region ReversingJournalBatchNbr
            [PXString(15, IsUnicode = true)]
            [PXUIField(DisplayName = ATPTEFMMessages.ReversingBatchNbr, Enabled = false)]
            public virtual string ReversingJournalBatchNbr { get; set; }
            public abstract class reversingJournalBatchNbr : BqlString.Field<reversingJournalBatchNbr> { }
            #endregion

            #region CashAdvanceStatus
            [PXString(IsFixed = true)]
            [ATPTEFMFundTransactionCashAdvanceStatusAttribute.ATPTEFMFundTrandactionCashAdvanceStatus]
            public virtual string CashAdvanceStatus { get; set; }
            public abstract class cashAdvanceStatus : BqlString.Field<cashAdvanceStatus> { }
            #endregion

            #region ProjectID
            [PXForeignReference(typeof(Field<projectID>.IsRelatedTo<PMProject.contractID>))]
            [ActiveProjectOrContractBase(DisplayName = Messages.ATPTEFMMessages.ProjectID)]
            [PXDefault(typeof(PX.Objects.PM.NonProject), PersistingCheck = PXPersistingCheck.Nothing)]
            [PXUIVisible(typeof(FeatureInstalled<FeaturesSet.projectAccounting>))]
            public virtual int? ProjectID { get; set; }
            public abstract class projectID : PX.Data.IBqlField { }
            #endregion

            #region ProjectTaskID
            [PXForeignReference(typeof(Field<projectTaskID>.IsRelatedTo<PMTask.taskID>))]
            [PXInt]
            [PXDefault(PersistingCheck = PXPersistingCheck.Nothing)]
            [PXUIRequired(typeof(
                                                                        Where<projectID, NotEqual<PX.Objects.PM.NonProject>>))]
            [PXUIField(DisplayName = Messages.ATPTEFMMessages.ProjectTaskid)]
            [PXSelector(typeof(Search<
                PMTask.taskID,
                Where<PMTask.projectID, Equal<Current<projectID>>>>), typeof(PMTask.taskCD), typeof(PMTask.description), DescriptionField = typeof(PMTask.description), SubstituteKey = typeof(PMTask.taskCD))]
            [PXUIVisible(typeof(FeatureInstalled<FeaturesSet.projectAccounting>))]
            public virtual int? ProjectTaskID { get; set; }
            public abstract class projectTaskID : PX.Data.IBqlField { }
            #endregion

            #region CostCodeID
            [PXForeignReference(typeof(Field<costCodeID>.IsRelatedTo<PMCostCode.costCodeID>))]
            [CostCode(typeof(InventoryItem.cOGSAcctID), typeof(projectTaskID), AccountType.Expense, DisplayName = "Cost Code")]
            [PXUIVisible(typeof(FeatureInstalled<FeaturesSet.costCodes>))]
            public virtual Int32? CostCodeID { get; set; }
            public abstract class costCodeID : PX.Data.BQL.BqlInt.Field<costCodeID> { }
            #endregion
        }

        [Serializable]
        [PXCacheName("Balance Summary")]
        public class ATPTEFMFundBalanceView : CashFundManagement.DAC.Base.ATPTEFMBase, IBqlTable
        {
            #region FundAmt
            [PXDecimal(2)]
            [PXUIField(DisplayName = Messages.ATPTEFMMessages.Amount, IsReadOnly = true)]
            [PXUnboundDefault(TypeCode.Decimal, "0.0", PersistingCheck = PXPersistingCheck.Nothing)]
            public virtual decimal? FundAmt { get; set; }
            public abstract class fundAmt : BqlDecimal.Field<fundAmt> { }
            #endregion

            #region BalanceAmt
            [PXDecimal(2)]
            [PXUIField(DisplayName = Messages.ATPTEFMMessages.OnHandBalanceAmount, Enabled = false)]
            [PXUnboundDefault(TypeCode.Decimal, "0.0", PersistingCheck = PXPersistingCheck.Nothing)]
            public virtual decimal? BalanceAmt { get; set; }
            public abstract class balanceAmt : BqlDecimal.Field<balanceAmt> { }
            #endregion

            #region LiquidatedAmt
            [PXDecimal(2)]
            [PXUIField(DisplayName = Messages.ATPTEFMMessages.Liquidated, Enabled = false)]
            [PXUnboundDefault(TypeCode.Decimal, "0.0", PersistingCheck = PXPersistingCheck.Nothing)]
            public virtual decimal? LiquidatedAmt { get; set; }
            public abstract class liquidatedAmt : BqlDecimal.Field<liquidatedAmt> { }
            #endregion

            #region UnliquidatedAmt
            [PXDecimal(2)]
            [PXUIField(DisplayName = Messages.ATPTEFMMessages.Unliquidated, Enabled = false)]
            [PXUnboundDefault(TypeCode.Decimal, "0.0", PersistingCheck = PXPersistingCheck.Nothing)]
            public virtual decimal? UnliquidatedAmt { get; set; }
            public abstract class unliquidatedAmt : BqlDecimal.Field<unliquidatedAmt> { }
            #endregion

            #region OnReplenishmentAmt
            [PXDecimal(2)]
            [PXUIField(DisplayName = Messages.ATPTEFMMessages.OnReplenishmentAmt, Enabled = false)]
            [PXUnboundDefault(TypeCode.Decimal, "0.0", PersistingCheck = PXPersistingCheck.Nothing)]
            public virtual decimal? OnReplenishmentAmt { get; set; }
            public abstract class onReplenishmentAmt : BqlDecimal.Field<onReplenishmentAmt> { }
            #endregion
        }

        [Serializable]
        [PXCacheName("Fund Increase")]
        public class ATPTEFMIncreaseFund : CashFundManagement.DAC.Base.ATPTEFMBase, IBqlTable
        {
            #region IncreaseFund
            [PXDecimal(2)]
            [PXUIField(DisplayName = Messages.ATPTEFMMessages.IncreasedBy)]
            [PXUnboundDefault(TypeCode.Decimal, "0.0", PersistingCheck = PXPersistingCheck.Nothing)]
            public virtual decimal? IncreaseFund { get; set; }
            public abstract class increaseFund : BqlDecimal.Field<increaseFund> { }
            #endregion
        }

        [Serializable]
        [PXCacheName("Fund Decrease")]
        public class ATPTEFMDecreaseFund : CashFundManagement.DAC.Base.ATPTEFMBase, IBqlTable
        {
            #region DecreaseFund
            [PXDecimal(2)]
            [PXUIField(DisplayName = Messages.ATPTEFMMessages.DecreaseBy)]
            [PXUnboundDefault(TypeCode.Decimal, "0.0", PersistingCheck = PXPersistingCheck.Nothing)]
            public virtual decimal? DecreaseFund { get; set; }
            public abstract class decreaseFund : BqlDecimal.Field<decreaseFund> { }
            #endregion
        }

        #endregion
    }
}