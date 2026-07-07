using CashFundManagement.Attributes;
using CashFundManagement.DAC;
using CashFundManagement.DAC.Setup;
using CashFundManagement.Extensions.DAC;
using CashFundManagement.Helper;
using CashFundManagement.Messages;
using PX.Data;
using PX.Objects.AP;
using PX.Objects.CR;
using PX.Objects.CS;
using PX.Objects.EP;
using PX.Objects.PM;
using System;
using System.Collections;
using System.Collections.Generic;

namespace CashFundManagement.BLC {
    public class ATPTEFMCAReclassProcess : PXGraph<ATPTEFMCAReclassProcess>
    {

        #region Views + ctor
        public PXSetup<APSetup> ApSetup;
        public PXSetup<ATPTEFMCASetup> Setup;
        public PXCancel<ATPTEFMCashAdvanceReclass> Cancel;
        public PXProcessing<
            ATPTEFMCashAdvanceReclass,
            Where<ATPTEFMCashAdvanceReclass.status, Equal<ATPTEFMCashAdvanceStatusAttribute.pendingLiquidationValue>,
                And<ATPTEFMCashAdvanceReclass.liqDate, Less<Current<AccessInfo.businessDate>>,
                And<ATPTEFMCashAdvanceReclass.reclassified, Equal<False>>>>,
            OrderBy<Asc<ATPTEFMCashAdvanceReclass.cashAdvanceNbr>>>
            Summary;
        public PXFilter<ATPTEFMCashAdvanceExtendFilter> ExtendFilter;

        public ATPTEFMCAReclassProcess()
        {
            // Block access when Cash Fund Management is disabled in Preferences (ATPT5016)
#if !Version23R2
            if (!(Setup?.Current?.EnableCFM ?? false))
            {
                throw new PXException(ATPTEFMMessages.CashFundManagementFeatureDisabled);
            }
#endif
            Summary.SetProcessCaption(ATPTEFMMessages.CashAdvanceReclass);
            Summary.SetProcessAllVisible(false);

            ATPTEFMCASetup setup = Setup.Current;
            ATPTEFMCAReclassProcess graph = this;
            Summary.SetProcessDelegate(delegate (List<ATPTEFMCashAdvanceReclass> list) { ProcessReclass(list, graph); });
        }
        #endregion

        #region Methods
        protected string GetInvoiceNbr() => IsRaiseErrorDuplicateVendorRef() ? $"{Summary.Current.CashAdvanceNbr}-{GetBillCount()}" : Summary.Current.CashAdvanceNbr;
        protected int GetBillCount() => PXSelectJoin<APInvoice, InnerJoin<APRegister, On<APRegister.refNbr, Equal<APInvoice.refNbr>,
                And<APRegister.docType, Equal<APInvoice.docType>>>>,
                Where<APRegister.origRefNbr, Equal<Required<APRegister.origRefNbr>>>>.Select(this, Summary.Current.CashAdvanceNbr).Count + 1;
        protected bool IsRaiseErrorDuplicateVendorRef() => ApSetup.Current.RaiseErrorOnDoubleInvoiceNbr ?? false;
        public virtual APInvoice DoAdditionalInvoiceProcess(APInvoice row) { return row; }

        public DateTime GetLiquidationDateWorkCalendar()
        {
            DateTime liquidationDate = new DateTime();
            ATPTEFMCashAdvance ca = Summary.Current;

            if (ca != null)
            {
                EPEmployee employee = PXSelect<
                    EPEmployee,
                    Where<EPEmployee.bAccountID, Equal<Required<EPEmployee.bAccountID>>>>
                    .Select(this, Summary.Current.RequestedByID);

                CSCalendar calendar = PXSelect<
                    CSCalendar,
                    Where<CSCalendar.calendarID,
                    Equal<Required<CSCalendar.calendarID>>>>
                    .Select(this, employee.CalendarID);

                bool isNonWorkDay = false;
                liquidationDate = ca.LiqDate.Value;
                int dayCounter = 0;
                int? liquidationDays = ExtendFilter.Current.Days;

                do
                {
                    liquidationDate = liquidationDate.AddDays(1);
                    isNonWorkDay = IsNonWorkDay(calendar, employee.CalendarID, liquidationDate);

                    if (isNonWorkDay)
                        continue;

                    dayCounter++;
                }
                while (dayCounter < liquidationDays);
            }

            return liquidationDate;
        }
        private bool IsNonWorkDay(CSCalendar calendar, string calendarId, DateTime date)
        {
            var isHoliday = CalendarHelper.IsHoliday(this, calendarId, date);
            return (date.DayOfWeek == DayOfWeek.Monday && (calendar.MonWorkDay == false || isHoliday))
                || (date.DayOfWeek == DayOfWeek.Tuesday && (calendar.TueWorkDay == false || isHoliday))
                || (date.DayOfWeek == DayOfWeek.Wednesday && (calendar.WedWorkDay == false || isHoliday))
                || (date.DayOfWeek == DayOfWeek.Thursday && (calendar.ThuWorkDay == false || isHoliday))
                || (date.DayOfWeek == DayOfWeek.Friday && (calendar.FriWorkDay == false || isHoliday))
                || (date.DayOfWeek == DayOfWeek.Saturday && (calendar.SatWorkDay == false || isHoliday))
                || (date.DayOfWeek == DayOfWeek.Sunday && (calendar.SunWorkDay == false || isHoliday));
        }
        #endregion

        #region Process
        public static void ProcessReclass(List<ATPTEFMCashAdvanceReclass> list, ATPTEFMCAReclassProcess graph)
        {
            using (PXTransactionScope ts = new PXTransactionScope())
            {
                ATPTEFMCASetup setup = graph.Setup.Current;
                foreach (ATPTEFMCashAdvanceReclass l in list)
                {
                    //AP Invoice
                    APInvoiceEntry aPInvoice = PXGraph.CreateInstance<APInvoiceEntry>();
                    aPInvoice.Clear();

                    APInvoice getRefNbr = PXSelect<APInvoice, Where<APInvoice.invoiceNbr, Equal<Required<APInvoice.invoiceNbr>>>>.Select(aPInvoice, l.CashAdvanceNbr);

                    APInvoice invoice = aPInvoice.Document.Insert(new APInvoice
                    {
                        DocType = APInvoiceType.CreditAdj,
                        VendorID = l.RequestedByID,
                        DocDate = graph.Accessinfo?.BusinessDate,
                        InvoiceNbr = graph.GetInvoiceNbr(),
                        DocDesc = ATPTEFMMessages.ReclassInvoiceNbr(l.CashAdvanceNbr),
                    });


                    ATPTEFMAPRegisterExt invExt = invoice.GetExtension<ATPTEFMAPRegisterExt>();
                    invExt.UsrATPTEFMSourceType = Attributes.ATPTEFMSourceTypeAttribute.CashAdvance;
                    invExt.UsrATPTEFMSourceRef = l.CashAdvanceNbr;

                    invoice = graph.DoAdditionalInvoiceProcess(invoice);

                    aPInvoice.Document.Update(invoice);


                    APTran transaction = aPInvoice.Transactions.Insert(new APTran
                    {
                        Qty = 1M,
                        CuryUnitCost = l.CuryChangeAmount ?? 0M,
                        CuryLineAmt = l.CuryChangeAmount ?? 0M,
                        TranDesc = ATPTEFMMessages.ReclassTranDesc,
                        AccountID = setup.ReClassAccoundID,
                        SubID = setup.ReClassSubID,
                        ProjectID = ProjectDefaultAttribute.NonProject()
                    });

                    aPInvoice.Transactions.Update(transaction);

                    if (graph.ApSetup.Current.RequireControlTotal ?? false)
                        invoice.CuryOrigDocAmt = l.CuryChangeAmount ?? 0M;

                    aPInvoice.Document.Update(invoice);
                    aPInvoice.Save.Press();

                    //Cash Advance
                    ATPTEFMCashAdvanceEntry caGraph = PXGraph.CreateInstance<ATPTEFMCashAdvanceEntry>();

                    l.Reclassified = true;
                    l.ReclassifiedInvoiceRefNbr = aPInvoice.Transactions.Current.RefNbr;

                    caGraph.CashAdvances.Current = l;
                    caGraph.CashAdvances.UpdateCurrent();

                    caGraph.Save.Press();
                }
                ts.Complete();
            }
        }
        #endregion

        #region Action
        public PXAction<ATPTEFMCashAdvanceReclass> Extend;
        #endregion

        #region Action Delegate
        [PXButton()]
        [PXUIField(DisplayName = ATPTEFMMessages.Extend)]
        public IEnumerable extend(PXAdapter adapter)
        {
            bool isAccepted = false;
            if (ExtendFilter.AskExt(true) == WebDialogResult.OK)
            {
                isAccepted = true;
                ExtendFilter.ClearDialog();
            }

            if (isAccepted)
            {
                ATPTEFMHelper.StartLongOperation(this, adapter, delegate ()
                {
                    using (PXTransactionScope ts = new PXTransactionScope())
                    {

                        ATPTEFMCashAdvanceEntry caGraph = PXGraph.CreateInstance<ATPTEFMCashAdvanceEntry>();

                        foreach (ATPTEFMCashAdvanceReclass row in Summary.Select())
                        {
                            if (row.Selected ?? false)
                            {
                                caGraph.CashAdvances.Current = PXSelect<ATPTEFMCashAdvance, Where<ATPTEFMCashAdvance.cashAdvanceNbr, Equal<Required<ATPTEFMCashAdvance.cashAdvanceNbr>>>>.Select(this, row.CashAdvanceNbr);
                                DateTime? extendDate = caGraph.CashAdvances.Current.LiqDate;
                                caGraph.CashAdvances.Current.DaysExtend = ExtendFilter.Current.Days;
                                caGraph.CashAdvances.Current.LiqDate = (Setup.Current.LiquidationDateBasedOnWorkCalendar ?? false) ? GetLiquidationDateWorkCalendar() : extendDate.Value.AddDays(ExtendFilter.Current.Days ?? 1);
                                caGraph.CashAdvances.UpdateCurrent();
                            }
                        }

                        if (caGraph.IsDirty)
                        {
                            caGraph.Save.Press();

                            Summary.Cache.Clear();
                            Summary.Cache.ClearQueryCache();
                        }
                        ts.Complete();
                    }
                });
            }
            return adapter.Get();
        }
        #endregion

        #region Internal Types
        [Serializable]
        [PXCacheName("Cash Advance Reclassify")]
        public class ATPTEFMCashAdvanceReclass : DAC.ATPTEFMCashAdvance, IPXSelectable
        {
            #region Selected
            [PXBool()]
            [PXDefault(false, PersistingCheck = PXPersistingCheck.Nothing)]
            [PXUIField(DisplayName = "")]
            public bool? Selected { get; set; }
            public abstract class selected : PX.Data.BQL.BqlBool.Field<selected> { }
            #endregion
        }

        [Serializable]
        [PXCacheName("Cash Advance Extend Filter")]
        public class ATPTEFMCashAdvanceExtendFilter : CashFundManagement.DAC.Base.ATPTEFMBase, IBqlTable
        {
            #region Days
            [PXInt]
            [PXUnboundDefault]
            public virtual int? Days { get; set; }
            public abstract class days : PX.Data.BQL.BqlInt.Field<days> { }
            #endregion
        }

        #endregion

    }
}