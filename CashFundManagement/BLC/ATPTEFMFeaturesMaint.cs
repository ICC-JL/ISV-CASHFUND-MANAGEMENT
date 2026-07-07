using CashFundManagement.DAC.Setup;
using CashFundManagement.Helper;
using CashFundManagement.Messages;
using PX.Data;
using System.Linq;
using static CashFundManagement.Classes.ATPTEFMBudgetLibrary;
using PX.Objects.RQ;
using PX.Objects.Common.Extensions;
using CashFundManagement.Classes;

namespace CashFundManagement.BLC {
    public class ATPTEFMFeaturesMaint : PXGraph<ATPTEFMFeaturesMaint>
    {
        public PXCancel<ATPTEFMFeatures> Cancel;
        public PXSave<ATPTEFMFeatures> Save;

        public PXSelect<ATPTEFMFeatures> Setup;
        public PXSelect<ATPTEFMEnableDisable> EnableDisable;
        public PXSelect<ATPTEFMCASetup> CAPreference;
        public PXSelect<ATPTEFMSetup> FMPreference;


        public const string FEATURE_1 = "F1";
        public const string FEATURE_2 = "F2";
        public const string NOT_APPLICABLE = "NA";

        #region Events
        protected virtual void _(Events.FieldUpdated<ATPTEFMFeatures, ATPTEFMFeatures.budgetModules> e)
        {
            ATPTEFMFeatures row = e.Row;
            ATPTEFMCASetup caPreference = CAPreference.Select();
            ATPTEFMSetup fmPreference = FMPreference.Select();

            if (row == null) return;
            if (caPreference != null)
            {
                PXCache cache = this.Caches[typeof(ATPTEFMCASetup)];
                caPreference.AllowManualReceipts = BudgetVisible(Setup.Current, "C") ? false : caPreference.AllowManualReceipts;
                cache.Update(caPreference);
            }

            if (fmPreference != null)
            {
                PXCache fmCache = this.Caches[typeof(ATPTEFMSetup)];
                fmPreference.AllowManualReceipts = BudgetVisible(Setup.Current, "F") ? false : fmPreference.AllowManualReceipts;
                fmCache.Update(fmPreference);
            }
        }
        protected virtual void _(Events.FieldUpdated<ATPTEFMFeatures, ATPTEFMFeatures.projectBudgetModules> e)
        {
            ATPTEFMFeatures row = e.Row;
            ATPTEFMCASetup caPreference = CAPreference.Select();
            ATPTEFMSetup fmPreference = FMPreference.Select();

            if (row == null) return;
            if (caPreference != null)
            {
                PXCache cache = this.Caches[typeof(ATPTEFMCASetup)];
                caPreference.AllowManualReceipts = ATPTEFMProjectBudgetLibrary.ProjectBudgetVisible(Setup.Current, "C") ? false : caPreference.AllowManualReceipts;
                cache.Update(caPreference);
            }

            if (fmPreference != null)
            {
                PXCache fmCache = this.Caches[typeof(ATPTEFMSetup)];
                fmPreference.AllowManualReceipts = ATPTEFMProjectBudgetLibrary.ProjectBudgetVisible(Setup.Current, "F") ? false : fmPreference.AllowManualReceipts;
                fmCache.Update(fmPreference);
            }
        }
        protected virtual void _(Events.RowSelected<ATPTEFMFeatures> e)
        {
            ATPTEFMFeatures row = e.Row;
            if (row == null) return;

            bool containsRemovedCheckboxes = (row.BudgetModules?.Split(',').Contains("B") ?? false) || (row.BudgetModules?.Split(',').Contains("R") ?? false);

            if (containsRemovedCheckboxes)
                e.Cache.RaiseExceptionHandling<ATPTEFMFeatures.budgetModules>(row, null, ATPTEFMHelper.GetPropertyException(row, ATPTEFMMessages.SetupStillHasEnabledCheckboxesWhichAreAlreadyRemoved, PXErrorLevel.Warning));

            bool PbudgetContainsRemovedCheckboxes = (row.ProjectBudgetModules?.Split(',').Contains("B") ?? false) || (row.ProjectBudgetModules?.Split(',').Contains("R") ?? false);

            if (PbudgetContainsRemovedCheckboxes)
                e.Cache.RaiseExceptionHandling<ATPTEFMFeatures.projectBudgetModules>(row, null, ATPTEFMHelper.GetPropertyException(row, ATPTEFMMessages.SetupStillHasEnabledCheckboxesWhichAreAlreadyRemoved, PXErrorLevel.Warning));
        }
        #endregion
    }
}