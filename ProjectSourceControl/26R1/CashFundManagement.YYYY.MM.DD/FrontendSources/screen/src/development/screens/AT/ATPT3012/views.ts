import { PXView, PXFieldState, gridConfig, treeConfig, fieldConfig, controlConfig, actionConfig, headerDescription, ICurrencyInfo, disabled, PXFieldOptions, linkCommand, columnConfig, GridColumnShowHideMode, GridColumnType, PXActionState, TextAlign, GridPreset, GridFilterBarVisibility, GridFastFilterVisibility } from "client-controls";


// Views

export class ATPTEFMReplenishment extends PXView  {

	FundType : PXFieldState<PXFieldOptions.CommitChanges>;
	ReplenishmentNbr : PXFieldState;
	FundID : PXFieldState<PXFieldOptions.CommitChanges>;
	Status : PXFieldState;
	Hold : PXFieldState<PXFieldOptions.CommitChanges>;
	Date : PXFieldState;
	Approved : PXFieldState;
	RequestApproval : PXFieldState;
	Descr : PXFieldState<PXFieldOptions.CommitChanges | PXFieldOptions.Multiline>;
	CustodianID : PXFieldState;
	PayeeID : PXFieldState;
	DepartmentID : PXFieldState;
	BranchID : PXFieldState<PXFieldOptions.CommitChanges>;
	ClaimAmount : PXFieldState;
	WithholdingTaxAmount : PXFieldState;
	VatAmount : PXFieldState;
}

@gridConfig({
	syncPosition: true,
	allowDelete: false,
	allowInsert: false,
	showFastFilter: GridFastFilterVisibility.False,
	preset: GridPreset.ReadOnly
})
export class EPExpenseClaimDetails extends PXView  {

	@columnConfig({allowNull: false, textAlign: TextAlign.Center, type: GridColumnType.CheckBox})	Selected : PXFieldState;
	ClaimDetailCD : PXFieldState;
	ExpenseDate : PXFieldState;
	ExpenseRefNbr : PXFieldState;
	EmployeeID : PXFieldState;
	TranDesc : PXFieldState;
	TaxZoneID : PXFieldState;
	@columnConfig({textAlign: TextAlign.Right})	CuryTranAmtWithTaxes : PXFieldState;
	CuryID : PXFieldState;
	Status : PXFieldState;
}

@gridConfig({
	syncPosition: true,
	preset: GridPreset.Details
})
export class EPTaxTran extends PXView  {

	TaxID : PXFieldState<PXFieldOptions.CommitChanges>;
	@columnConfig({textAlign: TextAlign.Right})	TaxRate : PXFieldState<PXFieldOptions.CommitChanges>;
	@columnConfig({textAlign: TextAlign.Right})	CuryTaxableAmt : PXFieldState<PXFieldOptions.CommitChanges>;
	@columnConfig({textAlign: TextAlign.Right})	CuryTaxAmt : PXFieldState<PXFieldOptions.CommitChanges>;
	@columnConfig({textAlign: TextAlign.Right})	NonDeductibleTaxRate : PXFieldState<PXFieldOptions.CommitChanges>;
	@columnConfig({textAlign: TextAlign.Right})	CuryExpenseAmt : PXFieldState<PXFieldOptions.CommitChanges>;
	Tax__TaxType : PXFieldState<PXFieldOptions.CommitChanges>;
	@columnConfig({textAlign: TextAlign.Center, type: GridColumnType.CheckBox})	Tax__PendingTax : PXFieldState<PXFieldOptions.CommitChanges>;
	@columnConfig({textAlign: TextAlign.Center, type: GridColumnType.CheckBox})	Tax__ReverseTax : PXFieldState<PXFieldOptions.CommitChanges>;
	@columnConfig({textAlign: TextAlign.Center, type: GridColumnType.CheckBox})	Tax__ExemptTax : PXFieldState<PXFieldOptions.CommitChanges>;
	@columnConfig({textAlign: TextAlign.Center, type: GridColumnType.CheckBox})	Tax__StatisticalTax : PXFieldState<PXFieldOptions.CommitChanges>;
}

@gridConfig({
	syncPosition: true,
	showFastFilter: GridFastFilterVisibility.False,
	preset: GridPreset.Details,
	topBarItems: {
	ShowSubmitReceipt: {index: 0, config: {commandName: "ShowSubmitReceipt", text: "Add Receipts"}},
}
})
export class ATPTEFMReplenishmentDetail extends PXView  {

	ShowSubmitReceipt : PXActionState;
	EPExpenseClaimDetails__BranchID : PXFieldState;
	@linkCommand("OpenReceipts")
	@columnConfig({hideViewLink: true})	ExpenseReceiptNbr : PXFieldState;
	EPExpenseClaimDetails__ExpenseDate : PXFieldState;
	EPExpenseClaimDetails__TranDesc : PXFieldState;
	@columnConfig({textAlign: TextAlign.Right})	EPExpenseClaimDetails__Qty : PXFieldState;
	EPExpenseClaimDetails__InventoryID : PXFieldState;
	EPExpenseClaimDetails__UOM : PXFieldState;
	@columnConfig({textAlign: TextAlign.Right})	EPExpenseClaimDetails__CuryUnitCost : PXFieldState;
	@columnConfig({textAlign: TextAlign.Right})	EPExpenseClaimDetails__CuryExtCost : PXFieldState;
	@linkCommand("ViewTaxes")
	@columnConfig({textAlign: TextAlign.Right})	CuryTaxTotal : PXFieldState<PXFieldOptions.CommitChanges>;
	EPExpenseClaimDetails__ExpenseAccountID : PXFieldState;
	EPExpenseClaimDetails__ExpenseSubID : PXFieldState;
	EPExpenseClaimDetails__ExpenseRefNbr : PXFieldState;
	Contract__ContractCD : PXFieldState;
	EPExpenseClaimDetails__TaskID : PXFieldState;
	EPExpenseClaimDetails__CostCodeID : PXFieldState;
	EPExpenseClaimDetails__UsrATPTVendID : PXFieldState;
	EPExpenseClaimDetails__UsrATPTVendName : PXFieldState;
	EPExpenseClaimDetails__UsrATPTAddress : PXFieldState;
	EPExpenseClaimDetails__UsrATPTVendTIN : PXFieldState;
	EPExpenseClaimDetails__TaxZoneID : PXFieldState;
	EPExpenseClaimDetails__TaxCategoryID : PXFieldState;
	InventoryItem__ItemType : PXFieldState;
}

export class ATPTEFMReplenishment2 extends PXView  {

}

@gridConfig({
	showFastFilter: GridFastFilterVisibility.False,
	preset: GridPreset.Details
})
export class ATPTEFMReplenishmentTaxDetail extends PXView  {

	TaxID : PXFieldState<PXFieldOptions.CommitChanges>;
	@columnConfig({textAlign: TextAlign.Right})	TaxType : PXFieldState;
	@columnConfig({textAlign: TextAlign.Right})	TaxRate : PXFieldState;
	@columnConfig({textAlign: TextAlign.Right})	TaxableAmt : PXFieldState;
	@columnConfig({textAlign: TextAlign.Right})	TaxAmt : PXFieldState;
}

export class ATPTEFMReplenishment3 extends PXView  {

	TaxZone : PXFieldState<PXFieldOptions.CommitChanges>;
}

@gridConfig({
	syncPosition: true,
	showFastFilter: GridFastFilterVisibility.False,
	preset: GridPreset.ReadOnly
})
export class APInvoice extends PXView  {

	DocType : PXFieldState;
	@linkCommand("OpenTransaction")
	@columnConfig({hideViewLink: true})	RefNbr : PXFieldState;
	@columnConfig({textAlign: TextAlign.Right})	CuryOrigDocAmt : PXFieldState;
	TaxZoneID : PXFieldState;
	TaxCalcMode : PXFieldState;
	Status : PXFieldState;
}

@gridConfig({
	syncPosition: true,
	autoAdjustColumns: true,
	showFastFilter: GridFastFilterVisibility.False,
	preset: GridPreset.ReadOnly
})
export class APPayment extends PXView  {

	DocType : PXFieldState;
	@linkCommand("ViewCheck")
	@columnConfig({hideViewLink: true})	RefNbr : PXFieldState<PXFieldOptions.CommitChanges>;
	@columnConfig({textAlign: TextAlign.Right})	OrigDocAmt : PXFieldState;
	Status : PXFieldState;
}

@gridConfig({
	allowDelete: false,
	allowInsert: false,
	showFastFilter: GridFastFilterVisibility.False,
	preset: GridPreset.Details
})
export class EPApproval extends PXView  {

	ApproverEmployee__AcctCD : PXFieldState;
	ApproverEmployee__AcctName : PXFieldState;
	ApprovedByEmployee__AcctCD : PXFieldState;
	ApprovedByEmployee__AcctName : PXFieldState;
	ApproveDate : PXFieldState;
	@columnConfig({allowUpdate: false, allowNull: false})	Status : PXFieldState;
	WorkgroupID : PXFieldState;
}

@gridConfig({
	syncPosition: true,
	allowDelete: false,
	allowInsert: false,
	showFastFilter: GridFastFilterVisibility.False,
	preset: GridPreset.ReadOnly
})
export class ATPTEFMExpRecReceiptsForSubmit extends PXView  {

	@columnConfig({ allowCheckAll: true })
	Selected : PXFieldState;
	ExpenseRecognitionLineNbr : PXFieldState;
	RefNbr : PXFieldState;
	LineNbr : PXFieldState;
	ExpenseDate : PXFieldState;
	EmployeeID : PXFieldState;
	TranDesc : PXFieldState;
	LineDesc : PXFieldState;
	ExpenseRefNbr : PXFieldState;
	TaxZoneID : PXFieldState;
	@columnConfig({textAlign: TextAlign.Right})	CuryExtCost : PXFieldState;
	CuryID : PXFieldState;
}

@gridConfig({
	syncPosition: true,
	showFastFilter: GridFastFilterVisibility.False,
	preset: GridPreset.Details,
	topBarItems: {
	ShowSubmitExpRecReceipt: {index: 0, config: {commandName: "ShowSubmitExpRecReceipt", text: "Add Expense Recognition Receipts"}},
	ViewExpRecTaxes: {index: 1, config: {commandName: "ViewExpRecTaxes", text: "View Taxes"}},
}
})
export class ATPTEFMReplenishmentExpRecDetail extends PXView  {

	ShowSubmitExpRecReceipt : PXActionState;
	ViewExpRecTaxes : PXActionState;
	ExpenseReceiptNbr : PXFieldState;
	ExpenseRecognitionRefNbr : PXFieldState;
	LineNbr : PXFieldState;
	ATPTEFMExpenseRecognitionDetails__BranchID : PXFieldState;
	ATPTEFMExpenseRecognitionDetails__ExpenseDate : PXFieldState;
	ATPTEFMExpenseRecognitionDetails__TranDesc : PXFieldState;
	@columnConfig({textAlign: TextAlign.Right})	ATPTEFMExpenseRecognitionDetails__Qty : PXFieldState;
	ATPTEFMExpenseRecognitionDetails__InventoryID : PXFieldState;
	ATPTEFMExpenseRecognitionDetails__Uom : PXFieldState;
	@columnConfig({textAlign: TextAlign.Right})	ATPTEFMExpenseRecognitionDetails__CuryUnitCost : PXFieldState;
	@columnConfig({textAlign: TextAlign.Right})	ATPTEFMExpenseRecognitionDetails__CuryExtCost : PXFieldState;
	@columnConfig({textAlign: TextAlign.Right})	CuryTaxTotal : PXFieldState;
	ATPTEFMExpenseRecognitionDetails__ExpenseAccountID : PXFieldState;
	ATPTEFMExpenseRecognitionDetails__ExpenseSubID : PXFieldState;
	ATPTEFMExpenseRecognitionDetails__TaxZoneID : PXFieldState;
	ATPTEFMExpenseRecognitionDetails__TaxCategoryID : PXFieldState;
	ATPTEFMExpenseRecognitionDetails__VendorName : PXFieldState;
	ATPTEFMExpenseRecognitionDetails__VendorAddress : PXFieldState;
	ATPTEFMExpenseRecognitionDetails__VendorTIN : PXFieldState;
}

@gridConfig({
	showFastFilter: GridFastFilterVisibility.False,
	preset: GridPreset.Details
})
export class ATPTEFMReplenishmentExpRecTaxAggregate extends PXView  {

	TaxID : PXFieldState;
	@columnConfig({textAlign: TextAlign.Right})	Tax__TaxType : PXFieldState;
	@columnConfig({textAlign: TextAlign.Right})	TaxRate : PXFieldState;
	@columnConfig({textAlign: TextAlign.Right})	CuryTaxableAmt : PXFieldState;
	@columnConfig({textAlign: TextAlign.Right})	CuryTaxAmt : PXFieldState;
	@columnConfig({textAlign: TextAlign.Right})	CuryExpenseAmt : PXFieldState;
}

@gridConfig({
	syncPosition: true,
	preset: GridPreset.Details
})
export class ATPTEFMExpenseRecognitionTaxTran extends PXView  {

	TaxID : PXFieldState;
	@columnConfig({textAlign: TextAlign.Right})	TaxRate : PXFieldState;
	@columnConfig({textAlign: TextAlign.Right})	CuryTaxableAmt : PXFieldState;
	@columnConfig({textAlign: TextAlign.Right})	CuryTaxAmt : PXFieldState;
	@columnConfig({textAlign: TextAlign.Right})	NonDeductibleTaxRate : PXFieldState;
	@columnConfig({textAlign: TextAlign.Right})	CuryExpenseAmt : PXFieldState;
	Tax__TaxType : PXFieldState;
}