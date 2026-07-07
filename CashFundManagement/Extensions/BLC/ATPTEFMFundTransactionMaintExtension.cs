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
    public class ATPTEFMFundTransactionMaintExtension : PXGraphExtension<ATPTEFMFundTransactionMaint>
    {
#if Version23R2
        public static bool IsActive() => ATPTEFMPrefetchSetup.IsFMFilterByEmployeeDelegates;
#else
        public static bool IsActive() => ATPTEFMPrefetchSetup.IsActive && ATPTEFMPrefetchSetup.IsFMFilterByEmployeeDelegates;
#endif
        public override void Initialize() {
            base.Initialize();
			Base.FundTransaction.WhereAnd<Where<EPEmployee.defContactID, Equal<Current<AccessInfo.contactID>>,
                Or<ATPTEFMFundTransaction.requestedByID, WingmanUser<Current<AccessInfo.userID>>,
                Or<EPEmployee.defContactID, IsSubordinateOfContact<Current<AccessInfo.contactID>>,
                Or<ATPTEFMFundTransaction.noteID, Approver<Current<AccessInfo.contactID>>>>>>>();
		}

        #region Cache Attached
		[PXMergeAttributes(Method = MergeMethod.Append)]
        [PXRemoveBaseAttribute(typeof(PXSelectorAttribute))]
        [PXSubordinateAndWingmenSelector()]
		protected virtual void ATPTEFMFundTransactionFilter_EmployeeID_CacheAttached(PXCache sender) { }
        #endregion
	}
}
