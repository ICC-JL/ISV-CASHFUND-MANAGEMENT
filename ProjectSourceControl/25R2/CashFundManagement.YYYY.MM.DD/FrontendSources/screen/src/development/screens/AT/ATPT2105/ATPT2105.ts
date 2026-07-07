import {
  PXScreen,
  graphInfo,
  createSingle,
  createCollection,
} from "client-controls";
import { ReqClasses, CurrentReqClass, Items } from "./views";

@graphInfo({
  graphType: "CashFundManagement.BLC.ATPTEFMReqClassMaint",
  primaryView: "ReqClasses",
})
export class ATPT2105 extends PXScreen {
  ReqClasses = createSingle(ReqClasses);
  CurrentReqClass = createSingle(CurrentReqClass);
  Items = createCollection(Items);
}
