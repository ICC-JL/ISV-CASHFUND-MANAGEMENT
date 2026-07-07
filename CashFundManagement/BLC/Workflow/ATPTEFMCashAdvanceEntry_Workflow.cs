using System;
using PX.Data;
using PX.Objects.Common;
using PX.Data.WorkflowAPI;
using CashFundManagement.DAC;
using CashFundManagement.Attributes.Base;
using static CashFundManagement.BLC.ATPTEFMCashAdvanceEntry;
using CashFundManagement.Attributes;
using PX.Objects.AP;

namespace CashFundManagement.BLC.Workflow
{
    using static ATPTEFMCashAdvance;
    using static BoundedTo<ATPTEFMCashAdvanceEntry, ATPTEFMCashAdvance>;
    using State = ATPTEFMCashAdvanceStatusAttribute;
    /// <remarks>
    /// 2025-04-22 : Enable PutOnHold if PPM Bill from CA is rejected/voided : 011135 : RFS
    /// </remarks>
    public class ATPTEFMCashAdvanceEntry_Workflow : PXGraphExtension<ATPTEFMCashAdvanceEntry>
    {
        public static bool IsActive() => true;

        public override void Configure(PXScreenConfiguration config) =>
           Configure(config.GetScreenConfigurationContext<ATPTEFMCashAdvanceEntry, ATPTEFMCashAdvance>());

        #region BQL Conditions
        public class Conditions : Condition.Pack
        {
            public Condition IsOnHold => GetOrCreate(c => c.FromBql<
               hold.IsEqual<True>
            >());
            public Condition IsClosed => GetOrCreate(c => c.FromBql<
                status.IsEqual<ATPTEFMBaseStatus.closedValue>
            >());

            public Condition IsBillCreated => GetOrCreate(c => c.FromBql<
                billRefNbr.IsNotNull.And<billRefNbr.IsNotEqual<Empty>.And<billStatus.IsNotEqual<APDocStatus.voided>.And<billStatus.IsNotEqual<APDocStatus.rejected>>>>
            >());

            public Condition IsCancelled => GetOrCreate(c => c.FromBql<
                status.IsEqual<ATPTEFMBaseStatus.cancelledValue>
            >());

            public Condition IsImported => GetOrCreate(c => c.FromBql <
            isImported.IsEqual<True>>());

        }
        #endregion

        public static void DisableWholeScreen(FieldState.IContainerFillerFields fields)
        {
            fields.AddTable<ATPTEFMCARequestDetail>(c => c.IsDisabled());
        }
        public static void DisableReceiptTab(FieldState.IContainerFillerFields fields)
        {
            fields.AddTable<ATPTEFMCAReceiptDetail>(c => c.IsDisabled());
        }
        protected virtual void Configure(WorkflowContext<ATPTEFMCashAdvanceEntry, ATPTEFMCashAdvance> context)
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
                    .StateIdentifierIs<ATPTEFMCashAdvance.status>()
                    .AddDefaultFlow(DefaultDocumentFlow)
                    .WithActions(actions =>
                    {
                        actions.Add(g => g.InitializeState);

                        #region Processing
                        actions.Add(g => g.RemoveFromHold, c => c
                            .WithCategory(processingCategory)
                            .WithPersistOptions(ActionPersistOptions.PersistBeforeAction)
                            .WithFieldAssignments(fa => fa.Add<hold>(false))
                            .IsDisabledWhen(conditions.IsImported));
                        actions.Add(g => g.PutOnHold, c => c
                            .WithCategory(processingCategory)
                            .WithPersistOptions(ActionPersistOptions.PersistBeforeAction)
                            .WithFieldAssignments(fa => fa.Add<hold>(true))
                            .IsDisabledWhen(conditions.IsBillCreated));
                        actions.Add(g => g.CreateAPBill, c => c
                            .WithCategory(processingCategory)
                            .IsDisabledWhen(conditions.IsCancelled));
                        actions.Add(g => g.CACancel, c => c
                            .WithCategory(processingCategory)
                            .IsDisabledWhen(conditions.IsCancelled));
                        actions.Add(g => g.PendingForLiquidationImport, c => c
                            .WithCategory(processingCategory));
                        #endregion
                    })
                    .WithFieldStates(fields =>
                    {
                        fields.Add<ATPTEFMCashAdvance.hold>(c => c.IsHiddenAlways());
                        fields.Add<ATPTEFMCashAdvance.reqClassID>(c => c.IsDisabledWhen(!conditions.IsOnHold));
                        fields.Add<ATPTEFMCashAdvance.date>(c => c.IsDisabledWhen(!conditions.IsOnHold));
                        fields.Add<ATPTEFMCashAdvance.finPeriodID>(c => c.IsDisabledWhen(!conditions.IsOnHold));
                        fields.Add<ATPTEFMCashAdvance.descr>(c => c.IsDisabledWhen(!conditions.IsOnHold));
                        fields.Add<ATPTEFMCashAdvance.branchID>(c => c.IsDisabledWhen(!conditions.IsOnHold));
                        fields.Add<ATPTEFMCashAdvance.requestedByID>(c => c.IsDisabledWhen(!conditions.IsOnHold));
                        fields.Add<ATPTEFMCashAdvance.departmentID>(c => c.IsDisabledWhen(!conditions.IsOnHold));
                        fields.Add<ATPTEFMCashAdvance.dateOfUse>(c => c.IsDisabledWhen(!conditions.IsOnHold));
                        fields.Add<ATPTEFMCashAdvance.liqDate>(c => c.IsDisabledWhen(!conditions.IsOnHold));
                        fields.Add<ATPTEFMCashAdvance.initialLiqDate>(c => c.IsDisabledWhen(!conditions.IsOnHold));
                        fields.Add<ATPTEFMCashAdvance.requestedAmount>(c => c.IsDisabledWhen(!conditions.IsOnHold));
                        fields.Add<ATPTEFMCashAdvance.curyID>(c => c.IsDisabledWhen(!conditions.IsOnHold));
                        fields.Add<ATPTEFMCashAdvance.taxZoneID>(c => c.IsDisabledWhen(!conditions.IsOnHold));

                        //fields.Add<ATPTEFMCashAdvance.invoiceRefNbr>(c => c.IsDisabledWhen(conditions.True));
                        //fields.Add<ATPTEFMCashAdvance.status>(c => c.IsDisabledWhen(conditions.True));
                        //fields.Add<ATPTEFMCashAdvance.ppmType>(c => c.IsDisabledWhen(conditions.True));
                        //fields.Add<ATPTEFMCashAdvance.ppmRefNbr>(c => c.IsDisabledWhen(conditions.True));
                        //fields.Add<ATPTEFMCashAdvance.pmtType>(c => c.IsDisabledWhen(conditions.True));
                        //fields.Add<ATPTEFMCashAdvance.pmtRefNbr>(c => c.IsDisabledWhen(conditions.True));
                        //fields.Add<ATPTEFMCashAdvance.billType>(c => c.IsDisabledWhen(conditions.True));
                        //fields.Add<ATPTEFMCashAdvance.billRefNbr>(c => c.IsDisabledWhen(conditions.True));
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
                                    actions.Add(g => g.PendingForLiquidationImport, c => c.IsDuplicatedInToolbar().WithConnotation(ActionConnotation.Success));
                                });
                        });
                        states.Add<State.openValue>(flowState =>
                        {
                            return flowState
                                .WithActions(actions =>
                                {
                                    actions.Add(g => g.PutOnHold, c => c.IsDuplicatedInToolbar().WithConnotation(ActionConnotation.Warning));
                                    actions.Add(g => g.PendingForLiquidationImport, c => c.IsDuplicatedInToolbar().WithConnotation(ActionConnotation.Success));
                                })
                                .WithFieldStates(DisableWholeScreen)
                                .WithFieldStates(DisableReceiptTab);
                        });
                        states.Add<State.closedValue>(flowState =>
                        {
                            return flowState
                                .WithActions(actions =>
                                {
                                })
                                .WithFieldStates(DisableWholeScreen);
                        });
                        states.Add<State.pendingLiquidationValue>(flowState =>
                        {
                            return flowState
                                .WithActions(actions =>
                                {
                                });
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
                            ts.Add(t => t.To<State.pendingLiquidationValue>()
                                .IsTriggeredOn(g => g.PendingForLiquidationImport));
                        });
                        transitions.AddGroupFrom<State.openValue>(ts =>
                        {
                            ts.Add(t => t
                                .To<State.holdValue>()
                                .IsTriggeredOn(g => g.PutOnHold)
                                .When(conditions.IsOnHold));
                            ts.Add(t => t.To<State.pendingLiquidationValue>()
                                .IsTriggeredOn(g => g.PendingForLiquidationImport));
                        });
                    });
            }

            #endregion
        }
    }
}
