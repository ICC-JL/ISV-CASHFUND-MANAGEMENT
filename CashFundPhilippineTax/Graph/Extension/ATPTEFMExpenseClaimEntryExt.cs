using ATPTPhilippineTax.DAC.Extensions;
using ATPTPhilippineTax.Helpers;
using CashFundManagement.Extensions.DAC;
using CashFundManagement.Helper;
using PX.Data;
using PX.Data.BQL;
using PX.Objects.AP;
using PX.Objects.EP;
using PX.Objects.GL;

namespace CashFundPhilippineTax.Graph.Extension 
{
    /// <remarks>
    /// 2024-08-14 : Adds multi-tenant support. {RRS} <br/>
    /// 2025-02-13:  Apply multi tenant to 24r1 CASE: 010269 {JLTG}
    /// </remarks>
    public class ATPTEFMExpenseClaimEntryExt : PXGraphExtension<CashFundManagement.Extensions.BLC.ATPTEFMExpenseClaimEntry_Extension, PX.Objects.EP.ExpenseClaimEntry>
    {
#if Version23R2
        public static bool IsActive() => ATPTModule.IsActive;
#else
        public static bool IsActive() => ATPTModule.IsActive && ATPTEFMPrefetchSetup.IsActive;
#endif

        public delegate APTran PBudgetIsReversedDelegate(EPExpenseClaimDetails row);
        [PXOverride]
        public APTran PBudgetIsReversed(EPExpenseClaimDetails row, PBudgetIsReversedDelegate baseMethod)
        {
            ATPTEFMEPExpenseClaimDetailsExt rowExt = row.GetExtension<ATPTEFMEPExpenseClaimDetailsExt>();

            if (row.APRefNbr != null)
            {
                return PXSelectJoin<
                    APTran,
                    InnerJoin<APRegister,
                        On<APTran.refNbr, Equal<APRegister.refNbr>,
                        And<APTran.tranType, Equal<APRegister.docType>>>,
                    InnerJoin<APAdjust,
                        On<APAdjust.adjgRefNbr, Equal<APRegister.refNbr>,
                        And<APAdjust.adjgDocType, Equal<APRegister.docType>>>,
                        InnerJoin<Account, On<Account.accountID, Equal<APTran.accountID>>>>>,
                    Where<APTran.tranType, Equal<APDocType.debitAdj>,
                        And<APTran.projectID, Equal<@P.AsInt>,
                        And<APTran.taskID, Equal<@P.AsInt>,
                        And<APTran.costCodeID, Equal<@P.AsInt>,
                        And<Account.accountGroupID, Equal<@P.AsInt>,
                        And<APRegister.origRefNbr, Equal<P.AsString>,
                        And<APRegister.status, Equal<APDocStatus.closed>,
                        And<ATPTAPTran.usrATPTExpenseReceiptNbr, Equal<P.AsString>,
                        And<APAdjust.adjdRefNbr, Equal<@P.AsString>,
                        And<APAdjust.adjdDocType, Equal<@P.AsString>,
                        And<APAdjust.released, Equal<True>>>>>>>>>>>>>
                    .Select(Base, row.ContractID, row.TaskID, row.CostCodeID, rowExt.UsrATPTEFMAccountGroup, row.APRefNbr, row.ClaimDetailCD, row.APRefNbr, row.APDocType);
            }

            return null;
        }

        public delegate string GetVendorDelegate(EPExpenseClaimDetails row);
        [PXOverride]
        public string GetVendor(EPExpenseClaimDetails row, GetVendorDelegate baseMethod)
        {
            ATPTEPExpenseClaimDetails rowExt = row.GetExtension<ATPTEPExpenseClaimDetails>();

            return rowExt.UsrATPTVendID;
        }

        public delegate bool DuplicateERRefNbrDelegate(EPExpenseClaimDetails row, string checkRefNbr, string TranType);
        [PXOverride]
        public bool DuplicateERRefNbr(EPExpenseClaimDetails row, string checkRefNbr, string TranType, DuplicateERRefNbrDelegate baseMethod)
        {
            ATPTEPExpenseClaimDetails rowExt = row.GetExtension<ATPTEPExpenseClaimDetails>();

            EPExpenseClaimDetails dup = null;

            if (rowExt.UsrATPTVendID == null)
            {
                dup = PXSelect<
                    EPExpenseClaimDetails, 
                    Where<EPExpenseClaimDetails.expenseRefNbr, Equal<@P.AsString>, 
                        And<ATPTEFMEPExpenseClaimDetailsExt.usrATPTEFMTranType, Equal<@P.AsString>, 
                        And<EPExpenseClaimDetails.claimDetailCD, NotEqual<@P.AsString>,
                        And<EPExpenseClaimDetails.inventoryID, Equal<@P.AsInt>,
                        And<Where<ATPTEPExpenseClaimDetails.usrATPTVendID, IsNull, 
                            Or<ATPTEPExpenseClaimDetails.usrATPTVendID, Equal<Empty>>>>>>>>>
                    .Select(Base, checkRefNbr, TranType, row.ClaimDetailCD, row.InventoryID);
            }
            else
            {
                dup = PXSelect<
                    EPExpenseClaimDetails, 
                    Where<EPExpenseClaimDetails.expenseRefNbr, Equal<@P.AsString>, 
                        And<ATPTEPExpenseClaimDetails.usrATPTVendID, Equal<@P.AsString>, 
                        And<ATPTEFMEPExpenseClaimDetailsExt.usrATPTEFMTranType, Equal<@P.AsString>, 
                        And<EPExpenseClaimDetails.claimDetailCD, NotEqual<@P.AsString>,
                        And<EPExpenseClaimDetails.inventoryID, Equal<@P.AsInt>>>>>>>
                    .Select(Base, checkRefNbr, rowExt.UsrATPTVendID, TranType, row.ClaimDetailCD, row.InventoryID);
            }

            return dup != null;
        }

        public delegate string GetDefATCDelegate(EPExpenseClaimDetails row);
        [PXOverride]
        public string GetDefATC(EPExpenseClaimDetails row, GetDefATCDelegate baseMethod)
        {
            ATPTEPExpenseClaimDetails rowExt = row.GetExtension<ATPTEPExpenseClaimDetails>();
            return rowExt.UsrATPTATCCode;
        }
    }
}
