using CashFundManagement.DAC;
using CashFundManagement.Messages;
using PX.Data;
using PX.Objects.CS;
using PX.Objects.PM;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashFundManagement.BLC
{
    public class ATPTEFMProjectBudgetProcess : PXGraph<ATPTEFMProjectBudgetProcess>
    {
        #region Views + ctor

        public PXCancel<ATPTEFMProjectBudgetLineSummary> Cancel;

        [PXFilterable]
        public PXProcessing<ATPTEFMProjectBudgetLineSummary,
            Where<ATPTEFMProjectBudgetLineSummary.released, Equal<False>,
                And<ATPTEFMProjectBudgetLineSummary.amount, Equal<ATPTEFMProjectBudgetLineSummary.distributedAmount>>>
            > Summary;

        public ATPTEFMProjectBudgetProcess()
        {
            Summary.SetProcessDelegate(ReleaseDoc);
            Summary.SetProcessCaption(ATPTEFMMessages.Release);
            Summary.SetProcessAllCaption(ATPTEFMMessages.ReleaseAll);
            Summary.SetSelected<ATPTEFMProjectBudgetLineSummary.selected>();
        }

        #endregion

        #region View Delegates

        public virtual IEnumerable summary()
        {
            var List = new List<ATPTEFMProjectBudgetLineSummary>();
            var Articles = PXSelect<ATPTEFMProjectBudgetLineSummary,
                Where<ATPTEFMProjectBudgetLineSummary.released, Equal<False>,
                    And<ATPTEFMProjectBudgetLineSummary.amount, Equal<ATPTEFMProjectBudgetLineSummary.distributedAmount>,
                    And<ATPTEFMProjectBudgetLineSummary.amount, Greater<decimal0>>>>
                >.Select(this).RowCast<ATPTEFMProjectBudgetLineSummary>().ToList();

            foreach (ATPTEFMProjectBudgetLineSummary item in Articles)
            {
                var CostCodeList = PXSelect<PMCostBudget,
                    Where<PMCostBudget.projectTaskID, Equal<Required<PMCostBudget.projectTaskID>>,
                        And<PMCostBudget.costCodeID, Equal<Required<PMCostBudget.costCodeID>>>>
                    >.Select(this, item.ProjectTaskID, item.CostCodeID);
                if ((CostCodeList?.Count ?? null) == 0) continue;
                List.Add(item);
            }

            return List;
        }

        #endregion

        #region Methods

        public static void ReleaseDoc(ATPTEFMProjectBudgetLineSummary summary)
        {
            PXGraph.CreateInstance<ATPTEFMProjectBudgetEntry>().ReleaseSummary(summary);
        }

        #endregion
    }
}
