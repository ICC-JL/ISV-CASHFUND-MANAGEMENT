using System;
using PX.Data;
using PX.Data.EP;
using PX.Objects.EP;
using CashFundManagement.DAC;

namespace CashFundManagement.BLC
{
	/// <remarks>
	/// 010193 - CFM 2024R1 - Funds Primary List ATPT2104
	/// </remarks>
	public class ATPTEFMFundsMaint : PXGraph<ATPTEFMFundsMaint>
	{
		public ATPTEFMFundsMaint()
		{
			Document.View.IsReadOnly = true;
		}

		#region Views 
		public PXFilter<ATPTEFMFundFilter> Filter;

		[PXFilterable]
		public PXSelectJoin<
			ATPTEFMFund,
			InnerJoin<EPEmployee,
				On<EPEmployee.bAccountID, Equal<ATPTEFMFund.custodianID>>>,
			Where<Current2<ATPTEFMFundFilter.employeeID>, IsNotNull,
				And<ATPTEFMFund.custodianID, Equal<Current2<ATPTEFMFundFilter.employeeID>>,
				Or<Current2<ATPTEFMFundFilter.employeeID>, IsNull>>>,
			OrderBy<
				Desc<ATPTEFMFund.documentDate>>>
			Document;

		#endregion

		#region Actions
		public PXCancel<ATPTEFMFundFilter> Cancel;

		public PXAction<ATPTEFMFundFilter> Insert;
		[PXInsertButton]
		[PXUIField(DisplayName = "")]
		[PXEntryScreenRights(typeof(ATPTEFMFund), nameof(ATPTEFMFundMaint.Insert))]
		protected virtual void insert()
		{
			using (new PXPreserveScope())
			{
				ATPTEFMFundMaint graph = (ATPTEFMFundMaint)PXGraph.CreateInstance(typeof(ATPTEFMFundMaint));
				graph.Clear(PXClearOption.ClearAll);
				ATPTEFMFund cashAdvance = (ATPTEFMFund)graph.Document.Cache.CreateInstance();
				graph.Document.Insert(cashAdvance);
				graph.Document.Cache.IsDirty = false;
				PXRedirectHelper.TryRedirect(graph, PXRedirectHelper.WindowMode.InlineWindow);
			}
		}

		public PXAction<ATPTEFMFundFilter> EditDetail;
		[PXEditDetailButton]
		[PXUIField(DisplayName = "", MapEnableRights = PXCacheRights.Select)]
		protected virtual void editDetail()
		{
			ATPTEFMFund row = Document.Current;
			if (row == null) return;
			PXRedirectHelper.TryRedirect(this, row, PXRedirectHelper.WindowMode.InlineWindow);
		}
		#endregion

		#region Internal Type
		[Serializable]
		[PXHidden]
		public partial class ATPTEFMFundFilter : CashFundManagement.DAC.Base.ATPTEFMBase, IBqlTable
		{
			#region EmployeeID

			[PXDBInt]
			[PXUIField(DisplayName = "Employee")]
			[PXSelector(typeof(Search<EPEmployee.bAccountID>),
				typeof(EPEmployee.acctCD),
				typeof(EPEmployee.acctName),
								DescriptionField = typeof(EPEmployee.acctName),
								SubstituteKey = typeof(EPEmployee.acctCD))]
			[PXDefault(typeof(Search<EPEmployee.bAccountID, Where<EPEmployee.userID, Equal<Current<AccessInfo.userID>>>>), PersistingCheck = PXPersistingCheck.Nothing)]
			[PXFieldDescription]
			public virtual Int32? EmployeeID { get; set; }
			public abstract class employeeID : PX.Data.BQL.BqlInt.Field<employeeID> { }

			#endregion
		}
		#endregion
	}
}
