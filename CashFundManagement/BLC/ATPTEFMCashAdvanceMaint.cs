using CashFundManagement.DAC;
using PX.Data;
using PX.Data.EP;
using PX.Objects.EP;
using System;

namespace CashFundManagement.BLC {
    /// <remarks>
    /// 2025-02-10 : 009431 - CFM 2024R1: Cash Advance Primary List (Reopened)
    /// </remarks>
    public class ATPTEFMCashAdvanceMaint : PXGraph<ATPTEFMCashAdvanceMaint>
    {
        public ATPTEFMCashAdvanceMaint()
        {
            CashAdvance.View.IsReadOnly = true;
        }

        #region Views 

        public PXFilter<ATPTEFMCashAdvanceFilter> Filter;

        //[PXFilterable]
        //public PXSelect<
        //    ATPTEFMCashAdvance,
        //    Where<ATPTEFMCashAdvance.requestedByID, Equal<Current<ATPTEFMCashAdvanceFilter.employeeID>>,
        //        Or<Current2<ATPTEFMCashAdvanceFilter.employeeID>, IsNull>>,
        //    OrderBy<
        //        Desc<ATPTEFMCashAdvance.cashAdvanceNbr>>>
        //    CashAdvance;

        [PXFilterable]
        public PXSelectJoin<
            ATPTEFMCashAdvance,
            InnerJoin<EPEmployee,
                On<EPEmployee.bAccountID, Equal<ATPTEFMCashAdvance.requestedByID>>>,
            Where<Current2<ATPTEFMCashAdvanceFilter.employeeID>, IsNotNull,
                And<ATPTEFMCashAdvance.requestedByID, Equal<Current2<ATPTEFMCashAdvanceFilter.employeeID>>,
                Or<Current2<ATPTEFMCashAdvanceFilter.employeeID>, IsNull>>>,
            OrderBy<
                Desc<ATPTEFMCashAdvance.date>>>
            CashAdvance;

        public PXSelect<
            EPDepartment,
            Where<True, Equal<False>>>
            EPDepartment;

        #endregion

        #region Actions
        public PXCancel<ATPTEFMCashAdvanceFilter> Cancel;

        public PXAction<ATPTEFMCashAdvanceFilter> Insert;
		[PXInsertButton]
		[PXUIField(DisplayName = "")]
        [PXEntryScreenRights(typeof(ATPTEFMCashAdvance), nameof(ATPTEFMCashAdvanceEntry.Insert))]
        protected virtual void insert()
		{
			using (new PXPreserveScope())
			{
				ATPTEFMCashAdvanceEntry graph = (ATPTEFMCashAdvanceEntry)PXGraph.CreateInstance(typeof(ATPTEFMCashAdvanceEntry));
				graph.Clear(PXClearOption.ClearAll);
				ATPTEFMCashAdvance cashAdvance = (ATPTEFMCashAdvance)graph.CashAdvances.Cache.CreateInstance();
				graph.CashAdvances.Insert(cashAdvance);
				graph.CashAdvances.Cache.IsDirty = false;
				PXRedirectHelper.TryRedirect(graph, PXRedirectHelper.WindowMode.InlineWindow);
			}
		}

		public PXAction<ATPTEFMCashAdvanceFilter> EditDetail;
		[PXEditDetailButton]
		[PXUIField(DisplayName = "", MapEnableRights = PXCacheRights.Select)]
		protected virtual void editDetail()
		{
			ATPTEFMCashAdvance row = CashAdvance.Current;
			if (row == null) return;
			PXRedirectHelper.TryRedirect(this, row, PXRedirectHelper.WindowMode.InlineWindow);
		}
		#endregion

		#region Internal Type
		[Serializable]
        [PXHidden]
        public partial class ATPTEFMCashAdvanceFilter : CashFundManagement.DAC.Base.ATPTEFMBase, IBqlTable
        {
			#region EmployeeID

			[PXDBInt]
			[PXUIField(DisplayName = "Employee")]
			[PXSelector(typeof(Search<EPEmployee.bAccountID>),
				typeof(EPEmployee.acctCD),
				typeof(EPEmployee.acctName),
				SubstituteKey = typeof(EPEmployee.acctCD),
                DescriptionField = typeof(EPEmployee.acctName))]
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