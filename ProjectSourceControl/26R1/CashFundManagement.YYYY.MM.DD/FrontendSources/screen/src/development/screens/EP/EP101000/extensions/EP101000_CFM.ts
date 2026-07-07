import { EP101000 } from "src/screens/EP/EP101000/EP101000";
import { Setup } from "src/screens/EP/EP101000/views";
import { PXFieldState, PXFieldOptions } from "client-controls";

// Extend main screen class
export interface EP101000_CFM extends EP101000 { }
export class EP101000_CFM { }

// Extend Setup view to add custom fields in Expense Claim Settings
export interface Setup_CFM extends Setup { }
export class Setup_CFM {
	// Raise Error on Duplicate Ref Nbr
	UsrATPTEFMRaiseErrorOnDuplicateRefNbr: PXFieldState;
	// Require Approval for RFP Bill
	UsrATPTEFMIsRequireApprovalRFPBill: PXFieldState<PXFieldOptions.CommitChanges>;
	// Use Financial Tab Tax Zone for Details
	UsrATPTEFMUseFinancialTabTaxZoneForDetails: PXFieldState;
}
