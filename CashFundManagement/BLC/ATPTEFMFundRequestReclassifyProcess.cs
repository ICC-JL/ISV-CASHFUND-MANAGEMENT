using CashFundManagement.Attributes;
using CashFundManagement.DAC;
using CashFundManagement.DAC.Setup;
using CashFundManagement.Extensions.DAC;
using CashFundManagement.Helper;
using CashFundManagement.Messages;
using PX.Data;
using PX.Data.BQL;
using PX.Objects.CR;
using PX.Objects.CS;
using PX.Objects.EP;
using PX.Objects.IN;
using System;
using System.Collections;
using System.Collections.Generic;
using static CashFundManagement.BLC.ATPTEFMFundMaint;

namespace CashFundManagement.BLC
{
    public class ATPTEFMFundRequestReclassifyProcess : PXGraph<ATPTEFMFundRequestReclassifyProcess>
    {
        #region Views + CTOR
        public PXSetup<ATPTEFMCASetup> CASetupPreferences;
        public PXCancel<ATPTEFMFundRequestReclass> Cancel;
        public PXFilter<ATPTEFMFundRequestExtendFilter> ExtendFilter;
        /*public PXProcessing<
            ATPTEFMFundRequestReclass,
            Where<ATPTEFMFundRequestReclass.cashAdvanceStatus, Equal<ATPTEFMFundTransactionCashAdvanceStatusAttribute.forReclassificationValue>,
                And<ATPTEFMFundRequestReclass.liqDate, Less<Current<AccessInfo.businessDate>>,
                And<ATPTEFMFundRequestReclass.balance, Greater<decimal0>>>>,
            OrderBy<
                Desc<ATPTEFMFundRequestReclass.refNbr>>>
            Summary;*/

        public PXProcessing<
            ATPTEFMFundRequestReclass,
            Where<True, Equal<False>>,
            OrderBy<
                Desc<ATPTEFMFundRequestReclass.refNbr>>>
            Summary;

        public PXSetup<ATPTEFMSetup> FundSetup;

        [PXViewName("Fund")]
        public PXSelect<
            ATPTEFMFund,
            Where<ATPTEFMFund.fundCD, Equal<Required<ATPTEFMFundTransaction.fundID>>>>
            Fund;

        [PXViewName("Transaction History")]
        public PXSelect<
            ATPTEFMFundTransactionHistoryView,
            Where<ATPTEFMFundTransactionHistoryView.refNbr, Equal<Required<ATPTEFMFundTransaction.refNbr>>>>
            TransactionHistoryView;

        #region View Delegates
        public IEnumerable summary()
        {
            List<ATPTEFMFundRequestReclass> FundTransactionReclass = new List<ATPTEFMFundRequestReclass>();


            var forReclassificationFtStatus = PXSelect<
                ATPTEFMFundRequestReclass,
                Where<ATPTEFMFundRequestReclass.cashAdvanceStatus, Equal<ATPTEFMFundTransactionCashAdvanceStatusAttribute.forReclassificationValue>,
                    And<ATPTEFMFundRequestReclass.liqDate, Less<Current<AccessInfo.businessDate>>,
                    And<ATPTEFMFundRequestReclass.fundTransactionType, Equal<ATPTEFMFundTransactionTypeAttribute.cashAdvanceValue>>>>>
                .Select(this);

            foreach (ATPTEFMFundRequestReclass reclassificationFT in forReclassificationFtStatus)
            {
                FundTransactionReclass.Add(reclassificationFT);
            }

            var unliquidatedTransactions = PXSelect<
                ATPTEFMFundRequestReclass,
                Where<ATPTEFMFundRequestReclass.fundTransactionType, Equal<ATPTEFMFundTransactionTypeAttribute.cashAdvanceValue>,
                    And<ATPTEFMFundRequestReclass.cashAdvanceStatus, Equal<ATPTEFMFundTransactionCashAdvanceStatusAttribute.unliquidatedValue>,
                    And<ATPTEFMFundRequestReclass.liqDate, Less<Current<AccessInfo.businessDate>>>>>>
                .Select(this);

            foreach (ATPTEFMFundRequestReclass unliquidatedFT in unliquidatedTransactions)
            {
                ATPTEFMFundTransactionReceiptDetail ftReceipt = PXSelect<
                    ATPTEFMFundTransactionReceiptDetail,
                    Where<ATPTEFMFundTransactionReceiptDetail.fundTransactionRefNbr, Equal<Required<ATPTEFMFundTransactionReceiptDetail.fundTransactionRefNbr>>,
                        And<ATPTEFMFundTransactionReceiptDetail.expenseReceiptRefNbr, IsNotNull>>>
                    .Select(this, unliquidatedFT.RefNbr);

                if (ftReceipt != null)
                    continue;

                FundTransactionReclass.Add(unliquidatedFT);
            }

            return FundTransactionReclass;

        }
        #endregion

        public ATPTEFMFundRequestReclassifyProcess()
        {
            // Block access when Cash Fund Management is disabled in Preferences (ATPT5017)
#if !Version23R2
            if (!(CASetupPreferences?.Current?.EnableCFM ?? false))
            {
                throw new PXException(ATPTEFMMessages.CashFundManagementFeatureDisabled);
            }
#endif
            Summary.SetProcessCaption(ATPTEFMMessages.CashAdvanceReclass);
            Summary.SetProcessAllVisible(false);

            ATPTEFMFundRequestReclassifyProcess graph = this;
            Summary.SetProcessDelegate(delegate (List<ATPTEFMFundRequestReclass> list) { ProcessReclass(list, graph); });
        }
        #endregion


        /// <remarks>
        /// 2026-01-13 : (KING25R1 Staging) Fund transaction>Reclassify fund request: When reclassified, the system generated expense receipt should follow the fund branch. : 014854 : JLTG
        /// </remarks>
        #region Process
        public static void ProcessReclass(List<ATPTEFMFundRequestReclass> list, ATPTEFMFundRequestReclassifyProcess graph)
        {
            using (PXTransactionScope ts = new PXTransactionScope())
            {
                ATPTEFMSetup setup = graph.FundSetup.Current;
                InventoryItem item = InventoryItem.PK.Find(graph, setup.ReclassificationItem);
                decimal qty = 1;
                int counter = 1;

                foreach (ATPTEFMFundRequestReclass fundTransaction in list)
                {
                    #region Create Expense Receipt

                    ExpenseClaimDetailEntry entry = PXGraph.CreateInstance<ExpenseClaimDetailEntry>();
                    entry.Clear();

                    EPExpenseClaimDetails claim = new EPExpenseClaimDetails();
                    claim = (EPExpenseClaimDetails)entry.ClaimDetails.Insert(claim);
                    ATPTEFMEPExpenseClaimDetailsExt claimExt = claim.GetExtension<ATPTEFMEPExpenseClaimDetailsExt>();
                    graph.Fund.Current = graph.Fund.Select(fundTransaction.FundID);

                    claimExt.UsrATPTEFMTranType = ATPTEFMExpenseTypeAttribute.Replenishment;
                    claimExt.UsrATPTEFMFundType = fundTransaction.FundType;
                    claim.EmployeeID = fundTransaction.RequestedByID;
                    claimExt.UsrATPTEFMIsReclassifyDoc = true;

                    claim = entry.ClaimDetails.Update(claim);

                    claimExt.UsrATPTEFMRequestRefNbr = fundTransaction.RefNbr;
                    claim.ExpenseDate = graph.Accessinfo.BusinessDate;

                    entry.ClaimDetails.Cache.SetValueExt<EPExpenseClaimDetails.inventoryID>(claim, item.InventoryID);

                    claim.TranDesc = string.Format("Reclass {0}", fundTransaction.RefNbr);

                    entry.ClaimDetails.Cache.SetValueExt<EPExpenseClaimDetails.qty>(claim, qty);

                    decimal? reclassAmt = (fundTransaction.CashAdvanceStatus.Equals(ATPTEFMFundTransactionCashAdvanceStatusAttribute.ForReclassificationValue))
                        ? fundTransaction.ReclassifyBalanceAmt : fundTransaction.RequestedAmount;

                    entry.ClaimDetails.Cache.SetValueExt<EPExpenseClaimDetails.curyUnitCost>(claim, reclassAmt);

                    claim.BranchID = graph.Fund?.Current?.BranchID;
                    claim.UOM = item.BaseUnit;
                    claim.ExpenseRefNbr = fundTransaction.RefNbr;
                    claim.ExpenseAccountID = item.COGSAcctID;
                    claim.ExpenseSubID = item.COGSSubID;
                    claim = entry.ClaimDetails.Update(claim);

                    entry.ClaimDetails.Current.TaxCalcMode = item.TaxCalcMode;
                    claim.CuryID = graph.Fund.Current.CuryID;
                    claim.TaxCategoryID = null;

                    claim = graph.ClearATCCode(claim);

                    entry.ClaimDetails.Update(claim);

                    entry.Save.Press();

                    #endregion

                    #region Update Fund Transaction and Insert Receipt

                    ATPTEFMFundTransactionEntry ftGraph = PXGraph.CreateInstance<ATPTEFMFundTransactionEntry>();
                    ftGraph.Clear();

                    ftGraph.FundTransactions.Current = fundTransaction;

                    ATPTEFMFundTransactionReclassficationReceiptDetail ftReclassRec = PXSelect<
                        ATPTEFMFundTransactionReclassficationReceiptDetail,
                        Where<ATPTEFMFundTransactionReclassficationReceiptDetail.fundTransactionRefNbr, Equal<Required<ATPTEFMFundTransactionReclassficationReceiptDetail.fundTransactionRefNbr>>>>
                        .Select(graph, fundTransaction.RefNbr);

                    if (ftReclassRec != null && (ftReclassRec.ExpenseReceiptRefNbr == null || ftReclassRec.ExpenseReceiptRefNbr == string.Empty))
                    {
                        ftReclassRec.ExpenseReceiptRefNbr = claim.ClaimDetailCD;
                        ftReclassRec.InventoryID = item.InventoryID;
                        ftReclassRec.RefNbr = fundTransaction.RefNbr;
                        ftReclassRec.TaxZoneID = claim.TaxZoneID;
                        ftReclassRec.NetQty = claim.Qty;
                        ftReclassRec.NetUnitCost = claim.CuryUnitCost;
                        ftGraph.FundTransactionReclassficationReceiptDetail.Update(ftReclassRec);
                        ftReclassRec.TaxCategoryID = null;
                        ftGraph.FundTransactionReclassficationReceiptDetail.Update(ftReclassRec);

                        fundTransaction.ReclassificationAmt = claim.CuryExtCost;
                        fundTransaction.Status = ATPTEFMFundStatusAttribute.ClosedValue;
                        fundTransaction.CashAdvanceStatus = ATPTEFMFundTransactionCashAdvanceStatusAttribute.LiquidatedValue;
                        fundTransaction.Step = ATPTEFMFundTransactionStepAttribute.LiquidatedReceiptValue;

                        ftGraph.FundTransactions.Update(fundTransaction);
                        ftGraph.Save.Press();
                    }
                    else
                    {
                        ATPTEFMFundTransactionReclassficationReceiptDetail ftReclassReceipt = new ATPTEFMFundTransactionReclassficationReceiptDetail();
                        ftReclassReceipt = ftGraph.FundTransactionReclassficationReceiptDetail.Insert(ftReclassReceipt);

                        ftReclassReceipt.ExpenseReceiptRefNbr = claim.ClaimDetailCD;
                        ftReclassReceipt.InventoryID = item.InventoryID;
                        ftReclassReceipt.RefNbr = fundTransaction.RefNbr;
                        ftReclassReceipt.TaxZoneID = claim.TaxZoneID;
                        ftReclassReceipt.NetQty = claim.Qty;
                        ftReclassReceipt.NetUnitCost = claim.CuryUnitCost;
                        ftGraph.FundTransactionReclassficationReceiptDetail.Update(ftReclassReceipt);
                        ftReclassReceipt.TaxCategoryID = null;
                        ftGraph.FundTransactionReclassficationReceiptDetail.Update(ftReclassReceipt);

                        fundTransaction.ReclassificationAmt = claim.CuryExtCost;
                        fundTransaction.Status = ATPTEFMFundStatusAttribute.ClosedValue;
                        fundTransaction.CashAdvanceStatus = ATPTEFMFundTransactionCashAdvanceStatusAttribute.LiquidatedValue;
                        fundTransaction.Step = ATPTEFMFundTransactionStepAttribute.LiquidatedReceiptValue;

                        ftGraph.FundTransactions.Update(fundTransaction);
                        ftGraph.Save.Press();
                    }
                    #endregion

                    #region Get Fund Transaction History Balance Amount
                    decimal? balanceAmt = decimal.Zero;
                    graph.TransactionHistoryView.Current = graph.TransactionHistoryView.Select(fundTransaction.RefNbr);
                    if (graph.TransactionHistoryView.Current != null)
                    {
                        balanceAmt = graph.TransactionHistoryView.Current.CuryBalanceAmt;

                        graph.TransactionHistoryView.Current.Status = ATPTEFMFundStatusAttribute.ClosedValue;
                        graph.TransactionHistoryView.Current.CuryUnliquidatedAmt = decimal.Zero;
                        graph.TransactionHistoryView.UpdateCurrent();
                        graph.TransactionHistoryView.Cache.Persist(PXDBOperation.Update);
                    }
                    #endregion

                    #region  Insert Fund Transaction Receipt History
                    ATPTEFMFundTransactionHistoryView transactionHistory = graph.TransactionHistoryView.Insert();
                    transactionHistory.FundRefNbr = fundTransaction.FundID;
                    transactionHistory.TransactionType = ATPTEFMTransactionHistoryView.transactionType.Reclassificaation;
                    transactionHistory.RefNbr = claim.ClaimDetailCD;
                    transactionHistory.OrderDate = claim.ExpenseDate;
                    transactionHistory.FundBranchID = claim.BranchID;
                    transactionHistory.FundType = claimExt.UsrATPTEFMFundType;
                    transactionHistory.TransactionDate = claim.ExpenseDate;
                    transactionHistory.CuryFundTransactionDocumentAmt = claim.CuryExtCost;
                    transactionHistory.Status = ATPTEFMFundStatusAttribute.LiquidatedValue;
                    transactionHistory.CuryLiquidatedAmt = claim.CuryExtCost.GetValueOrDefault();
                    transactionHistory.CuryFundReturnAmt = decimal.Zero;
                    transactionHistory.ProjectID = claim.ContractID;
                    transactionHistory.ProjectTaskID = claim.TaskID;
                    transactionHistory.CostCodeID = claim.CostCodeID;
                    transactionHistory.CuryBalanceAmt = balanceAmt;
                    //transactionHistory.SortNbr = $"{fundTransaction.RefNbr}-{ftGraph.FundTransactionReceiptLines.Select().Count + 1}";
                    transactionHistory.SortNbr = $"FT-{fundTransaction.RefNbr}-{ftGraph.FundTransactionReceiptLines.Select().Count + 1}";
                    transactionHistory.Source = ATPTEFMTransactionHistoryView.source.ExpenseReceipt;
                    transactionHistory.FundTransactionSortNbr = $"FT-{fundTransaction.RefNbr}";
                    graph.TransactionHistoryView.Update(transactionHistory);
                    graph.TransactionHistoryView.Cache.Persist(PXDBOperation.Insert);
                    #endregion

                    #region Update Fund Summary Balances
                    if (graph.Fund.Current != null)
                    {
                        graph.Fund.Current.CuryLiquidatedAmt += claim.CuryExtCost.GetValueOrDefault();
                        graph.Fund.Current.CuryUnliquidatedAmt -= claim.CuryExtCost.GetValueOrDefault();
                        graph.Fund.UpdateCurrent();

                        if (counter == list.Count)
                        {
                            graph.Fund.Cache.Persist(PXDBOperation.Update);
                        }
                    }
                    #endregion
                    counter++;
                }
                ts.Complete();
            }
        }
        #endregion

        #region Action
        public PXAction<ATPTEFMFundRequestReclass> Extend;
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

                        ATPTEFMFundTransactionEntry ftGraph = PXGraph.CreateInstance<ATPTEFMFundTransactionEntry>();

                        foreach (ATPTEFMFundRequestReclass row in Summary.Select())
                        {
                            if (row.Selected ?? false)
                            {
                                ftGraph.FundTransactions.Current = PXSelect<
                                    ATPTEFMFundTransaction,
                                    Where<ATPTEFMFundTransaction.refNbr, Equal<Required<ATPTEFMFundTransaction.refNbr>>>>
                                    .Select(this, row.RefNbr);
                                DateTime? extendDate = ftGraph.FundTransactions.Current.LiqDate;
                                ftGraph.FundTransactions.Current.DaysExtend = ExtendFilter.Current.Days;
                                ftGraph.FundTransactions.Current.LiqDate = (FundSetup.Current.LiquidationDateBasedOnWorkCalendar ?? false) ? GetLiquidationDateWorkCalendar() : extendDate.Value.AddDays(ExtendFilter.Current.Days ?? 1);
                                ftGraph.FundTransactions.UpdateCurrent();
                            }
                        }

                        if (ftGraph.IsDirty)
                        {
                            ftGraph.Save.Press();

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

        #region Methods
        public virtual EPExpenseClaimDetails ClearATCCode(EPExpenseClaimDetails er)
        {
            return er;
            //See Connector
        }

        public DateTime GetLiquidationDateWorkCalendar()
        {
            DateTime liquidationDate = new DateTime();
            ATPTEFMFundTransaction fundTran = Summary.Current;

            if (fundTran != null)
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
                liquidationDate = fundTran.LiqDate.Value;
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

        #region Internal Types
        [Serializable]
        [PXCacheName("Fund Request Reclassify")]
        public class ATPTEFMFundRequestReclass : DAC.ATPTEFMFundTransaction, IPXSelectable
        {
            #region Selected
            [PXBool()]
            [PXDefault(false, PersistingCheck = PXPersistingCheck.Nothing)]
            [PXUIField(DisplayName = "")]
            public bool? Selected { get; set; }
            public abstract class selected : PX.Data.BQL.BqlBool.Field<selected> { }
            #endregion

            #region ReclassifyBalanceAmt
            [PXDecimal(2)]
            [PXUIField(DisplayName = "Amount", Enabled = false)]
            [PXUnboundDefault(TypeCode.Decimal, "0.0", PersistingCheck = PXPersistingCheck.Nothing)]
            /*[PXFormula(typeof(Add<Sub<changeAmount, amountReceived>, amountReleased>))]
            [PXFormula(typeof(Default<changeAmount, amountReceived, amountReleased>))]*/
            [PXFormula(typeof(Switch<Case<
                Where<cashAdvanceStatus, Equal<ATPTEFMFundTransactionCashAdvanceStatusAttribute.unliquidatedValue>>, requestedAmount>,
                Add<Sub<changeAmount, amountReceived>, amountReleased>>))]
            [PXFormula(typeof(Default<requestedAmount, changeAmount, amountReceived, amountReleased>))]
            public virtual decimal? ReclassifyBalanceAmt { get; set; }
            public abstract class reclassifyBalanceAmt : BqlDecimal.Field<reclassifyBalanceAmt> { }
            #endregion

        }

        [Serializable]
        [PXCacheName("Fund Request Extend Filter")]
        public class ATPTEFMFundRequestExtendFilter : CashFundManagement.DAC.Base.ATPTEFMBase, IBqlTable
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
