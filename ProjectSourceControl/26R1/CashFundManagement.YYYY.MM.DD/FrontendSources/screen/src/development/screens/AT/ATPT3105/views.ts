import { PXView, PXFieldState, gridConfig, treeConfig, fieldConfig, controlConfig, actionConfig, headerDescription, ICurrencyInfo, disabled, PXFieldOptions, linkCommand, columnConfig, GridColumnShowHideMode, GridColumnType, PXActionState, TextAlign, GridPreset, GridFilterBarVisibility, GridFastFilterVisibility } from "client-controls";


// Views

export class ATPTEFMProjectBudgetFilter extends PXView  {

	FinYear : PXFieldState<PXFieldOptions.CommitChanges>;
	LedgerID : PXFieldState<PXFieldOptions.CommitChanges>;
	ProjectID : PXFieldState<PXFieldOptions.CommitChanges>;
	LastModifiedByID : PXFieldState<PXFieldOptions.CommitChanges>;
	CompareFinYear : PXFieldState<PXFieldOptions.CommitChanges>;
	CompareLedgerID : PXFieldState<PXFieldOptions.CommitChanges>;
	CompareProjectID : PXFieldState<PXFieldOptions.CommitChanges>;
}

@gridConfig({
	syncPosition: true,
	showFastFilter: GridFastFilterVisibility.False,
	preset: GridPreset.Details,
	topBarItems: {
	distribute: {index: 0, config: {commandName: "distribute", text: "Distribute"}},
}
})
export class ATPTEFMProjectBudgetLineSummary extends PXView  {

	distribute : PXActionState;
	@columnConfig({width: 60, textAlign: TextAlign.Center, type: GridColumnType.CheckBox})	Released : PXFieldState;
	@columnConfig({width: 100, textAlign: TextAlign.Left})	ProjectTaskID : PXFieldState<PXFieldOptions.CommitChanges>;
	@columnConfig({width: 100, textAlign: TextAlign.Left})	AccountGroupID : PXFieldState;
	@columnConfig({width: 100, textAlign: TextAlign.Left})	CostCodeID : PXFieldState<PXFieldOptions.CommitChanges>;
	@columnConfig({width: 100, textAlign: TextAlign.Left})	Description : PXFieldState;
	@columnConfig({width: 100, textAlign: TextAlign.Right})	Amount : PXFieldState<PXFieldOptions.CommitChanges>;
	@columnConfig({width: 100, textAlign: TextAlign.Right})	DistributedAmount : PXFieldState<PXFieldOptions.CommitChanges>;
	@columnConfig({width: 100, textAlign: TextAlign.Right})	FinPeriod01 : PXFieldState;
	@columnConfig({width: 100, textAlign: TextAlign.Right})	FinPeriod02 : PXFieldState;
	@columnConfig({width: 100, textAlign: TextAlign.Right})	FinPeriod03 : PXFieldState;
	@columnConfig({width: 100, textAlign: TextAlign.Right})	FinPeriod04 : PXFieldState;
	@columnConfig({width: 100, textAlign: TextAlign.Right})	FinPeriod05 : PXFieldState;
	@columnConfig({width: 100, textAlign: TextAlign.Right})	FinPeriod06 : PXFieldState;
	@columnConfig({width: 100, textAlign: TextAlign.Right})	FinPeriod07 : PXFieldState;
	@columnConfig({width: 100, textAlign: TextAlign.Right})	FinPeriod08 : PXFieldState;
	@columnConfig({width: 100, textAlign: TextAlign.Right})	FinPeriod09 : PXFieldState;
	@columnConfig({width: 100, textAlign: TextAlign.Right})	FinPeriod10 : PXFieldState;
	@columnConfig({width: 100, textAlign: TextAlign.Right})	FinPeriod11 : PXFieldState;
	@columnConfig({width: 100, textAlign: TextAlign.Right})	FinPeriod12 : PXFieldState;
}

export class ATPTEFMBudgetDistributeFilter extends PXView  {

	Method : PXFieldState<PXFieldOptions.CommitChanges>;
	ApplyToAll : PXFieldState<PXFieldOptions.CommitChanges>;
	ApplyToSubGroups : PXFieldState<PXFieldOptions.CommitChanges>;
}

export class ATPTEFMPreloadBudgetFilter extends PXView  {

	FinYear : PXFieldState<PXFieldOptions.CommitChanges>;
	LedgerID : PXFieldState<PXFieldOptions.CommitChanges>;
	ProjectID : PXFieldState<PXFieldOptions.CommitChanges>;
	Mutliplier : PXFieldState<PXFieldOptions.CommitChanges>;
	@fieldConfig({
  controlType: "qp-radio",
  controlConfig: {
    class: "vertical",
    options: [
      {
        value: "1",
        text: "Update Existing Articles Only"
      },
      {
        value: "2",
        text: "Load Nonexistent Articles Only"
      },
      {
        value: "3",
        text: "Both"
      }
    ]
  }
})
	PreloadAction : PXFieldState<PXFieldOptions.CommitChanges>;
}