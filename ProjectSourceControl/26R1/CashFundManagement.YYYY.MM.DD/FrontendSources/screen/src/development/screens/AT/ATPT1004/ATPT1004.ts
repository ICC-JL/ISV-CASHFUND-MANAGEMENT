import { PXScreen, graphInfo, createSingle } from "client-controls";
import { ATPTEFMFeaturesSetup } from "./views";

@graphInfo({ graphType: "CashFundManagement.BLC.ATPTEFMFeaturesMaint", primaryView: "Setup" })
export class ATPT1004 extends PXScreen {
  Setup = createSingle(ATPTEFMFeaturesSetup);
}
