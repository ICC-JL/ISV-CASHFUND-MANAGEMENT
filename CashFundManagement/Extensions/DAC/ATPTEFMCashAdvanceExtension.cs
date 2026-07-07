using CashFundManagement.DAC;
using CashFundManagement.Helper;
using PX.Data;
using PX.Objects.CR;
using PX.Objects.EP;
using static CashFundManagement.DAC.ATPTEFMCashAdvance;
using static CashFundManagement.DAC.Base.ATPTEFMAudit;

namespace CashFundManagement.Extensions.DAC
{
    /// <remarks>
    /// 2024-08-14 : Adds multi-tenant support. {RRS}
    /// </remarks>
    public sealed class ATPTEFMCashAdvanceExtension : PXCacheExtension<ATPTEFMCashAdvance>
    {
#if Version23R2
        public static bool IsActive() => ATPTEFMPrefetchSetup.IsCAFilterByEmployeeDelegates; 
#else
        public static bool IsActive() => ATPTEFMPrefetchSetup.IsActive && ATPTEFMPrefetchSetup.IsCAFilterByEmployeeDelegates;
#endif

        #region CashAdvanceNbr
        [PXMergeAttributes(Method = MergeMethod.Append)]
        [PXRemoveBaseAttribute(typeof(PXSelectorAttribute))]
        [PXSelector(typeof(Search2<
            cashAdvanceNbr,
            InnerJoin<EPEmployee,
                On<EPEmployee.bAccountID, Equal<requestedByID>>>,
            Where2<
                Where<reqClassID, Equal<Current<reqClassID>>>,
                And<
            Where<createdByID, Equal<Optional<AccessInfo.userID>>,
                    Or<EPEmployee.userID, Equal<Optional<AccessInfo.userID>>,
                    Or<noteID, Approver<Optional<AccessInfo.contactID>>,
                    Or<requestedByID, WingmanUser<Optional<AccessInfo.userID>>>>>>>>,
            OrderBy<
                Desc<cashAdvanceNbr>>>),
            typeof(branchID),
            typeof(cashAdvanceNbr),
            typeof(date),
            typeof(status),
            typeof(finPeriodID),
            typeof(descr),
            typeof(dateOfUse),
            typeof(liqDate),
            typeof(curyID),
            typeof(curyRequestedAmount),
            typeof(curyChangeAmount))]
        public string CashAdvanceNbr { get; set; }
        public abstract class cashAdvanceNbr : PX.Data.BQL.BqlString.Field<cashAdvanceNbr> { }
        #endregion

        #region RequestedByID
        [PXMergeAttributes(Method = MergeMethod.Append)]
        [PXRemoveBaseAttribute(typeof(PXSelectorAttribute))]
        [PXSubordinateAndWingmenSelector]
        public int? RequestedByID { get; set; }
        public abstract class requestedByID : PX.Data.BQL.BqlInt.Field<requestedByID> { }
        #endregion

        #region DepartmentID
        [PXMergeAttributes(Method = MergeMethod.Append)]
        [PXRemoveBaseAttribute(typeof(PXDefaultAttribute))]
        [PXDefault(typeof(Selector<requestedByID, CREmployee.departmentID>))]
        public string DepartmentID { get; set; }
        public abstract class departmentID : PX.Data.BQL.BqlString.Field<departmentID> { }
        #endregion
    }
}
