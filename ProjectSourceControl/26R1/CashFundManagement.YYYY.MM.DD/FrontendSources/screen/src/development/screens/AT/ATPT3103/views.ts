import { PXView, PXFieldState, gridConfig, treeConfig, fieldConfig, controlConfig, actionConfig, headerDescription, ICurrencyInfo, disabled, PXFieldOptions, linkCommand, columnConfig, GridColumnShowHideMode, GridColumnType, PXActionState, TextAlign, GridPreset, GridFilterBarVisibility, GridFastFilterVisibility } from "client-controls";


// Views

export class ATPTEFMCashAdvance extends PXView  {

	ReqClassID : PXFieldState<PXFieldOptions.CommitChanges>;
	CashAdvanceNbr : PXFieldState;
	Status : PXFieldState;
	Hold : PXFieldState<PXFieldOptions.CommitChanges>;
	Approved : PXFieldState;
	Reclassified : PXFieldState;
	IsOverbudget : PXFieldState<PXFieldOptions.CommitChanges>;
	HasInitialBudget : PXFieldState<PXFieldOptions.CommitChanges>;
	BudgetEnabled : PXFieldState;
	ProjectBudgetEnabled : PXFieldState;
	IsImported : PXFieldState<PXFieldOptions.CommitChanges>;
	Date : PXFieldState<PXFieldOptions.CommitChanges>;
	FinPeriodID : PXFieldState<PXFieldOptions.CommitChanges>;
	RequestApproval : PXFieldState;
	Descr : PXFieldState<PXFieldOptions.CommitChanges | PXFieldOptions.Multiline>;
	BranchID : PXFieldState<PXFieldOptions.CommitChanges>;
	RequestedByID : PXFieldState<PXFieldOptions.CommitChanges>;
	DepartmentID : PXFieldState<PXFieldOptions.CommitChanges>;
	DateOfUse : PXFieldState<PXFieldOptions.CommitChanges>;
	InitialLiqDate : PXFieldState<PXFieldOptions.CommitChanges>;
	LiqDate : PXFieldState<PXFieldOptions.CommitChanges>;
	CuryID : PXFieldState;
	CuryRequestedAmount : PXFieldState<PXFieldOptions.CommitChanges>;
	CuryActualSpentAmount : PXFieldState<PXFieldOptions.CommitChanges>;
	CuryWhtTaxAmount : PXFieldState<PXFieldOptions.CommitChanges>;
	RefundAmount : PXFieldState<PXFieldOptions.CommitChanges>;
	CuryChangeAmount : PXFieldState<PXFieldOptions.CommitChanges>;
}

export class ATPTEFMCashAdvance2 extends PXView  {

	BranchID : PXFieldState;
	TaxZoneID : PXFieldState;
	BillType : PXFieldState;
	@controlConfig({ linkCommand: "ViewBill" })
	BillRefNbr : PXFieldState;
	BillBalance : PXFieldState;
	BillStatus : PXFieldState;
	PmtType : PXFieldState;
	@controlConfig({ linkCommand: "ViewPayment" })
	PmtRefNbr : PXFieldState;
	PmtBalance : PXFieldState;
	PmtStatus : PXFieldState;
	PpmType : PXFieldState;
	@controlConfig({ linkCommand: "ViewPrepayment" })
	PpmRefNbr : PXFieldState;
	PpmBalance : PXFieldState;
	PpmStatus : PXFieldState;
	ReclassifyType : PXFieldState;
	@controlConfig({ linkCommand: "ViewReclassifyBill" })
	ReclassifiedInvoiceRefNbr : PXFieldState;
	ReclassifyBalance : PXFieldState;
	ReclassifyStatus : PXFieldState;
	VendorRefundType : PXFieldState;
	@controlConfig({ linkCommand: "ViewRefund" })
	VendorRefundRefNbr : PXFieldState;
	VendorRefundBalance : PXFieldState;
	VendorRefundStatus : PXFieldState;
}

export class ATPTEFMCARequestDetail extends PXView  {

	InventoryID : PXFieldState<PXFieldOptions.CommitChanges>;
	InventoryItem__Descr : PXFieldState;
	SubID : PXFieldState<PXFieldOptions.CommitChanges>;
	Remarks : PXFieldState;
	Qty : PXFieldState<PXFieldOptions.CommitChanges>;
	Uom : PXFieldState;
	CuryUnitCost : PXFieldState<PXFieldOptions.CommitChanges>;
	CuryAmount : PXFieldState<PXFieldOptions.CommitChanges>;
	AccountID : PXFieldState;
	AccountDescription : PXFieldState;
	ProjectID : PXFieldState<PXFieldOptions.CommitChanges>;
	ProjectTaskID : PXFieldState<PXFieldOptions.CommitChanges>;
	CostCodeID : PXFieldState<PXFieldOptions.CommitChanges>;
}

export class ATPTEFMCAReceiptDetail extends PXView  {

	LoadRequest: PXActionState;

	VendID : PXFieldState<PXFieldOptions.CommitChanges>;
	SubID : PXFieldState<PXFieldOptions.CommitChanges>;
	Reversed : PXFieldState<PXFieldOptions.CommitChanges>;
	@linkCommand("OpenExpClaim")
	LiquidationRef : PXFieldState<PXFieldOptions.CommitChanges>;
	@linkCommand("OpenTransaction")
	ExpenseReceiptRefNbr : PXFieldState;
	Date : PXFieldState;
	InventoryID : PXFieldState<PXFieldOptions.CommitChanges>;
	InventoryItem__Descr : PXFieldState;
	LineDescription : PXFieldState;
	InventoryItem__BaseUnit : PXFieldState;
	ATPTEFMCARequestDetail__Qty : PXFieldState;
	ATPTEFMCARequestDetail__CuryUnitCost : PXFieldState;
	ATPTEFMCARequestDetail__CuryAmount : PXFieldState;
	TaxZoneID : PXFieldState<PXFieldOptions.CommitChanges>;
	TaxCategoryID : PXFieldState;
	AtcCode : PXFieldState;
	NetQty : PXFieldState<PXFieldOptions.CommitChanges>;
	CuryNetUnitCost : PXFieldState<PXFieldOptions.CommitChanges>;
	CuryNetAmt : PXFieldState<PXFieldOptions.CommitChanges>;
	AccountID : PXFieldState<PXFieldOptions.CommitChanges>;
	AccountDescription : PXFieldState;
	ProjectID : PXFieldState<PXFieldOptions.CommitChanges>;
	ProjectTaskID : PXFieldState<PXFieldOptions.CommitChanges>;
	CostCodeID : PXFieldState<PXFieldOptions.CommitChanges>;
	RefNbr : PXFieldState<PXFieldOptions.CommitChanges>;
	VendorName : PXFieldState<PXFieldOptions.CommitChanges>;
	VendorAddress : PXFieldState<PXFieldOptions.CommitChanges>;
	VendorTin : PXFieldState<PXFieldOptions.CommitChanges>;
	InventoryItem__ItemType : PXFieldState;
	Remarks : PXFieldState;
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

export class EPApproval extends PXView  {

	ApproverEmployee__AcctCD : PXFieldState;
	ApproverEmployee__AcctName : PXFieldState;
	ApprovedByEmployee__AcctCD : PXFieldState;
	ApprovedByEmployee__AcctName : PXFieldState;
	ApproveDate : PXFieldState;
	@columnConfig({allowUpdate: false, allowNull: false})	Status : PXFieldState;
	WorkgroupID : PXFieldState;
}

export class ATPTEFMVoidedDocument extends PXView  {

	DocType : PXFieldState;
	@linkCommand("ViewVoidCheck")
	RefNbr : PXFieldState;
	BranchID : PXFieldState;
	Date : PXFieldState;
	VendorID : PXFieldState;
	Descr : PXFieldState;
	@columnConfig({textAlign: TextAlign.Right})	Amount : PXFieldState;
}

export class ATPTEFMCARequestDetail2 extends PXView  {
  
	@columnConfig({ allowCheckAll: true })
	Selected : PXFieldState;
	InventoryID : PXFieldState;
	InventoryID_description : PXFieldState;
	RunningQty : PXFieldState;
	CuryUnitCost : PXFieldState;
	Balance : PXFieldState;
}

export class CurrencyInfo extends PXView implements ICurrencyInfo {

	CuryInfoID : PXFieldState;
	BaseCuryID : PXFieldState;
	BaseCalc : PXFieldState;
	DisplayCuryID : PXFieldState;
	CuryRateTypeID : PXFieldState;
	BasePrecision : PXFieldState;
	CuryRate : PXFieldState;
	CuryEffDate : PXFieldState;
	RecipRate : PXFieldState;
	SampleCuryRate : PXFieldState;
	SampleRecipRate : PXFieldState;
	CuryID : PXFieldState;
}