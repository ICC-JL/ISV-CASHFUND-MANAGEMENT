using CashFundManagement.Attributes;
using CashFundManagement.Classes;
using CashFundManagement.DAC;
using CashFundManagement.DAC.Setup;
using CashFundManagement.Helper;
using CashFundManagement.Messages;
using PX.Caching;
using PX.Data;
using PX.Metadata;
using PX.Objects.CA;
using PX.Objects.CR;
using PX.Objects.EP;
using System.Collections;
using System.Linq;
using static CashFundManagement.Classes.ATPTEFMBudgetLibrary;

namespace CashFundManagement.BLC
{
    [PXCacheName(Messages.ATPTEFMMessages.ATPTEFMFundTransactionPreference)]
    public class ATPTEFMFundTransactionPreference : PXGraph<ATPTEFMFundTransactionPreference>
    {
        #region  Views    
        public PXSelect<ATPTEFMSetup> Preference;
        public PXSelect<CASetup> CASetupRecord;
        public PXSelect<ATPTEFMFundTransactionSetupApproval> FundTransactionApproval;
        public PXSelect<ATPTEFMMonthEndSetupApproval> MonthEndApproval;
        public PXSelect<ATPTEFMReplenishmentReportSettings> ReplenishmentReport;
        public PXSelect<ATPTEFMFundsApprovalSetup> FundsApproval;
        public PXSelect<ATPTEFMReplenishmentSetupApproval> ReplenishmentApproval;
        public PXSetup<ATPTEFMFeatures> FeatureSetup;
        public PXSetup<EPSetup> EPSetup;
        #endregion

        #region Action
        public PXCancel<ATPTEFMSetup> Cancel;
        public PXSave<ATPTEFMSetup> Save;

        /// <remarks>
        /// 04-20-2026: 015991 - Add new validation to approval map ID + Recode hyperlink navigation. {JCL} <br/>
        /// </remarks>
        public PXAction<ATPTEFMSetup> viewAssignmentMap;
        [PXButton]
        [PXUIField(DisplayName = "", MapEnableRights = PXCacheRights.Select, MapViewRights = PXCacheRights.Select, Visible = false)]
        public virtual IEnumerable ViewAssignmentMap(PXAdapter adapter)
        {
            int? assignmentMapID = null;
            
            switch (Preference.Current?.ApprovalModule)
            {
                case ATPTEFMApprovalModuleAttribute.Funds:
                    assignmentMapID = (FundsApproval.Current as IAssignedMap)?.AssignmentMapID;
                    break;
                case ATPTEFMApprovalModuleAttribute.Replenishment:
                    assignmentMapID = (ReplenishmentApproval.Current as IAssignedMap)?.AssignmentMapID;
                    break;
                case ATPTEFMApprovalModuleAttribute.FundTransaction:
                    assignmentMapID = (FundTransactionApproval.Current as IAssignedMap)?.AssignmentMapID;
                    break;
                case ATPTEFMApprovalModuleAttribute.MonthEnd:
                    assignmentMapID = (MonthEndApproval.Current as IAssignedMap)?.AssignmentMapID;
                    break;
            }

            if (assignmentMapID != null)
            {
                EPApprovalMapMaint graph = PXGraph.CreateInstance<EPApprovalMapMaint>();
                graph.AssigmentMap.Current = graph.AssigmentMap.Search<EPAssignmentMap.assignmentMapID>(assignmentMapID);
                
                if (graph.AssigmentMap.Current != null)
                    throw new PXRedirectRequiredException(graph, true, ATPTEFMMessages.AssignmentMapID);
            }

            return adapter.Get();
        }
        #endregion

        #region Prefetch
        // Acuminator disable once PX1076 CallToInternalApi [Use in PXDatabase.GetSlot]
        [InjectDependency]
        protected ICacheControl<PageCache> PageCacheControl { get; set; }
        // Acuminator disable once PX1076 CallToInternalApi [Use in PXDatabase.GetSlot]
        [InjectDependency]
        protected IScreenInfoCacheControl ScreenInfoCacheControl { get; set; }
        #endregion

        #region Events

        protected virtual void _(Events.RowUpdating<ATPTEFMFundsApprovalSetup> e)
        {
            ATPTEFMFundsApprovalSetup row = e.NewRow;
            if (row == null) return;

            if (IsDuplicateApprovalMap(row))
            {

                e.Cache.RaiseExceptionHandling<ATPTEFMFundsApprovalSetup.assignmentMapID>(row, row.AssignmentMapID,
                    new PXSetPropertyException<ATPTEFMFundsApprovalSetup.assignmentMapID>(ATPTEFMMessages.DuplicateApprovalMapType));
            }
        }
        
        protected virtual void _(Events.RowInserting<ATPTEFMFundsApprovalSetup> e)
        {
            ATPTEFMFundsApprovalSetup row = e.Row;
            if (row == null) return;

            if (IsDuplicateApprovalMap(row))
            {
                e.Cache.RaiseExceptionHandling<ATPTEFMFundsApprovalSetup.assignmentMapID>(row, row.AssignmentMapID,
                    new PXSetPropertyException<ATPTEFMFundsApprovalSetup.assignmentMapID>(ATPTEFMMessages.DuplicateApprovalMapType));
            }
        }

        private bool IsDuplicateApprovalMap(ATPTEFMFundsApprovalSetup row)
        {
            return FundsApproval.Select().RowCast<ATPTEFMFundsApprovalSetup>()
                .Any(s => s.ApprovalID != row.ApprovalID && 
                          s.FundType == row.FundType && 
                          s.AssignmentMapID == row.AssignmentMapID);
        }

        protected virtual void _(Events.FieldUpdated<ATPTEFMSetup, ATPTEFMSetup.fundsApprovalSetup> e)
        {
            foreach (ATPTEFMFundsApprovalSetup setup in FundsApproval.Select())
            {
                setup.IsActive = (bool)e.NewValue;
                FundsApproval.Update(setup);
            }   
        }

        protected virtual void _(Events.FieldUpdated<ATPTEFMSetup, ATPTEFMSetup.fundTransactionRequestApproval> e)
        {
            foreach (ATPTEFMFundTransactionSetupApproval setup in FundTransactionApproval.Select())
            {
                setup.IsActive = (bool)e.NewValue;
                FundTransactionApproval.Update(setup);
            }
        }

        protected virtual void _(Events.FieldUpdated<ATPTEFMSetup, ATPTEFMSetup.replenishmentRequestApproval> e)
        {
            foreach (ATPTEFMReplenishmentSetupApproval setup in ReplenishmentApproval.Select())
            {
                setup.IsActive = (bool)e.NewValue;
                ReplenishmentApproval.Update(setup);
            }
        }
        protected virtual void _(Events.FieldUpdated<ATPTEFMSetup, ATPTEFMSetup.monthEndRequestApproval> e)
        {
            foreach (ATPTEFMMonthEndSetupApproval setup in MonthEndApproval.Select())
            {
                setup.IsActive = (bool)e.NewValue;
                MonthEndApproval.Update(setup);
            }
        }
        protected virtual void _(Events.RowPersisting<ATPTEFMSetup> e)
        {
            ATPTEFMSetup row = e.Row;
            EPSetup epSetup = EPSetup.Select();

            if (row.RequireExternalReferenceNbr == false && epSetup.RequireRefNbrInExpenseReceipts == true)
                this.Preference.Cache.RaiseExceptionHandling<ATPTEFMSetup.requireExternalReferenceNbr>(row, row.RequireExternalReferenceNbr,
                                   ATPTEFMHelper.GetPropertyException(row, Messages.ATPTEFMMessages.WarningMsgForRequireRefNbr, PXErrorLevel.Error));

            if(row.NoOfDaysToLiquidate == 0)
                this.Preference.Cache.RaiseExceptionHandling<ATPTEFMSetup.noOfDaysToLiquidate>(row, row.NoOfDaysToLiquidate,
                                       ATPTEFMHelper.GetPropertyException(row, Messages.ATPTEFMMessages.NoOfDaysToLiquidateShouldBeGreaterThanZero, PXErrorLevel.Error));
        }

        protected virtual void _(Events.RowSelected<ATPTEFMSetup> e)
        {
            ATPTEFMSetup row = e.Row;
            if (row == null) return;

            EPSetup te = EPSetup.Current;
            bool IsBudgetOn = BudgetVisible(FeatureSetup.Current, "F") || ATPTEFMProjectBudgetLibrary.ProjectBudgetVisible(FeatureSetup.Current, "F");
            PXUIFieldAttribute.SetEnabled<ATPTEFMSetup.allowManualReceipts>(e.Cache, null, IsBudgetOn ? false : true);
            if (IsBudgetOn)
                PXUIFieldAttribute.SetWarning<ATPTEFMSetup.allowManualReceipts>(e.Cache, row, ATPTEFMMessages.WarningMessageForAllowManualReceipt);

            PXUIFieldAttribute.SetEnabled<ATPTEFMSetup.requireVendorDetails>(e.Cache, null, !IsTERequireVendorDetails(te));
            if (IsTERequireVendorDetails(te))
                PXUIFieldAttribute.SetWarning<ATPTEFMSetup.requireVendorDetails>(e.Cache, row, ATPTEFMMessages.WarningMessageForRequireVendorDetails);

            ShowView(row.ApprovalModule);

            ReplenishmentReport.AllowSelect = false;
        }
        #endregion

        #region Method
        public virtual bool IsTERequireVendorDetails(EPSetup te)
        {
            return false;
        }
        public virtual void ShowView(string module)
        {
            FundTransactionApproval.AllowSelect = module == ATPTEFMApprovalModuleAttribute.FundTransaction;
            FundsApproval.AllowSelect = module == ATPTEFMApprovalModuleAttribute.Funds;
            ReplenishmentApproval.AllowSelect = module == ATPTEFMApprovalModuleAttribute.Replenishment;
            MonthEndApproval.AllowSelect = module == ATPTEFMApprovalModuleAttribute.MonthEnd;
        }
        public override void Persist()
        {
            base.Persist();
            PXDatabase.ResetSlots();
            // Acuminator disable once PX1076 CallToInternalApi [Use in PXDatabase.GetSlot]
            PageCacheControl.InvalidateCache();
            // Acuminator disable once PX1076 CallToInternalApi [Use in PXDatabase.GetSlot]
            ScreenInfoCacheControl.InvalidateCache();
        }
        #endregion
    }
}