using CashFundManagement.Extensions.DAC;
using PX.Data;
using PX.Objects.CS;
using PX.Objects.EP;
using System;

namespace CashFundManagement.Attributes {
    public class ATPTEFMLiquidationNbrAutonumberAttribute : AutoNumberAttribute
    {
        public ATPTEFMLiquidationNbrAutonumberAttribute(Type doctypeField, Type dateField)
    : base(doctypeField, dateField)
        {
        }
        public ATPTEFMLiquidationNbrAutonumberAttribute(Type doctypeField, Type dateField, string[] doctypeValues, Type[] setupFields)
          : base(doctypeField, dateField, doctypeValues, setupFields)
        {
        }

        public override void RowPersisting(PXCache sender, PXRowPersistingEventArgs e)
        {
            EPExpenseClaim row = e.Row as EPExpenseClaim;
            ATPTEFMEPExpenseClaimExt rowExt = row.GetExtension<ATPTEFMEPExpenseClaimExt>();

            if (rowExt.UsrATPTEFMTranType.Equals(ATPTEFMExpenseTypeAttribute.RequestforPayment) )
            {
                sender.SetValue(e.Row, _FieldName, String.Empty);
            }
            else base.RowPersisting(sender, e);
        }
    }
}
