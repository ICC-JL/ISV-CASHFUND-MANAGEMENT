import { PXView, PXFieldState, PXFieldOptions } from "client-controls";

export class ATPTEFMFeaturesSetup extends PXView {
  // Budget Features
  BudgetModules: PXFieldState<PXFieldOptions.CommitChanges>;
  BudgetFeatureSet: PXFieldState<PXFieldOptions.CommitChanges>;
  BudgetDocumentAmount: PXFieldState;
  BudgetRequestAmount: PXFieldState;
  BudgetBudgetAmount: PXFieldState;
  BudgetSpentAmount: PXFieldState;
  BudgetReturnAmount: PXFieldState;
  BudgetApprovedAmount: PXFieldState;
  BudgetUnapprovedAmount: PXFieldState;
  BudgetLedgerID: PXFieldState<PXFieldOptions.CommitChanges>;
  BudgetCalculation: PXFieldState;
  BudgetValidation: PXFieldState;

  // Project Budget Features
  ProjectBudgetModules: PXFieldState<PXFieldOptions.CommitChanges>;
  ProjectBudgetFeatureSet: PXFieldState<PXFieldOptions.CommitChanges>;
  ProjectBudgetDocumentAmount: PXFieldState;
  ProjectBudgetRequestAmount: PXFieldState;
  ProjectBudgetReturnAmount: PXFieldState;
  ProjectBudgetBudgetAmount: PXFieldState;
  ProjectBudgetSpentAmount: PXFieldState;
  ProjectBudgetApprovedAmount: PXFieldState;
  ProjectBudgetUnapprovedAmount: PXFieldState;
  ProjectBudgetLedgerID: PXFieldState<PXFieldOptions.CommitChanges>;
  ProjectBudgetCalculation: PXFieldState;
  ProjectBudgetValidation: PXFieldState;

  // Transfer Asset
  TransferAssetCustodian: PXFieldState;
  TransferAssetBuilding: PXFieldState;
  TransferAssetFloor: PXFieldState;
  TransferAssetRoom: PXFieldState;

  // Fund Transaction
  LimitValidation: PXFieldState<PXFieldOptions.CommitChanges>;
  LimitValidationAmt: PXFieldState<PXFieldOptions.CommitChanges>;

  // Cash Advance
  CashAdvanceOverride: PXFieldState;
}
