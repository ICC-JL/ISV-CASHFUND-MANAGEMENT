using CashFundManagement.BLC;
using CashFundManagement.DAC;
using CashFundManagement.Helper;
using CashFundManagement.Messages;
using PX.Data;
using PX.Objects.AP;
using PX.Objects.PM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashFundManagement.Extensions.BLC
{
    /// <remarks>
    /// 2024-08-14 : Adds multi-tenant support. {RRS}
    /// </remarks>
    public class ATPTEFMProjectEntryExtension : PXGraphExtension<ProjectEntry>
    {
#if Version23R2
        public static bool IsActive() => true;
#else
        public static bool IsActive() => ATPTEFMPrefetchSetup.IsActive;
#endif

        #region Views
        public PXSelect<ATPTEFMProjectBudgetLineSummary,
                                    Where<ATPTEFMProjectBudgetLineSummary.projectID, Equal<Required<ATPTEFMProjectBudgetLineSummary.projectID>>,
                                        And<ATPTEFMProjectBudgetLineSummary.projectTaskID, Equal<Required<ATPTEFMProjectBudgetLineSummary.projectTaskID>>,
                                        And<ATPTEFMProjectBudgetLineSummary.costCodeID, Equal<Required<ATPTEFMProjectBudgetLineSummary.costCodeID>>,
                                        And<ATPTEFMProjectBudgetLineSummary.accountGroupID, Equal<Required<ATPTEFMProjectBudgetLineSummary.accountGroupID>>>>>>> ATPTEFMProjectBudgetLine;
        #endregion

        #region Events
        protected virtual void _(Events.RowUpdating<PMCostBudget> e, PXRowUpdating baseEvent)
        {
            if (baseEvent != null) baseEvent(e.Cache, e.Args);

            PMCostBudget row = e.Row;
            if (row == null) return;

            bool releasedWithAmt = false;
            foreach (ATPTEFMProjectBudgetLineSummary line in ATPTEFMProjectBudgetLine.Select(row.ProjectID, row.ProjectTaskID, row.CostCodeID, row.AccountGroupID))
            {
                if ((line.ReleasedAmount ?? 0m) > 0m)
                {
                    releasedWithAmt = true;
                    break;
                }
            }
            if (releasedWithAmt)
            {
                throw new PXException(ATPTEFMMessages.ProjectBudgetCannotBeEdited, PXErrorLevel.Error);
            }
        }
        protected virtual void _(Events.RowDeleting<PMCostBudget> e, PXRowDeleting baseMethod)
        {
            if (baseMethod != null) baseMethod(e.Cache, e.Args);

            PMCostBudget row = e.Row;
            if (row == null) return;

            bool releasedWithAmt = false;
            foreach (ATPTEFMProjectBudgetLineSummary line in ATPTEFMProjectBudgetLine.Select(row.ProjectID, row.ProjectTaskID, row.CostCodeID, row.AccountGroupID))
            {
                if ((line.ReleasedAmount ?? 0m) > 0m)
                {
                    releasedWithAmt = true;
                    break;
                }
            }
            if (releasedWithAmt)
            {
                throw new PXException(ATPTEFMMessages.ProjectBudgetWasDeleteMessage, PXErrorLevel.Error);
            }
        }
        protected virtual void _(Events.RowDeleted<PMCostBudget> e, PXRowDeleted baseMethod)
        {
            if (baseMethod != null) baseMethod(e.Cache, e.Args);

            PMCostBudget row = e.Row;
            if (row == null) return;

            foreach (ATPTEFMProjectBudgetLineSummary line in ATPTEFMProjectBudgetLine.Select(row.ProjectID, row.ProjectTaskID, row.CostCodeID, row.AccountGroupID))
            {
                ATPTEFMProjectBudgetLine.Delete(line);
            }
        }
        #endregion
    }
}
