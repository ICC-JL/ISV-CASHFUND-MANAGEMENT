import { PXView, PXFieldState, gridConfig, treeConfig, fieldConfig, controlConfig, actionConfig, headerDescription, ICurrencyInfo, disabled, PXFieldOptions, linkCommand, columnConfig, GridColumnShowHideMode, GridColumnType, PXActionState, TextAlign, GridPreset, GridFilterBarVisibility, GridFastFilterVisibility, GridColumnDisplayMode } from "client-controls";


// Views

export class ATPTEFMCASetup extends PXView  {

	EnableCFM : PXFieldState;
	@controlConfig({allowEdit:true})
	CANumberingID : PXFieldState;
	@controlConfig({allowEdit:true})
	LiquidationNumberingID : PXFieldState;
	AllowableCAAmt : PXFieldState<PXFieldOptions.CommitChanges>;
	RVAllowableCAAmt : PXFieldState;
	AllowableOpenCA : PXFieldState<PXFieldOptions.CommitChanges>;
	RVAllowableOpenCA : PXFieldState;
	IsFilterByEmployeeDelegates : PXFieldState<PXFieldOptions.CommitChanges>;
	AutoReleaseAP : PXFieldState;
	RestrictCAEmployees : PXFieldState;
	CopyAPNotes : PXFieldState;
	IsRequireApprovalCashAdvanceBill : PXFieldState<PXFieldOptions.CommitChanges>;
	AllowSubmissionExcessCA : PXFieldState;
	CopyECNotes : PXFieldState;
	LiquidationRule : PXFieldState<PXFieldOptions.CommitChanges>;
	StandardAllowableDays : PXFieldState;
	ReClassAccoundID : PXFieldState<PXFieldOptions.CommitChanges>;
	ReClassSubID : PXFieldState<PXFieldOptions.CommitChanges>;
	LiquidationDateBasedOnWorkCalendar : PXFieldState;
	AutoApplyPPT : PXFieldState;
	AutoApplyCredAdjPPT : PXFieldState;
	AllowManualReceipts : PXFieldState;
	RequireExtRef : PXFieldState;
	IsRequireApprovalLiquidationBill : PXFieldState;
	RequireVendorDetails : PXFieldState;
	CashAdvanceAccountEnable : PXFieldState;
	CashAdvanceSubAccountEnable : PXFieldState;
	SetNonStockItemDescriptionAsDefault : PXFieldState<PXFieldOptions.CommitChanges>;
	CashAdvanceRequestApproval : PXFieldState;
	IsCashAdvanceMigration : PXFieldState<PXFieldOptions.CommitChanges>;
}

@gridConfig({
	showFastFilter: GridFastFilterVisibility.False,
	preset: GridPreset.Details
})
export class ATPTEFMCASetupApproval extends PXView  {

	@columnConfig({width: 220, textAlign: TextAlign.Left})
	AssignmentMapID : PXFieldState;
	
	@columnConfig({width: 280, displayMode: GridColumnDisplayMode.Text})
	AssignmentNotificationID : PXFieldState;
}

@gridConfig({
	autoAdjustColumns: true,
	showFastFilter: GridFastFilterVisibility.False,
	preset: GridPreset.Details
})
export class EPEmployee extends PXView  {

	AcctCD : PXFieldState;
	AcctName : PXFieldState;
	@columnConfig({textAlign: TextAlign.Right})	UsrATPTEFMAllowableDays : PXFieldState;
	@columnConfig({textAlign: TextAlign.Right})	UsrATPTEFMMaxCAAmt : PXFieldState;
	@columnConfig({textAlign: TextAlign.Right})	UsrATPTEFMOpenCA : PXFieldState;
	@columnConfig({textAlign: TextAlign.Center, type: GridColumnType.CheckBox})	UsrATPTEFMCAUser : PXFieldState<PXFieldOptions.CommitChanges>;
}