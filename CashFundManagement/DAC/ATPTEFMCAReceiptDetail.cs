using CashFundManagement.Attributes;
using CashFundManagement.DAC.Setup;
using CashFundManagement.Messages;
using PX.Data;
using PX.Data.BQL;
using PX.Data.ReferentialIntegrity.Attributes;
using PX.Objects.AP;
using PX.Objects.CM;
using PX.Objects.CN.ProjectAccounting.Descriptor;
using PX.Objects.CN.ProjectAccounting.PM.Descriptor;
using PX.Objects.Common;
using PX.Objects.CR;
using PX.Objects.CS;
using PX.Objects.GL;
using PX.Objects.IN;
using PX.Objects.PM;
using PX.Objects.TX;
using System;

namespace CashFundManagement.DAC {
    [PXPrimaryGraph(typeof(BLC.ATPTEFMCashAdvanceEntry))]
    [Serializable]
    [PXCacheName(Messages.ATPTEFMMessages.ATPTEFMCAReceiptDetail)]
    public class ATPTEFMCAReceiptDetail : Base.ATPTEFMAudit, IBqlTable
    {
        #region CashAdvanceReceiptDetailIID
        [PXDBIdentity(IsKey = true)]
        public virtual int? CashAdvanceReceiptDetailIID { get; set; }
        public abstract class cashAdvanceReceiptDetailIID : BqlInt.Field<cashAdvanceReceiptDetailIID> { }
        #endregion

        #region CashAdvanceNbr
        [PXDBString(15, IsUnicode = true)]
        [PXDBDefault(typeof(ATPTEFMCashAdvance.cashAdvanceNbr))]
        [PXParent(typeof(Select<
            ATPTEFMCashAdvance,
            Where<ATPTEFMCashAdvance.cashAdvanceNbr, Equal<Current<cashAdvanceNbr>>>>))]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.CashAdvanceNbrRqstID)]
        public virtual string CashAdvanceNbr { get; set; }
        public abstract class cashAdvanceNbr : BqlString.Field<cashAdvanceNbr> { }
        #endregion

        #region CashAdvanceRequestDetailID
        [PXDBInt()]
        [PXDefault(TypeCode.Int32, "0", PersistingCheck = PXPersistingCheck.Null)]
        public virtual int? CashAdvanceRequestDetailID { get; set; }
        public abstract class cashAdvanceRequestDetailID : BqlInt.Field<cashAdvanceRequestDetailID> { }
        #endregion

        #region Date
        [PXDBDate]
        [PXDefault(typeof(AccessInfo.businessDate))]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.Date)]
        public virtual DateTime? Date { get; set; }
        public abstract class date : BqlDateTime.Field<date> { }
        #endregion

        #region RefNbr
        [PXDBString(40, IsUnicode = true)]
        [PXDefault()]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.RefNbr)]
        [PXUIRequired(typeof(Where<GetSetupValue<ATPTEFMCASetup.requireExtRef>, Equal<boolTrue>>))]
        [ATPTEFMDuplicateORNbr]
        public virtual string RefNbr { get; set; }
        public abstract class refNbr : BqlString.Field<refNbr> { }
        #endregion

        #region VendorID
        [PXDBInt()]
        [PXDefault(PersistingCheck = PXPersistingCheck.Nothing)]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.VendorID)]
        [PXSelector(typeof(Search2<
            Vendor.bAccountID,
            LeftJoin<Location, 
                On<Location.bAccountID, Equal<Vendor.bAccountID>,
                And<Vendor.defLocationID, Equal<Location.locationID>>>>>),
            typeof(Vendor.bAccountID), typeof(Vendor.acctCD), typeof(Vendor.acctName), SubstituteKey = typeof(Vendor.acctCD),
            ValidateValue = false)]
        public virtual int? VendorID { get; set; }
        public abstract class vendorID : BqlInt.Field<vendorID> { }
        #endregion

        #region VendID
        [ATPTEFMVendorSelector]
        [PXDBDefault(PersistingCheck = PXPersistingCheck.Nothing)]
        public virtual int? VendID { get; set; }
        public abstract class vendID : BqlInt.Field<vendID> { }
        #endregion

        #region OldVendID
        [PXDBInt]
        [PXDefault(PersistingCheck = PXPersistingCheck.Nothing)]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.VendorID)]
        [PXSelector(typeof(Search2<
            Vendor.bAccountID,
            LeftJoin<Location,
                On<Location.bAccountID, Equal<Vendor.bAccountID>,
                And<Vendor.defLocationID, Equal<Location.locationID>>>>>),
            typeof(Vendor.bAccountID), typeof(Vendor.acctCD), typeof(Vendor.acctName),
            SubstituteKey = typeof(Vendor.acctCD),
            DescriptionField = typeof(Vendor.acctName), ValidateValue = false)]
        public virtual int? OldVendID { get; set; }
        public abstract class oldVendID : BqlInt.Field<oldVendID> { }
        #endregion

        #region VendorName
        [PXDBString(100, IsUnicode = true)]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.UsrVendName)]
        [PXUIRequired(typeof(Where<GetSetupValue<ATPTEFMCASetup.requireVendorDetails>, Equal<True>>))]
        [PXDefault(PersistingCheck = PXPersistingCheck.Nothing)]
        public virtual string VendorName { get; set; }
        public abstract class vendorName : BqlString.Field<vendorName> { }
        #endregion

        #region VendorAddress
        [PXDBString(250, IsUnicode = true)]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.UsrAddress)]
        [PXUIRequired(typeof(Where<GetSetupValue<ATPTEFMCASetup.requireVendorDetails>, Equal<True>>))]
        [PXDefault(PersistingCheck = PXPersistingCheck.Nothing)]
        public virtual string VendorAddress { get; set; }
        public abstract class vendorAddress : BqlString.Field<vendorAddress> { }
        #endregion

        #region VendorTin
        [PXDBString(50, IsUnicode = true)]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.UsrVendTin)]
        [PXUIRequired(typeof(Where<GetSetupValue<ATPTEFMCASetup.requireVendorDetails>, Equal<True>>))]
        [PXDefault(PersistingCheck = PXPersistingCheck.Nothing)]
        public virtual string VendorTin { get; set; }
        public abstract class vendorTin : BqlString.Field<vendorTin> { }
        #endregion

        #region TaxZoneID
        [PXDBString(10, IsUnicode =true)]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.TaxZone, Required = true)]
        [PXSelector(typeof(Search<TaxZone.taxZoneID>), DescriptionField = typeof(TaxZone.descr))]
        [PXDefault()]
        /*[PXFormula(typeof(Default<vendorID>))]
        [PXDefault(typeof(Search2<Location.vTaxZoneID,
            LeftJoin<Vendor, On<Location.bAccountID, Equal<Vendor.bAccountID>,
                And<Vendor.defLocationID, Equal<Location.locationID>>>>,
            Where<Vendor.bAccountID, Equal<Current<vendorID>>>>))]*/
        public virtual String TaxZoneID { get; set; }
        public abstract class taxZoneID : BqlString.Field<taxZoneID> { }
        #endregion

        #region TaxCategoryID
        [PXDBString(TaxCategory.taxCategoryID.Length, IsUnicode = true)]
        [PXUIField(DisplayName = "Tax Category", Visibility = PXUIVisibility.Visible)]
        [PXSelector(typeof(TaxCategory.taxCategoryID), DescriptionField = typeof(TaxCategory.descr))]
        [PXRestrictor(typeof(Where<TaxCategory.active, Equal<True>>), ATPTEFMMessages.InactiveTaxCategory, typeof(TaxCategory.taxCategoryID))]
        [PXFormula(typeof(Selector<inventoryID, InventoryItem.taxCategoryID>))]
        public virtual string TaxCategoryID { get; set; }
        public abstract class taxCategoryID : PX.Data.BQL.BqlString.Field<taxCategoryID> { }
        #endregion

        #region ATCCode
        [PXDBString(30, IsUnicode = true)]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.AtcCode)]
        [PXSelector(typeof(Search<Tax.taxID, Where<Tax.taxType, Equal<CSTaxType.withholding>>>),
            typeof(Tax.taxID),
            typeof(Tax.descr),
            DescriptionField = typeof(Tax.descr), Filterable = true)]
        public string AtcCode { get; set; }
        public abstract class atcCode : PX.Data.BQL.BqlString.Field<atcCode> { }
        #endregion

        #region CuryNetAmt
        [PXDBCurrency(typeof(ATPTEFMCashAdvance.curyInfoID), typeof(ATPTEFMCAReceiptDetail.netAmt))]
        [PXDefault(TypeCode.Decimal, "0.0", PersistingCheck = PXPersistingCheck.Null)]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.NetAmt, IsReadOnly = true)]
        [PXFormula(typeof(Default<netQty, curyNetUnitCost>))]
        [PXFormula(typeof(Mult<netQty, curyNetUnitCost>))]
        public virtual decimal? CuryNetAmt { get; set; }
        public abstract class curyNetAmt : PX.Data.BQL.BqlDecimal.Field<curyNetAmt> { }
        #endregion

        #region NetAmt
        [PXDBBaseCury()]
        [PXDefault(TypeCode.Decimal, "0.0", PersistingCheck = PXPersistingCheck.Null)]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.NetAmt, IsReadOnly = true)]
        //[PXFormula(typeof(Default<netQty, netUnitCost>))]
        //[PXFormula(typeof(Mult<netQty, netUnitCost>), typeof(SumCalc<ATPTEFMCashAdvance.curyActualSpentAmount>))]
        public virtual decimal? NetAmt { get; set; }
        public abstract class netAmt : BqlDecimal.Field<netAmt> { }
        #endregion

        #region ExpenseReceiptRefNbr
        [PXDBString(15, IsUnicode =true)]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.ExpenseReceiptRefNbr, IsReadOnly = true)]
        public virtual string ExpenseReceiptRefNbr { get; set; }
        public abstract class expenseReceiptRefNbr : BqlString.Field<expenseReceiptRefNbr> { }
        #endregion

        #region NetQty
        [PXDBDecimal(2)]
        [PXDefault(TypeCode.Decimal, "0.0", PersistingCheck = PXPersistingCheck.NullOrBlank)]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.NetQty, Required = true)]
        public virtual decimal? NetQty { get; set; }
        public abstract class netQty : BqlDecimal.Field<netQty> { }
        #endregion

        #region NetUnitCost
        [PXDBBaseCury()]
        [PXDefault(TypeCode.Decimal, "0.0", PersistingCheck = PXPersistingCheck.Null)]
        [PXUIField(DisplayName = ATPTEFMMessages.NetUnitCost)]
        public virtual decimal? NetUnitCost { get; set; }
        public abstract class netUnitCost : BqlDecimal.Field<netUnitCost> { }
        #endregion

        #region CuryNetUnitCost
        [PXDBCurrency(typeof(ATPTEFMCashAdvance.curyInfoID), typeof(ATPTEFMCAReceiptDetail.netUnitCost))]
        [PXDefault(TypeCode.Decimal, "0.0", PersistingCheck = PXPersistingCheck.NullOrBlank)]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.NetUnitCost, Required = true)]
        public virtual decimal? CuryNetUnitCost { get; set; }
        public abstract class curyNetUnitCost : BqlDecimal.Field<curyNetUnitCost> { }
        #endregion

        #region AccountID
        [Account(DisplayName = Messages.ATPTEFMMessages.AccountID)]
        [PXDefault()]
        public int? AccountID { get; set; }
        public abstract class accountID : BqlInt.Field<accountID> { }
        #endregion

        #region Account Description
        [PXString(255)]
        [PXUIField(DisplayName = "Account Description", Enabled = false)]
        [PXUnboundDefault(typeof(Selector<accountID, Account.description>), PersistingCheck = PXPersistingCheck.Nothing)]
        [PXFormula(typeof(Default<accountID>))]
        public virtual string AccountDescription { get; set; }
        public abstract class accountDescription : PX.Data.BQL.BqlString.Field<accountDescription> { }
        #endregion

        #region Account Group
        [PXInt]
        [PXUnboundDefault(typeof(Selector<accountID, Account.accountGroupID>), PersistingCheck = PXPersistingCheck.Nothing)]
        [PXSelector(typeof(Search<PMAccountGroup.groupID>), SubstituteKey = typeof(PMAccountGroup.groupCD))]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.AccountGroup, Enabled = false)]
        [PXFormula(typeof(Default<accountID>))]
        public int? AccountGroup { get; set; }
        public abstract class accountGroup : PX.Data.BQL.BqlInt.Field<accountGroup> { }
        #endregion

        #region SubID
        [SubAccount(typeof(accountID), DisplayName = Messages.ATPTEFMMessages.Subaccount)]
        [PXDefault()]
        public int? SubID { get; set; }
        public abstract class subID : BqlInt.Field<subID> { }
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
        [PXSelector(typeof(Search<PMTask.taskID, Where<PMTask.projectID, Equal<Current<projectID>>>>), typeof(PMTask.taskCD), typeof(PMTask.description), SubstituteKey = typeof(PMTask.taskCD))]
        [PXUIRequired(typeof(Where<projectID, NotEqual<NonProject>>))]
        [PXUIVisible(typeof(FeatureInstalled<FeaturesSet.projectAccounting>))]
        [PXRestrictor(typeof(Where<PMTask.type, NotEqual<ProjectTaskType.revenue>>),
            ProjectAccountingMessages.TaskTypeIsNotAvailable)]
        public virtual int? ProjectTaskID { get; set; }
        public abstract class projectTaskID : BqlInt.Field<projectTaskID> { }
        #endregion

        #region CostCodeID
        [PXForeignReference(typeof(Field<costCodeID>.IsRelatedTo<PMCostCode.costCodeID>))]
        [CostCode(typeof(accountID), typeof(projectTaskID), PX.Objects.GL.AccountType.Expense, ProjectField = typeof(projectID),
            Visibility = PXUIVisibility.SelectorVisible, DescriptionField = typeof(PMCostCode.description))]
        [PXUIVisible(typeof(FeatureInstalled<FeaturesSet.costCodes>))]
        public virtual Int32? CostCodeID { get; set; }
        public abstract class costCodeID : PX.Data.BQL.BqlInt.Field<costCodeID> { }
        #endregion

        #region Remarks
        [PXDBString(500, IsUnicode = true)]
        [PXUIField(DisplayName = "Remarks")]
        public virtual string Remarks { get; set; }
        public abstract class remarks : BqlString.Field<remarks> { }
        #endregion

        #region LiquidationRef
        [PXDBString(20, IsUnicode = true)]
        [PXDefault(PersistingCheck = PXPersistingCheck.Nothing)]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.LiqRef, Enabled =false, IsReadOnly =true)]
        public virtual string LiquidationRef { get; set; }
        public abstract class liquidationRef : PX.Data.BQL.BqlString.Field<liquidationRef> { }
        #endregion

        #region Reversed
        [PXDBBool]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.Reversed, Enabled = false)]
        [PXDefault(false, PersistingCheck = PXPersistingCheck.Nothing)]
        public virtual bool? Reversed { get; set; }
        public abstract class reversed : PX.Data.BQL.BqlBool.Field<reversed> { }
        #endregion

        #region NoteID

        public abstract class noteID : PX.Data.BQL.BqlGuid.Field<noteID> { }
        protected Guid? _NoteID;
        [PXNote()]
        public virtual Guid? NoteID
        {
            get
            {
                return this._NoteID;
            }
            set
            {
                this._NoteID = value;
            }
        }
        #endregion

        #region InventoryID
        [PXDBInt]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.InventoryID)]
        [ATPTEFMCARequestClassItemSelector(typeof(ATPTEFMCashAdvance), typeof(ATPTEFMCashAdvance.reqClassID), typeof(Null), typeof(Null), typeof(Null))]
        [PXDefault(typeof(Search<ATPTEFMCARequestDetail.inventoryID, Where<ATPTEFMCARequestDetail.cashAdvanceRequestDetailID, Equal<Current<cashAdvanceRequestDetailID>>>>))]
        public virtual int? InventoryID { get; set; }
        public abstract class inventoryID : BqlInt.Field<inventoryID> { }
        #endregion

        #region IsImported
        [PXDBBool()]
        [PXDefault(false, PersistingCheck = PXPersistingCheck.Nothing)]
        public virtual bool? IsImported { get; set; }
        public abstract class isImported : PX.Data.BQL.BqlBool.Field<isImported> { }
        #endregion

        #region LineDescription
        [PXDBString(256, IsUnicode = true)]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.LineDescription)]
        [PXDefault(typeof(Switch<Case<Where<GetSetupValue<ATPTEFMCASetup.setNonStockItemDescriptionAsDefault>, Equal<True>>, Selector<inventoryID, InventoryItem.descr>>, Null>),
            PersistingCheck = PXPersistingCheck.NullOrBlank)]
        [PXFormula(typeof(Default<inventoryID>))]
        public virtual string LineDescription { get; set; }
        public abstract class lineDescription : BqlString.Field<lineDescription> { }
        #endregion
    }
}