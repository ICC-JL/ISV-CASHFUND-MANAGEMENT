using ATPTPhilippineTax.Attributes;
using ATPTPhilippineTax.DAC.Extensions;
using ATPTPhilippineTax.Helpers;
using CashFundManagement.Attributes;
using CashFundManagement.Extensions.DAC;
using CashFundManagement.Helper;
using PX.Data;
using PX.Data.BQL;
using PX.Data.EP;
using PX.Objects.AP;
using PX.Objects.Common;
using PX.Objects.CR;
using PX.Objects.CR.MassProcess;
using PX.Objects.EP;
using Message = ATPTPhilippineTax.Messages.ATPTMessages;

namespace CashFundPhilippineTax.Graph.Extension 
{
    /// <remarks>
    /// 2024-08-14 : Adds multi-tenant support. {RRS} <br/>
    /// 2025-02-13:  Apply multi tenant to 24r1 CASE: 010269 {JLTG}
    /// </remarks>
    public class ATPTEFMExpenseClaimDetailEntryExt : PXGraphExtension<CashFundManagement.Extensions.BLC.ATPTEFMExpenseClaimDetailEntry_Extension, PX.Objects.EP.ExpenseClaimDetailEntry>
    {
#if Version23R2
        public static bool IsActive() => ATPTModule.IsActive;
#else
        public static bool IsActive() => ATPTModule.IsActive && ATPTEFMPrefetchSetup.IsActive;
#endif

        public delegate string GetDefATCDelegate(EPExpenseClaimDetails row);
        [PXOverride]
        public string GetDefATC(EPExpenseClaimDetails row, GetDefATCDelegate baseMethod)
        {
            ATPTEPExpenseClaimDetails rowExt = row.GetExtension<ATPTEPExpenseClaimDetails>();

            return rowExt.UsrATPTATCCode;
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
    }
}
