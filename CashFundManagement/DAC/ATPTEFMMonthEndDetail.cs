using CashFundManagement.BLC;
using CashFundManagement.Messages;
using PX.Data;
using PX.Data.BQL;
using PX.Data.ReferentialIntegrity.Attributes;
using PX.Objects.CS;
using PX.Objects.CT;
using PX.Objects.EP;
using PX.Objects.GL;
using PX.Objects.IN;
using PX.Objects.PM;
using System;

namespace CashFundManagement.DAC {
    /// <remarks>
    ///
    /// </remarks>
    [PXPrimaryGraph(typeof(ATPTEFMMonthEndEntry))]
    [Serializable]
    [PXCacheName("Month-End Detail")]
    public class ATPTEFMMonthEndDetail : CashFundManagement.DAC.Base.ATPTEFMBase, IBqlTable
    {
        #region MonthEndDetailID
        [PXDBLongIdentity(IsKey = true)]
        public virtual long? MonthEndDetailID { get; set; }
        public abstract class monthEndDetailID : PX.Data.BQL.BqlLong.Field<monthEndDetailID> { }
        #endregion

        #region MonthEndRefNbr
        [PXDBString(15, IsUnicode = true)]
        [PXDBDefault(typeof(ATPTEFMMonthEnd.refNbr))]
        [PXParent(typeof(Select<
            ATPTEFMMonthEnd,
            Where<ATPTEFMMonthEnd.refNbr, Equal<Current<ATPTEFMMonthEndDetail.monthEndRefNbr>>>>))]
        public virtual string MonthEndRefNbr { get; set; }
        public abstract class monthEndRefNbr : PX.Data.IBqlField { }
        #endregion

        #region ExpenseReceiptRefNbr
        [PXDBString(15, IsUnicode = true)]
        [PXDefault(PersistingCheck = PXPersistingCheck.NullOrBlank)]
        [PXUIField(DisplayName = ATPTEFMMessages.ReceiptNbr)]
        public virtual string ExpenseReceiptRefNbr { get; set; }
        public abstract class expenseReceiptRefNbr : PX.Data.IBqlField { }
        #endregion

        #region InventoryID
        [PXDefault]
        [Inventory(DisplayName = ATPTEFMMessages.ExpenseItm)]
        [PXRestrictor(typeof(Where<InventoryItem.itemType, Equal<INItemTypes.expenseItem>>), ATPTEFMMessages.InventoryItemIsNotAnExpenseType)]
        [PXForeignReference(typeof(Field<inventoryID>.IsRelatedTo<InventoryItem.inventoryID>))]
        public virtual int? InventoryID { get; set; }
        public abstract class inventoryID : PX.Data.BQL.BqlInt.Field<inventoryID> { }
        #endregion

        #region AccountID
        [PXDefault]
        [Account(DisplayName = ATPTEFMMessages.AccountID, Visibility = PXUIVisibility.Visible)]
        public virtual int? AccountID { get; set; }
        public abstract class accountID : PX.Data.BQL.BqlInt.Field<accountID> { }
        #endregion

        #region SubID
        [PXDefault]
        [SubAccount(typeof(accountID), DisplayName = ATPTEFMMessages.Subid, Visibility = PXUIVisibility.Visible)]
        public virtual int? SubID { get; set; }
        public abstract class subID : PX.Data.BQL.BqlInt.Field<subID> { }
        #endregion

        #region ContractID
        [PXDBInt]
        [PXUIField(DisplayName = ATPTEFMMessages.ContractID)]
        [PXSelector(typeof(Contract.contractID), typeof(Contract.contractCD), typeof(Contract.description), SubstituteKey = typeof(Contract.contractCD), DescriptionField = typeof(Contract.description))]
        [PXUIVisible(typeof(FeatureInstalled<FeaturesSet.projectAccounting>))]
        public virtual int? ContractID { get; set; }
        public abstract class contractID : PX.Data.BQL.BqlInt.Field<contractID> { }
        #endregion

        #region TaskID
        [EPExpenseAllowProjectTaskAttribute(typeof(contractID), BatchModule.EA, DisplayName = "Project Task")]
        [PXUIVisible(typeof(FeatureInstalled<FeaturesSet.projectAccounting>))]
        public virtual int? TaskID { get; set; }
        public abstract class taskID : PX.Data.BQL.BqlInt.Field<taskID> { }
        #endregion

        #region CostCodeID
        [CostCode(typeof(accountID), typeof(taskID), PX.Objects.GL.AccountType.Expense, DisplayName = "Cost Code")]
        [PXUIVisible(typeof(FeatureInstalled<FeaturesSet.costCodes>))]
        public virtual Int32? CostCodeID { get; set; }
        public abstract class costCodeID : PX.Data.BQL.BqlInt.Field<costCodeID> { }
        #endregion

        #region Amount
        [PXDBDecimal(2)]
        [PXDefault(TypeCode.Decimal, "0.0")]
        [PXUIField(DisplayName = ATPTEFMMessages.Amount)]
        [PXFormula(null, typeof(SumCalc<ATPTEFMMonthEnd.amount>))]
        public virtual decimal? Amount { get; set; }
        public abstract class amount : PX.Data.BQL.BqlDecimal.Field<amount> { }
        #endregion

        #region BranchID
        [Branch(typeof(AccessInfo.branchID), DisplayName = Messages.ATPTEFMMessages.Branch)]
        public virtual int? BranchID { get; set; }
        public abstract class branchID : BqlInt.Field<branchID> { }
        #endregion

        #region CreatedByID
        public abstract class createdByID : PX.Data.BQL.BqlGuid.Field<createdByID> { }
		protected Guid? _CreatedByID;
		[PXDBCreatedByID()]
		public virtual Guid? CreatedByID
		{
			get
			{
				return this._CreatedByID;
			}
			set
			{
				this._CreatedByID = value;
			}
		}
		#endregion

		#region CreatedByScreenID
		public abstract class createdByScreenID : PX.Data.BQL.BqlString.Field<createdByScreenID> { }
		protected String _CreatedByScreenID;
		[PXDBCreatedByScreenID()]
		public virtual String CreatedByScreenID
		{
			get
			{
				return this._CreatedByScreenID;
			}
			set
			{
				this._CreatedByScreenID = value;
			}
		}
		#endregion

		#region CreatedDateTime
		public abstract class createdDateTime : PX.Data.BQL.BqlDateTime.Field<createdDateTime> { }
		protected DateTime? _CreatedDateTime;
		[PXDBCreatedDateTime()]
		public virtual DateTime? CreatedDateTime
		{
			get
			{
				return this._CreatedDateTime;
			}
			set
			{
				this._CreatedDateTime = value;
			}
		}
		#endregion

		#region LastModifiedByID
		public abstract class lastModifiedByID : PX.Data.BQL.BqlGuid.Field<lastModifiedByID> { }
		protected Guid? _LastModifiedByID;
		[PXDBLastModifiedByID()]
		public virtual Guid? LastModifiedByID
		{
			get
			{
				return this._LastModifiedByID;
			}
			set
			{
				this._LastModifiedByID = value;
			}
		}
		#endregion

		#region LastModifiedByScreenID
		public abstract class lastModifiedByScreenID : PX.Data.BQL.BqlString.Field<lastModifiedByScreenID> { }
		protected String _LastModifiedByScreenID;
		[PXDBLastModifiedByScreenID()]
		public virtual String LastModifiedByScreenID
		{
			get
			{
				return this._LastModifiedByScreenID;
			}
			set
			{
				this._LastModifiedByScreenID = value;
			}
		}
		#endregion

		#region LastModifiedDateTime
		public abstract class lastModifiedDateTime : PX.Data.BQL.BqlDateTime.Field<lastModifiedDateTime> { }
		protected DateTime? _LastModifiedDateTime;
		[PXDBLastModifiedDateTime()]
		public virtual DateTime? LastModifiedDateTime
		{
			get
			{
				return this._LastModifiedDateTime;
			}
			set
			{
				this._LastModifiedDateTime = value;
			}
		}
		#endregion

		#region tstamp
		public abstract class Tstamp : PX.Data.BQL.BqlByteArray.Field<Tstamp> { }
		protected Byte[] _tstamp;
		[PXDBTimestamp()]
		public virtual Byte[] tstamp
		{
			get
			{
				return this._tstamp;
			}
			set
			{
				this._tstamp = value;
			}
		}
		#endregion

		#region NoteID
		[PXNote]
		public virtual Guid? NoteID { get; set; }
		public abstract class noteID : BqlGuid.Field<noteID> { }
		#endregion

	}
}
