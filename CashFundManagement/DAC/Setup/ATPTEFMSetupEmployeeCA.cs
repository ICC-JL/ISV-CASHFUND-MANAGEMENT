using System;
using System.Collections;
using System.Collections.Generic;
using PX.SM;
using PX.Data;
using CashFundManagement.Messages;
using PX.Objects.EP;
//TODO : Remove dead codes on next upgrade. This class might be removed on next upgrade.
namespace CashFundManagement.DAC.Setup
{
    //[Serializable]
    //[PXCacheName(ATPTEFMMessages.ATPTEFMSetupEmployeeCA)]
    //public class ATPTEFMSetupEmployeeCA : Base.ATPTEFMAudit, IBqlTable
    //{
    //    #region SetupEmployeeCAID
    //    [PXDBIdentity(IsKey = true)]
    //    public virtual int? SetupEmployeeCAID { get; set; }
    //    public abstract class setupEmployeeCAID : PX.Data.BQL.BqlLong.Field<setupEmployeeCAID> { }
    //    #endregion
        
    //    #region EmployeeID
    //    [PXDBInt]
    //    [PXUnique(ErrorMessage = ATPTEFMMessages.CashAdvanceEmployeeExist)]
    //    [PXSelector(
    //        typeof(Search<EPEmployee.bAccountID>),
    //        SubstituteKey = typeof(EPEmployee.acctCD),
    //        DescriptionField = typeof(EPEmployee.acctName)
    //    )]
    //    [PXDefault]
    //    [PXUIField(DisplayName = ATPTEFMMessages.EmployeeID)]
    //    public virtual int? EmployeeID { get; set; }
    //    public abstract class employeeID : PX.Data.BQL.BqlInt.Field<employeeID> { }
    //    #endregion

    //    #region AllowedCA
    //    [PXDBInt]
    //    [PXDefault(TypeCode.Int32, "0")]
    //    [PXUIField(DisplayName = ATPTEFMMessages.AllowedCA)]
    //    public virtual int? AllowedCA { get; set; }
    //    public abstract class allowedCA : PX.Data.BQL.BqlInt.Field<allowedCA> { }
    //    #endregion

    //    #region AdditionalCA
    //    [PXDBInt]
    //    [PXDefault(TypeCode.Int32, "0")]
    //    [PXUIField(DisplayName = ATPTEFMMessages.AdditionalCA)]
    //    public virtual int? AdditionalCA { get; set; }
    //    public abstract class additionalCA : PX.Data.BQL.BqlInt.Field<additionalCA> { }
    //    #endregion

    //    #region EndDate
    //    [PXDBDate]
    //    [PXDefault]
    //    [PXUIField(DisplayName = ATPTEFMMessages.EndDate)]
    //    public virtual DateTime? EndDate { get; set; }
    //    public abstract class endDate : PX.Data.BQL.BqlDateTime.Field<endDate> { }
    //    #endregion
    //}
}