using CashFundManagement.Extensions.DAC;
using PX.Data;
using PX.Objects.EP;
using System;

namespace CashFundManagement.Attributes {
    public class ATPTEFMRFPRefNbrAutonumberAttribute : PX.Objects.CS.AutoNumberAttribute
    {
        public ATPTEFMRFPRefNbrAutonumberAttribute(Type doctypeField, Type dateField)
      : base(doctypeField, dateField)
        {
        }
        public ATPTEFMRFPRefNbrAutonumberAttribute(Type doctypeField, Type dateField, string[] doctypeValues, Type[] setupFields)
          : base(doctypeField, dateField, doctypeValues, setupFields)
        {
        }

        public override void RowPersisting(PXCache sender, PXRowPersistingEventArgs e)
        {
            EPExpenseClaim row = e.Row as EPExpenseClaim;
            ATPTEFMEPExpenseClaimExt rowExt = row.GetExtension<ATPTEFMEPExpenseClaimExt>();

            if (rowExt.UsrATPTEFMTranType.Equals(ATPTEFMExpenseTypeAttribute.RequestforPayment))
            {
                base.RowPersisting(sender, e);                
            } 
            else sender.SetValue(e.Row, _FieldName, String.Empty);            
        }
    }
}
