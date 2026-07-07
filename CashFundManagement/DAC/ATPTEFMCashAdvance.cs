using CashFundManagement.Attributes;
using CashFundManagement.DAC.Setup;
using PX.Data;
using PX.Data.BQL;
using PX.Data.BQL.Fluent;
using PX.Data.EP;
using PX.Data.ReferentialIntegrity.Attributes;
using PX.Objects.AP;
using PX.Objects.CM;
using PX.Objects.Common;
using PX.Objects.CR;
using PX.Objects.CS;
using PX.Objects.EP;
using PX.Objects.GL;
using PX.Objects.GL.FinPeriods;
using PX.Objects.TX;
using PX.SM;
using PX.TM;
using System;
using messages = CashFundManagement.Messages.ATPTEFMMessages;


namespace CashFundManagement.DAC
{
    /// <remarks>
    /// 2025-01-07 : Adds key classes for faster fetch of records. {RRS}
    /// 2025-01-27 : Add condition for the CA nbr numbering ID to consider Request classes with the same Request class ID but different transaction types. CaseID : 015034 {RFS}
    /// </remarks>
    [PXPrimaryGraph(typeof(CashFundManagement.BLC.ATPTEFMCashAdvanceEntry))]
    [Serializable]
    [PXCacheName(messages.ATPTEFMCashAdvance)]
    public class ATPTEFMCashAdvance : Base.ATPTEFMAudit, IBqlTable, IAssign
    {
        #region Keys
        public class PK : PrimaryKeyOf<ATPTEFMCashAdvance>.By<reqClassID, cashAdvanceNbr>
        {
            public static ATPTEFMCashAdvance Find(PXGraph graph, string reqClassID, string cashAdvanceNbr, PKFindOptions options = PKFindOptions.None)
            {
                return FindBy(graph, reqClassID, cashAdvanceNbr, options);
            }
        }
        public class UK : PrimaryKeyOf<ATPTEFMCashAdvance>.By<cashAdvanceNbr>
        {
            public static ATPTEFMCashAdvance Find(PXGraph graph, string cashAdvanceNbr, PKFindOptions options = PKFindOptions.None)
            {
                return FindBy(graph, cashAdvanceNbr, options);
            }
        }
        #endregion

        #region ReqClassID
        [PXDBString(10, IsKey = true, IsUnicode = true)]
        [PXUIField(DisplayName = messages.ReqClassID)]
        [PXSelector(typeof(Search<
            ATPTEFMReqClass.reqClassID,
            Where<ATPTEFMReqClass.tranType, Equal<ATPTEFMTranTypeAttribute.cashAdvance>>>))]
        [PXDefault("", PersistingCheck = PXPersistingCheck.NullOrBlank)]
        [PX.Data.EP.PXFieldDescription]
        public virtual string ReqClassID { get; set; }
        public abstract class reqClassID : PX.Data.BQL.BqlString.Field<reqClassID> { }
        #endregion

        #region CashAdvanceNbr                
        [PXDBString(15, IsKey = true, InputMask = ">CCCCCCCCCCCCCCC", IsUnicode = true)]
        [PXDefault()]
        [PXUIField(DisplayName = messages.CashAdvanceNbr)]
        [PXSelector(typeof(Search<
            ATPTEFMCashAdvance.cashAdvanceNbr,
            Where<ATPTEFMCashAdvance.reqClassID, Equal<Current<ATPTEFMCashAdvance.reqClassID>>>,
            OrderBy<
                Desc<cashAdvanceNbr>>>),
            typeof(branchID),
            typeof(cashAdvanceNbr),
            typeof(date),
            typeof(status),
            typeof(finPeriodID),
            typeof(descr),
            typeof(dateOfUse),
            typeof(liqDate),
            typeof(curyID),
            typeof(curyRequestedAmount),
            typeof(curyChangeAmount))]
        //[AutoNumber(typeof(ATPTEFMCASetup.cANumberingID), typeof(AccessInfo.businessDate))]
        [AutoNumber(typeof(SearchFor<ATPTEFMReqClass.numberingID>.Where<ATPTEFMReqClass.reqClassID.IsEqual<reqClassID.FromCurrent>.And<ATPTEFMReqClass.tranType.IsEqual<ATPTEFMTranTypeAttribute.cashAdvance>>>), typeof(AccessInfo.businessDate))]
        public virtual string CashAdvanceNbr { get; set; }
        public abstract class cashAdvanceNbr : PX.Data.BQL.BqlString.Field<cashAdvanceNbr> { }
        #endregion

        #region Status
        [PXDBString(1, IsFixed = true)]
        [ATPTEFMCashAdvanceStatusAttribute.ATPTEFMCashAdvanceStatus]
        [PXDefault(ATPTEFMCashAdvanceStatusAttribute.HoldValue)]
        [PXUIField(DisplayName = messages.Status)]
        public virtual string Status { get; set; }
        public abstract class status : PX.Data.BQL.BqlString.Field<status> { }
        #endregion

        #region Hold
        [PXDBBool]
        [PXDefault(true)]
        [PXUIField(DisplayName = messages.Hold)]
        [PXNoUpdate]
        public virtual bool? Hold { get; set; }
        public abstract class hold : PX.Data.BQL.BqlBool.Field<hold> { }
        #endregion

        #region Date
        [PXDBDate()]
        [PXDefault(typeof(AccessInfo.businessDate))]
        [PXUIField(DisplayName = messages.Date, Visibility = PXUIVisibility.SelectorVisible)]
        public virtual DateTime? Date { get; set; }
        public abstract class date : PX.Data.BQL.BqlDateTime.Field<date> { }
        #endregion

        #region FinPeriodID

        public abstract class finPeriodID : PX.Data.BQL.BqlString.Field<finPeriodID> { }
        [OpenPeriod(null, typeof(date), typeof(branchID),
            IsHeader = true, IsDBField = true)]
        [PXDefault(typeof(SearchFor<MasterFinPeriod.finPeriodID>
            .Where<MasterFinPeriod.endDate.IsGreaterEqual<AccessInfo.businessDate.FromCurrent>
            .And<MasterFinPeriod.startDate.IsLessEqual<AccessInfo.businessDate.FromCurrent>>>),
            PersistingCheck = PXPersistingCheck.Nothing)]
        [PXUIField(DisplayName = "Post Period", Visibility = PXUIVisibility.Invisible)]
        public virtual string FinPeriodID { get; set; }
        #endregion

        #region Descr
        [PXDBString(256, IsUnicode = true)]
        [PXDefault()]
        [PXUIField(DisplayName = messages.Descr)]
        public virtual string Descr { get; set; }
        public abstract class descr : PX.Data.BQL.BqlString.Field<descr> { }
        #endregion

        #region RequestedByID
        [PXDBInt]
        [PXDefault(typeof(Search<
            EPEmployee.bAccountID,
            Where<EPEmployee.userID, Equal<Current<AccessInfo.userID>>>>))]
        [PXUIField(DisplayName = messages.RequestedByID)]
        [PXSelector(typeof(Search2<
            EPEmployee.bAccountID,
            LeftJoin<Location,
                On<Location.bAccountID, Equal<EPEmployee.bAccountID>>>>),
            typeof(EPEmployee.bAccountID),
            typeof(EPEmployee.acctCD),
            typeof(EPEmployee.acctName),
            typeof(EPEmployee.departmentID),
            typeof(Location.vTaxZoneID),
            DescriptionField = typeof(EPEmployee.acctName),
            SubstituteKey = typeof(EPEmployee.acctCD))]
        [PXRestrictor(typeof(
                    Where<EPEmployee.userID, IsNotNull>), messages.EmployeeNotUser)]

        public virtual int? RequestedByID { get; set; }
        public abstract class requestedByID : PX.Data.BQL.BqlInt.Field<requestedByID> { }
        #endregion

        #region DepartmentID
        [PXDBString(IsUnicode = true)]
        [PXDefault(typeof(Selector<requestedByID, EPEmployee.departmentID>))]
        [PXFormula(typeof(Default<requestedByID>))]
        [PXUIField(DisplayName = messages.DepartmentID, Enabled = false)]
        [PXSelector(typeof(Search<EPDepartment.departmentID>), typeof(EPDepartment.departmentID), typeof(EPDepartment.description), DescriptionField = typeof(EPDepartment.description))]
        public virtual string DepartmentID { get; set; }
        public abstract class departmentID : PX.Data.BQL.BqlString.Field<departmentID> { }
        #endregion

        #region PositionID
        [PXString(IsUnicode = true)]
        [PXUnboundDefault(typeof(Search<
            EPEmployeePosition.positionID,
            Where<EPEmployeePosition.employeeID, Equal<Current<requestedByID>>,
                And<EPEmployeePosition.isActive, Equal<True>>>>))]
        [PXFormula(typeof(Default<requestedByID>))]
        [PXUIField(DisplayName = messages.Positions, Visibility = PXUIVisibility.SelectorVisible)]
        [PXSelector(typeof(Search<EPPosition.positionID>), typeof(EPPosition.positionID), typeof(EPPosition.description), DescriptionField = typeof(EPPosition.positionID))]
        public virtual string PositionID { get; set; }
        public abstract class positionID : PX.Data.BQL.BqlString.Field<positionID> { }
        #endregion

        #region InvoiceRefNbr
        [PXDBString(15, IsUnicode = true)]
        [PXUIField(DisplayName = messages.InvoiceRefNbr, Visible = false)]
        public virtual string InvoiceRefNbr { get; set; }
        public abstract class invoiceRefNbr : PX.Data.BQL.BqlString.Field<invoiceRefNbr> { }
        #endregion

        #region ExtRefNbr
        [PXDBString(50, IsUnicode = true)]
        [PXUIField(DisplayName = "External Reference Nbr.")]
        public virtual string ExtRefNbr { get; set; }
        public abstract class extRefNbr : PX.Data.BQL.BqlString.Field<extRefNbr> { }
        #endregion

        #region BranchID

        public abstract class branchID : PX.Data.BQL.BqlInt.Field<branchID> { }
        protected Int32? _BranchID;
        [Branch(typeof(AccessInfo.branchID), Required = true)] //Added Attribute
        //[PXDefault(typeof(Search<EPEmployee.parentBAccountID, Where<EPEmployee.userID, Equal<Current<AccessInfo.userID>>>>))]
        public virtual Int32? BranchID
        {
            get
            {
                return this._BranchID;
            }
            set
            {
                this._BranchID = value;
            }
        }
        #endregion

        #region OwnerID
        [Owner(typeof(workgroupID), DisplayName = messages.OwnerID)]
        [PXDefault(typeof(Search<
            EPEmployee.defContactID,
            Where<EPEmployee.userID, Equal<Current<AccessInfo.userID>>>>), PersistingCheck = PXPersistingCheck.Nothing)]
        //[PXUIField(DisplayName = messages.OwnerID)]
        public virtual int? OwnerID { get; set; }
        public abstract class ownerID : PX.Data.BQL.BqlInt.Field<ownerID> { }
        #endregion

        #region WorkgroupID
        [PXDBInt]
        [PXDefault(typeof(EPCompanyTree.workGroupID), PersistingCheck = PXPersistingCheck.Nothing)]
        [PX.TM.PXCompanyTreeSelector]
        [PXUIField(DisplayName = messages.WorkgroupID, Enabled = false)]
        public virtual int? WorkgroupID { get; set; }
        public abstract class workgroupID : PX.Data.BQL.BqlInt.Field<workgroupID> { }
        #endregion

        #region Approved
        [PXDBBool()]
        [PXDefault(false, PersistingCheck = PXPersistingCheck.Nothing)]
        [PXUIField(DisplayName = messages.Approved, Visibility = PXUIVisibility.Visible, Enabled = false)]
        public virtual bool? Approved { get; set; }
        public abstract class approved : PX.Data.BQL.BqlBool.Field<approved> { }
        #endregion

        #region Rejected
        public abstract class rejected : PX.Data.BQL.BqlBool.Field<rejected> { }
        [PXDBBool]
        [PXDefault(false, PersistingCheck = PXPersistingCheck.Nothing)]
        public bool? Rejected { get; set; }
        #endregion

        #region NoteID  
        [PXNote]
        [PXSearchable(PX.Objects.SM.SearchCategory.AP, "Cash Advance {0}: {1} - {3}", new Type[] { typeof(ATPTEFMCashAdvance.cashAdvanceNbr), typeof(ATPTEFMCashAdvance.reqClassID), typeof(ATPTEFMCashAdvance.requestedByID), typeof(EPEmployee.acctName) },
            new Type[] { typeof(ATPTEFMCashAdvance.descr) },
            NumberFields = new Type[] { typeof(ATPTEFMCashAdvance.cashAdvanceNbr) },
            Line1Format = "{0:d}{1}{2}", Line1Fields = new Type[] { typeof(ATPTEFMCashAdvance.date), typeof(ATPTEFMCashAdvance.status), typeof(EPEmployee.acctName) },
            Line2Format = "{0}", Line2Fields = new Type[] { typeof(ATPTEFMCashAdvance.descr) },
            MatchWithJoin = typeof(InnerJoin<EPEmployee, On<EPEmployee.bAccountID, Equal<ATPTEFMCashAdvance.requestedByID>>>),
            SelectForFastIndexing = typeof(Select2<
                ATPTEFMCashAdvance,
                InnerJoin<EPEmployee,
                    On<ATPTEFMCashAdvance.requestedByID, Equal<EPEmployee.bAccountID>>>>)
        )]
        public virtual Guid? NoteID { get; set; }
        public abstract class noteID : PX.Data.BQL.BqlGuid.Field<noteID> { }
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

        #region DateOfUse
        [PXDBDate()]
        [PXDefault(PersistingCheck = PXPersistingCheck.NullOrBlank)]
        [PXUIField(DisplayName = messages.DateOfUse, Visibility = PXUIVisibility.SelectorVisible)]
        [PXVerifyEndDate(typeof(currentDate), AllowAutoChange = false)]
        public virtual DateTime? DateOfUse { get; set; }
        public abstract class dateOfUse : PX.Data.BQL.BqlDateTime.Field<dateOfUse> { }
        #endregion

        #region InitialLiqDate
        [PXDBDate()]
        [PXDefault(PersistingCheck = PXPersistingCheck.Nothing)]
        [PXUIField(DisplayName = messages.InitialLiquidationDate, Visibility = PXUIVisibility.SelectorVisible)]
        public virtual DateTime? InitialLiqDate { get; set; }
        public abstract class initialLiqDate : PX.Data.BQL.BqlDateTime.Field<initialLiqDate> { }
        #endregion

        #region LiqDate
        [PXDBDate()]
        [PXDefault(PersistingCheck = PXPersistingCheck.Nothing)]
        [PXUIField(DisplayName = messages.LiquidationDate, Visibility = PXUIVisibility.SelectorVisible)]
        public virtual DateTime? LiqDate { get; set; }
        public abstract class liqDate : PX.Data.BQL.BqlDateTime.Field<liqDate> { }
        #endregion

        #region UnmodifiedLiqDate
        [PXDBDate()]
        [PXDefault(PersistingCheck = PXPersistingCheck.Nothing)]
        public virtual DateTime? UnmodifiedLiqDate { get; set; }
        public abstract class unmodifiedLiqDate : PX.Data.BQL.BqlDateTime.Field<unmodifiedLiqDate> { }
        #endregion

        #region CuryRequestedAmount
        //[PXDBDecimal(2)]
        [PXDBCurrency(typeof(ATPTEFMCashAdvance.curyInfoID), typeof(ATPTEFMCashAdvance.requestedAmount))]
        [PXDefault("0.0", PersistingCheck = PXPersistingCheck.Null)]
        //[PXUIVerify(typeof(Where<curyRequestedAmount, NotEqual<decimal0>>), 
        //    PXErrorLevel.Error, messages.CannotBeZero, CheckOnRowSelected = false, CheckOnInserted = false)]
        [PXUIField(DisplayName = messages.CaAmount, Enabled = false)]
        public virtual decimal? CuryRequestedAmount { get; set; }
        public abstract class curyRequestedAmount : PX.Data.BQL.BqlDecimal.Field<curyRequestedAmount> { }
        #endregion

        #region RequestedAmount
        [PXDBBaseCury()]
        [PXDefault("0.0", PersistingCheck = PXPersistingCheck.Null)]
        //[PXUIVerify(typeof(Where<requestedAmount, NotEqual<decimal0>>), PXErrorLevel.Error, messages.CannotBeZero, CheckOnRowPersisting = true, CheckOnRowSelected = false, CheckOnInserted = false)]
        [PXUIField(DisplayName = messages.RequestedAmount, Enabled = false)]
        public virtual decimal? RequestedAmount { get; set; }
        public abstract class requestedAmount : PX.Data.BQL.BqlDecimal.Field<requestedAmount> { }
        #endregion

        #region CuryActualSpentAmount
        [PXDBCurrency(typeof(ATPTEFMCashAdvance.curyInfoID), typeof(ATPTEFMCashAdvance.actualSpentAmount))]
        [PXDefault("0.0", PersistingCheck = PXPersistingCheck.Null)]
        [PXUIField(DisplayName = messages.ActualSpentAmount, Enabled = false)]
        public virtual decimal? CuryActualSpentAmount { get; set; }
        public abstract class curyActualSpentAmount : PX.Data.BQL.BqlDecimal.Field<curyActualSpentAmount> { }
        #endregion

        #region ActualSpentAmount
        [PXDBBaseCury()]
        [PXDefault("0.0", PersistingCheck = PXPersistingCheck.Null)]
        [PXUIField(DisplayName = messages.ActualSpentAmount, Enabled = false)]
        public virtual decimal? ActualSpentAmount { get; set; }
        public abstract class actualSpentAmount : PX.Data.BQL.BqlDecimal.Field<actualSpentAmount> { }
        #endregion

        #region CuryWhtTaxAmount
        [PXDBCurrency(typeof(ATPTEFMCashAdvance.curyInfoID), typeof(ATPTEFMCashAdvance.whtTaxAmount))]
        [PXDefault("0.0", PersistingCheck = PXPersistingCheck.Nothing)]
        [PXUIField(DisplayName = messages.WithholdingTax, Enabled = false)]
        public virtual decimal? CuryWhtTaxAmount { get; set; }
        public abstract class curyWhtTaxAmount : PX.Data.BQL.BqlDecimal.Field<curyWhtTaxAmount> { }
        #endregion

        #region WhtTaxAmount
        [PXDBBaseCury()]
        [PXDefault("0.0", PersistingCheck = PXPersistingCheck.Nothing)]
        [PXUIField(DisplayName = messages.WithholdingTax, Enabled = false)]
        public virtual decimal? WhtTaxAmount { get; set; }
        public abstract class whtTaxAmount : PX.Data.BQL.BqlDecimal.Field<whtTaxAmount> { }
        #endregion

        #region RefundAmount
        [PXDBDecimal(2)]
        [PXDefault(TypeCode.Decimal, "0.0")]
        [PXUIField(DisplayName = messages.ReturnAmt, Enabled = false)]
        public virtual decimal? RefundAmount { get; set; }
        public abstract class refundAmount : BqlDecimal.Field<refundAmount> { }
        #endregion

        #region CuryChangeAmount
        [PXDBCurrency(typeof(ATPTEFMCashAdvance.curyInfoID), typeof(ATPTEFMCashAdvance.changeAmount))]
        /*[PXFormula(typeof(Default<curyRequestedAmount, curyActualSpentAmount>))]
        [PXFormula(typeof(Sub<curyRequestedAmount, curyActualSpentAmount>))]*/
        [PXUIField(DisplayName = messages.ChangeAmount, Enabled = false)]
        [PXDefault("0.0", PersistingCheck = PXPersistingCheck.Null)]
        public virtual decimal? CuryChangeAmount { get; set; }
        public abstract class curyChangeAmount : PX.Data.BQL.BqlDecimal.Field<curyChangeAmount> { }
        #endregion    

        #region ChangeAmount
        [PXDBBaseCury()]
        [PXUIField(DisplayName = messages.ChangeAmount, Enabled = false)]
        [PXDefault("0.0", PersistingCheck = PXPersistingCheck.Null)]
        /*[PXFormula(typeof(Default<requestedAmount, actualSpentAmount>))]
        [PXFormula(typeof(Sub<requestedAmount, actualSpentAmount>))]*/
        public virtual decimal? ChangeAmount { get; set; }
        public abstract class changeAmount : PX.Data.BQL.BqlDecimal.Field<changeAmount> { }
        #endregion

        #region DaysExtend
        [PXDBInt]
        [PXDefault(TypeCode.Int32, "0", PersistingCheck = PXPersistingCheck.Null)]
        public virtual int? DaysExtend { get; set; }
        public abstract class daysExtend : PX.Data.BQL.BqlInt.Field<daysExtend> { }
        #endregion

        #region Reclassify Date
        [PXDBDate]
        [PXUIField(DisplayName = messages.ReclassifyDate)]
        [PXFormula(typeof(Default<dateOfUse, daysExtend>))]
        [PXFormula(typeof(Add<dateOfUse, Add<Current<Setup.ATPTEFMCASetup.standardAllowableDays>, daysExtend>>))]
        public virtual DateTime? ReclassifyDate { get; set; }
        public abstract class reclassifyDate : PX.Data.BQL.BqlDateTime.Field<reclassifyDate> { }
        #endregion

        #region Reclassified
        [PXDBBool]
        [PXDefault(false, PersistingCheck = PXPersistingCheck.Nothing)]
        [PXUIField(DisplayName = messages.Reclassified, Enabled = false)]
        public virtual bool? Reclassified { get; set; }
        public abstract class reclassified : PX.Data.BQL.BqlBool.Field<reclassified> { }
        #endregion

        #region ReclassifiedInvoiceRefNbr
        [PXDBString(15, IsUnicode = true)]
        [PXUIField(DisplayName = messages.RefNbr, Enabled = false)]
        public virtual string ReclassifiedInvoiceRefNbr { get; set; }
        public abstract class reclassifiedInvoiceRefNbr : PX.Data.BQL.BqlString.Field<reclassifiedInvoiceRefNbr> { }
        #endregion

        #region CuryID
        public abstract class curyID : PX.Data.BQL.BqlString.Field<curyID> { }
        protected String _CuryID;
        [PXDBString(5, IsUnicode = true, InputMask = ">LLLLL")]
        [PXUIField(DisplayName = "Currency", Visibility = PXUIVisibility.SelectorVisible)]
        [PXDefault(typeof(Current<AccessInfo.baseCuryID>))]
        [PXSelector(typeof(Currency.curyID))]
        public virtual String CuryID
        {
            get
            {
                return this._CuryID;
            }
            set
            {
                this._CuryID = value;
            }
        }
        #endregion

        #region CuryInfoID
        public abstract class curyInfoID : PX.Data.BQL.BqlLong.Field<curyInfoID> { }
        protected Int64? _CuryInfoID;
        [PXDBLong()]
        [CurrencyInfo(ModuleCode = BatchModule.AP)]
        public virtual Int64? CuryInfoID
        {
            get
            {
                return this._CuryInfoID;
            }
            set
            {
                this._CuryInfoID = value;
            }
        }
        #endregion

        #region TaxZoneID
        public abstract class taxZoneID : PX.Data.BQL.BqlString.Field<taxZoneID> { }
        /// <remarks>
        /// 2024-10-02 : Default to requestor's tax zone. CaseID : 007548 {RRS}
        /// </remarks>
        [PXDBString(10, IsUnicode = true)]
        [PXDefault(
            typeof(Search2<
                Location.vTaxZoneID,
                InnerJoin<BAccount,
                    On<BAccount.bAccountID, Equal<Location.bAccountID>,
                    And<BAccount.defLocationID, Equal<Location.locationID>>>,
                InnerJoin<EPEmployee,
                    On<EPEmployee.bAccountID, Equal<BAccount.bAccountID>>>>,
                Where<EPEmployee.bAccountID, Equal<Current<requestedByID>>>>),
            PersistingCheck = PXPersistingCheck.Nothing)]
        [PXFormula(typeof(Default<requestedByID>))]
        [PXUIField(DisplayName = "Tax Zone", Visibility = PXUIVisibility.Visible)]        
        [PXSelector(typeof(TaxZone.taxZoneID), DescriptionField = typeof(TaxZone.descr), Filterable = true)]
        public virtual string TaxZoneID { get; set; }
        #endregion

        #region BillType
        [PXDBString(10, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "Type", Enabled = false)]
        [APDocType.List()]
        public virtual string BillType { get; set; }
        public abstract class billType : PX.Data.BQL.BqlString.Field<billType> { }
        #endregion

        #region BillRefNbr
        [PXDBString(20, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "Reference Nbr.", Enabled = false)]
        public virtual string BillRefNbr { get; set; }
        public abstract class billRefNbr : PX.Data.BQL.BqlString.Field<billRefNbr> { }
        #endregion

        #region BillBalance
        [PXDecimal()]
        [PXUIField(DisplayName = "Balance", Enabled = false, IsReadOnly = true)]
        [PXUnboundDefault(typeof(Search<
            APRegister.curyDocBal,
            Where<APRegister.docType, Equal<Current<billType>>,
                And<APRegister.refNbr, Equal<Current<billRefNbr>>>>>))]
        public virtual Decimal? BillBalance { get; set; }
        public abstract class billBalance : PX.Data.BQL.BqlDecimal.Field<billBalance> { }
        #endregion

        #region BillStatus
        [PXString(1, IsFixed = true, InputMask = "")]
        [PXUIField(DisplayName = "Status", Enabled = false, IsReadOnly = true)]
        [APDocStatus.List()]
        [PXUnboundDefault(typeof(Search<
            APRegister.status,
            Where<APRegister.docType, Equal<Current<billType>>,
                And<APRegister.refNbr, Equal<Current<billRefNbr>>>>>))]
        public virtual string BillStatus { get; set; }
        public abstract class billStatus : PX.Data.BQL.BqlString.Field<billStatus> { }
        #endregion

        #region PmtType
        [PXDBString(10, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "Type", Enabled = false)]
        [APDocType.List()]
        public virtual string PmtType { get; set; }
        public abstract class pmtType : PX.Data.BQL.BqlString.Field<pmtType> { }
        #endregion

        #region PmtRefNbr
        [PXDBString(20, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "Reference Nbr.", Enabled = false)]
        public virtual string PmtRefNbr { get; set; }
        public abstract class pmtRefNbr : PX.Data.BQL.BqlString.Field<pmtRefNbr> { }
        #endregion

        #region PmtBalance
        [PXDecimal()]
        [PXUIField(DisplayName = "Balance", Enabled = false, IsReadOnly = true)]
        [PXUnboundDefault(typeof(Search<
            APPayment.curyDocBal,
            Where<APPayment.docType, Equal<Current<ATPTEFMCashAdvance.pmtType>>,
                And<APPayment.refNbr, Equal<Current<ATPTEFMCashAdvance.pmtRefNbr>>>>>))]
        [PXFormula(typeof(Default<ATPTEFMCashAdvance.pmtRefNbr>))]

        public virtual decimal? PmtBalance { get; set; }
        public abstract class pmtBalance : PX.Data.BQL.BqlDecimal.Field<pmtBalance> { }
        #endregion

        #region PmtStatus
        [PXString(1, IsFixed = true, InputMask = "")]
        [PXUIField(DisplayName = "Status", Enabled = false, IsReadOnly = true)]
        [APDocStatus.List()]
        [PXUnboundDefault(typeof(Search<
            APPayment.status,
            Where<APPayment.docType, Equal<Current<ATPTEFMCashAdvance.pmtType>>,
                And<APPayment.refNbr, Equal<Current<ATPTEFMCashAdvance.pmtRefNbr>>>>>))]
        public virtual string PmtStatus { get; set; }
        public abstract class pmtStatus : PX.Data.BQL.BqlString.Field<pmtStatus> { }
        #endregion        

        #region PpmType
        [PXDBString(10, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "Type")]
        [APDocType.List()]
        public virtual string PpmType { get; set; }
        public abstract class ppmType : PX.Data.BQL.BqlString.Field<ppmType> { }
        #endregion

        #region PpmRefNbr
        [PXDBString(20, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "Reference Nbr.")]
        public virtual string PpmRefNbr { get; set; }
        public abstract class ppmRefNbr : PX.Data.BQL.BqlString.Field<ppmRefNbr> { }
        #endregion

        #region PpmBalance
        [PXDecimal()]
        [PXUIField(DisplayName = "Balance", Enabled = false)]
        [PXUnboundDefault(typeof(Search<
            APPayment.curyDocBal, 
            Where<APPayment.docType, Equal<Current<ppmType>>,
                And<APPayment.refNbr, Equal<Current<ppmRefNbr>>>>>), PersistingCheck = PXPersistingCheck.Nothing)]
        [PXFormula(typeof(Default<ATPTEFMCashAdvance.ppmRefNbr>))]
        public virtual Decimal? PpmBalance { get; set; }
        public abstract class ppmBalance : PX.Data.BQL.BqlDecimal.Field<ppmBalance> { }
        #endregion

        #region PpmStatus
        [PXString(1, IsFixed = true, InputMask = "")]
        [PXUIField(DisplayName = "Status", Enabled = false, IsReadOnly = true)]
        [APDocStatus.List()]
        [PXUnboundDefault(typeof(Search<
            APPayment.status,
            Where<APPayment.docType, Equal<Current<ATPTEFMCashAdvance.ppmType>>,
                And<APPayment.refNbr, Equal<Current<ATPTEFMCashAdvance.ppmRefNbr>>>>>))]
        public virtual string PpmStatus { get; set; }
        public abstract class ppmStatus : PX.Data.BQL.BqlString.Field<ppmStatus> { }
        #endregion

        #region VendorRefundRefNbr
        [PXDBString(20, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "Reference Nbr.", Enabled = false, IsReadOnly = true)]
        public virtual string VendorRefundRefNbr { get; set; }
        public abstract class vendorRefundRefNbr : PX.Data.BQL.BqlString.Field<vendorRefundRefNbr> { }
        #endregion

        #region VendorRefundType
        [PXString(10, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "Type", Enabled = false)]
        [APDocType.List()]
        [PXUnboundDefault(typeof(APDocType.refund))]
        public virtual string VendorRefundType { get; set; }
        public abstract class vendorRefundType : PX.Data.BQL.BqlString.Field<vendorRefundType> { }
        #endregion

        #region VendorRefundBalance
        [PXDecimal()]
        [PXUIField(DisplayName = "Balance", Enabled = false, IsReadOnly = true)]
        [PXUnboundDefault(typeof(Search<
            APPayment.curyDocBal,
            Where<APPayment.docType, Equal<Current<ATPTEFMCashAdvance.vendorRefundType>>,
                And<APPayment.refNbr, Equal<Current<ATPTEFMCashAdvance.vendorRefundRefNbr>>>>>))]
        [PXFormula(typeof(Default<ATPTEFMCashAdvance.vendorRefundRefNbr>))]
        public virtual decimal? VendorRefundBalance { get; set; }
        public abstract class vendorRefundBalance : PX.Data.BQL.BqlDecimal.Field<vendorRefundBalance> { }
        #endregion

        #region VendorRefundStatus
        [PXString(1, IsFixed = true, InputMask = "")]
        [PXUIField(DisplayName = "Status", Enabled = false, IsReadOnly = true)]
        [APDocStatus.List()]
        [PXUnboundDefault(typeof(Search<
            APPayment.status,
            Where<APPayment.docType, Equal<Current<ATPTEFMCashAdvance.vendorRefundType>>,
                And<APPayment.refNbr, Equal<Current<ATPTEFMCashAdvance.vendorRefundRefNbr>>>>>))]
        public virtual string VendorRefundStatus { get; set; }
        public abstract class vendorRefundStatus : PX.Data.BQL.BqlString.Field<vendorRefundStatus> { }
        #endregion 

        #region IsCancelled
        [PXDBBool()]
        [PXDefault(false, PersistingCheck = PXPersistingCheck.Nothing)]
        [PXUIField(DisplayName = messages.CACancel, Visibility = PXUIVisibility.Visible, Enabled = false)]
        public virtual bool? IsCancelled { get; set; }
        public abstract class isCancelled : PX.Data.BQL.BqlBool.Field<isCancelled> { }
        #endregion

        #region IsImported
        [PXDBBool()]
        [PXUIField(DisplayName = "Imported", Visible = false)]
        [PXDefault(typeof(Search<ATPTEFMCASetup.isCashAdvanceMigration>), PersistingCheck = PXPersistingCheck.Nothing)]
        public virtual bool? IsImported { get; set; }
        public abstract class isImported : PX.Data.BQL.BqlBool.Field<isImported> { }
        #endregion

        #region IsOverbudget
        [PXDBBool()]
        [PXDefault(typeof(False), PersistingCheck = PXPersistingCheck.Nothing)]
        [PXUIField(DisplayName = messages.IsOverBudget, Enabled = false)]
        [PXUIVisible(typeof(False))]
        public bool? IsOverbudget { get; set; }
        public abstract class isOverbudget : PX.Data.BQL.BqlBool.Field<isOverbudget> { }
        #endregion

        #region HasInitialBudget
        [PXDBBool()]
        [PXDefault(typeof(False), PersistingCheck = PXPersistingCheck.Nothing)]
        [PXUIField(DisplayName = messages.HasInitialBudget, Enabled = false)]
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
        [PXUnboundDefault(typeof(GetSetupValue<ATPTEFMCASetup.cashAdvanceRequestApproval>), PersistingCheck = PXPersistingCheck.Null)]
        [PXUIField(DisplayName = messages.RequestApproval, Visible = false)]
        public virtual bool? RequestApproval { get; set; }

        public abstract class requestApproval : PX.Data.BQL.BqlBool.Field<requestApproval> { }
        #endregion

        #region ApprovalWorkgroupID
        public abstract class approvalWorkgroupID : PX.Data.BQL.BqlInt.Field<approvalWorkgroupID>
        {
        }
        protected int? _ApprovalWorkgroupID;
        [PXInt]
        [PXSelector(typeof(Search<EPCompanyTree.workGroupID>), SubstituteKey = typeof(EPCompanyTree.description))]
        [PXUIField(DisplayName = messages.ApprovalWorkgroupID, Enabled = false)]
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
        public abstract class approvalOwnerID : PX.Data.BQL.BqlInt.Field<approvalOwnerID>
        {
        }
        protected int? _ApprovalOwnerID;
        [Owner(IsDBField = false, DisplayName = messages.ApprovalOwnerID, Enabled = false)]
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

        #region EmployeeName
        [PXString(255)]
        [PXUIField(DisplayName = messages.EmployeeName)]
        [PXUnboundDefault(typeof(Selector<requestedByID, EPEmployee.acctName>), PersistingCheck = PXPersistingCheck.Nothing)]
        [PXFormula(typeof(Default<requestedByID>))]
        public virtual string EmployeeName { get; set; }
        public abstract class employeeName : PX.Data.BQL.BqlString.Field<employeeName> { }
        #endregion

        #region CurrentDate
        [PXDate()]
        [PXUnboundDefault(typeof(Current<AccessInfo.businessDate>))]
        public virtual DateTime? CurrentDate { get; set; }
        public abstract class currentDate : PX.Data.BQL.BqlDateTime.Field<currentDate> { }
        #endregion

        #region InvoiceStatus
        [PXString(1, IsFixed = true)]
        [PXUnboundDefault(typeof(Search<
            APRegister.status,
            Where<APRegister.refNbr, Equal<Current<ATPTEFMCashAdvance.invoiceRefNbr>>>>))]
        public virtual string InvoiceStatus { get; set; }
        public abstract class invoiceStatus : PX.Data.BQL.BqlString.Field<invoiceStatus> { }
        #endregion

        #region ATPTEFMCashAdvanceNbr                
        [PXString(15, InputMask = ">CCCCCCCCCCCCCCC", IsUnicode = true)]
        [PXUIField(DisplayName = messages.CashAdvanceNbr)]
        [PXSelector(typeof(Search<ATPTEFMCashAdvance.cashAdvanceNbr>))]
        public virtual string ATPTEFMCashAdvanceNbr { get; set; }
        public abstract class aTPTEFMCashAdvanceNbr : PX.Data.BQL.BqlString.Field<aTPTEFMCashAdvanceNbr> { }
        #endregion


        #region ReclassifyType
        [PXString(10, IsUnicode = true)]
        [PXUIField(DisplayName = "Type", Enabled = false)]
        [APDocType.List()]
        [PXUnboundDefault(typeof(APDocType.creditAdj))]
        public virtual string ReclassifyType { get; set; }
        public abstract class reclassifyType : PX.Data.BQL.BqlString.Field<reclassifyType> { }
        #endregion

        #region ReclassifyBalance
        [PXDecimal()]
        [PXUIField(DisplayName = "Balance", Enabled = false, IsReadOnly = true)]
        [PXUnboundDefault(typeof(Search<
            APRegister.curyDocBal,
            Where<APRegister.docType, Equal<Current<reclassifyType>>,
                And<APRegister.refNbr, Equal<Current<reclassifiedInvoiceRefNbr>>>>>))]
        public virtual Decimal? ReclassifyBalance { get; set; }
        public abstract class reclassifyBalance : PX.Data.BQL.BqlDecimal.Field<reclassifyBalance> { }
        #endregion

        #region ReclassifyStatus
        [PXString(1, IsFixed = true)]
        [PXUIField(DisplayName = "Status", Enabled = false, IsReadOnly = true)]
        [APDocStatus.List()]
        [PXUnboundDefault(typeof(Search<
            APRegister.status,
            Where<APRegister.docType, Equal<Current<reclassifyType>>,
                And<APRegister.refNbr, Equal<Current<reclassifiedInvoiceRefNbr>>>>>))]
        public virtual string ReclassifyStatus { get; set; }
        public abstract class reclassifyStatus : PX.Data.BQL.BqlString.Field<reclassifyStatus> { }
        #endregion

        #region ExecuteValidations
        [PXBool]
        [PXUnboundDefault(typeof(True), PersistingCheck = PXPersistingCheck.Nothing)]
        public virtual bool? ExecuteValidations { get; set; }

        public abstract class executeValidations : PX.Data.BQL.BqlBool.Field<executeValidations> { }
        #endregion

        #region ReceiptsWithNoERRefNbrs
        [PXInt]
        [PXUnboundDefault(PersistingCheck = PXPersistingCheck.Nothing)]
        public virtual int? ReceiptsWithNoERRefNbrs { get; set; }
        public abstract class receiptsWithNoERRefNbrs : PX.Data.BQL.BqlInt.Field<receiptsWithNoERRefNbrs> { }
        #endregion

        #region ReceiptsWithNoLiquidationRefNbr
        [PXInt]
        [PXUnboundDefault(PersistingCheck = PXPersistingCheck.Nothing)]
        public virtual int? ReceiptsWithNoLiquidationRefNbr { get; set; }
        public abstract class receiptsWithNoLiquidationRefNbr : PX.Data.BQL.BqlInt.Field<receiptsWithNoLiquidationRefNbr> { }
        #endregion

        #region CAPendingforLiquidationAndEmptyReceipts
        [PXBool]
        [PXUnboundDefault(PersistingCheck = PXPersistingCheck.Nothing)]
        public virtual bool? CAPendingforLiquidationAndEmptyReceipts { get; set; }
        public abstract class cAPendingforLiquidationAndEmptyReceipts : PX.Data.BQL.BqlBool.Field<cAPendingforLiquidationAndEmptyReceipts> { }
        #endregion

        #region AnyUnprocessedEC
        [PXBool]
        [PXUnboundDefault(PersistingCheck = PXPersistingCheck.Nothing)]
        public virtual bool? AnyUnprocessedEC { get; set; }
        public abstract class anyUnprocessedEC : PX.Data.BQL.BqlBool.Field<anyUnprocessedEC> { }
        #endregion

        #region AnyUnprocessedReceipts
        [PXBool]
        [PXUnboundDefault(PersistingCheck = PXPersistingCheck.Nothing)]
        public virtual bool? AnyUnprocessedReceipts { get; set; }
        public abstract class anyUnprocessedReceipts : PX.Data.BQL.BqlBool.Field<anyUnprocessedReceipts> { }
        #endregion

        #region AnyUnprocessedClaim
        [PXBool]
        [PXUnboundDefault(PersistingCheck = PXPersistingCheck.Nothing)]
        public virtual bool? AnyUnprocessedClaim { get; set; }
        public abstract class anyUnprocessedClaim : PX.Data.BQL.BqlBool.Field<anyUnprocessedClaim> { }
        #endregion

        #region HasRefund
        [PXBool]
        [PXUnboundDefault(PersistingCheck = PXPersistingCheck.Nothing)]
        public virtual bool? HasRefund { get; set; }
        public abstract class hasRefund : PX.Data.BQL.BqlBool.Field<hasRefund> { }
        #endregion

        #region HasBalance
        [PXBool]
        [PXUnboundDefault(PersistingCheck = PXPersistingCheck.Nothing)]
        public virtual bool? HasBalance { get; set; }
        public abstract class hasBalance : PX.Data.BQL.BqlBool.Field<hasBalance> { }
        #endregion

        #endregion
    }
}