using CashFundManagement.Attributes;
using PX.Data;
using PX.Data.BQL;
using PX.Data.ReferentialIntegrity.Attributes;
using PX.Objects.IN;
using PX.Objects.GL;
using PX.Objects.PM;
using System;

namespace CashFundManagement.DAC.Unbound {
    [Serializable]
    [PXCacheName(Messages.ATPTEFMMessages.ATPTEFMPBudget)]
    public class ATPTEFMPBudget : CashFundManagement.DAC.Base.ATPTEFMBase, IBqlTable
    {
        #region ProjectID
        [PXDBInt]
        [PXSelector(typeof(PMProject.contractID),
                typeof(PMProject.contractCD),
                typeof(PMProject.description),
                SubstituteKey = typeof(PMProject.contractCD),
                DescriptionField = typeof(PMProject.description))]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.ProjectID)]
        public virtual int? ProjectID { get; set; }
        public abstract class projectID : PX.Data.BQL.BqlInt.Field<projectID> { }
        #endregion

        #region ProjectTaskID
        [PXDBInt]
        [PXSelector(typeof(PMTask.taskID),
                    typeof(PMTask.taskCD),
                    typeof(PMTask.description),
                    SubstituteKey = typeof(PMTask.taskCD))]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.ProjectTaskID)]
        public virtual int? ProjectTaskID { get; set; }
        public abstract class projectTaskID : PX.Data.BQL.BqlInt.Field<projectTaskID> { }
        #endregion

        #region CostCodeID
        [CostCode(null, typeof(projectTaskID), "E", DisplayName = Messages.ATPTEFMMessages.CostCodeID)]
        //[PXUIField(DisplayName = Messages.ATPTEFMMessages.CostCodeID)]
        public virtual int? CostCodeID { get; set; }
        public abstract class costCodeID : PX.Data.BQL.BqlInt.Field<costCodeID> { }
        #endregion

        #region CuryID
        [PXDBString(5, IsUnicode = true)]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.CuryID)]
        public virtual string CuryID { get; set; }
        public abstract class curyID : BqlString.Field<curyID> { }
        #endregion

        #region InitialBudget
        [PXDecimal(2)]
        [PXDefault(TypeCode.Decimal, "0.0", PersistingCheck = PXPersistingCheck.Nothing)]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.InitialBudget)]
        public virtual decimal? InitialBudget { get; set; }
        public abstract class initialBudget : BqlDecimal.Field<initialBudget> { }
        #endregion

        #region DocAmt
        [PXDecimal(2)]
        [PXDefault(TypeCode.Decimal, "0.0", PersistingCheck = PXPersistingCheck.Nothing)]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.DocAmt)]
        public virtual decimal? DocAmt { get; set; }
        public abstract class docAmt : BqlDecimal.Field<docAmt> { }
        #endregion

        #region RequestAmt
        [PXDecimal(2)]
        [PXDefault(TypeCode.Decimal, "0.0", PersistingCheck = PXPersistingCheck.Nothing)]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.RequestAmt)]
        public virtual decimal? RequestAmt { get; set; }
        public abstract class requestAmt : BqlDecimal.Field<requestAmt> { }
        #endregion

        #region BudgetAmt
        [PXDecimal(2)]
        [PXDefault(TypeCode.Decimal, "0.0", PersistingCheck = PXPersistingCheck.Nothing)]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.BudgetAmt)]
        public virtual decimal? BudgetAmt { get; set; }
        public abstract class budgetAmt : BqlDecimal.Field<budgetAmt> { }
        #endregion

        #region SpentAmt
        [PXDecimal(2)]
        [PXDefault(TypeCode.Decimal, "0.0", PersistingCheck = PXPersistingCheck.Nothing)]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.SpentAmt)]
        public virtual decimal? SpentAmt { get; set; }
        public abstract class spentAmt : BqlDecimal.Field<spentAmt> { }
        #endregion

        #region ApprovedAmt
        [PXDecimal(2)]
        [PXDefault(TypeCode.Decimal, "0.0", PersistingCheck = PXPersistingCheck.Nothing)]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.ApprovedAmt)]
        public virtual decimal? ApprovedAmt { get; set; }
        public abstract class approvedAmt : BqlDecimal.Field<approvedAmt> { }
        #endregion

        #region ApprovedAdjAmt
        [PXDecimal(2)]
        [PXDefault(TypeCode.Decimal, "0.0", PersistingCheck = PXPersistingCheck.Nothing)]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.ApprovedAmt)]
        public virtual decimal? ApprovedAdjAmt { get; set; }
        public abstract class approvedAdjAmt : BqlDecimal.Field<approvedAdjAmt> { }
        #endregion

        #region CurrentApprovedAdjAmt
        [PXDecimal(2)]
        [PXDefault(TypeCode.Decimal, "0.0", PersistingCheck = PXPersistingCheck.Nothing)]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.ApprovedAmt)]
        public virtual decimal? CurrentApprovedAdjAmt { get; set; }
        public abstract class currentApprovedAdjAmt : BqlDecimal.Field<currentApprovedAdjAmt> { }
        #endregion

        #region UnapprovedAmt
        [PXDecimal(2)]
        [PXDefault(TypeCode.Decimal, "0.0", PersistingCheck = PXPersistingCheck.Nothing)]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.UnapprovedAmt)]
        public virtual decimal? UnapprovedAmt { get; set; }
        public abstract class unapprovedAmt : BqlDecimal.Field<unapprovedAmt> { }
        #endregion

        #region ReturnAmt
        [PXDecimal(2)]
        [PXDefault(TypeCode.Decimal, "0.0", PersistingCheck = PXPersistingCheck.Nothing)]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.ReturnAmt)]
        public virtual decimal? ReturnAmt { get; set; }
        public abstract class returnAmt : BqlDecimal.Field<returnAmt> { }
        #endregion

        #region CurrentReturnAmt
        [PXDecimal(2)]
        [PXDefault(TypeCode.Decimal, "0.0", PersistingCheck = PXPersistingCheck.Nothing)]
        public virtual decimal? CurrentReturnAmt { get; set; }
        public abstract class currentReturnAmt : BqlDecimal.Field<currentReturnAmt> { }
        #endregion

        #region ReturnAmtAdj
        [PXDecimal(2)]
        [PXDefault(TypeCode.Decimal, "0.0", PersistingCheck = PXPersistingCheck.Nothing)]
        public virtual decimal? ReturnAmtAdj { get; set; }
        public abstract class returnAmtAdj : BqlDecimal.Field<returnAmtAdj> { }
        #endregion

        #region CurrentReturnAmtAdj
        [PXDecimal(2)]
        [PXDefault(TypeCode.Decimal, "0.0", PersistingCheck = PXPersistingCheck.Nothing)]
        public virtual decimal? CurrentReturnAmtAdj { get; set; }
        public abstract class currentReturnAmtAdj : BqlDecimal.Field<currentReturnAmtAdj> { }
        #endregion

        #region POAmt		
        //[PXDecimal(2)]		
        //[PXDefault(TypeCode.Decimal, "0.0", PersistingCheck = PXPersistingCheck.Nothing)]		
        //[PXUIField(DisplayName = "PO Amount")]		
        //[PXUIVisible(typeof(Where<Current<RLFeatures.budgetPOAmount>, NotEqual<FEATURE_NA>>))]		
        //public virtual decimal? POAmt { get; set; }		
        //public abstract class pOAmt : BqlDecimal.Field<pOAmt> { }		
        #endregion

        #region Year
        [PXString(4)]
        public virtual string Year { get; set; }
        public abstract class year : PX.Data.BQL.BqlString.Field<year> { }
        #endregion

        #region FinPeriodID
        [PXString]
        public virtual string FinPeriodID { get; set; }
        public abstract class finPeriodID : BqlString.Field<finPeriodID> { }
        #endregion

        #region Origin
        [PXInt]
        public int? Origin { get; set; }
        public abstract class origin : BqlInt.Field<origin> { }
        #endregion

        #region RefNbr
        [PXString(15)]
        public virtual String RefNbr { get; set; }
        public abstract class refNbr : BqlString.Field<refNbr> { }
        #endregion

        #region IsApproved
        [PXBool]
        [PXDefault(false, PersistingCheck = PXPersistingCheck.Nothing)]
        public virtual bool? IsApproved { get; set; }
        public abstract class isApproved : BqlBool.Field<isApproved> { }
        #endregion

        #region IsReleased
        [PXBool]
        [PXDefault(false, PersistingCheck = PXPersistingCheck.Nothing)]
        public virtual bool? IsReleased { get; set; }
        public abstract class isReleased : BqlBool.Field<isReleased> { }
        #endregion

        #region AccountGroupID
        [ATPTEFMAccountGroupExt(typeof(Where<PMAccountGroup.isExpense, Equal<True>>), DisplayName = "Account Group")]
        [PXDefault(PersistingCheck = PXPersistingCheck.Nothing)]
        public virtual int? AccountGroupID { get; set; }
        public abstract class accountGroupID : PX.Data.BQL.BqlInt.Field<accountGroupID> { }
        #endregion

        //#region InventoryID
        //[PXInt]
        //[PXUIField(DisplayName = Messages.ATPTEFMMessages.InventoryID)]
        //[PXSelector(typeof(Search<
        //    InventoryItem.inventoryID, 
        //    Where<InventoryItem.itemType, Equal<INItemTypes.expenseItem>, 
        //        And<InventoryItem.itemStatus, Equal<InventoryItemStatus.active>>>>), SubstituteKey = typeof(InventoryItem.inventoryCD))]
        //public virtual int? InventoryID { get; set; }
        //public abstract class inventoryID : PX.Data.BQL.BqlInt.Field<inventoryID> { }
        //#endregion

        #region LedgerID
        [PXInt(IsKey = true)]
        [PXUnboundDefault]
        [PXSelector(typeof(Ledger.ledgerID),
                    typeof(Ledger.ledgerCD),
                    typeof(Ledger.descr),
                    SubstituteKey = typeof(Ledger.ledgerCD),
                    DescriptionField = typeof(Ledger.descr))]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.LedgerID, Enabled = true)]
        public virtual int? LedgerID { get; set; }
        public abstract class ledgerID : PX.Data.BQL.BqlInt.Field<ledgerID> { }
        #endregion
    }
}