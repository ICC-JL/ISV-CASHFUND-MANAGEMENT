using CashFundManagement.Attributes;
using CashFundManagement.DAC;
using CashFundManagement.Helper;
using PX.Data;
using PX.Data.BQL;
using PX.Objects.CS;
using PX.Objects.EP;
using PX.TM;
using static CashFundManagement.DAC.ATPTEFMReplenishment;
using static CashFundManagement.DAC.Base.ATPTEFMAudit;

namespace CashFundManagement.Extensions.DAC {
    /// <remarks>
    /// 2024-08-14 : Adds multi-tenant support. {RRS}
    /// CFM 2024R1 - Replenishment Screen [Fund ID Field]
    /// 2025-07-21 - Visibility of Fund IDs where Filtered View is Enabled CASE: 011309 {JLTG}
    /// </remarks>
    public sealed class ATPTEFMReplenishmentExtension : PXCacheExtension<ATPTEFMReplenishment>
    {
#if Version23R2
        public static bool IsActive() => ATPTEFMPrefetchSetup.IsFMFilterByEmployeeDelegates; 
#else
        public static bool IsActive() => ATPTEFMPrefetchSetup.IsActive && ATPTEFMPrefetchSetup.IsFMFilterByEmployeeDelegates;
#endif

        #region RefNbr
        [PXMergeAttributes(Method = MergeMethod.Append)]
        [PXRemoveBaseAttribute(typeof(PXSelectorAttribute))]
        [PXSelector(typeof(Search2<
            replenishmentNbr,
            InnerJoin<EPEmployee,
                On<EPEmployee.bAccountID, Equal<custodianID>>>,
            Where<createdByID, Equal<Optional<AccessInfo.userID>>,
                Or<EPEmployee.userID, Equal<Optional<AccessInfo.userID>>,
                Or<noteID, Approver<Optional<AccessInfo.contactID>>,
                Or<custodianID, WingmanUser<Optional<AccessInfo.userID>>>>>>,
            OrderBy<
                Desc<replenishmentNbr>>>),
            typeof(branchID),
            typeof(date),
            typeof(replenishmentNbr),
            typeof(fundID),
            typeof(custodianID),
            typeof(departmentID),
            typeof(descr),
            typeof(claimAmount))]
        public string RefNbr { get; set; }
        public abstract class refNbr : BqlString.Field<refNbr> { }
        #endregion

        #region FundID
        [PXMergeAttributes(Method = MergeMethod.Append)]
        [PXRemoveBaseAttribute(typeof(PXSelectorAttribute))]
        [PXSelector(typeof(Search2<
            ATPTEFMFund.fundCD,
            InnerJoin<EPEmployee,
                On<EPEmployee.bAccountID, Equal<ATPTEFMFund.custodianID>>>,
            Where<ATPTEFMFund.fundType, Equal<Current<fundType>>,
                And<ATPTEFMFund.closed, NotEqual<boolTrue>,
                And<ATPTEFMFund.status, NotEqual<ATPTEFMFundStatusAttribute.pendingCloseValue>,
                And<ATPTEFMFund.isActive, Equal<True>,
                And<Where<EPEmployee.defContactID, Equal<Current<AccessInfo.contactID>>,
                    Or<EPEmployee.defContactID, IsSubordinateOfContact<Current<AccessInfo.contactID>>,
                    Or<ATPTEFMFund.custodianID, WingmanUser<Current<AccessInfo.userID>>>>>>>>>>,
            OrderBy<
                Desc<ATPTEFMFund.fundCD>>>),
                    typeof(ATPTEFMFund.fundCD),
                    typeof(EPEmployee.acctName),
                    SubstituteKey = typeof(ATPTEFMFund.fundCD),
                    DescriptionField = typeof(ATPTEFMFund.descr))]
        public string FundID { get; set; }
        public abstract class fundID : BqlString.Field<fundID> { }
        #endregion
    }
}
