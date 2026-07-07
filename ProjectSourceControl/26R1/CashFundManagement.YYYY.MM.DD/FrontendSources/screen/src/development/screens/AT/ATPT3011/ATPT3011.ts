import { Messages as SysMessages } from "client-controls/services/messages";
import { createCollection, createSingle, PXScreen, graphInfo, PXActionState, viewInfo, handleEvent, CustomEventType, actionConfig, RowSelectedHandlerArgs, PXViewCollection, PXPageLoadBehavior, ControlParameter } from "client-controls";
import { ATPTEFMFundTransaction, ATPTEFMFundTransactionDetail, ATPTEFMFundTransactionReceiptDetail, ATPTEFMFundTransaction2, ATPTEFMFundTransactionReclassficationReceiptDetail, EPApproval, ATPTEFMBudget, ATPTEFMPBudget, ATPTEFMFundTransactionDetail2 } from "./views";

@graphInfo({graphType: "CashFundManagement.BLC.ATPTEFMFundTransactionEntry", primaryView: "FundTransactions", showUDFIndicator: true})
export class ATPT3011 extends PXScreen {
	SubmitReceipt: PXActionState;
	CancelSubmitReceipt: PXActionState;

	FundTransactions = createSingle(ATPTEFMFundTransaction);
	FundTransactionCash = createSingle(ATPTEFMFundTransaction2);
	
   	@viewInfo({containerName: "Document Details"})
	FundTransactionDetails = createCollection(ATPTEFMFundTransactionDetail);
   	@viewInfo({containerName: "Receipts"})
	FundTransactionReceiptLines = createCollection(ATPTEFMFundTransactionReceiptDetail);
   	@viewInfo({containerName: "Reclassification Receipts"})
	FundTransactionReclassficationReceiptDetail = createCollection(ATPTEFMFundTransactionReclassficationReceiptDetail);
   	@viewInfo({containerName: "Approvals"})
	Approval = createCollection(EPApproval);
   	@viewInfo({containerName: "Budget Details"})
	Budget = createCollection(ATPTEFMBudget);
   	@viewInfo({containerName: "Project Budget Details"})
	ProjectBudget = createCollection(ATPTEFMPBudget);
   	@viewInfo({containerName: "Add Receipt"})
	ReceiptsForSubmit = createCollection(ATPTEFMFundTransactionDetail2);
}