using CashFundManagement.Attributes;
using CashFundManagement.Helper;
using PX.Data;
using PX.Data.BQL;
using PX.Objects.AP;

namespace CashFundManagement.Extensions.DAC {
    /// <remarks>
    /// 2024-08-14 : Adds multi-tenant support. {RRS}
    /// </remarks>
    [PXCopyPasteHiddenFields(
    typeof(ATPTEFMAPRegisterExt.usrATPTEFMTranType),
    typeof(ATPTEFMAPRegisterExt.usrATPTEFMReqType),
    typeof(ATPTEFMAPRegisterExt.usrATPTEFMLiqNbr))]
    public sealed class ATPTEFMAPRegisterExt : PXCacheExtension<APRegister>
    {
#if Version23R2
        public static bool IsActive() => true;
#else
        public static bool IsActive() => ATPTEFMPrefetchSetup.IsActive;
#endif

        #region UsrATPTEFMSourceType
        [PXDBString(3, IsFixed =true)]
        [PXUIField(DisplayName = "Source Type")]        
        [Attributes.ATPTEFMSourceTypeAttribute.ATPTEFMList()]
        public string UsrATPTEFMSourceType { get; set; }
        public abstract class usrATPTEFMSourceType : PX.Data.BQL.BqlString.Field<usrATPTEFMSourceType> { }
        #endregion

        #region UsrATPTEFMSourceRef
        [PXDBString(20)]
        [PXUIField(DisplayName = "Source Reference")]
        //[PXSelector(typeof(Search<ATPTEFMCashAdvance.cashAdvanceNbr,
        //Where<ATPTEFMCashAdvance.reqClassID, Equal<Current<usrATPTEFMSourceReqClass>>>>), ValidateValue = false)]
        public string UsrATPTEFMSourceRef { get; set; }
        public abstract class usrATPTEFMSourceRef : PX.Data.BQL.BqlString.Field<usrATPTEFMSourceRef> { }
        #endregion

        #region UsrATPTEFMSourceTranType
        [PXDBString(3, IsFixed = true)]
        [PXUIField(DisplayName = "Source Transaction Type")]
        public string UsrATPTEFMSourceTranType { get; set; }
        public abstract class usrATPTEFMSourceTranType : PX.Data.BQL.BqlString.Field<usrATPTEFMSourceTranType> { }
        #endregion

        #region UsrATPTEFMSourceReqClass
        [PXDBString(20)]
        [PXUIField(DisplayName = "Source Request Class", Enabled = false, IsReadOnly = true)]
        public string UsrATPTEFMSourceReqClass { get; set; }
        public abstract class usrATPTEFMSourceReqClass : PX.Data.BQL.BqlString.Field<usrATPTEFMSourceReqClass> { }
        #endregion

        #region UsrATPTEFMTranType
        [PXDBString(3, IsFixed = true)]
        [PXUIField(DisplayName = "Transaction Type", Enabled = false, IsReadOnly = true)]
        [ATPTEFMExpenseTypeAttribute.ATPTEFMExpenseClaimList()]
        //[PXDefault(ATPTEFMExpenseTypeAttribute.Liquidation, PersistingCheck = PXPersistingCheck.Nothing)]
        [PX.Data.EP.PXFieldDescription]
        public string UsrATPTEFMTranType { get; set; }
        public abstract class usrATPTEFMTranType : PX.Data.BQL.BqlString.Field<usrATPTEFMTranType> { }
        #endregion

        #region UsrATPTEFMReqType
        [PXDBString(3, IsFixed = true)]
        [PXUIField(DisplayName = "Request Type", Enabled = false, IsReadOnly = true)]
        [ATPTEFMTranTypeAttribute.ATPTEFMList()]
        //[PXDefault(ATPTEFMTranTypeAttribute.CashAdvance, PersistingCheck = PXPersistingCheck.Nothing)]
        [PX.Data.EP.PXFieldDescription]
        public string UsrATPTEFMReqType { get; set; }
        public abstract class usrATPTEFMReqType : PX.Data.BQL.BqlString.Field<usrATPTEFMReqType> { }
        #endregion
        
        #region UsrATPTEFMLiqNbr
        [PXDBString(20, IsUnicode = true)]
        [PXUIField(DisplayName = "Reference Nbr.", Enabled =false, IsReadOnly =true)]        
        public string UsrATPTEFMLiqNbr { get; set; }
        public abstract class usrATPTEFMLiqNbr : PX.Data.BQL.BqlString.Field<usrATPTEFMLiqNbr> { }
        #endregion

        #region UsrATPTEFMIsOverbudget
        [PXDBBool()]
        [PXDefault(typeof(False), PersistingCheck = PXPersistingCheck.Nothing)]
        [PXUIField(DisplayName = "Is Over Budget", Enabled = false)]
        [PXUIVisible(typeof(False))]
        public bool? UsrATPTEFMIsOverbudget { get; set; }
        public abstract class usrATPTEFMIsOverbudget : PX.Data.BQL.BqlBool.Field<usrATPTEFMIsOverbudget> { }
        #endregion

        #region UsrATPTEFMIsFromReplenishment
        [PXDBBool]
        [PXDefault(false, PersistingCheck = PXPersistingCheck.Nothing)]
        public bool? UsrATPTEFMIsFromReplenishment { get; set; }
        public abstract class usrATPTEFMIsFromReplenishment : BqlBool.Field<usrATPTEFMIsFromReplenishment> { }
        #endregion

        #region UsrATPTEFMIsUnreplenishedReceiptBill
        [PXDBBool]
        [PXDefault(false, PersistingCheck = PXPersistingCheck.Nothing)]
        public bool? UsrATPTEFMIsUnreplenishedReceiptBill { get; set; }
        public abstract class usrATPTEFMIsUnreplenishedReceiptBill : BqlBool.Field<usrATPTEFMIsUnreplenishedReceiptBill> { }
        #endregion

        #region UsrATPTEFMIsFromCA
        [PXDBBool]
        [PXDefault(false, PersistingCheck = PXPersistingCheck.Nothing)]
        public bool? UsrATPTEFMIsFromCA { get; set; }
        public abstract class usrATPTEFMIsFromCA : BqlBool.Field<usrATPTEFMIsFromCA> { }
        #endregion

        /// <summary>
        /// Identifies if the document is either a Fund Establishment Bill, Fund Increase Bill and Close Fund Debit Adj Bill.
        /// 
        /// Purpose:
        /// - Acts as an identifier/flag for bills that require strict amount control
        /// - Controls field behavior in Check and Payment screen for both:
        ///   * Fund Establishment Bills
        ///   * Fund Increase Bills
        /// 
        /// Behavior:
        /// - When true, disables the Amount Paid field (curyAdjgAmt) in Document Details grid
        /// - Forces full payment of the bill amount (no partial payments allowed)
        /// - Used in APPaymentEntry to enforce payment validation rules
        /// 
        /// Usage:
        /// - Set to true when creating Fund Establishment or Fund Increase Bills
        /// - Validated during payment application to ensure full payment
        /// - Prevents partial payments for both bill types
        /// </summary>
        #region UsrATPTEFMIsAmountRestrictedBill
        [PXDBBool]
        [PXDefault(false, PersistingCheck = PXPersistingCheck.Nothing)]
        public bool? UsrATPTEFMIsAmountRestrictedBill { get; set; }
        public abstract class usrATPTEFMIsAmountRestrictedBill : BqlBool.Field<usrATPTEFMIsAmountRestrictedBill> { }
        #endregion

        #region UsrATPTEFMIsCaPrepaymentBill
        [PXDBBool]
        [PXDefault(false, PersistingCheck = PXPersistingCheck.Nothing)]
        public bool? UsrATPTEFMIsCaPrepaymentBill { get; set; }
        public abstract class usrATPTEFMIsCaPrepaymentBill : BqlBool.Field<usrATPTEFMIsCaPrepaymentBill> { }
        #endregion

        #region UsrATPTEFMIsFundEstablishmentBill
        [PXDBBool]
        [PXDefault(false, PersistingCheck = PXPersistingCheck.Nothing)]
        public bool? UsrATPTEFMIsFundEstablishmentBill { get; set; }
        public abstract class usrATPTEFMIsFundEstablishmentBill : BqlBool.Field<usrATPTEFMIsFundEstablishmentBill> { }
        #endregion

        #region UsrATPTEFMIsCloseFundDebitAdjBill
        [PXDBBool]
        [PXDefault(false, PersistingCheck = PXPersistingCheck.Nothing)]
        public bool? UsrATPTEFMIsCloseFundDebitAdjBill { get; set; }
        public abstract class usrATPTEFMIsCloseFundDebitAdjBill : BqlBool.Field<usrATPTEFMIsCloseFundDebitAdjBill> { }
        #endregion
    }
}