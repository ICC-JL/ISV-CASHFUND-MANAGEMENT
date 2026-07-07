using CashFundManagement.Attributes;
using CashFundManagement.DAC.Setup;
using CashFundManagement.Helper;
using CashFundManagement.Messages;
using PX.Data;
using PX.Data.BQL;
using PX.Objects.AP;
using PX.Objects.CR;
using PX.Objects.EP;
using PX.Objects.Common;
using System;
using PX.SM;

namespace CashFundManagement.Extensions.DAC 
{
    /// <remarks>
    /// 2024-08-14 : Adds multi-tenant support. {RRS}
    /// </remarks>
    public sealed class ATPTEFMEPExpenseClaimExt : PXCacheExtension<EPExpenseClaim>
    {
#if Version23R2
        public static bool IsActive() => true;
#else
        public static bool IsActive() => ATPTEFMPrefetchSetup.IsActive;
#endif

        #region UsrATPTEFMTranType
        [PXDBString(3, IsFixed = true)]
        [PXUIField(DisplayName = "Transaction Type", Required = true)]
        [ATPTEFMExpenseTypeAttribute.ATPTEFMExpenseClaimList()]
        [PXDefault(ATPTEFMExpenseTypeAttribute.Liquidation, PersistingCheck = PXPersistingCheck.Nothing)]
        [PX.Data.EP.PXFieldDescription]
        public string UsrATPTEFMTranType { get; set; }
        public abstract class usrATPTEFMTranType : PX.Data.BQL.BqlString.Field<usrATPTEFMTranType> { }
        #endregion

        #region UsrATPTEFMReqType
        [PXDBString(3, IsFixed = true)]
        [PXUIField(DisplayName = "Request Type", Required = true)]
        [ATPTEFMTranTypeAttribute.ATPTEFMList()]
        [PXDefault(ATPTEFMTranTypeAttribute.CashAdvance, PersistingCheck = PXPersistingCheck.Nothing)]
        [PX.Data.EP.PXFieldDescription]
        public string UsrATPTEFMReqType { get; set; }
        public abstract class usrATPTEFMReqType : PX.Data.BQL.BqlString.Field<usrATPTEFMReqType> { }
        #endregion

        #region UsrATPTEFMReqClass
        // Acuminator disable once PX1030 PXDefaultIncorrectUse [For to require default value]
        [PXDBString(10, IsUnicode = true)]
        [PXUIField(DisplayName = "Request Class ID", Required = true)]
        [PXDefault(PersistingCheck = PXPersistingCheck.NullOrBlank)]
        [ATPTEFMReqClass(typeof(EPExpenseClaim), typeof(usrATPTEFMReqType))]

        [PX.Data.EP.PXFieldDescription]
        public string UsrATPTEFMReqClass { get; set; }
        public abstract class usrATPTEFMReqClass : PX.Data.BQL.BqlString.Field<usrATPTEFMReqClass> { }
        #endregion

        #region UsrATPTEFMLiqNbr
        [PXDBString(20, IsUnicode = true)]
        [PXUIField(DisplayName = "Liquidation Nbr.")]
        //[AutoNumber(typeof(Search<ATPTEFMCASetup.liquidationNumberingID>), typeof(AccessInfo.businessDate))]
        [ATPTEFMLiquidationNbrAutonumber(typeof(ATPTEFMCASetup.liquidationNumberingID), typeof(AccessInfo.businessDate))]
        [PXUIVisible(typeof(Where<usrATPTEFMTranType, NotEqual<ATPTEFMExpenseTypeAttribute.requestforPayment>>))]
        public string UsrATPTEFMLiqNbr { get; set; }
        public abstract class usrATPTEFMLiqNbr : PX.Data.BQL.BqlString.Field<usrATPTEFMLiqNbr> { }
        #endregion

        #region UsrATPTVendorID
        [ATPTEFMVendorSelector]
        [PXDefault(PersistingCheck = PXPersistingCheck.Nothing)]
        [PXUIRequired(typeof(Where<usrATPTEFMTranType, Equal<ATPTEFMExpenseTypeAttribute.requestforPayment>>))]
        [PXUIVisible(typeof(Where<usrATPTEFMTranType, Equal<ATPTEFMExpenseTypeAttribute.requestforPayment>>))]
        public int? UsrATPTVendorID { get; set; }
        public abstract class usrATPTVendorID : BqlInt.Field<usrATPTVendorID> { }
        #endregion

        #region UsrATPTAmountToWords
        public abstract class usrATPTAmountToWords : PX.Data.BQL.BqlString.Field<usrATPTAmountToWords> { }
        [PXString]
        [PXUIField(DisplayName = "Amount in Words", Visible = false)]
        [ToWords(typeof(EPExpenseClaim.curyDocBal))]
        public string UsrATPTAmountToWords { get; set; }
        #endregion

        #region UsrATPTEFMRFPReqRef
        [PXDBString(15, InputMask = ">CCCCCCCCCCCCCCC", IsUnicode = true)]
        [PXUIField(DisplayName = ATPTEFMMessages.RFPRequestReference)]
        [PXSelector(typeof(Search<usrATPTEFMRFPReqRef, Where<usrATPTEFMReqClass, Equal<Current<usrATPTEFMReqClass>>>>), ValidateValue = false)]
        [ATPTEFMRFPRefNbrAutonumber(typeof(Search<ATPTEFMReqClass.numberingID, Where<ATPTEFMReqClass.reqClassID, Equal<Current<usrATPTEFMReqClass>>>>), typeof(AccessInfo.businessDate))]
        [PXUIVisible(typeof(Where<usrATPTEFMTranType, Equal<ATPTEFMExpenseTypeAttribute.requestforPayment>>))]
        [PXFormula(typeof(Default<usrATPTEFMTranType, usrATPTEFMReqClass>))]
        [PXDefault(PersistingCheck = PXPersistingCheck.Nothing)]
        public string UsrATPTEFMRFPReqRef { get; set; }
        public abstract class usrATPTEFMRFPReqRef : BqlString.Field<usrATPTEFMRFPReqRef> { }
        #endregion

        #region UsrATPTEFMIsImported
        [PXDBBool()]
        [PXDefault(false, PersistingCheck = PXPersistingCheck.Nothing)]
        public bool? UsrATPTEFMIsImported { get; set; }
        public abstract class usrATPTEFMIsImported : PX.Data.BQL.BqlBool.Field<usrATPTEFMIsImported> { }
        #endregion

        #region UsrATPTEFMORCRRefNbr
        [PXDBString(15, IsUnicode = true)]
        [PXUIField(DisplayName = ATPTEFMMessages.ORCRRefNbr, Required = true)]
        //[PXDefault(PersistingCheck = PXPersistingCheck.NullOrBlank)]
        public string UsrATPTEFMORCRRefNbr { get; set; }
        public abstract class usrATPTEFMORCRRefNbr : BqlString.Field<usrATPTEFMORCRRefNbr> { }
        #endregion

        #region UsrATPTEFMIsOverbudget
        [PXDBBool()]
        [PXDefault(typeof(False), PersistingCheck = PXPersistingCheck.Nothing)]
        [PXUIField(DisplayName = ATPTEFMMessages.IsOverBudget, Enabled = false)]
        [PXUIVisible(typeof(False))]
        public bool? UsrATPTEFMIsOverbudget { get; set; }
        public abstract class usrATPTEFMIsOverbudget : PX.Data.BQL.BqlBool.Field<usrATPTEFMIsOverbudget> { }
        #endregion

        #region UsrATPTEFMHasInitialBudget
        [PXDBBool()]
        [PXDefault(typeof(False), PersistingCheck = PXPersistingCheck.Nothing)]
        [PXUIField(DisplayName = ATPTEFMMessages.HasInitialBudget, Enabled = false)]
        [PXUIVisible(typeof(False))]
        public bool? UsrATPTEFMHasInitialBudget { get; set; }
        public abstract class usrATPTEFMHasInitialBudget : PX.Data.BQL.BqlBool.Field<usrATPTEFMHasInitialBudget> { }
        #endregion

        #region UsrATPTEFMEnableCancel
        [PXDBBool()]
        [PXDefault(false, PersistingCheck = PXPersistingCheck.Nothing)]
        public bool? UsrATPTEFMEnableCancel { get; set; }
        public abstract class usrATPTEFMEnableCancel : PX.Data.BQL.BqlBool.Field<usrATPTEFMEnableCancel> { }
        #endregion

        #region UsrATPTEFMBudgetEnabled
        [PXDBBool()]
        [PXDefault(false, PersistingCheck = PXPersistingCheck.Nothing)]
        public bool? UsrATPTEFMBudgetEnabled { get; set; }
        public abstract class usrATPTEFMBudgetEnabled : PX.Data.BQL.BqlBool.Field<usrATPTEFMBudgetEnabled> { }
        #endregion

        #region UsrATPTEFMProjectBudgetEnabled
        [PXDBBool()]
        [PXDefault(PersistingCheck = PXPersistingCheck.Nothing)]
        public bool? UsrATPTEFMProjectBudgetEnabled { get; set; }
        public abstract class usrATPTEFMProjectBudgetEnabled : PX.Data.BQL.BqlBool.Field<usrATPTEFMProjectBudgetEnabled> { }
        #endregion

        #region Unbound
        #region UsrATPTEFMRefNbr
        [PXString(15, IsUnicode = true)]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.RefNbr)]
        [PXSelector(typeof(Search<
            ATPTEFMEPExpenseClaimExt.usrATPTEFMLiqNbr,
            Where<ATPTEFMEPExpenseClaimExt.usrATPTEFMTranType, Equal<ATPTEFMExpenseTypeAttribute.liquidation>>,
            OrderBy<
                Desc<ATPTEFMEPExpenseClaimExt.usrATPTEFMLiqNbr>>>),
            typeof(EPExpenseClaim.docDate),
            typeof(ATPTEFMEPExpenseClaimExt.usrATPTEFMLiqNbr),
            typeof(EPExpenseClaim.status),
            typeof(EPExpenseClaim.docDesc),
            typeof(EPExpenseClaim.curyDocBal))]
        public string UsrATPTEFMRefNbr { get; set; }
        public abstract class usrATPTEFMRefNbr : PX.Data.BQL.BqlString.Field<usrATPTEFMRefNbr> { }
        #endregion

        #region UsrATPTEFMReqRefNbr
        [PXString(15, IsUnicode = true)]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.RefNbr)]
        [PXSelector(typeof(Search<
            ATPTEFMEPExpenseClaimExt.usrATPTEFMRFPReqRef,
            Where<ATPTEFMEPExpenseClaimExt.usrATPTEFMTranType, Equal<ATPTEFMExpenseTypeAttribute.requestforPayment>>,
            OrderBy<
                Desc<ATPTEFMEPExpenseClaimExt.usrATPTEFMRFPReqRef>>>),
            typeof(EPExpenseClaim.docDate),
            typeof(ATPTEFMEPExpenseClaimExt.usrATPTEFMRFPReqRef),
            typeof(EPExpenseClaim.status),
            typeof(EPExpenseClaim.docDesc),
            typeof(EPExpenseClaim.curyDocBal))]
        public string UsrATPTEFMReqRefNbr { get; set; }
        public abstract class usrATPTEFMReqRefNbr : PX.Data.BQL.BqlString.Field<usrATPTEFMRefNbr> { }
        #endregion

        #region UsrATPTEFMPositionID
        [PXString(IsUnicode = true)]
        [PXUnboundDefault(typeof(Search<EPEmployeePosition.positionID, Where<EPEmployeePosition.employeeID, Equal<Current<EPExpenseClaim.employeeID>>,
            And<EPEmployeePosition.isActive, Equal<True>>>>))]
        [PXFormula(typeof(Default<EPExpenseClaim.employeeID>))]
        [PXUIField(DisplayName = ATPTEFMMessages.Positions, Visibility = PXUIVisibility.SelectorVisible)]
        [PXSelector(typeof(Search<EPPosition.positionID>), typeof(EPPosition.positionID), typeof(EPPosition.description), DescriptionField = typeof(EPPosition.positionID))]
        public string UsrATPTEFMPositionID { get; set; }
        public abstract class usrATPTEFMPositionID : PX.Data.BQL.BqlString.Field<usrATPTEFMPositionID> { }
        #endregion

        #region UsrATPTECAllBillsReversed
        [PXBool]
        [PXUnboundDefault(false)]
        public bool? UsrATPTECAllBillsReversed { get; set; }
        public abstract class usrATPTECAllBillsReversed : BqlBool.Field<usrATPTECAllBillsReversed> { }
        #endregion
        #endregion

        #region Override

        #region RequestedBy
        [PXMergeAttributes(Method = MergeMethod.Append)]
        [PXRemoveBaseAttribute(typeof(PXUIFieldAttribute))]
        [PXUIField(DisplayName = ATPTEFMMessages.RequestedBy, Visibility = PXUIVisibility.Visible)]
        public int? EmployeeID { get; set; }
        public abstract class employeeID : BqlInt.Field<employeeID> { }
        #endregion

        #region Status
        [PXMergeAttributes(Method = MergeMethod.Replace)]
        [PXDBString(1, IsFixed = true)]
        [PXDefault(ATPTEFMExpenseClaimStatusAttribute.HoldValue, PersistingCheck = PXPersistingCheck.Nothing)]
        [PXUIField(DisplayName = "Status", Enabled = false)]
        [ATPTEFMExpenseClaimStatusAttribute.ATPTEFMExpenseClaimStatus]
        public string Status { get; set; }
        public abstract class status : BqlString.Field<status> { }
        #endregion

        #region NoteID
        [PXMergeAttributes(Method = MergeMethod.Append)]
        [PXRemoveBaseAttribute(typeof(PXSearchableAttribute))]
        [PXSearchable(PX.Objects.SM.SearchCategory.TM, "Expense Claim: {0} - {2}", new Type[] { typeof(EPExpenseClaim.refNbr), typeof(EPExpenseClaim.employeeID), typeof(EPEmployee.acctName) },
                new Type[] { typeof(EPExpenseClaim.docDesc), typeof(ATPTEFMEPExpenseClaimExt.usrATPTEFMLiqNbr), typeof(ATPTEFMEPExpenseClaimExt.usrATPTEFMRFPReqRef) },
                NumberFields = new Type[] { typeof(EPExpenseClaim.refNbr), typeof(ATPTEFMEPExpenseClaimExt.usrATPTEFMLiqNbr), typeof(ATPTEFMEPExpenseClaimExt.usrATPTEFMRFPReqRef) },
                Line1Format = "{0:d}{1}{2}", Line1Fields = new Type[] { typeof(EPExpenseClaim.docDate), typeof(EPExpenseClaim.status), typeof(EPExpenseClaim.refNbr) },
                Line2Format = "{0}{1}{2}{3}", Line2Fields = new Type[] { typeof(EPExpenseClaim.docDesc), typeof(ATPTEFMEPExpenseClaimExt.usrATPTEFMTranType), typeof(ATPTEFMEPExpenseClaimExt.usrATPTEFMLiqNbr), typeof(ATPTEFMEPExpenseClaimExt.usrATPTEFMRFPReqRef) },
                SelectForFastIndexing = typeof(Select2<EPExpenseClaim, InnerJoin<EPEmployee, On<EPExpenseClaim.employeeID, Equal<EPEmployee.bAccountID>>>>),
                SelectDocumentUser = typeof(Select2<Users,
                InnerJoin<EPEmployee, On<Users.pKID, Equal<EPEmployee.userID>>>,
                Where<EPEmployee.bAccountID, Equal<Current<EPExpenseClaim.employeeID>>>>))]
        public Guid? NoteID { get; set; }
        public abstract class noteID : BqlGuid.Field<noteID> { }
        #endregion

        #endregion

    }
}