import { PXView, PXFieldState, gridConfig, treeConfig, fieldConfig, controlConfig, actionConfig, headerDescription, ICurrencyInfo, disabled, PXFieldOptions, linkCommand, columnConfig, GridColumnShowHideMode, GridColumnType, PXActionState, TextAlign, GridPreset, GridFilterBarVisibility, GridFastFilterVisibility } from "client-controls";


// Views

export class ATPTEFMReplenishmentFilter extends PXView  {

	EmployeeID : PXFieldState<PXFieldOptions.CommitChanges>;
}

@gridConfig({
	syncPosition: true,
	fastFilterByAllFields: false,
	showFastFilter: GridFastFilterVisibility.ToolBar,
	mergeToolbarWith: "ScreenToolbar",
	preset: GridPreset.Inquiry
})
export class ATPTEFMReplenishment extends PXView  {

	@columnConfig({width: 90})	Date : PXFieldState;
	@linkCommand("EditDetail")
	@columnConfig({width: 140, allowFastFilter: true})	ReplenishmentNbr : PXFieldState;
	Status : PXFieldState;
	FundID : PXFieldState;
	@columnConfig({width: 280, allowFastFilter: true})	Descr : PXFieldState;
	@columnConfig({width: 140})	CustodianID : PXFieldState;
	@columnConfig({width: 220})	CustodianID_EPEmployee_acctName : PXFieldState;
	@columnConfig({width: 220})	DepartmentID_EPDepartment_description : PXFieldState;
	@columnConfig({width: 280})	CreatedByID : PXFieldState;
	@columnConfig({width: 280})	CreatedDateTime : PXFieldState;
	@columnConfig({width: 250})	LastModifiedByID : PXFieldState;
	@columnConfig({width: 250})	LastModifiedDateTime : PXFieldState;
}