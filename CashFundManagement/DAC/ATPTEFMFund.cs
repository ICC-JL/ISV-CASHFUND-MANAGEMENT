using CashFundManagement.Attributes;
using CashFundManagement.BLC;
using CashFundManagement.DAC.Setup;
using CashFundManagement.Messages;
using PX.Data;
using PX.Data.BQL;
using PX.Data.EP;
using PX.Data.ReferentialIntegrity.Attributes;
using PX.Objects.AP;
using PX.Objects.CM;
using PX.Objects.CN.ProjectAccounting.Descriptor;
using PX.Objects.CN.ProjectAccounting.PM.Descriptor;
using PX.Objects.Common;
using PX.Objects.CS;
using PX.Objects.EP;
using PX.Objects.GL;
using PX.Objects.PM;
using PX.TM;
using System;

namespace CashFundManagement.DAC
{
    [Serializable]
    [PXCacheName(Messages.ATPTEFMMessages.ATPTEFMFund)]
    [PXPrimaryGraph(typeof(ATPTEFMFundMaint))]
    public class ATPTEFMFund : CashFundManagement.DAC.Base.ATPTEFMBase, IBqlTable, IAssign
    {
        #region Audit
        #region CreatedByID
        /// <summary>
        /// Audit Bql field.
        /// </summary>
        public abstract class createdByID : IBqlField { }
        /// <summary>
        /// Audit Bql property field.
        /// </summary>
        [PXDBCreatedByID()]
        public virtual Guid? CreatedByID { get; set; }
        #endregion

        #region CreatedByScreenID
        /// <summary>
        /// Audit Bql field.
        /// </summary>
        public abstract class createdByScreenID : IBqlField { }
        /// <summary>
        /// Audit Bql property field.
        /// </summary>
        [PXDBCreatedByScreenID()]
        public virtual String CreatedByScreenID { get; set; }
        #endregion

        #region CreatedDateTime
        /// <summary>
        /// Audit Bql field.
        /// </summary>
        public abstract class createdDateTime : IBqlField { }
        /// <summary>
        /// Audit Bql property field.
        /// </summary>
        [PXDBCreatedDateTime()]
        [PXUIField(DisplayName = PXDBLastModifiedByIDAttribute.DisplayFieldNames.CreatedDateTime, Enabled = false, IsReadOnly = true)]
        public virtual DateTime? CreatedDateTime { get; set; }
        #endregion

        #region LastModifiedByID
        /// <summary>
        /// Audit Bql field.
        /// </summary>
        public abstract class lastModifiedByID : IBqlField { }
        /// <summary>
        /// Audit Bql property field.
        /// </summary>
        [PXDBLastModifiedByID()]
        public virtual Guid? LastModifiedByID { get; set; }
        #endregion

        #region LastModifiedByScreenID
        /// <summary>
        /// Audit Bql field.
        /// </summary>
        public abstract class lastModifiedByScreenID : IBqlField { }
        /// <summary>
        /// Audit Bql property field.
        /// </summary>
        [PXDBLastModifiedByScreenID()]
        public virtual String LastModifiedByScreenID { get; set; }
        #endregion

        #region LastModifiedDateTime
        /// <summary>
        /// Audit Bql field.
        /// </summary>
        public abstract class lastModifiedDateTime : IBqlField { }
        /// <summary>
        /// Audit Bql property field.
        /// </summary>
        [PXDBLastModifiedDateTime()]
        [PXUIField(DisplayName = PXDBLastModifiedByIDAttribute.DisplayFieldNames.LastModifiedDateTime, Enabled = false, IsReadOnly = true)]
        public virtual DateTime? LastModifiedDateTime { get; set; }
        #endregion

        #region tstamp
        /// <summary>
        /// Audit Bql field.
        /// </summary>
        public abstract class Tstamp : IBqlField { }
        /// <summary>
        /// Audit Bql property field.
        /// </summary>
        [PXDBTimestamp()]
        public virtual Byte[] tstamp { get; set; }
        #endregion
        #endregion

        #region SUMMARY
        #region FundID
        [PXDBIdentity()]
        public virtual int? FundID { get; set; }
        public abstract class fundID : BqlInt.Field<fundID> { }
        #endregion

        #region FundCD                
        [PXDBString(15, IsKey = true, InputMask = ">CCCCCCCCCCCCCCC", IsUnicode = true)]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.RefNbr)]
        [PXSelector(typeof(Search3<
            ATPTEFMFund.fundCD,
            InnerJoin<EPEmployee,
                On<EPEmployee.bAccountID, Equal<custodianID>>>,
            OrderBy<
                Desc<fundCD>>>),
            typeof(branchID),
            typeof(fundType),
            typeof(fundCD),
            typeof(status),
            typeof(custodianID),
            typeof(employeeName),
            typeof(documentDate),
            typeof(descr),
            typeof(initialFund))]
        [AutoNumber(typeof(ATPTEFMSetup.fundNumberingID), typeof(AccessInfo.businessDate))]
        public virtual string FundCD { get; set; }
        public abstract class fundCD : BqlString.Field<fundCD> { }
        #endregion

        #region FundType
        [PXDBString(1, IsFixed = true)]
        [ATPTEFMFundTypeAttribute.ATPTEFMFundType]
        [PXDefault(ATPTEFMFundTypeAttribute.PettyCashValue)]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.FundType, Visibility = PXUIVisibility.SelectorVisible)]
        public virtual string FundType { get; set; }
        public abstract class fundType : BqlString.Field<fundType> { }
        #endregion

        #region Status
        [PXDBString(1, IsFixed = true)]
        [ATPTEFMFundStatusAttribute.ATPTEFMFundStatus()]
        [PXDefault(ATPTEFMFundStatusAttribute.HoldValue)]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.Status, Enabled = false, Visibility = PXUIVisibility.SelectorVisible)]
        public virtual string Status { get; set; }
        public abstract class status : BqlString.Field<status> { }
        #endregion

        #region Hold
        [PXDBBool]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.Hold, Enabled = false)]
        [PXDefault(true)]
        [PXNoUpdate]
        public virtual bool? Hold { get; set; }
        public abstract class hold : BqlBool.Field<hold> { }
        #endregion

        #region DocumentDate
        [PXDBDate]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.DocumentDate, Visibility = PXUIVisibility.SelectorVisible)]
        [PXDefault(typeof(AccessInfo.businessDate), PersistingCheck = PXPersistingCheck.Nothing)]
        public virtual DateTime? DocumentDate { get; set; }
        public abstract class documentDate : BqlDateTime.Field<documentDate> { }
        #endregion

        #region BranchID
        [Branch(typeof(AccessInfo.branchID), DisplayName = Messages.ATPTEFMMessages.Branch, Required = true)]
        //[PXDefault(typeof(AccessInfo.branchID))]
        //[PXUIField(DisplayName = Messages.ATPTEFMMessages.Branch)]
        //[PXDBInt]
        //[PXDefault(typeof(Search<Branch.bAccountID, Where<Branch.active, Equal<True>, And<Branch.branchID, Equal<Current<AccessInfo.branchID>>>>>))]
        //[PXUIField(DisplayName = Messages.ATPTEFMMessages.Branch)]
        //[PXDimensionSelector("BIZACCT", typeof(Search<Branch.bAccountID, Where<Branch.active, Equal<True>, And<MatchWithBranch<Branch.branchID>>>>), typeof(Branch.branchCD), DescriptionField = typeof(Branch.acctName))]
        public virtual int? BranchID { get; set; }
        public abstract class branchID : BqlInt.Field<branchID> { }
        #endregion

        #region CustodianID
        [PXDBInt]
        [PXDefault(typeof(Search<
            EPEmployee.bAccountID,
            Where<EPEmployee.userID, Equal<Current<AccessInfo.userID>>>>), PersistingCheck = PXPersistingCheck.Nothing)]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.CustodianID)]
        [PXSelector(typeof(Search<
            EPEmployee.bAccountID,
            Where<EPEmployee.status, NotEqual<VendorStatus.inactive>>>),
            typeof(EPEmployee.bAccountID),
            typeof(EPEmployee.departmentID),
            typeof(EPEmployee.parentBAccountID), DescriptionField = typeof(EPEmployee.acctName), SubstituteKey = typeof(EPEmployee.acctCD))]
        [PXRestrictor(typeof(
                Where<EPEmployee.userID, IsNotNull>), ATPTEFMMessages.EmployeeNotUser)]
        public virtual int? CustodianID { get; set; }
        public abstract class custodianID : BqlInt.Field<custodianID> { }
        #endregion

        #region CuryID
        public abstract class curyID : PX.Data.BQL.BqlString.Field<curyID> { }
        protected String _CuryID;
        [PXDBString(5, IsUnicode = true, InputMask = ">LLLLL")]
        [PXUIField(DisplayName = "Currency", Visibility = PXUIVisibility.SelectorVisible)]
        [PXDefault(PersistingCheck = PXPersistingCheck.Nothing)]
        [PXFormula(typeof(Selector<custodianID, EPEmployee.curyID>))]
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

        #region PayeeID
        [PXDBInt]
        [PXDefault(typeof(Current<custodianID>), PersistingCheck = PXPersistingCheck.Nothing)]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.PayeeID)]
        [PXSelector(typeof(Search<
            EPEmployee.bAccountID, 
            Where<EPEmployee.status, NotEqual<VendorStatus.inactive>>>),
            typeof(EPEmployee.bAccountID),
            typeof(EPEmployee.acctName),
            typeof(EPEmployee.departmentID),
            typeof(EPEmployee.parentBAccountID), DescriptionField = typeof(EPEmployee.acctName), SubstituteKey = typeof(EPEmployee.acctCD))]
        [PXRestrictor(typeof(
                Where<EPEmployee.userID, IsNotNull>), ATPTEFMMessages.EmployeeNotUser)]
        [PXFormula(typeof(Default<custodianID>))]
        public virtual int? PayeeID { get; set; }
        public abstract class payeeID : BqlInt.Field<payeeID> { }
        #endregion

        #region CuryInitialFund
        [PXDBCurrency(typeof(ATPTEFMFund.curyInfoID), typeof(ATPTEFMFund.initialFund))]
        [PXDefault("0.0", PersistingCheck = PXPersistingCheck.Null)]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.InitialFund, Visibility = PXUIVisibility.SelectorVisible)]
        public virtual decimal? CuryInitialFund { get; set; }
        public abstract class curyInitialFund : BqlDecimal.Field<curyInitialFund> { }
        #endregion

        #region InitialFund
        [PXDBBaseCury()]
        [PXDefault("0.0", PersistingCheck = PXPersistingCheck.Null)]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.InitialFund, Visibility = PXUIVisibility.SelectorVisible)]
        public virtual decimal? InitialFund { get; set; }
        public abstract class initialFund : BqlDecimal.Field<initialFund> { }
        #endregion

        #region Descr
        [PXDBString(255, IsUnicode = true)]
        [PXDefault(PersistingCheck = PXPersistingCheck.Nothing)]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.Descr, Visibility = PXUIVisibility.SelectorVisible)]
        public virtual string Descr { get; set; }
        public abstract class descr : BqlString.Field<descr> { }
        #endregion

        #region Released
        [PXDBBool]
        [PXDefault(false, PersistingCheck = PXPersistingCheck.Nothing)]
        public virtual bool? Released { get; set; }
        public abstract class released : BqlBool.Field<released> { }
        #endregion

        #region APPROVAL DETAILS

        #region Approved
        [PXDBBool()]
        [PXDefault(false, PersistingCheck = PXPersistingCheck.Nothing)]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.Approved, Visibility = PXUIVisibility.Visible, Enabled = false)]
        [PXUIVisible(typeof(
                        Where<GetSetupValue<ATPTEFMSetup.fundsApprovalSetup>, Equal<True>>))]
        public virtual bool? Approved { get; set; }
        public abstract class approved : BqlBool.Field<approved> { }
        #endregion

        #region Rejected
        public abstract class rejected : BqlBool.Field<rejected> { }
        [PXDBBool]
        [PXDefault(false, PersistingCheck = PXPersistingCheck.Nothing)]
        public bool? Rejected { get; set; }
        #endregion

        #region OwnerID

        [PXDefault(typeof(AccessInfo.contactID.FromCurrent), PersistingCheck = PXPersistingCheck.Nothing)]
        [Owner(typeof(workgroupID), DisplayName = Messages.ATPTEFMMessages.OwnerID)]
        public virtual int? OwnerID { get; set; }
        public abstract class ownerID : BqlInt.Field<ownerID> { }
        #endregion

        #region WorkgroupID
        public abstract class workgroupID : BqlInt.Field<workgroupID> { }
        protected int? _WorkgroupID;
        [PXDBInt]
        [PXDefault(typeof(EPCompanyTree.workGroupID), PersistingCheck = PXPersistingCheck.Nothing)]
        [PXCompanyTreeSelector]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.WorkgroupID, Enabled = false)]
        public virtual int? WorkgroupID { get; set; }
        #endregion

        #region NoteID  
        [PXNote]
        [PXSearchable(PX.Objects.SM.SearchCategory.AP, "Fund {0}: {1} - {3}", new Type[] { typeof(ATPTEFMFund.fundCD), typeof(ATPTEFMFund.fundType), typeof(ATPTEFMFund.custodianID), typeof(EPEmployee.acctName) },
            new Type[] { typeof(ATPTEFMFund.descr) },
            NumberFields = new Type[] { typeof(ATPTEFMFund.fundCD) },
            Line1Format = "{0:d}{1}{2}", Line1Fields = new Type[] { typeof(ATPTEFMFund.documentDate), typeof(ATPTEFMFund.status), typeof(EPEmployee.acctName) },
            Line2Format = "{0}", Line2Fields = new Type[] { typeof(ATPTEFMFundTransaction.descr) },
            MatchWithJoin = typeof(InnerJoin<EPEmployee, On<EPEmployee.bAccountID, Equal<ATPTEFMFund.custodianID>>>),
            SelectForFastIndexing = typeof(Select2<
                ATPTEFMFund,
                InnerJoin<EPEmployee,
                    On<ATPTEFMFund.custodianID, Equal<EPEmployee.bAccountID>>>>)
        )]
        public virtual Guid? NoteID { get; set; }
        public abstract class noteID : BqlGuid.Field<noteID> { }
        #endregion

        #region IAssign Members
        int? IAssign.WorkgroupID
        {
            get { return WorkgroupID; }
            set { WorkgroupID = value; }
        }

        int? IAssign.OwnerID
        {
            get { return OwnerID; }
            set { OwnerID = value; }
        }

        #endregion



        #endregion

        #region FINANCIAL DETAILS
        #region AccountID
        [Account(DisplayName = Messages.ATPTEFMMessages.AccountID, Required = true)]
        [PXDefault(PersistingCheck = PXPersistingCheck.NullOrBlank)]
        [PXFormula(typeof(Switch<
                            Case<
            Where<fundType, Equal<ATPTEFMFundTypeAttribute.pettyCashValue>>, IsNull<Current<ATPTEFMSetup.pCFAccount>, Null>>, IsNull<Current<ATPTEFMSetup.rVFAccount>, Null>>))]
        public int? AccountID { get; set; }

        public abstract class accountID : BqlInt.Field<accountID> { }
        #endregion

        #region SubID
        [SubAccount(typeof(accountID), DisplayName = Messages.ATPTEFMMessages.Subid, Required = true)]
        [PXDefault(PersistingCheck = PXPersistingCheck.NullOrBlank)]
        [PXFormula(typeof(Switch<
                            Case<
            Where<fundType, Equal<ATPTEFMFundTypeAttribute.pettyCashValue>>, IsNull<Current<ATPTEFMSetup.pCFSubaccount>, Null>>, IsNull<Current<ATPTEFMSetup.rVFSubaccount>, Null>>))]
        public int? SubID { get; set; }
        public abstract class subID : BqlInt.Field<subID> { }
        #endregion

        #region Clearing Account
        [Account(DisplayName = Messages.ATPTEFMMessages.ClearingAccount)]
        [PXDefault(PersistingCheck = PXPersistingCheck.NullOrBlank)]
        [PXFormula(typeof(Current<ATPTEFMSetup.clearingAccount>))]
        public int? ClearingAccount { get; set; }
        public abstract class clearingAccount : BqlInt.Field<clearingAccount> { }
        #endregion

        #region Clearing Subacccount
        [SubAccount(typeof(clearingAccount), DisplayName = Messages.ATPTEFMMessages.ClearingSubaccount)]
        [PXDefault(PersistingCheck = PXPersistingCheck.NullOrBlank)]
        [PXFormula(typeof(Current<ATPTEFMSetup.clearingSubaccount>))]
        public int? ClearingSubaccount { get; set; }
        public abstract class clearingSubaccount : BqlInt.Field<clearingAccount> { }
        #endregion

        #region EstablishmentRefNbr
        [PXDBString(15, IsUnicode = true)]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.EstablishmentAPRef)]
        [PXDefault(PersistingCheck = PXPersistingCheck.Nothing)]
        public virtual string EstablishmentRefNbr { get; set; }
        public abstract class establishmentRefNbr : BqlString.Field<establishmentRefNbr> { }
        #endregion

        #region CloseFundRefNbr
        [PXDBString(15, IsUnicode = true)]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.CloseFundAPRef)]
        [PXDefault(PersistingCheck = PXPersistingCheck.Nothing)]
        public virtual string CloseFundRefNbr { get; set; }
        public abstract class closeFundRefNbr : BqlString.Field<closeFundRefNbr> { }
        #endregion

        #region ExpenseBatchNbr
        [PXDBString(15, IsUnicode = true)]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.ExpenseBatchNbr)]
        [PXDefault(PersistingCheck = PXPersistingCheck.Nothing)]
        public virtual string ExpenseBatchNbr { get; set; }
        public abstract class expenseBatchNbr : BqlString.Field<expenseBatchNbr> { }
        #endregion

        #region ProjectID
        [PXDefault(typeof(NonProject), PersistingCheck = PXPersistingCheck.Nothing)]
        [APActiveProject]
        [PXForeignReference(typeof(Field<projectID>.IsRelatedTo<PMProject.contractID>))]
        [PXUIVisible(typeof(FeatureInstalled<FeaturesSet.projectAccounting>))]
        [PXUIRequired(typeof(FeatureInstalled<FeaturesSet.projectAccounting>))]
        public virtual int? ProjectID { get; set; }
        public abstract class projectID : BqlInt.Field<projectID> { }
        #endregion

        #region ProjectTaskID
        [PXDBInt]
        [PXDefault(PersistingCheck = PXPersistingCheck.Nothing)]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.ProjectTaskid)]
        [PXSelector(typeof(Search<
            PMTask.taskID,
            Where<PMTask.projectID, Equal<Current<projectID>>>>), typeof(PMTask.taskCD), typeof(PMTask.description), SubstituteKey = typeof(PMTask.taskCD))]
        [PXUIRequired(typeof(
                        Where<projectID, NotEqual<NonProject>>))]
        [PXUIVisible(typeof(FeatureInstalled<FeaturesSet.projectAccounting>))]
        [PXRestrictor(typeof(
                        Where<PMTask.type, NotEqual<ProjectTaskType.revenue>>),
            ProjectAccountingMessages.TaskTypeIsNotAvailable)]
        public virtual int? ProjectTaskID { get; set; }
        public abstract class projectTaskID : BqlInt.Field<projectTaskID> { }
        #endregion

        #region CostCodeID
        [PXForeignReference(typeof(Field<costCodeID>.IsRelatedTo<PMCostCode.costCodeID>))]
        [CostCode(typeof(accountID), typeof(projectTaskID), PX.Objects.GL.AccountType.Expense, ReleasedField = typeof(released), ProjectField = typeof(projectID),
            Visibility = PXUIVisibility.SelectorVisible, DescriptionField = typeof(PMCostCode.description))]
        [PXUIVisible(typeof(FeatureInstalled<FeaturesSet.costCodes>))]
        public virtual Int32? CostCodeID { get; set; }
        public abstract class costCodeID : PX.Data.BQL.BqlInt.Field<costCodeID> { }
        #endregion
        #endregion

        #region REPLENISHMENT DETAILS

        #region ReplenishmentLimit
        [PXDBString(1, IsFixed = true)]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.ReplenishmentLimit)]
        [ATPTEFMReplenishmentStringListAttribute.ATPTEFMReplenishmentLimitList()]
        [PXDefault(typeof(ATPTEFMSetup.replenishmentLimit), PersistingCheck = PXPersistingCheck.Nothing)]
        public virtual string ReplenishmentLimit { get; set; }
        public abstract class replenishmentLimit : BqlString.Field<replenishmentLimit> { }
        #endregion
        #region ReplenishPointPercent
        [PXDBDecimal(2, MinValue = 0, MaxValue = 100)]
        [PXDefault]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.ReplenishPointPercent)]
        [PXUIEnabled(typeof(
                        Where<replenishmentLimit, Equal<ATPTEFMReplenishmentStringListAttribute.percentValue>>))]
        [PXUIRequired(typeof(
                        Where<replenishmentLimit, Equal<ATPTEFMReplenishmentStringListAttribute.percentValue>>))]
        [PXFormula(typeof(Default<replenishmentLimit>))]
        public virtual decimal? ReplenishPointPercent { get; set; }
        public abstract class replenishPointPercent : BqlDecimal.Field<replenishPointPercent> { }
        #endregion
        #region ReplenishmentAmt
        [PXDBDecimal(2, MinValue = 0)]
        [PXDefault]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.ReplenishmentAmount)]
        [PXUIEnabled(typeof(
                        Where<replenishmentLimit, Equal<ATPTEFMReplenishmentStringListAttribute.amountValue>>))]
        [PXUIRequired(typeof(
                        Where<replenishmentLimit, Equal<ATPTEFMReplenishmentStringListAttribute.amountValue>>))]
        [PXFormula(typeof(Default<replenishmentLimit>))]
        public virtual decimal? ReplenishmentAmt { get; set; }
        public abstract class replenishmentAmt : BqlDecimal.Field<replenishmentAmt> { }
        #endregion
        #region ReplenishmentRestriction
        [PXDBString(1, IsFixed = true)]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.ReplenishmentRestrictions)]
        [ATPTEFMReplenishmentStringListAttribute.ATPTEFMReplenishmentRestrictionList()]
        [PXDefault(ATPTEFMReplenishmentStringListAttribute.Warning)]
        public virtual string ReplenishmentRestriction { get; set; }
        public abstract class replenishmentRestriction : BqlString.Field<replenishmentRestriction> { }
        #endregion

        #endregion

        #region Fund Transaction Restriction

        #region FundTransactionLimit
        [PXDBString(1, IsFixed = true)]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.FundTransactionLimit)]
        [ATPTEFMReplenishmentStringListAttribute.ATPTEFMFundTransactionLimitList()]
        [PXDefault(typeof(ATPTEFMSetup.fundTransactionLimit), PersistingCheck = PXPersistingCheck.Nothing)]
#if Version24R1
#else
        [PXUIVisible(typeof(Current<ATPTEFMSetup.enableFundTransactionLimit>))]
#endif
        public virtual string FundTransactionLimit { get; set; }
        public abstract class fundTransactionLimit : BqlString.Field<fundTransactionLimit> { }
        #endregion

        #region FundTransactionPointPercent
        [PXDBDecimal(2, MinValue = 0, MaxValue = 100)]
        [PXDefault(TypeCode.Decimal, "0.0", PersistingCheck = PXPersistingCheck.Nothing)]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.FundTransactionPointPercent/*, Required = true*/)]
        [PXUIEnabled(typeof(
                        Where<fundTransactionLimit, Equal<ATPTEFMReplenishmentStringListAttribute.percentValue>>))]
        //[PXUIRequired(typeof(Where<fundTransactionLimit, Equal<ATPTEFMReplenishmentStringListAttribute.percentValue>, And<Current<ATPTEFMSetup.enableFundTransactionLimit>, Equal<True>>>))]
        [PXFormula(typeof(Default<fundTransactionLimit>))]
#if Version24R1
#else
        [PXUIVisible(typeof(Current<ATPTEFMSetup.enableFundTransactionLimit>))]
#endif
        public virtual decimal? FundTransactionPointPercent { get; set; }
        public abstract class fundTransactionPointPercent : BqlDecimal.Field<fundTransactionPointPercent> { }
        #endregion

        #region FundTransactionAmt
        [PXDBDecimal(2, MinValue = 0)]
        [PXDefault(TypeCode.Decimal, "0.0", PersistingCheck = PXPersistingCheck.Nothing)]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.FundTransactionAmount/*, Required = true*/)]
        [PXUIEnabled(typeof(
                        Where<fundTransactionLimit, Equal<ATPTEFMReplenishmentStringListAttribute.amountValue>>))]
        //[PXUIRequired(typeof(Where<fundTransactionLimit, Equal<ATPTEFMReplenishmentStringListAttribute.amountValue>, And<Current<ATPTEFMSetup.enableFundTransactionLimit>, Equal<True>>>))]
        [PXFormula(typeof(Default<fundTransactionLimit>))]
#if Version24R1
#else
        [PXUIVisible(typeof(Current<ATPTEFMSetup.enableFundTransactionLimit>))]
#endif
        public virtual decimal? FundTransactionAmt { get; set; }
        public abstract class fundTransactionAmt : BqlDecimal.Field<fundTransactionAmt> { }
        #endregion

        #region FundTransactionRestriction
        [PXDBString(1, IsFixed = true)]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.FundTransactionRestrictions)]
        [ATPTEFMReplenishmentStringListAttribute.ATPTEFMFundTransactionRestrictionList()]
        [PXDefault(ATPTEFMReplenishmentStringListAttribute.Warning)]
#if Version24R1
#else
        [PXUIVisible(typeof(Current<ATPTEFMSetup.enableFundTransactionLimit>))]
#endif
        public virtual string FundTransactionRestriction { get; set; }
        public abstract class fundTransactionRestriction : BqlString.Field<fundTransactionRestriction> { }
        #endregion

        #endregion

        #region IsImported
        [PXDBBool()]
        [PXUIField(DisplayName = "Imported")]
        [PXDefault(false, PersistingCheck = PXPersistingCheck.Nothing)]
        public virtual bool? IsImported { get; set; }
        public abstract class isImported : PX.Data.BQL.BqlBool.Field<isImported> { }
        #endregion

        #region IsOldFund
        [PXDBBool()]
        [PXUIField(DisplayName = "Old Fund")]
        [PXDefault(false, PersistingCheck = PXPersistingCheck.Nothing)]
        public virtual bool? IsOldFund { get; set; }
        public abstract class isOldFund : PX.Data.BQL.BqlBool.Field<isOldFund> { }
        #endregion

        #region ATPTEFMValidateAmountReceivedAndAmountReleased
        [PXDBBool]
        public virtual bool? ATPTEFMValidateAmountReceivedAndAmountReleased { get; set; }
        public abstract class aTPTEFMValidateAmountReceivedAndAmountReleased : BqlBool.Field<aTPTEFMValidateAmountReceivedAndAmountReleased> { }
        #endregion

        #region Unbound Fields

        #region RequestApproval
        [PXBool()]
        [PXUnboundDefault(typeof(Search<ATPTEFMSetup.fundsApprovalSetup>))]
        //[PXFormula(typeof(Default<ATPTEFMSetup.fundsApprovalSetup>))]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.RequestApproval, Visible = false)]
        public virtual bool? RequestApproval { get; set; }
        public abstract class requestApproval : BqlBool.Field<requestApproval> { }
        #endregion

        #region EmployeeName
        [PXString]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.CustodianName, Enabled = false)]
        [PXFormula(typeof(Selector<custodianID, EPEmployee.acctName>))]
        public virtual string EmployeeName { get; set; }
        public abstract class employeeName : BqlString.Field<employeeName> { }
        #endregion

        #region PayeeName
        [PXString]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.PayeeName, Enabled = false)]
        [PXFormula(typeof(Selector<payeeID, EPEmployee.acctName>))]
        public virtual string PayeeName { get; set; }
        public abstract class payeeName : BqlString.Field<payeeName> { }
        #endregion

        #region IsActive
        [PXDBBool()]
        [PXDefault(false, PersistingCheck = PXPersistingCheck.Nothing)]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.IsActive)]
        public virtual bool? IsActive { get; set; }
        public abstract class isActive : BqlBool.Field<isActive> { }
        #endregion

        #region DepartmentID
        [PXString()]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.DepartmentID, Visible = false)]
        [PXUnboundDefault(typeof(Search<
            EPEmployee.departmentID,
            Where<EPEmployee.bAccountID, Equal<Current<custodianID>>>>))]
        [PXFormula(typeof(Default<custodianID>))]
        public virtual string DepartmentID { get; set; }
        public abstract class departmentID : BqlString.Field<departmentID> { }
        #endregion

        #region EstablishmentStatus
        [PXString]
        [APDocStatus.List()]
        [PXUnboundDefault(typeof(Search<
            APInvoice.status,
            Where<APInvoice.refNbr, Equal<Current<establishmentRefNbr>>>>), PersistingCheck = PXPersistingCheck.Nothing)]
        [PXFormula(typeof(Default<establishmentRefNbr>))]
        public virtual string EstablishmentStatus { get; set; }
        public abstract class establishmentStatus : BqlString.Field<establishmentStatus> { }
        #endregion



        #endregion

        // ==================================================
        // SUBJECT FOR REMOVAL.
        // ==================================================

        #region Unused fields
        #region Active
        [PXDBBool]
        [PXDefault(true, PersistingCheck = PXPersistingCheck.Nothing)]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.Active, Visible = false)]
        public virtual bool? Active { get; set; }
        public abstract class active : BqlBool.Field<active> { }
        #endregion

        #region CreditAccountID
        //[PXSelector(
        //    typeof(Search<Account.accountID>),
        //    typeof(Account.accountCD),
        //    typeof(Account.description),
        //    SubstituteKey = typeof(Account.accountCD))]
        [Account(DisplayName = Messages.ATPTEFMMessages.CreditAccountID, PersistingCheck = PXPersistingCheck.Nothing, Visible = false)]
        public int? CreditAccountID { get; set; }

        public abstract class creditAccountID : BqlInt.Field<creditAccountID> { }
        #endregion

        #region Closed
        [PXDBBool]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.Closed)]
        [PXDefault(false, PersistingCheck = PXPersistingCheck.Nothing)]
        public virtual bool? Closed { get; set; }
        public abstract class closed : BqlBool.Field<closed> { }
        #endregion
        #endregion

        #region Balance Summary
        #region CuryFundAmt
        [PXDBCurrency(typeof(ATPTEFMFund.curyInfoID), typeof(ATPTEFMFund.fundAmt))]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.Amount, IsReadOnly = true)]
        [PXDefault(TypeCode.Decimal, "0.0", PersistingCheck = PXPersistingCheck.Nothing)]
        public virtual decimal? CuryFundAmt { get; set; }
        public abstract class curyFundAmt : BqlDecimal.Field<curyFundAmt> { }
        #endregion

        #region FundAmt
        [PXDBBaseCury()]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.Amount, IsReadOnly = true)]
        [PXDefault(TypeCode.Decimal, "0.0", PersistingCheck = PXPersistingCheck.Nothing)]
        public virtual decimal? FundAmt { get; set; }
        public abstract class fundAmt : BqlDecimal.Field<fundAmt> { }
        #endregion

        #region CuryBalanceAmt
        [PXDBCurrency(typeof(ATPTEFMFund.curyInfoID), typeof(ATPTEFMFund.balanceAmt))]
        [PXDefault(TypeCode.Decimal, "0.0", PersistingCheck = PXPersistingCheck.Nothing)]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.OnHandBalanceAmount, Enabled = false)]
        public virtual decimal? CuryBalanceAmt { get; set; }
        public abstract class curyBalanceAmt : BqlDecimal.Field<curyBalanceAmt> { }
        #endregion

        #region BalanceAmt
        [PXDBBaseCury()]
        [PXDefault(TypeCode.Decimal, "0.0", PersistingCheck = PXPersistingCheck.Nothing)]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.OnHandBalanceAmount, Enabled = false)]
        public virtual decimal? BalanceAmt { get; set; }
        public abstract class balanceAmt : BqlDecimal.Field<balanceAmt> { }
        #endregion

        #region CuryLiquidatedAmt
        [PXDBCurrency(typeof(ATPTEFMFund.curyInfoID), typeof(ATPTEFMFund.liquidatedAmt))]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.Liquidated, Enabled = false)]
        [PXDefault(TypeCode.Decimal, "0.0", PersistingCheck = PXPersistingCheck.Nothing)]
        public virtual decimal? CuryLiquidatedAmt { get; set; }
        public abstract class curyLiquidatedAmt : BqlDecimal.Field<curyLiquidatedAmt> { }
        #endregion

        #region LiquidatedAmt
        [PXDBBaseCury()]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.Liquidated, Enabled = false)]
        [PXDefault(TypeCode.Decimal, "0.0", PersistingCheck = PXPersistingCheck.Nothing)]
        public virtual decimal? LiquidatedAmt { get; set; }
        public abstract class liquidatedAmt : BqlDecimal.Field<liquidatedAmt> { }
        #endregion

        #region CuryUnliquidatedAmt
        [PXDBCurrency(typeof(ATPTEFMFund.curyInfoID), typeof(ATPTEFMFund.unliquidatedAmt))]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.Unliquidated, Enabled = false)]
        [PXDefault(TypeCode.Decimal, "0.0", PersistingCheck = PXPersistingCheck.Nothing)]
        public virtual decimal? CuryUnliquidatedAmt { get; set; }
        public abstract class curyUnliquidatedAmt : BqlDecimal.Field<curyUnliquidatedAmt> { }
        #endregion

        #region UnliquidatedAmt
        [PXDBBaseCury()]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.Unliquidated, Enabled = false)]
        [PXDefault(TypeCode.Decimal, "0.0", PersistingCheck = PXPersistingCheck.Nothing)]
        public virtual decimal? UnliquidatedAmt { get; set; }
        public abstract class unliquidatedAmt : BqlDecimal.Field<unliquidatedAmt> { }
        #endregion

        #region CuryOnReplenishmentAmt
        [PXDBCurrency(typeof(ATPTEFMFund.curyInfoID), typeof(ATPTEFMFund.onReplenishmentAmt))]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.OnReplenishmentAmt, Enabled = false)]
        [PXDefault(TypeCode.Decimal, "0.0", PersistingCheck = PXPersistingCheck.Nothing)]
        public virtual decimal? CuryOnReplenishmentAmt { get; set; }
        public abstract class curyOnReplenishmentAmt : BqlDecimal.Field<curyOnReplenishmentAmt> { }
        #endregion

        #region OnReplenishmentAmt
        [PXDBBaseCury()]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.OnReplenishmentAmt, Enabled = false)]
        [PXDefault(TypeCode.Decimal, "0.0", PersistingCheck = PXPersistingCheck.Nothing)]
        public virtual decimal? OnReplenishmentAmt { get; set; }
        public abstract class onReplenishmentAmt : BqlDecimal.Field<onReplenishmentAmt> { }
        #endregion

        #endregion

        #endregion
    }
}
