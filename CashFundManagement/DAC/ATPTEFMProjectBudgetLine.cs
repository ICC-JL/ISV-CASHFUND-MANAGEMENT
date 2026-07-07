using PX.Data;
using PX.Data.ReferentialIntegrity.Attributes;
using PX.Objects.GL;
using PX.Objects.GL.Descriptor;
using PX.Objects.GL.FinPeriods.TableDefinition;
using PX.Objects.PM;
using PX.Objects.IN;
using System;

namespace CashFundManagement.DAC {
    [Serializable]
    [PXCacheName(Messages.ATPTEFMMessages.ATPTEFMProjectBudgetLine)]
    [PXPrimaryGraph(typeof(BLC.ATPTEFMProjectBudgetEntry))]
    public class ATPTEFMProjectBudgetLine : Base.ATPTEFMAudit, IBqlTable
    {
        #region Selected
        [PXBool]
        [PXUnboundDefault(false)]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.Selected)]
        public bool? Selected { get; set; }
        public abstract class selected : PX.Data.BQL.BqlBool.Field<selected> { }
        #endregion
        #region RLProjectBudgetLineDetailID
        [PXDBIdentity]
        public virtual int? RLProjectBudgetLineDetailID { get; set; }
        public abstract class rLProjectBudgetLineDetailID : PX.Data.BQL.BqlInt.Field<rLProjectBudgetLineDetailID> { }
        #endregion
        #region LedgerID
        [PXDBInt(IsKey = true)]
        [PXDefault]
        [PXSelector(typeof(Ledger.ledgerID),
                    typeof(Ledger.ledgerCD),
                    typeof(Ledger.descr),
                    SubstituteKey = typeof(Ledger.ledgerCD),
                    DescriptionField = typeof(Ledger.descr))]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.LedgerID, Enabled = true)]
        public virtual int? LedgerID { get; set; }
        public abstract class ledgerID : PX.Data.BQL.BqlInt.Field<ledgerID> { }
        #endregion
        #region FinYear
        [PXDBString(4, IsKey = true, IsUnicode = true)]
        [GenericFinYearSelector(typeof(Search3<FinYear.year, OrderBy<Desc<FinYear.year>>>), null)]
        [PXDefault]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.FinYear)]
        public virtual string FinYear { get; set; }
        public abstract class finYear : PX.Data.BQL.BqlString.Field<finYear> { }
        #endregion
        #region ProjectID
        [PXDBInt(IsKey = true)]
        [PXSelector(typeof(PMProject.contractID),
                    typeof(PMProject.contractCD),
                    typeof(PMProject.description),
                    SubstituteKey = typeof(PMProject.contractCD),
                    DescriptionField = typeof(PMProject.description))]
        [PXRestrictor(typeof(Where<PMProject.isActive, Equal<True>>), PX.Objects.GL.Messages.AccountInactive)]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.ProjectID, Visibility = PXUIVisibility.Visible, Enabled = true)]
        [PXDefault]
        public virtual int? ProjectID { get; set; }
        public abstract class projectID : PX.Data.BQL.BqlInt.Field<projectID> { }
        #endregion
        #region FinPeriodID
        [PXDBString(6, IsKey = true, IsUnicode = true)]
        [PXDefault]
        //[SOFinPeriod]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.FinPeriodid)]
        public virtual string FinPeriodID { get; set; }
        public abstract class finPeriodID : PX.Data.BQL.BqlString.Field<finPeriodID> { }
        #endregion
        #region ProjectTaskID
        [PXDBInt(IsKey = true)]
        [PXDefault]
        [PXSelector(typeof(Search<PMTask.taskID, Where<PMTask.projectID, Equal<Current<ATPTEFMProjectBudgetLine.projectID>>>>),
                    typeof(PMTask.taskCD),
                    typeof(PMTask.description),
                    SubstituteKey = typeof(PMTask.taskCD))]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.ProjectTaskID)]
        public virtual int? ProjectTaskID { get; set; }
        public abstract class projectTaskID : PX.Data.BQL.BqlInt.Field<projectTaskID> { }
        #endregion
        #region CostCodeID
        [CostCode(null, typeof(projectTaskID), "E", IsKey = true, DisplayName = Messages.ATPTEFMMessages.CostCodeID)]
        [PXDefault]
        //[PXUIField(DisplayName = Messages.ATPTEFMMessages.CostCodeID)]
        public virtual int? CostCodeID { get; set; }
        public abstract class costCodeID : PX.Data.BQL.BqlInt.Field<costCodeID> { }
        #endregion
        #region AccountGroupID
        [AccountGroup(typeof(Where<PMAccountGroup.isExpense, Equal<True>>), DisplayName = "Account Group", IsKey = true)]
        [PXDefault]
        [PXRestrictor(typeof(Where<PMAccountGroup.isActive, Equal<True>>), "The {0} account group is inactive. You can activate it on the Account Groups (PM201000) form.", new Type[] { typeof(PMAccountGroup.groupCD) })]
        [PXForeignReference(typeof(Field<accountGroupID>.IsRelatedTo<PMAccountGroup.groupID>))]
        public virtual int? AccountGroupID { get; set; }
        public abstract class accountGroupID : PX.Data.BQL.BqlInt.Field<accountGroupID> { }
        #endregion
        //#region InventoryID
        //[PXDBInt(IsKey = true)]
        //[PXUIField(DisplayName = "Inventory ID")]
        //[PXDefault]
        //[PMInventorySelector]
        //[PXForeignReference(typeof(Field<inventoryID>.IsRelatedTo<InventoryItem.inventoryID>))]
        //public virtual int? InventoryID { get; set; }
        //public abstract class inventoryID : PX.Data.BQL.BqlInt.Field<inventoryID> { }
        //#endregion
        #region Amount
        [PXDBDecimal(typeof(Search2<PX.Objects.CM.Currency.decimalPlaces,
            InnerJoin<Ledger, On<Ledger.baseCuryID, Equal<PX.Objects.CM.Currency.curyID>>>,
            Where<Ledger.ledgerID, Equal<Current<DAC.ATPTEFMProjectBudget.ledgerID>>>>))]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.Amount)]
        [PXDefault(TypeCode.Decimal, "0.0", PersistingCheck = PXPersistingCheck.Nothing)]
        public virtual decimal? Amount { get; set; }
        public abstract class amount : PX.Data.BQL.BqlDecimal.Field<amount> { }
        #endregion
        #region ReleasedAmount
        [PXDBDecimal]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.ReleasedAmnt, Enabled = false)]
        [PXDefault(TypeCode.Decimal, "0.0", PersistingCheck = PXPersistingCheck.Nothing)]
        public virtual decimal? ReleasedAmount { get; set; }
        public abstract class releasedAmount : PX.Data.BQL.BqlDecimal.Field<releasedAmount> { }
        #endregion
        #region Released
        [PXDBBool]
        [PXDefault(false, PersistingCheck = PXPersistingCheck.Nothing)]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.Released, IsReadOnly = true)]
        public virtual bool? Released { get; set; }
        public abstract class released : PX.Data.BQL.BqlBool.Field<released> { }
        #endregion
        #region WasReleased
        [PXDBBool]
        [PXDefault(false, PersistingCheck = PXPersistingCheck.Nothing)]
        public virtual bool? WasReleased { get; set; }
        public abstract class wasReleased : PX.Data.BQL.BqlBool.Field<wasReleased> { }
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