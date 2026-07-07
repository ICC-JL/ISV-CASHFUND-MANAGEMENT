using System;
using PX.Data;
using PX.Data.EP;
using PX.Objects.EP;
using CashFundManagement.DAC;

namespace CashFundManagement.BLC
{
	/// <remarks>
	/// 2025-02-10 : 009433 - CFM 2024R1: Replenishment Primary List (Reopened)
	/// </remarks>
	public class ATPTEFMReplenishmentMaint : PXGraph<ATPTEFMReplenishmentMaint>
	{
		public ATPTEFMReplenishmentMaint()
		{
			Replenishment.View.IsReadOnly = true;
		}

		#region Views 
		public PXFilter<ATPTEFMReplenishmentFilter> Filter;

		[PXFilterable]
		public PXSelectJoin<
				ATPTEFMReplenishment,
				InnerJoin<EPEmployee,
						On<EPEmployee.bAccountID, Equal<ATPTEFMReplenishment.custodianID>>>,
				Where<Current2<ATPTEFMReplenishmentFilter.employeeID>, IsNotNull,
						And<ATPTEFMReplenishment.custodianID, Equal<Current2<ATPTEFMReplenishmentFilter.employeeID>>,
						Or<Current2<ATPTEFMReplenishmentFilter.employeeID>, IsNull>>>,
				OrderBy<
						Desc<ATPTEFMReplenishment.date>>>
				Replenishment;

		public PXSelect<
				EPDepartment,
				Where<True, Equal<False>>>
				EPDepartment;

		#endregion

		#region Actions
		public PXCancel<ATPTEFMReplenishmentFilter> Cancel;

		public PXAction<ATPTEFMReplenishmentFilter> Insert;
		[PXInsertButton]
		[PXUIField(DisplayName = "")]
		[PXEntryScreenRights(typeof(ATPTEFMReplenishment), nameof(ATPTEFMReplenishmentEntry.Insert))]
		protected virtual void insert()
		{
			using (new PXPreserveScope())
			{
				ATPTEFMReplenishmentEntry graph = (ATPTEFMReplenishmentEntry)PXGraph.CreateInstance(typeof(ATPTEFMReplenishmentEntry));
				graph.Clear(PXClearOption.ClearAll);
				ATPTEFMReplenishment cashAdvance = (ATPTEFMReplenishment)graph.Replenishments.Cache.CreateInstance();
				graph.Replenishments.Insert(cashAdvance);
				graph.Replenishments.Cache.IsDirty = false;
				PXRedirectHelper.TryRedirect(graph, PXRedirectHelper.WindowMode.InlineWindow);
			}
		}

		public PXAction<ATPTEFMReplenishmentFilter> EditDetail;
		[PXEditDetailButton]
		[PXUIField(DisplayName = "", MapEnableRights = PXCacheRights.Select)]
		protected virtual void editDetail()
		{
			ATPTEFMReplenishment row = Replenishment.Current;
			if (row == null) return;
			PXRedirectHelper.TryRedirect(this, row, PXRedirectHelper.WindowMode.InlineWindow);
		}
		#endregion

		#region Internal Type
		[Serializable]
		[PXHidden]
		public partial class ATPTEFMReplenishmentFilter : CashFundManagement.DAC.Base.ATPTEFMBase, IBqlTable
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
