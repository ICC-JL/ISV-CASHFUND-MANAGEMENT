using ATPTPhilippineTax.Helpers;
using CashFundManagement.Helper;
using PX.Data;
using PX.Objects.AP;
using PX.Objects.CM;
using PX.Objects.EP;
using System.Collections;
using System.Collections.Generic;
using static ATPTPhilippineTax.Graph.Inquiry.ATPTPurchasesJournalInq;

namespace CashFundPhilippineTax.Graph.Extension 
{
    /// <remarks>
    /// 2024-08-14 : Adds multi-tenant support. {RRS} <br/>
    /// 2025-02-13:  Apply multi tenant to 24r1 CASE: 010269 {JLTG}
    /// </remarks>
    public class ATPTPurchasesJournalInq : PXGraphExtension<ATPTPhilippineTax.Graph.Inquiry.ATPTPurchasesJournalInq>
    {
#if Version23R2
        public static bool IsActive() => ATPTModule.IsActive;
#else
        public static bool IsActive() => ATPTModule.IsActive && ATPTEFMPrefetchSetup.IsActive;
#endif

        protected virtual IEnumerable journal()
        {
            ATPTPurchasesJournalFilter filter = Base.Filter.Current;
            List<ATPTPurchasesJournalResult> baseResult = Base.GetJournal(filter);
            List<ATPTPurchasesJournalResult> result = new List<ATPTPurchasesJournalResult>();
            List<CurrencyRate> curyRates = ATPTHelpers.GetRates(Base, filter.StartDate, filter.EndDate);

            #region Employee Expenses
            //PXSelectBase<EPTaxTran> query = new PXSelectReadonly2<EPTaxTran,
            //    InnerJoin<CurrencyInfo, On<CurrencyInfo.curyInfoID, Equal<EPTaxTran.curyInfoID>>,
            //    InnerJoin<EPExpenseClaimDetails, On<EPExpenseClaimDetails.claimDetailID, Equal<EPTaxTran.claimDetailID>>,
            //    InnerJoin<APInvoice, On<APInvoice.refNbr, Equal<EPExpenseClaimDetails.aPRefNbr>, //*** PREVIOUS CONDITION InnerJoin < APInvoice, On<APInvoice.invoiceNbr, Equal<EPExpenseClaimDetails.refNbr>, ***///
            //        And<EPExpenseClaimDetails.expenseDate, GreaterEqual<Current<ATPTPurchasesJournalFilter.startDate>>,
            //        And<EPExpenseClaimDetails.expenseDate, LessEqual<Current<ATPTPurchasesJournalFilter.endDate>>,
            //        And<APInvoice.released, Equal<True>,
            //        And<APInvoice.isMigratedRecord, NotEqual<True>,
            //        And<Where<APInvoice.docType, Equal<APDocType.invoice>,
            //            Or<APInvoice.docType, Equal<APDocType.debitAdj>,
            //            Or<APInvoice.docType, Equal<APDocType.creditAdj>>>>>>>>>>,
            //    InnerJoin<APTaxTran, On<APTaxTran.taxID, Equal<EPTaxTran.taxID>,
            //        And<APTaxTran.tranType, Equal<APInvoice.docType>,
            //        And<APTaxTran.refNbr, Equal<APInvoice.refNbr>>>>,
            //    InnerJoin<Tax, On<Tax.taxID, Equal<EPTaxTran.taxID>,
            //        And<Where<ATPTTax.usrATPTPurchaseClassification, Equal<ATPTTaxClassificationAttribute.goods>,
            //        Or<ATPTTax.usrATPTPurchaseClassification, Equal<ATPTTaxClassificationAttribute.services>>>>>>>>>>>(Base);

            ////query.WhereAnd<Where<Tax.taxID, NotEqual<ATPTImportationTaxType.taxable1>, And<Tax.taxID, NotEqual<ATPTImportationTaxType.exempt1>>>>();
            ////query.WhereAnd<Where<Tax.taxID, NotEqual<ATPTImportationTaxType.taxable2>, And<Tax.taxID, NotEqual<ATPTImportationTaxType.exempt2>>>>();

            //foreach (PXResult<EPTaxTran, CurrencyInfo, EPExpenseClaimDetails, APInvoice, APTaxTran, Tax> q in query.Select())
            //{
            //    EPExpenseClaimDetails er = (EPExpenseClaimDetails)q;
            //    CurrencyInfo curyInfo = q;
            //    ATPTEFMEPExpenseClaimDetailsExt erExt = er.GetExtension<ATPTEFMEPExpenseClaimDetailsExt>();

            //    ATPTPurchasesJournalResult item = new ATPTPurchasesJournalResult();

            //    //item.OrigModule = r.OrigModule;
            //    item.DocDate = er.ExpenseDate;
            //    item.VendorID = erExt.UsrATPTVendID; /*erExt.UsrATPTVendorID;*/
            //    item.VendorName = erExt.UsrATPTVendName;
            //    item.TaxRegistrationID = erExt.UsrATPTVendTIN;
            //    item.Address = erExt.UsrATPTAddress;
            //    item.Description = er.TranDesc;
            //    item.RefNbr = er.ClaimDetailCD;
            //    item.DocType = Attribute.Extension.ATPTAPDatDocType.ExpenseReceiptValue;
            //    item.VatablePurchases = decimal.Zero;
            //    item.NonVatablePurchases = decimal.Zero;
            //    item.ZeroRatedPurchases = decimal.Zero;
            //    item.ExemptPurchases = decimal.Zero;
            //    item.Discount = decimal.Zero;
            //    item.VATAmount = decimal.Zero;
            //    item.NetPurchases = decimal.Zero;
            //    item.GrossPurchases = decimal.Zero;

            //    EPTaxTran tran = (EPTaxTran)q;
            //    Tax tax = (Tax)q;
            //    ATPTTax taxExt = tax.GetExtension<ATPTTax>();

            //    decimal txbleamt = tran.TaxableAmt ?? decimal.Zero;
            //    decimal taxamt = tran.TaxAmt ?? decimal.Zero;

            //    #region MULTI CURRENCY 
            //    bool isCurrency = !String.IsNullOrEmpty(this.Base.Setup.Current.PhilTaxCurrency) || !String.IsNullOrEmpty(this.Base.Setup.Current.PhilTaxCurrencyType);
            //    bool isMultiCurrency = curyInfo.BaseCuryID != curyInfo.CuryID;
            //    bool isMultiCurEqualToPhiltaxCur = curyInfo.CuryID == this.Base.Setup.Current.PhilTaxCurrency;
            //    CurrencyRate curyRate = new CurrencyRate();

            //    if ((isCurrency) && (isMultiCurrency))
            //    {
            //        if (isMultiCurEqualToPhiltaxCur)
            //        {
            //            txbleamt = tran.CuryTaxableAmt ?? decimal.Zero;
            //            taxamt = tran.CuryTaxAmt ?? decimal.Zero;
            //        }
            //        else
            //        {
            //            curyRate = ATPTHelpers.GetSpecificRate(curyRates, item.DocDate);

            //            if (curyRate.CuryRateID != null)
            //            {
            //                PXDBCurrencyAttribute.CuryConvCury(this.Base.Journal.Cache, curyRate, (Math.Round((decimal)tran.TaxableAmt, 2, MidpointRounding.AwayFromZero)), out txbleamt, true);
            //                PXDBCurrencyAttribute.CuryConvCury(this.Base.Journal.Cache, curyRate, (Math.Round((decimal)tran.TaxAmt, 2, MidpointRounding.AwayFromZero)), out taxamt, true);
            //            }
            //        }

            //    }
            //    else if ((isCurrency) && !(isMultiCurrency))
            //    {
            //        curyRate = ATPTHelpers.GetSpecificRate(curyRates, item.DocDate);

            //        if (curyRate.CuryRateID != null)
            //        {
            //            PXDBCurrencyAttribute.CuryConvCury(this.Base.Journal.Cache, curyRate, (Math.Round((decimal)tran.TaxableAmt, 2, MidpointRounding.AwayFromZero)), out txbleamt, true);
            //            PXDBCurrencyAttribute.CuryConvCury(this.Base.Journal.Cache, curyRate, (Math.Round((decimal)tran.TaxAmt, 2, MidpointRounding.AwayFromZero)), out taxamt, true);
            //        }
            //    }
            //    #endregion

            //    if (taxExt.UsrATPTPurchaseType == ATPTPurchaseTaxTypeAttribute.CapGoodsGreater1M || taxExt.UsrATPTPurchaseType == ATPTPurchaseTaxTypeAttribute.CapGoodsLess1M ||
            //        taxExt.UsrATPTPurchaseType == ATPTPurchaseTaxTypeAttribute.DomGoods || taxExt.UsrATPTPurchaseType == ATPTPurchaseTaxTypeAttribute.DomServices || taxExt.UsrATPTPurchaseType == ATPTPurchaseTaxTypeAttribute.Others)
            //    {
            //        item.VatablePurchases += txbleamt;
            //    }
            //    if (taxExt.UsrATPTPurchaseType == ATPTPurchaseTaxTypeAttribute.ZeroRated)
            //        item.ZeroRatedPurchases += txbleamt;
            //    if (taxExt.UsrATPTPurchaseType == ATPTPurchaseTaxTypeAttribute.VATExempt)
            //        item.ExemptPurchases += txbleamt;
            //    if (taxExt.UsrATPTPurchaseType == ATPTPurchaseTaxTypeAttribute.PurchNotQualForInput)
            //        item.NonVatablePurchases += txbleamt;

            //    item.VATAmount += taxamt;

            //    item.NetPurchases = item.VatablePurchases + item.NonVatablePurchases + item.ZeroRatedPurchases + item.ExemptPurchases;

            //    item.GrossPurchases = item.NetPurchases + item.Discount + item.VATAmount;

            //    baseResult.Add(item);
            //}

            #endregion

            #region Replenishment
            //PXSelectBase<EPTaxTran> queryReplenishment = new PXSelectReadonly2<EPTaxTran,
            //InnerJoin<CurrencyInfo, On<CurrencyInfo.curyInfoID, Equal<EPTaxTran.curyInfoID>>,
            //InnerJoin<EPExpenseClaimDetails, On<EPExpenseClaimDetails.claimDetailID, Equal<EPTaxTran.claimDetailID>>,
            //InnerJoin<ATPTEFMReplenishmentDetail, On<ATPTEFMReplenishmentDetail.expenseReceiptNbr, Equal<EPExpenseClaimDetails.claimDetailCD>>,
            //InnerJoin<APInvoice, On<APInvoice.refNbr, Equal<ATPTEFMReplenishmentDetail.invoiceRefNbr>,
            //    And<EPExpenseClaimDetails.expenseDate, GreaterEqual<Current<ATPTPurchasesJournalFilter.startDate>>,
            //    And<EPExpenseClaimDetails.expenseDate, LessEqual<Current<ATPTPurchasesJournalFilter.endDate>>,
            //    And<APInvoice.released, Equal<True>,
            //    And<APInvoice.isMigratedRecord, NotEqual<True>,
            //    And<Where<APInvoice.docType, Equal<APDocType.invoice>,
            //        Or<APInvoice.docType, Equal<APDocType.debitAdj>,
            //        Or<APInvoice.docType, Equal<APDocType.creditAdj>>>>>>>>>>,
            //InnerJoin<APTaxTran, On<APTaxTran.taxID, Equal<EPTaxTran.taxID>,
            //    And<APTaxTran.tranType, Equal<APInvoice.docType>,
            //    And<APTaxTran.refNbr, Equal<APInvoice.refNbr>>>>,
            //InnerJoin<Tax, On<Tax.taxID, Equal<EPTaxTran.taxID>,
            //    And<Where<ATPTTax.usrATPTPurchaseClassification, Equal<ATPTPurchaseClass.goods>,
            //    Or<ATPTTax.usrATPTPurchaseClassification, Equal<ATPTPurchaseClass.services>>>>>>>>>>>>(Base);

            //queryReplenishment.WhereAnd<Where<Tax.taxID, NotEqual<ATPTImportationTaxType.taxable1>, And<Tax.taxID, NotEqual<ATPTImportationTaxType.exempt1>>>>();
            //queryReplenishment.WhereAnd<Where<Tax.taxID, NotEqual<ATPTImportationTaxType.taxable2>, And<Tax.taxID, NotEqual<ATPTImportationTaxType.exempt2>>>>();
            //foreach (PXResult<EPTaxTran, CurrencyInfo, EPExpenseClaimDetails, ATPTEFMReplenishmentDetail, APInvoice, APTaxTran, Tax> q in queryReplenishment.Select())
            //{
            //    EPExpenseClaimDetails er = (EPExpenseClaimDetails)q;
            //    CurrencyInfo curyInfo = (CurrencyInfo)q;
            //    ATPTEFMEPExpenseClaimDetailsExt erExt = er.GetExtension<ATPTEFMEPExpenseClaimDetailsExt>();

            //    ATPTPurchasesJournalResult item = new ATPTPurchasesJournalResult();

            //    //Remove Employee Expense
            //    List<ATPTPurchasesJournalResult> removeExpenseReceipts = baseResult.Where(w => w.RefNbr == er.ClaimDetailCD && w.DocType == Attribute.Extension.ATPTAPDatDocType.ExpenseReceiptValue).ToList();
            //    foreach (ATPTPurchasesJournalResult toRemove in removeExpenseReceipts) { baseResult.Remove(toRemove); }

            //    //item.OrigModule = r.OrigModule;
            //    item.DocDate = er.ExpenseDate;
            //    item.VendorID = erExt.UsrATPTVendorID;
            //    item.VendorName = erExt.UsrATPTVendName;
            //    item.TaxRegistrationID = erExt.UsrATPTVendTIN;
            //    item.Address = erExt.UsrATPTAddress;
            //    item.Description = er.TranDesc;
            //    item.RefNbr = er.ClaimDetailCD;
            //    item.DocType = Attribute.Extension.ATPTAPDatDocType.ExpenseReceiptValue;
            //    item.VatablePurchases = decimal.Zero;
            //    item.NonVatablePurchases = decimal.Zero;
            //    item.ZeroRatedPurchases = decimal.Zero;
            //    item.ExemptPurchases = decimal.Zero;
            //    item.Discount = decimal.Zero;
            //    item.VATAmount = decimal.Zero;
            //    item.NetPurchases = decimal.Zero;
            //    item.GrossPurchases = decimal.Zero;

            //    EPTaxTran tran = (EPTaxTran)q;
            //    Tax tax = (Tax)q;
            //    ATPTTax taxExt = tax.GetExtension<ATPTTax>();

            //    decimal txbleamt = tran.TaxableAmt ?? decimal.Zero;
            //    decimal taxamt = tran.TaxAmt ?? decimal.Zero;

            //    #region MULTI CURRENCY 
            //    bool isCurrency = !String.IsNullOrEmpty(this.Base.Setup.Current.Currency) || !String.IsNullOrEmpty(this.Base.Setup.Current.CurrencyType);
            //    bool isMultiCurrency = curyInfo.BaseCuryID != curyInfo.CuryID;
            //    bool isMultiCurEqualToPhiltaxCur = curyInfo.CuryID == this.Base.Setup.Current.Currency;
            //    CurrencyRate curyRate = new CurrencyRate();

            //    if ((isCurrency) && (isMultiCurrency))
            //    {
            //        if (isMultiCurEqualToPhiltaxCur)
            //        {
            //            txbleamt = tran.CuryTaxableAmt ?? decimal.Zero;
            //            taxamt = tran.CuryTaxAmt ?? decimal.Zero;
            //        }
            //        else
            //        {
            //            curyRate = ATPTCurrencyHelper.getCurConversionByDate(this.Base, item.DocDate);

            //            if (curyRate.CuryRateID != null)
            //            {
            //                PXDBCurrencyAttribute.CuryConvCury(this.Base.Journal.Cache, curyRate, (Math.Round((decimal)tran.TaxableAmt, 2, MidpointRounding.AwayFromZero)), out txbleamt, true);
            //                PXDBCurrencyAttribute.CuryConvCury(this.Base.Journal.Cache, curyRate, (Math.Round((decimal)tran.TaxAmt, 2, MidpointRounding.AwayFromZero)), out taxamt, true);
            //            }
            //        }
            //    }
            //    else if ((isCurrency) && !(isMultiCurrency))
            //    {
            //        curyRate = ATPTCurrencyHelper.getCurConversionByDate(this.Base, item.DocDate);

            //        if (curyRate.CuryRateID != null)
            //        {
            //            PXDBCurrencyAttribute.CuryConvCury(this.Base.Journal.Cache, curyRate, (Math.Round((decimal)tran.TaxableAmt, 2, MidpointRounding.AwayFromZero)), out txbleamt, true);
            //            PXDBCurrencyAttribute.CuryConvCury(this.Base.Journal.Cache, curyRate, (Math.Round((decimal)tran.TaxAmt, 2, MidpointRounding.AwayFromZero)), out taxamt, true);
            //        }
            //    }
            //    #endregion

            //    if (taxExt.UsrATPTPurchaseType == ATPTPurchaseType.CapGoodsGreater1M || taxExt.UsrATPTPurchaseType == ATPTPurchaseType.CapGoodsLess1M ||
            //        taxExt.UsrATPTPurchaseType == ATPTPurchaseType.DomGoods || taxExt.UsrATPTPurchaseType == ATPTPurchaseType.DomServices || taxExt.UsrATPTPurchaseType == ATPTPurchaseType.Others)
            //    {
            //        item.VatablePurchases += txbleamt;
            //    }
            //    if (taxExt.UsrATPTPurchaseType == ATPTPurchaseType.ZeroRated)
            //        item.ZeroRatedPurchases += txbleamt;
            //    if (taxExt.UsrATPTPurchaseType == ATPTPurchaseType.VATExempt)
            //        item.ExemptPurchases += txbleamt;
            //    if (taxExt.UsrATPTPurchaseType == ATPTPurchaseType.PurchNotQualForInput)
            //        item.NonVatablePurchases += txbleamt;

            //    item.VATAmount += taxamt;

            //    item.NetPurchases = item.VatablePurchases + item.NonVatablePurchases + item.ZeroRatedPurchases + item.ExemptPurchases;

            //    item.GrossPurchases = item.NetPurchases + item.Discount + item.VATAmount;

            //    baseResult.Add(item);
            //}
            #endregion

            #region Check bill
            //if (Base.Setup.Current.ManualProcessVatRecog == false) 
            //{
            //    PXSelectBase<APInvoice> check = new PXSelectReadonly2<APInvoice,
            //    LeftJoin<EPExpenseClaimDetails, On<APInvoice.refNbr, Equal<EPExpenseClaimDetails.aPRefNbr>>,
            //    LeftJoin<ATPTEFMReplenishmentDetail, On<APInvoice.refNbr, Equal<ATPTEFMReplenishmentDetail.invoiceRefNbr>>>>,
            //    Where<APInvoice.docDate, GreaterEqual<Current<ATPTPurchasesJournalFilter.startDate>>,
            //        And<APInvoice.docDate, LessEqual<Current<ATPTPurchasesJournalFilter.endDate>>,
            //        And<APInvoice.released, Equal<True>,
            //        And<APInvoice.isMigratedRecord, NotEqual<True>,
            //        And<Where<APInvoice.docType, Equal<APDocType.invoice>,
            //            Or<APInvoice.docType, Equal<APDocType.debitAdj>,
            //            Or<APInvoice.docType, Equal<APDocType.creditAdj>>>>>>>>>>(Base);

            //    foreach (PXResult<APInvoice, EPExpenseClaimDetails, ATPTEFMReplenishmentDetail> res in check.Select())
            //    {
            //        APInvoice inv = res;
            //        EPExpenseClaimDetails ec = res;
            //        ATPTEFMReplenishmentDetail rep = res;

            //        //Skip if there are no Replenishment and Expense Claim
            //        if (ec.RefNbr == null && rep.InvoiceRefNbr == null) continue;

            //        //Remove Bills
            //        List<ATPTPurchasesJournalResult> removeInvoices = baseResult.Where(w => w.RefNbr == inv.RefNbr && w.DocType == inv.DocType).ToList();
            //        foreach (ATPTPurchasesJournalResult r in removeInvoices) { baseResult.Remove(r); }

            //        //Remove Checks
            //        APAdjust checkPayment = PXSelect<APAdjust, Where<APAdjust.adjdRefNbr, Equal<Required<APAdjust.adjdRefNbr>>>>.Select(Base, inv.RefNbr);

            //        if (checkPayment != null)
            //        {
            //            List<ATPTPurchasesJournalResult> removeChecks = baseResult.Where(w => w.RefNbr == checkPayment.AdjgRefNbr && w.DocType == checkPayment.AdjgDocType).ToList();
            //            foreach (ATPTPurchasesJournalResult r in removeChecks) { baseResult.Remove(r); }
            //        }
            //    }



            //}
                    
            #endregion

            return baseResult;
        }

        public PXAction<ATPTPurchasesJournalFilter> ATPTViewPurchDocumentOverride;
        [PXUIField(DisplayName = "", MapEnableRights = PXCacheRights.Select, MapViewRights = PXCacheRights.Select)]
        [PXEditDetailButton]
        public virtual void aTPTViewPurchDocumentOverride()
        {
            ATPTPurchasesJournalResult row = Base.Journal.Current;
            if (row == null) return;

            if(row.DocType == Attribute.Extension.ATPTAPDatDocType.ExpenseReceiptValue)
            {
                PX.Objects.EP.ExpenseClaimDetailEntry graph = PXGraph.CreateInstance<PX.Objects.EP.ExpenseClaimDetailEntry>();
                graph.ClaimDetails.Current = graph.ClaimDetails.Search<EPExpenseClaimDetails.claimDetailCD>(row.RefNbr);
                throw new PXRedirectRequiredException(graph, true, "Expense Receipts") { Mode = PXBaseRedirectException.WindowMode.NewWindow };
            }
            else
            {
                Base.ViewPurchDocument();
            }
            
        }


        [PXRemoveBaseAttribute(typeof(APDocType.ListAttribute))]
        [PXMergeAttributes(Method = MergeMethod.Merge)]
        [Attribute.Extension.ATPTAPDatDocType]
        public virtual void ATPTPurchasesJournalResult_DocType_CacheAttached(PXCache sender){}

        [PXRemoveBaseAttribute(typeof(PXUIFieldAttribute))]
        [PXMergeAttributes(Method = MergeMethod.Merge)]
        [PXUIField(DisplayName = CashFundManagement.Messages.ATPTEFMMessages.RefNbr)]
        public virtual void ATPTPurchasesJournalResult_RefNbr_CacheAttached(PXCache sender) { }
    }
}