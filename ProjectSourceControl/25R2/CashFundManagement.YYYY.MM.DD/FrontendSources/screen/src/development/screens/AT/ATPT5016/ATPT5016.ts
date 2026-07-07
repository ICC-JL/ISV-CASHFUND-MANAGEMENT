import { Messages as SysMessages } from "client-controls/services/messages";
import { createCollection, createSingle, PXScreen, graphInfo, PXActionState, viewInfo, handleEvent, CustomEventType, actionConfig, RowSelectedHandlerArgs, PXViewCollection, PXPageLoadBehavior, ControlParameter } from "client-controls";
import { ATPTEFMCashAdvance, ATPTEFMCashAdvanceExtendFilter } from "./views";

@graphInfo({graphType: "CashFundManagement.BLC.ATPTEFMCAReclassProcess", primaryView: "Summary", })
export class ATPT5016 extends PXScreen {

	Summary = createCollection(ATPTEFMCashAdvance);
   	@viewInfo({containerName: "Days Extension"})
	ExtendFilter = createSingle(ATPTEFMCashAdvanceExtendFilter);
}