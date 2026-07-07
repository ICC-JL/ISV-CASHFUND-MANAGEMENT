using PX.Data;
using PX.Data.ReferentialIntegrity.Attributes;
using PX.Objects.CN.ProjectAccounting.PM.Descriptor;
using PX.Objects.GL;
using PX.Objects.GL.Descriptor;
using PX.Objects.GL.FinPeriods.TableDefinition;
using PX.Objects.IN;
using PX.Objects.PM;
using System;

namespace CashFundManagement.DAC {
    [Serializable]
    [PXCacheName(Messages.ATPTEFMMessages.ATPTEFMProjectBudgetLineSummary)]
    [PXPrimaryGraph(typeof(BLC.ATPTEFMProjectBudgetEntry))]
    public class ATPTEFMProjectBudgetLineSummary : Base.ATPTEFMAudit, IBqlTable
    {
        #region Selected
        [PXBool]
        [PXUnboundDefault(false)]
        [PXUIField(DisplayName = "Selected")]
        public bool? Selected { get; set; }
        public abstract class selected : PX.Data.BQL.BqlBool.Field<selected> { }
        #endregion
        #region Deleted
        [PXDBBool()]
        [PXDefault(false)]
        public virtual bool? Deleted { get; set; }
        public abstract class deleted : PX.Data.BQL.BqlBool.Field<deleted> { }
        #endregion
        #region BudgetDistributionMethod
        [PXDBString(1, IsFixed = true)]
        [Attributes.ATPTEFMBudgetDistributionMethodAttribute]
        public string BudgetDistributionMethod { get; set; }
        public abstract class budgetDistributionMethod : PX.Data.BQL.BqlString.Field<budgetDistributionMethod> { }
        #endregion
        #region RLProjectBudgetLineSummaryID
        [PXDBIdentity(IsKey = true)]
        public virtual int? RLProjectBudgetLineSummaryID { get; set; }
        public abstract class rLProjectBudgetLineSummaryID : PX.Data.BQL.BqlInt.Field<rLProjectBudgetLineSummaryID> { }
        #endregion
        #region LedgerID
        [PXDBInt()]
        [PXDefault]
        [PXSelector(typeof(Ledger.ledgerID),
                    typeof(Ledger.ledgerCD),
                    typeof(Ledger.descr),
                    SubstituteKey = typeof(Ledger.ledgerCD),
                    DescriptionField = typeof(Ledger.descr))]
        [PXUIField(DisplayName = "Ledger", Enabled = true)]
        public virtual int? LedgerID { get; set; }
        public abstract class ledgerID : PX.Data.BQL.BqlInt.Field<ledgerID> { }
        #endregion
        #region FinYear
        [PXDBString(4, IsUnicode = true)]
        [GenericFinYearSelector(typeof(Search3<FinYear.year, OrderBy<Desc<FinYear.year>>>), null)]
        [PXDefault]
        [PXUIField(DisplayName = "Financial Year")]
        public virtual string FinYear { get; set; }
        public abstract class finYear : PX.Data.BQL.BqlString.Field<finYear> { }
        #endregion
        #region ProjectID
        [PXDBInt()]
        [PXSelector(typeof(PMProject.contractID),
                    typeof(PMProject.contractCD),
                    typeof(PMProject.description),
                    SubstituteKey = typeof(PMProject.contractCD),
                    DescriptionField = typeof(PMProject.description))]
        [PXRestrictor(typeof(Where<PMProject.isActive, Equal<True>>), PX.Objects.GL.Messages.AccountInactive)]
        [PXUIField(DisplayName = "Project", Visibility = PXUIVisibility.Visible, Enabled = true)]
        [PXDefault]
        public virtual int? ProjectID { get; set; }
        public abstract class projectID : PX.Data.BQL.BqlInt.Field<projectID> { }
        #endregion
        #region ProjectTaskID
        [PXDBInt()]
        [PXDefault]
        [PXSelector(typeof(Search<
            PMTask.taskID, 
            Where<PMTask.projectID, Equal<Current<ATPTEFMProjectBudgetLineSummary.projectID>>, 
                And<Where<PMTask.type, Equal<ProjectTaskType.cost>, 
                    Or<PMTask.type, Equal<ProjectTaskType.costRevenue>>>>>>),
                    typeof(PMTask.taskCD),
                    typeof(PMTask.description),
                    SubstituteKey = typeof(PMTask.taskCD))]
        [PXUIField(DisplayName = "Task")]
        //[PXUIEnabled(typeof(Where<Current<released>, Equal<False>>))]
        public virtual int? ProjectTaskID { get; set; }
        public abstract class projectTaskID : PX.Data.BQL.BqlInt.Field<projectTaskID> { }
        #endregion
        #region CostCodeID
        [CostCode(null, typeof(projectTaskID), PX.Objects.GL.AccountType.Expense, DisplayName = "Cost Code")]
        [PXDefault]
        //[PXUIEnabled(typeof(Where<Current<released>, Equal<False>>))]
        public virtual int? CostCodeID { get; set; }
        public abstract class costCodeID : PX.Data.BQL.BqlInt.Field<costCodeID> { }
        #endregion
        #region AccountGroupID
        [AccountGroup(typeof(Where<PMAccountGroup.isExpense, Equal<True>>), DisplayName = "Account Group")]
        [PXDefault]
        [PXRestrictor(typeof(Where<PMAccountGroup.isActive, Equal<True>>), "The {0} account group is inactive. You can activate it on the Account Groups (PM201000) form.", new Type[] { typeof(PMAccountGroup.groupCD) })]
        [PXForeignReference(typeof(Field<accountGroupID>.IsRelatedTo<PMAccountGroup.groupID>))]
        public virtual int? AccountGroupID { get; set; }
        public abstract class accountGroupID : PX.Data.BQL.BqlInt.Field<accountGroupID> { }
        #endregion
        //#region InventoryID
        //[PXDBInt()]
        //[PXUIField(DisplayName = "Inventory ID")]
        //[PXDefault]
        //[PMInventorySelector]
        //[PXForeignReference(typeof(Field<inventoryID>.IsRelatedTo<InventoryItem.inventoryID>))]
        //public virtual int? InventoryID { get; set; }
        //public abstract class inventoryID : PX.Data.BQL.BqlInt.Field<inventoryID> { }
        //#endregion
        #region Description
        [PXString]
        [PXUnboundDefault(typeof(Search<PMCostCode.description, Where<PMCostCode.costCodeID, Equal<Current<costCodeID>>>>))]
        [PXFormula(typeof(Default<costCodeID>))]
        [PXUIField(DisplayName = "Description", IsReadOnly = true)]
        public virtual string Description { get; set; }
        public abstract class description : PX.Data.BQL.BqlString.Field<description> { }
        #endregion
        #region Amount
        [PXDBDecimal]
        [PXDefault(TypeCode.Decimal, "0.0", PersistingCheck = PXPersistingCheck.Nothing)]
        [PXUIField(DisplayName = "Amount")]
        [PXUIVerify(typeof(Where<amount, Equal<distributedAmount>>), PXErrorLevel.RowWarning, Messages.ATPTEFMMessages.AmountNotEqual, CheckOnRowSelected = true)]
        //[PXUIEnabled(typeof(Where<Current<released>, Equal<False>>))]
        public virtual decimal? Amount { get; set; }
        public abstract class amount : PX.Data.BQL.BqlDecimal.Field<amount> { }
        #endregion
        #region DistributedAmount
        [PXDBDecimal]
        [PXDefault(TypeCode.Decimal, "0.0", PersistingCheck = PXPersistingCheck.Nothing)]
        [PXFormula(typeof(
            Add<finPeriod01,
            Add<finPeriod02,
            Add<finPeriod03,
            Add<finPeriod04,
            Add<finPeriod05,
            Add<finPeriod06,
            Add<finPeriod07,
            Add<finPeriod08,
            Add<finPeriod09,
            Add<finPeriod10,
            Add<finPeriod11,
                finPeriod12
            >>>>>>>>>>>))]
        [PXUIField(DisplayName = "Distributed Amount", IsReadOnly = true)]
        public virtual decimal? DistributedAmount { get; set; }
        public abstract class distributedAmount : PX.Data.BQL.BqlDecimal.Field<distributedAmount> { }
        #endregion
        #region FinPeriod01
        [PXDBDecimal]
        [PXUIField(DisplayName = "Period 01")]
        [PXDefault(TypeCode.Decimal, "0.0")]
        //[PXUIEnabled(typeof(Where<Current<released>, Equal<False>>))]
        public virtual decimal? FinPeriod01 { get; set; }
        public abstract class finPeriod01 : PX.Data.BQL.BqlDecimal.Field<finPeriod01> { }
        #endregion
        #region FinPeriod02
        [PXDBDecimal]
        [PXUIField(DisplayName = "Period 02")]
        [PXDefault(TypeCode.Decimal, "0.0")]
        //[PXUIEnabled(typeof(Where<Current<released>, Equal<False>>))]
        public virtual decimal? FinPeriod02 { get; set; }
        public abstract class finPeriod02 : PX.Data.BQL.BqlDecimal.Field<finPeriod02> { }
        #endregion
        #region FinPeriod03
        [PXDBDecimal]
        [PXUIField(DisplayName = "Period 03")]
        [PXDefault(TypeCode.Decimal, "0.0")]
        //[PXUIEnabled(typeof(Where<Current<released>, Equal<False>>))]
        public virtual decimal? FinPeriod03 { get; set; }
        public abstract class finPeriod03 : PX.Data.BQL.BqlDecimal.Field<finPeriod03> { }
        #endregion
        #region FinPeriod04
        [PXDBDecimal]
        [PXUIField(DisplayName = "Period 04")]
        [PXDefault(TypeCode.Decimal, "0.0")]
        //[PXUIEnabled(typeof(Where<Current<released>, Equal<False>>))]
        public virtual decimal? FinPeriod04 { get; set; }
        public abstract class finPeriod04 : PX.Data.BQL.BqlDecimal.Field<finPeriod04> { }
        #endregion
        #region FinPeriod05
        [PXDBDecimal]
        [PXUIField(DisplayName = "Period 05")]
        [PXDefault(TypeCode.Decimal, "0.0")]
        //[PXUIEnabled(typeof(Where<Current<released>, Equal<False>>))]
        public virtual decimal? FinPeriod05 { get; set; }
        public abstract class finPeriod05 : PX.Data.BQL.BqlDecimal.Field<finPeriod05> { }
        #endregion
        #region FinPeriod06
        [PXDBDecimal]
        [PXUIField(DisplayName = "Period 06")]
        [PXDefault(TypeCode.Decimal, "0.0")]
        //[PXUIEnabled(typeof(Where<Current<released>, Equal<False>>))]
        public virtual decimal? FinPeriod06 { get; set; }
        public abstract class finPeriod06 : PX.Data.BQL.BqlDecimal.Field<finPeriod06> { }
        #endregion
        #region FinPeriod07
        [PXDBDecimal]
        [PXUIField(DisplayName = "Period 07")]
        [PXDefault(TypeCode.Decimal, "0.0")]
        //[PXUIEnabled(typeof(Where<Current<released>, Equal<False>>))]
        public virtual decimal? FinPeriod07 { get; set; }
        public abstract class finPeriod07 : PX.Data.BQL.BqlDecimal.Field<finPeriod07> { }
        #endregion
        #region FinPeriod08
        [PXDBDecimal]
        [PXUIField(DisplayName = "Period 08")]
        [PXDefault(TypeCode.Decimal, "0.0")]
        //[PXUIEnabled(typeof(Where<Current<released>, Equal<False>>))]
        public virtual decimal? FinPeriod08 { get; set; }
        public abstract class finPeriod08 : PX.Data.BQL.BqlDecimal.Field<finPeriod08> { }
        #endregion
        #region FinPeriod09
        [PXDBDecimal]
        [PXUIField(DisplayName = "Period 09")]
        [PXDefault(TypeCode.Decimal, "0.0")]
        //[PXUIEnabled(typeof(Where<Current<released>, Equal<False>>))]
        public virtual decimal? FinPeriod09 { get; set; }
        public abstract class finPeriod09 : PX.Data.BQL.BqlDecimal.Field<finPeriod09> { }
        #endregion
        #region FinPeriod10
        [PXDBDecimal]
        [PXUIField(DisplayName = "Period 10")]
        [PXDefault(TypeCode.Decimal, "0.0")]
        //[PXUIEnabled(typeof(Where<Current<released>, Equal<False>>))]
        public virtual decimal? FinPeriod10 { get; set; }
        public abstract class finPeriod10 : PX.Data.BQL.BqlDecimal.Field<finPeriod10> { }
        #endregion
        #region FinPeriod11
        [PXDBDecimal]
        [PXUIField(DisplayName = "Period 11")]
        [PXDefault(TypeCode.Decimal, "0.0")]
        //[PXUIEnabled(typeof(Where<Current<released>, Equal<False>>))]
        public virtual decimal? FinPeriod11 { get; set; }
        public abstract class finPeriod11 : PX.Data.BQL.BqlDecimal.Field<finPeriod11> { }
        #endregion
        #region FinPeriod12
        [PXDBDecimal]
        [PXUIField(DisplayName = "Period 12")]
        [PXDefault(TypeCode.Decimal, "0.0")]
        //[PXUIEnabled(typeof(Where<Current<released>, Equal<False>>))]
        public virtual decimal? FinPeriod12 { get; set; }
        public abstract class finPeriod12 : PX.Data.BQL.BqlDecimal.Field<finPeriod12> { }
        #endregion
        #region Released
        [PXDBBool]
        [PXDefault(false)]
        [PXUIField(DisplayName = "Released", IsReadOnly = true)]
        public virtual bool? Released { get; set; }
        public abstract class released : PX.Data.BQL.BqlBool.Field<released> { }
        #endregion
        #region WasReleased
        [PXDBBool]
        [PXDefault(false)]
        public virtual bool? WasReleased { get; set; }
        public abstract class wasReleased : PX.Data.BQL.BqlBool.Field<wasReleased> { }
        #endregion
        #region ReleasedAmount
        [PXDBDecimal]
        [PXDefault(TypeCode.Decimal, "0.0", PersistingCheck = PXPersistingCheck.Nothing)]
        public virtual decimal? ReleasedAmount { get; set; }
        public abstract class releasedAmount : PX.Data.BQL.BqlDecimal.Field<releasedAmount> { }
        #endregion

        #region IsCompare
        [PXBool]
        [PXUnboundDefault(false)]
        public bool? IsCompare { get; set; }
        public abstract class isCompare : PX.Data.BQL.BqlBool.Field<isCompare> { }
        #endregion
        #region CompareProjectTaskID
        [PXInt]
        public virtual int? CompareProjectTaskID { get; set; }
        public abstract class compareProjectTaskID : PX.Data.BQL.BqlInt.Field<compareProjectTaskID> { }
        #endregion
        #region CompareCostCodeID
        [PXInt]
        public virtual int? CompareCostCodeID { get; set; }
        public abstract class compareCostCodeID : PX.Data.BQL.BqlInt.Field<compareCostCodeID> { }
        #endregion
        #region SortOrder
        [PXInt]
        public virtual int? SortOrder { get; set; }
        public abstract class sortOrder : PX.Data.BQL.BqlInt.Field<sortOrder> { }
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

    }
}