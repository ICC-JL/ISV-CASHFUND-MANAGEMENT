import { Messages as SysMessages } from "client-controls/services/messages";
import { createCollection, createSingle, PXScreen, graphInfo, PXActionState, viewInfo, handleEvent, CustomEventType, actionConfig, RowSelectedHandlerArgs, PXViewCollection, PXPageLoadBehavior, ControlParameter } from "client-controls";
import { ATPTEFMCashAdvance, ATPTEFMCashAdvance2, ATPTEFMCARequestDetail, ATPTEFMCAReceiptDetail, ATPTEFMBudget, ATPTEFMPBudget, EPApproval, ATPTEFMVoidedDocument, ATPTEFMCARequestDetail2 } from "./views";

@graphInfo({graphType: "CashFundManagement.BLC.ATPTEFMCashAdvanceEntry", primaryView: "CashAdvances", showUDFIndicator: true})
export class ATPT3103 extends PXScreen {
	SubmitReceipt: PXActionState;
	CancelSubmitReceipt: PXActionState;

	CashAdvances = createSingle(ATPTEFMCashAdvance);
	CurrentCashAdvance = createSingle(ATPTEFMCashAdvance2);
   	@viewInfo({containerName: "Request Details"})
	CashAdvanceRequestLines = createCollection(ATPTEFMCARequestDetail);
   	@viewInfo({containerName: "Receipts"})
	CashAdvanceReceiptLines = createCollection(ATPTEFMCAReceiptDetail);
   	@viewInfo({containerName: "Budget Details"})
	Budget = createCollection(ATPTEFMBudget);
   	@viewInfo({containerName: "Project Budget Details"})
	ProjectBudget = createCollection(ATPTEFMPBudget);
   	@viewInfo({containerName: "Approval Details"})
	Approval = createCollection(EPApproval);
   	@viewInfo({containerName: "Voided Documents"})
	VoidedDocuments = createCollection(ATPTEFMVoidedDocument);
   	@viewInfo({containerName: "Add Receipt"})
	ReceiptsForSubmit = createCollection(ATPTEFMCARequestDetail2);
}