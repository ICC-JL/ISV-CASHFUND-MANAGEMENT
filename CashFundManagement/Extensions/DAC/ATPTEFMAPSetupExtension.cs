using CashFundManagement.Helper;
using PX.Data;
using PX.Objects.AP;
using PX.Objects.GL;
using PX.Objects.RQ;
using System;
using messages = CashFundManagement.Messages.ATPTEFMMessages;

namespace CashFundManagement.Extensions.DAC {

    //TODO : delete this class, remove on screen AP101000
    /// <remarks>
    /// 2024-08-14 : make IsActive to false, this is not used anymore .{RRS}
    /// </remarks>
    [Serializable]
    public sealed class ATPTEFMAPSetupExtension : PXCacheExtension<APSetup>
    {
        public static bool IsActive() => false;

        #region UsrATPTEFMDefaultReqClassCD
        [PXDBString(15)]
        [PXSelector(typeof(RQRequestClass.reqClassID),
                typeof(RQRequestClass.reqClassID),
                typeof(RQRequestClass.descr),
                DescriptionField = typeof(RQRequestClass.descr))]
        [PXUIField(DisplayName = messages.UsrRLDefaultReqClassCD)]
        public string UsrATPTEFMDefaultReqClassCD { get; set; }
        public abstract class usrATPTEFMDefaultReqClassCD : IBqlField { }
        #endregion
        #region UsrATPTEFMBudgetValidation
        //[PXDBInt()]
        //[PXDefault(RQRequestClassBudget.None, PersistingCheck = PXPersistingCheck.Nothing)]
        //[RQRequestClassBudget.List]
        //[PXUIField(DisplayName = messages.UsrRLBudgetValidation, Visibility = PXUIVisibility.Visible)]
        //public int? UsrATPTEFMBudgetValidation { get; set; }
        //public abstract class usrATPTEFMBudgetValidation : IBqlField { }
        #endregion
        #region UsrATPTEFMBudgetCalculation
        //[PXDBString(1, IsFixed = true)]
        //[PXDefault(RQBudgetCalculationType.YTD, PersistingCheck = PXPersistingCheck.Nothing)]
        //[PXUIField(DisplayName = Messages.ATPTEFMMessages.UsrRLBudgetCalculation)]
        //[RQBudgetCalculationType.List]
        //public string UsrATPTEFMBudgetCalculation { get; set; }
        //public abstract class usrATPTEFMBudgetCalculation : PX.Data.BQL.BqlString.Field<usrATPTEFMBudgetCalculation> { }
        #endregion
        #region UsrATPTEFMExpenseAccountDefault
        protected string _ExpenseAccountDefault;
        [PXDBString(1, IsFixed = true)]
        [PXDefault(RQAccountSource.None, PersistingCheck = PXPersistingCheck.Nothing)]
        [RQAccountSource.List]
        [PXUIField(DisplayName = messages.UsrRLExpenseAccountDefault)]
        public string UsrATPTEFMExpenseAccountDefault { get; set; }
        public abstract class usrATPTEFMExpenseAccountDefault : IBqlField { }
        #endregion
        #region UsrATPTEFMExpenseSubMask
        [PXDBString(10, InputMask = "##########")]
        [PXDefault("DDDDDDDDDD", PersistingCheck = PXPersistingCheck.Nothing)]
        [PXUIField(DisplayName = messages.UsrRLExpenseSubMask)]
        public string UsrATPTEFMExpenseSubMask { get; set; }
        public abstract class usrATPTEFMExpenseSubMask : IBqlField { }
        #endregion
        #region UsrATPTEFMExpenseAcctID
        [PXDBInt]
        [PXDefault(PersistingCheck = PXPersistingCheck.Nothing)]
        [PXSelector(typeof(Account.accountID),
                typeof(Account.accountCD),
                typeof(Account.accountClassID),
                typeof(Account.type),
                typeof(Account.description),
                typeof(Account.curyID),
                typeof(Account.accountGroupID),
                SubstituteKey = typeof(Account.accountCD),
                DescriptionField = typeof(Account.description))]
        [PXUIField(DisplayName = messages.UsrRLExpenseAcctID)]
        public int? UsrATPTEFMExpenseAcctID { get; set; }
        public abstract class usrATPTEFMExpenseAcctID : IBqlField { }
        #endregion
        #region UsrATPTEFMExpenseSubID
        [PXDBInt]
        [PXDefault(PersistingCheck = PXPersistingCheck.Nothing)]
        [PXSelector(typeof(Sub.subID),
                typeof(Sub.subCD),
                typeof(Sub.description),
                SubstituteKey = typeof(Sub.subCD),
                DescriptionField = typeof(Sub.description))]
        [PXUIField(DisplayName = messages.UsrRLExpenseSubID)]
        public int? UsrATPTEFMExpenseSubID { get; set; }
        public abstract class usrATPTEFMExpenseSubID : IBqlField { }
        #endregion
        #region UsrATPTEFMBudgetLedgerID
        //[PXDBInt]
        //[PXDefault(PersistingCheck = PXPersistingCheck.Nothing)]
        //[PXUIField(DisplayName = Messages.ATPTEFMMessages.UsrRLBudgetLedgerID, Required = true)]
        //[PXSelector(typeof(Search<Ledger.ledgerID, Where<Ledger.balanceType, Equal<LedgerBalanceType.budget>>>),
        //    SubstituteKey = typeof(Ledger.ledgerCD),
        //    DescriptionField = typeof(Ledger.descr))]
        //public int? UsrATPTEFMBudgetLedgerID { get; set; }
        //public abstract class usrATPTEFMBudgetLedgerID : PX.Data.BQL.BqlInt.Field<usrATPTEFMBudgetLedgerID> { }
        #endregion

    }

}
