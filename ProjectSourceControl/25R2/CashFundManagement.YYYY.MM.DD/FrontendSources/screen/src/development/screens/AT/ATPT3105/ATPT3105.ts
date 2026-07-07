import { Messages as SysMessages } from "client-controls/services/messages";
import { createCollection, createSingle, PXScreen, graphInfo, PXActionState, viewInfo, handleEvent, CustomEventType, actionConfig, RowSelectedHandlerArgs, PXViewCollection, PXPageLoadBehavior, ControlParameter } from "client-controls";
import { ATPTEFMProjectBudgetFilter, ATPTEFMProjectBudgetLineSummary, ATPTEFMBudgetDistributeFilter, ATPTEFMPreloadBudgetFilter } from "./views";

@graphInfo({graphType: "CashFundManagement.BLC.ATPTEFMProjectBudgetEntry", primaryView: "ProjectFilter", })
export class ATPT3105 extends PXScreen {
	distributeOK: PXActionState;
	preloadArticlesOK: PXActionState;

	ProjectFilter = createSingle(ATPTEFMProjectBudgetFilter);
	Summary = createCollection(ATPTEFMProjectBudgetLineSummary);
   	@viewInfo({containerName: "Dispose Parameters"})
	DistrFilter = createSingle(ATPTEFMBudgetDistributeFilter);
   	@viewInfo({containerName: "Preload Budget Articles"})
	PreloadFilter = createSingle(ATPTEFMPreloadBudgetFilter);
}