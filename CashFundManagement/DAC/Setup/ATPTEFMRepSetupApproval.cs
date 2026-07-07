using System;
using System.Collections;
using System.Collections.Generic;
using PX.SM;
using PX.Data;
using PX.Objects.EP;
using CashFundManagement.Extensions.DAC;

//TODO : Remove dead codes on next upgrade. This class might be removed on next upgrade.
namespace CashFundManagement.DAC.Setup
{
    
    //[PXCacheName("Replesnishment Setup Approval")]
    //public class ATPTEFMRepSetupApproval : CashFundManagement.DAC.Base.ATPTEFMAudit, IBqlTable, IAssignedMap

    //{
    //    #region ApprovalID
    //    /// <summary>
    //    /// RLReplenishmentSetupApproval approvedID Bql field.
    //    /// </summary>
    //    public abstract class approvalID : PX.Data.IBqlField
    //    {
    //    }
    //    protected int? _ApprovalID;
    //    /// <summary>
    //    /// RLReplenishmentSetupApproval ApprovalID nullable integer property field.
    //    /// </summary>
    //    [PXDBIdentity(IsKey = true)]
    //    public virtual int? ApprovalID
    //    {
    //        get
    //        {
    //            return this._ApprovalID;
    //        }
    //        set
    //        {
    //            this._ApprovalID = value;
    //        }
    //    }
    //    #endregion
    //    #region AssignmentMapID
    //    /// <summary>
    //    /// RLReplenishmentSetupApproval assignmentMapID Bql field.
    //    /// </summary>
    //    public abstract class assignmentMapID : PX.Data.IBqlField
    //    {
    //    }
    //    protected int? _AssignmentMapID;
    //    [PXDefault]
    //    [PXDBInt()]
    //    [PXSelector(
    //        typeof(Search<
    //            EPAssignmentMap.assignmentMapID,
    //            Where<EPAssignmentMap.entityType, Equal<ATPTEFMEPAssignmentMapExtension.ATPTEFMAssignmentMapType.AssignmentMapTypeRLReplenishment>,
    //                And<EPAssignmentMap.mapType, NotEqual<EPMapType.assignment>>>>),
    //            DescriptionField = typeof(EPAssignmentMap.name))]
    //    [PXRestrictor(typeof(Where<EPAssignmentMap.assignmentMapID,
    //                            NotIn2<Search<ATPTEFMRepSetupApproval.assignmentMapID>>>), null, ShowWarning = true)]
    //    [PXUIField(DisplayName = CashFundManagement.Messages.ATPTEFMMessages.AssignmentMapID)]
    //    /// <summary>
    //    /// RLReplenishmentSetupApproval AssignmentID nullable integer property field.
    //    /// </summary>
    //    public virtual int? AssignmentMapID
    //    {
    //        get
    //        {
    //            return this._AssignmentMapID;
    //        }
    //        set
    //        {
    //            this._AssignmentMapID = value;
    //        }
    //    }
    //    #endregion
    //    #region AssignmentNotificationID
    //    /// <summary>
    //    /// RLReplenishmentSetupApproval assignmentNotificationID Bql field.
    //    /// </summary>
    //    public abstract class assignmentNotificationID : PX.Data.IBqlField
    //    {
    //    }
    //    protected int? _AssignmentNotificationID;
    //    [PXDBInt]
    //    [PXSelector(typeof(PX.SM.Notification.notificationID), SubstituteKey = typeof(PX.SM.Notification.name))]
    //    [PXUIField(DisplayName = CashFundManagement.Messages.ATPTEFMMessages.AssignmentNotificationID)]
    //    /// <summary>
    //    /// RLReplenishmentSetupApproval AssignmentNotificationID nullable integer property field.
    //    /// </summary>
    //    public virtual int? AssignmentNotificationID
    //    {
    //        get
    //        {
    //            return this._AssignmentNotificationID;
    //        }
    //        set
    //        {
    //            this._AssignmentNotificationID = value;
    //        }
    //    }
    //    #endregion
    //    #region IsActive
    //    /// <summary>
    //    /// RLReplenishmentSetupApproval isActive Bql field.
    //    /// </summary>
    //    public abstract class isActive : PX.Data.IBqlField
    //    {
    //    }
    //    /// <summary>
    //    /// RLReplenishmentSetupApproval IsActive nullable boolean property field.
    //    /// </summary>
    //    protected Boolean? _IsActive;
    //    [PXDBBool()]
    //    [PXDefault(typeof(Search<ATPTEFMSetup.replenishmentRequestApproval>), PersistingCheck = PXPersistingCheck.Nothing)]
    //    [PXUIField(DisplayName = CashFundManagement.Messages.ATPTEFMMessages.IsActive)]
    //    public virtual Boolean? IsActive
    //    {
    //        get
    //        {
    //            return this._IsActive;
    //        }
    //        set
    //        {
    //            this._IsActive = value;
    //        }
    //    }
    //    #endregion
    //}

}