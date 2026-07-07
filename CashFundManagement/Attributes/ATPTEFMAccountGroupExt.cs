using PX.Data;
using PX.Objects.PM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashFundManagement.Attributes
{
    [PXInt]
    public class ATPTEFMAccountGroupExt : AccountGroupAttribute
    {
        public ATPTEFMAccountGroupExt(Type WhereType)
        {
            Type type = BqlCommand.Compose(typeof(Search<,,>), typeof(PMAccountGroup.groupID), WhereType, typeof(OrderBy<Asc<PMAccountGroup.sortOrder>>));
            PXDimensionSelectorAttribute item = new PXDimensionSelectorAttribute("ACCGROUP", type, typeof(PMAccountGroup.groupCD), typeof(PMAccountGroup.groupCD), typeof(PMAccountGroup.description), typeof(PMAccountGroup.type), typeof(PMAccountGroup.isActive))
            {
                DescriptionField = typeof(PMAccountGroup.description),
                CacheGlobal = true
            };
            _Attributes.Add(item);
            _SelAttrIndex = _Attributes.Count - 1;
        }
    }
}
