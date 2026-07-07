using PX.Data;
using System.Collections.Generic;
using System.Linq;

namespace CashFundManagement.BLC {

    public class ATPTEFMBudgetEntry : PXGraph<ATPTEFMBudgetEntry>
    {
        #region Views
        public PXSelect<DAC.ATPTEFMBudgetHistory> HistoryView;
        public PXSelect<DAC.ATPTEFMProjectBudgetHistory> ProjectHistoryView;
        public PXSetup<DAC.Setup.ATPTEFMFeatures> FeatureSetup;
        #endregion

        #region Methods
        public void AddBudgetHistory(List<DAC.Unbound.ATPTEFMBudget> BudgetList)
        {
            if (!BudgetList.Any() || (FeatureSetup?.Current?.BudgetFeatureSet?.Split(',')?.Contains("F1") ?? false) == false) return;

            Classes.ATPTEFMBudgetLibrary.DeleteBudgetHistory(HistoryView, BudgetList, true);
            this.Actions.PressSave();

            Classes.ATPTEFMBudgetLibrary.AddBudgetHistory(HistoryView, BudgetList);
            this.Actions.PressSave();
        }

        public void DeleteBudgetHistory(List<DAC.Unbound.ATPTEFMBudget> BudgetList)
        {
            if (!BudgetList.Any() || (FeatureSetup?.Current?.BudgetFeatureSet?.Split(',')?.Contains("F1") ?? false) == false) return;
            Classes.ATPTEFMBudgetLibrary.DeleteBudgetHistory(HistoryView, BudgetList, true);
            this.Actions.PressSave();
        }

        public void AddPBudgetHistory(List<DAC.Unbound.ATPTEFMPBudget> BudgetList)
        {
            if (!BudgetList.Any() || (FeatureSetup?.Current?.ProjectBudgetFeatureSet?.Split(',')?.Contains("F1") ?? false) == false) return;

            Classes.ATPTEFMProjectBudgetLibrary.DeleteProjectBudgetHistory(ProjectHistoryView, BudgetList, true);
            this.Actions.PressSave();

            Classes.ATPTEFMProjectBudgetLibrary.AddProjectBudgetHistory(ProjectHistoryView, BudgetList);
            this.Actions.PressSave();
        }

        public void DeletePBudgetHistory(List<DAC.Unbound.ATPTEFMPBudget> BudgetList)
        {
            if (!BudgetList.Any() || (FeatureSetup?.Current?.ProjectBudgetFeatureSet?.Split(',')?.Contains("F1") ?? false) == false) return;
            Classes.ATPTEFMProjectBudgetLibrary.DeleteProjectBudgetHistory(ProjectHistoryView, BudgetList, true);
            this.Actions.PressSave();
        }
        #endregion
    }

}