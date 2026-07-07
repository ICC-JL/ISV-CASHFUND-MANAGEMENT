import { Messages as SysMessages } from "client-controls/services/messages";
import { createCollection, createSingle, PXScreen, graphInfo, PXActionState, viewInfo, handleEvent, CustomEventType, actionConfig, RowSelectedHandlerArgs, PXViewCollection, PXPageLoadBehavior, ControlParameter } from "client-controls";
import { ATPTEFMFundTransactionFilter, ATPTEFMFundTransaction } from "./views";

@graphInfo({graphType: "CashFundManagement.BLC.ATPTEFMFundTransactionMaint", primaryView: "Filter", })
export class ATPT2102 extends PXScreen {

	Filter = createSingle(ATPTEFMFundTransactionFilter);
	FundTransaction = createCollection(ATPTEFMFundTransaction);
}