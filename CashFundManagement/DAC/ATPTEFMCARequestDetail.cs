using CashFundManagement.Attributes;
using CashFundManagement.DAC.Setup;
using PX.Data;
using PX.Data.BQL;
using PX.Data.ReferentialIntegrity.Attributes;
using PX.Objects.AP;
using PX.Objects.CM;
using PX.Objects.CN.ProjectAccounting.Descriptor;
using PX.Objects.CN.ProjectAccounting.PM.Descriptor;
using PX.Objects.Common;
using PX.Objects.CS;
using PX.Objects.GL;
using PX.Objects.IN;
using PX.Objects.PM;
using System;

namespace CashFundManagement.DAC {
    [PXPrimaryGraph(typeof(BLC.ATPTEFMCashAdvanceEntry))]
    [Serializable]
    [PXCacheName(Messages.ATPTEFMMessages.ATPTEFMCARequestDetail)]
    public class ATPTEFMCARequestDetail : Base.ATPTEFMAudit, IBqlTable
    {
        #region CashAdvanceRequestDetailID
        [PXDBIdentity(IsKey = true)]
        public virtual int? CashAdvanceRequestDetailID { get; set; }
        public abstract class cashAdvanceRequestDetailID : PX.Data.IBqlField { }
        #endregion

        #region CashAdvanceNbr
        [PXDBString(15, IsUnicode =true)]
        [PXDBDefault(typeof(ATPTEFMCashAdvance.cashAdvanceNbr))]
        [PXParent(typeof(Select<
            ATPTEFMCashAdvance,
            Where<ATPTEFMCashAdvance.cashAdvanceNbr, Equal<Current<ATPTEFMCARequestDetail.cashAdvanceNbr>>>>))]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.CashAdvanceNbrRqstID)]
        public virtual string CashAdvanceNbr { get; set; }
        public abstract class cashAdvanceNbr : PX.Data.IBqlField { }
        #endregion

        #region InventoryID
        [PXDBInt()]
        [PXDBDefault()]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.InventoryID)]
        [ATPTEFMCARequestClassItemSelector(typeof(ATPTEFMCashAdvance), typeof(ATPTEFMCashAdvance.reqClassID), typeof(Null), typeof(Null), typeof(Null))]
        public virtual int? InventoryID { get; set; }
        public abstract class inventoryID : IBqlField { }   
        #endregion

        #region Qty
        [PXDBDecimal(2)]
        [PXDefault(TypeCode.Decimal, "1", PersistingCheck = PXPersistingCheck.Null)]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.Qty)]
        public virtual decimal? Qty { get; set; }
        public abstract class qty : PX.Data.IBqlField { }
        #endregion

        #region UnitCost
        [PXDBBaseCury()]
        [PXDefault(TypeCode.Decimal, "0.0", PersistingCheck = PXPersistingCheck.Null)]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.UnitCost)]
        public virtual decimal? UnitCost { get; set; }
        public abstract class unitCost : PX.Data.IBqlField { }
        #endregion

        #region CuryUnitCost
        [PXDBCurrency(typeof(ATPTEFMCashAdvance.curyInfoID), typeof(ATPTEFMCARequestDetail.unitCost))]
        [PXDefault(TypeCode.Decimal, "0.0", PersistingCheck = PXPersistingCheck.Null)]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.UnitCost)]
        public virtual decimal? CuryUnitCost { get; set; }
        public abstract class curyUnitCost : PX.Data.BQL.BqlDecimal.Field<curyUnitCost> { }
        #endregion

        #region CuryAmount
        [PXDBCurrency(typeof(ATPTEFMCashAdvance.curyInfoID), typeof(ATPTEFMCARequestDetail.amount))]
        [PXDefault(TypeCode.Decimal, "0.0", PersistingCheck = PXPersistingCheck.Null)]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.Amount, IsReadOnly = true)]
        //[PXFormula(typeof(Default<cashAdvanceNbr>))]
        [PXFormula(typeof(Mult<ATPTEFMCARequestDetail.qty, ATPTEFMCARequestDetail.curyUnitCost>), typeof(SumCalc<ATPTEFMCashAdvance.curyRequestedAmount>))]
        public virtual decimal? CuryAmount { get; set; }
        public abstract class curyAmount : PX.Data.BQL.BqlDecimal.Field<curyAmount> { }
        #endregion

        #region Amount
        [PXDBBaseCury()]
        [PXDefault(TypeCode.Decimal, "0.0", PersistingCheck = PXPersistingCheck.Null)]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.Amount, IsReadOnly = true)]
        //[PXFormula(typeof(Default<cashAdvanceNbr>))]
        [PXFormula(typeof(Mult<ATPTEFMCARequestDetail.qty, ATPTEFMCARequestDetail.curyUnitCost>), typeof(SumCalc<ATPTEFMCashAdvance.requestedAmount>))]
        public virtual decimal? Amount { get; set; }
        public abstract class amount : PX.Data.IBqlField { }
        #endregion

        #region AccountID
        //[PXSelector(
        //    typeof(Search<Account.accountID>),
        //    typeof(Account.accountCD),
        //    typeof(Account.description),
        //    SubstituteKey = typeof(Account.accountCD))]
        [Account(DisplayName = Messages.ATPTEFMMessages.AccountID)]
        [PXDefault]
        //[PXFormula(typeof(Selector<inventoryID, InventoryItem.cOGSAcctID>))]
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

        #region SubID
        [SubAccount(typeof(accountID), DisplayName = Messages.ATPTEFMMessages.Subaccount)]
        [PXDefault]
        public int? SubID { get; set; }
        public abstract class subID : IBqlField { }
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
        public abstract class selected : PX.Data.BQL.BqlBool.Field<selected> { }
        #endregion

        #region Balance
        [PXDecimal(2)]
        [PXFormula(typeof(curyAmount))]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.Balance)]
        public virtual decimal? Balance { get; set; }
        public abstract class balance : PX.Data.IBqlField { }
        #endregion

        #region RunningQty
        [PXDecimal(2)]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.RunningQty)]
        public virtual decimal? RunningQty { get; set; }
        public abstract class runningQty : PX.Data.IBqlField { }
        #endregion

        #region LineDescription
        [PXDBString(255, IsUnicode = true)]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.LineDescription)]
        [PXDefault(typeof(Switch<Case<Where<GetSetupValue<ATPTEFMCASetup.setNonStockItemDescriptionAsDefault>, Equal<True>>, Selector<inventoryID, InventoryItem.descr>>, Null>),
            PersistingCheck = PXPersistingCheck.NullOrBlank)]
        [PXFormula(typeof(Default<inventoryID>))]
        public virtual string Remarks { get; set; }
        public abstract class remarks : BqlString.Field<remarks> { }
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

        #region Uom
        [INUnit(DisplayName = Messages.ATPTEFMMessages.UOM, Required = true)]
        [PXDefault]
        [PXFormula(typeof(Selector<inventoryID, InventoryItem.baseUnit>))]
        public virtual string Uom { get; set; }
        public abstract class uom : IBqlField { }
        #endregion

        #region IsImported
        [PXDBBool()]
        [PXDefault(false, PersistingCheck = PXPersistingCheck.Nothing)]
        public virtual bool? IsImported { get; set; }
        public abstract class isImported : PX.Data.BQL.BqlBool.Field<isImported> { }
        #endregion
    }
}