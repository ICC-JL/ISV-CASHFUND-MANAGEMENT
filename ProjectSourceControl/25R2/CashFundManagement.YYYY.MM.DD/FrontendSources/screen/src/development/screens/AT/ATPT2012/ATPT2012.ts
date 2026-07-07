import { Messages as SysMessages } from "client-controls/services/messages";
import { createCollection, createSingle, PXScreen, graphInfo, PXActionState, viewInfo, handleEvent, CustomEventType, actionConfig, RowSelectedHandlerArgs, PXViewCollection, PXPageLoadBehavior, ControlParameter } from "client-controls";
import { ATPTEFMFund, ATPTEFMFund2, ATPTEFMFundTransactionHistoryView, APInvoice, APPayment, EPApproval, ATPTEFMIncreaseFund, ATPTEFMDecreaseFund } from "./views";

@graphInfo({graphType: "CashFundManagement.BLC.ATPTEFMFundMaint", primaryView: "Document", showUDFIndicator: true})
export class ATPT2012 extends PXScreen {

	Document = createSingle(ATPTEFMFund);
	CurrentDocument = createSingle(ATPTEFMFund2);
   	@viewInfo({containerName: "Transaction History"})
	CurrentTransactionHistoryView = createCollection(ATPTEFMFundTransactionHistoryView);
   	@viewInfo({containerName: "Financial Details"})
	APInvoiceDocument = createCollection(APInvoice);
   	@viewInfo({containerName: "Financial Details"})
	APPaymentDocument = createCollection(APPayment);
   	@viewInfo({containerName: "Approvals"})
	Approval = createCollection(EPApproval);
   	@viewInfo({containerName: "Increase Fund"})
	IncreaseFundDocument = createSingle(ATPTEFMIncreaseFund);
   	@viewInfo({containerName: "Decrease Fund"})
	DecreaseFundDocument = createSingle(ATPTEFMDecreaseFund);
}