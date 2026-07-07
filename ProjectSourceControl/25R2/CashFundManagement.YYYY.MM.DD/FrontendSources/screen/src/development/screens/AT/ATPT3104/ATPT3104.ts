import { Messages as SysMessages } from "client-controls/services/messages";
import { createCollection, createSingle, PXScreen, graphInfo, PXActionState, viewInfo, handleEvent, CustomEventType, actionConfig, RowSelectedHandlerArgs, PXViewCollection, PXPageLoadBehavior, ControlParameter } from "client-controls";
import { ATPTEFMMonthEnd, EPExpenseClaimDetails, ATPTEFMMonthEndDetail, ATPTEFMMonthEnd2, EPApproval, CurrencyInfo } from "./views";

@graphInfo({graphType: "CashFundManagement.BLC.ATPTEFMMonthEndEntry", primaryView: "Document", showUDFIndicator: true})
export class ATPT3104 extends PXScreen {
	SubmitReceipt: PXActionState;
	CancelSubmitReceipt: PXActionState;

	Document = createSingle(ATPTEFMMonthEnd);
   	@viewInfo({containerName: "Add Receipts"})
	ExpenseReceipts = createCollection(EPExpenseClaimDetails);
   	@viewInfo({containerName: "Document Details"})
	Details = createCollection(ATPTEFMMonthEndDetail);
   	@viewInfo({containerName: "Financial Details"})
	CurrentDocument = createSingle(ATPTEFMMonthEnd2);
   	@viewInfo({containerName: "Approval Details"})
	Approval = createCollection(EPApproval);
	_ATPTEFMMonthEnd_CurrencyInfo_ = createSingle(CurrencyInfo);
}