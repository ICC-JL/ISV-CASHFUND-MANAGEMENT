import { Messages as SysMessages } from "client-controls/services/messages";
import { createCollection, createSingle, PXScreen, graphInfo, PXActionState, viewInfo, handleEvent, CustomEventType, actionConfig, RowSelectedHandlerArgs, PXViewCollection, PXPageLoadBehavior, ControlParameter } from "client-controls";
import { ATPTEFMReplenishmentFilter, ATPTEFMReplenishment } from "./views";

@graphInfo({graphType: "CashFundManagement.BLC.ATPTEFMReplenishmentMaint", primaryView: "Filter", })
export class ATPT2103 extends PXScreen {

	Filter = createSingle(ATPTEFMReplenishmentFilter);
	Replenishment = createCollection(ATPTEFMReplenishment);
}