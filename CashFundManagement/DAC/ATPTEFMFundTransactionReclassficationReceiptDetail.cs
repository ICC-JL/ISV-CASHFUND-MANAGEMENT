using CashFundManagement.BLC;
using CashFundManagement.DAC.Setup;
using CashFundManagement.Messages;
using PX.Data;
using PX.Data.BQL;
using PX.Objects.Common;
using PX.Objects.CS;
using PX.Objects.GL;
using PX.Objects.IN;
using PX.Objects.PM;
using PX.Objects.TX;
using System;

namespace CashFundManagement.DAC {
    [PXPrimaryGraph(typeof(ATPTEFMFundTransactionEntry))]
    [Serializable]
    [PXCacheName(Messages.ATPTEFMMessages.ATPTEFMFundTransactionReclassficationReceiptDetail)]
    public class ATPTEFMFundTransactionReclassficationReceiptDetail : Base.ATPTEFMAudit, IBqlTable
    {
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

        #region FundTransactionReclassficationReceiptDetailID
        [PXDBLongIdentity(IsKey = true)]
        public virtual long? FundTransactionReclassficationReceiptDetailID { get; set; }
        public abstract class fundTransactionReclassficationReceiptDetailID : BqlLong.Field<fundTransactionReclassficationReceiptDetailID> { }
        #endregion

        #region FundTransactionRefNbr
        [PXDBString(15, IsUnicode = true)]
        [PXDBDefault(typeof(ATPTEFMFundTransaction.refNbr))]
        [PXParent(typeof(Select<
            ATPTEFMFundTransaction,
            Where<ATPTEFMFundTransaction.refNbr, Equal<Current<ATPTEFMFundTransactionReclassficationReceiptDetail.fundTransactionRefNbr>>>>))]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.FundTransactionRefNbr)]
        public virtual string FundTransactionRefNbr { get; set; }
        public abstract class fundTransactionRefNbr : BqlString.Field<fundTransactionRefNbr> { }
        #endregion 

        #region Date
        [PXDBDate]
        [PXDefault(typeof(AccessInfo.businessDate))]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.Date, Required = false)]
        public virtual DateTime? Date { get; set; }
        public abstract class date : BqlDateTime.Field<date> { }
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

        #region SubID
        [SubAccount(typeof(accountID), DisplayName = Messages.ATPTEFMMessages.Subid, Required = false)]
        [PXDefault(typeof(Search<InventoryItem.cOGSSubID,
                            Where<InventoryItem.inventoryID,
                                Equal<Current<inventoryID>>>>))]
        [PXFormula(typeof(Default<inventoryID>))]
        public int? SubID { get; set; }
        public abstract class subID : BqlInt.Field<subID> { }
        #endregion

        #region RefNbr
        [PXDBString(15, IsUnicode = true)]
        [PXDefault(PersistingCheck = PXPersistingCheck.Nothing)]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.RefNbr)]
        [PXUIRequired(typeof(Where<GetSetupValue<ATPTEFMSetup.requireExternalReferenceNbr>, Equal<boolTrue>>))]
        public virtual string RefNbr { get; set; }
        public abstract class refNbr : BqlString.Field<refNbr> { }
        #endregion

        #region TaxZoneID
        [PXDBString(10, IsUnicode = true)]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.TaxZoneID, Required = false)]
        [PXSelector(typeof(Search<TaxZone.taxZoneID>), DescriptionField = typeof(TaxZone.descr))]
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
        public abstract class fundTransactionDetailID : PX.Data.IBqlField { }
        #endregion

        #region ProjectID
        [PXDBInt]
        [PXDefault(typeof(PX.Objects.PM.NonProject), PersistingCheck = PXPersistingCheck.Nothing)]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.ProjectID)]
        [PXSelector(typeof(Search<PMProject.contractID>), typeof(PMProject.contractID), typeof(PMProject.contractCD), typeof(PMProject.description), SubstituteKey = typeof(PMProject.contractCD))]
        [PXUIVisible(typeof(FeatureInstalled<FeaturesSet.projectAccounting>))]
        [PXUIRequired(typeof(FeatureInstalled<FeaturesSet.projectAccounting>))]
        public virtual int? ProjectID { get; set; }
        public abstract class projectID : BqlInt.Field<projectID> { }
        #endregion

        #region ProjectTaskID
        [PXDBInt]
        [PXDefault]
        [PXUIRequired(typeof(Where<projectID, NotEqual<NonProject>, And<FeatureInstalled<FeaturesSet.projectAccounting>>>))]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.ProjectTaskID)]
        [PXSelector(typeof(Search<PMTask.taskID, Where<PMTask.projectID, Equal<Current<projectID>>>>), typeof(PMTask.taskCD), typeof(PMTask.description), SubstituteKey = typeof(PMTask.taskCD))]
        [PXUIVisible(typeof(FeatureInstalled<FeaturesSet.projectAccounting>))]
        public virtual int? ProjectTaskID { get; set; }
        public abstract class projectTaskID : BqlInt.Field<projectTaskID> { }
        #endregion

        #region CostCodeID
        [PXDefault]
        [PXUIRequired(typeof(Where<projectID, NotEqual<NonProject>, And<FeatureInstalled<FeaturesSet.projectAccounting>>>))]
        [CostCode(typeof(InventoryItem.cOGSAcctID), typeof(projectTaskID), AccountType.Expense, DisplayName = Messages.ATPTEFMMessages.CostCodeID)]
        [PXUIVisible(typeof(FeatureInstalled<FeaturesSet.costCodes>))]
        public virtual Int32? CostCodeID { get; set; }
        public abstract class costCodeID : BqlInt.Field<costCodeID> { }
        #endregion

        #region NetQty
        [PXDBDecimal(2)]
        [PXDefault(TypeCode.Decimal, "0.0")]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.NetQty, Visible = true, Required = false)]
        [PXFormula(typeof(Validate<netQty, netUnitCost, netAmt>))]
        public virtual decimal? NetQty { get; set; }
        public abstract class netQty : BqlDecimal.Field<netQty> { }
        #endregion

        #region NetUnitCost
        [PXDBDecimal(2)]
        [PXDefault(TypeCode.Decimal, "0.0")]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.NetUnitCost, Visible = true, Required = false)]
        [PXFormula(typeof(Validate<netUnitCost, netQty, netAmt>))]
        public virtual decimal? NetUnitCost { get; set; }
        public abstract class netUnitCost : BqlDecimal.Field<netUnitCost> { }
        #endregion

        #region NetAmt
        [PXDBDecimal(2)]
        [PXDefault(TypeCode.Decimal, "0.0", PersistingCheck = PXPersistingCheck.Null)]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.NetAmt, Visible = true, IsReadOnly = true)]
        [PXFormula(typeof(Default<netQty, netUnitCost>))]
        [PXFormula(typeof(Mult<netQty, netUnitCost>),
            typeof(SumCalc<ATPTEFMFundTransaction.reclassificationAmt>))]
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
        public virtual decimal? FundReturn { get; set; }
        public abstract class fundReturn : BqlDecimal.Field<fundReturn> { }
        #endregion
    }
}
