using CashFundManagement.Attributes;
using CashFundManagement.BLC;
using CashFundManagement.DAC.Setup;
using PX.Data;
using PX.Data.BQL;
using PX.Data.EP;
using PX.Data.ReferentialIntegrity.Attributes;
using PX.Objects.CR;
using PX.Objects.CS;
using PX.Objects.EP;
using PX.Objects.GL;
using PX.Objects.TX;
using PX.SM;
using PX.TM;
using System;

namespace CashFundManagement.DAC {
    /// <remarks>
    /// 010204 - CFM 2024R1 - Replenishment Screen [Fund ID Field]
    /// </remarks>
    [PXPrimaryGraph(typeof(ATPTEFMReplenishmentEntry))]
    [Serializable]
    [PXCacheName(Messages.ATPTEFMMessages.ATPTEFMReplenishment)]
    public class ATPTEFMReplenishment : Base.ATPTEFMAudit, IBqlTable, IAssign
    {
        #region Keys    
        public class PK : PrimaryKeyOf<ATPTEFMReplenishment>.By<replenishmentNbr>
        {
            public static ATPTEFMReplenishment Find(PXGraph graph, string replenishmentNbr) => FindBy(graph, replenishmentNbr);
        }
        #endregion

        #region FundType
        [PXDBString(1, IsFixed = true)]
        [ATPTEFMFundTypeAttribute.ATPTEFMFundType]
        [PXDefault(ATPTEFMFundTypeAttribute.PettyCash)]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.FundType)]
        public virtual string FundType { get; set; }
        public abstract class fundType : BqlString.Field<fundType> { }
        #endregion

        #region ClaimAmount
        [PXDBDecimal(2)]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.ClaimAmount, Enabled = false)]
        [PXDefault(TypeCode.Decimal, "0.0", PersistingCheck = PXPersistingCheck.Nothing)]
        public virtual decimal? ClaimAmount { get; set; }
        public abstract class claimAmount : BqlDecimal.Field<claimAmount> { }
        #endregion

        #region WithholdingTaxAmount
        [PXDBDecimal(2)]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.WithholdingTax, Enabled = false)]
        [PXDefault(TypeCode.Decimal, "0.0", PersistingCheck = PXPersistingCheck.Nothing)]
        public virtual decimal? WithholdingTaxAmount { get; set; }
        public abstract class withholdingTaxAmount : BqlDecimal.Field<withholdingTaxAmount> { }
        #endregion

        #region VatAmount
        [PXDBDecimal(2)]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.VATAmount, Enabled = false)]
        [PXDefault(TypeCode.Decimal, "0.0", PersistingCheck = PXPersistingCheck.Nothing)]
        public virtual decimal? VatAmount { get; set; }
        public abstract class vatAmount : BqlDecimal.Field<vatAmount> { }
        #endregion

        #region OverrideTax
        [PXDBBool]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.OverrideTax)]
        [PXDefault(false, PersistingCheck = PXPersistingCheck.Nothing)]
        public virtual bool? OverrideTax { get; set; }
        public abstract class overrideTax : BqlBool.Field<overrideTax> { }
        #endregion

        #region ReplenishmentNbr                
        [PXDBString(15, IsKey = true, InputMask = ">CCCCCCCCCCCCCCC", IsUnicode = true)]
        [PXDefault()]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.ReplenishmentNbr)]
        [PXSelector(typeof(replenishmentNbr),
            typeof(branchID),
            typeof(date),
            typeof(replenishmentNbr),
            typeof(fundID),
            typeof(custodianID),
            typeof(departmentID),
            typeof(descr),
            typeof(claimAmount))]
        [AutoNumber(typeof(ATPTEFMSetup.replenishmentNumberingID), typeof(AccessInfo.businessDate))]
        public virtual string ReplenishmentNbr { get; set; }
        public abstract class replenishmentNbr : BqlString.Field<replenishmentNbr> { }
        #endregion

        #region FundID
        [PXDBString(IsUnicode = true)]
        [PXDBDefault()]
        [PXSelector(typeof(Search2<
            ATPTEFMFund.fundCD,
            LeftJoin<EPEmployee, 
                On<ATPTEFMFund.custodianID, Equal<EPEmployee.bAccountID>>>,
            Where<ATPTEFMFund.fundType, Equal<Current<fundType>>,
                And<ATPTEFMFund.closed, NotEqual<boolTrue>,
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

        #region Status
        [PXDBString(IsFixed = true)]
        [ATPTEFMReplenishmentStatusAttribute.ATPTEFMReplenishmentStatus]
        [PXDefault(ATPTEFMReplenishmentStatusAttribute.HoldValue)]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.Status, Enabled = false)]
        public virtual string Status { get; set; }
        public abstract class status : BqlString.Field<status> { }
        #endregion

        #region Hold
        [PXDBBool]
        [PXDefault(true)]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.Hold)]
        [PXUIEnabled(typeof(Where<isReleased, Equal<False>>))]
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

        #region Descr
        [PXDBString(255, IsUnicode = true)]
        [PXDefault()]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.Descr)]
        public virtual string Descr { get; set; }
        public abstract class descr : BqlString.Field<descr> { }
        #endregion

        #region CustodianID
        [PXDBInt]
        [PXDefault(typeof(Selector<fundID, ATPTEFMFund.custodianID>))]
        [PXFormula(typeof(Default<fundID>))]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.CustodianID, IsReadOnly = true)]
        [PXSelector(typeof(Search<EPEmployee.bAccountID>), typeof(EPEmployee.bAccountID), typeof(EPEmployee.acctCD), typeof(EPEmployee.acctName), typeof(EPEmployee.departmentID), DescriptionField = typeof(EPEmployee.acctName), SubstituteKey = typeof(EPEmployee.acctCD))]
        public virtual int? CustodianID { get; set; }
        public abstract class custodianID : BqlInt.Field<custodianID> { }
        #endregion

        #region PayeeID
        [PXDBInt]
        [PXDefault(typeof(Selector<fundID, ATPTEFMFund.payeeID>))]
        [PXFormula(typeof(Default<fundID>))]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.PayeeID, IsReadOnly = true)]
        [PXSelector(typeof(Search<EPEmployee.bAccountID>), typeof(EPEmployee.bAccountID), typeof(EPEmployee.acctCD), typeof(EPEmployee.acctName), typeof(EPEmployee.departmentID), DescriptionField = typeof(EPEmployee.acctName), SubstituteKey = typeof(EPEmployee.acctCD))]
        public virtual int? PayeeID { get; set; }
        public abstract class payeeID : BqlInt.Field<payeeID> { }
        #endregion

        #region DepartmentID
        [PXDBString(IsUnicode = true)]
        [PXDefault(typeof(Selector<fundID, ATPTEFMFund.departmentID>))]
        [PXFormula(typeof(Default<fundID>))]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.DepartmentID, IsReadOnly = true)]
        [PXSelector(typeof(Search<EPDepartment.departmentID>), typeof(EPDepartment.departmentID), typeof(EPDepartment.description), DescriptionField = typeof(EPDepartment.description))]
        public virtual string DepartmentID { get; set; }
        public abstract class departmentID : BqlString.Field<departmentID> { }
        #endregion

        #region BranchID
        [Branch(typeof(AccessInfo.branchID), DisplayName = Messages.ATPTEFMMessages.Branch, Required = true)]
        [PXUIEnabled(typeof(False))]
        public virtual int? BranchID { get; set; }
        public abstract class branchID : BqlInt.Field<branchID> { }
        #endregion

        #region OwnerID
        [Owner(typeof(workgroupID), DisplayName = Messages.ATPTEFMMessages.OwnerID)]
        [PXDefault(typeof(Search<EPEmployee.defContactID, Where<EPEmployee.userID, Equal<Current<AccessInfo.userID>>>>), PersistingCheck = PXPersistingCheck.Nothing)]
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
        [PXSearchable(PX.Objects.SM.SearchCategory.AP, "Replenishment {0}: {1} - {3}", new Type[] { typeof(ATPTEFMReplenishment.replenishmentNbr), typeof(ATPTEFMReplenishment.fundID), typeof(ATPTEFMReplenishment.custodianID), typeof(EPEmployee.acctName) },
            new Type[] { typeof(ATPTEFMReplenishment.descr) },
            NumberFields = new Type[] { typeof(ATPTEFMReplenishment.replenishmentNbr) },
            Line1Format = "{0:d}{1}{2}", Line1Fields = new Type[] { typeof(ATPTEFMReplenishment.date), typeof(ATPTEFMReplenishment.status), typeof(EPEmployee.acctName) },
            Line2Format = "{0}", Line2Fields = new Type[] { typeof(ATPTEFMFundTransaction.descr) },
            MatchWithJoin = typeof(InnerJoin<EPEmployee, On<EPEmployee.bAccountID, Equal<ATPTEFMReplenishment.custodianID>>>),
            SelectForFastIndexing = typeof(Select2<ATPTEFMReplenishment, InnerJoin<EPEmployee, On<ATPTEFMReplenishment.custodianID, Equal<EPEmployee.bAccountID>>>>)
        )]
        public virtual Guid? NoteID { get; set; }
        public abstract class noteID : BqlGuid.Field<noteID> { }
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

        #region Unbound

        #region RequestApproval
        [PXBool]
        //[PXDefault(typeof(Search<ATPTEFMSetup.replenishmentRequestApproval>), PersistingCheck = PXPersistingCheck.Nothing)]
        //[PXUnboundDefault(typeof(Current<ATPTEFMSetup.replenishmentRequestApproval>), PersistingCheck = PXPersistingCheck.Null)]
        [PXUnboundDefault(typeof(Search<ATPTEFMSetup.replenishmentRequestApproval>), PersistingCheck = PXPersistingCheck.Null)]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.RequestApproval, Visible = false)]
        public virtual bool? RequestApproval { get; set; }

        public abstract class requestApproval : BqlBool.Field<requestApproval> { }
        #endregion

        #region ApprovalWorkgroupID
        public abstract class approvalWorkgroupID : BqlInt.Field<approvalWorkgroupID>
        {
        }
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

        #region EmployeeID
        public abstract class employeeID : PX.Data.BQL.BqlInt.Field<employeeID> { }

        [PXInt]
        [PXUIField(DisplayName = "Employee")]
        [PXDefault(typeof(Search<EPEmployee.bAccountID, Where<EPEmployee.userID, Equal<Current<AccessInfo.userID>>>>), PersistingCheck = PXPersistingCheck.Nothing)]
        [PXSubordinateAndWingmenSelector()]
        [PXFieldDescription]
        public virtual Int32? EmployeeID
        {
            get;
            set;
        }

        #endregion

        #endregion

        #region Step
        [PXDBString(IsFixed = true)]
        [ATPTEFMFundTransactionStepAttribute.ATPTEFMFundTransactionStep]
        [PXDefault(ATPTEFMFundTransactionStepAttribute.DefaultValue)]
        public virtual string Step { get; set; }
        public abstract class step : PX.Data.BQL.BqlString.Field<step> { }
        #endregion

        #region InvoiceRefNbr
        [PXDBString(IsUnicode = true)]
        public virtual string InvoiceRefNbr { get; set; }
        public abstract class invoiceRefNbr : PX.Data.BQL.BqlString.Field<invoiceRefNbr> { }
        #endregion

        #region IsImported
        [PXDBBool()]
        [PXDefault(false, PersistingCheck = PXPersistingCheck.Nothing)]
        public virtual bool? IsImported { get; set; }
        public abstract class isImported : BqlBool.Field<isImported> { }
        #endregion

        #region TaxZone
        public abstract class taxZone : PX.Data.BQL.BqlString.Field<taxZone> { }
        [PXDBString(10, IsUnicode = true)]
        [PXDefault(
            typeof(Search2<Location.vTaxZoneID,
                InnerJoin<BAccount, On<BAccount.bAccountID, Equal<Location.bAccountID>,
                    And<BAccount.defLocationID, Equal<Location.locationID>>>,
                InnerJoin<EPEmployee, On<EPEmployee.bAccountID, Equal<BAccount.bAccountID>>>>,
                Where<EPEmployee.userID, Equal<Current<AccessInfo.userID>>>>),
            PersistingCheck = PXPersistingCheck.Nothing)]
        [PXUIField(DisplayName = "Tax Zone", Visibility = PXUIVisibility.Visible)]
        [PXSelector(typeof(TaxZone.taxZoneID), DescriptionField = typeof(TaxZone.descr), Filterable = true)]
        public virtual string TaxZone { get; set; }
        #endregion

        #region IsReleased
        [PXDBBool]
        [PXUIField(DisplayName = "")]
        [PXDefault(false, PersistingCheck = PXPersistingCheck.Nothing)]
        public virtual bool? IsReleased { get; set; }
        public abstract class isReleased : BqlBool.Field<isReleased> { }
        #endregion
    }
}