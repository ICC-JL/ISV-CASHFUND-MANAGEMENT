using CashFundManagement.Attributes;
using CashFundManagement.Messages;
using PX.Data;
using PX.Data.BQL;
using PX.Data.ReferentialIntegrity.Attributes;
using PX.Objects.CS;
using PX.Objects.GL;
using PX.Objects.IN;
using PX.Objects.PM;
using PX.Objects.CM;
using System;
//TODO : Remove dead codes on next upgrade.
namespace CashFundManagement.DAC {
    [Serializable]
    [PXCacheName("Fund Transaction History")]
    public class ATPTEFMFundTransactionHistoryView : Base.ATPTEFMAudit, IBqlTable
    {
        #region FundTransactionHistoryID
        [PXDBIdentity(IsKey = true)]
        public virtual int? FundTransactionHistoryID { get; set; }
        public abstract class fundTransactionHistoryID : BqlInt.Field<fundTransactionHistoryID> { }
        #endregion

        #region FundRefNbr
        [PXDBString(15, IsUnicode = true)]
        [PXDBDefault(typeof(ATPTEFMFund.fundCD))]
        [PXParent(typeof(Select<
            ATPTEFMFund,
            Where<ATPTEFMFund.fundCD, Equal<Current<ATPTEFMFundTransactionHistoryView.fundRefNbr>>>>))]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.FundTransactionRefNbr)]
        public virtual string FundRefNbr { get; set; }
        public abstract class fundRefNbr : BqlString.Field<fundRefNbr> { }
        #endregion

        #region TransactionType
        //TODO : Transfer attribute to another class.
        [PXDBString(1, IsFixed = true)]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.FundTransactionType)]
        [ATPTEFMFundTransactionTypeAttribute.ATPTEFMFundTransactionHistoryType]
        public virtual string TransactionType { get; set; }
        public abstract class transactionType : BqlString.Field<transactionType> { }
        #endregion

        #region ReferenceNbr
        [PXDBString(15, IsUnicode = true)]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.RefNbr)]
        public virtual string RefNbr { get; set; }
        public abstract class refNbr : BqlString.Field<refNbr> { }
        #endregion  

        #region Branch
        [Branch(DisplayName = Messages.ATPTEFMMessages.Branch)]
        public virtual int? FundBranchID { get; set; }
        public abstract class fundBranchID : BqlInt.Field<fundBranchID> { }
        #endregion

        #region Fund Type
        [PXDBString(1, IsFixed = true)]
        [ATPTEFMFundTypeAttribute.ATPTEFMFundType]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.FundType)]
        public virtual string FundType { get; set; }
        public abstract class fundType : BqlString.Field<fundType> { }
        #endregion

        #region Status
        [PXDBString(1, IsFixed = true)]
        [ATPTEFMFundStatusAttribute.ATPTEFMFundStatus()]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.Status)]
        public virtual string Status { get; set; }
        public abstract class status : BqlString.Field<status> { }
        #endregion

        #region OrderDate
        [PXDBDateAndTime]
        [PXUIField(DisplayName = "Order Date")]
        public virtual DateTime? OrderDate { get; set; }
        public abstract class orderDate : BqlDateTime.Field<orderDate> { }
        #endregion

        #region TransactionDate
        [PXDBDate]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.Date)]
        public virtual DateTime? TransactionDate { get; set; }
        public abstract class transactionDate : BqlDateTime.Field<transactionDate> { }
        #endregion

        #region CuryFundTransactionDocumentAmt
        [PXDBCurrency(typeof(ATPTEFMFund.curyInfoID), typeof(ATPTEFMFundTransactionHistoryView.fundTransactionDocumentAmt))]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.DocAmt)]
        [PXDefault(TypeCode.Decimal, "0.0", PersistingCheck = PXPersistingCheck.Nothing)]
        //[PXFormula(null, typeof(SumCalc<ATPTEFMFund.balanceAmt>))]
        public virtual decimal? CuryFundTransactionDocumentAmt { get; set; }
        public abstract class curyFundTransactionDocumentAmt : BqlDecimal.Field<curyFundTransactionDocumentAmt> { }
        #endregion

        #region FundTransactionDocumentAmt
        [PXDBBaseCury()]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.DocAmt)]
        [PXDefault(TypeCode.Decimal, "0.0", PersistingCheck = PXPersistingCheck.Nothing)]
        //[PXFormula(null, typeof(SumCalc<ATPTEFMFund.balanceAmt>))]
        public virtual decimal? FundTransactionDocumentAmt { get; set; }
        public abstract class fundTransactionDocumentAmt : BqlDecimal.Field<fundTransactionDocumentAmt> { }
        #endregion

        #region CuryWithholdingTax
        [PXDBCurrency(typeof(ATPTEFMFund.curyInfoID), typeof(ATPTEFMFundTransactionHistoryView.withholdingTax))]
        [PXDefault(TypeCode.Decimal, "0.00", PersistingCheck = PXPersistingCheck.Nothing)]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.WithholdingTax)]
        public virtual decimal? CuryWithholdingTax { get; set; }
        public abstract class curyWithholdingTax : BqlDecimal.Field<curyWithholdingTax> { }
        #endregion

        #region WithholdingTax
        [PXDBBaseCury()]
        [PXDefault(TypeCode.Decimal, "0.00", PersistingCheck = PXPersistingCheck.Nothing)]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.WithholdingTax)]
        public virtual decimal? WithholdingTax { get; set; }
        public abstract class withholdingTax : BqlDecimal.Field<withholdingTax> { }
        #endregion

        #region CuryUnliquidatedAmt
        [PXDBCurrency(typeof(ATPTEFMFund.curyInfoID), typeof(ATPTEFMFundTransactionHistoryView.unliquidatedAmt))]
        [PXDefault(TypeCode.Decimal, "0.00", PersistingCheck = PXPersistingCheck.Nothing)]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.Unliquidated)]
        public virtual decimal? CuryUnliquidatedAmt { get; set; }
        public abstract class curyUnliquidatedAmt : BqlDecimal.Field<curyUnliquidatedAmt> { }
        #endregion

        #region UnliquidatedAmt
        [PXDBBaseCury()]
        [PXDefault(TypeCode.Decimal, "0.00", PersistingCheck = PXPersistingCheck.Nothing)]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.Unliquidated)]
        public virtual decimal? UnliquidatedAmt { get; set; }
        public abstract class unliquidatedAmt : BqlDecimal.Field<unliquidatedAmt> { }
        #endregion

        #region CuryLiquidatedAmt
        [PXDBCurrency(typeof(ATPTEFMFund.curyInfoID), typeof(ATPTEFMFundTransactionHistoryView.liquidatedAmt))]
        [PXDefault(TypeCode.Decimal, "0.00", PersistingCheck = PXPersistingCheck.Nothing)]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.Liquidated)]
        public virtual decimal? CuryLiquidatedAmt { get; set; }
        public abstract class curyLiquidatedAmt : BqlDecimal.Field<curyLiquidatedAmt> { }
        #endregion

        #region LiquidatedAmt
        [PXDBBaseCury()]
        [PXDefault(TypeCode.Decimal, "0.00", PersistingCheck = PXPersistingCheck.Nothing)]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.Liquidated)]
        public virtual decimal? LiquidatedAmt { get; set; }
        public abstract class liquidatedAmt : BqlDecimal.Field<liquidatedAmt> { }
        #endregion

        #region CuryFundReturnAmt
        [PXDBCurrency(typeof(ATPTEFMFund.curyInfoID), typeof(ATPTEFMFundTransactionHistoryView.fundReturnAmt))]
        [PXDefault(TypeCode.Decimal, "0.00", PersistingCheck = PXPersistingCheck.Nothing)]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.ActualReturn)]
        public virtual decimal? CuryFundReturnAmt { get; set; }
        public abstract class curyFundReturnAmt : BqlDecimal.Field<curyFundReturnAmt> { }
        #endregion

        #region FundReturnAmt
        [PXDBBaseCury()]
        [PXDefault(TypeCode.Decimal, "0.00", PersistingCheck = PXPersistingCheck.Nothing)]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.ActualReturn)]
        public virtual decimal? FundReturnAmt { get; set; }
        public abstract class fundReturnAmt : BqlDecimal.Field<fundReturnAmt> { }
        #endregion

        #region CuryBalanceAmt
        [PXDBCurrency(typeof(ATPTEFMFund.curyInfoID), typeof(ATPTEFMFundTransactionHistoryView.balanceAmt))]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.Balance)]
        [PXDefault(TypeCode.Decimal, "0.0", PersistingCheck = PXPersistingCheck.Nothing)]
        public virtual decimal? CuryBalanceAmt { get; set; }
        public abstract class curyBalanceAmt : BqlDecimal.Field<curyBalanceAmt> { }
        #endregion

        #region BalanceAmt
        [PXDBBaseCury()]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.Balance)]
        [PXDefault(TypeCode.Decimal, "0.0", PersistingCheck = PXPersistingCheck.Nothing)]
        public virtual decimal? BalanceAmt { get; set; }
        public abstract class balanceAmt : BqlDecimal.Field<balanceAmt> { }
        #endregion

        #region CheckNbr
        [PXDBString(15, IsUnicode = true)]
        [PXDefault(PersistingCheck = PXPersistingCheck.Nothing)]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.UsrRLCheckNbr)]
        public virtual string CheckNbr { get; set; }
        public abstract class checkNbr : BqlString.Field<checkNbr> { }
        #endregion

        #region CuryCheckAmt
        [PXDBCurrency(typeof(ATPTEFMFund.curyInfoID), typeof(ATPTEFMFundTransactionHistoryView.checkAmt))]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.CheckAmount)]
        [PXDefault(TypeCode.Decimal, "0.0", PersistingCheck = PXPersistingCheck.Nothing)]
        public virtual decimal? CuryCheckAmt { get; set; }
        public abstract class curyCheckAmt : BqlDecimal.Field<curyCheckAmt> { }
        #endregion 

        #region CheckAmt
        [PXDBBaseCury()]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.CheckAmount)]
        [PXDefault(TypeCode.Decimal, "0.0", PersistingCheck = PXPersistingCheck.Nothing)]
        public virtual decimal? CheckAmt { get; set; }
        public abstract class checkAmt : BqlDecimal.Field<checkAmt> { }
        #endregion 

        #region ReversingJournalBatchNbr
        [PXDBString(15, IsUnicode = true)]
        [PXUIField(DisplayName = ATPTEFMMessages.ReversingBatchNbr, Enabled = false)]
        public virtual string ReversingJournalBatchNbr { get; set; }
        public abstract class reversingJournalBatchNbr : BqlString.Field<reversingJournalBatchNbr> { }
        #endregion

        #region ReplenishmentRefNbr
        [PXDBString(15, IsUnicode = true)]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.ReplenishmentRefNbrDetail)]
        public virtual string ReplenishmentRefNbr { get; set; }
        public abstract class replenishmentRefNbr : BqlString.Field<replenishmentRefNbr> { }
        #endregion

        #region ProjectID
        [PXForeignReference(typeof(Field<projectID>.IsRelatedTo<PMProject.contractID>))]
        [ActiveProjectOrContractBase(DisplayName = Messages.ATPTEFMMessages.ProjectID)]
        [PXDefault(PersistingCheck = PXPersistingCheck.Nothing)]
        [PXUIVisible(typeof(FeatureInstalled<FeaturesSet.projectAccounting>))]
        public virtual int? ProjectID { get; set; }
        public abstract class projectID : PX.Data.IBqlField { }
        #endregion

        #region ProjectTaskID
        [PXForeignReference(typeof(Field<projectTaskID>.IsRelatedTo<PMTask.taskID>))]
        [PXDBInt]
        [PXDefault(PersistingCheck = PXPersistingCheck.Nothing)]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.ProjectTaskid)]
        [PXSelector(typeof(Search<
            PMTask.taskID,
            Where<PMTask.projectID, Equal<Current<projectID>>>>), typeof(PMTask.taskCD), typeof(PMTask.description), DescriptionField = typeof(PMTask.description), SubstituteKey = typeof(PMTask.taskCD))]
        [PXUIVisible(typeof(FeatureInstalled<FeaturesSet.projectAccounting>))]
        public virtual int? ProjectTaskID { get; set; }
        public abstract class projectTaskID : PX.Data.IBqlField { }
        #endregion

        #region NoteID
        public abstract class noteID : PX.Data.BQL.BqlGuid.Field<noteID> { }
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

        #region Source
        [PXDBString(1, IsFixed = true)]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.Source)]
        [source.ATPTEFMList()]
        public virtual string Source { get; set; }
        public abstract class source : BqlString.Field<source>
        {
            public const string FundTransaction = "F";
            public const string Replenishment = "R";

            public class ATPTEFMListAttribute : PXStringListAttribute
            {
                public ATPTEFMListAttribute()
                    : base(new[] { Pair(FundTransaction, Messages.ATPTEFMMessages.ATPTEFMFundTransaction),
                                      Pair(Replenishment, Messages.ATPTEFMMessages.Replenishment) })
                { }
            }

            public class fundTransaction : BqlString.Constant<fundTransaction>
            {
                public fundTransaction() : base(FundTransaction) { }
            }

            public class replenishment : BqlString.Constant<replenishment>
            {
                public replenishment() : base(Replenishment) { }
            }
        }
        #endregion

        #region CashAdvanceStatus
        [PXDBString(1, IsFixed = true)]
        [ATPTEFMFundTransactionCashAdvanceStatusAttribute.ATPTEFMFundTrandactionCashAdvanceStatus]
        public virtual string CashAdvanceStatus { get; set; }
        public abstract class cashAdvanceStatus : BqlString.Field<cashAdvanceStatus> { }
        #endregion

        #region IsUnliquidatedRequest
        [PXDBBool]
        [PXDefault(false, PersistingCheck = PXPersistingCheck.Nothing)]
        public virtual bool? IsUnliquidatedRequest { get; set; }
        public abstract class isUnliquidatedRequest : BqlBool.Field<isUnliquidatedRequest> { }
        #endregion

        #region CuryFundAmt
        [PXDBCurrency(typeof(ATPTEFMFund.curyInfoID), typeof(ATPTEFMFundTransactionHistoryView.fundAmt))]
        [PXDefault(TypeCode.Decimal, "0.00", PersistingCheck = PXPersistingCheck.Nothing)]
        public virtual decimal? CuryFundAmt { get; set; }
        public abstract class curyFundAmt : BqlDecimal.Field<curyFundAmt> { }
        #endregion

        #region FundAmt
        [PXDBBaseCury()]
        [PXDefault(TypeCode.Decimal, "0.00", PersistingCheck = PXPersistingCheck.Nothing)]
        public virtual decimal? FundAmt { get; set; }
        public abstract class fundAmt : BqlDecimal.Field<fundAmt> { }
        #endregion

        #region IsReimbursement
        [PXDBBool]
        [PXUIField(DisplayName = "Is Reimbursement")]
        [PXDefault(false, PersistingCheck = PXPersistingCheck.Nothing)]
        public virtual bool? IsReimbursement { get; set; }
        public abstract class isReimbursement : BqlBool.Field<isReimbursement> { }
        #endregion

        #region CostCodeID
        [PXForeignReference(typeof(Field<costCodeID>.IsRelatedTo<PMCostCode.costCodeID>))]
        [CostCode(typeof(InventoryItem.cOGSAcctID), typeof(projectTaskID), AccountType.Expense, DisplayName = "Cost Code")]
        [PXUIVisible(typeof(FeatureInstalled<FeaturesSet.costCodes>))]
        public virtual Int32? CostCodeID { get; set; }
        public abstract class costCodeID : PX.Data.BQL.BqlInt.Field<costCodeID> { }
        #endregion

        #region HasReplenishmentCheckNbr
        [PXDBBool]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.ATPTEEFMReplinesmentDetail)]
        [PXDefault(false, PersistingCheck = PXPersistingCheck.Nothing)]
        public virtual bool? HasReplenishemtCheckNbr { get; set; }
        public abstract class hasReplenishemtCheckNbr : BqlBool.Field<hasReplenishemtCheckNbr> { }
        #endregion

        #region CuryDocumentBalanceAmt
        [PXDBCurrency(typeof(ATPTEFMFund.curyInfoID), typeof(ATPTEFMFundTransactionHistoryView.documentBalanceAmt))]
        [PXDefault(TypeCode.Decimal, "0.0", PersistingCheck = PXPersistingCheck.Nothing)]
        public virtual decimal? CuryDocumentBalanceAmt { get; set; }
        public abstract class curyDocumentBalanceAmt : BqlDecimal.Field<curyDocumentBalanceAmt> { }
        #endregion

        #region DocumentBalanceAmt
        [PXDBBaseCury()]
        [PXDefault(TypeCode.Decimal, "0.0", PersistingCheck = PXPersistingCheck.Nothing)]
        public virtual decimal? DocumentBalanceAmt { get; set; }
        public abstract class documentBalanceAmt : BqlDecimal.Field<documentBalanceAmt> { }
        #endregion

        #region SortNbr
        [PXDBString(-1, IsUnicode = true)]
        [PXDefault(PersistingCheck = PXPersistingCheck.Nothing)]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.UsrRLCheckNbr)]
        public virtual string SortNbr { get; set; }
        public abstract class sortNbr : BqlString.Field<sortNbr> { }
        #endregion

        #region FundTransactionSortNbr
        [PXDBString(100, IsUnicode = true)]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.FundTransactionNbr)]
        public virtual string FundTransactionSortNbr { get; set; }
        public abstract class fundTransactionSortNbr : BqlString.Field<fundTransactionSortNbr> { }
        #endregion
    }
}
