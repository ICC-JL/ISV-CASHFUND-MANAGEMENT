using CashFundManagement.Attributes;
using CashFundManagement.Extensions.Attribute;
using CashFundManagement.Messages;
using PX.Data;
using PX.Data.BQL;
using PX.Objects.CS;
using PX.Objects.EP;
using PX.Objects.GL;
using PX.Objects.IN;
using PX.Objects.RQ;
using System;

namespace CashFundManagement.DAC.Setup {
    /// <remarks>
    /// 010267 - (CFM2024R1/2024R2) Fund Management Preferences>Other Settings: Additional fields for 'Use Expense Account From:' and Combine Expense Sub From:
    /// </remarks>
    [Serializable]
    [PXCacheName(ATPTEFMMessages.ATPTEFMSetup)]
    public class ATPTEFMSetup : Base.ATPTEFMAudit, IBqlTable
    {
        #region NoteID
        [PXNote]
        public virtual Guid? NoteID { get; set; }
        public abstract class noteID : BqlGuid.Field<noteID> { }
        #endregion

        #region ApprovalModule
        [PXString]
        [ATPTEFMApprovalModule]
        [PXUnboundDefault(ATPTEFMApprovalModuleAttribute.Funds)]
        [PXUIField(DisplayName = ATPTEFMMessages.Module)]
        public virtual string ApprovalModule { get; set; }
        public abstract class approvalModule : BqlString.Field<approvalModule> { }
        #endregion

        #region CashadvanceRequestApproval
        [EPRequireApproval]
        [PXDefault(false, PersistingCheck = PXPersistingCheck.Nothing)]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.CashadvanceRequestApproval)]
        public virtual bool? CashAdvanceRequestApproval { get; set; }
        public abstract class cashAdvanceRequestApproval : BqlBool.Field<cashAdvanceRequestApproval> { }
        #endregion

        #region FundTransactionRequestApproval
        [EPRequireApproval]
        [PXDefault(false, PersistingCheck = PXPersistingCheck.Nothing)]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.FundTransactionRequestApproval)]
        public virtual bool? FundTransactionRequestApproval { get; set; }
        public abstract class fundTransactionRequestApproval : BqlBool.Field<fundTransactionRequestApproval> { }
        #endregion

        #region ReplenishmentRequestApproval
        [EPRequireApproval]
        [PXDefault(false, PersistingCheck = PXPersistingCheck.Nothing)]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.ReplenishmentRequestApproval)]
        public virtual bool? ReplenishmentRequestApproval { get; set; }
        public abstract class replenishmentRequestApproval : BqlBool.Field<replenishmentRequestApproval> { }
        #endregion

        #region Require Vendor Details on Receipts Tab and Expense Receipt
        [PXDBBool]
        [PXUIField(DisplayName = ATPTEFMMessages.RequireVendorDetails)]
        [PXDefault(false, PersistingCheck = PXPersistingCheck.Nothing)]
        public virtual bool? RequireVendorDetails { get; set; }
        public abstract class requireVendorDetails : PX.Data.BQL.BqlBool.Field<requireVendorDetails> { }
        #endregion

        #region Require External Reference Nbr. 
        [PXDBBool]
        [PXUIField(DisplayName = ATPTEFMMessages.RequireExternalReferenceNbr)]
        [PXDefault(false, PersistingCheck = PXPersistingCheck.Nothing)]
        public virtual bool? RequireExternalReferenceNbr { get; set; }
        public abstract class requireExternalReferenceNbr : PX.Data.BQL.BqlBool.Field<requireExternalReferenceNbr> { }
        #endregion

        #region AllowManualReceipts
        [PXDBBool]
        [PXUIField(DisplayName = ATPTEFMMessages.AllowManualReceipts)]
        [PXDefault(false, PersistingCheck = PXPersistingCheck.Nothing)]
        public virtual bool? AllowManualReceipts { get; set; }
        public abstract class allowManualReceipts : PX.Data.BQL.BqlBool.Field<allowManualReceipts> { }
        #endregion

        #region FundsApprovalSetup
        [EPRequireApproval]
        [PXDefault(false, PersistingCheck = PXPersistingCheck.Nothing)]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.FundsApprovalSetup)]
        public virtual bool? FundsApprovalSetup { get; set; }
        public abstract class fundsApprovalSetup : BqlBool.Field<fundsApprovalSetup> { }
        #endregion

        #region MonthEndRequestApproval
        [EPRequireApproval]
        [PXDefault(false, PersistingCheck = PXPersistingCheck.Nothing)]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.MonthEndRequestApproval)]
        public virtual bool? MonthEndRequestApproval { get; set; }
        public abstract class monthEndRequestApproval : BqlBool.Field<monthEndRequestApproval> { }
        #endregion

        #region MaterialRequestRequestApproval
        [EPRequireApproval]
        [PXDefault(false, PersistingCheck = PXPersistingCheck.Nothing)]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.MaterialRequestRequestApproval)]
        public virtual bool? MaterialRequestRequestApproval { get; set; }
        public abstract class materialRequestRequestApproval : BqlBool.Field<materialRequestRequestApproval> { }
        #endregion

        #region AllowMultipleCA
        [PXDBBool]
        [PXDefault(false)]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.AllowMultipleCA)]
        public virtual bool? AllowMultipleCA { get; set; }
        public abstract class allowMultipleCA : BqlBool.Field<allowMultipleCA> { }
        #endregion

        #region NoDaysToLiquidate
        [PXDBInt]
        [PXDefault(TypeCode.Int32, "0")]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.NoDaysToLiquidate)]
        public virtual int? NoDaysToLiquidate { get; set; }
        public abstract class noDaysToLiquidate : BqlInt.Field<noDaysToLiquidate> { }
        #endregion

        #region NoDaysToLiquidateFund
        [PXDBInt]
        [PXDefault(TypeCode.Int32, "0", PersistingCheck = PXPersistingCheck.Nothing)]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.NoDaysToLiquidateFund, Visible = false)]
        public virtual int? NoDaysToLiquidateFund { get; set; }
        public abstract class noDaysToLiquidateFund : BqlInt.Field<noDaysToLiquidate> { }
        #endregion

        #region ReClassAccoundIDFund
        //[PXSelector(
        //    typeof(Search<Account.accountID>),
        //    typeof(Account.accountCD),
        //    typeof(Account.description),
        //    SubstituteKey = typeof(Account.accountCD))]
        [Account(DisplayName = Messages.ATPTEFMMessages.ReClassAccountIDFund, Visible = false)]
        [PXDefault(PersistingCheck = PXPersistingCheck.Nothing)]
        //[PXUIField(Visible = false)]
        public int? ReClassAccoundIDFund { get; set; }
        public abstract class reClassAccoundIDFund : BqlInt.Field<reClassAccoundIDFund> { }
        #endregion

        #region ReClassSubIDFund 
        [SubAccount(typeof(reClassAccoundIDFund), DisplayName = Messages.ATPTEFMMessages.ReClassSubIDFund, Visible = false)]
        [PXDefault(PersistingCheck = PXPersistingCheck.Nothing)]
        //[PXUIField(Visible = false)]
        public int? ReClassSubIDFund { get; set; }
        public abstract class reClassSubIDFund : BqlInt.Field<reClassSubIDFund> { }
        #endregion

        #region Replenishment Limit
        [PXDBString(1, IsFixed = true)]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.ReplenishmentLimit)]
        [ATPTEFMReplenishmentStringListAttribute.ATPTEFMReplenishmentLimitList()]
        public virtual string ReplenishmentLimit { get; set; }
        public abstract class replenishmentLimit : BqlString.Field<replenishmentLimit> { }
        #endregion

        #region FundTransactionLimit
        [PXDBString(1, IsFixed = true)]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.FundTransactionLimit)]
        [ATPTEFMReplenishmentStringListAttribute.ATPTEFMFundTransactionLimitList()]
        public virtual string FundTransactionLimit { get; set; }
        public abstract class fundTransactionLimit : BqlString.Field<fundTransactionLimit> { }
        #endregion

        #region PCF Account
        //[PXSelector(
        //   typeof(Search<Account.accountID>),
        //   typeof(Account.accountCD),
        //   typeof(Account.description),
        //   SubstituteKey = typeof(Account.accountCD))]
        [Account(DisplayName = Messages.ATPTEFMMessages.PCFAccount)]
        [PXDefault(PersistingCheck = PXPersistingCheck.Nothing)]
        public int? PCFAccount { get; set; }
        public abstract class pCFAccount : IBqlField { }
        #endregion

        #region PCF Subaccount
        [SubAccount(typeof(pCFAccount), DisplayName = Messages.ATPTEFMMessages.PCFSubaccount)]
        [PXDefault(PersistingCheck = PXPersistingCheck.Nothing)]
        public int? PCFSubaccount { get; set; }
        public abstract class pCFSubaccount : BqlInt.Field<pCFSubaccount> { }
        #endregion

        #region RVF Account
        //[PXSelector(
        //    typeof(Search<Account.accountID>),
        //    typeof(Account.accountCD),
        //    typeof(Account.description),
        //    SubstituteKey = typeof(Account.accountCD))]
        [Account(DisplayName = Messages.ATPTEFMMessages.RVFAccount)]
        [PXDefault(PersistingCheck = PXPersistingCheck.Nothing)]
        public int? RVFAccount { get; set; }
        public abstract class rVFAccount : BqlInt.Field<rVFAccount> { }
        #endregion

        #region RVF Subaccount
        [SubAccount(typeof(rVFAccount), DisplayName = Messages.ATPTEFMMessages.RVFSubaccount)]
        [PXDefault(PersistingCheck = PXPersistingCheck.Nothing)]
        public int? RVFSubaccount { get; set; }
        public abstract class rVFSubaccount : BqlInt.Field<rVFSubaccount> { }
        #endregion

        #region Clearing Account
        //[PXSelector(
        //    typeof(Search<Account.accountID>),
        //    typeof(Account.accountCD),
        //    typeof(Account.description),
        //    SubstituteKey = typeof(Account.accountCD))]
        [Account(DisplayName = Messages.ATPTEFMMessages.ClearingAccount)]
        [PXDefault(PersistingCheck = PXPersistingCheck.Nothing)]
        public int? ClearingAccount { get; set; }
        public abstract class clearingAccount : BqlInt.Field<clearingAccount> { }
        #endregion

        #region Clearing Subacccount
        [SubAccount(typeof(clearingAccount), DisplayName = Messages.ATPTEFMMessages.ClearingSubaccount)]
        [PXDefault(PersistingCheck = PXPersistingCheck.Nothing)]
        public int? ClearingSubaccount { get; set; }
        public abstract class clearingSubaccount : BqlInt.Field<clearingAccount> { }
        #endregion

        #region Month-end Transaction Account
        //[PXSelector(
        //    typeof(Search<Account.accountID>),
        //    typeof(Account.accountCD),
        //    typeof(Account.description),
        //    SubstituteKey = typeof(Account.accountCD))]
        [Account(DisplayName = Messages.ATPTEFMMessages.MonthendTransactionAccount)]
        [PXDefault(PersistingCheck = PXPersistingCheck.Nothing)]
        public int? MonthEndTransactionAccount { get; set; }
        public abstract class monthEndTransactionAccount : BqlInt.Field<monthEndTransactionAccount> { }
        #endregion

        #region  Month-end Transaction SubAccount
        [SubAccount(typeof(monthEndTransactionAccount), DisplayName = Messages.ATPTEFMMessages.MonthendTransactionSubaccount)]
        [PXDefault(PersistingCheck = PXPersistingCheck.Nothing)]
        public int? MonthEndTransactionSubaccount { get; set; }
        public abstract class monthEndTransactionSubaccount : BqlInt.Field<monthEndTransactionSubaccount> { }
        #endregion

        #region Fund Type
        [PXDBString(255, IsUnicode = true)]
        [ATPTEFMFundTypeAttribute.ATPTEFMFundType]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.FundType, Visible = false)]
        public virtual string FundType { get; set; }
        public abstract class fundType : BqlString.Field<fundType> { }
        #endregion

        #region IsFilterByEmployeeDelegates
        [PXDBBool]
        [PXDefault(false, PersistingCheck = PXPersistingCheck.Nothing)]
        [PXUIField(DisplayName = ATPTEFMMessages.FilteredView)]
        public virtual bool? IsFilterByEmployeeDelegates { get; set; }
        public abstract class isFilterByEmployeeDelegates : PX.Data.BQL.BqlBool.Field<isFilterByEmployeeDelegates> { }
        #endregion

        #region IsRequireApprovalReplenishment
        [PXDBBool]
        [PXDefault(false, PersistingCheck = PXPersistingCheck.Nothing)]
        [PXUIField(DisplayName = ATPTEFMMessages.RequireApprovalOnReplenishmentBill)]
        public virtual bool? IsRequireApprovalReplenishment { get; set; }
        public abstract class isRequireApprovalReplenishment : BqlBool.Field<isRequireApprovalReplenishment> { }
        #endregion

        #region IsRequireApprovalOnFundEstablishment
        [PXDBBool]
        [PXDefault(false, PersistingCheck = PXPersistingCheck.Nothing)]
        [PXUIField(DisplayName = ATPTEFMMessages.RequireApprovalOnFundEstablishment)]
        public virtual bool? IsRequireApprovalOnFundEstablishment { get; set; }
        public abstract class isRequireApprovalOnFundEstablishment : BqlBool.Field<isRequireApprovalOnFundEstablishment> { }
        #endregion

        #region IsFundsMigration
        [PXDBBool]
        [PXDefault(false, PersistingCheck = PXPersistingCheck.Nothing)]
        [PXUIField(DisplayName = ATPTEFMMessages.ActivateFunds)]
        public virtual bool? IsFundsMigration { get; set; }
        public abstract class isFundsMigration : BqlBool.Field<isFundsMigration> { }
        #endregion

        #region FundTransactionImportMode
        [PXDBBool]
        [PXDefault(false, PersistingCheck = PXPersistingCheck.Nothing)]
        [PXUIField(DisplayName = ATPTEFMMessages.FundTransactionImportMode)]
        public virtual bool? FundTransactionImportMode { get; set; }
        public abstract class fundTransactionImportMode : BqlBool.Field<fundTransactionImportMode> { }
        #endregion

        #region Fund Numbering Sequence
        public abstract class fundNumberingID : BqlString.Field<fundNumberingID> { }
        [PXDBString(15, IsUnicode = true)]
        [PXSelector(typeof(Numbering.numberingID), DescriptionField = typeof(Numbering.descr))]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.FundNumberingID, Visibility = PXUIVisibility.Visible, Required = true)]
        public virtual string FundNumberingID { get; set; }
        #endregion

        #region MonthEndNumberingID
        public abstract class monthEndNumberingID : BqlString.Field<monthEndNumberingID> { }
        [PXDBString(15, IsUnicode = true)]
        [PXSelector(typeof(Numbering.numberingID), DescriptionField = typeof(Numbering.descr))]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.UsrRLMonthEndNumberingID, Visibility = PXUIVisibility.Visible, Required = true)]
        public virtual string MonthEndNumberingID { get; set; }
        #endregion

        #region AutoReleaseMonthEndJournal
        [PXDBBool]
        [PXDefault(false, PersistingCheck = PXPersistingCheck.Nothing)]
        [PXUIField(DisplayName = ATPTEFMMessages.AutomaticallyReleaseMonthEndJournalTransaction)]
        public virtual bool? AutoReleaseMonthEndJournal { get; set; }
        public abstract class autoReleaseMonthEndJournal : PX.Data.BQL.BqlBool.Field<autoReleaseMonthEndJournal> { }
        #endregion

        #region ValidateAmountReceivedAndReleasedUponLiquidation
        [PXDBBool]
        [PXUIField(DisplayName = ATPTEFMMessages.ValidateAmountReceivedAndReleasedUponLiquidation)]
        public virtual bool? ValidateAmountReceivedAndReleasedUponLiquidation { get; set; }
        public abstract class validateAmountReceivedAndReleasedUponLiquidation : PX.Data.BQL.BqlBool.Field<validateAmountReceivedAndReleasedUponLiquidation> { }
        #endregion

        #region RequireApprovalOnFundIncreaseCredAdj
        [PXDBBool]
        [PXDefault(false, PersistingCheck = PXPersistingCheck.Nothing)]
        [PXUIField(DisplayName = ATPTEFMMessages.RequireApprovalOnFundIncreaseCredAdj)]
        public virtual bool? RequireApprovalOnFundIncreaseCredAdj { get; set; }
        public abstract class requireApprovalOnFundIncreaseCredAdj : PX.Data.BQL.BqlBool.Field<requireApprovalOnFundIncreaseCredAdj> { }
        #endregion

        #region RequireApprovalOnFundDecreaseDebAdj
        [PXDBBool]
        [PXDefault(false, PersistingCheck = PXPersistingCheck.Nothing)]
        [PXUIField(DisplayName = ATPTEFMMessages.RequireApprovalOnFundDecreaseDebAdj)]
        public virtual bool? RequireApprovalOnFundDecreaseDebAdj { get; set; }
        public abstract class requireApprovalOnFundDecreaseDebAdj : PX.Data.BQL.BqlBool.Field<requireApprovalOnFundDecreaseDebAdj> { }
        #endregion

        #region FundRequestReclassicationSetup

        #region NoOfDaysToLiquidate
        [PXDBInt]
        [PXUIField(DisplayName = ATPTEFMMessages.NoOfDaysToLiquidate)]
        public virtual int? NoOfDaysToLiquidate { get; set; }
        public abstract class noOfDaysToLiquidate : PX.Data.BQL.BqlInt.Field<noOfDaysToLiquidate> { }
        #endregion

        #region ReclassificationItem
        [PXDBInt]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.ReclassificationItem)]
        [PXSelector(typeof(Search<InventoryItem.inventoryID, Where<InventoryItem.itemType, Equal<INItemTypes.expenseItem>, And<InventoryItem.itemStatus, Equal<InventoryItemStatus.active>>>>),
            typeof(InventoryItem.inventoryID),
            typeof(InventoryItem.inventoryCD),
            typeof(InventoryItem.descr),
            typeof(InventoryItem.taxCategoryID),
            SubstituteKey = typeof(InventoryItem.inventoryCD),
            DescriptionField = typeof(InventoryItem.descr))]
        public virtual int? ReclassificationItem { get; set; }
        public abstract class reclassificationItem : PX.Data.BQL.BqlInt.Field<reclassificationItem> { }
        #endregion

        #region LiquidationDateBasedOnWorkCalendar
        [PXDBBool]
        [PXDefault(false, PersistingCheck = PXPersistingCheck.Nothing)]
        [PXUIField(DisplayName = ATPTEFMMessages.LiquidationDateBasedOnWorkCalendar)]
        public virtual bool? LiquidationDateBasedOnWorkCalendar { get; set; }
        public abstract class liquidationDateBasedOnWorkCalendar : BqlBool.Field<liquidationDateBasedOnWorkCalendar> { }
        #endregion

        #endregion

        #region RestrictCustodianByBranch
        [PXDBBool]
        [PXDefault(false, PersistingCheck = PXPersistingCheck.Nothing)]
        [PXUIField(DisplayName = ATPTEFMMessages.RestrictCustodianByBranch)]
        public virtual bool? RestrictCustodianByBranch { get; set; }
        public abstract class restrictCustodianByBranch : BqlBool.Field<restrictCustodianByBranch> { }
        #endregion

#if Version24R1
        #region EnableFundTransactionLimit
        [PXDBBool]
        [PXUIField(DisplayName = ATPTEFMMessages.EnableFundTransactionLimit, Visibility = PXUIVisibility.Invisible)]
        [PXUIVisible(typeof(False))]
        [PXUIEnabled(typeof(False))]
        public virtual bool? EnableFundTransactionLimit { get; set; }
        public abstract class enableFundTransactionLimit : PX.Data.BQL.BqlBool.Field<enableFundTransactionLimit> { }
        #endregion
#else
        #region EnableFundTransactionLimit
        [PXDBBool]
        [PXUIField(DisplayName = ATPTEFMMessages.EnableFundTransactionLimit)]
        public virtual bool? EnableFundTransactionLimit { get; set; }
        public abstract class enableFundTransactionLimit : PX.Data.BQL.BqlBool.Field<enableFundTransactionLimit> { }
        #endregion
#endif

        #region SetNonStockItemDescriptionAsDefault
        [PXDBBool]
        [PXUIField(DisplayName = "Set Non-Stock Item Description as default Line Description")]
        public virtual bool? SetNonStockItemDescriptionAsDefault { get; set; }
        public abstract class setNonStockItemDescriptionAsDefault : PX.Data.BQL.BqlBool.Field<setNonStockItemDescriptionAsDefault> { }
        #endregion

        #region Fund Tarnsaction Numbering Sequence
        public abstract class fundTransactionNumberingID : BqlString.Field<fundTransactionNumberingID> { }
        [PXDBString(15, IsUnicode = true)]
        [PXSelector(typeof(Numbering.numberingID), DescriptionField = typeof(Numbering.descr))]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.FundTransactionNumberingID, Visibility = PXUIVisibility.Visible, Required = true)]
        public virtual string FundTransactionNumberingID { get; set; }
        #endregion

        #region ReplenishmentNumberingID
        [PXDBString(15, IsUnicode = true)]
        [PXDBDefault()]
        [PXSelector(typeof(Numbering.numberingID), DescriptionField = typeof(Numbering.descr))]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.ReplenishmentNumberingID, Visibility = PXUIVisibility.Visible, Required = true)]
        public string ReplenishmentNumberingID { get; set; }
        public abstract class replenishmentNumberingID : BqlString.Field<replenishmentNumberingID> { }
        #endregion

        #region Internal Attribute
        public static class FundTypeList
        {
            public const string PettyCash = "Petty Cash";
            public class pettyCash : BqlString.Constant<pettyCash>
            {
                public pettyCash()
                    : base(PettyCash)
                {
                }
            }

            public const string RevolvingFund = "Revolving Fund";
            public class revolvingFund : BqlString.Constant<revolvingFund>
            {
                public revolvingFund()
                    : base(RevolvingFund)
                {
                }
            }

        }
        #endregion

        #region UseExpenseAcctFrom
        [PXDBString(1, IsFixed = true)]
        [PXDefault(ATPTEFMFTAccountSource.PurchaseItem)]
        [PXUIField(DisplayName = "Use Expense Account From")]
        [ATPTEFMFTAccountSource.ATPTEFMList]
        public string UseExpenseAcctFrom { get; set; }
        public abstract class useExpenseAcctFrom : BqlString.Field<useExpenseAcctFrom> { }
        #endregion

        #region CombineExpSub
        [PXDefault(PersistingCheck = PXPersistingCheck.Nothing)]
        [ATPTEFMSubAccountMaskExtension(DisplayName = "Combine Expense Sub. From")]
        public string CombineExpSub { get; set; }
        public abstract class combineExpSub : BqlString.Field<combineExpSub> { }
        #endregion
    }
    /// <remarks>
    /// List Attribute without the Request Class option
    /// </remarks>
    public static class ATPTEFMFTAccountSource
    {
        public class ATPTEFMListAttribute : PXStringListAttribute
        {
            public ATPTEFMListAttribute() : base(
                new[]
                {
                    Pair(None, PX.Objects.RQ.Messages.None),
                    Pair(Requester, PX.Objects.RQ.Messages.Requester),
                    Pair(Department, PX.Objects.RQ.Messages.Department),
                    Pair(PurchaseItem, PX.Objects.RQ.Messages.PurchaseItem),
                })
            { }
        }

        public class department : PX.Data.BQL.BqlString.Constant<department>
        {
            public department() : base(Department) { }
        }

        public class purchaseItem : PX.Data.BQL.BqlString.Constant<purchaseItem>
        {
            public purchaseItem() : base(PurchaseItem) { }
        }

        public const string None = "N";
        public const string Department = "D";
        public const string Requester = "R";
        public const string PurchaseItem = "I";
    }
}

