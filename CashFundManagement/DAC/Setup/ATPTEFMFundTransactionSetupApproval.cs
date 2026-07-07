using CashFundManagement.Attributes;
using CashFundManagement.Extensions.DAC;
using PX.Data;
using PX.Data.BQL;
using PX.Objects.EP;
using System;

namespace CashFundManagement.DAC.Setup {
    [Serializable]
    [PXCacheName("Fund Transaction Setup Approval")]
    public class ATPTEFMFundTransactionSetupApproval : Base.ATPTEFMAudit, IBqlTable, IAssignedMap
    {
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
                    Where<EPAssignmentMap.entityType, Equal<ATPTEFMEPAssignmentMapExtension.ATPTEFMAssignmentMapType.AssignmentMapTypeATPTEFMFundTransaction>,
                        And<EPAssignmentMap.mapType, NotEqual<EPMapType.assignment>>>>),
            DescriptionField = typeof(EPAssignmentMap.name),
            SubstituteKey = typeof(EPAssignmentMap.name))]
        [PXRestrictor(
            typeof(Where<EPAssignmentMap.assignmentMapID,
                    NotIn2<Search<ATPTEFMFundTransactionSetupApproval.assignmentMapID>>>),
            null,
            ShowWarning = true)]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.AssignmentMapID)]
        public virtual int? AssignmentMapID { get; set; }
        public abstract class assignmentMapID : BqlInt.Field<assignmentMapID> { }
        #endregion

        #region AssignmentNotificationID
        [PXDBInt]
        [PXSelector(
            typeof(PX.SM.Notification.notificationID),
            SubstituteKey = typeof(PX.SM.Notification.name))]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.AssignmentNotificationID)]
        public virtual int? AssignmentNotificationID { get; set; }
        public abstract class assignmentNotificationID : BqlInt.Field<assignmentNotificationID> { }
        #endregion

        #region IsActive
        [PXDBBool()]
        [PXDefault(typeof(Search<ATPTEFMSetup.fundTransactionRequestApproval>), PersistingCheck = PXPersistingCheck.Nothing)]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.IsActive)]
        public virtual bool? IsActive { get; set; }
        public abstract class isActive : BqlBool.Field<isActive> { }
        #endregion

        #region FundTransactionType
        [PXDBString(1, IsFixed = true)]
        [ATPTEFMFundTransactionTypeAttribute.ATPTEFMFundTransactionType]
        [PXDefault(ATPTEFMFundTransactionTypeAttribute.IncreaseFundValue)]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.FundTransactionType)]
        public virtual string FundTransactionType { get; set; }
        public abstract class fundTransactionType : BqlString.Field<fundTransactionType> { }
        #endregion

        #region Module Type
        [PXDBString(255, IsUnicode = true)]
        [PXUIField(DisplayName = Messages.ATPTEFMMessages.ModuleType)]
        [ATPTEFMModuleTypeAttribute.ATPTEFMModuleType]
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
