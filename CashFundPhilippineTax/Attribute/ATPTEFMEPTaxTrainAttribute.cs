//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using PX.Data;
//using PX.Objects.EP;
//using PX.Objects.TX;
//using PX.Objects.IN;
//using PX.Objects.AP;
//using CashFundManagement.Attributes;
//using ATPTPhilippineTaxation.DAC.Extensions;
//using CashFundManagement.Extensions.DAC;
//using CashFundManagement.Messages;

//namespace CashFundPhilippineTax.Attributes
//{
//    public class ATPTEFMEPTaxTrainAttribute : EPTaxAttribute
//    {
//        protected override void DefaultTaxes(PXCache sender, object row, bool DefaultExisting)
//        {
//            if ((!(row is EPExpenseClaimDetails)))
//            {
//                base.DefaultTaxes(sender, row, DefaultExisting);
//                return;
//            }
//            string ATPTEFMDefaultATC = null;
//            EPExpenseClaimDetails currentDetails = row as EPExpenseClaimDetails;
//            ATPTEFMEPExpenseClaimDetailsExt claimDetailsExt = currentDetails.GetExtension<ATPTEFMEPExpenseClaimDetailsExt>();
//            if (claimDetailsExt.UsrATPTEFMTranType == ATPTEFMExpenseTypeAttribute.RequestforPayment)
//            {
//                ATPTBAccount vendorExt = GetVendorDetails(sender.Graph, claimDetailsExt.UsrATPTVendorID).GetExtension<ATPTBAccount>();

//                Tax tax = PXSelectJoin< 
//                    Tax,
//                    InnerJoin<TaxCategoryDet,
//                        On<Tax.taxID, Equal<TaxCategoryDet.taxID>>,
//                    InnerJoin<TaxZoneDet,
//                        On<Tax.taxID, Equal<TaxZoneDet.taxID>>>>,
//                    Where<TaxCategoryDet.taxCategoryID, Equal<Required<TaxCategoryDet.taxCategoryID>>,
//                        And<TaxZoneDet.taxZoneID, Equal<Required<TaxZoneDet.taxZoneID>>,
//                        And<Tax.taxID, Equal<Required<Tax.taxID>>>>>>
//                    .Select(sender.Graph, currentDetails.TaxCategoryID, currentDetails.TaxZoneID, vendorExt.UsrATPTDefaultATC);

//                if (tax != null) ATPTEFMDefaultATC = vendorExt?.UsrATPTDefaultATC;
//            }
//            else
//            {
//                ATPTBAccount employeeExt = GetEmployeeDetails(sender.Graph, currentDetails.EmployeeID).GetExtension<ATPTBAccount>();

//                Tax tax = PXSelectJoin<
//                    Tax,
//                    InnerJoin<TaxCategoryDet,
//                        On<Tax.taxID, Equal<TaxCategoryDet.taxID>>,
//                    InnerJoin<TaxZoneDet,
//                        On<Tax.taxID, Equal<TaxZoneDet.taxID>>>>,
//                    Where<TaxCategoryDet.taxCategoryID, Equal<Required<TaxCategoryDet.taxCategoryID>>,
//                        And<TaxZoneDet.taxZoneID, Equal<Required<TaxZoneDet.taxZoneID>>,
//                And<Tax.taxID, Equal<Required<Tax.taxID>>>>>>
//                    .Select(sender.Graph, currentDetails.TaxCategoryID, currentDetails.TaxZoneID, employeeExt.UsrATPTDefaultATC);

//                if (tax != null) ATPTEFMDefaultATC = employeeExt?.UsrATPTDefaultATC;
//            }

//            if (ATPTEFMDefaultATC == null)
//            {
//                base.DefaultTaxes(sender, row, DefaultExisting);
//                return;
//            }
//            PXCache cache = sender.Graph.Caches[_TaxType];
//            string taxzone = GetTaxZone(sender, row);
//            string taxcat = GetTaxCategory(sender, row);
//            DateTime? docdate = GetDocDate(sender, row);
//            var applicableTaxes = new HashSet<string>();
//            foreach (PXResult<TaxZoneDet, TaxCategory, TaxRev, TaxCategoryDet> r in PXSelectJoin<
//                TaxZoneDet,
//                CrossJoin<TaxCategory,
//                InnerJoin<TaxRev, 
//                    On<TaxRev.taxID, Equal<TaxZoneDet.taxID>>,
//                LeftJoin<TaxCategoryDet, 
//                    On<TaxCategoryDet.taxID, Equal<TaxZoneDet.taxID>,
//                    And<TaxCategoryDet.taxCategoryID, Equal<TaxCategory.taxCategoryID>>>>>>,
//                Where<TaxZoneDet.taxZoneID, Equal<Required<TaxZoneDet.taxZoneID>>,
//                    And<TaxCategory.taxCategoryID, Equal<Required<TaxCategory.taxCategoryID>>,
//                    And<Required<TaxRev.startDate>, Between<TaxRev.startDate, TaxRev.endDate>, 
//                    And<TaxRev.outdated, Equal<False>,
//                    And<Where<TaxCategory.taxCatFlag, Equal<False>, 
//                        And<TaxCategoryDet.taxCategoryID, IsNotNull,
//                        Or<TaxCategory.taxCatFlag, Equal<True>, 
//                        And<TaxCategoryDet.taxCategoryID, IsNull>>>>>>>>>>
//                .Select(sender.Graph, taxzone, taxcat, docdate))
//            {
//                Tax tax = GetTax(sender.Graph, ((TaxZoneDet)r).TaxID);
//                if (tax.TaxType != CSTaxType.Withholding)
//                {
//                    AddOneTax(cache, row, (TaxZoneDet)r);
//                    applicableTaxes.Add(((TaxZoneDet)r).TaxID);
//                }

//            }
//            AddOneTax(cache, row, new TaxZoneDet()
//            {
//                TaxID = ATPTEFMDefaultATC// employeeExt.UsrATPTDefaultATC
//            });
//            applicableTaxes.Add(ATPTEFMDefaultATC);
//            string taxID;
//            if ((taxID = GetTaxID(sender, row)) != null)
//            {
//                AddOneTax(cache, row, new TaxZoneDet() { TaxID = taxID });
//                applicableTaxes.Add(taxID);
//            }
//            foreach (ITaxDetail r in ManualTaxes(sender, row))
//            {
//                if (applicableTaxes.Contains(r.TaxID))
//                    applicableTaxes.Remove(r.TaxID);
//            }
//            foreach (string applicableTax in applicableTaxes)
//            {
//                AddTaxTotals(cache, applicableTax, row);
//            }
//            if (DefaultExisting)
//            {
//                foreach (ITaxDetail r in MatchesCategory(sender, row, ManualTaxes(sender, row)))
//                {
//                    AddOneTax(cache, row, r);
//                }
//            }
//        }
//        public EPEmployee GetEmployeeDetails(PXGraph graph, int? employeeID) => PXSelectReadonly<
//            EPEmployee, 
//            Where<EPEmployee.bAccountID, Equal<Required<EPEmployee.bAccountID>>>>
//            .Select(graph, employeeID);
//        public VendorR GetVendorDetails(PXGraph graph, int? vendorID) => PXSelectReadonly<
//            VendorR, 
//            Where<VendorR.bAccountID, Equal<Required<VendorR.bAccountID>>>>
//            .Select(graph, vendorID);
//        public Tax GetTax(PXGraph graph, string taxID) => PXSelectReadonly<
//            Tax, 
//            Where<Tax.taxID, Equal<Required<Tax.taxID>>>>
//            .Select(graph, taxID);
//    }
//}
