import { EP301000 } from "src/screens/EP/EP301000/EP301000";
import { ExpenseClaim, ExpenseClaimDetails } from "src/screens/EP/EP301000/views";
import { PXFieldState, PXFieldOptions, PXView, gridConfig, GridPreset, GridNoteFilesShowMode, createCollection } from "client-controls";

// Extend main screen class
export interface EP301000_CFM extends EP301000 { }
export class EP301000_CFM {
	ATPTEFMAPPaymentDocument = createCollection(ATPTEFMAPPaymentDocument);
}

// Extend ExpenseClaim view to add custom fields
export interface ExpenseClaim_CFM extends ExpenseClaim { }
export class ExpenseClaim_CFM {
	UsrATPTEFMTranType: PXFieldState<PXFieldOptions.CommitChanges>;
	UsrATPTEFMReqType: PXFieldState<PXFieldOptions.CommitChanges>;
	UsrATPTEFMReqClass: PXFieldState<PXFieldOptions.CommitChanges>;
	UsrATPTEFMLiqNbr: PXFieldState;
	UsrATPTEFMRFPReqRef: PXFieldState<PXFieldOptions.CommitChanges>;
	UsrATPTVendorID: PXFieldState<PXFieldOptions.CommitChanges>;
	UsrATPTEFMIsOverbudget: PXFieldState<PXFieldOptions.CommitChanges>;
	UsrATPTEFMHasInitialBudget: PXFieldState;
}

// Extend ExpenseClaimDetails grid to add custom fields
export interface ExpenseClaimDetails_CFM extends ExpenseClaimDetails { }
export class ExpenseClaimDetails_CFM {
	UsrATPTEFMReqRef: PXFieldState<PXFieldOptions.CommitChanges>;
	UsrATPTEFMAccountGroup: PXFieldState;
}

// Create custom view for AP Payment Document (Check) grid
@gridConfig({
	preset: GridPreset.Attributes,
	showNoteFiles: GridNoteFilesShowMode.Suppress,
	showTopBar: false,
})
export class ATPTEFMAPPaymentDocument extends PXView {
	DocType: PXFieldState;
	RefNbr: PXFieldState<PXFieldOptions.CommitChanges>;
	OrigDocAmt: PXFieldState;
	Status: PXFieldState;
}
