using ATPTPhilippineTax.Attributes;
using ATPTPhilippineTax.DAC.Extensions;
using CashFundManagement.Extensions.DAC;
using CashFundManagement.Helper;
using CashFundManagement.Messages;
using CashFundPhilippineTax.Attribute.Extension;
using PX.Data;
using PX.Data.BQL;
using PX.Data.EP;
using PX.Objects.AP;
using PX.Objects.Common;
using PX.Objects.CR;
using PX.Objects.CR.MassProcess;
using PX.Objects.EP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Message = ATPTPhilippineTax.Messages.ATPTMessages;

namespace CashFundPhilippineTax.DAC.Extension
{
    /// <remarks>
    /// 009901 - (CFM2024R1) Require Vendor Details in Expense Receipt and Expense Claim pages for CA, FT, and RFP
    /// 010492 - (CFM2023R2/2024R1/2024R2) Expense Claim and Expense Receipt pages>Rename 'Address 1' to 'Vendor Address' and 'TIN' to 'Vendor TIN.'
    /// </remarks>
    public sealed class ATPTEFMEPExpenseClaimDetailsExtension : PXCacheExtension<CashFundManagement.Extensions.DAC.ATPTEFMEPExpenseClaimDetailsExt, EPExpenseClaimDetails>
    {
#if Version23R2
        public static bool IsActive() => ATPTPhilippineTax.Helpers.ATPTModule.IsActive;

#else
        public static bool IsActive() => ATPTPhilippineTax.Helpers.ATPTModule.IsActive && ATPTEFMPrefetchSetup.IsActive;
#endif
        #region Check ExpenseClaimDetailEntry Extension on Connector Package. Override Attributes to use Philtax Standard Behavior if Connector is published
        #region UsrATPTEFMVendorID
        [PXMergeAttributes(Method = MergeMethod.Replace)]
        [PXDBString(30, IsUnicode = true)]
        [PXFieldDescription]
        [PXDefault(PersistingCheck = PXPersistingCheck.Nothing)]
        [PXUIField(DisplayName = Message.VendorID)]
        [PXSelector(typeof(Search5<
            VendorR.acctCD,
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
            typeof(VendorR.acctCD),
            typeof(VendorR.acctName),
            ValidateValue = false)]
        [ATPTField]
        public string UsrATPTVendID { get; set; }
        public abstract class usrATPTVendID : BqlString.Field<usrATPTVendID> { }
        #endregion

        #region UsrATPTEFMVendName
        [PXMergeAttributes(Method = MergeMethod.Replace)]
        [PXDBString(100, IsUnicode = true)]
        [PXFieldDescription]
        [PXDefault(PersistingCheck = PXPersistingCheck.Nothing)]
        [PXUIField(DisplayName = Message.VendorName)]
        [PXUIEnabled(typeof(Where<ATPTEFMEPExpenseClaimDetailsExt.usrATPTVendID, IsNull>))]
        [PXFormula(typeof(Selector<ATPTEFMEPExpenseClaimDetailsExt.usrATPTVendID, VendorR.acctName>))]
        [ATPTField]
        [PXUIRequired(typeof(
             Where<GetSetupValue<ATPTEPSetup.usrATPTRequireVendorDetails>, Equal<True>,
                 And2<Where<Selector<EPExpenseClaimDetails.taxZoneID, ATPTTaxZone.usrATPTVendorNotRequired>, Equal<False>,
                     Or<Selector<EPExpenseClaimDetails.taxZoneID, ATPTTaxZone.usrATPTVendorNotRequired>, IsNull>>,
                 And<Where<ATPTEFMEPExpenseClaimDetailsExt.usrATPTEFMIsReclassifyDoc, Equal<False>>>>>))]
        public string UsrATPTVendName { get; set; }
        public abstract class usrATPTVendName : BqlString.Field<usrATPTVendName> { }
        #endregion

        #region UsrATPTEFMVendTIN
        [PXMergeAttributes(Method = MergeMethod.Replace)]
        [PXDBString(50, IsUnicode = true)]
        [PXDefault(PersistingCheck = PXPersistingCheck.Nothing)]
        [PXUIField(DisplayName = ATPTEFMMessages.VendorTIN)]
        [PXUIEnabled(typeof(Where<ATPTEFMEPExpenseClaimDetailsExt.usrATPTVendID, IsNull>))]
        [PXFormula(typeof(Selector<ATPTEFMEPExpenseClaimDetailsExt.usrATPTVendID, Location.taxRegistrationID>))]
        [ATPTField]
        [PXUIRequired(typeof(
             Where<GetSetupValue<ATPTEPSetup.usrATPTRequireVendorDetails>, Equal<True>,
                 And2<Where<Selector<EPExpenseClaimDetails.taxZoneID, ATPTTaxZone.usrATPTVendorNotRequired>, Equal<False>,
                     Or<Selector<EPExpenseClaimDetails.taxZoneID, ATPTTaxZone.usrATPTVendorNotRequired>, IsNull>>,
                 And<Where<ATPTEFMEPExpenseClaimDetailsExt.usrATPTEFMIsReclassifyDoc, Equal<False>>>>>))]
        public string UsrATPTVendTIN { get; set; }
        public abstract class usrATPTVendTIN : BqlString.Field<usrATPTVendTIN> { }

        #endregion

        #region UsrATPTEFMVendAddr
        [PXMergeAttributes(Method = MergeMethod.Replace)]
        [PXDBString(250, IsUnicode = true)]
        [PXMassMergableField]
        [PXDefault(PersistingCheck = PXPersistingCheck.Nothing)]
        [PXUIField(DisplayName = ATPTEFMMessages.VendorAddress)]
        [PXUIEnabled(typeof(Where<ATPTEFMEPExpenseClaimDetailsExt.usrATPTVendID, IsNull>))]
        //Not working when selecting Employee as Vendor
        //[PXFormula(typeof(Selector<ATPTEFMEPExpenseClaimDetailsExt.usrATPTVendID, Address.addressLine1>))]
        [PXUIRequired(typeof(
             Where<GetSetupValue<ATPTEPSetup.usrATPTRequireVendorDetails>, Equal<True>,
                 And2<Where<Selector<EPExpenseClaimDetails.taxZoneID, ATPTTaxZone.usrATPTVendorNotRequired>, Equal<False>,
                     Or<Selector<EPExpenseClaimDetails.taxZoneID, ATPTTaxZone.usrATPTVendorNotRequired>, IsNull>>,
                 And<Where<ATPTEFMEPExpenseClaimDetailsExt.usrATPTEFMIsReclassifyDoc, Equal<False>>>>>))]
        [ATPTField]
        public string UsrATPTAddress { get; set; }
        public abstract class usrATPTAddress : BqlString.Field<usrATPTAddress> { }

        #endregion
        #endregion
    }
}
