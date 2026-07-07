import { EP301020 } from "src/screens/EP/EP301020/EP301020";
import { ClaimDetails, CurrentClaimDetails } from "src/screens/EP/EP301020/views";
import { PXFieldState, PXFieldOptions } from "client-controls";

// Extend main screen class
export interface EP301020_CFM extends EP301020 { }
export class EP301020_CFM { }

// Extend ClaimDetails view (top form) to add custom fields
export interface ClaimDetails_CFM extends ClaimDetails { }
export class ClaimDetails_CFM {
	// Transaction and Fund Type fields
	UsrATPTEFMTranType: PXFieldState<PXFieldOptions.CommitChanges>;
	UsrATPTEFMFundType: PXFieldState<PXFieldOptions.CommitChanges>;
	UsrATPTEFMReqType: PXFieldState<PXFieldOptions.CommitChanges>;
	UsrATPTEFMReqClass: PXFieldState<PXFieldOptions.CommitChanges>;
	UsrATPTEFMReqRef: PXFieldState<PXFieldOptions.CommitChanges>;
	UsrATPTEFMRequestRefNbr: PXFieldState<PXFieldOptions.CommitChanges>;
	UsrATPTVendorID: PXFieldState<PXFieldOptions.CommitChanges>;
}

// Extend CurrentClaimDetails view (Details tab) to add custom field
export interface CurrentClaimDetails_CFM extends CurrentClaimDetails { }
export class CurrentClaimDetails_CFM {
	// Fund Return field
	UsrATPTEFMFundReturn: PXFieldState;
}
