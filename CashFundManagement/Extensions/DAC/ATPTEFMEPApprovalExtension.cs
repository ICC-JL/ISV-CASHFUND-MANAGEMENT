using CashFundManagement.Helper;
using CashFundManagement.Messages;
using PX.Data;
using PX.Data.BQL;
using PX.Objects.EP;

namespace CashFundManagement.Extensions.DAC {
    /// <remarks>
    /// 2024-08-14 : Adds multi-tenant support. {RRS}
    /// </remarks>
    public sealed class ATPTEFMEPApprovalExtension : PXCacheExtension<EPApproval> {
#if Version23R2
        public static bool IsActive() => true;
#else
        public static bool IsActive() => ATPTEFMPrefetchSetup.IsActive;
#endif

        #region UsrATPTEFMRFPReqRef
        public abstract class usrATPTEFMRFPReqRef : BqlString.Field<usrATPTEFMRFPReqRef> { }

        [PXDBString(20, InputMask = ">CCCCCCCCCCCCCCCCCCCC", IsUnicode = true)]
        [PXUIField(DisplayName = ATPTEFMMessages.ATPTFieldExtensions.LiquidationRFPNbr)]        
        public string UsrATPTEFMRFPReqRef { get; set; }
        #endregion
    }
}
