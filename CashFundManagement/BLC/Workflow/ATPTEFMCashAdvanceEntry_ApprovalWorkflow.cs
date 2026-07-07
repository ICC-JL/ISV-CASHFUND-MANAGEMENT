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
    using static ATPTEFMCashAdvance;
    using static BoundedTo<ATPTEFMCashAdvanceEntry, ATPTEFMCashAdvance>;
    using Context = WorkflowContext<ATPTEFMCashAdvanceEntry, ATPTEFMCashAdvance>;
    using State = ATPTEFMCashAdvanceStatusAttribute;
    public class ATPTEFMCashAdvanceEntry_ApprovalWorkflow : PXGraphExtension<ATPTEFMCashAdvanceEntry_Workflow, ATPTEFMCashAdvanceEntry>
    {
        public static bool IsActive() => true;

        #region Prefetch
        private class ATPTEFMCashAdvanceEntryApproval : IPrefetchable
        {
            public static bool IsActive => PXDatabase.GetSlot<ATPTEFMCashAdvanceEntryApproval>(nameof(ATPTEFMCashAdvanceEntryApproval), typeof(ATPTEFMCASetup)).RequireApproval;
            private bool RequireApproval { get; set; }

            void IPrefetchable.Prefetch()
            {
                using (PXDataRecord xSetup = PXDatabase.SelectSingle<ATPTEFMCASetup>(new PXDataField<ATPTEFMCASetup.cashAdvanceRequestApproval>()))
                {
                    if (xSetup != null)
                    {
                        RequireApproval = (bool)xSetup.GetBoolean(0);
                        PXTrace.WriteInformation("{0} : Cash Advance", RequireApproval.ToString());
                    }
                }
            }
        }
        #endregion

        private static bool ApprovalIsActive => PXAccess.FeatureInstalled<FeaturesSet.approvalWorkflow>() && ATPTEFMCashAdvanceEntryApproval.IsActive;

        [PXWorkflowDependsOnType(typeof(ATPTEFMCASetup))]
        public override void Configure(PXScreenConfiguration config)
        {
            if (ApprovalIsActive)
                Configure(config.GetScreenConfigurationContext<ATPTEFMCashAdvanceEntry, ATPTEFMCashAdvance>());
            else
                HideApprovalActions(config.GetScreenConfigurationContext<ATPTEFMCashAdvanceEntry, ATPTEFMCashAdvance>());
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
            var baseConditions = context.Conditions.GetPack<ATPTEFMCashAdvanceEntry_Workflow.Conditions>();

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
                                        actions.Add(g => g.PendingForLiquidationImport, c => c.IsDuplicatedInToolbar().WithConnotation(ActionConnotation.Success));
                                    })
                                    .WithFieldStates(ATPTEFMCashAdvanceEntry_Workflow.DisableWholeScreen);
                            });
                            states.Add<State.rejectedValue>(flowState =>
                            {
                                return flowState
                                    .WithActions(actions =>
                                    {
                                        actions.Add(a => a.PutOnHold, g => g.IsDuplicatedInToolbar());
                                    })
                                    .WithFieldStates(ATPTEFMCashAdvanceEntry_Workflow.DisableWholeScreen);
                            });
                        })
                        .WithTransitions(transitions =>
                        {
                            transitions.UpdateGroupFrom<State.holdValue>(ts =>
                            {
                                ts.Add(t => t
                                    .To<State.pendingValue>()
                                    .IsTriggeredOn(g => g.RemoveFromHold)
                                    .When(!baseConditions.IsOnHold && !conditions.IsApproved)
                                    .PlaceBefore(tr => tr.To<State.openValue>()));
                            });
                            transitions.AddGroupFrom<State.pendingValue>(ts =>
                            {
                                ts.Add(t => t
                                    .To<State.openValue>()
                                    .IsTriggeredOn(approve)
                                    .When(conditions.IsApproved));
                                ts.Add(t => t
                                    .To<State.rejectedValue>()
                                    .IsTriggeredOn(reject)
                                    .When(conditions.IsRejected));
                                ts.Add(t => t
                                    .To<State.holdValue>()
                                    .IsTriggeredOn(g => g.PutOnHold));
                                ts.Add(t => t.To<State.pendingLiquidationValue>()
                                    .IsTriggeredOn(g => g.PendingForLiquidationImport));
                            });
                            transitions.AddGroupFrom<State.rejectedValue>(ts =>
                            {
                                ts.Add(t => t
                                .To<State.holdValue>()
                                .IsTriggeredOn(g => g.PutOnHold)
                                .When(baseConditions.IsOnHold));
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
