import { AP302000 } from "src/screens/AP/AP302000/AP302000";
import { CurrentDocument } from "src/screens/AP/AP302000/AP302000";
import { PXFieldState, PXFieldOptions } from "client-controls";

// Extend main screen class
export interface AP302000_CFM extends AP302000 { }
export class AP302000_CFM { }

// Extend CurrentDocument view to add custom fields
export interface CurrentDocument_CFM extends CurrentDocument { }
export class CurrentDocument_CFM {
	UsrATPTEFMSourceType: PXFieldState;
	UsrATPTEFMSourceRef: PXFieldState<PXFieldOptions.CommitChanges>;
	UsrATPTEFMTranType: PXFieldState;
	UsrATPTEFMReqType: PXFieldState;
	UsrATPTEFMLiqNbr: PXFieldState<PXFieldOptions.CommitChanges>;
}
