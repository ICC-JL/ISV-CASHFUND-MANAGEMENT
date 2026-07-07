using CashFundManagement.BLC;
using CashFundManagement.DAC;
using CashFundManagement.Helper;
using PX.Data;
using PX.Objects.EP;
using PX.TM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CashFundManagement.BLC.ATPTEFMFundsMaint;

namespace CashFundManagement.Extensions.BLC
{
    /// <remarks>
    /// 2024-08-14 : Adds multi-tenant support. {RRS}
	/// 010193 - CFM 2024R1 - Funds Primary List ATPT2104
    /// 2025-07-21 - Visibility of Fund IDs where Filtered View is Enabled CASE: 011309 {JLTG}
    /// </remarks>
    public class ATPTEFMFundsMaintExtension : PXGraphExtension<ATPTEFMFundsMaint>
    {
#if Version23R2
		public static bool IsActive() => ATPTEFMPrefetchSetup.IsFMFilterByEmployeeDelegates;
#else
        public static bool IsActive() => ATPTEFMPrefetchSetup.IsActive && ATPTEFMPrefetchSetup.IsFMFilterByEmployeeDelegates;
#endif

        public override void Initialize()
        {
            Base.Document.WhereAnd<Where<EPEmployee.defContactID, Equal<Current<AccessInfo.contactID>>,
                Or<ATPTEFMFund.custodianID, WingmanUser<Current<AccessInfo.userID>>,
                Or<EPEmployee.defContactID, IsSubordinateOfContact<Current<AccessInfo.contactID>>>>>>();
        }

		#region Cache Attached
		[PXMergeAttributes(Method = MergeMethod.Append)]
        [PXRemoveBaseAttribute(typeof(PXSelectorAttribute))]
        [PXSubordinateAndWingmenSelector()]
		protected virtual void ATPTEFMFundFilter_EmployeeID_CacheAttached(PXCache sender) { }
		#endregion
	}
}
