using CashFundManagement.Attributes;
using CashFundManagement.DAC;
using CashFundManagement.DAC.Setup;
using CashFundManagement.Extensions.DAC;
using CashFundManagement.Helper;
using CashFundManagement.Messages;
using CashFundManagement.MethodExtensions;
using PX.Api;
using PX.Common;
using PX.Data;
using PX.Data.BQL;
using PX.Data.BQL.Fluent;
using PX.Data.WorkflowAPI;
using PX.Objects.AP;
using PX.Objects.CA;
using PX.Objects.Common;
using PX.Objects.Common.Extensions;
using PX.Objects.CR;
using PX.Objects.CS;
using PX.Objects.CT;
using PX.Objects.EP;
using PX.Objects.GL;
using PX.Objects.GL.FinPeriods;
using PX.Objects.GL.FinPeriods.TableDefinition;
using PX.Objects.IN;
using PX.Objects.TX;
using PX.TM;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using static CashFundManagement.BLC.ATPTEFMFundMaint;
using static CashFundManagement.Helper.ATPTEFMShared;

namespace CashFundManagement.BLC
{
    /// <remarks>
    /// 2025-05-09 : Add the FUNDID Reference at the end of the bill description : 010514 : RFS
    /// 2025-12-15 : Reimplement persist override to update Fund History records after saving Replenishment. 014607 {RRS}
    /// </remarks>
    public class ATPTEFMReplenishmentEntry : ATPTPXGraphWithWorkflow<ATPTEFMReplenishmentEntry, ATPTEFMReplenishment>
    {
        #region ctor + views
        public ATPTEFMReplenishmentEntry()
        {
#if !Version23R2
            if (!(CASetupPreferences?.Current?.EnableCFM ?? false))
            {
                throw new PXException(ATPTEFMMessages.CashFundManagementFeatureDisabled);
            }
#endif

            OnBeforePersist += delegate (PXGraph graph)
            {
                var primaryObject = graph.GetCurrentPrimaryObject() as ATPTEFMReplenishment;
                if (primaryObject == null) return;
                UpdateApprovalAmount();
            };

            OnAfterPersist += delegate (PXGraph graph)
            {
                ATPTEFMFundHistorySimple aTPTEFMFundHistory = CreateInstance<ATPTEFMFundHistorySimple>();
                #region Delete related fund history
                var deletedReplenishment = _deletedReplenishment.FirstOrDefault();
                if (deletedReplenishment != null)
                {
                    aTPTEFMFundHistory.Filter.Current = SelectFrom<ATPTEFMFund>.Where<ATPTEFMFund.fundCD.IsEqual<@P.AsString>>.View.Select(this, deletedReplenishment.FundID).FirstOrDefault();
                    var fundHistList = aTPTEFMFundHistory.Records.Select().RowCast<ATPTEFMFundTransactionHistoryView>().ToList();
                    foreach (var item in fundHistList.Where(f => f.ReplenishmentRefNbr == deletedReplenishment.ReplenishmentNbr))
                    {
                        item.ReplenishmentRefNbr = string.Empty;
                        aTPTEFMFundHistory.Records.Update(item);
                    }
                    ATPTEFMFundTransactionHistoryView repFundHist = fundHistList.Where(f => f.RefNbr == deletedReplenishment.ReplenishmentNbr).FirstOrDefault();
                    if (repFundHist != null)
                    {
                        aTPTEFMFundHistory.Records.Delete(repFundHist);
                        _deletedReplenishment.Clear();
                    }
                } 
                #endregion
                var primaryObject = graph.GetCurrentPrimaryObject() as ATPTEFMReplenishment;
                if (primaryObject != null)
                {
                    #region update fund history records.
                    aTPTEFMFundHistory.Filter.Current = SelectFrom<ATPTEFMFund>.Where<ATPTEFMFund.fundCD.IsEqual<@P.AsString>>.View.Select(this, primaryObject.FundID).FirstOrDefault();
                    var fundHistList = aTPTEFMFundHistory.Records.Select().RowCast<ATPTEFMFundTransactionHistoryView>().ToList();
                    if (Replenishments.Current.Step == ATPTEFMReplenishmentStepAttribute.DefaultValue)
                    {
                        var repDetails = ReplenishmentDetails.Select().RowCast<ATPTEFMReplenishmentDetail>().ToList();
                        #region Add ReplenishmentRefNbr in fund history for related receipts
                        foreach (ATPTEFMReplenishmentDetail item in repDetails)
                        {
                            ATPTEFMFundTransactionHistoryView detFundHist = fundHistList.Where(f => f.RefNbr == item.ExpenseReceiptNbr).FirstOrDefault();
                            if (detFundHist != null)
                            {
                                detFundHist.ReplenishmentRefNbr = item.ReplenishmentNbr;
                                aTPTEFMFundHistory.Records.Update(detFundHist);
                            }
                        }
                        #endregion

                        #region Remove ReplenishmentRefNbr in fund history for deleted receipts
                        foreach (var item in fundHistList.Where(f => f.ReplenishmentRefNbr == primaryObject.ReplenishmentNbr))
                        {
                            if (repDetails.Where(d => d.ExpenseReceiptNbr == item.RefNbr).FirstOrDefault() == null)
                            {
                                item.ReplenishmentRefNbr = string.Empty;
                                aTPTEFMFundHistory.Records.Update(item);
                            }
                        } 
                        #endregion
                    }
                    #endregion

                    #region upsert fund history record for the replenishment.
                    ATPTEFMFundTransactionHistoryView repFundHist = fundHistList.Where(f => f.RefNbr == primaryObject.ReplenishmentNbr).FirstOrDefault();
                    if (repFundHist != null)
                    {
                        repFundHist.CuryFundTransactionDocumentAmt = primaryObject.ClaimAmount;
                        repFundHist.Status = primaryObject.Status;
                        repFundHist.TransactionDate = primaryObject.Date;
                        aTPTEFMFundHistory.Records.Update(repFundHist);
                    }
                    else
                    {
                        aTPTEFMFundHistory.Records.Insert(CreateFundTransactionHistory(primaryObject));
                    } 
                    #endregion
                }
                
                if (aTPTEFMFundHistory.Filter.Current != null && aTPTEFMFundHistory.Save.GetEnabled())
                {
                    aTPTEFMFundHistory.Save.Press();
                }
            };
        }

        public PXSetup<CASetup> Setup;
        public PXSetup<ATPTEFMSetup> Preferences;
        public PXSetup<ATPTEFMCASetup> CASetupPreferences;
        public PXSetup<APSetup> ApSetup;
        public PXSetup<FeaturesSet> EnableFeatures;

        [PXViewName("Replenishment")]
        public PXSelect<ATPTEFMReplenishment> Replenishments;

        [PXViewName("Transaction History")]
        public PXSelect<
            ATPTEFMFundTransactionHistoryView,
            Where<ATPTEFMFundTransactionHistoryView.refNbr, Equal<Required<ATPTEFMFundTransactionHistoryView.refNbr>>>>
            TransactionHistoryView;

        [PXViewName("Fund")]
        public PXSelect<
            ATPTEFMFund,
            Where<ATPTEFMFund.fundCD, Equal<Required<ATPTEFMFundTransaction.fundID>>>>
            Fund;

        [PXViewName("Fund Transaction")]
        public PXSelect<
            ATPTEFMFundTransaction,
            Where<ATPTEFMFundTransaction.refNbr, Equal<Required<ATPTEFMFundTransaction.refNbr>>>>
            FundTransaction;

        [PXViewName("Fund Transaction Receipt")]
        public PXSelect<
            ATPTEFMFundTransactionReceiptDetail,
            Where<ATPTEFMFundTransactionReceiptDetail.expenseReceiptRefNbr, Equal<Required<ATPTEFMFundTransactionReceiptDetail.expenseReceiptRefNbr>>>>
            FundTransactionReceiptDetail;

        [PXViewName("Fund Transaction Reclassification Receipt")]
        public PXSelect<
            ATPTEFMFundTransactionReclassficationReceiptDetail,
            Where<ATPTEFMFundTransactionReclassficationReceiptDetail.expenseReceiptRefNbr, Equal<Required<ATPTEFMFundTransactionReceiptDetail.expenseReceiptRefNbr>>>>
            FundTransactionReclassificationReceiptDetail;
        

        [PXCopyPasteHiddenFields(typeof(ATPTEFMReplenishment.taxZone))]
        public PXSelect<
            ATPTEFMReplenishment,
            Where<ATPTEFMReplenishment.replenishmentNbr, Equal<Current<ATPTEFMReplenishment.replenishmentNbr>>>>
            ReplenishmentCurrent;

        [PXViewName("Replenishment Details")]
        public PXSelectJoin<
            ATPTEFMReplenishmentDetail,
            InnerJoin<EPExpenseClaimDetails,
                On<EPExpenseClaimDetails.claimDetailCD, Equal<ATPTEFMReplenishmentDetail.expenseReceiptNbr>>,
            LeftJoin<InventoryItem,
                On<InventoryItem.inventoryID, Equal<EPExpenseClaimDetails.inventoryID>>,
            LeftJoin<Contract,
                On<Contract.contractID, Equal<EPExpenseClaimDetails.contractID>>,
            LeftJoin<Vendor,
                On<Vendor.bAccountID, Equal<ATPTEFMEPExpenseClaimDetailsExt.usrATPTEFMDetailVendorID>>,
            LeftJoin<BAccount,
                On<BAccount.bAccountID, Equal<Vendor.bAccountID>>,
            LeftJoin<Address,
                On<Address.addressID, Equal<BAccount.defAddressID>>,
            LeftJoin<LocationExtAddress,
                On<LocationExtAddress.locationID, Equal<BAccount.defLocationID>>>>>>>>>,
            Where<ATPTEFMReplenishmentDetail.replenishmentNbr, Equal<Current<ATPTEFMReplenishment.replenishmentNbr>>>>
            ReplenishmentDetails;

        [PXViewName("Expense Receipts")]
        public PXSelect<
            EPExpenseClaimDetails,
            Where<EPExpenseClaimDetails.claimDetailCD, Equal<Required<EPExpenseClaimDetails.claimDetailCD>>>>
            ExpenseClaimDetails;

        public PXSelect<
            EPExpenseClaimDetails,
            Where<True, Equal<False>>>
            ReceiptsForSubmit;

        public PXSelect<
            InventoryItem,
            Where<True, Equal<False>>>
            Items;
        public PXSelect<
            Vendor,
            Where<True, Equal<False>>>
            Vendors;
        public PXSelect<
            Address,
            Where<True, Equal<False>>>
            Addresses;
        public PXSelect<
            LocationExtAddress,
            Where<True, Equal<False>>>
            ExtAddresses;

        public PXSelect<
            Contract,
            Where<True, Equal<False>>>
            Contracts;

        [PXViewName("Setup Approval")]
        public PXSelect<
            ATPTEFMReplenishmentSetupApproval,
            Where<ATPTEFMReplenishmentSetupApproval.isActive, Equal<True>>>
            SetupApproval;

        [PXViewName("Approval")]
        public EPApprovalAutomation<
            ATPTEFMReplenishment, ATPTEFMReplenishment.approved, ATPTEFMReplenishment.rejected, ATPTEFMReplenishment.hold, ATPTEFMReplenishmentSetupApproval>
            Approval;

        public PXSelectJoinGroupBy<
            APInvoice,
            InnerJoin<ATPTEFMReplenishmentDetail,
                On<APInvoice.origRefNbr, Equal<ATPTEFMReplenishmentDetail.replenishmentNbr>>>,
            Where<ATPTEFMReplenishmentDetail.replenishmentNbr, Equal<Current<ATPTEFMReplenishment.replenishmentNbr>>>,
            Aggregate<
                GroupBy<APInvoice.refNbr>>>
            APDocuments;

        public PXSelect<
            ATPTEFMReplenishmentTaxDetail,
            Where<ATPTEFMReplenishmentTaxDetail.replenishmentNbr, Equal<Current<ATPTEFMReplenishment.replenishmentNbr>>>>
            Taxes;

        public PXSelect<
            ATPTEFMReplenishment,
            Where<ATPTEFMReplenishment.replenishmentNbr, Equal<Current<ATPTEFMReplenishment.replenishmentNbr>>>>
            CurrentReplenishment;
        [PXCopyPasteHiddenView]
        public PXSelect<APInvoice> APInvoiceDocument;
        public PXSelectReadonly<APPayment> APPaymentDocument;

        public PXSelect<
            EPEmployee,
            Where<EPEmployee.bAccountID, Equal<Current<ATPTEFMReplenishment.custodianID>>>>
            Custodian;

        [PXCopyPasteHiddenView]
        public PXSelectJoin<
            EPTaxTran,
            InnerJoin<Tax,
                On<Tax.taxID, Equal<EPTaxTran.taxID>>>,
            Where<EPTaxTran.claimDetailID, Equal<Optional<ATPTEFMReplenishmentDetail.expenseReceiptID>>>>
            Tax_Rows;

        public PXSelect<
            EPExpenseClaimDetails,
            Where<EPExpenseClaimDetails.claimDetailID, Equal<Current<ATPTEFMReplenishmentDetail.expenseReceiptID>>>,
            OrderBy<
                Asc<EPExpenseClaimDetails.submitedDate,
                Asc<EPExpenseClaimDetails.createdDateTime,
                Asc<EPExpenseClaimDetails.claimDetailID>>>>>
            TaxOverrideExpenseClaimDetails;

        public SelectFrom<EPExpenseClaimDetails>
            .InnerJoin<ATPTEFMReplenishmentDetail>.On<ATPTEFMReplenishmentDetail.expenseReceiptNbr.IsEqual<EPExpenseClaimDetails.claimDetailCD>>
            .Where<ATPTEFMReplenishmentDetail.replenishmentNbr.IsEqual<ATPTEFMReplenishment.replenishmentNbr.FromCurrent>>
            .View RepDetExpenseReceipts;
        #endregion

        #region View Delegates
        public IEnumerable receiptsForSubmit()
        {
            HashSet<string> notIncludedReceipts = new HashSet<string>();
            List<EPExpenseClaimDetails> receiptsToBeVerified = new List<EPExpenseClaimDetails>();

            foreach (ATPTEFMReplenishmentDetail Receipt in ReplenishmentDetails.Select())
            {
                EPExpenseClaimDetails getRequest = PXSelect<
                    EPExpenseClaimDetails,
                    Where<EPExpenseClaimDetails.claimDetailCD, Equal<Required<EPExpenseClaimDetails.claimDetailCD>>>>
                    .Select(this, Receipt.ExpenseReceiptNbr);
                if (getRequest != null)
                {
                    notIncludedReceipts.Add(Receipt.ExpenseReceiptNbr);
                }
            }

            foreach (ATPTEFMReplenishmentDetail replenish in PXSelectJoin<
                ATPTEFMReplenishmentDetail,
                InnerJoin<ATPTEFMReplenishment,
                    On<ATPTEFMReplenishment.replenishmentNbr, Equal<ATPTEFMReplenishmentDetail.replenishmentNbr>,
                    And<ATPTEFMReplenishment.fundID, Equal<Current<ATPTEFMReplenishment.fundID>>,
                    And<ATPTEFMReplenishment.status, NotEqual<ATPTEFMReplenishmentStatusAttribute.cancelledValue>>>>>>
                .Select(this))
            {
                if (replenish != null)
                {
                    notIncludedReceipts.Add(replenish.ExpenseReceiptNbr);
                }
            }

            foreach (EPExpenseClaimDetails receipt in PXSelectJoin<
                EPExpenseClaimDetails,
                InnerJoin<ATPTEFMFundTransactionReceiptDetail,
                    On<ATPTEFMFundTransactionReceiptDetail.expenseReceiptRefNbr, Equal<EPExpenseClaimDetails.claimDetailCD>,
                    And<ATPTEFMFundTransactionReceiptDetail.replenishmentRefNbr, IsNull>>,
                InnerJoin<ATPTEFMFundTransaction,
                    On<ATPTEFMFundTransaction.refNbr, Equal<ATPTEFMFundTransactionReceiptDetail.fundTransactionRefNbr>,
                    And<ATPTEFMFundTransaction.fundID, Equal<Current<ATPTEFMReplenishment.fundID>>,
                    And<ATPTEFMFundTransaction.status, Equal<ATPTEFMReplenishmentStatusAttribute.closedValue>>>>,
                InnerJoin<EPEmployee,
                    On<EPEmployee.bAccountID, Equal<EPExpenseClaimDetails.employeeID>>>>>,
                Where<EPEmployee.userID, Equal<Current<AccessInfo.userID>>,
                    Or<EPEmployee.bAccountID, WingmanUser<Current<AccessInfo.userID>>,
                    Or<EPEmployee.defContactID, IsSubordinateOfContact<Current<AccessInfo.contactID>>>>>>
                .Select(this))
            {
                receiptsToBeVerified.Add(receipt);
            }

            foreach (EPExpenseClaimDetails reclassReceipt in PXSelectJoin<
                EPExpenseClaimDetails,
                InnerJoin<ATPTEFMFundTransactionReclassficationReceiptDetail,
                    On<ATPTEFMFundTransactionReclassficationReceiptDetail.expenseReceiptRefNbr, Equal<EPExpenseClaimDetails.claimDetailCD>,
                    And<ATPTEFMFundTransactionReclassficationReceiptDetail.replenishmentRefNbr, IsNull>>,
                InnerJoin<ATPTEFMFundTransaction,
                    On<ATPTEFMFundTransaction.refNbr, Equal<ATPTEFMFundTransactionReclassficationReceiptDetail.fundTransactionRefNbr>,
                    And<ATPTEFMFundTransaction.fundID, Equal<Current<ATPTEFMReplenishment.fundID>>,
                    And<ATPTEFMFundTransaction.status, Equal<ATPTEFMReplenishmentStatusAttribute.closedValue>>>>,
                InnerJoin<EPEmployee,
                    On<EPEmployee.bAccountID, Equal<EPExpenseClaimDetails.employeeID>>>>>,
                Where<EPEmployee.userID, Equal<Current<AccessInfo.userID>>,
                    Or<EPEmployee.bAccountID, WingmanUser<Current<AccessInfo.userID>>,
                    Or<EPEmployee.defContactID, IsSubordinateOfContact<Current<AccessInfo.contactID>>>>>>
                .Select(this))
            {
                receiptsToBeVerified.Add(reclassReceipt);
            }

            foreach (EPExpenseClaimDetails result in receiptsToBeVerified)
            {
                if (notIncludedReceipts.Contains(result.ClaimDetailCD))
                    continue;

                yield return result;
            }
        }

        /// <remarks>
        /// 2025-10-06 : Replenishment transaction has applying wrong transaction automatically under Financial Details Tab CASEID: 013775 {JLG} <br/>             
        /// </remarks>
        protected virtual IEnumerable apInvoiceDocument()
        {
            if (ReplenishmentDetails.SelectSingle() != null)
            {
                return PXSelectJoinGroupBy<
                    APInvoice,
                    InnerJoin<ATPTEFMReplenishmentDetail,
                        On<APInvoice.origRefNbr, Equal<ATPTEFMReplenishmentDetail.replenishmentNbr>,
                        And<ATPTEFMAPRegisterExt.usrATPTEFMIsFromReplenishment, Equal<True>>>>,
                    Where<ATPTEFMReplenishmentDetail.replenishmentNbr, Equal<Current<ATPTEFMReplenishment.replenishmentNbr>>>,
                    Aggregate<
                        GroupBy<APInvoice.docType,
                        GroupBy<APInvoice.refNbr>>>>
                    .Select(this);
            }
            else
            {
                return PXSelectReadonly<
                    APInvoice,
                    Where<APInvoice.origModule, Equal<BatchModule.moduleEP>,
                        And<APInvoice.origRefNbr, Equal<Current<ATPTEFMReplenishmentDetail.replenishmentNbr>>>>>
                    .Select(this);
            }
        }
        /// <remarks>
        /// 2025-10-06 : Replenishment transaction has applying wrong transaction automatically under Financial Details Tab CASEID: 013775 {JLG} <br/>             
        /// </remarks>
        protected virtual IEnumerable apPaymentDocument()
        {
            APInvoice aPInvoice = APInvoiceDocument.Current;

            if (aPInvoice != null)
            {
                return PXSelectJoinGroupBy<
                    APPayment,
                    InnerJoin<APAdjust,
                        On<APPayment.refNbr, Equal<APAdjust.adjgRefNbr>,
                        And<APPayment.docType, Equal<APAdjust.adjgDocType>>>,
                    InnerJoin<APInvoice,
                        On<APInvoice.refNbr, Equal<APAdjust.adjdRefNbr>>,
                    InnerJoin<ATPTEFMReplenishmentDetail,
                        On<APInvoice.origRefNbr, Equal<ATPTEFMReplenishmentDetail.replenishmentNbr>,
                        And<ATPTEFMAPRegisterExt.usrATPTEFMSourceRef, IsNull>>>>>,
                    Where<ATPTEFMReplenishmentDetail.replenishmentNbr, Equal<Current<ATPTEFMReplenishment.replenishmentNbr>>>,
                    Aggregate<
                        GroupBy<APPayment.docType,
                        GroupBy<APPayment.refNbr>>>>
                    .Select(this);
            }
            else
            {
                return PXSelectReadonly<
                    APPayment,
                    Where<APPayment.origModule, Equal<BatchModule.moduleEP>,
                        And<APPayment.origRefNbr, Equal<Current<ATPTEFMReplenishment.replenishmentNbr>>>>>
                    .Select(this);
            }
        }
        #endregion

        #region Actions
        public PXInitializeState<ATPTEFMReplenishment> InitializeState;

        public PXAction<ATPTEFMReplenishment> showSubmitReceipt;
        [PXUIField(DisplayName = ATPTEFMMessages.ShowSubmitReceipt)]
        [PXButton(Tooltip = "Add Receipts")]
        protected virtual IEnumerable ShowSubmitReceipt(PXAdapter adapter)
        {
            if (ReceiptsForSubmit.AskExt(true) == WebDialogResult.OK)
            {
                return SubmitReceipt(adapter);
            }
            ReceiptsForSubmit.Cache.Clear();
            return adapter.Get();
        }

        /// <remarks>
        /// 2025-06-24 : No value is populated in Replenishment > Taxes Tab > Tax Type column CASEID: 011748 {JLG} <br/>  
        /// 2025-10-01 : 013750 - LFC - Line Attachments from Fund Transaction to Replenishment. {JLTG}
        /// </remarks>
        public PXAction<ATPTEFMReplenishment> submitReceipt;
        [PXUIField(DisplayName = ATPTEFMMessages.SubmitReceipt)]
        [PXButton()]
        protected virtual IEnumerable SubmitReceipt(PXAdapter adapter)
        {

            ATPTEFMReplenishment row = Replenishments.Current;
            if (row != null)
            {
                decimal? claimAmount = row.ClaimAmount;
                decimal? wTaxAmount = row.WithholdingTaxAmount;
                decimal? vatAmount = row.VatAmount;

                List<ATPTEFMReplenishmentTaxDetail> lsttaxdets = new List<ATPTEFMReplenishmentTaxDetail>();

                foreach (EPExpenseClaimDetails item in ReceiptsForSubmit.Select().RowCast<EPExpenseClaimDetails>().Where(w => w.Selected == true).ToList())
                {
                    if (item.Selected == true)
                    {
                        item.Selected = false;
                        ATPTEFMReplenishmentDetail detail = ReplenishmentDetails.Insert();

                        detail.ReplenishmentNbr = Replenishments.Current.ReplenishmentNbr;
                        detail.ExpenseReceiptID = item.ClaimDetailID;
                        detail.ExpenseReceiptNbr = item.ClaimDetailCD;
                        detail.CuryTaxTotal = item.CuryTaxTotal;

                        claimAmount += item.CuryExtCost; 
                        PXNoteAttribute.CopyNoteAndFiles(ReceiptsForSubmit.Cache, item, ReplenishmentDetails.Cache, detail);

                        ReplenishmentDetails.Update(detail);

                        PXResultset<EPTaxTran> taxes = PXSelectJoinGroupBy<
                            EPTaxTran,
                            InnerJoin<Tax,
                                On<Tax.taxID, Equal<EPTaxTran.taxID>>>,
                            Where<EPTaxTran.claimDetailID, Equal<Required<EPExpenseClaimDetails.claimDetailID>>>,
                            Aggregate<
                                GroupBy<Tax.taxID,
                                    Sum<EPTaxTran.curyTaxAmt,
                                    Sum<EPTaxTran.curyTaxableAmt>>>>>
                            .Select(this, item.ClaimDetailID);

                        foreach (PXResult<EPTaxTran, Tax> t in taxes)
                        {
                            EPTaxTran ePTaxTran = t;
                            Tax tax = t;

                            ATPTEFMReplenishmentTaxDetail detExist = PXSelect<
                                ATPTEFMReplenishmentTaxDetail,
                                Where<ATPTEFMReplenishmentTaxDetail.replenishmentNbr, Equal<Required<ATPTEFMReplenishmentTaxDetail.replenishmentNbr>>,
                                    And<ATPTEFMReplenishmentTaxDetail.taxID, Equal<Required<ATPTEFMReplenishmentTaxDetail.taxID>>>>>
                                .Select(this, Replenishments.Current.ReplenishmentNbr, ePTaxTran.TaxID);

                            if (detExist != null)
                            {
                                detExist.TaxableAmt += ePTaxTran.CuryTaxableAmt;
                                detExist.TaxAmt += ePTaxTran.CuryTaxAmt;
                                detExist.TaxType = tax.TaxType;
                                Taxes.Update(detExist);
                            }
                            else
                            {
                                ATPTEFMReplenishmentTaxDetail repTaxDetails = Taxes.Insert();
                                repTaxDetails.ReplenishmentNbr = Replenishments.Current.ReplenishmentNbr;
                                repTaxDetails.TaxID = ePTaxTran.TaxID;
                                repTaxDetails.TaxRate = ePTaxTran.TaxRate;
                                repTaxDetails.TaxableAmt = ePTaxTran.CuryTaxableAmt;
                                repTaxDetails.TaxAmt = ePTaxTran.CuryTaxAmt;
                                repTaxDetails.TaxType = tax.TaxType;
                                lsttaxdets.Add(repTaxDetails);
                                Taxes.Update(repTaxDetails);
                            }
                        }
                    }
                }
            }
            ReceiptsForSubmit.Cache.Clear();
            return adapter.Get();
        }

        public PXAction<ATPTEFMReplenishment> cancelSubmitReceipt;
        [PXUIField(DisplayName = ATPTEFMMessages.CancelSubmitReceipt)]
        [PXButton()]
        protected virtual IEnumerable CancelSubmitReceipt(PXAdapter adapter)
        {
            ReceiptsForSubmit.Cache.Clear();
            return adapter.Get();
        }

        //public PXAction<ATPTEFMReplenishment> Action;
        //[PXButton]
        //[PXUIField(DisplayName = ATPTEFMMessages.Action, MapEnableRights = PXCacheRights.Select)]
        //protected virtual IEnumerable action(PXAdapter adapter,
        //         [PXInt][PXIntList(new int[] { 1, 2 }, new string[] { "Persist", "Update" })] int? actionID,
        //         [PXBool] bool refresh,
        //         [PXString] string actionName
        //     )
        //{
        //    List<ATPTEFMReplenishment> result = new List<ATPTEFMReplenishment>();
        //    if (actionName != null)
        //    {
        //        PXAction a = this.Actions[actionName];
        //        if (a != null)
        //            foreach (PXResult<ATPTEFMReplenishment> e in a.Press(adapter))
        //                result.Add(e);
        //    }
        //    else
        //        foreach (ATPTEFMReplenishment e in adapter.Get<ATPTEFMReplenishment>())
        //            result.Add(e);

        //    if (refresh)
        //    {
        //        foreach (ATPTEFMReplenishment MyView in result)
        //            Replenishments.Search<ATPTEFMReplenishment.replenishmentNbr>(MyView.ReplenishmentNbr);
        //    }
        //    if (actionID == 1) Save.Press();

        //    return result;
        //}
        //public PXAction<ATPTEFMReplenishment> Hold;
        //[PXUIField(DisplayName = ATPTEFMMessages.Hold, Visible = false)]
        //[PXButton(ImageKey = PX.Web.UI.Sprite.Main.DataEntryF)]
        //protected virtual IEnumerable hold(PXAdapter adapter)
        //{
        //    return adapter.Get();
        //}


        public PXAction<ATPTEFMReplenishment> Release;
        /// <remarks>
        /// 2025-01-17 : Replenishment Bill: Vendor field should be the Custodian and not the Payee of the fund. CASEID: 009623  {JLG} <br/>      
        /// 2025-01-28 : 009658 - [DLS] Error upon initial Release of Replenishment: Expense Account is not included in any account group <br/>
        /// 2025-06-20 : Add code to retrigger tax category implementation, due to code reimplementation for DFT ATC of Philtax. 012116 {RRS} <br/>
        /// 2025-08-01 : Update ER APLineNbr field after inserting APTran detail 012780 {RFS} <br/>
        /// 2025-08-18 : The Branch specified in the Fund Profile should be carried under the Financial Tab of the Replenishment Bill CASE:012847 {JLG} 
        /// 2025-08-18 : The Branch specified in the Expense Receipt should be carried under the Details Tab of the Replenishment Bill CASE:012847 {JLG}
        /// 2025-10-24 : Use cache to update expense receipt instead of direct graph initialization. 014138 {RRS} <br/>
        /// </remarks>
        [PXUIField(DisplayName = ATPTEFMMessages.Release)]
        [PXButton()]
        public IEnumerable release(PXAdapter adapter)
        {
            ATPTEFMHelper.StartLongOperation(this, adapter, delegate ()
            {
                using (PXTransactionScope ts = new PXTransactionScope())
                {
                    ATPTEFMReplenishment currentReplenishment = Replenishments.Current;
                    RepDetExpenseReceipts.Select(); //populate the view for updating Expense Receipts.
                    ATPTEFMFundTransaction fundTransaction = PXSelectJoin<
                        ATPTEFMFundTransaction,
                        InnerJoin<ATPTEFMFundTransactionReceiptDetail,
                            On<ATPTEFMFundTransactionReceiptDetail.fundTransactionRefNbr, Equal<ATPTEFMFundTransaction.refNbr>>,
                        InnerJoin<ATPTEFMReplenishmentDetail,
                            On<ATPTEFMReplenishmentDetail.expenseReceiptNbr, Equal<ATPTEFMFundTransactionReceiptDetail.expenseReceiptRefNbr>,
                            And<ATPTEFMReplenishmentDetail.replenishmentNbr, Equal<Required<ATPTEFMReplenishmentDetail.replenishmentNbr>>>>>>,
                        Where<ATPTEFMFundTransaction.status, Equal<ATPTEFMFundStatusAttribute.openValue>>>
                        .Select(this, currentReplenishment.ReplenishmentNbr)
                        .FirstOrDefault();
                    ATPTEFMFund fund = Fund.Select(currentReplenishment.FundID);

                    if (fund == null)
                        return;

                    if (fundTransaction != null)
                    {
                        throw new PXException(ATPTEFMMessages.ReplenishmentCannotBeRelease, fundTransaction.RefNbr);
                    }

                    var requireTaxTotal = ApSetup.Current.RequireControlTaxTotal ?? false;
                    var netGross = EnableFeatures.Current.NetGrossEntryMode ?? false;

                    if (currentReplenishment != null)
                    {
                        //ExpenseClaimDetailEntry erGraph = PXGraph.CreateInstance<ExpenseClaimDetailEntry>();

                        APInvoiceEntry invEntry = PXGraph.CreateInstance<APInvoiceEntry>();

                        var receipts = PXSelectJoin<
                            EPExpenseClaimDetails,
                            InnerJoin<ATPTEFMReplenishmentDetail,
                                On<ATPTEFMReplenishmentDetail.expenseReceiptNbr, Equal<EPExpenseClaimDetails.claimDetailCD>>>,
                            Where<ATPTEFMReplenishmentDetail.replenishmentNbr, Equal<Required<ATPTEFMReplenishmentDetail.replenishmentNbr>>,
                                And<ATPTEFMReplenishmentDetail.invoiceRefNbr, IsNull>>>
                            .Select(this, currentReplenishment.ReplenishmentNbr);

                        var receiptsGrouped = receipts.Select(
                            result => (EPExpenseClaimDetails)result).GroupBy(
                            item => Tuple.Create(
                                item.TaxZoneID,
                                item.TaxCalcMode
                            )).ToDictionary(x => x.Key, group => group.ToList());

                        int billCount = 1, receiptsCount = 1;

                        string fundType = fund.FundType == ATPTEFMFundTypeAttribute.PettyCashValue ? "PCF" : "RF";

                        foreach (var res in receiptsGrouped)
                        {
                            invEntry.Clear();

                            APInvoice inv = invEntry.Document.Insert(new APInvoice
                            {
                                OrigModule = "EP",
                                OrigRefNbr = currentReplenishment.ReplenishmentNbr,
                                BranchID = fund.BranchID,
                                DocType = APDocType.Invoice,
                                DocDesc = $"Fund Replenishment: {currentReplenishment.ReplenishmentNbr} {fundType} {fund.FundCD}",
                                Hold = Preferences.Current?.IsRequireApprovalReplenishment ?? false ? true : false,
                                PaymentsByLinesAllowed = false,
                                VendorID = fund.CustodianID,
                                InvoiceNbr = GetInvoiceNbr(),
                                TaxZoneID = res.Key.Item1,
                                TaxCalcMode = res.Key.Item2,
                            });

                            inv.CuryID = fund.CuryID;
                            inv.Status = Preferences.Current?.IsRequireApprovalReplenishment ?? false ? APDocStatus.Hold : APDocStatus.Balanced;
                            inv = DoAdditionalCreateApBillProcess(inv);
                            inv = invEntry.Document.Update(inv);

                            ATPTEFMAPRegisterExt invExt = inv.GetExtension<ATPTEFMAPRegisterExt>();
                            invExt.UsrATPTEFMIsFromReplenishment = true;

                            Decimal? totalAmount = 0;

                            PXResultset<ATPTEFMReplenishmentDetail, EPExpenseClaimDetails> details = PXSelectJoin<
                                ATPTEFMReplenishmentDetail,
                                InnerJoin<EPExpenseClaimDetails,
                                    On<EPExpenseClaimDetails.claimDetailCD, Equal<ATPTEFMReplenishmentDetail.expenseReceiptNbr>>>,
                                Where<EPExpenseClaimDetails.taxZoneID, Equal<Required<EPExpenseClaimDetails.taxZoneID>>,
                                    And<EPExpenseClaimDetails.taxCalcMode, Equal<Required<EPExpenseClaimDetails.taxCalcMode>>,
                                    And<ATPTEFMReplenishmentDetail.replenishmentNbr, Equal<Required<ATPTEFMReplenishmentDetail.replenishmentNbr>>,
                                    And<ATPTEFMReplenishmentDetail.invoiceRefNbr, IsNull>>>>>
                                .Select<PXResultset<ATPTEFMReplenishmentDetail, EPExpenseClaimDetails>>
                               (this, res.Key.Item1, res.Key.Item2, currentReplenishment.ReplenishmentNbr);

                            //Item 1 == TaxZone || Item 2 == TaxCalcMode

                            foreach (PXResult<ATPTEFMReplenishmentDetail, EPExpenseClaimDetails> ds in details)
                            {
                                ATPTEFMReplenishmentDetail d = (ATPTEFMReplenishmentDetail)ds;
                                EPExpenseClaimDetails er = (EPExpenseClaimDetails)ds;

                                APTran tranDoc = new APTran();

                                tranDoc.TranDesc = er.TranDesc;
                                tranDoc.BranchID = er.BranchID;
                                tranDoc.InventoryID = er.InventoryID;
                                tranDoc.Qty = er.Qty;
                                tranDoc.CuryUnitCost = er.CuryUnitCost;
                                invEntry.Transactions.SetValueExt<APTran.curyUnitCost>(tranDoc, er.CuryUnitCost);
                                tranDoc.UnitCost = er.CuryUnitCost;
                                tranDoc.CuryLineAmt = er.CuryUnitCost;
                                tranDoc.CuryTranAmt = ((decimal?)er.Qty * er.CuryUnitCost).RoundDecimal(2);
                                tranDoc.AccountID = er.ExpenseAccountID;
                                tranDoc.SubID = er.ExpenseSubID;
                                tranDoc.ProjectID = er.ContractID;
                                tranDoc.TaskID = er.TaskID;
                                tranDoc.CostCodeID = er.CostCodeID;

                                //--TODO
                                //PHILTAX//
                                Extensions.DAC.ATPTEFMAPTranExtension tranDocExt = tranDoc.GetExtension<Extensions.DAC.ATPTEFMAPTranExtension>();
                                CashFundManagement.Extensions.DAC.ATPTEFMEPExpenseClaimDetailsExt erExt = er.GetExtension<CashFundManagement.Extensions.DAC.ATPTEFMEPExpenseClaimDetailsExt>();

                                tranDocExt.UsrATPTEFMClaimDetailCD = er.ClaimDetailCD;
                                tranDoc = invEntry.Transactions.Insert(tranDoc);
                                totalAmount += ((decimal?)er.Qty * er.CuryUnitCost).RoundDecimal(2);

                                if (tranDoc.ProjectID == null)
                                {
                                    tranDoc.ProjectID = er.ContractID;
                                    tranDoc.TaskID = er.TaskID;
                                    tranDoc.CostCodeID = er.CostCodeID;

                                    invEntry.Transactions.Update(tranDoc);
                                }
                                bool reTriggerTax = tranDoc?.TaxCategoryID == er?.TaxCategoryID;
                                tranDoc.TaxCategoryID = er.TaxCategoryID;

                                inv.CuryDocBal = totalAmount;
                                inv.CuryOrigDocAmt = totalAmount;
                                inv.CuryLineTotal = totalAmount;
                                //inv.TaxZoneID = res.Key.Item1;

                                tranDoc = DoAdditionalRelease(tranDoc, er);
                                invEntry.Transactions.Update(tranDoc);

                                #region Populate Tax Details from ER
                                // This is added due to the reimplementation of the tax calculation in APInvoiceEntry from Philtax under ATPTAPInvoiceDefaultATCAttribute 012116
                                if (reTriggerTax)
                                {
                                    var currentTaxCategory = tranDoc.TaxCategoryID;
                                    invEntry.Transactions.Cache.SetValueExt<APTran.taxCategoryID>(tranDoc, null);
                                    invEntry.Transactions.Cache.Update(tranDoc);
                                    invEntry.Transactions.Cache.SetValueExt<APTran.taxCategoryID>(tranDoc, currentTaxCategory);
                                    invEntry.Transactions.Cache.Update(tranDoc);
                                }
                                #endregion

                                #region Update Expense receipt status to released
                                //Using Cache for updating expense receipt records.
                                er.Status = EPExpenseClaimDetailsStatus.ReleasedStatus;
                                er.APLineNbr = tranDoc.LineNbr;
                                RepDetExpenseReceipts.Update(er);
                                #endregion

                                if ((Preferences.Current?.IsRequireApprovalReplenishment ?? false) == false)
                                {
                                    APInvoiceEntry.APInvoiceEntryDocumentExtension invoiceBaseGraphExtension = invEntry.GetExtension<APInvoiceEntry.APInvoiceEntryDocumentExtension>();
                                    invoiceBaseGraphExtension.SuppressApproval();
                                }

                                inv = invEntry.Document.Update(inv);

                                if (details.Count == receiptsCount)
                                {
                                    if (requireTaxTotal && netGross)
                                    {
                                        inv.CuryTaxAmt = inv.CuryTaxTotal;
                                        invEntry.Document.Update(inv);
                                    }

                                    invEntry.Save.Press();
                                    PXLongOperation.WaitCompletion(invEntry.UID);
                                    foreach (PXResult<ATPTEFMReplenishmentDetail, EPExpenseClaimDetails> dss in details)
                                    {
                                        EPExpenseClaimDetails erInv = (EPExpenseClaimDetails)dss;
                                        ATPTEFMReplenishmentDetail repInv = (ATPTEFMReplenishmentDetail)dss;

                                        //Using Cache for updating expense receipt records.
                                        erInv.APRefNbr = inv.RefNbr;
                                        erInv.APDocType = inv.DocType;
                                        RepDetExpenseReceipts.Update(erInv);
                                        
                                        repInv.InvoiceRefNbr = inv.RefNbr;
                                        ReplenishmentDetails.Update(repInv);
                                    }
                                }
                                receiptsCount++;
                            }
                            currentReplenishment.InvoiceRefNbr = inv.RefNbr;
                            currentReplenishment.Step = ATPTEFMReplenishmentStepAttribute.ReleaseValue;
                            currentReplenishment.Status = ATPTEFMReplenishmentStatusAttribute.ClosedValue;
                            currentReplenishment.IsReleased = true;

                            currentReplenishment = OnBeforeUpdateReleaseReplenishment(currentReplenishment);
                            Replenishments.Update(currentReplenishment);
                            billCount++;
                            receiptsCount = 1;
                        }
                        //invEntry.Save.Press();
                        //PXLongOperation.WaitCompletion(invEntry.UID);
                        this.Save.Press();
                    };
                    ts.Complete();
                }
            });
            PXLongOperation.WaitCompletion(UID);
            APInvoiceDocument.View.RequestRefresh();

            return adapter.Get();
        }

        [PXButton(CommitChanges = true), PXUIField(DisplayName = "Hold")]
        public override IEnumerable putOnHold(PXAdapter adapter)
        {
            using (PXTransactionScope ts = new PXTransactionScope())
            {
                ATPTEFMReplenishment r = Replenishments.Current;
                if (r != null && r.Step == ATPTEFMReplenishmentStepAttribute.SubmitReceiptValue)
                {
                    foreach (ATPTEFMReplenishmentDetail d in ReplenishmentDetails.Select())
                    {
                        #region Update Fund Transaction Receipt Detail
                        FundTransactionReceiptDetail.Current = FundTransactionReceiptDetail.Select(d.ExpenseReceiptNbr);
                        if (FundTransactionReceiptDetail.Current != null)
                        {
                            FundTransactionReceiptDetail.Current.ReplenishmentRefNbr = null;
                            FundTransactionReceiptDetail.UpdateCurrent();
                        }
                        #endregion

                        #region Update Fund Reclassification Receipt
                        FundTransactionReclassificationReceiptDetail.Current = FundTransactionReclassificationReceiptDetail.Select(d.ExpenseReceiptNbr);
                        if (FundTransactionReclassificationReceiptDetail.Current != null)
                        {
                            FundTransactionReclassificationReceiptDetail.Current.ReplenishmentRefNbr = null;
                            FundTransactionReclassificationReceiptDetail.UpdateCurrent();
                        }
                        #endregion
                    }
                    r.Step = ATPTEFMReplenishmentStepAttribute.DefaultValue;
                    Replenishments.Update(r);
                    this.Save.Press();
                }

                ts.Complete();
            }
            return adapter.Get();
        }

        public PXAction<ATPTEFMReplenishment> Submit;
        [PXUIField(DisplayName = ATPTEFMMessages.Submit, Enabled = false)]
        [PXButton()]
        public IEnumerable submit(PXAdapter adapter)
        {
            ATPTEFMHelper.StartLongOperation(this, adapter, delegate ()
            {
                using (PXTransactionScope ts = new PXTransactionScope())
                {
                    Save.Press();
                    ATPTEFMReplenishment r = Replenishments.Current;

                    if (r != null)
                    {
                        ATPTEFMFundTransactionEntry graph = PXGraph.CreateInstance<ATPTEFMFundTransactionEntry>();

                        foreach (ATPTEFMReplenishmentDetail d in ReplenishmentDetails.Select())
                        {
                            #region Update Fund Transaction Receipt Detail
                            FundTransactionReceiptDetail.Current = FundTransactionReceiptDetail.Select(d.ExpenseReceiptNbr);
                            if (FundTransactionReceiptDetail.Current != null)
                            {
                                FundTransactionReceiptDetail.Current.ReplenishmentRefNbr = r.ReplenishmentNbr;
                                FundTransactionReceiptDetail.UpdateCurrent();
                            }
                            #endregion

                            #region Update Fund Reclassification Receipt
                            FundTransactionReclassificationReceiptDetail.Current = FundTransactionReclassificationReceiptDetail.Select(d.ExpenseReceiptNbr);
                            if (FundTransactionReclassificationReceiptDetail.Current != null)
                            {
                                FundTransactionReclassificationReceiptDetail.Current.ReplenishmentRefNbr = r.ReplenishmentNbr;
                                FundTransactionReclassificationReceiptDetail.UpdateCurrent();
                            }
                            #endregion
                        }
                        r.Step = ATPTEFMReplenishmentStepAttribute.SubmitReceiptValue;
                        Replenishments.Update(r);
                        this.Save.Press();
                    }
                    ts.Complete();

                }
            });
            return adapter.Get();
        }

        public PXAction<ATPTEFMReplenishment> FundReplenishmentForm;
        [PXButton(Category = "Reports")]
        [PXUIField(DisplayName = ATPTEFMMessages.FundReplenishmentForm)]
        public IEnumerable fundReplenishmentForm(PXAdapter adapter)
        {
            foreach (ATPTEFMReplenishment replenishments in adapter.Get<ATPTEFMReplenishment>())
            {
                ATPTEFMReplenishmentEntry graph = PXGraph.CreateInstance<ATPTEFMReplenishmentEntry>();

                Dictionary<string, string> parameters = new Dictionary<string, string>
                {
                    ["ReplenishmentNbr"] = replenishments.ReplenishmentNbr
                };

                var report = new PXReportRequiredException(parameters, "ATPT6419", "Fund Replenishment Form");

                throw new PXRedirectWithReportException(graph, report, "Preview");
            }
            return adapter.Get();
        }

        public PXAction<ATPTEFMReplenishment> openTransaction;
        [PXButton(Tooltip = "Open Transaction", CommitChanges = true)]
        [PXUIField(DisplayName = ATPTEFMMessages.OpenTransaction, Visible = false)]
        public virtual void OpenTransaction()
        {
            APInvoiceEntry graph = PXGraph.CreateInstance<APInvoiceEntry>();

            graph.Document.Current = APDocuments.Current;

            throw new PXRedirectRequiredException(graph, true, "AP") { Mode = PXBaseRedirectException.WindowMode.NewWindow };
        }

        public PXAction<ATPTEFMReplenishment> viewCheck;
        [PXUIField(DisplayName = ATPTEFMMessages.ViewCheck , Visible = false)]
        [PXButton(Tooltip = "View Check", CommitChanges = true)]
        public virtual void ViewCheck()
        {
            APPaymentEntry graph = PXGraph.CreateInstance<APPaymentEntry>();

            graph.Document.Current = APPaymentDocument.Current;

            throw new PXRedirectRequiredException(graph, true, "AP") { Mode = PXBaseRedirectException.WindowMode.NewWindow };
        }

        public PXAction<ATPTEFMReplenishment> OpenReceipts;
        [PXButton(Tooltip = "Open Receipt", CommitChanges = true)]
        [PXUIField(DisplayName = "Open Receipt", Visible = false)]
        public virtual void openReceipts()
        {
            ExpenseClaimDetailEntry graph = PXGraph.CreateInstance<ExpenseClaimDetailEntry>();
            graph.ClaimDetails.Current = graph.ClaimDetails.Search<EPExpenseClaimDetails.claimDetailCD>(ReplenishmentDetails.Current.ExpenseReceiptNbr);
            throw new PXRedirectRequiredException(graph, true, "Expense Receipt") { Mode = PXBaseRedirectException.WindowMode.NewWindow };
        }

        public PXAction<ATPTEFMReplenishment> ReplenishmentCancel;
        [PXButton()]
        [PXUIField(DisplayName = ATPTEFMMessages.CACancel)]
        protected virtual IEnumerable replenishmentCancel(PXAdapter adapter)
        {

            ATPTEFMReplenishment replenishment = Replenishments.Current;
            if (replenishment != null)
            {
                ValidateAPBillStatus(replenishment.ReplenishmentNbr);

                foreach (ATPTEFMReplenishmentDetail item in ReplenishmentDetails.Select())
                {
                    #region Subject for Removal for optimization
                    /* ATPTEFMFundTransactionEntry graph = PXGraph.CreateInstance<ATPTEFMFundTransactionEntry>();

                         ATPTEFMFundTransactionReceiptDetail detail = PXSelect<
                         ATPTEFMFundTransactionReceiptDetail,
                        Where<ATPTEFMFundTransactionReceiptDetail.expenseReceiptRefNbr, Equal<Required<ATPTEFMFundTransactionReceiptDetail.expenseReceiptRefNbr>>>>
                        .Select(this, item.ExpenseReceiptNbr);
                        detail.ReplenishmentRefNbr = null;
                        graph.FundTransactionReceiptLines.Update(detail);
                        graph.Save.Press();*/
                    #endregion

                    #region Remove Replenishment Nbr for in Fund transaction Receipt details
                    //Normal Receipts
                    FundTransactionReceiptDetail.Current = FundTransactionReceiptDetail.Select(item.ExpenseReceiptNbr);

                    if (FundTransactionReceiptDetail.Current != null)
                    {
                        FundTransactionReceiptDetail.Current.ReplenishmentRefNbr = null;
                        FundTransactionReceiptDetail.UpdateCurrent();
                    }

                    //Reclassified receipts
                    FundTransactionReclassificationReceiptDetail.Current = FundTransactionReclassificationReceiptDetail.Select(item.ExpenseReceiptNbr);
                    if (FundTransactionReclassificationReceiptDetail.Current != null)
                    {
                        FundTransactionReclassificationReceiptDetail.Current.ReplenishmentRefNbr = null;
                        FundTransactionReclassificationReceiptDetail.UpdateCurrent();
                    }
                    #endregion

                    #region Remove Replenishment Number in Transaction History
                    TransactionHistoryView.Current = TransactionHistoryView.Select(item.ExpenseReceiptNbr);
                    if (TransactionHistoryView.Current != null)
                    {
                        TransactionHistoryView.Current.ReplenishmentRefNbr = null;
                        TransactionHistoryView.UpdateCurrent();
                    }
                    #endregion

                    #region Remove approvers
                    foreach (EPApproval approval in Approval.Select())
                    {
                        Approval.Delete(approval);
                    }
                    #endregion

                }

                #region Update Fund Summary Balances
                Fund.Current = Fund.Select(replenishment.FundID);
                if (Fund.Current != null)
                {
                    decimal? totalAmount = (replenishment.ClaimAmount - replenishment.WithholdingTaxAmount);
                    Fund.Current.CuryOnReplenishmentAmt -= totalAmount;
                    Fund.Current.CuryLiquidatedAmt += totalAmount;
                    Fund.UpdateCurrent();
                }
                #endregion

                #region Update Current Record
                replenishment.Status = ATPTEFMReplenishmentStatusAttribute.CancelledValue;
                replenishment.Step = ATPTEFMFundTransactionStepAttribute.CancelledValue;
                Replenishments.Update(replenishment);
                this.Save.Press();
                #endregion
            }
            return adapter.Get();
        }

        public PXAction<ATPTEFMReplenishment> ViewTaxes;
        [PXUIField(DisplayName = "View Taxes", Visible = false)]
        [PXButton]
        protected virtual IEnumerable viewTaxes(PXAdapter adapter)
        {
            ATPTEFMReplenishmentDetail repDet = ReplenishmentDetails.Current;

            if (repDet == null)
            {
                return adapter.Get();
            }

            EPExpenseClaimDetails ecDet = EPExpenseClaimDetails.PK.Find(this, repDet.ExpenseReceiptNbr);

            if (ecDet != null)
                TaxOverrideExpenseClaimDetails.Current = ecDet;

            Tax_Rows.AskExt(true);
            return adapter.Get();
        }

        /// <remarks>
        /// 2024-10-29 : The tax amount doesn't update after removing all taxes. CASEID: 008333 {JLG} <br/>             
        /// </remarks>
        public PXAction<ATPTEFMReplenishment> CommitTaxes;
        [PXUIField(DisplayName = "Ok")]
        [PXButton]
        protected virtual IEnumerable commitTaxes(PXAdapter adapter)
        {
            TaxOverrideExpenseClaimDetails.Update(TaxOverrideExpenseClaimDetails.Current);

            ATPTEFMReplenishment rep = Replenishments.Current;

            foreach (ATPTEFMReplenishmentTaxDetail result in Taxes.Select())
            {
                Taxes.Delete(result);
            }
            rep.WithholdingTaxAmount = 0;
            rep.VatAmount = 0;
            Replenishments.Update(rep);


            foreach (ATPTEFMReplenishmentDetail result in ReplenishmentDetails.Select())
            {
                result.CuryTaxTotal = 0;
                foreach (EPTaxTran taxTran in PXSelect<
                    EPTaxTran,
                    Where<EPTaxTran.claimDetailID, Equal<Required<EPTaxTran.claimDetailID>>>>
                    .Select(this, result.ExpenseReceiptID))
                {
                    if (taxTran.TaxID != null)
                    {
                        #region Update Replenishment Tax Detail
                        ATPTEFMReplenishmentTaxDetail tax = Taxes.Select().Where(x => ((ATPTEFMReplenishmentTaxDetail)x).TaxID == taxTran.TaxID).FirstOrDefault();
                        if (tax != null)
                        {
                            tax.TaxAmt += taxTran.CuryTaxAmt;
                            Taxes.Update(tax);
                        }
                        else
                        {
                            ATPTEFMReplenishmentTaxDetail newTax = new ATPTEFMReplenishmentTaxDetail();
                            newTax.ReplenishmentNbr = Replenishments.Current.ReplenishmentNbr;
                            newTax.TaxID = taxTran.TaxID;
                            newTax.TaxRate = taxTran.TaxRate;
                            newTax.TaxableAmt = taxTran.CuryTaxableAmt;
                            newTax.TaxAmt = taxTran.CuryTaxAmt;
                            Taxes.Cache.Insert(newTax);
                        }
                        #endregion
                        #region Update Replenishment Amounts
                        Tax taxes = Tax.PK.Find(this, taxTran.TaxID);
                        if (taxes != null)
                        {
                            if (taxes.TaxType == CSTaxType.Withholding)
                                rep.WithholdingTaxAmount += taxTran.CuryTaxAmt;
                            else if (taxes.TaxType == CSTaxType.VAT)
                                rep.VatAmount += taxTran.CuryTaxAmt;

                            Replenishments.Update(rep);
                        }
                        #endregion

                        #region Update Replenishment Detail CuryTaxTotal Field
                        result.CuryTaxTotal += taxTran.CuryTaxAmt;
                        if (ReplenishmentDetails.Cache.GetStatus(result) != PXEntryStatus.Inserted)
                        {
                            ReplenishmentDetails.Update(result);
                        }
                        #endregion
                    }
                }

                if (!Tax_Rows.Any() && ReplenishmentDetails.Cache.GetStatus(result) != PXEntryStatus.Inserted)
                    ReplenishmentDetails.Update(result);
            }

            return adapter.Get();
        }
        #endregion

        #region Events
        #region EPTaxTran Events
        protected virtual void EPTaxTran_CuryTaxAmt_FieldVerifying(PXCache cache, PXFieldVerifyingEventArgs e)
        {
            EPTaxTran row = (EPTaxTran)e.Row;
            EPExpenseClaimDetails doc = ExpenseClaimDetails.Current;
            if (e.NewValue != null)
            {
                decimal newValue = (decimal)e.NewValue;
                if (row != null && doc != null)
                {
                    if (newValue > 0 && doc.CuryExtCost < 0 || newValue < 0 && doc.CuryExtCost > 0)
                    {
                        cache.RaiseExceptionHandling<EPTaxTran.curyTaxAmt>(row,
                            row.CuryTaxAmt,
                            ATPTEFMHelper.GetPropertyException(row, PX.Objects.EP.Messages.TaxSign, PXErrorLevel.Error));
                        e.NewValue = row.CuryTaxAmt;
                    }
                }
            }
        }
        protected virtual void EPTaxTran_CuryTaxableAmt_FieldVerifying(PXCache cache, PXFieldVerifyingEventArgs e)
        {
            EPTaxTran row = (EPTaxTran)e.Row;
            EPExpenseClaimDetails doc = ExpenseClaimDetails.Current;
            if (e.NewValue != null)
            {
                decimal newValue = (decimal)e.NewValue;
                if (row != null && doc != null)
                {
                    if (newValue > 0 && doc.CuryExtCost < 0 || newValue < 0 && doc.CuryExtCost > 0)
                    {
                        cache.RaiseExceptionHandling<EPTaxTran.curyTaxableAmt>(row,
                            row.CuryTaxableAmt,
                            ATPTEFMHelper.GetPropertyException(row, PX.Objects.EP.Messages.TaxableSign, PXErrorLevel.Error));
                        e.NewValue = row.CuryTaxableAmt;
                    }
                }
            }
        }
        protected virtual void EPTaxTran_RowSelected(PXCache sender, PXRowSelectedEventArgs e)
        {
            if (e.Row == null)
                return;

            PXUIFieldAttribute.SetEnabled<EPTaxTran.taxID>(sender, e.Row, sender.GetStatus(e.Row) == PXEntryStatus.Inserted);
        }
        protected virtual void _(Events.RowInserting<EPTaxTran> e)
        {
            EPTaxTran row = e.Row;

            if (row != null)
            {
                if (row.TaxID.IsNullOrEmpty())
                {
                    e.Cancel = true;
                }
            }
        }
        protected virtual void _(Events.RowDeleting<EPTaxTran> e)
        {
            EPTaxTran row = e.Row;
            EPTaxTran compareRow = Tax_Rows.Current;

            if (!(row != null && e.Cache.GetStatus(row) == PXEntryStatus.Deleted && row == compareRow))
            {
                e.Cancel = true;
            }
        }
        #endregion
        /// <remarks>
        /// 2025-09-05 : Set BranchID when FundID is updated {RFS} <br/> Note --> DAC version does not work
        /// </remarks>
        /// <param name="e"></param>
        protected virtual void _(Events.FieldUpdated<ATPTEFMReplenishment, ATPTEFMReplenishment.fundID> e)
        {
            ATPTEFMReplenishment row = e.Row;
            if (row == null) return;

            ATPTEFMFund fund = Fund.Select(row.FundID);
            if (fund != null)
            {
                row.BranchID = fund.BranchID;
            }
        }
        /// <remarks>
        /// 2025-02-10 : Removing of unused fields {JLTG} <br/>
        /// </remarks>
        protected virtual void ATPTEFMReplenishment_RowSelected(PXCache sender, PXRowSelectedEventArgs e)
        {
            ATPTEFMReplenishment replenishment = (ATPTEFMReplenishment)e.Row;

            if (replenishment == null) return;

            if (Preferences.Current.ReplenishmentRequestApproval != true && replenishment.Approved == true)
                PXUIFieldAttribute.SetVisible<ATPTEFMReplenishment.approved>(sender, replenishment, false);
            else if (Preferences.Current.ReplenishmentRequestApproval != true && replenishment.Approved == false)
                PXUIFieldAttribute.SetVisible<ATPTEFMReplenishment.approved>(sender, replenishment, false);
            else
            {
                PXUIFieldAttribute.SetVisible<ATPTEFMReplenishment.approved>(sender, replenishment, true);
            }

            //Set Defaults 
            showSubmitReceipt.SetEnabled(true);

            Release.SetEnabled(false);

            //Approved Behavior

            ReplenishmentDetails.AllowDelete = (!replenishment.Approved ?? false);
            ReplenishmentDetails.AllowUpdate = false;

            showSubmitReceipt.SetEnabled(!replenishment.Approved ?? false);

            if (replenishment.Approved ?? false)
            {
                //Release Button Behavior
                if (replenishment.Step == ATPTEFMReplenishmentStepAttribute.SubmitReceiptValue)
                {
                    Release.SetEnabled(true);
                    Replenishments.AllowUpdate = false;
                }
            }
            PXUIFieldAttribute.SetEnabled(APInvoiceDocument.Cache, null, false);
            Taxes.AllowInsert = false;
            Taxes.Cache.AllowUpdate = false;
            Taxes.AllowDelete = false;

            Tax_Rows.Cache.SetAllEditPermissions(replenishment.Hold ?? false);
        }

        //TODO : Transfer logic to field updated event
        protected virtual void ATPTEFMSetup_ReplenishmentRequestApproval_FieldUpdating(PXCache sender, PXFieldUpdatingEventArgs e)
        {
            PXCache cache = this.Caches[typeof(ATPTEFMReplenishmentSetupApproval)];
            PXResultset<ATPTEFMReplenishmentSetupApproval> setups = PXSelect<ATPTEFMReplenishmentSetupApproval>.Select(sender.Graph, null);
            foreach (ATPTEFMReplenishmentSetupApproval setup in setups)
            {
                setup.IsActive = (bool?)e.NewValue;
                cache.Update(setup);

            }

        }
        
        protected virtual void _(Events.RowUpdated<ATPTEFMReplenishmentDetail> e)
        {
            ATPTEFMReplenishmentDetail repDetails = (ATPTEFMReplenishmentDetail)e.Row;
            ATPTEFMReplenishment doc = Replenishments.Current;

            if (repDetails is null) return;

            bool isInserted = this.ReplenishmentDetails.Cache.GetStatus(this.ReplenishmentDetails.Current) == PXEntryStatus.Inserted;

            if (isInserted)
            {
                (decimal? whtAmount, decimal? vatAmount) = CaclulateTaxes(repDetails.ExpenseReceiptNbr);
                doc.ClaimAmount += CalcClaimTotal(repDetails.ExpenseReceiptNbr);
                doc.WithholdingTaxAmount += whtAmount;
                doc.VatAmount += vatAmount;

                #region Add On Replenishment Amount
                UpdateOnReplenishmentSummaryBalances(repDetails.ExpenseReceiptNbr, true);
                #endregion
            }
        }
        protected virtual void _(Events.RowPersisted<ATPTEFMReplenishment> e)
        {
            ATPTEFMReplenishment replenish = e.Row;
        }
        protected virtual void _(Events.RowPersisting<ATPTEFMReplenishment> e)
        {
            ATPTEFMReplenishment replenish = e.Row;

            if (replenish != null)
            {
                var finID = ATPTEFMHelper.GetFinPeriod(this, replenish.Date);

                MasterFinPeriod period = PXSelect<
                    MasterFinPeriod,
                    Where<MasterFinPeriod.finPeriodID, Equal<Required<MasterFinPeriod.finPeriodID>>>>
                    .Select(this, finID);

                #region Financial Period Validation
                bool isMultipleCalendarSupport = EnableFeatures?.Current?.MultipleCalendarsSupport ?? false;

                DateTime periodDate = (DateTime)period.StartDate;

                if (isMultipleCalendarSupport == false && period.Status == FinPeriod.status.Inactive)
                {
                    string finPeriodErrMsg = $"The {periodDate.ToString("MM-yyyy")} financial period is inactive.";

                    Replenishments.Cache.RaiseExceptionHandling<ATPTEFMReplenishment.date>(replenish,
                            replenish.Date,
                            ATPTEFMHelper.GetPropertyException(replenish, finPeriodErrMsg, PXErrorLevel.Error));
                }
                #endregion
            }
        }
        
        private List<ATPTEFMReplenishment> _deletedReplenishment = new List<ATPTEFMReplenishment>();
        protected virtual void _(Events.RowDeleting<ATPTEFMReplenishment> e)
        {
            ATPTEFMReplenishment row = e.Row;

            if (row is null) return;

            if (ReplenishmentDetails.Current != null)
            {
                throw new PXException(ATPTEFMMessages.CouldNotDeleteTheReplenishmentExpenseReceiptsAlreadyAdded);
            }
            _deletedReplenishment.Add(row);
        }
        protected virtual void _(Events.RowDeleted<ATPTEFMReplenishmentDetail> e)
        {
            ATPTEFMReplenishmentDetail repDetails = (ATPTEFMReplenishmentDetail)e.Row;
            ATPTEFMReplenishment doc = Replenishments.Current;

            if (repDetails is null) return;
            (decimal? whtAmount, decimal? vatAmount) = CaclulateTaxes(repDetails.ExpenseReceiptNbr);
            doc.ClaimAmount -= CalcClaimTotal(repDetails.ExpenseReceiptNbr);
            doc.WithholdingTaxAmount -= whtAmount;
            doc.VatAmount -= vatAmount;
            RemoveTaxes(repDetails.ExpenseReceiptNbr);

            #region Remove On Replenishment Amount
            UpdateOnReplenishmentSummaryBalances(repDetails.ExpenseReceiptNbr, false);
            #endregion
        }
        protected virtual void _(Events.RowDeleting<ATPTEFMReplenishmentDetail> e)
        {
            ATPTEFMReplenishmentDetail row = e.Row;
            if (row is null) return;

            if (!string.IsNullOrEmpty(row.InvoiceRefNbr))
            {
                this.ReplenishmentDetails?.Cache?.RaiseExceptionHandling<ATPTEFMReplenishmentDetail.expenseReceiptNbr>(row, row?.ExpenseReceiptNbr,
                    ATPTEFMHelper.GetPropertyException(row, Messages.ATPTEFMMessages.ReceiptCannotBeDeletedLinkedBill, PXErrorLevel.Error));
                throw new Exception(Messages.ATPTEFMMessages.ReceiptCannotBeDeletedLinkedBill);
            }
        }

        #endregion

        #region EPApproval Cache Attached
        [PXDBDate()]
        [PXDefault(typeof(ATPTEFMReplenishment.date), PersistingCheck = PXPersistingCheck.Nothing)]
        protected virtual void EPApproval_DocDate_CacheAttached(PXCache sender)
        {
        }

        [PXDBInt()]
        [PXDefault(typeof(ATPTEFMReplenishment.payeeID), PersistingCheck = PXPersistingCheck.Nothing)]
        protected virtual void EPApproval_BAccountID_CacheAttached(PXCache sender)
        {
        }

        [PXDBString(60, IsUnicode = true)]
        [PXDefault(typeof(ATPTEFMReplenishment.descr), PersistingCheck = PXPersistingCheck.Nothing)]
        protected virtual void EPApproval_Descr_CacheAttached(PXCache sender)
        {
        }

        #endregion

        #region Methods

        private ATPTEFMFundTransactionHistoryView CreateFundTransactionHistory(ATPTEFMReplenishment curDoc)
        {
            Fund.Current = Fund.Select(curDoc.FundID);

            if (Fund.Current != null)
            {
                if (Fund.Current.FundAmt == null || Fund.Current.CuryFundAmt == decimal.Zero)
                {
                    throw new PXException(Messages.ATPTEFMMessages.ImportFundFirst);
                }

                ATPTEFMFundTransactionHistoryView transactionHistoryDetails = PXSelect<
                    ATPTEFMFundTransactionHistoryView,
                    Where<ATPTEFMFundTransactionHistoryView.fundRefNbr, Equal<Required<ATPTEFMFundTransactionHistoryView.fundRefNbr>>>,
                    OrderBy<
                        Asc<ATPTEFMFundTransactionHistoryView.sortNbr>>>
                    .Select(this, curDoc.FundID)
                    .LastOrDefault();

                if (transactionHistoryDetails != null)
                {
                    string transactionNbrSorting = string.Empty;

                    switch (transactionHistoryDetails.Source)
                    {
                        case ATPTEFMTransactionHistoryView.source.FundTransaction:
                            transactionNbrSorting = $"FT-{transactionHistoryDetails.RefNbr}";
                            break;
                        case ATPTEFMTransactionHistoryView.source.ExpenseReceipt:
                            transactionNbrSorting = transactionHistoryDetails.FundTransactionSortNbr;
                            break;
                        case ATPTEFMTransactionHistoryView.source.Replenishment:
                            transactionNbrSorting = transactionHistoryDetails.SortNbr;
                            break;
                        case ATPTEFMTransactionHistoryView.source.MonthEnd:
                            break;
                        default:
                            break;
                    }

                    return new ATPTEFMFundTransactionHistoryView
                    {
                        FundRefNbr = curDoc.FundID,
                        TransactionType = ATPTEFMTransactionHistoryView.transactionType.Replenishment,
                        OrderDate = curDoc.Date,
                        RefNbr = curDoc.ReplenishmentNbr,
                        FundBranchID = curDoc.BranchID,
                        FundType = curDoc.FundType,
                        TransactionDate = curDoc.Date,
                        CuryFundTransactionDocumentAmt = curDoc.ClaimAmount,
                        Status = curDoc.Status,
                        Source = ATPTEFMTransactionHistoryView.source.Replenishment,
                        HasReplenishemtCheckNbr = false,
                        CuryBalanceAmt = Fund.Current.CuryBalanceAmt,
                        SortNbr = $"{transactionNbrSorting}-R{curDoc.ReplenishmentNbr}"
                    };
                }
            }

            return null;
        }

        protected string GetInvoiceNbr()
        {
            ATPTEFMReplenishment replenishment = Replenishments.Current;

            return IsRaiseErrorDuplicateVendorRef() ? IncrementVendorRefNbr() : replenishment.ReplenishmentNbr;
        }
        protected string IncrementVendorRefNbr()
        {
            ATPTEFMReplenishment replenishment = Replenishments.Current;

            APInvoice lastBill = PXSelectJoin<APInvoice, InnerJoin<APRegister, On<APRegister.refNbr, Equal<APInvoice.refNbr>,
                And<APRegister.docType, Equal<APInvoice.docType>>>>,
                Where<APRegister.origRefNbr, Equal<Required<APRegister.origRefNbr>>>>.Select(this, replenishment.ReplenishmentNbr).LastOrDefault();

            if (lastBill == null) return $"{replenishment.ReplenishmentNbr}-1";

            string result = lastBill.InvoiceNbr.Split('-').LastOrDefault();

            if (int.TryParse(result, out int numericValue))
                numericValue += 1;
            else
                return replenishment.ReplenishmentNbr;

            return $"{replenishment.ReplenishmentNbr}-{numericValue}";
        }
        protected bool IsRaiseErrorDuplicateVendorRef() => ApSetup.Current.RaiseErrorOnDoubleInvoiceNbr ?? false;

        public virtual APTran DoAdditionalRelease(APTran row, EPExpenseClaimDetails er) { return row; }
        public virtual APInvoice DoAdditionalCreateApBillProcess(APInvoice row) { return row; }
        /// <remarks>
        /// 2025-03-26 : Approval amount should use extCost instead of unitCost - 010798 - RFS <br/>
        /// 2025-04-04 : To consider multiple approvers - 010798 - RFS <br/>
        /// </remarks>
        private void UpdateApprovalAmount()
        {
            ATPTEFMReplenishment r = (ATPTEFMReplenishment)Replenishments.Current;

            if (r != null)
            {
                decimal? amount = ReplenishmentDetails.Select().RowCast<EPExpenseClaimDetails>().Sum(s => s.CuryExtCost);

                foreach (EPApproval line in Approval.Select())
                {
                    line.CuryTotalAmount = amount;
                    Approval.Update(line);
                }
            }

        }

        (decimal? whtAmount, decimal? vatAmount) CaclulateTaxes(string refNbr)
        {
            decimal? totalWhtAmt = 0M, totalVatAmt = 0M;

            EPExpenseClaimDetails ecDetails = PXSelect<
                EPExpenseClaimDetails,
                Where<EPExpenseClaimDetails.claimDetailCD, Equal<Required<EPExpenseClaimDetails.claimDetailCD>>>>
                .Select(this, refNbr.Trim());

            if (ecDetails is null) return (0M, 0M);

            PXResultset<EPTaxTran> taxes = PXSelectJoinGroupBy<
                EPTaxTran,
                InnerJoin<Tax,
                    On<Tax.taxID, Equal<EPTaxTran.taxID>>>,
                Where<EPTaxTran.claimDetailID, Equal<Required<EPExpenseClaimDetails.claimDetailID>>>,
                Aggregate<
                    GroupBy<Tax.taxID,
                        Sum<EPTaxTran.curyTaxAmt,
                        Sum<EPTaxTran.curyTaxableAmt>>>>>
                .Select(this, ecDetails.ClaimDetailID);

            foreach (PXResult<EPTaxTran, Tax> t in taxes)
            {
                EPTaxTran epTax = t;
                Tax tax = t;

                if (tax.TaxType == CSTaxType.VAT)
                    totalVatAmt += epTax.CuryTaxAmt;

                if (tax.TaxType == CSTaxType.Withholding)
                    totalWhtAmt += epTax.CuryTaxAmt;
            }

            return (totalWhtAmt, totalVatAmt);
        }
        private decimal? CalcClaimTotal(string refNbr)
        {
            decimal? curyExtCost = 0M;

            EPExpenseClaimDetails ecDetails = PXSelect<
                EPExpenseClaimDetails,
                Where<EPExpenseClaimDetails.claimDetailCD, Equal<Required<EPExpenseClaimDetails.claimDetailCD>>>>
                .Select(this, refNbr.Trim());

            if (ecDetails != null)
                curyExtCost = ecDetails.CuryExtCost;

            return curyExtCost ?? 0M;
        }
        private void RemoveTaxes(string refNbr)
        {
            EPExpenseClaimDetails ecDetails = PXSelect<
                EPExpenseClaimDetails,
                Where<EPExpenseClaimDetails.claimDetailCD, Equal<Required<EPExpenseClaimDetails.claimDetailCD>>>>
                .Select(this, refNbr.Trim());

            if (ecDetails is null) return;


            PXResultset<EPTaxTran> taxes = PXSelectJoinGroupBy<
                EPTaxTran,
                InnerJoin<Tax,
                    On<Tax.taxID, Equal<EPTaxTran.taxID>>>,
                Where<EPTaxTran.claimDetailID, Equal<Required<EPExpenseClaimDetails.claimDetailID>>>,
                Aggregate<
                    GroupBy<Tax.taxID,
                        Sum<EPTaxTran.curyTaxAmt,
                        Sum<EPTaxTran.curyTaxableAmt>>>>>
                .Select(this, ecDetails.ClaimDetailID);

            foreach (PXResult<EPTaxTran, Tax> t in taxes)
            {
                EPTaxTran epTran = t;

                ATPTEFMReplenishmentTaxDetail replenishTaxDetail = PXSelect<
                    ATPTEFMReplenishmentTaxDetail,
                    Where<ATPTEFMReplenishmentTaxDetail.taxID, Equal<Required<ATPTEFMReplenishmentTaxDetail.taxID>>,
                        And<ATPTEFMReplenishmentTaxDetail.replenishmentNbr, Equal<Current<ATPTEFMReplenishment.replenishmentNbr>>>>>
                    .Select(this, epTran.TaxID);

                if (replenishTaxDetail != null)
                {
                    replenishTaxDetail.TaxAmt -= epTran.CuryTaxAmt;
                    replenishTaxDetail.TaxableAmt -= epTran.CuryTaxableAmt;
                    Taxes.Update(replenishTaxDetail);

                    if (replenishTaxDetail.TaxAmt == decimal.Zero && replenishTaxDetail.TaxableAmt == decimal.Zero)
                        Taxes.Cache.Delete(replenishTaxDetail);
                }
            }
        }
        private void UpdateOnReplenishmentSummaryBalances(string receiptNbr, bool isInserted)
        {
            FundTransactionReceiptDetail.Current = FundTransactionReceiptDetail.Select(receiptNbr);
            FundTransactionReclassificationReceiptDetail.Current = FundTransactionReclassificationReceiptDetail.Select(receiptNbr);

            if (FundTransactionReceiptDetail.Current != null)
            {
                #region 
                /* FundTransaction.Current = FundTransaction.Select(FundTransactionReceiptDetail.Current.FundTransactionRefNbr);

           if (FundTransaction.Current != null)
           {
               TransactionHistoryView.Current = TransactionHistoryView.Select(FundTransactionReceiptDetail.Current.ExpenseReceiptRefNbr);
               Fund.Current = Fund.Select(Replenishments.Current.FundID);
               bool isFundReimbursement = ATPTEFMFundTransactionHelper.IsFundReimbursement(FundTransaction.Current);

               decimal? onReplenishmentAmt = (TransactionHistoryView.Current.LiquidatedAmt - TransactionHistoryView.Current.WithholdingTax);

               if (isInserted)
               {
                   Fund.Current.OnReplenishmentAmt += (isFundReimbursement) ? onReplenishmentAmt : TransactionHistoryView.Current.LiquidatedAmt;
                   Fund.Current.LiquidatedAmt -= (isFundReimbursement) ? (TransactionHistoryView.Current.LiquidatedAmt - TransactionHistoryView.Current.WithholdingTax) : TransactionHistoryView.Current.LiquidatedAmt;
               }
               else
               {
                   Fund.Current.OnReplenishmentAmt -= (isFundReimbursement) ? onReplenishmentAmt : TransactionHistoryView.Current.LiquidatedAmt;
                   Fund.Current.LiquidatedAmt += (isFundReimbursement) ? onReplenishmentAmt : TransactionHistoryView.Current.LiquidatedAmt;
               }

               Fund.UpdateCurrent();
           }*/
                #endregion
                string fundTransactionRefNbr = FundTransactionReceiptDetail.Current.FundTransactionRefNbr;
                UpdateOnReplenishmentSummaryBalancesCalculations(fundTransactionRefNbr, receiptNbr, isInserted);
            }

            if (FundTransactionReclassificationReceiptDetail.Current != null)
            {
                string fundTransactionRefNbr = FundTransactionReclassificationReceiptDetail.Current.FundTransactionRefNbr;
                UpdateOnReplenishmentSummaryBalancesCalculations(fundTransactionRefNbr, receiptNbr, isInserted);
            }
        }
        private void UpdateOnReplenishmentSummaryBalancesCalculations(string fundTransactionRefNbr, string receiptNbr, bool isInserted)
        {
            FundTransaction.Current = FundTransaction.Select(fundTransactionRefNbr);

            if (FundTransaction.Current != null)
            {
                TransactionHistoryView.Current = TransactionHistoryView.Select(receiptNbr);
                Fund.Current = Fund.Select(Replenishments.Current.FundID);
                bool isFundReimbursement = ATPTEFMFundTransactionHelper.IsFundReimbursement(FundTransaction.Current);

                decimal? onReplenishmentAmt = (TransactionHistoryView.Current.CuryLiquidatedAmt - TransactionHistoryView.Current.CuryWithholdingTax);

                if (isInserted)
                {
                    Fund.Current.CuryOnReplenishmentAmt += TransactionHistoryView.Current.CuryLiquidatedAmt;
                    Fund.Current.CuryLiquidatedAmt -= TransactionHistoryView.Current.CuryLiquidatedAmt;
                }
                else
                {
                    Fund.Current.CuryOnReplenishmentAmt -= TransactionHistoryView.Current.CuryLiquidatedAmt;
                    Fund.Current.CuryLiquidatedAmt += TransactionHistoryView.Current.CuryLiquidatedAmt;
                }

                Fund.UpdateCurrent();
            }
        }
        protected virtual ATPTEFMReplenishment OnBeforeUpdateReleaseReplenishment(ATPTEFMReplenishment replenishment) => replenishment;


        #endregion
        #region InternalTypes

        //public PXSelect<ATPTEFMReplenishmentTaxDetailUnbound> TaxDetails_Unbound;
        [PXHidden]
        public partial class ATPTEFMReplenishmentTaxDetailUnbound : CashFundManagement.DAC.Base.ATPTEFMBase, IBqlTable
        {

            #region TaxID
            [PXDBString(Tax.taxID.Length, IsUnicode = true, InputMask = ">CCCCCCCCCCCCCCCCCCCCCCCCCCCCCC", IsKey = true)]
            [PXDefault()]
            [PXUIField(DisplayName = Messages.ATPTEFMMessages.TaxID, Visibility = PXUIVisibility.Visible)]
            [PXSelector(typeof(Tax.taxID), DescriptionField = typeof(Tax.descr), DirtyRead = true)]
            public virtual string TaxID { get; set; }
            public new abstract class taxID : PX.Data.BQL.BqlString.Field<taxID> { }
            #endregion

            #region TaxRate
            [PXDBDecimal(6)]
            [PXDefault(TypeCode.Decimal, "0.0")]
            [PXUIField(DisplayName = Messages.ATPTEFMMessages.TaxRate, Visibility = PXUIVisibility.Visible, Enabled = false)]
            public virtual decimal? TaxRate { get; set; }
            public abstract class taxRate : PX.Data.BQL.BqlDecimal.Field<taxRate> { }
            #endregion

            #region TaxableAmt
            [PXDBDecimal(4)]
            [PXDefault(TypeCode.Decimal, "0.0")]
            [PXUIField(DisplayName = Messages.ATPTEFMMessages.TaxableAmount, Visibility = PXUIVisibility.Visible)]
            public virtual decimal? TaxableAmt { get; set; }
            public abstract class taxableAmt : PX.Data.BQL.BqlDecimal.Field<taxableAmt> { }
            #endregion

            #region TaxAmt
            public abstract class taxAmt : PX.Data.BQL.BqlDecimal.Field<taxAmt> { }
            [PXDBDecimal(4)]
            [PXDefault(TypeCode.Decimal, "0.0")]
            [PXUIField(DisplayName = Messages.ATPTEFMMessages.TaxAmount, Visibility = PXUIVisibility.Visible)]
            public virtual decimal? TaxAmt { get; set; }
            #endregion

        }
        #endregion

        #region Cache Attached
        [PXMergeAttributes(Method = MergeMethod.Append)]
        [EPTax]
        protected virtual void EPExpenseClaimDetails_TaxCategoryID_CacheAttached(PXCache cache)
        {
        }

        [PXMergeAttributes(Method = MergeMethod.Merge)]
        [PXRemoveBaseAttribute(typeof(PXUIFieldAttribute))]
        [PXUIField(DisplayName = ATPTEFMMessages.Vendor)]
        protected virtual void Vendor_AcctName_CacheAttached(PXCache sender) { }

        [PXMergeAttributes(Method = MergeMethod.Merge)]
        [PXRemoveBaseAttribute(typeof(PXUIFieldAttribute))]
        [PXUIField(DisplayName = ATPTEFMMessages.Address)]
        protected virtual void Address_AddressLine1_CacheAttached(PXCache sender) { }

        [PXMergeAttributes(Method = MergeMethod.Merge)]
        [PXRemoveBaseAttribute(typeof(PXUIFieldAttribute))]
        [PXUIField(DisplayName = ATPTEFMMessages.TIN)]
        protected virtual void LocationExtAddress__TaxRegistrationID_CacheAttached(PXCache sender) { }

        [PXMergeAttributes(Method = MergeMethod.Merge)]
        [PXUIField(DisplayName = ATPTEFMMessages.TaxZone, Visibility = PXUIVisibility.Visible)]
        [PXSelector(typeof(TaxZone.taxZoneID), DescriptionField = typeof(TaxZone.taxZoneID), Filterable = true)]
        protected virtual void APInvoice_TaxZoneID_CacheAttached(PXCache cache)
        {
        }

        [PXMergeAttributes(Method = MergeMethod.Merge)]
        [PXRemoveBaseAttribute(typeof(PXUIVerifyAttribute))]
        protected virtual void EPExpenseClaimDetails_ExpenseAccountID_CacheAttached(PXCache sender) { }
        private void ValidateAPBillStatus(string replenishmentNbr)
        {
            APInvoice apInvoice = PXSelect<APInvoice,
                Where<APInvoice.origRefNbr, Equal<Required<APInvoice.origRefNbr>>,
                    And<APInvoice.status, In3<APDocStatus.hold, APDocStatus.balanced, APDocStatus.open>>>>
                .Select(this, replenishmentNbr);

            if (apInvoice != null)
                throw new PXException(ATPTEFMMessages.CouldNotCancelReplenishmentBillGenerated);
        }


        [PXMergeAttributes(Method = MergeMethod.Merge)]
        [PXRemoveBaseAttribute(typeof(PXUIFieldAttribute))]
        [PXUIField(DisplayName = ATPTEFMMessages.ContractID, Visibility = PXUIVisibility.SelectorVisible)]
        protected virtual void _(Events.CacheAttached<Contract.contractCD> e) { }
        #endregion
    }
}