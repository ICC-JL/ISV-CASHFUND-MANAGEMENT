using CashFundManagement.Attributes;
using CashFundManagement.BLC;
using CashFundManagement.DAC.Setup;
using CashFundManagement.Messages;
using PX.Data;
using PX.Data.BQL;
using PX.Data.ReferentialIntegrity.Attributes;
using PX.Objects.AP;
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

//TODO : Remove dead codes on next upgrade.
namespace CashFundManagement.DAC {
    [PXPrimaryGraph(typeof(ATPTEFMFundTransactionEntry))]
    [Serializable]
    [PXCacheName(Messages.ATPTEFMMessages.ATPTEFMFundTransactionReceiptDetail)]
    public class ATPTEFMFundTransactionReceiptDetail : Base.ATPTEFMAudit, IBqlTable
    {
        #region FundTransactionReceiptDetailID
        [PXDBLongIdentity(IsKey = true)]
        public virtual long? FundTransactionReceiptDetailID { get; set; }
        public abstract class fundTransactionReceiptDetailID : BqlLong.Field<fundTransactionReceiptDetailID> { }
        #endregion

        #region FundTransactionRefNbr
        [PXDBString(15, IsUnicode = true)]
        [PXDBDefault(typeof(ATPTEFMFundTransaction.refNbr))]
        [PXParent(typeof(Select<
            ATPTEFMFundTransaction,
            Where<ATPTEFMFundTransaction.refNbr, Equal<Current<ATPTEFMFundTransactionReceiptDetail.fundTransactionRefNbr>>>>))]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.FundTransactionRefNbr)]
        public virtual string FundTransactionRefNbr { get; set; }
        public abstract class fundTransactionRefNbr : BqlString.Field<fundTransactionRefNbr> { }
        #endregion 

        #region Date
        [PXDBDate]
        [PXDefault(typeof(AccessInfo.businessDate))]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.Date)]
        public virtual DateTime? Date { get; set; }
        public abstract class date : BqlDateTime.Field<date> { }
        #endregion

        #region Descr
        [PXDBString(256, IsUnicode = true)]
        [PXDefault(typeof(Selector<inventoryID, InventoryItem.descr>), PersistingCheck = PXPersistingCheck.Nothing)]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.Descr, IsReadOnly = true)]
        [PXFormula(typeof(Default<inventoryID>))]
        public virtual string Descr { get; set; }
        public abstract class descr : BqlString.Field<descr> { }
        #endregion

        #region Qty
        [PXDBDecimal(2)]
        [PXDefault(TypeCode.Decimal, "0.0", PersistingCheck = PXPersistingCheck.Null)]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.Qty)]
        [PXUIEnabled(typeof(Where<Current<ATPTEFMFundTransaction.fundTransactionType>, Equal<ATPTEFMFundTransactionTypeAttribute.reimbursementValue>>))]
        public virtual decimal? Qty { get; set; }
        public abstract class qty : BqlDecimal.Field<qty> { }
        #endregion

        #region InventoryID
        [PXDBInt()]
        [PXDBDefault()]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.InventoryID)]
        [PXSelector(typeof(Search<InventoryItem.inventoryID, Where<InventoryItem.itemStatus, Equal<InventoryItemStatus.active>>>), typeof(InventoryItem.inventoryID), typeof(InventoryItem.inventoryCD), typeof(InventoryItem.descr), SubstituteKey = typeof(InventoryItem.inventoryCD), DescriptionField = typeof(InventoryItem.descr))]
        //[PXUIEnabled(typeof(Where<Current<ATPTEFMFundTransaction.fundTransactionType>, Equal<ATPTEFMFundTransactionTypeAttribute.reimbursementValue>>))]
        [PXRestrictor(typeof(Where<InventoryItem.itemType, Equal<INItemTypes.expenseItem>>), null, ShowWarning = true)]
        public virtual int? InventoryID { get; set; }
        public abstract class inventoryID : BqlInt.Field<inventoryID> { }
        #endregion

        #region UnitCost
        [PXDBDecimal(2)]
        [PXDefault(TypeCode.Decimal, "0.0", typeof(Selector<inventoryID, InventoryItemCurySettings.basePrice>))]
        [PXFormula(typeof(Default<inventoryID>))]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.UnitCost)]
        [PXUIEnabled(typeof(Where<Current<ATPTEFMFundTransaction.fundTransactionType>, Equal<ATPTEFMFundTransactionTypeAttribute.reimbursementValue>>))]
        public virtual decimal? UnitCost { get; set; }
        public abstract class unitCost : BqlDecimal.Field<unitCost> { }
        #endregion

        #region Amount
        [PXDBDecimal(2)]
        [PXDefault(TypeCode.Decimal, "0.0", PersistingCheck = PXPersistingCheck.Null)]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.Amount, IsReadOnly = true)]
        [PXFormula(typeof(Default<fundTransactionRefNbr>))]
        [PXFormula(typeof(Mult<qty, unitCost>), typeof(SumCalc<ATPTEFMFundTransaction.totalReceipt>))]
        public virtual decimal? Amount { get; set; }
        public abstract class amount : BqlDecimal.Field<amount> { }
        #endregion

        #region AccountID
        //[PXSelector(
        //    typeof(Search<Account.accountID>),
        //    typeof(Account.accountCD),
        //    typeof(Account.description),
        //    SubstituteKey = typeof(Account.accountCD))]
        [Account(DisplayName = Messages.ATPTEFMMessages.AccountID, Enabled = false)]
        [PXDefault(typeof(Selector<inventoryID, InventoryItem.cOGSAcctID>))]
        [PXFormula(typeof(Default<inventoryID>))]
        public int? AccountID { get; set; }
        public abstract class accountID : BqlInt.Field<accountID> { }
        #endregion

        #region Account Description
        [PXString(255)]
        [PXUIField(DisplayName = "Account Description", IsReadOnly = true)]
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
        //[PXSelector(
        //    typeof(Search<Sub.subID>),
        //    typeof(Sub.subCD),
        //    typeof(Sub.description),
        //    SubstituteKey = typeof(Sub.subCD))]
        [SubAccount(typeof(accountID), DisplayName = Messages.ATPTEFMMessages.Subid)]
        [PXDefault(typeof(Search<InventoryItem.cOGSSubID,
                            Where<InventoryItem.inventoryID,
                                Equal<Current<inventoryID>>>>))]
        [PXFormula(typeof(Default<inventoryID>))]
        //[PXUIEnabled(typeof(Where<Current<ATPTEFMFundTransaction.fundTransactionType>, Equal<ATPTEFMFundTransactionTypeAttribute.reimbursementValue>>))]
        public int? SubID { get; set; }
        public abstract class subID : BqlInt.Field<subID> { }
        #endregion

        #region RefNbr
        [PXDBString(40, IsUnicode = true)]
        [PXDefault(PersistingCheck = PXPersistingCheck.Nothing)]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.RefNbr)]
        [PXUIRequired(typeof(Where<GetSetupValue<ATPTEFMSetup.requireExternalReferenceNbr>, Equal<True>>))]
        [ATPTEFMDuplicateORNbr]
        public virtual string RefNbr { get; set; }
        public abstract class refNbr : BqlString.Field<refNbr> { }
        #endregion

        #region VendorID
        [PXDBInt()]
        [PXDBDefault(PersistingCheck = PXPersistingCheck.Nothing)]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.VendorID)]
        [PXSelector(typeof(Search2<Vendor.bAccountID,
            LeftJoin<LocationExtAddress, On<LocationExtAddress.bAccountID, Equal<Vendor.bAccountID>>>>),
            typeof(Vendor.bAccountID), typeof(Vendor.acctCD), typeof(Vendor.acctName), SubstituteKey = typeof(Vendor.acctCD))]
        public virtual int? VendorID { get; set; }
        public abstract class vendorID : BqlInt.Field<vendorID> { }
        #endregion

        #region VendID
        [ATPTEFMVendorSelector]
        [PXDBDefault(PersistingCheck = PXPersistingCheck.Nothing)]
        public virtual int? VendID { get; set; }
        public abstract class vendID : BqlInt.Field<vendID> { }
        #endregion

        #region VendorName
        [PXDBString(100, IsUnicode = true)]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.UsrVendName)]
        [PXUIRequired(typeof(Where<GetSetupValue<ATPTEFMSetup.requireVendorDetails>, Equal<True>>))]
        [PXDefault(PersistingCheck = PXPersistingCheck.Nothing)]
        public virtual string VendorName { get; set; }
        public abstract class vendorName : PX.Data.IBqlField { }
        #endregion

        #region VendorAddress
        [PXDBString(250, IsUnicode = true)]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.UsrAddress)]
        [PXUIRequired(typeof(Where<GetSetupValue<ATPTEFMSetup.requireVendorDetails>, Equal<True>>))]
        [PXDefault(PersistingCheck = PXPersistingCheck.Nothing)]
        public virtual string VendorAddress { get; set; }
        public abstract class vendorAddress : PX.Data.IBqlField { }
        #endregion

        #region VendorTin
        [PXDBString(50, IsUnicode = true)]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.UsrVendTin)]
        [PXUIRequired(typeof(Where<GetSetupValue<ATPTEFMSetup.requireVendorDetails>, Equal<True>>))]
        [PXDefault(PersistingCheck = PXPersistingCheck.Nothing)]
        public virtual string VendorTin { get; set; }
        public abstract class vendorTin : PX.Data.IBqlField { }
        #endregion

        #region TaxZoneID
        [PXDBString(10, IsUnicode = true)]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.TaxZoneID)]
        [PXSelector(typeof(Search<TaxZone.taxZoneID>), DescriptionField = typeof(TaxZone.descr))]
        [PXFormula(typeof(Default<vendorID>))]
        [PXDefault(typeof(Selector<vendorID, LocationExtAddress.vTaxZoneID>))]
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

        #region ExpenseReceiptRefNbr
        [PXDBString(15, IsUnicode = true)]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.ExpenseReceiptRefNbr, IsReadOnly = true)]
        public virtual string ExpenseReceiptRefNbr { get; set; }
        public abstract class expenseReceiptRefNbr : BqlString.Field<expenseReceiptRefNbr> { }
        #endregion

        #region ReplenishmentRefNbr
        [PXDBString(IsUnicode = true)]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.ReplenishmentRefNbr, IsReadOnly = true)]
        public virtual string ReplenishmentRefNbr { get; set; }
        public abstract class replenishmentRefNbr : BqlString.Field<replenishmentRefNbr> { }
        #endregion

        #region FundTransactionDetailID
        [PXDBInt()]
        public virtual int? FundTransactionDetailID { get; set; }
        public abstract class fundTransactionDetailID : BqlInt.Field<fundTransactionDetailID> { }
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

        #region NetQty
        [PXDBDecimal(2)]
        [PXDefault(TypeCode.Decimal, "0.0")]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.NetQty, Visible = true, Required = true)]
        //[PXUIVerify(typeof(Where<netQty, LessEqual<decimal0>>),
        //                PXErrorLevel.Error, Messages.ATPTEFMMessages.QuantityMustBeGreaterThanZero,
        //                       CheckOnInserted = false)]
        [PXFormula(typeof(Validate<netQty, netUnitCost, netAmt>))]
        [PXUIEnabled(typeof(Where<Current<ATPTEFMFundTransaction.fundTransactionType>, Equal<ATPTEFMFundTransactionTypeAttribute.cashAdvanceValue>>))]
        //[PXUIVisible(typeof(Where<Current<ATPTEFMFundTransaction.fundTransactionType>, Equal<ATPTEFMFundTransactionTypeAttribute.cashAdvanceValue>>))]
        public virtual decimal? NetQty { get; set; }
        public abstract class netQty : BqlDecimal.Field<netQty> { }
        #endregion

        #region NetUnitCost
        [PXDBDecimal(2)]
        [PXDefault(TypeCode.Decimal, "0.0")]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.NetUnitCost, Visible = true, Required = true)]
        //[PXUIVerify(typeof(Where<netUnitCost, LessEqual<decimal0>>),
        //                PXErrorLevel.Error, Messages.ATPTEFMMessages.UnitCostMustBeGreaterThanZero,
        //                       CheckOnInserted = false)]
        [PXFormula(typeof(Validate<netUnitCost, netQty, netAmt>))]
        [PXUIEnabled(typeof(Where<Current<ATPTEFMFundTransaction.fundTransactionType>, Equal<ATPTEFMFundTransactionTypeAttribute.cashAdvanceValue>>))]
        //[PXUIVisible(typeof(Where<Current<ATPTEFMFundTransaction.fundTransactionType>, Equal<ATPTEFMFundTransactionTypeAttribute.cashAdvanceValue>>))]
        public virtual decimal? NetUnitCost { get; set; }
        public abstract class netUnitCost : BqlDecimal.Field<netUnitCost> { }
        #endregion

        #region NetAmt
        [PXDBDecimal(2)]
        [PXDefault(TypeCode.Decimal, "0.0", PersistingCheck = PXPersistingCheck.Null)]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.NetAmt, Visible = true, IsReadOnly = true)]
        [PXFormula(typeof(Default<qty, unitCost, netQty, netUnitCost>))]
        [PXFormula(typeof(Switch<Case<Where<Current<ATPTEFMFundTransaction.fundTransactionType>, Equal<ATPTEFMFundTransactionTypeAttribute.reimbursementValue>>, Mult<qty, unitCost>>, Mult<netQty, netUnitCost>>), typeof(SumCalc<ATPTEFMFundTransaction.actualSpentAmount>))]
        [PXUIEnabled(typeof(Where<Current<ATPTEFMFundTransaction.fundTransactionType>, Equal<ATPTEFMFundTransactionTypeAttribute.cashAdvanceValue>>))]
        //[PXUIVisible(typeof(Where<Current<ATPTEFMFundTransaction.fundTransactionType>, Equal<ATPTEFMFundTransactionTypeAttribute.cashAdvanceValue>>))]
        public virtual decimal? NetAmt { get; set; }
        public abstract class netAmt : BqlDecimal.Field<netAmt> { }
        #endregion

        #region WhtAmount
        [PXDBDecimal(2)]
        [PXDefault(TypeCode.Decimal, "0.0")]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.WHT, IsReadOnly = true)]
        public virtual decimal? WhtAmount { get; set; }
        public abstract class whtAmount : BqlDecimal.Field<whtAmount> { }
        #endregion

        #region FundReturn
        [PXDBDecimal(2)]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.FundReturn)]
        [PXDefault(TypeCode.Decimal, "0.0", PersistingCheck = PXPersistingCheck.Nothing)]
        //[PXFormula(null, typeof(SumCalc<ATPTEFMFundTransaction.changeAmount>))]
        //[PXFormula(typeof(Default<netAmt>))]
        public virtual decimal? FundReturn { get; set; }
        public abstract class fundReturn : BqlDecimal.Field<fundReturn> { }
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

        #region IsLiquidated
        [PXDBBool()]
        [PXDefault(false)]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.Liquidated)]
        public virtual bool? IsLiquidated { get; set; }
        public abstract class isLiquidated : BqlBool.Field<isLiquidated> { }
        #endregion

        #region IsImported
        [PXDBBool()]
        [PXDefault(false, PersistingCheck = PXPersistingCheck.Nothing)]
        public virtual bool? IsImported { get; set; }
        public abstract class isImported : PX.Data.BQL.BqlBool.Field<isImported> { }
        #endregion

        #region LoadedFromExcel
        [PXDBBool()]
        [PXDefault(false, PersistingCheck = PXPersistingCheck.Nothing)]
        public virtual bool? LoadedFromExcel { get; set; }
        public abstract class loadedFromExcel : PX.Data.BQL.BqlBool.Field<loadedFromExcel> { }
        #endregion

        #region LineDescription
        [PXDBString(256, IsUnicode = true)]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.LineDescription)]
        [PXDefault(typeof(Switch<Case<Where<GetSetupValue<ATPTEFMSetup.setNonStockItemDescriptionAsDefault>, Equal<True>>, Selector<inventoryID, InventoryItem.descr>>, Null>),
            PersistingCheck = PXPersistingCheck.NullOrBlank)]
        [PXFormula(typeof(Default<inventoryID>))]
        public virtual string LineDescription { get; set; }
        public abstract class lineDescription : BqlString.Field<lineDescription> { }
        #endregion

        #region BranchID
        [Branch(typeof(Current<ATPTEFMFundTransaction.branchID>), DisplayName = Messages.ATPTEFMMessages.Branch, Required = true)]
        public virtual int? BranchID { get; set; }
        public abstract class branchID : BqlInt.Field<branchID> { }
        #endregion
    }
}
