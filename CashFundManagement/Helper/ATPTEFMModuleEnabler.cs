using PX.Data;
using System;
using System.Collections.Generic;


namespace CashFundManagement.Helper {
    public class ATPTEFMModuleEnabler : IPrefetchable
    {
        protected List<bool> values = new List<bool>();

        public static bool IsFinancialModules
        {
            get
            {
                bool result = true;
                if (Values.Count != 0) result = Values[0];
                PXTrace.WriteInformation(String.Format("EFM Module is enabled : {0}", result));
                return result;
            }
        }

        private static List<bool> Values
        {
            get
            {
                return PXDatabase.GetSlot<ATPTEFMModuleEnabler>("EnableDisable", typeof(DAC.Setup.ATPTEFMEnableDisable)).values;
            }
        }    

        public void Prefetch()
        {
            foreach (PXDataRecord rec in PXDatabase.SelectMulti<DAC.Setup.ATPTEFMEnableDisable>(new PXDataField<DAC.Setup.ATPTEFMEnableDisable.financialModules>()))
            {
                values.Add(rec.GetBoolean(0) == true);
            }
        }
    }
}