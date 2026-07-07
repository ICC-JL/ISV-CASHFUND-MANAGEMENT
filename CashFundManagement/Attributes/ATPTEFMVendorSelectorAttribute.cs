using PX.Data;
using PX.Objects.AP;
using PX.Objects.CR;
using PX.Objects.EP;
using System;

namespace CashFundManagement.Attributes
{
    [PXDBInt]
    [PXUIField(DisplayName = "Vendor")]
    [PXDimensionSelector("VENDOR", typeof(Search5<
            VendorR.bAccountID,
            LeftJoin<Address,
                On<Address.bAccountID, Equal<VendorR.bAccountID>>,
            LeftJoin<Location,
                On<Location.bAccountID, Equal<VendorR.bAccountID>>,
            LeftJoin<EPEmployee,
                On<EPEmployee.bAccountID, Equal<VendorR.bAccountID>>>>>,
            Where2<
                Where<VendorR.type, Equal<BAccountType.vendorType>,
                    Or<VendorR.type, Equal<BAccountType.employeeType>,
                    Or<VendorR.type, Equal<BAccountType.combinedType>>>>,
                And2<
                    Where<VendorR.vOrgBAccountID, In2<Search<PX.Objects.GL.Branch.bAccountID, Where<PX.Objects.GL.Branch.branchID, Equal<Current<AccessInfo.branchID>>>>>,
                        Or<VendorR.vOrgBAccountID, Equal<Zero>,
                        Or<VendorR.vOrgBAccountID,
                                In2<Search<PX.Objects.GL.DAC.Organization.bAccountID, Where<PX.Objects.GL.DAC.Organization.organizationID,
                                    In2<Search<PX.Objects.GL.Branch.organizationID, Where<PX.Objects.GL.Branch.branchID, Equal<Current2<AccessInfo.branchID>>>>>>>>,
                        Or<VendorR.vOrgBAccountID,
                                    In2<Search<PX.Objects.GL.DAC.Organization.bAccountID, Where<PX.Objects.GL.DAC.Organization.organizationID,
                                        In2<Search<PX.Objects.GL.DAC.GroupOrganizationLink.groupID, Where<PX.Objects.GL.DAC.GroupOrganizationLink.organizationID,
                                            In2<Search<PX.Objects.GL.Branch.organizationID, Where<PX.Objects.GL.Branch.branchID, Equal<Current2<AccessInfo.branchID>>>>>>>>>>>>>>>,
                    And<Where<Vendor.vStatus, Equal<VendorStatus.active>,
                        Or<Vendor.vStatus, Equal<VendorStatus.oneTime>>>>>>,
            Aggregate<
                GroupBy<VendorR.bAccountID>>>),
            typeof(VendorR.acctCD), new Type[] { typeof(VendorR.acctCD), typeof(VendorR.acctName), typeof(EPEmployee.parentBAccountID), typeof(Address.addressLine1), typeof(Address.addressLine2), typeof(Address.city), typeof(Location.taxRegistrationID) },
            DescriptionField = typeof(VendorR.acctName))]
    public class ATPTEFMVendorSelectorAttribute : PXAggregateAttribute
    {
    }
}
