using CashFundManagement.Attributes;
using PX.Data;
using PX.Data.BQL;
using PX.Objects.AP;
using PX.Objects.CS;
using PX.Objects.EP;
using System;
using static PX.Data.BQL.BqlPlaceholder;
using static PX.Objects.EP.EPExpenseClaimDetails;

namespace CashFundManagement.DAC {
    [Serializable]
    [PXCacheName("Cash Fund Management Data Fix")]
    public class ATPTEFMCashFundDataFix : Base.ATPTEFMAudit, IBqlTable
    {
        #region Password
        [PXUIField(DisplayName = "Password")]
        [PXDefault()]
        [PXUIEnabled(typeof(
                    Where<enableFields, Equal<False>>))]
        public virtual string Password { get; set; }
        public abstract class password : BqlString.Field<password> { }
        #endregion

        #region EnableFields
        [PXBool]
        [PXDefault(false, PersistingCheck = PXPersistingCheck.Nothing)]
        public virtual bool? EnableFields { get; set; }
        public abstract class enableFields : PX.Data.BQL.BqlBool.Field<enableFields> { }
        #endregion

        #region Cash Advance Module

        #region CAPendingForLiquidationToClosed
        [PXBool]
        [PXUIField(DisplayName = "Cash Advance: Status = Pending for Liquidation")]
        [PXUnboundDefault(false, PersistingCheck = PXPersistingCheck.Nothing)]
        [PXUIEnabled(typeof(
                    Where<enableFields, Equal<True>>))]
        public virtual bool? CAPendingForLiquidationToClosed { get; set; }
        public abstract class cAPendingForLiquidationToClosed : BqlBool.Field<cAPendingForLiquidationToClosed> { }
        #endregion

        #region Unable to cancel due to empty Unit Cost
        [PXBool]
        [PXUIField(DisplayName = "Update Unit Cost")]
        [PXUnboundDefault(false, PersistingCheck = PXPersistingCheck.Nothing)]
        [PXUIEnabled(typeof(
                    Where<enableFields, Equal<True>>))]
        public virtual bool? BlankUnitCost { get; set; }
        public abstract class blankUnitCost : BqlBool.Field<blankUnitCost> { }
        #endregion

        #region Unable to cancel due to empty Unit Cost
        [PXBool]
        [PXUIField(DisplayName = "Update Receipt Unit Cost")]
        [PXUnboundDefault(false, PersistingCheck = PXPersistingCheck.Nothing)]
        [PXUIEnabled(typeof(
                    Where<enableFields, Equal<True>>))]
        public virtual bool? BlankReceiptUnitCost { get; set; }
        public abstract class blankReceiptUnitCost : BqlBool.Field<blankReceiptUnitCost> { }
        #endregion

        #region Unlink CA Bill Ref Nbr
        [PXString(255, IsUnicode = true, IsKey = true, InputMask = "")]
        [PXUnboundDefault()]
        [PXUIField(DisplayName = "Reference Nbr.")]
        [PXUIEnabled(typeof(
                    Where<enableFields, Equal<True>>))]
        [PXSelector(typeof(Search<
            ATPTEFMCashAdvance.cashAdvanceNbr, 
            Where<ATPTEFMCashAdvance.billRefNbr, IsNotNull, 
                And<ATPTEFMCashAdvance.billRefNbr, NotEqual<Empty>,
                And<ATPTEFMCashAdvance.billRefNbr, NotIn2<Search<APInvoice.refNbr>>>>>>), 
            typeof(ATPTEFMCashAdvance.cashAdvanceNbr),
            typeof(ATPTEFMCashAdvance.date), 
            typeof(ATPTEFMCashAdvance.status), 
            typeof(ATPTEFMCashAdvance.descr), 
            typeof(ATPTEFMCashAdvance.billRefNbr))]
        public virtual string UnlinkCABillRefNbr { get; set; }
        public abstract class unlinkCABillRefNbr : PX.Data.BQL.BqlString.Field<unlinkCABillRefNbr> { }
        #endregion

        #region CA Initial Liquidation Datafix for Old data
        [PXBool]
        [PXUIField(DisplayName = "CA Initial Liquidation Datafix for Old data")]
        [PXUnboundDefault(false, PersistingCheck = PXPersistingCheck.Nothing)]
        [PXUIEnabled(typeof(
            Where<enableFields, Equal<True>>))]
        public virtual bool? CAInitialLiquidationDatafixForOldData { get; set; }
        public abstract class cAInitialLiquidationDatafixForOldData : BqlBool.Field<cAInitialLiquidationDatafixForOldData> { }
        #endregion

        #region EFFOverrideReceiptsSetToFalse
        [PXBool]
        [PXUIField(DisplayName = "EFF Override Receipts Set To False")]
        [PXUnboundDefault(false, PersistingCheck = PXPersistingCheck.Nothing)]
        [PXUIEnabled(typeof(
                    Where<enableFields, Equal<True>>))]
        public virtual bool? EFFOverrideReceiptsSetToFalse { get; set; }
        public abstract class eFFOverrideReceiptsSetToFalse : BqlBool.Field<eFFOverrideReceiptsSetToFalse> { }
        #endregion

        #region CaPPMBalanceNotEqualToBalance
        [PXBool]
        [PXUIField(DisplayName = "PPM Balance Not Equal To Balance")]
        [PXUnboundDefault(false, PersistingCheck = PXPersistingCheck.Nothing)]
        [PXUIEnabled(typeof(
                    Where<enableFields, Equal<True>>))]
        public virtual bool? CaPPMBalanceNotEqualToBalance { get; set; }
        public abstract class caPPMBalanceNotEqualToBalance : BqlBool.Field<caPPMBalanceNotEqualToBalance> { }
        #endregion

        #region CA w/o Line description Datafix
        [PXBool]
        [PXUIField(DisplayName = "CA w/o Line description Datafix")]
        [PXUnboundDefault(false, PersistingCheck = PXPersistingCheck.Nothing)]
        [PXUIEnabled(typeof(
        Where<enableFields, Equal<True>>))]
        public virtual bool? CAwoLineDescriptionDatafix { get; set; }
        public abstract class cAwoLineDescriptionDatafix : BqlBool.Field<cAwoLineDescriptionDatafix> { }
        #endregion

        #region Closed CA with Balance
        [PXString(255, IsUnicode = true, IsKey = true, InputMask = "")]
        [PXUnboundDefault()]
        [PXUIField(DisplayName = "CA RefNbr")]
        [PXUIEnabled(typeof(
                    Where<enableFields, Equal<True>>))]
        [PXSelector(typeof(Search<
            ATPTEFMCashAdvance.cashAdvanceNbr, 
                Where<ATPTEFMCashAdvance.status, Equal<ATPTEFMCashAdvanceStatusAttribute.closedValue>, 
                    And<ATPTEFMCashAdvance.curyChangeAmount, Greater<decimal0>>>>),
            typeof(ATPTEFMCashAdvance.cashAdvanceNbr),
            typeof(ATPTEFMCashAdvance.date),
            typeof(ATPTEFMCashAdvance.status),
            typeof(ATPTEFMCashAdvance.descr),
            typeof(ATPTEFMCashAdvance.billRefNbr))]
        public virtual string CloseCAWithBalance { get; set; }
        public abstract class closeCAWithBalance : PX.Data.BQL.BqlString.Field<closeCAWithBalance> { }
        #endregion

        #region CAReceiptAlreadyCancelledButExistInCAReceiptsTab
        [PXBool]
        [PXUIField(DisplayName = "CA Receipt Already Cancelled But Exist In CA Receipts Tab")]
        [PXUnboundDefault(false, PersistingCheck = PXPersistingCheck.Nothing)]
        [PXUIEnabled(typeof(
                    Where<enableFields, Equal<True>>))]
        public virtual bool? CAReceiptAlreadyCancelledButExistInCAReceiptsTab { get; set; }
        public abstract class cAReceiptAlreadyCancelledButExistInCAReceiptsTab : BqlBool.Field<cAReceiptAlreadyCancelledButExistInCAReceiptsTab> { }
        #endregion

        #region CAVendorRefundWrongAmt
        [PXString(255, IsUnicode = true, IsKey = true, InputMask = "")]
        [PXUnboundDefault()]
        [PXUIField(DisplayName = "CA RefNbr (Wrong Vendor Refund Amt)")]
        [PXUIEnabled(typeof(
                    Where<enableFields, Equal<True>>))]
        [PXSelector(typeof(Search<
            ATPTEFMCashAdvance.cashAdvanceNbr,
                Where<ATPTEFMCashAdvance.status, Equal<ATPTEFMCashAdvanceStatusAttribute.pendingLiquidationValue>>>),
            typeof(ATPTEFMCashAdvance.cashAdvanceNbr),
            typeof(ATPTEFMCashAdvance.date),
            typeof(ATPTEFMCashAdvance.status),
            typeof(ATPTEFMCashAdvance.descr))]
        public virtual string CAVendorRefundWrongAmt { get; set; }
        public abstract class cAVendorRefundWrongAmt : PX.Data.BQL.BqlString.Field<cAVendorRefundWrongAmt> { }
        #endregion

        #region CleanUpCAReceiptsIssues
        [PXString(255, IsUnicode = true, IsKey = true, InputMask = "")]
        [PXUnboundDefault()]
        [PXUIField(DisplayName = "CA RefNbr")]
        [PXUIEnabled(typeof(
                    Where<enableFields, Equal<True>>))]
        [PXSelector(typeof(Search<
            ATPTEFMCashAdvance.cashAdvanceNbr>),
            typeof(ATPTEFMCashAdvance.cashAdvanceNbr),
            typeof(ATPTEFMCashAdvance.date),
            typeof(ATPTEFMCashAdvance.status),
            typeof(ATPTEFMCashAdvance.descr),
            typeof(ATPTEFMCashAdvance.billRefNbr))]
        public virtual string CleanUpCAReceiptsIssues { get; set; }
        public abstract class cleanUpCAReceiptsIssues : PX.Data.BQL.BqlString.Field<cleanUpCAReceiptsIssues> { }
        #endregion

        #region DeleteCAReceiptsWithRefs
        [PXString(255, IsUnicode = true, IsKey = true, InputMask = "")]
        [PXUnboundDefault()]
        [PXUIField(DisplayName = "CA RefNbr (delete receipts with request detail = 0)")]
        [PXUIEnabled(typeof(
                    Where<enableFields, Equal<True>>))]
        [PXSelector(typeof(Search<
            ATPTEFMCashAdvance.cashAdvanceNbr>),
            typeof(ATPTEFMCashAdvance.cashAdvanceNbr),
            typeof(ATPTEFMCashAdvance.date),
            typeof(ATPTEFMCashAdvance.status),
            typeof(ATPTEFMCashAdvance.descr),
            typeof(ATPTEFMCashAdvance.billRefNbr))]
        public virtual string DeleteCAReceiptsWithRefs { get; set; }
        public abstract class deleteCAReceiptsWithRefs : PX.Data.BQL.BqlString.Field<deleteCAReceiptsWithRefs> { }
        #endregion

        #region CAZeroLiquidationAmt
        [PXString(255, IsUnicode = true, IsKey = true, InputMask = "")]
        [PXUnboundDefault()]
        [PXUIField(DisplayName = "CA RefNbr")]
        [PXUIEnabled(typeof(
                    Where<enableFields, Equal<True>>))]
        [PXSelector(typeof(Search<
            ATPTEFMCashAdvance.cashAdvanceNbr,
                Where<ATPTEFMCashAdvance.status, Equal<ATPTEFMCashAdvanceStatusAttribute.pendingLiquidationValue>>>),
            typeof(ATPTEFMCashAdvance.cashAdvanceNbr),
            typeof(ATPTEFMCashAdvance.date),
            typeof(ATPTEFMCashAdvance.status),
            typeof(ATPTEFMCashAdvance.descr))]
        public virtual string CAZeroLiquidationAmt { get; set; }
        public abstract class cAZeroLiquidationAmt : PX.Data.BQL.BqlString.Field<cAZeroLiquidationAmt> { }
        #endregion

        #region CAStuckRefundFixer
        /// <summary>
        /// Data fixer for stuck CA refunds where the Documents to Apply amounts mismatch with the PPM Balance.
        /// This field allows selecting a CA with a vendor refund to sync the APAdjust amounts with the remaining PPM Balance.
        /// </summary>
        [PXString(255, IsUnicode = true, IsKey = true, InputMask = "")]
        [PXUnboundDefault()]
        [PXUIField(DisplayName = "CA RefNbr (Stuck Refund)")]
        [PXUIEnabled(typeof(
                    Where<enableFields, Equal<True>>))]
        [PXSelector(typeof(Search<
            ATPTEFMCashAdvance.cashAdvanceNbr,
                Where<ATPTEFMCashAdvance.vendorRefundRefNbr, IsNotNull,
                    And<ATPTEFMCashAdvance.vendorRefundRefNbr, NotEqual<Empty>>>>),
            typeof(ATPTEFMCashAdvance.cashAdvanceNbr),
            typeof(ATPTEFMCashAdvance.date),
            typeof(ATPTEFMCashAdvance.status),
            typeof(ATPTEFMCashAdvance.vendorRefundRefNbr),
            typeof(ATPTEFMCashAdvance.descr))]
        public virtual string CAStuckRefundFixer { get; set; }
        public abstract class cAStuckRefundFixer : BqlString.Field<cAStuckRefundFixer> { }
        #endregion

        #region CARecalcLiquidationAmt
        /// <summary>
        /// Data fixer that recalculates the CA Liquidation amount (CuryActualSpentAmount)
        /// from the sum of non-reversed CA Receipt Details. Used when liquidation summary
        /// does not reflect the actual released liquidations (e.g. after a stuck refund fix).
        /// </summary>
        [PXString(255, IsUnicode = true, IsKey = true, InputMask = "")]
        [PXUnboundDefault()]
        [PXUIField(DisplayName = "CA RefNbr (Recalc Liquidation Amt)")]
        [PXUIEnabled(typeof(
                    Where<enableFields, Equal<True>>))]
        [PXSelector(typeof(Search<
            ATPTEFMCashAdvance.cashAdvanceNbr>),
            typeof(ATPTEFMCashAdvance.cashAdvanceNbr),
            typeof(ATPTEFMCashAdvance.date),
            typeof(ATPTEFMCashAdvance.status),
            typeof(ATPTEFMCashAdvance.curyActualSpentAmount),
            typeof(ATPTEFMCashAdvance.curyChangeAmount),
            typeof(ATPTEFMCashAdvance.vendorRefundRefNbr),
            typeof(ATPTEFMCashAdvance.descr))]
        public virtual string CARecalcLiquidationAmt { get; set; }
        public abstract class cARecalcLiquidationAmt : BqlString.Field<cARecalcLiquidationAmt> { }
        #endregion

        #endregion

        #region Expense Claim Module
        #region Released EC with no Bill created
        #region RefNbr
        [PXString(255, IsUnicode = true, InputMask = "")]
        [PXUnboundDefault()]
        [PXUIField(DisplayName = "Reference Nbr.")]
        [PXUIEnabled(typeof(
                    Where<enableFields, Equal<True>>))]
        [PXSelector(typeof(
            Search3<
                EPExpenseClaim.refNbr,
                OrderBy<
                    Desc<EPExpenseClaim.refNbr>>>), typeof(EPExpenseClaim.docDate),
            typeof(EPExpenseClaim.refNbr), typeof(EPExpenseClaim.status), typeof(EPExpenseClaim.docDesc), typeof(EPExpenseClaim.curyDocBal), typeof(EPExpenseClaim.curyID),
            typeof(EPEmployee.acctName), typeof(EPExpenseClaim.departmentID))]
        public virtual string RefNbr { get; set; }
        public abstract class refNbr : PX.Data.BQL.BqlString.Field<refNbr> { }
        #endregion
        #region Status
        [PXString(1, IsFixed = true, InputMask = "")]
        [PXUIField(DisplayName = "Status")]
        [EPExpenseClaimStatus.List]
        [PXUnboundDefault()]
        [PXUIEnabled(typeof(
                    Where<enableFields, Equal<True>>))]
        public virtual string Status { get; set; }
        public abstract class status : PX.Data.BQL.BqlString.Field<status> { }
        #endregion
        #endregion
        #region EC with missing details
        #region ECMissingDetailsDataFix
        [PXBool]
        [PXUIField(DisplayName = "Data Fix for Expense Claim with Bill created but missing its Expense Claim Details")]
        [PXUnboundDefault(false, PersistingCheck = PXPersistingCheck.Nothing)]
        [PXUIEnabled(typeof(
                    Where<enableFields, Equal<True>>))]
        public virtual bool? ECMissingDetailsDataFix { get; set; }
        public abstract class eCMissingDetailsDataFix : BqlBool.Field<eCMissingDetailsDataFix> { }
        #endregion
        #endregion
        #region ECDeleteApprovalsForCancelledStatus
        [PXString(255, IsUnicode = true, InputMask = "")]
        [PXUnboundDefault()]
        [PXUIField(DisplayName = "Reference Nbr.")]
        [PXUIEnabled(typeof(
                    Where<enableFields, Equal<True>>))]
        [PXSelector(typeof(
            Search<
                EPExpenseClaim.refNbr, 
                Where<EPExpenseClaim.status, 
                    Equal<ATPTEFMExpenseClaimStatusAttribute.cancelledValue>>,
                OrderBy<
                    Desc<EPExpenseClaim.refNbr>>>), typeof(EPExpenseClaim.docDate),
            typeof(EPExpenseClaim.refNbr), typeof(EPExpenseClaim.status), typeof(EPExpenseClaim.docDesc), typeof(EPExpenseClaim.curyDocBal), typeof(EPExpenseClaim.curyID),
            typeof(EPEmployee.acctName), typeof(EPExpenseClaim.departmentID))]
        public virtual string ECDeleteApprovalsForCancelledStatus { get; set; }
        public abstract class eCDeleteApprovalsForCancelledStatus : PX.Data.BQL.BqlString.Field<eCDeleteApprovalsForCancelledStatus> { }
        #endregion
        #endregion

        #region Replenishment

        #region EmptyReplenishmentDetailReplenishmentRefNbr
        [PXString(255, IsUnicode = true, IsKey = true, InputMask = "")]
        [PXUnboundDefault()]
        [PXUIField(DisplayName = "Replenishment Nbr.")]
        [PXUIEnabled(typeof(
                    Where<enableFields, Equal<True>>))]
        [PXSelector(typeof(Search<
            ATPTEFMReplenishment.replenishmentNbr,
            Where<ATPTEFMReplenishment.replenishmentNbr, NotIn2<Search<ATPTEFMReplenishmentDetail.replenishmentNbr>>,
                And<ATPTEFMReplenishment.claimAmount, Greater<decimal0>>>,
            OrderBy<
                Desc<ATPTEFMReplenishment.replenishmentNbr>>>),
            typeof(ATPTEFMReplenishment.replenishmentNbr),
            typeof(ATPTEFMReplenishment.date),
            typeof(ATPTEFMReplenishment.status),
            typeof(ATPTEFMReplenishment.descr),
            typeof(ATPTEFMReplenishment.employeeID))]
        public virtual string EmptyReplenishmentDetailReplenishmentRefNbr { get; set; }
        public abstract class emptyReplenishmentDetailReplenishmentRefNbr : PX.Data.BQL.BqlString.Field<emptyReplenishmentDetailReplenishmentRefNbr> { }
        #endregion

        #region Data Fix For FT Receipts with Replenishment Ref Nbr but already Deleted in Replenishment

        #region DataFixForFTReceiptswithReplenishmentRefNbrbutalreadyDeletedinReplenishment
        [PXBool]
        [PXUIField(DisplayName = "Data Fix For FT Receipts with Replenishment Ref Nbr but already Deleted in Replenishment")]
        [PXUnboundDefault(false, PersistingCheck = PXPersistingCheck.Nothing)]
        [PXUIEnabled(typeof(
                    Where<enableFields, Equal<True>>))]
        public virtual bool? DataFixForFTReceiptswithReplenishmentRefNbrbutalreadyDeletedinReplenishment { get; set; }
        public abstract class dataFixForFTReceiptswithReplenishmentRefNbrbutalreadyDeletedinReplenishment : BqlBool.Field<dataFixForFTReceiptswithReplenishmentRefNbrbutalreadyDeletedinReplenishment> { }
        #endregion

        #endregion

        #region Replenishment Unable to Release due to FT Null Change Amount Field
        [PXString(255, IsUnicode = true, IsKey = true, InputMask = "")]
        [PXUnboundDefault()]
        [PXUIField(DisplayName = "Replenishment Unable to Release due to FT Null Change Amount Field Data Fix")]
        [PXUIEnabled(typeof(
                    Where<enableFields, Equal<True>>))]
        [PXSelector(typeof(Search<
            ATPTEFMReplenishment.replenishmentNbr>),
            typeof(ATPTEFMReplenishment.replenishmentNbr),
            typeof(ATPTEFMReplenishment.date),
            typeof(ATPTEFMReplenishment.status),
            typeof(ATPTEFMReplenishment.descr),
            typeof(ATPTEFMReplenishment.employeeID))]
        public virtual string NullChangeAmountDataFix { get; set; }
        public abstract class nullChangeAmountDataFix : PX.Data.BQL.BqlString.Field<nullChangeAmountDataFix> { }
        #endregion

        #region For those replenishment bills that have already been reversed but still have a status of closed that needs to be reopen.
        [PXString(255, IsUnicode = true, IsKey = true, InputMask = "")]
        [PXUnboundDefault()]
        [PXUIField(DisplayName = "Replenishment Nbr")]
        [PXUIEnabled(typeof(
                    Where<enableFields, Equal<True>>))]
        [PXSelector(typeof(Search<
            ATPTEFMReplenishment.replenishmentNbr>),
            typeof(ATPTEFMReplenishment.replenishmentNbr),
            typeof(ATPTEFMReplenishment.date),
            typeof(ATPTEFMReplenishment.status),
            typeof(ATPTEFMReplenishment.descr),
            typeof(ATPTEFMReplenishment.employeeID))]
        public virtual string ReplenishmentNrbNeedsToBeOpen { get; set; }
        public abstract class replenishmentNrbNeedsToBeOpen : PX.Data.BQL.BqlString.Field<nullChangeAmountDataFix> { }
        #endregion

        #endregion

        #region Fund Module

        #region Insert Fund Transaction Limit Defaults to Old Funds

        #region InsertDefaultForOldFundsFundCD
        [PXBool]
        [PXUIField(DisplayName = "Insert Default Values for Old Funds")]
        [PXUnboundDefault(false, PersistingCheck = PXPersistingCheck.Nothing)]
        [PXUIEnabled(typeof(
                    Where<enableFields, Equal<True>>))]
        public virtual bool? InsertDefaultForOldFundsFundCD { get; set; }
        public abstract class insertDefaultForOldFundsFundCD : BqlBool.Field<insertDefaultForOldFundsFundCD> { }
        #endregion

        #endregion

        #region Update Fund Clearing Accounts
        /// <summary>
        /// Data fixer that updates existing funds with empty or null clearing account
        /// or clearing subaccount values using the clearing account and subaccount
        /// defined in Fund Management Preferences (ATPTEFMSetup).
        /// </summary>
        [PXBool]
        [PXUIField(DisplayName = "Update Fund Clearing Accounts")]
        [PXUnboundDefault(false, PersistingCheck = PXPersistingCheck.Nothing)]
        [PXUIEnabled(typeof(
                    Where<enableFields, Equal<True>>))]
        public virtual bool? UpdateFundClearingAccounts { get; set; }
        public abstract class updateFundClearingAccounts : BqlBool.Field<updateFundClearingAccounts> { }
        #endregion

        #region Change Replenishment Point
        #region FundReplenishPoint                
        [PXString(256, InputMask = ">CCCCCCCCCCCCCCC", IsUnicode = true)]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.RefNbr)]
        [PXSelector(typeof(Search<
            ATPTEFMFund.fundCD, Where<ATPTEFMFund.fundCD, IsNotNull>>),
            typeof(ATPTEFMFund.branchID),
            typeof(ATPTEFMFund.fundType),
            typeof(fundCD),
            typeof(status),
            typeof(ATPTEFMFund.documentDate),
            typeof(ATPTEFMFund.descr),
            typeof(ATPTEFMFund.initialFund), ValidateValue = false)]
        public virtual string FundReplenishPoint { get; set; }
        public abstract class fundReplenishPoint : BqlString.Field<fundReplenishPoint> { }
        #endregion

        #region ReplenishPointPercent
        [PXDecimal(2, MinValue = 0, MaxValue = 100)]
        [PXUnboundDefault]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.ReplenishPointPercent)]
        public virtual decimal? ReplenishPointPercent { get; set; }
        public abstract class replenishPointPercent : BqlDecimal.Field<replenishPointPercent> { }
        #endregion
        #endregion

        #region DataFix for QMAZ Funds (Numbering sequence / Deleted Fund Issue)
        [PXString(15, IsKey = true, InputMask = ">CCCCCCCCCCCCCCC", IsUnicode = true)]
        [PXUIField(DisplayName = "Qmaz DataFix Fund RefNbr")]
        [PXSelector(typeof(ATPTEFMFund.fundCD))]
        [PXUIEnabled(typeof(
                    Where<enableFields, Equal<True>>))]
        public virtual string QmazFundCD { get; set; }
        public abstract class qmazFundCD : BqlString.Field<qmazFundCD> { }
        #endregion

        #endregion

        #region Fund Import Data from unbound to bound
        #region FundCD                
        [PXString(15, InputMask = ">CCCCCCCCCCCCCCC", IsUnicode = true)]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.RefNbr)]
        [PXSelector(typeof(Search<
            ATPTEFMFund.fundCD, Where<ATPTEFMFund.fundCD, IsNotNull>>),
            typeof(ATPTEFMFund.branchID),
            typeof(ATPTEFMFund.fundType),
            typeof(fundCD),
            typeof(status),
            typeof(ATPTEFMFund.documentDate),
            typeof(ATPTEFMFund.descr),
            typeof(ATPTEFMFund.initialFund), ValidateValue = false)]
        public virtual string FundCD { get; set; }
        public abstract class fundCD : BqlString.Field<fundCD> { }
        #endregion
        #endregion

        #region Fund Transaction History Balance And Sorting Fixer
        #region FundHistoryBalanceFixer                
        [PXString(15, InputMask = ">CCCCCCCCCCCCCCC", IsUnicode = true)]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.RefNbr)]
        [PXSelector(typeof(Search<
            ATPTEFMFund.fundCD, Where<ATPTEFMFund.fundCD, IsNotNull>>),
            typeof(ATPTEFMFund.branchID),
            typeof(ATPTEFMFund.fundType),
            typeof(fundCD),
            typeof(status),
            typeof(ATPTEFMFund.documentDate),
            typeof(ATPTEFMFund.descr),
            typeof(ATPTEFMFund.initialFund), ValidateValue = false)]
        public virtual string FundHistoryBalanceAndSortingFixer { get; set; }
        public abstract class fundHistoryBalanceAndSortingFixer : BqlString.Field<fundHistoryBalanceAndSortingFixer> { }
        #endregion
        #endregion

        #region Fund Transaction History Balance Fixer
        #region FundHistoryBalanceFixer                
        [PXString(15, InputMask = ">CCCCCCCCCCCCCCC", IsUnicode = true)]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.RefNbr)]
        [PXSelector(typeof(Search<
            ATPTEFMFund.fundCD, Where<ATPTEFMFund.fundCD, IsNotNull>>),
            typeof(ATPTEFMFund.branchID),
            typeof(ATPTEFMFund.fundType),
            typeof(fundCD),
            typeof(status),
            typeof(ATPTEFMFund.documentDate),
            typeof(ATPTEFMFund.descr),
            typeof(ATPTEFMFund.initialFund), ValidateValue = false)]
        public virtual string FundHistoryBalanceFixer { get; set; }
        public abstract class fundHistoryBalanceFixer : BqlString.Field<fundHistoryBalanceFixer> { }
        #endregion
        #endregion

        #region Fund History Remove Duplicate And Fix Balance
        #region FundHistoryBalanceFixer                
        [PXString(15, InputMask = ">CCCCCCCCCCCCCCC", IsUnicode = true)]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.RefNbr)]
        [PXSelector(typeof(Search<
            ATPTEFMFund.fundCD, Where<ATPTEFMFund.fundCD, IsNotNull>>),
            typeof(ATPTEFMFund.branchID),
            typeof(ATPTEFMFund.fundType),
            typeof(fundCD),
            typeof(status),
            typeof(ATPTEFMFund.documentDate),
            typeof(ATPTEFMFund.descr),
            typeof(ATPTEFMFund.initialFund), ValidateValue = false)]
        public virtual string FundHistoryDuplicateBalanceFixer { get; set; }
        public abstract class fundHistoryDuplicateBalanceFixer : BqlString.Field<fundHistoryDuplicateBalanceFixer> { }
        #endregion
        #endregion

        #region Fund Import Data Currency Fields
        #region fundCDCurrency                
        [PXString(15, InputMask = ">CCCCCCCCCCCCCCC", IsUnicode = true)]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.RefNbr)]
        [PXSelector(typeof(Search<
            ATPTEFMFund.fundCD, Where<ATPTEFMFund.fundCD, IsNotNull>>),
            typeof(ATPTEFMFund.branchID),
            typeof(ATPTEFMFund.fundType),
            typeof(fundCD),
            typeof(status),
            typeof(ATPTEFMFund.documentDate),
            typeof(ATPTEFMFund.descr),
            typeof(ATPTEFMFund.initialFund), ValidateValue = false)]
        public virtual string FundCDCurrency { get; set; }
        public abstract class fundCDCurrency : BqlString.Field<fundCDCurrency> { }
        #endregion
        #endregion

        #region FT Receipts BranchID Migration
        [PXBool]
        [PXUIField(DisplayName = "FT Receipts Branch Migration")]
        [PXUnboundDefault(false, PersistingCheck = PXPersistingCheck.Nothing)]
        public virtual bool? FTReceiptsBranchIDMigration { get; set; }
        public abstract class fTReceiptsBranchIDMigration : BqlBool.Field<fTReceiptsBranchIDMigration> { }
        #endregion

        #region Remove Detail Lines from Reimbursement Type FT
        [PXString(15, InputMask = ">CCCCCCCCCCCCCCC", IsUnicode = true)]
        [PXUnboundDefault(PersistingCheck = PXPersistingCheck.Nothing)]
        [PXUIField(DisplayName = "Reimbursement FT with Detail Lines to Remove")]
        [PXUIEnabled(typeof(Where<enableFields, Equal<True>>))]
        [PXSelector(typeof(Search<ATPTEFMFundTransaction.refNbr,
            Where<ATPTEFMFundTransaction.fundTransactionType, Equal<ATPTEFMFundTransactionTypeAttribute.reimbursementValue>,
                And<Exists<Select<ATPTEFMFundTransactionDetail,
                    Where<ATPTEFMFundTransactionDetail.fundTransactionRefNbr, Equal<ATPTEFMFundTransaction.refNbr>>>>>>>),
            typeof(ATPTEFMFundTransaction.refNbr),
            typeof(ATPTEFMFundTransaction.descr),
            typeof(ATPTEFMFundTransaction.status),
            ValidateValue = false)]
        public virtual string RemoveReimbursementFTDetails { get; set; }
        public abstract class removeReimbursementFTDetails : BqlString.Field<removeReimbursementFTDetails> { }
        #endregion

        #region Budget Update

        #region Update CA Budget Enabled checkbox for Old Data
        [PXBool]
        [PXUIField(DisplayName = "Update CA Budget Enabled checkbox for Old Data")]
        [PXUnboundDefault(false, PersistingCheck = PXPersistingCheck.Nothing)]
        [PXUIEnabled(typeof(Where<enableFields, Equal<True>>))]
        public virtual bool? UpdateCABudgetEnabled { get; set; }
        public abstract class updateCABudgetEnabled : BqlBool.Field<updateCABudgetEnabled> { }
        #endregion

        #region Update FT Budget Enabled checkbox for Old Data
        [PXBool]
        [PXUIField(DisplayName = "Update FT Budget Enabled checkbox for Old Data")]
        [PXUnboundDefault(false, PersistingCheck = PXPersistingCheck.Nothing)]
        [PXUIEnabled(typeof(Where<enableFields, Equal<True>>))]
        public virtual bool? UpdateFTBudgetEnabled { get; set; }
        public abstract class updateFTBudgetEnabled : BqlBool.Field<updateFTBudgetEnabled> { }
        #endregion

        #region Update RFP Budget Enabled checkbox for Old Data
        [PXBool]
        [PXUIField(DisplayName = "Update RFP Budget Enabled checkbox for Old Data")]
        [PXUnboundDefault(false, PersistingCheck = PXPersistingCheck.Nothing)]
        [PXUIEnabled(typeof(Where<enableFields, Equal<True>>))]
        public virtual bool? UpdateRFPBudgetEnabled { get; set; }
        public abstract class updateRFPBudgetEnabled : BqlBool.Field<updateRFPBudgetEnabled> { }
        #endregion

        #region Update Bills Budget Enabled checkbox for Old Data
        [PXBool]
        [PXUIField(DisplayName = "Update Bills Budget Enabled checkbox for Old Data")]
        [PXUnboundDefault(false, PersistingCheck = PXPersistingCheck.Nothing)]
        [PXUIEnabled(typeof(Where<enableFields, Equal<True>>))]
        public virtual bool? UpdateBillsBudgetEnabled { get; set; }
        public abstract class updateBillsBudgetEnabled : BqlBool.Field<updateBillsBudgetEnabled> { }
        #endregion

        #endregion

        #region Fund Transaction Module

        #region FT Initial Liquidation Datafix for Old data
        [PXBool]
        [PXUIField(DisplayName = "FT Initial Liquidation Datafix for Old data")]
        [PXUnboundDefault(false, PersistingCheck = PXPersistingCheck.Nothing)]
        [PXUIEnabled(typeof(
            Where<enableFields, Equal<True>>))]
        public virtual bool? FTInitialLiquidationDatafixForOldData { get; set; }
        public abstract class fTInitialLiquidationDatafixForOldData : BqlBool.Field<fTInitialLiquidationDatafixForOldData> { }
        #endregion

        #region FT Expected Date Of Use Datafix for Old data
        [PXBool]
        [PXUIField(DisplayName = "FT Expected Date of Use (Null) Datafix for Old data")]
        [PXUnboundDefault(false, PersistingCheck = PXPersistingCheck.Nothing)]
        [PXUIEnabled(typeof(
            Where<enableFields, Equal<True>>))]
        public virtual bool? FTExpectedDateOfUseDatafixForOldData { get; set; }
        public abstract class fTExpectedDateOfUseDatafixForOldData : BqlBool.Field<fTExpectedDateOfUseDatafixForOldData> { }
        #endregion

        #region FT w/o Line description Datafix
        [PXBool]
        [PXUIField(DisplayName = "FT w/o Line description Datafix")]
        [PXUnboundDefault(false, PersistingCheck = PXPersistingCheck.Nothing)]
        [PXUIEnabled(typeof(
        Where<enableFields, Equal<True>>))]
        public virtual bool? FTwoLineDescriptionDatafix { get; set; }
        public abstract class fTwoLineDescriptionDatafix : BqlBool.Field<fTwoLineDescriptionDatafix> { }
        #endregion

        #region FundManagementPreferencePopulateGLAccountsSetup
        [PXBool]
        [PXUIField(DisplayName = "Fund Management Preference populate GL Accounts Setup")]
        [PXUnboundDefault(false, PersistingCheck = PXPersistingCheck.Nothing)]
        [PXUIEnabled(typeof(
        Where<enableFields, Equal<True>>))]
        public virtual bool? FundManagementPreferencePopulateGLAccountsSetup { get; set; }
        public abstract class fundManagementPreferencePopulateGLAccountsSetup : BqlBool.Field<fundManagementPreferencePopulateGLAccountsSetup> { }
        #endregion

        #endregion

        #region LEPSetup
        [PXBool]
        [PXUIField(DisplayName = "Entry points for CFM Transaction pages with Manual GI (CA, FT, Funds, Replenishment")]
        [PXUnboundDefault(false, PersistingCheck = PXPersistingCheck.Nothing)]
        [PXUIEnabled(typeof(
            Where<enableFields, Equal<True>>))]
        public virtual bool? LEPSetup { get; set; }
        public abstract class lEPSetup : BqlBool.Field<lEPSetup> { }
        #endregion

        #region Time&Expenses

        #region DisableAutomaticReleaseAPAndRequireApprovalOnRFPBill
        [PXBool]
        [PXUIField(DisplayName = "Disable Automatic Release AP And Require Approval On RFP Bill")]
        [PXUnboundDefault(false, PersistingCheck = PXPersistingCheck.Nothing)]
        [PXUIEnabled(typeof(
                    Where<enableFields, Equal<True>>))]
        public virtual bool? DisableAutomaticReleaseAPAndRequireApprovalOnRFPBill { get; set; }
        public abstract class disableAutomaticReleaseAPAndRequireApprovalOnRFPBill : BqlBool.Field<disableAutomaticReleaseAPAndRequireApprovalOnRFPBill> { }
        #endregion

        #endregion

        #region Expense Receipts
        #region ExpenseReceiptCD                
        [PXString(15, InputMask = ">CCCCCCCCCCCCCCC", IsUnicode = true)]
        [PXUnboundDefault(PersistingCheck = PXPersistingCheck.Nothing)]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.RefNbr)]
        [PXSelector(typeof(Search<
            EPExpenseClaimDetails.claimDetailCD, Where<EPExpenseClaimDetails.claimDetailCD, IsNotNull>>),
            typeof(claimDetailCD), ValidateValue = false)]
        public virtual string ClaimDetailCD { get; set; }
        public abstract class claimDetailCD : BqlString.Field<claimDetailCD> { }
        #endregion

        #region FundHistoryErNbr                
        [PXString(15, InputMask = ">CCCCCCCCCCCCCCC", IsUnicode = true)]
        [PXUnboundDefault(PersistingCheck = PXPersistingCheck.Nothing)]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.RefNbr)]
        [PXSelector(typeof(Search<
            EPExpenseClaimDetails.claimDetailCD, Where<EPExpenseClaimDetails.claimDetailCD, IsNotNull>>),
            typeof(claimDetailCD), ValidateValue = false)]
        public virtual string FundHistoryErNbr { get; set; }
        public abstract class fundHistoryErNbr : BqlString.Field<fundHistoryErNbr> { }
        #endregion
        #endregion

        #region NoteID  
        [PXNote]
        public virtual Guid? NoteID { get; set; }
        public abstract class noteID : PX.Data.BQL.BqlGuid.Field<noteID> { }
        #endregion
    }
}
