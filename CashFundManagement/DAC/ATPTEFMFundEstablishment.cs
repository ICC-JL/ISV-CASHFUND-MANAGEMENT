using PX.Data;
using PX.Data.BQL;
using System;

namespace CashFundManagement.DAC {
    [Serializable]
    [PXCacheName("Fund Establishment")]
    public class ATPTEFMFundEstablishment : Base.ATPTEFMAudit, IBqlTable
    {
        #region EstablishmentID
        [PXDBIdentity(IsKey = true)]
        public virtual int? EstablishmentID { get; set; }
        public abstract class establishmentID : BqlInt.Field<establishmentID> { }
        #endregion

        #region FundRefNbr
        [PXDBString(15, IsUnicode = true)]
        [PXDBDefault(typeof(ATPTEFMFund.fundCD))]
        [PXParent(typeof(Select<
            ATPTEFMFund,
            Where<ATPTEFMFund.fundCD, Equal<Current<ATPTEFMFundEstablishment.fundRefNbr>>>>))]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.FundTransactionRefNbr)]
        public virtual string FundRefNbr { get; set; }
        public abstract class fundRefNbr : BqlString.Field<fundRefNbr> { }
        #endregion

        #region EstablishmentRefNbr
        [PXDBString(15, IsUnicode = true)]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.EstablishmentAPRef)]
        [PXDefault(PersistingCheck = PXPersistingCheck.Nothing)]
        public virtual string EstablishmentRefNbr { get; set; }
        public abstract class establishmentRefNbr : BqlString.Field<establishmentRefNbr> { }
        #endregion

        #region NoteID
        [PXNote]
        public virtual Guid? NoteID { get; set; }
        public abstract class noteID : BqlGuid.Field<noteID> { }
        #endregion

    }
}
