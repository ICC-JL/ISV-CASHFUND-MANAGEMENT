using PX.Data;
using PX.Objects.CS;
using PX.Objects.RQ;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashFundManagement.Extensions.Attribute
{
    /// <summary>
    /// Subaccount Mask Extension that uses Item as default value
    /// </summary>
    [PXDBString(30, IsUnicode = true, InputMask = "")]
    [PXUIField(DisplayName = "Subaccount Mask", Visibility = PXUIVisibility.Visible, FieldClass = _DimensionName)]
    public sealed class ATPTEFMSubAccountMaskExtension : PXEntityAttribute
    {
        private const string _DimensionName = "SUBACCOUNT";
        private const string _MaskName = "RQSETUP";
        public ATPTEFMSubAccountMaskExtension()
            : base()
        {
            PXDimensionMaskAttribute attr = new PXDimensionMaskAttribute(_DimensionName, _MaskName,
                ATPTEFMFTAcctSubDefault.MaskItem,
                new ATPTEFMFTAcctSubDefault.ATPTEFMClassListAttribute().AllowedValues,
                new ATPTEFMFTAcctSubDefault.ATPTEFMClassListAttribute().AllowedLabels);
            attr.ValidComboRequired = true;
            _Attributes.Add(attr);
            _SelAttrIndex = _Attributes.Count - 1;
        }

        public static string MakeSub<Field>(PXGraph graph, string mask, object[] sources, Type[] fields)
            where Field : IBqlField
        {
            try
            {
                return PXDimensionMaskAttribute.MakeSub<Field>(graph, mask, new ATPTEFMFTAcctSubDefault.ATPTEFMClassListAttribute().AllowedValues, 0, sources);
            }
            catch (PXMaskArgumentException ex)
            {
                PXCache cache = graph.Caches[BqlCommand.GetItemType(fields[ex.SourceIdx])];
                string fieldName = fields[ex.SourceIdx].Name;
                throw new PXMaskArgumentException(new ATPTEFMFTAcctSubDefault.ATPTEFMClassListAttribute().AllowedLabels[ex.SourceIdx], PXUIFieldAttribute.GetDisplayName(cache, fieldName));
            }
        }
    }
    /// <summary>
    /// RQAcctSubDefault without Request class option
    /// </summary>
    public class ATPTEFMFTAcctSubDefault
    {
        public class ATPTEFMClassListAttribute : PXCustomStringListAttribute
        {
            public ATPTEFMClassListAttribute()
                : base(new string[] { MaskDepartment, MaskItem, MaskRequester },
                    new string[] { PX.Objects.RQ.Messages.MaskDepartment, PX.Objects.RQ.Messages.MaskItem, PX.Objects.RQ.Messages.MaskRequester })
            {
            }
        }
        public const string MaskItem = "I";

        public const string MaskRequester = "R";

        public const string MaskDepartment = "D";
    }
}
