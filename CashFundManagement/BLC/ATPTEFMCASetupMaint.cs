using CashFundManagement.Classes;
using CashFundManagement.DAC.Setup;
using CashFundManagement.Helper;
using CashFundManagement.Messages;
using PX.Caching;
using PX.Data;
using PX.Metadata;
using PX.Objects.EP;
using System;
using System.Linq;
using System.Reflection;
using static CashFundManagement.Classes.ATPTEFMBudgetLibrary;

namespace CashFundManagement.BLC {
    /// <remarks>
    /// 010220 - (CFM24R1/24R2) RFP>Expense Claim: PR or PO error 'invalid processing of document' upon release of the bill, where the bill generated has a financial entry from a bill with 'On Hold' status.
    /// </remarks>
    public class ATPTEFMCASetupMaint : PXGraph<ATPTEFMCASetupMaint>
    {
        #region  Views    
        public PXSelect<ATPTEFMCASetup> Preference;
        public PXSelect<ATPTEFMCASetupApproval> CashAdvanceApproval;

        public PXSelect<EPEmployee> EmployeeCashAdvance;
        public PXSetup<ATPTEFMFeatures> FeatureSetup;
        public PXSetup<EPSetup> EPSetup;
        //public PXSelect<EFM.Setup.ATPTEFMRepSetupApproval> ReplenishmentApproval;
        //public PXSelectJoin<EFM.Setup.ATPTEFMSetupEmployeeCA, InnerJoin<BAccount, On<BAccount.bAccountID, Equal<EFM.Setup.ATPTEFMSetupEmployeeCA.employeeID>>>> EmployeeCashAdvance;
        #endregion

        #region Action
        public PXCancel<ATPTEFMCASetup> Cancel;
        public PXSave<ATPTEFMCASetup> Save;

        public PXAction<ATPTEFMCASetup> GetInfo;
        [PXButton]
        [PXUIField(DisplayName = "Get Info")]
        protected void getInfo()
        {

            const string dllName = "CashFundManagement";

            AppDomain domain = AppDomain.CurrentDomain;
            Assembly assembly = domain.GetAssemblies().Where(w => w.GetName().Name.Contains(dllName)).FirstOrDefault();

            if (assembly != null)
            {
                string title = assembly.GetCustomAttribute<AssemblyTitleAttribute>().Title;
                string description = assembly.GetCustomAttribute<AssemblyDescriptionAttribute>().Description;
                string version = assembly.GetCustomAttribute<AssemblyFileVersionAttribute>().Version;

                System.IO.FileInfo info = new System.IO.FileInfo(assembly.Location);
                string buildDate = string.Format("{0} {1}", info.LastWriteTime.ToShortDateString(), info.LastWriteTime.ToShortTimeString());

                string message = string.Format("{0}" + Environment.NewLine +
                    "{1}" + Environment.NewLine +
                    "{2}" + Environment.NewLine +
                    "{3}" + Environment.NewLine, title, description, version, buildDate);


                Preference.Ask(message, MessageButtons.OK);
            }
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
        //TODO : Transfer logic to field updated event
        protected virtual void ATPTEFMCASetup_CashAdvanceRequestApproval_FieldUpdating(PXCache sender, PXFieldUpdatingEventArgs e)
        {
            PXCache cache = this.Caches[typeof(ATPTEFMCASetupApproval)];
            PXResultset<ATPTEFMCASetupApproval> setups = PXSelect<ATPTEFMCASetupApproval>.Select(sender.Graph, null);
            foreach (ATPTEFMCASetupApproval setup in setups)
            {
                setup.IsActive = (bool?)e.NewValue;
                cache.Update(setup);

            }

        }

        protected virtual void _(Events.FieldUpdated<ATPTEFMCASetup, ATPTEFMCASetup.isRequireApprovalCashAdvanceBill> e)
        {
            ATPTEFMCASetup row = e.Row;

            if (row == null) return;

            if (row.IsRequireApprovalCashAdvanceBill ?? false)
            {
                row.AutoReleaseAP = false;
            }
        }

        protected virtual void _(Events.RowSelected<ATPTEFMCASetup> e)
        {
            ATPTEFMCASetup row = e.Row;

            if (row == null) return;

            EPSetup te = EPSetup.Current;
            bool IsBudgetOn = BudgetVisible(FeatureSetup.Current, "C") || ATPTEFMProjectBudgetLibrary.ProjectBudgetVisible(FeatureSetup.Current, "C");

            PXUIFieldAttribute.SetEnabled<ATPTEFMCASetup.allowManualReceipts>(e.Cache, null, IsBudgetOn ? false : true);
            if (IsBudgetOn)
                PXUIFieldAttribute.SetWarning<ATPTEFMCASetup.allowManualReceipts>(e.Cache, row, ATPTEFMMessages.WarningMessageForAllowManualReceipt);

            PXUIFieldAttribute.SetEnabled<ATPTEFMCASetup.requireVendorDetails>(e.Cache, null, !IsTERequireVendorDetails(te));
            if (IsTERequireVendorDetails(te))
                PXUIFieldAttribute.SetWarning<ATPTEFMCASetup.requireVendorDetails>(e.Cache, row, ATPTEFMMessages.WarningMessageForRequireVendorDetails);

            PXUIFieldAttribute.SetEnabled<ATPTEFMCASetup.isRequireApprovalLiquidationBill>(e.Cache, null, !te.AutomaticReleaseAP ?? false);
            if (te.AutomaticReleaseAP ?? false)
                PXUIFieldAttribute.SetWarning<ATPTEFMCASetup.isRequireApprovalLiquidationBill>(e.Cache, row, ATPTEFMMessages.WarningMessageForCARequireLiquidationBillApproval);
        }

        protected virtual void _(Events.RowPersisting<ATPTEFMCASetup> e)
        {
            ATPTEFMCASetup row = e.Row;
            EPSetup epSetup = EPSetup.Select();

            if (row.RequireExtRef == false && epSetup.RequireRefNbrInExpenseReceipts == true)
                this.Preference.Cache.RaiseExceptionHandling<ATPTEFMCASetup.requireExtRef>(row, row.RequireExtRef,
                                      ATPTEFMHelper.GetPropertyException(row, Messages.ATPTEFMMessages.WarningMsgForRequireRefNbr, PXErrorLevel.Error));
        }

        #endregion

        #region Methods
        public virtual bool IsTERequireVendorDetails(EPSetup te)
        {
            return false;
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