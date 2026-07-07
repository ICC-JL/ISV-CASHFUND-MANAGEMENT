using CashFundManagement.DAC.Setup;
using PX.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CashFundManagement.BLC.ATPTEFMCashAdvanceMaint;

namespace CashFundManagement.Helper
{

    /// <remarks>
    /// 2024-08-12 : Adds multi-tenant support controller
    /// </remarks>
    public class ATPTEFMPrefetchSetup : IPrefetchable
    {
        public static bool IsCAFilterByEmployeeDelegates => PXDatabase.GetSlot<ATPTEFMPrefetchSetup>(nameof(ATPTEFMPrefetchSetup), typeof(ATPTEFMCASetup)).isCAFilterByEmployeeDelegates;
        private bool isCAFilterByEmployeeDelegates;
        public static bool IsCAAllowManualReceipts => PXDatabase.GetSlot<ATPTEFMPrefetchSetup>(nameof(ATPTEFMPrefetchSetup), typeof(ATPTEFMCASetup)).isCAAllowManualReceipts;
        private bool isCAAllowManualReceipts;
        public static bool IsFTAllowManualReceipts => PXDatabase.GetSlot<ATPTEFMPrefetchSetup>(nameof(ATPTEFMPrefetchSetup), typeof(ATPTEFMSetup)).isFTAllowManualReceipts;
        private bool isFTAllowManualReceipts;
        public static bool IsFMFilterByEmployeeDelegates => PXDatabase.GetSlot<ATPTEFMPrefetchSetup>(nameof(ATPTEFMPrefetchSetup), typeof(ATPTEFMSetup)).isFMFilterByEmployeeDelegates;
        private bool isFMFilterByEmployeeDelegates;
        public static bool IsRequireApprovalForBills => PXDatabase.GetSlot<ATPTEFMPrefetchSetup>(nameof(ATPTEFMPrefetchSetup), typeof(ATPTEFMSetup)).isRequireApprovalForBills;
        private bool isRequireApprovalForBills;
#if !Version23R2
        public static bool IsActive => PXDatabase.GetSlot<ATPTEFMPrefetchSetup>(nameof(ATPTEFMPrefetchSetup), typeof(ATPTEFMCASetup)).EnableModule;
        private bool EnableModule;
#endif
        public void Prefetch()
        {
            using (PXDataRecord cASetup = PXDatabase.SelectSingle<ATPTEFMCASetup>(new PXDataField<ATPTEFMCASetup.isFilterByEmployeeDelegates>()))
            {
                if (cASetup != null)
                    isCAFilterByEmployeeDelegates = cASetup.GetBoolean(0) == true;
                else
                    isCAFilterByEmployeeDelegates = false;
            }

            using (PXDataRecord cASetup = PXDatabase.SelectSingle<ATPTEFMCASetup>(new PXDataField<ATPTEFMCASetup.allowManualReceipts>()))
            {
                if (cASetup != null)
                    isCAAllowManualReceipts = cASetup.GetBoolean(0) == true;
                else
                    isCAAllowManualReceipts = false;
            }

            using (PXDataRecord ftSetup = PXDatabase.SelectSingle<ATPTEFMSetup>(new PXDataField<ATPTEFMSetup.allowManualReceipts>()))
            {
                if (ftSetup != null)
                    isFTAllowManualReceipts = ftSetup.GetBoolean(0) == true;
                else
                    isFTAllowManualReceipts = false;
            }

            using (PXDataRecord fundSetup = PXDatabase.SelectSingle<ATPTEFMSetup>(new PXDataField<ATPTEFMSetup.isFilterByEmployeeDelegates>()))
            {
                if (fundSetup != null)
                    isFMFilterByEmployeeDelegates = fundSetup.GetBoolean(0) == true;
                else
                    isFMFilterByEmployeeDelegates = false;
            }

            using (PXDataRecord replenishmentSetup = PXDatabase.SelectSingle<ATPTEFMSetup>(new PXDataField<ATPTEFMSetup.isRequireApprovalReplenishment>()))
            {
                if (replenishmentSetup != null)
                    isRequireApprovalForBills = replenishmentSetup.GetBoolean(0) == true;
                else
                    isRequireApprovalForBills = false;
            }
#if !Version23R2
            #region Multi-tenant support
            using (PXDataRecord setup = PXDatabase.SelectSingle<ATPTEFMCASetup>(new PXDataField<ATPTEFMCASetup.enableCFM>()))
            {
                if (setup != null)
                    EnableModule = setup.GetBoolean(0) == true;
                else
                    EnableModule = false;
            }
            #endregion  
#endif
        }
    }
}
