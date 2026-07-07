import {
  PXView,
  PXFieldState,
  gridConfig,
  treeConfig,
  fieldConfig,
  controlConfig,
  actionConfig,
  headerDescription,
  ICurrencyInfo,
  disabled,
  PXFieldOptions,
  linkCommand,
  columnConfig,
  GridColumnShowHideMode,
  GridColumnType,
  PXActionState,
  TextAlign,
  GridPreset,
  GridFilterBarVisibility,
  GridFastFilterVisibility,
} from "client-controls";

// Views

export class ATPTEFMFund extends PXView {
  FundType: PXFieldState<PXFieldOptions.CommitChanges>;
  FundCD: PXFieldState;
  Status: PXFieldState;
  Hold: PXFieldState<PXFieldOptions.CommitChanges>;
  Approved: PXFieldState<PXFieldOptions.CommitChanges>;
  DocumentDate: PXFieldState;
  Descr: PXFieldState<PXFieldOptions.CommitChanges | PXFieldOptions.Multiline>;
  BranchID: PXFieldState<PXFieldOptions.CommitChanges>;
  CustodianID: PXFieldState<PXFieldOptions.CommitChanges>;
  CuryID: PXFieldState;
  PayeeID: PXFieldState<PXFieldOptions.CommitChanges>;
  CuryInitialFund: PXFieldState<PXFieldOptions.CommitChanges>;
  CuryFundAmt: PXFieldState;
  CuryBalanceAmt: PXFieldState;
  CuryOnReplenishmentAmt: PXFieldState;
  CuryLiquidatedAmt: PXFieldState;
  CuryUnliquidatedAmt: PXFieldState;
}

export class ATPTEFMFund2 extends PXView {
  @controlConfig({ linkCommand: "ViewInvoice" })
  EstablishmentRefNbr: PXFieldState<PXFieldOptions.Disabled>;
  @controlConfig({ linkCommand: "ViewCloseFundInvoice" })
  CloseFundRefNbr: PXFieldState<PXFieldOptions.Disabled>;
  @controlConfig({ linkCommand: "ViewExpenseBatchNbr" })
  ExpenseBatchNbr: PXFieldState<PXFieldOptions.Disabled>;
  ProjectID: PXFieldState<PXFieldOptions.CommitChanges>;
  ProjectTaskID: PXFieldState<PXFieldOptions.CommitChanges>;
  CostCodeID: PXFieldState<PXFieldOptions.CommitChanges>;
  AccountID: PXFieldState;
  SubID: PXFieldState;
  ClearingAccount: PXFieldState;
  ClearingSubaccount: PXFieldState;
  ReplenishmentLimit: PXFieldState<PXFieldOptions.CommitChanges>;
  ReplenishPointPercent: PXFieldState<PXFieldOptions.CommitChanges>;
  ReplenishmentAmt: PXFieldState<PXFieldOptions.CommitChanges>;
  ReplenishmentRestriction: PXFieldState;
  FundTransactionLimit: PXFieldState<PXFieldOptions.CommitChanges>;
  FundTransactionPointPercent: PXFieldState<PXFieldOptions.CommitChanges>;
  FundTransactionAmt: PXFieldState<PXFieldOptions.CommitChanges>;
  FundTransactionRestriction: PXFieldState<PXFieldOptions.CommitChanges>;
}

@gridConfig({
  syncPosition: true,
  showFastFilter: GridFastFilterVisibility.False,
  preset: GridPreset.ReadOnly,
})
export class ATPTEFMFundTransactionHistoryView extends PXView {
  TransactionType: PXFieldState<PXFieldOptions.CommitChanges>;
  @linkCommand("viewTransaction")
  RefNbr: PXFieldState;
  FundBranchID: PXFieldState;
  FundType: PXFieldState;
  Status: PXFieldState<PXFieldOptions.CommitChanges>;
  TransactionDate: PXFieldState;
  @columnConfig({ textAlign: TextAlign.Right })
  CuryFundTransactionDocumentAmt: PXFieldState;
  @columnConfig({ textAlign: TextAlign.Right })
  CuryWithholdingTax: PXFieldState;
  @columnConfig({ textAlign: TextAlign.Right })
  CuryUnliquidatedAmt: PXFieldState;
  @columnConfig({ textAlign: TextAlign.Right }) CuryLiquidatedAmt: PXFieldState;
  @columnConfig({ textAlign: TextAlign.Right }) CuryFundReturnAmt: PXFieldState;
  @columnConfig({ textAlign: TextAlign.Right }) CuryBalanceAmt: PXFieldState;
  @linkCommand("viewCheck")
  CheckNbr: PXFieldState;
  @columnConfig({ textAlign: TextAlign.Right }) CuryCheckAmt: PXFieldState;
  @linkCommand("viewReverseBatch")
  ReversingJournalBatchNbr: PXFieldState;
  @linkCommand("viewReplenishmentER")
  ReplenishmentRefNbr: PXFieldState;
  ProjectID: PXFieldState<PXFieldOptions.CommitChanges>;
  ProjectTaskID: PXFieldState<PXFieldOptions.CommitChanges>;
}

@gridConfig({
  autoAdjustColumns: true,
  showFastFilter: GridFastFilterVisibility.False,
  preset: GridPreset.ReadOnly,
})
export class APInvoice extends PXView {
  DocType: PXFieldState;
  @linkCommand("viewUnreplenishedBill")
  @columnConfig({ hideViewLink: true })
  RefNbr: PXFieldState;
  @columnConfig({ textAlign: TextAlign.Right }) CuryOrigDocAmt: PXFieldState;
  TaxZoneID: PXFieldState;
  TaxCalcMode: PXFieldState;
  Status: PXFieldState;
}

@gridConfig({
  syncPosition: true,
  showFastFilter: GridFastFilterVisibility.False,
  preset: GridPreset.ReadOnly,
})
export class APPayment extends PXView {
  DocType: PXFieldState;
  @linkCommand("viewLinkToCheck")
  @columnConfig({ hideViewLink: true })
  RefNbr: PXFieldState;
  @columnConfig({ textAlign: TextAlign.Right }) CuryOrigDocAmt: PXFieldState;
  Status: PXFieldState;
  CuryID: PXFieldState;
}

@gridConfig({
  allowDelete: false,
  allowInsert: false,
  showFastFilter: GridFastFilterVisibility.False,
  preset: GridPreset.Details,
})
export class EPApproval extends PXView {
  ApproverEmployee__AcctCD: PXFieldState;
  ApproverEmployee__AcctName: PXFieldState;
  ApprovedByEmployee__AcctCD: PXFieldState;
  ApprovedByEmployee__AcctName: PXFieldState;
  ApproveDate: PXFieldState;
  @columnConfig({ allowUpdate: false, allowNull: false }) Status: PXFieldState;
  WorkgroupID: PXFieldState;
}

export class ATPTEFMIncreaseFund extends PXView {
  IncreaseFund: PXFieldState<PXFieldOptions.CommitChanges>;
}

export class ATPTEFMDecreaseFund extends PXView {
  DecreaseFund: PXFieldState<PXFieldOptions.CommitChanges>;
}
