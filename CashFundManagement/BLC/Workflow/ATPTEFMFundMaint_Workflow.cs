using System;
using PX.Data;
using PX.Objects.Common;
using PX.Data.WorkflowAPI;
using CashFundManagement.DAC;
using CashFundManagement.Attributes.Base;
using static CashFundManagement.BLC.ATPTEFMFundMaint;

namespace CashFundManagement.BLC.Workflow
{
    using static ATPTEFMFund;
    using static BoundedTo<ATPTEFMFundMaint, ATPTEFMFund>;
    using State = CashFundManagement.Attributes.Base.ATPTEFMBaseStatus;
    public class ATPTEFMFundMaint_Workflow : PXGraphExtension<ATPTEFMFundMaint>
    {
        public static bool IsActive() => true;

        public override void Configure(PXScreenConfiguration config) =>
           Configure(config.GetScreenConfigurationContext<ATPTEFMFundMaint, ATPTEFMFund>());

        #region BQL Conditions
        public class Conditions : Condition.Pack
        {
            public Condition IsOnHold => GetOrCreate(c => c.FromBql<
                hold.IsEqual<True>
            >());
            public Condition IsOpen => GetOrCreate(c => c.FromBql<
                status.IsEqual<ATPTEFMBaseStatus.openValue>
            >());
        }
        #endregion

        public static void DisableWholeScreen(FieldState.IContainerFillerFields fields)
        {
            fields.AddTable<ATPTEFMTransactionHistoryView>(c => c.IsDisabled());
        }
        protected virtual void Configure(WorkflowContext<ATPTEFMFundMaint, ATPTEFMFund> context)
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
                    .StateIdentifierIs<ATPTEFMFund.status>()
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
                        actions.Add(g => g.ReleaseDocument, c => c
                            .WithCategory(processingCategory));
                        //actions.Add(g => g.CloseFund, c => c
                        //    .WithCategory(processingCategory));
                        #endregion
                    })
                    .WithFieldStates(fields =>
                    {
                        fields.Add<ATPTEFMFund.hold>(c => c.IsHiddenAlways());

                        fields.Add<ATPTEFMFund.fundType>(c => c.IsDisabledWhen(!conditions.IsOnHold));
                        fields.Add<ATPTEFMFund.documentDate>(c => c.IsDisabledWhen(!conditions.IsOnHold));
                        fields.Add<ATPTEFMFund.descr>(c => c.IsDisabledWhen(!conditions.IsOnHold));
                        fields.Add<ATPTEFMFund.branchID>(c => c.IsDisabledWhen(!conditions.IsOnHold));
                        fields.Add<ATPTEFMFund.custodianID>(c => c.IsDisabledWhen(!conditions.IsOnHold));
                        fields.Add<ATPTEFMFund.payeeID>(c => c.IsDisabledWhen(!conditions.IsOnHold));
                        fields.Add<ATPTEFMFund.initialFund>(c => c.IsDisabledWhen(!conditions.IsOnHold));
                        fields.Add<ATPTEFMFund.replenishmentLimit>(c => c.IsDisabledWhen(!conditions.IsOnHold));
                        fields.Add<ATPTEFMFund.fundTransactionLimit>(c => c.IsDisabledWhen(!conditions.IsOnHold));
                        fields.Add<ATPTEFMFund.replenishmentAmt>(c => c.IsDisabledWhen(!conditions.IsOnHold));
                        fields.Add<ATPTEFMFund.replenishmentRestriction>(c => c.IsDisabledWhen(!conditions.IsOnHold));
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
                        states.Add<State.balancedValue>(flowState =>
                        {
                            return flowState
                                .WithActions(actions =>
                                {
                                    actions.Add(g => g.PutOnHold, c => c.IsDuplicatedInToolbar().WithConnotation(ActionConnotation.Success));
                                    actions.Add(g => g.ReleaseDocument, c => c.IsDuplicatedInToolbar().WithConnotation(ActionConnotation.Success));
                                })
                                .WithFieldStates(DisableWholeScreen);
                        });
                        states.Add<State.openValue>(flowState =>
                        {
                            return flowState
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
                                .To<State.balancedValue>()
                                .IsTriggeredOn(g => g.RemoveFromHold)
                                .When(!conditions.IsOnHold));
                        });
                        transitions.AddGroupFrom<State.balancedValue>(ts =>
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
