import { PXView, PXFieldState, gridConfig, treeConfig, fieldConfig, controlConfig, actionConfig, headerDescription, ICurrencyInfo, disabled, PXFieldOptions, linkCommand, columnConfig, GridColumnShowHideMode, GridColumnType, PXActionState, TextAlign, GridPreset, GridFilterBarVisibility, GridFastFilterVisibility } from "client-controls";


// Views

@gridConfig({
	showFastFilter: GridFastFilterVisibility.False,
	mergeToolbarWith: "ScreenToolbar",
	preset: GridPreset.Primary
})
export class ATPTEFMCashAdvance extends PXView  {

	@columnConfig({width: 60, textAlign: TextAlign.Center, type: GridColumnType.CheckBox})	Selected : PXFieldState;
	@columnConfig({width: 140})	BranchID : PXFieldState;
	@columnConfig({width: 140})	CashAdvanceNbr : PXFieldState;
	@columnConfig({width: 280})	Descr : PXFieldState;
	@columnConfig({width: 280})	RequestedByID_description : PXFieldState;
	@columnConfig({width: 100, textAlign: TextAlign.Right})	CuryChangeAmount : PXFieldState;
	@columnConfig({width: 90})	CuryID : PXFieldState;
	@columnConfig({width: 100})	LiqDate : PXFieldState;
}

export class ATPTEFMCashAdvanceExtendFilter extends PXView  {

	Days : PXFieldState<PXFieldOptions.CommitChanges>;
}