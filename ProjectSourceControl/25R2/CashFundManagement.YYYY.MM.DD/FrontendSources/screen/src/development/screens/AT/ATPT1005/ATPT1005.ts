import { createCollection, createSingle, PXScreen, graphInfo, PXActionState, viewInfo } from "client-controls";
import { ATPTEFMSetup, ATPTEFMFundsApprovalSetup, ATPTEFMFundTransactionSetupApproval, ATPTEFMReplenishmentSetupApproval, 
	ATPTEFMMonthEndSetupApproval, ATPTEFMReplenishmentReportSettings } from "./views";

@graphInfo({graphType: "CashFundManagement.BLC.ATPTEFMFundTransactionPreference", primaryView: "Preference", })
export class ATPT1005 extends PXScreen {
	ViewAssignmentMap : PXActionState;

	Preference = createSingle(ATPTEFMSetup);
   	@viewInfo({containerName: "Approval"})
	FundsApproval = createCollection(ATPTEFMFundsApprovalSetup);
   	@viewInfo({containerName: "Approval"})
	FundTransactionApproval = createCollection(ATPTEFMFundTransactionSetupApproval);
   	@viewInfo({containerName: "Approval"})
	ReplenishmentApproval = createCollection(ATPTEFMReplenishmentSetupApproval);
   	@viewInfo({containerName: "Approval"})
	MonthEndApproval = createCollection(ATPTEFMMonthEndSetupApproval);
   	@viewInfo({containerName: "Replenishment Report Settings"})
	ReplenishmentReport = createCollection(ATPTEFMReplenishmentReportSettings);
}