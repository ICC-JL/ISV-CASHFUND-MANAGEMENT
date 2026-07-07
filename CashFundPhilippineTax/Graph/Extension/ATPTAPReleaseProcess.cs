using CashFundManagement.Attributes;
using CashFundManagement.DAC;
using CashFundManagement.Extensions.DAC;
using CashFundManagement.Helper;
using PX.Data;
using PX.Objects.AP;
using PX.Objects.GL;
using PX.Objects.TX;
using System;
using System.Linq;

namespace CashFundPhilippineTax.Graph.Extension
{
    /// <remarks>
    /// 2024-08-14 : Adds multi-tenant support. {RRS} <br/>
    /// 2025-02-13:  Apply multi tenant to 24r1 CASE: 010269 {JLTG}
    /// </remarks>
    public class ATPTAPReleaseProcess : PXGraphExtension<PX.Objects.AP.APReleaseProcess>
    {
#if Version23R2
        public static bool IsActive() => ATPTPhilippineTax.Helpers.ATPTModule.IsActive; 
#else
        public static bool IsActive() => ATPTPhilippineTax.Helpers.ATPTModule.IsActive && ATPTEFMPrefetchSetup.IsActive;

#endif

        #region Methods
        public delegate void VerifyPaymentRoundAndCloseDelegate(JournalEntry je, APRegister paymentRegister, APPayment payment, Vendor paymentVendor, PX.Objects.CM.Extensions.CurrencyInfo new_info, PX.Objects.CM.Extensions.Currency paycury, Tuple<APAdjust, PX.Objects.CM.Extensions.CurrencyInfo> lastAdjustment);
        [PXOverride]
        public void VerifyPaymentRoundAndClose(JournalEntry je, APRegister paymentRegister, APPayment payment, Vendor paymentVendor, PX.Objects.CM.Extensions.CurrencyInfo new_info, PX.Objects.CM.Extensions.Currency paycury, Tuple<APAdjust, PX.Objects.CM.Extensions.CurrencyInfo> lastAdjustment, VerifyPaymentRoundAndCloseDelegate baseMethod)
        {
            APAdjust adj = lastAdjustment.Item1;
            APInvoice curInvoice = APInvoice.PK.Find(this.Base, adj.AdjdDocType, adj.AdjdRefNbr);
            ATPTEFMAPRegisterExt invExt = curInvoice.GetExtension<ATPTEFMAPRegisterExt>();

            //Releasing of Debit Adjustment Application with net of WHT
            if (Base.Accessinfo.ScreenID == "AP.30.20.00")
            {
                //if (paymentRegister.DocType == APDocType.DebitAdj &&
                //(!String.IsNullOrEmpty(invExt.UsrATPTEFMSourceRef) ||
                //!String.IsNullOrEmpty(curInvoice.OrigRefNbr) ||
                //paymentRegister.OrigModule == BatchModule.EP))
                if (paymentRegister.DocType == APDocType.DebitAdj && paymentRegister.CreatedByScreenID == "ATPT2012")
                {
                    PXResultset<APTaxTran, Tax> taxes = PXSelectJoin<APTaxTran,
                        InnerJoin<Tax, On<Tax.taxID, Equal<APTaxTran.taxID>,
                            And<Tax.taxType, Equal<CSTaxType.withholding>>>>,
                        Where<APTaxTran.tranType, Equal<Required<APInvoice.docType>>,
                            And<APTaxTran.refNbr, Equal<Required<APInvoice.refNbr>>,
                            And<APTaxTran.taxAmt, Greater<Zero>>>>>
                    .Select<PXResultset<APTaxTran, Tax>>(Base, APDocType.DebitAdj, paymentRegister.RefNbr);

                    if (taxes.Any())
                    {
                        decimal taxamt = taxes.Sum(x => ((APTaxTran)x.GetItem<APTaxTran>()).TaxAmt ?? 0m);
                        decimal curytaxamt = taxes.Sum(x => ((APTaxTran)x.GetItem<APTaxTran>()).CuryTaxAmt ?? 0m);

                        PX.Objects.CM.CurrencyInfo curyInfoInvoice = PXSelect<PX.Objects.CM.CurrencyInfo,
                             Where<PX.Objects.CM.CurrencyInfo.curyInfoID,
                             Equal<Required<PX.Objects.CM.CurrencyInfo.curyInfoID>>>>
                        .Select(Base, paymentRegister.CuryInfoID);

                        bool isFinalPayment = (paymentRegister.DocBal - paymentRegister.WhTaxBal) == adj.AdjAmt;
                        decimal? adjPercent = (adj.AdjAmt / (paymentRegister.OrigDocAmt - paymentRegister.OrigWhTaxAmt)) ?? decimal.Zero;

                        PX.Objects.CM.PXDBCurrencyAttribute.CuryConvCury(
                                this.Base.Caches[typeof(APPayment)],
                                curyInfoInvoice,
                                (decimal)paymentRegister.WhTaxBal,
                                out decimal whtConvAmt);

                        decimal? curyWhtAmt = (!(isFinalPayment) ?
                                              (Math.Round((decimal)(paymentRegister.CuryOrigWhTaxAmt * adjPercent), 2)) :
                                              (whtConvAmt));

                        decimal? whtAmt = (!(isFinalPayment) ?
                                          (Math.Round((decimal)(paymentRegister.OrigWhTaxAmt * adjPercent), 2)) :
                                          (paymentRegister.WhTaxBal));


                        paymentRegister.CuryDocBal -= curyWhtAmt;
                        paymentRegister.DocBal -= whtAmt;
                        paymentRegister.CuryWhTaxBal -= (taxamt > 0) ? curytaxamt : curyWhtAmt;
                        paymentRegister.WhTaxBal -= (taxamt > 0) ? taxamt : whtAmt;

                    }
                }
            }

            baseMethod(je, paymentRegister, payment, paymentVendor, new_info, paycury, lastAdjustment);
        }

        public delegate void UpdateBalancesDelegate(APAdjust adj, APRegister adjddoc, Vendor vendor, APTran adjdtran);
        [PXOverride]
        public virtual void UpdateBalances(APAdjust adj, APRegister adjddoc, Vendor vendor, APTran adjdtran, UpdateBalancesDelegate baseMethod)
        {
            ATPTEFMAPRegisterExt invExt = adjddoc.GetExtension<ATPTEFMAPRegisterExt>();

            //CFM SCENARIO: USING NET OF WHT FOR DEBIT ADJ APPLICATION
            //if (!String.IsNullOrEmpty(invExt.UsrATPTEFMSourceRef) ||
            //    !String.IsNullOrEmpty(adjddoc.OrigRefNbr) ||
            //    adjddoc.OrigModule == BatchModule.EP)

            //If bill is from replenishment
            ATPTEFMReplenishment replenishment = PXSelect<
                ATPTEFMReplenishment,
                Where<ATPTEFMReplenishment.replenishmentNbr, Equal<Required<ATPTEFMReplenishment.replenishmentNbr>>>>
                .Select(Base, adjddoc.OrigRefNbr);

            bool isApplicableForDebitWithNetAmtApplication = false;

            if (replenishment != null)
            {
                ATPTEFMFund fund = PXSelect<ATPTEFMFund, Where<ATPTEFMFund.fundCD, Equal<Required<ATPTEFMFund.fundCD>>,
                    And<ATPTEFMFund.status, Equal<ATPTEFMFundStatusAttribute.pendingCloseValue>>>>.Select(Base, replenishment.FundID);

                if (fund != null)
                    isApplicableForDebitWithNetAmtApplication = true;
            }


            if (adjddoc.CreatedByScreenID == "ATPT2012" || isApplicableForDebitWithNetAmtApplication)
            {
                PX.Objects.CM.CurrencyInfo curyInfoInvoice = PXSelect<PX.Objects.CM.CurrencyInfo,
                     Where<PX.Objects.CM.CurrencyInfo.curyInfoID,
                     Equal<Required<PX.Objects.CM.CurrencyInfo.curyInfoID>>>>
                .Select(Base, adjddoc.CuryInfoID);

                #region Debit Adj
                if (adj.AdjdDocType == APDocType.Invoice)
                {
                    if (adj.AdjgDocType == APDocType.DebitAdj)
                    {
                        APPayment curPayment = APPayment.PK.Find(this.Base, adj.AdjgDocType, adj.AdjgRefNbr);

                        bool isFinalPayment = (adjddoc.DocBal - adjddoc.WhTaxBal) == adj.AdjAmt;
                        bool isPmtMultiCur = this.Base.Accessinfo.BaseCuryID != curPayment.CuryID;
                        decimal? adjPercent = (adj.AdjAmt / (adjddoc.OrigDocAmt - adjddoc.OrigWhTaxAmt)) ?? decimal.Zero;

                        PX.Objects.CM.PXDBCurrencyAttribute.CuryConvCury(
                            this.Base.Caches[typeof(APPayment)],
                            curyInfoInvoice,
                            (decimal)adjddoc.WhTaxBal,
                            out decimal whtConvAmt);

                        decimal? curyWhtAmt = (!(isFinalPayment) ?
                                              (Math.Round((decimal)(adjddoc.CuryOrigWhTaxAmt * adjPercent), 2)) :
                                              (whtConvAmt));

                        decimal? whtAmt = (!(isFinalPayment) ?
                                          (Math.Round((decimal)(adjddoc.OrigWhTaxAmt * adjPercent), 2)) :
                                          (adjddoc.WhTaxBal));

                        adjddoc.CuryWhTaxBal -= curyWhtAmt;
                        adjddoc.WhTaxBal -= whtAmt;
                        adjddoc.CuryDocBal -= curyWhtAmt;
                        adjddoc.DocBal -= whtAmt;

                        adjddoc = (APRegister)this.Base.APDocument.Cache.Update(adjddoc);
                    }
                }
                #endregion
            }

            // baseMethod(adj, adjddoc, vendor, null);
            baseMethod?.Invoke(adj, adjddoc, vendor, null);
        }
        #endregion
    }
}
