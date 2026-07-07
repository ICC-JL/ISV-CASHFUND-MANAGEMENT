import { PXView, PXFieldState, gridConfig, treeConfig, fieldConfig, controlConfig, actionConfig, headerDescription, ICurrencyInfo, disabled, PXFieldOptions, linkCommand, columnConfig, GridColumnShowHideMode, GridColumnType, PXActionState, TextAlign, GridPreset, GridFilterBarVisibility, GridFastFilterVisibility } from "client-controls";


// Views

@gridConfig({
	showFastFilter: GridFastFilterVisibility.False,
	mergeToolbarWith: "ScreenToolbar",
	preset: GridPreset.Primary
})
export class ATPTEFMFundTransaction extends PXView  {

	@columnConfig({width: 60, textAlign: TextAlign.Center, type: GridColumnType.CheckBox})	Selected : PXFieldState;
	@columnConfig({width: 140})	RefNbr : PXFieldState;
	@columnConfig({width: 280})	RequestedByID : PXFieldState;
	@columnConfig({width: 280})	ReclassifyBalanceAmt : PXFieldState;
	@columnConfig({width: 100, textAlign: TextAlign.Right})	LiqDate : PXFieldState;
}

export class ATPTEFMFundRequestExtendFilter extends PXView  {

	Days : PXFieldState<PXFieldOptions.CommitChanges>;
}