using CashFundManagement.Messages;
using PX.Data;
using PX.Data.BQL;
using PX.Objects.CS;
using PX.Objects.IN;
using System;

namespace CashFundManagement.DAC.Setup {
    [Serializable]
    [PXCacheName(ATPTEFMMessages.ATPTEFMReqClassItems)]
    public class ATPTEFMReqClassItems : CashFundManagement.DAC.Base.ATPTEFMAudit, IBqlTable
    {

        #region ReqClassItemsID
        [PXDBIdentity(IsKey = true)]
        public virtual int? ReqClassItemsID { get; set; }
        public abstract class reqClassItemsID : PX.Data.IBqlField { }
        #endregion

        #region TranType
        [PXDBString(3, IsFixed = true)]
        [PXUIField(DisplayName = "Tran Type")]
        [PXDefault(typeof(ATPTEFMReqClass.tranType))]
        public virtual string TranType { get; set; }
        public abstract class tranType : PX.Data.BQL.BqlString.Field<tranType> { }
        #endregion

        #region ReqClassID
        [PXDBString(10, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "Req Class ID")]
        [PXDBDefault(typeof(ATPTEFMReqClass.reqClassID))]
        [PXParent(typeof(Select<
            ATPTEFMReqClass,
            Where<ATPTEFMReqClass.reqClassID, Equal<Current<ATPTEFMReqClassItems.reqClassID>>>>))]
        public virtual string ReqClassID { get; set; }
        public abstract class reqClassID : PX.Data.BQL.BqlString.Field<reqClassID> { }
        #endregion

        #region InventoryID        
        [NonStockItem(DisplayName = "Inventory ID")]
        [PXRestrictor(typeof(Where<InventoryItem.itemType, Equal<INItemTypes.expenseItem>, And<InventoryItem.itemStatus, Equal<InventoryItemStatus.active>>>), "Expense Types")]
        public virtual int? InventoryID { get; set; }
        public abstract class inventoryID : PX.Data.BQL.BqlInt.Field<inventoryID> { }
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
        public abstract class isImported : BqlBool.Field<isImported> { }
        #endregion

        #region Amount
        [PXDBDecimal(2)]
        [PXDefault(TypeCode.Decimal, "0.0")]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.Amount, Enabled = false)]
        [PXUIEnabled(typeof(Where<Current<isPerDiem>, Equal<boolTrue>>))]
        public virtual decimal? Amount { get; set; }
        public abstract class amount : BqlDecimal.Field<amount> { }
        #endregion

        #region IsPerDiem
        [PXDBBool]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.PerDiem)]
        [PXDefault(false, PersistingCheck = PXPersistingCheck.Nothing)]
        public virtual bool? IsPerDiem { get; set; }
        public abstract class isPerDiem : BqlBool.Field<isPerDiem> { }
        #endregion
    }
}