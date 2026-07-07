using ATPTPhilippineTax.Attributes;
using ATPTPhilippineTax.DAC.Extensions;
using ATPTPhilippineTax.Helpers;
using CashFundManagement.Extensions.DAC;
using CashFundManagement.Helper;
using PX.Data;
using PX.Objects.AP;
using PX.Objects.CM;
using PX.Objects.EP;
using PX.Objects.TX;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using static ATPTPhilippineTax.Graph.Inquiry.ATPTPurchasesScheduleInq;

namespace CashFundPhilippineTax.Graph.Extension 
{
    /// <remarks>
    /// 2024-08-14 : Adds multi-tenant support. {RRS} <br/>
    /// 2025-02-13:  Apply multi tenant to 24r1 CASE: 010269 {JLTG}
    /// </remarks>
    public class ATPTPurchasesScheduleInq : PXGraphExtension<ATPTPhilippineTax.Graph.Inquiry.ATPTPurchasesScheduleInq>
    {
#if Version23R2
        public static bool IsActive() => ATPTModule.IsActive;
#else
        public static bool IsActive() => ATPTModule.IsActive && ATPTEFMPrefetchSetup.IsActive;
#endif

        protected virtual IEnumerable schedule()
        {
            ATPTPurchasesScheduleFilter filter = Base.Filter.Current;
            List<ATPTPurchasesScheduleResult> baseResult = Base.GetSchedule(filter);
            List<ATPTPurchasesScheduleResult> result = new List<ATPTPurchasesScheduleResult>();
            List<CurrencyRate> curyRates = ATPTHelpers.GetRates(Base, filter.StartDate, filter.EndDate);

            //Get filter type
            //Add condition to query base on filter type

            #region Employee Expenses

            PXSelectBase<EPTaxTran> queryExpenses = new PXSelectReadonly2<EPTaxTran,
                InnerJoin<CurrencyInfo, On<CurrencyInfo.curyInfoID, Equal<EPTaxTran.curyInfoID>>,
                InnerJoin<EPExpenseClaimDetails, On<EPExpenseClaimDetails.claimDetailID, Equal<EPTaxTran.claimDetailID>>,
                InnerJoin<APInvoice, On<APInvoice.refNbr, Equal<EPExpenseClaimDetails.aPRefNbr>,
                    And<EPExpenseClaimDetails.expenseDate, GreaterEqual<Current<ATPTPurchasesScheduleFilter.startDate>>,
                    And<EPExpenseClaimDetails.expenseDate, LessEqual<Current<ATPTPurchasesScheduleFilter.endDate>>,
                    And<APInvoice.released, Equal<True>,
                    And<APInvoice.isMigratedRecord, NotEqual<True>,
                    And<Where<APInvoice.docType, Equal<APDocType.invoice>,
                        Or<APInvoice.docType, Equal<APDocType.debitAdj>,
                        Or<APInvoice.docType, Equal<APDocType.creditAdj>>>>>>>>>>,
                InnerJoin<APTaxTran, On<APTaxTran.taxID, Equal<EPTaxTran.taxID>,
                    And<APTaxTran.tranType, Equal<APInvoice.docType>,
                    And<APTaxTran.refNbr, Equal<APInvoice.refNbr>>>>,
                InnerJoin<Tax, On<Tax.taxID, Equal<EPTaxTran.taxID>>>>>>>>(Base);

            //queryExpenses.WhereAnd<Where<Tax.taxID, NotEqual<ATPTImportationTaxType.taxable1>, And<Tax.taxID, NotEqual<ATPTImportationTaxType.exempt1>>>>();
            //queryExpenses.WhereAnd<Where<Tax.taxID, NotEqual<ATPTImportationTaxType.taxable2>, And<Tax.taxID, NotEqual<ATPTImportationTaxType.exempt2>>>>();

            if (filter.ScheduleType == ATPTTaxClassificationAttribute.Goods)
            {
                queryExpenses.WhereAnd<Where<ATPTTax.usrATPTPurchaseClassification, Equal<ATPTTaxClassificationAttribute.goods>>>();
            }
            else if (filter.ScheduleType == ATPTTaxClassificationAttribute.Services)
            {
                queryExpenses.WhereAnd<Where<ATPTTax.usrATPTPurchaseClassification, Equal<ATPTTaxClassificationAttribute.services>>>();
            }
            else
            {
                queryExpenses.WhereAnd<Where<ATPTTax.usrATPTPurchaseClassification, Equal<ATPTTaxClassificationAttribute.goods>,
                    Or<ATPTTax.usrATPTPurchaseClassification, Equal<ATPTTaxClassificationAttribute.services>>>>();
            }

            foreach (PXResult<EPTaxTran, CurrencyInfo, EPExpenseClaimDetails, APInvoice, APTaxTran, Tax> q in queryExpenses.Select())
            {
                EPExpenseClaimDetails er = (EPExpenseClaimDetails)q;
                CurrencyInfo curyInfo = (CurrencyInfo)q;

                ATPTPurchasesScheduleResult item = new ATPTPurchasesScheduleResult();

                //item.OrigModule = r.OrigModule;
                item.RefNbr = er.ClaimDetailCD;
                item.DocDate = er.ExpenseDate;

                ATPTEFMEPExpenseClaimDetailsExt erExt = er.GetExtension<ATPTEFMEPExpenseClaimDetailsExt>();
                item.VendorID = erExt.UsrATPTVendID; /*erExt.UsrATPTVendorID;*/
                item.VendorName = erExt.UsrATPTVendName;
                item.TaxRegistrationID = erExt.UsrATPTVendTIN;
                item.Description = er.TranDesc;

                item.GrossPurchases = decimal.Zero;
                item.DiscountTotal = decimal.Zero;
                item.InputVAT = decimal.Zero;
                item.NetPurchases = decimal.Zero;

                item.AddressLine1 = erExt.UsrATPTAddress;

                //TAXES
                EPTaxTran taxTran = (EPTaxTran)q;
                Tax tax = (Tax)q;
                ATPTTax taxExt = tax.GetExtension<ATPTTax>();

                item.TaxID = tax.TaxID;
                item.PurchaseClass = taxExt.UsrATPTPurchaseClassification;
                item.PurchaseType = taxExt.UsrATPTPurchaseType;

                decimal txbleamt = taxTran.TaxableAmt ?? decimal.Zero;
                decimal taxamt = taxTran.TaxAmt ?? decimal.Zero;

                #region MULTI CURRENCY 
                bool isCurrency = !String.IsNullOrEmpty(this.Base.Setup.Current.PhilTaxCurrency) || !String.IsNullOrEmpty(this.Base.Setup.Current.PhilTaxCurrencyType);
                bool isMultiCurrency = curyInfo.BaseCuryID != curyInfo.CuryID;
                bool isMultiCurEqualToPhiltaxCur = curyInfo.CuryID == this.Base.Setup.Current.PhilTaxCurrency;
                CurrencyRate curyRate = new CurrencyRate();

                if ((isCurrency) && (isMultiCurrency))
                {
                    if (isMultiCurEqualToPhiltaxCur)
                    {
                        txbleamt = taxTran.CuryTaxableAmt ?? decimal.Zero;
                        taxamt = taxTran.CuryTaxAmt ?? decimal.Zero;
                    }
                    else
                    {
                        curyRate = ATPTHelpers.GetSpecificRate(curyRates, item.DocDate);

                        if (curyRate.CuryRateID != null)
                        {
                            PXDBCurrencyAttribute.CuryConvCury(this.Base.Schedule.Cache, curyRate, (Math.Round((decimal)taxTran.TaxableAmt, 2, MidpointRounding.AwayFromZero)), out txbleamt, true);
                            PXDBCurrencyAttribute.CuryConvCury(this.Base.Schedule.Cache, curyRate, (Math.Round((decimal)taxTran.TaxAmt, 2, MidpointRounding.AwayFromZero)), out taxamt, true);
                        }
                    }
                }
                else if ((isCurrency) && !(isMultiCurrency))
                {
                    curyRate = ATPTHelpers.GetSpecificRate(curyRates, item.DocDate);

                    if (curyRate.CuryRateID != null)
                    {
                        PXDBCurrencyAttribute.CuryConvCury(this.Base.Schedule.Cache, curyRate, (Math.Round((decimal)taxTran.TaxableAmt, 2, MidpointRounding.AwayFromZero)), out txbleamt, true);
                        PXDBCurrencyAttribute.CuryConvCury(this.Base.Schedule.Cache, curyRate, (Math.Round((decimal)taxTran.TaxAmt, 2, MidpointRounding.AwayFromZero)), out taxamt, true);
                    }
                }
                #endregion

                item.InputVAT = taxamt;
                item.NetPurchases = txbleamt;

                //DISCOUNTS
                item.GrossPurchases = decimal.Zero;
                item.DiscountTotal = decimal.Zero;
                item.GrossPurchases = (txbleamt) + (taxamt);

                if (!result.Any(a => a.RefNbr == item.RefNbr && a.TaxID == item.TaxID))
                {
                    result.Add(item);
                }
            }

            #endregion

            #region Replenishment
            //PXSelectBase<EPTaxTran> queryReplenishment = new PXSelectReadonly2<EPTaxTran,
            //InnerJoin<CurrencyInfo, On<CurrencyInfo.curyInfoID, Equal<EPTaxTran.curyInfoID>>,
            //InnerJoin<EPExpenseClaimDetails, On<EPExpenseClaimDetails.claimDetailID, Equal<EPTaxTran.claimDetailID>>,
            //InnerJoin<ATPTEFMReplenishmentDetail, On<ATPTEFMReplenishmentDetail.expenseReceiptNbr, Equal<EPExpenseClaimDetails.claimDetailCD>>,
            //InnerJoin<APInvoice, On<APInvoice.refNbr, Equal<ATPTEFMReplenishmentDetail.invoiceRefNbr>,
            //    And<EPExpenseClaimDetails.expenseDate, GreaterEqual<Current<ATPTPurchasesScheduleFilter.startDate>>,
            //    And<EPExpenseClaimDetails.expenseDate, LessEqual<Current<ATPTPurchasesScheduleFilter.endDate>>,
            //    And<APInvoice.released, Equal<True>,
            //    And<APInvoice.isMigratedRecord, NotEqual<True>,
            //    And<Where<APInvoice.docType, Equal<APDocType.invoice>,
            //        Or<APInvoice.docType, Equal<APDocType.debitAdj>,
            //        Or<APInvoice.docType, Equal<APDocType.creditAdj>>>>>>>>>>,
            //InnerJoin<APTaxTran, On<APTaxTran.taxID, Equal<EPTaxTran.taxID>,
            //    And<APTaxTran.tranType, Equal<APInvoice.docType>,
            //    And<APTaxTran.refNbr, Equal<APInvoice.refNbr>>>>,
            //InnerJoin<Tax, On<Tax.taxID, Equal<EPTaxTran.taxID>>>>>>>>>(Base);

            //queryReplenishment.WhereAnd<Where<Tax.taxID, NotEqual<ATPTImportationTaxType.taxable1>, And<Tax.taxID, NotEqual<ATPTImportationTaxType.exempt1>>>>();
            //queryReplenishment.WhereAnd<Where<Tax.taxID, NotEqual<ATPTImportationTaxType.taxable2>, And<Tax.taxID, NotEqual<ATPTImportationTaxType.exempt2>>>>();

            //if (filter.ScheduleType == ATPTPurchaseClass.Goods)
            //{
            //    queryReplenishment.WhereAnd<Where<ATPTTax.usrATPTPurchaseClassification, Equal<ATPTPurchaseClass.goods>>>();
            //}
            //else if (filter.ScheduleType == ATPTPurchaseClass.Services)
            //{
            //    queryReplenishment.WhereAnd<Where<ATPTTax.usrATPTPurchaseClassification, Equal<ATPTPurchaseClass.services>>>();
            //}
            //else
            //{
            //    queryReplenishment.WhereAnd<Where<ATPTTax.usrATPTPurchaseClassification, Equal<ATPTPurchaseClass.goods>,
            //        Or<ATPTTax.usrATPTPurchaseClassification, Equal<ATPTPurchaseClass.services>>>>();
            //}

            //foreach (PXResult<EPTaxTran, CurrencyInfo, EPExpenseClaimDetails, ATPTEFMReplenishmentDetail, APInvoice, APTaxTran, Tax> q in queryReplenishment.Select())
            //{
            //    EPExpenseClaimDetails er = (EPExpenseClaimDetails)q;
            //    CurrencyInfo curyInfo = (CurrencyInfo)q;

            //    ATPTPurchasesScheduleResult item = new ATPTPurchasesScheduleResult();
            //    //&& w. == Attribute.Extension.ATPTAPDatDocType.ExpenseReceiptValue

            //    //Remove Employee Expense
            //    List<ATPTPurchasesScheduleResult> removeExpenseReceipts = baseResult.Where(w => w.RefNbr == er.ClaimDetailCD).ToList();
            //    foreach (ATPTPurchasesScheduleResult toRemove in removeExpenseReceipts) { baseResult.Remove(toRemove); }

            //    //item.OrigModule = r.OrigModule;
            //    item.RefNbr = er.ClaimDetailCD;
            //    item.DocDate = er.ExpenseDate;

            //    ATPTEFMEPExpenseClaimDetailsExt erExt = er.GetExtension<ATPTEFMEPExpenseClaimDetailsExt>();
            //    item.VendorID = erExt.UsrATPTVendorID;
            //    item.VendorName = erExt.UsrATPTVendName;
            //    item.TaxRegistrationID = erExt.UsrATPTVendTIN;
            //    item.Description = er.TranDesc;

            //    item.GrossPurchases = decimal.Zero;
            //    item.DiscountTotal = decimal.Zero;
            //    item.InputVAT = decimal.Zero;
            //    item.NetPurchases = decimal.Zero;

            //    item.AddressLine1 = erExt.UsrATPTAddress;

            //    //TAXES
            //    EPTaxTran taxTran = (EPTaxTran)q;
            //    Tax tax = (Tax)q;
            //    ATPTTax taxExt = tax.GetExtension<ATPTTax>();

            //    item.TaxID = tax.TaxID;
            //    item.PurchaseClass = taxExt.UsrATPTPurchaseClassification;
            //    item.PurchaseType = taxExt.UsrATPTPurchaseType;

            //    decimal txbleamt = taxTran.TaxableAmt ?? decimal.Zero;
            //    decimal taxamt = taxTran.TaxAmt ?? decimal.Zero;

            //    #region MULTI CURRENCY 
            //    bool isCurrency = !String.IsNullOrEmpty(this.Base.Setup.Current.Currency) || !String.IsNullOrEmpty(this.Base.Setup.Current.CurrencyType);
            //    bool isMultiCurrency = curyInfo.BaseCuryID != curyInfo.CuryID;
            //    bool isMultiCurEqualToPhiltaxCur = curyInfo.CuryID == this.Base.Setup.Current.Currency;
            //    CurrencyRate curyRate = new CurrencyRate();

            //    if ((isCurrency) && (isMultiCurrency))
            //    {
            //        if (isMultiCurEqualToPhiltaxCur)
            //        {
            //            txbleamt = taxTran.CuryTaxableAmt ?? decimal.Zero;
            //            taxamt = taxTran.CuryTaxAmt ?? decimal.Zero;
            //        }
            //        else
            //        {
            //            curyRate = ATPTCurrencyHelper.getCurConversionByDate(this.Base, item.DocDate);

            //            if (curyRate.CuryRateID != null)
            //            {
            //                PXDBCurrencyAttribute.CuryConvCury(this.Base.Schedule.Cache, curyRate, (Math.Round((decimal)taxTran.TaxableAmt, 2, MidpointRounding.AwayFromZero)), out txbleamt, true);
            //                PXDBCurrencyAttribute.CuryConvCury(this.Base.Schedule.Cache, curyRate, (Math.Round((decimal)taxTran.TaxAmt, 2, MidpointRounding.AwayFromZero)), out taxamt, true);
            //            }
            //        }
            //    }
            //    else if ((isCurrency) && !(isMultiCurrency))
            //    {
            //        curyRate = ATPTCurrencyHelper.getCurConversionByDate(this.Base, item.DocDate);

            //        if (curyRate.CuryRateID != null)
            //        {
            //            PXDBCurrencyAttribute.CuryConvCury(this.Base.Schedule.Cache, curyRate, (Math.Round((decimal)taxTran.TaxableAmt, 2, MidpointRounding.AwayFromZero)), out txbleamt, true);
            //            PXDBCurrencyAttribute.CuryConvCury(this.Base.Schedule.Cache, curyRate, (Math.Round((decimal)taxTran.TaxAmt, 2, MidpointRounding.AwayFromZero)), out taxamt, true);
            //        }
            //    }
            //    #endregion

            //    item.InputVAT = taxamt;
            //    item.NetPurchases = txbleamt;

            //    //DISCOUNTS
            //    item.GrossPurchases = decimal.Zero;
            //    item.DiscountTotal = decimal.Zero;
            //    item.GrossPurchases = txbleamt + taxamt;

            //    //APTran arTran = PXSelectJoinGroupBy<APTran,
            //    //    InnerJoin<TaxCategoryDet, On<TaxCategoryDet.taxCategoryID, Equal<APTran.taxCategoryID>,
            //    //        And<TaxCategoryDet.taxID, Equal<Required<TaxCategoryDet.taxID>>>>>,
            //    //    Where<APTran.tranType, Equal<Required<APTran.tranType>>,
            //    //        And<APTran.refNbr, Equal<Required<APTran.refNbr>>>>,
            //    //    Aggregate<Sum<ARTran.curyTranAmt>>>
            //    //.Select(this, tax.TaxID, invoice.DocType, invoice.RefNbr);

            //    //if (arTran != null)
            //    //{
            //    //    //item.GrossSales = arTran.CuryTranAmt ?? decimal.Zero;
            //    //    if (arTran.CuryTranAmt == 0) { continue; }
            //    //    decimal? discount = invoice.CuryLineTotal == 0 ? 0 : (arTran.CuryTranAmt / invoice.CuryLineTotal) * invoice.CuryDiscTot;
            //    //    item.DiscountTotal = discount;
            //    //    item.DiscountTotal = (arTran.CuryTranAmt / invoice.CuryLineTotal) * invoice.CuryDiscTot;
            //    //}
            //    //else
            //    //{
            //    //    decimal? prevAmount = result.Where(w => w.RefNbr == invoice.RefNbr).Sum(s => s.GrossPurchases);
            //    //    decimal? prevAmountNet = result.Where(w => w.RefNbr == invoice.RefNbr).Sum(s => s.NetPurchases);
            //    //    item.GrossPurchases = (invoice.CuryLineTotal ?? decimal.Zero) - prevAmount;
            //    //    item.NetPurchases = (invoice.CuryOrigDocAmt ?? decimal.Zero) - prevAmountNet;

            //    //    decimal? prevDiscount = result.Where(w => w.RefNbr == invoice.RefNbr).Sum(s => s.DiscountTotal);

            //    //    decimal? discount = invoice.CuryLineTotal == 0 ? 0 : (item.GrossPurchases / invoice.CuryLineTotal) * invoice.CuryDiscTot;
            //    //    item.DiscountTotal = discount;
            //    //    //item.DiscountTotal = ((item.GrossSales / invoice.CuryLineTotal) * invoice.CuryDiscTot) - prevDiscount;

            //    //}

            //    //if (invoice.DocType == APDocType.DebitAdj)
            //    //{
            //    //    item.GrossPurchases *= -1;
            //    //    item.InputVAT *= -1;
            //    //    item.NetPurchases *= -1;
            //    //    item.DiscountTotal *= -1;
            //    //}

            //    //if (invoice.DocType == APDocType.DebitAdj && arTran == null) continue;

            //    if (!result.Any(a => a.RefNbr == item.RefNbr && a.TaxID == item.TaxID))
            //    {
            //        result.Add(item);
            //    }

            //}
            #endregion

            #region Check bill

            //if (Base.Setup.Current.ManualProcessVatRecog == false)
            //{
            //    PXSelectBase<APInvoice> check = new PXSelectReadonly2<APInvoice,
            //    LeftJoin<EPExpenseClaimDetails, On<APInvoice.refNbr, Equal<EPExpenseClaimDetails.aPRefNbr>>,
            //    LeftJoin<ATPTEFMReplenishmentDetail, On<APInvoice.refNbr, Equal<ATPTEFMReplenishmentDetail.invoiceRefNbr>>>>,
            //    Where<APInvoice.docDate, GreaterEqual<Current<ATPTPurchasesScheduleFilter.startDate>>,
            //        And<APInvoice.docDate, LessEqual<Current<ATPTPurchasesScheduleFilter.endDate>>,
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
            //        List<ATPTPurchasesScheduleResult> removeInvoices = baseResult.Where(w => w.RefNbr == inv.RefNbr && w.DocType == inv.DocType).ToList();
            //        foreach (ATPTPurchasesScheduleResult r in removeInvoices) { baseResult.Remove(r); }

            //        //Remove Checks
            //        APAdjust checkPayment = PXSelect<APAdjust, Where<APAdjust.adjdRefNbr, Equal<Required<APAdjust.adjdRefNbr>>>>.Select(Base, inv.RefNbr);

            //        if (checkPayment != null)
            //        {
            //            List<ATPTPurchasesScheduleResult> removeChecks = baseResult.Where(w => w.RefNbr == checkPayment.AdjgRefNbr && w.DocType == checkPayment.AdjgDocType).ToList();
            //            foreach (ATPTPurchasesScheduleResult r in removeChecks) { baseResult.Remove(r); }
            //        }
            //    }



            //}

            #endregion

            return baseResult;
        }

        public PXAction<ATPTPurchasesScheduleFilter> ATPTViewDocument;
        [PXUIField(DisplayName = "", MapEnableRights = PXCacheRights.Select, MapViewRights = PXCacheRights.Select)]
        [PXEditDetailButton]
        public virtual void aTPTViewDocument()
        {
            ATPTPurchasesScheduleResult row = Base.Schedule.Current;
            if (row == null) return;

            if (row.OrigModule == PX.Objects.GL.BatchModule.EP)
            {
                PX.Objects.EP.ExpenseClaimDetailEntry graph = PXGraph.CreateInstance<PX.Objects.EP.ExpenseClaimDetailEntry>();
                graph.ClaimDetails.Current = graph.ClaimDetails.Search<EPExpenseClaimDetails.claimDetailCD>(row.RefNbr);
                throw new PXRedirectRequiredException(graph, true, "Expense Receipts") { Mode = PXBaseRedirectException.WindowMode.NewWindow };
            }
            else
            {
                PX.Objects.AP.APInvoiceEntry graph = PXGraph.CreateInstance<PX.Objects.AP.APInvoiceEntry>();
                graph.Document.Current = graph.Document.Search<PX.Objects.AP.APInvoice.refNbr>(row.RefNbr);
                throw new PXRedirectRequiredException(graph, true, "Bills") { Mode = PXBaseRedirectException.WindowMode.NewWindow };
            }

        }

        [PXRemoveBaseAttribute(typeof(PXUIFieldAttribute))]
        [PXMergeAttributes(Method = MergeMethod.Merge)]
        [PXUIField(DisplayName = CashFundManagement.Messages.ATPTEFMMessages.RefNbr)]
        public virtual void ATPTPurchasesScheduleResult_RefNbr_CacheAttached(PXCache sender) { }
    }
}