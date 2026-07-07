import { PXView, PXFieldState, gridConfig, treeConfig, fieldConfig, controlConfig, actionConfig, headerDescription, ICurrencyInfo, disabled, PXFieldOptions, linkCommand, columnConfig, GridColumnShowHideMode, GridColumnType, PXActionState, TextAlign, GridPreset, GridFilterBarVisibility, GridFastFilterVisibility } from "client-controls";


// Views

export class ATPTEFMFundTransaction extends PXView  {

	FundType : PXFieldState<PXFieldOptions.CommitChanges>;
	RefNbr : PXFieldState<PXFieldOptions.CommitChanges>;
	FundID : PXFieldState<PXFieldOptions.CommitChanges>;
	BranchID : PXFieldState<PXFieldOptions.CommitChanges>;
	FundTransactionType : PXFieldState<PXFieldOptions.CommitChanges>;
	Status : PXFieldState<PXFieldOptions.CommitChanges>;
	Hold : PXFieldState<PXFieldOptions.CommitChanges>;
	Approved : PXFieldState;
	IsImported : PXFieldState;
	IsOverbudget : PXFieldState<PXFieldOptions.CommitChanges>;
	HasInitialBudget : PXFieldState<PXFieldOptions.CommitChanges>;
	BudgetEnabled : PXFieldState;
	ProjectBudgetEnabled : PXFieldState;
	Date : PXFieldState<PXFieldOptions.CommitChanges>;
	FinPeriodID : PXFieldState<PXFieldOptions.CommitChanges>;
	Descr : PXFieldState<PXFieldOptions.CommitChanges | PXFieldOptions.Multiline>;
	RequestApproval : PXFieldState;
	RequestedByID : PXFieldState<PXFieldOptions.CommitChanges>;
	DepartmentID : PXFieldState<PXFieldOptions.CommitChanges>;
	CashAdvanceStatus : PXFieldState<PXFieldOptions.CommitChanges>;
	DateOfUse : PXFieldState<PXFieldOptions.CommitChanges>;
	InitialLiqDate : PXFieldState<PXFieldOptions.CommitChanges>;
	LiqDate : PXFieldState<PXFieldOptions.CommitChanges>;
	ShowBudgetValidation : PXFieldState;
	NoFund : PXFieldState<PXFieldOptions.CommitChanges>;
	RequestedAmount : PXFieldState;
	ActualSpentAmount : PXFieldState;
	TotalWhtAmount : PXFieldState;
	ChangeAmount : PXFieldState;
	AmountReceived : PXFieldState<PXFieldOptions.CommitChanges>;
	AmountReleased : PXFieldState<PXFieldOptions.CommitChanges>;
	ReclassificationAmt : PXFieldState<PXFieldOptions.CommitChanges>;
	Balance : PXFieldState<PXFieldOptions.CommitChanges>;
}

@gridConfig({
	preset: GridPreset.Details,
	initNewRow: true
})
export class ATPTEFMFundTransactionDetail extends PXView  {

	SubID : PXFieldState;
	Date : PXFieldState;
	Particulars : PXFieldState<PXFieldOptions.CommitChanges>;
	InventoryID : PXFieldState<PXFieldOptions.CommitChanges>;
	Description : PXFieldState;
	LineDescription : PXFieldState;
	Qty : PXFieldState<PXFieldOptions.CommitChanges>;
	UnitRecordID : PXFieldState;
	UnitCost : PXFieldState<PXFieldOptions.CommitChanges>;
	Amount : PXFieldState<PXFieldOptions.CommitChanges>;
	AccountID : PXFieldState<PXFieldOptions.CommitChanges>;
	AccountDescription : PXFieldState;
	AccountGroup : PXFieldState<PXFieldOptions.CommitChanges>;
	ProjectID : PXFieldState<PXFieldOptions.CommitChanges>;
	ProjectTaskID : PXFieldState<PXFieldOptions.CommitChanges>;
	CostCodeID : PXFieldState<PXFieldOptions.CommitChanges>;
}

@gridConfig({
	preset: GridPreset.Details,
	initNewRow: true
})
export class ATPTEFMFundTransactionReceiptDetail extends PXView  {

	LoadRequest: PXActionState;

	VendID : PXFieldState<PXFieldOptions.CommitChanges>;
	SubID : PXFieldState<PXFieldOptions.CommitChanges>;
	@linkCommand("OpenTransaction")
	ExpenseReceiptRefNbr : PXFieldState;
	BranchID : PXFieldState<PXFieldOptions.CommitChanges>;
	Date : PXFieldState;
	InventoryID : PXFieldState<PXFieldOptions.CommitChanges>;
	Descr : PXFieldState<PXFieldOptions.CommitChanges>;
	LineDescription : PXFieldState<PXFieldOptions.CommitChanges>;
	Qty : PXFieldState<PXFieldOptions.CommitChanges>;
	InventoryItem__BaseUnit : PXFieldState<PXFieldOptions.CommitChanges>;
	UnitCost : PXFieldState<PXFieldOptions.CommitChanges>;
	Amount : PXFieldState<PXFieldOptions.CommitChanges>;
	AccountID : PXFieldState<PXFieldOptions.CommitChanges>;
	AccountDescription : PXFieldState;
	AccountGroup : PXFieldState<PXFieldOptions.CommitChanges>;
	ProjectID : PXFieldState<PXFieldOptions.CommitChanges>;
	ProjectTaskID : PXFieldState<PXFieldOptions.CommitChanges>;
	CostCodeID : PXFieldState;
	RefNbr : PXFieldState<PXFieldOptions.CommitChanges>;
	VendorName : PXFieldState<PXFieldOptions.CommitChanges>;
	VendorAddress : PXFieldState<PXFieldOptions.CommitChanges>;
	VendorTin : PXFieldState<PXFieldOptions.CommitChanges>;
	TaxZoneID : PXFieldState<PXFieldOptions.CommitChanges>;
	AtcCode : PXFieldState;
	TaxCategoryID : PXFieldState;
	NetQty : PXFieldState<PXFieldOptions.CommitChanges>;
	NetUnitCost : PXFieldState<PXFieldOptions.CommitChanges>;
	NetAmt : PXFieldState<PXFieldOptions.CommitChanges>;
	WhtAmount : PXFieldState<PXFieldOptions.CommitChanges>;
	InventoryItem__ItemType : PXFieldState;
}

export class ATPTEFMFundTransaction2 extends PXView  {

	ReceivedAmount : PXFieldState<PXFieldOptions.CommitChanges>;
	ReleasedAmount : PXFieldState<PXFieldOptions.CommitChanges>;
}

export class ATPTEFMFundTransactionReclassficationReceiptDetail extends PXView  {

	SubID : PXFieldState<PXFieldOptions.CommitChanges>;
	@linkCommand("OpenReclassReceipt")
	ExpenseReceiptRefNbr : PXFieldState;
	Date : PXFieldState<PXFieldOptions.CommitChanges>;
	InventoryID : PXFieldState<PXFieldOptions.CommitChanges>;
	InventoryItem__BaseUnit : PXFieldState<PXFieldOptions.CommitChanges>;
	AccountID : PXFieldState<PXFieldOptions.CommitChanges>;
	AccountDescription : PXFieldState;
	ProjectID : PXFieldState<PXFieldOptions.CommitChanges>;
	ProjectTaskID : PXFieldState<PXFieldOptions.CommitChanges>;
	CostCodeID : PXFieldState;
	RefNbr : PXFieldState;
	TaxZoneID : PXFieldState;
	TaxCategoryID : PXFieldState;
	NetQty : PXFieldState<PXFieldOptions.CommitChanges>;
	NetUnitCost : PXFieldState<PXFieldOptions.CommitChanges>;
	NetAmt : PXFieldState<PXFieldOptions.CommitChanges>;
	WhtAmount : PXFieldState<PXFieldOptions.CommitChanges>;
	InventoryItem__ItemType : PXFieldState;
}

export class EPApproval extends PXView  {

	ApproverEmployee__AcctCD : PXFieldState;
	ApproverEmployee__AcctName : PXFieldState;
	ApprovedByEmployee__AcctCD : PXFieldState;
	ApprovedByEmployee__AcctName : PXFieldState;
	ApproveDate : PXFieldState;
	@columnConfig({allowUpdate: false, allowNull: false})	Status : PXFieldState;
	WorkgroupID : PXFieldState;
}

export class ATPTEFMBudget extends PXView  {

	AcctID : PXFieldState;
	SubID : PXFieldState;
	CuryID : PXFieldState;
	DocAmt : PXFieldState;
	InitialBudget : PXFieldState;
	BudgetAmt : PXFieldState<PXFieldOptions.CommitChanges>;
	RequestAmt : PXFieldState;
	ApprovedAmt : PXFieldState;
	UnapprovedAmt : PXFieldState;
	SpentAmt : PXFieldState;
	ReturnAmt : PXFieldState;
}

export class ATPTEFMPBudget extends PXView  {

	ProjectID : PXFieldState;
	ProjectTaskID : PXFieldState;
	CostCodeID : PXFieldState;
	AccountGroupID : PXFieldState;
	CuryID : PXFieldState;
	DocAmt : PXFieldState;
	InitialBudget : PXFieldState;
	BudgetAmt : PXFieldState;
	RequestAmt : PXFieldState;
	ApprovedAmt : PXFieldState;
	UnapprovedAmt : PXFieldState;
	SpentAmt : PXFieldState;
	ReturnAmt : PXFieldState;
}

@gridConfig({
	preset: GridPreset.ReadOnly,
	allowUpdate: false,
})
export class ATPTEFMFundTransactionDetail2 extends PXView  {

	@columnConfig({ allowCheckAll: true })
	Selected : PXFieldState<PXFieldOptions.CommitChanges>;
	InventoryID : PXFieldState;
	InventoryID_description : PXFieldState;
	LineDescription : PXFieldState;
	RunningQty : PXFieldState;
	UnitCost : PXFieldState;
	Balance : PXFieldState;
}