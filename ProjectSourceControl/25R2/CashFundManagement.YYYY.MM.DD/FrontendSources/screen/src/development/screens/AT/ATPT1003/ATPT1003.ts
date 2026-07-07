import { Messages as SysMessages } from "client-controls/services/messages";
import { createCollection, createSingle, PXScreen, graphInfo, PXActionState, viewInfo, handleEvent, CustomEventType, actionConfig, RowSelectedHandlerArgs, PXViewCollection, PXPageLoadBehavior, ControlParameter } from "client-controls";
import { ATPTEFMCASetup, ATPTEFMCASetupApproval, EPEmployee } from "./views";

@graphInfo({graphType: "CashFundManagement.BLC.ATPTEFMCASetupMaint", primaryView: "Preference", })
export class ATPT1003 extends PXScreen {

	Preference = createSingle(ATPTEFMCASetup);
   	@viewInfo({containerName: "Cash Advance Approval"})
	CashAdvanceApproval = createCollection(ATPTEFMCASetupApproval);
   	@viewInfo({containerName: "Employees"})
	EmployeeCashAdvance = createCollection(EPEmployee);
}