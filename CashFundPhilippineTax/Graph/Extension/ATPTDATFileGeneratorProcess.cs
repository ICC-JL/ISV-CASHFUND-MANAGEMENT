using ATPTPhilippineTax.Helpers;
using CashFundManagement.Helper;
using PX.Data;
using PX.Objects.AP;
using PX.Objects.CM;
using PX.Objects.EP;
using System.Collections;
using System.Collections.Generic;
using static ATPTPhilippineTax.Graph.Process.ATPTDATFileGeneratorProcess;

namespace CashFundPhilippineTax.Graph.Extension 
{
    /// <remarks>
    /// 2024-08-14 : Adds multi-tenant support. {RRS} <br/>
    /// 2025-02-13:  Apply multi tenant to 24r1 CASE: 010269 {JLTG}
    /// </remarks>
    public class ATPTDATFileGeneratorProcess : PXGraphExtension<ATPTPhilippineTax.Graph.Process.ATPTDATFileGeneratorProcess>
    {
#if Version23R2
        public static bool IsActive() => ATPTModule.IsActive;
#else
        public static bool IsActive() => ATPTModule.IsActive && ATPTEFMPrefetchSetup.IsActive;
#endif

        /// <remarks>
        /// 2025-09-17 : 013523 - Removed purchDocuments delegate. Cannot generate record. {JCL}
        /// </remarks>

        public sealed class ATPTPurchaseDATResultExt : PXCacheExtension<ATPTPurchaseDATResult>
        {
            public static bool IsActive() => true;

            [PXRemoveBaseAttribute(typeof(APDocType.ListAttribute))]
            [PXMergeAttributes(Method = MergeMethod.Merge)]
            [Attribute.Extension.ATPTAPDatDocType]
            [PXString]
            public string DocType { get; set; }
        }

        [PXOverride]

        public virtual void ViewPurchDocument()
        {
            if (this.Base.PurchDocuments.Current != null)
            {
                if (this.Base.PurchDocuments.Current.DocType == Attribute.Extension.ATPTAPDatDocType.ExpenseReceiptValue)
                {
                    ATPTPurchFilter aTPTPurchFilter = this.Base.Purchfilter.Current;
                    PX.Objects.EP.ExpenseClaimDetailEntry graph = PXGraph.CreateInstance<PX.Objects.EP.ExpenseClaimDetailEntry>();
                    EPExpenseClaimDetails record = PXSelectReadonly<EPExpenseClaimDetails,
                        Where<EPExpenseClaimDetails.claimDetailCD, Equal<Required<EPExpenseClaimDetails.claimDetailCD>>>>
                        .Select(Base, this.Base.PurchDocuments.Current.RefNbr);

                    graph.ClaimDetails.Current = record;
                    throw new PXRedirectRequiredException(graph, true, "Expense Receipt") { Mode = PXBaseRedirectException.WindowMode.NewWindow };
                }
                else
                {
                    this.Base.ViewPurchDocument();
                }
            }
        }

        protected virtual IEnumerable eWTPurchDocuments()
        {            
            ATPTEWTPurchFilter filter = this.Base.ewtpurchfilter.Current;

            List<ATPTEWTPurchDATResult> result = this.Base.GetEWTPurchaseDocuments(filter);

            List<CurrencyRate> curyRates = ATPTHelpers.GetRates(Base, filter.StartDate, filter.EndDate);

            //PXSelectBase<EPExpenseClaimDetails> query = new PXSelectReadonly2<EPExpenseClaimDetails,
            //    InnerJoin<EPTaxTran, On<EPTaxTran.claimDetailID, Equal<EPExpenseClaimDetails.claimDetailID>>,
            //    InnerJoin<Tax, On<Tax.taxID, Equal<EPTaxTran.taxID>, And<Tax.taxType, Equal<CSTaxType.withholding>>>,
            //    InnerJoin<CurrencyInfo, On<CurrencyInfo.curyInfoID, Equal<EPTaxTran.curyInfoID>>,
            //    InnerJoin<APInvoice, On<APInvoice.refNbr, Equal<EPExpenseClaimDetails.aPRefNbr>>>>>>,
            //    Where<EPExpenseClaimDetails.expenseDate, GreaterEqual<Required<EPExpenseClaimDetails.expenseDate>>,
            //    And<EPExpenseClaimDetails.expenseDate, LessEqual<Required<EPExpenseClaimDetails.expenseDate>>,
            //    And<EPExpenseClaimDetails.aPRefNbr, IsNotNull>>>>(Base);

            //foreach (PXResult<EPExpenseClaimDetails, EPTaxTran, Tax, CurrencyInfo, APInvoice> q in query.Select(filter.StartDate, filter.EndDate))
            //{
            //    EPExpenseClaimDetails er = q;
            //    EPTaxTran ertx = q;
            //    Tax tx = q;
            //    APInvoice inv = q;
            //    CurrencyInfo curyInfo = q;

            //    bool isNegative = (er.APDocType == APDocType.DebitAdj);

            //    //Remove Bills
            //    List<ATPTEWTPurchDATResult> removeInvoices = result.Where(w => w.RefNbr == inv.RefNbr && w.DocType == inv.DocType).ToList();
            //    foreach (ATPTEWTPurchDATResult r in removeInvoices) { result.Remove(r); }

            //    //Remove Checks
            //    APAdjust checkPayment = PXSelect<APAdjust, Where<APAdjust.adjdRefNbr, Equal<Required<APAdjust.adjdRefNbr>>>>.Select(Base, inv.RefNbr);

            //    if (checkPayment != null)
            //    {
            //        List<ATPTEWTPurchDATResult> removeChecks = result.Where(w => w.RefNbr == checkPayment.AdjgRefNbr && w.DocType == checkPayment.AdjgDocType).ToList();
            //        foreach (ATPTEWTPurchDATResult r in removeChecks) { result.Remove(r); }
            //    }

            //    ATPTEWTPurchDATResult item = new ATPTEWTPurchDATResult();

            //    item.DocType = Attribute.Extension.ATPTAPDatDocType.ExpenseReceiptValue;
            //    item.RefNbr = er.ClaimDetailCD;

            //    DateTime docdate = (DateTime)er.ExpenseDate;
            //    DateTime endOfMonth = new DateTime(docdate.Year, docdate.Month,
            //                  DateTime.DaysInMonth(docdate.Year, docdate.Month));
            //    item.TaxableMonth = docdate;

            //    ATPTEPExpenseClaimDetails ecExt = er.GetExtension<ATPTEPExpenseClaimDetails>();

            //    item.TaxRegistrationID = ecExt.UsrATPTVendTin;
            //    item.RegisteredName = ecExt.UsrATPTVendName;
            //    item.AddressLine1 = ecExt.UsrATPTAddress;
            //    item.AddressLine2 = string.Empty;

            //    item.LastName = string.Empty;
            //    item.FirstName = string.Empty;
            //    item.MidName = string.Empty;

            //    item.ATCCode = ertx.TaxID;
            //    item.NaturePayment = tx.Descr;

            //    decimal txbleamt = ertx.TaxableAmt ?? decimal.Zero;

            //    #region MULTI CURRENCY 
            //    bool isPhiltaxCurrency = !String.IsNullOrEmpty(this.Base.philTaxSetup.Current.PhilTaxCurrency) || !String.IsNullOrEmpty(this.Base.philTaxSetup.Current.PhilTaxCurrencyType);
            //    bool isMultiCurrency = curyInfo.BaseCuryID != curyInfo.CuryID;
            //    bool isMultiCurEqualToPhiltaxCur = curyInfo.CuryID == this.Base.philTaxSetup.Current.PhilTaxCurrency;
            //    CurrencyRate curyRate = new CurrencyRate();

            //    if ((isPhiltaxCurrency) && (isMultiCurrency))
            //    {
            //        if (isMultiCurEqualToPhiltaxCur)
            //        {
            //            txbleamt = ertx.CuryTaxableAmt ?? decimal.Zero;
            //        }
            //        else
            //        {
            //            curyRate = ATPTHelpers.GetSpecificRate(curyRates, item.TaxableMonth);

            //            if (curyRate.CuryRateID != null)
            //                PXDBCurrencyAttribute.CuryConvCury(this.Base.EWTPurchDocuments.Cache, curyRate, (Math.Round((decimal)ertx.TaxableAmt, 2, MidpointRounding.AwayFromZero)), out txbleamt, true);
            //        }
            //    }
            //    else if ((isPhiltaxCurrency) && !(isMultiCurrency))
            //    {
            //        curyRate = ATPTHelpers.GetSpecificRate(curyRates, item.TaxableMonth);

            //        if (curyRate.CuryRateID != null)
            //            PXDBCurrencyAttribute.CuryConvCury(this.Base.EWTPurchDocuments.Cache, curyRate, (Math.Round((decimal)ertx.TaxableAmt, 2, MidpointRounding.AwayFromZero)), out txbleamt, true);
            //    }
            //    #endregion

            //    item.AmountIncomePayment = (isNegative == true ? (txbleamt) * -1 : (txbleamt));
            //    item.TaxRate = ertx.TaxRate ?? decimal.Zero;
            //    item.AmountWTax = (isNegative == true ? ((txbleamt) * ((ertx.TaxRate ?? decimal.Zero) / 100)) * -1 :
            //        (txbleamt) * ((ertx.TaxRate ?? decimal.Zero) / 100));

            //    if (!result.Any(a => a.ATCCode == item.ATCCode && a.RefNbr == item.RefNbr))
            //    {
            //        result.Add(item);
            //    }

            //}

            return result;

        }
        public sealed class ATPTEWTPurchDATResultExt : PXCacheExtension<ATPTEWTPurchDATResult>
        {
            public static bool IsActive() => true;

            [PXRemoveBaseAttribute(typeof(APDocType.ListAttribute))]
            [PXMergeAttributes(Method = MergeMethod.Merge)]
            [Attribute.Extension.ATPTAPDatDocType]
            [PXString]
            public string DocType { get; set; }
        }

        public delegate void OpenOtherEWTDocumentDelegate();
        [PXOverride]
        public void OpenOtherEWTDocument(OpenOtherEWTDocumentDelegate baseMethod)
        {
            if (this.Base.EWTPurchDocuments.Current != null)
            {
                if (this.Base.EWTPurchDocuments.Current.DocType == Attribute.Extension.ATPTAPDatDocType.ExpenseReceiptValue)
                {
                    PX.Objects.EP.ExpenseClaimDetailEntry graph = PXGraph.CreateInstance<PX.Objects.EP.ExpenseClaimDetailEntry>();
                    EPExpenseClaimDetails record = PXSelectReadonly<EPExpenseClaimDetails,
                        Where<EPExpenseClaimDetails.claimDetailCD, Equal<Required<EPExpenseClaimDetails.claimDetailCD>>>>
                        .Select(Base, this.Base.EWTPurchDocuments.Current.RefNbr);

                    graph.ClaimDetails.Current = record;
                    throw new PXRedirectRequiredException(graph, true, "Expense Receipt") { Mode = PXBaseRedirectException.WindowMode.NewWindow };
                }
                else
                {
                    baseMethod();
                }
            }
        }

        #region ANNUAL EWT
        protected virtual IEnumerable aEWTPurchDocuments()
        {
            ATPTAEWTPurchFilter filter = this.Base.Aewtpurchfilter.Current;
            List<ATPTAEWTPurchDATResult> result = this.Base.GetAEWTPurchaseDocuments(filter);
            List<CurrencyRate> curyRates = ATPTHelpers.GetRates(Base, filter.StartDate, filter.EndDate);

            //PXSelectBase<EPExpenseClaimDetails> query = new PXSelectReadonly2<EPExpenseClaimDetails,
            //    InnerJoin<EPTaxTran, On<EPTaxTran.claimDetailID, Equal<EPExpenseClaimDetails.claimDetailID>>,
            //    InnerJoin<Tax, On<Tax.taxID, Equal<EPTaxTran.taxID>, And<Tax.taxType, Equal<CSTaxType.withholding>>>,
            //    InnerJoin<CurrencyInfo, On<CurrencyInfo.curyInfoID, Equal<EPTaxTran.curyInfoID>>,
            //    InnerJoin<APInvoice, On<APInvoice.refNbr, Equal<EPExpenseClaimDetails.aPRefNbr>>>>>>,
            //    Where<EPExpenseClaimDetails.expenseDate, GreaterEqual<Required<EPExpenseClaimDetails.expenseDate>>,
            //    And<EPExpenseClaimDetails.expenseDate, LessEqual<Required<EPExpenseClaimDetails.expenseDate>>,
            //    And<EPExpenseClaimDetails.aPRefNbr, IsNotNull>>>>(Base);

            //foreach (PXResult<EPExpenseClaimDetails, EPTaxTran, Tax, CurrencyInfo, APInvoice> q in query.Select(filter.StartDate, filter.EndDate))
            //{
            //    EPExpenseClaimDetails er = q;
            //    EPTaxTran ertx = q;
            //    Tax tx = q;
            //    APInvoice inv = q;
            //    CurrencyInfo curyInfo = q;

            //    bool isNegative = (er.APDocType == APDocType.DebitAdj);

            //    //Remove Bills
            //    List<ATPTAEWTPurchDATResult> removeInvoices = result.Where(w => w.RefNbr == inv.RefNbr && w.DocType == inv.DocType).ToList();
            //    foreach (ATPTAEWTPurchDATResult r in removeInvoices) { result.Remove(r); }

            //    //Remove Checks
            //    APAdjust checkPayment = PXSelect<APAdjust, Where<APAdjust.adjdRefNbr, Equal<Required<APAdjust.adjdRefNbr>>>>.Select(Base, inv.RefNbr);

            //    if (checkPayment != null)
            //    {
            //        List<ATPTAEWTPurchDATResult> removeChecks = result.Where(w => w.RefNbr == checkPayment.AdjgRefNbr && w.DocType == checkPayment.AdjgDocType).ToList();
            //        foreach (ATPTAEWTPurchDATResult r in removeChecks) { result.Remove(r); }
            //    }

            //    ATPTAEWTPurchDATResult item = new ATPTAEWTPurchDATResult();

            //    item.DocType = Attribute.Extension.ATPTAPDatDocType.ExpenseReceiptValue;
            //    item.RefNbr = er.ClaimDetailCD;

            //    DateTime docdate = (DateTime)er.ExpenseDate;
            //    DateTime endOfMonth = new DateTime(docdate.Year, docdate.Month,
            //                  DateTime.DaysInMonth(docdate.Year, docdate.Month));
            //    item.TaxableMonth = docdate;

            //    ATPTEPExpenseClaimDetails ecExt = er.GetExtension<ATPTEPExpenseClaimDetails>();

            //    item.TaxRegistrationID = ecExt.UsrATPTVendTin;
            //    item.RegisteredName = ecExt.UsrATPTVendName;
            //    item.AddressLine1 = ecExt.UsrATPTAddress;
            //    item.AddressLine2 = string.Empty;

            //    item.LastName = string.Empty;
            //    item.FirstName = string.Empty;
            //    item.MidName = string.Empty;

            //    item.ATCCode = ertx.TaxID;
            //    item.NaturePayment = tx.Descr;

            //    decimal txbleamt = ertx.TaxableAmt ?? decimal.Zero;

            //    #region MULTI CURRENCY 
            //    bool isPhiltaxCurrency = !String.IsNullOrEmpty(this.Base.philTaxSetup.Current.PhilTaxCurrency) || !String.IsNullOrEmpty(this.Base.philTaxSetup.Current.PhilTaxCurrencyType);
            //    bool isMultiCurrency = curyInfo.BaseCuryID != curyInfo.CuryID;
            //    CurrencyRate curyRate = new CurrencyRate();

            //    if ((isPhiltaxCurrency) && (isMultiCurrency))
            //    {
            //        txbleamt = ertx.CuryTaxableAmt ?? decimal.Zero;
            //    }
            //    else if ((isPhiltaxCurrency) && !(isMultiCurrency))
            //    {
            //        curyRate = ATPTHelpers.GetSpecificRate(curyRates, item.TaxableMonth);

            //        if (curyRate.CuryRateID != null)
            //            PXDBCurrencyAttribute.CuryConvCury(this.Base.AEWTPurchDocuments.Cache, curyRate, (Math.Round((decimal)ertx.TaxableAmt, 2, MidpointRounding.AwayFromZero)), out txbleamt, true);
            //    }
            //    #endregion

            //    item.AmountIncomePayment = (isNegative == true ? (txbleamt) * -1 : (txbleamt));
            //    item.TaxRate = ertx.TaxRate ?? decimal.Zero;
            //    item.AmountWTax = (isNegative == true ? ((txbleamt) * ((ertx.TaxRate ?? decimal.Zero) / 100)) * -1 :
            //        (txbleamt) * ((ertx.TaxRate ?? decimal.Zero) / 100));

            //    if (!result.Any(a => a.ATCCode == item.ATCCode && a.RefNbr == item.RefNbr))
            //    {
            //        result.Add(item);
            //    }

            //}

            return result;
        }

        public sealed class ATPTAEWTPurchDATResultExt : PXCacheExtension<ATPTAEWTPurchDATResult>
        {
            public static bool IsActive() => true;

            [PXRemoveBaseAttribute(typeof(APDocType.ListAttribute))]
            [PXMergeAttributes(Method = MergeMethod.Merge)]
            [Attribute.Extension.ATPTAPDatDocType]
            [PXString]
            public string DocType { get; set; }
        }

        public delegate void OpenOtherAEWTDocumentDelegate();
        [PXOverride]
        public void OpenOtherAEWTDocument(OpenOtherAEWTDocumentDelegate baseMethod)
        {

            if (this.Base.AEWTPurchDocuments.Current != null)
            {
                if (this.Base.AEWTPurchDocuments.Current.DocType == Attribute.Extension.ATPTAPDatDocType.ExpenseReceiptValue)
                {
                    PX.Objects.EP.ExpenseClaimDetailEntry graph = PXGraph.CreateInstance<PX.Objects.EP.ExpenseClaimDetailEntry>();
                    EPExpenseClaimDetails record = PXSelectReadonly<EPExpenseClaimDetails,
                        Where<EPExpenseClaimDetails.claimDetailCD, Equal<Required<EPExpenseClaimDetails.claimDetailCD>>>>
                        .Select(Base, this.Base.AEWTPurchDocuments.Current.RefNbr);

                    graph.ClaimDetails.Current = record;
                    throw new PXRedirectRequiredException(graph, true, "Expense Receipt") { Mode = PXBaseRedirectException.WindowMode.NewWindow };
                }
                else
                {
                    baseMethod();
                }
            }
        }
        #endregion

    }
}