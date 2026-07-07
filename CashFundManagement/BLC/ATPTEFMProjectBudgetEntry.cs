using System;
using System.Collections;
using System.Collections.Generic;
using PX.Data;
using CashFundManagement.DAC;
using System.Linq;
using CashFundManagement.Messages;
using CashFundManagement.Attributes;
using CashFundManagement.MethodExtensions;
using PX.Objects.GL;
using PX.Objects.GL.Descriptor;
using PX.Objects.GL.FinPeriods.TableDefinition;
using PX.Objects.PM;
using CashFundManagement.Helper;
using PX.Objects.CN.ProjectAccounting.PM.Descriptor;
using System.Diagnostics;
using PX.Data.BQL;

namespace CashFundManagement.BLC
{
    public class ATPTEFMProjectBudgetEntry : PXGraph<ATPTEFMProjectBudgetEntry>
    {
    // Block access when Cash Fund Management is disabled in Preferences (ATPT3105)
    public PXSetup<CashFundManagement.DAC.Setup.ATPTEFMCASetup> CASetupPreferences;
        /// <summary>
        /// ATPT3105
        /// </summary>
        #region Views + ctor
        public PXSelect<ATPTEFMProjectBudget> ProjectBudget;
        public PXSelect<ATPTEFMProjectBudgetLine> ProjectBudgetLine;
        [PXImport(typeof(ATPTEFMProjectBudget))]
        public PXSelectOrderBy<ATPTEFMProjectBudgetLineSummary,
            OrderBy<Asc<ATPTEFMProjectBudgetLineSummary.sortOrder>>> Summary;

        public PXFilter<ATPTEFMProjectBudgetFilter> ProjectFilter;
        public PXFilter<ATPTEFMBudgetDistributeFilter> DistrFilter;
        public PXFilter<ATPTEFMPreloadBudgetFilter> PreloadFilter;
        #endregion

        public ATPTEFMProjectBudgetEntry()
        {
#if !Version23R2
            if (!(CASetupPreferences?.Current?.EnableCFM ?? false))
            {
                throw new PXException(ATPTEFMMessages.CashFundManagementFeatureDisabled);
            }
#endif
        }

        #region View Delegates
        public virtual IEnumerable summary()
        {
            var list = new List<ATPTEFMProjectBudgetLineSummary>();
            var Articles = PXSelect<ATPTEFMProjectBudgetLineSummary,
                            Where<ATPTEFMProjectBudgetLineSummary.finYear, Equal<Current<ATPTEFMProjectBudgetFilter.finYear>>,
                                And<ATPTEFMProjectBudgetLineSummary.ledgerID, Equal<Current<ATPTEFMProjectBudgetFilter.ledgerID>>,
                                And<ATPTEFMProjectBudgetLineSummary.projectID, Equal<Current<ATPTEFMProjectBudgetFilter.projectID>>>>>,
                            OrderBy<Asc<ATPTEFMProjectBudgetLineSummary.projectTaskID>>>.Select(this);
            int SortOrder = 0;

            foreach (ATPTEFMProjectBudgetLineSummary item in Articles)
            {
                if (item.Deleted ?? false) continue;
                item.SortOrder = SortOrder++;
                list.Add(item);

                if (ProjectFilter.Current.CompareFinYear != null &&
                   ProjectFilter.Current.CompareLedgerID != null &&
                   ProjectFilter.Current.CompareProjectID != null &&
                   (item.IsCompare == null || item.IsCompare == false))
                {
                    ATPTEFMProjectBudgetLineSummary PrevArticle =
                        PXSelect<ATPTEFMProjectBudgetLineSummary,
                        Where<ATPTEFMProjectBudgetLineSummary.finYear, Equal<Required<ATPTEFMProjectBudgetLineSummary.finYear>>,
                            And<ATPTEFMProjectBudgetLineSummary.ledgerID, Equal<Required<ATPTEFMProjectBudgetLineSummary.ledgerID>>,
                            And<ATPTEFMProjectBudgetLineSummary.projectID, Equal<Required<ATPTEFMProjectBudgetLineSummary.projectID>>,
                            And<ATPTEFMProjectBudgetLineSummary.projectTaskID, Equal<Required<ATPTEFMProjectBudgetLineSummary.projectTaskID>>,
                            And<ATPTEFMProjectBudgetLineSummary.costCodeID, Equal<Required<ATPTEFMProjectBudgetLineSummary.costCodeID>>,
                            And<ATPTEFMProjectBudgetLineSummary.released, Equal<Required<ATPTEFMProjectBudgetLineSummary.released>>,
                            And<ATPTEFMProjectBudgetLineSummary.accountGroupID, Equal<Required<ATPTEFMProjectBudgetLineSummary.accountGroupID>>>>>>>>>
                        >.Select(this,
                            ProjectFilter.Current.CompareFinYear,
                            ProjectFilter.Current.CompareLedgerID,
                            ProjectFilter.Current.CompareProjectID,
                            item.ProjectTaskID,
                            item.CostCodeID,
                            true,
                            item.AccountGroupID).ToList().SingleOrDefault();

                    var cRow = new ATPTEFMProjectBudgetLineSummary()
                    {
                        IsCompare = true,
                        CompareProjectTaskID = item.ProjectTaskID,
                        CompareCostCodeID = item.CostCodeID,
                        Description = "Compared",
                        Amount = PrevArticle != null ? PrevArticle.Amount : 0,
                        DistributedAmount = PrevArticle != null ? PrevArticle.Amount : 0,
                        FinPeriod01 = PrevArticle != null ? PrevArticle.FinPeriod01 : 0,
                        FinPeriod02 = PrevArticle != null ? PrevArticle.FinPeriod02 : 0,
                        FinPeriod03 = PrevArticle != null ? PrevArticle.FinPeriod03 : 0,
                        FinPeriod04 = PrevArticle != null ? PrevArticle.FinPeriod04 : 0,
                        FinPeriod05 = PrevArticle != null ? PrevArticle.FinPeriod05 : 0,
                        FinPeriod06 = PrevArticle != null ? PrevArticle.FinPeriod06 : 0,
                        FinPeriod07 = PrevArticle != null ? PrevArticle.FinPeriod07 : 0,
                        FinPeriod08 = PrevArticle != null ? PrevArticle.FinPeriod08 : 0,
                        FinPeriod09 = PrevArticle != null ? PrevArticle.FinPeriod09 : 0,
                        FinPeriod10 = PrevArticle != null ? PrevArticle.FinPeriod10 : 0,
                        FinPeriod11 = PrevArticle != null ? PrevArticle.FinPeriod11 : 0,
                        FinPeriod12 = PrevArticle != null ? PrevArticle.FinPeriod12 : 0,
                        SortOrder = SortOrder++
                    };

                    Summary.Cache.SetStatus(cRow, PXEntryStatus.Held);
                    list.Add(cRow);
                }
            }

            return list;
        }
        #endregion

        #region Actions
        public PXSave<ATPTEFMProjectBudgetFilter> Save;
        public PXCancel<ATPTEFMProjectBudgetFilter> Cancel;
        public PXDelete<ATPTEFMProjectBudgetFilter> Delete;

        public PXAction<ATPTEFMProjectBudgetFilter> First;
        public PXAction<ATPTEFMProjectBudgetFilter> Prev;
        public PXAction<ATPTEFMProjectBudgetFilter> Next;
        public PXAction<ATPTEFMProjectBudgetFilter> WNext;
        public PXAction<ATPTEFMProjectBudgetFilter> Last;

        public PXAction<ATPTEFMProjectBudgetFilter> Distribute;
        public PXAction<ATPTEFMProjectBudgetFilter> DistributeOK;
        public PXAction<ATPTEFMProjectBudgetFilter> PreloadArticles;
        public PXAction<ATPTEFMProjectBudgetFilter> PreloadArticlesOK;
        public PXAction<ATPTEFMProjectBudgetFilter> PreloadArticlesNext;
        #endregion

        #region ActionDelegate
        [PXUIField(DisplayName = ActionsMessages.Cancel, MapEnableRights = PXCacheRights.Select, MapViewRights = PXCacheRights.Select)]
        [PXCancelButton]
        public virtual IEnumerable cancel(PXAdapter adapter)
        {
            //ATPTEFMProjectBudgetFilter oldfilter = ProjectFilter.Current;
            //oldfilter.CompareFinYear = null;
            //oldfilter.CompareLedgerID = null;
            //oldfilter.CompareProjectID = null;

            Clear();
            //ProjectFilter.Cache.RestoreCopy(ProjectFilter.Current, oldfilter);
            return adapter.Get();
        }

        [PXUIField(DisplayName = ActionsMessages.Delete, MapEnableRights = PXCacheRights.Select, MapViewRights = PXCacheRights.Select)]
        [PXDeleteButton]
        public virtual IEnumerable delete(PXAdapter adapter)
        {
            bool deleteAllowed = true;
            PXResultset<ATPTEFMProjectBudgetLineSummary> budget =
                PXSelect<ATPTEFMProjectBudgetLineSummary,
                Where<ATPTEFMProjectBudgetLineSummary.finYear, Equal<Current<ATPTEFMProjectBudgetFilter.finYear>>,
                    And<ATPTEFMProjectBudgetLineSummary.ledgerID, Equal<Current<ATPTEFMProjectBudgetFilter.ledgerID>>,
                    And<ATPTEFMProjectBudgetLineSummary.projectID, Equal<Current<ATPTEFMProjectBudgetFilter.projectID>>>>>>.Select(this);

            foreach (ATPTEFMProjectBudgetLineSummary line in budget)
            {
                if (line.Released.Value == true)
                {
                    deleteAllowed = false;
                    break;
                }
            }
            if (deleteAllowed)
            {
                ProjectFilter.Current.IsDelete = true;

                foreach (ATPTEFMProjectBudgetLineSummary line in budget)
                {
                    Summary.Delete(line);
                }

                this.Save.Press();
                Clear();
            }
            else
            {
                Summary.Ask(ATPTEFMMessages.ProjectBudgetDeleteTitle, ATPTEFMMessages.ProjectBudgetDeleteMessage, MessageButtons.OK);
            }
            return adapter.Get();
        }

        [PXUIField(DisplayName = ActionsMessages.First, MapEnableRights = PXCacheRights.Select, MapViewRights = PXCacheRights.Select)]
        [PXFirstButton(ConfirmationMessage = null)]
        public virtual IEnumerable first(PXAdapter adapter)
        {
            if (!Summary.Cache.IsDirty)
            {
                ATPTEFMProjectBudget article = PXSelectGroupBy<ATPTEFMProjectBudget,
                    Where<ATPTEFMProjectBudget.ledgerID, Equal<Current<ATPTEFMProjectBudgetFilter.ledgerID>>,
                    And<ATPTEFMProjectBudget.projectID, Equal<Current<ATPTEFMProjectBudgetFilter.projectID>>>>,
                    Aggregate<Min<ATPTEFMProjectBudget.finYear>>>
                    .Select(this);
                if (article != null)
                {
                    ProjectFilter.Current.FinYear = article.FinYear;
                    ProjectFilter.Update(ProjectFilter.Current);
                }
            }
            else
            {
                Summary.Ask(ATPTEFMMessages.BudgetPendingChangesTitle, ATPTEFMMessages.BudgetPendingChangesMessage, MessageButtons.OK);
            }
            return adapter.Get();
        }

        [PXUIField(DisplayName = ActionsMessages.Previous, MapEnableRights = PXCacheRights.Select, MapViewRights = PXCacheRights.Select)]
        [PXPreviousButton(ConfirmationMessage = null)]
        public virtual IEnumerable prev(PXAdapter adapter)
        {
            if (!Summary.Cache.IsDirty)
            {
                ATPTEFMProjectBudget article = PXSelectGroupBy<ATPTEFMProjectBudget,
                    Where<ATPTEFMProjectBudget.ledgerID, Equal<Current<ATPTEFMProjectBudgetFilter.ledgerID>>,
                    And<ATPTEFMProjectBudget.projectID, Equal<Current<ATPTEFMProjectBudgetFilter.projectID>>,
                    And<ATPTEFMProjectBudget.finYear, Less<Current<ATPTEFMProjectBudgetFilter.finYear>>>>>,
                    Aggregate<Max<ATPTEFMProjectBudget.finYear>>>
                    .Select(this);
                if (article != null)
                {
                    if (article.FinYear != null)
                    {
                        ProjectFilter.Current.FinYear = article.FinYear;
                        ProjectFilter.Update(ProjectFilter.Current);
                    }
                }
            }
            else
            {
                Summary.Ask(ATPTEFMMessages.BudgetPendingChangesTitle, ATPTEFMMessages.BudgetPendingChangesMessage, MessageButtons.OK);
            }
            return adapter.Get();
        }

        [PXUIField(DisplayName = ActionsMessages.Next, MapEnableRights = PXCacheRights.Select, MapViewRights = PXCacheRights.Select)]
        [PXNextButton(ConfirmationMessage = null)]
        public virtual IEnumerable next(PXAdapter adapter)
        {
            if (!Summary.Cache.IsDirty)
            {
                ATPTEFMProjectBudget article = PXSelectGroupBy<ATPTEFMProjectBudget,
                    Where<ATPTEFMProjectBudget.ledgerID, Equal<Current<ATPTEFMProjectBudgetFilter.ledgerID>>,
                    And<ATPTEFMProjectBudget.projectID, Equal<Current<ATPTEFMProjectBudgetFilter.projectID>>,
                    And<ATPTEFMProjectBudget.finYear, Greater<Required<ATPTEFMProjectBudgetFilter.finYear>>>>>,
                    Aggregate<Min<ATPTEFMProjectBudget.finYear>>>
                    .Select(this, ProjectFilter.Current.FinYear ?? "");
                if (article != null)
                {
                    if (article.FinYear != null)
                    {
                        ProjectFilter.Current.FinYear = article.FinYear;
                        ProjectFilter.Update(ProjectFilter.Current);
                    }
                }
            }
            else
            {
                Summary.Ask(ATPTEFMMessages.BudgetPendingChangesTitle, ATPTEFMMessages.BudgetPendingChangesMessage, MessageButtons.OK);
            }
            return adapter.Get();
        }

        [PXUIField(DisplayName = ActionsMessages.Last, MapEnableRights = PXCacheRights.Select, MapViewRights = PXCacheRights.Select)]
        [PXLastButton(ConfirmationMessage = null)]
        public virtual IEnumerable last(PXAdapter adapter)
        {
            if (!Summary.Cache.IsDirty)
            {
                ATPTEFMProjectBudget article = PXSelectGroupBy<ATPTEFMProjectBudget,
                    Where<ATPTEFMProjectBudget.ledgerID, Equal<Current<ATPTEFMProjectBudgetFilter.ledgerID>>,
                    And<ATPTEFMProjectBudget.projectID, Equal<Current<ATPTEFMProjectBudgetFilter.projectID>>>>,
                    Aggregate<Max<ATPTEFMProjectBudget.finYear>>>
                    .Select(this);
                if (article != null)
                {
                    ProjectFilter.Current.FinYear = article.FinYear;
                    ProjectFilter.Update(ProjectFilter.Current);
                }
            }
            else
            {
                Summary.Ask(ATPTEFMMessages.BudgetPendingChangesTitle, ATPTEFMMessages.BudgetPendingChangesMessage, MessageButtons.OK);
            }
            return adapter.Get();
        }

        [PXUIField(DisplayName = ATPTEFMMessages.Distribute, MapEnableRights = PXCacheRights.Select, MapViewRights = PXCacheRights.Select)]
        [PXButton]
        public virtual void distribute()
        {
            DistrFilter.AskExt();
        }

        [PXUIField(DisplayName = "Load", MapEnableRights = PXCacheRights.Select, MapViewRights = PXCacheRights.Select)]
        [PXButton]
        public virtual void distributeOK()
        {
            ATPTEFMBudgetDistributeFilter filter = DistrFilter.Current;

            foreach (ATPTEFMProjectBudgetLineSummary sum in Summary.Select())
            {
                ATPTEFMProjectBudgetLineSummary Detail = filter.ApplyToAll == true ? sum : Summary.Current;
                ATPTEFMProjectBudgetLineSummary DetailCompare = null;

                if (sum.IsCompare == true || Detail == null) continue; // || sum.Released == true

                if (filter.Method == ATPTEFMBudgetDistributionMethodAttribute.ComparedValuesVal)
                {
                    DetailCompare = Summary.Select().FirstTableItems.ToList().
                        Where(x => x.IsCompare == true
                                && x.CompareProjectTaskID == Detail.ProjectTaskID
                                && x.CompareCostCodeID == Detail.CostCodeID).FirstOrDefault();
                }
                else if (filter.Method == ATPTEFMBudgetDistributionMethodAttribute.PreviousYearVal)
                {
                    DetailCompare = PXSelect<ATPTEFMProjectBudgetLineSummary,
                        Where<ATPTEFMProjectBudgetLineSummary.finYear, Equal<Required<ATPTEFMProjectBudgetLineSummary.finYear>>,
                        And<ATPTEFMProjectBudgetLineSummary.ledgerID, Equal<Required<ATPTEFMProjectBudgetLineSummary.ledgerID>>,
                        And<ATPTEFMProjectBudgetLineSummary.projectID, Equal<Required<ATPTEFMProjectBudgetLineSummary.projectID>>,
                        And<ATPTEFMProjectBudgetLineSummary.projectTaskID, Equal<Required<ATPTEFMProjectBudgetLineSummary.projectTaskID>>,
                        And<ATPTEFMProjectBudgetLineSummary.costCodeID, Equal<Required<ATPTEFMProjectBudgetLineSummary.costCodeID>>,
                        And<ATPTEFMProjectBudgetLineSummary.released, Equal<Required<ATPTEFMProjectBudgetLineSummary.released>>>>>>>>
                        >.Select(this,
                            (int.Parse(ProjectFilter.Current.FinYear) - 1).ToString(),
                            ProjectFilter.Current.LedgerID,
                            ProjectFilter.Current.ProjectID,
                            Detail.ProjectTaskID,
                            Detail.CostCodeID,
                            true).FirstOrDefault();
                }

                DistributeSummary(ref Detail, filter.Method, DetailCompare ?? Detail);

                Summary.Cache.Update(Detail);

                if (filter.ApplyToAll == false) break;
            }
        }

        [PXUIField(DisplayName = ATPTEFMMessages.PreloadArticles)]
        [PXButton]
        public virtual void preloadArticles()
        {
            PreloadFilter.AskExt();
        }

        [PXUIField(DisplayName = "Load")]
        [PXButton]
        public virtual void preloadArticlesOK()
        {
            ATPTEFMPreloadBudgetFilter filter = PreloadFilter.Current;

            PXResultset<ATPTEFMProjectBudgetLineSummary> PreviouBudgetArticles =
                PXSelect<ATPTEFMProjectBudgetLineSummary,
                    Where<ATPTEFMProjectBudgetLineSummary.finYear, Equal<Required<ATPTEFMProjectBudgetLineSummary.finYear>>,
                        And<ATPTEFMProjectBudgetLineSummary.ledgerID, Equal<Required<ATPTEFMProjectBudgetLineSummary.ledgerID>>,
                        And<ATPTEFMProjectBudgetLineSummary.projectID, Equal<Required<ATPTEFMProjectBudgetLineSummary.projectID>>,
                        And<ATPTEFMProjectBudgetLineSummary.released, Equal<Required<ATPTEFMProjectBudgetLineSummary.released>>>>>>
                >.Select<PXResultset<ATPTEFMProjectBudgetLineSummary>>(this, filter.FinYear, filter.LedgerID, filter.ProjectID, true);

            foreach (ATPTEFMProjectBudgetLineSummary sum in PreviouBudgetArticles)
            {
                var PreviousBudgetArticle = Summary.Select().FirstTableItems.ToList()
                    .Where(x => x.ProjectTaskID == sum.ProjectTaskID && x.CostCodeID == sum.CostCodeID).FirstOrDefault();

                if ((filter.PreloadAction == (short)PreloadActionTypes.UpdateOnly && PreviousBudgetArticle == null) ||
                    (filter.PreloadAction == (short)PreloadActionTypes.LoadOnly && PreviousBudgetArticle != null))
                    continue;

                var Detail = PreviousBudgetArticle;
                if (PreviousBudgetArticle == null)
                {
                    Detail = sum;
                    Detail.Released = false;
                    Detail.WasReleased = false;
                }
                Detail.FinYear = ProjectFilter.Current.FinYear;
                Detail.Amount = sum.Amount * (filter.Mutliplier / 100);
                DistributeSummary(ref Detail, Detail.BudgetDistributionMethod);

                Summary.Cache.Update(Detail);
            }
        }

        [PXButton]
        [PXUIField(MapEnableRights = PXCacheRights.Update, Visible = false)]
        public virtual IEnumerable preloadArticlesNext(PXAdapter adapter)
        {
            //bool HasError = false;
            if (PreloadFilter.Current.LedgerID == null)
            {
                PreloadFilter.Cache.RaiseExceptionHandling<ATPTEFMPreloadBudgetFilter.ledgerID>(PreloadFilter.Current, PreloadFilter.Current.LedgerID,
                    ATPTEFMHelper.GetPropertyException(PreloadFilter.Current, ErrorMessages.FieldIsEmpty, PXErrorLevel.RowError));
                //HasError = true;
            }
            if (PreloadFilter.Current.FinYear == null)
            {
                PreloadFilter.Cache.RaiseExceptionHandling<ATPTEFMPreloadBudgetFilter.finYear>(PreloadFilter.Current, PreloadFilter.Current.FinYear,
                    ATPTEFMHelper.GetPropertyException(PreloadFilter.Current, ErrorMessages.FieldIsEmpty, PXErrorLevel.RowError));
                // HasError = true;
            }
            if (PreloadFilter.Current.ProjectID == null)
            {
                PreloadFilter.Cache.RaiseExceptionHandling<ATPTEFMPreloadBudgetFilter.projectID>(PreloadFilter.Current, PreloadFilter.Current.ProjectID,
                    ATPTEFMHelper.GetPropertyException(PreloadFilter.Current, ErrorMessages.FieldIsEmpty, PXErrorLevel.RowError));
                //  HasError = true;
            }
            if (PreloadFilter.Current.Mutliplier == null)
            {
                PreloadFilter.Cache.RaiseExceptionHandling<ATPTEFMPreloadBudgetFilter.mutliplier>(PreloadFilter.Current, PreloadFilter.Current.Mutliplier,
                    ATPTEFMHelper.GetPropertyException(PreloadFilter.Current, ErrorMessages.FieldIsEmpty, PXErrorLevel.RowError));
                //HasError = true;
            }
            return adapter.Get();
        }
        #endregion

        #region Events
        public override void Persist()
        {
            var row = ProjectFilter.Current;
            ATPTEFMProjectBudget item = PXSelect<ATPTEFMProjectBudget,
                    Where<ATPTEFMProjectBudget.finYear, Equal<Current<ATPTEFMProjectBudgetFilter.finYear>>,
                        And<ATPTEFMProjectBudget.ledgerID, Equal<Current<ATPTEFMProjectBudgetFilter.ledgerID>>,
                        And<ATPTEFMProjectBudget.projectID, Equal<Current<ATPTEFMProjectBudgetFilter.projectID>>>>>>
                    .SelectWindowed(this, 0, 1);

            if (row.IsDelete == true)
            {
                item = ProjectBudget.Delete(item);
            }
            else
            {
                if (item == null)
                {
                    item = new ATPTEFMProjectBudget()
                    {
                        LedgerID = row.LedgerID,
                        FinYear = row.FinYear,
                        ProjectID = row.ProjectID
                    };
                }
                item = ProjectBudget.Update(item);
            }

            foreach (ATPTEFMProjectBudgetLineSummary line in Summary.Select())
            {
                if (Summary.Cache.GetStatus(line) == PXEntryStatus.Inserted)
                {
                    ATPTEFMProjectBudgetLineSummary exist = PXSelect<
                        ATPTEFMProjectBudgetLineSummary,
                        Where<ATPTEFMProjectBudgetLineSummary.projectID, Equal<Required<ATPTEFMProjectBudgetLineSummary.projectID>>,
                            And<ATPTEFMProjectBudgetLineSummary.projectTaskID, Equal<Required<ATPTEFMProjectBudgetLineSummary.projectTaskID>>,
                            And<ATPTEFMProjectBudgetLineSummary.costCodeID, Equal<Required<ATPTEFMProjectBudgetLineSummary.costCodeID>>,
                            And<ATPTEFMProjectBudgetLineSummary.accountGroupID, Equal<Required<ATPTEFMProjectBudgetLineSummary.accountGroupID>>,
                            And<ATPTEFMProjectBudgetLineSummary.finYear, Equal<Required<ATPTEFMProjectBudgetLineSummary.finYear>>,
                            And<ATPTEFMProjectBudgetLineSummary.ledgerID, Equal<Required<ATPTEFMProjectBudgetLineSummary.ledgerID>>,
                            And<ATPTEFMProjectBudgetLineSummary.deleted, Equal<True>>>>>>>>>
                        .Select(this, line.ProjectID, line.ProjectTaskID, line.CostCodeID, line.AccountGroupID, row.FinYear, row.LedgerID);

                    if (exist != null) Summary.Delete(exist);
                }
            }

            CheckDuplicates();

            base.Persist();
        }

        protected virtual void ATPTEFMProjectBudgetFilter_RowSelected(PXCache sender, PXRowSelectedEventArgs e)
        {
            bool _IsActive = HasNull(ProjectFilter.Current.FinYear, ProjectFilter.Current.LedgerID, ProjectFilter.Current.ProjectID);
            //Cancel.SetEnabled(Summary.Cache.IsDirty || ProjectFilter.Cache.IsDirty);
            Delete.SetEnabled(!_IsActive);

            First.SetEnabled(!_IsActive);
            Prev.SetEnabled(!_IsActive);
            Next.SetEnabled(!_IsActive);
            Last.SetEnabled(!_IsActive);

            PreloadArticles.SetEnabled(!_IsActive);
            Distribute.SetEnabled(!_IsActive);
            ProjectBudget.AllowUpdate = !_IsActive;
            ProjectBudgetLine.AllowInsert = !_IsActive;
            ProjectBudgetLine.AllowUpdate = !_IsActive;
            ProjectBudgetLine.AllowDelete = !_IsActive;
            Summary.AllowInsert = !_IsActive;
            Summary.AllowUpdate = !_IsActive;
            Summary.AllowDelete = !_IsActive;
        }

        //protected virtual void ATPTEFMProjectBudgetLineSummary_RowSelected(PXCache sender, PXRowSelectedEventArgs e)
        //{
        //    var row = (ATPTEFMProjectBudgetLineSummary)e.Row;

        //    if (row == null) return;

        //    PXUIFieldAttribute.SetEnabled(sender, row, !((row.IsCompare ?? true) || (row.Released ?? true)));
        //}

        protected virtual void ATPTEFMProjectBudgetLineSummary_RowDeleting(PXCache sender, PXRowDeletingEventArgs e)
        {
            var row = (ATPTEFMProjectBudgetLineSummary)e.Row;

            if (row == null) return;

            if ((row.ReleasedAmount ?? 0m) > 0m)
                throw new PXException(ATPTEFMMessages.ProjectBudgetWasDeleteMessage, PXErrorLevel.Error);
        }

        protected virtual void ATPTEFMProjectBudgetLineSummary_RowPersisting(PXCache sender, PXRowPersistingEventArgs e)
        {
            var row = (ATPTEFMProjectBudgetLineSummary)e.Row;
            //if (this.Accessinfo.ScreenID == "RL.30.40.00") row.Released = false;

            //if((row.ProjectTaskID != oldRow.ProjectTaskID || row.AccountGroupID != oldRow.AccountGroupID || row.CostCodeID != oldRow.CostCodeID || row.InventoryID != oldRow.InventoryID) && sender.GetStatus(row) == PXEntryStatus.Updated)
            //    TransactDetail(row, oldRow, ActionTypes.InsertUpdate);
            if (sender.GetStatus(row) == PXEntryStatus.Updated)
                TransactDetail(row, ActionTypes.Update);
            else if (sender.GetStatus(row) == PXEntryStatus.Inserted)
                TransactDetail(row, ActionTypes.Insert);
            else if (sender.GetStatus(row) == PXEntryStatus.Deleted)
            {
                TransactDetail(row, ActionTypes.Delete);
                if (!row.Deleted ?? false)
                {
                    row.Deleted = true;
                    Summary.Cache.SetStatus(row, PXEntryStatus.Updated);
                    Summary.Cache.Update(row);
                }
            }
        }

        //protected virtual void ATPTEFMProjectBudgetLineSummary_RowDeleted(PXCache sender, PXRowDeletedEventArgs e)
        //{
        //    var row = (ATPTEFMProjectBudgetLineSummary)e.Row;
        //    TransactDetail(row, ActionTypes.Delete);

        //    if (!row.Deleted ?? false)
        //    {
        //        row.Deleted = true;
        //        Summary.Cache.SetStatus(row, PXEntryStatus.Updated);
        //        Summary.Cache.Update(row);
        //    }
        //}

        protected virtual void ATPTEFMProjectBudgetFilter_RowUpdated(PXCache sender, PXRowUpdatedEventArgs e)
        {
            var row = (ATPTEFMProjectBudgetFilter)e.Row;

            if (row.ProjectID != null)
            {
                foreach (PMCostBudget costBudget in PXSelect<
                    PMCostBudget,
                    Where<PMCostBudget.projectID, Equal<Required<PMCostBudget.projectID>>,
                    And<PMCostBudget.type, Equal<AccountType.expense>>>>
                    .Select(this, row.ProjectID))
                {
                    PMTask task = PMTask.PK.Find(this, costBudget.ProjectID, costBudget.ProjectTaskID);

                    if (task.Type == ProjectTaskType.Cost || task.Type == ProjectTaskType.CostRevenue)
                    {
                        ATPTEFMProjectBudgetLineSummary exist = PXSelect<
                            ATPTEFMProjectBudgetLineSummary,
                            Where<ATPTEFMProjectBudgetLineSummary.projectID, Equal<Required<ATPTEFMProjectBudgetLineSummary.projectID>>,
                                And<ATPTEFMProjectBudgetLineSummary.projectTaskID, Equal<Required<ATPTEFMProjectBudgetLineSummary.projectTaskID>>,
                                And<ATPTEFMProjectBudgetLineSummary.costCodeID, Equal<Required<ATPTEFMProjectBudgetLineSummary.costCodeID>>,
                                And<ATPTEFMProjectBudgetLineSummary.accountGroupID, Equal<Required<ATPTEFMProjectBudgetLineSummary.accountGroupID>>,
                                And<ATPTEFMProjectBudgetLineSummary.finYear, Equal<Required<ATPTEFMProjectBudgetLineSummary.finYear>>,
                                And<ATPTEFMProjectBudgetLineSummary.ledgerID, Equal<Required<ATPTEFMProjectBudgetLineSummary.ledgerID>>>>>>>>>
                            .Select(this, costBudget.ProjectID, costBudget.ProjectTaskID, costBudget.CostCodeID, costBudget.AccountGroupID, row.FinYear, row.LedgerID);

                        if (exist == null)
                        {
                            ATPTEFMProjectBudgetLineSummary article = new ATPTEFMProjectBudgetLineSummary();
                            article.ProjectTaskID = costBudget.ProjectTaskID;
                            article.CostCodeID = costBudget.CostCodeID;
                            article.AccountGroupID = costBudget.AccountGroupID;

                            Summary.Cache.Insert(article);
                        }
                    }
                }
            }
        }

        protected virtual void ATPTEFMProjectBudgetFilter_LedgerID_FieldUpdated(PXCache sender, PXFieldUpdatedEventArgs e)
        {
            var row = (ATPTEFMProjectBudgetFilter)e.Row;
            if (row == null) return;
            if (row.CompareLedgerID == null) row.CompareLedgerID = row.LedgerID;
        }

        protected virtual void ATPTEFMProjectBudgetFilter_ProjectID_FieldUpdated(PXCache sender, PXFieldUpdatedEventArgs e)
        {
            var row = (ATPTEFMProjectBudgetFilter)e.Row;
            if (row == null) return;
            if (row.CompareProjectID == null) row.CompareProjectID = row.ProjectID;
        }
        protected virtual void ATPTEFMProjectBudgetFilter_IsActive_FieldUpdated(PXCache sender, PXFieldUpdatedEventArgs e)
        {
            var row = (ATPTEFMProjectBudgetFilter)e.Row;
            if (row == null) return;
            Summary.Cache.IsDirty = true;
        }
        protected virtual void _(Events.RowUpdated<ATPTEFMProjectBudgetLineSummary> e)
        {
            ATPTEFMProjectBudgetLineSummary row = e.Row;
            ATPTEFMProjectBudgetLineSummary oldrow = e.OldRow;
            if (row == null || oldrow == null) return;

            if ((row.Released ?? false) && (oldrow.Amount != row.Amount || oldrow.DistributedAmount != row.DistributedAmount))
            {
                row.Released = false;
            }
                
        }
        #endregion

        #region Methods
        public static bool HasNull(params object[] values)
        {
            return values.ToList().Contains(null);
        }

        private void TransactDetail(ATPTEFMProjectBudgetLineSummary row, ActionTypes _Action)
        {
            decimal?[] _Periods = {
                row.FinPeriod01, row.FinPeriod02, row.FinPeriod03, row.FinPeriod04, row.FinPeriod05, row.FinPeriod06,
                row.FinPeriod07, row.FinPeriod08, row.FinPeriod09, row.FinPeriod10, row.FinPeriod11, row.FinPeriod12
            };
            switch (_Action)
            {
                case ActionTypes.Insert:
                    for (int i = 0; i < 12; i++)
                    {
                        ATPTEFMProjectBudgetLine Diteyl = new ATPTEFMProjectBudgetLine()
                        {
                            FinYear = row.FinYear,
                            LedgerID = row.LedgerID,
                            ProjectID = row.ProjectID,
                            FinPeriodID = string.Format("{0}{1}", ProjectFilter.Current.FinYear, (i + 1).ToString("D2")),
                            ProjectTaskID = row.ProjectTaskID,
                            CostCodeID = row.CostCodeID,
                            AccountGroupID = row.AccountGroupID,
                            Amount = _Periods[i],
                            ReleasedAmount = row.Released == true ? _Periods[i] : 0,
                            Released = row.Released,
                            WasReleased = row.WasReleased
                        };
                        Diteyl = ProjectBudgetLine.Insert(Diteyl);
                        ProjectBudgetLine.Cache.Persist(PXDBOperation.Insert);
                    }
                    break;
                case ActionTypes.Update:
                    for (int i = 0; i < 12; i++)
                    {
                        string finperiod = string.Format("{0}{1}", ProjectFilter.Current.FinYear, (i + 1).ToString("D2"));

                        ATPTEFMProjectBudgetLine exDiteyl =
                            PXSelect<ATPTEFMProjectBudgetLine,
                            Where<ATPTEFMProjectBudgetLine.finYear, Equal<Current<ATPTEFMProjectBudgetFilter.finYear>>,
                                And<ATPTEFMProjectBudgetLine.ledgerID, Equal<Current<ATPTEFMProjectBudgetFilter.ledgerID>>,
                                And<ATPTEFMProjectBudgetLine.projectID, Equal<Current<ATPTEFMProjectBudgetFilter.projectID>>,
                                And<ATPTEFMProjectBudgetLine.projectTaskID, Equal<Required<ATPTEFMProjectBudgetLine.projectTaskID>>,
                                And<ATPTEFMProjectBudgetLine.costCodeID, Equal<Required<ATPTEFMProjectBudgetLine.costCodeID>>,
                                And<ATPTEFMProjectBudgetLine.accountGroupID, Equal<@P.AsInt>,
                                And<ATPTEFMProjectBudgetLine.finPeriodID, Equal<@P.AsString>>>>>>>>>
                            .Select(this, row.ProjectTaskID, row.CostCodeID, row.AccountGroupID, finperiod);

                        if (exDiteyl != null)
                        {
                            exDiteyl.FinYear = row.FinYear;
                            exDiteyl.LedgerID = row.LedgerID;
                            exDiteyl.ProjectID = row.ProjectID;
                            exDiteyl.FinPeriodID = finperiod;
                            exDiteyl.ProjectTaskID = row.ProjectTaskID;
                            exDiteyl.CostCodeID = row.CostCodeID;
                            exDiteyl.AccountGroupID = row.AccountGroupID;
                            exDiteyl.Amount = _Periods[i];
                            exDiteyl.ReleasedAmount = row.Released == true ? _Periods[i] : 0;
                            exDiteyl.Released = row.Released;
                            exDiteyl.WasReleased = row.WasReleased;
                            exDiteyl = ProjectBudgetLine.Update(exDiteyl);
                            ProjectBudgetLine.Cache.Persist(PXDBOperation.Update);
                        }
                    }
                    break;
                case ActionTypes.Delete:
                    for (int i = 0; i < 12; i++)
                    {
                        ATPTEFMProjectBudgetLine Diteyl = ProjectBudgetLine.Select().FirstTableItems.ToList()
                            .Where(x => x.FinYear == row.FinYear
                                        && x.LedgerID == row.LedgerID
                                        && x.ProjectID == row.ProjectID
                                        && x.FinPeriodID == string.Format("{0}{1}", ProjectFilter.Current.FinYear, (i + 1).ToString("D2"))
                                        && x.ProjectTaskID == row.ProjectTaskID
                                        && x.CostCodeID == row.CostCodeID).FirstOrDefault();
                        Diteyl = ProjectBudgetLine.Delete(Diteyl);
                        ProjectBudgetLine.Cache.Persist(PXDBOperation.Delete);
                    }
                    break;
            }
        }

        private void DistributeSummary(ref ATPTEFMProjectBudgetLineSummary row, string method, ATPTEFMProjectBudgetLineSummary rowCompare = null)
        {
            rowCompare = rowCompare ?? row;
            if ((rowCompare.Amount ?? 0) == 0 && (rowCompare.DistributedAmount ?? 0) == 0)
                method = ATPTEFMBudgetDistributionMethodAttribute.EvenlyVal;
            if (rowCompare.DistributedAmount == 0) rowCompare.DistributedAmount = 1;
            switch (method)
            {
                case ATPTEFMBudgetDistributionMethodAttribute.EvenlyVal:
                    decimal? amt = (row.Amount / 12).RoundDecimal(2);
                    row.FinPeriod01 = amt;
                    row.FinPeriod02 = amt;
                    row.FinPeriod03 = amt;
                    row.FinPeriod04 = amt;
                    row.FinPeriod05 = amt;
                    row.FinPeriod06 = amt;
                    row.FinPeriod07 = amt;
                    row.FinPeriod08 = amt;
                    row.FinPeriod09 = amt;
                    row.FinPeriod10 = amt;
                    row.FinPeriod11 = amt;
                    row.FinPeriod12 = (row.Amount - (amt * 11)).RoundDecimal(2);
                    break;
                default:
                    row.FinPeriod01 = row.Amount * (rowCompare.FinPeriod01 / rowCompare.DistributedAmount);
                    row.FinPeriod02 = row.Amount * (rowCompare.FinPeriod02 / rowCompare.DistributedAmount);
                    row.FinPeriod03 = row.Amount * (rowCompare.FinPeriod03 / rowCompare.DistributedAmount);
                    row.FinPeriod04 = row.Amount * (rowCompare.FinPeriod04 / rowCompare.DistributedAmount);
                    row.FinPeriod05 = row.Amount * (rowCompare.FinPeriod05 / rowCompare.DistributedAmount);
                    row.FinPeriod06 = row.Amount * (rowCompare.FinPeriod06 / rowCompare.DistributedAmount);
                    row.FinPeriod07 = row.Amount * (rowCompare.FinPeriod07 / rowCompare.DistributedAmount);
                    row.FinPeriod08 = row.Amount * (rowCompare.FinPeriod08 / rowCompare.DistributedAmount);
                    row.FinPeriod09 = row.Amount * (rowCompare.FinPeriod09 / rowCompare.DistributedAmount);
                    row.FinPeriod10 = row.Amount * (rowCompare.FinPeriod10 / rowCompare.DistributedAmount);
                    row.FinPeriod11 = row.Amount * (rowCompare.FinPeriod11 / rowCompare.DistributedAmount);
                    row.FinPeriod12 = row.Amount * (rowCompare.FinPeriod12 / rowCompare.DistributedAmount);
                    break;
            }
            row.BudgetDistributionMethod = method;
            row.DistributedAmount = new decimal?[] {
                row.FinPeriod01, row.FinPeriod02, row.FinPeriod03, row.FinPeriod04,
                row.FinPeriod05, row.FinPeriod06, row.FinPeriod07, row.FinPeriod08,
                row.FinPeriod09, row.FinPeriod10, row.FinPeriod11, row.FinPeriod12
            }.Sum();
        }

        public void ReleaseSummary(ATPTEFMProjectBudgetLineSummary row, bool isMassProcess = false)
        {
            if (row.Released == true) throw new PXException(ATPTEFMMessages.AlreadyReleased);

            ProjectFilter.Current.FinYear = row.FinYear;
            ProjectFilter.Current.LedgerID = row.LedgerID;
            ProjectFilter.Current.ProjectID = row.ProjectID;

            row.Released = true;
            row.WasReleased = true;
            row.ReleasedAmount = row.Amount;

            Summary.Update(row);

            PXResultset<ATPTEFMProjectBudgetLine> budgetLines =
                PXSelect<ATPTEFMProjectBudgetLine,
                Where<ATPTEFMProjectBudgetLine.finYear, Equal<Current<ATPTEFMProjectBudgetFilter.finYear>>,
                    And<ATPTEFMProjectBudgetLine.ledgerID, Equal<Current<ATPTEFMProjectBudgetFilter.ledgerID>>,
                    And<ATPTEFMProjectBudgetLine.projectID, Equal<Current<ATPTEFMProjectBudgetFilter.projectID>>,
                    And<ATPTEFMProjectBudgetLine.projectTaskID, Equal<Required<ATPTEFMProjectBudgetLine.projectTaskID>>,
                    And<ATPTEFMProjectBudgetLine.costCodeID, Equal<Required<ATPTEFMProjectBudgetLine.costCodeID>>,
                    And<ATPTEFMProjectBudgetLine.accountGroupID, Equal<@P.AsInt>>>>>>>>
                .Select(this, row.ProjectTaskID, row.CostCodeID, row.AccountGroupID);

            foreach (ATPTEFMProjectBudgetLine line in budgetLines)
            {
                line.ReleasedAmount = line.Amount;
                line.Released = true;
                line.WasReleased = true;
                ProjectBudgetLine.Update(line);
            }

            Persist();

            if (isMassProcess) PXProcessing.SetInfo(ATPTEFMMessages.MassProcessReleased);
        }

        public bool PrepareImportRow(string viewName, IDictionary keys, IDictionary values)
        {
            return true;
        }

        public bool RowImporting(string viewName, object row)
        {
            return row == null;
        }

        public bool RowImported(string viewName, object row, object oldRow)
        {
            return oldRow == null;
        }

        public void PrepareItems(string viewName, IEnumerable items)
        {

        }
        private void CheckDuplicates()
        {
            foreach (ATPTEFMProjectBudgetLineSummary line in Summary.Select())
            {
                bool hasDuplicate = PXSelect<
                    ATPTEFMProjectBudgetLineSummary,
                    Where<ATPTEFMProjectBudgetLineSummary.finYear, Equal<Required<ATPTEFMProjectBudgetLineSummary.finYear>>,
                        And<ATPTEFMProjectBudgetLineSummary.ledgerID, Equal<Required<ATPTEFMProjectBudgetLineSummary.ledgerID>>,
                        And<ATPTEFMProjectBudgetLineSummary.projectID, Equal<Required<ATPTEFMProjectBudgetLineSummary.projectID>>,
                        And<ATPTEFMProjectBudgetLineSummary.projectTaskID, Equal<Required<ATPTEFMProjectBudgetLineSummary.projectTaskID>>,
                        And<ATPTEFMProjectBudgetLineSummary.costCodeID, Equal<Required<ATPTEFMProjectBudgetLineSummary.costCodeID>>,
                        And<ATPTEFMProjectBudgetLineSummary.accountGroupID, Equal<Required<ATPTEFMProjectBudgetLineSummary.accountGroupID>>>>>>>>
                        >
                    .Select(this,
                            line.FinYear,
                            line.LedgerID,
                            line.ProjectID,
                            line.ProjectTaskID,
                            line.CostCodeID,
                            line.AccountGroupID).Count() > 1;

                if (hasDuplicate)
                {
                    Summary.Cache.RaiseExceptionHandling<ATPTEFMProjectBudgetLineSummary.projectTaskID>(line, line.ProjectTaskID, ATPTEFMHelper.GetPropertyException(line, ATPTEFMMessages.DuplicateProjectCombinationEntry, PXErrorLevel.RowError));
                    throw new PXException(ATPTEFMMessages.DuplicateProjectCombinationEntry, PXErrorLevel.RowError);
                }
            }
        }
        #endregion

        #region Internal Types
        public enum ActionTypes { Insert, Delete, Update }
        public enum PreloadActionTypes { UpdateOnly = 1, LoadOnly = 2, Both = 3 }

        [Serializable]
        [PXCacheName("Project Budget Filter")]
        public partial class ATPTEFMProjectBudgetFilter : CashFundManagement.DAC.Base.ATPTEFMBase, IBqlTable
        {
            #region LedgerID
            [PXInt]
            [PXUnboundDefault]
            [PXSelector(typeof(Search<Ledger.ledgerID, Where<Ledger.balanceType, Equal<LedgerBalanceType.budget>>>),
                        typeof(Ledger.ledgerCD),
                        typeof(Ledger.descr),
                        SubstituteKey = typeof(Ledger.ledgerCD),
                        DescriptionField = typeof(Ledger.descr))]
            [PXUIField(DisplayName = ATPTEFMMessages.LedgerID, Enabled = true)]
            public virtual int? LedgerID { get; set; }
            public abstract class ledgerID : PX.Data.BQL.BqlInt.Field<ledgerID> { }
            #endregion
            #region FinYear
            [PXString(4, IsFixed = true)]
            [GenericFinYearSelector(typeof(Search3<FinYear.year, OrderBy<Desc<FinYear.year>>>), null)]
            [PXUnboundDefault]
            [PXUIField(DisplayName = ATPTEFMMessages.FinYear)]
            public virtual string FinYear { get; set; }
            public abstract class finYear : PX.Data.BQL.BqlString.Field<finYear> { }
            #endregion
            #region ProjectID
            [PXInt]
            [PXSelector(typeof(Search<PMProject.contractID, Where<PMProject.nonProject, Equal<False>>>),
                        typeof(PMProject.contractCD),
                        typeof(PMProject.description),
                        SubstituteKey = typeof(PMProject.contractCD),
                        DescriptionField = typeof(PMProject.description))]
            [PXRestrictor(typeof(Where<PMProject.isActive, Equal<True>>), PX.Objects.GL.Messages.AccountInactive)]
            [PXUIField(DisplayName = ATPTEFMMessages.ProjectID, Visibility = PXUIVisibility.Visible, Enabled = true)]
            [PXUnboundDefault]
            public virtual int? ProjectID { get; set; }
            public abstract class projectID : PX.Data.BQL.BqlInt.Field<projectID> { }
            #endregion

            #region LastModifiedByID
            [PXDBLastModifiedByID()]
            public virtual Guid? LastModifiedByID { get; set; }
            public abstract class lastModifiedByID : IBqlField { }
            #endregion

            #region CompareLedgerID
            [PXInt]
            [PXSelector(typeof(Ledger.ledgerID),
                        typeof(Ledger.ledgerCD),
                        typeof(Ledger.descr),
                        SubstituteKey = typeof(Ledger.ledgerCD),
                        DescriptionField = typeof(Ledger.descr))]
            [PXUIField(DisplayName = ATPTEFMMessages.CompareLedgerID, Enabled = true)]
            [PXUnboundDefault(typeof(Current<ATPTEFMProjectBudget.ledgerID>), PersistingCheck = PXPersistingCheck.Nothing)]
            [PXFormula(typeof(Default<ATPTEFMProjectBudget.ledgerID>))]
            public virtual int? CompareLedgerID { get; set; }
            public abstract class compareLedgerID : PX.Data.BQL.BqlInt.Field<compareLedgerID> { }
            #endregion
            #region CompareFinYear
            [PXString(4, IsFixed = true)]
            [GenericFinYearSelector(typeof(Search3<FinYear.year, OrderBy<Desc<FinYear.year>>>), null)]
            [PXUnboundDefault(PersistingCheck = PXPersistingCheck.Nothing)]
            [PXUIField(DisplayName = ATPTEFMMessages.CompareFinYear)]
            public virtual string CompareFinYear { get; set; }
            public abstract class compareFinYear : PX.Data.BQL.BqlString.Field<compareFinYear> { }
            #endregion
            #region CompareProjectID
            [PXInt]
            [PXSelector(typeof(PMProject.contractID),
                        typeof(PMProject.contractCD),
                        typeof(PMProject.description),
                        SubstituteKey = typeof(PMProject.contractCD),
                        DescriptionField = typeof(PMProject.description))]
            [PXRestrictor(typeof(Where<PMProject.isActive, Equal<True>>), PX.Objects.GL.Messages.AccountInactive)]
            [PXUIField(DisplayName = ATPTEFMMessages.CompareProjectID, Visibility = PXUIVisibility.Visible, Enabled = true)]
            [PXUnboundDefault(typeof(Current<ATPTEFMProjectBudget.projectID>), PersistingCheck = PXPersistingCheck.Nothing)]
            [PXFormula(typeof(Default<ATPTEFMProjectBudget.projectID>))]
            public virtual int? CompareProjectID { get; set; }
            public abstract class compareProjectID : PX.Data.BQL.BqlInt.Field<compareProjectID> { }
            #endregion

            #region IsDelete
            [PXBool]
            [PXUnboundDefault(false)]
            public virtual bool? IsDelete { get; set; }
            public abstract class isDelete : PX.Data.BQL.BqlBool.Field<isDelete> { }
            #endregion
        }

        [Serializable]
        [PXCacheName("Budget Distribute Filter")]
        public partial class ATPTEFMBudgetDistributeFilter : CashFundManagement.DAC.Base.ATPTEFMBase, IBqlTable
        {
            #region Method
            [PXDBString(1, IsFixed = true)]
            [PXDefault(ATPTEFMBudgetDistributionMethodAttribute.Evenly)]
            [ATPTEFMBudgetDistributionMethodAttribute.ATPTEFMBudgetDistributionMethod]
            [PXUIField(DisplayName = ATPTEFMMessages.Method)]
            public virtual string Method { get; set; }
            public abstract class method : PX.Data.BQL.BqlString.Field<method> { }
            #endregion
            #region ApplyToAll
            public abstract class applyToAll : PX.Data.BQL.BqlBool.Field<applyToAll> { }
            [PXUIField(DisplayName = ATPTEFMMessages.ApplyToAll)]
            [PXBool]
            public virtual bool? ApplyToAll { get; set; }
            #endregion
            #region ApplyToSubGroups
            public abstract class applyToSubGroups : PX.Data.BQL.BqlBool.Field<applyToSubGroups> { }
            [PXUIField(DisplayName = ATPTEFMMessages.ApplyToSubGroups, Enabled = false)]
            [PXBool]
            public virtual bool? ApplyToSubGroups { get; set; }
            #endregion
        }

        [Serializable]
        [PXCacheName("Preload Budget Filter")]
        public partial class ATPTEFMPreloadBudgetFilter : CashFundManagement.DAC.Base.ATPTEFMBase, IBqlTable
        {
            #region LedgerID
            [PXDBInt]
            [PXDefault]
            [PXSelector(typeof(Ledger.ledgerID),
                        typeof(Ledger.ledgerCD),
                        typeof(Ledger.descr),
                        SubstituteKey = typeof(Ledger.ledgerCD),
                        DescriptionField = typeof(Ledger.descr))]
            [PXUIField(DisplayName = ATPTEFMMessages.LedgerID, Enabled = true)]
            public virtual int? LedgerID { get; set; }
            public abstract class ledgerID : PX.Data.BQL.BqlInt.Field<ledgerID> { }
            #endregion
            #region FinYear
            [PXDBString(4, IsFixed = true)]
            [GenericFinYearSelector(typeof(Search3<FinYear.year, OrderBy<Desc<FinYear.year>>>), null)]
            [PXDefault]
            [PXUIField(DisplayName = ATPTEFMMessages.FinYear)]
            public virtual string FinYear { get; set; }
            public abstract class finYear : PX.Data.BQL.BqlString.Field<finYear> { }
            #endregion
            #region ProjectID
            [PXDBInt]
            [PXSelector(typeof(PMProject.contractID),
                        typeof(PMProject.contractCD),
                        typeof(PMProject.description),
                        SubstituteKey = typeof(PMProject.contractCD),
                        DescriptionField = typeof(PMProject.description))]
            [PXRestrictor(typeof(Where<PMProject.isActive, Equal<True>>), PX.Objects.GL.Messages.AccountInactive)]
            [PXUIField(DisplayName = ATPTEFMMessages.ProjectID, Visibility = PXUIVisibility.Visible, Enabled = true)]
            [PXDefault]
            public virtual int? ProjectID { get; set; }
            public abstract class projectID : PX.Data.BQL.BqlInt.Field<projectID> { }
            #endregion
            #region Mutliplier
            [PXDBDecimal]
            [PXUIField(DisplayName = ATPTEFMMessages.Multiplier)]
            [PXDefault(TypeCode.Decimal, "100.0", PersistingCheck = PXPersistingCheck.Nothing)]
            public virtual decimal? Mutliplier { get; set; }
            public abstract class mutliplier : PX.Data.BQL.BqlDecimal.Field<mutliplier> { }
            #endregion
            #region PreloadAction
            public abstract class preloadAction : BqlShort.Field<preloadAction> { }
            [PXShort]
            [PXUnboundDefault((short)3)]
            public virtual short? PreloadAction { get; set; }
            #endregion
        }
        #endregion

        #region CacheAttached
        #region LedgerID
        [PXRemoveBaseAttribute(typeof(PXUIFieldAttribute))]
        [PXMergeAttributes(Method = MergeMethod.Merge)]
        [PXDefault(typeof(Current<ATPTEFMProjectBudgetFilter.ledgerID>))]
        public virtual void ATPTEFMProjectBudgetLineSummary_LedgerID_CacheAttached(PXCache cache) { }
        #endregion
        #region ProjectID
        [PXRemoveBaseAttribute(typeof(PXUIFieldAttribute))]
        [PXMergeAttributes(Method = MergeMethod.Merge)]
        [PXDefault(typeof(Current<ATPTEFMProjectBudgetFilter.projectID>))]
        public virtual void ATPTEFMProjectBudgetLineSummary_ProjectID_CacheAttached(PXCache cache) { }
        #endregion
        #region FinYear
        [PXRemoveBaseAttribute(typeof(PXUIFieldAttribute))]
        [PXMergeAttributes(Method = MergeMethod.Merge)]
        [PXDefault(typeof(Current<ATPTEFMProjectBudgetFilter.finYear>))]
        public virtual void ATPTEFMProjectBudgetLineSummary_FinYear_CacheAttached(PXCache cache) { }
        #endregion
        #endregion
    }
}