using CashFundManagement.Messages;
using PX.Data;
using PX.Data.BQL;
using System;

namespace CashFundManagement.DAC.Setup {
    //TODO : Remove dead codes on next upgrade.
    [Serializable]
    [PXCacheName(ATPTEFMMessages.ATPTEFM2023R2Enhancements)]
    public class ATPTEFM2023R2Enhancements : Base.ATPTEFMAudit, IBqlTable
    {
        #region NoteID
        [PXNote]
        public virtual Guid? NoteID { get; set; }
        public abstract class noteID : BqlGuid.Field<noteID> { }
        #endregion

        //#region ValidateAmountReceivedAndReleasedUponLiquidation
        //[PXDBBool]
        //[PXUIField(DisplayName = ATPTEFMMessages.ValidateAmountReceivedAndReleasedUponLiquidation)]
        //public virtual bool? ValidateAmountReceivedAndReleasedUponLiquidation { get; set; }
        //public abstract class validateAmountReceivedAndReleasedUponLiquidation : PX.Data.BQL.BqlBool.Field<validateAmountReceivedAndReleasedUponLiquidation> { }
        //#endregion

        //#region EnableFundTransactionLimit
        //[PXDBBool]
        //[PXUIField(DisplayName = ATPTEFMMessages.EnableFundTransactionLimit)]
        //public virtual bool? EnableFundTransactionLimit { get; set; }
        //public abstract class enableFundTransactionLimit : PX.Data.BQL.BqlBool.Field<enableFundTransactionLimit> { }
        //#endregion

        /*#region CloseFundWithoutReplenishment
        [PXDBBool]
        [PXDefault(false, PersistingCheck = PXPersistingCheck.Nothing)]
        [PXUIField(DisplayName = "Close fund without replenishment")]
        public virtual bool? CloseFundWithoutReplenishment { get; set; }
        public abstract class closeFundWithoutReplenishment : PX.Data.BQL.BqlBool.Field<closeFundWithoutReplenishment> { }
        #endregion*/

        /*#region IncreaseDecreaseFund
        [PXDBBool]
        [PXDefault(false, PersistingCheck = PXPersistingCheck.Nothing)]
        [PXUIField(DisplayName = "Increase/Decrease Fund")]
        public virtual bool? IncreaseDecreaseFund { get; set; }
        public abstract class increaseDecreaseFund : PX.Data.BQL.BqlBool.Field<increaseDecreaseFund> { }
        #endregion*/

        /*#region Increase/Decrease Fund Approval Setup
        #region RequireApprovalOnFundIncreaseCredAdj
        [PXDBBool]
        [PXDefault(false, PersistingCheck = PXPersistingCheck.Nothing)]
        [PXUIField(DisplayName = ATPTEFMMessages.RequireApprovalOnFundIncreaseCredAdj)]
        public virtual bool? RequireApprovalOnFundIncreaseCredAdj { get; set; }
        public abstract class requireApprovalOnFundIncreaseCredAdj : PX.Data.BQL.BqlBool.Field<requireApprovalOnFundIncreaseCredAdj> { }
        #endregion

        #region RequireApprovalOnFundDecreaseDebAdj
        [PXDBBool]
        [PXDefault(false, PersistingCheck = PXPersistingCheck.Nothing)]
        [PXUIField(DisplayName = ATPTEFMMessages.RequireApprovalOnFundDecreaseDebAdj)]
        public virtual bool? RequireApprovalOnFundDecreaseDebAdj { get; set; }
        public abstract class requireApprovalOnFundDecreaseDebAdj : PX.Data.BQL.BqlBool.Field<requireApprovalOnFundDecreaseDebAdj> { }
        #endregion
        #endregion*/

        //#region FundRequestReclassicationSetup

        //#region NoOfDaysToLiquidate
        //[PXDBInt]
        //[PXUIField(DisplayName = ATPTEFMMessages.NoOfDaysToLiquidate)]
        //public virtual int? NoOfDaysToLiquidate { get; set; }
        //public abstract class noOfDaysToLiquidate : PX.Data.BQL.BqlInt.Field<noOfDaysToLiquidate> { }
        //#endregion

        //#region ReclassificationItem
        //[PXDBInt]
        //[PXUIField(DisplayName = Messages.ATPTEFMMessages.ReclassificationItem)]
        //[PXSelector(typeof(Search<InventoryItem.inventoryID, Where<InventoryItem.itemType, Equal<INItemTypes.expenseItem>, And<InventoryItem.itemStatus, Equal<InventoryItemStatus.active>>>>), 
        //    typeof(InventoryItem.inventoryID), 
        //    typeof(InventoryItem.inventoryCD), 
        //    typeof(InventoryItem.descr), 
        //    typeof(InventoryItem.taxCategoryID),
        //    SubstituteKey = typeof(InventoryItem.inventoryCD), 
        //    DescriptionField = typeof(InventoryItem.descr))]
        //public virtual int? ReclassificationItem { get; set; }
        //public abstract class reclassificationItem : PX.Data.BQL.BqlInt.Field<reclassificationItem> { }
        //#endregion

        //#endregion
    }
}
