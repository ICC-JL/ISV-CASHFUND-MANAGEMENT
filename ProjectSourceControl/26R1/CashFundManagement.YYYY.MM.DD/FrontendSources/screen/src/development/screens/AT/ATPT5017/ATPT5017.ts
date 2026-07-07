import { Messages as SysMessages } from "client-controls/services/messages";
import { createCollection, createSingle, PXScreen, graphInfo, PXActionState, viewInfo, handleEvent, CustomEventType, actionConfig, RowSelectedHandlerArgs, PXViewCollection, PXPageLoadBehavior, ControlParameter } from "client-controls";
import { ATPTEFMFundTransaction, ATPTEFMFundRequestExtendFilter } from "./views";

@graphInfo({graphType: "CashFundManagement.BLC.ATPTEFMFundRequestReclassifyProcess", primaryView: "Summary", })
export class ATPT5017 extends PXScreen {

	Summary = createCollection(ATPTEFMFundTransaction);
   	@viewInfo({containerName: "Days Extension"})
	ExtendFilter = createSingle(ATPTEFMFundRequestExtendFilter);
}