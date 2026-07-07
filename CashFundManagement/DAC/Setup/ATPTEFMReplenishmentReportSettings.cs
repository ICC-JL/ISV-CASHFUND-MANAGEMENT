using PX.Data;
using PX.Data.BQL;
using PX.Objects.IN;
using System;

namespace CashFundManagement.DAC.Setup {
    [Serializable]
    [PXCacheName(Messages.ATPTEFMMessages.ATPTEFMReplenishmentReportSettings)]
    public class ATPTEFMReplenishmentReportSettings : Base.ATPTEFMAudit, IBqlTable
    {
        #region Replenishment Report Settings ID 
        [PXDBIdentity(IsKey = true)]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.ColumnOrder)]
        public virtual int? ReplenishmentID { get; set; }
        public abstract class replenishmentID : BqlInt.Field<replenishmentID> { }
        #endregion

        #region IsActive
        [PXDBBool()]
        [PXDefault(false)]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.Active)]
        public virtual bool? IsActive { get; set; }

        public abstract class isActive : BqlBool.Field<isActive> { }
        #endregion

        #region Item Class
        [PXDBString(IsUnicode =true)]
        [PXSelector(typeof(Search2<INItemClass.itemClassCD, InnerJoin<InventoryItem,
            On<INItemClass.itemClassID, Equal<InventoryItem.itemClassID>>>,
            Where<INItemClass.itemClassID, Equal<InventoryItem.itemClassID>>>))]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.ItemClass)]
        public string ItemClass { get; set; }
        public abstract class itemClass : BqlString.Field<itemClass> { }
        #endregion

        #region Description
        [PXDBString(255, IsUnicode = true)]
        [PXDefault(typeof(Search<INItemClass.descr,
            Where<INItemClass.itemClassCD, Equal<Current<itemClass>>>>), PersistingCheck = PXPersistingCheck.Nothing)]
        [PXFormula(typeof(Default<itemClass>))]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.Description, Enabled = false)]
        public virtual string Descr { get; set; }
        public abstract class descr : BqlString.Field<descr> { }
        #endregion


        #region Fund Type
        [PXDBString(255, IsUnicode = true)]
        [PXDefault(typeof(ATPTEFMSetup.fundType), PersistingCheck = PXPersistingCheck.Nothing)]
        [PXFormula(typeof(Default<ATPTEFMSetup.fundType>))]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.FundType)]
        public virtual string FundType { get; set; }
        public abstract class fundType : BqlString.Field<fundType> { }
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
