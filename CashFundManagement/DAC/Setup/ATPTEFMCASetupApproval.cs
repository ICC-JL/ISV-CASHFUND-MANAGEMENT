using CashFundManagement.Extensions.DAC;
using PX.Data;
using PX.Objects.EP;
using System;

namespace CashFundManagement.DAC.Setup {
    [Serializable]
    [PXCacheName(Messages.ATPTEFMMessages.ATPTEFMSetupApproval)]
    public class ATPTEFMCASetupApproval : Base.ATPTEFMAudit, IBqlTable, IAssignedMap
    {
        #region ApprovalID
        [PXDBIdentity(IsKey = true)]
        public virtual int? ApprovalID { get; set; }

        public abstract class approvalID : IBqlField { }
        #endregion

        #region AssignmentMapID
        [PXDefault]
        [PXDBInt()]
        [PXSelector(
            typeof(Search<EPAssignmentMap.assignmentMapID,
                    Where<EPAssignmentMap.entityType, Equal<ATPTEFMEPAssignmentMapExtension.ATPTEFMAssignmentMapType.AssignmentMapTypeATPTEFMCashAdvance>,
                        And<EPAssignmentMap.mapType, NotEqual<EPMapType.assignment>>>>),
            DescriptionField = typeof(EPAssignmentMap.name),
            SubstituteKey = typeof(EPAssignmentMap.name)
        )]
        [PXRestrictor(
            typeof(Where<EPAssignmentMap.assignmentMapID,
                    NotIn2<Search<ATPTEFMCASetupApproval.assignmentMapID>>>),
            null,
            ShowWarning = true
        )]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.AssignmentMapID)]
        public virtual int? AssignmentMapID { get; set; }

        public abstract class assignmentMapID : IBqlField { }
        #endregion

        #region AssignmentNotificationID
        [PXDBInt]
        [PXSelector(
            typeof(PX.SM.Notification.notificationID),
            SubstituteKey = typeof(PX.SM.Notification.name)
        )]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.AssignmentNotificationID)]
        public virtual int? AssignmentNotificationID { get; set; }

        public abstract class assignmentNotificationID : IBqlField { }
        #endregion

        #region IsActive
        [PXDBBool()]
        [PXDefault(typeof(Search<ATPTEFMCASetup.cashAdvanceRequestApproval>), PersistingCheck = PXPersistingCheck.Nothing)]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.IsActive)]
        public virtual bool? IsActive { get; set; }

        public abstract class isActive : IBqlField { }
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