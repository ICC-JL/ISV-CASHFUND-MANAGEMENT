import { PXView, PXFieldState, gridConfig, treeConfig, fieldConfig, controlConfig, actionConfig, headerDescription, ICurrencyInfo, disabled, PXFieldOptions, linkCommand, columnConfig, GridColumnShowHideMode, GridColumnType, PXActionState, TextAlign, GridPreset, GridFilterBarVisibility, GridFastFilterVisibility, ISelectorControlConfig, ControlParameter } from "client-controls";
import { ATPT3104 } from "./ATPT3104";

export class EPApprovalType {
	AcctCD : PXFieldState;	
	AcctName : PXFieldState;	
}

// Views

export class ATPTEFMMonthEnd extends PXView  {

	RefNbr : PXFieldState;
	Status : PXFieldState;
	Hold : PXFieldState<PXFieldOptions.CommitChanges>;
	Approved : PXFieldState;
	FinPeriodID : PXFieldState<PXFieldOptions.CommitChanges>;
	Date : PXFieldState<PXFieldOptions.CommitChanges>;
	RequestApproval : PXFieldState;
	FundID : PXFieldState<PXFieldOptions.CommitChanges>;
	AccountID : PXFieldState;
	SubID : PXFieldState;
	CreditAccountID : PXFieldState;
	CuryID : PXFieldState;
	Amount : PXFieldState<PXFieldOptions.CommitChanges>;
}

@gridConfig({
	syncPosition: true,
	allowDelete : false,
	allowInsert: false,
	showFastFilter: GridFastFilterVisibility.False,
	preset: GridPreset.ReadOnly
})
export class EPExpenseClaimDetails extends PXView  {

	@columnConfig({allowNull: false, width: 30})	Selected : PXFieldState;
	ClaimDetailCD : PXFieldState;
	@columnConfig({width: 90})	ExpenseDate : PXFieldState;
	InventoryID : PXFieldState;
	@columnConfig({width: 120})	ExpenseAccountID : PXFieldState;
	@columnConfig({width: 140})	ExpenseSubID : PXFieldState;
	ContractID : PXFieldState;
	TaskID : PXFieldState;
	CostCodeID : PXFieldState;
	@columnConfig({width: 100})	CuryExtCost : PXFieldState;
	@columnConfig({width: 280})	TranDesc : PXFieldState;
}

@gridConfig({
	autoAdjustColumns: true,
	showFastFilter: GridFastFilterVisibility.False,
	preset: GridPreset.Details,
	topBarItems: {
	ShowSubmitReceipt: {index: 0, config: {commandName: "ShowSubmitReceipt", text: "Add Receipts"}},
}
})
export class ATPTEFMMonthEndDetail extends PXView  {

	ShowSubmitReceipt : PXActionState;
	@columnConfig({width: 140})	BranchID : PXFieldState;
	@columnConfig({width: 140})	ExpenseReceiptRefNbr : PXFieldState;
	InventoryID : PXFieldState;
	@columnConfig({width: 120})	AccountID : PXFieldState;
	@columnConfig({width: 140})	SubID : PXFieldState;
	ContractID : PXFieldState;
	TaskID : PXFieldState;
	CostCodeID : PXFieldState;
	Amount : PXFieldState;
}
export class ATPTEFMMonthEnd2 extends PXView  {

	@controlConfig({ linkCommand: "ViewBatch" })
	JournalBatchNbr : PXFieldState;
	@controlConfig({ linkCommand: "ViewReversingBatch" })
	ReversingJournalBatchNbr : PXFieldState;
	BranchID : PXFieldState<PXFieldOptions.CommitChanges>;
}

@gridConfig({
	allowDelete : false,
	allowInsert: false,
	showFastFilter: GridFastFilterVisibility.False,
	preset: GridPreset.Details
})
export class EPApproval extends PXView  {

	@columnConfig({width: 160})	ApproverEmployee : EPApprovalType;
	@columnConfig({width: 100})	ApprovedByEmployee : EPApprovalType;
	@columnConfig({width: 90})	ApproveDate : PXFieldState;
	@columnConfig({allowUpdate : false, allowNull: false})	Status : PXFieldState;
	@columnConfig({width: 150})	WorkgroupID : PXFieldState;
}

export class CurrencyInfo extends PXView implements ICurrencyInfo {

	CuryInfoID : PXFieldState;
	BaseCuryID : PXFieldState;
	BaseCalc : PXFieldState;
	DisplayCuryID : PXFieldState;
	CuryRateTypeID : PXFieldState;
	BasePrecision : PXFieldState;
	CuryRate : PXFieldState;
	CuryEffDate : PXFieldState;
	RecipRate : PXFieldState;
	SampleCuryRate : PXFieldState;
	SampleRecipRate : PXFieldState;
	CuryID : PXFieldState;
}