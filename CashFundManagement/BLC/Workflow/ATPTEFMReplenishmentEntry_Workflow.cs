using PX.Data;
using PX.Objects.Common;
using PX.Data.WorkflowAPI;
using CashFundManagement.DAC;
using CashFundManagement.Attributes;

namespace CashFundManagement.BLC.Workflow
{
    using static ATPTEFMReplenishment;
    using static BoundedTo<ATPTEFMReplenishmentEntry, ATPTEFMReplenishment>;
    using State = CashFundManagement.Attributes.Base.ATPTEFMBaseStatus;
    public class ATPTEFMReplenishmentEntry_Workflow : PXGraphExtension<ATPTEFMReplenishmentEntry>
    {
        public static bool IsActive() => true;

        public override void Configure(PXScreenConfiguration config) =>
           Configure(config.GetScreenConfigurationContext<ATPTEFMReplenishmentEntry, ATPTEFMReplenishment>());

        #region BQL Conditions
        public class Conditions : Condition.Pack
        {
            public Condition IsOnHold => GetOrCreate(c => c.FromBql<
                hold.IsEqual<True>
            >());
            public Condition IsReleased => GetOrCreate(c => c.FromBql<
                isReleased.IsEqual<True>
            >());
            public Condition EnableSubmit => GetOrCreate(c => c.FromBql<
                step.IsEqual<ATPTEFMReplenishmentStepAttribute.defaultValue>
           >());
            public Condition EnableReplenishmentCancel => GetOrCreate(c => c.FromBql<
                step.IsEqual<ATPTEFMReplenishmentStepAttribute.submitReceiptValue>
            >());
            public Condition EnableReplenishmentCancelRejectedStatus => GetOrCreate(c => c.FromBql<
               rejected.IsEqual<True>
            >());
        }
        #endregion

        public static void DisableWholeScreen(FieldState.IContainerFillerFields fields)
        {
            fields.AddTable<ATPTEFMFundTransactionReceiptDetail>(c => c.IsDisabled());
            fields.AddField<ATPTEFMReplenishment.taxZone>(c => c.IsDisabled());
        }

        protected virtual void Configure(WorkflowContext<ATPTEFMReplenishmentEntry, ATPTEFMReplenishment> context)
        {
            var conditions = context.Conditions.GetPack<Conditions>();

            #region Category
            var commonCategories = CommonActionCategories.Get(context);
            var processingCategory = commonCategories.Processing;
            var otherCategory = commonCategories.Other;
            #endregion

            #region Screen Configuration

            context.AddScreenConfigurationFor(screen =>
            {
                return screen
                    .StateIdentifierIs<ATPTEFMReplenishment.status>()
                    .AddDefaultFlow(DefaultDocumentFlow)
                    .WithActions(actions =>
                    {
                        actions.Add(g => g.InitializeState);

                        #region Processing
                        actions.Add(g => g.RemoveFromHold, c => c
                            .WithCategory(processingCategory)
                            .WithPersistOptions(ActionPersistOptions.PersistBeforeAction)
                            .WithFieldAssignments(fa => fa.Add<hold>(false)));
                        actions.Add(g => g.PutOnHold, c => c
                            .WithCategory(processingCategory)
                            .WithPersistOptions(ActionPersistOptions.PersistBeforeAction)
                            .WithFieldAssignments(fa => fa.Add<hold>(true)));
                        actions.Add(g => g.Release, c => c
                            .WithCategory(processingCategory)
                            .IsDisabledWhen(conditions.IsReleased));
                        actions.Add(g => g.Submit, c => c
                            .WithCategory(processingCategory)
                            .IsDisabledWhen(!conditions.EnableSubmit));
                        actions.Add(g => g.ReplenishmentCancel, c => c
                            .WithCategory(processingCategory)
                            .IsDisabledWhen(!conditions.EnableReplenishmentCancel && !conditions.EnableReplenishmentCancelRejectedStatus));
                        #endregion
                    })
                    .WithFieldStates(fields =>
                    {
                        fields.Add<ATPTEFMReplenishment.hold>(c => c.IsHiddenAlways());

                        fields.Add<ATPTEFMReplenishment.fundType>(c => c.IsDisabledWhen(!conditions.IsOnHold));
                        fields.Add<ATPTEFMReplenishment.fundID>(c => c.IsDisabledWhen(!conditions.IsOnHold));
                        fields.Add<ATPTEFMReplenishment.date>(c => c.IsDisabledWhen(!conditions.IsOnHold));
                        fields.Add<ATPTEFMReplenishment.descr>(c => c.IsDisabledWhen(!conditions.IsOnHold));
                        fields.Add<ATPTEFMReplenishment.taxZone>(c => c.IsDisabledWhen(!conditions.IsOnHold));

                    })
                    .WithCategories(categories =>
                    {
                        categories.Add(processingCategory);
                        categories.Add(otherCategory);
                        categories.Update(FolderType.ReportsFolder, category => category.PlaceAfter(otherCategory));
                    });
            });

            #endregion

            #region Workflows

            Workflow.IConfigured DefaultDocumentFlow(Workflow.INeedStatesFlow flow)
            {
                return flow
                    .WithFlowStates(states =>
                    {
                        states.Add(State.Initial, flowState => flowState.IsInitial(g => g.InitializeState));
                        states.Add<State.holdValue>(flowState =>
                        {
                            return flowState
                                .WithActions(actions =>
                                {
                                    actions.Add(g => g.RemoveFromHold, c => c.IsDuplicatedInToolbar().WithConnotation(ActionConnotation.Success));
                                });
                        });
                        states.Add<State.openValue>(flowState =>
                        {
                            return flowState
                                .WithActions(actions =>
                                {
                                    actions.Add(g => g.PutOnHold, c => c.IsDuplicatedInToolbar().WithConnotation(ActionConnotation.Warning));
                                    actions.Add(g => g.ReplenishmentCancel, c => c.IsDuplicatedInToolbar().WithConnotation(ActionConnotation.Danger));
                                    actions.Add(g => g.Submit, c => c.IsDuplicatedInToolbar().WithConnotation(ActionConnotation.Success));
                                    actions.Add(g => g.Release, c => c.IsDuplicatedInToolbar().WithConnotation(ActionConnotation.Success));
                                })
                                .WithFieldStates(DisableWholeScreen);
                        });
                        states.Add<State.closedValue>(flowState =>
                        {
                            return flowState
                                .WithActions(actions =>
                                {
                                })
                                .WithFieldStates(DisableWholeScreen);
                        });
                    })
                    .WithTransitions(transitions =>
                    {
                        transitions.AddGroupFrom(State.Initial, ts =>
                        {
                            ts.Add(t => t.To<State.holdValue>()
                                .IsTriggeredOn(g => g.InitializeState)
                                .When(conditions.IsOnHold));
                        });
                        transitions.AddGroupFrom<State.holdValue>(ts =>
                        {
                            ts.Add(t => t
                                .To<State.openValue>()
                                .IsTriggeredOn(g => g.RemoveFromHold)
                                .When(!conditions.IsOnHold));
                        });
                        transitions.AddGroupFrom<State.openValue>(ts =>
                        {
                            ts.Add(t => t
                                .To<State.holdValue>()
                                .IsTriggeredOn(g => g.PutOnHold)
                                .When(conditions.IsOnHold));
                        });
                    });
                
            }
            
            #endregion
        }
    }
}
