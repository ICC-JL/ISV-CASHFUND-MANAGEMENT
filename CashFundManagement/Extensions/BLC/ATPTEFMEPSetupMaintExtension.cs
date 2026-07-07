using CashFundManagement.DAC.Setup;
using CashFundManagement.Extensions.DAC;
using CashFundManagement.Helper;
using CashFundManagement.Messages;
using PX.Data;
using PX.Objects.Common.Extensions;
using PX.Objects.EP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static PX.Objects.CT.ContractAction;

namespace CashFundManagement.Extensions.BLC
{
    /// <remarks>
    /// 2024-08-14 : Adds multi-tenant support. {RRS}
    /// 010220 - (CFM24R1/24R2) RFP>Expense Claim: PR or PO error 'invalid processing of document' upon release of the bill, where the bill generated has a financial entry from a bill with 'On Hold' status.
    /// </remarks>
    public class ATPTEFMEPSetupMaintExtension : PXGraphExtension<EPSetupMaint>
    {
#if Version23R2
        public static bool IsActive() => true;
#else
        public static bool IsActive() => ATPTEFMPrefetchSetup.IsActive;
#endif
        public PXSelect<ATPTEFMSetup> ATPTEFMFMPreference;
        public PXSetup<ATPTEFMCASetup> ATPTEFMCAPreference;

        #region Events
        protected virtual void _(Events.FieldUpdated<EPSetup, EPSetup.requireRefNbrInExpenseReceipts> e)
        {
            EPSetup row = e.Row;
            ATPTEFMCASetup caPreference = ATPTEFMCAPreference.Select();
            ATPTEFMSetup fmPreference = ATPTEFMFMPreference.Select();

            if (row == null) return;
            if (caPreference != null)
            {
                PXCache cache = Base.Caches[typeof(ATPTEFMCASetup)];
                caPreference.RequireExtRef = (row.RequireRefNbrInExpenseReceipts ?? false) ? true : caPreference.RequireExtRef;
                cache.Update(caPreference);
            }

            if (fmPreference != null)
            {
                PXCache fmCache = Base.Caches[typeof(ATPTEFMSetup)];
                fmPreference.RequireExternalReferenceNbr = (row.RequireRefNbrInExpenseReceipts ?? false) ? true : fmPreference.RequireExternalReferenceNbr;
                fmCache.Update(fmPreference);
            }
        }
        protected virtual void _(Events.FieldUpdated<EPSetup, EPSetup.automaticReleaseAP> e)
        {
            EPSetup row = e.Row;
            if (row == null) return;

            ATPTEFMEPSetupExtension rowExt = row.GetExtension<ATPTEFMEPSetupExtension>();

            if ((bool)(e.NewValue ?? false))
            {
                rowExt.UsrATPTEFMIsRequireApprovalRFPBill = false;
            }
        }
        protected virtual void _(Events.FieldUpdated<EPSetup, ATPTEFMEPSetupExtension.usrATPTEFMIsRequireApprovalRFPBill> e)
        {
            EPSetup row = e.Row;
            if (row == null) return;

            if ((bool)(e.NewValue ?? false))
            {
                row.AutomaticReleaseAP = false;
            }
        }
        protected virtual void _(Events.RowSelected<EPSetup> e, PXRowSelected baseMethod)
        {
            if (baseMethod != null) baseMethod?.Invoke(e.Cache, e.Args);

            EPSetup row = e.Row;
            if (row == null) return;

            ATPTEFMCASetup caSetup = ATPTEFMCAPreference.Current;

            ATPTEFMEPSetupExtension rowExt = row.GetExtension<ATPTEFMEPSetupExtension>();

            PXUIFieldAttribute.SetEnabled<EPSetup.automaticReleaseAP>(e.Cache, row, (!rowExt.UsrATPTEFMIsRequireApprovalRFPBill ?? false) && (!caSetup.IsRequireApprovalLiquidationBill ?? false));
            PXUIFieldAttribute.SetEnabled<ATPTEFMEPSetupExtension.usrATPTEFMIsRequireApprovalRFPBill>(e.Cache, row, !row.AutomaticReleaseAP ?? false);

            if ((rowExt.UsrATPTEFMIsRequireApprovalRFPBill ?? false) || (caSetup.IsRequireApprovalLiquidationBill ?? false))
                PXUIFieldAttribute.SetWarning<EPSetup.automaticReleaseAP>(e.Cache, row, ATPTEFMMessages.WarningMessageForEPAutomaticReleaseAP);

            if (row.AutomaticReleaseAP ?? false)
                PXUIFieldAttribute.SetWarning<ATPTEFMEPSetupExtension.usrATPTEFMIsRequireApprovalRFPBill>(e.Cache, row, ATPTEFMMessages.WarningMessageForEPRequireRFPBillApproval);
        }
        #endregion
    }
}
