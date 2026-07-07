using PX.Data;
using PX.Objects.GL;
using PX.Objects.GL.Descriptor;
using PX.Objects.GL.FinPeriods.TableDefinition;
using PX.Objects.PM;
using System;

namespace CashFundManagement.DAC {
    [Serializable]
    [PXCacheName(Messages.ATPTEFMMessages.ATPTEFMPBudget)]
    [PXPrimaryGraph(typeof(BLC.ATPTEFMProjectBudgetEntry))]
    public class ATPTEFMProjectBudget : Base.ATPTEFMAudit, IBqlTable
    {
        #region Selected
        [PXBool]
        [PXUnboundDefault(false)]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.Selected)]
        public bool? Selected { get; set; }
        public abstract class selected : PX.Data.BQL.BqlBool.Field<selected> { }
        #endregion
        #region ATPTEFMProjectBudgetID
        [PXDBIdentity(IsKey = true)]
        [PXSelector(typeof(aTPTEFMProjectBudgetID))]
        public virtual int? ATPTEFMProjectBudgetID { get; set; }
        public abstract class aTPTEFMProjectBudgetID : PX.Data.BQL.BqlInt.Field<aTPTEFMProjectBudgetID> { }
        #endregion
        #region LedgerID
        [PXDBInt]
        [PXDefault]
        [PXSelector(typeof(Ledger.ledgerID),
                    typeof(Ledger.ledgerCD),
                    typeof(Ledger.descr),
                    SubstituteKey = typeof(Ledger.ledgerCD),
                    DescriptionField = typeof(Ledger.descr))]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.LedgerID, Enabled = true)]
        public virtual int? LedgerID { get; set; }
        public abstract class ledgerID : PX.Data.BQL.BqlInt.Field<ledgerID> { }
        #endregion
        #region FinYear
        [PXDBString(4, IsFixed = true)]
        [GenericFinYearSelector(typeof(Search3<FinYear.year, OrderBy<Desc<FinYear.year>>>), null)]
        [PXDefault]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.FinYear)]
        public virtual string FinYear { get; set; }
        public abstract class finYear : PX.Data.BQL.BqlString.Field<finYear> { }
        #endregion
        #region ProjectID
        [PXDBInt]
        [PXSelector(typeof(PMProject.contractID),
                    typeof(PMProject.contractCD),
                    typeof(PMProject.description),
                    SubstituteKey = typeof(PMProject.contractCD),
                    DescriptionField = typeof(PMProject.description))]
        [PXRestrictor(typeof(Where<PMProject.isActive, Equal<True>>), PX.Objects.GL.Messages.AccountInactive)]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.ProjectID, Visibility = PXUIVisibility.Visible, Enabled = true)]
        [PXDefault]
        public virtual int? ProjectID { get; set; }
        public abstract class projectID : PX.Data.BQL.BqlInt.Field<projectID> { }
        #endregion
        #region Description
        [PXDBString(60, IsUnicode = true)]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.Description)]
        public virtual string Description { get; set; }
        public abstract class description : PX.Data.BQL.BqlString.Field<description> { }
        #endregion
        #region IsActive
        [PXBool]
        [PXUnboundDefault(false)]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.Selected)]
        public bool? IsActive { get; set; }
        public abstract class isActive : PX.Data.BQL.BqlBool.Field<isActive> { }
        #endregion
        #region NoteID

        public abstract class noteID : PX.Data.BQL.BqlGuid.Field<noteID> { }
        protected Guid? _NoteID;
        [PXNote()]
        public virtual Guid? NoteID
        {
            get
            {
                return this._NoteID;
            }
            set
            {
                this._NoteID = value;
            }
        }
        #endregion

    }
}