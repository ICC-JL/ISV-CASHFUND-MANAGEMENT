using ATPTPhilippineTax.DAC.Extensions;
using ATPTPhilippineTax.Helpers;
using CashFundManagement.Attributes;
using CashFundManagement.Extensions.DAC;
using CashFundManagement.Helper;
using PX.Data;
using PX.Objects.AP;
using PX.Objects.CR;
using PX.Objects.EP;

namespace CashFundPhilippineTax.Graph.Extension 
{
    /// <remarks>
    /// 2024-08-14 : Adds multi-tenant support. {RRS}
    /// 010461 - (CFM2024R1, CFM2024R1A & B) Error upon changing the Vendor in the Expense Receipts from profiled to non-profiled
    /// 2025-03-12 : Removed Old Events related to CA Vendor fields : 010713 : RFS
    /// 2025-03-26 : Remove fieldupdated event : 010687 : RFS
    /// 2025-02-13:  Apply multi tenant to 24r1 CASE: 010269 {JLTG}
    /// </remarks>
    public class ATPTExpenseClaimEntry : PXGraphExtension<PX.Objects.EP.ExpenseClaimEntry>
    {
#if Version23R2
        public static bool IsActive() => ATPTModule.IsActive;
#else
        public static bool IsActive() => ATPTModule.IsActive && ATPTEFMPrefetchSetup.IsActive;
#endif

        #region Events
        //public void _(Events.RowInserting<EPExpenseClaimDetails> e, PXRowInserting baseMethod)
        //{
        //    if (baseMethod != null) baseMethod(e.Cache, e.Args);

        //    EPExpenseClaimDetails row = (EPExpenseClaimDetails)e.Row;
        //    ATPTEFMEPExpenseClaimDetailsExt rowExt = row.GetExtension<ATPTEFMEPExpenseClaimDetailsExt>();

        //    EPExpenseClaim claim = Base.ExpenseClaim.Current;
        //    ATPTEFMEPExpenseClaimExt claimExt = claim.GetExtension<ATPTEFMEPExpenseClaimExt>();

        //    if (claim == null) return;
        //    if (claimExt.UsrATPTEFMTranType == ATPTEFMExpenseTypeAttribute.RequestforPayment)
        //    {
        //        VendorR vendor = PXSelect<
        //        VendorR,
        //        Where<VendorR.bAccountID, Equal<Required<VendorR.bAccountID>>>>
        //        .Select(this.Base, claimExt.UsrATPTVendorID);

        //        if (vendor != null)
        //        {
        //            Address address = PXSelect<
        //              Address,
        //              Where<Address.bAccountID, Equal<Required<Address.bAccountID>>>>
        //              .Select(this.Base, vendor.BAccountID);

        //            Location location = PXSelect<
        //                 Location,
        //                 Where<Location.bAccountID, Equal<Required<Location.bAccountID>>>>
        //                 .Select(this.Base, vendor.BAccountID);

        //            ATPTEPExpenseClaimDetails philTax = row.GetExtension<ATPTEPExpenseClaimDetails>();
        //            rowExt.UsrATPTVendorID = vendor.BAccountID;
        //            if (philTax == null) return;
        //            philTax.UsrATPTVendID = vendor.AcctCD;
        //            philTax.UsrATPTVendName = vendor.AcctName;
        //            philTax.UsrATPTVendTin = location?.TaxRegistrationID;
        //            philTax.UsrATPTAddress = address?.AddressLine1;
        //        }
        //    }
        //}

        //public void _(Events.FieldUpdated<EPExpenseClaim, ATPTEFMEPExpenseClaimDetailsExt.usrATPTVendorID> e)
        //{
        //    EPExpenseClaim row = (EPExpenseClaim)e.Row;
        //    ATPTEFMEPExpenseClaimExt efmExt = row.GetExtension<ATPTEFMEPExpenseClaimExt>();

        //    if (row == null) return;

        //    PXResultset<VendorR, Address, Location> vendorInfo = PXSelectJoin<
        //                            VendorR,
        //                            LeftJoin<Address,
        //                                On<Address.addressID, Equal<VendorR.defAddressID>>,
        //                            LeftJoin<Location,
        //                                On<Location.bAccountID, Equal<VendorR.defLocationID>>>>,
        //                            Where<VendorR.bAccountID, Equal<Required<VendorR.bAccountID>>>>
        //                            .Select<PXResultset<VendorR, Address, Location>>(Base, efmExt.UsrATPTVendorID);

        //    Vendor vendor = (Vendor)vendorInfo;
        //    Address address = (Address)vendorInfo;
        //    Location extAddress = (Location)vendorInfo;

        //    foreach (EPExpenseClaimDetails epDetail in Base.ExpenseClaimDetails.Select())
        //    {
        //        ATPTEPExpenseClaimDetails philtaxExt = epDetail.GetExtension<ATPTEPExpenseClaimDetails>();
        //        ATPTEFMEPExpenseClaimDetailsExt cfmDetExt = epDetail.GetExtension<ATPTEFMEPExpenseClaimDetailsExt>();

        //        Base.ExpenseClaimDetails.Current = epDetail;

        //        Base.ExpenseClaimDetails.Current.GetExtension<ATPTEFMEPExpenseClaimDetailsExt>().UsrATPTVendorID = vendor.BAccountID;
        //        //Base.ExpenseClaimDetails.Current.GetExtension<ATPTEPExpenseClaimDetails>().UsrATPTVendID = vendor?.AcctCD;
        //        e.Cache.SetValueExt<ATPTEPExpenseClaimDetails.usrATPTVendID>(epDetail, vendor?.AcctCD);
        //        Base.ExpenseClaimDetails.Current.GetExtension<ATPTEPExpenseClaimDetails>().UsrATPTVendName = vendor?.AcctName;
        //        Base.ExpenseClaimDetails.Current.GetExtension<ATPTEPExpenseClaimDetails>().UsrATPTVendTin = extAddress?.TaxRegistrationID;
        //        Base.ExpenseClaimDetails.Current.GetExtension<ATPTEPExpenseClaimDetails>().UsrATPTAddress = address?.AddressLine1;
        //        Base.ExpenseClaimDetails.UpdateCurrent();
        //    }
        //}

        //public void _(Events.FieldUpdated<EPExpenseClaimDetails, ATPTEPExpenseClaimDetails.usrATPTVendID> e)
        //{
        //    EPExpenseClaimDetails row = (EPExpenseClaimDetails)e.Row;
        //    ATPTEPExpenseClaimDetails philtaxExt = row.GetExtension<ATPTEPExpenseClaimDetails>();

        //    ATPTEFMEPExpenseClaimDetailsExt efmExt = row.GetExtension<ATPTEFMEPExpenseClaimDetailsExt>();

        //    if (row == null) return;
        //    if (philtaxExt == null) return;

        //    VendorR vendor = PXSelect<
        //        VendorR,
        //        Where<VendorR.acctCD, Equal<Required<VendorR.acctCD>>>>
        //        .Select(this.Base, philtaxExt.UsrATPTVendID);

        //    if (vendor != null)
        //    {
        //        if (efmExt.UsrATPTEFMTranType == ATPTEFMExpenseTypeAttribute.RequestforPayment)
        //            efmExt.UsrATPTVendorID = vendor.BAccountID;

        //        else if (efmExt.UsrATPTEFMTranType == ATPTEFMExpenseTypeAttribute.Liquidation)
        //        {
        //            efmExt.UsrATPTEFMVendorBAccountID = vendor.BAccountID;
        //            efmExt.UsrATPTEFMDetailVendorID = vendor.BAccountID;
        //        }
        //    }
        //}

        public void _(Events.FieldUpdated<EPExpenseClaimDetails, ATPTEPExpenseClaimDetails.usrATPTATCCode> e)
        {
            EPExpenseClaimDetails row = (EPExpenseClaimDetails)e.Row;

            ATPTEPExpenseClaimDetails philtaxExt = row.GetExtension<ATPTEPExpenseClaimDetails>();
            ATPTEFMEPExpenseClaimDetailsExt efmExt = row.GetExtension<ATPTEFMEPExpenseClaimDetailsExt>();

            if (row == null) return;
            if (philtaxExt == null) return;

            efmExt.UsrATPTEFMATCCode = philtaxExt.UsrATPTATCCode;
        }

            //protected virtual void _(Events.RowUpdated<EPExpenseClaimDetails> e, PXRowUpdated baseMethod)
            //{
            //    baseMethod(e.Cache, e.Args);

            //    EPExpenseClaimDetails row = (EPExpenseClaimDetails)e.Row;
            //    if (row != null)
            //    {
            //        if (row.Released != true)
            //        {
            //            string oldTaxCat = row.TaxCategoryID;
            //            string oldTaxZone = row.TaxZoneID;

            //            Base.Caches[typeof(EPExpenseClaimDetails)].RaiseFieldUpdated<EPExpenseClaimDetails.taxZoneID>(e.Row, null);
            //            Base.Caches[typeof(EPExpenseClaimDetails)].RaiseFieldUpdated<EPExpenseClaimDetails.taxCategoryID>(e.Row, null);

            //            Base.Caches[typeof(EPExpenseClaimDetails)].SetValueExt<EPExpenseClaimDetails.taxZoneID>(e.Row, oldTaxZone);
            //            Base.Caches[typeof(EPExpenseClaimDetails)].SetValueExt<EPExpenseClaimDetails.taxCategoryID>(e.Row, oldTaxCat);
            //        }
            //    }
            //}
            #endregion

        #region Method
        //[PXMergeAttributes(Method = MergeMethod.Append)]
        //[PXRemoveBaseAttribute(typeof(EPTaxAttribute))]
        //[ATPTEFMEPTaxTrain]
        //protected virtual void EPExpenseClaimDetails_TaxCategoryID_CacheAttached(PXCache cache) { }

        //[PXMergeAttributes(Method = MergeMethod.Append)]
        //[PXSelector(typeof(Search2<
        //    VendorR.acctCD,
        //    InnerJoin<Address,
        //        On<Address.addressID, Equal<VendorR.defAddressID>>,
        //    LeftJoin<Location,
        //        On<Location.locationID, Equal<VendorR.defLocationID>>>>,
        //    Where2<
        //        Where<VendorR.vOrgBAccountID, In2<Search<PX.Objects.GL.Branch.bAccountID, Where<PX.Objects.GL.Branch.branchID, Equal<Current<AccessInfo.branchID>>>>>,
        //            Or<VendorR.vOrgBAccountID, Equal<Zero>,
        //                Or<VendorR.vOrgBAccountID,
        //                        In2<Search<PX.Objects.GL.DAC.Organization.bAccountID, Where<PX.Objects.GL.DAC.Organization.organizationID,
        //                            In2<Search<PX.Objects.GL.Branch.organizationID, Where<PX.Objects.GL.Branch.branchID, Equal<Current2<AccessInfo.branchID>>>>>>>>,
        //                        Or<VendorR.vOrgBAccountID,
        //                            In2<Search<PX.Objects.GL.DAC.Organization.bAccountID, Where<PX.Objects.GL.DAC.Organization.organizationID,
        //                                In2<Search<PX.Objects.GL.DAC.GroupOrganizationLink.groupID, Where<PX.Objects.GL.DAC.GroupOrganizationLink.organizationID,
        //                                    In2<Search<PX.Objects.GL.Branch.organizationID, Where<PX.Objects.GL.Branch.branchID, Equal<Current2<AccessInfo.branchID>>>>>>>>>>>>>>>,
        //        And<Where<Vendor.vStatus, Equal<VendorStatus.active>,
        //            Or<Vendor.vStatus, Equal<VendorStatus.oneTime>>>>>>), typeof(VendorR.acctCD), typeof(VendorR.acctName), DescriptionField = typeof(VendorR.acctName), ValidateValue = false)]
        //protected virtual void _(Events.CacheAttached<ATPTEPExpenseClaimDetails.usrATPTVendID> e) { }

        //[PXMergeAttributes(Method = MergeMethod.Append)]
        //[PXFormula(typeof(Selector<ATPTEPExpenseClaimDetails.usrATPTVendID, VendorR.acctName>))]
        //protected virtual void _(Events.CacheAttached<ATPTEPExpenseClaimDetails.usrATPTVendName> e) { }

        //[PXMergeAttributes(Method = MergeMethod.Append)]
        //[PXFormula(typeof(Selector<ATPTEPExpenseClaimDetails.usrATPTVendID, Address.addressLine1>))]
        //protected virtual void _(Events.CacheAttached<ATPTEPExpenseClaimDetails.usrATPTAddress> e) { }

        //[PXMergeAttributes(Method = MergeMethod.Append)]
        //[PXFormula(typeof(Selector<ATPTEPExpenseClaimDetails.usrATPTVendID, Location.taxRegistrationID>))]
        //protected virtual void _(Events.CacheAttached<ATPTEPExpenseClaimDetails.usrATPTVendTin> e) { }
        #endregion
    }
}