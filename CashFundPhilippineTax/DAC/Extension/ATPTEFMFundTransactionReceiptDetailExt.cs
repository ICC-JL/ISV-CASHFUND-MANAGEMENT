using ATPTPhilippineTax.DAC.Extensions;
using CashFundManagement.DAC;
using CashFundManagement.DAC.Setup;
using CashFundPhilippineTax.Attribute.Extension;
using PX.Data;
using PX.Data.BQL;
using PX.Objects.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CashFundManagement.Helper;

namespace CashFundPhilippineTax.DAC.Extension
{
    /// <remarks>
    /// 03/05/25 - Override Vendor Details - 10398 - RFS
    /// </remarks>
    public sealed class ATPTEFMFundTransactionReceiptDetailExt : PXCacheExtension<ATPTEFMFundTransactionReceiptDetail>
    {
#if Version23R2
        public static bool IsActive() => ATPTPhilippineTax.Helpers.ATPTModule.IsActive;

#else
        public static bool IsActive() => ATPTPhilippineTax.Helpers.ATPTModule.IsActive && ATPTEFMPrefetchSetup.IsActive;
#endif

        #region VendorName
        [PXMergeAttributes(Method = MergeMethod.Merge)]
        [PXUIRequired(typeof(
            Where<GetSetupValue<ATPTEFMSetup.requireVendorDetails>, Equal<True>,
                And<Where<Selector<ATPTEFMFundTransactionReceiptDetail.taxZoneID, ATPTTaxZone.usrATPTVendorNotRequired>, Equal<False>,
                    Or<Selector<ATPTEFMFundTransactionReceiptDetail.taxZoneID, ATPTTaxZone.usrATPTVendorNotRequired>, IsNull>>>>))]
        public string VendorName { get; set; }
        public abstract class vendorName : PX.Data.IBqlField { }
        #endregion

        #region VendorAddress
        [PXMergeAttributes(Method = MergeMethod.Merge)]
        [PXUIRequired(typeof(
            Where<GetSetupValue<ATPTEFMSetup.requireVendorDetails>, Equal<True>,
                And<Where<Selector<ATPTEFMFundTransactionReceiptDetail.taxZoneID, ATPTTaxZone.usrATPTVendorNotRequired>, Equal<False>,
                    Or<Selector<ATPTEFMFundTransactionReceiptDetail.taxZoneID, ATPTTaxZone.usrATPTVendorNotRequired>, IsNull>>>>))]
        public string VendorAddress { get; set; }
        public abstract class vendorAddress : PX.Data.IBqlField { }
        #endregion

        #region VendorTin
        [PXMergeAttributes(Method = MergeMethod.Merge)]
        [PXUIRequired(typeof(
            Where<GetSetupValue<ATPTEFMSetup.requireVendorDetails>, Equal<True>,
                And<Where<Selector<ATPTEFMFundTransactionReceiptDetail.taxZoneID, ATPTTaxZone.usrATPTVendorNotRequired>, Equal<False>,
                    Or<Selector<ATPTEFMFundTransactionReceiptDetail.taxZoneID, ATPTTaxZone.usrATPTVendorNotRequired>, IsNull>>>>))]
        public string VendorTin { get; set; }
        public abstract class vendorTin : PX.Data.IBqlField { }
        #endregion
    }
}
