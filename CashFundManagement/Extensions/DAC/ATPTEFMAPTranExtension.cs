using CashFundManagement.Helper;
using PX.Data;
using PX.Objects.AP;

namespace CashFundManagement.Extensions.DAC {

    //TODO : remove dead codes
    /// <remarks>
    /// 2024-08-14 : Adds multi-tenant support. {RRS}
    /// </remarks>
    public sealed class ATPTEFMAPTranExtension : PXCacheExtension<APTran>
    {

#if Version23R2
        public static bool IsActive() => true;
#else
        public static bool IsActive() => ATPTEFMPrefetchSetup.IsActive;
#endif

        //#region UsrATPTVendID
        //public abstract class usrATPTVendID : PX.Data.BQL.BqlString.Field<usrATPTVendID> { }
        //protected String _UsrATPTVendID;
        //[PXDBString(30)]
        //[PXUIField(DisplayName = "Vendor ID")]
        //[PXDefault(PersistingCheck = PXPersistingCheck.Nothing)]
        //public string UsrATPTVendID
        //{
        //    get { return this._UsrATPTVendID; }
        //    set { this._UsrATPTVendID = value; }
        //}
        //#endregion

        //#region UsrATPTVendName
        //public abstract class usrATPTVendName : PX.Data.BQL.BqlString.Field<usrATPTVendName> { }
        //protected String _UsrATPTVendName;
        //[PXDBString(100)]
        //[PXUIField(DisplayName = "Vendor Name")]
        //[PXDefault(PersistingCheck = PXPersistingCheck.Nothing)]
        //public string UsrATPTVendName
        //{
        //    get { return this._UsrATPTVendName; }
        //    set { this._UsrATPTVendName = value; }
        //}

        //#endregion

        //#region UsrATPTVendTin
        //public abstract class usrATPTVendTin : PX.Data.BQL.BqlString.Field<usrATPTVendTin> { }
        //protected String _UsrATPTVendTin;
        //[PXDBString(50)]
        //[PXUIField(DisplayName = "TIN")]
        //[PXDefault(PersistingCheck = PXPersistingCheck.Nothing)]
        //public string UsrATPTVendTin
        //{
        //    get { return this._UsrATPTVendTin; }
        //    set { this._UsrATPTVendTin = value; }
        //}

        //#endregion

        //#region UsrATPTAddress
        //public abstract class usrATPTAddress : PX.Data.BQL.BqlString.Field<usrATPTAddress> { }
        //protected String _UsrATPTAddress;
        //[PXDBString(250)]
        //[PXUIField(DisplayName = "Address")]
        //[PXDefault(PersistingCheck = PXPersistingCheck.Nothing)]
        //public string UsrATPTAddress
        //{
        //    get { return this._UsrATPTAddress; }
        //    set { this._UsrATPTAddress = value; }
        //}
        //#endregion

        #region UsrATPTEFMClaimDetailCD
        [PXDBString(15, IsUnicode = true)]
        [PXUIField(DisplayName = "Receipt Number.", Enabled = false, IsReadOnly = true)]
        [PXDefault(PersistingCheck = PXPersistingCheck.Nothing)]
        public string UsrATPTEFMClaimDetailCD { get; set; }
        public abstract class usrATPTEFMClaimDetailCD : PX.Data.BQL.BqlString.Field<usrATPTEFMClaimDetailCD> { }
        #endregion
    }
}
