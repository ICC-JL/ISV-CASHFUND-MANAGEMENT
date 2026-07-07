import { EP301030 } from "src/screens/EP/EP301030/EP301030";
import { Claim } from "src/screens/EP/EP301030/views";
import { PXFieldState } from "client-controls";

// Extend main screen class
export interface EP301030_CFM extends EP301030 { }
export class EP301030_CFM { }

// Extend Claim grid to add custom fields
export interface Claim_CFM extends Claim { }
export class Claim_CFM {
	UsrATPTEFMTranType: PXFieldState;
	UsrATPTEFMReqType: PXFieldState;
	UsrATPTEFMRFPReqRef: PXFieldState;
	UsrATPTVendorID: PXFieldState;
	UsrATPTVendorID_VendorR_acctName: PXFieldState;
	UsrATPTEFMLiqNbr: PXFieldState;
}
