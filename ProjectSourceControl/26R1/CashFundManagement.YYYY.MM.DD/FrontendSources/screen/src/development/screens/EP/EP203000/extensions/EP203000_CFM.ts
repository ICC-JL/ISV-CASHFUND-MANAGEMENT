import { EP203000, EPEmployee } from "src/screens/EP/EP203000/EP203000";
import { PXFieldState, PXFieldOptions } from "client-controls";

// Extend main screen class
export interface EP203000_CFM extends EP203000 { }
export class EP203000_CFM { }

// Extend EPEmployee view to add Cash Advance custom fields
export interface EPEmployee_CFM extends EPEmployee { }
export class EPEmployee_CFM {
	// Cash Advance Settings Fields
	UsrATPTEFMAllowableDays: PXFieldState;
	UsrATPTEFMMaxCAAmt: PXFieldState;
	UsrATPTEFMOpenCA: PXFieldState;
	UsrATPTEFMCAUser: PXFieldState;
}
