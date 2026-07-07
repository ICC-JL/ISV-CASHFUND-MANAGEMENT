using CashFundManagement.BLC;
using CashFundManagement.DAC;
using CashFundManagement.Helper;
using PX.Data;
using PX.Objects.EP;
using PX.TM;

namespace CashFundManagement.Extensions.BLC
{
    /// <remarks>
    /// 2024-08-14 : Adds multi-tenant support. {RRS}
    /// </remarks>
    public class ATPTEFMReplenishmentMaintExtension : PXGraphExtension<ATPTEFMReplenishmentMaint>
    {
#if Version23R2
        public static bool IsActive() =>  ATPTEFMPrefetchSetup.IsFMFilterByEmployeeDelegates;
#else
        public static bool IsActive() => ATPTEFMPrefetchSetup.IsActive && ATPTEFMPrefetchSetup.IsFMFilterByEmployeeDelegates;
#endif
        /// <remarks>
        /// 2024-10-04 : Simplify view override and remove created by condition. 007958 {RRS}
        /// </remarks>
        public override void Initialize() {			
            Base.Replenishment.WhereAnd<Where<EPEmployee.defContactID, Equal<Current<AccessInfo.contactID>>,
                Or<ATPTEFMReplenishment.custodianID, WingmanUser<Current<AccessInfo.userID>>,
                Or<EPEmployee.defContactID, IsSubordinateOfContact<Current<AccessInfo.contactID>>>>>>();
        }
  
        #region Cache Attached
        [PXMergeAttributes(Method = MergeMethod.Append)]
        [PXRemoveBaseAttribute(typeof(PXSelectorAttribute))]
        [PXSubordinateAndWingmenSelector()]
		protected virtual void ATPTEFMReplenishmentFilter_EmployeeID_CacheAttached(PXCache sender) { }

       
        #endregion
    }
}
