using CashFundManagement.Attributes;
using CashFundManagement.DAC.Setup;
using CashFundManagement.Helper;
using CashFundManagement.Messages;
using PX.Data;
using PX.Objects.Common;
using PX.Objects.CS;
using PX.Objects.EP;
using System;

namespace CashFundManagement.Extensions.DAC {
    /// <remarks>
    /// 2024-08-14 : Adds multi-tenant support. {RRS}
    /// </remarks>
    public sealed class ATPTEFMEPEmployeeExtension : PXCacheExtension<EPEmployee>
    {
#if Version23R2
        public static bool IsActive() => true;
#else
        public static bool IsActive() => ATPTEFMPrefetchSetup.IsActive;
#endif

        #region UsrATPTEFMAllowableDays
        [PXDBInt]
        [PXDefault(TypeCode.Int32, "0", PersistingCheck = PXPersistingCheck.Nothing)]
        [PXUIField(DisplayName = ATPTEFMMessages.AllowableDays)]
        [PXUIEnabled(typeof(Where<GetSetupValue<ATPTEFMCASetup.liquidationRule>, Equal<ATPTEFMLiquidationRuleAttribute.employee>>))]
        public int? UsrATPTEFMAllowableDays { get; set; }
        public abstract class usrATPTEFMAllowableDays : PX.Data.BQL.BqlInt.Field<usrATPTEFMAllowableDays> { }
        #endregion

        #region UsrATPTEFMMaxCAAmt
        [PXDBDecimal()]
        [PXDefault(TypeCode.Decimal, "0.00", PersistingCheck = PXPersistingCheck.Nothing)]
        [PXUIField(DisplayName = ATPTEFMMessages.MaxCAAmt)]
        [PXUIEnabled(typeof(Where<GetSetupValue<ATPTEFMCASetup.allowableCAAmt>, Equal<boolTrue>>))]
        public decimal? UsrATPTEFMMaxCAAmt { get; set; }
        public abstract class usrATPTEFMMaxCAAmt : PX.Data.BQL.BqlDecimal.Field<usrATPTEFMMaxCAAmt> { }
        #endregion

        #region UsrATPTEFMOpenCA
        [PXDBInt]
        [PXDefault(TypeCode.Int32, "0", PersistingCheck = PXPersistingCheck.Nothing)]
        [PXUIField(DisplayName = ATPTEFMMessages.NoOpenCA)]
        [PXUIEnabled(typeof(Where<GetSetupValue<ATPTEFMCASetup.allowableOpenCA>, Equal<boolTrue>>))]
        public int? UsrATPTEFMOpenCA { get; set; }
        public abstract class usrATPTEFMOpenCA : PX.Data.BQL.BqlInt.Field<usrATPTEFMOpenCA> { }
        #endregion

        #region UsrATPTEFMCAUser
        [PXDBBool]
        [PXDefault(false, PersistingCheck = PXPersistingCheck.Nothing)]
        [PXUIField(DisplayName = ATPTEFMMessages.CAUser)]
        [PXUIEnabled(typeof(Where<GetSetupValue<ATPTEFMCASetup.restrictCAEmployees>, Equal<boolTrue>>))]
        public Boolean? UsrATPTEFMCAUser { get; set; }
        public abstract class usrATPTEFMCAUser : PX.Data.BQL.BqlBool.Field<usrATPTEFMCAUser> { }
        #endregion

    }
}