using System;
using System.Collections;
using System.Collections.Generic;
using PX.SM;
using PX.Data;
using PX.Objects.EP;
using CashFundManagement.DAC;
using CashFundManagement.Helper;

namespace CashFundManagement.Extensions.BLC
{
    /// <remarks>
    /// 2024-08-14 : Adds multi-tenant support. {RRS}
    /// </remarks>
    public class ATPTEFMEPApprovalMapMaintExtension : PXGraphExtension<EPApprovalMapMaint>
    {
#if Version23R2
        public static bool IsActive() => ATPTEFMModuleEnabler.IsFinancialModules;
#else
        public static bool IsActive() => ATPTEFMPrefetchSetup.IsActive && ATPTEFMModuleEnabler.IsFinancialModules;
#endif
        #region Event Handlers
        public delegate IEnumerable<String> GetEntityTypeScreensDelegate();
        [PXOverride]
        public IEnumerable<String> GetEntityTypeScreens(GetEntityTypeScreensDelegate baseMethod)
        {

            IEnumerable<String> baseResult = baseMethod();

            //EFM 2020
            List<String> customList = new List<string>();
            foreach(var screen in baseResult)
            {
                customList.Add((string)screen);
            }
            customList.Add("ATPT3103"); //Cash Advance
            customList.Add("ATPT2012"); //Funds
            customList.Add("ATPT3011"); //Fund Transaction
            customList.Add("ATPT3012"); //Replenishment
            customList.Add("ATPT3104"); //Month-End Transactions


            return customList;
            //return result;

        }
        #endregion
    }

}