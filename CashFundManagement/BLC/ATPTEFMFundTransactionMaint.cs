using CashFundManagement.DAC;
using PX.Data;
using PX.Data.EP;
using PX.Objects.EP;
using System;

namespace CashFundManagement.BLC
{
    /// <remarks>
    /// 2025-02-10 : 009432 - CFM 2024R1: Fund Transaction Primary List (Reopened)
    /// </remarks>
    public class ATPTEFMFundTransactionMaint : PXGraph<ATPTEFMFundTransactionMaint>
    {
        public ATPTEFMFundTransactionMaint()
        {
            FundTransaction.View.IsReadOnly = true;
        }

        #region Views 
        public PXFilter<ATPTEFMFundTransactionFilter> Filter;

        [PXFilterable]
        public PXSelectJoin<
            ATPTEFMFundTransaction,
            InnerJoin<EPEmployee,
                On<EPEmployee.bAccountID, Equal<ATPTEFMFundTransaction.requestedByID>>>,
            Where<Current2<ATPTEFMFundTransactionFilter.employeeID>, IsNotNull,
                And<ATPTEFMFundTransaction.requestedByID, Equal<Current2<ATPTEFMFundTransactionFilter.employeeID>>,
                Or<Current2<ATPTEFMFundTransactionFilter.employeeID>, IsNull>>>,
            OrderBy<
                Desc<ATPTEFMFundTransaction.date>>>
            FundTransaction;

        public PXSelect<
            EPDepartment,
            Where<True, Equal<False>>>
            EPDepartment;

        #endregion

        #region Actions
        public PXCancel<ATPTEFMFundTransactionFilter> Cancel;

        public PXAction<ATPTEFMFundTransactionFilter> Insert;
        [PXInsertButton]
        [PXUIField(DisplayName = "")]
        [PXEntryScreenRights(typeof(ATPTEFMFundTransaction), nameof(ATPTEFMFundTransactionEntry.Insert))]
        protected virtual void insert()
        {
            using (new PXPreserveScope())
            {
                ATPTEFMFundTransactionEntry graph = (ATPTEFMFundTransactionEntry)PXGraph.CreateInstance(typeof(ATPTEFMFundTransactionEntry));
                graph.Clear(PXClearOption.ClearAll);
                ATPTEFMFundTransaction cashAdvance = (ATPTEFMFundTransaction)graph.FundTransactions.Cache.CreateInstance();
                graph.FundTransactions.Insert(cashAdvance);
                graph.FundTransactions.Cache.IsDirty = false;
                PXRedirectHelper.TryRedirect(graph, PXRedirectHelper.WindowMode.InlineWindow);
            }
        }

        public PXAction<ATPTEFMFundTransactionFilter> EditDetail;
        [PXEditDetailButton]
        [PXUIField(DisplayName = "", MapEnableRights = PXCacheRights.Select)]
        protected virtual void editDetail()
        {
            ATPTEFMFundTransaction row = FundTransaction.Current;
            if (row == null) return;
            PXRedirectHelper.TryRedirect(this, row, PXRedirectHelper.WindowMode.InlineWindow);
        }
        #endregion

        #region Internal Type
        [Serializable]
        [PXHidden]
        public partial class ATPTEFMFundTransactionFilter : CashFundManagement.DAC.Base.ATPTEFMBase, IBqlTable
        {
            #region EmployeeID

            [PXDBInt]
            [PXUIField(DisplayName = "Employee")]
            [PXSelector(typeof(Search<EPEmployee.bAccountID>),
                typeof(EPEmployee.acctCD),
                typeof(EPEmployee.acctName),
                DescriptionField = typeof(EPEmployee.acctName),
                SubstituteKey = typeof(EPEmployee.acctCD))]
            [PXDefault(typeof(Search<
                EPEmployee.bAccountID,
                Where<EPEmployee.userID, Equal<Current<AccessInfo.userID>>>>), PersistingCheck = PXPersistingCheck.Nothing)]
            [PXFieldDescription]
            public virtual Int32? EmployeeID { get; set; }
            public abstract class employeeID : PX.Data.BQL.BqlInt.Field<employeeID> { }

            #endregion
        }
        #endregion

        #region Cached Attached
        [PXMergeAttributes(Method = MergeMethod.Merge)]
        [PXRemoveBaseAttribute(typeof(PXUIFieldAttribute))]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.DepartmentID, Visibility = PXUIVisibility.SelectorVisible)]
        protected virtual void EPDepartment_Description_CacheAttached(PXCache sender) { }
        #endregion
    }
}
