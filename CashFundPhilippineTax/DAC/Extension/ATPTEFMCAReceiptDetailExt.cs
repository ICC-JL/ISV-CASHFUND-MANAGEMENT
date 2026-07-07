using ATPTPhilippineTax.DAC.Extensions;
using CashFundManagement.Attributes;
using CashFundManagement.DAC;
using CashFundManagement.DAC.Setup;
using CashFundPhilippineTax.Attribute.Extension;
using PX.Data;
using PX.Data.BQL;
using PX.Objects.Common;
using PX.Objects.CS;
using PX.Objects.EP;
using CashFundManagement.Helper;

namespace CashFundPhilippineTax.DAC.Extension {
    /// <remarks>
    /// 03/05/25 - Override Vendor Details - 10398 - RFS
    /// </remarks>
    public sealed class ATPTEFMCAReceiptDetailExt : PXCacheExtension<ATPTEFMCAReceiptDetail> {
#if Version23R2
        public static bool IsActive() => ATPTPhilippineTax.Helpers.ATPTModule.IsActive;

#else
        public static bool IsActive() => ATPTPhilippineTax.Helpers.ATPTModule.IsActive && ATPTEFMPrefetchSetup.IsActive;
#endif

        [PXMergeAttributes(Method = MergeMethod.Merge)]
        [PXFormula(typeof(Selector<ATPTEFMCAReceiptDetail.inventoryID, ATPTInventoryItem.usrATPTDefaultATC>))]
        public string AtcCode { get; set; }

        #region VendorName
        [PXMergeAttributes(Method = MergeMethod.Merge)]
        [PXUIRequired(typeof(
            Where<GetSetupValue<ATPTEFMCASetup.requireVendorDetails>, Equal<True>,
                And<Where<Selector<ATPTEFMCAReceiptDetail.taxZoneID, ATPTTaxZone.usrATPTVendorNotRequired>, Equal<False>,
                    Or<Selector<ATPTEFMCAReceiptDetail.taxZoneID, ATPTTaxZone.usrATPTVendorNotRequired>, IsNull>>>>))]
        public string VendorName { get; set; }
        public abstract class vendorName : PX.Data.IBqlField { }
        #endregion

        #region VendorAddress
        [PXMergeAttributes(Method = MergeMethod.Merge)]
        [PXUIRequired(typeof(
            Where<GetSetupValue<ATPTEFMCASetup.requireVendorDetails>, Equal<True>,
                And<Where<Selector<ATPTEFMCAReceiptDetail.taxZoneID, ATPTTaxZone.usrATPTVendorNotRequired>, Equal<False>,
                    Or<Selector<ATPTEFMCAReceiptDetail.taxZoneID, ATPTTaxZone.usrATPTVendorNotRequired>, IsNull>>>>))]
        public string VendorAddress { get; set; }
        public abstract class vendorAddress : PX.Data.IBqlField { }
        #endregion

        #region VendorTin
        [PXMergeAttributes(Method = MergeMethod.Merge)]
        [PXUIRequired(typeof(
            Where<GetSetupValue<ATPTEFMCASetup.requireVendorDetails>, Equal<True>,
                And<Where<Selector<ATPTEFMCAReceiptDetail.taxZoneID, ATPTTaxZone.usrATPTVendorNotRequired>, Equal<False>,
                    Or<Selector<ATPTEFMCAReceiptDetail.taxZoneID, ATPTTaxZone.usrATPTVendorNotRequired>, IsNull>>>>))]
        public string VendorTin { get; set; }
        public abstract class vendorTin : PX.Data.IBqlField { }
        #endregion
    }
}
