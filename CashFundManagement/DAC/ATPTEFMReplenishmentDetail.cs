using PX.Data;
using PX.Data.BQL;
using PX.Data.ReferentialIntegrity.Attributes;
using PX.Objects.CS;
using PX.Objects.EP;
using PX.Objects.GL;
using PX.Objects.IN;
using PX.Objects.PM;
using System;

namespace CashFundManagement.DAC {
    //[PXPrimaryGraph(typeof(ReplenishmentEntry))]
    [Serializable]
    [PXCacheName(Messages.ATPTEFMMessages.ATPTEEFMReplinesmentDetail)]
    public class ATPTEFMReplenishmentDetail : Base.ATPTEFMAudit, IBqlTable
    {
        #region ReplenishmentDetailID
        [PXDBIdentity(IsKey = true)]
        public virtual int? ReplenishmentDetailID { get; set; }
        public abstract class replenishmentDetailID : BqlInt.Field<replenishmentDetailID> { }
        #endregion

        #region ReplenishmentNbr
        [PXDBString(15, IsUnicode =true)]
        [PXDBDefault(typeof(ATPTEFMReplenishment.replenishmentNbr))]
        [PXParent(typeof(Select<
            ATPTEFMReplenishment,
            Where<ATPTEFMReplenishment.replenishmentNbr, Equal<Current<ATPTEFMReplenishmentDetail.replenishmentNbr>>>>))]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.ReplenishmentID)]
        public virtual string ReplenishmentNbr { get; set; }
        public abstract class replenishmentNbr : BqlString.Field<replenishmentNbr> { }
        #endregion

        #region ExpenseReceiptID
        [PXDBInt]
        [PXDefault(PersistingCheck = PXPersistingCheck.Nothing)]
        public virtual int? ExpenseReceiptID { get; set; }
        public abstract class expenseReceiptID : PX.Data.BQL.BqlInt.Field<expenseReceiptID> { }
        #endregion

        #region ExpenseReceiptNbr
        [PXDBString(15, IsUnicode =true)]
        [PXDBDefault()]
        [EPExpenceReceiptSelector]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.ExpenseReceiptNbr, IsReadOnly = true)]
        public virtual string ExpenseReceiptNbr { get; set; }
        public abstract class expenseReceiptNbr : BqlString.Field<expenseReceiptNbr> { }
        #endregion

        #region InvoiceRefNbr
        [PXDBString(15, IsUnicode = true)]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.InvoiceRefNbr)]
        public virtual string InvoiceRefNbr { get; set; }
        public abstract class invoiceRefNbr : BqlString.Field<invoiceRefNbr> { }
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

        #region CuryTaxTotal
        [PXDBDecimal(2)]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.TaxAmount, Visibility = PXUIVisibility.Visible, Enabled = false)]
        [PXDefault(TypeCode.Decimal, "0.0", PersistingCheck = PXPersistingCheck.Nothing)]
        public virtual decimal? CuryTaxTotal { get; set; }
        public abstract class curyTaxTotal : PX.Data.BQL.BqlDecimal.Field<curyTaxTotal> { }
        #endregion

        #region IsImported
        [PXDBBool()]
        [PXDefault(false, PersistingCheck = PXPersistingCheck.Nothing)]
        public virtual bool? IsImported { get; set; }
        public abstract class isImported : BqlBool.Field<isImported> { }
        #endregion

        /*#region Project Columns
        #region ContractID
        [PXForeignReference(typeof(Field<contractID>.IsRelatedTo<PMProject.contractID>))]
        [ActiveProjectOrContractBase(DisplayName = Messages.ATPTEFMMessages.ProjectID)]
        [PXDefault(typeof(PX.Objects.PM.NonProject), PersistingCheck = PXPersistingCheck.Nothing)]
        [PXUIVisible(typeof(FeatureInstalled<FeaturesSet.projectAccounting>))]
        public virtual int? ContractID { get; set; }
        public abstract class contractID : PX.Data.IBqlField { }
        #endregion

        #region ContractTaskID
        [PXForeignReference(typeof(Field<contractTaskID>.IsRelatedTo<PMTask.taskID>))]
        [PXDBInt]
        [PXDefault()]
        [PXUIRequired(typeof(Where<contractID, NotEqual<PX.Objects.PM.NonProject>>))]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.ProjectTaskid)]
        [PXSelector(typeof(Search<
            PMTask.taskID,
            Where<PMTask.projectID, Equal<Current<contractID>>>>), typeof(PMTask.taskCD), typeof(PMTask.description), DescriptionField = typeof(PMTask.description), SubstituteKey = typeof(PMTask.taskCD))]
        [PXUIVisible(typeof(FeatureInstalled<FeaturesSet.projectAccounting>))]
        public virtual int? ContractTaskID { get; set; }
        public abstract class contractTaskID : PX.Data.IBqlField { }
        #endregion

        #region CostCodeID
        [PXForeignReference(typeof(Field<costCodeID>.IsRelatedTo<PMCostCode.costCodeID>))]
        [CostCode(typeof(InventoryItem.cOGSAcctID), typeof(contractTaskID), AccountType.Expense, DisplayName = "Cost Code")]
        [PXUIVisible(typeof(FeatureInstalled<FeaturesSet.costCodes>))]
        public virtual Int32? CostCodeID { get; set; }
        public abstract class costCodeID : PX.Data.BQL.BqlInt.Field<costCodeID> { }
        #endregion

        #endregion*/
    }
}
