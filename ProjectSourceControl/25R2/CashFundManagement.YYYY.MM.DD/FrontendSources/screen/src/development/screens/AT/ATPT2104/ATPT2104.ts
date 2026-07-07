import { Messages as SysMessages } from "client-controls/services/messages";
import { createCollection, createSingle, PXScreen, graphInfo, PXActionState, viewInfo, handleEvent, CustomEventType, actionConfig, RowSelectedHandlerArgs, PXViewCollection, PXPageLoadBehavior, ControlParameter } from "client-controls";
import { ATPTEFMFundFilter, ATPTEFMFund } from "./views";

@graphInfo({graphType: "CashFundManagement.BLC.ATPTEFMFundsMaint", primaryView: "Filter", })
export class ATPT2104 extends PXScreen {

	Filter = createSingle(ATPTEFMFundFilter);
	Document = createCollection(ATPTEFMFund);
}