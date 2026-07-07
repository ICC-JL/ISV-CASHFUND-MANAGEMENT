using CashFundManagement.DAC;
using CashFundManagement.Helper;
using PX.Data;
using PX.Data.BQL;
using PX.Objects.EP;
using PX.TM;
using static CashFundManagement.DAC.ATPTEFMFund;
//using static CashFundManagement.DAC.Base.ATPTEFMAudit;

namespace CashFundManagement.Extensions.DAC {
    /// <remarks>
    /// 2024-08-14 : Adds multi-tenant support. {RRS}
    /// 2025-07-21 - Visibility of Fund IDs where Filtered View is Enabled CASE: 011309 {JLTG}
    /// </remarks>
    public sealed class ATPTEFMFundExtension : PXCacheExtension<ATPTEFMFund>
    {
#if Version23R2
        public static bool IsActive() => ATPTEFMPrefetchSetup.IsFMFilterByEmployeeDelegates;
#else
        public static bool IsActive() => ATPTEFMPrefetchSetup.IsActive && ATPTEFMPrefetchSetup.IsFMFilterByEmployeeDelegates;
#endif
        #region FundCD
        [PXMergeAttributes(Method = MergeMethod.Append)]
        [PXRemoveBaseAttribute(typeof(PXSelectorAttribute))]
        [PXSelector(typeof(Search2<
            fundCD,
            InnerJoin<EPEmployee,
                On<EPEmployee.bAccountID, Equal<custodianID>>>,
            Where<EPEmployee.defContactID, Equal<Current<AccessInfo.contactID>>,
                Or<EPEmployee.defContactID, IsSubordinateOfContact<Current<AccessInfo.contactID>>,
                Or<custodianID, WingmanUser<Current<AccessInfo.userID>>>>>,
            OrderBy<
                Desc<fundCD>>>),
            typeof(branchID),
            typeof(fundType),
            typeof(fundCD),
            typeof(status),
            typeof(custodianID),
            typeof(employeeName),
            typeof(documentDate),
            typeof(descr),
            typeof(initialFund))]
        public string FundCD { get; set; }
        public abstract class fundCD : BqlString.Field<fundCD> { }
        #endregion

        #region CustodianID
        [PXMergeAttributes(Method = MergeMethod.Append)]
        //[PXRemoveBaseAttribute(typeof(PXSelectorAttribute))] Commented out issue with PXFormula E.g. [PXFormula(typeof(Selector<custodianID, EPEmployee.acctName>))]
        //[PXSubordinateAndWingmenSelector]
        [PXRemoveBaseAttribute(typeof(PXSelectorAttribute))]
        [PXSelector(typeof(Search<
            EPEmployee.bAccountID,
            Where<EPEmployee.defContactID, Equal<Current<AccessInfo.contactID>>,
                Or<EPEmployee.defContactID, IsSubordinateOfContact<Current<AccessInfo.contactID>>,
                Or<EPEmployee.bAccountID, WingmanUser<Current<AccessInfo.userID>>>>>>),
            typeof(EPEmployee.bAccountID),
            typeof(EPEmployee.departmentID),
            typeof(EPEmployee.parentBAccountID), DescriptionField = typeof(EPEmployee.acctName), SubstituteKey = typeof(EPEmployee.acctCD))]
        public int? CustodianID { get; set; }
        public abstract class custodianID : BqlInt.Field<custodianID> { }
        #endregion
    }
}
