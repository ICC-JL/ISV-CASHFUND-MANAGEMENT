using CashFundManagement.Attributes;
using CashFundManagement.Extensions.DAC;
using CashFundManagement.Helper;
using PX.Data;
using PX.Objects.CS;
using PX.Objects.EP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashFundManagement.Extensions.BLC
{
    /// <summary>
    /// Expense Claim Detail Maint Extension
    /// </summary>
    /// <remarks>
    /// 2024-08-14 : Adds multi-tenant support. {RRS}
    /// </remarks>
    public class ATPTEFMExpenseClaimDetailMaintExt : PXGraphExtension<ExpenseClaimDetailMaint>
    {
#if Version23R2
        public static bool IsActive() => true;
#else
        public static bool IsActive() => ATPTEFMPrefetchSetup.IsActive;
#endif

        #region Events
        protected virtual void _(Events.RowSelected<EPExpenseClaimDetails> e, PXRowSelected baseMethod)
        {
            baseMethod(e.Cache, e.Args);

            Base.ClaimDetails.SetProcessDelegate(CFMClaimDetail);
        }
        #endregion

        #region Methods
        public static void CFMClaimDetail(List<EPExpenseClaimDetails> details)
        {
            CFMClaimDetail(details, false, false);
        }
        public static void CFMClaimSingleDetail(EPExpenseClaimDetails details, bool isApiContext = false)
        {
            CFMClaimDetail(new List<EPExpenseClaimDetails> { details }, isApiContext, true);
        }
        public static void CFMClaimDetail(List<EPExpenseClaimDetails> details, bool isApiContext, bool singleOperation)
        {
            ExpenseClaimEntry expenseClaimEntry = PXGraph.CreateInstance<ExpenseClaimEntry>();
            PXSetup<EPSetup> epsetup = new PXSetup<EPSetup>(PXGraph.CreateInstance(typeof(ExpenseClaimDetailEntry)));
            bool enabledApprovalReceipt = PXAccess.FeatureInstalled<FeaturesSet.approvalWorkflow>() && epsetup.Current.ClaimDetailsAssignmentMapID != null;
            bool isError = false;
            bool notAllApproved = false;
            Dictionary<string, EPExpenseClaim> result = new Dictionary<string, EPExpenseClaim>();

            var origDetails = details.ToDictionary(d => d.ClaimDetailCD);
            details = details.Select(d => EPExpenseClaimDetails.PK.Find(expenseClaimEntry, d)).ToList();

            IEnumerable<Receipts> List;

            if (epsetup.Current.AllowMixedTaxSettingInClaims == true)
            {
                List = details.Where(item => string.IsNullOrEmpty(item.RefNbr)).OrderBy(detail => detail.ClaimDetailID).GroupBy(
                                            item => new
                                            {
                                                item.EmployeeID,
                                                item.BranchID,
                                                item.CustomerID,
                                                item.CustomerLocationID,
                                                ClaimCuryID = ExpenseClaimDetailMaint.GetClaimCuryID(expenseClaimEntry, item),
                                                item.GetExtension<ATPTEFMEPExpenseClaimDetailsExt>().UsrATPTEFMTranType,
                                                item.GetExtension<ATPTEFMEPExpenseClaimDetailsExt>().UsrATPTEFMReqClass
                                            },
                                            (key, item) => new Receipts
                                            {
                                                employee = key.EmployeeID,
                                                branch = key.BranchID,
                                                customer = key.CustomerID,
                                                customerLocation = key.CustomerLocationID,
                                                claimCuryID = key.ClaimCuryID,
                                                details = item
                                            }
                                            );
            }
            else
            {
                List = details.Where(item => string.IsNullOrEmpty(item.RefNbr)).OrderBy(detail => detail.ClaimDetailID).GroupBy(
                                            item => new
                                            {
                                                item.EmployeeID,
                                                item.BranchID,
                                                item.CustomerID,
                                                item.CustomerLocationID,
                                                item.TaxZoneID,
                                                item.TaxCalcMode,
                                                ClaimCuryID = ExpenseClaimDetailMaint.GetClaimCuryID(expenseClaimEntry, item),
                                                item.GetExtension<ATPTEFMEPExpenseClaimDetailsExt>().UsrATPTEFMTranType,
                                                item.GetExtension<ATPTEFMEPExpenseClaimDetailsExt>().UsrATPTEFMReqClass
                                            },
                                            (key, item) => new Receipts
                                            {
                                                employee = key.EmployeeID,
                                                branch = key.BranchID,
                                                customer = key.CustomerID,
                                                customerLocation = key.CustomerLocationID,
                                                claimCuryID = key.ClaimCuryID,
                                                details = item
                                            }
                                            );
            }

            string errorMessage = null;

            foreach (Receipts item in List)
            {
                isError = false;
                notAllApproved = false;
                using (PXTransactionScope ts = new PXTransactionScope())
                {
                    expenseClaimEntry.Clear();
                    expenseClaimEntry.SelectTimeStamp();
                    EPExpenseClaim expenseClaim = (EPExpenseClaim)expenseClaimEntry.ExpenseClaim.Cache.CreateInstance();
                    expenseClaim.EmployeeID = item.employee;
                    expenseClaim.BranchID = item.branch;
                    expenseClaim.CustomerID = item.customer;
                    expenseClaim.DocDesc = PX.Objects.EP.Messages.SubmittedReceipt;
                    expenseClaim = expenseClaimEntry.ExpenseClaim.Update(expenseClaim);
                    expenseClaim.CuryID = item.claimCuryID;
                    expenseClaim = expenseClaimEntry.ExpenseClaim.Update(expenseClaim);
                    expenseClaim.CustomerLocationID = item.customerLocation;
                    expenseClaim.TaxCalcMode = item.details.First().TaxCalcMode;
                    expenseClaim.TaxZoneID = item.details.First().TaxZoneID;
                    #region Transfer customized field value from receipt to claim
                    if (item.details.First().GetExtension<ATPTEFMEPExpenseClaimDetailsExt>().UsrATPTEFMTranType == ATPTEFMExpenseTypeAttribute.RequestforPayment)
                    {
                        ATPTEFMEPExpenseClaimExt claimExt = expenseClaim.GetExtension<ATPTEFMEPExpenseClaimExt>();
                        claimExt.UsrATPTEFMTranType = item.details.First().GetExtension<ATPTEFMEPExpenseClaimDetailsExt>().UsrATPTEFMTranType;
                        claimExt.UsrATPTEFMReqType = item.details.First().GetExtension<ATPTEFMEPExpenseClaimDetailsExt>().UsrATPTEFMReqType;
                        claimExt.UsrATPTVendorID = item.details.First().GetExtension<ATPTEFMEPExpenseClaimDetailsExt>().UsrATPTVendorID;
                        claimExt.UsrATPTEFMReqClass = item.details.First().GetExtension<ATPTEFMEPExpenseClaimDetailsExt>().UsrATPTEFMReqClass;
                    }
                    #endregion

                    foreach (EPExpenseClaimDetails detail in item.details)
                    {
                        if (origDetails.TryGetValue(detail.ClaimDetailCD, out EPExpenseClaimDetails origRow))
                        {
                            PXProcessing<EPExpenseClaimDetails>.SetCurrentItem(origRow);
                        }
                        else
                        {
                            PXProcessing<EPExpenseClaimDetails>.SetCurrentItem(detail);
                        }


                        if (detail.Approved ?? false)
                        {
                            try
                            {
                                if (detail.IsPaidWithCard)
                                {
                                    EPEmployee employee =
                                        PXSelect<
                                            EPEmployee,
                                            Where<EPEmployee.bAccountID, Equal<Required<EPEmployee.bAccountID>>>>
                                            .Select(expenseClaimEntry, item.employee);

                                    if (employee.AllowOverrideCury != true && detail.CardCuryID != employee.CuryID)
                                    {
                                        errorMessage = PXMessages.Localize(PX.Objects.EP.Messages.ClaimCannotBeCreatedForReceiptBecauseCuryCannotBeOverriden);

                                        isError = true;
                                    }
                                }

                                if (!isError && detail.TipAmt != 0 && epsetup.Current.NonTaxableTipItem == null)
                                {
                                    errorMessage = PX.Objects.EP.Messages.TipItemIsNotDefined;
                                    isError = true;
                                }

                                if (!isError)
                                {
                                    expenseClaimEntry.ReceiptEntryExt.SubmitReceiptExt(expenseClaimEntry.ExpenseClaim.Cache,
                                        expenseClaimEntry.ExpenseClaimDetails.Cache, expenseClaimEntry.ExpenseClaim.Current, detail);

                                    expenseClaimEntry.Save.Press();
                                    if (!result.ContainsKey(expenseClaim.RefNbr))
                                    {
                                        result.Add(expenseClaim.RefNbr, expenseClaim);
                                    }
                                    detail.RefNbr = expenseClaim.RefNbr;

                                    // Display refNbr on processing grid
                                    if (origRow != null)
                                    {
                                        origRow.RefNbr = expenseClaim.RefNbr;
                                    }

                                    PXProcessing<EPExpenseClaimDetails>.SetProcessed();
                                }

                            }
                            catch (Exception ex)
                            {
                                errorMessage = ex.Message;
                                isError = true;
                            }
                        }
                        else
                        {
                            errorMessage = enabledApprovalReceipt
                                ? PX.Objects.EP.Messages.ReceiptNotApproved
                                : PX.Objects.EP.Messages.ReceiptTakenOffHold;

                            notAllApproved = true;
                        }

                        if (errorMessage != null)
                        {
                            PXProcessing<EPExpenseClaimDetails>.SetError(errorMessage);
                        }
                    }
                    if (!isError)
                    {
                        ts.Complete();
                    }
                }
            }

            if (!isError && !notAllApproved)
            {
                if (result.Count == 1 && isApiContext == false)
                {
                    expenseClaimEntry = PXGraph.CreateInstance<ExpenseClaimEntry>();
                    PXRedirectHelper.TryRedirect(expenseClaimEntry, result.First().Value, PXRedirectHelper.WindowMode.InlineWindow);
                }
            }
            else
            {
                PXProcessing<EPExpenseClaimDetails>.SetCurrentItem(null);
                if (singleOperation)
                {
                    throw new PXException(errorMessage);
                }
                else
                {
                    throw new PXException(PX.Objects.EP.Messages.ErrorProcessingReceipts);
                }
            }
        }
        #endregion

        #region Internal class
        private class Receipts
        {
            public int? employee;
            public int? branch;
            public int? customer;
            public int? customerLocation;
            public string claimCuryID;
            public IEnumerable<EPExpenseClaimDetails> details;
        };
        #endregion
    }
}
