using CashFundManagement.BLC;
using PX.Data;
using PX.Data.BQL;
using PX.Objects.TX;
using System;

namespace CashFundManagement.DAC {
    [PXPrimaryGraph(typeof(ATPTEFMReplenishmentEntry))]
    [Serializable]
    [PXCacheName(Messages.ATPTEFMMessages.ATPTEFMReplenishmentTaxDetail)]
    public class ATPTEFMReplenishmentTaxDetail : Base.ATPTEFMAudit, IBqlTable
    {
        #region ReplenishmentTaxDetailID
        [PXDBIdentity(IsKey = true)]
        public virtual int? ReplenishmentTaxDetailID { get; set; }
        public abstract class replenishmentTaxDetailID : BqlInt.Field<replenishmentTaxDetailID> { }
        #endregion

        #region ReplenishmentNbr
        [PXDBString(15, IsUnicode =true)]
        [PXDBDefault(typeof(ATPTEFMReplenishment.replenishmentNbr))]
        [PXParent(typeof(Select<
            ATPTEFMReplenishment,
            Where<ATPTEFMReplenishment.replenishmentNbr, Equal<Current<replenishmentNbr>>>>))]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.ReplenishmentID)]
        public virtual string ReplenishmentNbr { get; set; }
        public abstract class replenishmentNbr : BqlString.Field<replenishmentNbr> { }
        #endregion

        #region TaxID
        [PXDBString(Tax.taxID.Length, IsUnicode = true, InputMask = ">CCCCCCCCCCCCCCCCCCCCCCCCCCCCCC")]
        [PXDefault()]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.TaxID, Visibility = PXUIVisibility.Visible)]
        [PXSelector(typeof(Tax.taxID), DescriptionField = typeof(Tax.descr), DirtyRead = true)]
        public virtual string TaxID { get; set; }
        public new abstract class taxID : PX.Data.BQL.BqlString.Field<taxID> { }
        #endregion

        #region Tax Type
        [PXString(1, IsFixed = true)]
        [PXUnboundDefault(typeof(Search<Tax.taxType, Where<Tax.taxID, Equal<Current<taxID>>>>))]
        [CSTaxType.List]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.TaxType, Visibility = PXUIVisibility.Visible)]
        public virtual string TaxType { get; set; }
        public new abstract class taxType : PX.Data.BQL.BqlString.Field<taxType> { }

        #endregion

        #region TaxRate
        [PXDBDecimal(6)]
        [PXDefault(TypeCode.Decimal, "0.0")]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.TaxRate, Visibility = PXUIVisibility.Visible, Enabled = false)]
        public virtual decimal? TaxRate { get; set; }
        public abstract class taxRate : PX.Data.BQL.BqlDecimal.Field<taxRate> { }
        #endregion

        #region TaxableAmt
        [PXDBDecimal(4)]
        [PXDefault(TypeCode.Decimal, "0.0")]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.TaxableAmount, Visibility = PXUIVisibility.Visible)]
        public virtual decimal? TaxableAmt { get; set; }
        public abstract class taxableAmt : PX.Data.BQL.BqlDecimal.Field<taxableAmt> { }
        #endregion

        #region TaxAmt
        public abstract class taxAmt : BqlDecimal.Field<taxAmt> { }
        [PXDBDecimal(4)]
        [PXDefault(TypeCode.Decimal, "0.0")]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.TaxAmount, Visibility = PXUIVisibility.Visible)]
        public virtual decimal? TaxAmt { get; set; }
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

        #region IsImported
        [PXDBBool()]
        [PXDefault(false, PersistingCheck = PXPersistingCheck.Nothing)]
        public virtual bool? IsImported { get; set; }
        public abstract class isImported : BqlBool.Field<isImported> { }
        #endregion

    }

}
