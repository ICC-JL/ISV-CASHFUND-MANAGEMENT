using CashFundManagement.Attributes;
using CashFundManagement.Messages;
using PX.Data;
using PX.Data.ReferentialIntegrity.Attributes;
using PX.Objects.Common;
using PX.Objects.CS;
using PX.Objects.GL;
using PX.Objects.RQ;
using System;

namespace CashFundManagement.DAC.Setup {
    [Serializable]
    [PXCacheName(ATPTEFMMessages.ATPTEFMReqClass)]
    [PXPrimaryGraph(typeof(BLC.ATPTEFMReqClassMaint))]
    public class ATPTEFMReqClass : CashFundManagement.DAC.Base.ATPTEFMAudit, IBqlTable
    {

        public class PK : PrimaryKeyOf<ATPTEFMReqClass>.By<tranType, reqClassID>
        {
            public static ATPTEFMReqClass Find(PXGraph graph, string tranType, string reqClassID) => FindBy(graph, tranType, reqClassID);
        }

        #region TranType
        [PXDBString(3, IsKey = true, IsFixed = true)]
        [PXUIField(DisplayName = "Transaction Type")]        
        [ATPTEFMTranTypeAttribute.ATPTEFMList()]
        [PXDefault(ATPTEFMTranTypeAttribute.CashAdvance)]
        [PX.Data.EP.PXFieldDescription]
        public virtual string TranType { get; set; }
        public abstract class tranType : PX.Data.BQL.BqlString.Field<tranType> { }
        #endregion

        #region ReqClassID
        [PXDBString(10, IsKey = true, IsUnicode = true)]
        [PXUIField(DisplayName = "Request Class ID", Visibility = PXUIVisibility.SelectorVisible)]
        [ATPTEFMReqClass(typeof(ATPTEFMReqClass), typeof(tranType))]        
        [PXDefault("", PersistingCheck = PXPersistingCheck.NullOrBlank)]
        [PX.Data.EP.PXFieldDescription]
        public virtual string ReqClassID { get; set; }
        public abstract class reqClassID : PX.Data.BQL.BqlString.Field<reqClassID> { }
        #endregion

        #region NumberingID
        [PXDBString(10, IsUnicode = true)]
        [PXDefault]
        [PXUIField(DisplayName = "Numbering ID")]
        [PXSelector(typeof(Numbering.numberingID), ValidateValue =true)]
        public virtual string NumberingID { get; set; }
        public abstract class numberingID : PX.Data.BQL.BqlString.Field<numberingID> { }
        #endregion

        #region Descr
        [PXDBString(60, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "Description", Visibility = PXUIVisibility.SelectorVisible)]
        public virtual string Descr { get; set; }
        public abstract class descr : PX.Data.BQL.BqlString.Field<descr> { }
        #endregion

        #region RestrictItemList
        [PXDBBool()]
        [PXDefault(false, PersistingCheck =PXPersistingCheck.Nothing)]
        [PXUIField(DisplayName = "Restrict Item List")]
        public virtual bool? RestrictItemList { get; set; }
        public abstract class restrictItemList : PX.Data.BQL.BqlBool.Field<restrictItemList> { }
        #endregion

        #region EnableDocumentOverride
        [PXDBBool()]
        [PXDefault(false, PersistingCheck = PXPersistingCheck.Nothing)]
        [PXUIField(DisplayName = "Enable Document Override", Visible = false)]
        public virtual bool? EnableDocumentOverride { get; set; }
        public abstract class enableDocumentOverride : PX.Data.BQL.BqlBool.Field<enableDocumentOverride> { }
        #endregion

        #region RestrictMultInvIns
        [PXDBBool()]
        [PXDefault(false, PersistingCheck = PXPersistingCheck.Nothing)]
        [PXUIField(DisplayName = "Restrict Multiple Inventory Instance")]
        public virtual bool? RestrictMultInvIns { get; set; }
        public abstract class restrictMultInvIns : PX.Data.BQL.BqlBool.Field<restrictMultInvIns> { }
        #endregion

        #region NoDaysLiquidate
        [PXDBInt()]
        [PXDefault(0,PersistingCheck =PXPersistingCheck.Nothing)]
        [PXUIField(DisplayName = "Number of Days to Liquidate")]
        [PXUIEnabled(typeof(Where<GetSetupValue<ATPTEFMCASetup.liquidationRule>, Equal<ATPTEFMLiquidationRuleAttribute.requestClass>>))]
        public virtual int? NoDaysLiquidate { get; set; }
        public abstract class noDaysLiquidate : PX.Data.BQL.BqlInt.Field<noDaysLiquidate> { }
        #endregion

        #region UseExpenseAcctFrom

        public abstract class useExpenseAcctFrom : PX.Data.BQL.BqlString.Field<useExpenseAcctFrom> { }
        protected string _UseExpenseAcctFrom;
        [PXDBString(1, IsFixed = true)]
        [PXDefault(RQAccountSource.None)]
        [PXUIField(DisplayName = "Use Expense Account From")]
        [RQAccountSource.List]
        public virtual string UseExpenseAcctFrom
        {
            get
            {
                return this._UseExpenseAcctFrom;
            }
            set
            {
                this._UseExpenseAcctFrom = value;
            }
        }
        #endregion
        #region CombineExpSub

        public abstract class combineExpSub : PX.Data.BQL.BqlString.Field<combineExpSub> { }
        protected String _CombineExpSub;
        [PXDefault(PersistingCheck = PXPersistingCheck.Nothing)]
        [SubAccountMask(DisplayName = "Combine Expense Sub. From")]
        public virtual String CombineExpSub
        {
            get
            {
                return this._CombineExpSub;
            }
            set
            {
                this._CombineExpSub = value;
            }
        }
        #endregion
        #region ExpenseAcctID

        public abstract class expenseAcctID : PX.Data.BQL.BqlInt.Field<expenseAcctID> { }
        protected Int32? _ExpenseAcctID;
        [PXDefault(PersistingCheck = PXPersistingCheck.Nothing)]
        [Account(DisplayName = "Expense Account", Visibility = PXUIVisibility.Visible, DescriptionField = typeof(Account.description), AvoidControlAccounts = true)]
        public virtual Int32? ExpenseAcctID
        {
            get
            {
                return this._ExpenseAcctID;
            }
            set
            {
                this._ExpenseAcctID = value;
            }
        }
        #endregion
        #region ExpenseSubID

        public abstract class expenseSubID : PX.Data.BQL.BqlInt.Field<expenseSubID> { }
        protected Int32? _ExpenseSubID;
        [PXDefault(PersistingCheck = PXPersistingCheck.Nothing)]
        [SubAccount(typeof(expenseAcctID), DisplayName = "Expense Sub", Visibility = PXUIVisibility.Visible, DescriptionField = typeof(Sub.description))]
        public virtual Int32? ExpenseSubID
        {
            get
            {
                return this._ExpenseSubID;
            }
            set
            {
                this._ExpenseSubID = value;
            }
        }
        #endregion

        #region Noteid
        [PXNote()]
        public virtual Guid? Noteid { get; set; }
        public abstract class noteid : PX.Data.BQL.BqlGuid.Field<noteid> { }
        #endregion

        #region IsImported
        [PXDBBool()]
        [PXDefault(false, PersistingCheck = PXPersistingCheck.Nothing)]
        public virtual bool? IsImported { get; set; }
        public abstract class isImported : PX.Data.BQL.BqlBool.Field<isImported> { }
        #endregion
    }
}