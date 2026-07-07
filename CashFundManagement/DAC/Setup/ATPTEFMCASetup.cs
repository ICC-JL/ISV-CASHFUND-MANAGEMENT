using CashFundManagement.Messages;
using PX.Data;
using PX.Data.BQL;
using PX.Objects.CS;
using PX.Objects.EP;
using PX.Objects.GL;
using PX.Objects.RQ;
using System;

namespace CashFundManagement.DAC.Setup {
    [Serializable]
    [PXCacheName(ATPTEFMMessages.ATPTEFMSetup)]
    [PXPrimaryGraph(typeof(CashFundManagement.BLC.ATPTEFMCASetupMaint))]
    public class ATPTEFMCASetup : CashFundManagement.DAC.Base.ATPTEFMAudit, IBqlTable
    {
        #region EnableCFM      
        [PXDBBool()]
        [PXDefault(false, PersistingCheck = PXPersistingCheck.Nothing)]
#if Version23R2
        [PXUIField(DisplayName = ATPTEFMMessages.EnableCFM, Visible = false)]
#else
        [PXUIField(DisplayName = ATPTEFMMessages.EnableCFM, Visible = true)]
#endif
        public bool? EnableCFM { get; set; }
        public abstract class enableCFM : BqlBool.Field<enableCFM> { }
#endregion

        #region CashadvanceRequestApproval
        [EPRequireApproval]
        [PXDefault(false, PersistingCheck = PXPersistingCheck.Nothing)]
        [PXUIField(DisplayName = ATPTEFMMessages.CashadvanceRequestApproval)]
        public virtual bool? CashAdvanceRequestApproval { get; set; }
        public abstract class cashAdvanceRequestApproval : BqlBool.Field<cashAdvanceRequestApproval> { }
        #endregion

        #region FundTransactionRequestApproval
        [EPRequireApproval]
        [PXDefault(false, PersistingCheck = PXPersistingCheck.Nothing)]
        [PXUIField(DisplayName = ATPTEFMMessages.FundTransactionRequestApproval)]
        public virtual bool? FundTransactionRequestApproval { get; set; }
        public abstract class fundTransactionRequestApproval : BqlBool.Field<fundTransactionRequestApproval> { }
        #endregion

        #region ReplenishmentRequestApproval
        [EPRequireApproval]
        [PXDefault(false, PersistingCheck = PXPersistingCheck.Nothing)]
        [PXUIField(DisplayName = ATPTEFMMessages.ReplenishmentRequestApproval)]
        public virtual bool? ReplenishmentRequestApproval { get; set; }
        public abstract class replenishmentRequestApproval : BqlBool.Field<replenishmentRequestApproval> { }
        #endregion

        #region MonthEndRequestApproval
        [EPRequireApproval]
        [PXDefault(false, PersistingCheck = PXPersistingCheck.Nothing)]
        [PXUIField(DisplayName = ATPTEFMMessages.MonthEndRequestApproval)]
        public virtual bool? MonthEndRequestApproval { get; set; }
        public abstract class monthEndRequestApproval : BqlBool.Field<monthEndRequestApproval> { }
        #endregion

        #region MaterialRequestRequestApproval
        [EPRequireApproval]
        [PXDefault(false, PersistingCheck = PXPersistingCheck.Nothing)]
        [PXUIField(DisplayName = ATPTEFMMessages.MaterialRequestRequestApproval)]
        public virtual bool? MaterialRequestRequestApproval { get; set; }
        public abstract class materialRequestRequestApproval : BqlBool.Field<materialRequestRequestApproval> { }
        #endregion

        #region CANumberingID
        [PXDBString(10, IsUnicode = true)]
        [PXDBDefault()]
        [PXSelector(typeof(Numbering.numberingID), DescriptionField = typeof(Numbering.descr))]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.UsrRLCashAdvanceNumberingID, Visibility = PXUIVisibility.Visible, Required = true)]
        public string CANumberingID { get; set; }
        public abstract class cANumberingID : BqlString.Field<cANumberingID> { }
        #endregion
        #region LiquidationNumberingID
        [PXDBString(10, IsUnicode = true)]
        [PXDBDefault()]
        [PXSelector(typeof(Numbering.numberingID), DescriptionField = typeof(Numbering.descr))]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.UsrATPTEFMLiqNumberingID, Visibility = PXUIVisibility.Visible, Required = true)]
        public string LiquidationNumberingID { get; set; }
        public abstract class liquidationNumberingID : BqlString.Field<liquidationNumberingID> { }
        #endregion
        #region AllowableCAAmt
        [PXDBBool]
        [PXDefault(false)]
        [PXUIField(DisplayName = ATPTEFMMessages.AllowableCAAmt)]
        public virtual bool? AllowableCAAmt { get; set; }
        public abstract class allowableCAAmt : BqlBool.Field<allowableCAAmt> { }
        #endregion
        #region RVAllowableCAAmt
        [PXDBInt()]
        [PXDefault(RQRequestClassBudget.None, PersistingCheck = PXPersistingCheck.Nothing)]
        [RQRequestClassBudget.List]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.RestrictionValidation, Visibility = PXUIVisibility.Visible)]
        [PXUIEnabled(typeof(Where<Current<allowableCAAmt>, Equal<boolTrue>>))]
        public int? RVAllowableCAAmt { get; set; }
        public abstract class rVAllowableCAAmt : BqlInt.Field<rVAllowableCAAmt> { }
        #endregion
        #region AllowableOpenCA
        [PXDBBool]
        [PXDefault(false)]
        [PXUIField(DisplayName = ATPTEFMMessages.AllowableOpenCA)]
        public virtual bool? AllowableOpenCA { get; set; }
        public abstract class allowableOpenCA : BqlBool.Field<allowableOpenCA> { }
        #endregion
        #region RVAllowableOpenCA
        [PXDBInt()]
        [PXDefault(RQRequestClassBudget.None, PersistingCheck = PXPersistingCheck.Nothing)]
        [RQRequestClassBudget.List]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.RestrictionValidation, Visibility = PXUIVisibility.Visible)]
        [PXUIEnabled(typeof(Where<Current<allowableOpenCA>, Equal<boolTrue>>))]
        public int? RVAllowableOpenCA { get; set; }
        public abstract class rVAllowableOpenCA : BqlInt.Field<rVAllowableOpenCA> { }
        #endregion
        #region RestrictCAEmployees
        [PXDBBool]
        [PXDefault(false)]
        [PXUIField(DisplayName = ATPTEFMMessages.RestrictCAEmployees)]
        public virtual bool? RestrictCAEmployees { get; set; }
        public abstract class restrictCAEmployees : BqlBool.Field<restrictCAEmployees> { }
        #endregion
        #region AutoReleaseAP
        [PXDBBool]
        [PXDefault(false)]
        [PXUIField(DisplayName = ATPTEFMMessages.AutoReleaseAP)]
        [PXUIEnabled(typeof(Where<isRequireApprovalCashAdvanceBill, NotEqual<True>>))]
        public virtual bool? AutoReleaseAP { get; set; }
        public abstract class autoReleaseAP : BqlBool.Field<autoReleaseAP> { }
        #endregion
        #region CopyAPNotes
        [PXDBBool]
        [PXDefault(false)]
        [PXUIField(DisplayName = ATPTEFMMessages.CopyAPNotes)]
        public virtual bool? CopyAPNotes { get; set; }
        public abstract class copyAPNotes : BqlBool.Field<copyAPNotes> { }
        #endregion
        #region RequireExtRef
        [PXDBBool]
        [PXDefault(false)]
        [PXUIField(DisplayName = ATPTEFMMessages.RequireExtRef)]
        public virtual bool? RequireExtRef { get; set; }
        public abstract class requireExtRef : BqlBool.Field<requireExtRef> { }
        #endregion
        //#region ErrorDuplicateExtRef
        //[PXDBBool]
        //[PXDefault(false)]
        //[PXUIField(DisplayName = ATPTEFMMessages.ErrorDupExtRef)]
        //public virtual bool? ErrorDuplicateExtRef { get; set; }
        //public abstract class errorDuplicateExtRef : BqlBool.Field<errorDuplicateExtRef> { }
        //#endregion
        #region LiquidationRule
        [PXDBString(1, IsFixed = true)]
        [PXDefault(Attributes.ATPTEFMLiquidationRuleAttribute.Employee, PersistingCheck = PXPersistingCheck.Nothing)]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.LiquidationRule)]
        [Attributes.ATPTEFMLiquidationRuleAttribute.ATPTEFMList]
        public string LiquidationRule { get; set; }
        public abstract class liquidationRule : BqlString.Field<liquidationRule> { }
        #endregion
        #region StandardAllowableDays
        [PXDBInt]
        [PXDefault(TypeCode.Int32, "0")]
        [PXUIField(DisplayName = ATPTEFMMessages.StandardAllowableDays)]
        [PXUIEnabled(typeof(Where<Current<liquidationRule>, Equal<Attributes.ATPTEFMLiquidationRuleAttribute.standard>>))]
        public virtual int? StandardAllowableDays { get; set; }
        public abstract class standardAllowableDays : BqlInt.Field<standardAllowableDays> { }
        #endregion
        #region ReClassAccoundID 
        //[PXSelector(
        //    typeof(Search<Account.accountID>),
        //    typeof(Account.accountCD),
        //    typeof(Account.description),
        //    SubstituteKey = typeof(Account.accountCD))]
        [Account(DisplayName = ATPTEFMMessages.ReClassAccountID)]
        [PXDBDefault]
        public int? ReClassAccoundID { get; set; }
        public abstract class reClassAccoundID : BqlInt.Field<reClassAccoundID> { }
        #endregion
        #region ReClassSubID 
        [SubAccount(typeof(reClassAccoundID), DisplayName = ATPTEFMMessages.ReClassSubID)]
        [PXDBDefault]
        public int? ReClassSubID { get; set; }
        public abstract class reClassSubID : BqlInt.Field<reClassSubID> { }
        #endregion

        #region LiquidationDateBasedOnWorkCalendar
        [PXDBBool]
        [PXDefault(false, PersistingCheck = PXPersistingCheck.Nothing)]
        [PXUIField(DisplayName = ATPTEFMMessages.LiquidationDateBasedOnWorkCalendar)]
        public virtual bool? LiquidationDateBasedOnWorkCalendar { get; set; }
        public abstract class liquidationDateBasedOnWorkCalendar : BqlBool.Field<liquidationDateBasedOnWorkCalendar> { }
        #endregion

        #region AutoApplyPPT
        [PXDBBool]
        [PXDefault(false)]
        [PXUIField(DisplayName = ATPTEFMMessages.AutoApplyPPT)]
        public virtual bool? AutoApplyPPT { get; set; }
        public abstract class autoApplyPPT : BqlBool.Field<autoApplyPPT> { }
        #endregion
        #region AutoApplyCredAdjPPT
        [PXDBBool]
        [PXDefault(false)]
        [PXUIField(DisplayName = ATPTEFMMessages.AutoApplyCredAdjPPT)]
        public virtual bool? AutoApplyCredAdjPPT { get; set; }
        public abstract class autoApplyCredAdjPPT : BqlBool.Field<autoApplyCredAdjPPT> { }
        #endregion

        #region AllowManualReceipts
        [PXDBBool]
        [PXUIField(DisplayName = ATPTEFMMessages.AllowManualReceipts)]
        [PXDefault(false, PersistingCheck = PXPersistingCheck.Nothing)]
        public virtual bool? AllowManualReceipts { get; set; }
        public abstract class allowManualReceipts : BqlBool.Field<allowManualReceipts> { }
        #endregion

        #region Require Vendor Details on Receipts Tab and Expense Receipt
        [PXDBBool]
        [PXUIField(DisplayName = ATPTEFMMessages.RequireVendorDetails)]
        [PXDefault(false, PersistingCheck = PXPersistingCheck.Nothing)]
        public virtual bool? RequireVendorDetails { get; set; }
        public abstract class requireVendorDetails : BqlBool.Field<requireVendorDetails> { }
        #endregion

        #region CashAdvanceAccountEnable
        [PXDBBool]
        [PXDefault(false)]
        [PXUIField(DisplayName = ATPTEFMMessages.EnableRequestAccount)]
        public virtual bool? CashAdvanceAccountEnable { get; set; }
        public abstract class cashAdvanceAccountEnable : BqlBool.Field<cashAdvanceAccountEnable> { }
        #endregion

        #region CashAdvanceSubAccountEnable
        [PXDBBool]
        [PXDefault(true)]
        [PXUIField(DisplayName = ATPTEFMMessages.EnableRequestSubAccount)]
        public virtual bool? CashAdvanceSubAccountEnable { get; set; }
        public abstract class cashAdvanceSubAccountEnable : BqlBool.Field<cashAdvanceSubAccountEnable> { }
        #endregion

        #region Allow Submission of Excess CA using Vendor Refund
        [PXDBBool]
        [PXUIField(DisplayName = ATPTEFMMessages.AllowSubmissionExcessCA)]
        [PXDefault(false, PersistingCheck = PXPersistingCheck.Nothing)]
        public virtual bool? AllowSubmissionExcessCA { get; set; }
        public abstract class allowSubmissionExcessCA : BqlBool.Field<allowSubmissionExcessCA> { }
        #endregion

        #region IsCashAdvanceMigration
        [PXDBBool]
        [PXDefault(false, PersistingCheck = PXPersistingCheck.Nothing)]
        [PXUIField(DisplayName = ATPTEFMMessages.ActivateCashAdvance)]
        public virtual bool? IsCashAdvanceMigration { get; set; }
        public abstract class isCashAdvanceMigration : BqlBool.Field<isCashAdvanceMigration> { }
        #endregion

        #region IsFundsMigration
        [PXDBBool]
        [PXDefault(false, PersistingCheck = PXPersistingCheck.Nothing)]
        [PXUIField(DisplayName = ATPTEFMMessages.ActivateFunds)]
        public virtual bool? IsFundsMigration { get; set; }
        public abstract class isFundsMigration : BqlBool.Field<isFundsMigration> { }
        #endregion

        #region Copy Notes and Files to Expense Claim
        [PXDBBool]
        [PXDefault(false)]
        [PXUIField(DisplayName = ATPTEFMMessages.CopyECNotes)]
        public virtual bool? CopyECNotes { get; set; }
        public abstract class copyECNotes : BqlBool.Field<copyECNotes> { }
        #endregion

        #region NoteID

        public abstract class noteID : BqlGuid.Field<noteID> { }
        protected Guid? _NoteID;
        [PXNote()]
        public virtual Guid? NoteID
        {
            get
            {
                return this._NoteID;
            }
            set
            {
                this._NoteID = value;
            }
        }
        #endregion

        #region IsFilterByEmployeeDelegates
        [PXDBBool]
        [PXDefault(false, PersistingCheck = PXPersistingCheck.Nothing)]
        [PXUIField(DisplayName = ATPTEFMMessages.FilteredView)]
        public virtual bool? IsFilterByEmployeeDelegates { get; set; }
        public abstract class isFilterByEmployeeDelegates : BqlBool.Field<isFilterByEmployeeDelegates> { }
        #endregion

        #region IsRequireApprovalCashAdvanceBill
        [PXDBBool]
        [PXDefault(false, PersistingCheck = PXPersistingCheck.Nothing)]
        [PXUIField(DisplayName = ATPTEFMMessages.RequireApprovalOnCABill)]
        public virtual bool? IsRequireApprovalCashAdvanceBill { get; set; }
        public abstract class isRequireApprovalCashAdvanceBill : BqlBool.Field<isRequireApprovalCashAdvanceBill> { }
        #endregion

        #region IsRequireApprovalLiquidationBill
        [PXDBBool]
        [PXDefault(false, PersistingCheck = PXPersistingCheck.Nothing)]
        [PXUIField(DisplayName = ATPTEFMMessages.RequireApprovalOnLiquidationBill)]
        public virtual bool? IsRequireApprovalLiquidationBill { get; set; }
        public abstract class isRequireApprovalLiquidationBill : BqlBool.Field<isRequireApprovalLiquidationBill> { }
        #endregion

        #region SetNonStockItemDescriptionAsDefault
        [PXDBBool]
        [PXUIField(DisplayName = "Set Non-Stock Item Description as default Line Description")]
        public virtual bool? SetNonStockItemDescriptionAsDefault { get; set; }
        public abstract class setNonStockItemDescriptionAsDefault : PX.Data.BQL.BqlBool.Field<setNonStockItemDescriptionAsDefault> { }
        #endregion
    }
}