import { PXView, PXFieldState, gridConfig, treeConfig, fieldConfig, controlConfig, actionConfig, headerDescription, ICurrencyInfo, disabled, PXFieldOptions, linkCommand, columnConfig, GridColumnShowHideMode, GridColumnType, PXActionState, TextAlign, GridPreset, GridFilterBarVisibility, GridFastFilterVisibility } from "client-controls";


// Views

export class ATPTEFMFundFilter extends PXView  {

	EmployeeID : PXFieldState<PXFieldOptions.CommitChanges>;
}

@gridConfig({
	syncPosition: true,
	fastFilterByAllFields: false,
	showFastFilter: GridFastFilterVisibility.ToolBar,
	mergeToolbarWith: "ScreenToolbar",
	preset: GridPreset.Inquiry
})
export class ATPTEFMFund extends PXView  {

	FundType : PXFieldState;
	@linkCommand("EditDetail")
	@columnConfig({width: 140, allowFastFilter: true})	FundCD : PXFieldState;
	@columnConfig({width: 140})	CustodianID : PXFieldState;
	@columnConfig({ width: 220 }) EmployeeName: PXFieldState;
	@columnConfig({ width: 140 }) PayeeID: PXFieldState;
	@columnConfig({ width: 220 }) PayeeName: PXFieldState;
	@columnConfig({width: 280, allowFastFilter: true})	Descr : PXFieldState;
	Status : PXFieldState;
	CuryID : PXFieldState;
	@columnConfig({width: 100, textAlign: TextAlign.Right})	CuryInitialFund : PXFieldState;
	@columnConfig({width: 90})	DocumentDate : PXFieldState;
	@columnConfig({width: 250})	LastModifiedByID : PXFieldState;
}