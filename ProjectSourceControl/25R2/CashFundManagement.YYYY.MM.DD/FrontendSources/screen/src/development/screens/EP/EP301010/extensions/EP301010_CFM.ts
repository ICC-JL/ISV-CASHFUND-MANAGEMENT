import { EP301010 } from "src/screens/EP/EP301010/EP301010";
import { ClaimDetails } from "src/screens/EP/EP301010/views";
import { PXFieldState } from "client-controls";

// Extend main screen class
export interface EP301010_CFM extends EP301010 { }
export class EP301010_CFM { }

// Extend ClaimDetails grid to add custom field
export interface ClaimDetails_CFM extends ClaimDetails { }
export class ClaimDetails_CFM {
	UsrATPTEFMTranType: PXFieldState;
}
