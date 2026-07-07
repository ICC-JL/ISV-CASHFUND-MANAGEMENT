using PX.Data;
using PX.Data.BQL;
using System;

namespace CashFundManagement.DAC {

    [Serializable]
    [PXCacheName(Messages.ATPTEFMMessages.ATPTEFMBudgetHistory)]
    public class ATPTEFMBudgetHistory : Base.ATPTEFMAudit, IBqlTable
    {
        #region RLBudgetHistoryID
        [PXDBLongIdentity]
        public virtual long? RLBudgetHistoryID { get; set; }
        public abstract class rLBudgetHistoryID : BqlLong.Field<rLBudgetHistoryID> { }
        #endregion

        #region Origin
        [PXDBInt(IsKey = true)]
        public int? Origin { get; set; }
        public abstract class origin : BqlInt.Field<origin> { }
        #endregion

        #region RefNbr
        [PXDBString(15, IsKey = true, IsUnicode = true)]
        public virtual String RefNbr { get; set; }
        public abstract class refNbr : BqlString.Field<refNbr> { }
        #endregion

        #region AccountID
        [PXDBInt(IsKey = true)]
        public int? AcctID { get; set; }
        public abstract class acctID : BqlInt.Field<acctID> { }
        #endregion

        #region SubID
        [PXDBInt(IsKey = true)]
        public int? SubID { get; set; }
        public abstract class subID : BqlInt.Field<subID> { }
        #endregion

        #region FinPeriodID
        [PXDBString(6, IsKey = true, IsUnicode = true)]
        public virtual String FinPeriodID { get; set; }
        public abstract class finPeriodID : BqlString.Field<finPeriodID> { }
        #endregion

        #region CuryID
        [PXDBString(IsFixed = true)]
        public virtual String CuryID { get; set; }
        public abstract class curyID : BqlString.Field<curyID> { }
        #endregion

        #region BudgetAmt
        [PXDBDecimal(2)]
        [PXDefault(TypeCode.Decimal, "0.0", PersistingCheck = PXPersistingCheck.Nothing)]
        public virtual decimal? BudgetAmt { get; set; }
        public abstract class budgetAmt : BqlDecimal.Field<budgetAmt> { }
        #endregion

        #region CuryAmt
        [PXDBDecimal(2)]
        [PXDefault(TypeCode.Decimal, "0.0", PersistingCheck = PXPersistingCheck.Nothing)]
        public virtual decimal? CuryAmt { get; set; }
        public abstract class curyAmt : BqlDecimal.Field<curyAmt> { }
        #endregion

        #region IsApproved
        [PXDBBool]
        public virtual bool? IsApproved { get; set; }
        public abstract class isApproved : BqlBool.Field<isApproved> { }
        #endregion

        #region IsReleased
        [PXDBBool]
        public virtual bool? IsReleased { get; set; }
        public abstract class isReleased : BqlBool.Field<isReleased> { }
        #endregion

        #region CuryTaxableAmt
        [PXDBDecimal(2)]
        [PXDefault(TypeCode.Decimal, "0.0", PersistingCheck = PXPersistingCheck.Nothing)]
        public virtual decimal? CuryTaxableAmt { get; set; }
        public abstract class curyTaxableAmt : BqlDecimal.Field<curyTaxableAmt> { }
        #endregion

        #region TaxableAmt
        [PXDBDecimal(2)]
        [PXDefault(TypeCode.Decimal, "0.0", PersistingCheck = PXPersistingCheck.Nothing)]
        public virtual decimal? TaxableAmt { get; set; }
        public abstract class taxableAmt : BqlDecimal.Field<taxableAmt> { }
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