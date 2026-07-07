using PX.Data;
using PX.Data.BQL;
using System;

namespace CashFundManagement.DAC.Setup {
    [Serializable]
    [PXCacheName(CashFundManagement.Messages.ATPTEFMMessages.ATPTEFMEnableDisable)]
    public class ATPTEFMEnableDisable : Base.ATPTEFMAudit, IBqlTable
    {
        #region FinancialModules
        [PXDBBool]
        [PXDefault(false)]
        [PXUIField(DisplayName = "Financial Modules")]
        public virtual bool? FinancialModules { get; set; }
        public abstract class financialModules : BqlBool.Field<financialModules> { }
        #endregion

        #region NoteID  
        [PXNote]
        public virtual Guid? NoteID { get; set; }
        public abstract class noteID : IBqlField { }
        #endregion
    }
}