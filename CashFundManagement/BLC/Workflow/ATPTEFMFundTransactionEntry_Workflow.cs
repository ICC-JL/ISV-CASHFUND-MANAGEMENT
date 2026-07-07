using System;
using PX.Data;
using PX.Objects.Common;
using PX.Data.WorkflowAPI;
using CashFundManagement.DAC;
using CashFundManagement.Attributes.Base;

namespace CashFundManagement.BLC.Workflow
{
    using static ATPTEFMFundTransaction;
    using static BoundedTo<ATPTEFMFundTransactionEntry, ATPTEFMFundTransaction>;
    using State = CashFundManagement.Attributes.Base.ATPTEFMBaseStatus;
    public class ATPTEFMFundTransactionEntry_Workflow : PXGraphExtension<ATPTEFMFundTransactionEntry>
    {
        public static bool IsActive() => true;

        public override void Configure(PXScreenConfiguration config) =>
           Configure(config.GetScreenConfigurationContext<ATPTEFMFundTransactionEntry, ATPTEFMFundTransaction>());

        #region BQL Conditions
        public class Conditions : Condition.Pack
        {
            public Condition IsOnHold => GetOrCreate(c => c.FromBql<
                hold.IsEqual<True>
            >());
            public Condition IsClosed => GetOrCreate(c => c.FromBql<
                status.IsEqual<ATPTEFMBaseStatus.closedValue>
            >());
            public Condition IsCashReleased => GetOrCreate(c => c.FromBql<
                isReleasedCash.IsEqual<True>
            >());
            public Condition IsCancelled => GetOrCreate(c => c.FromBql<
               status.IsEqual<ATPTEFMBaseStatus.cancelledValue>
           >());
        }
        #endregion

        public static void DisableWholeScreen(FieldState.IContainerFillerFields fields)
        {
            fields.AddTable<ATPTEFMFundTransactionDetail>(c => c.IsDisabled());
        }
        public static void DisableMainView(FieldState.IContainerFillerFields fields)
        {
            fields.AddTable<ATPTEFMFundTransaction>(c => c.IsDisabled());
        }
        protected virtual void Configure(WorkflowContext<ATPTEFMFundTransactionEntry, ATPTEFMFundTransaction> context)
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
                    .StateIdentifierIs<ATPTEFMFundTransaction.status>()
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
                            .WithFieldAssignments(fa => fa.Add<hold>(true))
                            .IsDisabledWhen(conditions.IsCashReleased));
                        actions.Add(g => g.ReleaseCash, c => c
                            .WithCategory(processingCategory));
                        actions.Add(g => g.FTCancel, c => c
                            .WithCategory(otherCategory)
                            .IsDisabledWhen(conditions.IsCancelled));
                        #endregion
                    })
                    .WithFieldStates(fields =>
                    {
                        fields.Add<ATPTEFMFundTransaction.hold>(c => c.IsHiddenAlways());

                        fields.Add<ATPTEFMFundTransaction.fundType>(c => c.IsDisabledWhen(!conditions.IsOnHold));
                        fields.Add<ATPTEFMFundTransaction.fundID>(c => c.IsDisabledWhen(!conditions.IsOnHold));
                        fields.Add<ATPTEFMFundTransaction.fundTransactionType>(c => c.IsDisabledWhen(!conditions.IsOnHold));
                        fields.Add<ATPTEFMFundTransaction.date>(c => c.IsDisabledWhen(!conditions.IsOnHold));
                        fields.Add<ATPTEFMFundTransaction.finPeriodID>(c => c.IsDisabledWhen(!conditions.IsOnHold));
                        fields.Add<ATPTEFMFundTransaction.descr>(c => c.IsDisabledWhen(!conditions.IsOnHold));
                        fields.Add<ATPTEFMFundTransaction.requestedByID>(c => c.IsDisabledWhen(!conditions.IsOnHold));
                        fields.Add<ATPTEFMFundTransaction.departmentID>(c => c.IsDisabledWhen(!conditions.IsOnHold));
                        fields.Add<ATPTEFMFundTransaction.dateOfUse>(c => c.IsDisabledWhen(!conditions.IsOnHold));
                        fields.Add<ATPTEFMFundTransaction.liqDate>(c => c.IsDisabledWhen(!conditions.IsOnHold));
                        fields.Add<ATPTEFMFundTransaction.initialLiqDate>(c => c.IsDisabledWhen(!conditions.IsOnHold));
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
                                    actions.Add(g => g.ReleaseCash, c => c.IsDuplicatedInToolbar().WithConnotation(ActionConnotation.Success));
                                });
                        });
                        states.Add<State.closedValue>(flowState =>
                        {
                            return flowState
                                .WithActions(actions =>
                                {
                                })
                                .WithFieldStates(DisableWholeScreen);
                        });
                        states.Add<State.cancelledValue>(flowState =>
                        {
                            return flowState
                                .WithActions(actions =>
                                {
                                })
                                .WithFieldStates(DisableMainView);
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
                            ts.Add(t => t
                                .To<State.closedValue>()
                                .IsTriggeredOn(g => g.ReleaseCash)
                                .When(conditions.IsClosed));
                        });
                    });
            }

            #endregion
        }
    }
}
