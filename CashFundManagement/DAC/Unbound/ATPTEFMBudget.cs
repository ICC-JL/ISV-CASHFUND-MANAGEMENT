using PX.Data;
using PX.Data.BQL;
using PX.Objects.GL;
using System;

namespace CashFundManagement.DAC.Unbound {
    [Serializable]
    [PXCacheName(Messages.ATPTEFMMessages.ATPTEFMBudget)]
    public class ATPTEFMBudget : CashFundManagement.DAC.Base.ATPTEFMBase, IBqlTable
    {
        #region AccountID
        [PXInt]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.AccountID)]
        [PXSelector(
            typeof(Search<Account.accountID>),
            typeof(Account.accountCD),
            typeof(Account.description),
            SubstituteKey = typeof(Account.accountCD))]
        public int? AcctID { get; set; }
        public abstract class acctID : BqlInt.Field<acctID> { }
        #endregion

        #region SubID
        //[PXUIField(DisplayName = Messages.ATPTEFMMessages.SubID)]
        [SubAccount(typeof(acctID), DisplayName = Messages.ATPTEFMMessages.SubID)]
        public int? SubID { get; set; }
        public abstract class subID : BqlInt.Field<subID> { }
        #endregion

        #region CuryID
        [PXString(5, IsUnicode = true)]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.CuryID)]
        public virtual string CuryID { get; set; }
        public abstract class curyID : BqlString.Field<curyID> { }
        #endregion

        #region InitialBudget
        [PXDecimal(2)]
        [PXDefault(TypeCode.Decimal, "0.0", PersistingCheck = PXPersistingCheck.Nothing)]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.InitialBudget)]
        public virtual decimal? InitialBudget { get; set; }
        public abstract class initialBudget : BqlDecimal.Field<initialBudget> { }
        #endregion

        #region DocAmt
        [PXDecimal(2)]
        [PXDefault(TypeCode.Decimal, "0.0", PersistingCheck = PXPersistingCheck.Nothing)]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.DocAmt)]
        public virtual decimal? DocAmt { get; set; }
        public abstract class docAmt : BqlDecimal.Field<docAmt> { }
        #endregion

        #region RequestAmt
        [PXDecimal(2)]
        [PXDefault(TypeCode.Decimal, "0.0", PersistingCheck = PXPersistingCheck.Nothing)]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.RequestAmt)]
        public virtual decimal? RequestAmt { get; set; }
        public abstract class requestAmt : BqlDecimal.Field<requestAmt> { }
        #endregion

        #region BudgetAmt
        [PXDecimal(2)]
        [PXDefault(TypeCode.Decimal, "0.0", PersistingCheck = PXPersistingCheck.Nothing)]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.BudgetAmt)]
        public virtual decimal? BudgetAmt { get; set; }
        public abstract class budgetAmt : BqlDecimal.Field<budgetAmt> { }
        #endregion

        #region SpentAmt
        [PXDecimal(2)]
        [PXDefault(TypeCode.Decimal, "0.0", PersistingCheck = PXPersistingCheck.Nothing)]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.SpentAmt)]
        public virtual decimal? SpentAmt { get; set; }
        public abstract class spentAmt : BqlDecimal.Field<spentAmt> { }
        #endregion

        #region ApprovedAmt
        [PXDecimal(2)]
        [PXDefault(TypeCode.Decimal, "0.0", PersistingCheck = PXPersistingCheck.Nothing)]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.ApprovedAmt)]
        public virtual decimal? ApprovedAmt { get; set; }
        public abstract class approvedAmt : BqlDecimal.Field<approvedAmt> { }
        #endregion

        #region ApprovedAdjAmt
        [PXDecimal(2)]
        [PXDefault(TypeCode.Decimal, "0.0", PersistingCheck = PXPersistingCheck.Nothing)]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.ApprovedAmt)]
        public virtual decimal? ApprovedAdjAmt { get; set; }
        public abstract class approvedAdjAmt : BqlDecimal.Field<approvedAdjAmt> { }
        #endregion

        #region CurrentApprovedAdjAmt
        [PXDecimal(2)]
        [PXDefault(TypeCode.Decimal, "0.0", PersistingCheck = PXPersistingCheck.Nothing)]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.ApprovedAmt)]
        public virtual decimal? CurrentApprovedAdjAmt { get; set; }
        public abstract class currentApprovedAdjAmt : BqlDecimal.Field<currentApprovedAdjAmt> { }
        #endregion

        #region ReturnAmt
        [PXDecimal(2)]
        [PXDefault(TypeCode.Decimal, "0.0", PersistingCheck = PXPersistingCheck.Nothing)]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.ReturnAmt)]
        public virtual decimal? ReturnAmt { get; set; }
        public abstract class returnAmt : BqlDecimal.Field<returnAmt> { }
        #endregion

        #region CurrentReturnAmt
        [PXDecimal(2)]
        [PXDefault(TypeCode.Decimal, "0.0", PersistingCheck = PXPersistingCheck.Nothing)]
        public virtual decimal? CurrentReturnAmt { get; set; }
        public abstract class currentReturnAmt : BqlDecimal.Field<currentReturnAmt> { }
        #endregion

        #region ReturnAmtAdj
        [PXDecimal(2)]
        [PXDefault(TypeCode.Decimal, "0.0", PersistingCheck = PXPersistingCheck.Nothing)]
        public virtual decimal? ReturnAmtAdj { get; set; }
        public abstract class returnAmtAdj : BqlDecimal.Field<returnAmtAdj> { }
        #endregion

        #region CurrentReturnAmtAdj
        [PXDecimal(2)]
        [PXDefault(TypeCode.Decimal, "0.0", PersistingCheck = PXPersistingCheck.Nothing)]
        public virtual decimal? CurrentReturnAmtAdj { get; set; }
        public abstract class currentReturnAmtAdj : BqlDecimal.Field<currentReturnAmtAdj> { }
        #endregion

        #region UnapprovedAmt
        [PXDecimal(2)]
        [PXDefault(TypeCode.Decimal, "0.0", PersistingCheck = PXPersistingCheck.Nothing)]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.UnapprovedAmt)]
        public virtual decimal? UnapprovedAmt { get; set; }
        public abstract class unapprovedAmt : BqlDecimal.Field<unapprovedAmt> { }
        #endregion

        #region POAmt
        //[PXDecimal(2)]
        //[PXDefault(TypeCode.Decimal, "0.0", PersistingCheck = PXPersistingCheck.Nothing)]
        //[PXUIField(DisplayName = "PO Amount")]
        //[PXUIVisible(typeof(Where<Current<RLFeatures.budgetPOAmount>, NotEqual<FEATURE_NA>>))]
        //public virtual decimal? POAmt { get; set; }
        //public abstract class pOAmt : BqlDecimal.Field<pOAmt> { }
        #endregion

        #region FinPeriodID
        [PXString]
        public virtual string FinPeriodID { get; set; }
        public abstract class finPeriodID : BqlString.Field<finPeriodID> { }
        #endregion

        #region Origin
        [PXInt]
        public int? Origin { get; set; }
        public abstract class origin : BqlInt.Field<origin> { }
        #endregion

        #region RefNbr
        [PXString(15)]
        public virtual String RefNbr { get; set; }
        public abstract class refNbr : BqlString.Field<refNbr> { }
        #endregion

        #region IsApproved
        [PXBool]
        [PXDefault(false, PersistingCheck = PXPersistingCheck.Nothing)]
        public virtual bool? IsApproved { get; set; }
        public abstract class isApproved : BqlBool.Field<isApproved> { }
        #endregion

        #region IsReleased
        [PXBool]
        [PXDefault(false, PersistingCheck = PXPersistingCheck.Nothing)]
        public virtual bool? IsReleased { get; set; }
        public abstract class isReleased : BqlBool.Field<isReleased> { }
        #endregion
    }
}