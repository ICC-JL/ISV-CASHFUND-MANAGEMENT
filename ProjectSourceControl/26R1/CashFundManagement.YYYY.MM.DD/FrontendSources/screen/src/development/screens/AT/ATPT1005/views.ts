import { PXView, PXFieldState, gridConfig, treeConfig, fieldConfig, controlConfig, actionConfig, headerDescription, ICurrencyInfo, disabled, PXFieldOptions, linkCommand, columnConfig, GridColumnShowHideMode, GridColumnType, PXActionState, TextAlign, GridPreset, GridFilterBarVisibility, GridFastFilterVisibility, GridColumnDisplayMode } from "client-controls";


// Views

export class ATPTEFMSetup extends PXView  {

	@controlConfig({allowEdit:true})
	FundNumberingID : PXFieldState;
	@controlConfig({allowEdit:true})
	FundTransactionNumberingID : PXFieldState;
	@controlConfig({allowEdit:true})
	ReplenishmentNumberingID : PXFieldState;
	@controlConfig({allowEdit:true})
	MonthEndNumberingID : PXFieldState;
	ReplenishmentLimit : PXFieldState;
	FundTransactionLimit : PXFieldState;
	@controlConfig({allowEdit:true})
	PCFAccount : PXFieldState;
	@controlConfig({allowEdit:true})
	PCFSubaccount : PXFieldState;
	@controlConfig({allowEdit:true})
	RVFAccount : PXFieldState;
	@controlConfig({allowEdit:true})
	RVFSubaccount : PXFieldState;
	@controlConfig({allowEdit:true})
	ClearingAccount : PXFieldState;
	@controlConfig({allowEdit:true})
	ClearingSubaccount : PXFieldState;
	NoOfDaysToLiquidate : PXFieldState<PXFieldOptions.CommitChanges>;
	@controlConfig({allowEdit:true})
	ReclassificationItem : PXFieldState<PXFieldOptions.CommitChanges>;
	LiquidationDateBasedOnWorkCalendar : PXFieldState;
	FundsApprovalSetup : PXFieldState;
	FundTransactionRequestApproval : PXFieldState;
	ReplenishmentRequestApproval : PXFieldState;
	MonthEndRequestApproval : PXFieldState;
	IsFilterByEmployeeDelegates : PXFieldState<PXFieldOptions.CommitChanges>;
	AllowManualReceipts : PXFieldState<PXFieldOptions.CommitChanges>;
	IsRequireApprovalReplenishment : PXFieldState<PXFieldOptions.CommitChanges>;
	IsRequireApprovalOnFundEstablishment : PXFieldState<PXFieldOptions.CommitChanges>;
	RequireVendorDetails : PXFieldState<PXFieldOptions.CommitChanges>;
	RequireExternalReferenceNbr : PXFieldState;
	RequireApprovalOnFundIncreaseCredAdj : PXFieldState<PXFieldOptions.CommitChanges>;
	RequireApprovalOnFundDecreaseDebAdj : PXFieldState<PXFieldOptions.CommitChanges>;
	AutoReleaseMonthEndJournal : PXFieldState<PXFieldOptions.CommitChanges>;
	ValidateAmountReceivedAndReleasedUponLiquidation : PXFieldState<PXFieldOptions.CommitChanges>;
	EnableFundTransactionLimit : PXFieldState<PXFieldOptions.CommitChanges>;
	SetNonStockItemDescriptionAsDefault : PXFieldState<PXFieldOptions.CommitChanges>;
	UseExpenseAcctFrom : PXFieldState;
	CombineExpSub : PXFieldState;
	IsFundsMigration : PXFieldState<PXFieldOptions.CommitChanges>;
	ApprovalModule : PXFieldState<PXFieldOptions.CommitChanges>;
	FundType : PXFieldState;
}

@gridConfig({
	showFastFilter: GridFastFilterVisibility.False,
	preset: GridPreset.Details
})
export class ATPTEFMFundsApprovalSetup extends PXView  {

	@columnConfig({width: 60, textAlign: TextAlign.Center, type: GridColumnType.CheckBox})
	IsActive : PXFieldState;
	
	@columnConfig({width: 280})
	FundType : PXFieldState;
	
	@linkCommand("ViewAssignmentMap")
	@columnConfig({width: 180, textAlign: TextAlign.Left})
	AssignmentMapID : PXFieldState<PXFieldOptions.CommitChanges>;
	
	@columnConfig({width: 280, displayMode: GridColumnDisplayMode.Text})
	AssignmentNotificationID : PXFieldState;
	
	@columnConfig({width: 280})
	ModuleType : PXFieldState;
}

@gridConfig({
	showFastFilter: GridFastFilterVisibility.False,
	preset: GridPreset.Details,
	syncPosition: true,
})
export class ATPTEFMFundTransactionSetupApproval extends PXView  {

	@columnConfig({width: 60, textAlign: TextAlign.Center, type: GridColumnType.CheckBox})
	IsActive : PXFieldState;
	
	FundTransactionType : PXFieldState<PXFieldOptions.CommitChanges>;
	
	@linkCommand("ViewAssignmentMap")
	@columnConfig({width: 180, textAlign: TextAlign.Left})
	AssignmentMapID : PXFieldState<PXFieldOptions.CommitChanges>;
	
	@columnConfig({width: 280, displayMode: GridColumnDisplayMode.Text})
	AssignmentNotificationID : PXFieldState;
	
	@columnConfig({width: 280})
	ModuleType : PXFieldState;
}

@gridConfig({
	showFastFilter: GridFastFilterVisibility.False,
	preset: GridPreset.Details,
	syncPosition: true,
})
export class ATPTEFMReplenishmentSetupApproval extends PXView  {

	@columnConfig({width: 60, textAlign: TextAlign.Center, type: GridColumnType.CheckBox})
	IsActive : PXFieldState;
	
	@linkCommand("ViewAssignmentMap")
	@columnConfig({width: 180, textAlign: TextAlign.Left})
	AssignmentMapID : PXFieldState<PXFieldOptions.CommitChanges>;
	
	@columnConfig({width: 280, displayMode: GridColumnDisplayMode.Text})
	AssignmentNotificationID : PXFieldState;
}

@gridConfig({
	showFastFilter: GridFastFilterVisibility.False,
	preset: GridPreset.Details,
	syncPosition: true,
})
export class ATPTEFMMonthEndSetupApproval extends PXView  {

	@columnConfig({width: 60, textAlign: TextAlign.Center, type: GridColumnType.CheckBox})
	IsActive : PXFieldState;
	
	@linkCommand("ViewAssignmentMap")
	@columnConfig({width: 180, textAlign: TextAlign.Left})
	AssignmentMapID : PXFieldState<PXFieldOptions.CommitChanges>;
	
	@columnConfig({width: 280, displayMode: GridColumnDisplayMode.Text})
	AssignmentNotificationID : PXFieldState;
}

@gridConfig({
	showFastFilter: GridFastFilterVisibility.False,
	preset: GridPreset.Details
})
export class ATPTEFMReplenishmentReportSettings extends PXView  {

	@columnConfig({width: 60, textAlign: TextAlign.Center, type: GridColumnType.CheckBox})	IsActive : PXFieldState;
	ItemClass : PXFieldState<PXFieldOptions.CommitChanges>;
	Descr : PXFieldState;
	@columnConfig({textAlign: TextAlign.Right})	ReplenishmentID : PXFieldState;
}