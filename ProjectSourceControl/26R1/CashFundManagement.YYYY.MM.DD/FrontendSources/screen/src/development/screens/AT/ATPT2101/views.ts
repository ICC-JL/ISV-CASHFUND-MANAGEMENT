import { PXView, PXFieldState, gridConfig, treeConfig, fieldConfig, controlConfig, actionConfig, headerDescription, ICurrencyInfo, disabled, PXFieldOptions, linkCommand, columnConfig, GridColumnShowHideMode, GridColumnType, PXActionState, TextAlign, GridPreset, GridFilterBarVisibility, GridFastFilterVisibility } from "client-controls";


// Views

export class ATPTEFMCashAdvanceFilter extends PXView  {

	EmployeeID : PXFieldState<PXFieldOptions.CommitChanges>;
}

@gridConfig({
	syncPosition: true,
	fastFilterByAllFields: false,
	showFastFilter: GridFastFilterVisibility.ToolBar,
	mergeToolbarWith: "ScreenToolbar",
	preset: GridPreset.Inquiry
})
export class ATPTEFMCashAdvance extends PXView  {

	@columnConfig({width: 120})	ReqClassID : PXFieldState;
	@linkCommand("EditDetail")
	@columnConfig({width: 140, allowFastFilter: true})	CashAdvanceNbr : PXFieldState;
	Status : PXFieldState;
	@columnConfig({width: 90})	Date : PXFieldState;
	@columnConfig({width: 90})	DateOfUse : PXFieldState;
	@columnConfig({width: 90})	LiqDate : PXFieldState;
	@columnConfig({width: 72})	FinPeriodID : PXFieldState;
	@columnConfig({width: 280, allowFastFilter: true})	Descr : PXFieldState;
	@columnConfig({width: 140})	RequestedByID : PXFieldState;
	@columnConfig({width: 220})	RequestedByID_description : PXFieldState;
	@columnConfig({width: 220})	DepartmentID_EPDepartment_description : PXFieldState;
	@columnConfig({width: 280})	CreatedByID : PXFieldState;
	@columnConfig({width: 280})	CreatedDateTime : PXFieldState;
	@columnConfig({width: 250})	LastModifiedByID : PXFieldState;
	@columnConfig({width: 250})	LastModifiedDateTime : PXFieldState;
}