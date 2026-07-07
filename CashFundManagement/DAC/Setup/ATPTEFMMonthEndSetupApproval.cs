using CashFundManagement.Extensions.DAC;
using PX.Data;
using PX.Objects.EP;
using System;

namespace CashFundManagement.DAC.Setup {
    [Serializable]
    [PXCacheName(Messages.ATPTEFMMessages.ATPTEFMMonthEndSetupApproval)]
    public class ATPTEFMMonthEndSetupApproval : Base.ATPTEFMAudit, IBqlTable, IAssignedMap
    {
        #region ApprovalID
        public abstract class approvalID : PX.Data.IBqlField
        {
        }
        protected int? _ApprovalID;
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
        /// RLMonthEndSetupApproval assignmentMapID Bql field.
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
                Where<EPAssignmentMap.entityType, Equal<ATPTEFMEPAssignmentMapExtension.ATPTEFMAssignmentMapType.AssignmentMapTypeATPTEFMMonthEnd>,
                    And<EPAssignmentMap.mapType, NotEqual<EPMapType.assignment>>>>),
                DescriptionField = typeof(EPAssignmentMap.name),
                SubstituteKey = typeof(EPAssignmentMap.name))]
        [PXRestrictor(typeof(Where<EPAssignmentMap.assignmentMapID,
                                NotIn2<Search<ATPTEFMMonthEndSetupApproval.assignmentMapID>>>), null, ShowWarning = true)]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.AssignmentMapID)]
        /// <summary>
        /// RLMonthEndSetupApproval AssignmentID nullable integer property field.
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
        /// RLMonthEndSetupApproval assignmentNotificationID Bql field.
        /// </summary>
        public abstract class assignmentNotificationID : PX.Data.IBqlField
        {
        }
        protected int? _AssignmentNotificationID;
        [PXDBInt]
        [PXSelector(typeof(PX.SM.Notification.notificationID), SubstituteKey = typeof(PX.SM.Notification.name))]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.AssignmentNotificationID)]
        /// <summary>
        /// RLMonthEndSetupApproval AssignmentNotificationID nullable integer property field.
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
        /// RLMonthEndSetupApproval isActive Bql field.
        /// </summary>
        public abstract class isActive : PX.Data.IBqlField
        {
        }
        /// <summary>
        /// RLMonthEndSetupApproval IsActive nullable boolean property field.
        /// </summary>
        protected Boolean? _IsActive;
        [PXDBBool()]
        [PXDefault(typeof(Search<ATPTEFMSetup.monthEndRequestApproval>), PersistingCheck = PXPersistingCheck.Nothing)]
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
        public abstract class noteID : PX.Data.BQL.BqlGuid.Field<noteID> { }
        #endregion

    }
}
