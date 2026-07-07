using System;

namespace CashFundPhilippineTax.Attribute.Extension {
    public class ATPTAPDatDocType : PX.Objects.AP.APDocType.ListAttribute
    {
        public const string ExpenseReceiptLabel = "Expense Receipt";
        public const string ExpenseReceiptValue = "EPR";
        public ATPTAPDatDocType()
        {
            Array.Resize(ref _AllowedValues, _AllowedValues.Length + 1);
            _AllowedValues[_AllowedValues.Length - 1] = ExpenseReceiptValue;
            Array.Resize(ref _AllowedLabels, _AllowedLabels.Length + 1);
            _AllowedLabels[_AllowedLabels.Length - 1] = ExpenseReceiptLabel;
        }
    }
}
