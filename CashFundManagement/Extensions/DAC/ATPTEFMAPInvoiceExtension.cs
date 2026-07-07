using CashFundManagement.Attributes;
using CashFundManagement.Helper;
using PX.Data;
using PX.Objects.AP;

namespace CashFundManagement.Extensions.DAC
{
    /// <remarks>
    /// 2024-08-14 : Adds multi-tenant support. {RRS}
    /// 2024-11-07 : Comment out the InvoiceNbr override for throwing duplicate refnbr on APVendorRefnbr. <br/>
    ///             Implementation is on ATPTEFMAPInvoiceEntry_Extension under field defaulting of APInvoice.invoiceNbr. {RRS}
    /// </remarks>
    public sealed class ATPTEFMAPInvoiceExtension : PXCacheExtension<APInvoice>
    {
#if Version23R2
        public static bool IsActive() => true;
#else
        public static bool IsActive() => ATPTEFMPrefetchSetup.IsActive;
#endif

        //#region InvoiceNbr
        //[PXMergeAttributes(Method = MergeMethod.Append)]
        //[PXRemoveBaseAttribute(typeof(APVendorRefNbrAttribute))]
        //[ATPTEFMAPVendorRefNbr]
        //public string InvoiceNbr { get; set; }
        //public abstract class invoiceNbr : BqlString.Field<invoiceNbr> { }
        //#endregion

        #region UsrATPTEFMBudgetEnabled
        [PXDBBool()]
        [PXDefault(PersistingCheck = PXPersistingCheck.Nothing)]
        public bool? UsrATPTEFMBudgetEnabled { get; set; }
        public abstract class usrATPTEFMBudgetEnabled : PX.Data.BQL.BqlBool.Field<usrATPTEFMBudgetEnabled> { }
        #endregion
    }
}
