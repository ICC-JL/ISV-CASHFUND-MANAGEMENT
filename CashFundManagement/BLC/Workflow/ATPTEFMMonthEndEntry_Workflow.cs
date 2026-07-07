using System;
using PX.Data;
using PX.Objects.Common;
using PX.Data.WorkflowAPI;
using CashFundManagement.DAC;
using CashFundManagement.Attributes.Base;
using static CashFundManagement.BLC.ATPTEFMMonthEndEntry;
using CashFundManagement.Attributes;

namespace CashFundManagement.BLC.Workflow
{
    using static ATPTEFMMonthEnd;
    using static BoundedTo<ATPTEFMMonthEndEntry, ATPTEFMMonthEnd>;
    using State = ATPTEFMMonthEndStatusAttribute;
    public class ATPTEFMMonthEndEntry_Workflow : PXGraphExtension<ATPTEFMMonthEndEntry>
    {
        public static bool IsActive() => true;

        public override void Configure(PXScreenConfiguration config) =>
           Configure(config.GetScreenConfigurationContext<ATPTEFMMonthEndEntry, ATPTEFMMonthEnd>());

        #region BQL Conditions
        public class Conditions : Condition.Pack
        {
            public Condition IsOnHold => GetOrCreate(c => c.FromBql<
               hold.IsEqual<True>
            >());
            public Condition IsClosed => GetOrCreate(c => c.FromBql<
                status.IsEqual<ATPTEFMBaseStatus.closedValue>
            >());
        }
        #endregion
        //public static void DisableWholeScreen(FieldState.IContainerFillerFields fields)
        //{
        //    fields.AddTable<ATPTEFMCARequestDetail>(c => c.IsDisabled());
        //}
        protected virtual void Configure(WorkflowContext<ATPTEFMMonthEndEntry, ATPTEFMMonthEnd> context)
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
                    .StateIdentifierIs<ATPTEFMMonthEnd.status>()
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
                            .WithCategory(processingCategory));
                        #endregion
                    })
                    .WithFieldStates(fields =>
                    {
                        fields.Add<ATPTEFMMonthEnd.hold>(c => c.IsHiddenAlways());

                        fields.Add<ATPTEFMMonthEnd.finPeriodID>(c => c.IsDisabledWhen(!conditions.IsOnHold));
                        fields.Add<ATPTEFMMonthEnd.date>(c => c.IsDisabledWhen(!conditions.IsOnHold));
                        fields.Add<ATPTEFMMonthEnd.fundID>(c => c.IsDisabledWhen(!conditions.IsOnHold));
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
                        states.Add<State.releaseValue>(flowState =>
                        {
                            return flowState
                                .WithActions(actions =>
                                {
                                    actions.Add(g => g.PutOnHold, c => c.IsDuplicatedInToolbar().WithConnotation(ActionConnotation.Warning));
                                    actions.Add(g => g.Release, c => c.IsDuplicatedInToolbar().WithConnotation(ActionConnotation.Success));
                                });
                        });
                        //states.Add<State.closedValue>(flowState =>
                        //{
                        //    return flowState
                        //        .WithActions(actions =>
                        //        {
                        //        });
                        //});
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
                                .To<State.releaseValue>()
                                .IsTriggeredOn(g => g.RemoveFromHold)
                                .When(!conditions.IsOnHold));
                        });
                        transitions.AddGroupFrom<State.releaseValue>(ts =>
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
