import { PXView, PXFieldState, gridConfig, treeConfig, fieldConfig, controlConfig, actionConfig, headerDescription, ICurrencyInfo, disabled, PXFieldOptions, linkCommand, columnConfig, GridColumnShowHideMode, GridColumnType, PXActionState, TextAlign, GridPreset, GridFilterBarVisibility, GridFastFilterVisibility } from "client-controls";


// Views

export class ATPTEFMFundTransactionFilter extends PXView  {

	EmployeeID : PXFieldState<PXFieldOptions.CommitChanges>;
}

@gridConfig({
	syncPosition: true,
	fastFilterByAllFields: false,
	showFastFilter: GridFastFilterVisibility.ToolBar,
	mergeToolbarWith: "ScreenToolbar",
	preset: GridPreset.Inquiry
})
export class ATPTEFMFundTransaction extends PXView  {

	@columnConfig({width: 90})	Date : PXFieldState;
	@linkCommand("EditDetail")
	@columnConfig({width: 140, allowFastFilter: true})	RefNbr : PXFieldState;
	FundType : PXFieldState;
	FundTransactionType : PXFieldState;
	@columnConfig({width: 140})	FundID : PXFieldState;
	Status : PXFieldState;
	CashAdvanceStatus : PXFieldState;
	@columnConfig({width: 280, allowFastFilter: true})	Descr : PXFieldState;
	@columnConfig({width: 140})	RequestedByID : PXFieldState;
	@columnConfig({width: 220})	RequestedByID_description : PXFieldState;
	@columnConfig({width: 220})	DepartmentID_EPDepartment_description : PXFieldState;
	@columnConfig({width: 280})	CreatedByID : PXFieldState;
	@columnConfig({width: 280})	CreatedDateTime : PXFieldState;
	@columnConfig({width: 250})	LastModifiedByID : PXFieldState;
	@columnConfig({width: 250})	LastModifiedDateTime : PXFieldState;
	@columnConfig({width: 100, textAlign: TextAlign.Right})	RequestedAmount : PXFieldState;
	@columnConfig({width: 100, textAlign: TextAlign.Right})	ActualSpentAmount : PXFieldState;
	@columnConfig({width: 100, textAlign: TextAlign.Right})	TotalWhtAmount : PXFieldState;
	@columnConfig({width: 100, textAlign: TextAlign.Right})	ChangeAmount : PXFieldState;
}