using PX.Data;
using PX.SM;
using System;
using System.Collections;
using System.Collections.Generic;
using PX.Objects.EP;
using CashFundManagement.DAC;
using static CashFundManagement.BLC.ATPTEFMCashAdvanceMaint;
using CashFundManagement.Helper;
using PX.TM;

namespace CashFundManagement.Extensions.BLC
{
    //TODO : Remove dead Codes on next upgrade.
    /// <remarks>
    /// 2024-08-14 : Adds multi-tenant support. {RRS}
    /// </remarks>
    public class ATPTEFMCashAdvanceMaintExtension : PXGraphExtension<CashFundManagement.BLC.ATPTEFMCashAdvanceMaint>
    {
#if Version23R2
        public static bool IsActive() => ATPTEFMPrefetchSetup.IsCAFilterByEmployeeDelegates;
#else
        public static bool IsActive() => ATPTEFMPrefetchSetup.IsActive && ATPTEFMPrefetchSetup.IsCAFilterByEmployeeDelegates;
#endif
        /// <remarks>
        /// 2024-09-19 : Simplify view override and remove created by condition. 007582 {RRS}
        /// </remarks>
        public override void Initialize() {
            Base.CashAdvance.WhereAnd<Where<EPEmployee.defContactID, Equal<Current<AccessInfo.contactID>>,
                Or<ATPTEFMCashAdvance.requestedByID, WingmanUser<Current<AccessInfo.userID>>,
                Or<EPEmployee.defContactID, IsSubordinateOfContact<Current<AccessInfo.contactID>>,
                Or<ATPTEFMCashAdvance.noteID, Approver<Current<AccessInfo.contactID>>>>>>>();
        }

        #region Views
        
        //[PXFilterable]
        //public PXSelectJoin<
        //ATPTEFMCashAdvance,
        //InnerJoin<EPEmployee,
        //	On<EPEmployee.bAccountID, Equal<ATPTEFMCashAdvance.requestedByID>>>,
        //Where<Current2<ATPTEFMCashAdvanceFilter.employeeID>, IsNotNull,
        //	And<ATPTEFMCashAdvance.requestedByID, Equal<Current2<ATPTEFMCashAdvanceFilter.employeeID>>,
        //	Or<Current2<ATPTEFMCashAdvanceFilter.employeeID>, IsNull,
        //	And<Where<ATPTEFMCashAdvance.createdByID, Equal<Current<AccessInfo.userID>>,
        //		Or<EPEmployee.userID, Equal<Current<AccessInfo.userID>>,
        //		Or<ATPTEFMCashAdvance.noteID, Approver<Current<AccessInfo.contactID>>,
        //		Or<ATPTEFMCashAdvance.requestedByID, WingmanUser<Current<AccessInfo.userID>>>>>>>>>>,
        //OrderBy<
        //	Desc<ATPTEFMCashAdvance.cashAdvanceNbr>>>
        //CashAdvance;

  //      public IEnumerable cashAdvance()
		//{
		//	return PXSelectJoin<
		//			ATPTEFMCashAdvance,
		//			InnerJoin<EPEmployee,
		//				On<EPEmployee.bAccountID, Equal<ATPTEFMCashAdvance.requestedByID>>>,
		//			Where<Current2<ATPTEFMCashAdvanceFilter.employeeID>, IsNotNull,
		//				And<ATPTEFMCashAdvance.requestedByID, Equal<Current2<ATPTEFMCashAdvanceFilter.employeeID>>,
		//				Or<Current2<ATPTEFMCashAdvanceFilter.employeeID>, IsNull,
		//				And<Where<ATPTEFMCashAdvance.createdByID, Equal<Current<AccessInfo.userID>>,
		//					Or<EPEmployee.userID, Equal<Current<AccessInfo.userID>>,
		//					Or<ATPTEFMCashAdvance.noteID, Approver<Current<AccessInfo.contactID>>,
		//					Or<ATPTEFMCashAdvance.requestedByID, WingmanUser<Current<AccessInfo.userID>>>>>>>>>>,
		//			OrderBy<
		//				Desc<ATPTEFMCashAdvance.cashAdvanceNbr>>>.Select(Base);
		//}
		#endregion

		#region Cache Attached
		[PXMergeAttributes(Method = MergeMethod.Append)]
        [PXRemoveBaseAttribute(typeof(PXSelectorAttribute))]
        [PXSubordinateAndWingmenSelector()]
        protected virtual void ATPTEFMCashAdvanceFilter_EmployeeID_CacheAttached(PXCache sender) { }
        #endregion
    }
}