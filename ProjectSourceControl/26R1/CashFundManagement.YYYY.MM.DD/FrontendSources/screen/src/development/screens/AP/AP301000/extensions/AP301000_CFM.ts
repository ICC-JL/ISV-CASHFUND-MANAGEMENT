import { AP301000 } from "src/screens/AP/AP301000/AP301000";
import { APInvoice_CurrentDocument } from "src/screens/AP/AP301000/AP301000";
import { PXFieldState, PXFieldOptions } from "client-controls";

// Extend main screen class
export interface AP301000_CFM extends AP301000 { }
export class AP301000_CFM { }

// Extend APInvoice_CurrentDocument view to add custom fields
export interface APInvoice_CurrentDocument_CFM extends APInvoice_CurrentDocument { }
export class APInvoice_CurrentDocument_CFM {
	UsrATPTEFMSourceType: PXFieldState;
	UsrATPTEFMSourceRef: PXFieldState<PXFieldOptions.CommitChanges>;
	UsrATPTEFMTranType: PXFieldState;
	UsrATPTEFMReqType: PXFieldState;
	UsrATPTEFMLiqNbr: PXFieldState<PXFieldOptions.CommitChanges>;
}
