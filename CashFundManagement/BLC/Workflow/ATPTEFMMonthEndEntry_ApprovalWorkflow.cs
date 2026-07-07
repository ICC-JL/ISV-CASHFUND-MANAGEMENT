using System;
using System.Collections;
using PX.Data;
using PX.Objects.Common;
using PX.Data.WorkflowAPI;
using CashFundManagement.DAC;
using CashFundManagement.Attributes.Base;
using CashFundManagement.DAC.Setup;
using PX.Objects.CS;
using CashFundManagement.Attributes;

namespace CashFundManagement.BLC.Workflow
{
    using static ATPTEFMMonthEnd;
    using static BoundedTo<ATPTEFMMonthEndEntry, ATPTEFMMonthEnd>;
    using Context = WorkflowContext<ATPTEFMMonthEndEntry, ATPTEFMMonthEnd>;
    using State = ATPTEFMMonthEndStatusAttribute;
    public class ATPTEFMMonthEndEntry_ApprovalWorkflow : PXGraphExtension<ATPTEFMMonthEndEntry_Workflow, ATPTEFMMonthEndEntry>
    {
        public static bool IsActive() => true;

        #region Prefetch
        private class ATPTEFMMonthEndEntryApproval : IPrefetchable
        {
            public static bool IsActive => PXDatabase.GetSlot<ATPTEFMMonthEndEntryApproval>(nameof(ATPTEFMMonthEndEntryApproval), typeof(ATPTEFMSetup)).RequireApproval;
            private bool RequireApproval { get; set; }

            void IPrefetchable.Prefetch()
            {
                using (PXDataRecord xSetup = PXDatabase.SelectSingle<ATPTEFMSetup>(new PXDataField<ATPTEFMSetup.monthEndRequestApproval>()))
                {
                    if (xSetup != null)
                    {
                        RequireApproval = (bool)xSetup.GetBoolean(0);
                        PXTrace.WriteInformation("{0} : Month-End", RequireApproval.ToString());
                    }
                }
            }
        }
        #endregion

        private static bool ApprovalIsActive => PXAccess.FeatureInstalled<FeaturesSet.approvalWorkflow>() && ATPTEFMMonthEndEntryApproval.IsActive;

        [PXWorkflowDependsOnType(typeof(ATPTEFMSetup))]
        public override void Configure(PXScreenConfiguration config)
        {
            if (ApprovalIsActive)
                Configure(config.GetScreenConfigurationContext<ATPTEFMMonthEndEntry, ATPTEFMMonthEnd>());
            else
                HideApprovalActions(config.GetScreenConfigurationContext<ATPTEFMMonthEndEntry, ATPTEFMMonthEnd>());
        }

        public class Conditions : Condition.Pack
        {
            public Condition IsApproved => GetOrCreate(b => b.FromBql<
                approved.IsEqual<True>
            >());

            public Condition IsRejected => GetOrCreate(b => b.FromBql<
                rejected.IsEqual<True>
            >());
        }

        protected virtual void Configure(Context context)
        {
            var conditions = context.Conditions.GetPack<Conditions>();
            var baseConditions = context.Conditions.GetPack<ATPTEFMMonthEndEntry_Workflow.Conditions>();

            #region Category
            var commonCategories = CommonActionCategories.Get(context);
            var approvalCategory = commonCategories.Approval;
            #endregion

            #region Buttons
            var approve = context.ActionDefinitions
                .CreateNew("Approve", a => a
                .WithCategory(approvalCategory, g => g.PutOnHold)
                .PlaceAfter(g => g.PutOnHold)
                .PlaceInCategory(Placement.First)
                .WithFieldAssignments(fa => fa.Add<approved>(true)));

            var reject = context.ActionDefinitions
                .CreateNew("Reject", a => a
                .WithCategory(approvalCategory)
                .PlaceInCategory(Placement.First)
                .PlaceAfter(approve)
                .WithFieldAssignments(fa => fa.Add<rejected>(true)));
            #endregion

            context.UpdateScreenConfigurationFor(screen =>
            {
                return screen
                    .UpdateDefaultFlow(flow =>
                        flow
                        .WithFlowStates(states =>
                        {
                            states.Add<State.pendingValue>(flowState =>
                            {
                                return flowState
                                    .WithActions(actions =>
                                    {
                                        actions.Add(a => a.PutOnHold, g => g.IsDuplicatedInToolbar().WithConnotation(ActionConnotation.Warning));
                                        actions.Add(approve, g => g.IsDuplicatedInToolbar().WithConnotation(ActionConnotation.Success));
                                        actions.Add(reject, g => g.IsDuplicatedInToolbar().WithConnotation(ActionConnotation.Danger));
                                    });
                            });
                            states.Add<State.rejectedValue>(flowState =>
                            {
                                return flowState
                                    .WithActions(actions =>
                                    {
                                        actions.Add(a => a.PutOnHold, g => g.IsDuplicatedInToolbar().WithConnotation(ActionConnotation.Warning));
                                    });
                            });
                        })
                        .WithTransitions(transitions =>
                        {
                            transitions.UpdateGroupFrom<State.holdValue>(ts =>
                            {
                                //ts.Remove(t => t.To<State.open>().IsTriggeredOn(g => g.RemoveFromHold));
                                ts.Add(t => t
                                    .To<State.pendingValue>()
                                    .IsTriggeredOn(g => g.RemoveFromHold)
                                    .When(!baseConditions.IsOnHold && !conditions.IsApproved)
                                    .PlaceBefore(tr => tr.To<State.releaseValue>()));
                            });
                            transitions.AddGroupFrom<State.pendingValue>(ts =>
                            {
                                ts.Add(t => t
                                    .To<State.releaseValue>()
                                    .IsTriggeredOn(approve)
                                .When(conditions.IsApproved));
                                ts.Add(t => t
                                    .To<State.rejectedValue>()
                                    .IsTriggeredOn(reject)
                                    .When(conditions.IsRejected));
                                ts.Add(t => t
                                    .To<State.holdValue>()
                                    .IsTriggeredOn(g => g.PutOnHold));
                            });
                            transitions.AddGroupFrom<State.rejectedValue>(ts =>
                            {
                                ts.Add(t => t
                                .To<State.holdValue>()
                                .IsTriggeredOn(g => g.PutOnHold));
                            });
                        }))
                    .WithActions(actions =>
                    {
                        actions.Add(approve);
                        actions.Add(reject);
                        actions.Update(
                            g => g.PutOnHold,
                            a => a.WithFieldAssignments(fas =>
                            {
                                fas.Add<approved>(e => e.SetFromValue(false));
                                fas.Add<rejected>(e => e.SetFromValue(false));
                            }));
                    })
                    .WithCategories(categories =>
                    {
                        categories.Add(CommonActionCategories.Get(context).Approval);
                        categories.Update(CommonActionCategories.OtherCategoryID, category => category.PlaceAfter(CommonActionCategories.ApprovalCategoryID));
                    });
            });
        }

        protected virtual void HideApprovalActions(Context context)
        {
            var commonCategories = CommonActionCategories.Get(context);

            var approve = context.ActionDefinitions
                .CreateNew("Approve", a => a
                .WithCategory(commonCategories.Approval, g => g.PutOnHold)
                .PlaceAfter(g => g.PutOnHold)
                .IsHiddenAlways());
            var reject = context.ActionDefinitions
                .CreateNew("Reject", a => a
                .WithCategory(commonCategories.Approval, approve)
                .PlaceAfter(approve)
                .IsHiddenAlways());

            context.UpdateScreenConfigurationFor(screen =>
            {
                return screen
                    .WithActions(actions =>
                    {
                        actions.Add(approve);
                        actions.Add(reject);
                    });
            });
        }
    }
}
