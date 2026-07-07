using PX.Data;
using PX.Objects.AP;
using System;

namespace CashFundManagement.Attributes {
    public class ATPTEFMAPVendorRefNbrAttribute : APVendorRefNbrAttribute
    {
        protected override bool IsIgnored(PXCache sender, object row)
        {
            if (!string.IsNullOrEmpty(Convert.ToString(sender.GetValue(row, _FieldName))))
            {
                return !((int?)sender.GetValue(row, _VendorID.Name)).HasValue;
            }

            return true;
        }
    }
}
