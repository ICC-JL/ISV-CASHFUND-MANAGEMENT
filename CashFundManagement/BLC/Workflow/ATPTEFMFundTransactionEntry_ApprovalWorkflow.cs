using System;
using System.Collections;
using PX.Data;
using PX.Objects.Common;
using PX.Data.WorkflowAPI;
using CashFundManagement.DAC;
using CashFundManagement.DAC.Setup;
using PX.Objects.CS;

namespace CashFundManagement.BLC.Workflow
{
    using static ATPTEFMFundTransaction;
    using static BoundedTo<ATPTEFMFundTransactionEntry, ATPTEFMFundTransaction>;
    using Context = WorkflowContext<ATPTEFMFundTransactionEntry, ATPTEFMFundTransaction>;
    using State = CashFundManagement.Attributes.Base.ATPTEFMBaseStatus;
    public class ATPTEFMFundTransactionEntry_ApprovalWorkflow : PXGraphExtension<ATPTEFMFundTransactionEntry_Workflow, ATPTEFMFundTransactionEntry>
    {
        public static bool IsActive() => true;

        #region Prefetch
        private class ATPTEFMFundTransactionApproval : IPrefetchable
        {
            public static bool IsActive => PXDatabase.GetSlot<ATPTEFMFundTransactionApproval>(nameof(ATPTEFMFundTransactionApproval), typeof(ATPTEFMSetup)).RequireApproval;
            private bool RequireApproval { get; set; }

            void IPrefetchable.Prefetch()
            {
                using (PXDataRecord xSetup = PXDatabase.SelectSingle<ATPTEFMSetup>(new PXDataField<ATPTEFMSetup.fundTransactionRequestApproval>()))
                {
                    if (xSetup != null)
                    {
                        RequireApproval = (bool)xSetup.GetBoolean(0);
                        PXTrace.WriteInformation("{0} : Fund Transaction Approval", RequireApproval.ToString());
                    }
                }
            }
        }
        #endregion

        private static bool ApprovalIsActive => PXAccess.FeatureInstalled<FeaturesSet.approvalWorkflow>() && ATPTEFMFundTransactionApproval.IsActive;

        [PXWorkflowDependsOnType(typeof(ATPTEFMSetup))]
        public override void Configure(PXScreenConfiguration config)
        {
            if (ApprovalIsActive)
                Configure(config.GetScreenConfigurationContext<ATPTEFMFundTransactionEntry, ATPTEFMFundTransaction>());
            else
                HideApprovalActions(config.GetScreenConfigurationContext<ATPTEFMFundTransactionEntry, ATPTEFMFundTransaction>());
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
            var baseConditions = context.Conditions.GetPack<ATPTEFMFundTransactionEntry_Workflow.Conditions>();

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
                                    })
                                    .WithFieldStates(ATPTEFMFundTransactionEntry_Workflow.DisableWholeScreen);
                            });
                            states.Add<State.rejectedValue>(flowState =>
                            {
                                return flowState
                                    .WithActions(actions =>
                                    {
                                        actions.Add(a => a.PutOnHold, g => g.IsDuplicatedInToolbar().WithConnotation(ActionConnotation.Warning));
                                    })
                                    .WithFieldStates(ATPTEFMFundTransactionEntry_Workflow.DisableWholeScreen);
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
