using ATPTPhilippineTax.DAC.Extensions;
using ATPTPhilippineTax.Helpers;
using CashFundManagement.Attributes;
using CashFundManagement.Helper;
using PX.Data;
using PX.Objects.AP;
using PX.Objects.Common;
using PX.Objects.CR;
using PX.Objects.EP;
using EFMExt = CashFundManagement.Extensions.DAC;
using PhilTaxExt = ATPTPhilippineTax.DAC.Extensions;

namespace CashFundPhilippineTax.Graph.Extension
{
    /// <remarks>
    /// 2024-08-14 : Adds multi-tenant support. {RRS}
    /// 010461 - (CFM2024R1, CFM2024R1A & B) Error upon changing the Vendor in the Expense Receipts from profiled to non-profiled
    /// 2025-03-10 : Removed some events related to when CFM has its own Vendor Detail fields : 010624 : RFS
    /// 2025-06-25 : If the receipt is for reclassification, required vendor validation must not be included.   CASE 011620 : JLG
    /// 2025-02-13:  Apply multi tenant to 24r1 CASE: 010269 {JLTG}
    /// </remarks>
    public class ATPTExpenseClaimDetailEntry : PXGraphExtension<PX.Objects.EP.ExpenseClaimDetailEntry>
    {
#if Version23R2
        public static bool IsActive() => ATPTModule.IsActive;
#else
        public static bool IsActive() => ATPTModule.IsActive && ATPTEFMPrefetchSetup.IsActive;
#endif

        #region Events

        //public void _(Events.FieldUpdated<EPExpenseClaimDetails, EFMExt.ATPTEFMEPExpenseClaimDetailsExt.usrATPTVendorID> e)
        //{
        //    EPExpenseClaimDetails row = (EPExpenseClaimDetails)e.Row;
        //    if (row == null) return;
        //    EFMExt.ATPTEFMEPExpenseClaimDetailsExt efmExt = row.GetExtension<EFMExt.ATPTEFMEPExpenseClaimDetailsExt>();

        //    VendorR vendor = PXSelect<
        //        VendorR,
        //        Where<VendorR.bAccountID, Equal<Required<VendorR.bAccountID>>>>
        //        .Select(this.Base, efmExt.UsrATPTVendorID);

        //    if (vendor != null)
        //    {
        //        Address address = PXSelect<
        //            Address,
        //            Where<Address.bAccountID, Equal<Required<Address.bAccountID>>>>
        //            .Select(this.Base, vendor.BAccountID);

        //        Location location = PXSelect<
        //            Location,
        //            Where<Location.bAccountID, Equal<Required<Location.bAccountID>>>>
        //            .Select(this.Base, vendor.BAccountID);

        //        PhilTaxExt.ATPTEPExpenseClaimDetails rowExt = row.GetExtension<PhilTaxExt.ATPTEPExpenseClaimDetails>();
        //        if (rowExt == null) return;
        //        rowExt.UsrATPTVendID = vendor.AcctCD;
        //        rowExt.UsrATPTVendName = vendor.AcctName;
        //        rowExt.UsrATPTVendTin = location?.TaxRegistrationID;
        //        rowExt.UsrATPTAddress = address?.AddressLine1;
        //    }
        //}

        //public void _(Events.FieldUpdated<EPExpenseClaimDetails, EFMExt.ATPTEFMEPExpenseClaimDetailsExt.usrATPTEFMVendorBAccountID> e)
        //{
        //    EPExpenseClaimDetails row = (EPExpenseClaimDetails)e.Row;

        //    EFMExt.ATPTEFMEPExpenseClaimDetailsExt efmExt = row.GetExtension<EFMExt.ATPTEFMEPExpenseClaimDetailsExt>();
        //    if (row == null) return;

        //    // VendorR vendor = (VendorR)PXSelectorAttribute.Select<EFMExt.ATPTEFMEPExpenseClaimDetailsExt.usrATPTVendorID>(e.Cache, e.Row);

        //    VendorR vendor = PXSelect<
        //        VendorR,
        //        Where<VendorR.bAccountID, Equal<Required<VendorR.bAccountID>>>>
        //        .Select(this.Base, efmExt.UsrATPTEFMVendorBAccountID);

        //    if (vendor != null)
        //    {
        //        Address address = PXSelect<
        //            Address,
        //            Where<Address.addressID, Equal<Required<Address.addressID>>>>
        //            .Select(this.Base, vendor.DefAddressID);

        //        Location location = PXSelect<
        //            Location,
        //            Where<Location.locationID, Equal<Required<Location.locationID>>>>
        //            .Select(this.Base, vendor.DefLocationID);

        //        PhilTaxExt.ATPTEPExpenseClaimDetails rowExt = row.GetExtension<PhilTaxExt.ATPTEPExpenseClaimDetails>();
        //        rowExt.UsrATPTVendID = vendor.AcctCD;
        //        rowExt.UsrATPTVendName = vendor.AcctName;
        //        rowExt.UsrATPTVendTin = location?.TaxRegistrationID;
        //        rowExt.UsrATPTAddress = address?.AddressLine1;
        //    }
        //}
        public void _(Events.FieldUpdated<EPExpenseClaimDetails, PhilTaxExt.ATPTEPExpenseClaimDetails.usrATPTATCCode> e)
        {
            EPExpenseClaimDetails row = (EPExpenseClaimDetails)e.Row;

            if (row is null) return;

            PhilTaxExt.ATPTEPExpenseClaimDetails philtaxExt = row.GetExtension<PhilTaxExt.ATPTEPExpenseClaimDetails>();
            EFMExt.ATPTEFMEPExpenseClaimDetailsExt efmExt = row.GetExtension<EFMExt.ATPTEFMEPExpenseClaimDetailsExt>();

            if (efmExt != null)
            {
                efmExt.UsrATPTEFMATCCode = philtaxExt.UsrATPTATCCode;
            }
        }

        //Detail Vendor ID
        //public void _(Events.FieldUpdated<EPExpenseClaimDetails, PhilTaxExt.ATPTEPExpenseClaimDetails.usrATPTVendID> e)
        //{
        //    EPExpenseClaimDetails row = (EPExpenseClaimDetails)e.Row;
        //    PhilTaxExt.ATPTEPExpenseClaimDetails philtaxExt = row.GetExtension<PhilTaxExt.ATPTEPExpenseClaimDetails>();

        //    EFMExt.ATPTEFMEPExpenseClaimDetailsExt efmExt = row.GetExtension<EFMExt.ATPTEFMEPExpenseClaimDetailsExt>();

        //    if (row == null) return;
        //    if (philtaxExt == null) return;

        //    VendorR vendor = PXSelect<
        //        VendorR,
        //        Where<VendorR.acctCD, Equal<Required<VendorR.acctCD>>>>
        //        .Select(this.Base, philtaxExt.UsrATPTVendID);

        //    if (vendor != null)
        //    {
        //        Location location = PXSelect<
        //            Location,
        //            Where<Location.bAccountID, Equal<Required<Location.bAccountID>>>>
        //            .Select(this.Base, vendor.BAccountID);

        //        if (efmExt.UsrATPTEFMTranType == ATPTEFMExpenseTypeAttribute.RequestforPayment)
        //            efmExt.UsrATPTVendorID = vendor.BAccountID;

        //        efmExt.UsrATPTEFMDetailVendorID = vendor.BAccountID;
        //        efmExt.UsrATPTEFMVendorBAccountID = vendor.BAccountID;
        //        row.TaxZoneID = location.VTaxZoneID;
        //    }
        //}
        #endregion

        #region Override
        // -> 12/22/23 Behavior already in Philtax
        //protected void EPTaxTran_RowInserted(PXCache cache, PXRowInsertedEventArgs e, PXRowInserted InvokeBaseHandler)
        //{
        //    if (InvokeBaseHandler != null)
        //        InvokeBaseHandler(cache, e);

        //    EPExpenseClaimDetails expenseClaims = this.Base.CurrentClaimDetails.Current;
        //    if (expenseClaims != null)
        //    {
        //        EPEmployee employee = PXSelect<
        //            EPEmployee, 
        //            Where<EPEmployee.bAccountID, Equal<Required<EPEmployee.bAccountID>>>>
        //            .Select(this.Base, expenseClaims.EmployeeID);
        //        if (employee != null)
        //        {
        //            BAccount bAEmployee = PXSelect<
        //                BAccount, 
        //                Where<BAccount.bAccountID, Equal<Required<BAccount.bAccountID>>>>
        //                .Select(this.Base, employee.BAccountID);

        //            if (bAEmployee != null)
        //            {
        //                PhilTaxExt.ATPTBAccount employeeext = employee.GetExtension<PhilTaxExt.ATPTBAccount>();

        //                if (employeeext != null)
        //                {
        //                    if (employeeext.UsrATPTDefaultATC != null)
        //                    {
        //                        foreach (EPTaxTran taxtran in this.Base.Taxes.Select())
        //                        {
        //                            if (taxtran.TaxID == employeeext.UsrATPTDefaultATC)
        //                            {
        //                                Tax tax = PXSelectJoin<
        //                                    Tax,
        //                                    InnerJoin<TaxCategoryDet,
        //                                        On<Tax.taxID, Equal<TaxCategoryDet.taxID>>,
        //                                    InnerJoin<TaxZoneDet,
        //                                        On<Tax.taxID, Equal<TaxZoneDet.taxID>>>>,
        //                                    Where<TaxCategoryDet.taxCategoryID, Equal<Required<TaxCategoryDet.taxCategoryID>>,
        //                                        And<TaxZoneDet.taxZoneID, Equal<Required<TaxZoneDet.taxZoneID>>,
        //                                        And<Tax.taxID, Equal<Required<Tax.taxID>>>>>>
        //                                    .Select(Base, expenseClaims.TaxCategoryID, expenseClaims.TaxZoneID, employeeext.UsrATPTDefaultATC);

        //                                if (tax == null) this.Base.Taxes.Delete(taxtran);
        //                            }
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //    }
        //}
        #endregion

        #region CachedAttached
        [PXMergeAttributes(Method = MergeMethod.Append)]
        [PXRemoveBaseAttribute(typeof(PXUIRequiredAttribute))]
        [PXUIRequired(typeof(
        Where<GetSetupValue<ATPTEPSetup.usrATPTRequireVendorDetails>, Equal<True>,
         And2<Where<Selector<EPExpenseClaimDetails.taxZoneID, ATPTTaxZone.usrATPTVendorNotRequired>, Equal<False>,
             Or<Selector<EPExpenseClaimDetails.taxZoneID, ATPTTaxZone.usrATPTVendorNotRequired>, IsNull>>,
         And<Where<EFMExt.ATPTEFMEPExpenseClaimDetailsExt.usrATPTEFMIsReclassifyDoc, Equal<False>>>>>))]
        protected virtual void _(Events.CacheAttached<PhilTaxExt.ATPTEPExpenseClaimDetails.usrATPTVendName> e) { }

        [PXMergeAttributes(Method = MergeMethod.Append)]
        [PXRemoveBaseAttribute(typeof(PXUIRequiredAttribute))]
        [PXUIRequired(typeof(
             Where<GetSetupValue<ATPTEPSetup.usrATPTRequireVendorDetails>, Equal<True>,
                 And2<Where<Selector<EPExpenseClaimDetails.taxZoneID, ATPTTaxZone.usrATPTVendorNotRequired>, Equal<False>,
                     Or<Selector<EPExpenseClaimDetails.taxZoneID, ATPTTaxZone.usrATPTVendorNotRequired>, IsNull>>,
                 And<Where<EFMExt.ATPTEFMEPExpenseClaimDetailsExt.usrATPTEFMIsReclassifyDoc, Equal<False>>>>>))]

        protected virtual void _(Events.CacheAttached<PhilTaxExt.ATPTEPExpenseClaimDetails.usrATPTAddress> e) { }

        [PXMergeAttributes(Method = MergeMethod.Append)]
        [PXRemoveBaseAttribute(typeof(PXUIRequiredAttribute))]
        [PXUIRequired(typeof(
             Where<GetSetupValue<ATPTEPSetup.usrATPTRequireVendorDetails>, Equal<True>,
                 And2<Where<Selector<EPExpenseClaimDetails.taxZoneID, ATPTTaxZone.usrATPTVendorNotRequired>, Equal<False>,
                     Or<Selector<EPExpenseClaimDetails.taxZoneID, ATPTTaxZone.usrATPTVendorNotRequired>, IsNull>>,
                 And<Where<EFMExt.ATPTEFMEPExpenseClaimDetailsExt.usrATPTEFMIsReclassifyDoc, Equal<False>>>>>))]
        protected virtual void _(Events.CacheAttached<PhilTaxExt.ATPTEPExpenseClaimDetails.usrATPTVendTin> e) { }
        #endregion

    }
}