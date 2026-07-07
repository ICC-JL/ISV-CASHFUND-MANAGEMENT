using CashFundManagement.Helper;
using CashFundManagement.Messages;
using PX.Data;
using PX.Data.BQL;
using PX.Objects.EP;

namespace CashFundManagement.Extensions.DAC {
    /// <remarks>
    /// 2024-08-14 : Adds multi-tenant support. {RRS}
    /// </remarks>
    public sealed class ATPTEFMEPSetupExtension : PXCacheExtension<EPSetup>
    {
#if Version23R2
        public static bool IsActive() => true;
#else
        public static bool IsActive() => ATPTEFMPrefetchSetup.IsActive;
#endif

        #region UsrATPTEFMIsRequireApprovalRFPBill
        [PXDBBool]
        [PXDefault(false, PersistingCheck = PXPersistingCheck.Nothing)]
        [PXUIField(DisplayName = ATPTEFMMessages.RequireApprovalOnRFPBill)]
        public bool? UsrATPTEFMIsRequireApprovalRFPBill { get; set; }
        public abstract class usrATPTEFMIsRequireApprovalRFPBill : BqlBool.Field<usrATPTEFMIsRequireApprovalRFPBill> { }
        #endregion

        #region UsrATPTEFMUseFinancialTabTaxZoneForDetails
        [PXDBBool]
        [PXDefault(false, PersistingCheck = PXPersistingCheck.Nothing)]
        [PXUIField(DisplayName = ATPTEFMMessages.UseTaxZoneInExpenseClaimFinancialTabForRFP)]
        public bool? UsrATPTEFMUseFinancialTabTaxZoneForDetails { get; set; }
        public abstract class usrATPTEFMUseFinancialTabTaxZoneForDetails : BqlBool.Field<usrATPTEFMUseFinancialTabTaxZoneForDetails> { }
        #endregion

        #region UsrATPTEFMRaiseErrorOnDuplicateRefNbr
        [PXDBBool]
        [PXDefault(false, PersistingCheck = PXPersistingCheck.Nothing)]
        [PXUIField(DisplayName = ATPTEFMMessages.RaiseErrorOnDuplicateRefNbr)]
        public bool? UsrATPTEFMRaiseErrorOnDuplicateRefNbr { get; set; }
        public abstract class usrATPTEFMRaiseErrorOnDuplicateRefNbr : BqlBool.Field<usrATPTEFMRaiseErrorOnDuplicateRefNbr> { }
        #endregion
    }
}
