using CashFundManagement.Attributes;
using CashFundManagement.Extensions.DAC;
using PX.Data;
using PX.Data.BQL;
using PX.Objects.EP;
using System;

namespace CashFundManagement.DAC.Setup {
    [Serializable]
    [PXCacheName(Messages.ATPTEFMMessages.FundsSetupApproval)]
    public class ATPTEFMFundsApprovalSetup : CashFundManagement.DAC.Base.ATPTEFMBase, IBqlTable, IAssignedMap
    {
        #region Audit
        #region CreatedByID
        /// <summary>
        /// Audit Bql field.
        /// </summary>
        public abstract class createdByID : IBqlField { }
        /// <summary>
        /// Audit Bql property field.
        /// </summary>
        [PXDBCreatedByID()]
        public virtual Guid? CreatedByID { get; set; }
        #endregion

        #region CreatedByScreenID
        /// <summary>
        /// Audit Bql field.
        /// </summary>
        public abstract class createdByScreenID : IBqlField { }
        /// <summary>
        /// Audit Bql property field.
        /// </summary>
        [PXDBCreatedByScreenID()]
        public virtual String CreatedByScreenID { get; set; }
        #endregion

        #region CreatedDateTime
        /// <summary>
        /// Audit Bql field.
        /// </summary>
        public abstract class createdDateTime : IBqlField { }
        /// <summary>
        /// Audit Bql property field.
        /// </summary>
        [PXDBCreatedDateTime()]
        [PXUIField(DisplayName = PXDBLastModifiedByIDAttribute.DisplayFieldNames.CreatedDateTime, Enabled = false, IsReadOnly = true)]
        public virtual DateTime? CreatedDateTime { get; set; }
        #endregion

        #region LastModifiedByID
        /// <summary>
        /// Audit Bql field.
        /// </summary>
        public abstract class lastModifiedByID : IBqlField { }
        /// <summary>
        /// Audit Bql property field.
        /// </summary>
        [PXDBLastModifiedByID()]
        public virtual Guid? LastModifiedByID { get; set; }
        #endregion

        #region LastModifiedByScreenID
        /// <summary>
        /// Audit Bql field.
        /// </summary>
        public abstract class lastModifiedByScreenID : IBqlField { }
        /// <summary>
        /// Audit Bql property field.
        /// </summary>
        [PXDBLastModifiedByScreenID()]
        public virtual String LastModifiedByScreenID { get; set; }
        #endregion

        #region LastModifiedDateTime
        /// <summary>
        /// Audit Bql field.
        /// </summary>
        public abstract class lastModifiedDateTime : IBqlField { }
        /// <summary>
        /// Audit Bql property field.
        /// </summary>
        [PXDBLastModifiedDateTime()]
        [PXUIField(DisplayName = PXDBLastModifiedByIDAttribute.DisplayFieldNames.LastModifiedDateTime, Enabled = false, IsReadOnly = true)]
        public virtual DateTime? LastModifiedDateTime { get; set; }
        #endregion

        #region tstamp
        /// <summary>
        /// Audit Bql field.
        /// </summary>
        public abstract class Tstamp : IBqlField { }
        /// <summary>
        /// Audit Bql property field.
        /// </summary>
        [PXDBTimestamp()]
        public virtual Byte[] tstamp { get; set; }
        #endregion
        #endregion

        #region ApprovalID
        [PXDBIdentity(IsKey = true)]
        public virtual int? ApprovalID { get; set; }
        public abstract class approvalID : BqlInt.Field<approvalID> { }
        #endregion

        #region AssignmentMapID
        [PXDefault]
        [PXDBInt()]
        [PXSelector(
            typeof(Search<EPAssignmentMap.assignmentMapID,
                    Where<EPAssignmentMap.entityType, Equal<ATPTEFMEPAssignmentMapExtension.ATPTEFMAssignmentMapType.AssignmentMapTypeATPTEFMFund>,
                        And<EPAssignmentMap.mapType, NotEqual<EPMapType.assignment>>>>),
            DescriptionField = typeof(EPAssignmentMap.name)
        )]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.AssignmentMapID)]
        public virtual int? AssignmentMapID { get; set; }

        public abstract class assignmentMapID : BqlInt.Field<assignmentMapID> { }
        #endregion

        #region AssignmentNotificationID
        [PXDBInt]
        [PXSelector(
            typeof(PX.SM.Notification.notificationID),
            SubstituteKey = typeof(PX.SM.Notification.name)
        )]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.AssignmentNotificationID)]
        public virtual int? AssignmentNotificationID { get; set; }

        public abstract class assignmentNotificationID : BqlInt.Field<assignmentNotificationID> { }
        #endregion

        #region IsActive
        [PXDBBool()]
        [PXDefault(true, PersistingCheck = PXPersistingCheck.Nothing)]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.IsActive, Visible = false)]
        public virtual bool? IsActive { get; set; }
        public abstract class isActive : BqlBool.Field<isActive> { }
        #endregion

        #region Fund Type
        [PXDBString(255, IsUnicode = true)]
        [ATPTEFMFundTypeAttribute.ATPTEFMFundType]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.FundType)]
        public virtual string FundType { get; set; }
        public abstract class fundType : BqlString.Field<fundType> { }
        #endregion

        #region Module Type
        [PXDBString(255, IsUnicode = true)]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.ModuleType)]
        [ATPTEFMApprovalModuleAttribute]
        [PXDefault(typeof(ATPTEFMSetup.approvalModule.FromCurrent), PersistingCheck = PXPersistingCheck.NullOrBlank)]
        public virtual string ModuleType { get; set; }
        public abstract class moduleType : IBqlField { }
        #endregion

        #region NoteID
        [PXNote]
        public virtual Guid? NoteID { get; set; }
        public abstract class noteID : BqlGuid.Field<noteID> { }
        #endregion
    }
}
