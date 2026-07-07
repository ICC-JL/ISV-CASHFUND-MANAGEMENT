using CashFundManagement.Attributes;
using PX.Data;
using PX.Data.BQL;
using PX.Objects.GL;
using PX.Objects.RQ;
using System;
using messages = CashFundManagement.Messages.ATPTEFMMessages;

namespace CashFundManagement.DAC.Setup {
    //TODO : Remove dead codes on next upgrade.
    [PXPrimaryGraph(typeof(CashFundManagement.BLC.ATPTEFMFeaturesMaint))]
    [Serializable]
    [PXCacheName(messages.ATPTEFMFeatures)]
    public class ATPTEFMFeatures : Base.ATPTEFMAudit, IBqlTable
    {
        #region Budget

        #region BudgetModules
        [PXDBString(30,IsUnicode =true)]
        [PXDefault(PersistingCheck = PXPersistingCheck.Nothing)]
        [PXUIField(DisplayName = messages.BudgetModules)]
        [ATPTEFMFeatureBudgetModuleListAttribute.ATPTEFMFeatureBudgetModuleList(MultiSelect = true)]
        public virtual string BudgetModules { get; set; }
        public abstract class budgetModules : BqlString.Field<budgetModules> { }
        #endregion

        #region BudgetFeatureSet
        [PXDBString(100, IsUnicode = true)]
        [PXDefault(PersistingCheck = PXPersistingCheck.Nothing)]
        [PXUIField(DisplayName = messages.BudgetFeatureSet)]
        [PXStringList("F1;Deduct Budget on Record Creation", MultiSelect = true)]
        public virtual string BudgetFeatureSet { get; set; }
        public abstract class budgetFeatureSet : BqlString.Field<budgetModules> { }
        #endregion

        #region BudgetDocumentAmount
        [PXDBString(IsUnicode = true)]
        [PXDefault(PersistingCheck = PXPersistingCheck.NullOrBlank)]
        [PXUIField(DisplayName = messages.BudgetDocumentAmount)]
        [PXStringList("F1;Current Record Amount,NA;N/A")]
        public virtual string BudgetDocumentAmount { get; set; }
        public abstract class budgetDocumentAmount : BqlString.Field<budgetDocumentAmount> { }
        #endregion

        #region BudgetModules
        [PXDBString(IsUnicode = true)]
        [PXDefault(PersistingCheck = PXPersistingCheck.NullOrBlank)]
        [PXUIField(DisplayName = messages.BudgetModulesRqstAmt)]
        [PXStringList("F1;Approved Amount + Unapproved Amount or Current Module,NA;N/A")]
        public virtual string BudgetRequestAmount { get; set; }
        public abstract class budgetRequestAmount : BqlString.Field<budgetRequestAmount> { }
        #endregion

        #region BudgetBudgetAmount
        [PXDBString(IsUnicode = true)]
        [PXDefault(PersistingCheck = PXPersistingCheck.NullOrBlank)]
        [PXUIField(DisplayName = messages.BudgetBudgetAmount)]
        [PXStringList("F1;(Initial Budget - Amount Spent) + Returns,F2;(Initial Budget - (Total Approved + Total Unapproved)) + Returns,F3;Initial Budget - Total Approved + Returns,F4;(Initial Budget - (Total Approved/Spent + Total Unapproved)) + Returns,NA;N/A")]
        public virtual string BudgetBudgetAmount { get; set; }
        public abstract class budgetBudgetAmount : BqlString.Field<budgetBudgetAmount> { }
        #endregion

        #region BudgetSpentAmount
        [PXDBString(IsUnicode = true)]
        [PXDefault(PersistingCheck = PXPersistingCheck.NullOrBlank)]
        [PXUIField(DisplayName = messages.BudgetSpentAmount)]
        [PXStringList("F1;Total Journal Transaction Spent Amount,NA;N/A")]
        public virtual string BudgetSpentAmount { get; set; }
        public abstract class budgetSpentAmount : BqlString.Field<budgetSpentAmount> { }
        #endregion

        #region BudgetApprovedAmount
        [PXDBString(IsUnicode = true)]
        [PXDefault(PersistingCheck = PXPersistingCheck.NullOrBlank)]
        [PXUIField(DisplayName = messages.BudgetApprovedAmount)]
        [PXStringList("F1;Total Approved Amount of the Module,F2;Total Approved Amount (All Modules),NA;N/A")]
        public virtual string BudgetApprovedAmount { get; set; }
        public abstract class budgetApprovedAmount : BqlString.Field<budgetApprovedAmount> { }
        #endregion

        #region BudgetUnapprovedAmount
        [PXDBString(IsUnicode = true)]
        [PXDefault(PersistingCheck = PXPersistingCheck.NullOrBlank)]
        [PXUIField(DisplayName = messages.BudgetUnapprovedAmount)]
        [PXStringList("F1;Total Unapproved Amount of the Module,F2;Total Unapproved Amount (All Modules),NA;N/A")]
        public virtual string BudgetUnapprovedAmount { get; set; }
        public abstract class budgetUnapprovedAmount : BqlString.Field<budgetUnapprovedAmount> { }
        #endregion

        #region BudgetReturnAmount
        [PXDBString(IsUnicode = true)]
        [PXDefault(PersistingCheck = PXPersistingCheck.NullOrBlank)]
        [PXUIField(DisplayName = messages.BudgetReturnAmount)]
        [PXStringList("F1;Total Returned Amount,NA;N/A")]
        public virtual string BudgetReturnAmount { get; set; }
        public abstract class budgetReturnAmount : BqlString.Field<budgetReturnAmount> { }
        #endregion

        //#region BudgetPOAmount
        //[PXDBString(IsUnicode = true)]
        //[PXDefault(PersistingCheck = PXPersistingCheck.Nothing)]
        //[PXUIField(DisplayName = messages.BudgetPOAmount)]
        //[PXStringList("F1;Purchase Order Amount,NA;N/A")]
        //public virtual string BudgetPOAmount { get; set; }
        //public abstract class budgetPOAmount : BqlString.Field<budgetPOAmount> { }
        //#endregion

        #region BudgetValidation
        [PXDBInt()]
        [PXDefault(RQRequestClassBudget.None, PersistingCheck = PXPersistingCheck.Nothing)]
        [RQRequestClassBudget.List]
        [PXUIField(DisplayName = messages.UsrRLBudgetValidation, Visibility = PXUIVisibility.Visible)]
        public int? BudgetValidation { get; set; }
        public abstract class budgetValidation : IBqlField { }
        #endregion

        #region BudgetCalculation
        [PXDBString(1, IsFixed = true)]
        [PXDefault(RQBudgetCalculationType.YTD, PersistingCheck = PXPersistingCheck.Nothing)]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.UsrRLBudgetCalculation)]
        [RQBudgetCalculationType.List]
        public string BudgetCalculation { get; set; }
        public abstract class budgetCalculation : PX.Data.BQL.BqlString.Field<budgetCalculation> { }
        #endregion

        #region BudgetLedgerID
        [PXDBInt]
        [PXDefault(PersistingCheck = PXPersistingCheck.Nothing)]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.UsrRLBudgetLedgerID, Required = true)]
        [PXSelector(typeof(Search<Ledger.ledgerID, Where<Ledger.balanceType, Equal<LedgerBalanceType.budget>>>),
            SubstituteKey = typeof(Ledger.ledgerCD),
            DescriptionField = typeof(Ledger.descr))]
        public int? BudgetLedgerID { get; set; }
        public abstract class budgetLedgerID : PX.Data.BQL.BqlInt.Field<budgetLedgerID> { }
        #endregion

        #region Column Labels

        #region BudgetDocumentAmountLabel
        [PXDBString(50, IsUnicode = true)]
        [PXDefault(PersistingCheck = PXPersistingCheck.Nothing)]
        [PXUIField(DisplayName = messages.BudgetDocumentAmountLabel)]
        public virtual string BudgetDocumentAmountLabel { get; set; }
        public abstract class budgetDocumentAmountLabel : BqlString.Field<budgetDocumentAmountLabel> { }
        #endregion

        #region BudgetModules
        [PXDBString(50, IsUnicode = true)]
        [PXDefault(PersistingCheck = PXPersistingCheck.Nothing)]
        [PXUIField(DisplayName = messages.BudgetModulesRqstAmt)]
        public virtual string BudgetRequestAmountLabel { get; set; }
        public abstract class budgetRequestAmountLabel : BqlString.Field<budgetRequestAmountLabel> { }
        #endregion

        #region BudgetBudgetAmountLabel
        [PXDBString(50, IsUnicode = true)]
        [PXDefault(PersistingCheck = PXPersistingCheck.Nothing)]
        [PXUIField(DisplayName = messages.BudgetBudgetAmountLabel)]
        public virtual string BudgetBudgetAmountLabel { get; set; }
        public abstract class budgetBudgetAmountLabel : BqlString.Field<budgetBudgetAmountLabel> { }
        #endregion

        #region BudgetSpentAmountLabel
        [PXDBString(50, IsUnicode = true)]
        [PXDefault(PersistingCheck = PXPersistingCheck.Nothing)]
        [PXUIField(DisplayName = messages.BudgetSpentAmountLabel)]
        public virtual string BudgetSpentAmountLabel { get; set; }
        public abstract class budgetSpentAmountLabel : BqlString.Field<budgetSpentAmountLabel> { }
        #endregion

        #region BudgetApprovedAmountLabel
        [PXDBString(50, IsUnicode = true)]
        [PXDefault(PersistingCheck = PXPersistingCheck.Nothing)]
        [PXUIField(DisplayName = messages.BudgetApprovedAmountLabel)]
        public virtual string BudgetApprovedAmountLabel { get; set; }
        public abstract class budgetApprovedAmountLabel : BqlString.Field<budgetApprovedAmountLabel> { }
        #endregion

        #region BudgetUnapprovedAmountLabel
        [PXDBString(50, IsUnicode = true)]
        [PXDefault(PersistingCheck = PXPersistingCheck.Nothing)]
        [PXUIField(DisplayName = messages.BudgetUnapprovedAmountLabel)]
        public virtual string BudgetUnapprovedAmountLabel { get; set; }
        public abstract class budgetUnapprovedAmountLabel : BqlString.Field<budgetUnapprovedAmountLabel> { }
        #endregion

        #region BudgetReturnAmountLabel
        [PXDBString(50, IsUnicode = true)]
        [PXDefault(PersistingCheck = PXPersistingCheck.Nothing)]
        [PXUIField(DisplayName = messages.BudgetReturnAmountLabel)]
        public virtual string BudgetReturnAmountLabel { get; set; }
        public abstract class budgetReturnAmountLabel : BqlString.Field<budgetReturnAmountLabel> { }
        #endregion

        //#region BudgetPOAmountLabel
        //[PXDBString(50, IsUnicode = true)]
        //[PXDefault(PersistingCheck = PXPersistingCheck.Nothing)]
        //[PXUIField(DisplayName = messages.BudgetPOAmountLabel)]
        //public virtual string BudgetPOAmountLabel { get; set; }
        //public abstract class budgetPOAmountLabel : BqlString.Field<budgetPOAmountLabel> { }
        //#endregion

        #endregion

        #endregion

        #region ProjectBudget

        #region ProjectBudgetModules
        [PXDBString(30, IsUnicode = true)]
        [PXDefault(PersistingCheck = PXPersistingCheck.Nothing)]
        [PXUIField(DisplayName = messages.ProjectBudgetModules)]
        [PXStringList("C;Cash Advance,F;Fund Transaction,P;Request for Payment", MultiSelect = true)]
        public virtual string ProjectBudgetModules { get; set; }
        public abstract class projectBudgetModules : BqlString.Field<projectBudgetModules> { }
        #endregion

        #region ProjectBudgetFeatureSet
        [PXDBString(100, IsUnicode = true)]
        [PXDefault(PersistingCheck = PXPersistingCheck.Nothing)]
        [PXUIField(DisplayName = messages.ProjectBudgetFeatureSet)]
        [PXStringList("F1;Deduct Project Budget on Record Creation", MultiSelect = true)]
        public virtual string ProjectBudgetFeatureSet { get; set; }
        public abstract class projectBudgetFeatureSet : BqlString.Field<projectBudgetModules> { }
        #endregion

        #region ProjectBudgetDocumentAmount
        [PXDBString(IsUnicode = true)]
        [PXDefault(PersistingCheck = PXPersistingCheck.NullOrBlank)]
        [PXUIField(DisplayName = messages.ProjectBudgetDocumentAmount)]
        [PXStringList("F1;Current Record Amount,NA;N/A")]
        public virtual string ProjectBudgetDocumentAmount { get; set; }
        public abstract class projectBudgetDocumentAmount : BqlString.Field<projectBudgetDocumentAmount> { }
        #endregion

        #region ProjectBudgetRequestAmount
        [PXDBString(IsUnicode = true)]
        [PXDefault(PersistingCheck = PXPersistingCheck.NullOrBlank)]
        [PXUIField(DisplayName = messages.ProjectBudgetRequestAmount)]
        [PXStringList("F1;Approved Amount + Unapproved Amount or Current Module,NA;N/A")]
        public virtual string ProjectBudgetRequestAmount { get; set; }
        public abstract class projectBudgetRequestAmount : BqlString.Field<projectBudgetRequestAmount> { }
        #endregion

        #region ProjectBudgetBudgetAmount
        [PXDBString(IsUnicode = true)]
        [PXDefault(PersistingCheck = PXPersistingCheck.NullOrBlank)]
        [PXUIField(DisplayName = messages.ProjectBudgetBudgetAmount)]
        [PXStringList("F1;(Initial Budget - Amount Spent + Returns),F2;(Initial Budget - (Total Approved + Total Unapproved)) + Returns),F3;(Initial Budget - (Total Approved/Spent + Total Unapproved) + Returns),NA;N/A")]
        public virtual string ProjectBudgetBudgetAmount { get; set; }
        public abstract class projectBudgetBudgetAmount : BqlString.Field<projectBudgetBudgetAmount> { }
        #endregion

        #region ProjectBudgetSpentAmount
        [PXDBString(IsUnicode = true)]
        [PXDefault(PersistingCheck = PXPersistingCheck.NullOrBlank)]
        [PXUIField(DisplayName = messages.ProjectBudgetSpentAmount)]
        [PXStringList("F1;Total Journal Transaction Spent Amount,NA;N/A")]
        public virtual string ProjectBudgetSpentAmount { get; set; }
        public abstract class projectBudgetSpentAmount : BqlString.Field<projectBudgetSpentAmount> { }
        #endregion

        #region ProjectBudgetApprovedAmount
        [PXDBString(IsUnicode = true)]
        [PXDefault(PersistingCheck = PXPersistingCheck.NullOrBlank)]
        [PXUIField(DisplayName = messages.ProjectBudgetApprovedAmount)]
        [PXStringList("F1;Total Approved Amount of the Module,F2;Total Approved Amount (All Modules),NA;N/A")]
        public virtual string ProjectBudgetApprovedAmount { get; set; }
        public abstract class projectBudgetApprovedAmount : BqlString.Field<projectBudgetApprovedAmount> { }
        #endregion

        #region ProjectBudgetUnapprovedAmount
        [PXDBString( IsUnicode = true)]
        [PXDefault(PersistingCheck = PXPersistingCheck.NullOrBlank)]
        [PXUIField(DisplayName = messages.ProjectBudgetUnapprovedAmount)]
        [PXStringList("F1;Total Unapproved Amount of the Module,F2;Total Unapproved Amount (All Modules),NA;N/A")]
        public virtual string ProjectBudgetUnapprovedAmount { get; set; }
        public abstract class projectBudgetUnapprovedAmount : BqlString.Field<projectBudgetUnapprovedAmount> { }
        #endregion

        #region ProjectBudgetReturnAmount
        [PXDBString(IsUnicode = true)]
        [PXDefault(PersistingCheck = PXPersistingCheck.NullOrBlank)]
        [PXUIField(DisplayName = messages.ProjectBudgetReturnAmount)]
        [PXStringList("F1;Total Returned Amount,NA;N/A")]
        public virtual string ProjectBudgetReturnAmount { get; set; }
        public abstract class projectBudgetReturnAmount : BqlString.Field<projectBudgetReturnAmount> { }
        #endregion

        //#region ProjectBudgetPOAmount
        //[PXDBString(IsUnicode = true)]
        //[PXDefault(PersistingCheck = PXPersistingCheck.NullOrBlank)]
        //[PXUIField(DisplayName = messages.ProjectBudgetPOAmount)]
        //[PXStringList("F1;Purchase Order Amount,NA;N/A")]
        //public virtual string ProjectBudgetPOAmount { get; set; }
        //public abstract class projectBudgetPOAmount : BqlString.Field<projectBudgetPOAmount> { }
        //#endregion

        #region ProjectBudgetValidation
        [PXDBInt()]
        [PXDefault(RQRequestClassBudget.None, PersistingCheck = PXPersistingCheck.Nothing)]
        [RQRequestClassBudget.List]
        [PXUIField(DisplayName = messages.UsrRLBudgetValidation, Visibility = PXUIVisibility.Visible)]
        public int? ProjectBudgetValidation { get; set; }
        public abstract class projectBudgetValidation : IBqlField { }
        #endregion

        #region ProjectBudgetCalculation
        [PXDBString(1, IsFixed = true)]
        [PXDefault(RQBudgetCalculationType.YTD, PersistingCheck = PXPersistingCheck.Nothing)]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.UsrRLBudgetCalculation)]
        [RQBudgetCalculationType.List]
        public string ProjectBudgetCalculation { get; set; }
        public abstract class projectBudgetCalculation : PX.Data.BQL.BqlString.Field<projectBudgetCalculation> { }
        #endregion

        #region ProjectBudgetLedgerID
        [PXDBInt]
        [PXDefault(PersistingCheck = PXPersistingCheck.Nothing)]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.UsrRLBudgetLedgerID, Required = true)]
        [PXSelector(typeof(Search<Ledger.ledgerID, Where<Ledger.balanceType, Equal<LedgerBalanceType.budget>>>),
            SubstituteKey = typeof(Ledger.ledgerCD),
            DescriptionField = typeof(Ledger.descr))]
        public int? ProjectBudgetLedgerID { get; set; }
        public abstract class projectBudgetLedgerID : PX.Data.BQL.BqlInt.Field<projectBudgetLedgerID> { }
        #endregion

        #region Column Labels

        #region ProjectBudgetDocumentAmountLabel
        [PXDBString(50, IsUnicode = true)]
        [PXDefault(PersistingCheck = PXPersistingCheck.Nothing)]
        [PXUIField(DisplayName = messages.ProjectBudgetDocumentAmountLabel)]
        public virtual string ProjectBudgetDocumentAmountLabel { get; set; }
        public abstract class projectBudgetDocumentAmountLabel : BqlString.Field<projectBudgetDocumentAmountLabel> { }
        #endregion

        #region ProjectBudgetModules
        [PXDBString(50, IsUnicode = true)]
        [PXDefault(PersistingCheck = PXPersistingCheck.Nothing)]
        [PXUIField(DisplayName = messages.ProjectBudgetModulesRqstAmt)]
        public virtual string ProjectBudgetRequestAmountLabel { get; set; }
        public abstract class projectBudgetRequestAmountLabel : BqlString.Field<projectBudgetRequestAmountLabel> { }
        #endregion

        #region ProjectBudgetBudgetAmountLabel
        [PXDBString(50, IsUnicode = true)]
        [PXDefault(PersistingCheck = PXPersistingCheck.Nothing)]
        [PXUIField(DisplayName = messages.ProjectBudgetBudgetAmountLabel)]
        public virtual string ProjectBudgetBudgetAmountLabel { get; set; }
        public abstract class projectBudgetBudgetAmountLabel : BqlString.Field<projectBudgetBudgetAmountLabel> { }
        #endregion

        #region ProjectBudgetSpentAmountLabel
        [PXDBString(50, IsUnicode = true)]
        [PXDefault(PersistingCheck = PXPersistingCheck.Nothing)]
        [PXUIField(DisplayName = messages.ProjectBudgetSpentAmountLabel)]
        public virtual string ProjectBudgetSpentAmountLabel { get; set; }
        public abstract class projectBudgetSpentAmountLabel : BqlString.Field<projectBudgetSpentAmountLabel> { }
        #endregion

        #region ProjectBudgetApprovedAmountLabel
        [PXDBString(50, IsUnicode = true)]
        [PXDefault(PersistingCheck = PXPersistingCheck.Nothing)]
        [PXUIField(DisplayName = messages.ProjectBudgetApprovedAmountLabel)]
        public virtual string ProjectBudgetApprovedAmountLabel { get; set; }
        public abstract class projectBudgetApprovedAmountLabel : BqlString.Field<projectBudgetApprovedAmountLabel> { }
        #endregion

        #region ProjectBudgetUnapprovedAmountLabel
        [PXDBString(50, IsUnicode = true)]
        [PXDefault(PersistingCheck = PXPersistingCheck.Nothing)]
        [PXUIField(DisplayName = messages.ProjectBudgetUnapprovedAmountLabel)]
        public virtual string ProjectBudgetUnapprovedAmountLabel { get; set; }
        public abstract class projectBudgetUnapprovedAmountLabel : BqlString.Field<projectBudgetUnapprovedAmountLabel> { }
        #endregion

        #region ProjectBudgetReturnAmountLabel
        [PXDBString(50, IsUnicode = true)]
        [PXDefault(PersistingCheck = PXPersistingCheck.Nothing)]
        [PXUIField(DisplayName = messages.ProjectBudgetReturnAmountLabel)]
        public virtual string ProjectBudgetReturnAmountLabel { get; set; }
        public abstract class projectBudgetReturnAmountLabel : BqlString.Field<projectBudgetReturnAmountLabel> { }
        #endregion

        //#region ProjectBudgetPOAmountLabel
        //[PXDBString(50, IsUnicode = true)]
        //[PXDefault(PersistingCheck = PXPersistingCheck.Nothing)]
        //[PXUIField(DisplayName = messages.ProjectBudgetPOAmountLabel)]
        //public virtual string ProjectBudgetPOAmountLabel { get; set; }
        //public abstract class projectBudgetPOAmountLabel : BqlString.Field<projectBudgetPOAmountLabel> { }
        //#endregion

        #endregion

        #endregion

        #region Transfer Assets
        [PXDBString(2, IsUnicode = true)]
        [PXDefault(PersistingCheck = PXPersistingCheck.Nothing)]
        [PXUIField(DisplayName = messages.TransferAssetCustodian)]
        [PXStringList("F1;Show,NA;N/A")]
        [PXUIVisible(typeof(False))]
        public virtual string TransferAssetCustodian { get; set; }
        public abstract class transferAssetCustodian : BqlString.Field<transferAssetCustodian> { }

        [PXDBString(2, IsUnicode = true)]
        [PXDefault(PersistingCheck = PXPersistingCheck.Nothing)]
        [PXUIField(DisplayName = messages.TransferAssetBuilding)]
        [PXStringList("F1;Show,NA;N/A")]
        [PXUIVisible(typeof(False))]
        public virtual string TransferAssetBuilding { get; set; }
        public abstract class transferAssetBuilding : BqlString.Field<transferAssetBuilding> { }

        [PXDBString(2, IsUnicode = true)]
        [PXDefault(PersistingCheck = PXPersistingCheck.Nothing)]
        [PXUIField(DisplayName = messages.TransferAssetFloor)]
        [PXStringList("F1;Show,NA;N/A")]
        [PXUIVisible(typeof(False))]
        public virtual string TransferAssetFloor { get; set; }
        public abstract class transferAssetFloor : BqlString.Field<transferAssetFloor> { }

        [PXDBString(2, IsUnicode = true)]
        [PXDefault(PersistingCheck = PXPersistingCheck.Nothing)]
        [PXUIField(DisplayName = messages.TransferAssetRoom)]
        [PXStringList("F1;Show,NA;N/A")]
        [PXUIVisible(typeof(False))]
        public virtual string TransferAssetRoom { get; set; }
        public abstract class transferAssetRoom : BqlString.Field<transferAssetRoom> { }
        #endregion

        //-006523 Transfer fields to Cash Advance Preference
        //#region Cash Advance

        //[PXDBBool]
        //[PXDefault(false)]
        //[PXUIField(DisplayName = "Enable Account")]
        //public virtual bool? CashAdvanceAccountEnable { get; set; }
        //public abstract class cashAdvanceAccountEnable : BqlBool.Field<cashAdvanceAccountEnable> { }

        //[PXDBBool]
        //[PXDefault(true)]
        //[PXUIField(DisplayName = "Enable Subaccount")]
        //public virtual bool? CashAdvanceSubAccountEnable { get; set; }
        //public abstract class cashAdvanceSubAccountEnable : BqlBool.Field<cashAdvanceSubAccountEnable> { }

        [PXDBBool]
        [PXDefault(false)]
        [PXUIField(DisplayName = "Override Receipts")]
        [PXUIVisible(typeof(False))]
        public virtual bool? CashAdvanceOverride { get; set; }
        public abstract class cashAdvanceOverride : BqlBool.Field<cashAdvanceOverride> { }

        //#endregion

        #region FundTransaction

        #region LimitValidation
        [PXDBBool]
        [PXDefault(false)]
        [PXUIField(DisplayName = messages.LimitValidation)]
        [PXUIVisible(typeof(False))]
        public virtual bool? LimitValidation { get; set; }
        public abstract class limitValidation : BqlBool.Field<limitValidation> { }
        #endregion

        #region LimitValidationAmt
        [PXDBDecimal(2)]
        [PXDefault(TypeCode.Decimal, "0.0", PersistingCheck = PXPersistingCheck.Nothing)]
        [PXUIField(DisplayName = messages.LimitValidationAmt)]
        [PXUIEnabled(typeof(Where<Current<limitValidation>, Equal<True>>))]
        [PXUIVisible(typeof(False))]
        public virtual decimal? LimitValidationAmt { get; set; }
        public abstract class limitValidationAmt : BqlDecimal.Field<limitValidationAmt> { }
        #endregion

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

    }

}