using CashFundManagement.Helper;
using PX.Data;
using PX.Objects.AP;
using PX.Objects.EP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static PX.Data.Wiki.Parser.PXSpecialTagParser;
using static PX.Objects.Common.Constants;

namespace CashFundManagement.Extensions.BLC
{
    /// <remarks>
    /// 2024-08-14 : Adds multi-tenant support. {RRS}
    /// </remarks>
    public class ATPTEFMEmployeeMaintExtension : PXGraphExtension<EmployeeMaint>
    {
#if Version23R2
        public static bool IsActive() => true;
#else
        public static bool IsActive() => ATPTEFMPrefetchSetup.IsActive;
#endif

        protected virtual void _(Events.RowSelected<EPEmployee> e)
        {
            PXUIFieldAttribute.SetRequired<EPEmployee.termsID>(e.Cache, true);
        }
        #region CacheAttached

        #region TermsID
        [PXMergeAttributes(Method = MergeMethod.Append)]
        [PXRemoveBaseAttribute(typeof(PXDefaultAttribute))]
        [PXDefault]
        protected virtual void EPEmployee_TermsID_CacheAttached(PXCache sender) { }
        #endregion

        #region VTaxZoneID
        [PXMergeAttributes(Method = MergeMethod.Append)]
        [PXCustomizeBaseAttribute(typeof(PXUIFieldAttribute), nameof(PXUIFieldAttribute.Required), true)]
        [PXCustomizeBaseAttribute(typeof(PXDefaultAttribute), nameof(PXDefaultAttribute.PersistingCheck), PXPersistingCheck.NullOrBlank)]
        protected virtual void Location_VTaxZoneID_CacheAttached(PXCache sender) { }
        #endregion

        #endregion

    }
}
