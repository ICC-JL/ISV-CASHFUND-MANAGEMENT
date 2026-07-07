import { PXView, PXFieldState, PXFieldOptions } from "client-controls";

// Primary view bound to the form area
export class ReqClasses extends PXView {
  TranType: PXFieldState<PXFieldOptions.CommitChanges>;
  ReqClassID: PXFieldState<PXFieldOptions.CommitChanges>;
  NumberingID: PXFieldState;
  Descr: PXFieldState;
  RestrictItemList: PXFieldState;
  EnableDocumentOverride: PXFieldState;
  RestrictMultInvIns: PXFieldState;
  NoDaysLiquidate: PXFieldState;
}

// Secondary view used by GL Accounts tab/section
export class CurrentReqClass extends PXView {
  UseExpenseAcctFrom: PXFieldState;
  CombineExpSub: PXFieldState;
  ExpenseAcctID: PXFieldState<PXFieldOptions.CommitChanges>;
  ExpenseSubID: PXFieldState;
}

// Grid view for the Item List
export class Items extends PXView {
  InventoryID: PXFieldState;
  InventoryID_InventoryItem_descr: PXFieldState;
  Amount: PXFieldState<PXFieldOptions.CommitChanges>;
  IsPerDiem: PXFieldState<PXFieldOptions.CommitChanges>;
}
