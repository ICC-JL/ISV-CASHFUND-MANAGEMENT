using CashFundManagement.Helper;
using PX.Data;
using PX.Objects.AP;
using System;

namespace CashFundManagement.Extensions.BLC
{
    /// <remarks>
    /// 2024-08-14 : Adds multi-tenant support. {RRS}
    /// </remarks>
    public class ATPTEFMVendorMaintExtension : PXGraphExtension<VendorMaint>
    {
#if Version23R2
        public static bool IsActive() => true;
#else
        public static bool IsActive() => ATPTEFMPrefetchSetup.IsActive;
#endif

        protected virtual void _(Events.RowSelected<Vendor> e)
        {
            PXUIFieldAttribute.SetRequired<Vendor.termsID>(e.Cache, true);
        }

        public delegate void PersistDelegate();
        [PXOverride]
        public void Persist(PersistDelegate baseMethod)
        {
            if (Base.CurrentVendor.Current != null)
            {
                if (string.IsNullOrWhiteSpace(Base.CurrentVendor.Current.TermsID))
                    throw new Exception(Messages.ATPTEFMMessages.VendorTermsIsEmpty);
            }

            baseMethod();
        }  
    }
}
