import { Messages as SysMessages } from "client-controls/services/messages";
import { createCollection, createSingle, PXScreen, graphInfo, PXActionState, viewInfo, handleEvent, CustomEventType, actionConfig, RowSelectedHandlerArgs, PXViewCollection, PXPageLoadBehavior, ControlParameter } from "client-controls";
import { ATPTEFMReplenishment, EPExpenseClaimDetails, EPTaxTran, ATPTEFMReplenishmentDetail, ATPTEFMReplenishment2, ATPTEFMReplenishmentTaxDetail, ATPTEFMReplenishment3, APInvoice, APPayment, EPApproval, ATPTEFMExpRecReceiptsForSubmit, ATPTEFMReplenishmentExpRecDetail, ATPTEFMReplenishmentExpRecTaxAggregate, ATPTEFMExpenseRecognitionTaxTran } from "./views";

@graphInfo({graphType: "CashFundManagement.BLC.ATPTEFMReplenishmentEntry", primaryView: "Replenishments", showUDFIndicator: true})
export class ATPT3012 extends PXScreen {
	SubmitReceipt: PXActionState;
	CancelSubmitReceipt: PXActionState;
	CommitTaxes: PXActionState;
	SubmitExpRecReceipt: PXActionState;
	CancelSubmitExpRecReceipt: PXActionState;
	ViewExpRecTaxes: PXActionState;

	Replenishments = createSingle(ATPTEFMReplenishment);
   	@viewInfo({containerName: "Add Receipts"})
	ReceiptsForSubmit = createCollection(EPExpenseClaimDetails);
   	@viewInfo({containerName: "Add Expense Recognition Receipts"})
	ExpRecReceiptsForSubmit = createCollection(ATPTEFMExpRecReceiptsForSubmit);
   	@viewInfo({containerName: "Document Taxes"})
	Tax_Rows = createCollection(EPTaxTran);
   	@viewInfo({containerName: "Replenishment Details"})
	ReplenishmentDetails = createCollection(ATPTEFMReplenishmentDetail);
   	@viewInfo({containerName: "Replenishment Details"})
	ReplenishmentExpRecDetails = createCollection(ATPTEFMReplenishmentExpRecDetail);
   	@viewInfo({containerName: "Taxes"})
	CurrentReplenishment = createSingle(ATPTEFMReplenishment2);
   	@viewInfo({containerName: "Taxes"})
	Taxes = createCollection(ATPTEFMReplenishmentTaxDetail);
   	@viewInfo({containerName: "Taxes"})
	ExpRecTaxes = createCollection(ATPTEFMReplenishmentExpRecTaxAggregate);
   	@viewInfo({containerName: "Expense Recognition Taxes"})
	ExpRecTax_Rows = createCollection(ATPTEFMExpenseRecognitionTaxTran);
   	@viewInfo({containerName: "Financial Details"})
	ReplenishmentCurrent = createSingle(ATPTEFMReplenishment3);
   	@viewInfo({containerName: "Financial Details"})
	APInvoiceDocument = createCollection(APInvoice);
   	@viewInfo({containerName: "Financial Details"})
	APPaymentDocument = createCollection(APPayment);
   	@viewInfo({containerName: "Approvals"})
	Approval = createCollection(EPApproval);
}