using CashFundManagement.Attributes;
using CashFundManagement.BLC;
using CashFundManagement.DAC.Setup;
using CashFundManagement.Messages;
using PX.Data;
using PX.Data.BQL;
using PX.Data.EP;
using PX.Objects.CM;
using PX.Objects.Common;
using PX.Objects.CS;
using PX.Objects.EP;
using PX.Objects.GL;
using PX.TM;
using System;

namespace CashFundManagement.DAC {
    [PXPrimaryGraph(typeof(ATPTEFMMonthEndEntry))]
    [Serializable]
    [PXCacheName(Messages.ATPTEFMMessages.ATPTEFMMonthEnd)]
    public class ATPTEFMMonthEnd : Base.ATPTEFMAudit, IBqlTable, IAssign
    {
        #region RefNbr                
        [PXDBString(15, IsKey = true, InputMask = ">CCCCCCCCCCCCCCC", IsUnicode = true)]
        [PXDefault()]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.RefNbr)]
        [PXSelector(typeof(refNbr))]
        [AutoNumber(typeof(ATPTEFMSetup.monthEndNumberingID), typeof(AccessInfo.businessDate))]
        public virtual string RefNbr { get; set; }
        public abstract class refNbr : BqlString.Field<refNbr> { }
        #endregion

        #region Status
        [PXDBString(IsFixed = true)]
        [ATPTEFMMonthEndStatusAttribute.ATPTEFMMonthEndStatus]
        [PXDefault(ATPTEFMMonthEndStatusAttribute.HoldValue)]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.Status, Enabled = false)]
        public virtual string Status { get; set; }
        public abstract class status : BqlString.Field<status> { }
        #endregion

        #region Hold
        [PXDBBool()]
        [PXDefault(true)]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.Hold)]
        public virtual bool? Hold { get; set; }
        public abstract class hold : BqlBool.Field<hold> { }
        #endregion

        #region Date
        [PXDBDate()]
        [PXDefault(typeof(AccessInfo.businessDate))]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.Date, Visibility = PXUIVisibility.SelectorVisible)]
        public virtual DateTime? Date { get; set; }
        public abstract class date : BqlDateTime.Field<date> { }
        #endregion

        #region FinPeriodID
        [FinPeriodID(typeof(date), typeof(branchID))]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.FinPeriodID, Visibility = PXUIVisibility.SelectorVisible)]
        public virtual String FinPeriodID { get; set; }
        public abstract class finPeriodID : BqlString.Field<finPeriodID> { }
        #endregion

        #region FundID
        [PXDBString(15, IsUnicode = true)]
        [PXDefault(PersistingCheck = PXPersistingCheck.NullOrBlank)]
        [PXSelector(typeof(Search<
            ATPTEFMFund.fundCD, 
            Where<ATPTEFMFund.status, Equal<ATPTEFMFundStatusAttribute.activeValue>,
                And<ATPTEFMFund.isActive, Equal<boolTrue>>>, 
            OrderBy<
                Desc<ATPTEFMFund.fundCD>>>),
                    typeof(ATPTEFMFund.fundCD),
                    typeof(ATPTEFMFund.fundType),
                    SubstituteKey = typeof(ATPTEFMFund.fundCD),
                    DescriptionField = typeof(ATPTEFMFund.descr))]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.FundID)]
        public virtual string FundID { get; set; }
        public abstract class fundID : BqlString.Field<fundID> { }

        #endregion

        #region AccountID
        [Account(DisplayName = Messages.ATPTEFMMessages.ClearingAccount, Enabled = false)]
        [PXDefault(typeof(Search<
            ATPTEFMFund.clearingAccount,
            Where<ATPTEFMFund.fundCD, Equal<Current<fundID>>>>), PersistingCheck = PXPersistingCheck.Nothing)]
        [PXFormula(typeof(Default<fundID>))]
        public virtual int? AccountID { get; set; }

        public abstract class accountID : BqlInt.Field<accountID> { }
        #endregion

        #region SubID
        [SubAccount(typeof(accountID), DisplayName = Messages.ATPTEFMMessages.ClearingSubaccount, Enabled = false)]
        [PXDefault(typeof(Search<
            ATPTEFMFund.clearingSubaccount,
            Where<ATPTEFMFund.fundCD, Equal<Current<fundID>>>>), PersistingCheck = PXPersistingCheck.Nothing)]
        [PXFormula(typeof(Default<fundID>))]
        public virtual int? SubID { get; set; }

        public abstract class subID : BqlInt.Field<subID> { }
        #endregion

        #region CreditAccountID
        //[PXSelector(
        //    typeof(Search<Account.accountID>),
        //    typeof(Account.accountCD),
        //    typeof(Account.description),
        //    SubstituteKey = typeof(Account.accountCD))]
        [Account(DisplayName = Messages.ATPTEFMMessages.CreditAccountID, Enabled = false)]
        [PXDefault(typeof(Search<
            ATPTEFMFund.clearingAccount, 
            Where<ATPTEFMFund.fundCD, Equal<Current<fundID>>>>), PersistingCheck = PXPersistingCheck.Nothing)]
        [PXFormula(typeof(Default<fundID>))]
        public virtual int? CreditAccountID { get; set; }
        public abstract class creditAccountID : BqlInt.Field<creditAccountID> { }
        #endregion

        #region JournalBatchNbr
        [PXDBString(15, IsUnicode = true)]
        [PXUIField(DisplayName = ATPTEFMMessages.BatchNbr, Enabled = false)]
        public virtual string JournalBatchNbr { get; set; }
        public abstract class journalBatchNbr : BqlString.Field<journalBatchNbr> { }
        #endregion

        #region ReversingJournalBatchNbr
        [PXDBString(15, IsUnicode = true)]
        [PXUIField(DisplayName = ATPTEFMMessages.ReversingBatchNbr, Enabled = false)]
        public virtual string ReversingJournalBatchNbr { get; set; }
        public abstract class reversingJournalBatchNbr : BqlString.Field<reversingJournalBatchNbr> { }
        #endregion

        #region Released
        [PXDBBool]
        [PXDefault(false)]
        public virtual bool? Released { get; set; }
        public abstract class released : BqlBool.Field<released> { }
        #endregion

        #region BranchID

        public abstract class branchID : PX.Data.BQL.BqlInt.Field<branchID> { }
        protected Int32? _BranchID;
        [PXDefault(typeof(Search<ATPTEFMFund.branchID, Where<ATPTEFMFund.fundCD, Equal<Current<fundID>>>>), PersistingCheck = PXPersistingCheck.Nothing)]
        [PXFormula(typeof(Default<fundID>))]
        [Branch(typeof(AccessInfo.branchID), Required = true)]
        [PXUIEnabled(typeof(False))]
        public virtual Int32? BranchID
        {
            get
            {
                return this._BranchID;
            }
            set
            {
                this._BranchID = value;
            }
        }
        #endregion

        #region OwnerID
        [Owner(typeof(workgroupID), DisplayName = Messages.ATPTEFMMessages.OwnerID)]
        [PXDefault(typeof(Search<
            EPEmployee.defContactID, 
            Where<EPEmployee.userID, Equal<Current<AccessInfo.userID>>>>), PersistingCheck = PXPersistingCheck.Nothing)]
        //[PXUIField(DisplayName = Messages.ATPTEFMMessages.OwnerID)]
        public virtual int? OwnerID { get; set; }
        public abstract class ownerID : BqlInt.Field<ownerID> { }
        #endregion

        #region WorkgroupID
        [PXDBInt]
        [PXDefault(typeof(EPCompanyTree.workGroupID), PersistingCheck = PXPersistingCheck.Nothing)]
        [PX.TM.PXCompanyTreeSelector]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.WorkgroupID, Enabled = false)]
        public virtual int? WorkgroupID { get; set; }
        public abstract class workgroupID : BqlInt.Field<workgroupID> { }
        #endregion

        #region Approved
        [PXDBBool()]
        [PXDefault(false)]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.Approved, Visibility = PXUIVisibility.Visible, Enabled = false)]
        public virtual bool? Approved { get; set; }
        public abstract class approved : BqlBool.Field<approved> { }
        #endregion

        #region Rejected
        public abstract class rejected : BqlBool.Field<rejected> { }
        [PXDBBool]
        [PXDefault(false, PersistingCheck = PXPersistingCheck.Nothing)]
        public bool? Rejected { get; set; }
        #endregion

        #region CuryID
        public abstract class curyID : PX.Data.BQL.BqlString.Field<curyID> { }
        protected String _CuryID;
        [PXDBString(5, IsUnicode = true, InputMask = ">LLLLL")]
        [PXUIField(DisplayName = "Currency", Visibility = PXUIVisibility.SelectorVisible, Enabled = false)]
        [PXDefault(typeof(Search<Company.baseCuryID>))]
        [PXSelector(typeof(Currency.curyID))]
        public virtual String CuryID
        {
            get
            {
                return this._CuryID;
            }
            set
            {
                this._CuryID = value;
            }
        }
        #endregion

        #region CuryInfoID
        public abstract class curyInfoID : PX.Data.BQL.BqlLong.Field<curyInfoID> { }
        protected Int64? _CuryInfoID;
        [PXDBLong()]
        [CurrencyInfo(ModuleCode = BatchModule.AP)]
        public virtual Int64? CuryInfoID
        {
            get
            {
                return this._CuryInfoID;
            }
            set
            {
                this._CuryInfoID = value;
            }
        }
        #endregion

        #region Amount
        [PXDBDecimal(2)]
        [PXUIField(DisplayName = ATPTEFMMessages.Amount, Enabled = false)]
        [PXDefault(TypeCode.Decimal, "0.0", PersistingCheck = PXPersistingCheck.Nothing)]
        public virtual decimal? Amount { get; set; }
        public abstract class amount : BqlDecimal.Field<amount> { }
        #endregion

        #region NoteID  
        [PXNote]
        public virtual Guid? NoteID { get; set; }
        public abstract class noteID : BqlGuid.Field<noteID> { }
        #endregion

        #region IAssign Members
        int? PX.Data.EP.IAssign.WorkgroupID
        {
            get { return WorkgroupID; }
            set { WorkgroupID = value; }
        }

        int? PX.Data.EP.IAssign.OwnerID
        {
            get { return OwnerID; }
            set { OwnerID = value; }
        }
        #endregion

        #region Unbound

        #region RequestApproval
        [PXBool]
        [PXUnboundDefault(typeof(GetSetupValue<ATPTEFMSetup.monthEndRequestApproval>), PersistingCheck = PXPersistingCheck.Null)]
        [PXUIField(DisplayName = ATPTEFMMessages.RequestApproval, Visible = false)]
        public virtual bool? RequestApproval { get; set; }

        public abstract class requestApproval : PX.Data.BQL.BqlBool.Field<requestApproval> { }
        #endregion

        #region ApprovalWorkgroupID
        public abstract class approvalWorkgroupID : PX.Data.IBqlField
        {
        }
        protected int? _ApprovalWorkgroupID;
        [PXInt]
        [PXSelector(typeof(Search<EPCompanyTree.workGroupID>), SubstituteKey = typeof(EPCompanyTree.description))]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.ApprovalWorkgroupID, Enabled = false)]
        public virtual int? ApprovalWorkgroupID
        {
            get
            {
                return this._ApprovalWorkgroupID;
            }
            set
            {
                this._ApprovalWorkgroupID = value;
            }
        }
        #endregion

        #region ApprovalOwnerID

        public abstract class approvalOwnerID : PX.Data.BQL.BqlInt.Field<approvalOwnerID> { }
        protected int? _ApprovalOwnerID;
        [PX.TM.Owner(IsDBField = false, DisplayName = "Approver", Enabled = false)]
        public virtual int? ApprovalOwnerID
        {
            get
            {
                return this._ApprovalOwnerID;
            }
            set
            {
                this._ApprovalOwnerID = value;
            }
        }
        #endregion

        #endregion
    }
}
