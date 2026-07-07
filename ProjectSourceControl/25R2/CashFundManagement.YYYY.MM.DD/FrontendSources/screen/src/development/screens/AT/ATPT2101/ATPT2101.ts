import { Messages as SysMessages } from "client-controls/services/messages";
import { createCollection, createSingle, PXScreen, graphInfo, PXActionState, viewInfo, handleEvent, CustomEventType, actionConfig, RowSelectedHandlerArgs, PXViewCollection, PXPageLoadBehavior, ControlParameter } from "client-controls";
import { ATPTEFMCashAdvanceFilter, ATPTEFMCashAdvance } from "./views";

@graphInfo({graphType: "CashFundManagement.BLC.ATPTEFMCashAdvanceMaint", primaryView: "Filter", })
export class ATPT2101 extends PXScreen {

	Filter = createSingle(ATPTEFMCashAdvanceFilter);
	CashAdvance = createCollection(ATPTEFMCashAdvance);
}