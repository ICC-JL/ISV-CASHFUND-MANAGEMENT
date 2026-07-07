using CashFundManagement.Helper;
/*
using PX.Data;
using PX.Data.BQL;
using PX.Objects.CA;
using PX.Objects.CS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashFundManagement.Extensions.DAC
{
    //TODO : remove dead codes
    /// <remarks>
    /// 2024-08-14 : Comment the code, this is not used anymore
    /// </remarks>
    public sealed class ATPTEFMCASetupExtension : PXCacheExtension<CASetup>
    {
        public static bool IsActive() => ATPTEFMPrefetchSetup.IsActive && ATPTEFMModuleEnabler.IsFinancialModules ;

        #region UsrATPTEFMFundNumberingID
        [PXDBString(10, IsUnicode = true)]
        //[PXDBDefault()]
        [PXSelector(typeof(Numbering.numberingID), DescriptionField = typeof(Numbering.descr))]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.UsrRLFundNumberingID, Visibility = PXUIVisibility.Visible)]
        public string UsrATPTEFMFundNumberingID { get; set; }
        public abstract class usrATPTEFMFundNumberingID : BqlString.Field<usrATPTEFMFundNumberingID> { }
        #endregion

        #region UsrATPTEFMReplenishmentID
        [PXDBString(10, IsUnicode = true)]
        //[PXDBDefault()]
        [PXSelector(typeof(Numbering.numberingID), DescriptionField = typeof(Numbering.descr))]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.UsrRLReplishmentID, Visibility = PXUIVisibility.Visible)]
        public string UsrATPTEFMReplenishmentID { get; set; }
        public abstract class usrATPTEFMReplenishmentID : BqlString.Field<usrATPTEFMReplenishmentID> { }
        #endregion

        #region UsrATPTEFMMonthEndNumberingID
        [PXDBString(10, IsUnicode = true)]
        //[PXDBDefault()]
        [PXSelector(typeof(Numbering.numberingID), DescriptionField = typeof(Numbering.descr))]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.UsrRLMonthEndNumberingID, Visibility = PXUIVisibility.Visible)]
        public string UsrATPTEFMMonthEndNumberingID { get; set; }
        public abstract class usrATPTEFMMonthEndNumberingID : BqlString.Field<usrATPTEFMMonthEndNumberingID> { }
        #endregion

        #region UsrATPTEFMFundAcctName
        [PXDBString(200, IsUnicode = true)]
        //[PXDBDefault()]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.UsrRLFundAcctName, Visibility = PXUIVisibility.Visible)]
        public string UsrATPTEFMFundAcctName { get; set; }
        public abstract class usrATPTEFMFundAcctName : BqlString.Field<usrATPTEFMFundAcctName> { }
        #endregion
    }
}
*/