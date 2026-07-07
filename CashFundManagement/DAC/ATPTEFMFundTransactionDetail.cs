using CashFundManagement.BLC;
using CashFundManagement.DAC.Setup;
using PX.Data;
using PX.Data.BQL;
using PX.Data.ReferentialIntegrity.Attributes;
using PX.Objects.AP;
using PX.Objects.CN.ProjectAccounting.Descriptor;
using PX.Objects.CN.ProjectAccounting.PM.Descriptor;
using PX.Objects.Common;
using PX.Objects.CS;
using PX.Objects.GL;
using PX.Objects.IN;
using PX.Objects.PM;
using System;

//TODO : Remove dead codes on next upgrade.
namespace CashFundManagement.DAC {
    [PXPrimaryGraph(typeof(ATPTEFMFundTransactionEntry))]
    [Serializable]
    [PXCacheName(Messages.ATPTEFMMessages.ATPTEFMFundTransactionDetail)]
    public class ATPTEFMFundTransactionDetail : Base.ATPTEFMAudit, IBqlTable
    {
        #region FundTransactionDetailID
        [PXDBIdentity(IsKey = true)]
        public virtual int? FundTransactionDetailID { get; set; }
        public abstract class fundTransactionDetailID : BqlInt.Field<fundTransactionDetailID> { }
        #endregion

        #region FundTransactionRefNbr
        [PXDBString(15, IsUnicode =true)]
        [PXDBDefault(typeof(ATPTEFMFundTransaction.refNbr))]
        [PXParent(typeof(Select<
            ATPTEFMFundTransaction,
            Where<ATPTEFMFundTransaction.refNbr, Equal<Current<ATPTEFMFundTransactionDetail.fundTransactionRefNbr>>>>))]
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

        #region Particulars
        [PXDBString(100, IsUnicode = true)]
        [PXDefault(PersistingCheck = PXPersistingCheck.Nothing)]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.Particulars)]
        public virtual string Particulars { get; set; }
        public abstract class particulars : BqlString.Field<particulars> { }
        #endregion

        #region InventoryID
        [PXDBInt()]
        [PXDefault(PersistingCheck = PXPersistingCheck.Nothing)]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.InventoryID)]
        [PXSelector(typeof(Search<InventoryItem.inventoryID, Where<InventoryItem.itemStatus, Equal<InventoryItemStatus.active>>>), typeof(InventoryItem.inventoryID), typeof(InventoryItem.inventoryCD), typeof(InventoryItem.descr), SubstituteKey = typeof(InventoryItem.inventoryCD), DescriptionField = typeof(InventoryItem.descr))]
        [PXRestrictor(typeof(Where<InventoryItem.itemType, Equal<INItemTypes.expenseItem>>), null, ShowWarning = true)]
        public virtual int? InventoryID { get; set; }
        public abstract class inventoryID : BqlInt.Field<inventoryID> { }
        #endregion

        #region Qty
        [PXDBDecimal(2)]
        [PXDefault(TypeCode.Decimal, "1.0", PersistingCheck = PXPersistingCheck.Null)]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.Qty)]
        public virtual decimal? Qty { get; set; }
        public abstract class qty : BqlDecimal.Field<qty> { }
        #endregion

        #region UnitRecordID
        [PXDBString(6, IsUnicode =true)]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.UnitRecordID, IsReadOnly = true)]
        [PXFormula(typeof(Default<inventoryID>))]
        [PXDefault(typeof(Selector<inventoryID, InventoryItem.purchaseUnit>), PersistingCheck = PXPersistingCheck.Nothing)]
        public virtual string UnitRecordID { get; set; }
        public abstract class unitRecordID : BqlString.Field<unitRecordID> { }
        #endregion

        #region UnitCost
        [PXDBDecimal(2)]
        [PXDefault(TypeCode.Decimal, "0.0", PersistingCheck = PXPersistingCheck.Null)]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.UnitCost)]
        public virtual decimal? UnitCost { get; set; }
        public abstract class unitCost : BqlDecimal.Field<unitCost> { }
        #endregion

        #region Amount
        [PXDBDecimal(2)]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.Amount, IsReadOnly = true)]
        [PXFormula(typeof(Default<fundTransactionRefNbr>))]
        [PXFormula(typeof(Mult<ATPTEFMFundTransactionDetail.qty, ATPTEFMFundTransactionDetail.unitCost>), typeof(SumCalc<ATPTEFMFundTransaction.requestedAmount>))]
        [PXFormula(typeof(Mult<ATPTEFMFundTransactionDetail.qty, ATPTEFMFundTransactionDetail.unitCost>), typeof(SumCalc<ATPTEFMFundTransaction.balance>))]
        public virtual decimal? Amount { get; set; }
        public abstract class amount : BqlDecimal.Field<amount> { }
        #endregion

        #region AccountID
        [Account(DisplayName = Messages.ATPTEFMMessages.AccountID)]
        [PXDefault]
        public int? AccountID { get; set; }
        public abstract class accountID : IBqlField { }
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

        /*#region SubID
        //[PXDBInt]
        [PXSelector(
            typeof(Search<Sub.subID>),
            typeof(Sub.subCD),
            typeof(Sub.description),
            SubstituteKey = typeof(Sub.subCD))]
        [SubAccount(typeof(accountID), DisplayName = Messages.ATPTEFMMessages.Subid)]
        [PXDefault(typeof(Search<EPDepartment.expenseSubID,
                            Where<EPDepartment.departmentID,
                                Equal<Current<ATPTEFMFundTransaction.departmentID>>>>))]
        [PXFormula(typeof(Default<ATPTEFMFundTransaction.departmentID>))]
        public int? SubID { get; set; }
        public abstract class subID : BqlInt.Field<subID> { }
        #endregion*/

        #region SubID
        //[PXDBInt]
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

        #region Selected
        [PXBool]
        [PXDefault(false, PersistingCheck = PXPersistingCheck.Nothing)]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.Selected)]
        public bool? Selected { get; set; }
        public abstract class selected : BqlBool.Field<selected> { }
        #endregion

        #region Balance
        [PXDecimal(2)]
        [PXFormula(typeof(amount))]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.Balance)]
        public virtual decimal? Balance { get; set; }
        public abstract class balance : BqlDecimal.Field<balance> { }
        #endregion

        #region RunningQty
        [PXDecimal(2)]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.RunningQty)]
        public virtual decimal? RunningQty { get; set; }
        public abstract class runningQty : BqlDecimal.Field<runningQty> { }
        #endregion

        #region Description
        [PXString]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.Description, IsReadOnly = true)]
        [PXUnboundDefault(typeof(Selector<inventoryID, InventoryItem.descr>))]
        [PXFormula(typeof(Default<inventoryID>))]
        public virtual string Description { get; set; }
        public abstract class description : BqlString.Field<description> { }
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
    }
}
