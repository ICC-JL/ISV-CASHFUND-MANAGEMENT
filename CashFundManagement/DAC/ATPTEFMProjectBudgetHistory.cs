using PX.Data;
using PX.Objects.CM;
using PX.Objects.PM;
using PX.Objects.GL;
using PX.Objects.IN;
using System;
using PX.Data.ReferentialIntegrity.Attributes;

namespace CashFundManagement.DAC {
    [Serializable]
    [PXCacheName(Messages.ATPTEFMMessages.ATPTEFMProjectBudgetHistory)]
    public class ATPTEFMProjectBudgetHistory : Base.ATPTEFMAudit, IBqlTable
    {
        #region RLProjectBudgetHistoryID
        [PXDBIdentity]
        public virtual int? RLProjectBudgetHistoryID { get; set; }
        public abstract class rLProjectBudgetHistoryID : PX.Data.BQL.BqlInt.Field<rLProjectBudgetHistoryID> { }
        #endregion
        #region Origin
        [PXDBInt(IsKey = true)]
        public int? Origin { get; set; }
        public abstract class origin : PX.Data.BQL.BqlInt.Field<origin> { }
        #endregion
        #region RefNbr
        [PXDBString(15, IsKey = true, IsUnicode = true)]
        public virtual String RefNbr { get; set; }
        public abstract class refNbr : PX.Data.BQL.BqlString.Field<refNbr> { }
        #endregion
        #region ProjectID
        [PXDBInt(IsKey = true)]
        [PXSelector(typeof(PMProject.contractID),
                typeof(PMProject.contractCD),
                typeof(PMProject.description),
                SubstituteKey = typeof(PMProject.contractCD),
                DescriptionField = typeof(PMProject.description))]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.ProjectID, IsReadOnly = true)]
        public virtual int? ProjectID { get; set; }
        public abstract class projectID : PX.Data.BQL.BqlInt.Field<projectID> { }
        #endregion
        #region ProjectTaskID
        [PXDBInt(IsKey = true)]
        [PXSelector(typeof(PMTask.taskID),
                    typeof(PMTask.taskCD),
                    typeof(PMTask.description),
                    SubstituteKey = typeof(PMTask.taskCD))]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.ProjectTaskID)]
        public virtual int? ProjectTaskID { get; set; }
        public abstract class projectTaskID : PX.Data.BQL.BqlInt.Field<projectTaskID> { }
        #endregion
        #region CostCodeID
        [CostCode(null, typeof(projectTaskID), "E", IsKey = true, DisplayName = Messages.ATPTEFMMessages.CostCodeID)]
        //[PXUIField(DisplayName = Messages.ATPTEFMMessages.CostCodeID)]
        public virtual int? CostCodeID { get; set; }
        public abstract class costCodeID : PX.Data.BQL.BqlInt.Field<costCodeID> { }
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
        #region Year
        [PXDBString(4, IsUnicode = true)]
        public virtual string Year { get; set; }
        public abstract class year : PX.Data.BQL.BqlString.Field<year> { }
        #endregion
        #region FinPeriodID
        [PXDBString(6, IsKey = true, IsUnicode = true)]
        [PXDefault]
        //[SOFinPeriod]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.FinPeriodID)]
        public virtual string FinPeriodID { get; set; }
        public abstract class finPeriodID : PX.Data.BQL.BqlString.Field<finPeriodID> { }
        #endregion
        #region CuryID
        [PXDBString(5, IsUnicode = true, InputMask = ">LLLLL")]
        [PXSelector(typeof(Currency.curyID))]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.CuryID, IsReadOnly = true)]
        public virtual string CuryID { get; set; }
        public abstract class curyID : PX.Data.BQL.BqlString.Field<curyID> { }
        #endregion
        #region Amount
        [PXDBDecimal()]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.Amount)]
        [PXDefault(TypeCode.Decimal, "0.0", PersistingCheck = PXPersistingCheck.Nothing)]
        public virtual decimal? Amount { get; set; }
        public abstract class amount : PX.Data.BQL.BqlDecimal.Field<amount> { }
        #endregion
        #region CuryAmt
        [PXDecimal()]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.CuryAmt)]
        [PXUnboundDefault(TypeCode.Decimal, "0.0", PersistingCheck = PXPersistingCheck.Nothing)]
        public virtual decimal? CuryAmt { get; set; }
        public abstract class curyAmt : PX.Data.BQL.BqlDecimal.Field<curyAmt> { }
        #endregion
        #region IsActive
        [PXDBBool]
        [PXDefault(false, PersistingCheck = PXPersistingCheck.Nothing)]
        public virtual bool? IsActive { get; set; }
        public abstract class isActive : PX.Data.BQL.BqlBool.Field<isActive> { }
        #endregion
        #region IsApproved
        [PXDBBool]
        public virtual bool? IsApproved { get; set; }
        public abstract class isApproved : PX.Data.BQL.BqlBool.Field<isApproved> { }
        #endregion
        #region IsReleased
        [PXDBBool]
        public virtual bool? IsReleased { get; set; }
        public abstract class isReleased : PX.Data.BQL.BqlBool.Field<isReleased> { }
        #endregion

        #region Source
        [PXString(30)]
        public virtual String Source { get; set; }
        public abstract class source : PX.Data.BQL.BqlString.Field<source> { }
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