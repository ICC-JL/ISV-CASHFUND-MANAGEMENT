using CashFundManagement.Attributes;
using CashFundManagement.DAC;
using CashFundManagement.DAC.Setup;
using CashFundManagement.Helper;
using CashFundManagement.Messages;
using PX.Data;
using PX.Data.BQL;
using PX.Objects.AP;
using PX.Objects.EP;
using PX.Objects.GL;
using PX.Objects.PM;
using System;

namespace CashFundManagement.Extensions.DAC {
    /// <summary>
    /// Extension for Expense Receipts
    /// </summary>
    /// <remarks>
    /// 2024-08-14 : Adds multi-tenant support. {RRS}
    /// 010461 - (CFM2024R1, CFM2024R1A & B) Error upon changing the Vendor in the Expense Receipts from profiled to non-profiled
    /// </remarks>
    public sealed class ATPTEFMEPExpenseClaimDetailsExt : PXCacheExtension<EPExpenseClaimDetails>
    {

#if Version23R2
        public static bool IsActive() => true;
#else
        public static bool IsActive() => ATPTEFMPrefetchSetup.IsActive;
#endif

        #region UsrATPTEFMTranType
        [PXDBString(3, IsFixed = true)]
        [PXUIField(DisplayName = "Transaction Type")]
        [ATPTEFMExpenseTypeAttribute.ATPTEFMList()]
        [PXDefault(ATPTEFMExpenseTypeAttribute.Liquidation, PersistingCheck = PXPersistingCheck.Nothing)]
        [PX.Data.EP.PXFieldDescription]
        public string UsrATPTEFMTranType { get; set; }
        public abstract class usrATPTEFMTranType : PX.Data.BQL.BqlString.Field<usrATPTEFMTranType> { }
        #endregion

        #region UsrATPTEFMReqType
        [PXDBString(3, IsFixed = true)]
        [PXUIField(DisplayName = "Request Type")]
        [ATPTEFMTranTypeAttribute.ATPTEFMList()]
        [PXDefault(typeof(Switch<
            Case<Where<usrATPTEFMTranType, Equal<ATPTEFMExpenseTypeAttribute.requestforPayment>>, ATPTEFMTranTypeAttribute.requestforPayment,
            Case<Where<usrATPTEFMTranType, Equal<ATPTEFMExpenseTypeAttribute.liquidation>>, ATPTEFMTranTypeAttribute.cashAdvance>>, Null>),
            PersistingCheck = PXPersistingCheck.Nothing)]
        [PX.Data.EP.PXFieldDescription]
        [PXUIVisible(typeof(Where<usrATPTEFMTranType, NotEqual<ATPTEFMExpenseTypeAttribute.replenishment>>))]
        [PXFormula(typeof(Default<usrATPTEFMTranType>))]
        public string UsrATPTEFMReqType { get; set; }
        public abstract class usrATPTEFMReqType : BqlString.Field<usrATPTEFMReqType> { }
        #endregion

        #region UsrATPTEFMReqClass
        [PXDBString(10, IsUnicode = true)]
        [PXUIField(DisplayName = "Request Class ID")]
        [ATPTEFMReqClass(typeof(EPExpenseClaimDetails), typeof(usrATPTEFMReqType))]
        [PXDefault(PersistingCheck = PXPersistingCheck.Nothing)]
        [PX.Data.EP.PXFieldDescription]
        [PXUIVisible(typeof(Where<usrATPTEFMTranType, NotEqual<ATPTEFMExpenseTypeAttribute.replenishment>>))]
        [PXFormula(typeof(Default<usrATPTEFMTranType>))]
        [PXUIRequired(typeof(Where<usrATPTEFMTranType, NotEqual<ATPTEFMExpenseTypeAttribute.replenishment>>))]
        public string UsrATPTEFMReqClass { get; set; }
        public abstract class usrATPTEFMReqClass : PX.Data.BQL.BqlString.Field<usrATPTEFMReqClass> { }
        #endregion

        #region UsrATPTEFMReqRef
        [PXDBString(15, InputMask = ">CCCCCCCCCCCCCCC", IsUnicode = true)]
        [PXUIField(DisplayName = "Request Reference")]
        [PXDefault(PersistingCheck = PXPersistingCheck.Nothing)]
        [PXSelector(typeof(Search<
            ATPTEFMCashAdvance.cashAdvanceNbr,
            Where<ATPTEFMCashAdvance.reqClassID, Equal<Current<usrATPTEFMReqClass>>,
                And<ATPTEFMCashAdvance.status, Equal<ATPTEFMCashAdvanceStatusAttribute.pendingLiquidationValue>,
                And<ATPTEFMCashAdvance.branchID, Equal<Current<EPExpenseClaimDetails.branchID>>,
                    And<ATPTEFMCashAdvance.requestedByID, Equal<Current<EPExpenseClaimDetails.employeeID>>>>>>>))]
        [PXUIRequired(typeof(
                    Where<ATPTEFMEPExpenseClaimDetailsExt.usrATPTEFMTranType, Equal<ATPTEFMExpenseTypeAttribute.liquidation>>))]
        [PXUIVisible(typeof(Where<Current<usrATPTEFMTranType>, Equal<ATPTEFMExpenseTypeAttribute.liquidation>>))]
        [PXFormula(typeof(Default<usrATPTEFMTranType>))]
        public string UsrATPTEFMReqRef { get; set; }
        public abstract class usrATPTEFMReqRef : PX.Data.BQL.BqlString.Field<usrATPTEFMReqRef> { }
        #endregion

        #region Check ExpenseClaimDetailEntry Extension on Connector Package. Override Attributes to use Philtax Standard Behavior if Connector is published
        #region UsrATPTEFMVendorID
        [PXDBString(30, IsUnicode = true)]
        [PXUIField(DisplayName = "Vendor ID")]
        [PXFormula(typeof(Default<ATPTEFMEPExpenseClaimDetailsExt.usrATPTVendorID>))]
        [PXFormula(typeof(Selector<ATPTEFMEPExpenseClaimDetailsExt.usrATPTVendorID, VendorR.acctCD>))]
        public string UsrATPTVendID { get; set; }
        public abstract class usrATPTVendID : BqlString.Field<usrATPTVendID> { }
        #endregion

        #region UsrATPTEFMVendName
        [PXDBString(100, IsUnicode = true)]
        [PXUIField(DisplayName = "Vendor Name")]
        [PXDefault(PersistingCheck = PXPersistingCheck.Nothing)]
        [PXFormula(typeof(Default<ATPTEFMEPExpenseClaimDetailsExt.usrATPTVendorID>))]
        [PXFormula(typeof(Selector<ATPTEFMEPExpenseClaimDetailsExt.usrATPTVendorID, VendorR.acctName>))]
        public string UsrATPTVendName { get; set; }
        public abstract class usrATPTVendName : BqlString.Field<usrATPTVendName> { }
        #endregion

        #region UsrATPTEFMVendTIN
        [PXDBString(50, IsUnicode = true)]
        [PXUIField(DisplayName = "Vendor TIN")]
        [PXFormula(typeof(Default<ATPTEFMEPExpenseClaimDetailsExt.usrATPTVendorID>))]
        [PXDefault(PersistingCheck = PXPersistingCheck.Nothing)]
        public string UsrATPTVendTIN { get; set; }
        public abstract class usrATPTVendTIN : BqlString.Field<usrATPTVendTIN> { }

        #endregion

        #region UsrATPTEFMVendAddr
        [PXDBString(100, IsUnicode = true)]
        [PXUIField(DisplayName = "Vendor Address")]
        [PXFormula(typeof(Default<ATPTEFMEPExpenseClaimDetailsExt.usrATPTVendorID>))]
        [PXDefault(PersistingCheck = PXPersistingCheck.Nothing)]
        public string UsrATPTAddress { get; set; }
        public abstract class usrATPTAddress : BqlString.Field<usrATPTAddress> { }

        #endregion 
        #endregion

        #region UsrATPTEFMMonthEnd
        [PXDBBool]
        public bool? UsrATPTEFMMonthEnd { get; set; }
        public abstract class usrATPTEFMMonthEnd : PX.Data.BQL.BqlBool.Field<usrATPTEFMMonthEnd> { }
        #endregion

        #region UsrATPTEFMFundType
        [PXDBString(1, IsUnicode = true)]
        [ATPTEFMFundTypeAttribute.ATPTEFMFundType]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.FundType)]
        [PXUIVisible(typeof(Where<ATPTEFMEPExpenseClaimDetailsExt.usrATPTEFMTranType, Equal<ATPTEFMExpenseTypeAttribute.replenishment>>))]
        public string UsrATPTEFMFundType { get; set; }
        public abstract class usrATPTEFMFundType : BqlString.Field<usrATPTEFMFundType> { }
        #endregion

        #region UsrATPTEFMFundReturn
        [PXDBDecimal(2)]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.FundReturn)]
        [PXDefault(TypeCode.Decimal, "0.0", PersistingCheck = PXPersistingCheck.Nothing)]
        public decimal? UsrATPTEFMFundReturn { get; set; }
        public abstract class usrATPTEFMFundReturn : BqlDecimal.Field<usrATPTEFMFundReturn> { }
        #endregion

        #region UsrATPTEFMRequestRefNbr
        [PXDBString(15, IsUnicode = true)]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.RequestRefNbr, Visibility = PXUIVisibility.Visible)]
        [PXSelector(typeof(Search<ATPTEFMFundTransaction.refNbr, Where<ATPTEFMFundTransaction.status, Equal<ATPTEFMFundStatusAttribute.openValue>, And<ATPTEFMFundTransaction.fundType, Equal<Current<usrATPTEFMFundType>>>>>), typeof(ATPTEFMFundTransaction.refNbr), typeof(ATPTEFMFundTransaction.descr))]
        [PXDefault(PersistingCheck = PXPersistingCheck.Nothing)]
        [PXUIRequired(typeof(Where<ATPTEFMEPExpenseClaimDetailsExt.usrATPTEFMTranType, Equal<ATPTEFMExpenseTypeAttribute.replenishment>>))]
        [PXUIVisible(typeof(Where<ATPTEFMEPExpenseClaimDetailsExt.usrATPTEFMTranType, Equal<ATPTEFMExpenseTypeAttribute.replenishment>>))]
        [PXFormula(typeof(Default<usrATPTEFMTranType>))]
        public string UsrATPTEFMRequestRefNbr { get; set; }
        public abstract class usrATPTEFMRequestRefNbr : BqlString.Field<usrATPTEFMRequestRefNbr> { }
        #endregion

        #region UsrATPTEFMRFPReqRef
        //[PXDBString(15, InputMask = ">CCCCCCCCCCCCCCC", IsUnicode = true)]
        //[PXUIField(DisplayName = ATPTEFMMessages.RFPRequestReference)]
        //[PXSelector(typeof(Search<usrATPTEFMRFPReqRef, Where<usrATPTEFMReqClass, Equal<Current<usrATPTEFMReqClass>>>>), ValidateValue = false)]
        //[RFPRefNbrAutonumber(typeof(ATPTEFMReqClass.numberingID), typeof(AccessInfo.businessDate))]
        ////[AutoNumber(typeof(ATPTEFMReqClass.numberingID), typeof(AccessInfo.businessDate))]
        //[PXUIVisible(typeof(Where<usrATPTEFMTranType, Equal<ATPTEFMExpenseTypeAttribute.requestforPayment>>))]
        //[PXFormula(typeof(Default<usrATPTEFMTranType, usrATPTEFMReqClass>))]
        //public string UsrATPTEFMRFPReqRef { get; set; }
        //public abstract class usrATPTEFMRFPReqRef : BqlString.Field<usrATPTEFMRFPReqRef> { }
        #endregion

        #region UsrATPTVendorID
        [ATPTEFMVendorSelector]
        [PXDefault(PersistingCheck = PXPersistingCheck.Nothing)]
        [PXUIRequired(typeof(Where<usrATPTEFMTranType, Equal<ATPTEFMExpenseTypeAttribute.requestforPayment>>))]
        [PXUIVisible(typeof(Where<usrATPTEFMTranType, Equal<ATPTEFMExpenseTypeAttribute.requestforPayment>>))]
        public int? UsrATPTVendorID { get; set; }
        public abstract class usrATPTVendorID : BqlInt.Field<usrATPTVendorID> { }
        #endregion

        #region UsrATPTEFMIsImported
        [PXDBBool()]
        [PXDefault(false, PersistingCheck = PXPersistingCheck.Nothing)]
        public bool? UsrATPTEFMIsImported { get; set; }
        public abstract class usrATPTEFMIsImported : PX.Data.BQL.BqlBool.Field<usrATPTEFMIsImported> { }
        #endregion

        #region UsrATPTEFMIsReclassifyDoc
        [PXDBBool()]
        [PXDefault(false, PersistingCheck = PXPersistingCheck.Nothing)]
        public bool? UsrATPTEFMIsReclassifyDoc { get; set; }
        public abstract class usrATPTEFMIsReclassifyDoc : PX.Data.BQL.BqlBool.Field<usrATPTEFMIsReclassifyDoc> { }
        #endregion

        #region Override

        #region InventoryID
        [PXMergeAttributes(Method = MergeMethod.Append)]
        [PXRemoveBaseAttribute(typeof(PXRestrictorAttribute))]
        [ATPTEFMCARequestClassItemSelector(typeof(EPExpenseClaimDetails), typeof(ATPTEFMEPExpenseClaimDetailsExt.usrATPTEFMReqClass), typeof(ATPTEFMEPExpenseClaimDetailsExt.usrATPTEFMReqRef), typeof(ATPTEFMEPExpenseClaimDetailsExt.usrATPTEFMRequestRefNbr), typeof(ATPTEFMEPExpenseClaimDetailsExt.usrATPTEFMIsReclassifyDoc))]
        public int? InventoryID { get; set; }
        public abstract class inventoryID : BqlInt.Field<inventoryID> { }
        #endregion

        #region TranDesc
        [PXMergeAttributes(Method = MergeMethod.Append)]
        [PXRemoveBaseAttribute(typeof(PXUIFieldAttribute))]
        [PXUIField(DisplayName = ATPTEFMMessages.LineDescription, Visibility = PXUIVisibility.Visible)]
        public string TranDesc { get; set; }
        public abstract class tranDesc : BqlString.Field<tranDesc> { }
        #endregion

        #region ExpenseRefNbr
        [PXMergeAttributes(Method = MergeMethod.Append)]
        [PXRemoveBaseAttribute(typeof(PXUIFieldAttribute))]
        [PXUIField(DisplayName = ATPTEFMMessages.ORCRRefNbr, Visibility = PXUIVisibility.Visible)]
        [ATPTEFMDuplicateORNbr]
        public string ExpenseRefNbr { get; set; }
        public abstract class expenseRefNbr : BqlString.Field<expenseRefNbr> { }
        #endregion

        #region Status
        [PXMergeAttributes(Method = MergeMethod.Replace)]
        [PXDBString(1, IsFixed = true)]
        [PXDefault(ATPTEFMExpenseReceiptStatusAttribute.HoldValue, PersistingCheck = PXPersistingCheck.Nothing)]
        [PXUIField(DisplayName = "Status", Enabled = false)]
        [ATPTEFMExpenseReceiptStatusAttribute.ATPTEFMExpenseReceiptStatus]
        public string Status { get; set; }
        public abstract class status : BqlString.Field<status> { }
        #endregion

        #region BranchID
        [PXMergeAttributes(Method = MergeMethod.Append)]
        [PXDefault(typeof(Search<Branch.bAccountID, Where<Branch.active, Equal<True>, And<Branch.branchID, Equal<Current<AccessInfo.branchID>>>>>))]
        public int? BranchID { get; set; }
        public abstract class branchID : BqlInt.Field<branchID> { }
        #endregion

        #region ContractID
        [PXMergeAttributes(Method = MergeMethod.Append)]
        [PXUIEnabled(typeof(usrATPTEFMIsBudgetNotCashAdvance))]
        public int? ContractID { get; set; }
        public abstract class contractID : BqlInt.Field<contractID> { }
        #endregion

        #region TaskID        
        /// <remarks>
        /// 2024-10-02 : Exclude Inactive task. CaseID : 007661 {RRS}
        /// </remarks>
        [PXMergeAttributes(Method = MergeMethod.Append)]
        [PXUIEnabled(typeof(usrATPTEFMIsBudgetNotCashAdvance))]
        [PXRestrictor(typeof(Where<PMTask.isActive, Equal<True>>), PX.Objects.PM.Messages.InactiveTask)]
        public int? TaskID { get; set; }
        public abstract class taskID : BqlInt.Field<taskID> { }
        #endregion

        #region CostCodeID
        [PXMergeAttributes(Method = MergeMethod.Append)]
        [PXUIEnabled(typeof(usrATPTEFMIsBudgetNotCashAdvance))]
        public int? CostCodeID { get; set; }
        public abstract class costCodeID : BqlInt.Field<costCodeID> { }
        #endregion

        #region NoteID
        [PXMergeAttributes(Method = MergeMethod.Append)]
        [PXRemoveBaseAttribute(typeof(PXSearchableAttribute))]
        [PXSearchable(PX.Objects.SM.SearchCategory.TM, "Expense Receipt: {0} by {2}", new Type[] { typeof(EPExpenseClaimDetails.refNbr), typeof(EPExpenseClaimDetails.employeeID), typeof(EPEmployee.acctName) },
            new Type[] { typeof(EPExpenseClaimDetails.tranDesc), typeof(ATPTEFMEPExpenseClaimDetailsExt.usrATPTEFMReqRef), typeof(ATPTEFMEPExpenseClaimDetailsExt.usrATPTEFMRequestRefNbr) },
            NumberFields = new Type[] { typeof(EPExpenseClaimDetails.refNbr), typeof(ATPTEFMEPExpenseClaimDetailsExt.usrATPTEFMReqRef), typeof(ATPTEFMEPExpenseClaimDetailsExt.usrATPTEFMRequestRefNbr) },
            Line1Format = "{0:d}{1}{2}", Line1Fields = new Type[] { typeof(EPExpenseClaimDetails.expenseDate), typeof(EPExpenseClaimDetails.status), typeof(EPExpenseClaimDetails.refNbr) },
            Line2Format = "{0}{1}{2}{3}{4}{5}", Line2Fields = new Type[] { typeof(EPExpenseClaimDetails.tranDesc), typeof(ATPTEFMEPExpenseClaimDetailsExt.usrATPTEFMTranType), typeof(ATPTEFMEPExpenseClaimDetailsExt.usrATPTEFMReqType), typeof(ATPTEFMEPExpenseClaimDetailsExt.usrATPTEFMFundType), typeof(ATPTEFMEPExpenseClaimDetailsExt.usrATPTEFMReqRef), typeof(ATPTEFMEPExpenseClaimDetailsExt.usrATPTEFMRequestRefNbr) },
            SelectForFastIndexing = typeof(Select2<EPExpenseClaimDetails, InnerJoin<EPEmployee, On<EPExpenseClaimDetails.employeeID, Equal<EPEmployee.bAccountID>>>>),
            SelectDocumentUser = typeof(Select2<PX.SM.Users,
            InnerJoin<EPEmployee, On<PX.SM.Users.pKID, Equal<EPEmployee.userID>>>,
            Where<EPEmployee.bAccountID, Equal<Current<EPExpenseClaimDetails.employeeID>>>>)
        )]
        public Guid? NoteID { get; set; }
        public abstract class noteID : BqlGuid.Field<noteID> { }
        #endregion

        #region ExpenseAccountID
        [PXMergeAttributes(Method = MergeMethod.Append)]
        [PXUIEnabled(typeof(usrATPTEFMIsBudgetNotCashAdvance))]
        public int? ExpenseAccountID { get; set; }
        public abstract class expenseAccountID : BqlInt.Field<expenseAccountID> { }
        #endregion

        #region ExpenseSubID
        [PXMergeAttributes(Method = MergeMethod.Append)]
        [PXUIEnabled(typeof(usrATPTEFMIsBudgetNotCashAdvance))]
        public int? ExpenseSubID { get; set; }
        public abstract class expenseSubID : BqlInt.Field<expenseSubID> { }
        #endregion

        #region UsrATPTEFMIsBudgetNotCashAdvance
        [PXBool]
        [PXUnboundDefault(typeof(True))]
        public bool? UsrATPTEFMIsBudgetNotCashAdvance { get; set; }
        public abstract class usrATPTEFMIsBudgetNotCashAdvance : BqlBool.Field<usrATPTEFMIsBudgetNotCashAdvance> { }
        #endregion

        #endregion

        #region UsrATPTEFMVendorBAccountID
        [PXInt]
        [PXFormula(typeof(Default<ATPTEFMEPExpenseClaimDetailsExt.usrATPTVendorID>))]
        [PXFormula(typeof(Selector<ATPTEFMEPExpenseClaimDetailsExt.usrATPTVendorID, VendorR.bAccountID>))]
        public int? UsrATPTEFMVendorBAccountID { get; set; }
        public abstract class usrATPTEFMVendorBAccountID : BqlInt.Field<usrATPTEFMVendorBAccountID> { }
        #endregion

        #region UsrATPTEFMDetailVendorID
        [PXDBInt]
        [PXFormula(typeof(Default<ATPTEFMEPExpenseClaimDetailsExt.usrATPTVendID>))]
        [PXFormula(typeof(Selector<ATPTEFMEPExpenseClaimDetailsExt.usrATPTVendID, VendorR.bAccountID>))]
        public int? UsrATPTEFMDetailVendorID { get; set; }
        public abstract class usrATPTEFMDetailVendorID : BqlInt.Field<usrATPTEFMDetailVendorID> { }
        #endregion

        #region UsrATPTEFMATCCode
        [PXDBString(30, IsUnicode = true)]
        [PXDefault(PersistingCheck = PXPersistingCheck.Nothing)]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.AtcCode)]
        public string UsrATPTEFMATCCode { get; set; }
        public abstract class usrATPTEFMATCCode : PX.Data.BQL.BqlString.Field<usrATPTEFMATCCode> { }
        #endregion

        /// <remarks>
        /// 2024-10-10 : Flagging for receipts to indicate whether this is an unreplenished receipt. 008055 {JLG}
        /// </remarks>
        #region UsrATPTEFMIsUnreplenish
        [PXDBBool()]
        [PXDefault(false, PersistingCheck = PXPersistingCheck.Nothing)]
        public bool? UsrATPTEFMIsUnreplenish { get; set; }
        public abstract class usrATPTEFMIsUnreplenish : PX.Data.BQL.BqlBool.Field<usrATPTEFMIsUnreplenish> { }
        #endregion

        #region UsrATPTEFMAccountGroup
        [PXInt]
        [PXUnboundDefault(typeof(Selector<expenseAccountID, Account.accountGroupID>), PersistingCheck = PXPersistingCheck.Nothing)]
        [PXSelector(typeof(Search<PMAccountGroup.groupID>), SubstituteKey = typeof(PMAccountGroup.groupCD))]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.AccountGroup, Enabled = false)]
        [PXFormula(typeof(Default<expenseAccountID>))]
        public int? UsrATPTEFMAccountGroup { get; set; }
        public abstract class usrATPTEFMAccountGroup : PX.Data.BQL.BqlInt.Field<usrATPTEFMAccountGroup> { }
        #endregion
    }
}