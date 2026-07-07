using CashFundManagement.Extensions.DAC;
using PX.Data;
using PX.Data.BQL;
using PX.Objects.EP;
using System;

namespace CashFundManagement.DAC.Setup {
    [PXCacheName("Replesnishment Setup Approval")]
    public class ATPTEFMReplenishmentSetupApproval : Base.ATPTEFMAudit, IBqlTable, IAssignedMap
    {
        #region ApprovalID
        /// <summary>
        /// RLReplenishmentSetupApproval approvedID Bql field.
        /// </summary>
        public abstract class approvalID : PX.Data.IBqlField
        {
        }
        protected int? _ApprovalID;
        /// <summary>
        /// RLReplenishmentSetupApproval ApprovalID nullable integer property field.
        /// </summary>
        [PXDBIdentity(IsKey = true)]
        public virtual int? ApprovalID
        {
            get
            {
                return this._ApprovalID;
            }
            set
            {
                this._ApprovalID = value;
            }
        }
        #endregion

        #region AssignmentMapID
        /// <summary>
        /// RLReplenishmentSetupApproval assignmentMapID Bql field.
        /// </summary>
        public abstract class assignmentMapID : PX.Data.IBqlField
        {
        }
        protected int? _AssignmentMapID;
        [PXDefault]
        [PXDBInt()]
        [PXSelector(
            typeof(Search<
                EPAssignmentMap.assignmentMapID,
                Where<EPAssignmentMap.entityType, Equal<ATPTEFMEPAssignmentMapExtension.ATPTEFMAssignmentMapType.AssignmentMapTypeATPTEFMReplenishment>,
                    And<EPAssignmentMap.mapType, NotEqual<EPMapType.assignment>>>>),
                DescriptionField = typeof(EPAssignmentMap.name),
                SubstituteKey = typeof(EPAssignmentMap.name))]
        [PXRestrictor(typeof(Where<EPAssignmentMap.assignmentMapID,
                                NotIn2<Search<ATPTEFMReplenishmentSetupApproval.assignmentMapID>>>), null, ShowWarning = true)]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.AssignmentMapID)]
        /// <summary>
        /// RLReplenishmentSetupApproval AssignmentID nullable integer property field.
        /// </summary>
        public virtual int? AssignmentMapID
        {
            get
            {
                return this._AssignmentMapID;
            }
            set
            {
                this._AssignmentMapID = value;
            }
        }
        #endregion

        #region AssignmentNotificationID
        /// <summary>
        /// RLReplenishmentSetupApproval assignmentNotificationID Bql field.
        /// </summary>
        public abstract class assignmentNotificationID : PX.Data.IBqlField
        {
        }
        protected int? _AssignmentNotificationID;
        [PXDBInt]
        [PXSelector(typeof(PX.SM.Notification.notificationID), SubstituteKey = typeof(PX.SM.Notification.name))]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.AssignmentNotificationID)]
        /// <summary>
        /// RLReplenishmentSetupApproval AssignmentNotificationID nullable integer property field.
        /// </summary>
        public virtual int? AssignmentNotificationID
        {
            get
            {
                return this._AssignmentNotificationID;
            }
            set
            {
                this._AssignmentNotificationID = value;
            }
        }
        #endregion

        #region IsActive
        /// <summary>
        /// RLReplenishmentSetupApproval isActive Bql field.
        /// </summary>
        public abstract class isActive : PX.Data.IBqlField
        {
        }
        /// <summary>
        /// RLReplenishmentSetupApproval IsActive nullable boolean property field.
        /// </summary>
        protected Boolean? _IsActive;
        [PXDBBool()]
        [PXDefault(typeof(Search<ATPTEFMSetup.replenishmentRequestApproval>), PersistingCheck = PXPersistingCheck.Nothing)]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.IsActive)]
        public virtual Boolean? IsActive
        {
            get
            {
                return this._IsActive;
            }
            set
            {
                this._IsActive = value;
            }
        }
        #endregion

        #region NoteID
        [PXNote]
        public virtual Guid? NoteID { get; set; }
        public abstract class noteID : BqlGuid.Field<noteID> { }
        #endregion

    }
}
