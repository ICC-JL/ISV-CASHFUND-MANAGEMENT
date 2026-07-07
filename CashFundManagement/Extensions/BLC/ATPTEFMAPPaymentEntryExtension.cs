using System;
using PX.Data;
using PX.Objects.AP;
using PX.Objects.CR;
using System.Collections;
using CashFundManagement.DAC;
using CashFundManagement.BLC;
using CashFundManagement.DAC.Setup;
using CashFundManagement.Attributes;
using CashFundManagement.Extensions.DAC;
using CashFundManagement.Messages;
using PX.Api;
using PX.Objects.CM;
using static CashFundManagement.BLC.ATPTEFMFundMaint;
using static PX.Objects.TX.CSTaxCalcType;
using CashFundManagement.Helper;
using PX.Objects.FA.DepreciationMethods.Parameters;
using PX.Objects.Common.Extensions;
using static CashFundManagement.DAC.ATPTEFMCashAdvance;
using PX.Objects.GL;
using PX.Objects.EP;
using static PX.Data.Maintenance.GI.GIResult;
using System.Linq;
using PX.Objects.CA;

namespace CashFundManagement.Extensions.BLC
{
    /// <remarks>
    /// 2024-08-14 : Adds multi-tenant support. {RRS}
    /// </remarks>
    public class ATPTEFMAPPaymentEntry_Extension : PXGraphExtension<APPaymentEntry>
    {
#if Version23R2
        public static bool IsActive() => true;
#else
        public static bool IsActive() => ATPTEFMPrefetchSetup.IsActive;
#endif

        #region Views
        public PXSetup<ATPTEFMCASetup> ATPTEFMPreferences;

        public PXSelect<
            ATPTEFMCashAdvance,
            Where<ATPTEFMCashAdvance.cashAdvanceNbr, Equal<Current<APPayment.origRefNbr>>>>
            ATPTEFMRelatedCashAdvance;

        [PXViewName("Transaction History")]
        public PXSelect<
            ATPTEFMFundTransactionHistoryView,
            Where<ATPTEFMFundTransactionHistoryView.refNbr, Equal<Required<ATPTEFMFundTransactionHistoryView.refNbr>>>>
            ATPTEFMTransactionHistoryView;

        [PXViewName("Check and Payment Ref Nbr")]
        public PXSelect<
            ATPTEFMFundTransactionHistoryView,
            Where<ATPTEFMFundTransactionHistoryView.checkNbr, Equal<Required<ATPTEFMFundTransactionHistoryView.checkNbr>>>>
            ATPTEFMTransactionHistoryCheckNbr;

        [PXViewName("Fund")]
        public PXSelect<
            ATPTEFMFund,
            Where<ATPTEFMFund.fundCD, Equal<Required<ATPTEFMFund.fundCD>>>>
            ATPTEFMFund;

        public PXSelect<
            ATPTEFMCashAdvance,
            Where<ATPTEFMCashAdvance.cashAdvanceNbr,
                Equal<Required<ATPTEFMCashAdvance.cashAdvanceNbr>>>>
            ATPTRelatedCashAdvance;
        #endregion

        #region Actions
        public delegate IEnumerable ReleaseDelegate(PXAdapter adapter);
        [PXOverride]
        public IEnumerable Release(PXAdapter adapter, ReleaseDelegate baseMethod)
        {
            UpdateCashAdvance();
            ValidateReplenishmentCurrency();
            ValidateFundSourceCurrency();
            return baseMethod(adapter);
        }

        /// <remarks>
        /// 2025-01-17 :For fund related transactions. Payee is the default Account name under the Remittance tab. CASE: 009626 {JLG}
        /// 2025-05-26 :[RFC] No generated Payment ref. under check and payment CASE: 011631 {JLG} 
        /// </remarks>
        public delegate void CreatePaymentDelegate(APInvoice apdoc, String paymentType);
        [PXOverride]
        public void CreatePayment(APInvoice apdoc, String paymentType, CreatePaymentDelegate baseMethod)
        {
            baseMethod(apdoc, paymentType);

            ATPTEFMAPRegisterExt apDocExt = apdoc.GetExtension<ATPTEFMAPRegisterExt>();

            if (apDocExt != null)
            {
                if (apDocExt.UsrATPTEFMTranType == ATPTEFMExpenseTypeAttribute.RequestforPayment)
                {
                    Base.Document.Current.GetExtension<ATPTEFMAPRegisterExt>().UsrATPTEFMTranType = apDocExt.UsrATPTEFMTranType;
                    Base.Document.Current.GetExtension<ATPTEFMAPRegisterExt>().UsrATPTEFMReqType = apDocExt.UsrATPTEFMReqType;
                    Base.Document.Current.GetExtension<ATPTEFMAPRegisterExt>().UsrATPTEFMLiqNbr = apDocExt.UsrATPTEFMLiqNbr;
                }
                else
                {
                    Base.Document.Current.GetExtension<ATPTEFMAPRegisterExt>().UsrATPTEFMSourceType = apDocExt.UsrATPTEFMSourceType;
                    Base.Document.Current.GetExtension<ATPTEFMAPRegisterExt>().UsrATPTEFMSourceRef = apDocExt.UsrATPTEFMSourceRef;
                    Base.Document.Current.GetExtension<ATPTEFMAPRegisterExt>().UsrATPTEFMSourceReqClass = apDocExt.UsrATPTEFMSourceReqClass;
                }

                if (apDocExt.UsrATPTEFMSourceType == ATPTEFMSourceTypeAttribute.Funds)
                {
                    ATPTEFMFund fund = PXSelect<
                        ATPTEFMFund,
                        Where<ATPTEFMFund.fundCD, Equal<Required<ATPTEFMFund.fundCD>>>>
                        .Select(Base, apDocExt.UsrATPTEFMSourceRef);

                    if (fund != null)
                    {
                        if (fund.CustodianID != fund.PayeeID)
                        {
                            APContact oContact = Base.Remittance_Contact.Current;
                            APAddress oAddress = Base.Remittance_Address.Current;
                            BAccount bAccount = BAccount.PK.Find(Base, (int)fund.PayeeID);
                            if (bAccount != null)
                            {
                                Contact contact = Contact.PK.Find(Base, bAccount.DefContactID);
                                Address address = Address.PK.Find(Base, bAccount.DefAddressID);

                                oContact.OverrideContact = true;
                                oContact.FullName = bAccount.AcctName;
                                oContact.Attention = contact.Attention;
                                oContact.Phone1 = contact.Phone1;
                                oContact.Email = contact.EMail;
                                Base.Remittance_Contact.UpdateCurrent();

                                oAddress.OverrideAddress = true;
                                oAddress.AddressLine1 = address.AddressLine1;
                                oAddress.AddressLine2 = address.AddressLine2;
                                oAddress.City = address.City;
                                oAddress.CountryID = address.CountryID;
                                oAddress.State = address.State;
                                oAddress.PostalCode = address.PostalCode;
                                Base.Remittance_Address.UpdateCurrent();
                            }
                        }
                    }
                }

                ATPTEFMFund fundRelatedReplenishment = PXSelectJoin<
                    ATPTEFMFund,
                    InnerJoin<ATPTEFMReplenishment,
                        On<ATPTEFMReplenishment.fundID, Equal<ATPTEFMFund.fundCD>>>,
                    Where<ATPTEFMReplenishment.replenishmentNbr, Equal<Required<APInvoice.origRefNbr>>>>
                    .Select(Base, apdoc.OrigRefNbr);

                if (fundRelatedReplenishment != null)
                {
                    APContact oContact = Base.Remittance_Contact.Current;
                    APAddress oAddress = Base.Remittance_Address.Current;
                    BAccount bAccount = BAccount.PK.Find(Base, (int)fundRelatedReplenishment.PayeeID);

                    if (bAccount != null)
                    {
                        Contact contact = Contact.PK.Find(Base, bAccount.DefContactID);
                        Address address = Address.PK.Find(Base, bAccount.DefAddressID);

                        oContact.OverrideContact = true;
                        oContact.FullName = bAccount.AcctName;
                        oContact.Attention = contact.Attention;
                        oContact.Phone1 = contact.Phone1;
                        oContact.Email = contact.EMail;
                        Base.Remittance_Contact.UpdateCurrent();

                        oAddress.OverrideAddress = true;
                        oAddress.AddressLine1 = address.AddressLine1;
                        oAddress.AddressLine2 = address.AddressLine2;
                        oAddress.City = address.City;
                        oAddress.CountryID = address.CountryID;
                        oAddress.State = address.State;
                        oAddress.PostalCode = address.PostalCode;
                        Base.Remittance_Address.UpdateCurrent();
                    }

                }

                if (apDocExt.UsrATPTEFMSourceType == ATPTEFMSourceTypeAttribute.CashAdvance)
                {
                    PaymentMethodAccount cashAcc = PXSelect<PaymentMethodAccount,
                        Where<PaymentMethodAccount.cashAccountID, Equal<Required<PaymentMethodAccount.cashAccountID>>,
                        And<PaymentMethodAccount.paymentMethodID, Equal<Required<PaymentMethodAccount.paymentMethodID>>>>>.Select(Base, apdoc.PayAccountID, apdoc.PayTypeID);

                    if (cashAcc != null)
                    {
                        if (cashAcc.APAutoNextNbr != true)
                        {
                            ATPTEFMCashAdvance cashAdv = PXSelect<
                            ATPTEFMCashAdvance,
                            Where<ATPTEFMCashAdvance.cashAdvanceNbr, Equal<Required<ATPTEFMCashAdvance.cashAdvanceNbr>>>>
                            .Select(Base, apDocExt.UsrATPTEFMSourceRef);

                            if (cashAdv != null)
                            {
                                Base.Document.Cache.SetValueExt<APPayment.extRefNbr>(Base.Document.Current, (apDocExt.UsrATPTEFMSourceRef + "-1"));
                            }
                        }
                    }
                    else
                    {
                        ATPTEFMCashAdvance cashAdv = PXSelect<
                         ATPTEFMCashAdvance,
                         Where<ATPTEFMCashAdvance.cashAdvanceNbr, Equal<Required<ATPTEFMCashAdvance.cashAdvanceNbr>>>>
                         .Select(Base, apDocExt.UsrATPTEFMSourceRef);

                        if (cashAdv != null)
                        {
                            Base.Document.Cache.SetValueExt<APPayment.extRefNbr>(Base.Document.Current, (apDocExt.UsrATPTEFMSourceRef + "-1"));
                        }
                    }
                }

                Base.Document.UpdateCurrent();
            }
        }

        public delegate void VoidCheckProcDelegate(APPayment doc);
        [PXOverride]
        public void VoidCheckProc(APPayment doc, VoidCheckProcDelegate baseMethod)
        {
            baseMethod(doc);

            ATPTEFMAPRegisterExt apDocExt = doc.GetExtension<ATPTEFMAPRegisterExt>();

            if (apDocExt != null)
            {
                Base.Document.Current.GetExtension<ATPTEFMAPRegisterExt>().UsrATPTEFMSourceType = apDocExt.UsrATPTEFMSourceType;
                Base.Document.Current.GetExtension<ATPTEFMAPRegisterExt>().UsrATPTEFMSourceRef = apDocExt.UsrATPTEFMSourceRef;
                Base.Document.Current.GetExtension<ATPTEFMAPRegisterExt>().UsrATPTEFMSourceReqClass = apDocExt.UsrATPTEFMSourceReqClass;
                Base.Document.UpdateCurrent();
            }
        }

        public PXAction<APInvoice> ATPTEFMViewDocuments;
        [PXButton]
        [PXUIField(Visible = false)]
        protected virtual IEnumerable aTPTEFMViewDocuments(PXAdapter adapter)
        {
            APRegister aPRegister = Base.CurrentDocument.Current;
            ATPTEFMAPRegisterExt aPRegisterExt = aPRegister.GetExtension<ATPTEFMAPRegisterExt>();

            if (aPRegister is null) return adapter.Get();

            if (aPRegisterExt.UsrATPTEFMSourceType == ATPTEFMSourceTypeAttribute.Funds)
            {
                ATPTEFMFundMaint graph = PXGraph.CreateInstance<ATPTEFMFundMaint>();

                var document = graph.Document.Search<ATPTEFMFund.fundCD>(aPRegisterExt.UsrATPTEFMSourceRef);

                if (document != null)
                    throw new PXPopupRedirectException(graph, ATPTEFMMessages.ATPTEFMFund, true);
            }
            if (aPRegisterExt.UsrATPTEFMSourceType == ATPTEFMSourceTypeAttribute.CashAdvance)
            {
                ATPTEFMCashAdvanceEntry graph = PXGraph.CreateInstance<ATPTEFMCashAdvanceEntry>();

                var document = graph.CashAdvances.Search<ATPTEFMCashAdvance.cashAdvanceNbr>(aPRegisterExt.UsrATPTEFMSourceRef);

                if (document != null)
                    throw new PXPopupRedirectException(graph, ATPTEFMMessages.ATPTEFMCashAdvance, true);
            }

            return adapter.Get();
        }

        public PXAction<APPayment> ATPTEFMViewTransactionDocument;
        [PXButton]
        [PXUIField(Visible = false)]
        protected virtual IEnumerable aTPTEFMViewTransactionDocument(PXAdapter adapter)
        {
            APRegister aPRegister = Base.CurrentDocument.Current;
            ATPTEFMAPRegisterExt aPRegisterExt = aPRegister.GetExtension<ATPTEFMAPRegisterExt>();

            if (aPRegister is null) return adapter.Get();

            ExpenseClaimEntry graph = PXGraph.CreateInstance<ExpenseClaimEntry>();

            graph.ExpenseClaim.Current = PXSelect<
                EPExpenseClaim,
                Where<ATPTEFMEPExpenseClaimExt.usrATPTEFMRFPReqRef, Equal<Required<ATPTEFMEPExpenseClaimExt.usrATPTEFMRFPReqRef>>>>
                .Select(Base, aPRegisterExt.UsrATPTEFMLiqNbr);

            if (graph.ExpenseClaim.Current != null)
                throw new PXPopupRedirectException(graph, "Expense Claim", true);

            return adapter.Get();
        }

        #endregion

        #region Events
        //protected virtual void _(Events.RowSelecting<APPayment> e, PXRowSelecting baseEvent)
        //{
        //    if(baseEvent != null) baseEvent.Invoke(e.Cache, e.Args);

        //    APPayment payment = e.Row;
        //    if (payment == null) return;
        //    using (new PXConnectionScope())
        //    {
        //        ATPTRelatedCashAdvance.Current = ATPTRelatedCashAdvance.Select(Base?.CurrentDocument?.Current?.OrigRefNbr);
        //    }
        //}
        protected virtual void APPayment_RowSelected(PXCache sender, PXRowSelectedEventArgs e, PXRowSelected baseMethod)
        {
            baseMethod?.Invoke(sender, e);
            APPayment payment = (APPayment)e.Row;
            if (payment is null) return;
            ATPTEFMAPRegisterExt paymentExt = payment.GetExtension<ATPTEFMAPRegisterExt>();

            bool IsMigration = ATPTEFMPreferences?.Current?.IsCashAdvanceMigration ?? false;

            PXUIFieldAttribute.SetEnabled<ATPTEFMAPRegisterExt.usrATPTEFMSourceType>(sender, null, IsMigration);
            PXUIFieldAttribute.SetEnabled<ATPTEFMAPRegisterExt.usrATPTEFMSourceRef>(sender, null, IsMigration);

            //ATPTEFMCashAdvance ca = ATPTRelatedCashAdvance.Current;
            if (payment.DocType == APDocType.Refund)
            {
                Base.Adjustments.Cache.SetAllEditPermissions(!(paymentExt.UsrATPTEFMSourceType == ATPTEFMSourceTypeAttribute.CashAdvance));
                PXUIFieldAttribute.SetEnabled<APPayment.curyOrigDocAmt>(sender, payment, !(paymentExt.UsrATPTEFMSourceType == ATPTEFMSourceTypeAttribute.CashAdvance));
            }
        }
        protected virtual void APPayment_RowPersisted(PXCache sender, PXRowPersistedEventArgs e, PXRowPersisted baseEvent)
        {
            baseEvent(sender, e);
            APPayment payment = (APPayment)e.Row;

            if (payment.DocType == APDocType.Refund && sender.GetStatus(e.Row) == PXEntryStatus.Deleted)
            {
                ATPTEFMCashAdvance ca = ATPTRelatedCashAdvance.Select(payment.OrigRefNbr);
                if (ca != null)
                {
                    ca.VendorRefundRefNbr = null;
                    ATPTRelatedCashAdvance.Update(ca);
                }
            }
        }

        protected virtual void _(Events.RowInserted<APAdjust> e)
        {
            APAdjust row = e.Row;

            if (row is null) return;

            APPayment curPayment = Base.Document.Current;

            if (curPayment != null && curPayment.DocType == APDocType.DebitAdj)
            {
                APInvoice adjInv = PXSelect<
                    APInvoice,
                    Where<APInvoice.refNbr, Equal<Required<APInvoice.refNbr>>,
                        And<APInvoice.docType, Equal<Required<APInvoice.docType>>>>>
                    .Select(Base, row.AdjdRefNbr, row.AdjdDocType);

                if (adjInv != null)
                {
                    ATPTEFMAPRegisterExt aPRegisterExt = adjInv.GetExtension<ATPTEFMAPRegisterExt>();

                    if (aPRegisterExt != null && aPRegisterExt.UsrATPTEFMIsUnreplenishedReceiptBill == true)
                    {
                        decimal? netAmt = adjInv.CuryLineTotal - adjInv.CuryOrigWhTaxAmt;
                        e.Cache.SetValueExt<APAdjust.curyAdjgAmt>(row, netAmt);
                    }
                }
            }
        }
        protected virtual void _(Events.FieldUpdated<APAdjust, APAdjust.adjdRefNbr> e, PXFieldUpdated baseEvent)
        {
            if (baseEvent != null) baseEvent(e.Cache, e.Args);
            APAdjust rowAdj = e.Row as APAdjust;

            if (rowAdj != null)
            {
                APInvoice getInvoice = APInvoice.PK.Find(this.Base, rowAdj.AdjdDocType, rowAdj.AdjdRefNbr);
                ATPTEFMAPRegisterExt invExt = getInvoice.GetExtension<ATPTEFMAPRegisterExt>();

                if (getInvoice != null)
                {
                    //if (rowAdj.AdjgDocType == APDocType.DebitAdj && getInvoice.OrigModule == BatchModule.EP)
                    //if ((rowAdj.AdjgDocType == APDocType.DebitAdj) &&
                    //    (!String.IsNullOrEmpty(invExt.UsrATPTEFMSourceRef) ||
                    //    !String.IsNullOrEmpty(getInvoice.OrigRefNbr) ||
                    //    getInvoice.OrigModule == BatchModule.EP))
                    //{
                    //    rowAdj.CuryAdjgAmt = getInvoice.CuryDocBal - getInvoice.CuryWhTaxBal;
                    //    rowAdj.CuryAdjdAmt = getInvoice.CuryDocBal - getInvoice.CuryWhTaxBal;
                    //    rowAdj.AdjAmt = getInvoice.DocBal - getInvoice.WhTaxBal;
                    //}
                    if (rowAdj.AdjgDocType == APDocType.DebitAdj && getInvoice.OrigModule == BatchModule.EP
                        && (getInvoice.CreatedByScreenID == "ATPT3103" || getInvoice.CreatedByScreenID == "ATPT3103"))
                    {
                        rowAdj.CuryAdjgAmt = getInvoice.CuryDocBal;
                        rowAdj.CuryAdjdAmt = getInvoice.CuryDocBal;
                        rowAdj.AdjAmt = getInvoice.DocBal;
                    }
                    else if (rowAdj.AdjgDocType == APDocType.DebitAdj
                        && getInvoice.OrigModule == BatchModule.EP && getInvoice.CreatedByScreenID == "ATPT2012")
                    {
                        rowAdj.CuryAdjgAmt = getInvoice.CuryDocBal - getInvoice.CuryWhTaxBal;
                        rowAdj.CuryAdjdAmt = getInvoice.CuryDocBal - getInvoice.CuryWhTaxBal;
                        rowAdj.AdjAmt = getInvoice.DocBal - getInvoice.WhTaxBal;
                    }
                }
            }
        }
        /// <summary>
        /// Validates Fund Establishment Bill applications during persisting.
        /// Purpose:
        /// - Ensures Fund Establishment Bills are fully paid/applied
        /// - Prevents partial payments for Fund Establishment Bills
        /// Validation:
        /// - If applied amount (CuryAdjgAmt) is less than invoice total (CuryLineTotal)
        /// - Throws error: "Fund Establishment Bill {0} must be fully paid"
        /// </summary>
        /// 
        /// <remarks>
        /// 009836 - Run validation on Payment type only </br>
        /// Enhancement 30-01-2025 : Run validation if bill currency is equal to payment currency
        /// 009458 - Change document total condition {JLTG} </br>
        /// </remarks>
        /// <remarks>
        /// 2025-01-22 : Fixed NullReferenceException when deleting refund - check getInvoice != null before GetExtension {CFM}
        /// </remarks>
        protected virtual void _(Events.RowPersisting<APAdjust> e, PXRowPersisting baseMethod)
        {
            if (baseMethod != null) baseMethod(e.Cache, e.Args);

            var row = e.Row;
            if (row == null) return;

            // Skip validation during delete operations
            if (e.Operation == PXDBOperation.Delete) return;

            APPayment curPayment = Base.Document.Current;
            if (curPayment == null) return;

            APInvoice getInvoice = APInvoice.PK.Find(this.Base, row.AdjdDocType, row.AdjdRefNbr);
            if (getInvoice == null) return;

            ATPTEFMAPRegisterExt invExt = getInvoice.GetExtension<ATPTEFMAPRegisterExt>();
            if (invExt == null) return;

            if (invExt.UsrATPTEFMIsAmountRestrictedBill == true && (row.AdjgDocType == APPaymentType.Check || row.AdjgDocType == APPaymentType.Refund))
            {
                if (row.CuryAdjgAmt < getInvoice.CuryDocBal && curPayment.CuryID == getInvoice.CuryID)
                {
                    string errMsg = string.Format(ATPTEFMMessages.DocumentBillCannotBeReleased, getInvoice.RefNbr);
                    e.Cache.RaiseExceptionHandling<APAdjust.curyAdjgAmt>(row, ((APAdjust)e?.Row)?.CuryAdjgAmt, ATPTEFMHelper.GetPropertyException(row, errMsg, PXErrorLevel.Error));
                    throw new Exception(errMsg);
                }
            }
        }

        #endregion

        #region Methods

        private void UpdateCloseFundHistory()
        {
            APPayment aPPayment = Base.Document.Current;
            ATPTEFMAPRegisterExt apRegisterExt = aPPayment.GetExtension<ATPTEFMAPRegisterExt>();

            if (aPPayment.DocType == APDocType.Refund)
            {
                PXResultset<APAdjust, APInvoice> ap = PXSelectJoinGroupBy<
                    APAdjust,
                    InnerJoin<APInvoice,
                        On<APInvoice.refNbr, Equal<APAdjust.adjdRefNbr>>>,
                    Where<APAdjust.adjgRefNbr, Equal<Required<APAdjust.adjgRefNbr>>,
                        And<APAdjust.voided, NotEqual<True>,
                        And<APAdjust.adjdDocType, Equal<APPaymentType.debitAdj>>>>,
                    Aggregate<
                        GroupBy<APInvoice.docType,
                        GroupBy<APInvoice.refNbr>>>>
                    .Select<PXResultset<APAdjust, APInvoice>>(Base, aPPayment.RefNbr);

                if (ap.Count > 0)
                {
                    APAdjust apAdjust = ap;

                    ATPTEFMTransactionHistoryView.Current = ATPTEFMTransactionHistoryView.Select(apAdjust.AdjdRefNbr);

                    if (ATPTEFMTransactionHistoryView.Current != null)
                    {
                        ATPTEFMFund.Current = ATPTEFMFund.Select(ATPTEFMTransactionHistoryView.Current.FundRefNbr);

                        ATPTEFMTransactionHistoryView.Current.Status = APDocStatus.Closed;
                        ATPTEFMTransactionHistoryView.Current.CuryBalanceAmt = ATPTEFMFund.Current.CuryBalanceAmt;
                        ATPTEFMTransactionHistoryView.Current.CheckNbr = aPPayment.RefNbr;
                        ATPTEFMTransactionHistoryView.Current.CuryCheckAmt = apAdjust.CuryAdjgAmt;
                        ATPTEFMTransactionHistoryView.UpdateCurrent();
                        ATPTEFMTransactionHistoryView.Cache.Persist(PXDBOperation.Update);
                    }
                }
            }
        }
        private void ReplenishmentPayment()
        {
            APPayment aPPayment = Base.Document.Current;
            ATPTEFMAPRegisterExt apRegisterExt = aPPayment.GetExtension<ATPTEFMAPRegisterExt>();

            if (aPPayment.DocType == APDocType.Check)
            {
                PXResultset<APAdjust, APInvoice, ATPTEFMReplenishment> apDetails = PXSelectJoinGroupBy<
                    APAdjust,
                    InnerJoin<APInvoice,
                        On<APInvoice.refNbr, Equal<APAdjust.adjdRefNbr>,
                        And<APInvoice.docType, Equal<APAdjust.adjdDocType>>>,
                    InnerJoin<ATPTEFMReplenishment,
                        On<ATPTEFMReplenishment.replenishmentNbr, Equal<APInvoice.origRefNbr>>>>,
                    Where<APAdjust.adjgRefNbr, Equal<Required<APAdjust.adjgRefNbr>>,
                        And<APAdjust.voided, NotEqual<True>,
                        And<APAdjust.adjdDocType, NotEqual<APPaymentType.debitAdj>>>>,
                    Aggregate<
                        GroupBy<APInvoice.docType,
                        GroupBy<APInvoice.refNbr>>>>
                    .Select<PXResultset<APAdjust, APInvoice, ATPTEFMReplenishment>>(Base, aPPayment.RefNbr);

                if (apDetails.Count == 1)
                {
                    foreach (PXResult<APAdjust, APInvoice, ATPTEFMReplenishment> ap in apDetails)
                    {
                        APAdjust apAdjust = ap;
                        ATPTEFMReplenishment replenishment = ap;

                        if (replenishment != null)
                        {
                            ATPTEFMFund.Current = ATPTEFMFund.Select(replenishment.FundID);

                            #region Summary Area
                            if (ATPTEFMFund.Current != null)
                            {
                                ATPTEFMFund.Current.CuryOnReplenishmentAmt -= apAdjust.CuryAdjgAmt;
                                ATPTEFMFund.Current.CuryBalanceAmt += apAdjust.CuryAdjgAmt;
                                ATPTEFMFund.UpdateCurrent();
                                ATPTEFMFund.Cache.Persist(PXDBOperation.Update);
                            }
                            #endregion

                            #region Transaction History
                            ATPTEFMTransactionHistoryView.Current = ATPTEFMTransactionHistoryView.Select(replenishment.ReplenishmentNbr);

                            if (ATPTEFMTransactionHistoryView.Current != null)
                            {

                                if (string.IsNullOrEmpty(ATPTEFMTransactionHistoryView.Current.CheckNbr))
                                {
                                    ATPTEFMTransactionHistoryView.Current.CheckNbr = Base.Document.Current.RefNbr;
                                    ATPTEFMTransactionHistoryView.Current.CuryCheckAmt = apAdjust.CuryAdjgAmt;
                                    ATPTEFMTransactionHistoryView.Current.HasReplenishemtCheckNbr = true;
                                    ATPTEFMTransactionHistoryView.Current.CuryBalanceAmt = ATPTEFMFund.Current.CuryBalanceAmt;
                                    ATPTEFMTransactionHistoryView.UpdateCurrent();
                                    ATPTEFMTransactionHistoryView.Cache.Persist(PXDBOperation.Update);
                                }
                                else
                                {
                                    ATPTEFMFundTransactionHistoryView tranHistory = ATPTEFMTransactionHistoryView.Insert();
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
                                    tranHistory.CuryBalanceAmt = ATPTEFMFund.Current.CuryBalanceAmt;
                                    tranHistory.CheckNbr = Base.Document.Current.RefNbr;
                                    tranHistory.CuryCheckAmt = apAdjust.CuryAdjgAmt;
                                    tranHistory.HasReplenishemtCheckNbr = true;
                                    tranHistory.SortNbr = replenishment.ReplenishmentNbr;
                                    ATPTEFMTransactionHistoryView.Update(tranHistory);
                                    ATPTEFMTransactionHistoryView.Cache.Persist(PXDBOperation.Insert);
                                }
                            }
                            #endregion
                        }
                    }
                }

                if (apDetails.Count > 1)
                {
                    ATPTEFMReplenishment replenishment = apDetails;

                    decimal? checkAmt = decimal.Zero;
                    foreach (PXResult<APAdjust, APInvoice, ATPTEFMReplenishment> ap in apDetails)
                    {
                        APAdjust apAdjust = ap;
                        checkAmt += apAdjust.CuryAdjgAmt;
                    }

                    if (replenishment != null)
                    {
                        ATPTEFMFund.Current = ATPTEFMFund.Select(replenishment.FundID);

                        #region Summary Area
                        if (ATPTEFMFund.Current != null)
                        {
                            ATPTEFMFund.Current.CuryOnReplenishmentAmt -= checkAmt;
                            ATPTEFMFund.Current.CuryBalanceAmt += checkAmt;
                            ATPTEFMFund.UpdateCurrent();
                            ATPTEFMFund.Cache.Persist(PXDBOperation.Update);
                        }
                        #endregion

                        #region Transaction History
                        ATPTEFMTransactionHistoryView.Current = ATPTEFMTransactionHistoryView.Select(replenishment.ReplenishmentNbr);
                        if (ATPTEFMTransactionHistoryView.Current != null)
                        {
                            if (string.IsNullOrEmpty(ATPTEFMTransactionHistoryView.Current.CheckNbr))
                            {
                                ATPTEFMTransactionHistoryView.Current.CheckNbr = Base.Document.Current.RefNbr;
                                ATPTEFMTransactionHistoryView.Current.CuryCheckAmt = checkAmt;
                                ATPTEFMTransactionHistoryView.Current.HasReplenishemtCheckNbr = true;
                                ATPTEFMTransactionHistoryView.Current.CuryBalanceAmt = ATPTEFMFund.Current.CuryBalanceAmt;
                                ATPTEFMTransactionHistoryView.UpdateCurrent();
                                ATPTEFMTransactionHistoryView.Cache.Persist(PXDBOperation.Update);
                            }
                        }
                        #endregion
                    }
                }
            }
        }
        private void UpdateCashAdvance()
        {
            using (PXTransactionScope ts = new PXTransactionScope())
            {
                ATPTEFMCashAdvanceEntry caEntry = PXGraph.CreateInstance<ATPTEFMCashAdvanceEntry>();

                ATPTEFMCashAdvance cadv = PXSelect<
                    ATPTEFMCashAdvance,
                    Where<ATPTEFMCashAdvance.ppmType, Equal<Required<ATPTEFMCashAdvance.ppmType>>,
                        And<ATPTEFMCashAdvance.ppmRefNbr, Equal<Required<ATPTEFMCashAdvance.ppmRefNbr>>>>
                    >
                    .Select(this.Base, this.Base.Document.Current.DocType, this.Base.Document.Current.RefNbr);

                if (cadv != null)
                {
                    caEntry.CashAdvances.Current = cadv;
                    if ((Base.Document.Current.CuryDocBal ?? 0m) <= 0 && cadv.CuryChangeAmount <= 0)
                    {
                        cadv.Status = ATPTEFMCashAdvanceStatusAttribute.ClosedValue;
                        caEntry.CashAdvances.Update(cadv);
                        caEntry.Save.Press();
                    }

                    //caEntry.Save.Press();
                }
                /*else if (Base.Document.Current.DocType == APDocType.Refund)
                {
                    foreach (APAdjust invoice in PXSelect<
                        APAdjust,
                        Where<APAdjust.adjgRefNbr, Equal<Required<APAdjust.adjgRefNbr>>>>
                        .Select<PXResultset<APAdjust>>(this.Base, Base.Document.Current.RefNbr))
                    {
                        ATPTEFMCashAdvance caRefund = PXSelect<
                           ATPTEFMCashAdvance,
                            Where<ATPTEFMCashAdvance.billType, Equal<Required<ATPTEFMCashAdvance.billType>>,
                                And<ATPTEFMCashAdvance.billRefNbr, Equal<Required<ATPTEFMCashAdvance.billRefNbr>>>>
                               >
                            .Select(this.Base, APDocType.Prepayment, invoice.AdjdRefNbr);

                        if (caRefund != null)
                        {
                            caEntry.Clear();
                            caEntry.CashAdvances.Current = caRefund;
                            decimal refundAmt = (decimal)(caRefund?.RefundAmount + Base.Document.Current.CuryOrigDocAmt);

                            if (caRefund.Status == ATPTEFMCashAdvanceStatusAttribute.ClosedValue)
                            {
                                caRefund.VendorRefundType = Base.Document.Current.DocType;
                                caRefund.VendorRefundRefNbr = Base.Document.Current.RefNbr;
                                caRefund.RefundAmount += Base.Document.Current.CuryOrigDocAmt;
                                caRefund.CuryChangeAmount -= Base.Document.Current.CuryOrigDocAmt;
                                caRefund.ChangeAmount -= Base.Document.Current.CuryOrigDocAmt;
                            }

                            if (caRefund.Status == ATPTEFMCashAdvanceStatusAttribute.PendingLiquidationValue)
                            {
                                caRefund.VendorRefundType = Base.Document.Current.DocType;
                                caRefund.VendorRefundRefNbr = Base.Document.Current.RefNbr;
                                caRefund.RefundAmount += Base.Document.Current.CuryOrigDocAmt;
                                caRefund.CuryChangeAmount -= Base.Document.Current.CuryOrigDocAmt;
                                caRefund.ChangeAmount -= Base.Document.Current.CuryOrigDocAmt;

                                if (invoice.DocBal == decimal.Zero)
                                    caRefund.Status = ATPTEFMCashAdvanceStatusAttribute.ClosedValue;
                            }

                            caEntry.CashAdvances.Update(caRefund);
                            caEntry.Save.Press();
                        }
                    }
                }*/
                //CFM EXCEL #178 => https://infosoft888-my.sharepoint.com/:x:/g/personal/phillip_albores_infosoft_com_ph/ERtb7htMJPhMgHbkGBCDt8MBciwtxmYUytW75kzI-wLKwA?e=g3W5pf
                /*else if (Base.Document.Current.DocType == APDocType.VoidRefund)
                {
                    foreach (APAdjust invoice in PXSelect<
                        APAdjust,
                        Where<APAdjust.adjgRefNbr, Equal<Required<APAdjust.adjgRefNbr>>>>
                        .Select<PXResultset<APAdjust>>(this.Base, Base.Document.Current.RefNbr))
                    {
                        ATPTEFMCashAdvance caVoidRefund = PXSelect<
                           ATPTEFMCashAdvance,
                            Where<ATPTEFMCashAdvance.billType, Equal<Required<ATPTEFMCashAdvance.billType>>,
                                And<ATPTEFMCashAdvance.billRefNbr, Equal<Required<ATPTEFMCashAdvance.billRefNbr>>>>
                               >
                            .Select(this.Base, APDocType.Prepayment, invoice.AdjdRefNbr);

                        if (caVoidRefund != null)
                        {
                            caEntry.Clear();
                            caEntry.CashAdvances.Current = caVoidRefund;

                            if (caVoidRefund.Status == ATPTEFMCashAdvanceStatusAttribute.ClosedValue)
                            {
                                caVoidRefund.VendorRefundType = Base.Document.Current.DocType;
                                caVoidRefund.VendorRefundRefNbr = Base.Document.Current.RefNbr;
                                caVoidRefund.RefundAmount += Base.Document.Current.CuryOrigDocAmt;
                                caVoidRefund.CuryChangeAmount -= Base.Document.Current.CuryOrigDocAmt;
                                caVoidRefund.ChangeAmount -= Base.Document.Current.CuryOrigDocAmt;
                                caVoidRefund.Status = ATPTEFMCashAdvanceStatusAttribute.PendingLiquidationValue;
                            }

                            caEntry.CashAdvances.Update(caVoidRefund);
                            caEntry.Save.Press();
                        }
                    }
                }*/
                ts.Complete();
            }
        }
        protected virtual void ValidateReplenishmentCurrency()
        {
            var payment = Base.Document.Current;
            if (payment == null) return;

            // Get all adjustments
            var adjustments = Base.Adjustments.Select().RowCast<APAdjust>();

            foreach (var adj in adjustments)
            {
                // Get the bill details with replenishment and fund info
                PXResult<APInvoice, ATPTEFMReplenishment, ATPTEFMFund> billWithExt =
                    (PXResult<APInvoice, ATPTEFMReplenishment, ATPTEFMFund>)
                    PXSelectJoin<
                        APInvoice,
                        InnerJoin<ATPTEFMReplenishment,
                            On<ATPTEFMReplenishment.replenishmentNbr, Equal<APInvoice.origRefNbr>>,
                        InnerJoin<ATPTEFMFund,
                            On<ATPTEFMFund.fundCD, Equal<ATPTEFMReplenishment.fundID>>>>,
                        Where<APInvoice.docType, Equal<Required<APInvoice.docType>>,
                            And<APInvoice.refNbr, Equal<Required<APInvoice.refNbr>>>>>
                        .Select(Base, adj.AdjdDocType, adj.AdjdRefNbr)
                        .FirstOrDefault();

                if (billWithExt != null)
                {
                    var fund = billWithExt.GetItem<ATPTEFMFund>();

                    // Compare currencies
                    if (payment.CuryID != fund.CuryID)
                    {
                        throw new PXException(Messages.ATPTEFMMessages.PaymentCurrencyMismatch, payment.CuryID, fund.CuryID, adj.AdjdRefNbr);
                    }
                }
            }
        }
        protected virtual void ValidateFundSourceCurrency()
        {
            var payment = Base.Document.Current;
            if (payment == null) return;

            var adjustments = Base.Adjustments.Select().RowCast<APAdjust>();

            foreach (var adj in adjustments)
            {
                PXResult<APInvoice, ATPTEFMFund> result = (PXResult<APInvoice, ATPTEFMFund>) PXSelectJoin<
                    APInvoice,
                    InnerJoin<ATPTEFMFund,
                        On<ATPTEFMFund.fundCD, Equal<ATPTEFMAPRegisterExt.usrATPTEFMSourceRef>>>,
                    Where<APInvoice.docType, Equal<Required<APInvoice.docType>>,
                        And<APInvoice.refNbr, Equal<Required<APInvoice.refNbr>>>>>
                    .Select(Base, adj.AdjdDocType, adj.AdjdRefNbr)
                    .FirstOrDefault();

                if (result != null)
                {
                    ATPTEFMFund fund = (ATPTEFMFund)result;

                    // Your validation logic here
                    if (payment.CuryID != fund.CuryID)
                    {
                        throw new PXException(Messages.ATPTEFMMessages.PaymentCurrencyMismatch, payment.CuryID, fund.CuryID, adj.AdjdRefNbr);
                    }
                }
            }
        }
        #endregion

    }
}