using CashFundManagement.Attributes;
using CashFundManagement.BLC;
using CashFundManagement.DAC.Setup;
using CashFundManagement.Messages;
using PX.Data;
using PX.Data.BQL;
using PX.Data.EP;
using PX.Data.ReferentialIntegrity.Attributes;
using PX.Objects.CM;
using PX.Objects.CS;
using PX.Objects.EP;
using PX.Objects.GL;
using PX.TM;
using System;
//TODO : Remove dead codes on next upgrade.
namespace CashFundManagement.DAC {
    /// <remarks>
    /// 010199 - CFM 2024R1 - Fund Transaction Screen [Fund ID Field]
    /// </remarks>
    [PXPrimaryGraph(typeof(ATPTEFMFundTransactionEntry))]
    [Serializable]
    [PXCacheName(Messages.ATPTEFMMessages.ATPTEFMFundTransaction)]
    public class ATPTEFMFundTransaction : Base.ATPTEFMAudit, IBqlTable, IAssign
    {
        #region Keys
        public class PK : PrimaryKeyOf<ATPTEFMFundTransaction>.By<fundType, refNbr>
        {
            public static ATPTEFMFundTransaction Find(PXGraph graph, string fundType, string refNbr) => FindBy(graph, fundType, refNbr);
        }

        public static class FK
        {
            public class Fund : Field<fundID>.IsRelatedTo<ATPTEFMFund.fundCD>.AsSimpleKey.WithTablesOf<ATPTEFMFund, ATPTEFMFundTransaction> { }
            public class Employee : Field<requestedByID>.IsRelatedTo<EPEmployee.bAccountID>.AsSimpleKey.WithTablesOf<EPEmployee, ATPTEFMFundTransaction> { }
            public class Department : Field<departmentID>.IsRelatedTo<EPDepartment.departmentID>.AsSimpleKey.WithTablesOf<EPDepartment, ATPTEFMFundTransaction> { }
        }
        #endregion

        #region Audit
        public new abstract class createdByID : BqlGuid.Field<createdByID> { }
        public new abstract class createdByScreenID : BqlString.Field<createdByScreenID> { }
        public new abstract class createdDateTime : BqlDateTime.Field<createdDateTime> { }
        public new abstract class lastModifiedByID : BqlGuid.Field<lastModifiedByID> { }
        public new abstract class lastModifiedByScreenID : BqlString.Field<lastModifiedByScreenID> { }
        public new abstract class lastModifiedDateTime : BqlDateTime.Field<lastModifiedDateTime> { }
        public new abstract class Tstamp : BqlByteArray.Field<Tstamp> { }
        #endregion

        #region FundType
        [PXDBString(1, IsFixed = true, IsKey = true)]
        [ATPTEFMFundTypeAttribute.ATPTEFMFundType]
        [PXDefault(ATPTEFMFundTypeAttribute.PettyCashValue)]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.FundType)]
        public virtual string FundType { get; set; }
        public abstract class fundType : BqlString.Field<fundType> { }
        #endregion

        #region FundTransactionType
        [PXDBString(IsFixed = true)]
        [ATPTEFMFundTransactionTypeAttribute.ATPTEFMFundTransactionType]
        [PXDefault(ATPTEFMFundTransactionTypeAttribute.CashAdvanceValue)]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.FundTransactionType)]
        [PXUIEnabled(typeof(
            Where<IsNull<Selector<fundID, ATPTEFMFund.closed>, False>, Equal<False>>))]
        //[PXFormula(typeof(Default<fundID>))]
        public virtual string FundTransactionType { get; set; }
        public abstract class fundTransactionType : BqlString.Field<fundTransactionType> { }
        #endregion

        #region RefNbr                
        [PXDBString(15, IsKey = true, InputMask = ">CCCCCCCCCCCCCCC", IsUnicode = true)]
        [PXDefault()]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.RefNbr)]
        [PXSelector(typeof(refNbr),
            typeof(branchID),
            typeof(date),
            typeof(refNbr),
            typeof(fundID),
            typeof(descr),
            typeof(requestedByID),
            typeof(departmentID),
            typeof(requestedAmount))]
        [AutoNumber(typeof(ATPTEFMSetup.fundTransactionNumberingID), typeof(AccessInfo.businessDate))]
        public virtual string RefNbr { get; set; }
        public abstract class refNbr : BqlString.Field<refNbr> { }
        #endregion

        #region Status
        [PXDBString(IsFixed = true)]
        [ATPTEFMFundStatusAttribute.ATPTEFMFundTransactionStatus]
        [PXDefault(ATPTEFMFundStatusAttribute.HoldValue)]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.Status, Enabled = false)]
        public virtual string Status { get; set; }
        public abstract class status : BqlString.Field<status> { }
        #endregion

        #region Hold
        [PXDBBool()]
        [PXDefault(true)]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.Hold)]
        [PXUIEnabled(typeof(
            Where<isReleasedCash,Equal<False>>))]
        [PXNoUpdate]
        public virtual bool? Hold { get; set; }
        public abstract class hold : BqlBool.Field<hold> { }
        #endregion

        #region Date
        [PXDBDate()]
        [PXDefault(typeof(AccessInfo.businessDate))]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.Date, Visibility = PXUIVisibility.SelectorVisible)]
        public virtual DateTime? Date { get; set; }
        public abstract class date : BqlDateTime.Field<date> { }
        #endregion

        #region FinPeriodID
        [FinPeriodID(typeof(date), typeof(branchID))]
        [PXDBDefault()]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.FinPeriodID, Visibility = PXUIVisibility.SelectorVisible)]
        public virtual string FinPeriodID { get; set; }
        public abstract class finPeriodID : BqlString.Field<finPeriodID> { }
        #endregion

        #region Descr
        [PXDBString(255, IsUnicode = true)]
        [PXDefault()]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.Descr)]
        public virtual string Descr { get; set; }
        public abstract class descr : BqlString.Field<descr> { }
        #endregion

        #region RequestedByID
        [PXDBInt]
        [PXDefault(typeof(Search<
            EPEmployee.bAccountID,
            Where<EPEmployee.userID, Equal<Current<AccessInfo.userID>>>>))]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.RequestedByID)]
        [PXSelector(typeof(Search<EPEmployee.bAccountID>), typeof(EPEmployee.bAccountID), typeof(EPEmployee.acctCD), typeof(EPEmployee.acctName), typeof(EPEmployee.departmentID), DescriptionField = typeof(EPEmployee.acctName), SubstituteKey = typeof(EPEmployee.acctCD))]
        [PXRestrictor(typeof(
            Where<EPEmployee.userID, IsNotNull>), Messages.ATPTEFMMessages.EmployeeNotUser)]
        public virtual int? RequestedByID { get; set; }
        public abstract class requestedByID : BqlInt.Field<requestedByID> { }
        #endregion

        #region DepartmentID
        [PXDBString(10, IsUnicode = true)]
        [PXDefault(typeof(Search<EPEmployee.departmentID, Where<EPEmployee.bAccountID, Equal<Current<ATPTEFMFundTransaction.requestedByID>>>>))]
        [PXSelector(typeof(EPDepartment.departmentID), DescriptionField = typeof(EPDepartment.description))]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.DepartmentID)]
        public virtual string DepartmentID { get; set; }
        public abstract class departmentID : BqlString.Field<departmentID> { }
        #endregion

        #region PositionID
        [PXString(10,IsUnicode = true)]
        [PXUnboundDefault(typeof(Search<EPEmployeePosition.positionID, Where<EPEmployeePosition.employeeID, Equal<Current<requestedByID>>,
            And<EPEmployeePosition.isActive, Equal<True>>>>))]
        [PXFormula(typeof(Default<requestedByID>))]
        [PXUIField(DisplayName = ATPTEFMMessages.Positions, Visibility = PXUIVisibility.SelectorVisible)]
        [PXSelector(typeof(Search<EPPosition.positionID>), typeof(EPPosition.positionID), typeof(EPPosition.description), DescriptionField = typeof(EPPosition.positionID))]
        public virtual string PositionID { get; set; }
        public abstract class positionID : PX.Data.BQL.BqlString.Field<positionID> { }
        #endregion

        #region FundID
        [PXDBString(15, IsUnicode = true)]
        [PXDefault()]
        [PXUIRequired(typeof(
            Where<noFund, Equal<False>>))]
        //[ATPTEFMFundStatusAttribute.ATPTEFMFundStatus()]
        [PXUIEnabled(typeof(
            Where<noFund, Equal<False>>))]
        [PXSelector(typeof(Search2<
            ATPTEFMFund.fundCD,
            LeftJoin<EPEmployee, 
                On<EPEmployee.bAccountID, Equal<ATPTEFMFund.custodianID>>>,
            Where<ATPTEFMFund.fundType, Equal<Current<fundType>>,
                And<ATPTEFMFund.status, Equal<ATPTEFMFundStatusAttribute.activeValue>,
                And<ATPTEFMFund.isActive, Equal<boolTrue>>>>,
            OrderBy<
                Desc<ATPTEFMFund.fundCD>>>),
                    typeof(ATPTEFMFund.fundCD),
                    typeof(EPEmployee.acctName),
                    SubstituteKey = typeof(ATPTEFMFund.fundCD),
                    DescriptionField = typeof(ATPTEFMFund.descr))]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.FundID)]
        public virtual string FundID { get; set; }
        public abstract class fundID : BqlString.Field<fundID> { }
        #endregion

        #region CashAdvanceStatus
        [PXDBString(IsFixed = true)]
        [ATPTEFMFundTransactionCashAdvanceStatusAttribute.ATPTEFMFundTrandactionCashAdvanceStatus]
        [PXDefault(typeof(Switch<Case<
            Where<Current<ATPTEFMFundTransaction.fundTransactionType>, Equal<ATPTEFMFundTransactionTypeAttribute.cashAdvanceValue>>, ATPTEFMFundTransactionCashAdvanceStatusAttribute.unreleasedValue>, ATPTEFMFundTransactionCashAdvanceStatusAttribute.unliquidatedValue>))]
        [PXFormula(typeof(Default<ATPTEFMFundTransaction.fundTransactionType>))]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.CashAdvanceStatus, Enabled = false)]
        public virtual string CashAdvanceStatus { get; set; }
        public abstract class cashAdvanceStatus : BqlString.Field<cashAdvanceStatus> { }
        #endregion

        #region CurrentDate
        [PXDate()]
        [PXUnboundDefault(typeof(Current<AccessInfo.businessDate>))]
        public virtual DateTime? CurrentDate { get; set; }
        public abstract class currentDate : PX.Data.BQL.BqlDateTime.Field<currentDate> { }
        #endregion

        #region DateOfUse
        [PXDBDate()]
        [PXDefault(PersistingCheck = PXPersistingCheck.Nothing)]
        [PXUIField(DisplayName = ATPTEFMMessages.DateOfUse, Visibility = PXUIVisibility.SelectorVisible)]
        [PXVerifyEndDate(typeof(currentDate), AllowAutoChange = false)]
        public virtual DateTime? DateOfUse { get; set; }
        public abstract class dateOfUse : PX.Data.BQL.BqlDateTime.Field<dateOfUse> { }
        #endregion

        #region InitialLiqDate
        [PXDBDate()]
        [PXDefault(PersistingCheck = PXPersistingCheck.Nothing)]
        [PXUIField(DisplayName = ATPTEFMMessages.InitialLiquidationDate, Visibility = PXUIVisibility.SelectorVisible)]
        [PXUIRequired(typeof(Where<fundTransactionType, Equal<ATPTEFMFundTransactionTypeAttribute.cashAdvanceValue>>))]
        [PXUIVisible(typeof(Where<fundTransactionType, Equal<ATPTEFMFundTransactionTypeAttribute.cashAdvanceValue>>))]
        public virtual DateTime? InitialLiqDate { get; set; }
        public abstract class initialLiqDate : PX.Data.BQL.BqlDateTime.Field<initialLiqDate> { }
        #endregion

        #region LiqDate
        [PXDBDate()]
        [PXDefault(PersistingCheck = PXPersistingCheck.Nothing)]
        [PXUIField(DisplayName = ATPTEFMMessages.LiquidationDate, Visibility = PXUIVisibility.SelectorVisible)]
        [PXUIRequired(typeof(Where<fundTransactionType, Equal<ATPTEFMFundTransactionTypeAttribute.cashAdvanceValue>>))]
        public virtual DateTime? LiqDate { get; set; }
        public abstract class liqDate : PX.Data.BQL.BqlDateTime.Field<liqDate> { }
        #endregion

        #region UnmodifiedLiqDate
        [PXDBDate()]
        [PXDefault(PersistingCheck = PXPersistingCheck.Nothing)]
        public virtual DateTime? UnmodifiedLiqDate { get; set; }
        public abstract class unmodifiedLiqDate : PX.Data.BQL.BqlDateTime.Field<unmodifiedLiqDate> { }
        #endregion

        #region RequestedAmount
        [PXDBDecimal(2)]
        [PXDefault(TypeCode.Decimal, "0.0", PersistingCheck = PXPersistingCheck.Null)]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.RequestedAmount, Enabled = false)]
        public virtual decimal? RequestedAmount { get; set; }
        public abstract class requestedAmount : BqlDecimal.Field<requestedAmount> { }
        #endregion

        #region ActualSpentAmount
        [PXDBDecimal(2)]
        [PXDefault(TypeCode.Decimal, "0.0", PersistingCheck = PXPersistingCheck.Null)]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.ActualSpentAmount, Enabled = false)]
        public virtual decimal? ActualSpentAmount { get; set; }
        public abstract class actualSpentAmount : BqlDecimal.Field<actualSpentAmount> { }
        #endregion

        #region FundReturn
        [PXDBDecimal(2)]
        [PXDefault(TypeCode.Decimal, "0.0", PersistingCheck = PXPersistingCheck.Nothing)]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.FundReturn, Enabled = false)]
        public virtual decimal? ChangeAmount { get; set; }
        public abstract class changeAmount : BqlDecimal.Field<changeAmount> { }
        #endregion

        #region AmountReceived
        [PXDBDecimal(2)]
        [PXDefault(TypeCode.Decimal, "0.0", PersistingCheck = PXPersistingCheck.Nothing)]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.AmountReceived)]
        //[PXUIVisible(typeof(
        //    Where<fundTransactionType, Equal<ATPTEFMFundTransactionTypeAttribute.cashAdvanceValue>, 
        //        And<Current<ATPTEFMSetup.validateAmountReceivedAndReleasedUponLiquidation>, Equal<True>>>))]
        [PXUIEnabled(typeof(
            Where<step, Equal<ATPTEFMFundTransactionStepAttribute.submitReceiptValue>, 
                And<Current<changeAmount>, Greater<decimal0>>>))]
        //[PXUIVerify(typeof(
        //    Where<Current<amountReceived>, Greater<Current<changeAmount>>, And<status, Equal<ATPTEFMFundStatusAttribute.openValue>>>), PXErrorLevel.Error, Messages.ATPTEFMMessages.AmountReceivedCannotBeGreaterThanFundReturn)]
        public virtual decimal? AmountReceived { get; set; }
        public abstract class amountReceived : BqlDecimal.Field<amountReceived> { }
        #endregion

        #region AmountReleased
        [PXDBDecimal(2)]
        [PXDefault(TypeCode.Decimal, "0.0", PersistingCheck = PXPersistingCheck.Nothing)]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.AmountReleased)]
        //[PXUIVisible(typeof(
        //    Where<fundTransactionType, Equal<ATPTEFMFundTransactionTypeAttribute.cashAdvanceValue>,
        //        And<Current<ATPTEFMSetup.validateAmountReceivedAndReleasedUponLiquidation>, Equal<True>>>))]
        [PXUIEnabled(typeof(
            Where<step, Equal<ATPTEFMFundTransactionStepAttribute.submitReceiptValue>,
                And<Current<changeAmount>, Less<decimal0>>>))]
        //[PXUIVerify(typeof(
        //    Where<Sub<Current<changeAmount>, Current<amountReleased>>, Greater<decimal0>, And<status, Equal<ATPTEFMFundStatusAttribute.openValue>>>), PXErrorLevel.Error, Messages.ATPTEFMMessages.AmountReleasedCannotBeGreaterThanFundReturn)]
        public virtual decimal? AmountReleased { get; set; }
        public abstract class amountReleased : BqlDecimal.Field<amountReleased> { }
        #endregion

        #region ReclassificationAmt
        [PXDBDecimal(2)]
        [PXDefault(TypeCode.Decimal, "0.0", PersistingCheck = PXPersistingCheck.Nothing)]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.ReclassificationAmt, Enabled = false)]
        public virtual decimal? ReclassificationAmt { get; set; }
        public abstract class reclassificationAmt : BqlDecimal.Field<reclassificationAmt> { }
        #endregion

        #region TotalWhtAmount
        [PXDBDecimal(2)]
        [PXDefault(TypeCode.Decimal, "0.0", PersistingCheck = PXPersistingCheck.Nothing)]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.WithholdingTax, Enabled = false)]
        public virtual decimal? TotalWhtAmount { get; set; }
        public abstract class totalWhtAmount : BqlDecimal.Field<totalWhtAmount> { }
        #endregion

        #region ReceivedAmount
        [PXDBDecimal(2)]
        [PXDefault(TypeCode.Decimal, "0.0", PersistingCheck = PXPersistingCheck.Null)]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.ReceivedAmount)]
        public virtual decimal? ReceivedAmount { get; set; }
        public abstract class receivedAmount : BqlDecimal.Field<receivedAmount> { }
        #endregion

        #region ReleasedAmount
        [PXDBDecimal(2)]
        [PXDefault(TypeCode.Decimal, "0.0", PersistingCheck = PXPersistingCheck.Null)]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.ReleasedAmount)]
        public virtual decimal? ReleasedAmount { get; set; }
        public abstract class releasedAmount : BqlDecimal.Field<releasedAmount> { }
        #endregion

        #region InvoiceRefNbr
        [PXDBString(15, IsUnicode = true)]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.InvoiceRefNbr)]
        public virtual string InvoiceRefNbr { get; set; }
        public abstract class invoiceRefNbr : BqlString.Field<invoiceRefNbr> { }
        #endregion

        #region Step
        [PXDBString(IsFixed = true)]
        [ATPTEFMFundTransactionStepAttribute.ATPTEFMFundTransactionStep]
        [PXDefault(ATPTEFMFundTransactionStepAttribute.DefaultValue)]
        public virtual string Step { get; set; }
        public abstract class step : BqlString.Field<step> { }
        #endregion

        #region BranchID
        [Branch(typeof(AccessInfo.branchID), DisplayName = Messages.ATPTEFMMessages.Branch, Required = true)]
        public virtual int? BranchID { get; set; }
        public abstract class branchID : BqlInt.Field<branchID> { }
        #endregion

        #region OwnerID
        [Owner(typeof(workgroupID), DisplayName = Messages.ATPTEFMMessages.OwnerID)]
        [PXDefault(typeof(Search<
            EPEmployee.defContactID,
            Where<EPEmployee.userID, Equal<Current<AccessInfo.userID>>>>), PersistingCheck = PXPersistingCheck.Nothing)]
        //[PXUIField(DisplayName = Messages.ATPTEFMMessages.OwnerID)]
        public virtual int? OwnerID { get; set; }
        public abstract class ownerID : BqlInt.Field<ownerID> { }
        #endregion

        #region WorkgroupID
        [PXDBInt]
        [PXDefault(typeof(EPCompanyTree.workGroupID), PersistingCheck = PXPersistingCheck.Nothing)]
        [PX.TM.PXCompanyTreeSelector]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.WorkgroupID, Enabled = false)]
        public virtual int? WorkgroupID { get; set; }
        public abstract class workgroupID : BqlInt.Field<workgroupID> { }
        #endregion

        #region Approved
        [PXDBBool()]
        [PXDefault(false, PersistingCheck = PXPersistingCheck.Nothing)]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.Approved, Visibility = PXUIVisibility.Visible, Enabled = false)]
        public virtual bool? Approved { get; set; }
        public abstract class approved : BqlBool.Field<approved> { }
        #endregion

        #region Rejected
        public abstract class rejected : BqlBool.Field<rejected> { }
        [PXDBBool]
        [PXDefault(false, PersistingCheck = PXPersistingCheck.Nothing)]
        public bool? Rejected { get; set; }
        #endregion

        #region NoteID  
        [PXNote]
        [PXSearchable(PX.Objects.SM.SearchCategory.AP, "Fund Transaction {0}: {1} - {3}", new Type[] { typeof(ATPTEFMFundTransaction.refNbr), typeof(ATPTEFMFundTransaction.fundTransactionType), typeof(ATPTEFMFundTransaction.requestedByID), typeof(EPEmployee.acctName) },
            new Type[] { typeof(ATPTEFMFundTransaction.descr) },
            NumberFields = new Type[] { typeof(ATPTEFMFundTransaction.refNbr) },
            Line1Format = "{0:d}{1}{2}", Line1Fields = new Type[] { typeof(ATPTEFMFundTransaction.date), typeof(ATPTEFMFundTransaction.status), typeof(EPEmployee.acctName) },
            Line2Format = "{0}", Line2Fields = new Type[] { typeof(ATPTEFMFundTransaction.descr) },
            MatchWithJoin = typeof(InnerJoin<EPEmployee, On<EPEmployee.bAccountID, Equal<ATPTEFMFundTransaction.requestedByID>>>),
            SelectForFastIndexing = typeof(Select2<ATPTEFMFundTransaction, InnerJoin<EPEmployee, On<ATPTEFMFundTransaction.requestedByID, Equal<EPEmployee.bAccountID>>>>)
        )]
        public virtual Guid? NoteID { get; set; }
        public abstract class noteID : BqlGuid.Field<noteID> { }
        #endregion

        #region CuryInfoID
        public abstract class curyInfoID : BqlLong.Field<curyInfoID> { }
        [PXDBLong()]
        [CurrencyInfo(ModuleCode = PX.Objects.GL.BatchModule.PO)]
        public virtual long? CuryInfoID { get; set; }
        #endregion

        #region DaysExtend
        [PXDBInt]
        [PXDefault(TypeCode.Int32, "0", PersistingCheck = PXPersistingCheck.Null)]
        public virtual int? DaysExtend { get; set; }
        public abstract class daysExtend : BqlInt.Field<daysExtend> { }
        #endregion

        #region Reclassify Date
        [PXDBDate]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.ReclassifyDate)]
        [PXFormula(typeof(Default<date, daysExtend>))]
        [PXFormula(typeof(Add<date, Add<Current<ATPTEFMSetup.noDaysToLiquidateFund>, daysExtend>>))]
        public virtual DateTime? ReclassifyDate { get; set; }
        public abstract class reclassifyDate : BqlDateTime.Field<reclassifyDate> { }
        #endregion

        #region Reclassified
        [PXDBBool]
        [PXDefault(false, PersistingCheck = PXPersistingCheck.Nothing)]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.Reclassified)]
        public virtual bool? Reclassified { get; set; }
        public abstract class reclassified : BqlBool.Field<reclassified> { }
        #endregion

        #region ReclassifiedInvoiceRefNbr
        [PXDBString(15, IsUnicode = true)]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.ReclassifiedInvoiceRefNbr)]
        public virtual string ReclassifiedInvoiceRefNbr { get; set; }
        public abstract class reclassifiedInvoiceRefNbr : BqlString.Field<reclassifiedInvoiceRefNbr> { }
        #endregion

        #region TaxZoneID
        [PXDBString(IsUnicode = true)]
        public virtual string TaxZoneID { get; set; }
        public abstract class taxZoneID : BqlString.Field<taxZoneID> { }
        #endregion

        #region NoFund
        [PXDBBool()]
        [PXDefault(false)]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.NoFund, Visible = false)]
        //[PXUIVisible(typeof(Where<Current<fundTransactionType>, Equal<ATPTEFMFundTransactionTypeAttribute.reimbursementValue>>))]
        public virtual bool? NoFund { get; set; }
        public abstract class noFund : BqlBool.Field<noFund> { }
        #endregion

        #region IAssign Members
        int? PX.Data.EP.IAssign.WorkgroupID
        {
            get { return WorkgroupID; }
            set { WorkgroupID = value; }
        }

        int? PX.Data.EP.IAssign.OwnerID
        {
            get { return OwnerID; }
            set { OwnerID = value; }
        }
        #endregion

        #region TotalReceipt
        [PXDBDecimal(2)]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.TotalReceipt, Enabled = false)]
        public virtual decimal? TotalReceipt { get; set; }
        public abstract class totalReceipt : BqlDecimal.Field<totalReceipt> { }
        #endregion

        #region TotalApprovalAmount        
        [PXDBDecimal(2)]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.TotalApprovalAmount, Enabled = false)]
        public virtual decimal? TotalApprovalAmount { get; set; }
        public abstract class totalApprovalAmount : BqlDecimal.Field<totalApprovalAmount> { }
        #endregion

        #region IsImported
        [PXDBBool()]
        [PXDefault(false, PersistingCheck = PXPersistingCheck.Nothing)]
        [PXUIVisible(typeof(isImported))]
        [PXUIField(DisplayName = ATPTEFMMessages.IsImported, Enabled = false)]
        public virtual bool? IsImported { get; set; }
        public abstract class isImported : PX.Data.BQL.BqlBool.Field<isImported> { }
        #endregion

        #region IsOverbudget
        [PXDBBool()]
        [PXDefault(typeof(False), PersistingCheck = PXPersistingCheck.Nothing)]
        [PXUIField(DisplayName = ATPTEFMMessages.IsOverBudget, Enabled = false)]
        [PXUIVisible(typeof(False))]
        public bool? IsOverbudget { get; set; }
        public abstract class isOverbudget : PX.Data.BQL.BqlBool.Field<isOverbudget> { }
        #endregion

        #region HasInitialBudget
        [PXDBBool()]
        [PXDefault(typeof(False), PersistingCheck = PXPersistingCheck.Nothing)]
        [PXUIField(DisplayName = ATPTEFMMessages.HasInitialBudget, Enabled = false)]
        [PXUIVisible(typeof(False))]
        public bool? HasInitialBudget { get; set; }
        public abstract class hasInitialBudget : PX.Data.BQL.BqlBool.Field<hasInitialBudget> { }
        #endregion

        #region Budget Enabled
        [PXDBBool()]
        [PXDefault(PersistingCheck = PXPersistingCheck.Nothing)]
        public virtual bool? BudgetEnabled { get; set; }
        public abstract class budgetEnabled : PX.Data.BQL.BqlBool.Field<budgetEnabled> { }
        #endregion

        #region Project Budget Enabled
        [PXDBBool()]
        [PXDefault(PersistingCheck = PXPersistingCheck.Nothing)]
        public virtual bool? ProjectBudgetEnabled { get; set; }
        public abstract class projectBudgetEnabled : PX.Data.BQL.BqlBool.Field<projectBudgetEnabled> { }
        #endregion

        #region Unbound

        #region RequestApproval
        [PXBool]
        //[PXDefault(typeof(Search<ATPTEFMSetup.fundTransactionRequestApproval>), PersistingCheck = PXPersistingCheck.Nothing)]
        //[PXUnboundDefault(typeof(Current<ATPTEFMSetup.fundTransactionRequestApproval>), PersistingCheck = PXPersistingCheck.Null)]
        [PXUnboundDefault(typeof(Search<ATPTEFMSetup.fundTransactionRequestApproval>), PersistingCheck = PXPersistingCheck.Null)]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.RequestApproval, Visible = false)]
        public virtual bool? RequestApproval { get; set; }

        public abstract class requestApproval : BqlBool.Field<requestApproval> { }
        #endregion

        #region ApprovalWorkgroupID
        public abstract class approvalWorkgroupID : BqlInt.Field<approvalWorkgroupID> { }
        protected int? _ApprovalWorkgroupID;
        [PXInt]
        [PXSelector(typeof(Search<EPCompanyTree.workGroupID>), SubstituteKey = typeof(EPCompanyTree.description))]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.ApprovalWorkgroupID, Enabled = false)]
        public virtual int? ApprovalWorkgroupID
        {
            get
            {
                return this._ApprovalWorkgroupID;
            }
            set
            {
                this._ApprovalWorkgroupID = value;
            }
        }
        #endregion

        #region ApprovalOwnerID

        public abstract class approvalOwnerID : PX.Data.BQL.BqlInt.Field<approvalOwnerID> { }
        protected int? _ApprovalOwnerID;
        [PX.TM.Owner(IsDBField = false, DisplayName = "Approver", Enabled = false)]
        public virtual int? ApprovalOwnerID
        {
            get
            {
                return this._ApprovalOwnerID;
            }
            set
            {
                this._ApprovalOwnerID = value;
            }
        }
        #endregion

        #region ShowBudgetValidation
        [PXBool]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.ShowBudgetValidation, Visible = false)]
        public virtual bool? ShowBudgetValidation { get; set; }
        public abstract class showBudgetValidation : BqlBool.Field<showBudgetValidation> { }
        #endregion

        #region EmployeeName
        [PXString(255)]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.EmployeeName)]
        [PXUnboundDefault(typeof(Selector<requestedByID, EPEmployee.acctName>), PersistingCheck = PXPersistingCheck.Nothing)]
        [PXFormula(typeof(Default<requestedByID>))]
        public virtual string EmployeeName { get; set; }
        public abstract class employeeName : BqlString.Field<employeeName> { }
        #endregion

        #region Balance
        [PXDecimal(2)]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.Balance, Enabled = false)]
        [PXUnboundDefault(TypeCode.Decimal, "0.0", PersistingCheck = PXPersistingCheck.Nothing)]
        //[PXFormula(typeof(Sub<requestedAmount, Add<actualSpentAmount, changeAmount>>))]
        //[PXFormula(typeof(Default<requestedAmount, actualSpentAmount, changeAmount>))]
        public virtual decimal? Balance { get; set; }
        public abstract class balance : BqlDecimal.Field<balance> { }
        #endregion

        #region RefNbrUnb                
        [PXString(15)]
        [PXUnboundDefault()]
        [PXUIField(DisplayName = "RefNbrUnb")]
        [PXFormula(typeof(Default<refNbr>))]
        [PXSelector(typeof(Search<
            refNbr,
            Where<fundType, Equal<fundType>>,
            OrderBy<
                Desc<refNbr>>>),
                    typeof(refNbr),
                    typeof(branchID),
                    typeof(date),
                    typeof(fundID),
                    typeof(descr),
                    typeof(requestedByID),
                    typeof(departmentID),
                    typeof(requestedAmount))]
        public virtual string RefNbrUnb { get; set; }
        public abstract class refNbrUnb : BqlString.Field<refNbrUnb> { }
        #endregion

        #endregion

        #region For Release Cash && Submit Receipt
        #region IsReleasedCash
        [PXDBBool]
        [PXUIField(DisplayName = "")]
        [PXDefault(false, PersistingCheck = PXPersistingCheck.Nothing)]
        public virtual bool? IsReleasedCash { get; set; }
        public abstract class isReleasedCash : BqlBool.Field<isReleasedCash> { }
        #endregion

        #endregion

        #region SubmitReceiptFlag
        /// <summary>
        /// This field flags whether one of the receipts has been cancelled.
        /// </summary>
        public abstract class isSubmitReceiptFlag : BqlBool.Field<isSubmitReceiptFlag> { }
        [PXDBBool]
        [PXDefault(false, PersistingCheck = PXPersistingCheck.Nothing)]
        public bool? IsSubmitReceiptFlag { get; set; }
        #endregion

        #region ReceiptsWithNoERRefNbrs
        [PXInt]
        [PXUnboundDefault(PersistingCheck = PXPersistingCheck.Nothing)]
        public virtual int? ReceiptsWithNoERRefNbrs { get; set; }
        public abstract class receiptsWithNoERRefNbrs : PX.Data.BQL.BqlInt.Field<receiptsWithNoERRefNbrs> { }
        #endregion

        #region ReceiptsWithERRefNbrs
        [PXInt]
        [PXUnboundDefault(PersistingCheck = PXPersistingCheck.Nothing)]
        public virtual int? ReceiptsWithERRefNbrs { get; set; }
        public abstract class receiptsWithERRefNbrs : PX.Data.BQL.BqlInt.Field<receiptsWithERRefNbrs> { }
        #endregion
    }
}
